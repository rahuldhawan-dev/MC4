using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210723121538976), Tags("Production")]
    public class MC3572AddActionTakenTypesToNearMisses : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ActionTakenTypes", "Took_Immediate_Action_To_Mitigate_Hazard",
                "Made_Corrective_Action_To_Eliminate_Hazard",
                "Intervened_To_Correct_Unsafe_Act",
                "Reported_To_My_Supervisor",
                "Submitted_A_Work_Order");
            Alter.Table("NearMisses")
                 .AddForeignKeyColumn("ActionTakenTypeId", "ActionTakenTypes")
                 .AlterColumn("ActionTaken").AsString(255).Nullable();

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'working without required PPE'
            WHERE description = 'PPE';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'working under the influence of drugs or alcohol'
            WHERE description = 'Alcohol or Illegal Drugs';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'inadequate work zone safety near traffic'
            WHERE description = 'Work Zone Safety';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'non-compliant excavation hazard'
            WHERE description = 'Cave-in Protection';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'unsafe use of tools or use of unapproved tools'
            WHERE description = 'Approved Tool/Proper Usage';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'uncontrolled energy hazard (LOTO)'
            WHERE description = 'Hazardous Energy Control';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'working at height without adequate fall protection'
            WHERE description = 'Fall Protection';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'inadequate safeguards for confined space entry'
            WHERE description = 'Confined Space Safeguards';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'inadequate prevention of utility line contact'
            WHERE description = 'Contact with Utility Line';");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("NearMisses", "ActionTakenTypeId", "ActionTakenTypes");
            Delete.Table("ActionTakenTypes");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'PPE'
            WHERE description = 'working without required PPE';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Alcohol or Illegal Drugs'
            WHERE description = 'working under the influence of drugs or alcohol';");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Work Zone Safety'
            WHERE description = 'inadequate work zone safety near traffic'; ");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Cave-in Protection'
            WHERE description = 'non-compliant excavation hazard'; ");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Approved Tool/Proper Usage'
            WHERE description = 'unsafe use of tools or use of unapproved tools'; ");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Hazardous Energy Control'
            WHERE description = 'uncontrolled energy hazard (LOTO)'; ");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Fall Protection'
            WHERE description = 'working at height without adequate fall protection'; ");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Confined Space Safeguards'
            WHERE description = 'inadequate safeguards for confined space entry'; ");

            Execute.Sql(@"UPDATE LifeSavingRuleTypes
            SET Description = 'Contact with Utility Line'
            WHERE description = 'inadequate prevention of utility line contact'; ");
        }
    }
}

