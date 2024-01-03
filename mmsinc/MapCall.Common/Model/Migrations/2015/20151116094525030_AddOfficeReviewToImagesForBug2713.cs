using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151116094525030), Tags("Production")]
    public class AddOfficeReviewToImagesForBug2713 : Migration
    {
        public override void Up()
        {
            Alter.Table("TapImages").AddColumn("OfficeReviewRequired").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Alter.Table("ValveImages").AddColumn("OfficeReviewRequired").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Alter.Table("AsBuiltImages").AddColumn("OfficeReviewRequired").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("OfficeReviewRequired").FromTable("AsBuiltImages");
            Delete.Column("OfficeReviewRequired").FromTable("ValveImages");
            Delete.Column("OfficeReviewRequired").FromTable("TapImages");
        }
    }
}
