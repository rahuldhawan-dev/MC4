using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Contractors.Data.Models.Repositories;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public class RestorationViewModel : ViewModel<Restoration>
    {
        #region Consts

        protected const string VALIDATION_PARTIAL_NOTES = "Partial restoration notes are required when actual square footage is greater than estimated square footage.";
        protected const string VALIDATION_FINAL_NOTES = "Final restoration notes are required when actual square footage is greater than estimated square footage.";

        #endregion

        #region Fields

        private Restoration _display;

        #endregion

        #region Properties

        #region Initial Data

        [EntityMap(MapDirections.None)]
        public Restoration Display
        {
            get
            {
                if (_display == null)
                {
                    _display = _container.GetInstance<IRepository<Restoration>>().Find(Id);
                }
                return _display;
            }
        }

        public bool? TrafficControlRequired { get; set; }
        
        // This is a checkbox, not a dropdown.
        [CheckBox]
        public bool AcknowledgedByContractor { get; set; }

        #endregion

        #region Partial / Temporary Restoration

        [MultiSelect("RestorationMethod", "ByRestorationTypeID", DependsOn = "RestorationType")]
        [EntityMap(MapDirections.ToViewModel), EntityMustExist(typeof(RestorationMethod))]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public int[] PartialRestorationMethods { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        [StringLength(Restoration.StringLengths.PARTIAL_INVOICE_NUMBER), RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public string PartialRestorationInvoiceNumber { get; set; }

        public DateTime? PartialRestorationDate { get; set; }

        [StringLengthNotRequired]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        [ClientCallback("Restoration.validatePartialRestorationNotes", ErrorMessage = VALIDATION_PARTIAL_NOTES)]
        public string PartialRestorationNotes { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public int? PartialPavingSquareFootage { get; set; }

        [Range(0.00, 9999999.99)]
        [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public decimal? PartialRestorationActualCost { get; set; }

        [Range(0, 9999)]
        public int? PartialRestorationTrafficControlCost { get; set; }

        [StringLength(Restoration.StringLengths.PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER)]
       // [RequiredWhen("PartialRestorationDate", ComparisonType.NotEqualTo, null)]
        public string PartialRestorationTrafficControlInvoiceNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationPriorityUpchargeType))]
        public int? PartialRestorationPriorityUpchargeType { get; set; }

        [Min(0)]
        public decimal? PartialRestorationPriorityUpcharge { get; set; }

        #endregion

        #region Final Restoration

        [StringLengthNotRequired]
        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        [ClientCallback("Restoration.validateFinalRestorationNotes", ErrorMessage = VALIDATION_FINAL_NOTES)]
        public string FinalRestorationNotes { get; set; }

        [Min(0)]
        public int? FinalRestorationTrafficControlCost { get; set; }

        [StringLength(Restoration.StringLengths.PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER)]
        public string FinalRestorationTrafficControlInvoiceNumber { get; set; }

        [MultiSelect("RestorationMethod", "ByRestorationTypeID", DependsOn = "RestorationType")]
        [EntityMap(MapDirections.ToViewModel), EntityMustExist(typeof(RestorationMethod))]
        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public int[] FinalRestorationMethods { get; set; }

        [StringLength(Restoration.StringLengths.FINAL_INVOICE_NUMBER), RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public string FinalRestorationInvoiceNumber { get; set; }

        public DateTime? FinalRestorationDate { get; set; }

        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public int? FinalPavingSquareFootage { get; set; }

        [StringLength(Restoration.StringLengths.FINAL_PO_NUM)]
        public string FinalRestorationPurchaseOrderNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationPriorityUpchargeType))]
        public int? FinalRestorationPriorityUpchargeType { get; set; }

        [Min(0)]
        public decimal? FinalRestorationPriorityUpcharge { get; set; }

        [Range(0.00, 9999999.99)]
        [RequiredWhen("FinalRestorationDate", ComparisonType.NotEqualTo, null)]
        public decimal? FinalRestorationActualCost { get; set; }

        #endregion

        #endregion

        #region Constructors

        public RestorationViewModel(IContainer container) : base(container) { }

        #endregion

        #region Mapping

        public override Restoration MapToEntity(Restoration entity)
        {
            // Bug 2784: Set the CompletedBy fields to the current contractor when 
            //           they set the restoration date.

            var currentContractor = _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Contractor;
            var setPartialCompletedBy = (PartialRestorationDate.HasValue && entity.PartialRestorationCompletedBy == null);
            var setFinalCompletedBy = (FinalRestorationDate.HasValue && entity.FinalRestorationCompletedBy == null);

            base.MapToEntity(entity);

            if (setPartialCompletedBy)
            {
                entity.PartialRestorationCompletedBy = currentContractor;
            }

            if (setFinalCompletedBy)
            {
                entity.FinalRestorationCompletedBy = currentContractor;
            }

            // Certain fields are only mappable when the partial or final restoration has not been approved yet.
            var methodRepo = _container.GetInstance<IRestorationMethodRepository>();
            var isPartialApproved = entity.PartialRestorationApprovedAt.HasValue;
            if (!isPartialApproved)
            {
                entity.PartialPavingSquareFootage = PartialPavingSquareFootage;
                entity.PartialRestorationInvoiceNumber = PartialRestorationInvoiceNumber;
                entity.PartialRestorationActualCost = PartialRestorationActualCost;
                entity.PartialRestorationDate = PartialRestorationDate;
                entity.PartialRestorationPriorityUpcharge = PartialRestorationPriorityUpcharge;

                entity.PartialRestorationMethods.Clear();

                if (PartialRestorationMethods != null)
                {
                    foreach (var prm in PartialRestorationMethods)
                    {
                        entity.PartialRestorationMethods.Add(methodRepo.Find(prm));
                    }
                }
            }

            var isFinalApproved = entity.FinalRestorationApprovedAt.HasValue;
            if (!isFinalApproved)
            {
                entity.FinalPavingSquareFootage = FinalPavingSquareFootage;
                entity.FinalRestorationInvoiceNumber = FinalRestorationInvoiceNumber;
                entity.FinalRestorationActualCost = FinalRestorationActualCost;
                entity.FinalRestorationDate = FinalRestorationDate;
                entity.FinalRestorationPriorityUpcharge = FinalRestorationPriorityUpcharge;

                entity.FinalRestorationMethods.Clear();

                if (FinalRestorationMethods != null)
                {
                    foreach (var prm in FinalRestorationMethods)
                    {
                        entity.FinalRestorationMethods.Add(methodRepo.Find(prm));
                    }
                }
            }
            
            return entity;
        }

        #endregion

        #region Validation

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!EstimatedRestorationFootage.HasValue)
        //    {
        //        yield break;
        //    }

        //    if (string.IsNullOrWhiteSpace(PartialRestorationNotes) &&
        //        PartialPavingSquareFootage.GetValueOrDefault() > EstimatedRestorationFootage.Value)
        //    {
        //        yield return new ValidationResult(VALIDATION_PARTIAL_NOTES, new[] { "PartialRestorationNotes" });
        //    }

        //    if (string.IsNullOrWhiteSpace(FinalRestorationNotes) &&
        //        FinalPavingSquareFootage.GetValueOrDefault() > EstimatedRestorationFootage.Value)
        //    {
        //        yield return new ValidationResult(VALIDATION_FINAL_NOTES, new[] { "FinalRestorationNotes" });
        //    }
        //}

        #endregion
    }

    public class CreateRestoration : RestorationViewModel
    {
        #region Fields

        private WorkOrder _displayWorkOrder;

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [EntityMap(MapDirections.None)]
        public WorkOrder DisplayWorkOrder
        {
            get
            {
                if (_displayWorkOrder == null)
                {
                    _displayWorkOrder = _container.GetInstance<IRepository<WorkOrder>>().Find(WorkOrder.Value);
                }
                return _displayWorkOrder;
            }
        }


        [Required, DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public int? RestorationType { get; set; }

        [Required]
        public decimal? EstimatedRestorationFootage { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(RestorationResponsePriority))]
        public int? ResponsePriority { get; set; }

        [StringLengthNotRequired]
        public string RestorationNotes { get; set; }

        #endregion

        #region Constructors

        public CreateRestoration(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Restoration MapToEntity(Restoration entity)
        {
            // Bug #2784: According to Jeff, when a restoration is created via the contractors site, the user is
            //            entering it after the work has been completed. This is entirely different from MapCall
            //            where the the restoration is entered before work is started or assigned. 

            // Bug #3366: According to Jeff, restorations are created on the contractors site via "asset contractors"
            //            that are apparently different from "restoration contractors". The asset contractors are not
            //            the ones being assigned the restoration. NJAW later goes in via MapCall and assigns them to 
            //            a restoration contractor. 
            base.MapToEntity(entity);

            entity.CreatedByContractor = _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Contractor;
            entity.CreatedByContractorAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.OperatingCenter = DisplayWorkOrder.OperatingCenter;
            entity.WBSNumber = DisplayWorkOrder.AccountCharged;
            entity.Town = DisplayWorkOrder.Town;


            // This must happen after all other mapping. Also this is only done in Create mode here
            // because all of the other fields related to calculating this cost are not editable on
            // the contractors portal.
            entity.TotalAccruedCost = entity.CalculateAccruedCost();

            // Bug #2784: Due to contractors entering restorations when they're completed, setting related restoration info
            //            would most likely cause issues with already completed related restorations by messing up their info.
            //            So that isn't being done here like it is done in MVC.

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

    public class EditRestoration : RestorationViewModel
    {
        #region Properties

        // Needed for the RestorationMethod dropdowns. hidden field on edit page.
        [Required, DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public int? RestorationType { get; set; }

        #endregion

        #region Constructors

        public EditRestoration(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Display.EstimatedRestorationFootage.HasValue)
            {
                yield break;
            }

            if (string.IsNullOrWhiteSpace(PartialRestorationNotes) &&
                PartialPavingSquareFootage.GetValueOrDefault() > Display.EstimatedRestorationFootage.Value)
            {
                yield return new ValidationResult(VALIDATION_PARTIAL_NOTES, new[] { "PartialRestorationNotes" });
            }

            if (string.IsNullOrWhiteSpace(FinalRestorationNotes) &&
                FinalPavingSquareFootage.GetValueOrDefault() > Display.EstimatedRestorationFootage.Value)
            {
                yield return new ValidationResult(VALIDATION_FINAL_NOTES, new[] { "FinalRestorationNotes" });
            }
        }

        #endregion
    }

    [StringLengthNotRequired]
    public class SearchRestoration : SearchSet<Restoration>
    {
        [SearchAlias("WorkOrder", "wo", "Id")]
        public int? WorkOrder { get; set; }

        [SearchAlias("OperatingCenter", "Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [SearchAlias("Town", "Id")]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        public DateRange AssignedContractorAt { get; set; }

        public string WBSNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public int? RestorationType { get; set; }

        public DateRange PartialRestorationDueDate { get; set; }
        public DateRange FinalRestorationDueDate { get; set; }

        public bool? TrafficControlRequired { get; set; }

        [View("Base/Initial PO #")]
        public string PartialRestorationPurchaseOrderNumber { get; set; }

        [View("Final PO #")]
        public string FinalRestorationPurchaseOrderNumber { get; set; }

        //[Search(CanMap =false)]
        public bool? FinalRestorationCompleted { get; set; }

        public bool? AcknowledgedByContractor { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
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

            base.ModifyValues(mapper);
        }
    }
}