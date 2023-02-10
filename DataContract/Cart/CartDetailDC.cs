using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Cart
{
    public class CartDetailDC
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public string CookieValue { get; set; }

    }
}
