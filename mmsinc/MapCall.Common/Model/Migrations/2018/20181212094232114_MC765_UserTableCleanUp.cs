using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181212094232114), Tags("Production")]
    public class MC765_UserTableCleanUp : Migration
    {
        private void CleanupData()
        {
            // There is an immense amount of bad data that needs to be cleaned up so we can ensure
            // data integrity with changes to the schema.

            // 15 rows. Useless data without the UserLogId.
            Execute.Sql("delete from UserViewed where UserLogId is null");

            // 99 rows. This is a dead column.
            Execute.Sql("delete from UserViewed where ProductionId is not null");

            // 73 rows. Useless data.
            Execute.Sql(
                "delete from UserViewed where TapID is null and AsBuiltId is null and MapId is null and ValveId is null");

            // There are 14k UserViewed.UserId values that are null. About 1k can be linked back up via the associated UserLog.
            Execute.Sql(
                "update UserViewed set UserId = (select UserLog.UserId from UserLog where UserLog.UserLogId = UserViewed.UserLogId) where UserId is null");

            // There are 90k rows where the UserId references a UserTable record that does not reference the correct tblPermissions user.
            // This corrects the UserId by linking it to the UserTable record that correctly references tblPermissions by username.
            Execute.Sql(@"update uv 
set
	uv.UserId = ut2.UserId
from UserViewed uv 
left join UserTable on UserTable.UserId = uv.UserId -- This has the bad UserId that we don't want
left join tblPermissions on tblPermissions.uid = UserTable.uid 
inner join tblPermissions tp2 on tp2.UserName = UserTable.UserLogin
inner join UserTable ut2 on ut2.uid = tp2.uid
where tblPermissions.RecId is null ");

            // Remove any data where UserId is null. At this point there's no way to link it back to a proper user. There are about 13k rows like this.
            Execute.Sql("delete from UserViewed where UserId is null");

            // Also remove any data where UserId can't link back to UserTable. This is impossible to link to tblPermissions. There are about 20k rows like this.
            Execute.Sql(
                "delete from UserViewed where UserViewed.UserId not in (select distinct UserId from UserTable)");
        }

        public override void Up()
        {
            // Perform data clean up first
            CleanupData();

            Rename.Column("UserId").OnTable("UserViewed").To("UserTableId");
            // Add new UserId column with proper foreign key. Not nullable.

            Create.Column("UserId").OnTable("UserViewed")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_UserViewed_tblPermissions_UserId", "tblPermissions", "RecId");

            // This takes about a full minute to run.
            Execute.Sql(@"update uv 
set uv.UserId = tblPermissions.RecId
from UserViewed uv
left join UserTable on UserTable.UserId = uv.UserTableId
left join tblPermissions on tblPermissions.uid = UserTable.uid");

            // Delete rows that have null UserId. These can not be associated with tblPermissions.
            // There are about 200k rows but they all look seemingly ancient(2010 or older).
            Execute.Sql("delete from UserViewed where UserId is null");

            // Make UserId column not nullable.
            Alter.Column("UserId").OnTable("UserViewed").AsInt32().NotNullable();

            // Add UserViewed.DateViewed
            Create.Column("ViewedAt").OnTable("UserViewed").AsDateTime().Nullable();

            // Populate DateViewed information. This takes about 2 minutes to run.
            Execute.Sql(@"update uv 
set uv.ViewedAt = UserLog.UserStartTime 
from UserViewed uv 
join UserLog on UserLog.UserLogId = uv.UserLogId ");

            Alter.Column("ViewedAt").OnTable("UserViewed").AsDateTime().NotNullable();

            Delete.Column("ProductionId").FromTable("UserViewed");
        }

        public override void Down()
        {
            Delete.Column("ViewedAt").FromTable("UserViewed");

            Delete.ForeignKey("FK_UserViewed_tblPermissions_UserId").OnTable("UserViewed");
            Delete.Column("UserId").FromTable("UserViewed");

            Create.Column("ProductionId").OnTable("UserViewed").AsInt32().Nullable();

            Rename.Column("UserTableId").OnTable("UserViewed").To("UserId");
        }
    }
}
