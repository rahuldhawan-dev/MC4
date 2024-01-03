using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Hydrants
{
    /// <summary>
    /// Summary description for HydrantsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class HydrantsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetHydrantsByStreet(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var opCntr =
                OperatingCenterRepository.GetEntity(Int32.Parse(kv["undefined"]));
            var street = StreetRepository.GetEntity(Int32.Parse(kv["Street"]));
            var values = new List<CascadingDropDownNameValue>();

            foreach (var hydrant in street.Hydrants)
            {
                if (hydrant.OperatingCenter.OpCntr.ToLower() != opCntr.OpCntr.ToLower()) continue;

                values.Add(new CascadingDropDownNameValue($"{hydrant} - {hydrant.AssetStatus}", hydrant.HydrantID.ToString()));
            }

            return values.ToArray();
        }
    }
}
