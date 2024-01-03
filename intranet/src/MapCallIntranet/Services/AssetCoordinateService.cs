using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using StructureMap;

namespace MapCallIntranet.Services
{
    public class AssetCoordinateService : IAssetCoordinateService
    {
        private readonly IContainer _container;

        public AssetCoordinateService(IContainer container)
        {
            _container = container;
        }

        public int? GetCoordinateIdForAsset(int assetType, int assetId)
        {
            return FindAsset(assetType, assetId)?.Coordinate?.Id;
        }

        private IThingWithCoordinate FindAsset(int assetType, int assetId)
        {
            switch (assetType)
            {
                case AssetType.Indices.HYDRANT:
                    return _container.GetInstance<IHydrantRepository>().Find(assetId);
                case AssetType.Indices.VALVE:
                    return _container.GetInstance<IValveRepository>().Find(assetId);
                case AssetType.Indices.SEWER_OPENING:
                    return _container.GetInstance<ISewerOpeningRepository>().Find(assetId);
                case AssetType.Indices.MAIN_CROSSING:
                    return _container.GetInstance<IMainCrossingRepository>().Find(assetId);
            }

            return null;
        }
    }
}