using DigitalBanking.Core.Dtos;
using DigitalBanking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitalBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;

        public AuthController(IAuthService authService, IFileService fileService)
        {
            _authService = authService;
            _fileService = fileService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> SignUpAsUser([FromForm] RegisterDto dto,IFormFile? avatar)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? avatarUrl = null;
            if (avatar != null)
                avatarUrl = await _fileService.SaveAvatarAsync(avatar);


            var result = await _authService.Register(dto,avatarUrl);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignInForUser([FromBody] LoginDto dto)
        {
            var result = await _authService.Login(dto);
            if (!result.IsSuccess)
                return Unauthorized();

            return Ok(new
            {
                Token = result.Token,
                Expiration = result.Expiration,
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin()
        {
            var result=await _authService.CreateAdminAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Test üçün exception");
        }


    }
}
