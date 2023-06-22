using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Infrastructure.Interfaces;

namespace TMS.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(Category category)
        {
            _unitOfWork.Context.Categories.Add(category);
            await _unitOfWork.Context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> Delete(int id)
        {
            Category category = await _unitOfWork.Context.Categories.FindAsync(id);
            _unitOfWork.Context.Categories.Remove(category);
            await _unitOfWork.Context.SaveChangesAsync();
            return id;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = _unitOfWork.Context.Categories;
            return await categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _unitOfWork.Context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();            
            return category;
        }

        public async Task<int> Update(Category category)
        {
            int entry = 0;
            Category olddata = await _unitOfWork.Context.Categories.FindAsync(category.Id);
            if (olddata != null)
            {
                olddata.CategoryName = category.CategoryName;
                entry = await _unitOfWork.Context.SaveChangesAsync();
            }
            return entry;
        }
    }
}
