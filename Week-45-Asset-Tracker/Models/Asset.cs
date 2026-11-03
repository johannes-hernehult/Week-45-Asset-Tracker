namespace Week_45_Asset_Tracker.Models;

public abstract class Asset
{
    public string Brand { get; }
    public string Model { get; }
    public DateOnly PurchaseDate { get; }
    public decimal PriceUSD { get; }
    public string Office { get; }
    public abstract string AssetType { get; }

    protected Asset(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    {
        Brand = brand;
        Model = model;
        PurchaseDate = purchaseDate;
        PriceUSD = priceUsd;
        Office = office;
    }
}

public class Computer(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    : Asset(brand, model, purchaseDate, priceUsd, office)
{
    public override string AssetType => "Computer";
}

public class MobilePhone(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    : Asset(brand, model, purchaseDate, priceUsd, office)
{
    public override string AssetType => "Mobile Phone";
}

public class Tablet(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
    : Asset(brand, model, purchaseDate, priceUsd, office)
{
    public override string AssetType => "Tablet";
}