using System.Globalization;
using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;

namespace Week_45_Asset_Tracker.UI;

public static class AddAsset
{
    public static void Run(AssetService assetService)
    {
        try
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Add Asset =====");
                Console.WriteLine("Enter 'q' at any time to cancel.");

                var assetType = PromptForValidAssetType("Asset Type");
                var brand = PromptRequired("Brand");
                var model = PromptRequired("Model");
                var office = PromptForValidOffice("Office");
                var priceLocal = PromptForValidDecimal("Price", office);
                var priceUsd = ConvertCurrencyToUsd(priceLocal, office);
                var purchaseDate = PromptForValidDateOnly("Purchase Date (yyyy-MM-dd)");

                switch (assetType)
                {
                    case AssetTypes.Computer:
                        assetService.AddAsset(new Computer(brand, model, purchaseDate, priceUsd, office.Name));
                        break;
                    case AssetTypes.MobilePhone:
                        assetService.AddAsset(new MobilePhone(brand, model, purchaseDate, priceUsd, office.Name));
                        break;
                    case AssetTypes.Tablet:
                        assetService.AddAsset(new Tablet(brand, model, purchaseDate, priceUsd, office.Name));
                        break;
                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("SUCCESS: Asset added!");
                Console.ResetColor();

                var input = PromptRequired("Add a new asset (y/n)?", true);
                if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }
        catch (AddAssetCancelledException)
        {
            Console.ResetColor();
            Console.WriteLine("\nAdding asset cancelled.");
        }
    }

    private sealed class AddAssetCancelledException : Exception
    {
    }


    private static string NormalizeInputString(string input) => input.Trim();

    private static string PromptRequired(string prompt, bool allowEmpty = false)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{prompt}: ");
            Console.ResetColor();
            string? input = Console.ReadLine();

            if (string.Equals(input?.Trim(), "q", StringComparison.OrdinalIgnoreCase))
            {
                throw new AddAssetCancelledException();
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                if (allowEmpty)
                {
                    return string.Empty;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Input can not be empty, try again!");
                Console.ResetColor();
                continue;
            }

            return NormalizeInputString(input);
        }
    }


    private static string PromptForValidAssetType(string prompt)
    {
        while (true)
        {
            Console.Write("Available asset types: ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"{string.Join(", ", AssetTypes.All)}");
            Console.ResetColor();

            string input = PromptRequired(prompt);
            string? matchingAssetType = AssetTypes.All.FirstOrDefault(type =>
                string.Equals(type, input, StringComparison.OrdinalIgnoreCase));

            if (matchingAssetType is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Asset type {input} not supported, try again.");
                Console.ResetColor();
                continue;
            }

            return matchingAssetType;
        }
    }

    private static Office PromptForValidOffice(string prompt)
    {
        while (true)
        {
            Console.Write("Available offices: ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(string.Join(", ", OfficeCatalog.All.Select(o => o.Name)));
            Console.ResetColor();

            string input = PromptRequired(prompt);
            var selectedOffice = OfficeCatalog.All.FirstOrDefault(o =>
                string.Equals(o.Name, input, StringComparison.OrdinalIgnoreCase));

            if (selectedOffice is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Office {input} not available, try again.");
                Console.ResetColor();
                continue;
            }

            return selectedOffice;
        }
    }

    private static decimal PromptForValidDecimal(string prompt, Office office)
    {
        while (true)
        {
            string input = PromptRequired($"{prompt} ({office.CurrencyCode})")
                .Replace(',', '.');

            if (!decimal.TryParse(input, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out decimal price))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Only numbers allowed, try again.");
                Console.ResetColor();
                continue;
            }

            if (price <= 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Only positive values allowed, try again.");
                Console.ResetColor();
                continue;
            }

            return price;
        }
    }

    private static decimal ConvertCurrencyToUsd(decimal localPrice, Office office)
    {
        return localPrice * office.exchangeRate;
    }


    private static DateOnly PromptForValidDateOnly(string prompt)
    {
        while (true)
        {
            string input = PromptRequired($"{prompt}");

            bool isValidDate = DateOnly.TryParseExact(
                input.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly date);

            if (!isValidDate)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a valid date in YYYY-MM-DD format, e.g. 2026-11-05.");
                Console.ResetColor();
                continue;
            }

            return date;
        }
    }
}