using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContract.Address
{
    public class AddressResultDC
    {
        public StateDistrictDC stateDistrict { get; set; }
        public List<AddressDetailDC> addressDetail { get; set; }
    }
}
