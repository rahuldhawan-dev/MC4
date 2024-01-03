using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150903141449153), Tags("Production")]
    public class ImportMembershipLastActivityDateToAuthenticationLogsBug2476 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
insert into AuthenticationLogs (UserId, IpAddress, LoggedInAt, AuthCookieHash, ExpiresAt)
select
 UserId = p.RecId,
 IpAddress = '0.0.0.0',
 LoggedInAt = au.LastActivityDate,
 AuthCookieHash = NEWID(),
 ExpiresAt = au.LastActivityDate
from tblPermissions p
inner join aspnet_Users au on au.UserId = p.uid 
where not exists(select * from AuthenticationLogs where AuthenticationLogs.UserId = p.RecId)");
        }

        public override void Down()
        {
            // No need for a down for this.
        }
    }
}
