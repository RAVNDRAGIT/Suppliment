using DataContract.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public  interface IPincodeRepository
    {
        Task <AddressResultDC> GetDetailsbyPincode(string pincode);
    }
}
