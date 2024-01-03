using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectPipeDataLookupValueMap : ClassMap<RecurringProjectPipeDataLookupValue>
    {
        public const string TABLE_NAME = "RecurringProjectsPipeDataLookupValues";

        public RecurringProjectPipeDataLookupValueMap()
        {
            Table(TABLE_NAME);

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.RecurringProject).Not.Nullable();
            References(x => x.PipeDataLookupValue).Not.Nullable();
        }
    }
}
