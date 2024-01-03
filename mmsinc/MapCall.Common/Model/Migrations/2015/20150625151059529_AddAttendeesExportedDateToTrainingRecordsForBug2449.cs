using System.Configuration;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150625151059529), Tags("Production")]
    public class AddAttendeesExportedDateToTrainingRecordsForBug2449 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblTrainingRecords").AddColumn("AttendeesExportedDate").AsDateTime().Nullable();
            Execute.Sql(
                "CREATE NONCLUSTERED INDEX [_dta_index_EmployeeLink_19_1251535542__K2_K3_1_4_5_6] ON [dbo].[EmployeeLink](	[DataLinkID] ASC,	[DataTypeID] ASC) INCLUDE ( 	[EmployeeLinkID],	[CreatedOn],	[CreatedBy],	[tblEmployeeID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]");
        }

        public override void Down()
        {
            Execute.Sql(
                "if exists (Select 1 from sysindexes where name = '_dta_index_EmployeeLink_19_1251535542__K2_K3_1_4_5_6') DROP INDEX [_dta_index_EmployeeLink_19_1251535542__K2_K3_1_4_5_6] ON [dbo].[EmployeeLink]");
            Delete.Column("AttendeesExportedDate").FromTable("tblTrainingRecords");
        }
    }
}
