using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchSAPMaintenancePlan
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }
        [DropDown("", "OperatingCenter", "ByStateIdForProductionWorkManagement", DependsOn = "State", PromptText = "Please select a state above"),
            RequiredWhen("State", ComparisonType.NotEqualTo, null, ErrorMessage = "Required with State")]
        public virtual int? OperatingCenter { get; set; }
        [SearchAlias("Facility", "PlanningPlant.Id")]
        [DropDown("", "PlanningPlant", "ByOperatingCenterCodeAndNotId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above")]
        public virtual string[] PlanningPlant { get; set; }
        [UIHint("FunctionalLocation")]
        public virtual string FunctionalLocation { get; set; }
        [DropDown("", "Equipment", "EquipmentTypesByFunctionalLocation", DependsOn = "FunctionalLocation", PromptText = "Please select a Functional Location above")]
        public virtual string EquipmentType { get; set; }
        public virtual string SAPEquipmentID { get; set; }
        public virtual string MaintenancePlan { get; set; }


        // THESE TWO ARE USED FOR SHENANNIGANS,
        // TO POST VALUES TO THE FORMS VIA VIEWDATA
        // TODO: just pass the id and lookup these values later?
        [Search(CanMap = false)]
        public virtual string Equipment { get; set; }
        [Search(CanMap = false)]
        public virtual string MapCallEquipmentId { get; set; }

        [Search(CanMap = false)]
        public virtual bool ShowAddToMaintenancePlan { get; set; }
        [Search(CanMap = false)]
        public virtual bool ShowRemoveFromMaintenancePlan { get; set; }

        #endregion

        public SAPMaintenancePlanLookup ToSearchSAPMaintenancePlan(IRepository<PlanningPlant> planningPlant)
        {
            var OperatingCenters = planningPlant.Where(s => s.OperatingCenter.Id == OperatingCenter).Select(s => s.Code).ToArray();
            string emptyString = "";
            string[] emptyArr = new string[0];

            if (OperatingCenter == null)
            {
                OperatingCenter = 0;
                PlanningPlant = emptyArr;
            }
            else if 
                (PlanningPlant.Contains(emptyString) && OperatingCenter != null)
                PlanningPlant = OperatingCenters;
      
            return new SAPMaintenancePlanLookup {
                PlanningPlant = PlanningPlant,
                FunctionalLocation = FunctionalLocation ?? string.Empty,
                EquipmentType = EquipmentType ?? string.Empty,
                SAPEquipmentID = SAPEquipmentID ?? string.Empty,
                MaintenancePlan = MaintenancePlan ?? string.Empty
            };
        }
    }
}