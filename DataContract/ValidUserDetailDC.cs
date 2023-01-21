using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class ValidUserDetailDC
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public long UserLocation { get; set; }
        public string Name { get; set; }
        public string Role{ get; set; }
    }
}
