using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.ProductMaster;
using DataContract;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using ServiceLayer.Helper;
using ServiceLayer.Interface.IHelper;
using ServiceLayer.Interface.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository iUser;
        private IJwtMiddleware jwtMiddleware;
        public IUnitOfWork _unitOfWork;
        public AuthService(IUserRepository _iUser, IJwtMiddleware _jwtMiddleware, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            iUser = _iUser;
            jwtMiddleware = _jwtMiddleware;

        }

        public string Token(UserCredentialDC userCredentialDC)
        {
            var user = Mapper.Map(userCredentialDC).ToANew<User>();
            var data = _unitOfWork.UserRepository.Authentication(user);
            if (data == null)
            {
                return null;

            }
            else
            {
                var token = jwtMiddleware.GenerateToken(data);
                return token;
            }
        }

        public string GetUserName()
        {
            var jwtToken = jwtMiddleware.JwtToken();
            var username = jwtToken.Claims.First(x => x.Type == "UserName").Value;
            return username;

        }

        public long GetUserId()
        {
            var jwtToken = jwtMiddleware.JwtToken();
            var userid = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
            return userid;

        }
    }
}
