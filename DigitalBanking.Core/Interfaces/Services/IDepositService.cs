using Stripe;

namespace DigitalBanking.Core.Interfaces.Services;

public interface IDepositService
{
    Task<string> CreateDepositIntentAsync(string userId, decimal amount);
    Task HandlePaymentSucceededAsync(PaymentIntent intent);
}
