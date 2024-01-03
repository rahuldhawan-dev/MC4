using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160519104353050), Tags("Production")]
    public class MainCrossingModificationsBug2957 : Migration
    {
        public override void Up()
        {
            // NOTE: No values were given for this in the bug.
            this.CreateLookupTableWithValues("RailwayOwnerTypes");

            Alter.Table("MainCrossings")
                 .AddColumn("RailwayOwnerTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_MainCrossings_RailwayOwnerTypes_RailwayOwnerTypeId", "RailwayOwnerTypes", "Id")
                 .AddColumn("RailwayCrossingId").AsString(10).Nullable()
                 .AddColumn("EmergencyPhoneNumber").AsString(10).Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_MainCrossings_RailwayOwnerTypes_RailwayOwnerTypeId").OnTable("MainCrossings");
            Delete.Column("RailwayOwnerTypeId").FromTable("MainCrossings");
            Delete.Column("RailwayCrossingId").FromTable("MainCrossings");
            Delete.Column("EmergencyPhoneNumber").FromTable("MainCrossings");
            Delete.Table("RailwayOwnerTypes");
        }
    }
}
