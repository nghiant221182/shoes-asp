using System;
using System.ComponentModel.DataAnnotations;

namespace shoes_asp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public double Total { get; set; }
    }
}