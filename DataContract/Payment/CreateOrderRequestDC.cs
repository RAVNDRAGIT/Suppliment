using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Payment
{
    public class CreateOrderRequestDC
    {
        public string order_id { get; set; }
        public double order_amount { get; set; }
        public string order_currency { get; set; }
        public CustomerDetails customer_details { get; set; }
        public OrderMeta order_meta { get; set; }
        public DateTime order_expiry_time { get; set; }
        public string order_note { get; set; }
        public OrderTags order_tags { get; set; }
        public List<OrderSplit> order_splits { get; set; }


    }


        public class OrderTags
        {
            public string additionalProp { get; set; }
        }


    


    public class OrderMeta
    {
        public string return_url { get; set; }
        public string notify_url { get; set; }
        public string payment_methods { get; set; }
    }
    public class OrderSplit
    {
        public string vendor_id { get; set; }
        public string amount { get; set; }
    }
    public class CustomerDetails
    {
        public string customer_id { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string customer_bank_account_number { get; set; }
        public string customer_bank_ifsc { get; set; }
        public int customer_bank_code { get; set; }
    }
}