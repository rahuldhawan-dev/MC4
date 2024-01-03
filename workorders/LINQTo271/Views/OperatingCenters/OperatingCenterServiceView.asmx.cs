using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.OperatingCenters
{
    /// <summary>
    /// Summary description for OperatingCenterServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class OperatingCenterServiceView : System.Web.Services.WebService
    {
        #region Private methods

        private static CascadingDropDownNameValue[] GetOperatingCenters(IEnumerable<OperatingCenter> opCenters)
        {
            var opCDaiCs = opCenters.OrderBy(x => x.OpCntr).Select(x =>
                new CascadingDropDownNameValue(x.FullDescription,
                    x.OperatingCenterID.ToString()));
            return opCDaiCs.ToArray();
        }

        #endregion

        #region Public Methods

        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetRegulatedOperatingCentersByState(string knownCategoryValues)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            // I have no idea why this comes back as "undefined" other than the fact that the parent dropdown isn't 
            // a cascading dropdown. -Ross 3/28/2018
            var stateId = Int32.Parse(kv["undefined"]);
            var opCenters = OperatingCenterRepository.SelectAll271OperatingCentersThatAreRegulatedByState(stateId);
            return GetOperatingCenters(opCenters);
        }

        [WebMethod]
        [ScriptMethod]
        public CascadingDropDownNameValue[] GetUnregulatedOperatingCentersByState(string knownCategoryValues)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            // I have no idea why this comes back as "undefined" other than the fact that the parent dropdown isn't 
            // a cascading dropdown. -Ross 3/28/2018
            var stateId = Int32.Parse(kv["undefined"]);
            var opCenters = OperatingCenterRepository.SelectAll271OperatingCentersThatAreNotRegulatedByState(stateId);
            return GetOperatingCenters(opCenters);
        }

        #endregion
    }
}
