using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    // TODO: why is this not an entity lookup?
    public class ActionItemTypeMap : ClassMap<ActionItemType>
    {
        public ActionItemTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Description);

            References(x => x.DataType);
        }
    }
}
