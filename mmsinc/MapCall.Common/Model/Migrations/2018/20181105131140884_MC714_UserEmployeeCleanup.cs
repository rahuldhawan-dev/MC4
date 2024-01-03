using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181105131140884), Tags("Production")]
    public class MC714_UserEmployeeCleanup : Migration
    {
        public override void Up()
        {
            Create.Column("EmployeeId").OnTable("tblPermissions")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_tblPermissions_tblEmployee_EmployeeId", "tblEmployee", "tblEmployeeId");

            // Clean up EmpNum with white spaces
            Execute.Sql("update tblPermissions set EmpNum = LTRIM(RTRIM(EmpNum)) where EmpNum is not null");

            // Clean up a few duplicated EmpNums that cause duplicate EmployeeId values to appear later in the migration
            Execute.Sql(
                "update tblPermissions set EmpNum = null where username = 'nsantillasr'"); // User only logged in once.
            // Execute.Sql("update tblPermissions set EmpNum = '50305298' where username = 'santilnw'");
            Execute.Sql(
                "update tblPermissions set EmpNum = null where username = 'wilkenrl'"); // User with two accounts, this one had his lastname wrong and was never used.
            Execute.Sql(
                "update tblPermissions set EmpNum = '60002244' where username = 'walkertb'"); // User account had wrong employee number.

            // This is a legit employee number for someone, but not for the two users that have it set.
            Execute.Sql("update tblPermissions set EmpNum = null where EmpNum = '12345678'");

            Execute.Sql(
                "update tblPermissions set EmployeeId = (select tblEmployeeId from tblEmployee where EmployeeId = EmpNum) where EmployeeId is null and EmpNum is not null");

            // The Employees table doesn't have employee numbers with "0" at the front. We need to check for these too.
            Execute.Sql(
                "update tblPermissions set EmployeeId = (select tblEmployeeId from tblEmployee where CONCAT('0', EmployeeId) = EmpNum) where EmployeeId is null and EmpNum is not null");

            // Clean up a few non-unique users

            // Creates a unique index for User.EmployeeId that allows multiple nulls.
            Execute.Sql(
                "CREATE UNIQUE INDEX IX_UQ_tblPermissions_EmployeeId ON tblPermissions(EmployeeId) WHERE EmployeeId IS NOT NULL");
        }

        public override void Down()
        {
            Execute.Sql("DROP INDEX IX_UQ_tblPermissions_EmployeeId on tblPermissions");

            Delete.ForeignKey("FK_tblPermissions_tblEmployee_EmployeeId").OnTable("tblPermissions");
            Delete.Column("EmployeeId").FromTable("tblPermissions");
        }
    }
}
