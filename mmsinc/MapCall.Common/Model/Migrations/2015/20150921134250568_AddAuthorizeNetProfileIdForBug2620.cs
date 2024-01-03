using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150921134250568), Tags("Production")]
    public class AddAuthorizeNetProfileIdForBug2620 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblPermissions").AddColumn("CustomerProfileId").AsInt32().Nullable();
            Alter.Table("tblPermissions").AddColumn("ProfileLastVerified").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CustomerProfileId").FromTable("tblPermissions");
            Delete.Column("ProfileLastVerified").FromTable("tblPermissions");
        }
    }
}
