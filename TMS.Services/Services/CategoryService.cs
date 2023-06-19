using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;

namespace TMS.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<int> Add(Category category)
        {
            return _categoryRepository.Add(category);
        }

        public Task<int> Delete(int id)
        {
            return _categoryRepository.Delete(id);
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Task<Category> GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Task<int> Update(Category category)
        {
            return _categoryRepository.Update(category);
        }
    }
}
