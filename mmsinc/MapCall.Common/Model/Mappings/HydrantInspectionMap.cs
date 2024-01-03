using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantInspectionMap : ClassMap<HydrantInspection>
    {
        public HydrantInspectionMap()
        {
            Id(x => x.Id);

            References(x => x.Hydrant).Nullable();
            References(x => x.HydrantProblem).Nullable();
            References(x => x.HydrantTagStatus).Nullable();
            References(x => x.HydrantInspectionType, "InspectionTypeId").Nullable();
            References(x => x.WorkOrderRequestOne, "WorkOrderRequest1").Nullable();
            References(x => x.WorkOrderRequestTwo, "WorkOrderRequest2").Nullable();
            References(x => x.WorkOrderRequestThree, "WorkOrderRequest3").Nullable();
            References(x => x.WorkOrderRequestFour, "WorkOrderRequest4").Nullable();
            References(x => x.InspectedBy).Not.Nullable();
            References(x => x.FreeNoReadReason, "FreeNoReadReasonId").Nullable();
            References(x => x.TotalNoReadReason, "TotalNoReadReasonId").Nullable();

            Map(x => x.PreResidualChlorine).Nullable();
            Map(x => x.ResidualChlorine).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.DateInspected).Not.Nullable();
            Map(x => x.FullFlow).Nullable();
            Map(x => x.GallonsFlowed).Formula("MinutesFlowed * GPM");
            Map(x => x.GPM).Nullable();
            Map(x => x.MinutesFlowed).Nullable();
            Map(x => x.StaticPressure).Nullable();
            Map(x => x.Remarks).Nullable();
            Map(x => x.PreTotalChlorine).Nullable();
            Map(x => x.TotalChlorine).Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.SAPNotificationNumber).Nullable();
            Map(x => x.BusinessUnit)
               .Length(HydrantInspection.StringLengths.BUSINESS_UNIT)
               .Nullable();
        }
    }
}
