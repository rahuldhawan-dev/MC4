using System;
using FluentMigrator;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140304145636), Tags("Production")]
    public class CreateTablesForBug1774 : Migration
    {
        public struct TableNames
        {
            public const string PROJECT_TYPES = "EstimatingProjectTypes",
                                ESTIMATING_PROJECTS = "EstimatingProjects";
        }

        public struct ColumnNames
        {
            public struct EstimatingProjects
            {
                public const string PROJECT_NUMBER = "ProjectNumber",
                                    PROJECT_NAME = "ProjectName",
                                    PROJECT_TYPE_ID = "ProjectTypeId",
                                    STREET = "Street",
                                    MUNICIPALITY_ID = "MunicipalityId",
                                    OPERATING_CENTER_ID = "OperatingCenterId",
                                    DESCRIPTION = "Description",
                                    ESTIMATOR_ID = "EstimatorId",
                                    ESTIMATE_DATE = "EstimateDate",
                                    REMARKS = "Remarks",
                                    OVERHEAD_PERCENTAGE = "OverheadPercentage",
                                    CONTINGENCY_PERCENTAGE = "ContingencyPercentage",
                                    LUMP_SUM = "LumpSum";
            }

            public struct Common
            {
                public const string ID = "Id", DESCRIPTION = "Description";
            }
        }

        public struct StringLengths
        {
            public struct EstimatingProjects
            {
                public const int PROJECT_NAME = 50,
                                 PROJECT_NUMBER = 30,
                                 STREET = 30,
                                 DESCRIPTION = 75;
            }

            public struct ProjectTypes
            {
                public const int DESCRIPTION = 25;
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.PROJECT_TYPES)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.Common.DESCRIPTION).AsAnsiString(StringLengths.ProjectTypes.DESCRIPTION)
                  .NotNullable().Unique();

            foreach (var projectType in new[] {"Framework", "Non-Framework"})
            {
                Execute.Sql("INSERT INTO {0} ({1}) SELECT '{2}';", TableNames.PROJECT_TYPES,
                    ColumnNames.Common.DESCRIPTION, projectType);
            }

            Create.Table(TableNames.ESTIMATING_PROJECTS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.EstimatingProjects.PROJECT_NUMBER)
                  .AsAnsiString(StringLengths.EstimatingProjects.PROJECT_NUMBER).NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.PROJECT_NAME)
                  .AsAnsiString(StringLengths.EstimatingProjects.PROJECT_NAME).NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.PROJECT_TYPE_ID).AsInt32().ForeignKey(
                       String.Format("FK_{0}_{1}_{2}", TableNames.ESTIMATING_PROJECTS, TableNames.PROJECT_TYPES,
                           ColumnNames.EstimatingProjects.PROJECT_TYPE_ID), TableNames.PROJECT_TYPES,
                       ColumnNames.Common.ID).NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.STREET)
                  .AsAnsiString(StringLengths.EstimatingProjects.STREET).NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.MUNICIPALITY_ID).AsInt32()
                  .ForeignKey(
                       String.Format("FK_{0}_Towns_{1}", TableNames.ESTIMATING_PROJECTS,
                           ColumnNames.EstimatingProjects.MUNICIPALITY_ID), "Towns", "TownId").NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.OPERATING_CENTER_ID).AsInt32().ForeignKey(
                       String.Format("FK_{0}_OperatingCenters_{1}", TableNames.ESTIMATING_PROJECTS,
                           ColumnNames.EstimatingProjects.OPERATING_CENTER_ID), "OperatingCenters", "OperatingCenterId")
                  .NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.DESCRIPTION)
                  .AsAnsiString(StringLengths.EstimatingProjects.DESCRIPTION).Nullable()
                  .WithColumn(ColumnNames.EstimatingProjects.ESTIMATOR_ID).AsInt32().ForeignKey(
                       String.Format("FK_{0}_tblEmployee_{1}", TableNames.ESTIMATING_PROJECTS,
                           ColumnNames.EstimatingProjects.ESTIMATOR_ID), "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.ESTIMATE_DATE).AsDateTime().NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.REMARKS).AsCustom("text").Nullable()
                  .WithColumn(ColumnNames.EstimatingProjects.OVERHEAD_PERCENTAGE).AsInt16().NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.CONTINGENCY_PERCENTAGE).AsInt16().NotNullable()
                  .WithColumn(ColumnNames.EstimatingProjects.LUMP_SUM).AsCurrency().NotNullable();

            Execute.Sql(
                "declare @dataTypeId int; INSERT INTO [DataType] (Data_Type, Table_Name) VALUES ('{0}', '{0}'); SELECT @dataTypeId = @@IDENTITY; INSERT INTO [DocumentType] (Document_Type, DataTypeId) VALUES ('Estimating Project Document', @dataTypeId);",
                TableNames.ESTIMATING_PROJECTS);
        }

        public override void Down()
        {
            this.DeleteDataType(TableNames.ESTIMATING_PROJECTS);

            Delete.Table(TableNames.ESTIMATING_PROJECTS);

            Delete.Table(TableNames.PROJECT_TYPES);
        }
    }
}
