using BusinessLayer.Product;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract.Home;
using DataContract.Product;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Product
{
    public class UnitOfMeasurementMasterRepository : GenericRepository<UnitOfMeasurementMaster>, IUnitOfMeasurementRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public UnitOfMeasurementMasterRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public async Task<long> Save(List<UnitOfMeasurementMaster> unitOfMeasurementMaster)
        {
            //unitOfMeasurementMaster.IsActive = true;
            //unitOfMeasurementMaster.IsDelete = false;
            //unitOfMeasurementMaster.Created_Date = DateTime.Now;
            //unitOfMeasurementMaster.Created_By = 0;
            //unitOfMeasurementMaster.Updated_Date = DateTime.Now;
            //unitOfMeasurementMaster.Updated_By = 0;

            long res = await _sqlConnection.InsertAsync<List<UnitOfMeasurementMaster>>(unitOfMeasurementMaster, _transaction);
            return res;
        }

        public async Task<List<UnitOfMeasurementMaster>> GetAllUom()
        {
            var data = await _sqlConnection.GetAllAsync<UnitOfMeasurementMaster>();
            return data.ToList();
        }

        public async Task<long> AddUom(UnitOfMeasurementMaster unitOfMeasurementMaster)
        {
            
            
            unitOfMeasurementMaster.IsActive = true;
            unitOfMeasurementMaster.IsDelete = false;
            unitOfMeasurementMaster.Created_Date = DateTime.Now;
            unitOfMeasurementMaster.Created_By = 0;
            unitOfMeasurementMaster.Updated_Date = DateTime.Now;
            unitOfMeasurementMaster.Updated_By = 0;
            long res = await _sqlConnection.InsertAsync<UnitOfMeasurementMaster>(unitOfMeasurementMaster, _transaction);
            return res;

        }

        public async Task<List<UomDc>> GetActiveUom()
        {
            var dbArgs = new DynamicParameters();
            var data = (await _sqlConnection.QueryAsync<UomDc>("GetActiveUom", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
