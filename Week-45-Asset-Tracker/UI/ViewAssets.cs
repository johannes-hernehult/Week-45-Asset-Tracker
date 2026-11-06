using System.Globalization;
using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;

namespace Week_45_Asset_Tracker.UI;

public static class ViewAssets
{
    public static void Run(AssetService assetService)
    {
        char sortBy = 'a';
        bool descending = false;
        bool showLocalCurrency = false;
        string? statusMessage = null;

        while (true)
        {
            Console.Clear();

            List<Asset> assets = assetService.GetAssets();

            if (assets.Count == 0)
            {
                Console.WriteLine("No assets...");
                Console.ReadLine();
                break;
            }

            IEnumerable<Asset> sortedAssets = SortAssets(assets, sortBy, descending);

            Console.WriteLine(
                $"Current sorting: {GetSortDescription(sortBy)} ({(descending ? "descending" : "ascending")})");
            Console.WriteLine($"Price display: {(showLocalCurrency ? "Local currency" : "USD")}");
            RenderTable(sortedAssets, showLocalCurrency);

            if (statusMessage is not null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(statusMessage);
                Console.ResetColor();
            }

            Console.WriteLine(
                "Sort by: [a] Type  [d] Date  [o] Office  [b] Brand  [p] Price  (press again to reverse)");
            Console.WriteLine("Toggle price currency: [c]");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Enter a choice or [q] to go back: ");
            Console.ResetColor();

            char choice = char.ToLowerInvariant(Console.ReadKey(intercept: true).KeyChar);

            switch (choice)
            {
                case 'q':
                    return;
                case 'c':
                    showLocalCurrency = !showLocalCurrency;
                    statusMessage = null;
                    break;
                case 'a' or 'd' or 'o' or 'b' or 'p':
                    descending = choice == sortBy && !descending;
                    sortBy = choice;
                    statusMessage = null;
                    break;
                default:
                    statusMessage = "ERROR: Invalid choice. Choose a, d, o, b, p, c, or q.";
                    break;
            }
        }
    }

    private static void RenderTable(IEnumerable<Asset> assets, bool showLocalCurrency)
    {
        const int tableWidth = 118;
        const string separator = " | ";

        RenderHeader(tableWidth, separator);
        RenderRows(separator, assets, showLocalCurrency);
        Console.WriteLine(new string('-', tableWidth));
    }

    private static void RenderHeader(int width, string separator)
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

        Console.WriteLine(new string('-', width));
        Console.WriteLine(string.Join(
            separator,
            idHeader,
            officeHeader,
            typeHeader,
            brandHeader,
            modelHeader,
            priceHeader,
            purchaseDateHeader,
            ageHeader,
            statusHeader));
        Console.WriteLine(new string('-', width));
    }


    private static void RenderRows(
        string separator,
        IEnumerable<Asset> assets,
        bool showLocalCurrency)
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
            string type = asset.AssetType.PadRight(12);
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
                separator,
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

        Office? office = OfficeCatalog.All.FirstOrDefault(office =>
            string.Equals(office.Name, asset.Office, StringComparison.OrdinalIgnoreCase));

        if (office is null)
            throw new InvalidOperationException($"ERROR: No office configuration found for '{asset.Office}'.");

        if (office.exchangeRate <= 0)
            throw new InvalidOperationException(
                $"ERROR: The exchange rate for '{office.CurrencyCode}' must be greater than zero.");

        decimal localPrice = asset.PriceUsd / office.exchangeRate;

        return $"{localPrice.ToString("N2", CultureInfo.InvariantCulture)} {office.CurrencyCode}";
    }

    private static (string Label, ConsoleColor? Color) GetStatus(Asset asset)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        DateOnly expiryDate = asset.PurchaseDate.AddYears(3);

        if (today >= expiryDate.AddMonths(-3))
            return ("RED", ConsoleColor.Red);

        if (today >= expiryDate.AddMonths(-6))
            return ("YELLOW", ConsoleColor.Yellow);

        return ("", null);
    }

    private static string GetSortDescription(char sortBy)
    {
        return sortBy switch
        {
            'd' => "Purchase Date",
            'o' => "Office, then Purchase Date",
            'b' => "Brand, then Model",
            'p' => "Price",
            _ => "Asset Type, then Purchase Date"
        };
    }

    private static IEnumerable<Asset> SortAssets(List<Asset> assets, char sortBy, bool descending)
    {
        IOrderedEnumerable<Asset> sorted = sortBy switch
        {
            'd' => assets
                .OrderBy(asset => asset.PurchaseDate)
                .ThenBy(asset => asset.Id),
            'o' => assets
                .OrderBy(asset => asset.Office, StringComparer.OrdinalIgnoreCase)
                .ThenBy(asset => asset.PurchaseDate),
            'b' => assets
                .OrderBy(asset => asset.Brand, StringComparer.OrdinalIgnoreCase)
                .ThenBy(asset => asset.Model, StringComparer.OrdinalIgnoreCase),
            'p' => assets
                .OrderBy(asset => asset.PriceUsd)
                .ThenBy(asset => asset.Id),
            _ => assets
                .OrderBy(asset => asset.AssetType, StringComparer.OrdinalIgnoreCase)
                .ThenBy(asset => asset.PurchaseDate)
        };

        return descending ? sorted.Reverse() : sorted;
    }

    private static string FormatAge(int totalMonths)
    {
        int months = Math.Max(0, totalMonths);
        return $"{months / 12}y{months % 12}m";
    }
}