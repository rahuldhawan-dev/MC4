using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151008103649854), Tags("Production")]
    public class AtRiskBehaviorsBug2639 : Migration
    {
        private const string SUB_SECTIONS = "AtRiskBehaviorSubSections",
                             SECTIONS = "AtRiskBehaviorSections";

        public override void Up()
        {
            Create.Table(SECTIONS)
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Description").AsString(50).NotNullable().Unique()
                  .WithColumn("SectionNumber").AsInt32().NotNullable();

            Action<string, int> addSect = (sect, sectNum) => {
                Insert.IntoTable(SECTIONS).Row(new {Description = sect, SectionNumber = sectNum});
            };

            addSect("PPE", 1);
            addSect("Work Environment", 2);
            addSect("Body Locations", 3);
            addSect("Tools & Equipment", 4);
            addSect("Body Motion / Ergo", 5);
            addSect("Procedures", 6);
            addSect("Other", 7);

            Create.Table(SUB_SECTIONS)
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Description").AsString(50)
                  .WithForeignKeyColumn("AtRiskBehaviorSectionId", SECTIONS, "Id", nullable: false)
                  .WithColumn("SubSectionNumber").AsDecimal(2, 1).NotNullable();

            Action<string, int, decimal> addSubSect = (subSectDesc, sectNum, subSectNum) => {
                const string format = @"declare @sectionId int
                    set @sectionId = (select top 1 Id from AtRiskBehaviorSections where SectionNumber = {0})
                    insert into AtRiskBehaviorSubSections (Description, AtRiskBehaviorSectionId, SubSectionNumber) VALUES ('{1}', @sectionId, {2})";
                Execute.Sql(format, sectNum, subSectDesc, subSectNum);
            };

            // PPE
            addSubSect("Eye/Face", 1, 0.1m);
            addSubSect("Hearing", 1, 0.2m);
            addSubSect("Respiratory", 1, 0.3m);
            addSubSect("Hands", 1, 0.4m);
            addSubSect("Body/Arms/Legs", 1, 0.5m);
            addSubSect("Feet", 1, 0.6m);
            addSubSect("Head", 1, 0.7m);

            // Work Environment
            addSubSect("Walking/Working Surfaces", 2, 0.1m);
            addSubSect("Housekeeping", 2, 0.2m);
            addSubSect("Job Set Up", 2, 0.3m);
            addSubSect("Weather", 2, 0.4m);

            // Body Locations
            addSubSect("Line of Fire", 3, 0.1m);
            addSubSect("Pinch Point", 3, 0.2m);
            addSubSect("Eyes on Path/Roadway", 3, 0.3m);
            addSubSect("Eyes on Task/Hands", 3, 0.4m);
            addSubSect("Ascending/Descending", 3, 0.5m);

            // Tools & Equipment
            addSubSect("Selection & Condition", 4, 0.1m);
            addSubSect("Proper Tool/Material Use", 4, 0.2m);
            addSubSect("Proper Vehicle Acceleration", 4, 0.3m);
            addSubSect("Proper Vehicle Direction Change", 4, 0.4m);
            addSubSect("Proper Vehicle Distance/Speed", 4, 0.5m);

            // Body Motion / Ergo 
            addSubSect("Lifting/Lowering", 5, 0.1m);
            addSubSect("Twisting", 5, 0.2m);
            addSubSect("Pushing & Pulling", 5, 0.3m);
            addSubSect("Over Extended/Cramped", 5, 0.4m);
            addSubSect("Ergo Repetitive", 5, 0.5m);

            // Procedures
            addSubSect("Isolation of Work Area", 6, 0.1m);
            addSubSect("Confined Space", 6, 0.2m);
            addSubSect("Communication Planning", 6, 0.3m);
            addSubSect("Shoring/Trenching", 6, 0.4m);
            addSubSect("Lock Out & Tag Out", 6, 0.5m);
            addSubSect("Procedure Rest", 6, 0.6m);

            // Other
            addSubSect("Other", 7, 0.1m);
            addSubSect("Working at Night", 7, 0.2m);

            Alter.Table("Incidents").AddForeignKeyColumn("AtRiskBehaviorSectionId", SECTIONS).Nullable();
            Alter.Table("Incidents").AddForeignKeyColumn("AtRiskBehaviorSubSectionId", SUB_SECTIONS).Nullable();

            Action<string, string, string> convertData = (oldDesc, sectDesc, subSectDesc) => {
                const string format = @"
declare @oldId int; set @oldId = (select top 1 Id from AtRiskBehaviors where Description = '{0}')
declare @sectId int; set @sectId = (select top 1 Id from AtRiskBehaviorSections where Description = '{1}')
declare @subSectId int; set @subSectId = (select top 1 Id from AtRiskBehaviorSubSections where Description = '{1}')
update Incidents set AtRiskBehaviorSectionId = @sectId, AtRiskBehaviorSubSectionId = @subSectId where AtRiskBehaviorId = @oldId";
                Execute.Sql(format, oldDesc, sectDesc, subSectDesc);
            };

            convertData("Work Environment", "Work Environment", null);
            convertData("PPE", "PPE", null);
            convertData("Body Position", "Body Locations", null);
            convertData("Ergonomics", "Body Motion / Ergo", null);
            convertData("Procedures", "Procedures", null);
            convertData("Working Surface", "Work Environment", "Walking/Working Surfaces");
            convertData("Housekeeping", "Work Environment", "Housekeeping");
            convertData("Body Motion", "Body Motion / Ergo", null);
            convertData("Line of Fire", "Body Locations", "Line of Fire");
            convertData("Push Pull", "Body Motion / Ergo", "Pushing & Pullilng");
            convertData("None Associated", "Other", "Other");

            this.DeleteForeignKeyColumn("Incidents", "AtRiskBehaviorId", "AtRiskBehaviors");
            Delete.Table("AtRiskBehaviors");
        }

        public override void Down()
        {
            this.CreateLookupTableWithValues("AtRiskBehaviors", "Work Environment", "PPE", "Body Position",
                "Ergonomics",
                "Procedures", "Working Surface", "Housekeeping", "Body Motion", "Line of Fire", "Push Pull",
                "None Associated");

            Alter.Table("Incidents")
                 .AddForeignKeyColumn("AtRiskBehaviorId", "AtRiskBehaviors");

            this.DeleteForeignKeyColumn("Incidents", "AtRiskBehaviorSubSectionId", SUB_SECTIONS);
            this.DeleteForeignKeyColumn("Incidents", "AtRiskBehaviorSectionId", SECTIONS);
            this.DeleteForeignKeyColumn(SUB_SECTIONS, "AtRiskBehaviorSectionId", SECTIONS);
            Delete.Table(SUB_SECTIONS);
            Delete.Table(SECTIONS);
        }
    }
}
