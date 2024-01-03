using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220104150856192), Tags("Production")]
    public class MC3939AddingSubmittedBehalfOfEmployeeToNearMisses : Migration
    {
        public override void Up()
        {
            Alter.Table("NearMisses")
                 .AddColumn("SubmittedOnBehalfOfAnotherEmployee").AsBoolean().Nullable()
                 .AddForeignKeyColumn("EmployeeID", "tblEmployee", "tblEmployeeID");
        }

        public override void Down()
        {
            Delete.Column("SubmittedOnBehalfOfAnotherEmployee").FromTable("NearMisses");
            Delete.ForeignKeyColumn("NearMisses", "EmployeeID", "tblEmployee", "tblEmployeeID");
        }
    }
}

