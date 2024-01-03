using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220906084620969), Tags("Production")]
    public class MC3994AddHoursWorkedToEmployeeAssignmentTable : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("EmployeeAssignments")
                 .AddColumn("HoursWorked")
                 .AsDecimal(6, 2)
                 .NotNullable()
                 .WithDefaultValue(0);
        }
    }
}
