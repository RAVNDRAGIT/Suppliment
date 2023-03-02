using BusinessLayer.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IProductImageRepository
    {
        Task<long> SaveImage(List<ProductImage> images);
    }
}
