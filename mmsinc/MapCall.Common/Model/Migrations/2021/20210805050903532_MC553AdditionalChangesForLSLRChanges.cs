using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210805050903532), Tags("Production")]
    public class MC553AdditionalChangesForLSLRChanges : Migration
    {
        public override void Up()
        {
            Alter.Table("ServiceFlushes").AddColumn("SampleId").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("SampleId").FromTable("ServiceFlushes");
        }
    }
}

