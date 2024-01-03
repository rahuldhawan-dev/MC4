using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160819102657251), Tags("Production")]
    public class ModifyRestorationsTableBug2784 : Migration
    {
        private const string RESTORATIONS = "Restorations";

        private void CleanCompletedByFields()
        {
            const string sqlformat = @"
update [Restorations] set PartialRestorationCompletedByContractorId = (select top 1 ContractorId from Contractors where Name = '{0}' order by ContractorId desc) where PartialRestorationCompletedBy = '{1}'
update [Restorations] set FinalRestorationCompletedByContractorId = (select top 1 ContractorId from Contractors where Name = '{0}' order by ContractorId desc) where FinalRestorationCompletedBy = '{1}'
";

            Action<string, string> go = (valid, invalid) => { Execute.Sql(string.Format(sqlformat, valid, invalid)); };

            go("L & L Paving Co.", "L & L PAVING");
            go("L & L Paving Co.", "L&L");
            go("L & L Paving Co.", "L &  L Paving Co");
            go("L & L Paving Co.", "L &  L Paving Co.");
            go("L & L Paving Co.", "L & L  PAVING");
            go("L & L Paving Co.", "L & L. PAVING");
            go("L & L Paving Co.", "L &L PAVING");
            go("L & L Paving Co.", "L 7 L PAVING");
            go("L & L Paving Co.", "l and l");
            go("L & L Paving Co.", "l nl");
            go("L & L Paving Co.", "L& L PAVING");
            go("L & L Paving Co.", "L& L Paving Co");
            go("L & L Paving Co.", "L&& Paving Co.");
            go("L & L Paving Co.", "L&L PAVING");
            go("L & L Paving Co.", "L&L:");
            go("L & L Paving Co.", "L&LPaving Co.");
            go("L & L Paving Co.", "L, & L PAVING");
            go("L & L Paving Co.", "L7L");
            go("L & L Paving Co.", "L7L PAVING");
            go("L & L Paving Co.", "L&L Paving C0");
            go("L & L Paving Co.", "L&L Paving Co");
            go("L & L Paving Co.", "L&L Paving Co.");
            go("L & L Paving Co.", "L&L Paving Co. Inc");
            go("L & L Paving Co.", "L&L Paving Co. Inc.");
            go("L & L Paving Co.", " L & L PAVING");
            go("L & L Paving Co.", "L & L ");
            go("L & L Paving Co.", "L & L Paving Co");
            go("L & L Paving Co.", "L & L Paving Co.");
            go("L & L Paving Co.", "LNL");
            go("L & L Paving Co.", "L &L");

            go("Henkels & McCoy", "H & M");
            go("Henkels & McCoy", "H&M");
            go("Henkels & McCoy", "H &M");
            go("Henkels & McCoy", "H  & M");
            go("Henkels & McCoy", "henkel");
            go("Henkels & McCoy", "henkels");
            go("Henkels & McCoy", "henkel and mccoy");
            go("Henkels & McCoy", "henkels and mccoy");
            go("Henkels & McCoy", "henkels & mccoy");
            go("Henkels & McCoy", "hm");
            go("Henkels & McCoy", "hnm");
            go("Henkels & McCoy", " H & M");
            go("Henkels & McCoy", "H  M");
            go("Henkels & McCoy", "H &  M");
            go("Henkels & McCoy", "H & M2/19/");
            go("Henkels & McCoy", "H& M");
            go("Henkels & McCoy", "Henkels & McCoy Inc.");

            go("J. F. Kiely", "J F  Kiely");
            go("J. F. Kiely", "J.F. Kiely");
            go("J. F. Kiely", "jf kiely");
            go("J. F. Kiely", "JFK");
            go("J. F. Kiely", "keily");
            go("J. F. Kiely", "Kiely");
            go("J. F. Kiely", "kiley");
            go("J. F. Kiely", "kly");
            go("J. F. Kiely", "J.F Kiely");
            go("J. F. Kiely", "Kielly");

            go("J. Young", " J.Young");
            go("J. Young", "J Yong Concrete");
            go("J. Young", "J Young");
            go("J. Young", "J Young C oncrete");
            go("J. Young", "J Young Concrete");
            go("J. Young", "J Young Construction");
            go("J. Young", "J Ypung");
            go("J. Young", "J, Young");
            go("J. Young", "J. :Young Construction");
            go("J. Young", "J. Young");
            go("J. Young", "J. Young Concrete");
            go("J. Young", "J. Young Constructin");
            go("J. Young", "J. Young Construction");
            go("J. Young", "J. Young Constuction");
            go("J. Young", "J.Young");
            go("J. Young", "J.Young Concrete");
            go("J. Young", "J.Young Concrete Const");
            go("J. Young", "J.Young Construction");
            go("J. Young", "JYoung Construction");
            go("J. Young", "Y. Young Construction");
            go("J. Young", "Young");

            go("M & N Associates", "M&N");

            go("Valvetek", "VALVETEK");

            go("J. F. Creamer", "CREAMER");
            go("J. F. Creamer", "done per jfc");
            go("J. F. Creamer", "DUPLICATE PER JFC");
            go("J. F. Creamer", "JF C");
            go("J. F. Creamer", "JFC");
            go("J. F. Creamer", "JFC SAID NOT NEEDED");
            go("J. F. Creamer", "JFC.");
            go("J. F. Creamer", "JFC/Atlantic");
            go("J. F. Creamer", "jfc1");
            go("J. F. Creamer", "JFC21");
            go("J. F. Creamer", "jfc5/8/12");
            go("J. F. Creamer", "jfcc");
            go("J. F. Creamer", "JFCreamer");
            go("J. F. Creamer", "jffc");
            go("J. F. Creamer", "PER JFC CMPLETE 5/11/15");
            go("J. F. Creamer", "PER JFC COMPLETE 1/1/16");
            go("J. F. Creamer", "PER JFC COMPLETE 1/11/16");
            go("J. F. Creamer", "PER JFC COMPLETE 1/15/16");
            go("J. F. Creamer", "PER JFC COMPLETE 1/21/16");
            go("J. F. Creamer", "PER JFC COMPLETE 1/7/16");
            go("J. F. Creamer", "PER JFC COMPLETE 10/1/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/19/14");
            go("J. F. Creamer", "PER JFC COMPLETE 10/19/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/21/14");
            go("J. F. Creamer", "PER JFC COMPLETE 10/22/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/23/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/27/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/29/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/6/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/7/15");
            go("J. F. Creamer", "PER JFC COMPLETE 10/7/16");
            go("J. F. Creamer", "PER JFC COMPLETE 10/8/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/11/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/14/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/16/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/18/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/20/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/21/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/25/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/30/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/4/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/5/14");
            go("J. F. Creamer", "PER JFC COMPLETE 11/5/15");
            go("J. F. Creamer", "PER JFC COMPLETE 11/6/15");
            go("J. F. Creamer", "PER JFC COMPLETE 12/12/15");
            go("J. F. Creamer", "PER JFC COMPLETE 12/15/15");
            go("J. F. Creamer", "PER JFC COMPLETE 12/17/14");
            go("J. F. Creamer", "PER JFC COMPLETE 12/19/15");
            go("J. F. Creamer", "PER JFC COMPLETE 12/9/15");
            go("J. F. Creamer", "PER JFC COMPLETE 4/2/15");
            go("J. F. Creamer", "PER JFC COMPLETE 4/25/14");
            go("J. F. Creamer", "PER JFC COMPLETE 4/9/15");
            go("J. F. Creamer", "PER JFC COMPLETE 5/13/15");
            go("J. F. Creamer", "PER JFC COMPLETE 5/20/15");
            go("J. F. Creamer", "PER JFC COMPLETE 5/28/14");
            go("J. F. Creamer", "PER JFC COMPLETE 5/9/14");
            go("J. F. Creamer", "PER JFC COMPLETE 6/16/15");
            go("J. F. Creamer", "PER JFC COMPLETE 6/23/14");
            go("J. F. Creamer", "PER JFC COMPLETE 6/8/15");
            go("J. F. Creamer", "PER JFC COMPLETE 7/18/15");
            go("J. F. Creamer", "PER JFC COMPLETE 8/18/15");
            go("J. F. Creamer", "PER JFC COMPLETE 8/28/14");
            go("J. F. Creamer", "PER JFC COMPLETE 8/9/14");
            go("J. F. Creamer", "PER JFC COMPLETE 9/13/14");
            go("J. F. Creamer", "PER JFC COMPLETE 9/23/15");
            go("J. F. Creamer", "PER JFC COMPLETE1/18/16");
            go("J. F. Creamer", "PER JFC COMPLETE10/21/14");
            go("J. F. Creamer", "rd paved/jfc reported");
            go("J. F. Creamer", "repaved per JFC");
            go("J. F. Creamer", " JFC");
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues("RestorationPriorityUpchargeTypes", "On Demand", "On Demand After Hours",
                "Emergency Response", "Priority Response");
            Insert.IntoTable("DataType").Row(new {Data_Type = "Restorations", Table_Name = "Restorations"});

            this.AddDocumentType("Paving Slip", "Restorations");

            Alter.Table("Restorations")
                 .AlterColumn("ResponsePriorityId").AsInt32().NotNullable()
                 .AlterColumn("WorkOrderId").AsInt32().Nullable()
                 .AddColumn("WBSNumber").AsString(30).Nullable()
                 .AddColumn("AssignedContractorId")
                 .AsInt32().Nullable()
                 .ForeignKey("FK_Restorations_Contractors_AssignedContractorId", "Contractors", "ContractorId")
                 .AddColumn("AssignedContractorAt").AsDateTime().Nullable()
                 .AddColumn("TownId").AsInt32().Nullable()
                 .ForeignKey("FK_Restorations_Towns_TownId", "Towns", "TownId")
                 .AddColumn("OperatingCenterId").AsInt32().Nullable()
                 .ForeignKey("FK_Restorations_OperatingCenters_OperatingCenterId", "OperatingCenters",
                      "OperatingCenterId")
                 .AddColumn("CompletedByOthers").AsBoolean().Nullable()
                 .AddColumn("CompletedByOthersNotes").AsText().Nullable()
                 .AddColumn("TrafficControlRequired").AsBoolean().Nullable()
                 .AddColumn("InitialPurchaseOrderNumber").AsString(20).Nullable()
                  // Partials
                 .AddColumn("PartialRestorationCompletedByContractorId")
                 .AsInt32().Nullable()
                 .ForeignKey("FK_Restorations_Contractors_PartialRestorationCompletedByContractorId", "Contractors",
                      "ContractorId")
                 .AddColumn("PartialRestorationNotes")
                 .AsCustom("nvarchar(max)").Nullable()
                 .AddColumn("PartialRestorationPurchaseOrderNumber").AsString(20).Nullable()
                 .AddColumn("PartialRestorationPriorityUpchargeTypeId").AsInt32().Nullable()
                 .ForeignKey(
                      "FK_Restorations_RestorationPriorityUpchargeTypes_PartialRestorationPriorityUpchargeTypeId",
                      "RestorationPriorityUpchargeTypes", "Id")
                 .AddColumn("PartialRestorationPriorityUpcharge").AsDecimal(18, 2).Nullable()
                 .AddColumn("PartialRestorationTrafficControlInvoiceNumber").AsString(30).Nullable()
                 .AddColumn("PartialRestorationDueDate").AsDateTime().Nullable()
                 .AddColumn("PartialRestorationApprovedAt").AsDateTime().Nullable()
                 .AddColumn("PartialRestorationBreakoutBilling").AsText()
                 .Nullable() // This is only for partial, not final. Why? Who knows.

                  // Finals
                 .AddColumn("FinalRestorationCompletedByContractorId")
                 .AsInt32().Nullable()
                 .ForeignKey("FK_Restorations_Contractors_FinalRestorationCompletedByContractorId", "Contractors",
                      "ContractorId")
                 .AddColumn("FinalRestorationNotes")
                 .AsCustom("nvarchar(max)").Nullable()
                 .AddColumn("FinalRestorationPurchaseOrderNumber").AsString(20).Nullable()
                 .AddColumn("FinalRestorationPriorityUpchargeTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_Restorations_RestorationPriorityUpchargeTypes_FinalRestorationPriorityUpchargeTypeId",
                      "RestorationPriorityUpchargeTypes", "Id")
                 .AddColumn("FinalRestorationPriorityUpcharge").AsDecimal(18, 2).Nullable()
                 .AddColumn("FinalRestorationTrafficControlInvoiceNumber").AsString(30).Nullable()
                 .AddColumn("FinalRestorationDueDate").AsDateTime().Nullable()
                 .AddColumn("DateReopened").AsDateTime().Nullable()
                 .AddColumn("DateRescheduled").AsDateTime().Nullable()
                 .AddColumn("DateRecompleted").AsDateTime().Nullable();

            Rename.Column("DateApproved").OnTable(RESTORATIONS).To("FinalRestorationApprovedAt");
            Rename.Column("TotalInitialActualCost").OnTable(RESTORATIONS).To("PartialRestorationActualCost");

            Create.Table("PartialRestorationsRestorationMethods")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("RestorationId").AsInt32().NotNullable()
                  .ForeignKey("FK_PartialRestorationsRestorationMethods_Restoration_RestorationId", RESTORATIONS,
                       "RestorationId")
                  .WithColumn("RestorationMethodId").AsInt32().NotNullable()
                  .ForeignKey("FK_PartialRestorationsRestorationMethods_RestorationMethod_RestorationMethodId",
                       "RestorationMethods", "RestorationMethodId");

            // Insert all the partial/initial restorations
            Execute.Sql(@"insert into [PartialRestorationsRestorationMethods] (RestorationId, RestorationMethodId)
                          select
                            [RestorationId],
                            [PartialRestorationMethodId]
                          from [Restorations] where [PartialRestorationMethodId] is not null");

            Create.Table("FinalRestorationsRestorationMethods")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("RestorationId").AsInt32().NotNullable()
                  .ForeignKey("FK_FinalRestorationsRestorationMethods_Restoration_RestorationId", RESTORATIONS,
                       "RestorationId")
                  .WithColumn("RestorationMethodId").AsInt32().NotNullable()
                  .ForeignKey("FK_FinalRestorationsRestorationMethods_RestorationMethod_RestorationMethodId",
                       "RestorationMethods", "RestorationMethodId");

            // Insert all the final restorations
            Execute.Sql(@"insert into [FinalRestorationsRestorationMethods] (RestorationId, RestorationMethodId)
                          select
                            [RestorationId],
                            [FinalRestorationMethodId]
                          from [Restorations] where [FinalRestorationMethodId] is not null");

            Delete.ForeignKey("FK_Restorations_RestorationMethods_PartialRestorationMethodID").OnTable(RESTORATIONS);
            Delete.ForeignKey("FK_Restorations_RestorationMethods_FinalRestorationMethodID").OnTable(RESTORATIONS);
            Delete.Column("PartialRestorationMethodID").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationMethodID").FromTable(RESTORATIONS);

            Alter.Table("RestorationTypes")
                 .AddColumn("PartialRestorationDaysToComplete").AsInt32().NotNullable().WithDefaultValue(30)
                 .AddColumn("FinalRestorationDaysToComplete").AsInt32().NotNullable().WithDefaultValue(30);

            Update.Table("RestorationTypes")
                  .Set(new {PartialRestorationDaysToComplete = 90, FinalRestorationDaysToComplete = 90})
                  .Where(new {Description = "ASPHALT-DRIVEWAY"});
            Update.Table("RestorationTypes")
                  .Set(new {PartialRestorationDaysToComplete = 30, FinalRestorationDaysToComplete = 90})
                  .Where(new {Description = "ASPHALT-STREET"});
            Update.Table("RestorationTypes")
                  .Set(new {PartialRestorationDaysToComplete = 90, FinalRestorationDaysToComplete = 90})
                  .Where(new {Description = "DRIVEWAY APRON RESTORATION"});

            CleanCompletedByFields();

            Execute.Sql(@"
update Restorations set 
	OperatingCenterId = (select top 1 OperatingCenterId from WorkOrders where WorkOrders.WorkOrderId = Restorations.WorkOrderId),
	TownId = (select top 1 TownId from WorkOrders where WorkOrders.WorkOrderId = Restorations.WorkOrderId),
    WBSNumber = (select top 1 AccountCharged from WorkOrders where WorkOrders.WorkOrderId = Restorations.WorkOrderId)");

            /* RestorationTypeID	Description
1	ASPHALT-STREET
2	ASPHALT-DRIVEWAY
3	CONCRETE STREET
4	CURB RESTORATION
5	CURB/GUTTER RESTORATION
6	DRIVEWAY APRON RESTORATION
7	GROUND RESTORATION
8	SIDEWALK RESTORATION
9	ASPHALT - ALLEY
            */

            AddRestMethod("2\" Overlay", false, true, 1, 2, 9);
            AddRestMethod("4\" Concrete", false, true, 8);
            AddRestMethod("6\" Concrete", false, true, 8);
            AddRestMethod("Pavers", false, true, 8);
            AddRestMethod("2\" Sidewalk Bituminous", false, true, 8);
            AddRestMethod("Base", true, false, 4, 5);
            AddRestMethod("6x18 Concrete", false, true, 4);
            AddRestMethod("Belgium Block", false, true, 4);
            AddRestMethod("Asphalt", false, true, 4);
            AddRestMethod("Mono with Gutter", false, true, 5);
            AddRestMethod("4\" Apron Bituminous", false, true, 6);
            AddRestMethod("6\" Apron Concrete", false, true, 6);
            AddRestMethod("Topsoil and Sod", false, true, 7);
            AddRestMethod("Black Mulch", false, true, 7);
            AddRestMethod("6\" Concrete Pavement", false, true, 3);
            AddRestMethod("8\" Concrete Pavement", false, true, 3);
            AddRestMethod("10\" Concrete Pavement", false, true, 3);
        }

        private void AddRestMethod(string methodName, bool isInitial, bool isFinal, params int[] types)
        {
            //Insert.IntoTable("RestorationMethods").Row(new { Description = methodName });

            Execute.Sql(@"
IF NOT EXISTS (SELECT * FROM [RestorationMethods] 
                   WHERE Description = '{0}')
   BEGIN
       INSERT INTO [RestorationMethods] (Description) VALUES ('{0}')
   END
", methodName);

            foreach (var type in types)
            {
                Execute.Sql(@"
        declare @methodId int;
        set @methodId = (select RestorationMethodId from RestorationMethods where Description = '{0}')

        insert into [RestorationMethodsRestorationTypes] (RestorationMethodID, RestorationTypeID, InitialMethod, FinalMethod)
        values (@methodId, {1}, '{2}', '{3}')

", methodName, type, isInitial, isFinal);
            }
        }

        public override void Down()
        {
            Delete.Column("PartialRestorationDaysToComplete").FromTable("RestorationTypes");
            Delete.Column("FinalRestorationDaysToComplete").FromTable("RestorationTypes");

            Create.Column("PartialRestorationMethodID").OnTable(RESTORATIONS).AsInt32().Nullable()
                  .ForeignKey("FK_Restorations_RestorationMethods_PartialRestorationMethodID", "RestorationMethods",
                       "RestorationMethodID");
            Create.Column("FinalRestorationMethodID").OnTable(RESTORATIONS).AsInt32().Nullable()
                  .ForeignKey("FK_Restorations_RestorationMethods_FinalRestorationMethodID", "RestorationMethods",
                       "RestorationMethodID");

            // If rolled back, this will break if any restorations have multiple restoration methods. Only select
            // the first result for rolling back.
            Execute.Sql(@"
            update [Restorations] set PartialRestorationMethodId = (select top 1 RestorationMethodId from PartialRestorationsRestorationMethods rm where rm.RestorationId = Restorations.RestorationId)
            update [Restorations] set FinalRestorationMethodId = (select top 1 RestorationMethodId from FinalRestorationsRestorationMethods rm where rm.RestorationId = Restorations.RestorationId)");

            Delete.ForeignKey("FK_FinalRestorationsRestorationMethods_RestorationMethod_RestorationMethodId")
                  .OnTable("FinalRestorationsRestorationMethods");
            Delete.ForeignKey("FK_FinalRestorationsRestorationMethods_Restoration_RestorationId")
                  .OnTable("FinalRestorationsRestorationMethods");
            Delete.Table("FinalRestorationsRestorationMethods");

            Delete.ForeignKey("FK_PartialRestorationsRestorationMethods_RestorationMethod_RestorationMethodId")
                  .OnTable("PartialRestorationsRestorationMethods");
            Delete.ForeignKey("FK_PartialRestorationsRestorationMethods_Restoration_RestorationId")
                  .OnTable("PartialRestorationsRestorationMethods");
            Delete.Table("PartialRestorationsRestorationMethods");

            Delete.ForeignKey("FK_Restorations_Contractors_AssignedContractorId").OnTable(RESTORATIONS);
            Delete.ForeignKey(
                       "FK_Restorations_RestorationPriorityUpchargeTypes_PartialRestorationPriorityUpchargeTypeId")
                  .OnTable(RESTORATIONS);
            Delete.ForeignKey("FK_Restorations_RestorationPriorityUpchargeTypes_FinalRestorationPriorityUpchargeTypeId")
                  .OnTable(RESTORATIONS);
            Delete.ForeignKey("FK_Restorations_Contractors_PartialRestorationCompletedByContractorId")
                  .OnTable(RESTORATIONS);
            Delete.ForeignKey("FK_Restorations_Contractors_FinalRestorationCompletedByContractorId")
                  .OnTable(RESTORATIONS);
            Delete.ForeignKey("FK_Restorations_Towns_TownId").OnTable(RESTORATIONS);
            Delete.ForeignKey("FK_Restorations_OperatingCenters_OperatingCenterId").OnTable(RESTORATIONS);

            Rename.Column("PartialRestorationActualCost").OnTable(RESTORATIONS).To("TotalInitialActualCost");
            Rename.Column("FinalRestorationApprovedAt").OnTable(RESTORATIONS).To("DateApproved");

            Delete.Column("AssignedContractorId").FromTable(RESTORATIONS);
            Delete.Column("AssignedContractorAt").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationDueDate").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationDueDate").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationNotes").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationPurchaseOrderNumber").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationPurchaseOrderNumber").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationNotes").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationPriorityUpchargeTypeId").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationPriorityUpchargeTypeId").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationPriorityUpcharge").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationPriorityUpcharge").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationTrafficControlInvoiceNumber").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationTrafficControlInvoiceNumber").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationApprovedAt").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationCompletedByContractorId").FromTable(RESTORATIONS);
            Delete.Column("FinalRestorationCompletedByContractorId").FromTable(RESTORATIONS);
            Delete.Column("WBSNumber").FromTable(RESTORATIONS);
            Delete.Column("TownId").FromTable(RESTORATIONS);
            Delete.Column("OperatingCenterId").FromTable(RESTORATIONS);
            Delete.Column("CompletedByOthers").FromTable(RESTORATIONS);
            Delete.Column("CompletedByOthersNotes").FromTable(RESTORATIONS);
            Delete.Column("TrafficControlRequired").FromTable(RESTORATIONS);
            Delete.Column("DateReopened").FromTable(RESTORATIONS);
            Delete.Column("DateRescheduled").FromTable(RESTORATIONS);
            Delete.Column("DateRecompleted").FromTable(RESTORATIONS);
            Delete.Column("InitialPurchaseOrderNumber").FromTable(RESTORATIONS);
            Delete.Column("PartialRestorationBreakoutBilling").FromTable(RESTORATIONS);

            Alter.Table(RESTORATIONS)
                 .AlterColumn("ResponsePriorityId").AsInt32().Nullable();

            this.RemoveDocumentTypeAndAllRelatedDocuments("Paving Slip", "Restorations");
            this.DeleteDataType("Restorations");

            Delete.Table("RestorationPriorityUpchargeTypes");
        }
    }
}
