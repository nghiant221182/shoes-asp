using System.ComponentModel.DataAnnotations;

namespace shoes_asp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string Role { get; set; } = "Customer";

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Cart>? Carts { get; set; }
        public ICollection<Wishlist>? Wishlists { get; set; }
    }
}