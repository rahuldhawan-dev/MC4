using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200717095525814), Tags("Production")]
    public class AddAmountOfDebrisGritToSewerManholeInspectionsForMC2398 : Migration
    {
        public override void Up()
        {
            Alter.Table("SewerManholeInspections")
                 .AddColumn("AmountOfDebrisGritCubicFeet").AsDecimal(7, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column("AmountOfDebrisGritCubicFeet").FromTable("SewerManholeInspections");
        }
    }
}
