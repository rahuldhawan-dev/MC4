using FluentMigrator;
using System.Collections.Generic;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230703123226427), Tags("Production")]
    public class MC4807_AddHelpCategoryTypes : Migration
    {
        public override void Up()
        {
            foreach (var description in new List<string>{ "Template", "Informational" })
            {
                Execute.Sql($@"
                IF NOT EXISTS(SELECT * FROM HelpCategories WHERE Description = '{description}')
                BEGIN
                    INSERT INTO HelpCategories([Description]) VALUES ('{description}');
                END");
            }
        }

        public override void Down()
        {
            foreach (var description in new List<string> { "Template", "Informational" })
            {
                Execute.Sql($@"DELETE HelpCategories WHERE [Description] = '{description}';");
            }
        }
    }
}

