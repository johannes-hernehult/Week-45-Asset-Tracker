using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;
using Week_45_Asset_Tracker.UI;


AssetService assetService = new AssetService();
SeedTestData(assetService);

Console.WriteLine("======= Asset Tracker =======");

var mainMenu = new Menu("Main menu")
    .Add("Add Asset", () => AddAsset.Run(assetService))
    .Add("View Asset", () => ViewAssets.Run(assetService))
    .Add("Sort Asset", () => Console.WriteLine("Sorting asset..."))
    .Add("Search Asset", () => Console.WriteLine("Searching asset..."));
mainMenu.Show();

static void SeedTestData(AssetService assetService)
{
    var today = DateOnly.FromDateTime(DateTime.Today);

    assetService.AddAsset(new Computer("Lenovo", "ThinkPad X1", today.AddMonths(-5), 1450m, "Sweden"));
    assetService.AddAsset(new MobilePhone("Apple", "iPhone 15", today.AddMonths(-18), 999m, "USA"));
    assetService.AddAsset(new Tablet("Samsung", "Galaxy Tab S9", today.AddMonths(-30), 780m, "Germany"));
    assetService.AddAsset(new Computer("Apple", "MacBook Pro", today.AddMonths(-12), 2400m, "USA"));
    assetService.AddAsset(new MobilePhone("Google", "Pixel 8", today.AddMonths(-2), 699m, "Germany"));
    assetService.AddAsset(new Tablet("Apple", "iPad Air", today.AddMonths(-40), 599m, "Sweden"));
    assetService.AddAsset(new Computer("Dell", "Latitude 7440", today.AddMonths(-24), 1300m, "Germany"));
    assetService.AddAsset(new MobilePhone("Samsung", "Galaxy S24", today.AddMonths(-8), 850m, "Sweden"));
}