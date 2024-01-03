using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Towns
{
    /// <summary>
    /// Summary description for TownsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false),
    ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TownsServiceView : WebService
    {
        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByOperatingCenter(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var operatingCenter = OperatingCenterRepository.GetOperatingCenterByOpCntr(kv["undefined"]);
            var values =
                new List<CascadingDropDownNameValue>();
            var towns = TownRepository.SelectByOperatingCenter(operatingCenter);

            foreach (var town in towns)
                values.Add(new CascadingDropDownNameValue(town.Name,
                    town.TownID.ToString()));
            return values.ToArray();
        }

        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByOperatingCenterID(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var operatingCenter = OperatingCenterRepository.GetEntity(kv["undefined"]);
            var values =
                new List<CascadingDropDownNameValue>();
            var towns = TownRepository.SelectByOperatingCenter(operatingCenter);

            foreach (var town in towns)
                values.Add(new CascadingDropDownNameValue(town.Name,
                    town.TownID.ToString()));
            return values.ToArray();
        }

        [WebMethod]
        [ScriptMethod]
        public string GetTownCriticalMainBreakNotes(int townId)
        {
            var town = TownRepository.GetEntity(townId);
            return (town != null) ? town.CriticalMainBreakNotes : string.Empty;
        }
    }
}
