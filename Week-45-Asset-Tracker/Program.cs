using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.UI;

Console.WriteLine("======= Asset Tracker =======");
//
// Computer comp1 = new Computer("MSI", "Leopard 6GE", new DateOnly(), 1500, "Sweden");
// MobilePhone phone1 = new MobilePhone("Iphone", "14", new DateOnly(), 500, "Sweden");
//
// Console.WriteLine(comp1.AssetType);
// Console.WriteLine(phone1.AssetType);

var mainMenu = new Menu("Main menu")
    .Add("Add Asset", () => Console.WriteLine("Adding asset..."))
    .Add("View Asset", () => Console.WriteLine("Viewing assets..."))
    .Add("Sort Asset", () => Console.WriteLine("Sorting asset..."))
    .Add("Search Asset", () => Console.WriteLine("Searching asset..."));
mainMenu.Show();