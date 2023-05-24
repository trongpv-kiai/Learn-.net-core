using System;
using WebApi.Data;
using WebApi.Models.Domain;

namespace WebApi.Repositories
{
    public class LocalImageRepossitory : IImageRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly WebApiDbContext _webApiDbContext;

        public LocalImageRepossitory(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, WebApiDbContext webApiDbContext)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _webApiDbContext = webApiDbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_environment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}" );

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}" +
                $"{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            await _webApiDbContext.Images.AddAsync(image);
            await _webApiDbContext.SaveChangesAsync();
            return image;
        }
    }
}
