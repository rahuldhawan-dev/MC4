using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class SearchNearMiss : SearchSet<NearMiss>
    {
        #region Properties

        public virtual int? Id { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdOrIsContractedOperationsForHealthAndSafetyNearMiss", DependsOn = "State,IsContractedOperations", DependentsRequired = DependentRequirement.None)]
        [SearchAlias("OperatingCenter", "oc", "Id", Required = true)]
        public int[] OperatingCenter { get; set; }

        [SearchAlias("OperatingCenter", "oc", "IsContractedOperations")]
        public bool? IsContractedOperations { get; set; }

        public DateRange OccurredAt { get; set; }

        public DateRange CreatedAt { get; set; }

        public DateRange DateCompleted { get; set; }

        [Search(CanMap = false)]
        public bool? Completed { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]
        public int? State { get; set; }

        [MultiSelect("", "Facility", "ByOperatingCenterIds", DependsOn = "OperatingCenter", PromptText = "Select an OperatingCenter above")]
        [EntityMap, EntityMustExist(typeof(Facility))]
        public int[] Facility { get; set; }

        [DropDown, EntityMustExist(typeof(NearMissType)), EntityMap]
        public int? Type { get; set; }

        [EntityMustExist(typeof(NearMissCategory)), EntityMap]
        [DropDown("HealthAndSafety", "NearMissCategory", "ByType", DependsOn = nameof(Type))]
        public int? Category { get; set; }

        [EntityMustExist(typeof(NearMissSubCategory)), EntityMap]
        [DropDown("HealthAndSafety", "NearMissSubCategory", "ByCategory", DependsOn = nameof(Category))]
        public int? SubCategory { get; set; }

        [EntityMap, EntityMustExist(typeof(ActionTakenType))]
        [MultiSelect]
        public int[] ActionTakenType { get; set; }

        public string ActionTaken { get; set; }

        [StringLength(NearMiss.StringLengths.INCIDENT_NUMBER)]
        public virtual string IncidentNumber { get; set; }

        [CheckBox]
        [View("SIF Potential")]
        public virtual bool? SeriousInjuryOrFatality { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(LifeSavingRuleType))]
        [View("Life Saving Rule")]
        public int[] LifeSavingRuleType { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(StopWorkUsageType))]
        [View("Who Was Stopped?")]
        public int[] StopWorkUsageType { get; set; }

        [AutoComplete("HealthAndSafety", "NearMiss", "ReportedByEmployeePartialFirstOrLastName")]
        public string ReportedBy { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (Completed.HasValue)
            {
                mapper.MappedProperties["DateCompleted"].Value = Completed.Value
                    ? SearchMapperSpecialValues.IsNotNull
                    : SearchMapperSpecialValues.IsNull;
            }
        }

        #endregion
    }
}