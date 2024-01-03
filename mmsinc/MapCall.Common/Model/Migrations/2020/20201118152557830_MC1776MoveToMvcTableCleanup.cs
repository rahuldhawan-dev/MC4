using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201118152557830), Tags("Production")]
    public class MC1776MoveToMvcTableCleanup : Migration
    {
        private const string EVENTS = "Events", EVENTS_OLD_NAME = "tblEventManagement";

        public override void Up()
        {
            Execute.Sql("ALTER TABLE tblEventManagement DROP CONSTRAINT FK_tblEventManagement_Lookup_Event_Category");
            Execute.Sql(
                "ALTER TABLE tblEventManagement DROP CONSTRAINT FK_tblEventManagement_Lookup_Event_Sub_Category");

            //lookups
            this.ExtractLookupTableLookup("tblEventManagement", "Event_Category", "EventCategories", 255,
                "Event_Category");
            this.ExtractLookupTableLookup("tblEventManagement", "Event_Sub_Category", "EventSubCategories", 255,
                "Event_Sub_Category");
            //table
            Rename.Table(EVENTS_OLD_NAME).To(EVENTS);
            Rename.Column("Event_ID").OnTable(EVENTS).To("EventId");
            Rename.Column("OpCode").OnTable(EVENTS).To("OperatingCenterId");
            Rename.Column("Event_Category").OnTable(EVENTS).To("EventCategoryId");
            Rename.Column("Event_Sub_Category").OnTable(EVENTS).To("EventSubcategoryId");
            Rename.Column("Event_Summary").OnTable(EVENTS).To("EventSummary");
            Rename.Column("Active").OnTable(EVENTS).To("IsActive");
            Rename.Column("Root_Cause").OnTable(EVENTS).To("RootCause");
            Rename.Column("Response_Actions").OnTable(EVENTS).To("ResponseActions");
            Rename.Column("Estimated_Duration_Hours").OnTable(EVENTS).To("EstimatedDurationHours");
            Rename.Column("Number_Customers_Impacted").OnTable(EVENTS).To("NumberCustomersImpacted");
            Rename.Column("Start_Date").OnTable(EVENTS).To("StartDate");
            Rename.Column("End_Date").OnTable(EVENTS).To("EndDate");
            //Notes Docs
            Execute.Sql(
                $"UPDATE DataType SET Data_Type = 'Event', Table_Name = '{EVENTS}' WHERE Table_Name = '{EVENTS_OLD_NAME}';");
        }

        public override void Down()
        {
            Rename.Table(EVENTS).To(EVENTS_OLD_NAME);
            Rename.Column("EventId").OnTable(EVENTS_OLD_NAME).To("Event_ID");
            Rename.Column("OperatingCenterId").OnTable(EVENTS_OLD_NAME).To("OpCode");
            Rename.Column("EventCategoryId").OnTable(EVENTS_OLD_NAME).To("Event_Category");
            Rename.Column("EventSubcategoryId").OnTable(EVENTS_OLD_NAME).To("Event_Sub_Category");
            Rename.Column("EventSummary").OnTable(EVENTS_OLD_NAME).To("Event_Summary");
            Rename.Column("IsActive").OnTable(EVENTS_OLD_NAME).To("Active");
            Rename.Column("RootCause").OnTable(EVENTS_OLD_NAME).To("Root_Cause");
            Rename.Column("ResponseActions").OnTable(EVENTS_OLD_NAME).To("Response_Actions");
            Rename.Column("EstimatedDurationHours").OnTable(EVENTS_OLD_NAME).To("Estimated_Duration_Hours");
            Rename.Column("NumberCustomersImpacted").OnTable(EVENTS_OLD_NAME).To("Number_Customers_Impacted");
            Rename.Column("StartDate").OnTable(EVENTS_OLD_NAME).To("Start_Date");
            Rename.Column("EndDate").OnTable(EVENTS_OLD_NAME).To("End_Date");
            //lookups
            this.ReplaceLookupTableLookup(EVENTS_OLD_NAME, "Event_Category", "EventCategories", 255, "Event_Category");
            this.ReplaceLookupTableLookup(EVENTS_OLD_NAME, "Event_Sub_Category", "EventSubcategories", 255,
                "Event_Sub_Category");
            //notes docs
            Execute.Sql(
                $"UPDATE DataType SET Data_Type = 'Event Management', Table_Name = '{EVENTS_OLD_NAME}' WHERE Table_Name = '{EVENTS}'; ");
        }
    }
}
