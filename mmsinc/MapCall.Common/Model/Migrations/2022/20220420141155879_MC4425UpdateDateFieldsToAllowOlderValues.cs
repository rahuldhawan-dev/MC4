using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220420141155879), Tags("Production")]
    public class MC4425UpdateDateFieldsToAllowOlderValues : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_ApplApvd];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_ApplRcvd];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_ApplSent];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_ContactDate];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_DateClosed];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_DateInstalled];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_DateIssuedtoField];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_InspDate];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_InstInvDate];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_PermitExpDate];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_PermitRcvdDate];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_PermitSentDate];
ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_tblNJAWService_RetireDate];
DROP INDEX [IDX_RetireDate] ON [dbo].[Services];
            ");
            Alter.Column("ApplicationApprovedOn").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("ApplicationReceivedOn").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("ApplicationSentOn").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("ContactDate").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("DateClosed").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("DateInstalled").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("DateIssuedtoField").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("InspectionDate").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("InstallationInvoiceDate").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("PermitExpirationDate").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("PermitReceivedDate").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("PermitSentDate").OnTable("Services").AsDateTime().Nullable();
            Alter.Column("RetiredDate").OnTable("Services").AsDateTime().Nullable();
            Execute.Sql(@"
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_ApplApvd]  DEFAULT (1 / 1 / 1900) FOR [ApplicationApprovedOn];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_ApplRcvd]  DEFAULT (1 / 1 / 1900) FOR [ApplicationReceivedOn];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_ApplSent]  DEFAULT (1 / 1 / 1900) FOR [ApplicationSentOn];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_ContactDate]  DEFAULT (1 / 1 / 1900) FOR [ContactDate];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_DateClosed]  DEFAULT (1 / 1 / 1900) FOR [DateClosed];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_DateInstalled]  DEFAULT (1 / 1 / 1900) FOR [DateInstalled];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_DateIssuedtoField]  DEFAULT (1 / 1 / 1900) FOR [DateIssuedtoField];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_InspDate]  DEFAULT (1 / 1 / 1900) FOR [InspectionDate];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_InstInvDate]  DEFAULT (1 / 1 / 1900) FOR [InstallationInvoiceDate];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_PermitExpDate]  DEFAULT (1 / 1 / 1900) FOR [PermitExpirationDate];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_PermitRcvdDate]  DEFAULT (1 / 1 / 1900) FOR [PermitReceivedDate];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_PermitSentDate]  DEFAULT (1 / 1 / 1900) FOR [PermitSentDate];
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [DF_tblNJAWService_RetireDate]  DEFAULT (1 / 1 / 1900) FOR [RetiredDate];
CREATE NONCLUSTERED INDEX [IDX_RetireDate] ON [dbo].[Services] ([RetiredDate] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
            ");
        }

        public override void Down()
        {
            // can't down these as we would lose data after this point
        }
    }
}

