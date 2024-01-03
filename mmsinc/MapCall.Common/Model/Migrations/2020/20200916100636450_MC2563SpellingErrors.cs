using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916100636450), Tags("Production")]
    public class MC2563SpellingErrors : Migration
    {
        public override void Up()
        {
            Update.Table("ConfinedSpaceFormEntrantTypes")
                  .Set(new {Description = "Attendant"})
                  .Where(new {Description = "Attendent"});
        }

        public override void Down()
        {
            Update.Table("ConfinedSpaceFormEntrantTypes")
                  .Set(new {Description = "Attendent"})
                  .Where(new {Description = "Attendant"});
        }
    }
}
