using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeHeadCountMap : ClassMap<EmployeeHeadCount>
    {
        public EmployeeHeadCountMap()
        {
            Table("EmployeeHeadCounts");
            Id(x => x.Id);

            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedAt).Nullable(); // Not that it should be, but there's data that's null here.
            Map(x => x.EndDate).Nullable();
            Map(x => x.NonUnionCount).Not.Nullable();
            Map(x => x.MiscNotes, "Notes").Nullable().AsTextField();
            Map(x => x.OtherCount).Not.Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.TotalCount).Not.Nullable();
            Map(x => x.UnionCount).Not.Nullable();
            Map(x => x.Year).Nullable();

            References(x => x.BusinessUnit, "BusinessUnitID").Nullable();
            References(x => x.Category, "EmployeeHeadCountCategoryId").Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
