using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}