using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Auth
{
    public class ValidUserDetailDC
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
