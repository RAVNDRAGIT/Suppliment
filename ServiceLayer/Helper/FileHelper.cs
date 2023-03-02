
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helper
{
    public class FileHelper
    {
        private readonly DbContext _dbContext;
        public FileHelper(DbContext dbContext)
        {
            _dbContext=dbContext;
           
        }
        public static string GetUniqueFileName(string fileName)


        {


            fileName = Path.GetFileName(fileName);


            return string.Concat(Path.GetFileNameWithoutExtension(fileName)


                                , "_"


                                , Guid.NewGuid().ToString().AsSpan(0, 4)


                                , Path.GetExtension(fileName));


        }


        public string UploadImageUrl(string img)
        {
            Account account = new Account(
   _dbContext.GetCloudinaryName(),
   _dbContext.GetCloudinaryApiKey(),
  _dbContext.GetCloudinaryApiSecret());

            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription( img),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.Uri.ToString();
        }
    }
}
