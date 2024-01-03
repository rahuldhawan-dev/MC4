using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191021105803291), Tags("Production")]
    public class MC1712UpdatedCoordinateIDForEquipmentRecordsFromFacilityTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "UPDATE dbo.Equipment \r\nSET CoordinateID = fac.CoordinateID\r\nFROM dbo.Equipment equ\r\nINNER JOIN dbo.tblFacilities fac\r\nON fac.RecordId = equ.FacilityID\r\nWHERE equ.CoordinateId <> fac.CoordinateID");
        }

        public override void Down() { }
    }
}
