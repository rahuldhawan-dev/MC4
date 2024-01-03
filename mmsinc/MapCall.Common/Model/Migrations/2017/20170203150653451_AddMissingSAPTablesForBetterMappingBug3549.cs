using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170203150653451), Tags("Production")]
    public class AddMissingSAPTablesForBetterMappingBug3549 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SAPWorkOrderPriorities",
                "Emergency 1-2 Hrs", // 1
                "Emergency 24 Hrs.", // 2 *eyetwitch*
                "One Business Day", // 3
                "Within 5 Days", // 4 
                "Within 15 Days", // 5
                "Within 30 Days", // 6
                "Within 90 Days", // 7
                "Within 160 Days"); // 8 
            Create.Table("SAPWorkOrderPurposes").WithIdentityColumn()
                  .WithColumn("Code").AsAnsiString(3).NotNullable()
                  .WithColumn("Description").AsAnsiString(100).NotNullable();
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I01','Customer (Compliant/Request)')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I02','Equipment Reliability')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I03','Safety')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I04','AW Compliance')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I05','Regulatory')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I06','Seasonal')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I07','Leak Detection')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I08','Revenue $150-$500')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I09','Revenue $500-%1000')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I10','Revenue >$1000')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I11','Damaged/Billiable')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I12','Estimates')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I13','Water Quality')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I14','Asset Record Control')");
            Execute.Sql("INSERT INTO [SAPWorkOrderPurposes] Values('I15','Demolition')");
        }

        public override void Down()
        {
            Delete.Table("SAPWorkOrderPriorities");
            Delete.Table("SAPWorkOrderPurposes");
        }
    }
}
