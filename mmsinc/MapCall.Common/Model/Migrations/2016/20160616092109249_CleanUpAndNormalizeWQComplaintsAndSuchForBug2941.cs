using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160616092109249), Tags("Production")]
    public class CleanUpAndNormalizeWQComplaintsAndSuchForBug2941 : Migration
    {
        public const string TABLE_NAME = "WaterQualityComplaints";

        public override void Up()
        {
            var oldName = "tblWQ_Complaints";

            Rename.Column("Complaint_Number").OnTable(oldName).To("Id");
            Rename.Column("ORCOM_OrderNumber").OnTable(oldName).To("ORCOMOrderNumber");

            this.ExtractLookupTableLookup(oldName, "ORCOM_OrderType", "ORCOMOrderTypes", 20,
                "ORCOM_OrderType", deleteOldForeignKey: true);
            Rename.Column("ORCOM_OrderType").OnTable(oldName).To("ORCOMOrderTypeId");

            this.ExtractLookupTableLookup(oldName, "WQ_Complaint_Type", "WaterQualityComplaintTypes", 50,
                "WQ_Complaint_Type", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("WQ_Complaint_Type").OnTable(oldName).To("ComplaintTypeId");

            Rename.Column("Date_Complaint_Received").OnTable(oldName).To("DateComplaintReceived");

            this.ExtractLookupTableLookup(oldName, "InitialLocalResponseType",
                "WaterQualityComplaintLocalResponseTypes",
                30, "InitialLocalResponseType", deleteOldForeignKey: true);
            Rename.Column("InitialLocalResponseType").OnTable(oldName).To("InitialLocalResponseTypeId");

            Rename.Column("Entered_By").OnTable(oldName).To("EnteredBy");
            Rename.Column("Complaint_Close_Date").OnTable(oldName).To("ComplaintCloseDate");
            Rename.Column("Closed_By").OnTable(oldName).To("ClosedBy");
            Rename.Column("Customer_Name").OnTable(oldName).To("CustomerName");
            Rename.Column("Home_Phone_Number").OnTable(oldName).To("HomePhoneNumber");

            Delete.Column("Work_Phone_Number").FromTable(oldName);

            Rename.Column("Street_Number").OnTable(oldName).To("StreetNumber");
            Rename.Column("Street_Name").OnTable(oldName).To("StreetName");
            Rename.Column("Apartment_Number").OnTable(oldName).To("ApartmentNumber");
            Rename.Column("Town").OnTable(oldName).To("TownId");
            Rename.Column("Town_Section").OnTable(oldName).To("TownSectionId");

            Rename.Column("State").OnTable(oldName).To("StateId");
            Execute.Sql($"UPDATE {oldName} SET StateId = (select stateid from states where abbreviation = 'NJ');");
            Alter.Column("StateId").OnTable(oldName).AsForeignKey("StateId", "States", "StateId");

            Rename.Column("Zip_Code").OnTable(oldName).To("ZipCode");
            Rename.Column("Premise_Number").OnTable(oldName).To("PremiseNumber");
            Rename.Column("Service_Number").OnTable(oldName).To("ServiceNumber");
            Rename.Column("Account_Number").OnTable(oldName).To("AccountNumber");
            Rename.Column("Complaint_Description").OnTable(oldName).To("ComplaintDescription");
            Rename.Column("Complaint_Start_Date").OnTable(oldName).To("ComplaintStartDate");

            this.ExtractLookupTableLookup(oldName, "WQ_Complaint_Problem_Area", "WaterQualityComplaintProblemAreas", 30,
                "WQ_Complaint_Problem_Area", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("WQ_Complaint_Problem_Area").OnTable(oldName).To("ProblemAreaId");

            this.ExtractLookupTableLookup(oldName, "WQ_Complaint_Source", "WaterQualityComplaintSources", 30,
                "WQ_Complaint_Source", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("WQ_Complaint_Source").OnTable(oldName).To("ComplaintSourceId");

            Rename.Column("Site_Visit_Required").OnTable(oldName).To("SiteVisitRequired");
            Rename.Column("Site_Visit_By").OnTable(oldName).To("SiteVisitBy");
            Rename.Column("Site_Comments").OnTable(oldName).To("SiteComments");
            Rename.Column("Water_Filter_On_Complaint_Source").OnTable(oldName).To("WaterFilterOnComplaintSource");
            Rename.Column("Cross_Connection_Detected").OnTable(oldName).To("CrossConnectionDetected");

            Delete.Column("Nearest_Hydrant").FromTable(oldName);
            Delete.Column("Material_Of_Main").FromTable(oldName);

            Execute.Sql(
                "INSERT INTO MainSizes (Size) SELECT convert(decimal(8,2), LookupValue) FROM Lookup WHERE LookupType = 'Size_Of_Main' AND convert(decimal(8, 2), LookupValue) NOT IN (SELECT Size FROM MainSizes)");
            Delete.ForeignKey("FK_tblWQ_Complaints_Lookup_Size_Of_Main").OnTable(oldName);
            Execute.Sql(
                $"UPDATE {oldName} SET Size_Of_Main = s.MainSizeId FROM MainSizes s INNER JOIN Lookup l ON l.LookupType = 'Size_Of_Main' AND convert(decimal(8, 2), l.LookupValue) = s.Size WHERE l.LookupId = {oldName}.Size_Of_Main");
            Execute.Sql("DELETE FROM Lookup where LookupType = 'Size_Of_Main'");
            Alter.Column("Size_Of_Main").OnTable(oldName).AsInt32()
                 .ForeignKey($"FK_{TABLE_NAME}_MainSizes_MainSizeId", "MainSizes", "MainSizeId").Nullable();
            Rename.Column("Size_Of_Main").OnTable(oldName).To("MainSizeId");

            Delete.Column("Service_Year_Installed").FromTable(oldName);

            this.ExtractLookupTableLookup(oldName, "WQ_Complaint_Probable_Cause", "WaterQualityComplaintProbableCauses",
                30,
                "WQ_Complaint_Probable_Cause", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("WQ_Complaint_Probable_Cause").OnTable(oldName).To("ProbableCauseId");

            this.ExtractLookupTableLookup(oldName, "WQ_Complaint_Action_Taken",
                "WaterQualityComplaintActionsWhichCanBeTaken", 40,
                "WQ_Complaint_Action_Taken", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("WQ_Complaint_Action_Taken").OnTable(oldName).To("ActionTakenId");

            Rename.Column("Customer_Anticipated_Followup_Date").OnTable(oldName).To("CustomerAnticipatedFollowupDate");
            Rename.Column("Actual_Customer_Followup_Date").OnTable(oldName).To("ActualCustomerFollowupDate");

            this.ExtractLookupTableLookup(oldName, "Customer_Expectation", "WaterQualityComplaintCustomerExpectations",
                20,
                "Customer_Expectation", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("Customer_Expectation").OnTable(oldName).To("CustomerExpectationId");

            this.ExtractLookupTableLookup(oldName, "Customer_Satisfaction",
                "WaterQualityComplaintCustomerSatisfactions", 25,
                "Customer_Satisfaction", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("Customer_Satisfaction").OnTable(oldName).To("CustomerSatisfactionId");

            Rename.Column("Customer_Satisfaction_Followup_Letter").OnTable(oldName)
                  .To("CustomerSatisfactionFollowupLetter");
            Rename.Column("Customer_Satisfaction_Followup_Call").OnTable(oldName)
                  .To("CustomerSatisfactionFollowupCall");
            Rename.Column("Customer_Satisfaction_Followup_Comments").OnTable(oldName)
                  .To("CustomerSatisfactionFollowupComments");
            Rename.Column("Root_Cause_Identified").OnTable(oldName).To("RootCauseIdentified");

            this.ExtractLookupTableLookup(oldName, "Root_Cause", "WaterQualityComplaintRootCauses", 20,
                "Root_Cause", deleteOldForeignKey: true, lookupIsTableSpecific: false);
            Rename.Column("Root_Cause").OnTable(oldName).To("RootCauseId");

            Rename.Table(oldName).To(TABLE_NAME);

            Execute.Sql($"UPDATE DataType SET Table_Name = '{TABLE_NAME}' WHERE Table_Name = '{oldName}'");
        }

        public override void Down()
        {
            var oldName = "tblWQ_Complaints";

            Rename.Table(TABLE_NAME).To(oldName);

            Rename.Column("Id").OnTable(oldName).To("Complaint_Number");
            Rename.Column("ORCOMOrderNumber").OnTable(oldName).To("ORCOM_OrderNumber");

            Rename.Column("ORCOMOrderTypeId").OnTable(oldName).To("ORCOM_OrderType");
            this.ReplaceLookupTableLookup(oldName, "ORCOM_OrderType", "ORCOMOrderTypes", 20,
                "ORCOM_OrderType");

            Rename.Column("ComplaintTypeId").OnTable(oldName).To("WQ_Complaint_Type");
            this.ReplaceLookupTableLookup(oldName, "WQ_Complaint_Type", "WaterQualityComplaintTypes", 50,
                "WQ_Complaint_Type", lookupIsTableSpecific: false);

            Rename.Column("DateComplaintReceived").OnTable(oldName).To("Date_Complaint_Received");

            Rename.Column("InitialLocalResponseTypeId").OnTable(oldName).To("InitialLocalResponseType");
            this.ReplaceLookupTableLookup(oldName, "InitialLocalResponseType",
                "WaterQualityComplaintLocalResponseTypes",
                30, "InitialLocalResponseType");

            Rename.Column("EnteredBy").OnTable(oldName).To("Entered_By");
            Rename.Column("ComplaintCloseDate").OnTable(oldName).To("Complaint_Close_Date");
            Rename.Column("ClosedBy").OnTable(oldName).To("Closed_By");
            Rename.Column("CustomerName").OnTable(oldName).To("Customer_Name");
            Rename.Column("HomePhoneNumber").OnTable(oldName).To("Home_Phone_Number");

            Create.Column("Work_Phone_Number").OnTable(oldName).AsString(15).Nullable();

            Rename.Column("StreetNumber").OnTable(oldName).To("Street_Number");
            Rename.Column("StreetName").OnTable(oldName).To("Street_Name");
            Rename.Column("ApartmentNumber").OnTable(oldName).To("Apartment_Number");
            Rename.Column("TownId").OnTable(oldName).To("Town");
            Rename.Column("TownSectionId").OnTable(oldName).To("Town_Section");

            Rename.Column("StateId").OnTable(oldName).To("State");
            Alter.Column("State").OnTable(oldName).AsString(4);

            Rename.Column("ZipCode").OnTable(oldName).To("Zip_Code");
            Rename.Column("PremiseNumber").OnTable(oldName).To("Premise_Number");
            Rename.Column("ServiceNumber").OnTable(oldName).To("Service_Number");
            Rename.Column("AccountNumber").OnTable(oldName).To("Account_Number");
            Rename.Column("ComplaintDescription").OnTable(oldName).To("Complaint_Description");
            Rename.Column("ComplaintStartDate").OnTable(oldName).To("Complaint_Start_Date");

            Rename.Column("ProblemAreaId").OnTable(oldName).To("WQ_Complaint_Problem_Area");
            this.ReplaceLookupTableLookup(oldName, "WQ_Complaint_Problem_Area", "WaterQualityComplaintProblemAreas", 30,
                "WQ_Complaint_Problem_Area", lookupIsTableSpecific: false);

            Rename.Column("ComplaintSourceId").OnTable(oldName).To("WQ_Complaint_Source");
            this.ReplaceLookupTableLookup(oldName, "WQ_Complaint_Source", "WaterQualityComplaintSources", 30,
                "WQ_Complaint_Source", lookupIsTableSpecific: false);

            Rename.Column("SiteVisitRequired").OnTable(oldName).To("Site_Visit_Required");
            Rename.Column("SiteVisitBy").OnTable(oldName).To("Site_Visit_By");
            Rename.Column("SiteComments").OnTable(oldName).To("Site_Comments");
            Rename.Column("WaterFilterOnComplaintSource").OnTable(oldName).To("Water_Filter_On_Complaint_Source");
            Rename.Column("CrossConnectionDetected").OnTable(oldName).To("Cross_Connection_Detected");

            Create.Column("Nearest_Hydrant").OnTable(oldName).AsString(15).Nullable();
            Create.Column("Material_Of_Main").OnTable(oldName).AsString(20).Nullable();

            Delete.ForeignKey($"FK_{TABLE_NAME}_MainSizes_MainSizeId").OnTable(oldName);
            Rename.Column("MainSizeId").OnTable(oldName).To("Size_Of_Main");
            Execute.Sql("INSERT INTO Lookup (LookupType, LookupValue) SELECT 'Size_Of_Main', Size FROM MainSizes");
            Execute.Sql(
                $"UPDATE {oldName} SET Size_Of_Main = l.LookupId FROM Lookup l INNER JOIN MainSizes s ON l.LookupType = 'Size_Of_Main' AND convert(decimal(8, 2), l.LookupValue) = s.Size WHERE s.MainSizeId = {oldName}.Size_Of_Main");
            Alter.Column("Size_Of_Main").OnTable(oldName).AsInt32()
                 .ForeignKey("FK_tblWQ_Complaints_Lookup_Size_Of_Main", "Lookup", "LookupId").Nullable();

            Create.Column("Service_Year_Installed").OnTable(oldName).AsFloat().Nullable();

            Rename.Column("ProbableCauseId").OnTable(oldName).To("WQ_Complaint_Probable_Cause");
            this.ReplaceLookupTableLookup(oldName, "WQ_Complaint_Probable_Cause", "WaterQualityComplaintProbableCauses",
                30,
                "WQ_Complaint_Probable_Cause", lookupIsTableSpecific: false);

            Rename.Column("ActionTakenId").OnTable(oldName).To("WQ_Complaint_Action_Taken");
            this.ReplaceLookupTableLookup(oldName, "WQ_Complaint_Action_Taken",
                "WaterQualityComplaintActionsWhichCanBeTaken", 40,
                "WQ_Complaint_Action_Taken", lookupIsTableSpecific: false);

            Rename.Column("CustomerAnticipatedFollowupDate").OnTable(oldName).To("Customer_Anticipated_Followup_Date");
            Rename.Column("ActualCustomerFollowupDate").OnTable(oldName).To("Actual_Customer_Followup_Date");

            Rename.Column("CustomerExpectationId").OnTable(oldName).To("Customer_Expectation");
            this.ReplaceLookupTableLookup(oldName, "Customer_Expectation", "WaterQualityComplaintCustomerExpectations",
                20,
                "Customer_Expectation", lookupIsTableSpecific: false);

            Rename.Column("CustomerSatisfactionId").OnTable(oldName).To("Customer_Satisfaction");
            this.ReplaceLookupTableLookup(oldName, "Customer_Satisfaction",
                "WaterQualityComplaintCustomerSatisfactions", 25,
                "Customer_Satisfaction", lookupIsTableSpecific: false);

            Rename.Column("CustomerSatisfactionFollowupLetter").OnTable(oldName)
                  .To("Customer_Satisfaction_Followup_Letter");
            Rename.Column("CustomerSatisfactionFollowupCall").OnTable(oldName)
                  .To("Customer_Satisfaction_Followup_Call");
            Rename.Column("CustomerSatisfactionFollowupComments").OnTable(oldName)
                  .To("Customer_Satisfaction_Followup_Comments");
            Rename.Column("RootCauseIdentified").OnTable(oldName).To("Root_Cause_Identified");

            Rename.Column("RootCauseId").OnTable(oldName).To("Root_Cause");
            this.ReplaceLookupTableLookup(oldName, "Root_Cause", "WaterQualityComplaintRootCauses", 20,
                "Root_Cause", lookupIsTableSpecific: false);

            Execute.Sql($"UPDATE DataType SET Table_Name = '{oldName}' WHERE Table_Name = '{TABLE_NAME}'");
        }
    }
}
