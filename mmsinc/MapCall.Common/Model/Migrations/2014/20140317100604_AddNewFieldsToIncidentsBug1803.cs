using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140317100604), Tags("Production")]
    public class AddNewFieldsToIncidentsBug1803 : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents")
                 .AddColumn("QuestionHaveHadSimilarInjuryBefore")
                 .AsBoolean()
                 .WithDefaultValue(false)
                 .NotNullable()
                 .AddColumn("PriorInjuryDate")
                 .AsDateTime()
                 .Nullable()
                 .AddColumn("NatureOfPriorInjury")
                 .AsCustom("ntext")
                 .Nullable()
                 .AddColumn("PriorInjuryMedicalProvider")
                 .AsCustom("ntext")
                 .Nullable();

            Alter.Table("Incidents")
                 .AddColumn("QuestionParticipatedInAthleticActivitiesInLastTwelveMonths")
                 .AsBoolean()
                 .WithDefaultValue(false)
                 .NotNullable()
                 .AddColumn("AthleticActivitiesInLastTwelveMonths")
                 .AsCustom("ntext")
                 .Nullable();

            Alter.Table("Incidents")
                 .AddColumn("QuestionHaveJobOutsideOfAmericanWater")
                 .AsBoolean()
                 .WithDefaultValue(false)
                 .NotNullable()
                 .AddColumn("OtherEmployers")
                 .AsCustom("ntext")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("QuestionHaveHadSimilarInjuryBefore").FromTable("Incidents");
            Delete.Column("PriorInjuryDate").FromTable("Incidents");
            Delete.Column("NatureOfPriorInjury").FromTable("Incidents");
            Delete.Column("PriorInjuryMedicalProvider").FromTable("Incidents");
            Delete.Column("QuestionParticipatedInAthleticActivitiesInLastTwelveMonths").FromTable("Incidents");
            Delete.Column("AthleticActivitiesInLastTwelveMonths").FromTable("Incidents");
            Delete.Column("QuestionHaveJobOutsideOfAmericanWater").FromTable("Incidents");
            Delete.Column("OtherEmployers").FromTable("Incidents");
        }
    }
}
