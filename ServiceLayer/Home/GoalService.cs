using BusinessLayer.Home;
using DataContract.Home;
using DataLayer.Interface;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Home
{
    public class GoalService
    {
        private IUnitOfWork _unitOfWork;
        private ExcelHelper _excelHelper;
        private FileHelper _fileHelper;
        public GoalService(IGoalRepository goalRepository, ExcelHelper excelHelper, FileHelper fileHelper, IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
            _excelHelper = excelHelper;
            _fileHelper=fileHelper;
        }
        public async Task<List<GoalDC>> GetActiveGoals()
        {
            var data= await _unitOfWork.GoalRepository.GetActiveGoalsAsync();
            return data;
        }

        public async Task<bool> Save()
        {
            bool result = false;
            List<Goal> goals = new List<Goal>();
            var dt = await _excelHelper.ReadExcelFileAsync();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Goal goal = new Goal();
                goal.Name = Convert.ToString( dt.Rows[i][0]);
                goal.Description = Convert.ToString(dt.Rows[i][1]);

                string imgurl = _fileHelper.UploadImageUrl(Convert.ToString(dt.Rows[i][2]));

                goal.ImagePath = imgurl;
                goal.IsActive = true;
                goal.IsDelete = false;
                goal.Created_Date = DateTime.Now;
                goal.Created_By = 0;
                goal.Updated_By = 0;
                goal.Updated_Date = DateTime.Now;
                goals.Add(goal);
                
            }
            if(true)
            {
                long res = await _unitOfWork.GoalRepository.Save(goals);
                if (res > 1)
                {
                    result = true;
                }
                _unitOfWork.Commit();
            }
            return result;
        }
    }
}
