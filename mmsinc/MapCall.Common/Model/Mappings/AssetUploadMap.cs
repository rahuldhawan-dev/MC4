using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AssetUploadMap : ClassMap<AssetUpload>
    {
        public AssetUploadMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.CreatedAt).Not.Update().Not.Nullable();
            Map(x => x.FileName).Not.Update().Not.Nullable();
            Map(x => x.FileGuid).Not.Update().Not.Nullable();
            Map(x => x.ErrorText).Length(int.MaxValue).Nullable();

            References(x => x.CreatedBy).Not.Update().Not.Nullable();
            References(x => x.Status).Not.Nullable();
        }
    }
}
