using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper.Contrib.Extensions;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Interface;
using DataLayer.Context;
using DataLayer.Repository.Auth;
using System.Transactions;

namespace DataLayer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
      
         private DbContext _dbContext;

        IDbTransaction _dbTransaction;
        #region Configuration
        public IOrderDetailRepository OrderDetailRepository
        { get;  }
        public IOrderMasterRepository OrderMasterRepository 
        { get; }





        public UnitOfWork(DbContext dbContext, IDbTransaction dbTransaction, IOrderDetailRepository orderDetailRepository, IOrderMasterRepository orderMasterRepository)
        {
            _dbContext = dbContext;
            _dbTransaction = dbTransaction;
            OrderDetailRepository = orderDetailRepository;
            OrderMasterRepository = orderMasterRepository;
        }
        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                // By adding this we can have muliple transactions as part of a single request
                //_dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
           
        }
        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
            
           
        }
        

        public string AuthKey()
        {
           
            string authKey = _dbContext.AuthKey();
            return authKey;
        }

        public string MongoConString()
        {
            string con = _dbContext.MongoConString();
            return con;
        }

        public string MongoDbName()
        {
            string con = _dbContext.MongoDbName();
            return con;
        }

        public string MongoOrderCollection()
        {
            string con = _dbContext.MongoOrderCollection();
            return con;
        }

       
        #endregion
    }
}
