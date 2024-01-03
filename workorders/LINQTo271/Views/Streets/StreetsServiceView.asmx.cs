using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Streets
{
    /// <summary>
    /// Summary description for StreetsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
     WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
     ToolboxItem(false),
     ScriptService]
    public class StreetsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStreetsByTown(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var town = TownRepository.GetEntity(Int32.Parse(kv["undefined"]));
            return GetStreets(town);
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStreetsByTownDefined(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var town = TownRepository.GetEntity(Int32.Parse(kv["town"]));
            return GetStreets(town);
        }

        private CascadingDropDownNameValue[] GetStreets(Town town)
        {
            var values = new List<CascadingDropDownNameValue>();
            //var streets = StreetRepository.SelectByTownName(town.Name);
            var streets = StreetRepository.SelectByTownID(town.TownID);

            foreach (var street in streets)
                values.Add(new CascadingDropDownNameValue(street.FullStName,
                    street.StreetID.ToString()));
            return values.ToArray();
        }
    }
}