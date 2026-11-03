namespace Week_45_Asset_Tracker.Models;

public abstract class Asset(string brand, string model, DateOnly purchaseDate, decimal priceUsd, string office)
{
    public int Id { get; } = Random.Shared.Next(10000, 100000);
    public string Brand { get; } = brand;
    public string Model { get; } = model;
    public DateOnly PurchaseDate { get; } = purchaseDate;
    public decimal PriceUsd { get; } = priceUsd;
    public string Office { get; } = office;
    public abstract string AssetType { get; }
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