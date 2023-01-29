using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IUserLocationRepository
    {
        Task<long> SubmitUserLocation(UserLocation userLocations,long userid);
        Task<UserLocation> GetById(long id);
    }
}
