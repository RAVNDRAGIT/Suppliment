using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Users
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public long? UserLocation { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public string Email { get; set; }

    }
}
