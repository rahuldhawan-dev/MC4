using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210608154121096), Tags("Production")]
    public class AddLastLoggedInAtToUsersForMC3347 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblPermissions")
                 .AddColumn("LastLoggedInAt").AsDateTime().Nullable();
            Execute.Sql(@"with cte (UserId, LoggedInAt)
as
(
    SELECT
        UserId, max(LoggedInAt) as LoggedInAt
    FROM
        AuthenticationLogs
    GROUP BY 
        UserId
)
UPDATE
    tblPermissions
SET
    LastLoggedInAt = cte.LoggedInAt
FROM
    cte
WHERE
    cte.UserId = tblPermissions.RecID");
            Alter.Table("ContractorUsers").AddColumn("LastLoggedInAt").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("LastLoggedInAt").FromTable("ContractorUsers");
            Delete.Column("LastLoggedInAt").FromTable("tblPermissions");
        }
    }
}

