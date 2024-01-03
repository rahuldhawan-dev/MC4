using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.AssetTypes
{
    /// <summary>
    /// Summary description for AssetTypesServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class AssetTypesServiceView : WebService
    {
        [ScriptMethod,WebMethod]
        public CascadingDropDownNameValue[] GetAssetTypesByOperatingCenter(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var operatingCenter = OperatingCenterRepository.GetEntity(kv["undefined"]);
            var values =
                new List<CascadingDropDownNameValue>();
            var assetTypes =
                OperatingCenterAssetTypeRepository.
                    SelectAssetTypesByOperatingCenter(operatingCenter);

            foreach (var assetType in assetTypes)
                values.Add(new CascadingDropDownNameValue(assetType.Description,
                    assetType.AssetTypeID.ToString()));
            return values.ToArray();
        }
    }
}
