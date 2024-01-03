using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval
{
    public class SearchWorkOrderSupervisorApproval : SearchWorkOrder
    {
        [DropDown, RequiredWhen(nameof(Id), ComparisonType.EqualTo, null)]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }

        // 271 had this as a dropdown that was just Yes/No with no ability to select neither.
        // By default it's false/No.
        [CheckBox, Search(CanMap = false)]
        public bool IsApproved { get; set; }
        // this property is needed for the search mapper, but it's not used on the page itself
        public DateTime? ApprovedOn { get; set; }

        [SearchAlias("CurrentAssignment", "Crew.Id", Required = true)]
        [DropDown("FieldOperations", "Crew", "ByOperatingCenterOrAll", DependsOn = nameof(OperatingCenter), PromptText = "Select an operating center")]
        public int? LastCrewAssigned { get; set; }
        
        protected override void ModifyValuesWhenIdHasValueIsFalse(ISearchMapper mapper)
        {
            base.ModifyValuesWhenIdHasValueIsFalse(mapper);

            mapper.MappedProperties[nameof(ApprovedOn)].Value = IsApproved
                ? SearchMapperSpecialValues.IsNotNull
                : SearchMapperSpecialValues.IsNull;
        }
    }
}