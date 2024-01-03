using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorMeterCrewMap : ClassMap<ContractorMeterCrew>
    {
        public ContractorMeterCrewMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Length(ContractorMeterCrew.StringLengths.DESCRIPTION).Not.Nullable();
            Map(x => x.AMLargeMeters).Not.Nullable();
            Map(x => x.AMMeters).Not.Nullable();
            Map(x => x.PMLargeMeters).Not.Nullable();
            Map(x => x.PMMeters).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();

            References(x => x.Contractor).Not.Nullable();
        }
    }
}
