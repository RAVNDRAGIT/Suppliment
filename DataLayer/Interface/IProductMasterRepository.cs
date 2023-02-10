using BusinessLayer.ProductMaster;
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
        Task<List<ProductMaster>> GetProductDynamically(ProductFilterDC productFilterDC);
    }
}
