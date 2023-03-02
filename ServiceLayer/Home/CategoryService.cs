using BusinessLayer.Home;
using DataContract.Home;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Home
{
    public class CategoryService
    {
       
        private ExcelHelper _excelHelper;
        private FileHelper _fileHelper;
        private IUnitOfWork _unitOfWork;
        public CategoryService(ICategoryRepository categoryRepository, FileHelper fileHelper, IUnitOfWork unitOfWork,ExcelHelper excelHelper)
        {
            
            _unitOfWork = unitOfWork;
            _excelHelper = excelHelper;
            _fileHelper = fileHelper;
        }
        public async Task<List<CategoryDC>> GetActiveCategories()
        {
            var data = await _unitOfWork.CategoryRepository.GetAllActiveCategories();
            return data;
        }

        public async Task<bool> Save()
        {
            bool result = false;
            List<CategoryMaster> list = new List<CategoryMaster>();
            var dt = await _excelHelper.ReadExcelFileAsync();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CategoryMaster cm = new CategoryMaster();
                cm.Name = Convert.ToString(dt.Rows[i][0]);
                

                string imgurl = _fileHelper.UploadImageUrl(Convert.ToString(dt.Rows[i][1]));

                cm.ImagePath = imgurl;
                cm.IsActive = true;
                cm.IsDelete = false;
                cm.Created_Date = DateTime.Now;
                cm.Created_By = 0;
                cm.Updated_By = 0;
                cm.Updated_Date = DateTime.Now;
                list.Add(cm);

            }
            if (list.Count>0)
            {
                long res = await _unitOfWork.CategoryRepository.Save(list);
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
