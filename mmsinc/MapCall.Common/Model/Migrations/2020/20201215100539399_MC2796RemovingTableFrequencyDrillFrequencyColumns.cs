using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201215100539399), Tags("Production")]
    public class MC2796RemovingTableFrequencyDrillFrequencyColumns : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("EmergencyResponsePlans", "TabletopFrequencyId", "TabletopFrequencies");
            Delete.ForeignKeyColumn("EmergencyResponsePlans", "DrillFrequencyId", "DrillFrequencies");
            this.CreateLookupTableWithValues("ReviewFrequencies", "Quarterly", "Semiannual", "Annual", "2 Year",
                "3 Year", "4 Year", "5 Year", "Other");

            Alter.Table("EmergencyResponsePlans")
                 .AddForeignKeyColumn("ReviewFrequencyId", "ReviewFrequencies");
        }

        public override void Down()
        {
            Alter.Table("EmergencyResponsePlans")
                 .AddForeignKeyColumn("TabletopFrequencyId", "TabletopFrequencies").Nullable()
                 .AddForeignKeyColumn("DrillFrequencyId", "DrillFrequencies").Nullable();
            Delete.ForeignKeyColumn("EmergencyResponsePlans", "ReviewFrequencyId", "ReviewFrequencies");
        }
    }
}
