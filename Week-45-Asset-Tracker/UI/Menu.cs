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
            ConsoleUi.Prompt("Press a number to select: ");

            char key = Console.ReadKey(intercept: true).KeyChar;
            Console.WriteLine();

            if (key == '0')
            {
                if (ConsoleUi.Confirm("Are you sure you want to exit?"))
                    return;

                continue;
            }

            int choice = key - '0';

            if (choice < 1 || choice > _items.Count)
            {
                ConsoleUi.Error("ERROR: Invalid choice. Press any key to continue.");
                Console.ReadKey(intercept: true);
                continue;
            }

            _items[choice - 1].Execute();
        }
    }
}