using AgileObjects.AgileMapper;
using BusinessLayer.Product;
using Dapper;
using DataContract.Product;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{

    public class ProductService
    {
        private IUnitOfWork _unitOfWork;
        private IProductMasterRepository _productMasterRepository;
        private JwtMiddleware _jwtMiddleware;
        public ProductService(IUnitOfWork unitOfWork, IProductMasterRepository productMasterRepository, JwtMiddleware JwtMiddleware)
        {
            _unitOfWork = unitOfWork;
            _productMasterRepository = productMasterRepository;
            _jwtMiddleware = JwtMiddleware;
        }
        public async Task<bool> AddProduct(ProductMasterDC productMasterDC)
        {
            bool result = false;
            long userid = _jwtMiddleware.GetUserId() ?? 0;
            if (productMasterDC != null)
            {
                var productMaster = Mapper.Map(productMasterDC).ToANew<ProductMaster>();
                bool res = await _productMasterRepository.AddProduct(productMaster, userid);
                if (res)
                {

                    _unitOfWork.Commit();
                }
                result = res;
            }
            return result;
        }

        public async Task<List<DynamicProductDC>> GetProductDynamically(ProductFilterDC productFilterDC)

        {
            var data = await _productMasterRepository.GetProductDynamically(productFilterDC);
            return data;
        }

        public async Task<List<DynamicProductDC>> GetLikeProduct(long producttypeid, long productid)
        {

            var data = await _productMasterRepository.GetLikeProduct(producttypeid,productid);
            return data;
        }

    }
}
