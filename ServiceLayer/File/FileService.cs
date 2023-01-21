using Azure.Core;
using DataLayer.Context;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Helper;
using ServiceLayer.Interface.IHelper;
using ServiceLayer.Interface.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.File
{
    public class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        public FileService(IHttpContextAccessor httpContextAccessor)

        {
            _httpContextAccessor = httpContextAccessor;
            

        }
        public async Task<string> SaveImageAsync(string filename)
        {
            string imagepath = string.Empty;
           // var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];

            var formCollection = await _httpContextAccessor.HttpContext.Request.ReadFormAsync();
            var file = formCollection.Files.First();
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string UniquefileName = FileHelper.GetUniqueFileName(fileName);
                string fullPath = Path.Combine(pathToSave, UniquefileName);
                
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                imagepath = Path.Combine(folderName, UniquefileName);
            }
            return imagepath;
        }

      
    }
}
