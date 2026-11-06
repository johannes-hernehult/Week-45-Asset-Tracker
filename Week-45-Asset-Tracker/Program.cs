using Week_45_Asset_Tracker.Models;
using Week_45_Asset_Tracker.Services;
using Week_45_Asset_Tracker.UI;


AssetService assetService = new AssetService();
SeedTestData(assetService);

Console.WriteLine("======= Asset Tracker =======");

var mainMenu = new Menu("Asset Tracker 5000")
    .Add("Add Asset", () => AddAsset.Run(assetService))
    .Add("View Asset", () => ViewAssets.Run(assetService));
mainMenu.Show();

static void SeedTestData(AssetService assetService)
{
    var today = DateOnly.FromDateTime(DateTime.Today);


    // Less than 3 months left.
    assetService.AddAsset(new MobilePhone("Apple", "iPhone 14", today.AddMonths(-33), 899m, "Sweden"));
    assetService.AddAsset(new Computer("Lenovo", "ThinkPad T14", today.AddMonths(-33).AddDays(-10), 1600m, "Germany"));
    assetService.AddAsset(new Tablet("Samsung", "Galaxy Tab S8", today.AddMonths(-34), 720m, "USA"));
    assetService.AddAsset(new MobilePhone("Google", "Pixel 7", today.AddMonths(-34).AddDays(-10), 649m, "Germany"));
    assetService.AddAsset(new Computer("Apple", "MacBook Air M2", today.AddMonths(-35), 1800m, "Sweden"));

    // Less than 6 months left.
    assetService.AddAsset(new Computer("Dell", "Latitude 7440", today.AddMonths(-30), 1300m, "Germany"));
    assetService.AddAsset(new MobilePhone("Samsung", "Galaxy S23", today.AddMonths(-30).AddDays(-10), 800m, "Sweden"));
    assetService.AddAsset(new Tablet("Apple", "iPad Air 5", today.AddMonths(-31), 699m, "USA"));
    assetService.AddAsset(new Computer("HP", "ProBook 450", today.AddMonths(-31).AddDays(-10), 1050m, "Sweden"));
    assetService.AddAsset(new MobilePhone("Google", "Pixel 7a", today.AddMonths(-32), 499m, "Germany"));

    // More than 6 months left: no warning status yet.
    assetService.AddAsset(new Computer("Apple", "MacBook Pro", today.AddMonths(-2), 2400m, "USA"));
    assetService.AddAsset(new MobilePhone("Google", "Pixel 8", today.AddMonths(-5), 699m, "Germany"));
    assetService.AddAsset(new Tablet("Apple", "iPad Air", today.AddMonths(-12), 599m, "Sweden"));
    assetService.AddAsset(new Computer("Lenovo", "ThinkPad X1 Carbon", today.AddMonths(-18), 1750m, "Germany"));
    assetService.AddAsset(new MobilePhone("Samsung", "Galaxy S24", today.AddMonths(-24), 850m, "Sweden"));
    assetService.AddAsset(new Tablet("Samsung", "Galaxy Tab S9", today.AddMonths(-29), 780m, "USA"));
}