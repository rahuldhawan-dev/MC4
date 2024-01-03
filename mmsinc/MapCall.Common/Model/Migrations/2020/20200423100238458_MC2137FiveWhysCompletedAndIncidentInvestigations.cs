using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200423100238458), Tags("Production")]
    public class MC2137FiveWhysCompletedAndIncidentInvestigations : Migration
    {
        #region Private Methods

        private List<Tuple<string, string, string>> _levels = new List<Tuple<string, string, string>>();

        private void PrepareLevels(string level1, string level2, string level3)
        {
            _levels.Add(new Tuple<string, string, string>(level1, level2, level3));
        }

        private void CreateDatabaseLevels()
        {
            foreach (var level1Group in _levels.GroupBy(x => x.Item1))
            {
                var level1 = level1Group.Key;

                Insert.IntoTable("IncidentInvestigationRootCauseLevel1Types").Row(new {Description = level1});

                var level2Descs = level1Group.Select(x => x.Item2).Distinct();
                foreach (var level2 in level2Descs)
                {
                    Execute.Sql($@"
declare @level1 int
set @level1 = (select Id from IncidentInvestigationRootCauseLevel1Types where Description = '{level1}')
insert into IncidentInvestigationRootCauseLevel2Types (Description, IncidentInvestigationRootCauseLevel1TypeId) values ('{level2}' ,@level1)
");

                    // Not all level2s have a level3, so skip inserting a level3 if it's null/empty.
                    var level3Descs = _levels
                                     .Where(x => x.Item1 == level1 && x.Item2 == level2 &&
                                                 !string.IsNullOrWhiteSpace(x.Item3))
                                     .Select(x => x.Item3).Distinct();
                    foreach (var level3 in level3Descs)
                    {
                        Execute.Sql($@"
declare @level2 int
set @level2 = (select Id from IncidentInvestigationRootCauseLevel2Types where Description = '{level2}')
insert into IncidentInvestigationRootCauseLevel3Types (Description, IncidentInvestigationRootCauseLevel2TypeId) values ('{level3}' , @level2)
");
                    }
                }
            }
        }

        private void DoLevelInserts()
        {
            // Parse root cause matrix
            PrepareLevels("Procedures", "Not Used/Not Followed", "No Procedure");
            PrepareLevels("Procedures", "Not Used/Not Followed", "Procedure Not Available or Inconvenient for Use");
            PrepareLevels("Procedures", "Not Used/Not Followed", "Procedure Difficult to Use");
            PrepareLevels("Procedures", "Not Used/Not Followed", "Procedure Use Not Required But Should Be");
            PrepareLevels("Procedures", "Wrong", "Typo");
            PrepareLevels("Procedures", "Wrong", "Sequence Wrong");
            PrepareLevels("Procedures", "Wrong", "Facts Wrong");
            PrepareLevels("Procedures", "Wrong", "Situation Not Covered");
            PrepareLevels("Procedures", "Wrong", "Wrong Revision Used");
            PrepareLevels("Procedures", "Wrong", "Second Checker Needed");
            PrepareLevels("Procedures", "Followed Incorrectly", "Format Confusing");
            PrepareLevels("Procedures", "Followed Incorrectly", "Action/Step");
            PrepareLevels("Procedures", "Followed Incorrectly", "Excess References");
            PrepareLevels("Procedures", "Followed Incorrectly", "Multi Unit References");
            PrepareLevels("Procedures", "Followed Incorrectly", "Limits NI");
            PrepareLevels("Procedures", "Followed Incorrectly", "Details NI");
            PrepareLevels("Procedures", "Followed Incorrectly", "Data/Computations Wrong or Incomplete");
            PrepareLevels("Procedures", "Followed Incorrectly", "Graphics NI");
            PrepareLevels("Procedures", "Followed Incorrectly", "No Checkoff");
            PrepareLevels("Procedures", "Followed Incorrectly", "Checkoff Misused");
            PrepareLevels("Procedures", "Followed Incorrectly", "Misused Second Check");
            PrepareLevels("Procedures", "Followed Incorrectly", "Ambiguous Instructions");
            PrepareLevels("Procedures", "Followed Incorrectly", "Equip Identification NI");
            PrepareLevels("Training", "No Training", "Task Not Analyzed");
            PrepareLevels("Training", "No Training", "Decided Not To Train");
            PrepareLevels("Training", "No Training", "No Learning Objective");
            PrepareLevels("Training", "No Training", "Missed Required Training");
            PrepareLevels("Training", "Understanding NI", "Learning Objective NI");
            PrepareLevels("Training", "Understanding NI", "Lesson Plan NI");
            PrepareLevels("Training", "Understanding NI", "Instruction NI");
            PrepareLevels("Training", "Understanding NI", "Practice/Repetition NI");
            PrepareLevels("Training", "Understanding NI", "Testing NI");
            PrepareLevels("Training", "Understanding NI", "Continuing Training NI");
            PrepareLevels("Quality Control", "No Inspection", "Inspection Not Required");
            PrepareLevels("Quality Control", "No Inspection", "No Hold Point");
            PrepareLevels("Quality Control", "No Inspection", "Hold Point Not Performed");
            PrepareLevels("Quality Control", "QC NI", "Inspection Instructions NI");
            PrepareLevels("Quality Control", "QC NI", "Inspection Techniques NI");
            PrepareLevels("Quality Control", "QC NI", "Foreign Material Exclusion During Work NI");
            PrepareLevels("Communications", "No Comm or Not Timely", "");
            PrepareLevels("Communications", "Turnover NI", "No Standard Turnover Process");
            PrepareLevels("Communications", "Turnover NI", "Turnover Process Not Used");
            PrepareLevels("Communications", "Turnover NI", "Turnover Process NI");
            PrepareLevels("Communications", "Misunderstood Verbal Comm", "Standard Terminology Not Used");
            PrepareLevels("Communications", "Misunderstood Verbal Comm", "Standard Terminology NI");
            PrepareLevels("Communications", "Misunderstood Verbal Comm", "Repeat Back Not Used");
            PrepareLevels("Communications", "Misunderstood Verbal Comm", "Long Message");
            PrepareLevels("Communications", "Misunderstood Verbal Comm", "Noisy Environment");
            PrepareLevels("Management System", "SPAC NI", "No SPAC");
            PrepareLevels("Management System", "SPAC NI", "Not Strict Enough");
            PrepareLevels("Management System", "SPAC NI", "Confusing or Incomplete");
            PrepareLevels("Management System", "SPAC NI", "Technical Error");
            PrepareLevels("Management System", "SPAC NI", "Drawings/Prints NI");
            PrepareLevels("Management System", "SPAC Not Used", "Comm of SPAC NI");
            PrepareLevels("Management System", "SPAC Not Used", "Recently Changed");
            PrepareLevels("Management System", "SPAC Not Used", "Enforcement NI");
            PrepareLevels("Management System", "SPAC Not Used", "No Way to Implement");
            PrepareLevels("Management System", "SPAC Not Used", "Accountability NI");
            PrepareLevels("Management System", "Oversight/Employee Relations", "Infrequent Audits & Evaluations (A&E)");
            PrepareLevels("Management System", "Oversight/Employee Relations", "A&E Lack Depth");
            PrepareLevels("Management System", "Oversight/Employee Relations", "A&E Not Independent");
            PrepareLevels("Management System", "Oversight/Employee Relations", "Employee Communications NI");
            PrepareLevels("Management System", "Oversight/Employee Relations", "Employee Feedback NI");
            PrepareLevels("Management System", "Corrective Action", "Corrective Action NI");
            PrepareLevels("Management System", "Corrective Action", "Corrective Action Not Yet Implemented");
            PrepareLevels("Management System", "Corrective Action", "Trending NI");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Labels NI");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Arrangement/Placement NI");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Displays NI");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Controls NI");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Montoring Alertness NI");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Plant/Unit Differences");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Excessive Lifting");
            PrepareLevels("Human Engineering", "Human/Machine Interface", "Tools/Instruments NI");
            PrepareLevels("Human Engineering", "Work Environment", "Housekeeping NI");
            PrepareLevels("Human Engineering", "Work Environment", "Hot/Cold");
            PrepareLevels("Human Engineering", "Work Environment", "Wet/Slick");
            PrepareLevels("Human Engineering", "Work Environment", "Lights NI");
            PrepareLevels("Human Engineering", "Work Environment", "Noisy");
            PrepareLevels("Human Engineering", "Work Environment", "Obstruction");
            PrepareLevels("Human Engineering", "Work Environment", "Cramped Quarters");
            PrepareLevels("Human Engineering", "Work Environment", "Equipment Guard NI");
            PrepareLevels("Human Engineering", "Work Environment", "High Radiation/Contamination");
            PrepareLevels("Human Engineering", "Complex System", "");
            PrepareLevels("Human Engineering", "Non Fault Tolerant System", "");
            PrepareLevels("Work Direction", "Preparation", "No Preparation");
            PrepareLevels("Work Direction", "Preparation", "Work Package/Permit NI");
            PrepareLevels("Work Direction", "Preparation", "Pre-Job Briefing NI");
            PrepareLevels("Work Direction", "Preparation", "Walk-thru NI");
            PrepareLevels("Work Direction", "Preparation", "Scheduling NI");
            PrepareLevels("Work Direction", "Preparation", "Lock Out/Tag Out NI");
            PrepareLevels("Work Direction", "Preparation", "Fall Protection NI");
            PrepareLevels("Work Direction", "Selection of Worker", "Not Qualified");
            PrepareLevels("Work Direction", "Selection of Worker", "Fatigued");
            PrepareLevels("Work Direction", "Selection of Worker", "Upset");
            PrepareLevels("Work Direction", "Selection of Worker", "Substance Abuse");
            PrepareLevels("Work Direction", "Selection of Worker", "Team Selection NI");
            PrepareLevels("Work Direction", "Supervision During Work", "No Supervision");
            PrepareLevels("Work Direction", "Supervision During Work", "Crew Teamwork NI");
            PrepareLevels("Not Applicable/None Identified", "Not Applicable/None Identified", "");

            // Then actually import everything
            CreateDatabaseLevels();
        }

        #endregion

        public override void Up()
        {
            // They want all existing incident records to be set to "False" for this.
            Create.Column("FiveWhysCompleted").OnTable("Incidents").AsBoolean().NotNullable().WithDefaultValue(false);

            // Discussed with Nicole. They don't have values for this yet.
            this.CreateLookupTableWithValues("IncidentInvestigationRootCauseFindingTypes");

            this.CreateLookupTableWithValues("IncidentInvestigationRootCauseLevel1Types");

            Create.Table("IncidentInvestigationRootCauseLevel2Types")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(50).NotNullable().Unique()
                  .WithColumn("IncidentInvestigationRootCauseLevel1TypeId").AsInt32().NotNullable()
                   // Can't use WithForeignKey here because the foreign key name generated is too long
                  .ForeignKey(
                       "FK_IncidentInvestigationRootCauseLevel2Types_IncidentInvestigationRootCauseLevel1Types_Level1TypeId",
                       "IncidentInvestigationRootCauseLevel1Types", "Id");

            Create.Table("IncidentInvestigationRootCauseLevel3Types")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(50).NotNullable().Unique()
                  .WithColumn("IncidentInvestigationRootCauseLevel2TypeId").AsInt32().NotNullable()
                   // Can't use WithForeignKey here because the foreign key name generated is too long
                  .ForeignKey(
                       "FK_IncidentInvestigationRootCauseLevel3Types_IncidentInvestigationRootCauseLevel2Types_Level2TypeId",
                       "IncidentInvestigationRootCauseLevel2Types", "Id");

            Create.Table("IncidentInvestigations")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("IncidentId", "Incidents").NotNullable()
                  .WithForeignKeyColumn("IncidentInvestigationRootCauseFindingTypeId",
                       "IncidentInvestigationRootCauseFindingTypes").NotNullable()
                  .WithForeignKeyColumn("IncidentInvestigationRootCauseLevel1TypeId",
                       "IncidentInvestigationRootCauseLevel1Types").NotNullable()
                  .WithForeignKeyColumn("IncidentInvestigationRootCauseLevel2TypeId",
                       "IncidentInvestigationRootCauseLevel2Types").NotNullable()
                  .WithForeignKeyColumn("IncidentInvestigationRootCauseLevel3TypeId",
                       "IncidentInvestigationRootCauseLevel3Types").Nullable(); // Not all Level2s have a Level3

            Create.Table("IncidentInvestigationsRootCauseFindingPerformedByUsers")
                  .WithForeignKeyColumn("IncidentInvestigationId", "IncidentInvestigations").NotNullable()
                  .Indexed("IX_IncidentInvestigationId")
                  .WithForeignKeyColumn("UserId", "tblPermissions", "RecId").NotNullable().Indexed();

            DoLevelInserts();
        }

        public override void Down()
        {
            Delete.Table("IncidentInvestigationsRootCauseFindingPerformedByUsers");
            Delete.Table("IncidentInvestigations");
            Delete.Table("IncidentInvestigationRootCauseLevel3Types");
            Delete.Table("IncidentInvestigationRootCauseLevel2Types");
            Delete.Table("IncidentInvestigationRootCauseLevel1Types");
            Delete.Table("IncidentInvestigationRootCauseFindingTypes");

            Delete.Column("FiveWhysCompleted").FromTable("Incidents");
        }
    }
}
