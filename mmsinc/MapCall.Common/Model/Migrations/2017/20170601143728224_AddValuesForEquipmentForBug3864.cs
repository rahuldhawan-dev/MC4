using System;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170601143728224), Tags("Production")]
    public class AddValuesForEquipmentForBug3864 : Migration
    {
        public struct EquipmentTypes
        {
            public const int ENG = 154,
                             GEN = 163,
                             MOT = 183,
                             PMP_CENT = 190,
                             PMP_GRND = 191,
                             PMP_PD = 192,
                             RTU_PLC = 209,
                             TNK_CHEM = 220,
                             TNK_FUEL = 221,
                             TNK_PVAC = 222,
                             TNK_WNON = 223,
                             TNK_WPOT = 224,
                             TNK_WSTE = 225;
        }

        public struct FieldTypes
        {
            public const int STRING = 1, NUMBER = 2, CURRENCY = 3, DATE = 4, DROP_DOWN = 5;
        }

        public override void Up()
        {
            Alter.Table("Equipment").AddColumn("CriticalNotes").AsAnsiString(150).Nullable();
            //Alter.Table("EquipmentCharacteristicDropDownValues").AddColumn("SAPCode").AsAnsiString(30).Nullable();
            Alter.Table("Equipment").AddColumn("DateInstalled").AsDateTime().Nullable();
            Execute.Sql("update Equipment SET YearInstalled = 2009 where YearInstalled = 22009;" +
                        "update Equipment SET YearInstalled = 2014 where YearInstalled = 20104;" +
                        "UPDATE Equipment Set DateInstalled = '1/1/' + cast(YearInstalled as varchar) WHERE isNull(YearInstalled, 0) <> 0");
            Delete.Column("YearInstalled").FromTable("Equipment");

            /*
             * For each of the 14 types we need to determine which properties for them are missing.
             * For existing properties we need to determine if any drop down values are missing and add them
             * All drop down values will need an SAPCode identified and set.
             * If any properties are missing they need to be added. If they are drop downs the values need to be
             * added with SAPCodes.
             * C:\Solutions\sql\2017-05-24-SAPEquipmentCharacteristicsAndValues.sql
             * 
             */
            Action<string, short, short> addCharacteristic =
                (string fieldName, short sapEquipmentTypeId, short fieldTypeId) => {
                    Execute.Sql(
                        $"IF NOT EXISTS(SELECT 1 FROM EquipmentCharacteristicFields WHERE FieldName='{fieldName}' AND SAPEquipmentTypeID = {sapEquipmentTypeId} AND FieldTypeID = {fieldTypeId} ) " +
                        $" INSERT INTO EquipmentCharacteristicFields(FieldName, SAPEquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic)" +
                        $" VALUES('{fieldName}', {sapEquipmentTypeId}, {fieldTypeId}, 0, 1)");
                };

            Action<string, short, short, string[]> addDropDownCharacteristic =
                (string fieldName, short sapEquipmentTypeId, short fieldTypeId, string[] values) => {
                    var sql =
                        $"IF NOT EXISTS(SELECT 1 FROM EquipmentCharacteristicFields WHERE FieldName='{fieldName}' AND SAPEquipmentTypeID = {sapEquipmentTypeId} AND FieldTypeID = {fieldTypeId} ) " +
                        $"BEGIN " +
                        $"DECLARE @id int " +
                        $" INSERT INTO EquipmentCharacteristicFields(FieldName, SAPEquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic)" +
                        $" VALUES('{fieldName}', {sapEquipmentTypeId}, {fieldTypeId}, 0, 1);" +
                        $"SELECT @id = @@IDENTITY";
                    foreach (var value in values)
                    {
                        sql += $" INSERT INTO EquipmentCharacteristicDropDownValues VALUES('{value}', @id);";
                    }

                    sql += " END";
                    Execute.Sql(sql);
                };

            Action<string, short, short, string> addDropDownCharacteristicValue =
                (string fieldName, short sapEquipmentTypeId, short fieldTypeId, string value) => {
                    var sql =
                        $"IF NOT EXISTS(SELECT 1 FROM EquipmentCharacteristicDropDownValues WHERE Value = '{value}' AND FieldId = (SELECT Id FROM EquipmentCharacteristicFields WHERE FieldName='{fieldName}' AND SAPEquipmentTypeID = {sapEquipmentTypeId} AND FieldTypeID = {fieldTypeId}) ) " +
                        $" INSERT INTO EquipmentCharacteristicDropDownValues SELECT '{value}', Id FROM EquipmentCharacteristicFields WHERE FieldName='{fieldName}' AND SAPEquipmentTypeID = {sapEquipmentTypeId} AND FieldTypeID = {fieldTypeId};";
                    Execute.Sql(sql);
                };

            #region ENG

            //APPLICATION_ENG - exists
            //ENG_FUEL_UOM - exists
            //ENG_TYP -- exists
            //HP_RATING - exists
            //SELF_STARTING - exists
            //OWNED_BY -- add
            addCharacteristic("OWNED_BY", EquipmentTypes.ENG, FieldTypes.STRING);
            //ENG_MAX_FUEL -- add
            addCharacteristic("ENG_MAX_FUEL", EquipmentTypes.ENG, FieldTypes.NUMBER);
            //ENG_CYLINDERS -- add
            addCharacteristic("ENG_CYLINDERS", EquipmentTypes.ENG, FieldTypes.NUMBER);
            //TNK_VOLUME -- add
            addCharacteristic("TNK_VOLUME", EquipmentTypes.ENG, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.ENG, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region GEN

            //APPLICATION_GEN - 
            //GEN_LOAN - 
            //GEN_PORTABLE - 
            //GEN_TYP - 
            //GEN_VOLTAGE_TP - 
            //PHASES - 
            //RPM_RATING - 
            //SELF_STARTING - 
            //VOLT_RATING - 

            addDropDownCharacteristic("FUEL_TYPE", EquipmentTypes.GEN, FieldTypes.DROP_DOWN,
                new[] {"TAXABLE", "UNLEADED"});
            addDropDownCharacteristic("APPLICATION_TNK-FUEL", EquipmentTypes.GEN, FieldTypes.DROP_DOWN,
                new[] {"GASOLINE", "DIESEL", "LPG", "FUEL OIL", "NG", "OTHER"});
            addDropDownCharacteristic("FUEL_TNK", EquipmentTypes.GEN, FieldTypes.DROP_DOWN,
                new[] {"N/A", "INTEGRAL", "SEPARATE"});

            //OWNED_BY -- add
            addCharacteristic("OWNED_BY", EquipmentTypes.GEN, FieldTypes.STRING);
            //GEN_KW -- add
            addCharacteristic("GEN_KW", EquipmentTypes.GEN, FieldTypes.NUMBER);
            //GEN_CURRENT -- add
            addCharacteristic("GEN_CURRENT", EquipmentTypes.GEN, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS -- add
            //SPECIAL_MAINT_NOTES_DIST -- add
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.GEN, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region MOT

            #region Existing

            //APPLICATION_MOT
            //DUTY_CYCLE
            //HP_RATING
            //INSULATION_CLASS
            //MOT_ANTI_REVERSE
            //MOT_BEARING_TP_COUP_END
            //MOT_BEARING_TP_FREE_END
            //MOT_CODE
            //MOT_COUPLING_TP
            //MOT_ENCLOSURE_TP
            //MOT_HOLLOW_SHAFT
            //MOT_INVERTER_DUTY
            //MOT_LUBE_TP_COUP_END
            //MOT_LUBE_TP_FREE_END
            //MOT_NAMEPLATE_DESIGN
            //MOT_SERVICE_FACTOR
            //MOT_TYP
            //MOT_VOLTAGE_RUNNING
            //ORIENTATION
            //ROTATION_DIRECTION
            //RPM_RATING
            //VOLT_RATING

            #endregion

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.MOT, FieldTypes.STRING);
            //FULL_LOAD_AMPS
            addCharacteristic("FULL_LOAD_AMPS", EquipmentTypes.MOT, FieldTypes.STRING);
            //RPM_OPERATING
            addCharacteristic("RPM_OPERATING", EquipmentTypes.MOT, FieldTypes.STRING);
            //MOT_FRAME_TP
            addCharacteristic("MOT_FRAME_TP", EquipmentTypes.MOT, FieldTypes.STRING);
            //TEMPERATURE_RISE
            addCharacteristic("TEMPERATURE_RISE", EquipmentTypes.MOT, FieldTypes.NUMBER);
            //MOT_EXCITATION_VOLTAGE
            addCharacteristic("MOT_EXCITATION_VOLTAGE", EquipmentTypes.MOT, FieldTypes.STRING);
            //MOT_BEARING_FREE_END
            addCharacteristic("MOT_BEARING_FREE_END", EquipmentTypes.MOT, FieldTypes.STRING);
            //MOT_BEARING_COUP_END
            addCharacteristic("MOT_BEARING_COUP_END", EquipmentTypes.MOT, FieldTypes.STRING);
            //MOT_CATALOG_NUMBER
            addCharacteristic("MOT_CATALOG_NUMBER", EquipmentTypes.MOT, FieldTypes.STRING);
            //MOT_LUBE_TP_COUP_END
            //addCharacteristic("MOT_LUBE_TP_COUP_END", EquipmentTypes.MOT, FieldTypes.STRING);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.MOT, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region PMP - CENT

            #region Existing

            //APPLICATION_PMP - CENT
            //BHP_RATING
            //FLOW_UOM
            //LUBE_TP
            //LUBE_TP_2
            //ORIENTATION
            //PMP_BEARING_TP_COUP_END
            //PMP_BEARING_TP_FREE_END
            //PMP_DISCHARGE_SIZE
            //PMP_IMPELLER_SIZE
            //PMP_MATERIAL
            //PMP_SEAL_TP
            //PMP_STAGES
            //PMP - CENT_TYP
            //ROTATION_DIRECTION
            //RPM_RATING

            #endregion

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //FLOW_RATING
            addCharacteristic("FLOW_RATING", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            //PMP_TDH_RATING
            addCharacteristic("PMP_TDH_RATING", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //PMP_INLET_SIZE
            //addCharacteristic("PMP_INLET_SIZE", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //FLOW_MAXIMUM
            addCharacteristic("FLOW_MAXIMUM", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            //PMP_IMPELLER_MATL
            addCharacteristic("PMP_IMPELLER_SIZE", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            //PMP_EFICIENCY
            addCharacteristic("PMP_EFICIENCY", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            //PMP_SHUT_OFF_HEAD
            addCharacteristic("PMP_SHUT_OFF_HEAD", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //PMP_NPSH_RATING
            addCharacteristic("PMP_NPSH_RATING", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //BEARINGNUM_COUP_END
            addCharacteristic("BEARINGNUM_COUP_END", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //BEARINGNUM_FREE_END
            addCharacteristic("BEARINGNUM_FREE_END", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.PMP_CENT, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region PMP - GRND

            #region existing

            //APPLICATION_PMP - GRND
            //BHP_RATING
            //FLOW_UOM
            //LUBE_TP
            //LUBE_TP_2
            //ORIENTATION
            //PMP_BEARING_TP_COUP_END
            //PMP_BEARING_TP_FREE_END
            //PMP_DISCHARGE_SIZE
            //PMP_IMPELLER_SIZE
            //PMP_INLET_SIZE
            //PMP_MATERIAL
            //PMP_SEAL_TP
            //PMP_STAGES
            //PMP - GRND_TYP
            //ROTATION_DIRECTION
            //RPM_RATING

            #endregion

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //FLOW_RATING
            addCharacteristic("FLOW_RATING", EquipmentTypes.PMP_GRND, FieldTypes.NUMBER);
            //PMP_TDH_RATING
            addCharacteristic("PMP_TDH_RATING", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //FLOW_MAXIMUM
            addCharacteristic("FLOW_MAXIMUM", EquipmentTypes.PMP_GRND, FieldTypes.NUMBER);
            //PMP_IMPELLER_MATL
            //addCharacteristic("PMP_IMPELLER_MATL", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //PMP_EFICIENCY
            addCharacteristic("PMP_EFICIENCY", EquipmentTypes.PMP_GRND, FieldTypes.NUMBER);
            //PMP_SHUT_OFF_HEAD
            addCharacteristic("PMP_SHUT_OFF_HEAD", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //PMP_NPSH_RATING
            addCharacteristic("PMP_NPSH_RATING", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //BEARINGNUM_COUP_END
            addCharacteristic("BEARINGNUM_COUP_END", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //BEARINGNUM_FREE_END
            addCharacteristic("BEARINGNUM_FREE_END", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.PMP_GRND, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region PMP - PD

            #region existing

            //APPLICATION_PMP - PD
            //BHP_RATING
            //FLOW_UOM
            //LUBE_TP
            //LUBE_TP_2
            //ORIENTATION
            //PMP_BEARING_TP_COUP_END
            //PMP_BEARING_TP_FREE_END
            //PMP_DISCHARGE_SIZE
            //PMP_IMPELLER_MATL
            //PMP_INLET_SIZE
            //PMP_MATERIAL
            //PMP_SEAL_TP
            //PMP_STAGES
            //PMP - PD_TYP
            //ROTATION_DIRECTION
            //RPM_RATING

            #endregion

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            //FLOW_RATING
            addCharacteristic("FLOW_RATING", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            //PMP_TDH_RATING
            addCharacteristic("PMP_TDH_RATING", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            //FLOW_MAXIMUM
            addCharacteristic("FLOW_MAXIMUM", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            //PMP_IMPELLER_SIZE
            addCharacteristic("PMP_IMPELLER_SIZE", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            //PMP_EFICIENCY
            addCharacteristic("PMP_EFICIENCY", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            //PMP_SHUT_OFF_HEAD
            addCharacteristic("PMP_SHUT_OFF_HEAD", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            //PMP_NPSH_RATING
            addCharacteristic("PMP_NPSH_RATING", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            //BEARINGNUM_COUP_END
            addCharacteristic("BEARINGNUM_COUP_END", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            //BEARINGNUM_FREE_END
            addCharacteristic("BEARINGNUM_FREE_END", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.PMP_PD, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region RTU - PLC

            #region existing

            //APPLICATION_RTU - PLC
            //COMM1_DEVICE
            //COMM1_TP
            //COMM2_DEVICE
            //COMM2_TP
            //COMM3_DEVICE
            //COMM3_TP
            //COMM4_DEVICE
            //COMM4_TP
            //COMM5_DEVICE
            //COMM5_TP
            //COMM6_DEVICE
            //COMM6_TP
            //END_NODE
            //REMOTE_IO
            //RTU - PLC_TYP
            //STANDBY_POWER_TP
            //VOLT_RATING_RTU - PLC

            #endregion

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.RTU_PLC, FieldTypes.STRING);
            //CPU_BATTERY
            addCharacteristic("CPU_BATTERY", EquipmentTypes.RTU_PLC, FieldTypes.STRING);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.RTU_PLC, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region TNK - CHEM

            //APPLICATION_TNK - CHEM
            //LOCATION
            //TNK_AUTO_REFILL
            //TNK_MATERIAL
            //TNK_PRESSURE_RATING
            //TNK_STATE_INSPECTION_REQ
            //TNK - CHEM_TYP
            //UNDERGROUND

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.TNK_CHEM, FieldTypes.STRING);
            //TNK_VOLUME
            addCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_CHEM, FieldTypes.NUMBER);
            //TNK_SIDE_LENGTH
            addCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_CHEM, FieldTypes.NUMBER);
            //TNK_DIAMETER
            addCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_CHEM, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_CHEM, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region TNK - FUEL

            //APPLICATION_TNK - FUEL
            //LOCATION
            //TNK_AUTO_REFILL
            //TNK_MATERIAL
            //TNK_PRESSURE_RATING
            //TNK_STATE_INSPECTION_REQ
            //TNK - FUEL_TYP
            //UNDERGROUND

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.TNK_FUEL, FieldTypes.STRING);
            //TNK_VOLUME
            addCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_FUEL, FieldTypes.NUMBER);
            //TNK_SIDE_LENGTH
            addCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_FUEL, FieldTypes.NUMBER);
            //TNK_DIAMETER
            addCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_FUEL, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_FUEL, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });
            addDropDownCharacteristicValue("APPLICATION_TNK-FUEL", EquipmentTypes.TNK_FUEL, FieldTypes.DROP_DOWN, "NG");
            addDropDownCharacteristicValue("APPLICATION_TNK-FUEL", EquipmentTypes.TNK_FUEL, FieldTypes.DROP_DOWN,
                "OTHER");

            #endregion

            #region TNK - PVAC

            //APPLICATION_TNK - PVAC
            //LOCATION
            //TNK_AUTO_REFILL
            //TNK_MATERIAL
            //TNK_PRESSURE_RATING
            //TNK_STATE_INSPECTION_REQ
            //TNK - PVAC_TYP
            //UNDERGROUND

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.TNK_PVAC, FieldTypes.STRING);
            //TNK_VOLUME
            addCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_PVAC, FieldTypes.NUMBER);
            //TNK_SIDE_LENGTH
            addCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_PVAC, FieldTypes.NUMBER);
            //TNK_DIAMETER
            addCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_PVAC, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_PVAC, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region TNK - WNON

            //APPLICATION_TNK - WNON
            //LOCATION
            //TNK_AUTO_REFILL
            //TNK_MATERIAL
            //TNK_PRESSURE_RATING
            //TNK_STATE_INSPECTION_REQ
            //TNK - WNON_TYP
            //UNDERGROUND

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.TNK_WNON, FieldTypes.STRING);
            //TNK_VOLUME
            addCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_WNON, FieldTypes.NUMBER);
            //TNK_SIDE_LENGTH
            addCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_WNON, FieldTypes.NUMBER);
            //TNK_DIAMETER
            addCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_WNON, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_WNON, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion

            #region TNK - WPOT

            //APPLICATION_TNK - WPOT
            //LOCATION
            //TNK_AUTO_REFILL
            //TNK_MATERIAL
            //TNK_PRESSURE_RATING
            //TNK_STATE_INSPECTION_REQ
            //TNK - WPOT_TYP
            //UNDERGROUND

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.TNK_WPOT, FieldTypes.STRING);
            //TNK_VOLUME
            addCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_WPOT, FieldTypes.NUMBER);
            //TNK_SIDE_LENGTH
            addCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_WPOT, FieldTypes.NUMBER);
            //TNK_DIAMETER
            addCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_WPOT, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_WPOT, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });
            Execute.Sql("UPDATE EquipmentCharacteristicFields SET FieldTypeId = 5 WHERE Id = 828");
            addDropDownCharacteristicValue("APPLICATION_TNK-WPOT", EquipmentTypes.TNK_WPOT, FieldTypes.DROP_DOWN,
                "WATER");

            #endregion

            #region TNK - WSTE

            //APPLICATION_TNK - WSTE
            //LOCATION
            //TNK_AUTO_REFILL
            //TNK_MATERIAL
            //TNK_PRESSURE_RATING
            //TNK_STATE_INSPECTION_REQ
            //TNK - WSTE_TYP
            //UNDERGROUND

            //OWNED_BY
            addCharacteristic("OWNED_BY", EquipmentTypes.TNK_WSTE, FieldTypes.STRING);
            //TNK_VOLUME
            addCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_WSTE, FieldTypes.NUMBER);
            //TNK_SIDE_LENGTH
            addCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_WSTE, FieldTypes.NUMBER);
            //TNK_DIAMETER
            addCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_WSTE, FieldTypes.NUMBER);
            //SPECIAL_MAINT_NOTES_DETAILS
            //SPECIAL_MAINT_NOTES_DIST
            addDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_WSTE, FieldTypes.DROP_DOWN, new[] {
                "ALERT CUSTOMERS BEFORE", "SPECIAL FLUSHING CONCERN", "VALVE CLOSED/PLUGGED", "OPEN SLOWLY",
                "CLOSE SLOWLY", "NO VEHICLE ACCESS", "BACK-SIDE YARD", "PUMPOUT REQUIRED", "OTHER"
            });

            #endregion
        }

        public override void Down()
        {
            Delete.Column("CriticalNotes").FromTable("Equipment");
            Alter.Table("Equipment").AddColumn("YearInstalled").AsInt32().Nullable();
            Execute.Sql("UPDATE Equipment Set YearInstalled = YEAR(DateInstalled) WHERE DateInstalled is not null");
            Delete.Column("DateInstalled").FromTable("Equipment");

            Action<string, short, short> removeCharacteristic =
                (string fieldName, short sapEquipmentTypeId, short fieldTypeId) => {
                    Execute.Sql(
                        $"IF NOT EXISTS(select 1 from EquipmentCharacteristics where FieldId = (select Id from EquipmentCharacteristicFields where " +
                        $"FieldName = '{fieldName}' AND SAPEquipmentTypeId = {sapEquipmentTypeId} AND FieldTypeId = {fieldTypeId}))" +
                        $" DELETE EquipmentCharacteristicFields WHERE FieldName = '{fieldName}' AND SAPEquipmentTypeId = {sapEquipmentTypeId} AND FieldTypeId = {fieldTypeId}");
                };

            Action<string, short, short> removeDropDownCharacteristic =
                (string fieldName, short sapEquipmentTypeId, short fieldTypeId) => {
                    var sql =
                        $"IF NOT EXISTS(select 1 from EquipmentCharacteristics where FieldId = (select Id from EquipmentCharacteristicFields where " +
                        $"FieldName = '{fieldName}' AND SAPEquipmentTypeId = {sapEquipmentTypeId} AND FieldTypeId = {fieldTypeId}))" +
                        $" BEGIN " +
                        $" DELETE EquipmentCharacteristicDropDownValues WHERE FieldId = (SELECT Id from EquipmentCharacteristicFields WHERE FieldName = '{fieldName}' AND SAPEquipmentTypeId = {sapEquipmentTypeId} AND FieldTypeId = {fieldTypeId});" +
                        $" DELETE EquipmentCharacteristicFields WHERE FieldName = '{fieldName}' AND SAPEquipmentTypeId = {sapEquipmentTypeId} AND FieldTypeId = {fieldTypeId};" +
                        $" END";
                    Execute.Sql(sql);
                };

            Action<string, short, short, string> removeDropDownCharacteristicValue =
                (string fieldName, short sapEquipmentTypeId, short fieldTypeId, string value) => {
                    var sql =
                        $"IF NOT EXISTS(select 1 from EquipmentCharacteristics where cast([value] as varchar) = cast((SELECT Id FROM EquipmentCharacteristicDropDownValues WHERE Value = '{value}' AND FieldId = (SELECT Id FROM EquipmentCharacteristicFields WHERE FieldName='{fieldName}' AND SAPEquipmentTypeID = {sapEquipmentTypeId} AND FieldTypeID = {fieldTypeId}) ) as varchar)) " +
                        $" DELETE EquipmentCharacteristicDropDownValues WHERE value = '{value}' AND FieldId = (SELECT Id from EquipmentCharacteristicFields WHERE FieldName = '{fieldName}' AND SAPEquipmentTypeId = {sapEquipmentTypeId} AND FieldTypeId = {fieldTypeId});";
                    Execute.Sql(sql);
                };

            #region ENG

            removeCharacteristic("OWNED_BY", EquipmentTypes.ENG, FieldTypes.STRING);
            removeCharacteristic("ENG_MAX_FUEL", EquipmentTypes.ENG, FieldTypes.NUMBER);
            removeCharacteristic("ENG_CYLINDERS", EquipmentTypes.ENG, FieldTypes.NUMBER);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.ENG, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.ENG, FieldTypes.DROP_DOWN);

            #endregion

            #region GEN

            removeDropDownCharacteristic("FUEL_TYPE", EquipmentTypes.GEN, FieldTypes.DROP_DOWN);
            removeDropDownCharacteristic("APPLICATION_TNK-FUEL", EquipmentTypes.GEN, FieldTypes.DROP_DOWN);
            removeDropDownCharacteristic("FUEL_TNK", EquipmentTypes.GEN, FieldTypes.DROP_DOWN);

            removeCharacteristic("OWNED_BY", EquipmentTypes.GEN, FieldTypes.STRING);
            removeCharacteristic("GEN_KW", EquipmentTypes.GEN, FieldTypes.NUMBER);
            removeCharacteristic("GEN_CURRENT", EquipmentTypes.GEN, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.GEN, FieldTypes.DROP_DOWN);

            #endregion

            #region MOT

            removeCharacteristic("OWNED_BY", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("FULL_LOAD_AMPS", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("RPM_OPERATING", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("MOT_FRAME_TP", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("TEMPERATURE_RISE", EquipmentTypes.MOT, FieldTypes.NUMBER);
            removeCharacteristic("MOT_EXCITATION_VOLTAGE", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("MOT_BEARING_FREE_END", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("MOT_BEARING_COUP_END", EquipmentTypes.MOT, FieldTypes.STRING);
            removeCharacteristic("MOT_CATALOG_NUMBER", EquipmentTypes.MOT, FieldTypes.STRING);
            //removeCharacteristic("MOT_LUBE_TP_COUP_END", EquipmentTypes.MOT, FieldTypes.STRING);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.MOT, FieldTypes.DROP_DOWN);

            #endregion

            #region PMP - CENT

            removeCharacteristic("OWNED_BY", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            removeCharacteristic("FLOW_RATING", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            removeCharacteristic("PMP_TDH_RATING", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            //removeCharacteristic("PMP_INLET_SIZE", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            removeCharacteristic("FLOW_MAXIMUM", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            removeCharacteristic("PMP_IMPELLER_SIZE", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            removeCharacteristic("PMP_EFICIENCY", EquipmentTypes.PMP_CENT, FieldTypes.NUMBER);
            removeCharacteristic("PMP_SHUT_OFF_HEAD", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            removeCharacteristic("PMP_NPSH_RATING", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            removeCharacteristic("BEARINGNUM_COUP_END", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            removeCharacteristic("BEARINGNUM_FREE_END", EquipmentTypes.PMP_CENT, FieldTypes.STRING);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.PMP_CENT, FieldTypes.DROP_DOWN);

            #endregion

            #region PMP - GRND

            removeCharacteristic("OWNED_BY", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeCharacteristic("FLOW_RATING", EquipmentTypes.PMP_GRND, FieldTypes.NUMBER);
            removeCharacteristic("PMP_TDH_RATING", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeCharacteristic("FLOW_MAXIMUM", EquipmentTypes.PMP_GRND, FieldTypes.NUMBER);
            //removeCharacteristic("PMP_IMPELLER_MATL", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeCharacteristic("PMP_EFICIENCY", EquipmentTypes.PMP_GRND, FieldTypes.NUMBER);
            removeCharacteristic("PMP_SHUT_OFF_HEAD", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeCharacteristic("PMP_NPSH_RATING", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeCharacteristic("BEARINGNUM_COUP_END", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeCharacteristic("BEARINGNUM_FREE_END", EquipmentTypes.PMP_GRND, FieldTypes.STRING);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.PMP_GRND, FieldTypes.DROP_DOWN);

            #endregion

            #region PMP - PD

            removeCharacteristic("OWNED_BY", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            removeCharacteristic("FLOW_RATING", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            removeCharacteristic("PMP_TDH_RATING", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            removeCharacteristic("FLOW_MAXIMUM", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            removeCharacteristic("PMP_IMPELLER_SIZE", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            removeCharacteristic("PMP_EFICIENCY", EquipmentTypes.PMP_PD, FieldTypes.NUMBER);
            removeCharacteristic("PMP_SHUT_OFF_HEAD", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            removeCharacteristic("PMP_NPSH_RATING", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            removeCharacteristic("BEARINGNUM_COUP_END", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            removeCharacteristic("BEARINGNUM_FREE_END", EquipmentTypes.PMP_PD, FieldTypes.STRING);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.PMP_PD, FieldTypes.DROP_DOWN);

            #endregion

            #region RTU - PLC

            removeCharacteristic("OWNED_BY", EquipmentTypes.RTU_PLC, FieldTypes.STRING);
            removeCharacteristic("CPU_BATTERY", EquipmentTypes.RTU_PLC, FieldTypes.STRING);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.RTU_PLC, FieldTypes.DROP_DOWN);

            #endregion

            #region TNK - CHEM

            removeCharacteristic("OWNED_BY", EquipmentTypes.TNK_CHEM, FieldTypes.STRING);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_CHEM, FieldTypes.NUMBER);
            removeCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_CHEM, FieldTypes.NUMBER);
            removeCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_CHEM, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_CHEM, FieldTypes.DROP_DOWN);

            #endregion

            #region TNK - FUEL

            removeCharacteristic("OWNED_BY", EquipmentTypes.TNK_FUEL, FieldTypes.STRING);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_FUEL, FieldTypes.NUMBER);
            removeCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_FUEL, FieldTypes.NUMBER);
            removeCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_FUEL, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_FUEL, FieldTypes.DROP_DOWN);
            removeDropDownCharacteristicValue("APPLICATION_TNK-FUEL", EquipmentTypes.TNK_FUEL, FieldTypes.DROP_DOWN,
                "NG");
            removeDropDownCharacteristicValue("APPLICATION_TNK-FUEL", EquipmentTypes.TNK_FUEL, FieldTypes.DROP_DOWN,
                "OTHER");

            #endregion

            #region TNK - PVAC

            removeCharacteristic("OWNED_BY", EquipmentTypes.TNK_PVAC, FieldTypes.STRING);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_PVAC, FieldTypes.NUMBER);
            removeCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_PVAC, FieldTypes.NUMBER);
            removeCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_PVAC, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_PVAC, FieldTypes.DROP_DOWN);

            #endregion

            #region TNK - WNON

            removeCharacteristic("OWNED_BY", EquipmentTypes.TNK_WNON, FieldTypes.STRING);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_WNON, FieldTypes.NUMBER);
            removeCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_WNON, FieldTypes.NUMBER);
            removeCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_WNON, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_WNON, FieldTypes.DROP_DOWN);

            #endregion

            #region TNK - WPOT

            removeCharacteristic("OWNED_BY", EquipmentTypes.TNK_WPOT, FieldTypes.STRING);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_WPOT, FieldTypes.NUMBER);
            removeCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_WPOT, FieldTypes.NUMBER);
            removeCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_WPOT, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_WPOT, FieldTypes.DROP_DOWN);
            //remove ddcv 'WATER'
            Execute.Sql(
                " IF NOT EXISTS (SELECT 1 FROM EquipmentCharacteristics where FieldId = (select Id from EquipmentCharacteristicFields where FieldName = 'APPLICATION_TNK-WPOT' AND SAPEquipmentTypeId = 224 AND FieldTypeId = 5))" +
                " begin" +
                " DELETE EquipmentCharacteristicDropDownValues WHERE Value = 'WATER' and FieldID = (select Id from EquipmentCharacteristicFields where FieldName = 'APPLICATION_TNK-WPOT' AND SAPEquipmentTypeId = 224 AND FieldTypeId = 5)" +
                " UPDATE EquipmentCharacteristicFields SET FieldTypeId = 1 WHERE Id = (select Id from EquipmentCharacteristicFields where FieldName = 'APPLICATION_TNK-WPOT' AND SAPEquipmentTypeId = 224 AND FieldTypeId = 5)" +
                " end");

            #endregion

            #region TNK - WSTE

            removeCharacteristic("OWNED_BY", EquipmentTypes.TNK_WSTE, FieldTypes.STRING);
            removeCharacteristic("TNK_VOLUME", EquipmentTypes.TNK_WSTE, FieldTypes.NUMBER);
            removeCharacteristic("TNK_SIDE_LENGTH", EquipmentTypes.TNK_WSTE, FieldTypes.NUMBER);
            removeCharacteristic("TNK_DIAMETER", EquipmentTypes.TNK_WSTE, FieldTypes.NUMBER);
            removeDropDownCharacteristic("SPECIAL_MAINT_NOTES_DIST", EquipmentTypes.TNK_WSTE, FieldTypes.DROP_DOWN);

            #endregion
        }
    }
}
