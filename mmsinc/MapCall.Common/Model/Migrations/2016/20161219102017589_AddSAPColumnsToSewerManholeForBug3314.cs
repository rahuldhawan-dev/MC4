using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20161219102017589), Tags("Production")]
    public class AddSAPColumnsToSewerManholeForBug3314 : Migration
    {
        public override void Up()
        {
            Alter.Table("SewerManholes").AddColumn("SAPErrorCode").AsCustom("varchar(max)").Nullable();
            Execute.Sql("Update MainTypes set SAPCode = 'CIU'  WHERE Description = 'Cast IronUnline'");
        }

        public override void Down()
        {
            Delete.Column("SAPErrorCode").FromTable("SewerManholes");
        }
    }
}
