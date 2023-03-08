using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.Order;
using DataContract;
using DataContract.Cart;
using DataLayer.Context;
using DataLayer.Interface;
using DataLayer.Repository.Product;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceLayer.Auth;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer.Carts
{
    public class CartService
    {

        public IUnitOfWork _unitofWork;
        public MongoHelper _mongoHelper;
        private long? userid;
        public JwtMiddleware _jwtMiddleware;
   
        public CartService(
            IOptions<MongoDbDC> mongoDbDC, IUnitOfWork unitOfWork, JwtMiddleware jwtMiddleware, MongoHelper mongoHelper)
        {
            _unitofWork = unitOfWork;
            _mongoHelper = mongoHelper;
            _jwtMiddleware = jwtMiddleware;

           
        }

        public async Task<List<Cart>> GetAsync() =>
             await _mongoHelper.OrderCollection().Find(_ => true).ToListAsync();

        public async Task<Cart?> GetAsync(string cookievalue)
        {
            userid = _jwtMiddleware.GetUserId();
            Cart existscart = new Cart();
            if (userid != null && userid.HasValue)
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid).FirstOrDefaultAsync();
            }
            else
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.CookieValue == cookievalue).FirstOrDefaultAsync();

            }

            
            return existscart;

        }

        public async Task<string> CreateAsync(CartDetailDC cartDetailDC)
        {
            Cart existscart = new Cart();
            userid = _jwtMiddleware.GetUserId();
            if (userid != null && userid.HasValue)
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid).FirstOrDefaultAsync();
            }
            else
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.CookieValue == cartDetailDC.CookieValue).FirstOrDefaultAsync();

            }

            if (existscart != null && cartDetailDC.Quantity <= 0)
            {
                if (existscart.cartDetails.Any(x => x.ProductId == cartDetailDC.ProductId) && existscart.cartDetails.Count > 1)
                {
                    var update = Builders<Cart>.Update.PullFilter(p => p.cartDetails,
                                                    f => f.ProductId == cartDetailDC.ProductId);
                    var result = _mongoHelper.OrderCollection()
                        .FindOneAndUpdateAsync(p => p.Id == existscart.Id, update).Result;
                    return result.Id;
                }
                else
                {
                    await _mongoHelper.OrderCollection().DeleteOneAsync(x => x.Id == existscart.Id);
                    return null;
                }
                
            }
            else if(existscart == null && cartDetailDC.Quantity == 0)
            {
                return null;
            }
            //else if (existscart != null && cartDetailDC.Quantity == 0 && existscart.cartDetails.Any(x => x.ProductId == cartDetailDC.ProductId) && existscart.cartDetails.Count == 1)
            //{
               
            //}
            else
            {
                List<CartDetails> listcartDetails = new List<CartDetails>();
                Cart cart = new Cart();
                // var data = await _productMasterRepository.GetProduct(cartDetailDC.ProductId);
                var data = await _unitofWork.ProductMasterRepository.GetProductForCart(cartDetailDC.ProductId);
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
                    var existcartproduct = existscart.cartDetails.Where(x => x.ProductId == cartDetailDC.ProductId).FirstOrDefault();
                    if(existcartproduct!=null)
                    {
                        existscart.cartDetails.Remove(existcartproduct);
                    }
                    existscart.cartDetails.AddRange(listcartDetails);
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
                    existscart.cartDetails= existscart.cartDetails.OrderByDescending(x => x.Created_Date).ToList();
                   await _mongoHelper.OrderCollection().ReplaceOneAsync(x => x.Id == existscart.Id, existscart, new UpdateOptions { IsUpsert = true });
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
                    existscart.cartDetails = existscart.cartDetails.OrderByDescending(x => x.Created_Date).ToList();

                    await _mongoHelper.OrderCollection().ReplaceOneAsync(x => x.Id == existscart.Id, existscart, new UpdateOptions { IsUpsert = true });
                    return existscart.Id;
                }



                else
                {
                    await _mongoHelper.OrderCollection().InsertOneAsync(cart);
                }

                return cart.Id;
            }
        }




        public async Task<string> AssignCartAsync(AssignCartDC assignCartDC)
        {

            Cart existingcartuser = new Cart();
            userid = _jwtMiddleware.GetUserId();
            var currentcart = await _mongoHelper.OrderCollection().Find(x => x.CookieValue == assignCartDC.CookieValue).FirstOrDefaultAsync();

            if (userid != null && userid.HasValue && currentcart != null)
            {
                existingcartuser = await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid).FirstOrDefaultAsync();
                if (existingcartuser != null)
                {
                    List<CartDetails> listcartDetails = new List<CartDetails>();
                    Cart cart = new Cart();
                    foreach (var cartitem in existingcartuser.cartDetails)
                    {
                        // var data = await _productMasterRepository.GetProduct(cartitem.ProductId);
                        var data = await _unitofWork.ProductMasterRepository.GetProduct(cartitem.ProductId);
                        if (data != null)
                        {
                            CartDetails cartDetails = Mapper.Map(data).ToANew<CartDetails>();

                            var existingproduct = existingcartuser.cartDetails.Where(x => x.ProductId == cartitem.ProductId).FirstOrDefault();
                            if (existingproduct != null)
                            {
                                cartDetails.TotalMrp = data.Mrp * cartitem.Quantity;
                                cartDetails.TotalPrice = data.Price * cartitem.Quantity;
                                cartDetails.TotalDiscount = cartDetails.Quantity * cartDetails.Discount;
                            }
                            else
                            {
                                cartDetails.TotalMrp = data.Mrp * existingproduct.Quantity;
                                cartDetails.TotalPrice = data.Price * existingproduct.Quantity;
                                cartDetails.TotalDiscount = existingproduct.Quantity * existingproduct.Discount;
                            }


                            cartDetails.ProductId = cartitem.ProductId;
                            cartDetails.Created_By = userid;
                            cartDetails.Updated_By = userid;
                            cartDetails.Created_Date = DateTime.Now;
                            cartDetails.Updated_Date = DateTime.Now;
                            cartDetails.Quantity = cartitem.Quantity;
                            listcartDetails.Add(cartDetails);

                        }
                        cart.cartDetails = listcartDetails;
                        cart.TotalMrp = listcartDetails.Sum(X => X.TotalMrp);
                        cart.TotalDiscount = listcartDetails.Sum(X => X.TotalDiscount);
                        cart.TotalPrice = listcartDetails.Sum(X => X.TotalPrice);
                        cart.Created_By = userid;
                        cart.Updated_By = userid;
                        cart.Updated_Date = DateTime.Now;
                        cart.Id = assignCartDC.MongoId;
                        cart.cartDetails = cart.cartDetails.OrderByDescending(x => x.Created_Date).ToList();

                        await _mongoHelper.OrderCollection().ReplaceOneAsync(x => x.Id == assignCartDC.MongoId, cart, new UpdateOptions { IsUpsert = true });
                        return existingcartuser.Id;
                    }

                }
                else
                {
                    currentcart.Created_By = userid;
                    currentcart.Updated_By = userid;
                    currentcart.Updated_Date = DateTime.Now;
                    await _mongoHelper.OrderCollection().ReplaceOneAsync(x => x.Id == assignCartDC.MongoId, currentcart, new UpdateOptions { IsUpsert = true });
                    return currentcart.Id;
                }

            }
            else
            {
                return null;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(string cookievalue)
        {
            Cart existscart = new Cart();
            userid = _jwtMiddleware.GetUserId();
            if (userid != null && userid.HasValue)
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid).FirstOrDefaultAsync();
            }
            else
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.CookieValue == cookievalue).FirstOrDefaultAsync();

            }

            var res = await _mongoHelper.OrderCollection().DeleteOneAsync(x => x.Id == existscart.Id);
            if (res.IsAcknowledged)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> Removeitem(RemoveItemDC removeItemDC)
        {
            bool finalresult = false;
            Cart existscart = new Cart();
           
            userid = _jwtMiddleware.GetUserId();
            if (userid != null && userid.HasValue)
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid).FirstOrDefaultAsync();

            }
            else
            {
                existscart = await _mongoHelper.OrderCollection().Find(x => x.CookieValue == removeItemDC.cookievalue).FirstOrDefaultAsync();

            }
            if (existscart != null && existscart.cartDetails.Count <= 1)
            {
                var res = await _mongoHelper.OrderCollection().DeleteOneAsync(x => x.Id == existscart.Id);
                if (res.IsAcknowledged)
                {
                    finalresult= true;
                }
                else
                {
                    finalresult= false;
                }
            }
            else
            {
                var update = Builders<Cart>.Update.PullFilter(p => p.cartDetails,
                                               f => f.ProductId == removeItemDC.productid);
                var result = _mongoHelper.OrderCollection()
                    .FindOneAndUpdateAsync(p => p.Id == removeItemDC.mongoid, update);
                if (result.IsCompleted)
                {
                    finalresult = true;
                }
                else
                {
                    finalresult = false;
                }
            }

            return finalresult;
        }

        public async Task<Cart> GetCartForUser(string  mongoId)
        {
            userid = _jwtMiddleware.GetUserId();
            var cart= await _mongoHelper.OrderCollection().Find(x => x.Id == mongoId && x.Created_By==userid).FirstOrDefaultAsync();
            return cart;
        }

    }
}

