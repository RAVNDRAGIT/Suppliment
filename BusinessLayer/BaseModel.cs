using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BaseModel
    {
       
        public long Id { get;set; }  
        public long Created_By { get;set; }

        public DateTime Created_Date { get; set; }

        public long Updated_By { get;set;}
        public DateTime Updated_Date { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        }
    
}
