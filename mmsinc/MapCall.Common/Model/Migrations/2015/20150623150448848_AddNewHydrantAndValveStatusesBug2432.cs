using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150623150448848), Tags("Production")]
    public class AddNewHydrantAndValveStatusesBug2432 : Migration
    {
        public override void Up()
        {
            Alter.Table("HydrantStatuses")
                 .AddColumn("IsUserAdminOnly").AsBoolean().NotNullable().WithDefaultValue(true);
            Alter.Table("ValveStatuses")
                 .AddColumn("IsUserAdminOnly").AsBoolean().NotNullable().WithDefaultValue(true);

            this.EnableIdentityInsert("HydrantStatuses");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM HydrantStatuses WHERE Id = 5) INSERT INTO HydrantStatuses(Id, Description, IsUserAdminOnly) VALUES(5, 'INSTALLED', 0)");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM HydrantStatuses WHERE Id = 6) INSERT INTO HydrantStatuses(Id, Description, IsUserAdminOnly) VALUES(6, 'REQUEST RETIREMENT', 0)");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM HydrantStatuses WHERE Id = 7) INSERT INTO HydrantStatuses(Id, Description, IsUserAdminOnly) VALUES(7, 'REQUEST CANCELLATION', 0)");
            //Insert.IntoTable("HydrantStatuses")
            //    .Row(new { Id = 5, Description = "INSTALLED", IsUserAdminOnly = false })
            //    .Row(new { Id = 6, Description = "REQUEST RETIREMENT", IsUserAdminOnly = false })
            //    .Row(new { Id = 7, Description = "REQUEST CANCELLATION", IsUserAdminOnly = false });
            this.DisableIdentityInsert("HydrantStatuses");

            this.EnableIdentityInsert("ValveStatuses");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM ValveStatuses WHERE Id = 8) INSERT INTO ValveStatuses(Id, Description, IsUserAdminOnly) Values(8, 'INSTALLED', 0)");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM ValveStatuses WHERE Id = 9) INSERT INTO ValveStatuses(Id, Description, IsUserAdminOnly) Values(9, 'REQUEST RETIREMENT', 0)");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM ValveStatuses WHERE Id = 10) INSERT INTO ValveStatuses(Id, Description, IsUserAdminOnly) Values(10, 'REQUEST CANCELLATION', 0)");
            //Insert.IntoTable("ValveStatuses")
            //    .Row(new { Id = 8, Description = "INSTALLED", IsUserAdminOnly = false })
            //    .Row(new { Id = 9, Description = "REQUEST RETIREMENT", IsUserAdminOnly = false })
            //    .Row(new { Id = 10, Description = "REQUEST CANCELLATION", IsUserAdminOnly = false });
            this.DisableIdentityInsert("ValveStatuses");
        }

        public override void Down()
        {
            //Execute.Sql("UPDATE Valves set ValveStatusId = NULL where ValveStatusID in (5,6,7)");
            //Delete.FromTable("ValveStatuses")
            //      .Row(new { Description = "INSTALLED" })
            //      .Row(new { Description = "REQUEST RETIREMENT" })
            //      .Row(new { Description = "REQUEST CANCELLATION" });

            //Execute.Sql("UPDATE Hydrants set HydrantStatusId = NULL where HydrantStatusID in (8,9,10)");
            //Delete.FromTable("HydrantStatuses")
            //      .Row(new { Description = "INSTALLED" })
            //      .Row(new { Description = "REQUEST RETIREMENT" })
            //      .Row(new { Description = "REQUEST CANCELLATION" });

            Delete.Column("IsUserAdminOnly").FromTable("ValveStatuses");
            Delete.Column("IsUserAdminOnly").FromTable("HydrantStatuses");
        }
    }
}
