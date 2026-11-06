namespace Week_45_Asset_Tracker.Models;

public static class AssetTypes
{
    public const string Computer = "Computer";
    public const string MobilePhone = "Mobile Phone";
    public const string Tablet = "Tablet";

    public static IReadOnlyList<string> All { get; } = Array.AsReadOnly(
    [
        Computer,
        MobilePhone,
        Tablet
    ]);
}

public abstract class Asset(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
{
    private static readonly HashSet<int> UsedIds = [];

    public int Id { get; } = GenerateUniqueId();
    public string Brand { get; } = brand;
    public string Model { get; } = model;
    public DateOnly PurchaseDate { get; } = purchaseDate;
    public decimal PriceUsd { get; } = priceUsd;
    public string Office { get; } = office;
    public abstract string AssetType { get; }

    public static bool CheckCollision(int id) => UsedIds.Contains(id);

    public int AgeInMonths
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int months = (today.Year - PurchaseDate.Year) * 12 + today.Month - PurchaseDate.Month;

            if (today.Day < PurchaseDate.Day)
            {
                months--;
            }

            return months;
        }
    }

    private static int GenerateUniqueId()
    {
        if (UsedIds.Count >= 90000)
            throw new InvalidOperationException("ERROR: No free asset IDs left.");

        int id;
        do
        {
            id = Random.Shared.Next(10000, 100000);
        } while (CheckCollision(id));

        UsedIds.Add(id);
        return id;
    }
}

public sealed class Computer(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    : Asset(brand, model, purchaseDate, priceUsd, office)
{
    public override string AssetType => AssetTypes.Computer;
}

public sealed class MobilePhone(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    : Asset(brand, model, purchaseDate, priceUsd, office)
{
    public override string AssetType => AssetTypes.MobilePhone;
}

public sealed class Tablet(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    : Asset(brand, model, purchaseDate, priceUsd, office)
{
    public override string AssetType => AssetTypes.Tablet;
}