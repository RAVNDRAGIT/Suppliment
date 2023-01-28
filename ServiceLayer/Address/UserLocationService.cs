using AgileObjects.AgileMapper;
using BusinessLayer;
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
        private readonly JwtMiddleware jwtMiddleware;
        public UserLocationService(JwtMiddleware jwtMiddleware, IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> SubmitUserLocation(UserLocationDC userLocationDC)
        {
            var data = Mapper.Map(userLocationDC).ToANew<UserLocations>();
            long userid = jwtMiddleware.GetUserId()??0;
            long res = await _unitOfWork.UserLocationRepository.SubmitUserLocation(data, userid);
            return res;
        }
    }
}
