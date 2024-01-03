using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public class EditMeterChangeOut : ViewModel<MeterChangeOut>
    {
        #region Fields

        private MeterChangeOut _original;

        #endregion

        #region Constructors

        public EditMeterChangeOut(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public MeterChangeOut Display
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IMeterChangeOutRepository>().Find(Id);
                }
                return _original;
            }
        }

        [StringLength(MeterChangeOut.StringLengths.SERVICE_PHONE)]
        public string ServicePhone { get; set; }

        [StringLength(MeterChangeOut.StringLengths.SERVICE_PHONE_EXTENSION)]
        [View(DisplayName = "Ext")]
        public string ServicePhoneExtension { get; set; }

        [StringLength(MeterChangeOut.StringLengths.SERVICE_PHONE)]
        public string ServicePhone2 { get; set; }

        [StringLength(MeterChangeOut.StringLengths.SERVICE_PHONE_EXTENSION)]
        [View(DisplayName = "Ext2")]
        public string ServicePhone2Extension { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CustomerMeterLocation))]
        [View(DisplayName = "Owner Location")]
        public int? OwnerCustomerMeterLocation { get; set; }

        [StringLengthNotRequired]
        public string FieldNotes { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterChangeOutWorkScope))]
        public virtual int? WorkScope { get; set; }

        [RequiredWhenScheduled(ErrorMessage = "The Date Scheduled field is required when the change out status is set to scheduled.")]
        public DateTime? DateScheduled { get; set; }

        [RequiredWhen("MeterChangeOutStatus", ComparisonType.EqualToAny, "GetNotScheduledLVMOrVaultStatuses",
            typeof(EditMeterChangeOut),
            ErrorMessage = "The Date Status Changed is required when the change out status is not scheduled, lvm, or vault")]
        public DateTime? DateStatusChanged { get; set; }

        [RequiredWhenScheduled(ErrorMessage = "The Scheduled Time field is required when the change out status is set to scheduled.")]
        [DropDown, EntityMap, EntityMustExist(typeof(MeterScheduleTime))]
        public int? MeterScheduleTime { get; set; }

        [RequiredWhenScheduled(ErrorMessage = "The Assigned Contractor Meter Crew field is required when the change out status is set to scheduled.")]
        [DropDown, EntityMap, EntityMustExist(typeof(ContractorMeterCrew))]
        [View(DisplayName = "Assigned Tech")]
        public int? AssignedContractorMeterCrew { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterChangeOutStatus))]
        public int? MeterChangeOutStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorMeterCrew))]
        public int? CalledInByContractorMeterCrew { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Out Read must be numeric")]
        public string OutRead { get; set; }

        // bug 3405: validate that these are digits even though they are strings.
        [RegularExpression("^[0-9]*$", ErrorMessage = "New Serial Number must be numeric")]
        [StringLength(MeterChangeOut.StringLengths.NEW_SERIAL_NUMBER, MinimumLength = 8)]
        public string NewSerialNumber { get; set; }

        // bug 3405: validate that these are digits even though they are strings.
        [RegularExpression("^[0-9]*$", ErrorMessage = "New RF Number must be numeric")]
        [StringLength(MeterChangeOut.StringLengths.NEW_RF_NUMBER, MinimumLength = MeterChangeOut.StringLengths.NEW_RF_NUMBER)]
        public string NewRFNumber { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Start Read must be numeric")]
        public string StartRead { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterDirection))]
        public int? MeterDirection { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SmallMeterLocation))]
        public int? MeterLocation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterSupplementalLocation))]
        public int? MeterSupplementalLocation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterDirection))]
        public int? RFDirection { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SmallMeterLocation))]
        public int? RFLocation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterSupplementalLocation))]
        public int? RFSupplementalLocation { get; set; }

        [StringLength(MeterChangeOut.StringLengths.METER_READ_COMMENT)]
        public string MeterReadComment4 { get; set; }

        public bool? IsNeptuneRFOnly { get; set; }
        public bool? IsHotRodRFOnly { get; set; }
        [RequiredWhen("MeterChangeOutStatus", ComparisonType.EqualToAny, "GetNotScheduledLVMOrVaultStatuses",
            typeof(EditMeterChangeOut),
            ErrorMessage = "The Date Status Changed is required when the change out status is not scheduled, lvm, or vault")]
        public bool? OperatedPointOfControlAtAnyValve { get; set; }
        public bool? IsMuellerMeter { get; set; }
        [RequiredWhen("MeterChangeOutStatus", ComparisonType.EqualToAny, "GetNotScheduledLVMOrVaultStatuses",
            typeof(EditMeterChangeOut),
            ErrorMessage = "The Date Status Changed is required when the change out status is not scheduled, lvm, or vault")]
        [DropDown, EntityMap, EntityMustExist(typeof(WaterServiceStatus))]
        public int? StartStatus { get; set; }
        [RequiredWhen("MeterChangeOutStatus", ComparisonType.EqualToAny, "GetNotScheduledLVMOrVaultStatuses",
            typeof(EditMeterChangeOut),
            ErrorMessage = "The Date Status Changed is required when the change out status is not scheduled, lvm, or vault")]
        [DropDown, EntityMap, EntityMustExist(typeof(WaterServiceStatus))]
        public int? EndStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TypeOfPlumbing))]
        public int? TypeOfPlumbing { get; set; }

        public bool? TurnedOffWater { get; set; }
        public bool? MeterTurnedOnAfterHours { get; set; }
        public bool? MovedMeterToPit { get; set; }
        public bool? RanNewWire { get; set; }
        public bool? InstalledJumperBar { get; set; }
        public bool? ContractorDrilledLid { get; set; }
        public bool? PartialExcavation { get; set; }
        public bool? CutBolts { get; set; }
        public bool? Canvassed { get; set; }
        public bool? ClickAdvantexUpdated { get; set; }
        [StringLength(MeterChangeOut.StringLengths.SAP_ORDER_NUMBER)]
        public string SAPOrderNumber { get; set; }

        #endregion

        #region Public Methods

        public static int[] GetNotScheduledLVMOrVaultStatuses()
        {
            return DependencyResolver.Current.GetService<IMeterChangeOutStatusRepository> ().GetNotScheduledLVMOrVaultStatuses();
        }

        public override MeterChangeOut MapToEntity(MeterChangeOut entity)
        {
            base.MapToEntity(entity);

            // Because we have an int field that needs to pretend is a string, and ViewModel does NOT convert
            // ints and strings, we have to do it manually.
            entity.OutRead = !string.IsNullOrWhiteSpace(OutRead) ? Convert.ToInt32(OutRead) : (int?)null;
            entity.StartRead = !string.IsNullOrWhiteSpace(StartRead) ? Convert.ToInt32(StartRead) : (int?)null;
            return entity;
        }

        public override void Map(MeterChangeOut entity)
        {
            base.Map(entity);
            // Because we have an int field that needs to pretend is a string, and ViewModel does NOT convert
            // ints and strings, we have to do it manually.
            this.OutRead = Convert.ToString(entity.OutRead);
            this.StartRead = Convert.ToString(entity.StartRead);
        }

        #endregion

        #region Helper Validation

        [StringLengthNotRequired] // This gets caught by the string length test for some reason
        private class RequiredWhenScheduledAttribute : RequiredWhenAttribute
        {
            public RequiredWhenScheduledAttribute() : base("MeterChangeOutStatus", "GetScheduledStatus", typeof(RequiredWhenScheduledAttribute)) { }

            public static int GetScheduledStatus()
            {
                return MapCall.Common.Model.Entities.MeterChangeOutStatus.Indices.SCHEDULED;
            }
        }

        #endregion
    }

    [StringLengthNotRequired]
    public class SearchMeterChangeOutCompletions : SearchSet<MeterChangeOutCompletionReportItem>, ISearchMeterChangeOutCompletions
    {
        [DropDown, EntityMap, EntityMustExist(typeof(ContractorMeterCrew))]
        public int? CalledInByContractorMeterCrew { get; set; }
        public DateRange DateStatusChanged { get; set; }
        [MultiSelect, EntityMap, EntityMustExist(typeof(MeterChangeOutStatus))]
        public int[] MeterChangeOutStatus { get; set; }
        [SearchAlias("Contract", "contract", "OperatingCenter.Id")]
        [MultiSelect, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }
    }

    // ComboBox - Operating Center, Project Year, ChangeOutStatus, MeterSizes

    [StringLengthNotRequired]
    public class SearchMeterChangeOut : SearchSet<MeterChangeOut>
    {
        [View("Id")]
        public IntRange EntityId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterChangeOutContract))]
        public int? Contract { get; set; }

        // This alias is different from the one in MapCallMVC because of the Criteria property override
        // in MeterChangeOutRepository.
        [SearchAlias("Contract", "mcoc", "OperatingCenter.Id")]
        [MultiSelect, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? ServiceCity { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(MeterChangeOutStatus))]
        public int[] MeterChangeOutStatus { get; set; }

        public string ServiceStreet { get; set; }
        public string ServiceStreetNumber { get; set; }
        [View(DisplayName = "Service Street Address")]
        public string ServiceStreetAddressCombined { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int[] MeterSize { get; set; }
        public string RemovedSerialNumber { get; set; }
        public string NewSerialNumber { get; set; }
        public string MeterReadComment4 { get; set; }
        public bool? ClickAdvantexUpdated { get; set; }

        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorMeterCrew))]
        public int? AssignedContractorMeterCrew { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorMeterCrew))]
        public int? CalledInByContractorMeterCrew { get; set; }

        public DateRange DateScheduled { get; set; }
        public DateRange DateStatusChanged { get; set; }

        public bool? HasStatus { get; set; }
        public bool? MeterTurnedOnAfterHours { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CustomerMeterLocation))]
        public int? OwnerCustomerMeterLocation { get; set; }

        public string SAPOrderNumber { get; set; }

        [MultiString]
        public string[] RouteNumber { get; set; }

        [Search(CanMap = false)]
        [View(Description = "Determines the sorting when exporting to PDF")]
        public bool IsUnscheduledSearch { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (HasStatus.HasValue)
            {
                var mappedProp = mapper.MappedProperties["HasStatus"];
                mappedProp.ActualName = "MeterChangeOutStatus";

                if (HasStatus.Value)
                {
                    mappedProp.Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mappedProp.Value = SearchMapperSpecialValues.IsNull;
                }
            }
        }

        [View(Description = "Marks the ClickAdvantexUpdated to the selected value when exporting.")]
        [BoolFormat("True", "False")]
        [Search(CanMap = false)]
        public bool? MarkAdvantexExport { get; set; }

        public string ProjectYear { get; set; }

        public string ServicePhone { get; set; }
        public string ServicePhone2 { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TypeOfPlumbing))]
        public int? TypeOfPlumbing { get; set; }

        #region Constructors

        public SearchMeterChangeOut()
        {

        }

        #endregion
    }
}