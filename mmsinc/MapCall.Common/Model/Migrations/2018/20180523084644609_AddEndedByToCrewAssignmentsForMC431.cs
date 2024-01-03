using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180523084644609), Tags("Production")]
    public class AddEndedByToCrewAssignmentsForMC431 : Migration
    {
        public override void Up()
        {
            Alter.Table("CrewAssignments")
                 .AddForeignKeyColumn("StartedById", "tblPermissions",
                      "RecId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("CrewAssignments", "StartedById",
                "tblPermissions", "RecId");
        }
    }
}
