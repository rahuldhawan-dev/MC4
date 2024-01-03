using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130627151038), Tags("Production")]
    public class NormalizeServicesFieldsForBug1502 : Migration
    {
        #region Constants

        //Meter Setting Size	- SizeofTap
        //Backflow			    - BackflowDevice -- create table from
        //permit type			- PermitType
        //status				- InspSignoffReady
        //work issued to		- WorkIssuedto
        //town section	    	- TwnSection

        public struct Sql
        {
            public const string UPDATE_SIZE_OF_TAP =
                                    "UPDATE tblNJAWService SET SizeofTap = SS.RecID FROM tblNJAWService SE LEFT JOIN tblNJAWSizeServ SS ON SS.SizeServ = SE.SizeofTap;" +
                                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SizeofTap]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_SizeofTap] END;",
                                ROLLBACK_SIZE_OF_TAP =
                                    "UPDATE tblNJAWService SET SizeofTap = SS.SizeServ FROM tblNJAWService SE LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = SE.SizeofTap;" +
                                    "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SizeofTap]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_SizeofTap]  DEFAULT ('') FOR [SizeofTap] END;",
                                UPDATE_BACKFLOW =
                                    "INSERT INTO BackflowDevices(Description) Select Distinct FlowBackDevice from tblNJAWService where isNull(FlowBackDevice, '') <> '' order by 1;" +
                                    "UPDATE [tblNJAWService] SET FlowBackDevice = (Select BackflowDeviceID from BackflowDevices where Description = FlowBackDevice)",
                                ROLLBACK_BACKFLOW =
                                    "UPDATE [tblNJAWService] SET FlowBackDevice = (Select Description from BackflowDevices where BackflowDeviceID = FlowBackDevice)",
                                UPDATE_PERMIT_TYPES =
                                    "INSERT INTO PermitTypes Select Distinct PermitType from tblNJAWService where isNull(PermitType, '') <> '' order by 1;" +
                                    "UPDATE [tblNJAWService] SET PermitType = (Select PermitTypeID from PermitTypes where Description = PermitType);" +
                                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_PermitType]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_PermitType] END;",
                                ROLLBACK_PERMIT_TYPES =
                                    "UPDATE [tblNJAWService] SET PermitType = (Select Description from PermitTypes where PermitTypeID = PermitType);" +
                                    "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_PermitType]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_PermitType]  DEFAULT ('') FOR [PermitType] END",
                                UPDATE_SERVICE_STATUS =
                                    "INSERT INTO [ServiceStatuses] SELECT DISTINCT InspSignoffReady FROM tblNJAWService where isNull(InspSignoffReady, '') <> '' ORDER BY 1;" +
                                    "UPDATE tblNJAWService SET InspSignoffReady = (Select ServiceStatusID from ServiceStatuses where Description = InspSignoffReady);" +
                                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_InspSignoffReady]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_InspSignoffReady] END;",
                                ROLLBACK_SERVICE_STATUS =
                                    "UPDATE [tblNJAWService] SET InspSignoffReady = (Select Description from ServiceStatuses where ServiceStatusID = InspSignoffReady);" +
                                    "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_InspSignoffReady]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_InspSignoffReady]  DEFAULT ('') FOR [InspSignoffReady] END;",
                                UPDATE_TOWN_SECTIONS =
                                    "UPDATE tblNJAWService SET TwnSection = 'HEWLETT BAY' WHERE TwnSection = 'HEWLET BAY';" +
                                    "UPDATE tblNJAWService SET TwnSection = 'WEST ALLENHURST' WHERE TwnSection = 'W ALLENHURST';" +
                                    "UPDATE tblNJAWService SET TwnSection = TS.TownSectionID " +
                                    "FROM tblNJAWService SV " +
                                    "LEFT JOIN Towns T on T.TownID = SV.Town " +
                                    "LEFT JOIN TownSections TS on TS.TownID = T.TownID and TS.Name = IsNull(TwnSection,'');" +
                                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_TwnSection]') AND type = 'D')" +
                                    "    BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_TwnSection] END;",
                                ROLLBACK_TOWN_SECTIONS =
                                    "UPDATE tblNJAWService SET TwnSection = (Select Name from TownSections where TownSectionID = TwnSection);" +
                                    "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_TwnSection]') AND type = 'D')" +
                                    "   BEGIN ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_TwnSection]  DEFAULT ('') FOR [TwnSection] END;";
        }

        public struct Tables
        {
            public const string SERVICES = "tblNJAWService",
                                BACKFLOW_DEVICES = "BackflowDevices",
                                SERVICE_SIZES = "tblNJAWSizeServ",
                                PERMIT_TYPES = "PermitTypes",
                                SERVICE_STATUSES = "ServiceStatuses",
                                TOWN_SECTIONS = "TownSections";
        }

        public struct Columns
        {
            public const string DESCRIPTION = "Description",
                                SIZE_OF_TAP = "SizeofTap",
                                SIZE_OF_TAP_ID = "RecID",
                                BACKFLOW_DEVICE = "FlowbackDevice",
                                BACKFLOW_DEVICE_ID = "BackflowDeviceID",
                                PERMIT_TYPE = "PermitType",
                                PERMIT_TYPE_ID = "PermitTypeID",
                                SERVICE_STATUS = "InspSignoffReady",
                                SERVICE_STATUS_ID = "ServiceStatusID",
                                TOWN_SECTION = "TwnSection",
                                TOWN_SECTION_ID = "TownSectionID";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             SIZE_OF_TAP = 10,
                             FLOWBACK_DEVICE = 40,
                             PERMIT_TYPE = 6,
                             SERVICE_STATUS = 25,
                             TOWN_SECTION = 30;
        }

        public struct ForeignKeys
        {
            public const string FK_SERVICES_BACKFLOW_DEVICES = "FK_tblNJAWService_BackflowDevices_FlowbackDevice",
                                FK_SERVICES_SIZE_OF_TAP = "FK_tblNJAWService_tblNJAWSizeServ_SizeofTap",
                                FK_SERVICES_PERMIT_TYPE = "FK_tblNJAWService_PermitTypes_PermitType",
                                FK_SERVICES_SERVICE_STATUS = "FK_tblNJAWService_ServiceStatuses_InspSignoffReady",
                                FK_SERVICES_TOWN_SECTIONS = "FK_tblNJAWService_TownSections_TwnSection";
        }

        #endregion

        public override void Up()
        {
            #region Add Tables

            // SizeOfTap- table already exists
            Create.Table(Tables.BACKFLOW_DEVICES)
                  .WithColumn(Columns.BACKFLOW_DEVICE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.PERMIT_TYPES)
                  .WithColumn(Columns.PERMIT_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.SERVICE_STATUSES)
                  .WithColumn(Columns.SERVICE_STATUS_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            // TwnSection - table already exists

            #endregion

            #region Alter Data

            Execute.Sql(Sql.UPDATE_SIZE_OF_TAP);
            Execute.Sql(Sql.UPDATE_BACKFLOW);
            Execute.Sql(Sql.UPDATE_PERMIT_TYPES);
            Execute.Sql(Sql.UPDATE_SERVICE_STATUS);
            Execute.Sql(Sql.UPDATE_TOWN_SECTIONS);

            #endregion

            #region Alter Columns

            Alter.Table(Tables.SERVICES).AlterColumn(Columns.SIZE_OF_TAP)
                 .AsInt32().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.BACKFLOW_DEVICE)
                 .AsInt32().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.PERMIT_TYPE)
                 .AsInt32().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.SERVICE_STATUS)
                 .AsInt32().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.TOWN_SECTION)
                 .AsInt32().Nullable();

            #endregion

            #region Foreign Keys

            Create.ForeignKey(ForeignKeys.FK_SERVICES_BACKFLOW_DEVICES)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.BACKFLOW_DEVICE)
                  .ToTable(Tables.BACKFLOW_DEVICES).PrimaryColumn(Columns.BACKFLOW_DEVICE_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_SIZE_OF_TAP)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.SIZE_OF_TAP)
                  .ToTable(Tables.SERVICE_SIZES).PrimaryColumn(Columns.SIZE_OF_TAP_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_PERMIT_TYPE)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.PERMIT_TYPE)
                  .ToTable(Tables.PERMIT_TYPES).PrimaryColumn(Columns.PERMIT_TYPE_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_STATUS)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.SERVICE_STATUS)
                  .ToTable(Tables.SERVICE_STATUSES).PrimaryColumn(Columns.SERVICE_STATUS_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_TOWN_SECTIONS)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.TOWN_SECTION)
                  .ToTable(Tables.TOWN_SECTIONS).PrimaryColumn(Columns.TOWN_SECTION_ID);

            #endregion
        }

        public override void Down()
        {
            #region Foreign Keys

            Delete.ForeignKey(ForeignKeys.FK_SERVICES_TOWN_SECTIONS).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_STATUS).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_PERMIT_TYPE).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_SIZE_OF_TAP).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_BACKFLOW_DEVICES).OnTable(Tables.SERVICES);

            #endregion

            #region Alter Columns

            Alter.Table(Tables.SERVICES).AlterColumn(Columns.TOWN_SECTION)
                 .AsAnsiString(StringLengths.TOWN_SECTION).Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.PERMIT_TYPE)
                 .AsAnsiString(StringLengths.PERMIT_TYPE + 20).Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.BACKFLOW_DEVICE)
                 .AsAnsiString(StringLengths.FLOWBACK_DEVICE).Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.SIZE_OF_TAP)
                 .AsAnsiString(StringLengths.SIZE_OF_TAP).Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.SERVICE_STATUS)
                 .AsAnsiString(StringLengths.SERVICE_STATUS).Nullable();

            #endregion

            #region Alter Data

            Execute.Sql(Sql.ROLLBACK_TOWN_SECTIONS);
            Execute.Sql(Sql.ROLLBACK_SERVICE_STATUS);
            Execute.Sql(Sql.ROLLBACK_PERMIT_TYPES);
            Execute.Sql(Sql.ROLLBACK_BACKFLOW);
            Execute.Sql(Sql.ROLLBACK_SIZE_OF_TAP);

            #endregion

            #region Drop Tables

            // TownSections - table needs to remain
            Delete.Table(Tables.SERVICE_STATUSES);
            Delete.Table(Tables.PERMIT_TYPES);
            Delete.Table(Tables.BACKFLOW_DEVICES);
            // SizeOfTap- table needs to remain

            #endregion
        }
    }
}
