using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AcousticMonitoringTypeMap : EntityLookupMap<AcousticMonitoringType>
    {
        #region Constructors

        public AcousticMonitoringTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }

        #endregion
    }
}
