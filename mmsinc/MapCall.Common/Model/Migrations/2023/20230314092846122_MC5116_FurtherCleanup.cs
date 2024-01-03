using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20230314092846122), Tags("Production")]
    public class MC5116_FurtherCleanup : Migration
    {
        public override void Up()
        {
            // this column seemed to have tracked the same date as DateAdded, which has been renamed to
            // CreatedAt
            Delete.Column("CreatedOn").FromTable("Hydrants");
            Delete.Column("CreatedOn").FromTable("ValveInspections");
            
            // same, only this was just a date
            Delete.Column("DateAdded").FromTable("Vehicles");
        }

        public override void Down()
        {
            Create.Column("DateAdded").OnTable("Vehicles").AsDateTime().Nullable();
            Create.Column("CreatedOn").OnTable("ValveInspections").AsDateTime().Nullable();
            Create.Column("CreatedOn").OnTable("Hydrants").AsDateTime().Nullable();
        }
    }
}

