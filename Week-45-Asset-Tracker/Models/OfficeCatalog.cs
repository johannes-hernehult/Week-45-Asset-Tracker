namespace Week_45_Asset_Tracker.Models;

public sealed record Office(string Name, string CurrencyCode, decimal ExchangeRate);

public static class OfficeCatalog
{
    public static IReadOnlyList<Office> All { get; } =
    [
        new("Sweden", "SEK", 0.095m),
        new("Germany", "EUR", 1.08m),
        new("USA", "USD", 1.00m),
        new("Turkey", "TRY", 0.02m)
    ];
}