using MMSINC.Common;
using WorkOrders.Model;

namespace LINQTo271.Views.Assets
{
    public class AssetEntityEventArgs : EntityEventArgs<Asset>
    {
        #region Constructors

        public AssetEntityEventArgs(Asset asset)
            : base(asset)
        {
        }

        public AssetEntityEventArgs(Asset asset, double latitude, double longitude)
            : this(asset)
        {
            Entity.Latitude = latitude;
            Entity.Longitude = longitude;
        }

        public AssetEntityEventArgs(int assetTypeID, int assetID) : this(GetAssetByIDs(assetTypeID, assetID))
        {
        }

        public AssetEntityEventArgs(int assetTypeID, int assetID, double latitude, double longitude) : this(GetAssetByIDs(assetTypeID, assetID), latitude, longitude)
        {
        }

        #endregion

        #region Private Static Methods

        private static Asset GetAssetByIDs(int assetTypeID, int assetID)
        {
            return Asset.GetAssetByIDs(assetTypeID, assetID);
        }

        #endregion
    }
}
