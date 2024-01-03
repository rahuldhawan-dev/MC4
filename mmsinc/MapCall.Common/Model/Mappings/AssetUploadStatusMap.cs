using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AssetUploadStatusMap : EntityLookupMap<AssetUploadStatus>
    {
        public AssetUploadStatusMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
