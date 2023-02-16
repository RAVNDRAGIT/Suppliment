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
       
        private JwtMiddleware _jwtMiddleware;
        public ProductTypeService(IUnitOfWork unitOfWork,  JwtMiddleware jwtMiddleware)
        {
            _unitOfWork = unitOfWork;
            _jwtMiddleware = jwtMiddleware;
        }

        public async Task<List<ProductTypeDC>> GetProductTypeList()
        {
            var data = await _unitOfWork.ProductTypeRepository.GetProductTypeList();
            return data;
        }
    }
}
