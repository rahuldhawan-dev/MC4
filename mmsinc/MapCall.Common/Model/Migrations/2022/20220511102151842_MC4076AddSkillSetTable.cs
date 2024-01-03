using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220511102151842)]
    [Tags("Production")]
    public class Mc4076AddSkillSetTable : Migration
    {
        private const string SKILL_SETS_TABLE = "SkillSets";

        public override void Up()
        {
            Create.Table(SKILL_SETS_TABLE).WithIdentityColumn()
                  .WithColumn("Name").AsAnsiString(50).NotNullable()
                  .WithColumn("Abbreviation").AsAnsiString(5).NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable()
                  .WithColumn("Description").AsAnsiString(255).NotNullable();

            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Maintenance Services - Specialist", Abbreviation = "MMS", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Maintenance Services - Mechanic", Abbreviation = "MSM", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Electrician - Hi Voltage", Abbreviation = "EHV", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Mechanic", Abbreviation = "MEC", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Basin/Plant Operator", Abbreviation = "OPR", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Water Quality - Supervisor", Abbreviation = "WQS", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Water Quality - Technician", Abbreviation = "WQT", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Utility Worker", Abbreviation = "UTL", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Electrician - Low Voltage", Abbreviation = "ELV", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Controls Programming", Abbreviation = "SCA", Description = "", IsActive = 1 });
            Insert.IntoTable(SKILL_SETS_TABLE).Row(new
                { Name = "Engineering", Abbreviation = "ENG", Description = "", IsActive = 1 });
        }

        public override void Down()
        {
            Delete.Table(SKILL_SETS_TABLE);
        }
    }
}