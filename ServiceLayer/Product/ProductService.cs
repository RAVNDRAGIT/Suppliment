using AgileObjects.AgileMapper;
using BusinessLayer.Home;
using BusinessLayer.Product;
using Dapper;
using DataContract.Product;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.DataClassification;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ServiceLayer.Product
{

    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductMasterRepository _productMasterRepository;
        private readonly JwtMiddleware _jwtMiddleware;
        private readonly ExcelHelper _excelHelper;
        private readonly FileHelper _fileHelper;
        public ProductService(IUnitOfWork unitOfWork, JwtMiddleware JwtMiddleware, ExcelHelper excelHelper, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtMiddleware = JwtMiddleware;
            _excelHelper = excelHelper;
            _fileHelper = fileHelper;
        }
        public async Task<bool> AddProduct(ProductMasterDC productMasterDC)
        {
            bool result = false;
            long userid = _jwtMiddleware.GetUserId() ?? 0;
            if (productMasterDC != null)
            {
                var productMaster = Mapper.Map(productMasterDC).ToANew<ProductMaster>();
                bool res = await _productMasterRepository.AddProduct(productMaster, userid);
                if (res)
                {

                    _unitOfWork.Commit();
                }
                result = res;
            }
            return result;
        }

        public async Task<List<DynamicProductDC>> GetProductDynamically(ProductFilterDC productFilterDC)

        {
            var data = await _productMasterRepository.GetProductDynamically(productFilterDC);
            return data;
        }

        public async Task<List<DynamicProductDC>> GetLikeProduct(long producttypeid, long productid)
        {

            var data = await _productMasterRepository.GetLikeProduct(producttypeid, productid);
            return data;
        }

        public async Task<List<DynamicProductDC>> GetDiscountProduct(int skip, int take)

        {
            var data = await _productMasterRepository.GetDiscountProduct(skip, take);
            return data;
        }

        public async Task<bool> AddExcelProduct()
        {
            bool result = false;
            long categoryid = 0;
            long producttypeid = 0;
            long goalid = 0;
            long uomid = 0;
            List<ProductMaster> list = new List<ProductMaster>();
            List<ProductImage> productImages = new List<ProductImage>();
            var dt = await _excelHelper.ReadExcelFileAsync();
            if (dt != null && dt.Rows.Count > 0)
            {
                var categorydata = await _unitOfWork.CategoryRepository.GetAllActiveCategories();
                var uomdata = await _unitOfWork.UnitOfMeasurementRepository.GetActiveUom();
                var producttypedata = await _unitOfWork.ProductTypeRepository.GetProductTypeList();
                var goaldata = await _unitOfWork.GoalRepository.GetActiveGoalsAsync();
                for (int i = 0; i <=46; i++)
                {
                    #region SaveCategories
                    bool isexistcategory = !categorydata.Any(x => x.Name.ToUpper() == Convert.ToString(dt.Rows[i][8]).ToUpper());
                    if (isexistcategory)
                    {
                        var category = new CategoryMaster()
                        {
                            Name = Convert.ToString(dt.Rows[i][8]),
                            ImagePath = "http://res.cloudinary.com/dmp8pixmj/image/upload/v1677499300/WheyProtein.jpg"
                        };
                        categoryid = await _unitOfWork.CategoryRepository.Save(category);
                    }
                    else
                    {
                        categoryid = categorydata.Where(x => x.Name.ToUpper() == Convert.ToString(dt.Rows[i][8]).ToUpper()).FirstOrDefault().Id;
                    }
                    #endregion

                    #region SaveUom
                    bool isexistsiuom = !uomdata.Any(x => x.Value.ToUpper() == Convert.ToString(dt.Rows[i][6]).ToUpper());
                    if (isexistsiuom)
                    {
                        var uom = new UnitOfMeasurementMaster()
                        {
                            Value = Convert.ToString(dt.Rows[i][6])
                        };
                        uomid = await _unitOfWork.UnitOfMeasurementRepository.AddUom(uom);
                    }
                    else
                    {
                        uomid = uomdata.Where(x => x.Value.ToUpper() == Convert.ToString(dt.Rows[i][6]).ToUpper()).FirstOrDefault().Id;
                    }
                    #endregion

                    #region SaveProductType
                    bool isexistsproducttype = !producttypedata.Any(x => x.Name.ToUpper() == Convert.ToString(dt.Rows[i][9]).ToUpper());
                    if (isexistsproducttype)
                    {
                        var productype = new ProductType()
                        {
                            Name = Convert.ToString(dt.Rows[i][9]),
                            ImagePath = "http://res.cloudinary.com/dmp8pixmj/image/upload/v1677506560/sbp1.jpg"
                        };
                        producttypeid = await _unitOfWork.ProductTypeRepository.AddProductType(productype);
                    }
                    else
                    {
                        producttypeid = producttypedata.Where(x => x.Name.ToUpper() == Convert.ToString(dt.Rows[i][9]).ToUpper()).FirstOrDefault().Id;
                    }
                    #endregion

                    #region SaveGoal
                    bool isexistsgoal = !goaldata.Any(x => x.Name.ToUpper() == Convert.ToString(dt.Rows[i][10]).ToUpper());
                    if (isexistsgoal)
                    {
                        var goal = new Goal()
                        {
                            Name = Convert.ToString(dt.Rows[i][10]),
                            ImagePath = "http://res.cloudinary.com/dmp8pixmj/image/upload/v1677493507/1.jpg"
                        };
                        goalid = await _unitOfWork.GoalRepository.AddGoal(goal);
                    }
                    else
                    {
                        goalid = goaldata.Where(x => x.Name.ToUpper() == Convert.ToString(dt.Rows[i][10]).ToUpper()).FirstOrDefault().Id;
                    }
                    #endregion

                    #region saveProductMaster
                    ProductMaster pm = new ProductMaster();
                    pm.ProductName = Convert.ToString(dt.Rows[i][0]);
                    pm.ProductDescription = Convert.ToString(dt.Rows[i][1]);
                    pm.Mrp = Math.Round(Convert.ToDouble(dt.Rows[i][2]), 2, MidpointRounding.AwayFromZero);
                    pm.Price = Math.Round(Convert.ToDouble(dt.Rows[i][3]), 2, MidpointRounding.AwayFromZero);
                    double discountpercentage = Math.Round(100 * (Math.Round(Convert.ToDouble(dt.Rows[i][2]), 2, MidpointRounding.AwayFromZero) - Math.Round(Convert.ToDouble(dt.Rows[i][3]), 2, MidpointRounding.AwayFromZero)) / Math.Round(Convert.ToDouble(dt.Rows[i][2]), 2, MidpointRounding.AwayFromZero),2, MidpointRounding.AwayFromZero);
                    pm.DiscountPercentage = discountpercentage;
                    pm.IsActive = Convert.ToBoolean(dt.Rows[i][4]);
                    pm.Weight = Convert.ToInt32(dt.Rows[i][5]);
                    pm.Discount = Math.Round(Math.Round(Convert.ToDouble(dt.Rows[i][2]), 2, MidpointRounding.AwayFromZero) - Math.Round(Convert.ToDouble(dt.Rows[i][3]), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero);
                    pm.IsDelete = false;
                    pm.Created_By = 0;
                    pm.Updated_By = 0;
                    pm.Created_Date = DateTime.Now;
                    pm.Updated_Date = DateTime.Now;
                    pm.UomId = uomid;
                    pm.CategoryId = categoryid;
                    pm.ProductTypeId = producttypeid;
                    pm.GoalId = goalid;
                    #endregion


                    #region SaveImage
                    ProductImage productImage = new ProductImage();
                    string BaseFolder = "F:/ProductImage/";
                    string imgurl = null;
                    if (i <= 46)
                    {
                        //FileInfo file = new FileInfo(BaseFolder + Convert.ToString(i+1) + ".jpg");
                        //if (file.Exists.Equals(true))
                        //{
                        //    imgurl = _fileHelper.UploadImageUrl(BaseFolder + "/" + Convert.ToString(i + 1) + ".jpg");
                        //}
                        //else
                        //{
                        //    imgurl = _fileHelper.UploadImageUrl(BaseFolder + "/" + Convert.ToString(i + 1) + ".webp");

                        //}
                        productImage.ImagePath = "https://res.cloudinary.com/dmp8pixmj/image/upload/v1677784198/"+ (i + 1) + ".webp";
                        productImage.ProductId = i+1;
                        productImage.Created_Date = DateTime.Now;
                        productImage.Created_By = 0;
                        productImage.Updated_Date = DateTime.Now;
                        productImage.Updated_By = 0;
                        productImage.IsActive = true;
                        productImage.IsDelete = false;
                        productImage.IsDefault = true;
                        productImages.Add(productImage);
                    }

                    //else
                    //{
                    //    for (int j = 1; j <= 7; j++)
                    //    {
                    //        FileInfo file = new FileInfo(BaseFolder + "/" + Convert.ToString(j) + ".jpg");
                    //        if (file.Exists.Equals(true))
                    //        {
                    //            imgurl = _fileHelper.UploadImageUrl(BaseFolder + "/" + Convert.ToString(j) + ".jpg");
                    //        }
                    //        else
                    //        {
                    //            FileInfo fileweb = new FileInfo(BaseFolder + "/" + Convert.ToString(j) + ".webp");
                    //            if (fileweb.Exists.Equals(true))
                    //            {
                    //                imgurl = _fileHelper.UploadImageUrl(BaseFolder + "/" + Convert.ToString(j) + ".webp");
                    //            }

                    //        }
                    //        if (j == 1)
                    //        {
                    //            productImage.IsDefault = true;
                    //        }
                    //        else
                    //        {
                    //            productImage.IsDefault = false;
                    //        }
                    //        productImage.ImagePath = imgurl;
                    //        productImage.Created_Date = DateTime.Now;
                    //        productImage.Created_By = 0;
                    //        productImage.Updated_Date = DateTime.Now;
                    //        productImage.Updated_By = 0;
                    //        productImage.IsActive = true;
                    //        productImage.IsDelete = false;
                    //        productImage.ProductId = j;
                    //        productImages.Add(productImage);
                    //    }
                    //}




                    #endregion

                    list.Add(pm);
                }

                if (list != null && list.Count > 0)
                {
                    bool res = await _unitOfWork.ProductMasterRepository.AddProductList(list);
                    long productimageres = await _unitOfWork.ProductImageRepository.SaveImage(productImages);
                    if (res && productimageres > 0)
                    {
                        _unitOfWork.Commit();
                        result = true;
                    }
                }



            }

            return result;
        }
    }
}

            

            

               
        


