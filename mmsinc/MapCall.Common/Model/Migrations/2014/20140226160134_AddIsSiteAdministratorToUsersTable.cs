using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140226160134), Tags("Production")]
    public class AddIsSiteAdministratorToUsersTable : Migration
    {
        private void SetSiteAdmin(string username)
        {
            Update.Table("tblPermissions").Set(new {IsSiteAdministrator = true}).Where(new {UserName = username});
        }

        public override void Up()
        {
            Alter.Table("tblPermissions")
                 .AddColumn("IsSiteAdministrator")
                 .AsBoolean()
                 .WithDefaultValue(false);

            SetSiteAdmin("mcadmin");
            SetSiteAdmin("kevinkirwan");
            SetSiteAdmin("dougthorn");
            SetSiteAdmin("keanek");
            SetSiteAdmin("wigganme");
        }

        public override void Down()
        {
            Delete.Column("IsSiteAdministrator").FromTable("tblPermissions");
        }
    }
}
