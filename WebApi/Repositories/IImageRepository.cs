using WebApi.Models.Domain;

namespace WebApi.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
