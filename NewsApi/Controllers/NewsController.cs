using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApi.DataTransferObjects;
using NewsApi.Models;
using NewsApi.Services;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsQueries _queries;
        private readonly IService<News, NewsDto> _service;
        private readonly ILogger<NewsController> _logger;

        public NewsController(IService<News, NewsDto> service,
                              INewsQueries queries,
                              ILogger<NewsController> logger)
        {
            _queries = queries;
            _service = service;
            _logger = logger;
        }


        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<News>))]
        public IActionResult GetAll()
        {
            var news = _service.GetAll();
            return Ok(news);
        }


        [HttpGet("{id}", Name = nameof(GetNewsById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(News))]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var news = await _service.GetById(id);
            return Ok(news);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(NewsDto entity)
        {
            var news = await _service.Add(entity);
            _logger.LogInformation("Added new news {Id}", news.Id);
            return CreatedAtAction(nameof(GetNewsById), new { id = news.Id }, news);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(int id, NewsDto entity)
        {
            await _service.Update(entity, id);
            _logger.LogInformation("Updated news {id}", id);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            _logger.LogInformation("Deleted news {id}", id);
            return NoContent();
        }



        [HttpGet("SearchByDate")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<News>))]
        public IActionResult SearchByDate(DateDto date)
        {
            var news = _queries.SearchByDate(date);
            return Ok(news);
        }

        [HttpGet("SearchByText")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<News>))]
        public IActionResult SearchByText(string text)
        {
            var news = _queries.SearchByText(text); 
            return Ok(news);
        }

    }
}
