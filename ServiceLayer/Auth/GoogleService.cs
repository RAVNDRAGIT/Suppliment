using BusinessLayer.Users;
using DataContract.Auth;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using DataLayer.Repository.Auth;
using Google.Apis.Auth;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Auth
{
    public class GoogleService
    {
        private readonly DbContext _dbContext;
        public IUnitOfWork _unitOfWork;
        private readonly JwtMiddleware _jwtMiddleware;
      

        public GoogleService(DbContext dbContext, IUnitOfWork unitOfWork, JwtMiddleware jwtMiddleware)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _jwtMiddleware = jwtMiddleware;
           
        }
        public async Task<ValidUserDetailDC> ValidateGoogleToken(GooglePayloadDc googlePayloadDc)
        {
            string validtoken = null;
            ValidUserDetailDC validuserdetails = new ValidUserDetailDC();
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

            string clientid = _dbContext.GetGoogleClientId();
            // Change this to your google client ID
            settings.Audience = new List<string>() { clientid };

            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(googlePayloadDc.token, settings).Result;
            //payload.
            bool isuserexist =  await _unitOfWork.UserRepository.CheckUserExist(payload.Email);
            if (isuserexist)
            {
                var data = await _unitOfWork.UserRepository.GetUserDetailbyusername(payload.Email);
                if (data != null)
                {
                    validtoken = _jwtMiddleware.GenerateToken(data);
                     validuserdetails = await _unitOfWork.UserRepository.GetUserDetailbyusername(payload.Email);
                    if (validuserdetails != null)
                    {
                        validtoken = _jwtMiddleware.GenerateToken(validuserdetails);
                        validuserdetails.Token = validtoken;
                    }
                }
            }

            else
            {
                User user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    RoleId = 1,
                    Name = payload.Name

                };

                long res = await  _unitOfWork.UserRepository.SubmitUser(user);
                if (res > 0)
                {
                    _unitOfWork.Commit();
                     validuserdetails = await _unitOfWork.UserRepository.GetUserDetailbyusername(payload.Email);
                    if (validuserdetails != null)
                    {
                        validtoken = _jwtMiddleware.GenerateToken(validuserdetails);
                        validuserdetails.Token = validtoken;
                    }

                }
            }
            
            return validuserdetails;
            //return Ok(new { AuthToken = _jwtGenerator.CreateUserAuthToken(payload.Email) });
        }
    }
}
