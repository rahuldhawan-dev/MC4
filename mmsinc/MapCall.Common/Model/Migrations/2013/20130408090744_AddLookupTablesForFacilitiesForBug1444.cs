using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130408090744), Tags("Production")]
    public class AddLookupTablesForFacilitiesForBug1444 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string FACILITY_OWNERS_PERMISSIONS = "GRANT ALL ON [FacilityOwners] TO MCUser",
                                FACILITY_STATUSES_PERMISSIONS = "GRANT ALL ON [FacilityStatuses] TO MCUser",
                                FEMA_FLOOD_RATINGS_PERMISSIONS = "GRANT ALL ON [FEMAFloodRatings] TO MCUser",
                                FACILITY_OWNERS_INSERT =
                                    "SET IDENTITY_INSERT FacilityOwners ON;INSERT INTO FacilityOwners(FacilityOwnerID, Description) SELECT LookupID, LookupValue FROM Lookup WHERE LookupType = 'Facility_Ownership' ORDER BY 2;SET IDENTITY_INSERT FacilityOwners OFF;UPDATE tblFacilities SET Facility_Ownership = null WHERE Facility_Ownership NOT IN (SELECT LookupID FROM [Lookup] WHERE LookupType = 'Facility_Ownership')",
                                FACILITY_STATUSES_INSERT =
                                    "SET IDENTITY_INSERT FacilityStatuses ON;INSERT INTO FacilityStatuses(FacilityStatusID, Description) SELECT LookupID, LookupValue FROM Lookup WHERE LookupType = 'Facility_Status' ORDER BY 2;SET IDENTITY_INSERT FacilityStatuses OFF;UPDATE tblFacilities SET Status = null WHERE Status NOT IN (SELECT LookupID FROM [Lookup] WHERE LookupType = 'Facility_Status');",
                                FEMA_FLOOD_RATINGS_INSERT =
                                    "SET IDENTITY_INSERT FEMAFloodRatings ON;INSERT INTO FEMAFloodRatings(FEMAFloodRatingID, Description) SELECT LookupID, LookupValue FROM Lookup WHERE LookupType = 'FEMA_Flood_Rating' ORDER BY 2;SET IDENTITY_INSERT FEMAFloodRatings OFF;UPDATE tblFacilities SET FEMA_Flood_Rating = null WHERE FEMA_Flood_Rating NOT IN (SELECT LookupID FROM [Lookup] WHERE LookupType = 'FEMA_Flood_Rating');",
                                OPERATING_CENTER_UPDATE =
                                    "UPDATE [tblFacilities] SET OperatingCenterID = (SELECT OperatingCenterID from OperatingCenters oc WHERE oc.OperatingCenterCode = tblFacilities.OpCode)",
                                OPERATING_CENTER_ROLLBACK =
                                    "UPDATE [tblFacilities] SET OpCode = (SELECT OperatingCenterCode from OperatingCenters oc WHERE oc.OperatingCenterID = tblFacilities.OperatingCenterID)";
        }

        public struct Tables
        {
            public const string FACILITIES = "tblFacilities",
                                FACILITY_OWNERS = "FacilityOwners",
                                FACILITY_STATUSES = "FacilityStatuses",
                                FEMA_FLOOD_RATINGS = "FEMAFloodRatings",
                                TOWNS = "Towns",
                                OPERATING_CENTERS = "OperatingCenters";
        }

        public struct Columns
        {
            public const string DESCRIPTION = "Description",
                                FACILITY_OWNER_ID = "FacilityOwnerID",
                                FACILITY_OWNERSHIP = "Facility_Ownership",
                                FACILITY_STATUS_ID = "FacilityStatusID",
                                STATUS = "Status",
                                FEME_FLOOD_RATING_ID = "FEMAFloodRatingID",
                                FEME_FLOOD_RATING = "FEMA_Flood_Rating",
                                TOWN_ID = "TownID",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                OP_CODE = "OpCode";
        }

        public struct ForeignKeys
        {
            public const string FK_FACILITIES_FACILITY_OWNERS = "FK_tblFacilities_FacilityOwners_Facility_Ownership",
                                FK_FACILITIES_FACILITY_STATUSES = "FK_tblFacilities_FacilityStatuses_Status",
                                FK_FACILITIES_FEMA_FLOOD_RATINGS =
                                    "FK_tblFacilities_FEMAFloodRatings_FEMA_Flood_Rating",
                                FK_FACILITIES_TOWNS = "FK_tblFacilities_Towns_TownID",
                                FK_FACILITIES_OPERATING_CENTERS = "FK_tblFacilities_OperatingCenters_OperatingCenterID";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             OP_CODE = 25;
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.FACILITY_OWNERS)
                  .WithColumn(Columns.FACILITY_OWNER_ID).AsInt32().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.FACILITY_STATUSES)
                  .WithColumn(Columns.FACILITY_STATUS_ID).AsInt32().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.FEMA_FLOOD_RATINGS)
                  .WithColumn(Columns.FEME_FLOOD_RATING_ID).AsInt32().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Alter.Table(Tables.FACILITIES)
                 .AddColumn(Columns.OPERATING_CENTER_ID)
                 .AsInt32().Nullable();
            Execute.Sql(Sql.OPERATING_CENTER_UPDATE);
            Alter.Column(Columns.OPERATING_CENTER_ID).OnTable(Tables.FACILITIES)
                 .AsInt32().NotNullable();
            Alter.Column(Columns.TOWN_ID).OnTable(Tables.FACILITIES)
                 .AsInt32().Nullable();

            Execute.Sql(Sql.FACILITY_OWNERS_INSERT);
            Execute.Sql(Sql.FACILITY_STATUSES_INSERT);
            Execute.Sql(Sql.FEMA_FLOOD_RATINGS_INSERT);

            Create.ForeignKey(ForeignKeys.FK_FACILITIES_FACILITY_OWNERS)
                  .FromTable(Tables.FACILITIES).ForeignColumn(Columns.FACILITY_OWNERSHIP)
                  .ToTable(Tables.FACILITY_OWNERS).PrimaryColumn(Columns.FACILITY_OWNER_ID);
            Create.ForeignKey(ForeignKeys.FK_FACILITIES_FACILITY_STATUSES)
                  .FromTable(Tables.FACILITIES).ForeignColumn(Columns.STATUS)
                  .ToTable(Tables.FACILITY_STATUSES).PrimaryColumn(Columns.FACILITY_STATUS_ID);
            Create.ForeignKey(ForeignKeys.FK_FACILITIES_FEMA_FLOOD_RATINGS)
                  .FromTable(Tables.FACILITIES).ForeignColumn(Columns.FEME_FLOOD_RATING)
                  .ToTable(Tables.FEMA_FLOOD_RATINGS).PrimaryColumn(Columns.FEME_FLOOD_RATING_ID);
            Create.ForeignKey(ForeignKeys.FK_FACILITIES_TOWNS)
                  .FromTable(Tables.FACILITIES).ForeignColumn(Columns.TOWN_ID)
                  .ToTable(Tables.TOWNS).PrimaryColumn(Columns.TOWN_ID);
            Create.ForeignKey(ForeignKeys.FK_FACILITIES_OPERATING_CENTERS)
                  .FromTable(Tables.FACILITIES).ForeignColumn(Columns.OPERATING_CENTER_ID)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);

            Delete.Column(Columns.OP_CODE).FromTable(Tables.FACILITIES);

            Execute.Sql(Sql.FACILITY_OWNERS_PERMISSIONS);
            Execute.Sql(Sql.FACILITY_STATUSES_PERMISSIONS);
            Execute.Sql(Sql.FEMA_FLOOD_RATINGS_PERMISSIONS);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_FACILITIES_FEMA_FLOOD_RATINGS).OnTable(Tables.FACILITIES);
            Delete.ForeignKey(ForeignKeys.FK_FACILITIES_FACILITY_STATUSES).OnTable(Tables.FACILITIES);
            Delete.ForeignKey(ForeignKeys.FK_FACILITIES_FACILITY_OWNERS).OnTable(Tables.FACILITIES);
            Delete.ForeignKey(ForeignKeys.FK_FACILITIES_TOWNS).OnTable(Tables.FACILITIES);
            Delete.ForeignKey(ForeignKeys.FK_FACILITIES_OPERATING_CENTERS).OnTable(Tables.FACILITIES);

            Alter.Column(Columns.TOWN_ID).OnTable(Tables.FACILITIES)
                 .AsFloat().Nullable();
            Alter.Table(Tables.FACILITIES)
                 .AddColumn(Columns.OP_CODE)
                 .AsAnsiString(StringLengths.OP_CODE).Nullable();
            Execute.Sql(Sql.OPERATING_CENTER_ROLLBACK);
            Alter.Column(Columns.OP_CODE)
                 .OnTable(Tables.FACILITIES)
                 .AsAnsiString(StringLengths.OP_CODE).NotNullable();
            Delete.Column(Columns.OPERATING_CENTER_ID).FromTable(Tables.FACILITIES);

            Delete.Table(Tables.FEMA_FLOOD_RATINGS);
            Delete.Table(Tables.FACILITY_STATUSES);
            Delete.Table(Tables.FACILITY_OWNERS);
        }
    }
}
