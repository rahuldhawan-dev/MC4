using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210423113720246), Tags("Production")]
    public class AddMissingSAPWorkOrderPurposesForMC3224 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"IF NOT EXISTS (SELECT 1 FROM SAPWorkOrderPurposes where [Description] = 'Locate' AND Code = 'I16' and CodeGroup = 'N-D-PUR1')
                            INSERT INTO SAPWorkOrderPurposes values('I16', 'Locate', 'N-D-PUR1');");
            Execute.Sql(@"IF NOT EXISTS (SELECT 1 FROM SAPWorkOrderPurposes where [Description] = 'Clean Out' AND Code = 'I17' and CodeGroup = 'N-D-PUR1')
                            INSERT INTO SAPWorkOrderPurposes values('I17', 'Clean Out', 'N-D-PUR1');");
        }

        public override void Down()
        {
            // can't remove as these could be used at this point
        }
    }
}