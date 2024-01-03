using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150910122044616), Tags("Production")]
    public class AddRoadwayNotificationStreetsForBug2596 : Migration
    {
        public struct TableNames
        {
            public const string ROADWAY_NOTIFICATION_STREETS = "RoadwayNotificationStreets",
                                ROADWAY_NOTIFICATION_STREET_STATUSES = "RoadwayImprovementNotificationStreetStatuses";
        }

        public struct StringLengths
        {
            public const int START_POINT = 25, TERMINUS = 25;
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.ROADWAY_NOTIFICATION_STREET_STATUSES, "Pending", "Complete");

            Create.Table(TableNames.ROADWAY_NOTIFICATION_STREETS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("RoadwayImprovementNotificationId", "RoadwayImprovementNotifications",
                       nullable: false)
                  .WithForeignKeyColumn("StreetId", "Streets", "StreetId", false)
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID")
                  .WithColumn("StartPoint").AsAnsiString(StringLengths.START_POINT).Nullable()
                  .WithColumn("Terminus").AsAnsiString(StringLengths.TERMINUS).Nullable()
                  .WithForeignKeyColumn("MainSizeId", "MainSizes", "MainSizeID")
                  .WithForeignKeyColumn("MainTypeId", "MainTypes")
                  .WithColumn("MainBreakActivity").AsInt32().Nullable()
                  .WithColumn("NumberOfServicesToBeReplaced").AsInt32().Nullable()
                  .WithForeignKeyColumn("ReviewStatusId", TableNames.ROADWAY_NOTIFICATION_STREET_STATUSES);

            Delete.ForeignKey(Utilities.CreateForeignKeyName("RoadwayImprovementNotifications",
                       "RoadwayImprovementNotificationStatuses", "RoadwayImprovementNotificationEntityId"))
                  .OnTable("RoadwayImprovementNotifications");
            Create.ForeignKey(
                       Utilities.CreateForeignKeyName("RoadwayImprovementNotifications",
                           "RoadwayImprovementNotificationEntities", "RoadwayImprovementNotificationEntityId"))
                  .FromTable("RoadwayImprovementNotifications")
                  .ForeignColumn("RoadwayImprovementNotificationEntityId")
                  .ToTable("RoadwayImprovementNotificationEntities")
                  .PrimaryColumn("Id");

            Execute.Sql(
                "INSERT INTO DataType(Data_Type, Table_Name) Values('RoadwayImprovementNotification', 'RoadwayImprovementNotifications')");
        }

        public override void Down()
        {
            Execute.Sql(@"
                declare @dataType int
                set @dataType = (select top 1 DataTypeID from DataType WHERE Data_Type = 'RoadwayImprovementNotification' AND Table_Name = 'RoadwayImprovementNotifications')
delete from DocumentLink where DataTypeID = @dataType
delete from Document where documentTypeId in (select DocumentTypeId from DocumentType where DataTypeId = @dataType)    
delete from DocumentType where DataTypeID = @dataType

");
            Execute.Sql(
                "DELETE FROM DataType WHERE Data_Type = 'RoadwayImprovementNotification' AND Table_Name = 'RoadwayImprovementNotifications'");
            Delete.ForeignKey(Utilities.CreateForeignKeyName("RoadwayImprovementNotifications",
                       "RoadwayImprovementNotificationEntities", "RoadwayImprovementNotificationEntityId"))
                  .OnTable("RoadwayImprovementNotifications");
            Create.ForeignKey(
                       Utilities.CreateForeignKeyName("RoadwayImprovementNotifications",
                           "RoadwayImprovementNotificationStatuses", "RoadwayImprovementNotificationEntityId"))
                  .FromTable("RoadwayImprovementNotifications")
                  .ForeignColumn("RoadwayImprovementNotificationEntityId")
                  .ToTable("RoadwayImprovementNotificationEntities")
                  .PrimaryColumn("Id");

            Delete.Table(TableNames.ROADWAY_NOTIFICATION_STREETS);
            Delete.Table(TableNames.ROADWAY_NOTIFICATION_STREET_STATUSES);
        }
    }
}
