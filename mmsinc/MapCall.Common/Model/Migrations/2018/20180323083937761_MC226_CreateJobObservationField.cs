using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180323083937761), Tags("Production")]
    public class MC226 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblJobObservations")
                 .AddForeignKeyColumn("JobObservedById", "tblEmployee", "tblEmployeeID");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblJobObservations_tblEmployee_EmployeeID").OnTable("tblJobObservations");
            Delete.Column("JobObservedById").FromTable("tblJobObservations");
        }
    }
}
