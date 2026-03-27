using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoes_asp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public int Stock { get; set; }
        public double Rate { get; set; }

        public string? Size { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Cart>? Carts { get; set; }
        public ICollection<Wishlist>? Wishlists { get; set; }
    }
}