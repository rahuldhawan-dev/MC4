using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161103143245585), Tags("Production")]
    public class Bug3268 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters").AddColumn("IsContractedOperations").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            // true only for  EW4, LWC, SOV

            Update.Table("OperatingCenters").Set(new {IsContractedOperations = true})
                  .Where(new {OperatingCenterCode = "EW4"});
            Update.Table("OperatingCenters").Set(new {IsContractedOperations = true})
                  .Where(new {OperatingCenterCode = "LWC"});
            Update.Table("OperatingCenters").Set(new {IsContractedOperations = true})
                  .Where(new {OperatingCenterCode = "SOV"});

            Alter.Table("FunctionalLocations").AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("FunctionalLocations");
            Delete.Column("IsContractedOperations").FromTable("OperatingCenters");
        }
    }
}
