using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150828093748057), Tags("Production")]
    public class AddAtRiskBehaviorToIncidentsForBug2582 : Migration
    {
        public const string TABLE_NAME = "AtRiskBehaviors";

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(30);

            Alter.Table("Incidents")
                 .AddForeignKeyColumn("AtRiskBehaviorId", TABLE_NAME);

            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Work Environment');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('PPE');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Body Position');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Ergonomics');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Procedures');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Working Surface');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Housekeeping');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Body Motion');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Line of Fire');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('Push Pull');", TABLE_NAME);
            Execute.Sql("INSERT INTO {0} (Description) VALUES ('None Associated');", TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Incidents", "AtRiskBehaviorId", TABLE_NAME);
            Delete.Table(TABLE_NAME);
        }
    }
}
