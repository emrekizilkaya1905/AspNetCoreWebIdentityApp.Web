using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreWebIdentityApp.Service.Services
{
    public class FileService : IFileService
    {
        private readonly IFileProvider _fileProvider;

        public FileService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<string> SaveUserPictureAsync(IFormFile picture)
        {
            var webRoot = _fileProvider.GetFileInfo("wwwroot").PhysicalPath
                ?? throw new InvalidOperationException("wwwroot directory not found.");
            var folder = Path.Combine(webRoot, "userpictures");
            Directory.CreateDirectory(folder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(picture.FileName)}";
            await using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.CreateNew);
            await picture.CopyToAsync(stream);
            return fileName;
        }
    }
}