using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190125112713354), Tags("Production")]
    public class MC769_RemoveUserTableAndUserLog : Migration
    {
        public override void Up()
        {
            this.DeleteIndexIfItExists("UserViewed", "IX_USERVIEWED_USERLOGID");
            this.DeleteForeignKeyIfItExists("UserViewed", "stateID", "States");
            Delete.Column("UserTableId").FromTable("UserViewed");
            Delete.Column("UserLogId").FromTable("UserViewed");
            // These two fields have values that were only stored but never
            // used for anything.
            Delete.Column("StateId").FromTable("UserViewed");
            Delete.Column("Opcode").FromTable("UserViewed");

            this.DeleteForeignKeyIfItExists("UserLog", "stateID", "States");
            Delete.Table("UserLog");

            this.DeleteIndexIfItExists("UserTable", "_dta_index_UserTable_c_17_1003150619__K1");
            Delete.Table("UserTable");
        }

        public override void Down()
        {
            Create.Table("UserTable")
                  .WithColumn("UserId").AsInt32().NotNullable().Identity()
                  .WithColumn("UserUniqueId").AsString(50).Nullable()
                  .WithColumn("UserCustomerID").AsInt32().Nullable()
                  .WithColumn("UserLogin").AsString(50).Nullable()
                  .WithColumn("UserPassword").AsString(50).Nullable()
                  .WithColumn("UserFName").AsString(50).Nullable()
                  .WithColumn("UserLName").AsString(50).Nullable()
                  .WithColumn("UserEmail").AsString(50).Nullable()
                  .WithColumn("UserRights").AsString(50).Nullable()
                  .WithColumn("UserActive").AsBoolean().Nullable()
                  .WithColumn("InActiveMsg").AsString(400).Nullable()
                  .WithColumn("Ip1").AsString(16).Nullable()
                  .WithColumn("Ip2").AsString(16).Nullable()
                  .WithColumn("Ip3").AsString(16).Nullable()
                  .WithColumn("Ip4").AsString(16).Nullable()
                  .WithColumn("Ip5").AsString(16).Nullable()
                  .WithColumn("UserPermits").AsBoolean().Nullable()
                  .WithColumn("UserIPBypass").AsBoolean().Nullable()
                  .WithColumn("UserIPTimeout").AsDateTime().Nullable()
                  .WithColumn("lastCounty").AsString(10).Nullable()
                  .WithColumn("EmployeeID").AsString(50).Nullable()
                  .WithColumn("uid").AsString(500).Nullable();

            Execute.Sql(@"
CREATE CLUSTERED INDEX [_dta_index_UserTable_c_17_1003150619__K1] ON [dbo].[UserTable]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");

            Create.Table("UserLog")
                  .WithColumn("UserLogId").AsInt32().Identity().NotNullable()
                  .WithColumn("UserId").AsInt32().Nullable()
                  .WithColumn("UserCustomerID").AsString(50).Nullable()
                  .WithColumn("UserStartTime").AsDateTime().Nullable()
                  .WithColumn("UserEndTime").AsDateTime().Nullable()
                  .WithColumn("numTapsViewed").AsInt32().Nullable()
                  .WithColumn("numValvesViewed").AsInt32().Nullable()
                  .WithColumn("numAsBuiltsViewed").AsInt32().Nullable()
                  .WithColumn("numProductionViewed").AsInt32().Nullable()
                  .WithColumn("numSCADAViewed").AsInt32().Nullable()
                  .WithColumn("numPermitsSubmitted").AsInt32().Nullable()
                  .WithColumn("numMapsViewed").AsInt32().Nullable()
                  .WithColumn("tapIDsViewed").AsString(3000).Nullable()
                  .WithColumn("valveIDsViewed").AsString(3000).Nullable()
                  .WithColumn("asBuiltsIDsViewed").AsString(3000).Nullable()
                  .WithColumn("productionIDsViewed").AsString(3000).Nullable()
                  .WithColumn("permitIDsSubmitted").AsString(3000).Nullable()
                  .WithColumn("MapIDsViewed").AsString(3000).Nullable()
                  .WithColumn("UserIP").AsString(16).Nullable()
                  .WithColumn("stateID").AsInt32().Nullable()
                  .ForeignKey("FK_UserLog_States_stateID", "States", "StateID")
                  .WithColumn("numReportReservoir").AsInt32().Nullable()
                  .WithColumn("reportIDReservoir").AsString(3000).Nullable()
                  .WithColumn("numReportTurbidity").AsInt32().Nullable()
                  .WithColumn("reportIDTurbidity").AsString(3000).Nullable()
                  .WithColumn("numReportChemical").AsInt32().Nullable()
                  .WithColumn("reportIDChemical").AsString(3000).Nullable()
                  .WithColumn("numReportChemInv").AsInt32().Nullable()
                  .WithColumn("reportIDChemInv").AsString(3000).Nullable();

            Create.Column("Opcode").OnTable("UserViewed").AsString(3000).Nullable();
            Create.Column("StateId").OnTable("UserViewed").AsInt32().Nullable()
                  .ForeignKey("FK_UserViewed_States_stateID", "States", "StateID");
            Create.Column("UserLogId").OnTable("UserViewed").AsInt32().Nullable();
            Create.Column("UserTableId").OnTable("UserViewed").AsInt32().Nullable();

            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [IX_USERVIEWED_USERLOGID] ON [dbo].[UserViewed]
(
	[UserLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
        }
    }
}
