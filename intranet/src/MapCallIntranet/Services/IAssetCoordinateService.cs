namespace MapCallIntranet.Services
{
    public interface IAssetCoordinateService
    {
        int? GetCoordinateIdForAsset(int assetType, int assetId);
    }
}
