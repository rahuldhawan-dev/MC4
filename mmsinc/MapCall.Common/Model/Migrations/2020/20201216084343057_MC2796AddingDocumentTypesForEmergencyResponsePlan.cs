using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201216084343057), Tags("Production")]
    public class MC2796AddingDocumentTypesForEmergencyResponsePlan : Migration
    {
        private const string EmergencyResponsePlanTableName = "EmergencyResponsePlans";

        public override void Up()
        {
            this.AddDocumentType("Business Continuity", EmergencyResponsePlanTableName);
            this.AddDocumentType("Conservation / Demand Management", EmergencyResponsePlanTableName);
            this.AddDocumentType("Cyanotoxin Management", EmergencyResponsePlanTableName);
            this.AddDocumentType("Dam Safety", EmergencyResponsePlanTableName);
            this.AddDocumentType("Operations and Maintenance", EmergencyResponsePlanTableName);
            this.AddDocumentType("Other", EmergencyResponsePlanTableName);
            this.AddDocumentType("Process Safety / Risk Management", EmergencyResponsePlanTableName);
            this.AddDocumentType("Source Water Contingency", EmergencyResponsePlanTableName);
            this.AddDocumentType("Source Water Protection", EmergencyResponsePlanTableName);
            this.AddDocumentType("Spill Prevention", EmergencyResponsePlanTableName);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Business Continuity", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Conservation / Demand Management",
                EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Cyanotoxin Management", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Dam Safety", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Operations and Maintenance", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Other", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Process Safety / Risk Management",
                EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Source Water Contingency", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Source Water Protection", EmergencyResponsePlanTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Spill Prevention", EmergencyResponsePlanTableName);
        }
    }
}
