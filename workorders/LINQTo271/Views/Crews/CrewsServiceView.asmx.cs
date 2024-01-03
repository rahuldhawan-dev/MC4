using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Crews
{
    [WebService(Namespace = "http://tempuri.org/"),
    WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
    ToolboxItem(false), ScriptService]
    public class CrewsServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetCrewsByOperatingCenterID(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var operatingCenter = OperatingCenterRepository.GetEntity(kv["undefined"]);
            var values =
                new List<CascadingDropDownNameValue>();
            var crews = CrewRepository.SelectByOperatingCenter(operatingCenter);

            foreach (Crew crew in crews)
                values.Add(new CascadingDropDownNameValue(crew.Description,
                    crew.CrewID.ToString()));
            return values.ToArray();
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetCrewsByContractor(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var contractor = ContractorRepository.GetEntity(kv["undefined"]);
            var values =
                new List<CascadingDropDownNameValue>();
            var crews = CrewRepository.SelectByContractor(contractor);

            foreach (Crew crew in crews)
                values.Add(new CascadingDropDownNameValue(crew.Description,
                    crew.CrewID.ToString()));
            return values.ToArray();
        }
    }
}
