using BusinessLayer;
using DataContract;
using DataContract.Auth;
using DataLayer.Context;
using DataLayer.Interface;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using System.Threading.Tasks;

namespace ServiceLayer.Helper
{
    public class JwtMiddleware
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly DbContext _dbContext;
        private readonly DbContext _dbContext;
        public JwtMiddleware(IHttpContextAccessor httpContextAccessor, DbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public ResultViewModel<JwtSecurityToken> JwtToken()
        {


            ResultViewModel<JwtSecurityToken> result = new ResultViewModel<JwtSecurityToken>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretkey = _dbContext.AuthKey();
            var key = Encoding.ASCII.GetBytes(secretkey);
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            JwtSecurityToken jwtSecurityToken;
            if (Convert.ToString(token) != null && token != "" && token != "null")
            {
                jwtSecurityToken = new JwtSecurityToken(token);
                if (jwtSecurityToken.ValidTo > DateTime.UtcNow)
                {

                    tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero

                    }, out SecurityToken validatedToken);
                    var jwtToken = (JwtSecurityToken)validatedToken;
                    result.IsSucess = true;
                    result.response = jwtToken;
                    return result;
                }
                else
                {
                    result.IsError = true;
                    result.ErrorMessage = "Token Expired";
                }

            }
            else
            {
                result.IsSucess = true;
                result.SuccessMessage = null;
            }
            return result;
        }



        public string GenerateToken(ValidUserDetailDC validUserDetailDC)
        {
            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey =  Encoding.ASCII.GetBytes( _dbContext.AuthKey());

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                new Claim[]
                {
                        new Claim(ClaimTypes.Name, validUserDetailDC.UserName),
                        new Claim(ClaimTypes.Role, validUserDetailDC.Role),
                        new Claim("UserId",Convert.ToString( validUserDetailDC.UserId)),
                        new Claim("UserName", validUserDetailDC.UserName),

                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
        public string GetUserName()
        {
            var jwtToken = JwtToken();
            if(jwtToken.IsSucess==true)
            {
                var username = jwtToken.response.Claims.First(x => x.Type == "UserName").Value;
                return username;
            }
           else
            {
                return null;

            }

        }

        public long? GetUserId()
        {
            var jwtToken = JwtToken();
            if (jwtToken.IsSucess == true && jwtToken.response!=null)
            {
                var userid = int.Parse(jwtToken.response.Claims.First(x => x.Type == "UserId").Value);
                return userid;
            }
            else
            {
                return null;
            }


        }

       
    }
}
