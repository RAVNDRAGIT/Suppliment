using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IUnitOfWork : IDisposable
    {
       
        void Commit();
        IOrderDetailRepository OrderDetailRepository { get; }
        IOrderMasterRepository OrderMasterRepository { get; }
        IUserRepository UserRepository { get; }
        IUserLocationRepository UserLocationRepository { get; }
        //void BeginTrans();
    }
}
