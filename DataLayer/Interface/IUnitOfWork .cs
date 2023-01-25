using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        string AuthKey();
        string MongoConString();
        string MongoDbName();
        string MongoOrderCollection();
        void Commit();
        IOrderDetailRepository OrderDetailRepository { get; }
        IOrderMasterRepository OrderMasterRepository { get; }
        //void BeginTrans();
    }
}
