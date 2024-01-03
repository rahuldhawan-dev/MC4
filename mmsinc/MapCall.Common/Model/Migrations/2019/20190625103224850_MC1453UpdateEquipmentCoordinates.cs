using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190625103224850), Tags("Production")]
    public class MC1453UpdateEquipmentCoordinates : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "UPDATE Equipment SET CoordinateId = F.CoordinateID from Equipment E Join tblFacilities F on F.RecordId = E.FacilityID where E.CoordinateId is null and E.FacilityId is not null and F.CoordinateID is not null");
        }

        public override void Down()
        {
            //noop
        }
    }
}
