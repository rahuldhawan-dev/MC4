using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantDSICReportItemMap : ClassMap<HydrantDSICReportItem>
    {
        public const string TABLE_NAME = "HydrantsDSICView";

        public HydrantDSICReportItemMap()
        {
            Table(TABLE_NAME);
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.StreetNumber);
            Map(x => x.HydrantNumber);
            Map(x => x.SAPEquipmentId);
            Map(x => x.DateInstalled);
            Map(x => x.WBSNumber);
            Map(x => x.PremiseNumber);
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
