using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200430103725794), Tags("Production")]
    public class AddWork1VLeadPipingColumnsForMC2238 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("InternalLeadPipingIndicator").AsBoolean().Nullable()
                 .AddForeignKeyColumn("CustomerSideMaterialId", "ServiceMaterials", "ServiceMaterialID").Nullable();
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddColumn("InternalLeadPipingIndicator").AsBoolean().Nullable()
                 .AddForeignKeyColumn("CustomerSideMaterialId", "ServiceMaterials", "ServiceMaterialID").Nullable()
                 .AddColumn("LeadInspectionDate").AsDateTime().Nullable()
                 .AddColumn("LeadInspectedBy").AsAnsiString(7).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LeadInspectedBy").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("LeadInspectionDate").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("CustomerSideMaterial").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("InternalLeadPipingIndicator").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("CustomerSideMaterial").FromTable("ShortCycleWorkOrder");
            Delete.Column("InternalLeadPipingIndicator").FromTable("ShortCycleWorkOrder");
        }
    }
}
