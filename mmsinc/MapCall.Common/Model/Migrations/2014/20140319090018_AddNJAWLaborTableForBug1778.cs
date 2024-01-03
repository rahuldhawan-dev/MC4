using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140319090018), Tags("Production")]
    public class AddNJAWLaborTableForBug1778 : Migration
    {
        public struct TableNames
        {
            public const string COMPANY_LABOR_COSTS = "CompanyLaborCosts",
                                ESTIMATING_PROJECTS_COMPANY_LABOR_COSTS = "EstimatingProjectsCompanyLaborCosts",
                                ESTIMATING_PROJECTS = "EstimatingProjects";
        }

        public struct ColumnNames
        {
            public const string ID = "Id",
                                DESCRIPTION = "Description",
                                UNIT = "Unit",
                                COST = "Cost",
                                LABOR_ITEM = "LaborItem",
                                QUANTITY = "Quantity",
                                ESTIMATING_PROJECT_ID = "EstimatingProjectId",
                                COMPANY_LABOR_COST_ID = "CompanyLaborCostId";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 75, UNIT = 5, LABOR_ITEM = 10;
        }

        public struct ForeignKeys
        {
            public const string
                FK_EstimatingProjectsCompanyLaborCosts_CompanyLaborCosts =
                    "FK_EstimatingProjectsCompanyLaborCosts_CompanyLaborCosts_CompanyLaborCostId",
                FK_EstimatingProjectsCompanyLaborCosts_EstimatingProjects =
                    "FK_EstimatingProjectsCompanyLaborCosts_EstimatingProjects_EstimatingProjectId";
        }

        public override void Up()
        {
            Create.Table(TableNames.COMPANY_LABOR_COSTS)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().PrimaryKey().NotNullable().Unique()
                  .WithColumn(ColumnNames.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn(ColumnNames.UNIT).AsAnsiString(StringLengths.UNIT).NotNullable()
                  .WithColumn(ColumnNames.COST).AsCurrency().NotNullable()
                  .WithColumn(ColumnNames.LABOR_ITEM).AsAnsiString(StringLengths.LABOR_ITEM).Nullable();
            Create.Table(TableNames.ESTIMATING_PROJECTS_COMPANY_LABOR_COSTS)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().PrimaryKey().NotNullable().Unique()
                  .WithColumn(ColumnNames.ESTIMATING_PROJECT_ID).AsInt32()
                  .ForeignKey(ForeignKeys.FK_EstimatingProjectsCompanyLaborCosts_EstimatingProjects,
                       TableNames.ESTIMATING_PROJECTS, ColumnNames.ID).NotNullable()
                  .WithColumn(ColumnNames.COMPANY_LABOR_COST_ID).AsInt32()
                  .ForeignKey(ForeignKeys.FK_EstimatingProjectsCompanyLaborCosts_CompanyLaborCosts,
                       TableNames.COMPANY_LABOR_COSTS, ColumnNames.ID).NotNullable()
                  .WithColumn(ColumnNames.QUANTITY).AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.ESTIMATING_PROJECTS_COMPANY_LABOR_COSTS);
            Delete.Table(TableNames.COMPANY_LABOR_COSTS);
        }
    }
}
