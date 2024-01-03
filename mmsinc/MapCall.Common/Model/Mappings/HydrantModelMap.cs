using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantModelMap : EntityLookupMap<HydrantModel>
    {
        public HydrantModelMap()
        {
            References(x => x.HydrantManufacturer).Column("ManufacturerID").Not.Nullable();
        }
    }
}
