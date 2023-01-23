using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Order
{
    public class CartDetails
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public long? CategoryId { get; set; }
        public long? AttributeId { get; set; }
        public double Mrp { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public double Discount { get; set; }
        public double DiscountPercentage { get; set; }
        public double TotalMrp { get; set; }
        public double TotalPrice { get; set; }
        public double TotalDiscount { get; set; }
        public int Quantity { get; set; }
        public long? Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public long? Updated_By { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
