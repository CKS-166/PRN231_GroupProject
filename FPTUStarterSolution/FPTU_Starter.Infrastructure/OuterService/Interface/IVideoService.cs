using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.OuterService.Interface
{
    public interface IVideoService
    {
        Task<VideoUploadResult> UploadVideoAsync(IFormFile file);
        Task<DeletionResult> DeleteVideoAsync(string photoId);
    }
}
