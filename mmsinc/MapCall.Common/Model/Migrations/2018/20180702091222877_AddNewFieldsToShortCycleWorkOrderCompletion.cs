using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180702091222877), Tags("Production")]
    public class AddNewFieldsToShortCycleWorkOrderCompletion : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddColumn("ManufacturerSerialNumber").AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ManufacturerSerialNumber").FromTable("ShortCycleWorkOrderCompletions");
        }
    }
}
