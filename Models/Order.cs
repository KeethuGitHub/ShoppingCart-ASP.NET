using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class Order
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get;set; }

        [Required]
        public double Price { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}