using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131211105813), Tags("Production")]
    public class MoveGrievanceCategorizationsAndStatusesToTheirOwnTable : Migration
    {
        public struct TableNames
        {
            public const string GRIEVANCE_CATEGORIZATIONS = "GrievanceCategorizations",
                                GRIEVANCE_STATUSES = "GrievanceStatuses";
        }

        public struct ColumnNames
        {
            public const string ID = "Id", DESCRIPTION = "Description";
        }

        public const int DESCRIPTION_LENGTH = 255;

        public override void Up()
        {
            Create.Table(TableNames.GRIEVANCE_CATEGORIZATIONS)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.DESCRIPTION).AsAnsiString(DESCRIPTION_LENGTH).NotNullable();

            Create.Table(TableNames.GRIEVANCE_STATUSES)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.DESCRIPTION).AsAnsiString(DESCRIPTION_LENGTH).NotNullable();

            Execute.Sql(
                "INSERT INTO GrievanceCategorizations (Description) SELECT LookupValue FROM Lookup WHERE LookupType = 'Grievance_Categorization';");
            Execute.Sql(
                "INSERT INTO GrievanceStatuses (Description) SELECT LookupValue FROM Lookup WHERE LookupType = 'Grievance_Status';");

            Execute.Sql(
                "UPDATE tblBargaining_Unit_Grievances SET [Grievance Categorization] = gc.Id FROM GrievanceCategorizations gc INNER JOIN Lookup l ON gc.Description = l.LookupValue WHERE l.LookupId = [Grievance Categorization] AND l.LookupType = 'Grievance_Categorization';");
            Execute.Sql(
                "UPDATE tblBargaining_Unit_Grievances SET [Grievance Status] = gs.Id FROM GrievanceStatuses gs INNER JOIN Lookup l ON gs.Description = l.LookupValue WHERE l.LookupId = [Grievance Status] AND l.LookupType = 'Grievance_Status';");

            Execute.Sql("DELETE from Lookup WHERE LookupType IN ('Grievance_Categorization', 'Grievance_Status');");
        }

        public override void Down()
        {
            Execute.Sql(
                "INSERT INTO Lookup (LookupType, LookupValue, TableName) SELECT 'Grievance_Categorization', [Description], 'tblBargaining_Unit_Grievances' FROM GrievanceCategorizations;");
            Execute.Sql(
                "INSERT INTO Lookup (LookupType, LookupValue, TableName) SELECT 'Grievance_Status', [Description], 'tblBargaining_Unit_Grievances' FROM GrievanceStatuses;");

            Execute.Sql(
                "UPDATE tblBargaining_Unit_Grievances SET [Grievance Categorization] = l.LookupId FROM GrievanceCategorizations gc INNER JOIN Lookup l ON gc.Description = l.LookupValue WHERE gc.Id = [Grievance Categorization] AND l.LookupType = 'Grievance_Categorization';");
            Execute.Sql(
                "UPDATE tblBargaining_Unit_Grievances SET [Grievance Status] = l.LookupId FROM GrievanceStatuses gs INNER JOIN Lookup l ON gs.Description = l.LookupValue WHERE gs.Id = [Grievance Status] AND l.LookupType = 'Grievance_Categorization';");

            Delete.Table(TableNames.GRIEVANCE_CATEGORIZATIONS);
            Delete.Table(TableNames.GRIEVANCE_STATUSES);
        }
    }
}
