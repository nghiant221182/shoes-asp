using System.ComponentModel.DataAnnotations;

namespace shoes_asp.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string ReceiverPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        public string? Note { get; set; }

        public List<Cart>? CartItems { get; set; }

        public decimal GrandTotal { get; set; }
    }
}