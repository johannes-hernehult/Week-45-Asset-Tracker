namespace Week_45_Asset_Tracker.Services;

using Week_45_Asset_Tracker.Models;

public enum SortBy
{
    AssetType,
    PurchaseDate,
}

public class AssetManager
{
    public List<Asset> assets = [];

    // === CRUD ===
    // Add asset
    public void AddAsset(Asset asset)
    {
        assets.Add(asset);
    }

    // Show assets
    public List<Asset> ShowAssets()
    {
        return assets;
    }

    // Remove asset
    public void RemoveAsset(int id)
    {
        assets.RemoveAll(asset => asset.Id == id);
    }

    // Search asset
    public List<Asset> SearchAssets(string searchTerm)
    {
        // Figure this one out
        // Maybe concat all values into a string and search those strings? 
        return assets;
    }

    // === HELPERS ===
    // SortAssets()

    // 
}