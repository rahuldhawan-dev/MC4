using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210607142919407), Tags("Production")]
    public class MC3285DropDuplicateConstraintsFromSampleSites : Migration
    {
        public override void Up()
        {
            Delete.UniqueConstraint("FK_tblWQSample_Sites_Lookup_Site_Status_ID").FromTable("SampleSites");
            Delete.UniqueConstraint("FK_tblWQSample_Sites_Lookup_Availability").FromTable("SampleSites");
        }

        public override void Down()
        {
            // noop - These constraints don't need to come back
        }
    }
}

