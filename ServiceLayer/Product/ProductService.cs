using AgileObjects.AgileMapper;
using BusinessLayer.ProductMaster;
using DataContract;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using ServiceLayer.Interface.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{

    public class ProductService : IProductMasterService
    {
        public IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddProduct(ProductMasterDC productMasterDC)
        {
            bool result = false;
            if (productMasterDC != null)
            {
                var productMaster = Mapper.Map(productMasterDC).ToANew<ProductMaster>();
                bool res = await _unitOfWork.ProductMasterRepository.AddProduct(productMaster);
                if (res)
                {

                    _unitOfWork.Commit();
                }
                result = res;
            }
            return result;
        }
    }
}
