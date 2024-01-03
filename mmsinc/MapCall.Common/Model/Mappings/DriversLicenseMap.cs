using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class DriversLicenseMap : ClassMap<DriversLicense>
    {
        #region Constants

        public struct Sql
        {
            public const string ENDORSEMENT =
                                    "(CASE WHEN EXISTS (SELECT 1 FROM DriversLicensesEndorsements dle WHERE dle.EndorsementId = {0} AND dle.LicenseId = Id) THEN 1 ELSE 0 END)",
                                RESTRICTION =
                                    "(CASE WHEN EXISTS (SELECT 1 FROM DriversLicensesRestrictions dlr WHERE dlr.RestrictionId = {0} AND dlr.LicenseId = Id) THEN 1 ELSE 0 END)";
        }

        #endregion

        public DriversLicenseMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter)
               .Formula("(Select e.OperatingCenterId FROM tblEmployee e WHERE e.tblEmployeeId = EmployeeId)")
               .ReadOnly();
            References(x => x.Employee).Not.Nullable();
            References(x => x.DriversLicenseClass).Not.Nullable();
            References(x => x.State).Not.Nullable();

            Map(x => x.LicenseNumber).Not.Nullable();
            Map(x => x.IssuedDate).Nullable();
            Map(x => x.RenewalDate).Nullable();

            Map(x => x.Expired).DbSpecificFormula(
                "(CASE WHEN RenewalDate <= GETDATE() THEN 1 ELSE 0 END)",
                "(CASE WHEN RenewalDate <= date('now') THEN 1 ELSE 0 END)");

            HasMany(x => x.Endorsements).KeyColumn("LicenseId").Cascade.AllDeleteOrphan();
            HasMany(x => x.Restrictions).KeyColumn("LicenseId").Cascade.AllDeleteOrphan();

            HasMany(x => x.DriversLicenseDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.DriversLicenseNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();

            #region Logical Properties

            Map(x => x.HasHazardMaterialEndorsement).Formula(String.Format(Sql.ENDORSEMENT,
                DriversLicenseEndorsement.Indices.HAZARDOUS_MATERIAL));
            Map(x => x.HasLiquidBulkTankCargoEndorsement).Formula(String.Format(Sql.ENDORSEMENT,
                DriversLicenseEndorsement.Indices.LIQUID_BULK_TANK_CARGO));
            Map(x => x.HasHazardMaterialAndTankCombinedEndorsement).Formula(String.Format(Sql.ENDORSEMENT,
                DriversLicenseEndorsement.Indices.HAZARDOUS_MATERIAL_AND_TANK_COMBINED));
            Map(x => x.HasMedicalWavierRequiredRestriction).Formula(String.Format(Sql.RESTRICTION,
                DriversLicenseRestriction.Indices.MEDICAL_WAIVER_REQUIRED));

            #endregion
        }
    }
}
