using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.Order;
using DataContract;
using DataLayer.Context;
using DataLayer.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceLayer.Auth;
using ServiceLayer.Interface.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer.Mongo
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _cart;
        public IUnitOfWork _unitofWork;
        public DbContext _dbContext;
        private long? userid;
        public IAuthService _authService;
        public CartService(
            IOptions<MongoDbDC> mongoDbDC, IUnitOfWork unitOfWork, DbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            var mongoClient = new MongoClient(
               _dbContext.MongoConString());

            var mongoDatabase = mongoClient.GetDatabase(
                _dbContext.MongoDbName());

            _cart = mongoDatabase.GetCollection<Cart>(
                _dbContext.MongoOrderCollection());
            _unitofWork = unitOfWork;
            _authService = authService;
            userid = _authService.GetUserId();
        }

        public async Task<List<Cart>> GetAsync() =>
            await _cart.Find(_ => true).ToListAsync();

        public async Task<Cart?> GetAsync( string cookievalue)
        {
            Cart existscart = new Cart();
            if (userid != null && userid.HasValue)
            {
                existscart = await _cart.Find(x => x.Created_By == userid).FirstOrDefaultAsync();
            }
            else
            {
                existscart = await _cart.Find(x => x.CookieValue == cookievalue).FirstOrDefaultAsync();

            }
            return existscart;

        }

        public async Task<string> CreateAsync(CartDetailDC cartDetailDC)
        {
            Cart existscart = new Cart();
            if (userid != null && userid.HasValue)
            {
                existscart = await _cart.Find(x => x.Created_By == userid).FirstOrDefaultAsync();
            }
            else
            {
                existscart = await _cart.Find(x => x.CookieValue == cartDetailDC.CookieValue).FirstOrDefaultAsync();

            }

            if (existscart != null && cartDetailDC.Quantity == 0 && existscart.cartDetails.Any(x => x.ProductId == cartDetailDC.ProductId) && existscart.cartDetails.Count > 1)
            {
                var update = Builders<Cart>.Update.PullFilter(p => p.cartDetails,
                                                f => f.ProductId == cartDetailDC.ProductId);
                var result = _cart
                    .FindOneAndUpdateAsync(p => p.Id == existscart.Id, update).Result;
                return result.Id;
            }
            else if (existscart != null && cartDetailDC.Quantity == 0 && existscart.cartDetails.Any(x => x.ProductId == cartDetailDC.ProductId) && existscart.cartDetails.Count == 1)
            {
                await _cart.DeleteOneAsync(x => x.Id == existscart.Id);
                return null;
            }
            else
            {
                List<CartDetails> listcartDetails = new List<CartDetails>();
                Cart cart = new Cart();
                var data = await _unitofWork.ProductMasterRepository.GetProduct(cartDetailDC.ProductId);
                if (data != null)
                {
                    CartDetails cartDetails = Mapper.Map(data).ToANew<CartDetails>();

                    cartDetails.TotalMrp = data.Mrp * cartDetailDC.Quantity;
                    cartDetails.TotalPrice = data.Price * cartDetailDC.Quantity;
                    cartDetails.TotalDiscount = cartDetails.Quantity * cartDetails.Discount;
                    cartDetails.ProductId = cartDetailDC.ProductId;
                    cartDetails.Created_By = userid;
                    cartDetails.Updated_By = userid;
                    cartDetails.Created_Date = DateTime.Now;
                    cartDetails.Updated_Date = DateTime.Now;
                    cartDetails.Quantity = cartDetailDC.Quantity;
                    listcartDetails.Add(cartDetails);


                    cart.cartDetails = listcartDetails;
                    cart.TotalMrp = data.Mrp * cartDetailDC.Quantity;
                    cart.TotalDiscount = cartDetails.Quantity * cartDetails.Discount;
                    cart.TotalPrice = data.Price * cartDetailDC.Quantity;
                    cart.Created_By = userid;
                    cart.CookieValue = cartDetailDC.CookieValue;
                    cart.Updated_By = userid;
                    cart.Created_Date = DateTime.Now;
                    cart.Updated_Date = DateTime.Now;

                }

                if (existscart != null && existscart.cartDetails.Any(x => x.ProductId == cartDetailDC.ProductId))
                {
                    var filter = Builders<Cart>.Filter.Eq(s => s.Id, existscart.Id);
                    existscart.cartDetails = listcartDetails;
                    if (existscart.cartDetails.Count > 1)
                    {
                        existscart.TotalMrp = existscart.TotalMrp + cart.TotalMrp;
                        existscart.TotalDiscount = existscart.TotalDiscount + cart.TotalDiscount;
                        existscart.TotalPrice = existscart.TotalPrice + cart.TotalPrice;

                    }
                    else
                    {
                        existscart.TotalMrp = cart.TotalMrp;
                        existscart.TotalDiscount = cart.TotalDiscount;
                        existscart.TotalPrice = cart.TotalPrice;
                    }


                    existscart.Updated_By = userid;
                    existscart.Updated_Date = DateTime.Now;
                    //var update = Builders<CartDetails>.Filter.Where(s => s.ProductId== cartDetailDC.ProductId);
                    //var updatecurrentcart= Builders<CartDetails>.Update.Set(s => s.Quantity, data.Quantity);
                    //var result = await _cart.UpdateOneAsync(filter, update);
                    await _cart.ReplaceOneAsync(x => x.Id == existscart.Id, existscart, new UpdateOptions { IsUpsert = true });
                    return existscart.Id;

                }
                else if (existscart != null && existscart.cartDetails.Any(x => x.ProductId != cartDetailDC.ProductId))
                {
                    var filter = Builders<Cart>.Filter.Eq(s => s.Id, existscart.Id);
                    existscart.cartDetails.AddRange(listcartDetails);
                    existscart.TotalMrp = existscart.TotalMrp + cart.TotalMrp;
                    existscart.TotalDiscount = existscart.TotalDiscount + cart.TotalDiscount;
                    existscart.TotalPrice = existscart.TotalPrice + cart.TotalPrice;
                    existscart.Updated_By = userid;
                    existscart.Updated_Date = DateTime.Now;
                    //var update = Builders<CartDetails>.Filter.Where(s => s.ProductId== cartDetailDC.ProductId);
                    //var updatecurrentcart= Builders<CartDetails>.Update.Set(s => s.Quantity, data.Quantity);
                    //var result = await _cart.UpdateOneAsync(filter, update);
                    await _cart.ReplaceOneAsync(x => x.Id == existscart.Id, existscart, new UpdateOptions { IsUpsert = true });
                    return existscart.Id;
                }



                else
                {
                    await _cart.InsertOneAsync(cart);
                }

                return cart.Id;
            }
        }




        public async Task UpdateAsync(string id, Cart updatedBook) =>
            await _cart.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task<bool> RemoveAsync( string cookievalue)
        {
            Cart existscart = new Cart();
            if (userid != null && userid.HasValue)
            {
                existscart = await _cart.Find(x => x.Created_By == userid).FirstOrDefaultAsync();
            }
            else
            {
                existscart = await _cart.Find(x => x.CookieValue == cookievalue).FirstOrDefaultAsync();

            }

            var res = await _cart.DeleteOneAsync(x => x.Id == existscart.Id);
            if (res.IsAcknowledged)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}

