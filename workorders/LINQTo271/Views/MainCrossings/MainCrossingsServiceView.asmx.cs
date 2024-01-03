using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.MainCrossings
{
    [WebService(Namespace = "http://tempuri.org/"),
        WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
        ToolboxItem(false), ScriptService]
    public partial class MainCrossingsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetMainCrossingsByTown(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var town = TownRepository.GetEntity(Int32.Parse(kv["Town"]));
            var values = new List<CascadingDropDownNameValue>();

            foreach (var mainCrossing in town.MainCrossings)
            {
                values.Add(new CascadingDropDownNameValue($"{mainCrossing} - {mainCrossing.MainCrossingStatus}", mainCrossing.MainCrossingID.ToString()));
            }

            return values.ToArray();
        }

    }
}