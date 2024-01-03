using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190110102906584), Tags("Production")]
    public class MC757AddAddressToShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("PremiseAddress")
                 .AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("PremiseAddress").FromTable("ShortCycleWorkOrders");
        }
    }
}
