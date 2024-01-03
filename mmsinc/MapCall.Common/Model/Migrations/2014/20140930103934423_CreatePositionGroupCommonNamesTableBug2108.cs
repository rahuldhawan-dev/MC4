using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140930103934423), Tags("Production")]
    public class CreatePositionGroupCommonNamesTableBug2108 : Migration
    {
        #region Consts

        public const int DESCRIPTION_LENGTH = 100;
        public const string POSITION_GROUP_COMMON_NAMES = "PositionGroupCommonNames";
        public const string TRAINING_REQUIREMENTS = "TrainingRequirements";

        #endregion

        private void CreateRequirement(string description, int freq, params int[] moduleIds)
        {
            Insert.IntoTable(TRAINING_REQUIREMENTS).Row(new {
                Description = description,
                RecurringTrainingFrequencyDays = freq,

                // These are all static values in the excel file from bug 2108.
                IsDOTRequirement = false,
                IsOSHARequirement = true,
                IsDPCCRequirement = false,
                IsPSMTCPARequirement = false,
                IsProductionRequirement = true,
                IsFieldOperationsRequirement = true,
                IsInitialRequirement = true,
                IsActive = true
            });

            foreach (var id in moduleIds)
            {
                var format = @"
    BEGIN
        declare @moduleId int; set @moduleId = {0};
        declare @requirementId int; set @requirementId = (select top 1 Id from TrainingRequirements where Description = '{1}')
    
        IF EXISTS (select * from tblTrainingModules where TrainingModuleId = @moduleId)
        BEGIN
            insert into [TrainingModulesTrainingRequirements] (TrainingModuleId, TrainingRequirementId) VALUES (@moduleId, @requirementId)
        END
    END 
";
                Execute.Sql(string.Format(format, id, description));
            }
        }

        private void CreateTrainingRequirementsTable()
        {
            Create.Table(TRAINING_REQUIREMENTS)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(DESCRIPTION_LENGTH).NotNullable().Unique()
                  .WithColumn("RecurringTrainingFrequencyDays").AsInt32().NotNullable()
                  .WithColumn("IsDOTRequirement").AsBoolean().NotNullable()
                  .WithColumn("IsOSHARequirement").AsBoolean().NotNullable()
                  .WithColumn("IsDPCCRequirement").AsBoolean().NotNullable()
                  .WithColumn("IsPSMTCPARequirement").AsBoolean().NotNullable()
                  .WithColumn("IsFieldOperationsRequirement").AsBoolean().NotNullable()
                  .WithColumn("IsProductionRequirement").AsBoolean().NotNullable()
                  .WithColumn("IsInitialRequirement").AsBoolean().NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable()
                  .WithColumn("RegulationId").AsInt32().Nullable()
                  .ForeignKey("FK_TrainingRequirements_Regulations_RegulationId", "Regulations", "RegulationID");

            Create.Table("TrainingModulesTrainingRequirements")
                  .WithColumn("TrainingModuleId").AsInt32().NotNullable()
                  .ForeignKey("FK_TrainingModulesTrainingRequirements_tblTrainingModules_TrainingModuleId",
                       "tblTrainingModules", "TrainingModuleId")
                  .WithColumn("TrainingRequirementId").AsInt32().NotNullable()
                  .ForeignKey("FK_TrainingModulesTrainingRequirements_TrainingRequirements_TrainingRequirementId",
                       "TrainingRequirements", "Id");

            // TODO: This needs an "If exists" thing

            CreateRequirement("40 hour HazWoper", 0, 251);
            CreateRequirement("8 Hour HazWoper Refresher", 365, 224);
            CreateRequirement("Electrical Safety in the Work Place / Arc Flash Awareness", 1095, 154, 222, 281, 247);
            CreateRequirement("Electrical Safety / Arc Flash Authorize", 1095, 247);
            CreateRequirement("Asbestos Awareness", 0, 276);
            CreateRequirement("Blood Borne Pathogens", 365, 76);
            CreateRequirement("Chlorine Site Specific Emergency Procedures", 365);
            CreateRequirement("Confined Space", 1095, 248, 258);
            CreateRequirement("Commercial Vehicle Operation / DOT Regulations", 0);
            CreateRequirement("Defensive Driving", 1095, 96);
            CreateRequirement("Dog Safety", 0, 99);
            CreateRequirement("Ergonomics", 0, 266);
            CreateRequirement("Excavation and Shoring", 1095, 280, 257);
            CreateRequirement("Fall Protection / Walking / Working Surface", 1095, 238, 240);
            CreateRequirement("Fire Extinguishers/Protection", 365, 260);
            CreateRequirement("First Aid CPR AED", 730, 16);
            CreateRequirement("Gang Awareness", 0, 156);
            CreateRequirement("Hand and Power Tools", 0);
            CreateRequirement("Hazard Communication, RTK", 365, 229);
            CreateRequirement("Hearing Conservation", 365);
            CreateRequirement("Hoist and Crane Safety", 1095, 253, 254);
            CreateRequirement("Job Safety Analysis", 0);
            CreateRequirement("Lab Safety", 0);
            CreateRequirement("Lock Out Tag Out (LOTO)", 1095, 244);
            CreateRequirement("Machine Guarding", 0);
            CreateRequirement("Personal Protective Equipment (PPE)", 1095, 278);
            CreateRequirement("Power Industrial Trucks/Fork Lifts", 1095, 283);
            CreateRequirement("Respiration Protection SCBA Fit Test", 365, 261);
            CreateRequirement("Vactor Truck Operation", 1825);
            CreateRequirement("Backhoe Operation", 1825, 231);
            CreateRequirement("Work Zone Protection", 1825, 252);
        }

        public override void Up()
        {
            Create.Table(POSITION_GROUP_COMMON_NAMES)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(DESCRIPTION_LENGTH).NotNullable().Unique();

            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "FSR"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Meter Reader"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Utility Mechanic/Inspector"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Stock Clerk"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Construction Administrative"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Production Operations"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Production Maintenance"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Water Quality"});
            Insert.IntoTable(POSITION_GROUP_COMMON_NAMES).Row(new {Description = "Engineering"});

            Alter.Table("PositionGroups")
                 .AddColumn("PositionGroupCommonNameId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_PositionGroups_PositionGroupCommonNames_PositionGroupCommonNameId",
                      "PositionGroupCommonNames", "Id");

            Create.Table("PositionGroupCommonNamesTrainingModules")
                  .WithColumn("PositionGroupCommonNameId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_PositionGroupCommonNamesTrainingModules_PositionGroupCommonNames_PositionGroupCommonNameId",
                       "PositionGroupCommonNames", "Id")
                  .WithColumn("TrainingModuleId").AsInt32().NotNullable()
                  .ForeignKey("FK_PositionGroupCommonNamesTrainingModules_tblTrainingModules_TrainingModuleId",
                       "tblTrainingModules", "TrainingModuleId");

            // Map the JobTitleCommonNamesTrainingModules rows to the PositionGroupCommonNamesTrainingModules table.
            Execute.Sql(@"
insert into PositionGroupCommonNamesTrainingModules (PositionGroupCommonNameId, TrainingModuleId)
select
PositionGroupCommonNameId = (select top 1 Id from PositionGroupCommonNames where Description = 'FSR'),
jt.TrainingModuleID
from JobTitleCommonNamesTrainingModules jt
inner join JobTitleCommonNames jtcn on jtcn.JobTitleCommonNameID = jt.JobTitleCommonNameID
where jtcn.Description = 'FSR'

insert into PositionGroupCommonNamesTrainingModules (PositionGroupCommonNameId, TrainingModuleId)
select
PositionGroupCommonNameId = (select top 1 Id from PositionGroupCommonNames where Description = 'Utility Mechanic/Inspector'),
jt.TrainingModuleID
from JobTitleCommonNamesTrainingModules jt
inner join JobTitleCommonNames jtcn on jtcn.JobTitleCommonNameID = jt.JobTitleCommonNameID
where jtcn.Description = 'Utility Mechanic' or jtcn.Description = 'Inspector'

insert into PositionGroupCommonNamesTrainingModules (PositionGroupCommonNameId, TrainingModuleId)
select
PositionGroupCommonNameId = (select top 1 Id from PositionGroupCommonNames where Description = 'Production Operations'),
jt.TrainingModuleID
from JobTitleCommonNamesTrainingModules jt
inner join JobTitleCommonNames jtcn on jtcn.JobTitleCommonNameID = jt.JobTitleCommonNameID
where jtcn.Description like '%Operator%'
");

            Delete.Table("JobTitleCommonNamesTrainingModules");

            CreateTrainingRequirementsTable();
        }

        public override void Down()
        {
            Delete.Table("TrainingModulesTrainingRequirements");
            Delete.Table("TrainingRequirements");

            Create.Table("JobTitleCommonNamesTrainingModules")
                  .WithColumn("JobTitleCommonNameId").AsInt32().NotNullable()
                  .ForeignKey("FK_JobTitleCommonNamesTrainingModules_JobTitleCommonNames_JobTitleCommonNameId",
                       "JobTitleCommonNames", "JobTitleCommonNameId")
                  .WithColumn("TrainingModuleId").AsInt32().NotNullable()
                  .ForeignKey("FK_JobTitleCommonNamesTrainingModules_tblTrainingModules_TrainingModuleId",
                       "tblTrainingModules", "TrainingModuleId");

            Delete.ForeignKey(
                       "FK_PositionGroupCommonNamesTrainingModules_PositionGroupCommonNames_PositionGroupCommonNameId")
                  .OnTable("PositionGroupCommonNamesTrainingModules");
            Delete.ForeignKey(
                       "FK_PositionGroupCommonNamesTrainingModules_tblTrainingModules_TrainingModuleId")
                  .OnTable("PositionGroupCommonNamesTrainingModules");

            Delete.Table("PositionGroupCommonNamesTrainingModules");

            Delete.ForeignKey("FK_PositionGroups_PositionGroupCommonNames_PositionGroupCommonNameId")
                  .OnTable("PositionGroups");
            Delete.Column("PositionGroupCommonNameId").FromTable("PositionGroups");
            Delete.Table(POSITION_GROUP_COMMON_NAMES);
        }
    }
}
