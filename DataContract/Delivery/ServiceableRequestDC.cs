using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Delivery
{
    public class ServiceableRequestDC
    {
        public string token { get; set; }
        public int delivery_postcode { get; set; }
        public string mongoId { get; set; }
        public long? productid { get; set; }
    }
    
}
