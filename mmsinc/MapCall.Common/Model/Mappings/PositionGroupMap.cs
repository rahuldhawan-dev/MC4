using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PositionGroupMap : ClassMap<PositionGroup>
    {
        public PositionGroupMap()
        {
            Id(x => x.Id)
               .Not.Nullable()
               .GeneratedBy.Identity();

            Map(x => x.BusinessUnit)
               .Length(PositionGroup.StringLengths.BUSINESS_UNIT)
               .Not.Nullable();
            Map(x => x.BusinessUnitDescription)
               .Length(PositionGroup.StringLengths.BUSINESS_UNIT_DESCRIPTION)
               .Not.Nullable();
            // This is GroupCode because SQLite chokes on columns named "Group".
            Map(x => x.Group, "GroupCode")
               .Length(PositionGroup.StringLengths.GROUP)
               .Not.Nullable();
            Map(x => x.PositionDescription)
               .Length(PositionGroup.StringLengths.POSITION_DESCRIPTION);
            Map(x => x.SAPPositionGroupKey)
               .Length(PositionGroup.StringLengths.SAP_POSITION_GROUP_KEY)
               .Unique() // NOTE: This has an index setup to be unique but allow multiple nulls.
               .Nullable();

            References(x => x.State)
               .Nullable();
            References(x => x.SAPCompanyCode)
               .Not.Nullable();
            References(x => x.CommonName, "PositionGroupCommonNameId")
               .Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
