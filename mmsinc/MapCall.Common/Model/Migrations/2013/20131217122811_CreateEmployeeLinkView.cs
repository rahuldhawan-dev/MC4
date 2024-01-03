using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131217122811), Tags("Production")]
    public class CreateEmployeeLinkView : Migration
    {
        public const string VIEW_NAME = "EmployeeLinkView";
        public const string DROP_SQL = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS
SELECT
    link.EmployeeLinkId as Id,
    link.DataLinkId as LinkedId,
    dat.DataTypeId,
    dat.Table_Name as TableName,
    link.CreatedOn as LinkedOn,
    link.CreatedBy as LinkedBy,
    link.tblEmployeeId as EmployeeId
FROM
    EmployeeLink link
INNER JOIN
    DataType dat
ON
    link.DataTypeId = dat.DataTypeId;";

        public override void Up()
        {
            Execute.Sql(CREATE_SQL);
        }

        public override void Down()
        {
            Execute.Sql(DROP_SQL);
        }
    }
}
