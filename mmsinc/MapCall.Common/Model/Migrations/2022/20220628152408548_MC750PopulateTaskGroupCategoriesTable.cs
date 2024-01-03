using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220628152408548), Tags("Production")]
    public class MC750PopulateTaskGroupCategoriesTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                UPDATE TaskGroupCategories SET Description = 'Chemical Full Description', Type = 'Chemical', Abbreviation = 'CHEM', IsActive = 1
                WHERE Description = 'Chemical';
                UPDATE TaskGroupCategories SET Description = 'Electrical Full Description', Type = 'Electrical', Abbreviation = 'ELEC', IsActive = 1
                WHERE Description = 'Electrical';
                UPDATE TaskGroupCategories SET Description = 'Instrumentation Full Description', Type = 'Instrumentation', Abbreviation = 'INST', IsActive = 1
                WHERE Description = 'Instrumentation';
                UPDATE TaskGroupCategories SET Description = 'Mechanical Full Description', Type = 'Mechanical', Abbreviation = 'MECH', IsActive = 1
                WHERE Description = 'Mechanical';
                UPDATE TaskGroupCategories SET Description = 'Operational Full Description', Type = 'Operational', Abbreviation = 'OPER', IsActive = 1
                WHERE Description = 'Operational';
                UPDATE TaskGroupCategories SET Description = 'Safety Full Description', Type = 'Safety', Abbreviation = 'SAFE', IsActive = 1
                WHERE Description = 'Safety';
            ");

            Execute.Sql(@"
                UPDATE TaskGroups SET TaskGroupId = SUBSTRING(TaskGroupId, 1, 8);
            ");

            Alter.Table("TaskGroups")
                 .AlterColumn("TaskGroupId").AsString(8).NotNullable().WithDefaultValue("");
        }

        public override void Down()
        {
            Execute.Sql(@"
                UPDATE TaskGroupCategories SET Description = 'Chemical', Type = '', Abbreviation = '', IsActive = 0
                WHERE Type = 'Chemical';
                UPDATE TaskGroupCategories SET Description = 'Electrical', Type = '', Abbreviation = '', IsActive = 0
                WHERE Type = 'Electrical';
                UPDATE TaskGroupCategories SET Description = 'Instrumentation', Type = '', Abbreviation = '', IsActive = 0
                WHERE Type = 'Instrumentation';
                UPDATE TaskGroupCategories SET Description = 'Mechanical', Type = '', Abbreviation = '', IsActive = 0
                WHERE Type = 'Mechanical';
                UPDATE TaskGroupCategories SET Description = 'Operational', Type = '', Abbreviation = '', IsActive = 0
                WHERE Type = 'Operational';
                UPDATE TaskGroupCategories SET Description = 'Safety', Type = '', Abbreviation = '', IsActive = 0
                WHERE Type = 'Safety';
            ");
        }
    }
}

