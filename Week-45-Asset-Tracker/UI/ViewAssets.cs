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

            IReadOnlyList<Asset> assets = assetService.GetAssets();

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
            AssetTable.Render(sortedAssets, showLocalCurrency);

            if (statusMessage is not null)
            {
                ConsoleUi.Error(statusMessage);
            }

            Console.WriteLine(
                "Sort by: [a] Type  [d] Date  [o] Office  [b] Brand  [p] Price  (press again to reverse)");
            Console.WriteLine("Toggle price currency: [c]");
            ConsoleUi.Prompt("Enter a choice or [q] to go back: ");

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

    private static IEnumerable<Asset> SortAssets(IEnumerable<Asset> assets, char sortBy, bool descending)
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
                .OrderBy(asset => asset.AssetType)
                .ThenBy(asset => asset.PurchaseDate)
        };

        return descending ? sorted.Reverse() : sorted;
    }
}