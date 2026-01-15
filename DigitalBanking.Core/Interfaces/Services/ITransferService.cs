namespace DigitalBanking.Core.Interfaces.Services;

public interface ITransferService
{
    Task<(bool IsSuccess, string Message)> TransferAsync(string senderUserId, int fromCardId, string toCardNumber, decimal amount);
}
