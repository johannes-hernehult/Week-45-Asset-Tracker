namespace Week_45_Asset_Tracker.UI;

public static class ConsoleUi
{
    public static void Write(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    public static void WriteLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void Error(string message) => WriteLine(message, ConsoleColor.DarkRed);

    public static void Success(string message) => WriteLine(message, ConsoleColor.DarkGreen);

    public static void Info(string message) => WriteLine(message, ConsoleColor.Blue);

    public static void Prompt(string text) => Write(text, ConsoleColor.DarkGray);

    public static void Hint(string text) => WriteLine(text, ConsoleColor.DarkGray);

    public static bool Confirm(string question)
    {
        while (true)
        {
            Prompt($"{question} [y/n] ");
            char key = char.ToLowerInvariant(Console.ReadKey(intercept: true).KeyChar);
            Console.WriteLine();

            if (key == 'y')
                return true;

            if (key == 'n')
                return false;

            Console.WriteLine("Please press Y or N.");
        }
    }
}