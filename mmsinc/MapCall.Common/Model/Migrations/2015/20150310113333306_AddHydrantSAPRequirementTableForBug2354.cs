using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150310113333306), Tags("Production")]
    public class AddHydrantSAPRequirementTableForBug2354 : Migration
    {
        public const string TABLE_NAME = "HydrantSAPRequirements";
        public const int REPORTED_BY = 50, FUNCTIONAL_LOCATION = 50;

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithIdentityColumn()
                  .WithColumn("NotificationType").AsInt32().Nullable()
                  .WithColumn("Notification").AsInt32().NotNullable()
                  .WithColumn("NotificationDateTime").AsDateTime().Nullable()
                  .WithColumn("SAPEquipmentID").AsInt32().NotNullable()
                  .WithColumn("FunctionalLocation").AsAnsiString(FUNCTIONAL_LOCATION).Nullable()
                  .WithColumn("RequiredStartDateTime").AsDateTime().Nullable()
                  .WithColumn("RequiredEndDateTime").AsDateTime().Nullable()
                  .WithColumn("ReportedBy").AsAnsiString(REPORTED_BY).Nullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable();

            Alter.Table("tblNJAWHydInspData").AddColumn("HydrantId").AsInt32().Nullable();
            Execute.Sql(
                "update tblNJAWHydInspData set HydrantId = (select Top 1 RecID from tblNJAWHydrant H where H.HydNum = hid.HydNum and H.OpCntr = hid.OpCntr) from tblNJAWHydInspData hid;" +
                "CREATE STATISTICS [_dta_stat_237959924_3_26] ON [dbo].[tblNJAWHydInspData]([DateInspect], [HydrantId]);" +
                "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydInspData_19_237959924__K26_K3_9_10] ON [dbo].[tblNJAWHydInspData]([HydrantId] ASC,[DateInspect] ASC) INCLUDE ([Inspect], [InspectedBy]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
        }

        public override void Down()
        {
            Execute.Sql(
                "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_3_26') DROP STATISTICS [dbo].[tblNJAWHydInspData].[_dta_stat_237959924_3_26];" +
                "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydInspData_19_237959924__K26_K3_9_10') DROP INDEX [tblNJAWHydInspData].[_dta_index_tblNJAWHydInspData_19_237959924__K26_K3_9_10];");
            Delete.Table(TABLE_NAME);
            Delete.Column("HydrantId").FromTable("tblNJAWHydInspData");
        }
    }
}
