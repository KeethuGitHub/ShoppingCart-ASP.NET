using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class Product
    {
        public int ID { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}