using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170417164951135), Tags("Production")]
    public class AddMapIdCoordinateToOperatingCenterForBug3769 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters")
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId")
                 .AddColumn("MapId").AsAnsiString(32).Nullable();
            Execute.Sql(@"
                UPDATE OperatingCenters set MapId = (select MapId from GisLayerUpdates)
                SET NOCOUNT ON 
                DECLARE @latitude float
                DECLARE @longitude float
                DECLARE @OperatingCenterId int
                DECLARE @coordinateID int

                DECLARE	tableCursor 
                CURSOR FOR 
	                SELECT OperatingCenterId, 40.3224 as latitude, -74.1481 as longitude FROM OperatingCenters 

                OPEN tableCursor 
	                FETCH NEXT FROM tableCursor INTO @OperatingCenterId, @latitude, @longitude; 
	                WHILE @@FETCH_STATUS = 0 
	                BEGIN 
		                Insert Into Coordinates(latitude, longitude) values(@latitude, @longitude)
		                update OperatingCenters set CoordinateId = @@Identity where OperatingCenterID = @OperatingCenterId
		                FETCH NEXT FROM tableCursor INTO @OperatingCenterId, @latitude, @longitude; 
	                END
                CLOSE tableCursor; 
                DEALLOCATE tableCursor;");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("OperatingCenters", "CoordinateId", "Coordinates", "CoordinateID");
            Delete.Column("MapId").FromTable("OperatingCenters");
        }
    }
}
