using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190501124520717), Tags("Production")]
    public class AddCodeThingsForProductionWorkOrdersForMC1058 : Migration
    {
        public override void Up()
        {
            Create.LookupTable("ProductionWorkOrderActionCodes");
            Create.LookupTable("ProductionWorkOrderFailureCodes");
            Create.LookupTable("ProductionWorkOrderCauseCodes");

            Insert.IntoTable("ProductionWorkOrderActionCodes")
                  .Rows(new {Description = "CLEAN"},
                       new {Description = "RESET"},
                       new {Description = "REPAIR"},
                       new {Description = "REPLACE"},
                       new {Description = "MODIFY"},
                       new {Description = "ADJUST"},
                       new {Description = "CHECK"},
                       new {Description = "SERVICE"},
                       new {Description = "TEST"},
                       new {Description = "INPSECT"},
                       new {Description = "REBUILD"},
                       new {Description = "COMBINATION"},
                       new {Description = "OTHER"});

            Insert.IntoTable("ProductionWorkOrderFailureCodes")
                  .Rows(new {Description = "BEARING"},
                       new {Description = "CASING"},
                       new {Description = "SEAL"},
                       new {Description = "PACKING"},
                       new {Description = "COUPLING"},
                       new {Description = "IMPELLER"},
                       new {Description = "SHAFT"},
                       new {Description = "ROTOR"},
                       new {Description = "STATOR"},
                       new {Description = "TERMINATIONS"},
                       new {Description = "STRUCTURAL LOOSENESS"},
                       new {Description = "ROTATING LOOSENESS"},
                       new {Description = "FRAME"},
                       new {Description = "DEBRIS"},
                       new {Description = "GASKET"},
                       new {Description = "SEAT "},
                       new {Description = "ACTUATOR"},
                       new {Description = "PNUEMATICS"},
                       new {Description = "HYDRAULIC"},
                       new {Description = "DIRTY"},
                       new {Description = "OUT OF CALIBRATION"},
                       new {Description = "OUT OF RANGE"},
                       new {Description = "HIGH OUTPUT"},
                       new {Description = "LOW OUTPUT"},
                       new {Description = "BLOCKAGE"},
                       new {Description = "CAVITATION"},
                       new {Description = "CLEARANCE"},
                       new {Description = "CONTAMINATION"},
                       new {Description = "CORROSION"},
                       new {Description = "DEFROMATION"},
                       new {Description = "ERSOSION"},
                       new {Description = "FATIGUE"},
                       new {Description = "IMBALANCE"},
                       new {Description = "ALIGNMENT"},
                       new {Description = "IMPROPER FIT"},
                       new {Description = "IMPROPER LUBRICATION"},
                       new {Description = "LEAKAGE"},
                       new {Description = "LOOSENESS"},
                       new {Description = "OVERHEATING"},
                       new {Description = "VIBRATION"},
                       new {Description = "NOISE"},
                       new {Description = "STICKING"},
                       new {Description = "LOCKED UP"},
                       new {Description = "COMM FAILURE"},
                       new {Description = "POWER LOSS"},
                       new {Description = "OVERCURRENT"},
                       new {Description = "UNDER CURRENT"},
                       new {Description = "OVER VOLTAGE"},
                       new {Description = "UNDER VOLTAGE"},
                       new {Description = "TRIPPED"},
                       new {Description = "OPERATOR ERROR"},
                       new {Description = "MAINTENANCE ERROR"},
                       new {Description = "CONTROLS POSITION"},
                       new {Description = "SHORT CIRCUIT "},
                       new {Description = "GROUND FAULT"},
                       new {Description = "ADJUSTMENT"},
                       new {Description = "FLOW"},
                       new {Description = "PRESSURE"},
                       new {Description = "SPEED"},
                       new {Description = "LEVEL"},
                       new {Description = "FLUID LEVEL"},
                       new {Description = "PESTS"},
                       new {Description = "COMPONENT DEFECT"},
                       new {Description = "UNKNOWN"},
                       new {Description = "COMBINATION"});

            Insert.IntoTable("ProductionWorkOrderCauseCodes")
                  .Rows(new {Description = "AGING COMPONENTS"},
                       new {Description = "IMPROPER CAPACITY"},
                       new {Description = "DESIGN FLAW"},
                       new {Description = "DEFECTIVE MATERIAL"},
                       new {Description = "OPERATOR ERROR"},
                       new {Description = "MAINTENANCE ERROR"},
                       new {Description = "NORMAL WEAR & TEAR"},
                       new {Description = "POWER FAILURE"},
                       new {Description = "IMPROPER PM CYCLE"},
                       new {Description = "INCORRECT PROCEDURES"},
                       new {Description = "POOR HOUSEKEEPING"},
                       new {Description = "ABNORMAL CONDITIONS"},
                       new {Description = "INSTALLATION ERROR"},
                       new {Description = "FABRICATION ERROR"},
                       new {Description = "IMPROPER TRAINING"},
                       new {Description = "ABUSE"},
                       new {Description = "LACK OF MAINTENANCE"},
                       new {Description = "UNKOWN"},
                       new {Description = "OTHER"});

            Alter.Table("ProductionWorkOrders")
                 .AddForeignKeyColumn("ActionCodeId", "ProductionWorkOrderActionCodes")
                 .AddForeignKeyColumn("FailureCodeId", "ProductionWorkOrderFailureCodes")
                 .AddForeignKeyColumn("CauseCodeId", "ProductionWorkOrderCauseCodes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ProductionWorkOrders", "ActionCodeId", "ProductionWorkOrderActionCodes");
            Delete.ForeignKeyColumn("ProductionWorkOrders", "FailureCodeId", "ProductionWorkOrderFailureCodes");
            Delete.ForeignKeyColumn("ProductionWorkOrders", "CauseCodeId", "ProductionWorkOrderCauseCodes");

            Delete.Table("ProductionWorkOrderActionCodes");
            Delete.Table("ProductionWorkOrderFailureCodes");
            Delete.Table("ProductionWorkOrderCauseCodes");
        }
    }
}
