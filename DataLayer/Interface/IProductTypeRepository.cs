using DataContract.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IProductTypeRepository
    {
        Task<List<ProductTypeDC>> GetProductTypeList();

    }
}
