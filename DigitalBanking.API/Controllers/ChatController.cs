using DigitalBanking.Core.Interfaces.Repository;
using DigitalBanking.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalBanking.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatRepository _repo;

    public ChatController(IChatRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("{otherUserId}")]
    public async Task<IActionResult> GetHistory(string otherUserId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId == null)
        {
            return NotFound();
        }

        var messages = await _repo.GetConversationAsync(currentUserId, otherUserId);

        return Ok(messages.Select(m => new {
            from = m.FromUserId == currentUserId ? "me" : "other",
            text = m.Text,
            time = m.CreatedAt
        }));
    }

    [HttpDelete("{otherUserId}")]
    public async Task<IActionResult> DeleteConversation(string otherUserId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId == null)
        {
            return NotFound();
        }
        await _repo.DeleteConversationAsync(currentUserId, otherUserId);
        return NoContent();
    }

}
