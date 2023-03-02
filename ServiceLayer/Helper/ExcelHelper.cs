using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helper
{
    public class ExcelHelper
    {
        #region Variable Declaration
      
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExcelHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        string message = "";
        DataSet dsexcelRecords = new DataSet();
        IExcelDataReader reader = null;

        Stream FileStream = null;
        #endregion

        public async Task<DataTable> ReadExcelFileAsync()
        {
            var formCollection = await _httpContextAccessor.HttpContext.Request.ReadFormAsync();
            var Inputfile = formCollection.Files.First();


            FileStream = Inputfile.OpenReadStream();

            if (Inputfile != null && FileStream != null)
            {
                if (Inputfile.FileName.EndsWith(".xls"))
                    reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                else if (Inputfile.FileName.EndsWith(".xlsx"))
                    reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                else
                    message = "The file format is not supported.";

                dsexcelRecords = reader.AsDataSet();
                reader.Close();

                if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                {
                    DataTable dt = dsexcelRecords.Tables[0];
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
}

