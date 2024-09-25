using System.ComponentModel.DataAnnotations;

namespace FormWork.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public decimal? Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}
