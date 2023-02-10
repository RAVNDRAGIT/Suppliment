using DataContract.Home;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Home
{
    public class CategoryService
    {
        private ICategoryRepository _categoryRepository ;
        public CategoryService(ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDC>> GetActiveCategories()
        {
            var data = await _categoryRepository.GetAllActiveCategories();
            return data;
        }
    }
}
