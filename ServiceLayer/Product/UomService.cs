using BusinessLayer.Product;
using DataLayer.Interface;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{
    public class UomService
    {
        private readonly ExcelHelper _excelHelper;
        private readonly IUnitOfWork _unitOfWork;
        public UomService(ExcelHelper excelHelper, IUnitOfWork unitOfWork) {
            _excelHelper=excelHelper;
            _unitOfWork=unitOfWork;
        }

        public async Task<bool> Save()
        {
            bool result = false;
            List<UnitOfMeasurementMaster> list = new List<UnitOfMeasurementMaster>();
            var dt = await _excelHelper.ReadExcelFileAsync();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UnitOfMeasurementMaster unitOfMeasurementMaster = new UnitOfMeasurementMaster();
                unitOfMeasurementMaster.Value = Convert.ToString(dt.Rows[i][0]);
                unitOfMeasurementMaster.IsActive = true;
                unitOfMeasurementMaster.IsDelete = false;
                unitOfMeasurementMaster.Created_Date = DateTime.Now;
                unitOfMeasurementMaster.Created_By = 0;
                unitOfMeasurementMaster.Updated_Date = DateTime.Now;
                unitOfMeasurementMaster.Updated_By = 0;
                list.Add(unitOfMeasurementMaster);


            }

            long res = await _unitOfWork.UnitOfMeasurementRepository.Save(list);
            if(res>0)
            {
                _unitOfWork.Commit();
                result = true;
            }
            return result;
        }
    }
}
