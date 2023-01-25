using AgileObjects.AgileMapper;
using BusinessLayer.ProductMaster;
using DataContract;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{

    public class ProductService 
    {
        public IUnitOfWork _unitOfWork;
        public IProductMasterRepository _productMasterRepository;
        public ProductService(IUnitOfWork unitOfWork, IProductMasterRepository productMasterRepository)
        {
            _unitOfWork = unitOfWork;
            _productMasterRepository=productMasterRepository;
        }
        public async Task<bool> AddProduct(ProductMasterDC productMasterDC)
        {
            bool result = false;
            if (productMasterDC != null)
            {
                var productMaster = Mapper.Map(productMasterDC).ToANew<ProductMaster>();
                bool res = await _productMasterRepository.AddProduct(productMaster);
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
