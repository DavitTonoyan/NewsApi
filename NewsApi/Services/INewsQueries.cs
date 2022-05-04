using NewsApi.DataTransferObjects;
using NewsApi.Models;

namespace NewsApi.Services
{
    public interface INewsQueries
    {
        IAsyncEnumerable<News> SearchByDate(DateDto date);
        IAsyncEnumerable<News> SearchByText(string text);
    }
}
