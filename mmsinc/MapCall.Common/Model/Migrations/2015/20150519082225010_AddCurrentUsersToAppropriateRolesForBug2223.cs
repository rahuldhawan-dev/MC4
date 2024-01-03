using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225010), Tags("Production")]
    public class AddCurrentUsersToAppropriateRolesForBug2223 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE NONCLUSTERED INDEX [_dta_index_Roles_19_1090207034__K6_K3_K4_K5_K7] ON [dbo].[Roles] ([ActionID] ASC,[OperatingCenterID] ASC,[ApplicationID] ASC,[ModuleID] ASC,[UserId] ASC)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [_dta_index_tblPermissions_19_1645300971__K1_13] ON [dbo].[tblPermissions]([RecID] ASC)INCLUDE ([OpCntr1]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [_dta_index_tblPermissions_19_1645300971__K16_K1_K15] ON [dbo].[tblPermissions]([OpCntr4] ASC,[RecID] ASC,[OpCntr3] ASC)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [_dta_index_tblPermissions_19_1645300971__K1_14_17] ON [dbo].[tblPermissions]([RecID] ASC) INCLUDE (	[OpCntr2],[OpCntr5]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [_dta_index_tblPermissions_19_1645300971__K1_18] ON [dbo].[tblPermissions]([RecID] ASC)INCLUDE ( 	[OpCntr6]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

                CREATE STATISTICS [_dta_stat_1645300971_17_18] ON [dbo].[tblPermissions]([OpCntr5], [OpCntr6])
                CREATE STATISTICS [_dta_stat_1645300971_15_18_17] ON [dbo].[tblPermissions]([OpCntr3], [OpCntr6], [OpCntr5])
                CREATE STATISTICS [_dta_stat_1645300971_14_1_13] ON [dbo].[tblPermissions]([OpCntr2], [RecID], [OpCntr1])
                CREATE STATISTICS [_dta_stat_1645300971_14_18_17_16] ON [dbo].[tblPermissions]([OpCntr2], [OpCntr6], [OpCntr5], [OpCntr4])
                CREATE STATISTICS [_dta_stat_1645300971_15_1_14_13] ON [dbo].[tblPermissions]([OpCntr3], [RecID], [OpCntr2], [OpCntr1])
                CREATE STATISTICS [_dta_stat_1645300971_13_18_17_16_15] ON [dbo].[tblPermissions]([OpCntr1], [OpCntr6], [OpCntr5], [OpCntr4], [OpCntr3])
                CREATE STATISTICS [_dta_stat_1645300971_16_1_15_14_13] ON [dbo].[tblPermissions]([OpCntr4], [RecID], [OpCntr3], [OpCntr2], [OpCntr1])
                CREATE STATISTICS [_dta_stat_1645300971_18_1_17_16_15_14] ON [dbo].[tblPermissions]([OpCntr6], [RecID], [OpCntr5], [OpCntr4], [OpCntr3], [OpCntr2])
                CREATE STATISTICS [_dta_stat_1645300971_17_1_16_15_14_13] ON [dbo].[tblPermissions]([OpCntr5], [RecID], [OpCntr4], [OpCntr3], [OpCntr2], [OpCntr1])
                CREATE STATISTICS [_dta_stat_1645300971_16_18_17_15_14_13_1] ON [dbo].[tblPermissions]([OpCntr4], [OpCntr6], [OpCntr5], [OpCntr3], [OpCntr2], [OpCntr1], [RecID])
                CREATE STATISTICS [_dta_stat_1090207034_3_4] ON [dbo].[Roles]([OperatingCenterID], [ApplicationID])
                CREATE STATISTICS [_dta_stat_1090207034_6_4] ON [dbo].[Roles]([ActionID], [ApplicationID])
                CREATE STATISTICS [_dta_stat_1090207034_4_5_6] ON [dbo].[Roles]([ApplicationID], [ModuleID], [ActionID])
                CREATE STATISTICS [_dta_stat_1090207034_7_3_4_5] ON [dbo].[Roles]([UserId], [OperatingCenterID], [ApplicationID], [ModuleID])
                CREATE STATISTICS [_dta_stat_1090207034_7_4_5_6] ON [dbo].[Roles]([UserId], [ApplicationID], [ModuleID], [ActionID])
                CREATE STATISTICS [_dta_stat_1090207034_5_3_4_6_7] ON [dbo].[Roles]([ModuleID], [OperatingCenterID], [ApplicationID], [ActionID], [UserId]);");
            Execute.Sql(@"
                SET NOCOUNT ON 
                DECLARE @operatingCenterId int, @applicationid int, @addActionId int, @editActionId int, @userId int
                declare @hydrantModuleId int, @hydrantInspectionModuleId int, @valveModuleId int, @valveInspectionModuleId int
                select @applicationId = (select applicationID from applications where Name = 'Field Services')
                select @hydrantModuleID = (select moduleID from Modules where Name = 'Hydrants' and ApplicationID = @applicationId)
                select @hydrantInspectionModuleID = (select moduleID from Modules where Name = 'Hydrant Inspections' and ApplicationID = @applicationId)
                select @valveModuleID = (select moduleID from Modules where Name = 'Valves' and ApplicationID = @applicationId)
                select @valveInspectionModuleID = (select moduleID from Modules where Name = 'Valve Inspections' and ApplicationID = @applicationId)
                select @addActionId = (select actionId from actions where name = 'add')
                select @editActionId = (select actionId from actions where name = 'edit')

                DECLARE	tableCursor 
                CURSOR FOR 
	                SELECT (select operatingCenterId from operatingCenters where operatingCenterCode = OpCntr6), RecID from tblPermissions where isNull(OpCntr6, '') <> ''
	                UNION ALL 
	                SELECT (select operatingCenterId from operatingCenters where operatingCenterCode = OpCntr5), RecID from tblPermissions where isNull(OpCntr5, '') <> ''
	                UNION ALL 
	                SELECT (select operatingCenterId from operatingCenters where operatingCenterCode = OpCntr4), RecID from tblPermissions where isNull(OpCntr4, '') <> ''
	                UNION ALL 
	                SELECT (select operatingCenterId from operatingCenters where operatingCenterCode = OpCntr3), RecID from tblPermissions where isNull(OpCntr3, '') <> ''
	                UNION ALL 
	                SELECT (select operatingCenterId from operatingCenters where operatingCenterCode = OpCntr2), RecID from tblPermissions where isNull(OpCntr2, '') <> ''
	                UNION ALL 
	                SELECT (select operatingCenterId from operatingCenters where operatingCenterCode = OpCntr1), RecID from tblPermissions where isNull(OpCntr1, '') <> ''

                OPEN tableCursor 
	                FETCH NEXT FROM tableCursor INTO @operatingCenterId, @userId

	                WHILE @@FETCH_STATUS = 0 
	                BEGIN 
		                Print 'Adding Roles for : ' + cast(@userId as varchar)
		                -- hydrant add/edit
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @hydrantModuleId and actionId = @editActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @hydrantModuleId, @editActionId, @userId)
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @hydrantModuleId and actionId = @addActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @hydrantModuleId, @addActionId, @userId)
		                -- valve add/edit
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @valveModuleId and actionId = @editActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @valveModuleId, @editActionId, @userId)
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @valveModuleId and actionId = @addActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @valveModuleId, @addActionId, @userId)
		                -- hydrant inspection add/edit
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @hydrantInspectionModuleId and actionId = @editActionId and userId = @userId)
		                Insert Into Roles values(@operatingCenterId, @applicationId, @hydrantInspectionModuleId, @editActionId, @userId)
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @hydrantInspectionModuleId and actionId = @addActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @hydrantInspectionModuleId, @addActionId, @userId)
		                -- valve inspection add/edit
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @valveInspectionModuleId and actionId = @editActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @valveInspectionModuleId, @editActionId, @userId)
		                IF NOT EXISTS (SELECT 1 from Roles where OperatingCenterId = @operatingCenterId and applicationId = @applicationId and moduleId = @valveInspectionModuleId and actionId = @addActionId and userId = @userId)
			                Insert Into Roles values(@operatingCenterId, @applicationId, @valveInspectionModuleId, @addActionId, @userId)

		                FETCH NEXT FROM tableCursor INTO @operatingCenterId, @userId;
	                END
                CLOSE tableCursor; 
                DEALLOCATE tableCursor;");
            Execute.Sql(@"
                drop index [_dta_index_Roles_19_1090207034__K6_K3_K4_K5_K7] on Roles
                drop index [_dta_index_tblPermissions_19_1645300971__K1_13] on tblPermissions
                drop index [_dta_index_tblPermissions_19_1645300971__K16_K1_K15] on tblPermissions
                drop index [_dta_index_tblPermissions_19_1645300971__K1_14_17] on tblPermissions
                drop index [_dta_index_tblPermissions_19_1645300971__K1_18] on tblPermissions

                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_17_18] 
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_15_18_17]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_14_1_13]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_14_18_17_16]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_15_1_14_13]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_13_18_17_16_15]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_16_1_15_14_13]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_18_1_17_16_15_14]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_17_1_16_15_14_13]
                DROP STATISTICS [tblPermissions].[_dta_stat_1645300971_16_18_17_15_14_13_1]
                DROP STATISTICS [Roles].[_dta_stat_1090207034_3_4]
                DROP STATISTICS [Roles].[_dta_stat_1090207034_6_4]
                DROP STATISTICS [Roles].[_dta_stat_1090207034_4_5_6]
                DROP STATISTICS [Roles].[_dta_stat_1090207034_7_3_4_5]
                DROP STATISTICS [Roles].[_dta_stat_1090207034_7_4_5_6]
                DROP STATISTICS [Roles].[_dta_stat_1090207034_5_3_4_6_7]");
        }

        public override void Down() { }
    }
}
