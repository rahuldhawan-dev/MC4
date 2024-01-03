using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceDSICReportItemMap : ClassMap<ServiceDSICReportItem>
    {
        public const string TABLE_NAME = "ServicesDSIC";

        public ServiceDSICReportItemMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            Map(x => x.StreetNumber);
            Map(x => x.ServiceNumber);
            Map(x => x.DateInstalled);
            Map(x => x.TaskNumber1);
            Map(x => x.OperatingCenterStr, "OperatingCenter");
            Map(x => x.Town);
            Map(x => x.Street);
            Map(x => x.Latitude);
            Map(x => x.Longitude);

            References(x => x.OperatingCenter);

            //need this to prevent SchemaExport from creating a table
            SchemaAction.None();
        }
    }
}
