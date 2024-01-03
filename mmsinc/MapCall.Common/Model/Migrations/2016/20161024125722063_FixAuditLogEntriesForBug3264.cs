using FluentMigrator;

namespace MapCall.Common
{
    [Migration(20161024125722063), Tags("Production")]
    public class FixAuditLogEntriesForBug3264 : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE AuditLogEntries SET EntityName = 'Hydrant' WHERE EntityName = 'HydrantProxy';" +
                        "UPDATE AuditLogEntries SET EntityName = 'Employee' WHERE EntityName = 'EmployeeProxy';" +
                        "UPDATE AuditLogEntries SET EntityName = 'TrainingRecord' WHERE EntityName = 'TrainingRecordProxy'");
        }

        public override void Down() { }
    }
}
