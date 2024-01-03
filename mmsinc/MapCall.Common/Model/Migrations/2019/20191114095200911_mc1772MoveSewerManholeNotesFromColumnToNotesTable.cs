using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191114095200911), Tags("Production")]
    public class mc1772MoveSewerManholeNotesFromColumnToNotesTable : Migration
    {
        public override void Up()
        {
            var sql = @"INSERT INTO Note(Note, Date_Added, DataLinkID, DataTypeID, CreatedBy)
                        SELECT Notes, CreatedOn, SewerManholeID, 95, TP.FullName
                        FROM SewerManholes SM
                        INNER JOIN tblPermissions TP
                        ON TP.RecID = SM.CreatedBy
                        WHERE Notes IS NOT NULL AND Notes <> ' '";

            Execute.Sql(sql);

            Delete.Column("Notes").FromTable("SewerManholes");
        }

        public override void Down()
        {
            Alter.Table("SewerManholes").AddColumn("Notes").AsAnsiString(1000).Nullable();
        }
    }
}
