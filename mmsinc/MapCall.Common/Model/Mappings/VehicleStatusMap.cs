using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class VehicleStatusMap : EntityLookupMap<VehicleStatus>
    {
        public VehicleStatusMap()
        {
            Table("VehicleStatuses");
        }
    }
}
