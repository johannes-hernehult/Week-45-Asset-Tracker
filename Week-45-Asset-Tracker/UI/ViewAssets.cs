using System.Globalization;
using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;

namespace Week_45_Asset_Tracker.UI;

public static class ViewAssets
{
    public static void Run(AssetService assetService)
    {
        char sortBy = 'a';
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

            IOrderedEnumerable<Asset> sortedAssets = SortAssets(assets, sortBy);

            Console.WriteLine($"Current sorting: {GetSortDescription(sortBy)}");
            RenderTable(sortedAssets);

            if (statusMessage is not null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(statusMessage);
                Console.ResetColor();
            }

            Console.WriteLine(
                "Sort by: [a] Asset Type  [d] Purchase Date  [o] Office  [b] Brand  [p] Price");
            Console.Write("Enter a choice and press Enter, or [q] to go back: ");

            string? choice = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (choice)
            {
                case "q":
                    return;
                case "a":
                    sortBy = 'a';
                    statusMessage = null;
                    break;
                case "d":
                    sortBy = 'd';
                    statusMessage = null;
                    break;
                case "o":
                    sortBy = 'o';
                    statusMessage = null;
                    break;
                case "b":
                    sortBy = 'b';
                    statusMessage = null;
                    break;
                case "p":
                    sortBy = 'p';
                    statusMessage = null;
                    break;
                default:
                    statusMessage = "Invalid choice. Choose a, d, o, b, p, or q.";
                    break;
            }
        }
    }

    private static void RenderTable(IEnumerable<Asset> assets)
    {
        int tableWidth = 106;
        string seperator = " | ";


        RenderHeader(tableWidth, seperator);
        RenderRows(seperator, assets);
        Console.WriteLine(new string('-', tableWidth));
    }

    private static void RenderHeader(int width, string separator)
    {
        string officeHeader = "Office".PadRight(8);
        string typeHeader = "Type".PadRight(12);
        string brandHeader = "Brand".PadRight(12);
        string modelHeader = "Model".PadRight(16);
        string priceHeader = "Price(USD)".PadLeft(12);
        string purchaseDateHeader = "Purchase At".PadRight(12);
        string statusHeader = "Status".PadRight(10);

        Console.WriteLine(new string('-', width));
        Console.WriteLine(string.Join(separator, officeHeader, typeHeader, brandHeader, modelHeader,
            priceHeader, purchaseDateHeader, statusHeader));
        Console.WriteLine(new string('-', width));
    }


    private static void RenderRows(string separator, IEnumerable<Asset> assets)
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

            string office = $"{asset.Office}".PadRight(8);
            string type = $"{asset.AssetType}".PadRight(12);
            string brand = $"{asset.Brand}".PadRight(12);
            string model = (asset.Model.Length > 16
                ? asset.Model[..13] + "..." // Truncate long Model names
                : asset.Model).PadRight(16);
            string price = $"{asset.PriceUsd:F2}".PadLeft(12);
            string purchaseDate = asset.PurchaseDate
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                .PadRight(12);
            var assetStatus = GetStatus(asset);
            string status = assetStatus.Label.PadRight(10);

            if (assetStatus.Color is { } color)
                Console.ForegroundColor = color;

            if (useGrayBackground)
                Console.Write(subtleGrayBackground);

            Console.WriteLine(string.Join(separator, office, type, brand, model, price, purchaseDate, status));
            Console.Write(resetAnsiColors);
            Console.ResetColor();
            rowIndex++;
        }
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
            'd' => "Purchase Date (oldest first)",
            'o' => "Office (A-Z)",
            'b' => "Brand (A-Z)",
            'p' => "Price (lowest first)",
            _ => "Asset Type (A-Z)"
        };
    }

    private static IOrderedEnumerable<Asset> SortAssets(List<Asset> assets, char sortBy)
    {
        return sortBy switch
        {
            'd' => assets
                .OrderBy(asset => asset.PurchaseDate)
                .ThenBy(asset => asset.Id),
            'o' => assets
                .OrderBy(asset => asset.Office, StringComparer.OrdinalIgnoreCase)
                .ThenBy(asset => asset.Id),
            'b' => assets
                .OrderBy(asset => asset.Brand, StringComparer.OrdinalIgnoreCase)
                .ThenBy(asset => asset.Id),
            'p' => assets
                .OrderBy(asset => asset.PriceUsd)
                .ThenBy(asset => asset.Id),
            _ => assets
                .OrderBy(asset => asset.AssetType, StringComparer.OrdinalIgnoreCase)
                .ThenBy(asset => asset.Id)
        };
    }
}