using System.Globalization;
using Week_45_Asset_Tracker.Models;

namespace Week_45_Asset_Tracker.UI;

public static class AssetTable
{
    private const int TableWidth = 117;
    private const string Separator = " | ";

    public static void Render(IEnumerable<Asset> assets, bool showLocalCurrency)
    {
        RenderHeader();
        RenderRows(assets, showLocalCurrency);
        Console.WriteLine(new string('-', TableWidth));
    }

    private static void RenderHeader()
    {
        string idHeader = "ID".PadRight(5);
        string officeHeader = "Office".PadRight(8);
        string typeHeader = "Type".PadRight(12);
        string brandHeader = "Brand".PadRight(12);
        string modelHeader = "Model".PadRight(16);
        string priceHeader = "Price".PadLeft(16);
        string purchaseDateHeader = "Purchase At".PadRight(12);
        string ageHeader = "Age".PadRight(5);
        string statusHeader = "Status".PadRight(7);

        Console.WriteLine(new string('-', TableWidth));
        Console.WriteLine(string.Join(
            Separator,
            idHeader,
            officeHeader,
            typeHeader,
            brandHeader,
            modelHeader,
            priceHeader,
            purchaseDateHeader,
            ageHeader,
            statusHeader));
        Console.WriteLine(new string('-', TableWidth));
    }

    private static void RenderRows(IEnumerable<Asset> assets, bool showLocalCurrency)
    {
        const string subtleGrayBackground = "\u001b[48;2;48;48;48m";
        const string resetAnsiColors = "\u001b[0m";
        int rowIndex = 0;

        foreach (var asset in assets)
        {
            Console.ResetColor();
            bool useGrayBackground = rowIndex % 2 == 0;

            if (useGrayBackground)
                Console.Write(subtleGrayBackground);

            string id = asset.Id.ToString().PadRight(5);
            string office = asset.Office.PadRight(8);
            string type = asset.AssetType.ToDisplayString().PadRight(12);
            string brand = asset.Brand.PadRight(12);
            string model = (asset.Model.Length > 16
                ? asset.Model[..13] + "..."
                : asset.Model).PadRight(16);
            string price = GetPriceDisplay(asset, showLocalCurrency).PadLeft(16);
            string purchaseDate = asset.PurchaseDate
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                .PadRight(12);
            string age = FormatAge(asset.AgeInMonths).PadRight(5);
            var assetStatus = GetStatus(asset);
            string status = assetStatus.Label.PadRight(7);

            if (assetStatus.Color is { } color)
                Console.ForegroundColor = color;

            if (useGrayBackground)
                Console.Write(subtleGrayBackground);

            Console.WriteLine(string.Join(
                Separator,
                id,
                office,
                type,
                brand,
                model,
                price,
                purchaseDate,
                age,
                status));
            Console.Write(resetAnsiColors);
            Console.ResetColor();
            rowIndex++;
        }
    }

    private static string GetPriceDisplay(Asset asset, bool showLocalCurrency)
    {
        if (!showLocalCurrency)
        {
            return $"{asset.PriceUsd.ToString("N2", CultureInfo.InvariantCulture)} USD";
        }

        Office office = OfficeCatalog.Find(asset.Office)
                        ?? throw new InvalidOperationException(
                            $"ERROR: No office configuration found for '{asset.Office}'.");

        decimal localPrice = office.FromUsd(asset.PriceUsd);

        return $"{localPrice.ToString("N2", CultureInfo.InvariantCulture)} {office.CurrencyCode}";
    }

    private static (string Label, ConsoleColor? Color) GetStatus(Asset asset)
    {
        switch (asset.Status)
        {
            case AssetStatus.Red:
                return ("RED", ConsoleColor.Red);
            case AssetStatus.Yellow:
                return ("YELLOW", ConsoleColor.Yellow);
            default:
                return ("", null);
        }
    }

    private static string FormatAge(int totalMonths)
    {
        int months = Math.Max(0, totalMonths);
        return $"{months / 12}y{months % 12}m";
    }
}