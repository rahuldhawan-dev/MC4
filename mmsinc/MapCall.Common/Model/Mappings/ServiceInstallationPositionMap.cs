using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationPositionMap : ClassMap<ServiceInstallationPosition>
    {
        public ServiceInstallationPositionMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Nullable();
            Map(x => x.CodeGroup).Nullable();
            Map(x => x.SAPCode).Nullable();
        }
    }
}
