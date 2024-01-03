using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200806182550166), Tags("Production")]
    public class MC93AddPremisesCriticalCareTypes : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("PremiseCriticalCareTypes",
                "Critical Illness",
                "Dialysis",
                "Hospital",
                "Large Recreation",
                "Large User",
                "Manufacturing",
                "Nursing Home",
                "Other",
                "Public Authority",
                "School",
                "Stable",
                "University",
                "Wholesale");
            Alter.Table("Premises").AddForeignKeyColumn("CriticalCareTypeId", "PremiseCriticalCareTypes").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Premises", "CriticalCareTypeId", "PremiseCriticalCareTypes");
            Delete.Table("PremiseCriticalCareTypes");
        }
    }
}
