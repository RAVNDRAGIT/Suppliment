using DataContract.Address;
using DataContract.Delivery;
using DataContract.Product;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using ServiceLayer.Helper;
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
        private readonly ShippingRocketHelper _shippingRocketHelper;
        private readonly IUnitOfWork _unitOfWork;
       

        public PinCodeService(IPincodeRepository pincodeRepository, ShippingRocketHelper shippingRocketHelper,IUnitOfWork unitOfWork) {
            _pincodeRepository=pincodeRepository;
            _shippingRocketHelper = shippingRocketHelper;
            _unitOfWork=unitOfWork;
            
        }

        public async Task<AddressResultDC> GetDetailsbyPincode(string pincode)
        {
            var data = await _pincodeRepository.GetDetailsbyPincode(pincode);
            return data;
        }

        public async Task<string> Getetd(EtdRequestDC etdRequestDC )
        {
                string res = null;
           
                var product = await _unitOfWork.ProductMasterRepository.GetProduct(etdRequestDC.productid);
                if(product!=null)
                {
                    List<ProductQuantityDC> list = new List<ProductQuantityDC>();

                    ProductQuantityDC productQuantityDC = new ProductQuantityDC();
                    productQuantityDC.Quantity = 1;
                    productQuantityDC.ProductMasterId = product.Id;
                    list.Add(productQuantityDC);
                   int weight = await _unitOfWork.ProductMasterRepository.GetWeight(list);
                    ServiciabilityDC serviciabilityDC = new ServiciabilityDC
                    {
                        pickup_postcode = 462026,
                        delivery_postcode = etdRequestDC.delivery_postcode,
                        length = 0,
                        height = 0,
                        breadth = 0,
                        weight = weight,
                        cod = true,

                        //declared_value= Convert.ToInt32( productdata.TotalPrice),
                        mode = "SURFACE",
                        only_local = 0,
                        token= etdRequestDC.token
                    };
                   
                     res = await _shippingRocketHelper.GetEtd(serviciabilityDC);
                    
                }
            

            return res;
        }
    }
}
