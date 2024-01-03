using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using WorkOrders.Model;

namespace LINQTo271.Views.Equipments
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false), ScriptService]
    public class EquipmentServiceView : WebService
    {
        #region Constants

        public const int T_AND_D_DEPARTMENT_ID = 1, PRODUCTION_DEPARTMENT_ID = 3;

        #endregion
        
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetEquipmentByTown(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var town = TownRepository.GetEntity(Int32.Parse(kv["Town"]));
            var values = new List<CascadingDropDownNameValue>();
            // Where T&D, not production
            var facilities = town.Facilities.Where(x => x.DepartmentID == T_AND_D_DEPARTMENT_ID);
            
            // If an operating center has workorder invoicing, we want to enable production equipment
            foreach (var operatingCenterTown in town.OperatingCentersTowns)
            {
                if (operatingCenterTown.OperatingCenter.HasWorkOrderInvoicing)
                {
                    facilities =
                        facilities.Union(
                            town.Facilities.Where(
                                x =>
                                    x.DepartmentID ==
                                    PRODUCTION_DEPARTMENT_ID &&
                                    x.OperatingCenterID ==
                                    operatingCenterTown.OperatingCenterID));
                }
            }

            foreach (var facility in facilities)
            {
                values.AddRange(facility.Equipments.Select(equipment => new CascadingDropDownNameValue(
                    $"{equipment.Identifier} - {equipment.AssetStatus}", 
                    equipment.EquipmentID.ToString())));
            }

            return values.ToArray();
        }
    }
}
