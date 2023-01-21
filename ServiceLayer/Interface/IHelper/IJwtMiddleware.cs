using DataContract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface.IHelper
{
    public interface IJwtMiddleware
    {
        JwtSecurityToken JwtToken();
        string GenerateToken(ValidUserDetailDC validUserDetailDC);
    }
}
