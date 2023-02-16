using BusinessLayer.Users;
using DataContract.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IUserRepository
    {
        ValidUserDetailDC Authentication(User user);
        Task<User> GetUser(long userid);
        Task<long> SubmitUser(User user);
        Task <bool> CheckUserExist(string username);
        Task<ValidUserDetailDC> GetUserDetailbyusername(string username);
    }
}
