using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public int? ValidDays { get; set; }
        public int? Months { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string ScopeOfProduct { get; set; }
        public bool Paid { get; set; }
        public bool Promotion { get; set; }
        public bool Social { get; set; }
        public string TypeOfPayment { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
