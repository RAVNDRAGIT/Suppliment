using BusinessLayer.Product;
using DataContract.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IProductMasterRepository
    {
       Task<bool> AddProduct(ProductMaster productMaster,long userid);
        Task<ProductMaster> GetProduct(long id);
        Task<List<DynamicProductDC>> GetProductDynamically(ProductFilterDC productFilterDC);

        Task<List<DynamicProductDC>> GetLikeProduct(long producttypeid,long productid);

        Task<int> GetWeight(List<ProductQuantityDC> productQuantities);
        Task<List<DynamicProductDC>> GetDiscountProduct(int skip, int take);

        Task<bool> AddProductList(List<ProductMaster> productMaster);


    }
}
