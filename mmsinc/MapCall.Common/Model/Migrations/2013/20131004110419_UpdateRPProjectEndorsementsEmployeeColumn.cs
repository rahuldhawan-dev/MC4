using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131004110419), Tags("Production")]
    public class UpdateRPProjectEndorsementsEmployeeColumn : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string UPDATE_NAMES = "UPDATE [" + TABLE_NAME + "] SET [" + Columns.EMPLOYEE +
                                               "] = P.UserName	FROM RPProjectEndorsements PE JOIN tblEmployee E ON E.tblEmployeeID = pe.tblEmployeeID LEFT JOIN tblPermissions P on P.EmpNum = E.EmployeeID",
                                ROLLBACK_NAMES = "UPDATE [" + TABLE_NAME + "] SET [" + Columns.EMPLOYEE +
                                                 "] = E.tblEmployeeID FROM RPProjectEndorsements pe LEFT JOIN tblPermissions P on P.username = pe.tblEmployeeID LEFT JOIN tblEmployee E ON cast(E.EmployeeID as varchar)= p.EmpNum where E.tblEmployeeId IS NOT NULL;";
        }

        public const string TABLE_NAME = "RPProjectEndorsements";

        public struct Columns
        {
            public const string EMPLOYEE = "tblEmployeeID";
        }

        public struct StringLengths
        {
            public const int EMPLOYEE = 20;
        }

        #endregion

        public override void Up()
        {
            Delete.ForeignKey("FK_RPProjectEndorsements_tblEmployee_tblEmployeeID").OnTable(TABLE_NAME);
            Alter.Table(TABLE_NAME).AlterColumn(Columns.EMPLOYEE).AsAnsiString(StringLengths.EMPLOYEE).NotNullable();
            Execute.Sql(Sql.UPDATE_NAMES);
        }

        public override void Down()
        {
            Execute.Sql(Sql.ROLLBACK_NAMES);
            // no need to convert back to int and put the foreign key back
        }
    }
}
