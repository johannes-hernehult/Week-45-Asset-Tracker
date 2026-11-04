using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.UI;

Console.WriteLine("======= Asset Tracker =======");

var mainMenu = new Menu("Main menu")
    .Add("Add Asset", AddAsset.Run)
    .Add("View Asset", () => Console.WriteLine("Viewing assets..."))
    .Add("Sort Asset", () => Console.WriteLine("Sorting asset..."))
    .Add("Search Asset", () => Console.WriteLine("Searching asset..."));
mainMenu.Show();

