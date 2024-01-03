using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.ScheduleOfValue
{
    /// <summary>
    /// Summary description for ScheduleOfValueView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false),
    ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ScheduleOfValuesServiceView : WebService
    {
        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetScheduleOfValuesByCategoryID(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var categoryId = ScheduleOfValueCategoryRepository.GetEntity(Int32.Parse(kv["undefined"]));
            var items = ScheduleOfValueRepository.SelectByScheduleOfValueCategory(categoryId).ToList();
            if (!items.Any())
                return null;

            var ret = items.Select(item => new CascadingDropDownNameValue(GetDescriptionForDropDown(item), item.Id.ToString())).ToArray();
            return ret;
        }

        private string GetDescriptionForDropDown(global::WorkOrders.Model.ScheduleOfValue item)
        {
            if (item.UnitOfMeasure != null)
                return $"{item.Description} + [{item.UnitOfMeasure.Description}]";
            return item.Description;
        }
    }
}
