using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161107104610635), Tags("Production")]
    public class IncidentsBug3306 : Migration
    {
        public override void Up()
        {
            Create.Table("IncidentNurseRecommendationTypes")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Description").AsString(100).Unique().NotNullable();

            Insert.IntoTable("IncidentNurseRecommendationTypes")
                  .Row(new {Description = "Medical Evaluation Recommended"});
            Insert.IntoTable("IncidentNurseRecommendationTypes").Row(new
                {Description = "Non Medical Treatment Recommendation, No Action Required"});

            Alter.Table("Incidents")
                 .AddColumn("EmployeeSpokeToNurse").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("IncidentNurseRecommendationTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_Incidents_IncidentNurseRecommendationTypes_IncidentNurseRecommendationTypesId",
                      "IncidentNurseRecommendationTypes", "Id")
                 .AddColumn("RecommendedMedicalProvider").AsString(255).Nullable()
                 .AddColumn("NonMedicalTreatmentRecommendation").AsString(255).Nullable()
                 .AddColumn("EmployeeAcceptedRecommendationByNurse").AsBoolean().Nullable()
                 .AddColumn("ReasonEmployeeDidNotAcceptRecommendationByNurse").AsCustom("ntext").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Incidents_IncidentNurseRecommendationTypes_IncidentNurseRecommendationTypesId")
                  .OnTable("Incidents");

            Delete.Column("EmployeeSpokeToNurse").FromTable("Incidents");
            Delete.Column("IncidentNurseRecommendationTypeId").FromTable("Incidents");
            Delete.Column("RecommendedMedicalProvider").FromTable("Incidents");
            Delete.Column("NonMedicalTreatmentRecommendation").FromTable("Incidents");
            Delete.Column("EmployeeAcceptedRecommendationByNurse").FromTable("Incidents");
            Delete.Column("ReasonEmployeeDidNotAcceptRecommendationByNurse").FromTable("Incidents");

            Delete.Table("IncidentNurseRecommendationTypes");
        }
    }
}
