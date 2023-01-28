using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Payment
{
    public class CapturePaymentDC
    {
        public string action { get; set; }
        public double amount { get;set; }
    }
}
