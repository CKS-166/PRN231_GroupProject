using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FPTU_Starter.Infrastructure.CloudinaryClassSettings;
using FPTU_Starter.Infrastructure.OuterService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.OuterService.Implementation
{
    public class UploadPhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public UploadPhotoService(IOptions<CloudinarySettings> config) {
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        public Task<DeletionResult> DeletePhotoAsync(string photoId)
        {
            throw new NotImplementedException();
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParam = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParam);
               
            }
            return uploadResult;
        }
    }
}
