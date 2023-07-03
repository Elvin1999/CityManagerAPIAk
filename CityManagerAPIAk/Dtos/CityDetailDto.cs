using CityManagerAPIAk.Models;

namespace CityManagerAPIAk.Dtos
{
    public class CityDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<CityImage>? CityImages { get; set; }
    }
}
