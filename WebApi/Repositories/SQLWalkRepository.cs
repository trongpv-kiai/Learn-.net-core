using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models.Domain;
using WebApi.Models.DTO;
using WebApi.Repositories.Interface;

namespace WebApi.Repositories
{
    public class SQLWalkRepository: IWalkRepository
    {
        private readonly WebApiDbContext _webApiDbContext;
        public SQLWalkRepository(WebApiDbContext webApiDbContext)
        {
            _webApiDbContext = webApiDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _webApiDbContext.Walks.AddAsync(walk);
            await _webApiDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool? isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _webApiDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).AsQueryable();

            // Filtering
            if(filterOn.IsNullOrEmpty() == false && filterQuery.IsNullOrEmpty() == false)
            {
                if(filterOn != null && filterQuery != null && filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if(sortBy.IsNullOrEmpty() == false)
            {
                if(sortBy != null && sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy != null && sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetById(Guid id)
        {
            return await _webApiDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existedWalk = await _webApiDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
            if (existedWalk == null) return null;
            existedWalk.Name = walk.Name;
            existedWalk.DifficultyId = walk.DifficultyId;
            existedWalk.WalkImageUrl = walk.WalkImageUrl;
            existedWalk.LengthInKm = walk.LengthInKm;
            existedWalk.RegionId = walk.RegionId;

            await _webApiDbContext.SaveChangesAsync();
            return existedWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existedWalk = await _webApiDbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
            if (existedWalk == null) return null;
            _webApiDbContext.Walks.Remove(existedWalk);
            return existedWalk;
        }
    }
}
