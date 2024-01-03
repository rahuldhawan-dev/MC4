using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DevelopmentProjectMap : ClassMap<DevelopmentProject>
    {
        public DevelopmentProjectMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("ProjectID");

            References(x => x.OperatingCenter).Column("OpCode").Not.Nullable();
            References(x => x.Category).Column("Category").Not.Nullable();
            References(x => x.BusinessUnit).Column("BU").Not.Nullable();
            References(x => x.ProjectManager).Column("ProjectManager").Nullable();
            References(x => x.PublicWaterSupply).Column("PWSID").Not.Nullable();
            References(x => x.Town).Column("Town").Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.CreatedBy).Not.Nullable();

            Map(x => x.DomesticWaterServices).Nullable();
            Map(x => x.FireServices).Nullable();
            Map(x => x.DomesticSanitaryServices).Nullable();
            Map(x => x.DeveloperServicesId).Length(20);
            Map(x => x.WBSNumber).Length(20);
            Map(x => x.ProjectDescription).Not.Nullable();
            Map(x => x.StreetName).Length(50);

            // This is nullable in the db, but it's a required field. Some nulls in the db though.
            Map(x => x.ForecastedInServiceDate)
               .Nullable();
            Map(x => x.InServiceDate);

            HasMany(x => x.DevelopmentProjectDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.DevelopmentProjectNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
