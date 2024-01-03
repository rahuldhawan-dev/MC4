using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140912163144103), Tags("Production")]
    public class NormalizeCreatedByForBug2092 : Migration
    {
        public const string TABLE_NAME = "tblJobObservations";

        public struct ColumnNames
        {
            public const string CREATED_BY = "CreatedBy", CREATED_BY_ID = "CreatedById";
        }

        public struct Sql
        {
            public const string UPDATE_CREATED_BY_IDS =
                                    "UPDATE tblJobObservations set CreatedById = (select recID from tblPermissions where username = CreatedBy)",
                                UPDATE_CREATED_BY =
                                    "UPDATE tblJobObservations set CreatedBy = (select username from tblPermissions where recId = CreatedById)";
        }

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn(ColumnNames.CREATED_BY_ID).AsInt32().Nullable()
                 .ForeignKey(String.Format("FK_{0}_{1}_{2}", TABLE_NAME, "tblPermissions", "RecId"), "tblPermissions",
                      "RecId");
            Execute.Sql(Sql.UPDATE_CREATED_BY_IDS);
            Delete.Column(ColumnNames.CREATED_BY).FromTable(TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKey(String.Format("FK_{0}_{1}_{2}", TABLE_NAME, "tblPermissions", "RecId"))
                  .OnTable(TABLE_NAME);
            Alter.Table(TABLE_NAME).AddColumn(ColumnNames.CREATED_BY).AsAnsiString(50).Nullable();
            Execute.Sql(Sql.UPDATE_CREATED_BY);
            Delete.Column(ColumnNames.CREATED_BY_ID).FromTable(TABLE_NAME);
        }
    }
}
