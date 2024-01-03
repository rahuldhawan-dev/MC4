using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    // Even though this seeds, it isn't treated as a seed with a profile
    // attribute. 
    [Tags("Development"), Migration(20130131165450)]
    public class InitialDatabase : Migration
    {
        public override void Up()
        {
            Execute.Script(@"..\mmsinc\MapCall.Common\Model\Scripts\01 Create Database.sql");
            Execute.Script(@"..\mmsinc\MapCall.Common\Model\Scripts\02 Import Development Data.sql");
            Execute.Script(@"..\mmsinc\MapCall.Common\Model\Scripts\03 Import Roles.sql");
            Execute.Script(@"..\mmsinc\MapCall.Common\Model\Scripts\04 Import Common Data.sql");
        }

        public override void Down() { }
    }
}
