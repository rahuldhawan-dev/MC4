using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ActionItemMap : ClassMap<ActionItem>
    {
        public ActionItemMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Note).Length(int.MaxValue);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.LinkedId);
            Map(x => x.DateCompleted).Nullable();
            Map(x => x.NotListedType).Nullable();
            Map(x => x.TargetedCompletionDate).Not.Nullable();

            References(x => x.DataType).Not.Nullable();
            References(x => x.ResponsibleOwner).Nullable();
            References(x => x.Type, "ActionItemTypeId").Not.Nullable();
        }
    }
}
