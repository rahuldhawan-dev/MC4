using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceSizeMap : ClassMap<ServiceSize>
    {
        #region Constructors

        public ServiceSizeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Hydrant).Not.Nullable();
            Map(x => x.Lateral).Not.Nullable();
            Map(x => x.Main).Not.Nullable();
            Map(x => x.Meter).Not.Nullable();
            Map(x => x.SortOrder).Nullable();
            Map(x => x.Service).Not.Nullable();
            Map(x => x.Size).Not.Nullable();
            Map(x => x.ServiceSizeDescription).Not.Nullable();
            Map(x => x.SAPCode).Nullable();
        }

        #endregion
    }
}
