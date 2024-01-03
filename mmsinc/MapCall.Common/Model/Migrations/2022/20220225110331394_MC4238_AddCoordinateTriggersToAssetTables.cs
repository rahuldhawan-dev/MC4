using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220225110331394), Tags("Production")]
    public class MC4238_AddCoordinateTriggersToAssetTables : Migration
    {
        public override void Up()
        {
            // Hydrants
            Execute.Sql(@"
CREATE TRIGGER TR_HydrantUpdateCoordinatesOnInsert ON Hydrants 
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                           
    UPDATE Hydrants
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Hydrants h
    INNER JOIN Coordinates c
    ON c.CoordinateID = h.CoordinateId
    WHERE h.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            Execute.Sql(@"
CREATE TRIGGER TR_HydrantUpdateCoordinatesOnUpdate ON Hydrants 
AFTER UPDATE
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                            	
    UPDATE Hydrants
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Hydrants h
    INNER JOIN Coordinates c
    ON c.CoordinateID = h.CoordinateId
    WHERE h.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            // Valves
            
            Execute.Sql(@"
CREATE TRIGGER TR_ValveUpdateCoordinatesOnInsert ON Valves 
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                           	
    UPDATE Valves
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Valves v
    INNER JOIN Coordinates c
    ON c.CoordinateID = v.CoordinateId
    WHERE v.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            Execute.Sql(@"
CREATE TRIGGER TR_ValveUpdateCoordinatesOnUpdate ON Valves 
AFTER UPDATE
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                            	
    UPDATE Valves
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Valves v
    INNER JOIN Coordinates c
    ON c.CoordinateID = v.CoordinateId
    WHERE v.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            // MainCrossings
            Execute.Sql(@"
CREATE TRIGGER TR_MainCrossingUpdateCoordinatesOnInsert ON MainCrossings 
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON
                         	
    DECLARE @Id int
    SELECT @Id = MainCrossingID FROM inserted
                           	
    UPDATE MainCrossings
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM MainCrossings m
    INNER JOIN Coordinates c
    ON c.CoordinateID = m.CoordinateId
    WHERE m.MainCrossingID = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            Execute.Sql(@"
CREATE TRIGGER TR_MainCrossingUpdateCoordinatesOnUpdate ON MainCrossings 
AFTER UPDATE
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = MainCrossingID FROM inserted
                            	
    UPDATE MainCrossings
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM MainCrossings m
    INNER JOIN Coordinates c
    ON c.CoordinateID = m.CoordinateId
    WHERE m.MainCrossingId = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            // Equipment 

            Execute.Sql(@"
CREATE TRIGGER TR_EquipmentUpdateCoordinatesOnInsert ON Equipment 
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON
                         	
    DECLARE @Id int
    SELECT @Id = EquipmentID FROM inserted
                           	
    UPDATE Equipment
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Equipment e
    INNER JOIN Coordinates c
    ON c.CoordinateID = e.CoordinateId
    WHERE e.EquipmentID = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            Execute.Sql(@"
CREATE TRIGGER TR_EquipmentUpdateCoordinatesOnUpdate ON Equipment 
AFTER UPDATE
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = EquipmentID FROM inserted
                            	
    UPDATE Equipment
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Equipment e
    INNER JOIN Coordinates c
    ON c.CoordinateID = e.CoordinateId
    WHERE e.EquipmentID = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            // Sewer Openings
            Execute.Sql(@"
CREATE TRIGGER TR_SewerOpeningUpdateCoordinatesOnInsert ON SewerOpenings 
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                          	
    UPDATE SewerOpenings
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM SewerOpenings s
    INNER JOIN Coordinates c
    ON c.CoordinateID = s.CoordinateId
    WHERE s.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            Execute.Sql(@"
CREATE TRIGGER TR_SewerOpeningUpdateCoordinatesOnUpdate ON SewerOpenings 
AFTER UPDATE
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                          	
    UPDATE SewerOpenings
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM SewerOpenings s
    INNER JOIN Coordinates c
    ON c.CoordinateID = s.CoordinateId
    WHERE s.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            // Services
            Execute.Sql(@"
CREATE TRIGGER TR_ServiceUpdateCoordinatesOnInsert ON Services 
AFTER INSERT
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                          	
    UPDATE Services
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Services s
    INNER JOIN Coordinates c
    ON c.CoordinateID = s.CoordinateId
    WHERE s.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");

            Execute.Sql(@"
CREATE TRIGGER TR_ServiceUpdateCoordinatesOnUpdate ON Services 
AFTER UPDATE
AS
BEGIN

    SET NOCOUNT ON
                          	
    DECLARE @Id int
    SELECT @Id = Id FROM inserted
                          	
    UPDATE Services
    SET Latitude = c.Latitude,
    Longitude = c.Longitude
    FROM Services s
    INNER JOIN Coordinates c
    ON c.CoordinateID = s.CoordinateId
    WHERE s.Id = @Id
    AND NOT (c.Latitude > 90 or c.Latitude < -90)
    AND NOT (c.Longitude > 180 or c.Longitude < -180)
END");
        }
        public override void Down()
        {
            Execute.Sql("DROP TRIGGER TR_HydrantUpdateCoordinatesOnInsert");
            Execute.Sql("DROP TRIGGER TR_HydrantUpdateCoordinatesOnUpdate");

            Execute.Sql("DROP TRIGGER TR_ValveUpdateCoordinatesOnInsert");
            Execute.Sql("DROP TRIGGER TR_ValveUpdateCoordinatesOnUpdate");

            Execute.Sql("DROP TRIGGER TR_MainCrossingUpdateCoordinatesOnInsert");
            Execute.Sql("DROP TRIGGER TR_MainCrossingUpdateCoordinatesOnUpdate");

            Execute.Sql("DROP TRIGGER TR_EquipmentUpdateCoordinatesOnInsert");
            Execute.Sql("DROP TRIGGER TR_EquipmentUpdateCoordinatesOnUpdate");

            Execute.Sql("DROP TRIGGER TR_SewerOpeningUpdateCoordinatesOnInsert");
            Execute.Sql("DROP TRIGGER TR_SewerOpeningUpdateCoordinatesOnUpdate");

            Execute.Sql("DROP TRIGGER TR_ServiceUpdateCoordinatesOnInsert");
            Execute.Sql("DROP TRIGGER TR_ServiceUpdateCoordinatesOnUpdate");
        }
    }
}

