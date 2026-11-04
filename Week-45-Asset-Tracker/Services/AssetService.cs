namespace Week_45_Asset_Tracker.Services;

using Week_45_Asset_Tracker.Models;

public class AssetService
{
    private List<Asset> _assets = [];

    public void AddAsset(Asset asset)
    {
        _assets.Add(asset);
    }

    public List<Asset> GetAssets()
    {
        return _assets;
    }

    public void RemoveAsset(int id)
    {
        _assets.RemoveAll(asset => asset.Id == id);
    }

    public List<Asset> SearchAssets(string searchTerm)
    {
        //To be done
        return _assets;
    }

}