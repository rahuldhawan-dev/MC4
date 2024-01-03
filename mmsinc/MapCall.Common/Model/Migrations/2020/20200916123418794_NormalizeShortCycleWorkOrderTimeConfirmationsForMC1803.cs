using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418794), Tags("Production")]
    public class NormalizeShortCycleWorkOrderTimeConfirmationsForMC1803 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderTimeConfirmations")
                 .AlterColumn("OperationId").AsInt32().Nullable();

            this.NormalizeToExistingTable("ShortCycleWorkOrderTimeConfirmations", "WorkCenter", "ShortCycleWorkCenters",
                newColumn: "WorkCenterId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderTimeConfirmations", "UnitOfMeasure",
                "ShortCycleWorkOrderTimeConfirmationUnitsOfMeasure", 3, newColumnName: "UnitOfMeasureId");
        }

        public override void Down()
        {
            Alter.Table("ShortCycleWorkOrderTimeConfirmations")
                 .AlterColumn("OperationId").AsString(4).Nullable();

            this.DeNormalizeFromExistingTable("ShortCycleWorkOrderTimeConfirmations", "WorkCenter",
                "ShortCycleWorkCenters",
                newColumn: "WorkCenterId", stringLength: 8);
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderTimeConfirmations", "UnitOfMeasure",
                "ShortCycleWorkOrderTimeConfirmationUnitsOfMeasure", 3, newColumnName: "UnitOfMeasureId");
        }
    }
}
