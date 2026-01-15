using DigitalBanking.Core.Dtos;

namespace DigitalBanking.Core.Interfaces.Services;

public interface IUserService
{
    Task<decimal> GetBalance(string userId);
    Task UpdateProdileAsync(string userId, UpdateProfileDto dto);
    Task UpdateProfileAvatarAsync(string userId, string avatar);
    Task<List<CardDto>> GetMyCardsAsync(string userId);
    Task<UserProfileDto?> GetProfileInfosAsync(string userId);
    Task<List<TransactionDto>> GetTransactionInfoAsync(string userId);

}
