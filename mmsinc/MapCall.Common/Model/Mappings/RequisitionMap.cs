using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RequisitionMap : ClassMap<Requisition>
    {
        #region Constructors

        public RequisitionMap()
        {
            Id(x => x.Id, "RequisitionID");

            Map(x => x.SAPRequisitionNumber)
               .Length(Requisition.StringLengths.SAP_REQUISITION_NUMBER_MAX_LENGTH)
               .Not.Nullable();
            Map(x => x.CreatedAt)
               .Not.Nullable();

            References(x => x.CreatedBy, "CreatorID");
            References(x => x.RequisitionType)
               .Not.Nullable();
            References(x => x.WorkOrder)
               .Not.Nullable();
        }

        #endregion
    }
}
