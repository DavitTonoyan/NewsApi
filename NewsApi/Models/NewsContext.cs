using Microsoft.EntityFrameworkCore;

namespace NewsApi.Models
{
    public class NewsContext :DbContext
    {
        public DbSet<News>? News { get; set;}
        public DbSet<Category>? Categories { get; set;}

        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options) 
        { 
        }
    }
}
