namespace Week_45_Asset_Tracker.Models;

public static class Offices
{
    public const string Sweden = "Sweden";
    public const string Germany = "Germany";
    public const string Usa = "USA";

    public static readonly string[] All =
    [
        Sweden,
        Germany,
        Usa,
    ];
}

public abstract class Office(string brand, string currencyToken, decimal exchangeRate)
{
    public string location { get; } = brand;
    public string currencyToken { get; } = currencyToken;
    public decimal exchangeRate { get; } = exchangeRate;
}

