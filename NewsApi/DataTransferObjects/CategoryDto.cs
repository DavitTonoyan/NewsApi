using System.ComponentModel.DataAnnotations;

namespace NewsApi.DataTransferObjects
{
    public class CategoryDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}
