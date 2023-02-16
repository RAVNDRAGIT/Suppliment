using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Product
{
    public class ProductType:BaseModel
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
