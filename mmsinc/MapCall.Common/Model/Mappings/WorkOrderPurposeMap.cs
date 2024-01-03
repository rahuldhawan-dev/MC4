using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderPurposeMap : EntityLookupMap<WorkOrderPurpose>
    {
        #region Constructors

        public WorkOrderPurposeMap()
        {
            Id(x => x.Id, "WorkOrderPurposeID").GeneratedBy.Assigned();

            Map(x => x.IsProduction).Not.Nullable();
            Map(x => x.SapCode).Length(WorkOrderPurpose.StringLengths.CODE).Nullable();
            Map(x => x.CodeGroup).Length(WorkOrderPurpose.StringLengths.CODE_GROUP).Nullable();
        }

        #endregion
    }
}
