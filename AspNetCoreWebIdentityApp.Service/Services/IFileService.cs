using Microsoft.AspNetCore.Http;

namespace AspNetCoreWebIdentityApp.Service.Services
{
    public interface IFileService
    {
        Task<string> SaveUserPictureAsync(IFormFile picture);
    }
}