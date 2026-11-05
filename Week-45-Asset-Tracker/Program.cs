using Week_45_Asset_Tracker.Services;
using Week_45_Asset_Tracker.UI;


AssetService assetService = new AssetService();

Console.WriteLine("======= Asset Tracker =======");

var mainMenu = new Menu("Main menu")
    .Add("Add Asset", () => AddAsset.Run(assetService))
    .Add("View Asset", () => Console.WriteLine("Viewing assets..."))
    .Add("Sort Asset", () => Console.WriteLine("Sorting asset..."))
    .Add("Search Asset", () => Console.WriteLine("Searching asset..."));
mainMenu.Show();

