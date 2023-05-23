using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebApi.CustomActionFilters;
using WebApi.Models.DTO;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var userExists = await _userManager.FindByEmailAsync(registerRequestDto.UserName);
            if(userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "User already exists!", Status = "Error" });
            }

            var user = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);



            if (result.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    result = await _userManager.AddToRolesAsync(user, registerRequestDto.Roles);
                    if (result.Succeeded) return Ok("User was registered! Please login.");
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                // Create token
                var userRoles = await _userManager.GetRolesAsync(user);
                if(userRoles != null)
                {
                    var token = _tokenRepository.CreateJWTToken(user, userRoles.ToList());
                    return Ok(new LoginResponseDto
                    {
                        JwtToken = token,
                    });
                }
            }
            return Unauthorized(new Response { Message = "Username or password incorrect!", Status = "Error" });
        }

       
    }
}
