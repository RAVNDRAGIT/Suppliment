using DataContract.Address;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Address
{
    public  class PinCodeService
    {
        private readonly IPincodeRepository _pincodeRepository;

        public PinCodeService(IPincodeRepository pincodeRepository) {
            _pincodeRepository=pincodeRepository;
        }

        public async Task<AddressResultDC> GetDetailsbyPincode(string pincode)
        {
            var data = await _pincodeRepository.GetDetailsbyPincode(pincode);
            return data;
        }
    }
}
