using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140430095512), Tags("Production")]
    public class AddDoNotOrderColumnToMaterialsTableBug1841 : Migration
    {
        public override void Up()
        {
            Alter.Table("Materials")
                 .AddColumn("DoNotOrder")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("DoNotOrder").FromTable("Materials");
        }
    }
}
