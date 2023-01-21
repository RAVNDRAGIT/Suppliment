using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface.IService
{
    public interface IFileService
    {
         Task<string> SaveImageAsync(string filename);
    }
}
