namespace DigitalBanking.Core.Dtos;

public class UserProfileDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int AccountId { get; set; }
    public string? PhotoUrl { get; set; }

}
