namespace Week_45_Asset_Tracker.Models;

public sealed record Office(string Name, string CurrencyCode, decimal ExchangeRate)
{
    public decimal ToUsd(decimal localAmount) => localAmount * GetValidRate();

    public decimal FromUsd(decimal usdAmount) => usdAmount / GetValidRate();

    private decimal GetValidRate()
    {
        if (ExchangeRate <= 0)
            throw new InvalidOperationException(
                $"ERROR: The exchange rate for '{CurrencyCode}' must be greater than zero.");

        return ExchangeRate;
    }
}

public static class OfficeCatalog
{
    public static IReadOnlyList<Office> All { get; } =
    [
        new("Sweden", "SEK", 0.095m),
        new("Germany", "EUR", 1.08m),
        new("USA", "USD", 1.00m),
        new("Turkey", "TRY", 0.02m)
    ];

    public static Office? Find(string name) =>
        All.FirstOrDefault(office =>
            string.Equals(office.Name, name, StringComparison.OrdinalIgnoreCase));
}