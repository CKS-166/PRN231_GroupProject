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
    public class UploadVideoService : IVideoService
    {
        private readonly Cloudinary _cloudinary;
        public UploadVideoService(IOptions<CloudinarySettings> config) {
            var acc = new Account
           (
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
           );
            _cloudinary = new Cloudinary(acc);
        }

        public Task<DeletionResult> DeleteVideoAsync(string photoId)
        {
            throw new NotImplementedException();
        }

        public async Task<VideoUploadResult> UploadVideoAsync(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParam = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    EagerTransforms = new List<Transformation>()
                    {
                        new EagerTransformation().Width(300).Height(300).Crop("pad").AudioCodec("none"),
                        new EagerTransformation().Width(160).Height(100).Crop("crop").Gravity("south").AudioCodec("none")
                    }
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParam);

            }
            return uploadResult;
        }
    }
}
