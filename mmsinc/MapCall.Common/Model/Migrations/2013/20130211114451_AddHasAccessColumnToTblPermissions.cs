using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130211114451), Tags("Production")]
    public class AddHasAccessColumnToTblPermissions : Migration
    {
        public override void Up()
        {
            Alter.Table("tblPermissions")
                 .AddColumn("HasAccess")
                 .AsBoolean()
                 .WithDefaultValue(false);

            Execute.Sql(@"update [tblPermissions] 
                          set [HasAccess] = [aspnet_Membership].[IsApproved]
                          from [tblPermissions]
                          inner join [aspnet_Membership] on [aspnet_Membership].[UserId] = [tblPermissions].[uid]
                          where [aspnet_Membership].[UserId] = [tblPermissions].[uid]");
        }

        public override void Down()
        {
            Delete.Column("HasAccess").FromTable("tblPermissions");
        }
    }
}
