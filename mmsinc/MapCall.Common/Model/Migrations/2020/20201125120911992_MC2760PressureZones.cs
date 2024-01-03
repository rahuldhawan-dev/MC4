using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201125120911992), Tags("Production")]
    public class MC2760PressureZones : Migration
    {
        public const string TABLE_NAME = "PublicWaterSupplyPressureZones";

        public override void Up()
        {
            Create
               .Table(TABLE_NAME)
               .WithIdentityColumn()
               .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies", nullable: false)
               .WithColumn("Name")
               .AsAnsiString(50)
               .NotNullable()
               .WithColumn("HydraulicGradientMin")
               .AsInt32()
               .NotNullable()
               .WithColumn("HydraulicGradientMax")
               .AsInt32()
               .NotNullable()
               .WithColumn("PressureMin")
               .AsInt32()
               .NotNullable()
               .WithColumn("PressureMax")
               .AsInt32()
               .NotNullable();

            this.AddDataType(TABLE_NAME);

            Alter
               .Table("PublicWaterSupplies")
               .AddForeignKeyColumn("CurrentPublicWaterSupplyFirmCapacityId", "PublicWaterSupplyFirmCapacities");

            // This update statement will associate the new CurrentPublicWaterSupplyFirmCapacityId value to the 
            // most recently updated Firm Capacity for a given Public Water Supply.
            Execute.Sql(@"
with PwsToLatestFcMap as 
(
    select PublicWaterSupplyId
         , Id [PublicWaterSupplyFirmCapacityId]
         , row_number() over 
         (
           partition by PublicWaterSupplyId 
           order by DateUpdated desc
         ) as RowNumber
      from PublicWaterSupplyFirmCapacities
)

update PublicWaterSupplies 
   set CurrentPublicWaterSupplyFirmCapacityId = PwsToLatestFcMap.PublicWaterSupplyFirmCapacityId
  from PwsToLatestFcMap
  join PublicWaterSupplies pws
    on PwsToLatestFcMap.PublicWaterSupplyId = pws.Id
   and PwsToLatestFcMap.RowNumber = 1
");
        }

        public override void Down()
        {
            Delete
               .Table(TABLE_NAME);

            this.RemoveDataType(TABLE_NAME);

            Delete
               .ForeignKeyColumn("PublicWaterSupplies", "CurrentPublicWaterSupplyFirmCapacityId",
                    "PublicWaterSupplyFirmCapacities");
        }
    }
}
