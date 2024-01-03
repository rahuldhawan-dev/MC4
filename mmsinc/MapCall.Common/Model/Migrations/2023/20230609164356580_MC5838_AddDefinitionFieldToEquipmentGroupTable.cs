using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230609164356580), Tags("Production")]
    public class MC5838_AddDefinitionFieldToEquipmentGroupTable : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentGroups").AddColumn("Definition").AsString(500).Nullable();
            Execute.Sql(@"update [EquipmentGroups] set [Definition] = 'Buildings include treatment and administrative structures, plus components of the buildings like HVAC, Boilers, water heaters, cooling towers, and elevators.' where [Code] = 'B'
                        update [EquipmentGroups] set [Definition] = 'Equipment used to feed chemicals including gas, dry and liquid forms. Also includes chemical generators, scrubbers, piping, and secondary containment allocated to the feed system.' where [Code] = 'C'
                        update [EquipmentGroups] set [Definition] = 'Equipment used to distribute electricity including adjustable speed drives, batteries, battery chargers, cathodic protection, motor contactors, emergency generators, motors, motor starters, operator computer terminals, phase converters, power breakers, power conditioners, power disconnects, power feeder cable, power monitors, panels, relays, surge protection, transfer switch, UPS and transformers.' where [Code] = 'E'
                        update [EquipmentGroups] set [Definition] = 'Meters used in the treatment process including system delivery meters' where [Code] = 'FM'
                        update [EquipmentGroups] set [Definition] = 'Equipment owned or maintained by the company to suppress fires' where [Code] = 'H'
                        update [EquipmentGroups] set [Definition] = 'Equipment packaged as a kit for operational activities lik a chlorine tank emergency kit' where [Code] = 'K'
                        update [EquipmentGroups] set [Definition] = 'Electrical, pneumatic or hydraulic equipment used in the treatment process including blowers, air compressors, conveyors, engines, gearbox, grinders, hoists, mixers, and centrifugal, grinder and positive displacement pumps.' where [Code] = 'M'
                        update [EquipmentGroups] set [Definition] = 'Equipment used to protect employee or public health including defibrillators, emergency lights, eye wash stations, fire alarms, fire suppression, fire extinguishers, arch flash protection, fall protection, floatation devices, respirators, gas detectors, safety showers and security systems.' where [Code] = 'S'
                        update [EquipmentGroups] set [Definition] = 'Equipment used to store liquids or gases including chemicals, fuel, air/vacuum, potable water, non-potable water and waste water.' where [Code] = 'T'
                        update [EquipmentGroups] set [Definition] = 'Equipment used in the water/wastewater treatment process including screens, filters, clarifiers, aerators, contactors, air strippers, & UV sanitizers' where [Code] = 'TR'
                        update [EquipmentGroups] set [Definition] = 'Equipment used to isolate hydraulic, pneumatic and liquids in the treatment and distribution process including plant valves, street valves and blow off valves.' where [Code] = 'V'
                        update [EquipmentGroups] set [Definition] = 'Fresh water, aquifer recharge, slant and desalination wells used for the production of drinking water' where [Code] = 'W'
            ");
        }

        public override void Down()
        {
            Delete.Column("Definition").FromTable("EquipmentGroups");
        }
    }
}

