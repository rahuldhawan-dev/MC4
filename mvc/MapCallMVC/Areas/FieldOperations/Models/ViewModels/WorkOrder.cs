using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchWorkOrder : SearchSet<WorkOrder>
    {
        #region Properties

        [View("Work Order Number")]
        public int? Id { get; set; }

        // "Hey, why isn't this required, but it's overridden in every other view model that inherits from this?"
        // Because the Premise/Show page has a work orders tab that searches only by premise number.
        // So because of that, the GeneralWorkOrder.Index doesn't require OperatingCenter. 
        // We can't do an override property to *remove* the validator either.
        [DropDown]
        public virtual int? OperatingCenter { get; set; }

        [MultiSelect("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int[] Town { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }
       
        [AutoComplete(
            "",
            nameof(Street),
            "GetByTownIdAndPartialStreetName",
            DependsOn = nameof(Town),
            PlaceHolder = "Please select a Town and enter more than 2 characters",
            DisplayProperty = nameof(MapCall.Common.Model.Entities.Street.FullStName))]
        public int? Street { get; set; }

        public SearchString StreetNumber { get; set; }

        public SearchString ApartmentAddtl { get; set; }
        
        [AutoComplete(
            "",
            nameof(Street),
            "GetByTownIdAndPartialStreetName",
            DependsOn = nameof(Town),
            PlaceHolder = "Please select a Town and enter more than 2 characters",
            DisplayProperty = nameof(MapCall.Common.Model.Entities.Street.FullStName))]
        public int? NearestCrossStreet { get; set; }

        [MultiSelect("", "AssetType", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above"), EntityMap, EntityMustExist(typeof(AssetType))]
        public int[] AssetType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkOrderPriority))]
        public int? Priority { get; set; }

        [MultiSelect("FieldOperations", "WorkDescription", "UsedByAssetTypeIds", DependsOn = "AssetType", PromptText = "Select an asset type above"), EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int[] WorkDescription { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(ServiceMaterial)),
         View(WorkOrder.DisplayNames.PREVIOUS_SERVICE_LINE_MATERIAL)]
        public int[] PreviousServiceLineMaterial { get; set; }

        public long? SAPNotificationNumber { get; set; }
        public long? SAPWorkOrderNumber { get; set; }
        // Document Types
        public DateRange DateReceived { get; set; }
        public DateRange DateCompleted { get; set; }
        public DateRange CancelledAt { get; set; }

        [Search(CanMap = false)] // Needs to be done in the override
        public bool? IsCancelled { get; set; }

        // DateDocumentAttached
        public bool? Completed { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(WorkOrderPurpose))]
        public int[] Purpose { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? CreatedBy { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(WorkOrderRequester))]
        public int[] RequestedBy { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AcousticMonitoringType))]
        public int? AcousticMonitoringType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MarkoutRequirement))]
        public int? MarkoutRequirement { get; set; }

        [BoolFormat("Required", "Not Required")]
        public bool? StreetOpeningPermitRequired { get; set; }

        public bool? RequiresInvoice { get; set; }
        public bool? HasInvoice { get; set; }

        public bool? HasSAPErrorCode { get; set; }
        public bool? HasMaterialsUsed { get; set; }
        [View(DisplayName = "SAP Status")]
        public string SAPErrorCode { get; set; }
        [DisplayName("WBS Charged")]
        public SearchString AccountCharged { get; set; }

        public SearchString Notes { get; set; }
        [Search(CanMap = false)] // Needs to be done in the override
        public bool? IsAssignedContractor { get; set; }
        public bool? StreetOpeningPermitRequested { get; set; }
        public bool? StreetOpeningPermitIssued { get; set; }
        public IntRange LostWater { get; set; }
        public string BusinessUnit { get; set; }
        [Search(CanMap = false)]
        public bool? HasBusinessUnit { get; set; }

        // Below fields are used in reporting
        public bool? HasJobSiteCheckLists { get; set; }

        public bool? HasPreJobSafetyBriefs { get; set; }

        public string PremiseNumber { get; set; }

        public bool? DigitalAsBuiltCompleted { get; set; }

        public DateRange DatePitcherFilterDeliveredToCustomer { get; set; }

        public bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }

        /// <summary>
        /// This is sealed to enforce modifying values only when Id.HasValue == false.
        /// Use ModifyValuesWhenIdHasValueIsFalse instead.
        /// </summary>
        /// <param name="mapper"></param>
        public sealed override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            // If they're searching by the work order number, they often do it without
            // resetting the form. We want to reset the model for them in this case and
            // only search for the work order number.
            if (Id.HasValue)
            {
                mapper.ClearValues();
                mapper.MappedProperties[nameof(Id)].Value = this.Id;
            }
            else
            {
                ModifyValuesWhenIdHasValueIsFalse(mapper);
            }
        }

        protected virtual void ModifyValuesWhenIdHasValueIsFalse(ISearchMapper mapper)
        {
            if (IsCancelled.HasValue)
            {
                var isCanned = IsCancelled.Value ? SearchMapperSpecialValues.IsNotNull : SearchMapperSpecialValues.IsNull;
                mapper.MappedProperties["CancelledAt"].Value = isCanned;
            }

            if (IsAssignedContractor.HasValue)
            {
                var isAssigned = IsAssignedContractor.Value
                    ? SearchMapperSpecialValues.IsNotNull
                    : SearchMapperSpecialValues.IsNull;
                mapper.MappedProperties["AssignedContractor"].Value = isAssigned;
            }

            if (HasBusinessUnit == false)
            {
                mapper.MappedProperties["BusinessUnit"].Value = SearchMapperSpecialValues.IsNullOrEmpty;
            }
        }

        public int? AssignedContractor { get; set; }

        [View(WorkOrder.DisplayNames.PLANNED_COMPLETION_DATE)]
        public DateRange PlannedCompletionDate { get; set; }

        #endregion
    }

    public class HistoryWorkOrder : SearchSet<WorkOrder>
    {
        #region Properties

        [Required, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, EntityMustExist(typeof(AssetType))]
        public int? AssetType { get; set; }

        [Required, Search(CanMap = false)]
        public string AssetId { get; set; }

        public int? Hydrant { get; set; }
        public int? Valve { get; set; }
        public int? Equipment { get; set; }
        public int? MainCrossing { get; set; }
        public int? SewerOpening { get; set; }
        public string PremiseNumber { get; set; }
        public string ServiceNumber { get; set; }

        #endregion

        #region Exposed Methods

        public static void FixAssetInfo(HistoryWorkOrder search)
        {
            switch (search.AssetType)
            {
                case MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT:
                    search.Hydrant = Int32.Parse(search.AssetId);
                    break;
                case MapCall.Common.Model.Entities.AssetType.Indices.VALVE:
                    search.Valve = Int32.Parse(search.AssetId);
                    break;
                case MapCall.Common.Model.Entities.AssetType.Indices.EQUIPMENT:
                    search.Valve = Int32.Parse(search.AssetId);
                    break;
                case MapCall.Common.Model.Entities.AssetType.Indices.MAIN_CROSSING:
                    search.MainCrossing = Int32.Parse(search.AssetId);
                    break;
                case MapCall.Common.Model.Entities.AssetType.Indices.SERVICE:
                case MapCall.Common.Model.Entities.AssetType.Indices.SEWER_LATERAL:
                    var split = search.AssetId.Split(',');
                    search.PremiseNumber = split[0];
                    if (!string.IsNullOrWhiteSpace(split[1]))
                        search.ServiceNumber = split[1];
                    break;
                case MapCall.Common.Model.Entities.AssetType.Indices.SEWER_OPENING:
                    search.SewerOpening = Int32.Parse(search.AssetId);
                    break;
            }
        }

        #endregion
    }

    // This is used for looking up work order information for Job Observations
    public class SearchWorkOrderId
    {
        #region Properties

        [Required]
        public int? WorkOrderId { get; set; }

        #endregion
    }

    public class SearchCompletedWorkOrdersWithPreJobSafetyBriefs : SearchSet<CompletedWorkOrderWithPreJobSafetyBriefReportItem>, ISearchCompletedWorkOrdersWithPreJobSafetyBriefs
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdForFieldServicesWorkManagement", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int[] OperatingCenter { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int[] WorkDescription { get; set; }

        [Required]
        public RequiredDateRange DateCompleted { get; set; }

        public bool? IsAssignedContractor { get; set; }
    }

    public class SearchCompletedWorkOrdersWithJobSiteCheckLists : SearchSet<CompletedWorkOrderWithJobSiteCheckListReportItem>, ISearchCompletedWorkOrdersWithJobSiteCheckLists
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [MultiSelect("", nameof(OperatingCenter), "ByStateIdForFieldServicesWorkManagement", DependsOn = nameof(State), DependentsRequired = DependentRequirement.None)]
        public int[] OperatingCenter { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int[] WorkDescription { get; set; }

        [Required]
        public RequiredDateRange DateCompleted { get; set; }

        public bool? IsAssignedContractor { get; set; }
    }

    public class SearchCompletedWorkOrdersWithMaterial : SearchSet<CompletedWorkOrderWithMaterialReportItem>, ISearchCompletedWorkOrdersWithMaterial
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdForFieldServicesWorkManagement", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int[] OperatingCenter { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int[] WorkDescription { get; set; }

        [Required]
        public RequiredDateRange DateCompleted { get; set; }

        public bool? IsAssignedContractor { get; set; }
    }

    public class SearchWaterLoss : SearchSet<WaterLossSearchResultViewModel>, ISearchWaterLoss
    {
        [Required, DisplayName("Date Completed")]
        public RequiredDateRange Date { get; set; }
        [MultiSelect]
        public int[] OperatingCenter { get; set; }
    }

    public class SearchCompletedWorkOrdersWithMarkout : SearchSet<CompletedWorkOrderWithMarkoutReportItem>, ISearchCompletedWorkOrdersWithMarkout
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdForFieldServicesWorkManagement", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int[] OperatingCenter { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int[] WorkDescription { get; set; }

        [Required]
        public RequiredDateRange DateCompleted { get; set; }

        public bool? IsAssignedContractor { get; set; }
    }
}