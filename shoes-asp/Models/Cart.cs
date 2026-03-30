using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoes_asp.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public string? SelectedSize { get; set; }

        [NotMapped]
        public decimal TotalPrice => (Product != null) ? Product.Price * Quantity : 0;
    }
}