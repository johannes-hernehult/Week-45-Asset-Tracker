using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;

namespace Week_45_Asset_Tracker.UI;

public static class RemoveAsset
{
    public static void Run(AssetService assetService)
    {
        (string Text, ConsoleColor Color)? message = null;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== Remove Asset =====");

            IReadOnlyList<Asset> assets = assetService.GetAssets();

            if (assets.Count == 0)
            {
                Console.WriteLine("No assets...");
                Console.ReadLine();
                return;
            }

            ViewAssets.RenderTable(
                assets.OrderBy(asset => asset.AssetType).ThenBy(asset => asset.PurchaseDate),
                showLocalCurrency: false);

            if (message is { } current)
            {
                Console.ForegroundColor = current.Color;
                Console.WriteLine(current.Text);
                Console.ResetColor();
                message = null;
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Enter the ID of the asset to remove or [q] to go back: ");
            Console.ResetColor();

            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (!int.TryParse(input, out int id))
            {
                message = ("ERROR: Enter a numeric ID.", ConsoleColor.DarkRed);
                continue;
            }

            Asset? asset = assets.FirstOrDefault(a => a.Id == id);

            if (asset is null)
            {
                message = ($"ERROR: No asset with ID {id}.", ConsoleColor.DarkRed);
                continue;
            }

            if (!Confirm($"Remove {asset.Brand} {asset.Model} (ID {asset.Id})? [y/n] "))
            {
                message = ("INFO: Removal cancelled.", ConsoleColor.Blue);
                continue;
            }

            assetService.RemoveAsset(id);
            message = ($"SUCCESS: Asset {id} removed.", ConsoleColor.DarkGreen);
        }
    }

    private static bool Confirm(string prompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(prompt);
            Console.ResetColor();
            char key = char.ToLowerInvariant(Console.ReadKey(intercept: true).KeyChar);
            Console.WriteLine();

            if (key == 'y')
                return true;

            if (key == 'n')
                return false;
        }
    }
}