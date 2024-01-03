using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130327113825), Tags("Production")]
    public class AddCoordinatesModifiedOn : Migration
    {
        #region Constants

        public const string TABLE_NAME = "AsBuiltImages";

        public struct Sql
        {
            public const string UPDATE_DATE_INSTALLED =
                                    "UPDATE AsBuiltImages SET DateInstalled = CAST(LEFT(DateInstalled, 2) + '/' + SUBSTRING(DateInstalled, 3, 2) + '/' + SUBSTRING(DateInstalled, 5, 4) as DateTime) WHERE ISDATE(DateInstalled) <> 1 and LEN(DateInstalled) = 8 and CHARINDEX('/', DateInstalled) = 0",
                                UPDATE_DATE_INSTALLED_REMAINING =
                                    "UPDATE AsBuiltImages SET DateInstalled = null WHERE ISDATE(DateInstalled) <> 1 ",
                                UPDATE_OPERATING_CENTER_ID =
                                    "UPDATE AsBuiltImages SET OperatingCenterID = (Select OperatingCenterID from OperatingCenters where OperatingCenters.OperatingCenterCode = AsBuiltImages.OperatingCenter)",
                                UPDATE_OPERATING_CENTER =
                                    "UPDATE AsBuiltImages SET OperatingCenter = (Select OperatingCenterCode from OperatingCenters OC WHERE OC.OperatingCenterID = AsBuiltImages.OperatingCenterID)",
                                UPDATE_STATE =
                                    "UPDATE AsBuiltImages SET State = (Select Abbreviation from States where States.StateID = Towns.StateID) from AsBuiltImages JOIN Towns ON Towns.TownID = AsBuiltImages.TownID",
                                UPDATE_COUNTY =
                                    "UPDATE AsBuiltImages SET County = (Select Name from Counties where Counties.CountyID = Towns.CountyID) FROM AsBuiltImages JOIN Towns on Towns.TownID = AsBuiltImages.TownID",
                                UPDATE_TOWN =
                                    "UPDATE AsBuiltImages SET Town = (Select Town from Towns where Towns.TownID = AsBuiltImages.TownID)";
        }

        #endregion

        public override void Up()
        {
            //coordinates modified on
            Alter.Table(TABLE_NAME)
                 .AddColumn("CoordinatesModifiedOn").AsDateTime().Nullable();

            //date installed
            Execute.Sql(Sql.UPDATE_DATE_INSTALLED);
            Execute.Sql(Sql.UPDATE_DATE_INSTALLED_REMAINING);
            Alter.Table(TABLE_NAME).AlterColumn("DateInstalled").AsDateTime().Nullable();

            ////state, county, town - these fields are read from TownID
            Delete.Column("State").FromTable(TABLE_NAME);
            Delete.Column("County").FromTable(TABLE_NAME);
            Delete.Column("Town").FromTable(TABLE_NAME);

            ////Operating Center?
            Alter.Table(TABLE_NAME).AddColumn("OperatingCenterID").AsInt32().Nullable();
            Execute.Sql(Sql.UPDATE_OPERATING_CENTER_ID);
            Create.ForeignKey("FK_AsBuiltImages_OperatingCenters_OperatingCenterID")
                  .FromTable(TABLE_NAME).ForeignColumn("OperatingCenterID")
                  .ToTable("OperatingCenters").PrimaryColumn("OperatingCenterID");
            Delete.Column("OperatingCenter").FromTable(TABLE_NAME);
        }

        public override void Down()
        {
            //state, county
            //Alter.Column("State").OnTable(TABLE_NAME).AsAnsiString(2).NotNullable();
            Alter.Table(TABLE_NAME).AddColumn("OperatingCenter").AsAnsiString(50).Nullable();
            Execute.Sql(Sql.UPDATE_OPERATING_CENTER);
            Delete.ForeignKey("FK_AsBuiltImages_OperatingCenters_OperatingCenterID").OnTable(TABLE_NAME);
            Delete.Column("OperatingCenterID").FromTable(TABLE_NAME);
            Alter.Table(TABLE_NAME).AddColumn("State").AsAnsiString(2).Nullable();
            Execute.Sql(Sql.UPDATE_STATE);
            Alter.Column("State").OnTable(TABLE_NAME).AsAnsiString(2).NotNullable();
            Alter.Table(TABLE_NAME).AddColumn("County").AsAnsiString(50).Nullable();
            Execute.Sql(Sql.UPDATE_COUNTY);
            Alter.Table(TABLE_NAME).AddColumn("Town").AsAnsiString(50).Nullable();
            Execute.Sql(Sql.UPDATE_TOWN);

            //date installed
            Delete.Column("CoordinatesModifiedOn").FromTable(TABLE_NAME);

            //coordinates modified on
            Alter.Table(TABLE_NAME).AlterColumn("DateInstalled").AsAnsiString(50).Nullable();
        }
    }
}
