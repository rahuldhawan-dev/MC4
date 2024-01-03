using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RepairTimeRangeMap : EntityLookupMap<RepairTimeRange>
    {
        #region Constructors

        public RepairTimeRangeMap()
        {
            Id(x => x.Id).Column("RepairTimeRangeID").GeneratedBy.Assigned();
        }

        #endregion
    }
}
