using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220121080223453), Tags("Production")]
    public class MC3207_AddLongitudeAndLatitudeColumnsToAssets : Migration
    {
        // This is so we can easily pull the coordinate information from V1 to V2. 
        // Assets include Hydrants, Valves, Main Crossings, Equipment, SewerOpenings, Services
        public override void Up()
        {
            Alter.Table("Hydrants").AddColumn("Longitude").AsDecimal(10, 7).Nullable();
            Alter.Table("Hydrants").AddColumn("Latitude").AsDecimal(9, 7).Nullable();

            Alter.Table("Valves").AddColumn("Longitude").AsDecimal(10, 7).Nullable();
            Alter.Table("Valves").AddColumn("Latitude").AsDecimal(9, 7).Nullable();

            Alter.Table("MainCrossings").AddColumn("Longitude").AsDecimal(10, 7).Nullable();
            Alter.Table("MainCrossings").AddColumn("Latitude").AsDecimal(9, 7).Nullable();

            Alter.Table("Equipment").AddColumn("Longitude").AsDecimal(10, 7).Nullable();
            Alter.Table("Equipment").AddColumn("Latitude").AsDecimal(9, 7).Nullable();

            Alter.Table("SewerOpenings").AddColumn("Longitude").AsDecimal(10, 7).Nullable();
            Alter.Table("SewerOpenings").AddColumn("Latitude").AsDecimal(9, 7).Nullable();

            Alter.Table("Services").AddColumn("Longitude").AsDecimal(10, 7).Nullable();
            Alter.Table("Services").AddColumn("Latitude").AsDecimal(9, 7).Nullable();

            // Back fill the data from the coordinate id

            // Hydrants
            Execute.Sql(@"UPDATE Hydrants 
                          SET Longitude = c.Longitude,
                          Latitude = c.Latitude
                          FROM Hydrants Hyd
                          INNER JOIN Coordinates c 
                          ON c.CoordinateID = hyd.CoordinateId
                          WHERE NOT (c.Latitude > 90 or c.Latitude < -90)
						  AND NOT (c.Longitude > 180 or c.Longitude < -180)");

            // Valves
            Execute.Sql(@"UPDATE Valves 
                          SET Longitude = c.Longitude,
                          Latitude = c.Latitude
                          FROM Valves V
                          INNER JOIN Coordinates c 
                          ON c.CoordinateID = V.CoordinateId
                          WHERE NOT (c.Latitude > 90 or c.Latitude < -90)
						  AND NOT (c.Longitude > 180 or c.Longitude < -180)");

            // Main Crossing
            Execute.Sql(@"UPDATE MainCrossings 
                          SET Longitude = c.Longitude,
                          Latitude = c.Latitude
                          FROM MainCrossings MC
                          INNER JOIN Coordinates c 
                          ON c.CoordinateID = MC.CoordinateId
                          WHERE NOT (c.Latitude > 90 or c.Latitude < -90)
						  AND NOT (c.Longitude > 180 or c.Longitude < -180)");
            // Equipment
            Execute.Sql(@"UPDATE Equipment 
                          SET Longitude = c.Longitude,
                          Latitude = c.Latitude
                          FROM Equipment E
                          INNER JOIN Coordinates c 
                          ON c.CoordinateID = E.CoordinateId
                          WHERE NOT (c.Latitude > 90 or c.Latitude < -90)
						  AND NOT (c.Longitude > 180 or c.Longitude < -180)");

            // Sewer Openings
            Execute.Sql(@"UPDATE SewerOpenings 
                          SET Longitude = c.Longitude,
                          Latitude = c.Latitude
                          FROM SewerOpenings SO
                          INNER JOIN Coordinates c 
                          ON c.CoordinateID = SO.CoordinateId
                          WHERE NOT (c.Latitude > 90 or c.Latitude < -90)
						  AND NOT (c.Longitude > 180 or c.Longitude < -180)");

            // Services
            Execute.Sql(@"UPDATE Services 
                          SET Longitude = c.Longitude,
                          Latitude = c.Latitude
                          FROM Services S
                          INNER JOIN Coordinates c 
                          ON c.CoordinateID = S.CoordinateId
                          WHERE NOT (c.Latitude > 90 or c.Latitude < -90)
						  AND NOT (c.Longitude > 180 or c.Longitude < -180)");
        }

        public override void Down()
        {
            Delete.Column("Longitude").FromTable("Hydrants");
            Delete.Column("Latitude").FromTable("Hydrants");

            Delete.Column("Longitude").FromTable("Valves");
            Delete.Column("Latitude").FromTable("Valves");

            Delete.Column("Longitude").FromTable("MainCrossings");
            Delete.Column("Latitude").FromTable("MainCrossings");

            Delete.Column("Longitude").FromTable("Equipment");
            Delete.Column("Latitude").FromTable("Equipment");

            Delete.Column("Longitude").FromTable("SewerOpenings");
            Delete.Column("Latitude").FromTable("SewerOpenings");

            Delete.Column("Longitude").FromTable("Services");
            Delete.Column("Latitude").FromTable("Services");
        }
    }
}

