namespace Week_45_Asset_Tracker.Services;

using Week_45_Asset_Tracker.Models;

public class AssetService
{
    private List<Asset> _assets = [];

    public void AddAsset(Asset asset)
    {
        _assets.Add(asset);
    }

    public IReadOnlyList<Asset> GetAssets()
    {
        return _assets.AsReadOnly();
    }

    public void RemoveAsset(int id)
    {
        _assets.RemoveAll(asset => asset.Id == id);
    }

    public List<Asset> SearchAssets(string searchTerm)
    {
        string term = searchTerm.Trim();

        if (term.Length == 0)
            return [.. _assets];

        return _assets
            .Where(asset =>
                asset.Brand.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                asset.Model.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}