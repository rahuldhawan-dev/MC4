using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalPermitMap : ClassMap<EnvironmentalPermit>
    {
        #region Constants

        public const string
            SQL_HAS_FACILITY_OR_EQUIPMENT =
                "(CASE " +
                "   WHEN (SELECT COUNT(1) FROM EnvironmentalPermitsEquipment epe WHERE epe.EnvironmentalPermitID = EnvironmentalPermitID) > 0 " +
                "           AND " +
                "        (SELECT Count(1) FROM EnvironmentalPermitsFacilities epf WHERE epf.EnvironmentalPermitID = EnvironmentalPermitID) > 0 " +
                "   THEN 1 " +
                "   ELSE 0 " +
                "END)";

        #endregion

        #region Constructors

        public EnvironmentalPermitMap()
        {
            Id(x => x.Id, "EnvironmentalPermitID");

            References(x => x.EnvironmentalPermitType).Nullable();
            References(x => x.EnvironmentalPermitStatus).Nullable();
            References(x => x.PublicWaterSupply).Nullable();
            References(x => x.WasteWaterSystem).Nullable();
            References(x => x.State).Nullable();

            // Required field, but nullable in the db with many many null values.
            References(x => x.FacilityType).Nullable();
            References(x => x.FirstFacility)
               .DbSpecificFormula(
                    "(SELECT TOP 1 EPF.FacilityID from EnvironmentalPermitsFacilities EPF JOIN tblFacilities F on F.RecordId = EPF.FacilityId where EPF.EnvironmentalPermitId = EnvironmentalPermitId ORDER BY F.FacilityName)",
                    "(SELECT EPF.FacilityID from EnvironmentalPermitsFacilities EPF JOIN tblFacilities F on F.RecordId = EPF.FacilityId where EPF.EnvironmentalPermitId = EnvironmentalPermitId ORDER BY F.FacilityName LIMIT 1)");
            References(x => x.FirstEquipment)
               .DbSpecificFormula(
                    "(SELECT TOP 1 EPE.EquipmentId from EnvironmentalPermitsEquipment EPE JOIN Equipment E on E.EquipmentID = EPE.EquipmentID where EPE.EnvironmentalPermitId = EnvironmentalPermitId ORDER BY E.EquipmentID)",
                    "(SELECT EPE.EquipmentId from EnvironmentalPermitsEquipment EPE JOIN Equipment E on E.EquipmentID = EPE.EquipmentID where EPE.EnvironmentalPermitId = EnvironmentalPermitId ORDER BY E.EquipmentID LIMIT 1)");

            Map(x => x.PermitNumber);
            Map(x => x.ProgramInterestNumber);
            Map(x => x.PermitCrossReferenceNumber);
            Map(x => x.PermitEffectiveDate);
            Map(x => x.PermitRenewalDate);
            Map(x => x.PermitExpirationDate);
            Map(x => x.Description);
            Map(x => x.RequiresFees);

            // Required field, but db has many nullable values.
            Map(x => x.ReportingRequired).Nullable();
            Map(x => x.RequiresRequirements).Not.Nullable();
            Map(x => x.IsLinkedToFacilityOrEquipment).Formula(SQL_HAS_FACILITY_OR_EQUIPMENT);
            Map(x => x.PermitName);

            HasMany(x => x.AllocationPermits).KeyColumn("EnvironmentalPermitID");
            HasMany(x => x.Fees).KeyColumn("EnvironmentalPermitId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Requirements).KeyColumn("PermitId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();

            HasManyToMany(x => x.OperatingCenters)
               .Table("EnvironmentalPermitsOperatingCenters")
               .ParentKeyColumn("EnvironmentalPermitID")
               .ChildKeyColumn("OperatingCenterID");
            HasManyToMany(x => x.Equipment)
               .Table("EnvironmentalPermitsEquipment")
               .ParentKeyColumn("EnvironmentalPermitID")
               .ChildKeyColumn("EquipmentID");
            HasManyToMany(x => x.Facilities)
               .Table("EnvironmentalPermitsFacilities")
               .ParentKeyColumn("EnvironmentalPermitID")
               .ChildKeyColumn("FacilityID");
        }

        #endregion
    }
}
