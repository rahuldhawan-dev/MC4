using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class BacterialSampleTypeMap : EntityLookupMap<BacterialSampleType>
    {
        #region Constructors

        public BacterialSampleTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }

        #endregion
    }
}
