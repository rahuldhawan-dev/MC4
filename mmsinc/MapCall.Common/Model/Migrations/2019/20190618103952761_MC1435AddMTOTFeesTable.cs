using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190618103952761), Tags("Production")]
    public class MC1435AddMTOTFeesTable : Migration
    {
        public override void Up()
        {
            Create.Table("MerchantTotalFees")
                  .WithIdentityColumn()
                  .WithColumn("Fee").AsDecimal(4, 3).NotNullable()
                  .WithColumn("IsCurrent").AsBoolean().NotNullable();
            Execute.Sql(@"
                CREATE UNIQUE NONCLUSTERED INDEX ix_MerchantTotalFees_ThereCanBeOnlyOne
                ON MerchantTotalFees (IsCurrent)
                WHERE IsCurrent = 1");
            Execute.Sql("INSERT INTO MerchantTotalFees Values(0.034, 0)");
            Execute.Sql("INSERT INTO MerchantTotalFees Values(0.036, 1)");
            Alter.Table("TrafficControlTickets").AddForeignKeyColumn("MerchantTotalFeeId", "MerchantTotalFees");
            Execute.Sql("UPDATE TrafficControlTickets SET MerchantTotalFeeId = 1");
        }

        public override void Down()
        {
            Execute.Sql("DROP INDEX MerchantTotalFees.ix_MerchantTotalFees_ThereCanBeOnlyOne");
            Delete.ForeignKeyColumn("TrafficControlTickets", "MerchantTotalFeeId", "MerchantTotalFees");
            Delete.Table("MerchantTotalFees");
        }
    }
}
