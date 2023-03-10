using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class OrderDetailDC
    {
        public long OrderId { get; set; }
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
    }
}
