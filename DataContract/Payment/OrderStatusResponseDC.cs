using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Payment
{
    public class OrderStatusResponseDC
    {
        public int cf_order_id { get; set; }
        public DateTime created_at { get; set; }
        public CustomerDetails customer_details { get; set; }
        public string entity { get; set; }
        public double order_amount { get; set; }
        public string order_currency { get; set; }
        public DateTime order_expiry_time { get; set; }
        public string order_id { get; set; }
        public OrderMeta order_meta { get; set; }
        public object order_note { get; set; }
        public string order_status { get; set; }
        public string payment_session_id { get; set; }
        public Payments payments { get; set; }
        public Refunds refunds { get; set; }
        public Settlements settlements { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
    

 

}
