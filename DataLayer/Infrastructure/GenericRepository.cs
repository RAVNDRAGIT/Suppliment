using Dapper.Contrib.Extensions;
using DataLayer.Context;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure
{
    public class GenericRepository<T> where T : class
    {
        private SqlConnection _connection;

       

        protected IDbTransaction _transaction;

       //private DbContext _dbContext;

        protected readonly DbContext _dbContext;

        public GenericRepository(SqlConnection sqlConnection, IDbTransaction transaction)
        {
            _transaction = transaction;
            
                _connection = sqlConnection;
            
            
        }
        //protected GenericRepository(DbContext context)
        //{
        //    _dbContext = context;
        //}
        //protected IDbConnection _connection
        //{
        //    get
        //    {

        //        return _dbContext.CreateConnection();
        //    }
        //}

        public async Task<dynamic> InsertAsync(T entity)
        {
            var id = await _connection.InsertAsync(entity, _transaction);
            return id;
        }

        public async Task<dynamic> InsertAsync1(T entity, string primarykey)
        {
            try
            {
                var id = await _connection.InsertAsync(entity, _transaction);
                if (typeof(T).GetProperties().ToList().Any(x => x.Name == primarykey))
                    typeof(T).GetProperties().ToList().FirstOrDefault(x => x.Name == primarykey).SetValue(entity, id);

                return entity;
            }
            catch (Exception EE)
            {
                throw EE;
            }




        }


        public async Task<T> GetByIdAsync(long Id)
        {
            var entity = await _connection.GetAsync<T>(Id, _transaction);
            return entity;
        }

        public T GetById(long Id)
        {
            T entity = _connection.Get<T>(Id, _transaction);
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await _connection.UpdateAsync(entity, _transaction);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await _connection.DeleteAsync(entity, _transaction);
        }

    }
}
