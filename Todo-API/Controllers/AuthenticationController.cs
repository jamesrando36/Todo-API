using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo_API.Entities;
using Todo_API.Exceptions;
using Todo_API.Models.AuthResultDto;
using Todo_API.Models.UserRequestDtos;

namespace Todo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequestDto userRequestDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userRequestDto.Email);

                if (user != null)
                {
                    throw new NotFoundException("This user does not exist, please try again");
                }

                var newUser = new IdentityUser { UserName = userRequestDto.Email, Email = userRequestDto.Email };

                var createUser = await _userManager.CreateAsync(newUser, userRequestDto.Password);

                if (createUser.Succeeded)
                {
                    var token = GenerateToken(newUser);

                    return Ok(new AuthResult()
                    {
                        Token = token,
                        Result = true
                    });
                }
                else
                {
                    throw new BadRequestException("This user has failed to be created, please try again");
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userLoginRequestDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginRequestDto.Email);

                if (user == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() { "Invalid email, please try again" },
                        Result = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(user, userLoginRequestDto.Password);

                if (!isCorrect) 
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() { "Invalid credentials, please try again" },
                        Result = false
                    });
                }

                var jwtToken = GenerateToken(user);

                return Ok(new AuthResult() { Token = jwtToken, Result = true });
            }

            throw new BadRequestException("This user login has failed authentication, please try again");
        }

        private string GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256 )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}