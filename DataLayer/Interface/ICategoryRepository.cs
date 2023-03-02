using BusinessLayer.Home;
using DataContract.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface ICategoryRepository
    {
       Task< List<CategoryDC>> GetAllActiveCategories();
        Task<long> Save(List<CategoryMaster> list);
        Task<long> Save(CategoryMaster category);
    }
}
