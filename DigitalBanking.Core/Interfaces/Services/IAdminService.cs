using DigitalBanking.Core.Dtos;

namespace DigitalBanking.Core.Interfaces.Services;

public interface IAdminService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<List<TransactionsInfoDto>> GetAllTransactionsAsync();
}
