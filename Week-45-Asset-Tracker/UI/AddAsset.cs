namespace Week_45_Asset_Tracker.UI;

public static class AddAsset
{
    public static void Run()
    {
        // Entry loop for asset data
        while (true)
        {
            // Reset console
            Console.Clear();
            // Render heading
            Console.WriteLine("===== Add Asset =====");
            // Prompt user for data

            string? assetType = PromptRequired("Asset Type");

            string? brand = PromptRequired("Brand");

            string? model  = PromptRequired("Model");

            string? office  = PromptRequired("Office");

            string? price  = PromptRequired("Price");

            string? purchaseDate = PromptRequired("Purchase Date");

            // Render error messages if invalid inputs
            // Success *asset* added: Add new asset? (y/n)
            //y = continue
            //n = break
            Console.ReadLine();
        }
    }

    private static string NormalizeInputString(string input)
    {
        return input.Trim().ToLowerInvariant();
    }

    private static string PromptRequired(string label)
    {
        while (true)
        {
            Console.WriteLine($"{label}: ");
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
}