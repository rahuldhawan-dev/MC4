using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130211103555), Tags("Production")]
    public class AddProperUserIdColumnToRolesTable : Migration
    {
        public const string FOREIGN_KEY_NAME = "FK_Roles_tblPermissions_UserId",
                            INDEX_NAME = "IDX_Everything",
                            ROLES_TABLE = "Roles";

        public override void Up()
        {
            // There's some(6500 or so) roles that exists for users that exist
            // in aspnet_Membership but otherwise do not exist in tblPermissions.
            // These roles can die,
            Delete.Index(INDEX_NAME).OnTable(ROLES_TABLE);
            Execute.Sql(
                "delete from [Roles] where (select top 1 [RecId] from [tblPermissions] where [tblPermissions].[uid] = [Roles].[UserID]) is null");

            Alter.Table(ROLES_TABLE)
                 .AddColumn("UserIdProper")
                 .AsInt32()
                 .Nullable();

            Execute.Sql(@"update [Roles] set [UserIdProper] = [tblPermissions].[RecID] 
                            from [Roles]
                            inner join [aspnet_Users] on [aspnet_Users].[UserId] = [Roles].[UserID]
                            inner join [tblPermissions] on [tblPermissions].[UserName] = [aspnet_Users].[UserName]
                            where [aspnet_users].[UserId] = [Roles].[UserID]");

            Delete.Column("UserId").FromTable(ROLES_TABLE);
            Rename.Column("UserIdProper").OnTable(ROLES_TABLE).To("UserId");

            Alter.Column("UserId")
                 .OnTable(ROLES_TABLE)
                 .AsInt32()
                 .NotNullable()
                 .ForeignKey(FOREIGN_KEY_NAME, "tblPermissions", "RecID");
        }

        public override void Down()
        {
            Delete.ForeignKey(FOREIGN_KEY_NAME).OnTable(ROLES_TABLE);
            Rename.Column("UserId").OnTable(ROLES_TABLE).To("UserIdProper");
            Alter.Table(ROLES_TABLE)
                 .AddColumn("UserId")
                 .AsGuid()
                 .Nullable();

            // Reset the old roles userid if possible
            Execute.Sql(@"update [Roles] set [UserId] = [tblPermissions].[uid] 
                        from [Roles]
                        inner join [tblPermissions] on [tblPermissions].[RecId] = [Roles].[UserIdProper]");

            Delete.Column("UserIdProper").FromTable(ROLES_TABLE);

            Execute.Sql(
                "DELETE Roles FROM Roles LEFT OUTER JOIN (SELECT CONVERT(uniqueidentifier, MIN(CONVERT(char(36), UserId))) as RowId, OperatingCenterId, ApplicationId, ModuleId, ActionId FROM Roles GROUP BY OperatingCenterId, ApplicationId, ModuleId, ActionId) as KeepRows on Roles.UserId = KeepRows.RowId AND Roles.OperatingCenterId = KeepRows.OperatingCenterId AND Roles.ApplicationId = KeepRows.ApplicationId AND Roles.ModuleId = KeepRows.ModuleId AND Roles.ActionId = KeepRows.ActionId WHERE KeepRows.RowId IS NULL");

            Execute.Sql(@"CREATE UNIQUE NONCLUSTERED INDEX [IDX_Everything] ON [Roles] 
(
	[UserID] ASC,
	[OperatingCenterID] ASC,
	[ApplicationID] ASC,
	[ModuleID] ASC,
	[ActionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
        }
    }
}
