using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181022131114307), Tags("Production")]
    public class AddShortCycleWorkOrderColumnForMC689 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("ServiceFound").AsAnsiString(40).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ServiceFound").FromTable("ShortCycleWorkOrders");
        }
    }
}
