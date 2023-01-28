using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Payment
{
    public  class CreateOrderResponseDC
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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
        public List<object> order_splits { get; set; }
        public string order_status { get; set; }
        public object order_tags { get; set; }
        public string payment_session_id { get; set; }
        public Payments payments { get; set; }
        public Refunds refunds { get; set; }
        public Settlements settlements { get; set; }
        public object terminal_data { get; set; }
        public long Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        public long Updated_By { get; set; }
        public DateTime Updated_Date { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
   

    public class Payments
    {
        public string url { get; set; }
    }

    public class Refunds
    {
        public string url { get; set; }
    }

    public class Settlements
    {
        public string url { get; set; }
    }
   
    
}
