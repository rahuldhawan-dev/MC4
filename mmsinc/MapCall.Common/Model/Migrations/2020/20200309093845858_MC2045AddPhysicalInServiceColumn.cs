using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2020
{
    // Add this Migration Version Number in your migration name
    [Migration(20200309093845858), Tags("Production")]
    public class MC2045AddPhysicalInServiceColumn : Migration
    {
        public override void Up()
        {
            // Up is for changes you want to make
            Alter.Table("AsBuiltImages").AddColumn("PhysicalInService").AsDateTime().Nullable();
            this.CreateNotificationPurpose("Field Services", "Images", "AsBuiltImage Coordinate Changed");
        }

        public override void Down()
        {
            // Down is for rollbacks in case we need to rollback
            Delete.Column("PhysicalInService").FromTable("AsBuiltImages");
            this.DeleteNotificationPurpose("Field Services", "Images", "AsBuiltImage Coordinate Changed");
        }
    }
}
