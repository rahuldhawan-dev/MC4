using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161128104245167), Tags("Production")]
    public class RestorationsBug3366 : Migration
    {
        public override void Up()
        {
            Alter.Table("Restorations")
                 .AddColumn("CreatedByContractorId").AsInt32()
                 .ForeignKey("FK_Restorations_Contractors_CreatedByContractorId", "Contractors", "ContractorId")
                 .Nullable()
                 .AddColumn("CreatedByContractorAt").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Restorations_Contractors_CreatedByContractorId").OnTable("Restorations");
            Delete.Column("CreatedByContractorId").FromTable("Restorations");
            Delete.Column("CreatedByContractorAt").FromTable("Restorations");
        }
    }
}
