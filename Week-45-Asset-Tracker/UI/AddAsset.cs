namespace Week_45_Asset_Tracker.UI;

public static class AddAsset
{
    public static void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== Add Asset =====");

            var assetType = PromptRequired("Asset Type");
            //Check that asset type matches available types

            var brand = PromptRequired("Brand");
            var model = PromptRequired("Model");
            var office = PromptRequired("Office");
            //Check that office matches available offices

            var price = PromptRequired("Price");
            //Validate to decimal
            //Convert from local currency to USD

            var purchaseDate = PromptRequired("Purchase Date (yyyy-MM-dd)");
            //Validate to be DateOnly

            // Render error messages if invalid inputs
            // Success *asset* added: Add new asset? (y/n)
            //y = continue
            //n = break
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