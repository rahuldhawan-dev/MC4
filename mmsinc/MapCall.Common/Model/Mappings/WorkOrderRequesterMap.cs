using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderRequesterMap : ClassMap<WorkOrderRequester>
    {
        #region Constructors

        public WorkOrderRequesterMap()
        {
            Id(x => x.Id).Column("WorkOrderRequesterID").GeneratedBy.Assigned();

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
