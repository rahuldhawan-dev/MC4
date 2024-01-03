using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderEquipmentMap : ClassMap<ProductionWorkOrderEquipment>
    {
        public const string TABLE_NAME = "ProductionWorkOrdersEquipment";

        public ProductionWorkOrderEquipmentMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.Equipment).Nullable();
            References(x => x.AsLeftCondition).Nullable();
            References(x => x.AsFoundCondition).Nullable();
            References(x => x.AsFoundConditionReason).Nullable();
            References(x => x.AsLeftConditionReason).Nullable();
            Map(x => x.SAPNotificationNumber).Nullable();
            Map(x => x.CompletedOn).Nullable();
            Map(x => x.SAPEquipmentId).Nullable();
            Map(x => x.IsParent).Nullable();
            Map(x => x.AsFoundConditionComment).Length(ProductionWorkOrderEquipment.StringLengths.COMMENT).Nullable();
            Map(x => x.AsLeftConditionComment).Length(ProductionWorkOrderEquipment.StringLengths.COMMENT).Nullable();
            Map(x => x.RepairComment).Length(ProductionWorkOrderEquipment.StringLengths.COMMENT).Nullable();
        }
    }
}