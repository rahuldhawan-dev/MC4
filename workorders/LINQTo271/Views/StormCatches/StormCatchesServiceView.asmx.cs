using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.StormCatches
{
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class StormCatchesServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStormCatchesByStreet(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var street = StreetRepository.GetEntity(Int32.Parse(kv["Street"]));
            var values = new List<CascadingDropDownNameValue>();

            foreach (var StormCatch in street.StormCatches)
            {
                //TODO: Check the assetStatus here, for now add everything forever.
                values.Add(new CascadingDropDownNameValue(StormCatch.ToString(),
                    StormCatch.StormCatchID.ToString()));
            }

            return values.ToArray();
        }
    }
}
