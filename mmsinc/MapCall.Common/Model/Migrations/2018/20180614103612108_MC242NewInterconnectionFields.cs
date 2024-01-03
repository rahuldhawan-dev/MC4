using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180614103612108), Tags("Production")]
    public class MC242NewInterconnectionFields : Migration
    {
        public override void Up()
        {
            Create.Column("ContractStartDate").OnTable("Interconnections").AsDateTime().Nullable();
            Create.Column("ContractEndDate").OnTable("Interconnections").AsDateTime().Nullable();
            Create.Column("ContractEndNotificationSent").OnTable("Interconnections").AsBoolean().NotNullable()
                  .WithDefaultValue(false);

            Delete.Column("FacilityName").FromTable("Interconnections");
            Rename.Column("NJDEPDesignation").OnTable("Interconnections").To("DEPDesignation");

            this.AddDocumentType("General", "Interconnections");
            this.AddNotificationType("Human Resources", "Facilities", "Interconnection Contract Ends In 30 Days");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Human Resources", "Facilities", "Interconnection Contract Ends In 30 Days");
            this.RemoveDocumentTypeAndAllRelatedDocuments("General", "Interconnections");

            Rename.Column("DEPDesignation").OnTable("Interconnections").To("NJDEPDesignation");
            Create.Column("FacilityName").OnTable("Interconnections").AsString(75).Nullable();

            Delete.Column("ContractStartDate").FromTable("Interconnections");
            Delete.Column("ContractEndDate").FromTable("Interconnections");
            Delete.Column("ContractEndNotificationSent").FromTable("Interconnections");
        }
    }
}
