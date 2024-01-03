using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ValveInspectionMap : ClassMap<ValveInspection>
    {
        public ValveInspectionMap()
        {
            Id(x => x.Id);

            References(x => x.Valve).Nullable();
            References(x => x.InspectedBy).Nullable();
            References(x => x.NormalPosition).Nullable();
            References(x => x.PositionFound).Nullable();
            References(x => x.PositionLeft).Nullable();
            References(x => x.WorkOrderRequestOne).Column("WorkOrderRequest1Id").Nullable();
            References(x => x.WorkOrderRequestTwo).Column("WorkOrderRequest2Id").Nullable();
            References(x => x.WorkOrderRequestThree).Column("WorkOrderRequest3Id").Nullable();

            //FORMULA
            References(x => x.OperatingCenter)
               .Formula("(SELECT vvv.OperatingCenterId from Valves vvv where vvv.Id = ValveId)");

            Map(x => x.DateInspected).Not.Nullable();
            Map(x => x.MinimumRequiredTurns).Nullable();
            Map(x => x.Inspected).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.Remarks).Nullable();
            Map(x => x.Turns).Nullable();
            Map(x => x.TurnsNotCompleted).Not.Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.SAPNotificationNumber).Nullable();
        }
    }
}
