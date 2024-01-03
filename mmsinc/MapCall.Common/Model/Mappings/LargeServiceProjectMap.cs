using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class LargeServiceProjectMap : ClassMap<LargeServiceProject>
    {
        public LargeServiceProjectMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.WBSNumber).Nullable();
            Map(x => x.ProjectTitle).Not.Nullable();
            Map(x => x.ProjectAddress).Not.Nullable();
            Map(x => x.ContactName).Nullable();
            Map(x => x.ContactEmail).Nullable();
            Map(x => x.ContactPhone).Nullable();
            Map(x => x.InitialContactDate).Nullable();
            Map(x => x.InServiceDate).Nullable();

            Map(x => x.CreatedBy)
               .DbSpecificFormula(
                    "(select top 1 P.FullName From AuditLogEntries ale join tblPermissions P on P.RecId = ale.UserId where ale.AuditEntryType = 'INSERT' and ale.EntityName = 'LargeServiceProject' and ale.EntityId = Id)",
                    "(select P.FullName From AuditLogEntries ale join tblPermissions P on P.RecId = ale.UserId where ale.AuditEntryType = 'INSERT' and ale.EntityName = 'LargeServiceProject' and ale.EntityId = Id LIMIT 1)");

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Not.Nullable();
            References(x => x.AssetCategory).Nullable();
            References(x => x.AssetType).Nullable();
            References(x => x.ProposedPipeDiameter, "ProposedDiameterId").Nullable();
            References(x => x.Coordinate).Nullable();

            HasMany(x => x.LargeServiceProjectDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.LargeServiceProjectNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
