using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131008145659), Tags("Production")]
    public class AddCustomerLocations : Migration
    {
        #region Constants

        public struct Columns
        {
            public const string CUSTOMER_LOCATION_ID = "CustomerLocationID",
                                CUSTOMER_COORDINATE_ID = "CustomerCoordinateID",
                                PREMISE_NUMBER = "PremiseNumber",
                                ADDRESS = "Address",
                                CITY = "City",
                                STATE = "State",
                                ZIP = "Zip",
                                COORDINATE_ID = "CoordinateID",
                                ACCURACY = "Accuracy",
                                LATITUDE = "Latitude",
                                LONGITUDE = "Longitude",
                                SOURCE = "Source",
                                VERIFIED = "Verified";
        }

        public struct Tables
        {
            public const string CUSTOMER_LOCATIONS = "CustomerLocations",
                                CUSTOMER_COORDINATES = "CustomerCoordinates";
        }

        public struct ForeignKeys
        {
            public const string FK_CUSTOMER_LOCATIONS_CUSTOMER_COORDINATES =
                "FK_CustomerCoordinates_CustomerLocations_CustomerLocationId";
        }

        public struct StringLengths
        {
            public const int PREMISE_NUMBER = 9,
                             ADDRESS = 50,
                             CITY = 25,
                             STATE = 2,
                             ZIP = 10;
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.CUSTOMER_LOCATIONS)
                  .WithColumn(Columns.CUSTOMER_LOCATION_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.PREMISE_NUMBER).AsAnsiString(StringLengths.PREMISE_NUMBER).NotNullable()
                  .WithColumn(Columns.ADDRESS).AsAnsiString(StringLengths.ADDRESS).Nullable()
                  .WithColumn(Columns.CITY).AsAnsiString(StringLengths.CITY).Nullable()
                  .WithColumn(Columns.STATE).AsAnsiString(StringLengths.STATE).Nullable()
                  .WithColumn(Columns.ZIP).AsAnsiString(StringLengths.ZIP).Nullable();

            Create.Table(Tables.CUSTOMER_COORDINATES)
                  .WithColumn(Columns.CUSTOMER_COORDINATE_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.CUSTOMER_LOCATION_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.LATITUDE).AsCustom("float").NotNullable()
                  .WithColumn(Columns.LONGITUDE).AsCustom("float").NotNullable()
                  .WithColumn(Columns.SOURCE).AsInt32().NotNullable()
                  .WithColumn(Columns.ACCURACY).AsInt32().Nullable()
                  .WithColumn(Columns.VERIFIED).AsBoolean().Nullable();

            Create.ForeignKey(ForeignKeys.FK_CUSTOMER_LOCATIONS_CUSTOMER_COORDINATES)
                  .FromTable(Tables.CUSTOMER_COORDINATES).ForeignColumn(Columns.CUSTOMER_LOCATION_ID)
                  .ToTable(Tables.CUSTOMER_LOCATIONS).PrimaryColumn(Columns.CUSTOMER_LOCATION_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_CUSTOMER_LOCATIONS_CUSTOMER_COORDINATES)
                  .OnTable(Tables.CUSTOMER_COORDINATES);

            Delete.Table(Tables.CUSTOMER_COORDINATES);
            Delete.Table(Tables.CUSTOMER_LOCATIONS);
        }
    }
}
