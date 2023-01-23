using CloudinaryDotNet.Actions;

namespace DemoWebAppMvc.Interface
{
    public interface IPhotoServices
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
