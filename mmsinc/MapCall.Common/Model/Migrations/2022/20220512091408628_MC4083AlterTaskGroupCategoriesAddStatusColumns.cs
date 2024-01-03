using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220512091408628), Tags("Production")]
    public class AlterTaskGroupCategoriesAddStatusColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("TaskGroupCategories")
                 .AddColumn("Type").AsAnsiString(25).NotNullable().WithDefaultValue("")
                 .AddColumn("Abbreviation").AsAnsiString(5).NotNullable().WithDefaultValue("")
                 .AddColumn("IsActive").AsBoolean().Nullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("Type").FromTable("TaskGroupCategories");
            Delete.Column("Abbreviation").FromTable("TaskGroupCategories");
            Delete.Column("IsActive").FromTable("TaskGroupCategories");
        }
    }
}

