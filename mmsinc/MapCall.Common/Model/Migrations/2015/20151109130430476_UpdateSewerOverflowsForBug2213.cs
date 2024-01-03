using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151109130430476), Tags("Production")]
    public class UpdateSewerOverflowsForBug2213 : Migration
    {
        public override void Up()
        {
            Alter.Table("SewerOverflows").AddForeignKeyColumn("CreatedById", "tblPermissions", "RecId");
            Execute.Sql(
                "UPDATE SewerOverflows SET CreatedById = (Select RecID from tblPermissions where UserName = CreatedBy)");
            Delete.Column("CreatedBy").FromTable("SewerOverflows");

            Delete.Table("SewerOverflowDEPInformation");

            this.AddNotificationType("Field Services", "Assets", "Sewer Overflow");

            Alter.Table("OperatingCenters").AddColumn("MaximumOverflowGallons").AsInt32().Nullable();
            Execute.Sql("UPDATE OperatingCenters SET MaximumOverflowGallons = 0 WHERE OperatingCenterID = 11");
            Execute.Sql("UPDATE OperatingCenters SET MaximumOverflowGallons = 25 WHERE OperatingCenterID = 14");
            Execute.Sql("UPDATE OperatingCenters SET MaximumOverflowGallons = 0 WHERE OperatingCenterID = 13");

            Execute.Sql(
                "INSERT INTO NotificationConfigurations(ContactId, OperatingCenterID, NotificationPurposeID) SELECT (select ContactID from Contacts where lastname = 'bauer'), 14, (select NotificationPurposeId from NotificationPurposes where ModuleID = 73 and Purpose = 'Sewer Overflow')");
            Execute.Sql(
                "INSERT INTO NotificationConfigurations(ContactId, OperatingCenterID, NotificationPurposeID) SELECT (select ContactID from Contacts where lastname = 'donoso'), 14, (select NotificationPurposeId from NotificationPurposes where ModuleID = 73 and Purpose = 'Sewer Overflow')");
            Execute.Sql(
                "INSERT INTO NotificationConfigurations(ContactId, OperatingCenterID, NotificationPurposeID) SELECT (select ContactID from Contacts where lastname = 'bauer'), 11, (select NotificationPurposeId from NotificationPurposes where ModuleID = 73 and Purpose = 'Sewer Overflow')");
            Execute.Sql(
                "INSERT INTO NotificationConfigurations(ContactId, OperatingCenterID, NotificationPurposeID) SELECT (select ContactID from Contacts where lastname = 'donoso'), 11, (select NotificationPurposeId from NotificationPurposes where ModuleID = 73 and Purpose = 'Sewer Overflow')");
        }

        public override void Down()
        {
            Delete.Column("MaximumOverflowGallons").FromTable("OperatingCenters");

            this.RemoveNotificationType("Field Services", "Assets", "Sewer Overflow");

            Create.Table("SewerOverflowDEPInformation")
                  .WithForeignKeyColumn("OperatingCenterID", "OperatingCenters", "OperatingCenterID")
                  .WithColumn("MaximumGallons").AsInt32().NotNullable()
                  .WithColumn("PrimaryEmail").AsAnsiString(255).NotNullable()
                  .WithColumn("SecondaryEmail").AsAnsiString(255).NotNullable();
            Execute.Sql(
                "INSERT INTO SewerOverflowDEPInformation VALUES(11, 0, 'Juan.Donoso@amwater.com', 'George.Bauer@amwater.com')");
            Execute.Sql(
                "INSERT INTO SewerOverflowDEPInformation VALUES(14, 25, 'Juan.Donoso@amwater.com', 'George.Bauer@amwater.com')");

            Alter.Table("SewerOverflows").AddColumn("CreatedBy").AsAnsiString(50).Nullable();
            Execute.Sql(
                "UPDATE SewerOverflows SET CreatedBy = (Select UserName from tblPermissions where RecID = CreatedById)");
            Delete.ForeignKeyColumn("SewerOverflows", "CreatedById", "tblPermissions", "RecID");
        }
    }
}
