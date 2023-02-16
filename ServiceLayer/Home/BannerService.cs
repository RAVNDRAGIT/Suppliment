using DataContract.Home;
using DataLayer.Interface;
using DataLayer.Repository.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Home
{
    public class BannerService
    {
        private IBannerRepository    _bannerRepository;
        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

       
        public async Task<List<BannerDC>> GetBannerList()
        {
            var data = await _bannerRepository.GetBanner();
            return data;
        }
    }
}
