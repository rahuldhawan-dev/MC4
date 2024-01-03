using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160712141246464), Tags("Production")]
    public class AddIsAWRContractorToContractorsForBug3034 : Migration
    {
        public override void Up()
        {
            Alter.Table("Contractors").AddColumn("AWR").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("AWR").FromTable("Contractors");
        }
    }
}
