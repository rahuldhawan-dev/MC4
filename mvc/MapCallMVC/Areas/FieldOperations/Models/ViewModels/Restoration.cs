using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class RestorationViewModel : ViewModel<Restoration>
    {
        #region Consts

        private const string VALIDATION_PARTIAL_NOTES = "Partial restoration notes are required when actual square footage is greater than estimated square footage.";
        private const string VALIDATION_FINAL_NOTES = "Final restoration notes are required when actual square footage is greater than estimated square footage.";

        #endregion

        #region Properties

        #region Initial Data

        [DoesNotAutoMap("Display only")]
        public Restoration Display
        {
            get { return _container.GetInstance<IRepository<Restoration>>().Find(Id); }
        }

        [DisplayName("Work Order"), DoesNotAutoMap("Display only")]
        public WorkOrder WorkOrderDisplay
        {
            get
            {
                return _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.GetValueOrDefault());
            }
        }

        // Restorations can be made without a workorder.
        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [RequiredWhen("AssignedContractor", ComparisonType.NotEqualTo, null)]
        [StringLength(Restoration.StringLengths.WBS_NUMBER)]
        public string WBSNumber { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public int? RestorationType { get; set; }

        [Required]
        public decimal? EstimatedRestorationFootage { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(RestorationResponsePriority))]
        public int? ResponsePriority { get; set; }

        //[Required]
        public string RestorationNotes { get; set; }

       // [DropDown]
        [RequiredWhen("CompletedByOthers", false), EntityMap, EntityMustExist(typeof(Contractor))]
        public virtual int? AssignedContractor { get; set; }

        [Secured]
        public DateTime? AssignedContractorAt { get; set; }

        // This is required but I want a checkbox, so no nullables.
        public bool EightInchStabilizeBaseByCompanyForces { get; set; }

        [Secured]
        public decimal? TotalAccruedCost { get; set; }

        public bool? CompletedByOthers { get; set; }

        [RequiredWhen("CompletedByOthers", true)]
        public string CompletedByOthersNotes { get; set; }

        public bool TrafficControlRequired { get; set; }

        [RequiredWhen("AssignedContractor", ComparisonType.NotEqualTo, null)]
        [StringLength(Restoration.StringLengths.INITIAL_PO_NUM)]
        public string InitialPurchaseOrderNumber { get; set; }

        // This is a checkbox, not a dropdown.
        [CheckBox]
        public bool AcknowledgedByContractor { get; set; }

        #endregion

        #region Reopening

        public DateTime? DateReopened { get; set; }
        public DateTime? DateRescheduled { get; set; }
        public DateTime? DateRecompleted { get; set; }

        #endregion

        #region Partial / Temporary Restoration

        [MultiSelect("FieldOperations", "RestorationMethod", "ByRestorationTypeID", DependsOn = "RestorationType")]
        [EntityMap, EntityMustExist(typeof(RestorationMethod))]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public int[] PartialRestorationMethods { get; set; }

        [StringLength(Restoration.StringLengths.PARTIAL_INVOICE_NUMBER), RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public string PartialRestorationInvoiceNumber { get; set; }

        [RequiredWhen("PartialPavingSquareFootage", ComparisonType.NotEqualTo, null)]
        public DateTime? PartialRestorationDate { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(Contractor))]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public int? PartialRestorationCompletedBy { get; set; }

        [Range(0.00, 9999999.99)]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public decimal? PartialRestorationActualCost { get; set; }

        [Range(0, 9999)]
        public int? PartialRestorationTrafficControlCost { get; set; }

        //[RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        [StringLength(Restoration.StringLengths.PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER)]
        public string PartialRestorationTrafficControlInvoiceNumber { get; set; }

        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        [ClientCallback("Restoration.validatePartialRestorationNotes", ErrorMessage = VALIDATION_PARTIAL_NOTES)]
        public string PartialRestorationNotes { get; set; }

        [StringLength(Restoration.StringLengths.PARTIAL_PO_NUM)]
        public string PartialRestorationPurchaseOrderNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationPriorityUpchargeType))]
        public int? PartialRestorationPriorityUpchargeType { get; set; }

        [Min(0)]
        public decimal? PartialRestorationPriorityUpcharge { get; set; }

        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public int? PartialPavingSquareFootage { get; set; }

        [AutoMap(MapDirections.ToPrimary)] // We do not want mapping to unintentionally change this value on the entity.
        public DateTime? PartialRestorationDueDate { get; set; }

        public DateTime? PartialRestorationApprovedAt { get; set; }

        public string PartialRestorationBreakoutBilling { get; set; }

        #endregion

        #region Final Restoration

        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        [ClientCallback("Restoration.validateFinalRestorationNotes", ErrorMessage = VALIDATION_FINAL_NOTES)]
        public string FinalRestorationNotes { get; set; }

        public int? FinalRestorationTrafficControlCost { get; set; }

        [StringLength(Restoration.StringLengths.PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER)]
        public string FinalRestorationTrafficControlInvoiceNumber { get; set; }

        [MultiSelect("FieldOperations", "RestorationMethod", "ByRestorationTypeID", DependsOn = "RestorationType")]
        [EntityMap, EntityMustExist(typeof(RestorationMethod))]
        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public int[] FinalRestorationMethods { get; set; }

        [StringLength(Restoration.StringLengths.FINAL_INVOICE_NUMBER), RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public string FinalRestorationInvoiceNumber { get; set; }

        public DateTime? FinalRestorationDate { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(Contractor))]
        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public int? FinalRestorationCompletedBy { get; set; }

        [StringLength(Restoration.StringLengths.FINAL_PO_NUM)]
        public string FinalRestorationPurchaseOrderNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationPriorityUpchargeType))]
        public int? FinalRestorationPriorityUpchargeType { get; set; }

        [Min(0)]
        public decimal? FinalRestorationPriorityUpcharge { get; set; }

        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public int? FinalPavingSquareFootage { get; set; }

        [Range(0.00, 9999999.99)]
        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public decimal? FinalRestorationActualCost { get; set; }

        [AutoMap(MapDirections.ToPrimary)] // We do not want mapping to unintentionally change this value on the entity.
        public DateTime? FinalRestorationDueDate { get; set; }

        public DateTime? FinalRestorationApprovedAt { get; set; }

        #endregion

        #endregion

        #region Constructors

        public RestorationViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            if (string.IsNullOrWhiteSpace(WBSNumber) && WorkOrder.HasValue)
            {
                var wo = _container.GetInstance<IRepository<WorkOrder>>().Find(WorkOrder.Value);
                WBSNumber = wo.AccountCharged;
                OperatingCenter = wo.OperatingCenter.Id;
                Town = wo.Town.Id;
            }
        }

        public override Restoration MapToEntity(Restoration entity)
        {
            // Defaults to "Standard" if a priority isn't selected.
            // Do this before calling base.MapToEntity so it maps correctly.
            ResponsePriority = ResponsePriority.GetValueOrDefault(RestorationResponsePriority.Indices.STANDARD);

            // InitialPurchaseOrderNumber is to be copied to the other PO # fields if they do not have values.
            if (!string.IsNullOrWhiteSpace(InitialPurchaseOrderNumber))
            {
                PartialRestorationPurchaseOrderNumber = PartialRestorationPurchaseOrderNumber ?? InitialPurchaseOrderNumber;
                FinalRestorationPurchaseOrderNumber = FinalRestorationPurchaseOrderNumber ?? InitialPurchaseOrderNumber;
            }

            var previousContractor = entity.AssignedContractor;

            base.MapToEntity(entity);

            // Bug 2784 says to set the restoration dates to now if it is completed by others.
            // Also if CompletedByOthers is true then a contractor *will not be assigned*.
            if (entity.CompletedByOthers.GetValueOrDefault())
            {
                var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                if (!entity.FinalRestorationDate.HasValue)
                {
                    entity.FinalRestorationDate = now;
                }
                if (!entity.PartialRestorationDate.HasValue)
                {
                    entity.PartialRestorationDate = now;
                }
            }

            // This should return false even after base.MapToEntity call because the AssignedContractorAt value
            // will not have been set if it didn't already have a value.
            else 
            {
                if ((AssignedContractor.HasValue && !entity.HasBeenAssignedToContractor) || entity.AssignedContractor != previousContractor)
                {
                    // We can use entity.AssignedContractor here since base.MapToEntity will 
                    // have already set that property.
                    entity.AssignContractor(entity.AssignedContractor, _container.GetInstance<IDateTimeProvider>().GetCurrentDate());
                }
            }

            // This must happen after the other properties have been mapped.
            entity.TotalAccruedCost = entity.CalculateAccruedCost();

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!EstimatedRestorationFootage.HasValue)
            {
                yield break;
            }

            if (string.IsNullOrWhiteSpace(PartialRestorationNotes) &&
                PartialPavingSquareFootage.GetValueOrDefault() > EstimatedRestorationFootage.Value)
            {
                yield return new ValidationResult(VALIDATION_PARTIAL_NOTES, new[] { "PartialRestorationNotes" });
            }

            if (string.IsNullOrWhiteSpace(FinalRestorationNotes) &&
                FinalPavingSquareFootage.GetValueOrDefault() > EstimatedRestorationFootage.Value)
            {
                yield return new ValidationResult(VALIDATION_FINAL_NOTES, new[] { "FinalRestorationNotes" });
            }
        }

        #endregion
    }

    public class CreateRestoration : RestorationViewModel
    {
        #region Properties

        [DropDown("Contractors", "Contractor", "ActiveContractorsByOperatingCenterId", DependsOn = "OperatingCenter")]
        public override int? AssignedContractor
        {
            get
            {
                return base.AssignedContractor;
            }

            set
            {
                base.AssignedContractor = value;
            }
        }

        #endregion

        #region Constructors

        public CreateRestoration(IContainer container) : base(container) { }

        #endregion

        public void SetDefaultsFromLastRestoration(WorkOrder wo)
        {
            if (wo.Restorations != null && wo.Restorations.Any())
            {
                var lastRestoration = wo.Restorations.OrderByDescending(x => x.Id).First();
                AssignedContractor = lastRestoration.AssignedContractor?.Id;
                WBSNumber = lastRestoration.WBSNumber;
                PartialRestorationPurchaseOrderNumber = lastRestoration.PartialRestorationPurchaseOrderNumber;
                FinalRestorationPurchaseOrderNumber = lastRestoration.FinalRestorationPurchaseOrderNumber;
            }
        }
    }

    public class EditRestoration : RestorationViewModel
    {
        #region Properties

        [DropDown("Contractors", "Contractor", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public override int? AssignedContractor
        {
            get
            {
                return base.AssignedContractor;
            }

            set
            {
                base.AssignedContractor = value;
            }
        }

        #endregion

        #region Constructors

        public EditRestoration(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchRestoration : SearchSet<Restoration>
    {
        [SearchAlias("WorkOrder", "wo", "Id", Required = true)]
        public int? WorkOrder { get; set; }

        [SearchAlias("WorkOrder", "wo", "DateCompleted")]
        public DateRange WorkOrderDateCompleted { get; set; }

        [SearchAlias("OperatingCenter", "Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [SearchAlias("Town", "Id")]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above.")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown("Contractors", "Contractor", "ActiveContractorsByOperatingCenterId", DependsOn = "OperatingCenter", PromptText ="Select an Operating Center above.")]
        [EntityMap, EntityMustExist(typeof(Contractor))]
        public int? AssignedContractor { get; set; }

        public DateRange AssignedContractorAt { get; set; }

        public bool? HasAssignedContractor { get; set; }

        [DropDown("Contractors", "Contractor", "ActiveContractorsByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above.")]
        [EntityMap, EntityMustExist(typeof(Contractor))]
        public int? CreatedByContractor { get; set; }

        public DateRange CreatedByContractorAt { get; set; }

        public string WBSNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public int? RestorationType { get; set; }

        public DateRange PartialRestorationDueDate { get; set; }
        public DateRange FinalRestorationDueDate { get; set; }

        public bool? TrafficControlRequired { get; set; }

        [View("Base/Initial PO #")]
        public virtual string PartialRestorationPurchaseOrderNumber { get; set; }

        [View("Final PO #")]
        public virtual string FinalRestorationPurchaseOrderNumber { get; set; }

        public bool? IsIncomplete { get; set; }

        public bool? FinalRestorationCompleted { get; set; }

        public DateRange PartialRestorationDate { get; set; }
        public DateRange FinalRestorationDate { get; set; }

        public bool? AcknowledgedByContractor { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            var prop = mapper.MappedProperties["IsIncomplete"];

            if (IsIncomplete.HasValue)
            {
                prop.ActualName = "FinalRestorationDate";
                prop.Value = IsIncomplete.Value ? SearchMapperSpecialValues.IsNull : SearchMapperSpecialValues.IsNotNull;
            }
            else
            {
                prop.Value = null;
            }

            if (FinalRestorationCompleted.HasValue)
            {
                var mappedProp = mapper.MappedProperties["FinalRestorationCompleted"];
                mappedProp.ActualName = "FinalRestorationDate";

                if (FinalRestorationCompleted.Value)
                {
                    mappedProp.Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mappedProp.Value = SearchMapperSpecialValues.IsNull;
                }
            }

            var hacProp = mapper.MappedProperties["HasAssignedContractor"];

            if (HasAssignedContractor.HasValue)
            {
                hacProp.ActualName = "AssignedContractor";

                if (HasAssignedContractor.Value)
                {
                    hacProp.Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    hacProp.Value = SearchMapperSpecialValues.IsNull;
                }
            }
            else
            {
                hacProp.Value = null;
            }
        }
    }
}