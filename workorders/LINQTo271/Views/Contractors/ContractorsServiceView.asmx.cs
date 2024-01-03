using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Contractors
{
    /// <summary>
    /// Summary description for ContractorServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ContractorsServiceView : System.Web.Services.WebService
    {
        [WebMethod, ScriptMethod]

        public CascadingDropDownNameValue[] GetContractorsByOperatingCenterID(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var operatingCenter =
                OperatingCenterRepository.GetEntity(kv["undefined"]);
            var values = new List<CascadingDropDownNameValue>();
            var contractors =
                ContractorRepository.GetContractorsByOperatingCenterID(
                    operatingCenter.OperatingCenterID);

            foreach (var contractor in contractors)
            {
                values.Add(new CascadingDropDownNameValue(contractor.Name,
                    contractor.ContractorID.ToString()));
            }
            return values.ToArray();
        }
    }
}
