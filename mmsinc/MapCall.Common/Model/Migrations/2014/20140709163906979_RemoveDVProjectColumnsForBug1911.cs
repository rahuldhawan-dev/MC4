using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140709163906979), Tags("Production")]
    public class RemoveDVProjectColumnsForBug1911 : Migration
    {
        public struct TableNames
        {
            public const string PROJECTS_DV = "projectsDV",
                                CONTRACTORS = "tblContractors",
                                LOOKUP = "Lookup",
                                EMPLOYEES = "tblEmployee",
                                FACILITIES = "tblFacilities";
        }

        public struct ColumnNames
        {
            public const string APPROVAL_STATUS = "ApprovalStatus", // int FK
                                FINAL_PROJECT_COST = "FinalProjectCost", //decimal 18,10
                                ASSET_OWNER = "AssetOwner", // int FK
                                CONSTRUCTION_MANAGER = "ConstructionManager", // int  FK
                                COMPANY_INSPECTOR = "CompanyInspector", // int FK
                                CONTRACTED_INSPECTOR = "ContractedInspector", //varchar(30)
                                ENGINEERING_CONTRACTOR = "EngineeringContractor", // int FK
                                CONSTRUCTION_CONTRACTOR = "ConstructionContractor", // int FK
                                FACILITY_ID = "FacilityID", // int FK
                                FACILITY_ID_ID = "RecordId",
                                CIM_DATE = "CIMDate", // smalldatetime
                                PROJECT_FLAGGED = "ProjectFlagged", //bit
                                CURRENT_YEAR_ACTIVE = "CurrentYearActive", //bit
                                BULK_SALE = "BulkSale", // bit
                                RATE_CASE = "RateCase", // bit
                                GEOGRAPHY = "Geography", // bit
                                CONTROL_DATE = "ControlDate", //smalldatetime
                                PP_DATE = "PPDate", //smalldatetime
                                PP_SCORE = "PPScore", //smallint
                                LOOKUP_ID = "LookupID",
                                CONTRACTOR_ID = "ContractorID",
                                EMPLOYEE_ID = "tblEmployeeID";
        }

        public struct ForeignKeys
        {
            public const string
                FK_PROJECTSDV_LOOKUP_APPROVALSTATUS = "FK_ProjectsDV_Lookup_ApprovalStatus",
                FK_PROJECTSDV_TBLEMPLOYEE_ASSETOWNER = "FK_ProjectsDV_tblEmployee_AssetOwner",
                FK_PROJECTSDV_TBLEMPLOYEE_CONSTRUCTIONMANAGER = "FK_ProjectsDV_tblEmployee_ConstructionManager",
                FK_PROJECTSDV_TBLEMPLOYEE_COMPANYINSPECTOR = "FK_ProjectsDV_tblEmployee_CompanyInspector",
                FK_PROJECTSDV_TBLCONTRACTORS_ENGINEERINGCONTRACTOR =
                    "FK_ProjectsDV_tblContractors_EngineeringContractor",
                FK_PROJECTSDV_TBLCONTRACTORS_CONSTRUCTIONCONTRACTOR =
                    "FK_ProjectsDV_tblContractors_ConstructionContractor",
                FK_PROJECTSDV_TBLFACILITIES_FACILITYID = "FK_ProjectsDV_tblFacilities_FacilityID";
        }

        public override void Up()
        {
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_LOOKUP_APPROVALSTATUS).OnTable(TableNames.PROJECTS_DV);
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLCONTRACTORS_CONSTRUCTIONCONTRACTOR)
                  .OnTable(TableNames.PROJECTS_DV);
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLCONTRACTORS_ENGINEERINGCONTRACTOR)
                  .OnTable(TableNames.PROJECTS_DV);
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLEMPLOYEE_ASSETOWNER).OnTable(TableNames.PROJECTS_DV);
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLEMPLOYEE_COMPANYINSPECTOR).OnTable(TableNames.PROJECTS_DV);
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLEMPLOYEE_CONSTRUCTIONMANAGER)
                  .OnTable(TableNames.PROJECTS_DV);
            Delete.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLFACILITIES_FACILITYID).OnTable(TableNames.PROJECTS_DV);

            Delete.Column(ColumnNames.APPROVAL_STATUS).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.CONSTRUCTION_CONTRACTOR).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.ENGINEERING_CONTRACTOR).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.ASSET_OWNER).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.COMPANY_INSPECTOR).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.CONSTRUCTION_MANAGER).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.FACILITY_ID).FromTable(TableNames.PROJECTS_DV);

            Delete.Column(ColumnNames.FINAL_PROJECT_COST).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.CONTRACTED_INSPECTOR).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.CIM_DATE).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.PROJECT_FLAGGED).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.CURRENT_YEAR_ACTIVE).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.BULK_SALE).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.RATE_CASE).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.GEOGRAPHY).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.CONTROL_DATE).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.PP_DATE).FromTable(TableNames.PROJECTS_DV);
            Delete.Column(ColumnNames.PP_SCORE).FromTable(TableNames.PROJECTS_DV);
        }

        public override void Down()
        {
            Create.Column(ColumnNames.PP_SCORE).OnTable(TableNames.PROJECTS_DV).AsInt16().Nullable();
            Create.Column(ColumnNames.PP_DATE).OnTable(TableNames.PROJECTS_DV).AsDate().Nullable();
            Create.Column(ColumnNames.CONTROL_DATE).OnTable(TableNames.PROJECTS_DV).AsDate().Nullable();
            Create.Column(ColumnNames.GEOGRAPHY).OnTable(TableNames.PROJECTS_DV).AsBoolean().Nullable();
            Create.Column(ColumnNames.RATE_CASE).OnTable(TableNames.PROJECTS_DV).AsBoolean().Nullable();
            Create.Column(ColumnNames.BULK_SALE).OnTable(TableNames.PROJECTS_DV).AsBoolean().Nullable();
            Create.Column(ColumnNames.CURRENT_YEAR_ACTIVE).OnTable(TableNames.PROJECTS_DV).AsBoolean().Nullable();
            Create.Column(ColumnNames.PROJECT_FLAGGED).OnTable(TableNames.PROJECTS_DV).AsBoolean().Nullable();
            Create.Column(ColumnNames.CIM_DATE).OnTable(TableNames.PROJECTS_DV).AsDate().Nullable();
            Create.Column(ColumnNames.FACILITY_ID).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.CONSTRUCTION_CONTRACTOR).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.ENGINEERING_CONTRACTOR).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.CONTRACTED_INSPECTOR).OnTable(TableNames.PROJECTS_DV).AsAnsiString(30).Nullable();
            Create.Column(ColumnNames.COMPANY_INSPECTOR).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.CONSTRUCTION_MANAGER).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.ASSET_OWNER).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.FINAL_PROJECT_COST).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();
            Create.Column(ColumnNames.APPROVAL_STATUS).OnTable(TableNames.PROJECTS_DV).AsInt32().Nullable();

            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_LOOKUP_APPROVALSTATUS)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.APPROVAL_STATUS)
                  .ToTable(TableNames.LOOKUP)
                  .PrimaryColumn(ColumnNames.LOOKUP_ID);
            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLCONTRACTORS_CONSTRUCTIONCONTRACTOR)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.CONSTRUCTION_CONTRACTOR)
                  .ToTable(TableNames.CONTRACTORS)
                  .PrimaryColumn(ColumnNames.CONTRACTOR_ID);
            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLCONTRACTORS_ENGINEERINGCONTRACTOR)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.ENGINEERING_CONTRACTOR)
                  .ToTable(TableNames.CONTRACTORS)
                  .PrimaryColumn(ColumnNames.CONTRACTOR_ID);
            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLEMPLOYEE_ASSETOWNER)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.ASSET_OWNER)
                  .ToTable(TableNames.EMPLOYEES)
                  .PrimaryColumn(ColumnNames.EMPLOYEE_ID);
            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLEMPLOYEE_COMPANYINSPECTOR)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.COMPANY_INSPECTOR)
                  .ToTable(TableNames.EMPLOYEES)
                  .PrimaryColumn(ColumnNames.EMPLOYEE_ID);
            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLEMPLOYEE_CONSTRUCTIONMANAGER)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.CONSTRUCTION_MANAGER)
                  .ToTable(TableNames.EMPLOYEES)
                  .PrimaryColumn(ColumnNames.EMPLOYEE_ID);
            Create.ForeignKey(ForeignKeys.FK_PROJECTSDV_TBLFACILITIES_FACILITYID)
                  .FromTable(TableNames.PROJECTS_DV)
                  .ForeignColumn(ColumnNames.FACILITY_ID)
                  .ToTable(TableNames.FACILITIES)
                  .PrimaryColumn(ColumnNames.FACILITY_ID_ID);
        }
    }
}
