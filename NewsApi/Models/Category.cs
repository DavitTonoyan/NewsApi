using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsApi.Models
{
    [Index("Name", IsUnique = true, Name = "Name_Index")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public List<News> News { get; set; } = new();
    }
}
