using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using WebStore.Configs;

namespace WebStore.Services
{
    public class ImageService
    {
        private readonly Account cloudinaryAccount;
        public ImageService(IOptions<CloudinaryConfig> options)
        {
            cloudinaryAccount = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
            );
        }

        public string SaveImage(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var cloudinary = new Cloudinary(cloudinaryAccount);
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUri.ToString();
        }
    }
}
