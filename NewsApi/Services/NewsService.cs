using Microsoft.EntityFrameworkCore;
using NewsApi.DataTransferObjects;
using NewsApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NewsApi.Services
{
    public class NewsService : IService<News, NewsDto>, INewsQueries
    {
        private readonly NewsContext _context;

        public NewsService(NewsContext context)
        {
            this._context = context;
        }
        public async Task<News> Add(NewsDto entity)
        {
            List<Category> categories = new();
            foreach (var categoryId in entity.CategoriesIds)
            {
                var category = await _context.Categories.FindAsync(categoryId);

                if (category is null)
                    continue;

                categories.Add(category);
            }

            var news = new News()
            {
                Title = entity.Title,
                Content = entity.Content,
                CreatedDate = entity.CreatedDate,
                Categories = categories
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }
        public async Task Delete(int id)
        {
            News news = await GetById(id);
            _context.News.Remove(news);

            await _context.SaveChangesAsync();
        }
        public async IAsyncEnumerable<News> GetAll()
        {
            var news = _context.News
                .AsNoTracking()
                .Include(news => news.Categories)
                .AsAsyncEnumerable();

            await foreach (var item in news)
            {
                yield return item;
            }
        }
        public async Task<News> GetById([Range(0, int.MaxValue)] int id)
        {
            var news = await _context.News
                .AsNoTracking()
                .Include(news => news.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
            {
                throw new ArgumentException("Id is not found");
            }
            return news;
        }


        public async Task Update(NewsDto entity, [Range(0, int.MaxValue)] int id)
        {
            var news = await _context.News
                .Include(news => news.Categories)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (news is null)
            {
                throw new ArgumentException("Id is not found");
            }

            news.Categories.Where(category => !entity.CategoriesIds
                                       .Any(id => id == category.Id))
                                       .ToList()
                                       .ForEach(category => news.Categories.Remove(category));

            entity.CategoriesIds.Where(id => !news.Categories
                                       .Any(category => id == category.Id))
                                       .ToList()
                                       .ForEach(id => news.Categories.Add(new Category { Id = id }));

            news.Title = entity.Title;
            news.Content = entity.Content;
            news.CreatedDate = entity.CreatedDate;

            await _context.SaveChangesAsync();
        }



        public async IAsyncEnumerable<News> SearchByDate(DateDto date)
        {
            var news = _context.News
                .AsNoTracking()
                .Include(n => n.Categories)
                .Where(n => n.CreatedDate > date.From && n.CreatedDate < date.To)
                .AsAsyncEnumerable();

            await foreach(var n in news)
            {
                yield return n;
            }
        }

  
        public async IAsyncEnumerable<News> SearchByText(string text)
        {
            var news = _context.News
                .AsNoTracking()
                .Where(n => n.Title.Contains(text) || n.Content.Contains(text))
                .AsAsyncEnumerable();


            await foreach (var n in news)
            {
                yield return n;
            }
        }

    }
}
