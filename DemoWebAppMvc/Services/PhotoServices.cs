using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DemoWebAppMvc.Helper;
using Microsoft.Extensions.Options;
using DemoWebAppMvc.Interface;

namespace DemoWebAppMvc.Services
{
    public class PhotoServices : IPhotoServices
    {
        private readonly Cloudinary _config;
        public PhotoServices(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );

            _config = new Cloudinary(acc);


        }



        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")

                };
                uploadResult = await _config.UploadAsync(uploadParams);
            }
            return uploadResult;

        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _config.DestroyAsync(deleteParams);

            return result;
        }
    }
}
