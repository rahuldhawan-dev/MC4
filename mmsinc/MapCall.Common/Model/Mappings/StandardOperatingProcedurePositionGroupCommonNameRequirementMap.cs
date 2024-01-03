using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class
        StandardOperatingProcedurePositionGroupCommonNameRequirementMap : ClassMap<
            StandardOperatingProcedurePositionGroupCommonNameRequirement>
    {
        public StandardOperatingProcedurePositionGroupCommonNameRequirementMap()
        {
            Id(x => x.Id);

            Map(x => x.Frequency).Not.Nullable();

            References(x => x.FrequencyUnit).Not.Nullable();
            References(x => x.PositionGroupCommonName).Not.Nullable();
            References(x => x.StandardOperatingProcedure).Not.Nullable();
        }
    }
}
