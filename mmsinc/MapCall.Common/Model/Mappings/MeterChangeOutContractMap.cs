using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterChangeOutContractMap : ClassMap<MeterChangeOutContract>
    {
        public MeterChangeOutContractMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Length(MeterChangeOutContract.StringLengths.DESCRIPTION).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();

            References(x => x.Contractor).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();

            HasMany(x => x.MeterChangeOuts).KeyColumn("MeterChangeOutContractId").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
