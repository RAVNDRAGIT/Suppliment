using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Product
{
    public class ProductForCartDc
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Mrp { get; set; } = 0;
        public double Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public double DiscountPercentage { get; set; } = 0;

        public long CategoryId { get; set; }
        public string ImagePath { get; set; }





        
    }
}
