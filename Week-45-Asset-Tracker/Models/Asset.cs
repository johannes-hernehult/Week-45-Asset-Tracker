namespace Week_45_Asset_Tracker.Models;

public enum AssetType
{
    Computer,
    MobilePhone,
    Tablet
}

public static class AssetTypeExtensions
{
    public static string ToDisplayString(this AssetType type) => type switch
    {
        AssetType.Computer => "Computer",
        AssetType.MobilePhone => "Mobile Phone",
        AssetType.Tablet => "Tablet",
        _ => type.ToString()
    };
}

public enum AssetStatus
{
    Ok,
    Yellow, // less than 6 months left
    Red // less than 3 months left
}

// Pass an existing id when restoring an asset (e.g. loading from file).
// Leave it out to get a newly generated unique id.
public abstract class Asset(
    string brand,
    string model,
    DateOnly purchaseDate,
    decimal priceUsd,
    string office,
    int? id = null)
{
    public const int LifespanYears = 3;

    private static readonly HashSet<int> UsedIds = [];

    public int Id { get; } = id.HasValue ? ReserveId(id.Value) : GenerateUniqueId();
    public string Brand { get; } = brand;
    public string Model { get; } = model;
    public DateOnly PurchaseDate { get; } = purchaseDate;
    public decimal PriceUsd { get; } = priceUsd;
    public string Office { get; } = office;
    public abstract AssetType AssetType { get; }

    public static bool CheckCollision(int id) => UsedIds.Contains(id);

    public DateOnly ExpiryDate => PurchaseDate.AddYears(LifespanYears);

    public AssetStatus Status
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (today >= ExpiryDate.AddMonths(-3))
                return AssetStatus.Red;

            if (today >= ExpiryDate.AddMonths(-6))
                return AssetStatus.Yellow;

            return AssetStatus.Ok;
        }
    }

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

    private static int ReserveId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Asset ID must be positive.");

        if (!UsedIds.Add(id))
            throw new InvalidOperationException($"ERROR: Asset ID {id} is already in use.");

        return id;
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

public sealed class Computer(
    string brand,
    string model,
    DateOnly purchaseDate,
    decimal priceUsd,
    string office,
    int? id = null)
    : Asset(brand, model, purchaseDate, priceUsd, office, id)
{
    public override AssetType AssetType => AssetType.Computer;
}

public sealed class MobilePhone(
    string brand,
    string model,
    DateOnly purchaseDate,
    decimal priceUsd,
    string office,
    int? id = null)
    : Asset(brand, model, purchaseDate, priceUsd, office, id)
{
    public override AssetType AssetType => AssetType.MobilePhone;
}

public sealed class Tablet(
    string brand,
    string model,
    DateOnly purchaseDate,
    decimal priceUsd,
    string office,
    int? id = null)
    : Asset(brand, model, purchaseDate, priceUsd, office, id)
{
    public override AssetType AssetType => AssetType.Tablet;
}