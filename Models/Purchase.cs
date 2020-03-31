using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class Purchase
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public List<Guid> ActivationCodes { get; set; }
    }
}