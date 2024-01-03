using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.StockLocations
{
    /// <summary>
    /// Summary description for StockLocationServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false),
    ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StockLocationServiceView : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetStockLocationsByOperatingCenterID(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var operatingCenterId = int.Parse(kv["undefined"]);
            var values =
                new List<CascadingDropDownNameValue>();
            var stockLocations = StockLocationRepository.SelectByOperatingCenter(operatingCenterId);

            foreach (var stockLocation in stockLocations)
                values.Add(new CascadingDropDownNameValue(stockLocation.Description,
                    stockLocation.StockLocationID.ToString()));
            return values.ToArray();
        }
    }
}
