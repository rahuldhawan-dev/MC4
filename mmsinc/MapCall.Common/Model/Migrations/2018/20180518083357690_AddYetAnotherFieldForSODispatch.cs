using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20180518083357690), Tags("Production")]
    public class AddYetAnotherFieldForSODispatch : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("LiabilityIndicator")
                 .AsAnsiString(80).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LiabilityIndicator").FromTable("ShortCycleWorkOrders");
        }
    }
}
