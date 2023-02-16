using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Delivery
{
    public class ServiciabilityDC
    {
        public int pickup_postcode { get; set; }
        public int delivery_postcode { get; set; }
        public bool cod { get; set; } = false;
        public int? weight { get; set; }
        public int? length { get; set; }
        public int? breadth { get; set; }
        public int? height { get; set; }
        public string mode { get; set; }
        public int? only_local { get; set; }
        public string token { get; set; }
       
       
       
    }
}
