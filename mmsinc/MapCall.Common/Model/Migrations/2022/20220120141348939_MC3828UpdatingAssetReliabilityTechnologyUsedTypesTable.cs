using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220120141348939), Tags("Production")]
    public class MC3828UpdatingAssetReliabilityTechnologyUsedTypesTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                update AssetReliabilityTechnologyUsedTypes set Description='Electrical Testing' where id = 8;
                update AssetReliabilityTechnologyUsedTypes set Description='Infrared Thermography' where id = 1;
                update AssetReliabilityTechnologyUsedTypes set Description='Motor Winding Analysis/Insulation Resistance' where id = 3;
                update AssetReliabilityTechnologyUsedTypes set Description='Protective Relay Testing' where id = 12;
                update AssetReliabilityTechnologyUsedTypes set Description='Airborne Ultrasound' where id = 5;
                update AssetReliabilityTechnologyUsedTypes set Description='Vibration Analysis' where id = 2;
                update AssetReliabilityTechnologyUsedTypes set Description='Visual Inspection' where id = 4;
                update AssetReliabilityTechnologyUsedTypes set Description='Wire to Water/Pump Performance' where id = 9; 
                update AssetReliabilities set AssetReliabilityTechnologyUsedTypeId = 13 where AssetReliabilityTechnologyUsedTypeId = 10;
                delete from AssetReliabilityTechnologyUsedTypes where id = 10; 
            ");
            Execute.Sql("SET IDENTITY_INSERT [dbo].[AssetReliabilityTechnologyUsedTypes] ON");
            Insert.IntoTable("AssetReliabilityTechnologyUsedTypes").Row(new { id = 14, Description = "Battery Testing" });
            Insert.IntoTable("AssetReliabilityTechnologyUsedTypes").Row(new { id = 15, Description = "Dynamic Motor Testing/ESA" });
            Insert.IntoTable("AssetReliabilityTechnologyUsedTypes").Row(new { id = 16, Description = "Micro-Ohmmeter Testing" });
            Execute.Sql("SET IDENTITY_INSERT [dbo].[AssetReliabilityTechnologyUsedTypes] OFF");
        }

        public override void Down()
        {
            Execute.Sql(@"
                update AssetReliabilityTechnologyUsedTypes set Description='Overload Testing' where id = 8;
                update AssetReliabilityTechnologyUsedTypes set Description='IR' where id = 1;
                update AssetReliabilityTechnologyUsedTypes set Description='MWA' where id = 3;
                update AssetReliabilityTechnologyUsedTypes set Description='Relay Testing' where id = 12;
                update AssetReliabilityTechnologyUsedTypes set Description='Ultrasound' where id = 5;
                update AssetReliabilityTechnologyUsedTypes set Description='Vibration' where id = 2;
                update AssetReliabilityTechnologyUsedTypes set Description='Visual' where id = 4;
                update AssetReliabilityTechnologyUsedTypes set Description='W2W' where id = 9;
                delete from AssetReliabilityTechnologyUsedTypes where Description='Battery Testing';
                delete from AssetReliabilityTechnologyUsedTypes where Description='Dynamic Motor Testing/ESA';
                delete from AssetReliabilityTechnologyUsedTypes where Description='Micro-Ohmmeter Testing';
            ");
        }
    }
}

