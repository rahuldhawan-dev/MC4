using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200422102850365), Tags("Production")]
    public class MC2156AddFreeNoReadReasonAndTotalNoReadReason : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("NoReadReasons", "Kit Not Available", "Not Directed by Manager");

            Alter.Table("HydrantInspections")
                 .AddForeignKeyColumn("FreeNoReadReasonId", "NoReadReasons")
                 .AddForeignKeyColumn("TotalNoReadReasonId", "NoReadReasons");
            Alter.Table("BlowOffInspections")
                 .AddForeignKeyColumn("FreeNoReadReasonId", "NoReadReasons")
                 .AddForeignKeyColumn("TotalNoReadReasonId", "NoReadReasons");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("HydrantInspections", "FreeNoReadReasonId", "NoReadReasons");
            Delete.ForeignKeyColumn("BlowOffInspections", "FreeNoReadReasonId", "NoReadReasons");
            Delete.ForeignKeyColumn("HydrantInspections", "TotalNoReadReasonId", "NoReadReasons");
            Delete.ForeignKeyColumn("BlowOffInspections", "TotalNoReadReasonId", "NoReadReasons");
            Delete.Table("NoReadReasons");
        }
    }
}
