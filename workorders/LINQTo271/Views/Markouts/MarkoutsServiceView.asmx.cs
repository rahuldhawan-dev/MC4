using System;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using MapCall.Common.Utility;

namespace LINQTo271.Views.Markouts
{
    /// <summary>
    /// Summary description for MarkoutsServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false),
    ScriptService]
    public class MarkoutsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public string GetCallDateForDateNeeded(string dateNeeded)
        {
            var date = DateTime.Parse(dateNeeded);
            return WorkOrdersWorkDayEngine.GetCallDate(date,
                MarkoutRequirementEnum.Routine).ToString("d");
        }
    }
}
