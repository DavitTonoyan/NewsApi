using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewsApi.Models
{

    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Content { get; set; } = string.Empty;

        [Column(TypeName = "Date")]
        [Range(typeof(DateTime), "01-01-1970", "01-04-2022")]
        public DateTime CreatedDate { get; set; }

        public List<Category> Categories { get; set; } = new();
    }
}
