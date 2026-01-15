using DigitalBanking.Core.Dtos;
using DigitalBanking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigitalBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        public ProfileController(IUserService userService, IFileService fileService)
        {
            _userService = userService;
            _fileService = fileService;
        }
        [HttpGet("get-balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var balance = await _userService.GetBalance(userId);
            return Ok(balance);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            await _userService.UpdateProdileAsync(userId, dto);
            return Ok("Update successfully");
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var photoUrl = await _fileService.SaveAvatarAsync(file);

            await _userService.UpdateProfileAvatarAsync(userId, photoUrl);

            return Ok(new { photoUrl });
        }

        [HttpGet("my-cards")]
        public async Task<IActionResult> GetMyCards()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _userService.GetMyCardsAsync(userId);
            return Ok(result);
        }


        [HttpGet("get-profile-info")]
        public async Task<IActionResult> GetProfileInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var profileInfo=await _userService.GetProfileInfosAsync(userId);
            
            if (profileInfo == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(profileInfo.PhotoUrl))
            {
                profileInfo.PhotoUrl = $"{Request.Scheme}://{Request.Host}{profileInfo.PhotoUrl}";
            }
            return Ok(profileInfo);

        }

        [HttpGet("my-transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _userService.GetTransactionInfoAsync(userId);

            return Ok(result);
        }

    }
}
