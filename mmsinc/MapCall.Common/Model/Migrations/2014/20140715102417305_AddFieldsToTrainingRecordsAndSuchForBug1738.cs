using System.Text.RegularExpressions;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140715102417305), Tags("Production")]
    public class AddFieldsToTrainingRecordsAndSuchForBug1738 : Migration
    {
        public const string DELIMITER = "|";

        public const string ALTER_VIEW = @"ALTER VIEW [" + ViewNames.EMPLOYEE_LINK_VIEW + @"] AS
SELECT
    link.EmployeeLinkId as Id,
    link.DataLinkId as LinkedId,
    dat.DataTypeId,
    dat.Data_Type as DataTypeName,
    dat.Table_Name as TableName,
    dat.Data_Type + '" + DELIMITER + @"' + dat.Table_Name as DataTypeAndTableName,
    link.CreatedOn as LinkedOn,
    link.CreatedBy as LinkedBy,
    link.tblEmployeeId as EmployeeId
FROM
    EmployeeLink link
INNER JOIN
    DataType dat
ON
    link.DataTypeId = dat.DataTypeId;";

        public struct ViewNames
        {
            public const string EMPLOYEE_LINK_VIEW = CreateEmployeeLinkView.VIEW_NAME;
        }

        public struct TableNames
        {
            public const string TRAINING_RECORDS = "tblTrainingRecords";
        }

        public struct ColumnNames
        {
            public const string SCHEDULED_DATE = "ScheduledDate",
                                MAXIMUM_CLASS_SIZE = "MaximumClassSize";
        }

        public override void Up()
        {
            Alter.Table(TableNames.TRAINING_RECORDS).AddColumn(ColumnNames.SCHEDULED_DATE).AsDateTime().Nullable();
            Alter.Table(TableNames.TRAINING_RECORDS).AddColumn(ColumnNames.MAXIMUM_CLASS_SIZE).AsInt32().Nullable();
            Execute.Sql(ALTER_VIEW);

            Execute.Sql(
                "UPDATE DataType SET Data_Type = 'Employees Attended' WHERE Table_Name = 'tblTrainingRecords' AND Data_Type = 'Training Record';");
            Execute.Sql(
                "INSERT INTO DataType (Data_Type, Table_Name) VALUES ('Employees Scheduled', 'tblTrainingRecords');");
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.MAXIMUM_CLASS_SIZE).FromTable(TableNames.TRAINING_RECORDS);
            Delete.Column(ColumnNames.SCHEDULED_DATE).FromTable(TableNames.TRAINING_RECORDS);
            Execute.Sql(new Regex("^CREATE").Replace(CreateEmployeeLinkView.CREATE_SQL, "ALTER"));
            Execute.Sql(
                "DELETE FROM DataType WHERE Data_Type = 'Employees Scheduled' AND Table_Name = 'tblTrainingRecords';");
            Execute.Sql(
                "UPDATE DataType SET Data_Type = 'Training Record' WHERE Table_Name = 'tblTrainingRecords' AND Data_Type = 'Employees Attended';");
        }
    }
}
