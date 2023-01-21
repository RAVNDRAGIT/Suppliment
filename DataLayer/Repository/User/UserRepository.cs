using BusinessLayer;
using BusinessLayer.ProductMaster;
using Dapper;
using DataContract;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Auth
{
    public class UserRepository : GenericRepository<User> ,IUserRepository
    {
       
       
        public UserRepository(DbContext dbContext) :base(dbContext)
        {
            
        }
      
        public ValidUserDetailDC Authentication(User user)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@username", value: user.UserName);
            dbArgs.Add(name: "@password", value: user.Password);
            var data = ( _connection.Query<ValidUserDetailDC>("GetValidUserDetail", param: dbArgs, commandType: CommandType.StoredProcedure)).FirstOrDefault();

            if (data==null)
            {
                return null;
            }
            else
            {
                return data;
                
            }
          
          
        }

        
    }
}
