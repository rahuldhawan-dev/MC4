using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkDescriptions
{
    /// <summary>
    /// Summary description for WorkDescriptionsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
     WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
     ToolboxItem(false),
     ScriptService]
    public class WorkDescriptionsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetWorkDescriptionsByAssetType(string knownCategoryValues, string category, string contextKey)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var assetType = AssetTypeRepository.GetEntity(Int32.Parse(kv["AssetType"]));
            var values = new List<CascadingDropDownNameValue>();

            var descriptions =
				WorkDescriptionRepository.SelectByAssetType(assetType,
                    (contextKey.Contains("input") ? true : false),
                    (contextKey.Contains("revisit") ? true : false));

            foreach (var description in descriptions)
                values.Add(new CascadingDropDownNameValue(description.Description, description.WorkDescriptionID.ToString()));
            return values.ToArray();
        }
    }
}