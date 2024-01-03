using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170207103520242), Tags("Production")]
    public class Bug3533 : Migration
    {
        public override void Up()
        {
            Create.Column("OperatedByOperatingCenterId").OnTable("OperatingCenters")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_OperatingCenters_OperatedByOperatingCenterId", "OperatingCenters",
                       "OperatingCenterId");

            // Set the existing SOV -> NJ6 used by JobSiteCheckLists.
            Update.Table("OperatingCenters").Set(new {OperatedByOperatingCenterId = 12})
                  .Where(new {OperatingCenterId = 93});

            // EW1 manages EW4
            Update.Table("OperatingCenters").Set(new {OperatedByOperatingCenterId = 15})
                  .Where(new {OperatingCenterId = 19});

            // EW1 also manages LWC
            Update.Table("OperatingCenters").Set(new {OperatedByOperatingCenterId = 15})
                  .Where(new {OperatingCenterId = 18});
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OperatingCenters_OperatedByOperatingCenterId").OnTable("OperatingCenters");
            Delete.Column("OperatedByOperatingCenterId").FromTable("OperatingCenters");
        }
    }
}
