using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170221093332518), Tags("Production")]
    public class Bug3610MainCrossings : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("MainCrossingConsequenceOfFailureTypes", "Review Required", "High",
                "Medium", "Low");
            this.CreateLookupTableWithValues("MainCrossingImpactToTypes", "Highway", "Large Customer",
                "Medical Facility", "School", "Single Point of Failulre", "Railroad");

            // Add column for failure type
            Create.Column("MainCrossingConsequenceOfFailureTypeId").OnTable("MainCrossings")
                  .AsInt32().Nullable()
                  .ForeignKey(
                       "FK_MainCrossings_MainCrossingConsequenceOfFailureTypes_MainCrossingConsequenceOfFailureTypeId",
                       "MainCrossingConsequenceOfFailureTypes", "Id");

            // Set default to Medium for all existing values
            Execute.Sql(@"
    declare @medium int;
    set @medium = (select top 1 Id from MainCrossingConsequenceOfFailureTypes where Description = 'Review Required')

    update MainCrossings set MainCrossingConsequenceOfFailureTypeId = @medium
");

            Alter.Column("MainCrossingConsequenceOfFailureTypeId").OnTable("MainCrossings").AsInt32().NotNullable();

            // Add many-to-many table for impact to types
            Create.Table("MainCrossingsImpactToTypes")
                  .WithColumn("MainCrossingId").AsInt32().NotNullable()
                  .ForeignKey("FK_MainCrossingsImpactToTypes_MainCrossings_MainCrossingId", "MainCrossings",
                       "MainCrossingId")
                  .WithColumn("MainCrossingImpactToTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_MainCrossingsImpactToTypes_MainCrossings_MainCrossingImpactToTypeId",
                       "MainCrossingImpactToTypes", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_MainCrossingsImpactToTypes_MainCrossings_MainCrossingImpactToTypeId")
                  .OnTable("MainCrossingsImpactToTypes");
            Delete.ForeignKey("FK_MainCrossingsImpactToTypes_MainCrossings_MainCrossingId")
                  .OnTable("MainCrossingsImpactToTypes");
            Delete.Table("MainCrossingsImpactToTypes");

            Delete.ForeignKey(
                       "FK_MainCrossings_MainCrossingConsequenceOfFailureTypes_MainCrossingConsequenceOfFailureTypeId")
                  .OnTable("MainCrossings");
            Delete.Column("MainCrossingConsequenceOfFailureTypeId").FromTable("MainCrossings");

            Delete.Table("MainCrossingImpactToTypes");
            Delete.Table("MainCrossingConsequenceOfFailureTypes");
        }
    }
}
