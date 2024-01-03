using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Vehicle
        : IEntityWithCreationTimeTracking, IThingWithOperatingCenter, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int MAX_VIN = 18,
                             MAX_MODEL_YEAR = 4,
                             MAX_MAKE = 25,
                             MAX_MODEL = 50,
                             MAX_ARI = 6,
                             MAX_PLATE_NUMBER = 10,
                             REQUISITION_NUMBER = 50,
                             EXISTING_VEHICLE_NUMBER = 50,
                             DECAL_NUMBER = 8,
                             VEHICLE_LABEL = 50,
                             ASSET_DETAILS = 255,
                             LEASING_COMPANY = 255,
                             LEASE_TERM = 20,
                             ALV_ID = 50,
                             TOUGHBOOK_SERIAL_NUMBER = 255,
                             TOUGHBOOK_MOUNT = 255,
                             FUEL_CARD_NUMBER = 50,
                             COMMENTS = 255,
                             CREATED_BY = 50,
                             MAX_NEDAP_SERIAL_NUMBER = 50,
                             WBS_NUMBER = 15;
        }

        #endregion

        #region Properties

        // These two properties are columns on the table. They wanted these gone
        // in bug 2712 but I did not delete the columns.
        //  public virtual Employee VehicleAssignedTo { get; set; }
        //  public virtual string ExistingVehicleNumber { get; set; }

        public virtual int Id { get; set; }

        [DisplayName("VIN #")]
        public virtual string VehicleIdentificationNumber { get; set; }

        public virtual string Make { get; set; }
        public virtual string Model { get; set; }
        public virtual string ModelYear { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string PlateNumber { get; set; }

        [DisplayName("Vehicle #")]
        public virtual string ARIVehicleNumber { get; set; }

        [DisplayName("WBS #")]
        public virtual string WBSNumber { get; set; }

        public virtual VehicleAssignmentCategory AssignmentCategory { get; set; }
        public virtual VehicleAssignmentJustification AssignmentJustification { get; set; }
        public virtual VehicleAssignmentStatus AssignmentStatus { get; set; }
        public virtual VehicleAccountingRequirement AccountingRequirement { get; set; }
        public virtual VehicleEZPass EZPass { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual VehicleDepartment Department { get; set; }

        [DisplayName("Vehicle Type")]
        public virtual VehicleType Type { get; set; }

        public virtual Vehicle ReplacementVehicle { get; set; }
        public virtual VehicleStatus Status { get; set; }
        public virtual VehicleIcon VehicleIcon { get; set; }
        public virtual VehicleFuelType FuelType { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual Employee FleetContactPerson { get; set; }
        public virtual Employee PrimaryDriver { get; set; }
        public virtual VehiclePrimaryUse PrimaryVehicleUse { get; set; }
        public virtual bool Flag { get; set; }
        public virtual bool PoolUse { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? DateRequisitioned { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? DateOrdered { get; set; }

        public virtual string RequisitionNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? DateInService { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? DateRetired { get; set; }

        public virtual string DecalNumber { get; set; }
        public virtual string NedapSerialNumber { get; set; }
        public virtual bool LogoWaiver { get; set; }
        public virtual bool? Upbranded { get; set; }
        public virtual string VehicleLabel { get; set; }
        public virtual int? District { get; set; }
        public virtual bool? EmergencyUse { get; set; }
        public virtual float? GVW { get; set; }
        public virtual VehicleServiceCompany ServiceCompany { get; set; }
        public virtual string AssetDetails { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? RegistrationRenewalDate { get; set; }

        public virtual float? RegistrationAnnualCost { get; set; }
        public virtual VehicleOwnershipType OwnershipType { get; set; }
        public virtual string LeasingCompany { get; set; }
        public virtual string LeaseTerm { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? LeaseExpiration { get; set; }

        public virtual float? LeaseCostMth { get; set; }
        public virtual float? OriginalAssetValueCapCost { get; set; }
        public virtual int? PlannedReplacementYear { get; set; }
        public virtual string AlvId { get; set; }

        // [DisplayName("GPS Type")]
        public virtual VehicleGPSType GPSType { get; set; }
        public virtual string ToughbookSerialNumber { get; set; }
        public virtual string ToughbookMount { get; set; }
        public virtual string FuelCardNumber { get; set; }
        public virtual bool? MileageTracked { get; set; }
        public virtual string Comments { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual IList<VehicleAudit> Audits { get; set; }
        public virtual IList<Incident> Incidents { get; set; }

        public virtual IList<Document<Vehicle>> Documents { get; set; }
        public virtual IList<Note<Vehicle>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => "Vehicles";

        [DoesNotExport]
        public virtual string Description =>
            string.Format("{0} : {1} - {2} {3} {4} - {5}",
                ARIVehicleNumber,
                VehicleIdentificationNumber,
                ModelYear,
                Make,
                Model,
                PlateNumber);

        #endregion

        #endregion

        #region Constructor

        public Vehicle()
        {
            Audits = new List<VehicleAudit>();
            Incidents = new List<Incident>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    [Serializable]
    public class VehicleAccountingRequirement : EntityLookup { }

    [Serializable]
    public class VehicleAssignmentCategory : EntityLookup { }

    [Serializable]
    public class VehicleAssignmentStatus : EntityLookup { }

    [Serializable]
    public class VehicleAssignmentJustification : EntityLookup { }

    [Serializable]
    public class VehicleDepartment : EntityLookup { }

    [Serializable]
    public class VehicleFuelType : EntityLookup { }

    [Serializable]
    public class VehicleGPSType : EntityLookup { }

    [Serializable]
    public class VehicleIcon : EntityLookup { }

    [Serializable]
    public class VehicleOwnershipType : EntityLookup { }

    [Serializable]
    public class VehiclePrimaryUse : EntityLookup { }

    [Serializable]
    public class VehicleServiceCompany : EntityLookup { }

    [Serializable]
    public class VehicleStatus : EntityLookup { }

    [Serializable]
    public class VehicleType : EntityLookup { }
}
