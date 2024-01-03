using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170404140248833), Tags("Production")]
    public class EmployeeStatusesBug3684 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("EmployeeStatuses").Row(new {Description = "Withdrawn"});
            Insert.IntoTable("EmployeeStatuses").Row(new {Description = "Retiree"});
        }

        public override void Down()
        {
            Delete.FromTable("EmployeeStatuses").Row(new {Description = "Withdrawn"});
            Delete.FromTable("EmployeeStatuses").Row(new {Description = "Retiree"});
        }
    }
}
