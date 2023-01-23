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
        public IAuthService _authService { get; set; }
        public AuthService(IUserRepository _iUser, IJwtMiddleware _jwtMiddleware, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            iUser = _iUser;
            jwtMiddleware = _jwtMiddleware;

        }

        public ValidUserDetailDC Token(UserCredentialDC userCredentialDC)
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
                ValidUserDetailDC validUserDetailDC = Mapper.Map(data).ToANew<ValidUserDetailDC>();
                validUserDetailDC.Token = token;
                return validUserDetailDC;
            }
        }

        public string GetUserName()
        {
            var jwtToken = jwtMiddleware.JwtToken();
            var username = jwtToken.Claims.First(x => x.Type == "UserName").Value;
            return username;

        }

        public long? GetUserId()
        {
            var jwtToken = jwtMiddleware.JwtToken();
            if (jwtToken != null)
            {
                var userid = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
                return userid;
            }
            else
            {
                return null;
            }


        }
    }
}
