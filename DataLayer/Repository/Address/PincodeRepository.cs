using Dapper;
using DataContract.Address;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using DataLayer.Repository.Order;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Address
{
    public class PincodeRepository : GenericRepository<OrderDetailRepository>, IPincodeRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public PincodeRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task< AddressResultDC> GetDetailsbyPincode(string pincode)
        {
            var dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@pincode", value: pincode);
        
            //var data =(_sqlConnection.QueryMultiple<AddressDetailDC>)
            var data = (await _sqlConnection.QueryMultipleAsync("GetDetailsbyPincode", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            if (data != null)
            {
                var addressdetail = (data.Read<AddressDetailDC>()).ToList();
                var statedistrict = (data.Read<StateDistrictDC>()).FirstOrDefault();
                AddressResultDC addressResultDC = new AddressResultDC()
                {
                    addressDetail = addressdetail,
                    stateDistrict = statedistrict

                };
                return addressResultDC;
            }
            else
            {
                return null;
            }
        }
    }
}
