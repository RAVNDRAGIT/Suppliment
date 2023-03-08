using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract
{
    public class NotificationDC
    {
        public string to { get; set; }
        public NotificationDetailDC notification { get; set; }
    }

    public class NotificationDetailDC
    {
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
    }
}
