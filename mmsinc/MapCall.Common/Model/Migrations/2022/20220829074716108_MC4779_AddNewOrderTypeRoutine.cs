using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220829074716108), Tags("Production")]
    public class Mc4779AddNewOrderTypeRoutine : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO OrderTypes Values('Routine', '0013');");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM OrderTypes WHERE SAPCode = '0013';");
        }
    }
}

