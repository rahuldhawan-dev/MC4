using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140307151211), Tags("Production")]
    public class CreateTablesForBug1775 : Migration
    {
        public struct TableNames
        {
            public const string ESTIMATING_PROJECTS_MATERIALS = "EstimatingProjectsMaterials";
        }

        public struct ColumnNames
        {
            public struct EstimatingProjectsMaterials
            {
                public const string ID = "Id",
                                    ESTIMATING_PROJECT_ID = "EstimatingProjectId",
                                    MATERIAL_ID = "MaterialId";
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.ESTIMATING_PROJECTS_MATERIALS)
                  .WithColumn(ColumnNames.EstimatingProjectsMaterials.ID).AsInt32().NotNullable().Identity()
                  .PrimaryKey()
                  .WithColumn(ColumnNames.EstimatingProjectsMaterials.ESTIMATING_PROJECT_ID).AsInt32().NotNullable()
                  .ForeignKey(
                       String.Format("FK_{0}_{1}_{2}", TableNames.ESTIMATING_PROJECTS_MATERIALS,
                           CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS,
                           ColumnNames.EstimatingProjectsMaterials.ESTIMATING_PROJECT_ID),
                       CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS,
                       CreateTablesForBug1774.ColumnNames.Common.ID)
                  .WithColumn(ColumnNames.EstimatingProjectsMaterials.MATERIAL_ID).AsInt32().NotNullable().ForeignKey(
                       String.Format("FK_{0}_Materials_{1}", TableNames.ESTIMATING_PROJECTS_MATERIALS,
                           ColumnNames.EstimatingProjectsMaterials.MATERIAL_ID), "Materials", "MaterialId");
        }

        public override void Down()
        {
            Delete.Table(TableNames.ESTIMATING_PROJECTS_MATERIALS);
        }
    }
}
