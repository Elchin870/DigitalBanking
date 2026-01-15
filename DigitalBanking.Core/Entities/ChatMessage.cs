namespace DigitalBanking.Core.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }
    public string FromUserId { get; set; }
    public string ToUserId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}
