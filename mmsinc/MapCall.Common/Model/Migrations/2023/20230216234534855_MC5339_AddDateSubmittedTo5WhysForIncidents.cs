using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230216234534855), Tags("Production")]
    public class MC5339_AddDateSubmittedTo5WhysForIncidents : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents")
                 .AddColumn("DateSubmitted").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("DateSubmitted").FromTable("Incidents");
        }
    }
}

