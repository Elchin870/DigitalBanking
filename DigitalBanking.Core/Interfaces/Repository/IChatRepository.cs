using DigitalBanking.Core.Entities;

namespace DigitalBanking.Core.Interfaces.Repository;

public interface IChatRepository
{
    Task SaveAsync(ChatMessage message);
    Task<List<ChatMessage>> GetConversationAsync(string user1, string user2);
    Task DeleteConversationAsync(string user1, string user2);

}
