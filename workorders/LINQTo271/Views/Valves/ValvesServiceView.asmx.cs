using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Valves
{
    /// <summary>
    /// Summary description for ValvesServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class ValvesServiceView : WebService
    {
        /// <summary>
        /// Gets a list of Valves that are active or pending
        /// </summary>
        /// <param name="knownCategoryValues"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetValvesByStreet(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var opCntr =
                OperatingCenterRepository.GetEntity(Int32.Parse(kv["undefined"]));
            var street = StreetRepository.GetEntity(Int32.Parse(kv["Street"]));
            var values = new List<CascadingDropDownNameValue>();

            foreach (var valve in street.Valves)
            {
                if (valve.OperatingCenter.OpCntr.ToLower() != opCntr.OpCntr.ToLower()) continue;
                
                values.Add(new CascadingDropDownNameValue($"{valve.FullValveSuffix} - {valve.AssetStatus}", valve.ValveID.ToString()));
            }
            return values.ToArray();
        }
    }
}
