using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150216143916585), Tags("Production")]
    public class AddPrimaryKeyToUserViewedTableBug2144 : Migration
    {
        public override void Up()
        {
            Alter.Table("UserViewed")
                 .AddColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("Id").FromTable("UserViewed");
        }
    }
}
