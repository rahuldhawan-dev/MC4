using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150714110703529), Tags("Production")]
    public class AddContactTypeForHydrantsOutOfServiceForBug2151 : Migration
    {
        public override void Up()
        {
            this.EnableIdentityInsert("ContactTypes");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM ContactTypes where ContactTypeID = 7) INSERT INTO ContactTypes(ContactTypeID, Name) Values(7, 'Hydrant Out Of Service');");
            this.DisableIdentityInsert("ContactTypes");
            this.AddNotificationType("Field Services", "Assets", "Hydrant Out Of Service");
        }

        public override void Down()
        {
            // noop
            this.RemoveNotificationType("Field Services", "Assets", "Hydrant Out Of Service");
        }
    }
}
