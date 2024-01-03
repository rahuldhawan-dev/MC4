using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180803090321568), Tags("Production")]
    public class AddCreatedByToSCWORequests : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderRequests").AddColumn("UniqueId").AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("UniqueId").FromTable("ShortCycleWorkOrderRequests");
        }
    }
}
