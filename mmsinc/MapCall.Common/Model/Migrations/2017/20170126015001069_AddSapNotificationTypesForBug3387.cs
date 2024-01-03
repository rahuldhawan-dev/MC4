using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20170126015001069), Tags("Production")]
    public class AddSapNotificationTypesForBug3387 : Migration
    {
        public override void Up()
        {
            Create.Table("SAPNotificationTypes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable();
            Execute.Sql("SET IDENTITY_INSERT SAPNotificationTypes ON;" +
                        "INSERT INTO SAPNotificationTypes(Id, Description) VALUES(20, 'Corrective Action');" +
                        "INSERT INTO SAPNotificationTypes(Id, Description) VALUES(33, 'Demolition');" +
                        "INSERT INTO SAPNotificationTypes(Id, Description) VALUES(35, 'Water Quality');" +
                        "INSERT INTO SAPNotificationTypes(Id, Description) VALUES(36, 'Emergency');" +
                        "INSERT INTO SAPNotificationTypes(Id, Description) VALUES(40, 'RP Capital');" +
                        "SET IDENTITY_INSERT SAPNotificationTypes OFF;");
        }

        public override void Down()
        {
            Delete.Table("SAPNotificationTypes");
        }
    }
}
