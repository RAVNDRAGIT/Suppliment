using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Order
{
    public  class OrderMaster:BaseModel
    {
        public double TotalMrp { get; set; }
        public double TotalPrice { get; set; }
        public double TotalDiscount { get; set; }
        public bool IsPaid { get; set; }
        public int Status { get; set; }
        public long UserLocationId { get; set; }
        public bool? IsDelievered { get; set; }
        public string DeliveryToken { get; set; }
        public string PaymentType { get; set; }
        public bool? IsMsgSent { get; set; }
        public string PaymentRequestOrderId { get; set; }
    }
}
