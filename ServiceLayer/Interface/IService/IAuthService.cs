using DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface.IService
{
    public interface IAuthService
    {
        string Token(UserCredentialDC userCredentialDC);
        string GetUserName();
        long? GetUserId();

    }
}
