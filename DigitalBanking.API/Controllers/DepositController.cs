using DigitalBanking.Core.Dtos;
using DigitalBanking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;

        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        [HttpPost("create-intent")]
        public async Task<IActionResult> CreateIntent([FromBody] DepositRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var clientSecret = await _depositService
                .CreateDepositIntentAsync(userId, dto.Amount);

            return Ok(new { clientSecret });
        }
    }
}
