using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.RestorationMethods
{
    /// <summary>
    /// Summary description for HydrantsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class RestorationMethodsServiceView : WebService
    {
        #region Exposed Methods

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetFinalRestorationMethodsByRestorationType(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var type =
                RestorationTypeRepository.GetEntity(Int32.Parse(kv["undefined"]));
            var values = new List<CascadingDropDownNameValue>();

            var methods =
                (from mt in type.RestorationMethodsRestorationTypes
                 where mt.FinalMethod
                 select mt.RestorationMethod);

            foreach (var method in methods)
            {
                values.Add(new CascadingDropDownNameValue(method.ToString(),
                    method.RestorationMethodID.ToString()));
            }

            return values.ToArray();
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetInitialRestorationMethodsByRestorationType(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var type =
                RestorationTypeRepository.GetEntity(Int32.Parse(kv["undefined"]));
            var values = new List<CascadingDropDownNameValue>();

            var methods =
                (from mt in type.RestorationMethodsRestorationTypes
                 where mt.InitialMethod
                 select mt.RestorationMethod);

            foreach (var method in methods)
            {
                values.Add(new CascadingDropDownNameValue(method.ToString(),
                    method.RestorationMethodID.ToString()));
            }

            return values.ToArray();
        }

        #endregion
    }
}
