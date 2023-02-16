using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Product
{
    public class DynamicProductDC
    {
        public long id { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public double mrp { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double discount { get; set; }
        public double discountPercentage { get; set; }
        public string imagePath { get; set; }
        public long producttypeid { get; set; }
    }
    }
