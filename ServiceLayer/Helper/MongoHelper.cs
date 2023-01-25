﻿using BusinessLayer.Order;
using DataLayer.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helper
{
    public class MongoHelper
    {
        private DbContext _dbContext;
        private readonly IMongoCollection<Cart> _cart;
        private readonly string _connectionString;
        private readonly string _dBName;
        public MongoHelper(DbContext dbContext)
        {
            _dbContext = dbContext;
            var mongoClient = new MongoClient(
           _dbContext.MongoConString());

            var mongoDatabase = mongoClient.GetDatabase(
                _dbContext.MongoDbName());
            _cart = mongoDatabase.GetCollection<Cart>(
                _dbContext.MongoOrderCollection());
        }

        public MongoClient GetMongoConstr()
        {
            var mongoClient = new MongoClient(
           _dbContext.MongoConString());
            return mongoClient;
        }
        public IMongoDatabase GetDatabase()
        {
            var mongoClient = GetMongoConstr();
            var mongoDatabase = mongoClient.GetDatabase(
               _dbContext.MongoDbName());
            return mongoDatabase;
        }

        public IMongoCollection<Cart> OrderCollection()
        {
            var mongoDatabase= GetDatabase();
            var collection = mongoDatabase.GetCollection<Cart>(
               _dbContext.MongoOrderCollection());
            return collection;
        }

    }
}
