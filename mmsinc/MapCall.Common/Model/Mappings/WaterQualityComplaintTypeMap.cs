using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterQualityComplaintTypeMap : ClassMap<WaterQualityComplaintType>
    {
        #region Constructors

        public WaterQualityComplaintTypeMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(50);
        }

        #endregion
    }
}
