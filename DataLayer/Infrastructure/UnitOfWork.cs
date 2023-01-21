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

namespace DataLayer.Infrastructure
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public IProductMasterRepository ProductMasterRepository { get; }
        public IUserRepository UserRepository { get; }
        public IOrderMasterRepository OrderMasterRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }
       

        //private DbContext _dbContext;
        //private string connStr => _dbContext.CreateConnection().ConnectionString;

        #region Configuration

        public UnitOfWork(DbContext dbContext, IProductMasterRepository productMasterRepository, IUserRepository userRepository, IOrderMasterRepository orderMasterRepository, IOrderDetailRepository orderDetailRepository)
        {
            //_configuration = configuration;
            _connection = new SqlConnection();
            //_connection.ConnectionString = _configuration.GetConnectionString("SqlConnection");
            _connection.ConnectionString = dbContext.CreateConnection().ConnectionString;
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            ProductMasterRepository = productMasterRepository;
            UserRepository = userRepository;
            OrderMasterRepository = orderMasterRepository;
            OrderDetailRepository = orderDetailRepository;
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                //_transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
        private void ResetRepositories()
        {
            
        }
        #endregion
    }
}
