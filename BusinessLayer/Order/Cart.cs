using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Order
{
  
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public double TotalMrp { get; set; }
        public double TotalPrice { get; set; }
        public double TotalDiscount { get; set; }
        public long? Created_By { get; set; }
        public DateTime Created_Date { get; set; }    
        public long? Updated_By { get; set; }
        public DateTime Updated_Date { get; set; }
        public string CookieValue { get; set; }
        public List<CartDetails> cartDetails { get; set; }
    }
}
