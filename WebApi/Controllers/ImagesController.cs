using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Domain;
using WebApi.Models.DTO;
using WebApi.Repositories.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            ValidateImage(imageUploadRequestDto);
            if(ModelState.IsValid)
            {
                // Map dto to model image
                var image = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileDescription = imageUploadRequestDto.FileDescription,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileName = imageUploadRequestDto.FileName,
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                };

                // Save file
                await _imageRepository.Upload(image);
                return Ok(image);
            }
            return BadRequest(ModelState);
        }

        private void ValidateImage(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            
            if(!allowedExtensions.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if(imageUploadRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "The file is too big, please upload a smaller file size");
            }
        }
    }
}
