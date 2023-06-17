using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Model
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

    }
}
