using WebApi.Models.Domain;

namespace WebApi.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
