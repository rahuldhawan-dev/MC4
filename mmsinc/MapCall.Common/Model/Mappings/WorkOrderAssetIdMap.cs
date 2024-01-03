using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderAssetIdMap : ClassMap<WorkOrderAssetId>
    {
        public WorkOrderAssetIdMap()
        {
            ReadOnly();
            Table("WorkOrderAssetIdView");

            Id(x => x.Id);

            References(x => x.WorkOrder, "Id")
               .Not.Nullable()
               .Not.Insert()
               .Not.Update();

            Map(x => x.AssetId).Nullable();

            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
