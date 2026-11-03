using System.Runtime.InteropServices;

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
        return this; // allows chaining
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {_title} ===");
            for (int i = 0; i < _items.Count; i++)
                Console.WriteLine($"{i + 1}. {_items[i].Label}");
            Console.WriteLine("0. Back/Exit");

            Console.Write("> ");
            if (!int.TryParse(Console.ReadLine(), out int choice)
                || choice < 0 || choice > _items.Count)
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            if (choice == 0) return;
            _items[choice - 1].Execute();
        }
    }
}
