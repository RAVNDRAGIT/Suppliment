using BusinessLayer;
using DataContract;
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
    }
}
