using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200406125248285), Tags("Production")]
    public class MC1757AddAssignedByFieldAndBackfillDataForEmployeeAssignment : Migration
    {
        public const string UPDATE_SQL =
            @"SELECT te.tblEmployeeID, EA.Id INTO #myData FROM AuditLogEntries ALE 
                INNER JOIN tblPermissions tp ON tp.userID = ALE.UserId
                INNER JOIN tblEmployee te ON te.tblEmployeeID = tp.EmployeeId 
                INNER JOIN EmployeeAssignments EA ON EA.AssignedOn = ALE.Timestamp 
                WHERE EntityName = 'EmployeeAssignment' 
                AND FieldName = 'AssignedTo' 
                AND EA.AssignedToId = CONVERT(INT, CONVERT(VARCHAR(5),ale.NewValue));

            UPDATE EmployeeAssignments 
                SET AssignedById = md.tblEmployeeId 
                FROM #myData md 
                WHERE md.Id = EmployeeAssignments.Id;
            
            DROP TABLE #myData";

        public override void Up()
        {
            Alter.Table("EmployeeAssignments").AddForeignKeyColumn("AssignedById", "tblEmployee", "tblEmployeeID");

            // Backfill the AssignedBy user, yay
            Execute.Sql(UPDATE_SQL);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EmployeeAssignments", "AssignedById", "tblEmployee", "tblEmployeeID");
        }
    }
}
