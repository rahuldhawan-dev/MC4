using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class FamilyMedicalLeaveActCaseMap : ClassMap<FamilyMedicalLeaveActCase>
    {
        public const string TABLE_NAME = "FMLACases";

        public FamilyMedicalLeaveActCaseMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "FMLACaseID");
            References(x => x.Employee, "Employee").Not.Nullable();
            References(x => x.CompanyAbsenceCertification, "CompanyAbsenceCertification").Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.FrequencyDays).Nullable();
            Map(x => x.CertificationExtended).Not.Nullable();
            Map(x => x.SendPackage, "SendFMLAPackage").Not.Nullable();
            Map(x => x.PackageDateSent, "FMLAPackageDateSent").Nullable();
            Map(x => x.PackageDateReceived, "FMLAPackageDateReceived").Nullable();
            Map(x => x.PackageDateDue, "FMLAPackageDateDue").Nullable();
            Map(x => x.ChronicCondition).Not.Nullable();
            Map(x => x.AbsenseId, AddAbsenseIDToFMLATableForBug2621.COLUMN_NAME).Nullable();
            Map(x => x.Duration, AddDurationToFamilyMedicalLeaveActForBug2623.COLUMN_NAME).Nullable();

            // Notes/Docs
            HasMany(x => x.FamilyMedicalLeaveActCaseDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.FamilyMedicalLeaveActCaseNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
