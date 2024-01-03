using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FleetManagement.Models
{
    public abstract class VehicleViewModel : ViewModel<Vehicle>
    {
        #region Properties

        [StringLength(Vehicle.StringLengths.MAX_VIN)]
        public string VehicleIdentificationNumber { get; set; }

        [StringLength(Vehicle.StringLengths.MAX_MAKE)]
        public string Make { get; set; }

        // This property can't be named "Model". The value gets ignored for some reason.
        [DisplayName("Model")]
        [StringLength(Vehicle.StringLengths.MAX_MODEL)]
        [AutoMap("Model")]
        public string ModelType { get; set; }

        [StringLength(Vehicle.StringLengths.MAX_MODEL_YEAR)]
        public string ModelYear { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [StringLength(Vehicle.StringLengths.MAX_PLATE_NUMBER)]
        public string PlateNumber { get; set; }

        [StringLength(Vehicle.StringLengths.MAX_ARI)]
        public string ARIVehicleNumber { get; set; }

        [StringLength(Vehicle.StringLengths.WBS_NUMBER)]
        public string WBSNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentCategory))]
        public int? AssignmentCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentJustification))]
        public int? AssignmentJustification { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentStatus))]
        public int? AssignmentStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAccountingRequirement))]
        public int? AccountingRequirement { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleEZPass))]
        public int? EZPass { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleDepartment))]
        public int? Department { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleType))]
        public int? Type { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Vehicle))]
        public int? ReplacementVehicle { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleStatus))]
        public int? Status { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleIcon))]
        public int? VehicleIcon { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleFuelType))]
        public int? FuelType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? Manager { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? FleetContactPerson { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn="OperatingCenter"), EntityMap, EntityMustExist(typeof(Employee))]
        public int? PrimaryDriver { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehiclePrimaryUse))]
        public int? PrimaryVehicleUse { get; set; }

        [Required]
        public bool? Flag { get; set; }

        [Required]
        public bool? PoolUse { get; set; }

        public DateTime? DateRequisitioned { get; set; }
        public DateTime? DateOrdered { get; set; }

        [StringLength(Vehicle.StringLengths.REQUISITION_NUMBER)]
        public string RequisitionNumber { get; set; }
        public DateTime? DateInService { get; set; }
        public DateTime? DateRetired { get; set; }

        [StringLength(Vehicle.StringLengths.DECAL_NUMBER)]
        public string DecalNumber { get; set; }

        [StringLength(Vehicle.StringLengths.MAX_NEDAP_SERIAL_NUMBER)]
        public string NedapSerialNumber { get; set; }
        public bool LogoWaiver { get; set; }
        public bool? Upbranded { get; set; }

        [StringLength(Vehicle.StringLengths.VEHICLE_LABEL)]
        public string VehicleLabel { get; set; }
        public int? District { get; set; }
        public bool? EmergencyUse { get; set; }
        public float? GVW { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleServiceCompany))]
        public int? ServiceCompany { get; set; }

        [StringLength(Vehicle.StringLengths.ASSET_DETAILS)]
        public string AssetDetails { get; set; }
        public DateTime? RegistrationRenewalDate { get; set; }
        public float? RegistrationAnnualCost { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleOwnershipType))]
        public int? OwnershipType { get; set; }

        [StringLength(Vehicle.StringLengths.LEASING_COMPANY)]
        public string LeasingCompany { get; set; }

        [StringLength(Vehicle.StringLengths.LEASE_TERM)]
        public string LeaseTerm { get; set; }
        public DateTime? LeaseExpiration { get; set; }
        public float? LeaseCostMth { get; set; }
        public float? OriginalAssetValueCapCost { get; set; }
        public int? PlannedReplacementYear { get; set; }

        [DisplayName("ALV ID")]
        [StringLength(Vehicle.StringLengths.ALV_ID)]
        public string AlvId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleGPSType))]
        public int? GPSType { get; set; }

        [StringLength(Vehicle.StringLengths.TOUGHBOOK_SERIAL_NUMBER)]
        public string ToughbookSerialNumber { get; set; }

        [StringLength(Vehicle.StringLengths.TOUGHBOOK_MOUNT)]
        public string ToughbookMount { get; set; }

        [StringLength(Vehicle.StringLengths.FUEL_CARD_NUMBER)]
        public string FuelCardNumber { get; set; }
        public bool? MileageTracked { get; set; }

        [StringLength(Vehicle.StringLengths.COMMENTS)]
        public string Comments { get; set; }
        public DateTime? CreatedAt { get; set; }

        #endregion

        #region Constructors

        protected VehicleViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateVehicle : VehicleViewModel
    {
        public CreateVehicle(IContainer container) : base(container) { }

        public override Vehicle MapToEntity(Vehicle entity)
        {
            base.MapToEntity(entity);
            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            return entity;
        }
    }

    public class EditVehicle : VehicleViewModel
    {
        public EditVehicle(IContainer container) : base(container) { }
    }

    public class SearchVehicle : SearchSet<Vehicle>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        // Needs to map to Id
        public int? VehicleId { get; set; }

        public string VehicleLabel { get; set; }
        public string ARIVehicleNumber { get; set; }
        public string DecalNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentCategory))]
        public int? AssignmentCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentJustification))]
        public int? AssignmentJustification { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleDepartment))]
        public int? Department { get; set; }
        public bool? EmergencyUse { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleType))]
        public int? Type { get; set; }
        public string PlateNumber { get; set; }
        public string VehicleIdentificationNumber { get; set; }
        public bool? Flag { get; set; }
        public int? PlannedReplacementYear { get; set; }
        public DateRange DateRequisitioned { get; set; }
        public DateRange DateOrdered { get; set; }
        public DateRange DateInService { get; set; }
        public DateRange DateRetired { get; set; }
        public bool? PoolUse { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAssignmentStatus))]
        public int? AssignmentStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(VehicleAccountingRequirement))]
        public int? AccountingRequirement { get; set; }
        public string FuelCardNumber { get; set; }
        public string WBSNumber { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (VehicleId.HasValue)
            {
                mapper.MappedProperties["VehicleId"].ActualName = "Id";
            }
        
        }
    }

    public class CreateVehicleAudit : ViewModel<Vehicle>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public int? Mileage { get; set; }

        [Required, DoesNotAutoMap]
        public DateTime? AuditedOn { get; set; }

        [Required, StringLength(VehicleAudit.StringLengths.DECAL_NUMBER)]
        public string DecalNumber { get; set; }
        
        [Required, StringLength(VehicleAudit.StringLengths.PLATE_NUMBER)]
        public string PlateNumber { get; set; }

        #endregion

        #region Constructors

        public CreateVehicleAudit(IContainer container) : base(container) { }

        #endregion

        public override Vehicle MapToEntity(Vehicle entity)
        {
            // Don't call base MapToEntity.

            var audit = new VehicleAudit {
                AuditedOn = AuditedOn.Value,
                DecalNumber = DecalNumber,
                Mileage = Mileage.Value,
                PlateNumber = PlateNumber,
                Vehicle = entity
            };

            entity.Audits.Add(audit);
            return entity;
        }
    }
}