﻿using BusinessLayer.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IProductMasterRepository
    {
       Task<bool> AddProduct(ProductMaster productMaster);
        Task<ProductMaster> GetProduct(long id);
    }
}