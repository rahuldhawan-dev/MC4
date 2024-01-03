using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.SewerOpenings
{
    /// <summary>
    /// Summary description for SewerOpeningsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class SewerOpeningsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetSewerOpeningsByStreet(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var street = StreetRepository.GetEntity(Int32.Parse(kv["Street"]));
            var values = new List<CascadingDropDownNameValue>();

            foreach (var SewerOpening in street.SewerOpenings)
            {
                //TODO: Check the assetStatus here, for now add everything forever.
                //if (SewerOpening.AssetStatus !=) continue;
                values.Add(new CascadingDropDownNameValue($"{SewerOpening} - {SewerOpening.AssetStatus}",
                    SewerOpening.Id.ToString()));
            }

            return values.ToArray();
        }
    }
}
