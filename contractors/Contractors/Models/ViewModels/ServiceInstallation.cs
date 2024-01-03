using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    [StringLengthNotRequired]
    public class SearchServiceInstallation : SearchSet<ServiceInstallation>
    {
        public int? WorkOrder { get; set; }

        // SearchAlias "o" needs to match the "o" for WorkOrder that's setup
        // in the ServiceInsllationRepository.
        [DropDown, SearchAlias("o.OperatingCenter", "Id"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
    }

    public class ServiceInstallationViewModel : ViewModel<ServiceInstallation>
    {
        #region Properties

        // Service Installations can be made without a workorder?
        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }
        [Required, StringLength(ServiceInstallation.StringLengths.METER_MANUFACTURER_SERIAL_NUMBER)]
        public string MeterManufacturerSerialNumber { get; set; }
        [StringLength(ServiceInstallation.StringLengths.MANUFACTURER)]
        public string Manufacturer { get; set; }
        [StringLength(ServiceInstallation.StringLengths.METER_SIZE)]
        public string MeterSize { get; set; }
        [StringLength(ServiceInstallation.StringLengths.SERVICE_TYPE)]
        public string ServiceType { get; set; }
        [StringLength(ServiceInstallation.StringLengths.METER_SERIAL_NUMBER)]
        public string MeterSerialNumber { get; set; }
        [StringLength(ServiceInstallation.StringLengths.MATERIAL_NUMBER)]
        public string MaterialNumber { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(MeterSupplementalLocation))]
        public int? MeterLocation { get; set; }
        [Required, DropDown(DependsOn = "MeterLocation", Action = "ByMeterSupplementalLocationId", Controller = "SmallMeterLocation", Area = "FieldOperations"), EntityMap, EntityMustExist(typeof(SmallMeterLocation))]
        public int? MeterPositionalLocation { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(MeterDirection))]
        public int? MeterDirectionalLocation { get; set; }
        public virtual string MeterDeviceCategory { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(MeterSupplementalLocation))]
        public int? ReadingDeviceSupplemental { get; set; }
        [Required, DropDown(DependsOn = "ReadingDeviceSupplemental", Action = "ByMeterSupplementalLocationId", Controller = "SmallMeterLocation", Area = "FieldOperations"), EntityMap, EntityMustExist(typeof(SmallMeterLocation))]
        public int? ReadingDevicePosition { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(MeterDirection))]
        public int? ReadingDeviceDirectionalInformation { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_DIALS)]
        public virtual string Register1Dials { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_UNIT_OF_MEASURE)]
        public virtual string Register1UnitOfMeasure { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationReadType))]
        public int? Register1ReadType { get; set; }
        [Required, StringLength(ServiceInstallation.StringLengths.REGISTER_MIU)]
        public virtual string Register1RFMIU { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_SIZE)]
        public virtual string Register1Size { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_ENCODER)]
        public virtual string Register1TPEncoderID { get; set; }
        [Required, StringLength(ServiceInstallation.StringLengths.REGISTER_CURRENT_READ)]
        [ClientCallback("ServiceInstallation.validateReading1", ErrorMessage = "Please enter a numeric reading with leading zeros that matches the number of dials.")]
        public virtual string Register1CurrentRead { get; set; }
        public virtual string Register1DeviceCategory { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_DIALS)]
        public virtual string RegisterTwoDials { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_UNIT_OF_MEASURE)]
        public virtual string Register2UnitOfMeasure { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationReadType))]
        [RequiredWhen("RegisterTwoDials", ComparisonType.NotEqualTo, null)]
        public virtual int? Register2ReadType { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_MIU)]
        [RequiredWhen("RegisterTwoDials", ComparisonType.NotEqualTo, null)]
        public virtual string Register2RFMIU { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_SIZE)]
        public virtual string Register2Size { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_ENCODER)]
        public virtual string Register2TPEncoderID { get; set; }
        [StringLength(ServiceInstallation.StringLengths.REGISTER_CURRENT_READ)]
        [RequiredWhen("RegisterTwoDials", ComparisonType.NotEqualTo, null)]
        [ClientCallback("ServiceInstallation.validateReading2", ErrorMessage = "Please enter a numeric reading with leading zeros that matches the number of dials.")]
        public virtual string Register2CurrentRead { get; set; }
        public virtual string Register2DeviceCategory { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationFirstActivity))]
        public int? Activity1 { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationSecondActivity))]
        public int? Activity2 { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationThirdActivity))]
        public int? Activity3 { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationWorkType))]
        public int? AdditionalWorkNeeded { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(SAPWorkOrderPurpose))]
        public int? Purpose { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationPosition))]
        public int? ServiceFound { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationPosition))]
        public int? ServiceLeft { get; set; }
        [Required]
        public bool? OperatedPointOfControl { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationReason))]
        public int? ServiceInstallationReason { get; set; }
        [Required, Multiline]
        public string MeterLocationInformation { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(MiuInstallReasonCode))]
        public int? MiuInstallReason { get; set; }

        #endregion

        #region Constructors

        public ServiceInstallationViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateServiceInstallation : ServiceInstallationViewModel
    {
        #region Constructors

        public CreateServiceInstallation(IContainer container) : base(container) { }

        #endregion
    }

    public class EditServiceInstallation : ServiceInstallationViewModel
    {
        #region Constructors

        public EditServiceInstallation(IContainer container) : base(container) { }

        #endregion
    }
}