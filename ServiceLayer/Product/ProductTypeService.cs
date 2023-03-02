using BusinessLayer.Home;
using BusinessLayer.Product;
using DataContract.Product;
using DataLayer.Interface;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{
    public class ProductTypeService
    {
        private IUnitOfWork _unitOfWork;
        private ExcelHelper _excelHelper;
        private JwtMiddleware _jwtMiddleware;
        private FileHelper _fileHelper;
        public ProductTypeService(IUnitOfWork unitOfWork, JwtMiddleware jwtMiddleware, ExcelHelper excelHelper, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtMiddleware = jwtMiddleware;
            _excelHelper = excelHelper;
            _fileHelper = fileHelper;
        }

        public async Task<List<ProductTypeDC>> GetProductTypeList()
        {
            var data = await _unitOfWork.ProductTypeRepository.GetProductTypeList();
            return data;
        }

        public async Task<bool> Save()
        {
            bool result = false;
            List<ProductType> list = new List<ProductType>();
            var dt = await _excelHelper.ReadExcelFileAsync();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductType cm = new ProductType();
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
            if (list.Count > 0)
            {
                long res = await _unitOfWork.ProductTypeRepository.Save(list);
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
