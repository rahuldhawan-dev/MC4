using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140220133506), Tags("Production")]
    public class CreateJobSiteCheckListTables : Migration
    {
        #region consts

        private const string JOB_SITE_CHECKLISTS_TABLE_NAME = "JobSiteCheckLists",
                             EXCAVATIONS_TABLE_NAME = "JobSiteExcavations",
                             JOB_SITE_CHECKLISTS_PROTECTION_TYPES_TABLE =
                                 "JobSiteCheckListsJobSiteExcavationProtectionTypes";

        public const int MAX_WORK_ORDER_ID_LENGTH = 50,
                         MAX_CREATED_BY_ID_LENGTH = 50,
                         MAX_MARKOUT_NUMBER_LENGTH = 50;

        #endregion

        #region Private Methods

        private void CreateLookupTable(string table, params string[] descriptions)
        {
            Create.Table(table)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(50).Unique().NotNullable();

            foreach (var d in descriptions)
            {
                Insert.IntoTable(table).Row(new {Description = d});
            }
        }

        #endregion

        public override void Up()
        {
            CreateLookupTable("JobSiteExcavationLocationTypes", "Behind Curb", "Roadway");
            CreateLookupTable("JobSiteExcavationSoilTypes", "A", "B", "C");
            CreateLookupTable("JobSiteExcavationProtectionTypes", "Benching", "Shoring", "Sloping", "Trench Box");

            Create.Table(JOB_SITE_CHECKLISTS_TABLE_NAME)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("OperatingCenterId").AsInt32().NotNullable().ForeignKey(
                       "FK_JobSiteCheckLists_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId")
                  .WithColumn("CompetentEmployeeId").AsInt32().NotNullable().ForeignKey(
                       "FK_JobSiteCheckLists_tblEmployee_CompetentEmployeeId", "tblEmployee", "tblEmployeeId")
                  .WithColumn("SupervisorSignOffEmployeeId").AsInt32().Nullable().ForeignKey(
                       "FK_JobSiteCheckLists_tblEmployee_SupervisorSignOffEmployeeId", "tblEmployee", "tblEmployeeId")
                  .WithColumn("SupervisorSignOffDate").AsDateTime().Nullable()
                  .WithColumn("Address").AsCustom("ntext").NotNullable()
                  .WithColumn("WorkOrderId").AsString(MAX_WORK_ORDER_ID_LENGTH).NotNullable()
                  .WithColumn("Comments").AsCustom("ntext").NotNullable()
                  .WithColumn("CrewMembers").AsCustom("ntext").NotNullable()
                  .WithColumn("CheckListDate").AsDateTime().NotNullable()
                  .WithColumn("CreatedBy").AsString(MAX_CREATED_BY_ID_LENGTH)
                  .WithColumn("CreatedOn").AsDateTime().NotNullable()

                   // Work Zone Set Up
                  .WithColumn("AllEmployeesWearingAppropriatePersonalProtectionEquipment").AsBoolean().NotNullable()
                  .WithColumn("CompliesWithStandards").AsBoolean().NotNullable()
                  .WithColumn("AllStructuresSupportedOrProtected").AsBoolean().NotNullable()
                  .WithColumn("HasBarricadesForTrafficControl").AsBoolean().NotNullable()
                  .WithColumn("HasConesForTrafficControl").AsBoolean().NotNullable()
                  .WithColumn("HasFlagPersonForTrafficControl").AsBoolean().NotNullable()
                  .WithColumn("HasPoliceForTrafficControl").AsBoolean().NotNullable()
                  .WithColumn("HasSignsForTrafficControl").AsBoolean().NotNullable()

                   // Utility Verification
                  .WithColumn("IsMarkoutValidForSite").AsBoolean().NotNullable()
                  .WithColumn("MarkoutNumber").AsString(MAX_MARKOUT_NUMBER_LENGTH).Nullable()
                  .WithColumn("IsEmergencyMarkoutRequest").AsBoolean().NotNullable()
                  .WithColumn("MarkedSanitarySewer").AsBoolean().Nullable()
                  .WithColumn("MarkedTelephone").AsBoolean().Nullable()
                  .WithColumn("MarkedFuelGas").AsBoolean().Nullable()
                  .WithColumn("MarkedElectric").AsBoolean().Nullable()
                  .WithColumn("MarkedWater").AsBoolean().Nullable()
                  .WithColumn("MarkedOther").AsBoolean().Nullable()

                   // Excavations
                  .WithColumn("AllMaterialsSetBackFromEdgeOfTrenches").AsBoolean().NotNullable()
                  .WithColumn("WaterControlSystemsInUse").AsBoolean().NotNullable()
                  .WithColumn("AreExposedUtilitiesProtected").AsBoolean().NotNullable()
                  .WithColumn("HasExcavationOverFourFeetDeep").AsBoolean().NotNullable()
                  .WithColumn("IsALadderInPlace").AsBoolean().Nullable()
                  .WithColumn("LadderExtendsAboveGrade").AsBoolean().Nullable()
                  .WithColumn("IsLadderOnSlope").AsBoolean().Nullable()
                  .WithColumn("HasAtmosphereBeenTested").AsBoolean().Nullable()
                  .WithColumn("AtmosphericOxygenLevel").AsDecimal(18, 5).Nullable()
                  .WithColumn("AtmosphericCarbonMonoxideLevel").AsDecimal(18, 5).Nullable()
                  .WithColumn("AtmosphericLowerExplosiveLimit").AsDecimal(18, 5).Nullable()
                  .WithColumn("HasExcavationFiveFeetOrDeeper").AsBoolean().NotNullable()
                  .WithColumn("IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical").AsBoolean().Nullable()
                  .WithColumn("IsShoringSystemUsed").AsBoolean().Nullable()
                  .WithColumn("ShoringSystemSidesExtendAboveBaseOfSlope").AsBoolean().Nullable()
                  .WithColumn("ShoringSystemInstalledTwoFeetFromBottomOfTrench").AsBoolean().Nullable();

            Create.Table(EXCAVATIONS_TABLE_NAME)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("JobSiteCheckListId")
                  .AsInt32()
                  .NotNullable()
                  .ForeignKey("FK_JobSiteExcavations_JobSiteCheckLists_JobSiteCheckListId",
                       JOB_SITE_CHECKLISTS_TABLE_NAME, "Id")
                  .WithColumn("WidthInFeet").AsDecimal().NotNullable()
                  .WithColumn("LengthInFeet").AsDecimal().NotNullable()
                  .WithColumn("DepthInInches").AsDecimal().NotNullable()
                  .WithColumn("JobSiteExcavationLocationTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_JobSiteExcavations_JobSiteExcavationLocationTypes_JobSiteExcavationLocationTypeId",
                       "JobSiteExcavationLocationTypes", "Id")
                  .WithColumn("JobSiteExcavationSoilTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_JobSiteExcavations_JobSiteExcavationSoilTypes_JobSiteExcavationSoilTypeId",
                       "JobSiteExcavationSoilTypes", "Id")
                  .WithColumn("ExcavationDate").AsDateTime().NotNullable();

            Create.Table(JOB_SITE_CHECKLISTS_PROTECTION_TYPES_TABLE)
                  .WithColumn("JobSiteCheckListId")
                  .AsInt32()
                  .NotNullable()
                  .ForeignKey(
                       "FK_JobSiteCheckListsJobSiteExcavationProtectionTypes_JobSiteCheckLists_JobSiteCheckListId",
                       "JobSiteCheckLists", "Id")
                  .WithColumn("JobSiteExcavationProtectionTypeId")
                  .AsInt32()
                  .NotNullable()
                  .ForeignKey(
                       "FK_JobSiteCheckListsJobSiteExcavationProtectionTypes_JobSiteExcavationProtectionTypes_JobSiteExcavationProtectionTypeId",
                       "JobSiteExcavationProtectionTypes", "Id");

            Execute.Sql(@"
                declare @moduleId int
                set @moduleId = (select ModuleID from [Modules] where Name = 'Health and Safety')
                insert into [NotificationPurposes] ([ModuleID], [Purpose]) VALUES(@moduleId, 'Job Site Check List')
            ");

            // Needed for 271.
            Execute.Sql(@"GRANT ALL on JobSiteCheckLists to MCuser");
        }

        public override void Down()
        {
            Execute.Sql(@"
                declare @purposeId int
                set @purposeId = (select top 1 NotificationPurposeID from [NotificationPurposes] where [Purpose] = 'Job Site Check List')
                
                delete from [NotificationConfigurations] where [NotificationPurposeID] = @purposeId
                delete from [NotificationPurposes] where [NotificationPurposeID] = @purposeId 
            ");

            Delete.ForeignKey(
                       "FK_JobSiteCheckListsJobSiteExcavationProtectionTypes_JobSiteExcavationProtectionTypes_JobSiteExcavationProtectionTypeId")
                  .OnTable(JOB_SITE_CHECKLISTS_PROTECTION_TYPES_TABLE);
            Delete.ForeignKey(
                       "FK_JobSiteCheckListsJobSiteExcavationProtectionTypes_JobSiteCheckLists_JobSiteCheckListId")
                  .OnTable(JOB_SITE_CHECKLISTS_PROTECTION_TYPES_TABLE);
            Delete.ForeignKey("FK_JobSiteExcavations_JobSiteExcavationSoilTypes_JobSiteExcavationSoilTypeId")
                  .OnTable(EXCAVATIONS_TABLE_NAME);
            Delete.ForeignKey("FK_JobSiteExcavations_JobSiteExcavationLocationTypes_JobSiteExcavationLocationTypeId")
                  .OnTable(EXCAVATIONS_TABLE_NAME);
            Delete.ForeignKey("FK_JobSiteExcavations_JobSiteCheckLists_JobSiteCheckListId")
                  .OnTable(EXCAVATIONS_TABLE_NAME);
            Delete.ForeignKey("FK_JobSiteCheckLists_tblEmployee_SupervisorSignOffEmployeeId")
                  .OnTable(JOB_SITE_CHECKLISTS_TABLE_NAME);
            Delete.ForeignKey("FK_JobSiteCheckLists_tblEmployee_CompetentEmployeeId")
                  .OnTable(JOB_SITE_CHECKLISTS_TABLE_NAME);
            Delete.ForeignKey("FK_JobSiteCheckLists_OperatingCenters_OperatingCenterId")
                  .OnTable(JOB_SITE_CHECKLISTS_TABLE_NAME);

            Delete.Table(JOB_SITE_CHECKLISTS_PROTECTION_TYPES_TABLE);
            Delete.Table(EXCAVATIONS_TABLE_NAME);
            Delete.Table("JobSiteExcavationProtectionTypes");
            Delete.Table("JobSiteExcavationSoilTypes");
            Delete.Table("JobSiteExcavationLocationTypes");
            Delete.Table(JOB_SITE_CHECKLISTS_TABLE_NAME);
        }
    }
}
