using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.ProductMaster;
using DataContract;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Auth
{
    public class AuthService 
    {
        private readonly IUserRepository _iUser;
        public IUnitOfWork _unitOfWork;
       public JwtMiddleware _jwtMiddleware { get; set; }
        public AuthService(IUserRepository iUser, IUnitOfWork unitOfWork,JwtMiddleware jwtMiddleware)
        {
            _unitOfWork = unitOfWork;
            _iUser = iUser;
            _jwtMiddleware = jwtMiddleware;



        }

        public ValidUserDetailDC Token(UserCredentialDC userCredentialDC)
        {
            var user = Mapper.Map(userCredentialDC).ToANew<User>();
            var data = _iUser.Authentication(user);
            if (data == null)
            {
                return null;

            }
            else
            {
                var token = _jwtMiddleware.GenerateToken(data);
                ValidUserDetailDC validUserDetailDC = Mapper.Map(data).ToANew<ValidUserDetailDC>();
                validUserDetailDC.Token = token;
                return validUserDetailDC;
            }
        }

       
    }
}
