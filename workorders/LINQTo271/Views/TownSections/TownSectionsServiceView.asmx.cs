using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.TownSections
{
    /// <summary>
    /// Summary description for TownSectionsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false),
    ScriptService]
    public class TownSectionsServiceView : WebService
    {
        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetTownSectionsByTown(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var town = TownRepository.GetEntity(Int32.Parse(kv["undefined"]));
            return GetTownSections(town);
        }

        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetTownSectionsByTownDefined(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var town = TownRepository.GetEntity(Int32.Parse(kv["town"]));
            return GetTownSections(town);
        }

        private CascadingDropDownNameValue[] GetTownSections(Town town)
        {
            var values =
                new List<CascadingDropDownNameValue>();
            var townSections = TownSectionRepository.SelectByTownID(town.TownID);

            foreach (var townSection in townSections)
                values.Add(new CascadingDropDownNameValue(townSection.Name,
                    townSection.TownSectionID.ToString()));
            return values.ToArray();
        }
    }
}