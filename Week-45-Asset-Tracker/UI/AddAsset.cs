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
                var priceUsd = office.ToUsd(priceLocal);
                var purchaseDate = PromptForValidDateOnly("Purchase Date (yyyy-MM-dd)");

                switch (assetType)
                {
                    case AssetType.Computer:
                        assetService.AddAsset(new Computer(brand, model, purchaseDate, priceUsd, office.Name));
                        break;
                    case AssetType.MobilePhone:
                        assetService.AddAsset(new MobilePhone(brand, model, purchaseDate, priceUsd, office.Name));
                        break;
                    case AssetType.Tablet:
                        assetService.AddAsset(new Tablet(brand, model, purchaseDate, priceUsd, office.Name));
                        break;
                }

                ConsoleUi.Success("SUCCESS: Asset added!");

                if (!ConsoleUi.Confirm("Add a new asset?"))
                {
                    break;
                }
            }
        }
        catch (AddAssetCancelledException)
        {
            ConsoleUi.Info("INFO: Adding asset cancelled. Press 'Enter' to continue...");
            Console.ReadLine();
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
            ConsoleUi.Write($"{prompt}: ", ConsoleColor.Gray);
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

                ConsoleUi.Error("ERROR: Input can not be empty, try again!");
                continue;
            }

            return NormalizeInputString(input);
        }
    }

    private static AssetType PromptForValidAssetType(string prompt)
    {
        var assetTypes = Enum.GetValues<AssetType>();

        while (true)
        {
            Console.Write("Available asset types: ");
            ConsoleUi.WriteLine(
                string.Join(", ", assetTypes.Select(type => type.ToDisplayString())),
                ConsoleColor.DarkBlue);

            string input = PromptRequired(prompt);

            foreach (var type in assetTypes)
            {
                if (string.Equals(type.ToDisplayString(), input, StringComparison.OrdinalIgnoreCase))
                {
                    return type;
                }
            }

            ConsoleUi.Error($"ERROR: Asset type {input} not supported, try again.");
        }
    }

    private static Office PromptForValidOffice(string prompt)
    {
        while (true)
        {
            Console.Write("Available offices: ");
            ConsoleUi.WriteLine(
                string.Join(", ", OfficeCatalog.All.Select(o => o.Name)),
                ConsoleColor.DarkBlue);

            string input = PromptRequired(prompt);
            var selectedOffice = OfficeCatalog.Find(input);

            if (selectedOffice is null)
            {
                ConsoleUi.Error($"ERROR: Office {input} not available, try again.");
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
                ConsoleUi.Error("ERROR: Only numbers allowed, try again.");
                continue;
            }

            if (price <= 0)
            {
                ConsoleUi.Error("ERROR: Only positive values allowed, try again.");
                continue;
            }

            return price;
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
                ConsoleUi.Error("ERROR: Enter a valid date in YYYY-MM-DD format, e.g. 2026-11-05.");
                continue;
            }

            if (date > DateOnly.FromDateTime(DateTime.Today))
            {
                ConsoleUi.Error("ERROR: Purchase date can not be in the future, try again.");
                continue;
            }

            return date;
        }
    }
}