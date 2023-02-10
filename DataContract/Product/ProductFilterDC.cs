using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Product
{
    public class ProductFilterDC
    {
        public long? productid { get; set; }
        public string productname { get; set; }
        public int? @skip { get; set; }
        public int? @take { get; set; }
    }
}
