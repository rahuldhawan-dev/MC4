using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using WorkOrders.Model;

namespace LINQTo271.Views.Coordinates
{
    /// <summary>
    /// Summary description for CoordinateServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class CoordinateServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public object GetCoordinatesForAsset(string assetTypeID, string assetID)
        {
            var asset =
                Asset.GetAssetByTypeAndKey(
                    AssetTypeRepository.GetEntity(assetTypeID), assetID);
            if (asset != null && asset.Latitude != null && asset.Longitude != null)
            {
                return new {
                    latitude = asset.Latitude,
                    longitude = asset.Longitude
                };
            }
            return null;
        }
    }
}
