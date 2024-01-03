using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180709110237990), Tags("Production")]
    public class AddUniqueIndexFoSAPEquipmentId : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE UNIQUE NONCLUSTERED INDEX 
	                UNIQUE_IX_SAP_EQUIPMENT_ID_tblEquipment
                ON
	                tblEquipment (SAP_EQUIPMENT_ID ASC)
                WHERE
	                SAP_EQUIPMENT_ID IS NOT NULL;");
        }

        public override void Down()
        {
            Execute.Sql("DROP INDEX [UNIQUE_IX_SAP_EQUIPMENT_ID_tblEquipment] ON [tblEquipment]");
        }
    }
}
