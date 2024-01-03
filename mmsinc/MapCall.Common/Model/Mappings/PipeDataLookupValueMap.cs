using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PipeDataLookupValueMap : ClassMap<PipeDataLookupValue>
    {
        #region Constructors

        public PipeDataLookupValueMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            References(x => x.PipeDataLookupType);

            Map(x => x.Description).Not.Nullable();
            Map(x => x.VariableScore).Not.Nullable();
            Map(x => x.PriorityWeightedScore).Not.Nullable();
            Map(x => x.IsEnabled).Not.Nullable();
            Map(x => x.IsDefault).Not.Nullable();
        }

        #endregion
    }
}
