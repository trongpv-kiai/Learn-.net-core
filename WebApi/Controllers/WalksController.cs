using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.CustomActionFilters;
using WebApi.Models.Domain;
using WebApi.Models.DTO;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(AddWalkRequestDto addWalkRequestDto)
        {
            // Map dto to domain model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy = null,
            [FromQuery] bool? isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var listWalkDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(_mapper.Map<List<WalkDto>>(listWalkDomainModel));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walkDomainModel = await _walkRepository.GetById(id);
            if (walkDomainModel == null) return NotFound();
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null) return NotFound();
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            if (walkDomainModel == null) return NotFound();
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
