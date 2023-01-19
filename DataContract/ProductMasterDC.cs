using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class ProductMasterDC
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Mrp { get; set; } = 0;
        public double Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public double Discount { get; set; } = 0;
    }
}
