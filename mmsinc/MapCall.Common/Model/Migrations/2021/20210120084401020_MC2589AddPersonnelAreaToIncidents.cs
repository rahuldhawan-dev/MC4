using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210120084401020), Tags("Production")]
    public class MC2589AddPersonnelAreaToIncidents : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents")
                 .AddForeignKeyColumn("PersonnelAreaId", "PersonnelAreas", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Incidents", "PersonnelAreaId", "PersonnelAreas", "Id");
        }
    }
}
