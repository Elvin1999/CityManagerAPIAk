using CityManagerAPIAk.Data;
using CityManagerAPIAk.Dtos;
using CityManagerAPIAk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityManagerAPIAk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IAuthRepository _authRepository;
        private IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto dto)
        {
            if(await _authRepository.UserExists(dto.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new User
            {
                Username = dto.Username,
            };

            await _authRepository.Register(userToCreate, dto.Username);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto dto)
        {
            var user = await _authRepository.Login(dto.Username, dto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var word = _configuration.GetSection("AppSettings:Token").Value;
            var key=Encoding.ASCII.GetBytes(word);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.Username)
                    }),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);
            return Ok(tokenString);
        }

    }
}
