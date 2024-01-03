using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140320163100), Tags("Production")]
    public class AddContractorForEstimatingProjects : Migration
    {
        public override void Up()
        {
            Alter.Table("EstimatingProjects")
                 .AddColumn("ContractorId").AsInt32().Nullable()
                 .ForeignKey("FK_EstimatingProjects_Contractors_ContractorId", "Contractors", "ContractorID");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_EstimatingProjects_Contractors_ContractorId").OnTable("EstimatingProjects");

            Delete.Column("ContractorId").FromTable("EstimatingProjects");
        }
    }
}
