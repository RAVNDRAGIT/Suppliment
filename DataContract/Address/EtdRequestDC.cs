using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Address
{
    public class EtdRequestDC
    {
        public string token { get; set; }
        public int delivery_postcode { get; set; }
        public long productid { get; set; }
    }
}
