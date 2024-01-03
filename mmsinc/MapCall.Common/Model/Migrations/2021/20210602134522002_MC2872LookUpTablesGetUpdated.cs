using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210602134522002), Tags("Production")]
    public class MC2872LookUpTablesGetUpdated : Migration
    {
        public override void Up()
        {
            Rename.Table("HazardApproachRecommended").To("HazardApproachRecommendedTypes");
        }

        public override void Down()
        {
            Rename.Table("HazardApproachRecommendedTypes").To("HazardApproachRecommended");
        }
    }
}