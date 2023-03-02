using BusinessLayer.Product;
using DataContract.Home;
using DataContract.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IUnitOfMeasurementRepository
    {
        Task<long> Save(List<UnitOfMeasurementMaster> unitOfMeasurementMaster);
        Task<List<UnitOfMeasurementMaster>> GetAllUom();
        Task<long> AddUom(UnitOfMeasurementMaster unitOfMeasurementMaster);

        Task<List<UomDc>> GetActiveUom();
    }
}
