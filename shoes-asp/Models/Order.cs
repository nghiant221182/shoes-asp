using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shoes_asp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Chờ xác nhận";

        public string ShippingAddress { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverPhone { get; set; }

        public string? Note { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}