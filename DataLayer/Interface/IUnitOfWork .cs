using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IProductMasterRepository ProductMasterRepository { get; }
        IUserRepository UserRepository { get; }

        IOrderMasterRepository OrderMasterRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        void Commit();
    }
}
