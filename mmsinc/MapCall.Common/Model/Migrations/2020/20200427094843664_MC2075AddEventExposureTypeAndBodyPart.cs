using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200427094843664), Tags("Production")]
    public class MC2075AddEventExposureTypeAndBodyPart : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("EventExposureTypes",
                "Acute Exposure / Contact with Harmful Substance",
                "Chronic Exposure / Contact with Harmful Substance",
                "Foreign Body",
                "Slip/Trip",
                "Ergonomic - Awkward Position/Twisting",
                "Ergonomic - Overexertion",
                "Ergonomic - Repetition",
                "Line of Fire",
                "Fire/Explosion",
                "Fall from Height",
                "Temperature",
                "Transportation Incident",
                "Assault",
                "Animal/Insect");

            this.CreateLookupTableWithValues("BodyParts",
                "Head",
                "Neck",
                "Back",
                "Shoulder",
                "Arm",
                "Hand",
                "Eye",
                "Leg",
                "Foot",
                "Abdomen/Chest",
                "Groin",
                "None Identified");

            Create.Table("IncidentsBodyParts")
                  .WithForeignKeyColumn("IncidentId", "Incidents").NotNullable()
                  .WithForeignKeyColumn("BodyPartId", "BodyParts").NotNullable();

            Alter.Table("Incidents")
                 .AddForeignKeyColumn("EventExposureTypeId", "EventExposureTypes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Incidents", "EventExposureTypeId", "EventExposureTypes");
            Delete.Table("IncidentsBodyParts");
            Delete.Table("EventExposureTypes");
            Delete.Table("BodyParts");
        }
    }
}
