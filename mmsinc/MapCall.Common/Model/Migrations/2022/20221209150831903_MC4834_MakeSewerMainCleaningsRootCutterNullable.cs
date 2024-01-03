using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221209150831903), Tags("Production")]
    public class MC4834_MakeSewerMainCleaningsRootCutterNullable : Migration
    {
        public override void Up()
        {
            Alter.Column("RootCutter").OnTable("SewerMainCleanings").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Update.Table("SewerMainCleanings")
                  .Set(new {RootCutter = true})
                  .Where(new {RootCutter = (bool?)null});
            
            Alter.Column("RootCutter").OnTable("SewerMainCleanings").AsBoolean().NotNullable();
        }
    }
}

