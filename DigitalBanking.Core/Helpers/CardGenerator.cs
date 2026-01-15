using System.Text;

namespace DigitalBanking.Core.Helpers;

public static class CardGenerator
{
    private static readonly Random _random = new Random();

    public static string GenerateCardNumber()
    {
        int[] digits = new int[16];

        for (int i = 0; i < 15; i++)
        {
            digits[i] = _random.Next(0, 10);
        }

        digits[15] = CalculateCheckDigit(digits);

        var sb = new StringBuilder();
        foreach (var d in digits)
            sb.Append(d);

        return sb.ToString();
    }

    private static int CalculateCheckDigit(int[] digits)
    {
        int sum = 0;

        for (int i = 14; i >= 0; i--)
        {
            int digit = digits[i];

            if ((14 - i) % 2 == 0)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
        }

        return (10 - (sum % 10)) % 10;
    }

    public static bool IsValid(string cardNumber)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            int n = cardNumber[i] - '0';

            if (alternate)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }

            sum += n;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }
}
