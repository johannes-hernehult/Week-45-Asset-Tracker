namespace Week_45_Asset_Tracker.UI;

public record MenuItem(string Label, Action Execute);

public class Menu
{
    private readonly string _title;
    private readonly List<MenuItem> _items = new();

    public Menu(string title) => _title = title;

    public Menu Add(string label, Action action)
    {
        _items.Add(new MenuItem(label, action));
        return this;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {_title} ===");

            for (int i = 0; i < _items.Count; i++)
                Console.WriteLine($"{i + 1}. {_items[i].Label}");

            Console.WriteLine("0. Exit");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Press a number to select: ");
            Console.ResetColor();

            char key = Console.ReadKey(intercept: true).KeyChar;
            Console.WriteLine();

            if (key == '0')
            {
                if (ConfirmExit())
                    return;

                continue;
            }

            int choice = key - '0';

            if (choice < 1 || choice > _items.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Invalid choice. Press any key to continue.");
                Console.ReadKey(intercept: true);
                Console.ResetColor();
                continue;
            }

            _items[choice - 1].Execute();
        }
    }

    private static bool ConfirmExit()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Are you sure you want to exit? [y/n] ");
            Console.ResetColor();
            char confirmation = char.ToLowerInvariant(
                Console.ReadKey(intercept: true).KeyChar);
            Console.WriteLine();

            if (confirmation == 'y')
                return true;

            if (confirmation == 'n')
                return false;

            Console.WriteLine("Please press Y or N.");
        }
    }
}