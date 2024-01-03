using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200529111036251), Tags("Production")]
    public class AddLeakMonitoringTableForMC2130 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("PointOfInterestStatuses", "TBD1", "TBD2",
                "Field Investigation Recommended");

            Create.Table("EchoshoreSites")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(255).NotNullable()
                  .WithForeignKeyColumn("TownId", "Towns", "TownID")
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID");

            Create.Table("EchoshoreLeakAlerts")
                  .WithIdentityColumn()
                  .WithColumn("PersistedCorrelatedNoiseId").AsInt32().NotNullable()
                  .WithColumn("DateCreated").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("PointOfInterestStatusId", "PointOfInterestStatuses", nullable: false)
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID", false).NotNullable()
                  .WithForeignKeyColumn("SockedId1Id", "Hydrants")
                  .WithForeignKeyColumn("SockedId2Id", "Hydrants")
                  .WithColumn("SocketId1Text").AsAnsiString(20).Nullable()
                  .WithColumn("SocketId2Text").AsAnsiString(20).Nullable()
                  .WithColumn("DistanceFrom1").AsDecimal(18, 5).NotNullable()
                  .WithColumn("DistanceFrom2").AsDecimal(18, 5).NotNullable()
                  .WithColumn("Note").AsCustom("varchar(max)").Nullable()
                  .WithForeignKeyColumn("EchoshoreSiteId", "EchoshoreSites", nullable: false);
            Insert.IntoTable("EchoshoreSites")
                  .Row(new {Description = "Berkeleyheights", TownId = 87, OperatingCenterId = 12})
                  .Row(new {Description = "Chatham", TownId = 130, OperatingCenterId = 12})
                  .Row(new {Description = "dunellensouthplainfield", TownId = 226, OperatingCenterId = 15})
                  .Row(new {Description = "hillside", TownId = 112, OperatingCenterId = 12})
                  .Row(new {Description = "irvingtonld", TownId = 95, OperatingCenterId = 12})
                  .Row(new {Description = "Jamesburg", TownId = 185, OperatingCenterId = 16})
                  .Row(new {Description = "Kenilworth", TownId = 259, OperatingCenterId = 15})
                  .Row(new {Description = "Knollcroft", TownId = 88, OperatingCenterId = 12})
                  .Row(new {Description = "Littlefallsnj", TownId = 118, OperatingCenterId = 12})
                  .Row(new {Description = "Longhill", TownId = 128, OperatingCenterId = 12})
                  .Row(new {Description = "Manville", TownId = 245, OperatingCenterId = 16})
                  .Row(new {Description = "Maplewood", TownId = 98, OperatingCenterId = 12})
                  .Row(new {Description = "Mounthoreb", TownId = 114, OperatingCenterId = 12})
                  .Row(new {Description = "Piscataway", TownId = 232, OperatingCenterId = 15})
                  .Row(new {Description = "Raritantownship", TownId = 211, OperatingCenterId = 16})
                  .Row(new {Description = "Roselle", TownId = 264, OperatingCenterId = 15})
                  .Row(new {Description = "Somerville", TownId = 270, OperatingCenterId = 16})
                  .Row(new {Description = "Summit", TownId = 107, OperatingCenterId = 12})
                  .Row(new {Description = "Njawunion", TownId = 109, OperatingCenterId = 15})
                  .Row(new {Description = "njaw-westorange", TownId = 96, OperatingCenterId = 12})
                ;
        }

        public override void Down()
        {
            Delete.Table("EchoshoreLeakAlerts");
            Delete.Table("EchoshoreSites");
            Delete.Table("PointOfInterestStatuses");
        }
    }
}
