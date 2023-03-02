using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Product
{
    public class ProductImage:BaseModel
    {
        public string ImagePath { get;set; }
        public long ProductId { get;set; }
        public bool IsDefault { get; set; }
    }
}
