using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150612140259155), Tags("Production")]
    public class AddFKConstraintForValveImagesValvesForBug2433 : Migration
    {
        public override void Up()
        {
            Execute.Sql("update ValveImages set ValveId = null where ValveId not in (select id from valves)");
            Alter.Column("ValveId")
                 .OnTable("ValveImages")
                 .AsInt32()
                 .ForeignKey(Utilities.CreateForeignKeyName("ValveImages", "Valves", "ValveId"), "Valves", "Id")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey(Utilities.CreateForeignKeyName("ValveImages", "Valves", "ValveId"))
                  .OnTable("ValveImages");
        }
    }
}
