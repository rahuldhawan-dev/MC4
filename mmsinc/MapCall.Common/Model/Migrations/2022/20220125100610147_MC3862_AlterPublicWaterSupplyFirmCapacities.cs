using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220125100610147), Tags("Production")]
    public class MC3862_AlterPublicWaterSupplyFirmCapacities : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplyFirmCapacities")
                 .AddColumn("FirmCapacityMultiplier")
                 .AsDecimal(5, 4)
                 .NotNullable()
                 .SetExistingRowsTo(0);
        }

        public override void Down() => Delete.Column("FirmCapacityMultiplier").FromTable("PublicWaterSupplyFirmCapacities");
    }
}

