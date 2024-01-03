using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Employees
{
    /// <summary>
    /// Summary description for EmployeesServiceView
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/"),
     WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1),
     ToolboxItem(false), ScriptService]
    public class EmployeesServiceView : WebService
    {
        [WebMethod, ScriptMethod]
        public string[] GetEmployees(string prefixText, int count)
        {
            var employeeArray = new List<string>();
            foreach (var e in EmployeeRepository.SelectByNamePart(prefixText, count))
                employeeArray.Add(e.ToString());
            return employeeArray.ToArray();
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetEmployeesByOperatingCenterID(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var operatingCenterID = Int32.Parse(kv["undefined"]);
            var values = new List<CascadingDropDownNameValue>();
            var employees = EmployeeRepository.GetEmployeesByOperatingCenterID(operatingCenterID);
            foreach (var employee in employees)
                values.Add(new CascadingDropDownNameValue(employee.FullName,
                    employee.EmployeeID.ToString()));
            return values.ToArray();
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetEmployeesByOperatingCenterText(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var opCntr = kv["undefined"];
            var values = new List<CascadingDropDownNameValue>();
            var employees = EmployeeRepository.GetEmployeesByOperatingCenterText(opCntr);
            foreach (var employee in employees)
                values.Add(new CascadingDropDownNameValue(employee.FullName,
                    employee.EmployeeID.ToString()));
            return values.ToArray();
        }
    }
}