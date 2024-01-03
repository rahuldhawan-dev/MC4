using System.ComponentModel;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using WorkOrders.Model;

namespace LINQTo271.Views.OperatingCenterStockedMaterials
{
    /// <summary>
    /// Summary description for OperatingCenterStockedMaterialServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class OperatingCenterStockedMaterialServiceView : WebService
    {
        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public object LookupMaterials(string search, string operatingCenterID)
        {
            var opCntrID = int.Parse(operatingCenterID);
            return
                OperatingCenterStockedMaterialRepository.
                    LookupMaterialByStockNumber(opCntrID,
                        search).Union(
                    OperatingCenterStockedMaterialRepository.
                        LookupMaterialByDescription(
                        opCntrID, search)).Select(
                    m => new {
                        m.MaterialID,
                        m.Description,
                        m.PartNumber,
                        Size = m.Size ?? string.Empty
                    }).OrderBy(m => m.PartNumber);
        }
    }
}
