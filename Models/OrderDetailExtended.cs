using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class OrderDetailExtended
    {
        public int ID { get; set; }

        public int OrderDetailID { get; set; }

        [Required]
        public Guid ActivationCode { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}