using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class OrderDetail
    {
        public int ID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }   

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}