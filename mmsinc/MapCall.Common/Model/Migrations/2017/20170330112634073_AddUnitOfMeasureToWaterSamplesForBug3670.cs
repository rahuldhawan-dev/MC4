using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170330112634073), Tags("Production")]
    public class AddUnitOfMeasureToWaterSamplesForBug3670 : Migration
    {
        public const string TABLE_NAME = "UnitsOfWaterSampleMeasure";

        public override void Up()
        {
            Create.LookupTable(TABLE_NAME);

            Insert.IntoTable(TABLE_NAME).Rows(
                new {Description = "%LUM"},
                new {Description = "%PUR"},
                new {Description = "ADMIU"},
                new {Description = "AGGR"},
                new {Description = "C"},
                new {Description = "CFU"},
                new {Description = "CM-1"},
                new {Description = "Color Units"},
                new {Description = "Count"},
                new {Description = "CT"},
                new {Description = "CU"},
                new {Description = "F"},
                new {Description = "FTU"},
                new {Description = "LANG"},
                new {Description = "LBS/CFT"},
                new {Description = "LBS/GAL"},
                new {Description = "MFL"},
                new {Description = "MG/L"},
                new {Description = "MREM"},
                new {Description = "MREMY"},
                new {Description = "NG/L"},
                new {Description = "NM"},
                new {Description = "NTU"},
                new {Description = "OBSVNS"},
                new {Description = "PH"},
                new {Description = "PCI/L"},
                new {Description = "SU"},
                new {Description = "TON"},
                new {Description = "UG/L"},
                new {Description = "UMHO/CM"}
            );

            Alter.Table("WaterSamples").AddForeignKeyColumn("UnitOfMeasureId", TABLE_NAME);

            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "C"})
                  .Where(new {UnitOfMeasure = "Degrees C"});
            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "UG/L"})
                  .Where(new {UnitOfMeasure = "mcg/l"});
            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "UMHO/CM"})
                  .Where(new {UnitOfMeasure = "Micro Ohms (microS/cm)"});
            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "UG/L"})
                  .Where(new {UnitOfMeasure = "micrograms per liter (µg/L)"});
            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "MREMY"})
                  .Where(new {UnitOfMeasure = "mrem/yr"});
            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "UG/L"})
                  .Where(new {UnitOfMeasure = "ppb"});
            Update.Table("WaterConstituents")
                  .Set(new {UnitOfMeasure = "Count"})
                  .Where(new {UnitOfMeasure = "Units"});

            Alter.Table("WaterConstituents").AddForeignKeyColumn("UnitOfMeasureId", TABLE_NAME);

            Execute.Sql(
                $"UPDATE {"WaterConstituents"} SET UnitOfMeasureId = uom.Id FROM {TABLE_NAME} uom WHERE {"WaterConstituents"}.UnitOfMeasure = uom.Description");

            Delete.Column("UnitOfMeasure").FromTable("WaterConstituents");
        }

        public override void Down()
        {
            Alter.Table("WaterConstituents").AddColumn("UnitOfMeasure").AsString(20);

            Execute.Sql(
                $"UPDATE {"WaterConstituents"} SET UnitOfMeasure = uom.Description FROM {TABLE_NAME} uom WHERE {"WaterConstituents"}.UnitOfMeasureId = uom.Id");

            Delete.ForeignKeyColumn("WaterConstituents", "UnitOfMeasureId", TABLE_NAME);
            Delete.ForeignKeyColumn("WaterSamples", "UnitOfMeasureId", TABLE_NAME);

            Delete.Table(TABLE_NAME);
        }
    }
}
