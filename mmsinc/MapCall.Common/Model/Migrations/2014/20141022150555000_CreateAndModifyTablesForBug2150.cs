using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141022150555000), Tags("Production")]
    public class CreateAndModifyTablesForBug2150 : Migration
    {
        // I get sad when the const names are longer than the values. -Ross
        private const string TABLE_RECURRING_FREQUENCY_UNITS = "RecurringFrequencyUnits",
                             TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING =
                                 "MainCrossingInspectionAssessmentRatings",
                             TABLE_MAIN_CROSSINGS = "MainCrossings",
                             TABLE_MAIN_CROSSING_INSPECTIONS = "MainCrossingInspections";

        public override void Up()
        {
            Create.Table(TABLE_RECURRING_FREQUENCY_UNITS)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable().Unique();

            Insert.IntoTable(TABLE_RECURRING_FREQUENCY_UNITS).Row(new {Description = "Day"});
            Insert.IntoTable(TABLE_RECURRING_FREQUENCY_UNITS).Row(new {Description = "Week"});
            Insert.IntoTable(TABLE_RECURRING_FREQUENCY_UNITS).Row(new {Description = "Month"});
            Insert.IntoTable(TABLE_RECURRING_FREQUENCY_UNITS).Row(new {Description = "Year"});

            Create.Table(TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable().Unique();

            Insert.IntoTable(TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING)
                  .Row(new {Description = "Poor - Immediate Action Required"});
            Insert.IntoTable(TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING)
                  .Row(new {Description = "Fair - NO Immediate Action Required"});
            Insert.IntoTable(TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING).Row(new {Description = "Good"});
            Insert.IntoTable(TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING).Row(new {Description = "Excellent"});

            Alter.Table(TABLE_MAIN_CROSSINGS)
                 .AddColumn("InspectionFrequency").AsInt32().NotNullable().WithDefaultValue(1)
                 .AddColumn("InspectionFrequencyUnitId")
                 .AsInt32()
                 .Nullable() // Needs to be nullable at first, later sql can grab the Year freq value, set it, then set this to not nullable.
                 .ForeignKey("FK_RecurringFrequencyUnits_InspectionFrequencyUnitId", TABLE_RECURRING_FREQUENCY_UNITS,
                      "Id");

            Execute.Sql(@"
    declare @yearFreq int; set @yearFreq = (select top 1 Id from RecurringFrequencyUnits where Description = 'Year');
    update MainCrossings set InspectionFrequencyUnitId = @yearFreq
");

            Alter.Column("InspectionFrequencyUnitId")
                 .OnTable(TABLE_MAIN_CROSSINGS)
                 .AsInt32()
                 .NotNullable();

            Create.Table(TABLE_MAIN_CROSSING_INSPECTIONS)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("MainCrossingId").AsInt32().NotNullable()
                  .ForeignKey("FK_MainCrossings_MainCrossingId", "MainCrossings", "MainCrossingID")
                  .WithColumn("InspectedOn").AsDateTime().NotNullable()
                  .WithColumn("InspectedById").AsInt32()
                  .NotNullable()
                  .ForeignKey("FK_tblPermissions_InspectedById", "tblPermissions", "RecID")
                  .WithColumn("CreatedById").AsInt32()
                  .NotNullable()
                  .ForeignKey("FK_tblPermissions_CreatedById", "tblPermissions", "RecID")
                  .WithColumn("AssessmentRatingId").AsInt32().NotNullable()
                  .ForeignKey("FK_MainCrossingInspectionAssessmentRatings_AssessmentRatingId",
                       TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING, "Id")
                  .WithColumn("Comments").AsCustom("text").Nullable()
                  .WithColumn("PipeIsInService").AsBoolean().NotNullable()
                  .WithColumn("PipeHasExcessiveCorrosion").AsBoolean().NotNullable()
                  .WithColumn("PipeHasDelaminatedSteel").AsBoolean().NotNullable()
                  .WithColumn("PipeIsDamaged").AsBoolean().NotNullable()
                  .WithColumn("PipeHasCracks").AsBoolean().NotNullable()
                  .WithColumn("PipeHasConcreteSpools").AsBoolean().NotNullable()
                  .WithColumn("PipeLacksInsulation").AsBoolean().NotNullable()
                  .WithColumn("JointsAreLeaking").AsBoolean().NotNullable()
                  .WithColumn("JointsFailedSeparated").AsBoolean().NotNullable()
                  .WithColumn("JointsRestraintDamaged").AsBoolean().NotNullable()
                  .WithColumn("JointsBondStrapsDamaged").AsBoolean().NotNullable()
                  .WithColumn("SupportsHaveDeficientSupport").AsBoolean().NotNullable()
                  .WithColumn("SupportsAreDamaged").AsBoolean().NotNullable()
                  .WithColumn("SupportsHaveCorrosion").AsBoolean().NotNullable()
                  .WithColumn("EnvironmentIsInHazardousLocation").AsBoolean().NotNullable()
                  .WithColumn("EnvironmentHasDebrisBuildUp").AsBoolean().NotNullable()
                  .WithColumn("EnvironmentIsSubmergedInWater").AsBoolean().NotNullable()
                  .WithColumn("EnvironmentIsExposedToVehicleImpact").AsBoolean().NotNullable()
                  .WithColumn("EnvironmentIsNotSecuredFromPublic").AsBoolean().NotNullable()
                  .WithColumn("EnvironmentIsSusceptibleToStormDamage").AsBoolean().NotNullable()
                  .WithColumn("AdjacentFacilityHasBankErosion").AsBoolean().NotNullable()
                  .WithColumn("AdjacentFacilityHasBridgeDamage").AsBoolean().NotNullable()
                  .WithColumn("AdjacentFacilityHasPavementFailure").AsBoolean().NotNullable()
                  .WithColumn("AdjacentFacilityOverheadPowerLinesAreDown").AsBoolean().NotNullable()
                  .WithColumn("AdjacentFacilityHasPropertyDamage").AsBoolean().NotNullable();

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('MainCrossingInspections', 'MainCrossingInspections')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Main Crossing Inspection', @dataTypeId)");
        }

        public override void Down()
        {
            this.DeleteDataType("MainCrossingInspections");

            Delete.ForeignKey("FK_MainCrossings_MainCrossingId").OnTable(TABLE_MAIN_CROSSING_INSPECTIONS);
            Delete.ForeignKey("FK_MainCrossingInspectionAssessmentRatings_AssessmentRatingId")
                  .OnTable(TABLE_MAIN_CROSSING_INSPECTIONS);
            Delete.ForeignKey("FK_tblPermissions_InspectedById").OnTable(TABLE_MAIN_CROSSING_INSPECTIONS);
            Delete.ForeignKey("FK_tblPermissions_CreatedById").OnTable(TABLE_MAIN_CROSSING_INSPECTIONS);
            Delete.Table(TABLE_MAIN_CROSSING_INSPECTIONS);
            Delete.ForeignKey("FK_RecurringFrequencyUnits_InspectionFrequencyUnitId").OnTable(TABLE_MAIN_CROSSINGS);
            Delete.Column("InspectionFrequencyUnitId").FromTable(TABLE_MAIN_CROSSINGS);
            Delete.Column("InspectionFrequency").FromTable(TABLE_MAIN_CROSSINGS);
            Delete.Table(TABLE_MAIN_CROSSING_INSPECTION_ASSESSMENT_RATING);
            Delete.Table(TABLE_RECURRING_FREQUENCY_UNITS);
        }
    }
}
