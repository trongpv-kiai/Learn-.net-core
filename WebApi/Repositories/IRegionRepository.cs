using WebApi.Models.Domain;

namespace WebApi.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetListAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
