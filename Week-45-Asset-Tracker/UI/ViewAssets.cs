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
        int tableWidth = 104;
        string seperator = " | ";


        RenderHeader(tableWidth, seperator);

        foreach (var asset in assets)
        {
            (string Label, ConsoleColor? Color) status = GetStatus(asset);

            if (status.Color is { } color) Console.ForegroundColor = color;


            Console.ResetColor();
        }
    }

    private static void RenderHeader(int width, string separator)
    {
        string officeHeader = "Office".PadRight(10);
        string typeHeader = "Type".PadRight(12);
        string brandHeader = "Brand".PadRight(12);
        string modelHeader = "Model".PadRight(14);
        string priceHeader = "Price".PadLeft(12);
        string purchaseDateHeader = "Purchase At".PadRight(10);
        string statusHeader = "Status".PadRight(15);

        Console.WriteLine(new string('-', width));
        Console.WriteLine(string.Join(separator, officeHeader, typeHeader, brandHeader, modelHeader,
            priceHeader, purchaseDateHeader, statusHeader));
        Console.WriteLine(new string('-', width));
    }
    
    

    private static (string Label, ConsoleColor? Color) GetStatus(Asset asset)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        DateOnly expiryDate = asset.PurchaseDate.AddYears(3);

        if (today > expiryDate)
            return ("Expired", ConsoleColor.Red);

        if (today >= expiryDate.AddMonths(-3))
            return ("< 3 Months left", ConsoleColor.DarkYellow);

        if (today >= expiryDate.AddMonths(-6))
            return ("< 6 months left", ConsoleColor.Yellow);

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