using AutoMapper;
using WebApi.Models.Domain;
using WebApi.Models.DTO;

namespace WebApi.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>();
            CreateMap<UpdateRegionRequestDto, Region>();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
