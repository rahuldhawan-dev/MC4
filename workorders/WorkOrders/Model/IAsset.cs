using System.Data.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Interface used to specify all common properties to an asset object
    /// (Valve, Hydrant, Main, Service, Sewer Assets).
    /// </summary>
    public interface IAsset
    {
        #region Properties

        EntitySet<WorkOrder> WorkOrders { get; set; }
        object AssetKey { get; }
        string AssetID { get; }
        double? Latitude { get; set; }
        double? Longitude { get; set; }

        #endregion
    }
}
