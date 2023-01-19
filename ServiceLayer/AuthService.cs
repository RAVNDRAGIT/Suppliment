using BusinessLayer;
using DataContract;
using DataLayer.Interface;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AuthService : IAuthService
    {
        private readonly IAuth iAuth;
        public static User user=new User();
        public AuthService(IAuth _iAuth)
        {
            iAuth = _iAuth;
        }

        

        public string GetUserName(string token)
        {
            throw new NotImplementedException();
        }

        public string Token(UserCredentialDC userCredentialDC)
        {
            var token = iAuth.Authentication(userCredentialDC.username, userCredentialDC.password);
            if (token == null)
            {
                return null;

            }
            else
            {
                return token;
            }
        }
    }
}
