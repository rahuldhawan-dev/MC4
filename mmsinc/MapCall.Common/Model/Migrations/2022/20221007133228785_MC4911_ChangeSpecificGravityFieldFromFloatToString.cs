using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221007133228785), Tags("Production")]
    public class MC4911ChangeSpecificGravityFieldFromFloatToString : Migration
    {
        public override void Up()
        {
            Rename.Column("SpecificGravity")
                  .OnTable("Chemicals")
                  .To("SpecificGravityMin");
            Alter.Table("Chemicals")
                 .AddColumn("SpecificGravityMax")
                 .AsFloat()
                 .Nullable();
        }

        public override void Down()
        {
            Rename.Column("SpecificGravityMin")
                  .OnTable("Chemicals")
                  .To("SpecificGravity");
            Delete.Column("SpecificGravityMax").FromTable("Chemicals");
        }
    }
}