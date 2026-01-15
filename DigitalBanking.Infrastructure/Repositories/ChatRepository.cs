using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Interfaces.Repository;
using DigitalBanking.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace DigitalBanking.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly DigitalBankingDbContext _context;

    public ChatRepository(DigitalBankingDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ChatMessage>> GetConversationAsync(string user1, string user2)
    {
        return await _context.ChatMessages
            .Where(m =>
                (m.FromUserId == user1 && m.ToUserId == user2) ||
                (m.FromUserId == user2 && m.ToUserId == user1))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteConversationAsync(string user1, string user2)
    {
        var messages = await _context.ChatMessages
            .Where(m =>
                (m.FromUserId == user1 && m.ToUserId == user2) ||
                (m.FromUserId == user2 && m.ToUserId == user1))
            .ToListAsync();

        _context.ChatMessages.RemoveRange(messages);
        await _context.SaveChangesAsync();
    }

}
