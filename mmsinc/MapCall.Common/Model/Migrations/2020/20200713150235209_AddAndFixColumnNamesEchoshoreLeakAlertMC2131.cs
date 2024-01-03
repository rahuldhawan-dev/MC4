using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200713150235209), Tags("Production")]
    public class AddAndFixColumnNamesEchoshoreLeakAlertMC2131 : Migration
    {
        public override void Up()
        {
            Rename.Column("SockedID1Id").OnTable("EchoshoreLeakAlerts").To("Hydrant1Id");
            Rename.Column("SockedID2Id").OnTable("EchoshoreLeakAlerts").To("Hydrant2Id");
            Rename.Column("SocketId1Text").OnTable("EchoshoreLeakAlerts").To("Hydrant1Text");
            Rename.Column("SocketId2Text").OnTable("EchoshoreLeakAlerts").To("Hydrant2Text");
            Rename.Column("DistanceFrom1").OnTable("EchoshoreLeakAlerts").To("DistanceFromHydrant1");
            Rename.Column("DistanceFrom2").OnTable("EchoshoreLeakAlerts").To("DistanceFromHydrant2");
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("EchoshoreLeakAlertId", "EchoshoreLeakAlerts").Nullable();
            Execute.Sql("IF NOT Exists(select 1 from sysindexes where name = 'IX_EchoshoreLeakAlertIds') " +
                        "   CREATE INDEX IX_EchoshoreLeakAlertIds ON WorkOrders(EchoshoreLeakAlertId)");
        }

        public override void Down()
        {
            Execute.Sql("IF Exists(select 1 from sysindexes where name = 'IX_EchoshoreLeakAlertIds')" +
                        "   DROP INDEX IX_EchoshoreLeakAlertIds ON WorkOrders");
            Delete.ForeignKeyColumn("WorkOrders", "EchoshoreLeakAlertId", "EchoshoreLeakAlerts");
            Rename.Column("SocketId1Id").OnTable("EchoshoreLeakAlerts").To("Hydrant1Id");
            Rename.Column("SocketId2Id").OnTable("EchoshoreLeakAlerts").To("Hydrant2Id");
            Rename.Column("Hydrant1Text").OnTable("EchoshoreLeakAlerts").To("SocketId1Text");
            Rename.Column("Hydrant2Text").OnTable("EchoshoreLeakAlerts").To("SocketId2Text");
            Rename.Column("DistanceFromHydrant1").OnTable("EchoshoreLeakAlerts").To("DistanceFrom1");
            Rename.Column("DistanceFromHydrant2").OnTable("EchoshoreLeakAlerts").To("DistanceFrom2");
        }
    }
}
