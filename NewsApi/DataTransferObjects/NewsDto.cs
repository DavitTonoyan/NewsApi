using System.ComponentModel.DataAnnotations;

namespace NewsApi.DataTransferObjects
{
    public class NewsDto
    {
        [Required]
        [MaxLength(30)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Content { get; set; } = string.Empty;

        [Range(typeof(DateTime), "01-01-1970", "01-04-2022")]
        public DateTime CreatedDate { get; set; }

        public List<int> CategoriesIds { get; set; } = new();
    }
}
