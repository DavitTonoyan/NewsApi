using Microsoft.EntityFrameworkCore;
using NewsApi.DataTransferObjects;
using NewsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace NewsApi.Services
{
    public class CategoryService : IService<Category, CategoryDto>
    {
        private readonly NewsContext _context;

        public CategoryService(NewsContext context)
        {
            this._context = context;
        }
        public async Task<Category> Add(CategoryDto entity)
        {
            Category category = new Category()
            {
                Name = entity.Name
            };

            _context?.Categories?.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task Delete(int id)
        {
            var category = await GetById(id);
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }
        public async IAsyncEnumerable<Category> GetAll()
        {
            var categories = _context.Categories
                .AsNoTracking()
                .Include(category => category.News)
                .AsAsyncEnumerable();
            await foreach (var category in categories)
            {
                yield return category;
            }
        }
        public async Task<Category> GetById([Range(0, int.MaxValue)] int id)
        {
            var category = await _context.Categories
                 .AsNoTracking()
                .Include(category => category.News)
                .FirstOrDefaultAsync(category => category.Id == id);

            if (category is null)
            {
                throw new ArgumentException("Id is not found");
            }

            return category;
        }
        public async Task Update(CategoryDto entity, [Range(0, int.MaxValue)] int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(category => category.Id == id);

            if (category is null)
            {
                throw new ArgumentException("Id is not found");
            }

            category.Name = entity.Name;
            await _context.SaveChangesAsync();
        }
    }
}
