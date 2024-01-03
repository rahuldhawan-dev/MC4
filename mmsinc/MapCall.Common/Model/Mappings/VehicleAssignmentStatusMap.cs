using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class VehicleAssignmentStatusMap : EntityLookupMap<VehicleAssignmentStatus>
    {
        public VehicleAssignmentStatusMap()
        {
            Table("VehicleAssignmentStatuses");
        }
    }
}
