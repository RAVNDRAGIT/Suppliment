using AgileObjects.AgileMapper;
using BusinessLayer.Users;
using DataContract.Address;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using DataLayer.Repository.Address;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Address
{
    public class UserLocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtMiddleware _jwtMiddleware;
        public UserLocationService(JwtMiddleware jwtMiddleware, IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
            _jwtMiddleware = jwtMiddleware;
        }

        public async Task<bool> SubmitUserLocation(UserLocationDC userLocationDC)
        {
            bool result = false;
            var data = Mapper.Map(userLocationDC).ToANew<UserLocation>();
            long userid = _jwtMiddleware.GetUserId()??0;
            long res = await _unitOfWork.UserLocationRepository.SubmitUserLocation(data, userid);
            if(res>0)
            {
                _unitOfWork.Commit();
                result = true;

            }
            return result;
        }

        public async Task<List<UserLocationResultDC>> GetUserAddressbyUserid()
        {
            long userid = _jwtMiddleware.GetUserId() ?? 0;
            var data = await _unitOfWork.UserLocationRepository.GetAddressbyUserId(userid);
            return data;
        }
    }
}
