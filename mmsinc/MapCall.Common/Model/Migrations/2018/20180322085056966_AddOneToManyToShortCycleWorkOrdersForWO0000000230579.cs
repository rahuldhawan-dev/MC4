using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180322085056966), Tags("Production")]
    public class AddOneToManyToShortCycleWorkOrdersForWO0000000230579 : Migration
    {
        public override void Up()
        {
            Delete.Column("SecurityThreat").FromTable("ShortCycleWorkOrders");
            Delete.Column("PoliceEscort").FromTable("ShortCycleWorkOrders");
            Create.Table("ShortCycleWorkOrderSecurityThreats")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders").NotNullable()
                  .WithColumn("SecurityThreat").AsCustom("text").Nullable()
                  .WithColumn("PoliceEscort").AsAnsiString(3).Nullable();
        }

        public override void Down()
        {
            Delete.Table("ShortCycleWorkOrderSecurityThreats");
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("SecurityThreat").AsCustom("text").Nullable()
                 .AddColumn("PoliceEscort").AsAnsiString().Nullable();
        }
    }
}
