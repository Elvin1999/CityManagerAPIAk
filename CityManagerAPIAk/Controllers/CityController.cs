using AutoMapper;
using CityManagerAPIAk.Data;
using CityManagerAPIAk.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityManagerAPIAk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;

        public CityController(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        [HttpGet("{id?}")]
        public IActionResult GetCities(int id = 1)
        {
            //var cities = _appRepository.GetCities(id)
            //    .Select(c =>
            //    {
            //        return new CityForListDto
            //        {
            //            Id = c.Id,
            //            Description = c.Description,
            //            Name = c.Name,
            //            PhotoUrl = c.CityImages.FirstOrDefault(p => p.IsMain).Url
            //        };
            //    });
            //return Ok(cities);

            var result = _appRepository.GetCities(id);
            var dtos = _mapper.Map<List<CityForListDto>>(result);

            return Ok(dtos);
        }
    }
}
