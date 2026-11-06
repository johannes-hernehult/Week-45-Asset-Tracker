using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;

namespace Week_45_Asset_Tracker.UI;

public static class SearchAssets
{
    public static void Run(AssetService assetService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== Search Assets =====");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Search brand or model (empty or 'q' to go back): ");
            Console.ResetColor();

            string term = Console.ReadLine()?.Trim() ?? string.Empty;

            if (term.Length == 0 || string.Equals(term, "q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            List<Asset> results = assetService.SearchAssets(term);
            Console.WriteLine();

            if (results.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"No assets found for '{term}'.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{results.Count} result(s) for '{term}':");
                ViewAssets.RenderTable(
                    results.OrderBy(asset => asset.AssetType).ThenBy(asset => asset.PurchaseDate),
                    showLocalCurrency: false);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to search again...");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
        }
    }
}