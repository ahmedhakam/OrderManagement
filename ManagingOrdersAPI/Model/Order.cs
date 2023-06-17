using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Model
{
    public class Order
    {
        [Required]
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        [Required]
        public virtual List<Item> OrderedItems { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public string Status;
        [Required]
        public string StatusSetter
        {
            get{ return Status; }
            set 
            {
                
                if (value == "pending" || value == "shipped" || value== "delivered")
                {
                        Status = value.ToLower();
                }
                else
                {
                        Status = "Not specified";
                }
            }

        }

        
    }
}
