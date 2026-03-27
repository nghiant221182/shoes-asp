using System.ComponentModel.DataAnnotations;

namespace shoes_asp.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Tên thương hiệu không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Logo { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}