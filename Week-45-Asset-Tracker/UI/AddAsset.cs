using System.Globalization;
using Week_45_Asset_Tracker.Models;

namespace Week_45_Asset_Tracker.UI;

public static class AddAsset
{
    public static void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== Add Asset =====");

            var assetType = PromptForValidAssetType("Asset Type");
            var brand = PromptRequired("Brand");
            var model = PromptRequired("Model");
            var office = PromptForValidOffice("Office");
            var priceLocal = PromptForValidDecimal("Price", office);
            var priceUsd = ConvertCurrencyToUsd(priceLocal, office);

            var purchaseDate = PromptForValidDateOnly("Purchase Date (yyyy-MM-dd)");
            //Validate to be DateOnly

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("SUCCESS: Asset added!");
            Console.ResetColor();

            var input = PromptRequired("Add a new asset (y/n)? ");
            if (input == "n") break;
        }
    }

    private static string NormalizeInputString(string input)
    {
        return input.Trim().ToLowerInvariant();
    }

    private static string PromptRequired(string prompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{prompt}: ");
            Console.ResetColor();
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
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
            bool isValid = AssetTypes.All.Contains(input, StringComparer.OrdinalIgnoreCase);
            if (!isValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Asset type {input} not supported, try again.");
                Console.ResetColor();
                continue;
            }

            return input;
        }
    }

    private static string PromptForValidOffice(string prompt)
    {
        while (true)
        {
            Console.Write("Available offices: ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(
                $"{string.Join(", ", OfficeCatalog.All.Select(office => office.Name))}");
            Console.ResetColor();

            string input = PromptRequired(prompt);
            var selectedOffice = OfficeCatalog.All.FirstOrDefault(office =>
                string.Equals(office.Name, input, StringComparison.OrdinalIgnoreCase));

            if (selectedOffice is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Office {input} not available, try again.");
                Console.ResetColor();
                continue;
            }

            return input;
        }
    }

    private static decimal PromptForValidDecimal(string prompt, string officeName)
    {
        while (true)
        {
            var selectedOffice = OfficeCatalog.All.FirstOrDefault(office =>
                string.Equals(office.Name, officeName, StringComparison.OrdinalIgnoreCase));

            if (selectedOffice is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Office not found, try again.");
                Console.ResetColor();
                continue;
            }

            string? currencyCode = selectedOffice.CurrencyCode;
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Currency code is not available, try again.");
                Console.ResetColor();
                continue;
            }

            string input = PromptRequired(@$"{prompt} ({currencyCode})");
            input = input.Replace(',', '.');
            bool isDecimal = decimal.TryParse(input, out decimal price);
            if (!isDecimal)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Only numbers allowed, try again.");
                Console.ResetColor();
                continue;
            }

            if (price < 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Only positive values allowed, try again.");
                Console.ResetColor();
                continue;
            }

            return price;
        }
    }

    private static decimal ConvertCurrencyToUsd(decimal localPrice, string officeName)
    {
        while (true)
        {
            var selectedOffice = OfficeCatalog.All.FirstOrDefault(office =>
                string.Equals(office.Name, officeName, StringComparison.OrdinalIgnoreCase));

            if (selectedOffice is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Office not found, try again.");
                Console.ResetColor();
                continue;
            }

            decimal exchangeRate = selectedOffice.exchangeRate;
            decimal priceUsd = exchangeRate * localPrice;

            Console.WriteLine(priceUsd);
            Console.WriteLine(localPrice);

            return priceUsd;
        }
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
            }

            return date;
        }
    }
}