using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190813101743462), Tags("Production")]
    public class MakeSampleIdMatrixSampleSiteIdNotNullable : Migration
    {
        public override void Up()
        {
            Alter.Column("SampleSiteId").OnTable("SampleIDMatrices")
                 .AsForeignKey("SampleSiteId", "tblWQSample_Sites", "SampleSiteId");
        }

        public override void Down() { }
    }
}
