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


        private readonly string _connectionString;
        IDbTransaction _dbTransaction;
        #region Configuration
        public IOrderDetailRepository OrderDetailRepository
        { get;  }
        public IOrderMasterRepository OrderMasterRepository 
        { get; }

        public IUserRepository UserRepository { get; }

        public IUserLocationRepository UserLocationRepository { get; }

        public IProductTypeRepository ProductTypeRepository { get; }
        public IProductMasterRepository ProductMasterRepository { get; }
        public IUnitOfMeasurementRepository UnitOfMeasurementRepository { get; }

        public IGoalRepository GoalRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IProductImageRepository ProductImageRepository { get; }
        public IBannerRepository BannerRepository { get; }
        public UnitOfWork( IDbTransaction dbTransaction, IOrderDetailRepository orderDetailRepository, IOrderMasterRepository orderMasterRepository, IUserRepository userRepository,IUserLocationRepository userLocationRepository,IProductTypeRepository productTypeRepository,IProductMasterRepository productMasterRepository, IUnitOfMeasurementRepository unitOfMeasurementRepository, IGoalRepository goalRepository,ICategoryRepository categoryRepository, IProductImageRepository productImageRepository, IBannerRepository bannerRepository)
        {
           
            _dbTransaction = dbTransaction;
            OrderDetailRepository = orderDetailRepository;
            OrderMasterRepository = orderMasterRepository;
            UserRepository = userRepository;
            UserLocationRepository = userLocationRepository;
            ProductTypeRepository = productTypeRepository;
            ProductMasterRepository = productMasterRepository;
            UnitOfMeasurementRepository = unitOfMeasurementRepository;
            GoalRepository = goalRepository;
            CategoryRepository = categoryRepository;
            ProductImageRepository= productImageRepository;
            BannerRepository = bannerRepository;
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

        public IDbConnection CreateConnection()
      => new SqlConnection(_connectionString);
        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();


        }
        

      

       
        #endregion
    }
}
