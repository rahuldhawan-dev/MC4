using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class VehicleMap : ClassMap<Vehicle>
    {
        public VehicleMap()
        {
            Id(x => x.Id, "VehicleID");

            Map(x => x.Make)
               .Nullable()
               .Length(Vehicle.StringLengths.MAX_MAKE);
            Map(x => x.Model)
               .Nullable()
               .Length(Vehicle.StringLengths.MAX_MODEL);
            Map(x => x.ModelYear)
               .Nullable()
               .Length(Vehicle.StringLengths.MAX_MODEL_YEAR);
            Map(x => x.PlateNumber)
               .Nullable()
               .Length(Vehicle.StringLengths.MAX_PLATE_NUMBER);
            Map(x => x.VehicleIdentificationNumber, "VINNumber")
               .Nullable()
               .Length(Vehicle.StringLengths.MAX_VIN);
            Map(x => x.ARIVehicleNumber).Nullable()
                                        .Length(Vehicle.StringLengths.MAX_ARI);
            Map(x => x.Flag).Not.Nullable();
            Map(x => x.PoolUse).Not.Nullable();
            Map(x => x.LogoWaiver).Not.Nullable();
            Map(x => x.NedapSerialNumber).Length(Vehicle.StringLengths.MAX_NEDAP_SERIAL_NUMBER).Nullable();

            Map(x => x.DateRequisitioned).Nullable();
            Map(x => x.DateOrdered).Nullable();
            Map(x => x.RequisitionNumber).Nullable().Length(Vehicle.StringLengths.REQUISITION_NUMBER);
            Map(x => x.DateInService).Nullable();
            Map(x => x.DateRetired).Nullable();
            //Map(x => x.ExistingVehicleNumber).Nullable().Length(Vehicle.StringLengths.EXISTING_VEHICLE_NUMBER);
            Map(x => x.DecalNumber).Column("Decal_Number").Nullable().Length(Vehicle.StringLengths.DECAL_NUMBER);
            Map(x => x.Upbranded).Nullable();
            Map(x => x.VehicleLabel).Column("Vehicle_Label").Nullable().Length(Vehicle.StringLengths.VEHICLE_LABEL);
            Map(x => x.District).Nullable();
            Map(x => x.EmergencyUse).Nullable();
            Map(x => x.GVW).Nullable().Precision(53);
            Map(x => x.AssetDetails).Nullable().Length(Vehicle.StringLengths.ASSET_DETAILS);
            Map(x => x.RegistrationRenewalDate).Nullable();
            Map(x => x.RegistrationAnnualCost).Nullable().Precision(53);
            Map(x => x.LeasingCompany).Nullable().Length(Vehicle.StringLengths.LEASING_COMPANY);
            Map(x => x.LeaseTerm).Nullable().Length(Vehicle.StringLengths.LEASE_TERM);
            Map(x => x.LeaseExpiration).Nullable();
            Map(x => x.LeaseCostMth).Column("LeaseCostMTH").Nullable().Precision(53);
            Map(x => x.OriginalAssetValueCapCost).Nullable().Precision(53);
            Map(x => x.PlannedReplacementYear).Nullable();
            Map(x => x.AlvId).Column("ALV_ID").Nullable().Length(Vehicle.StringLengths.ALV_ID);
            Map(x => x.ToughbookSerialNumber).Nullable().Length(Vehicle.StringLengths.TOUGHBOOK_SERIAL_NUMBER);
            Map(x => x.ToughbookMount).Nullable().Length(Vehicle.StringLengths.TOUGHBOOK_MOUNT);
            Map(x => x.FuelCardNumber).Nullable().Length(Vehicle.StringLengths.FUEL_CARD_NUMBER);
            Map(x => x.MileageTracked).Nullable();
            Map(x => x.Comments).Nullable().Length(Vehicle.StringLengths.COMMENTS);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.CreatedBy).Column("createdBy").Nullable().Length(Vehicle.StringLengths.CREATED_BY);
            Map(x => x.WBSNumber).Length(Vehicle.StringLengths.WBS_NUMBER).Nullable();

            References(x => x.OperatingCenter, "OpCenterID").Nullable();
            References(x => x.AssignmentCategory).Column("AssignmentCategory").Nullable();
            References(x => x.AssignmentJustification).Column("AssignmentJustification").Nullable();
            References(x => x.AssignmentStatus).Column("AssignedStatus").Nullable();
            References(x => x.AccountingRequirement).Column("AccountingRequirement").Nullable();
            References(x => x.Status).Column("VehicleStatus").Nullable();
            References(x => x.EZPass).Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.Manager).Column("Manager").Nullable();
            References(x => x.FleetContactPerson).Column("FleetContactPerson").Nullable();
            // References(x => x.VehicleAssignedTo).Column("VehicleAssignedTo").Nullable();
            References(x => x.PrimaryDriver).Column("PrimaryDriver").Nullable();
            References(x => x.Department).Column("Department").Nullable();
            References(x => x.PrimaryVehicleUse).Column("PrimaryVehicleUse").Nullable();
            References(x => x.Type).Column("VehicleType").Nullable();
            References(x => x.FuelType).Column("FuelType").Nullable();
            References(x => x.ReplacementVehicle).Nullable();
            References(x => x.VehicleIcon).Nullable();
            References(x => x.OwnershipType, "VehicleOwnershipTypeId").Nullable();
            References(x => x.ServiceCompany, "VehicleServiceCompanyId").Nullable();
            References(x => x.GPSType, "GPSType").Nullable();

            HasMany(x => x.Audits).KeyColumn("VehicleId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Incidents).KeyColumn("VehicleId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
