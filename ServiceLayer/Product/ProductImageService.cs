using DataContract.Product;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{
    public class ProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductImageDC>> GetProductImageList(long productid)
        {
            var data = await _unitOfWork.ProductImageRepository.GetProductImagebyProductId(productid);
            return data;
        }
    }
}
