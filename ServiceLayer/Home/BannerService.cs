using BusinessLayer.Home;
using DataContract.Home;
using DataLayer.Interface;
using DataLayer.Repository.Home;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Home
{
    public class BannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;
        public BannerService(IUnitOfWork unitOfWork ,FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

       
        public async Task<List<BannerDC>> GetBannerList()
        {
            var data = await _unitOfWork.BannerRepository.GetBanner();
            return data;
        }

        public async Task<long> SaveBulkBanner()
        {
            List<Banner> list = new List<Banner>();
            
           // string bannerpath = "F:/Resources/Images/Banner.jpg";
            //string imagepath= _fileHelper.UploadImageUrl(bannerpath);
            for(int i=0;i<=1;i++)
            {
                Banner banner = new Banner();
                banner.Created_By = 0;
                banner.Created_Date = DateTime.Now;
                banner.Updated_By = 0;
                banner.Updated_Date = DateTime.Now;
                banner.IsActive = true;
                banner.IsDelete = false;
                banner.ImagePath = "http://res.cloudinary.com/dmp8pixmj/image/upload/v1677786521/Banner.webp";
                banner.Redirecturl = "xyz";
                list.Add(banner);

            }
            var data = await _unitOfWork.BannerRepository.SaveBulkBanner(list);
            if(data>0)
            {
                _unitOfWork.Commit();
            }
            return data;
        }
    }
}
