using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140708110532926), Tags("Production")]
    public class MakeDescriptionFieldVarcharForBug1970 : Migration
    {
        public override void Up()
        {
            Alter.Column("Description")
                 .OnTable("Materials")
                 .AsCustom("nvarchar(MAX)")
                 .Nullable();
        }

        public override void Down() { }
    }
}
