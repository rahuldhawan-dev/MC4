using System;

namespace MapCallImporter.SampleValues
{
    public static class EquipmentCharacteristicFields
    {
        public static string GetInsertQuery(string equipmentType)
        {
            switch (equipmentType)
            {
                #region ADJUSTABLE SPEED DRIVE

                case "ADJUSTABLE SPEED DRIVE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (811, 'AMP_RATING', 121, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (852, 'HP_RATING', 121, 2, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1260, 'PULSE_TP', 121, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1461, 'VOLT_RATING', 121, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1462, 'ADJSPD_TYP', 121, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1691, 'FULL_LOAD_AMPS', 121, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1692, 'SPECIAL_MAINT_NOTES', 121, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1708, 'Owned By', 121, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2008, 'NARUC_MAINTENANCE_ACCOUNT', 121, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2009, 'NARUC_Operations_Account', 121, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2274, 'NARUC_SPECIAL_MAINT_NOTES', 121, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2275, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 121, 1, 0, 1, 1, 12);
";

                #endregion

                #region AERATOR

                case "AERATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1045, 'DIFFUSER_MATERIAL', 228, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1168, 'MEMBRANE_MATERIAL', 228, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1391, 'TRT-AER_TYP', 228, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2239, 'NARUC_MAINTENANCE_ACCOUNT', 228, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2240, 'NARUC_OPERATIONS_ACCOUNT', 228, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2485, 'NARUC_SPECIAL_MAINT_NOTES', 228, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2486, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 228, 1, 0, 1, 1, 7);
";

                #endregion

                #region AIR COMPRESSOR

                case "AIR COMPRESSOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (829, 'BHP_RATING', 145, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (836, 'CAPACITY_UOM_COMP', 145, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (920, 'APPLICATION_COMP', 145, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1083, 'COMP_TYP', 145, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1187, 'STAGES', 145, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1415, 'DRIVE_TP', 145, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1724, 'OWNED_BY', 145, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1725, 'RPM_OPERATING', 145, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1726, 'CAPACITY_RATING', 145, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1727, 'TNK_VOL_(GAL)', 145, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1728, 'MAX_PRESSURE', 145, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1729, 'SPECIAL_MAINT_NOTES', 145, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2056, 'NARUC_MAINTENANCE_ACCOUNT', 145, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2057, 'NARUC_OPERATIONS_ACCOUNT', 145, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2322, 'NARUC_SPECIAL_MAINT_NOTES', 145, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2323, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 145, 1, 0, 1, 1, 16);
";

                #endregion

                #region AIR/ VACUUM TANK

                case "AIR/ VACUUM TANK":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (998, 'TNK_STATE_INSPECTION_REQ', 222, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1063, 'LOCATION', 222, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1081, 'TNK_PRESSURE_RATING', 222, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1119, 'TNK-PVAC_TYP', 222, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1158, 'TNK_AUTO_REFILL', 222, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1313, 'APPLICATION_TNK-PVAC', 222, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1435, 'UNDERGROUND', 222, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1479, 'TNK_MATERIAL', 222, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1659, 'OWNED_BY', 222, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1660, 'TNK_VOLUME', 222, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1661, 'TNK_SIDE_LENGTH', 222, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1662, 'TNK_DIAMETER', 222, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1663, 'SPECIAL_MAINT_NOTES_DIST', 222, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2227, 'NARUC_MAINTENANCE_ACCOUNT', 222, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2228, 'NARUC_OPERATIONS_ACCOUNT', 222, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2473, 'NARUC_SPECIAL_MAINT_NOTES', 222, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2474, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 222, 1, 0, 1, 1, 17);
";

                #endregion

                #region AM WATER NARUC ACCOUNT

                case "AM WATER NARUC ACCOUNT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1137, 'NARUCMAINT', 185, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1316, 'NARUCOPS', 185, 5, 0, 1, 1, 0);
";

                #endregion

                #region AMIDATACOLL

                case "AMIDATACOLL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2511, 'AMIDC_LOC', 241, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2512, 'AMIDC_LOC_ACCESS', 241, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2513, 'AMIDC_POWR', 241, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2514, 'AMIDC_BATT_SIZE', 241, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2515, 'ADMIDC_FREQ', 241, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2516, 'AMIDC_BACKUP_SOURCE', 241, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2517, 'ADMIDC_ANTENNATOP_ELEV', 241, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2518, 'ADMIDC_ANTENNABOT_ELEV', 241, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2519, 'INSTALLATION_WO', 241, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2520, 'OWNED_BY', 241, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2521, 'NARUC_SPECIAL_MAINT_NOTES', 241, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2522, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 241, 1, 0, 1, 1, 12);
";

                #endregion

                #region ARC FLASH PROTECTION

                case "ARC FLASH PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1227, 'RETEST_REQUIRED', 193, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1541, 'PPE-ARC_TYP', 193, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1562, 'PPE_RATING', 193, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1742, 'OWNED_BY', 193, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1743, 'SPECIAL_MAINT_NOTES', 193, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2151, 'NARUC_MAINTENANCE_ACCOUNT', 193, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2152, 'NARUC_OPERATIONS_ACCOUNT', 193, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2416, 'NARUC_SPECIAL_MAINT_NOTES', 193, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2417, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 193, 1, 0, 1, 1, 9);
";

                #endregion

                #region BATTERY

                case "BATTERY":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1105, 'BATT_VOLT_RATING', 123, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1265, 'BATT_CELL_TP', 123, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1284, 'BATT_TYP', 123, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1695, 'OWNED_BY', 123, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1696, '#_BATTERIES_BANK', 123, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1697, 'COLD_CRANKING_AMPS', 123, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1698, 'AMP_HOURS', 123, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1699, 'BAT-CELL_TYPE', 123, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1700, 'SPECIAL_MAINT_NOTES', 123, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2012, 'NARUC_MAINTENANCE_ACCOUNT', 123, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2013, 'NARUC_OPERATIONS_ACCOUNT', 123, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2278, 'NARUC_SPECIAL_MAINT_NOTES', 123, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2279, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 123, 1, 0, 1, 1, 12);
";

                #endregion

                #region BATTERY CHARGER

                case "BATTERY CHARGER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (812, 'AMP_RATING', 124, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1486, 'BATTCHGR_TYP', 124, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1579, 'VOLT_RATING', 124, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1701, 'OWNED_BY', 124, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1702, 'SPECIAL_MAINT_NOTES', 124, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2014, 'NARUC_MAINTENANCE_ACCOUNT', 124, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2015, 'NARUC_OPERATIONS_ACCOUNT', 124, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2280, 'NARUC_SPECIAL_MAINT_NOTES', 124, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2281, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 124, 1, 0, 1, 1, 9);
";

                #endregion

                #region BLOW OFF VALVE

                case "BLOW OFF VALVE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (827, 'APPLICATION_SVLV-BO', 219, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (841, 'DEPENDENCY_DRIVER_1', 219, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (845, 'DEPENDENCY_DRIVER_2', 219, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (858, 'NORMAL_SYS_PRESSURE', 219, 2, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (862, 'NUMBER_OF_TURNS', 219, 2, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (887, 'VLV_SPECIAL_V_BOX_MARKING', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (911, 'ON_SCADA', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (913, 'PRESSURE_CLASS', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (923, 'OPEN_DIRECTION', 219, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1029, 'SVLV-BO_TYP', 219, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1036, 'JOINT_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1069, 'NORMAL_POSITION', 219, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1150, 'VLV_VALVE_SIZE', 219, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1179, 'VLV_OPER_NUT_SIZE', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1231, 'BYPASS_VALVE', 219, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1257, 'VLV_SEAT_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1268, 'SPECIAL_MAINT_NOTES_DIST', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1283, 'ACTUATOR_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1344, 'GEAR_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1431, 'VLV_VALVE_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1437, 'OPERATING_NUT_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1464, 'PIPE_MATERIAL', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1490, 'ACCESS_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1496, 'SURFACE_COVER_LOC_TP', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1565, 'EAM_PIPE_SIZE', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1586, 'SURFACE_COVER', 219, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2211, 'OWNED_BY', 219, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2212, 'SPECIAL_MAINT_NOTE', 219, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2213, 'NARUC_MAINTENANCE_ACCOUNT', 219, 1, 0, 1, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2214, 'NARUC_OPERATIONS_ACCOUNT', 219, 1, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2215, 'SPECIAL_MAINT_NOTE_DETAILS', 219, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2216, 'SUBDIVISION', 219, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2217, 'PRESSURE_ZONE', 219, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2218, 'PRESSURE_ZONE_HGL', 219, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2219, 'MAP_PAGE', 219, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2220, 'BOOK_PAGE', 219, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2221, 'TORQUE_LIMIT', 219, 1, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2467, 'NARUC_SPECIAL_MAINT_NOTES', 219, 1, 0, 1, 1, 32);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2468, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 219, 1, 0, 1, 1, 33);
";

                #endregion

                #region BLOWER

                case "BLOWER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (831, 'BHP_RATING', 125, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (835, 'CAPACITY_UOM_BLWR', 125, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1004, 'BLWR_TYP', 125, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1165, 'DRIVE_TP', 125, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1407, 'APPLICATION_BLWR', 125, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1703, 'OWNED_BY', 125, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1704, 'CAPACITY_RATING', 125, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1705, 'RPM_OPERATING', 125, 2, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1706, 'MAX_PRESSURE', 125, 2, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1707, 'SPECIAL_MAINT_NOTES', 125, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2016, 'NARUC_MAINTENANCE_ACCOUNT', 125, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2017, 'NARUC_OPERATIONS_ACCOUNT', 125, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2282, 'NARUC_SPECIAL_MAINT_NOTES', 125, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2283, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 125, 1, 0, 1, 1, 14);
";

                #endregion

                #region BOILER

                case "BOILER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (813, 'AMP_RATING', 126, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1084, 'ENERGY_TP', 126, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1191, 'BOILER_TYP', 126, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1288, 'DUTY_CYCLE', 126, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1444, 'VOLT_RATING', 126, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1505, 'APPLICATION_BOILER', 126, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1556, 'OUTPUT_UOM', 126, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1710, 'OWNED_BY', 126, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1711, 'OUTPUT_VALUE', 126, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1712, 'SPECIAL_MAINT_NOTES', 126, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2018, 'NARUC_MAINTENANCE_ACCOUNT', 126, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2019, 'NARUC_OPERATIONS_ACCOUNT', 126, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2284, 'NARUC_SPECIAL_MAINT_NOTES', 126, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2285, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 126, 1, 0, 1, 1, 14);
";

                #endregion

                #region BURNER

                case "BURNER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (814, 'AMP_RATING', 127, 2, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (987, 'OUTPUT_UOM', 127, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1088, 'VOLT_RATING', 127, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1139, 'BURNER_TYP', 127, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1237, 'DUTY_CYCLE', 127, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1248, 'ENERGY_TP', 127, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1467, 'APPLICATION_BURNER', 127, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2020, 'NARUC_MAINTENANCE_ACCOUNT', 127, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2021, 'NARUC_OPERATIONS_ACCOUNT', 127, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2286, 'NARUC_SPECIAL_MAINT_NOTES', 127, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2287, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 127, 1, 0, 1, 1, 11);
";

                #endregion

                #region CALIBRATION DEVICE

                case "CALIBRATION DEVICE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (880, 'RETEST_REQUIRED', 128, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1439, 'CALIB_TYP', 128, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1713, 'OWNED_BY', 128, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1714, 'SPECIAL_MAINT_NOTES', 128, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2022, 'NARUC_MAINTENANCE_ACCOUNT', 128, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2023, 'NARUC_OPERATIONS_ACCOUNT', 128, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2288, 'NARUC_SPECIAL_MAINT_NOTES', 128, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2289, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 128, 1, 0, 1, 1, 8);
";

                #endregion

                #region CATHODIC PROTECTION

                case "CATHODIC PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (934, 'VOLT_RATING_CATHODIC', 129, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (962, 'CATHODIC_TYP', 129, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1537, 'APPLICATION_CATHODIC', 129, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2024, 'NARUC_MAINTENANCE_ACCOUNT', 129, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2025, 'NARUC_OPERATIONS_ACCOUNT', 129, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2290, 'NARUC_SPECIAL_MAINT_NOTES', 129, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2291, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 129, 1, 0, 1, 1, 7);
";

                #endregion

                #region CHEMICAL DRY FEEDER

                case "CHEMICAL DRY FEEDER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (873, 'CHM_MATERIAL', 132, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (899, 'APPLICATION_CHMF-DRY', 132, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1093, 'CHM_FEED_RATE_UOM', 132, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1232, 'CHMF-DRY_TYP', 132, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1552, 'CHM_DOSING_CONTROL', 132, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1715, 'OWNED_BY', 132, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1716, 'CHEM_FEED_RATE', 132, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1717, 'SPECIAL_MAINT_NOTES', 132, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2030, 'NARUC_MAINTENANCE_ACCOUNT', 132, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2031, 'NARUC_OPERATIONS_ACCOUNT', 132, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2296, 'NARUC_SPECIAL_MAINT_NOTES', 132, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2297, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 132, 1, 0, 1, 1, 12);
";

                #endregion

                #region CHEMICAL GAS FEEDER

                case "CHEMICAL GAS FEEDER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (907, 'CHM_DOSING_CONTROL', 133, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (931, 'APPLICATION_CHMF-GAS', 133, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (950, 'CHMF-GAS_TYP', 133, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1065, 'CHM_MATERIAL', 133, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1144, 'CHM_FEED_RATE_UOM', 133, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1718, 'OWNED_BY', 133, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1719, 'CHM_FEED_RATE', 133, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1720, 'SPECIAL_MAINT_NOTES', 133, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2032, 'NARUC_MAINTENANCE_ACCOUNT', 133, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2033, 'NARUC_OPERATIONS_ACCOUNT', 133, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2298, 'NARUC_SPECIAL_MAINT_NOTES', 133, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2299, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 133, 1, 0, 1, 1, 12);
";

                #endregion

                #region CHEMICAL GENERATORS

                case "CHEMICAL GENERATORS":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (973, 'CHM_DOSING_CONTROL', 130, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1034, 'APPLICATION_CHEM-GEN', 130, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1195, 'CHM_FEED_RATE_UOM', 130, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1366, 'CHEM-GEN_TYP', 130, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1370, 'CHM_MATERIAL', 130, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1873, 'OWNED_BY', 130, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1874, 'CHEM_FEED_RATE', 130, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2026, 'NARUC_MAINTENANCE_ACCOUNT', 130, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2027, 'NARUC_OPERATIONS_ACCOUNT', 130, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2292, 'NARUC_SPECIAL_MAINT_NOTES', 130, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2293, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 130, 1, 0, 1, 1, 11);
";

                #endregion

                #region CHEMICAL LIQUID FEEDER

                case "CHEMICAL LIQUID FEEDER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1094, 'CHMF-LIQ_TYP', 134, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1132, 'CHM_FEED_RATE_UOM', 134, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1213, 'CHM_MATERIAL', 134, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1253, 'CHM_DOSING_CONTROL', 134, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1319, 'APPLICATION_CHMF-LIQ', 134, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1721, 'OWNED_BY', 134, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1722, 'CHEM_FEED_RATE', 134, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1983, 'SPECIAL_MAINT_NOTES', 134, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2034, 'NARUC_MAINTENANCE_ACCOUNT', 134, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2035, 'NARUC_OPERATIONS_ACCOUNT', 134, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2300, 'NARUC_SPECIAL_MAINT_NOTES', 134, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2301, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 134, 1, 0, 1, 1, 12);
";

                #endregion

                #region CHEMICAL PIPING

                case "CHEMICAL PIPING":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (915, 'CHM_DOSING_CONTROL', 131, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (974, 'CHM_FEED_RATE_UOM', 131, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1197, 'CHEM-PIP_TYP', 131, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1465, 'APPLICATION_CHEM-PIP', 131, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1473, 'CHM_MATERIAL', 131, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1875, 'OWNED_BY', 131, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1876, 'CHEM_FEED_RATE', 131, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2028, 'NARUC_MAINTENANCE_ACCOUNT', 131, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2029, 'NARUC_OPERATIONS_ACCOUNT', 131, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2294, 'NARUC_SPECIAL_MAINT_NOTES', 131, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2295, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 131, 1, 0, 1, 0, 11);
";

                #endregion

                #region CHEMICAL TANK

                case "CHEMICAL TANK":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1056, 'TNK_STATE_INSPECTION_REQ', 220, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1091, 'UNDERGROUND', 220, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1143, 'TNK_MATERIAL', 220, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1185, 'LOCATION', 220, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1245, 'APPLICATION_TNK-CHEM', 220, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1451, 'TNK_AUTO_REFILL', 220, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1516, 'TNK_PRESSURE_RATING', 220, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1536, 'TNK-CHEM_TYP', 220, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1649, 'OWNED_BY', 220, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1650, 'TNK_VOLUME', 220, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1651, 'TNK_SIDE_LENGTH', 220, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1652, 'TNK_DIAMETER', 220, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1653, 'SPECIAL_MAINT_NOTES_DIST', 220, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2222, 'SPECIAL_MAINT_NOTE', 220, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2223, 'NARUC_MAINTENANCE_ACCOUNT', 220, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2224, 'NARUC_OPERATIONS_ACCOUNT', 220, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2469, 'NARUC_SPECIAL_MAINT_NOTES', 220, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2470, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 220, 1, 0, 1, 1, 17);
";

                #endregion

                #region CLARIFIER

                case "CLARIFIER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1317, 'TRT-CLAR_TYP', 229, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1408, 'MATERIAL_OF_CONSTRUCTION', 229, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1485, 'LOCATION', 229, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1528, 'AUTO_REMOVAL', 229, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1842, 'OWNED_BY', 229, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1843, 'APPLICATION', 229, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1844, 'SURFACE_AREA_SQFT', 229, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1845, 'FLOW_NORMAL_RANGE', 229, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1846, 'CONTACT_TIME_MINUTES', 229, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1847, 'SPECIAL_MAINT_NOTES', 229, 1, 0, 0, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2241, 'NARUC_MAINTENANCE_ACCOUNT', 229, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2242, 'NARUC_OPERATIONS_ACCOUNT', 229, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2487, 'NARUC_SPECIAL_MAINT_NOTES', 229, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2488, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 229, 1, 0, 1, 1, 14);
";

                #endregion

                #region CLEAN OUT

                case "CLEAN OUT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (838, 'DEPENDENCY_DRIVER_1', 137, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (848, 'DEPENDENCY_DRIVER_2', 137, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (975, 'CO_SIZE', 137, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1172, 'CO_TYP', 137, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1218, 'SPECIAL_MAINT_NOTES_CO', 137, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1256, 'SURFACE_COVER', 137, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1261, 'SURFACE_COVER_LOC_TP', 137, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1279, 'CO_FITTING_TP', 137, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1345, 'ASSET_LOCATION', 137, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1509, 'CO_SWEEP_TP', 137, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1540, 'MATERIAL_OF_CONSTRUCTION_CO', 137, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1575, 'WASTE_WATER_TP', 137, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2040, 'NARUC_MAINTENANCE_ACCOUNT', 137, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2041, 'NARUC_OPERATIONS_ACCOUNT', 137, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2306, 'NARUC_SPECIAL_MAINT_NOTES', 137, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2307, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 137, 1, 0, 1, 1, 16);
";

                #endregion

                #region COLLECTION SYSTEM GENERAL

                case "COLLECTION SYSTEM GENERAL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (935, 'COLLSYS_TYP', 138, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1879, 'OWNED_BY', 138, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2042, 'NARUC_MAINTENANCE_ACCOUNT', 138, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2043, 'NARUC_OPERATIONS_ACCOUNT', 138, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2308, 'NARUC_SPECIAL_MAINT_NOTES', 138, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2309, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 138, 1, 0, 1, 1, 6);
";

                #endregion

                #region CONTROL PANEL

                case "CONTROL PANEL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (816, 'AMP_RATING', 135, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1103, 'VOLT_RATING', 135, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1569, 'CNTRLPNL_TYP', 135, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1735, 'OWNED_BY', 135, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1736, 'SPECIAL_MAINT_NOTES', 135, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2036, 'NARUC_MAINTENANCE_ACCOUNT', 135, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2037, 'NARUC_OPERATIONS_ACCOUNT', 135, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2302, 'NARUC_SPECIAL_MAINT_NOTES', 135, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2303, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 135, 1, 0, 1, 1, 9);
";

                #endregion

                #region CONTROLLER

                case "CONTROLLER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (866, 'COMM4_TP', 136, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (895, 'VOLT_RATING_CNTRLR', 136, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (941, 'COMM1_TP', 136, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (943, 'APPLICATION_CNTRLR', 136, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (981, 'STANDBY_POWER_TP', 136, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1066, 'CNTRLR_TYP', 136, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1096, 'COMM6_DEVICE', 136, 5, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1147, 'COMM2_DEVICE', 136, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1206, 'COMM6_TP', 136, 5, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1282, 'REMOTE_IO', 136, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1309, 'COMM5_TP', 136, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1424, 'COMM3_DEVICE', 136, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1440, 'COMM3_TP', 136, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1453, 'COMM5_DEVICE', 136, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1463, 'COMM4_DEVICE', 136, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1553, 'END_NODE', 136, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1570, 'COMM2_TP', 136, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1573, 'COMM1_DEVICE', 136, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1877, 'OWNED_BY', 136, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1878, 'CPU_BATTERY # OR (NONE)', 136, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2038, 'NARUC_MAINTENANCE_ACCOUNT', 136, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2039, 'NARUC_OPERATIONS_ACCOUNT', 136, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2304, 'NARUC_SPECIAL_MAINT_NOTES', 136, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2305, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 136, 1, 0, 1, 1, 24);
";

                #endregion

                #region CONVEYOR

                case "CONVEYOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1077, 'CONVEYOR_TYP', 148, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1889, 'OWNED_BY', 148, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1890, 'SPEED (FPS)', 148, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1891, 'RPM_OPERATING', 148, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1892, 'LENGTH (FT)', 148, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1893, 'WIDTH (FT)', 148, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1894, 'LIFT (FT)', 148, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2062, 'NARUC_MAINTENANCE_ACCOUNT', 148, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2063, 'NARUC_OPERATIONS_ACCOUNT', 148, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2328, 'NARUC_SPECIAL_MAINT_NOTES', 148, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2329, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 148, 1, 0, 1, 1, 11);
";

                #endregion

                #region COOLING TOWER

                case "COOLING TOWER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1001, 'APPLICATION_HVAC-TWR', 172, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1130, 'ENERGY_TP', 172, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1149, 'OUTPUT_UOM', 172, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1167, 'HVAC-TWR_TYP', 172, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1782, 'OWNED_BY', 172, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1783, 'CFM', 172, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1784, 'OUTPUT_VALUE', 172, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1785, 'SPECIAL_MAINT_NOTES', 172, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2110, 'NARUC_MAINTENANCE_ACCOUNT', 172, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2111, 'NARUC_OPERATIONS_ACCOUNT', 172, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2376, 'NARUC_SPECIAL_MAINT_NOTES', 172, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2377, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 172, 1, 0, 1, 1, 12);
";

                #endregion

                #region DAM

                case "DAM":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1380, 'DAM_TYP', 149, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1895, 'OWNED_BY', 149, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1896, 'STATE_ID_NUMBER', 149, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1897, 'HEIGHT (FT)', 149, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1898, 'NORMAL POOL HEIGHT (FT)', 149, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1899, 'NORMAL POOL CAPACITY (MG)', 149, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2064, 'NARUC_MAINTENANCE_ACCOUNT', 149, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2065, 'NARUC_OPERATIONS_ACCOUNT', 149, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2330, 'NARUC_SPECIAL_MAINT_NOTES', 149, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2331, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 149, 1, 0, 1, 1, 10);
";

                #endregion

                #region DEFIBRILLATOR

                case "DEFIBRILLATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1000, 'AED_TYP', 122, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1159, 'BACKUP_POWER', 122, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1551, 'PPE_RATING', 122, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1568, 'RETEST_REQUIRED', 122, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1693, 'OWNED_BY', 122, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1694, 'SPECIAL_MAINT_NOTES', 122, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2010, 'NARUC_Maintenance_Account', 122, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2011, 'NARUC_OPERATIONS_ACCOUNT', 122, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2276, 'NARUC_SPECIAL_MAINT_NOTES', 122, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2277, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 122, 1, 0, 1, 1, 10);
";

                #endregion

                #region DISTRIBUTION SYSTEM

                case "DISTRIBUTION SYSTEM":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (837, 'DEPENDENCY_DRIVER_1', 150, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (948, 'DISTSYS_TYP', 150, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2066, 'NARUC_MAINTENANCE_ACCOUNT', 150, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2067, 'NARUC_OPERATIONS_ACCOUNT', 150, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2332, 'NARUC_SPECIAL_MAINT_NOTES', 150, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2333, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 150, 1, 0, 1, 1, 6);
";

                #endregion

                #region DISTRIBUTION TOOL

                case "DISTRIBUTION TOOL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (849, 'DISTTOOL_MAX_SENSORS', 151, 2, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1177, 'DISTTOOL_DETECTOR_TYPE', 151, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1184, 'DISTTOOL_TYP', 151, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1545, 'DISTTOOL_SENSOR_TYPE', 151, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2068, 'NARUC_MAINTENANCE_ACCOUNT', 151, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2069, 'NARUC_OPERATIONS_ACCOUNT', 151, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2336, 'NARUC_SPECIAL_MAINT_NOTES', 151, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2337, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 151, 1, 0, 1, 1, 8);
";

                #endregion

                #region ELEVATOR

                case "ELEVATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1402, 'CAPACITY_UOM_E', 152, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1483, 'RETEST_REQUIRED', 152, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1559, 'ELEVATOR_TYP', 152, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1900, 'OWNED_BY', 152, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1901, 'CAPACITY_RATING', 152, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2070, 'NARUC_MAINTENANCE_ACCOUNT', 152, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2071, 'NARUC_OPERATIONS_ACCOUNT', 152, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2334, 'NARUC_SPECIAL_MAINT_NOTES', 152, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2335, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 152, 1, 0, 1, 1, 9);
";

                #endregion

                #region EMERGENCY GENERATOR

                case "EMERGENCY GENERATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (905, 'VOLT_RATING', 163, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (971, 'RPM_RATING', 163, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1110, 'APPLICATION_GEN', 163, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1215, 'GEN_TYP', 163, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1293, 'GEN_LOAN', 163, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1339, 'PHASES', 163, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1420, 'SELF_STARTING', 163, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1469, 'GEN_VOLTAGE_TP', 163, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1584, 'GEN_PORTABLE', 163, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1597, 'FUEL_TYPE', 163, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1598, 'APPLICATION_TNK-FUEL', 163, 5, 0, 1, 0, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1599, 'FUEL_TNK', 163, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1600, 'OWNED_BY', 163, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1601, 'GEN_KW', 163, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1602, 'GEN_CURRENT', 163, 2, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1603, 'SPECIAL_MAINT_NOTES_DIST', 163, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2092, 'NARUC_MAINTENANCE_ACCOUNT', 163, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2093, 'NARUC_OPERATIONS_ACCOUNT', 163, 1, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2358, 'NARUC_SPECIAL_MAINT_NOTES', 163, 1, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2359, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 163, 1, 0, 1, 1, 20);
";

                #endregion

                #region EMERGENCY LIGHT

                case "EMERGENCY LIGHT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (893, 'RETEST_REQUIRED', 153, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1341, 'BACKUP_POWER', 153, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1481, 'ELIGHT_TYP', 153, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1737, 'OWNED_BY', 153, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1738, 'SPECIAL_MAINT_NOTES', 153, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2072, 'NARUC_MAINTENANCE_ACCOUNT', 153, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2073, 'NARUC_OPERATIONS_ACCOUNT', 153, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2338, 'NARUC_SPECIAL_MAINT_NOTES', 153, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2339, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 153, 1, 0, 1, 1, 9);
";

                #endregion

                #region ENGINE

                case "ENGINE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (851, 'HP_RATING', 154, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1075, 'APPLICATION_ENG', 154, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1272, 'SELF_STARTING', 154, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1457, 'ENG_TYP', 154, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1513, 'ENG_FUEL_UOM', 154, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1592, 'OWNED_BY', 154, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1593, 'ENG_MAX_FUEL', 154, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1594, 'ENG_CYLINDERS', 154, 2, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1595, 'TNK_VOLUME', 154, 2, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1596, 'SPECIAL_MAINT_NOTES_DIST', 154, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2074, 'NARUC_MAINTENANCE_ACCOUNT', 154, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2075, 'NARUC_OPERATIONS_ACCOUNT', 154, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2340, 'NARUC_SPECIAL_MAINT_NOTES', 154, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2341, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 154, 1, 0, 1, 1, 14);
";

                #endregion

                #region EYEWASH

                case "EYEWASH":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (890, 'RETEST_REQUIRED', 155, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (957, 'EYEWASH_TYP', 155, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1730, 'OWNED_BY', 155, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1731, 'SPECIAL_MAINT_NOTES', 155, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2076, 'NARUC_MAINTENANCE_ACCOUNT', 155, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2077, 'NARUC_OPERATIONS_ACCOUNT', 155, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2342, 'NARUC_SPECIAL_MAINT_NOTES', 155, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2343, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 155, 1, 0, 1, 1, 8);
";

                #endregion

                #region FACILITY AND GROUNDS

                case "FACILITY AND GROUNDS":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (914, 'ON_SCADA', 156, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1100, 'FACILITY_FIRE_ALARM', 156, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1181, 'FACILITY_SECURITY_TP', 156, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1201, 'FACILITY_VOLTAGE', 156, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1390, 'FACILITY_STAFFING', 156, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1395, 'BACKUP_POWER', 156, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1399, 'FACILITY_TYP', 156, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1679, 'Block', 156, 2, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1680, 'Lot', 156, 2, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1902, 'OWNED_BY', 156, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1903, 'TOTAL_SQ_FT', 156, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2078, 'NARUC_MAINTENANCE_ACCOUNT', 156, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2079, 'NARUC_OPERATIONS_ACCOUNT', 156, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2344, 'NARUC_SPECIAL_MAINT_NOTES', 156, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2345, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 156, 1, 0, 1, 1, 15);
";

                #endregion

                #region FALL PROTECTION

                case "FALL PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1333, 'PPE-FALL_TYP', 194, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1365, 'PPE_RATING', 194, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1401, 'RETEST_REQUIRED', 194, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1744, 'OWNED_BY', 194, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1745, 'SPECIAL_MAINT_NOTES', 194, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2153, 'NARUC_MAINTENANCE_ACCOUNT', 194, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2154, 'NARUC_OPERATIONS_ACCOUNT', 194, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2418, 'NARUC_SPECIAL_MAINT_NOTES', 194, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2419, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 194, 1, 0, 1, 1, 9);
";

                #endregion

                #region FILTER

                case "FILTER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (903, 'MEDIA_2_TP_TRT-FILT', 231, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1043, 'MEDIA_3_TP', 231, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1053, 'LOCATION', 231, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1186, 'MATERIAL_OF_CONSTRUCTION', 231, 5, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1194, 'APPLICATION_TRT-FILT', 231, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1203, 'MEDIA_1_TP_TRT-FILT', 231, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1233, 'MEDIA_4_TP', 231, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1259, 'MEDIA_5_TP', 231, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1393, 'WASH_TP', 231, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1434, 'TRT-FILT_TYP', 231, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1588, 'MEDIA_REGENERATION_REQD', 231, 5, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1863, 'OWNED_BY', 231, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1864, 'SURFACE_AREA_SQFT', 231, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1865, 'MEDIA_1_DEPTH', 231, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1866, 'MEDIA_2_DEPTH', 231, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1867, 'MEDIA_3_DEPTH', 231, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1868, 'MEDIA_4_DEPTH', 231, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1869, 'MEDIA_5_DEPTH', 231, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1870, 'FLOW_NORMAL_RANGE', 231, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1871, 'BACKWASH_RATE_GPM/SGFT', 231, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1872, 'SPECIAL_MAINT_NOTES', 231, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2245, 'NARUC_MAINTENANCE_ACCOUNT', 231, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2246, 'NARUC_OPERATIONS_ACCOUNT', 231, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2491, 'NARUC_SPECIAL_MAINT_NOTES', 231, 1, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2492, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 231, 1, 0, 1, 1, 25);
";

                #endregion

                #region FIRE ALARM

                case "FIRE ALARM":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (967, 'RETEST_REQUIRED', 157, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (985, 'FIRE-AL_TYP', 157, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1009, 'FIRE_CLASS_RATING', 157, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1024, 'ACTION_TAKEN_UPON_ALARM', 157, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1904, 'OWNED_BY', 157, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2080, 'NARUC_MAINTENANCE_ACCOUNT', 157, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2081, 'NARUC_OPERATIONS_ACCOUNT', 157, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2346, 'NARUC_SPECIAL_MAINT_NOTES', 157, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2347, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 157, 1, 0, 1, 1, 9);
";

                #endregion

                #region FIRE EXTINGUISHER

                case "FIRE EXTINGUISHER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (954, 'RETEST_REQUIRED', 158, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1085, 'FIRE-EX_TYP', 158, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1422, 'FIRE_CLASS_RATING', 158, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1905, 'OWNED_BY', 158, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2082, 'NARUC_MAINTENANCE_ACCOUNT', 158, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2083, 'NARUC_OPERATIONS_ACCOUNT', 158, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2348, 'NARUC_SPECIAL_MAINT_NOTES', 158, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2349, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 158, 1, 0, 1, 1, 8);
";

                #endregion

                #region FIRE SUPPRESSION

                case "FIRE SUPPRESSION":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (910, 'FIRE_CLASS_RATING', 159, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1039, 'RETEST_REQUIRED', 159, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1448, 'FIRE-SUP_TYP', 159, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1580, 'ACTION_TAKEN_UPON_ALARM', 159, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1906, 'OWNED_BY', 159, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2084, 'NARUC_MAINTENANCE_ACCOUNT', 159, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2085, 'NARUC_OPERATIONS_ACCOUNT', 159, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2350, 'NARUC_SPECIAL_MAINT_NOTES', 159, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2351, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 159, 1, 0, 1, 1, 9);
";

                #endregion

                #region FIREWALL

                case "FIREWALL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (825, 'ANNUAL_COST', 139, 3, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (888, 'COMMUNICATION_TP', 139, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1246, 'BAUD_RATE', 139, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1352, 'APPLICATION_COMM-FWL', 139, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1384, 'COMM-FWL_TYP', 139, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1455, 'STANDBY_POWER_TP', 139, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1880, 'OWNED_BY', 139, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2044, 'NARUC_MAINTENANCE_ACCOUNT', 139, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2045, 'NARUC_OPERATIONS_ACCOUNT', 139, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2310, 'NARUC_SPECIAL_MAINT_NOTES', 139, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2311, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 139, 1, 0, 1, 1, 11);
";

                #endregion

                #region FLOATATION DEVICE

                case "FLOATATION DEVICE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1243, 'RETEST_REQUIRED', 195, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1334, 'PPE-FLOT_TYP', 195, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1421, 'BACKUP_POWER', 195, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1488, 'PPE_RATING', 195, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2155, 'OWNED_BY', 195, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2156, 'SPECIAL_MAINT_NOTE', 195, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2157, 'NARUC_MAINTENANCE_ACCOUNT', 195, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2158, 'NARUC_OPERATIONS_ACCOUNT', 195, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2420, 'NARUC_SPECIAL_MAINT_NOTES', 195, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2421, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 195, 1, 0, 1, 1, 10);
";

                #endregion

                #region FLOW METER (NON PREMISE)

                case "FLOW METER (NON PREMISE)":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (869, 'NOMINAL_SIZE', 160, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1025, 'FLOW_UOM', 160, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1098, 'TRANSMITTER_MANUFACTURER', 160, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1126, 'OUTPUT_TP', 160, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1176, 'CALIBRATION_UOM', 160, 5, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1262, 'TRANSMITTER', 160, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1302, 'SQUARE_ROOT', 160, 5, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1311, 'INSTRUMENT_POWER', 160, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1320, 'COMM_PROTOCOL', 160, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1342, 'BYPASS_VALVE', 160, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1369, 'MET_MATERIAL', 160, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1419, 'ON_SCADA', 160, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1436, 'APPLICATION_FLO-MET', 160, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1548, 'FLO-MET_TYP', 160, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1907, 'OWNED_BY', 160, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1908, 'FLOW_CALIBRATED_RANGE', 160, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1909, 'FLOW_NORMAL_RANGE', 160, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1910, 'FLOW_MAX_INTERMITTENT', 160, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1911, 'FLOW_VELOCITY_RANGE', 160, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1912, 'MAX_PRESSURE', 160, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1913, 'DIFFERENTIAL_PSI_RANGE', 160, 1, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1914, 'BETA_RATIO', 160, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1915, 'SPECIFIC_GRAVITY', 160, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1916, 'PRESSURE_DROP_MAX', 160, 1, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2086, 'NARUC_MAINTENANCE_ACCOUNT', 160, 1, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2087, 'NARUC_OPERATIONS_ACCOUNT', 160, 1, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2352, 'NARUC_SPECIAL_MAINT_NOTES', 160, 1, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2353, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 160, 1, 0, 1, 1, 28);
";

                #endregion

                #region FLOW WEIR

                case "FLOW WEIR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (912, 'OUTPUT_TP', 161, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (969, 'TRANSMITTER', 161, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1014, 'MET_MATERIAL', 161, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1057, 'CALIBRATION_UOM', 161, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1133, 'FLO-WEIR_TYP', 161, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1157, 'FLOW_UOM', 161, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1163, 'ON_SCADA', 161, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1306, 'NOMINAL_SIZE', 161, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1330, 'APPLICATION_FLO-WEIR', 161, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1363, 'BYPASS_VALVE', 161, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1385, 'INSTRUMENT_POWER', 161, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1423, 'SQUARE_ROOT', 161, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1480, 'COMM_PROTOCOL', 161, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1526, 'TRANSMITTER_MANUFACTURER', 161, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2088, 'NARUC_MAINTENANCE_ACCOUNT', 161, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2089, 'NARUC_OPERATIONS_ACCOUNT', 161, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2354, 'NARUC_SPECIAL_MAINT_NOTES', 161, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2355, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 161, 1, 0, 1, 1, 18);
";

                #endregion

                #region FUEL TANK

                case "FUEL TANK":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (872, 'APPLICATION_TNK-FUEL', 221, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (894, 'TNK_AUTO_REFILL', 221, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (960, 'TNK_STATE_INSPECTION_REQ', 221, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1062, 'TNK-FUEL_TYP', 221, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1073, 'UNDERGROUND', 221, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1372, 'TNK_PRESSURE_RATING', 221, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1533, 'TNK_MATERIAL', 221, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1546, 'LOCATION', 221, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1654, 'OWNED_BY', 221, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1655, 'TNK_VOLUME', 221, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1656, 'TNK_SIDE_LENGTH', 221, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1657, 'TNK_DIAMETER', 221, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1658, 'SPECIAL_MAINT_NOTES_DIST', 221, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1682, 'CONTAINMENT', 221, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1683, 'LEAK_DETECTION', 221, 1, 0, 1, 0, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1685, 'FIRE_RESISTANT - MIN UL 2080', 221, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1687, 'FIRE_PROTECTED - MIN UL 2085', 221, 1, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1688, 'DISTANCE_DITCH_WATERWAY', 221, 2, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1709, 'Spill and Overfill Protection', 221, 1, 0, 1, 0, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2225, 'NARUC_MAINTENANCE_ACCOUNT', 221, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2226, 'NARUC_OPERATIONS_ACCOUNT', 221, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2471, 'NARUC_SPECIAL_MAINT_NOTES', 221, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2472, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 221, 1, 0, 1, 1, 23);
";

                #endregion

                #region GAS DETECTOR

                case "GAS DETECTOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1223, 'SAFGASDT_TYP', 210, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1300, 'GAS_MITIGATED', 210, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1374, 'ACTION_TAKEN_UPON_ALARM', 210, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1504, 'BACKUP_POWER', 210, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2189, 'OWNED_BY', 210, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2190, 'GAS_ALARM_SETPOINT', 210, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2191, 'SPECIAL_MAINT_NOTES', 210, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2192, 'NARUC_MAINTENANCE_ACCOUNT', 210, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2193, 'NARUC_OPERATIONS_ACCOUNT', 210, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2449, 'NARUC_SPECIAL_MAINT_NOTES', 210, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2450, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 210, 1, 0, 1, 1, 11);
";

                #endregion

                #region GEARBOX

                case "GEARBOX":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1026, 'GEARBOX_TYP', 162, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1051, 'APPLICATION_GEARBOX', 162, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1776, 'OWNED_BY', 162, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1777, 'RPM_OPERATING', 162, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1778, 'GEAR_RATIO', 162, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1779, 'OIL_CAPACITY (GAL)', 162, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1780, 'OIL_TYPE', 162, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1781, 'SPECIAL_MAINT_NOTES', 162, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2090, 'NARUC_MAINTENANCE_ACCOUNT', 162, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2091, 'NARUC_OPERATIONS_ACCOUNT', 162, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2356, 'NARUC_SPECIAL_MAINT_NOTES', 162, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2357, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 162, 1, 0, 1, 1, 12);
";

                #endregion

                #region GRAVITY SEWER MAIN

                case "GRAVITY SEWER MAIN":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (842, 'DEPENDENCY_DRIVER_1', 164, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (854, 'LINED_DATE', 164, 4, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (898, 'SPECIAL_MAINT_NOTES_GM', 164, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (932, 'LINED', 164, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (986, 'EAM_PIPE_SIZE', 164, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1074, 'ASSET_LOCATION', 164, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1089, 'GMAIN_TYP', 164, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1220, 'LINED_MATERIAL', 164, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1239, 'WASTE_WATER_TP', 164, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1388, 'PIPE_MATERIAL', 164, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1441, 'FLOW_DIRECTION', 164, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1564, 'SURFACE_COVER_LOC_TP', 164, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2094, 'NARUC_MAINTENANCE_ACCOUNT', 164, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2095, 'NARUC_OPERATIONS_ACCOUNT', 164, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2360, 'NARUC_SPECIAL_MAINT_NOTES', 164, 2, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2361, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 164, 1, 0, 1, 1, 16);
";

                #endregion

                #region GRINDER

                case "GRINDER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1151, 'EAM_PIPE_SIZE', 165, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1196, 'NUMBER_OF_CUTTER_TEETH', 165, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1362, 'ORIENTATION', 165, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1364, 'MOUNTING_GRINDER', 165, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1477, 'GRINDER_TYP', 165, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2096, 'NARUC_MAINTENANCE_ACCOUNT', 165, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2097, 'NARUC_OPERATIONS_ACCOUNT', 165, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2362, 'NARUC_SPECIAL_MAINT_NOTES', 165, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2363, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 165, 1, 0, 1, 1, 9);
";

                #endregion

                #region HEAT EXCHANGER

                case "HEAT EXCHANGER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (902, 'MATERIAL_OF_CONSTRUCTION_HVAC', 170, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (952, 'MEDIA_2_TP_HVAC-EXC', 170, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1054, 'MATERIAL_2', 170, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1101, 'HVAC-EXC_TYP', 170, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1337, 'MEDIA_1_TP_HVAC-EXC', 170, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1571, 'OUTPUT_UOM', 170, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1761, 'OWNED_BY', 170, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1762, 'OUTPUT_VALUE', 170, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1763, 'MAX_PRESSURE', 170, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1764, 'SPECIAL_MAINT_NOTES', 170, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2106, 'NARUC_MAINTENANCE_ACCOUNT', 170, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2107, 'NARUC_OPERATIONS_ACCOUNT', 170, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2372, 'NARUC_SPECIAL_MAINT_NOTES', 170, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2373, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 170, 1, 0, 1, 1, 14);
";

                #endregion

                #region HOIST

                case "HOIST":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1042, 'CAPACITY_UOM', 166, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1064, 'RETEST_REQUIRED', 166, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1267, 'HOIST_TYP', 166, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1732, 'OWNED_BY', 166, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1733, 'CAPACITY_RATING', 166, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1734, 'SPECIAL_MAINT_NOTES', 166, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2098, 'NARUC_MAINTENANCE_ACCOUNT', 166, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2099, 'NARUC_OPERATIONS_ACCOUNT', 166, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2364, 'NARUC_SPECIAL_MAINT_NOTES', 166, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2365, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 166, 1, 0, 1, 1, 10);
";

                #endregion

                #region HVAC CHILLER

                case "HVAC CHILLER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (803, 'AMP_RATING', 167, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (916, 'HVAC-CHL_TYP', 167, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (970, 'DUTY_CYCLE', 167, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1173, 'ENERGY_TP', 167, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1278, 'OUTPUT_UOM', 167, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1297, 'APPLICATION_HVAC-CHL', 167, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1350, 'VOLT_RATING', 167, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1752, 'OWNED_BY', 167, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1753, 'OUTPUT_VALUE', 167, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1754, 'SPECIAL_MAINT_NOTES', 167, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2100, 'NARUC_MAINTENANCE_ACCOUNT', 167, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2101, 'NARUC_OPERATIONS_ACCOUNT', 167, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2366, 'NARUC_SPECIAL_MAINT_NOTES', 167, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2367, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 167, 1, 0, 1, 1, 14);
";

                #endregion

                #region HVAC COMBINATION UNIT

                case "HVAC COMBINATION UNIT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (804, 'AMP_RATING', 168, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (926, 'APPLICATION_HVAC-CMB', 168, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1050, 'OUTPUT_UOM', 168, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1111, 'HVAC-CMB_TYP', 168, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1169, 'VOLT_RATING', 168, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1474, 'DUTY_CYCLE', 168, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1482, 'ENERGY_TP', 168, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1755, 'OWNED_BY', 168, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1756, 'OUTPUT_VALUE', 168, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1757, 'SPECIAL_MAINT_NOTES', 168, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2102, 'NARUC_MAINTENANCE_ACCOUNT', 168, 1, 1, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2103, 'NARUC_OPERATIONS_ACCOUNT', 168, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2368, 'NARUC_SPECIAL_MAINT_NOTES', 168, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2369, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 168, 1, 0, 1, 1, 14);
";

                #endregion

                #region HVAC DEHUMIDIFIER

                case "HVAC DEHUMIDIFIER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (805, 'AMP_RATING', 169, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1030, 'VOLT_RATING', 169, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1086, 'HVAC-DHM_TYP', 169, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1154, 'OUTPUT_UOM', 169, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1531, 'APPLICATION_HVAC-DHM', 169, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1538, 'DUTY_CYCLE', 169, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1560, 'ENERGY_TP', 169, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1758, 'OWNED_BY', 169, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1759, 'OUTPUT_VALUE', 169, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1760, 'SPECIAL_MAINT_NOTES', 169, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2104, 'NARUC_MAINTENANCE_ACCOUNT', 169, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2105, 'NARUC_OPERATIONS_ACCOUNT', 169, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2370, 'NARUC_SPECIAL_MAINT_NOTES', 169, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2371, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 169, 1, 0, 1, 1, 14);
";

                #endregion

                #region HVAC HEATER

                case "HVAC HEATER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (806, 'AMP_RATING', 171, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (922, 'ENERGY_TP', 171, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (951, 'HVAC-HTR_TYP', 171, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (972, 'APPLICATION_HVAC-HTR', 171, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (996, 'OUTPUT_UOM', 171, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1131, 'VOLT_RATING', 171, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1550, 'DUTY_CYCLE', 171, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1765, 'OWNED_BY', 171, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1766, 'OUTPUT_VALUE', 171, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1767, 'SPECIAL_MAINT_NOTES', 171, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2114, 'NARUC_MAINTENANCE_ACCOUNT', 171, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2115, 'NARUC_OPERATIONS_ACCOUNT', 171, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2374, 'NARUC_SPECIAL_MAINT_NOTES', 171, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2375, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 171, 1, 0, 1, 1, 14);
";

                #endregion

                #region HVAC VENTILATOR

                case "HVAC VENTILATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (807, 'AMP_RATING', 173, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1008, 'VOLT_RATING', 173, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1035, 'DUTY_CYCLE', 173, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1327, 'ENERGY_TP', 173, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1361, 'HVAC-VNT_TYP', 173, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1456, 'APPLICATION_HVAC-VNT', 173, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1576, 'OUTPUT_UOM', 173, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1917, 'OWNED_BY', 173, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1918, 'OUTPUT_VALUE', 173, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2112, 'NARUC_MAINTENANCE_ACCOUNT', 173, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2113, 'NARUC_OPERATIONS_ACCOUNT', 173, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2378, 'NARUC_SPECIAL_MAINT_NOTES', 173, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2379, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 173, 1, 0, 1, 1, 13);
";

                #endregion

                #region HYDRANT

                case "HYDRANT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (840, 'DEPENDENCY_DRIVER_1', 175, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (846, 'DEPENDENCY_DRIVER_2', 175, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (857, 'NORMAL_SYS_PRESSURE', 175, 2, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (936, 'HYD_TYP', 175, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (955, 'HYD_STEAMER_THREAD_TP', 175, 5, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (984, 'HYD_COLOR_CODE_METHOD', 175, 5, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (990, 'HYD_BARREL_SIZE', 175, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (991, 'HYD_LOCK_DEVICE_TP', 175, 5, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1011, 'OPEN_DIRECTION', 175, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1019, 'HYD_SIDE_NOZZLE_SIZE', 175, 5, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1021, 'HYD_COLOR_CODE_TP', 175, 5, 0, 1, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1046, 'EAM_PIPE_SIZE', 175, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1068, 'HYD_STEAMER_SIZE', 175, 5, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1107, 'JOINT_TP', 175, 5, 0, 1, 1, 34);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1174, 'HYD_STEM_LUBE', 175, 5, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1178, 'PIPE_MATERIAL', 175, 5, 0, 1, 1, 36);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1207, 'HYD_OUTLET_CONFIG', 175, 5, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1280, 'HYD_BARREL_TP', 175, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1289, 'HYD_AUX_VALVE_BRANCH_SIZE', 175, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1418, 'HYD_COLORCODE', 175, 5, 0, 1, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1428, 'SPECIAL_MAINT_NOTES_DIST', 175, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1484, 'HYD_BILLING_TP', 175, 5, 0, 1, 1, 41);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1493, 'HYD_AUXILLARY_VALVE', 175, 5, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1502, 'PRESSURE_CLASS', 175, 5, 0, 1, 1, 33);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1518, 'HYD_DEAD_END_MAIN', 175, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1921, 'OWNED_BY', 175, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1922, 'SPECIAL_MAINT_NOTE', 175, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1923, 'SUBDIVISION', 175, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1924, 'PRESSURE ZONE', 175, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1925, 'PRESSURE_ZONE_HGL', 175, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1926, 'MAP_PAGE', 175, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1927, 'BOOK_PAGE', 175, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1928, 'HYD_BURY_DEPTH', 175, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1929, 'HYD-EXTENSIONS AND SIZES', 175, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1930, 'HYD-BRANCH_LENGTH', 175, 1, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1931, 'HYD-AUX VALVE #', 175, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1932, 'HYD-SIDE NOZZLE SIZE', 175, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1933, 'HYD-SIDE_PORT_THREAD_TYPE', 175, 1, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1934, 'REPAIR_KIT #', 175, 1, 0, 1, 1, 32);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1935, 'PIPE_CHANNEL_SIZE', 175, 1, 0, 1, 1, 35);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1936, 'INSTALLATION_WO #', 175, 1, 0, 1, 1, 37);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1937, 'SKETCH #', 175, 1, 0, 1, 1, 38);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1938, 'HYD-FIRE_DISTRICT', 175, 1, 0, 1, 1, 39);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1939, 'ECIS ACCOUNT #', 175, 1, 0, 1, 1, 40);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1940, 'HISTORICAL_ID', 175, 1, 0, 1, 1, 42);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1941, 'GEOACCURACY GIS-DATASOURCETYPE', 175, 1, 0, 1, 1, 43);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2116, 'NARUC_MAINTENANCE_ACCOUNT', 175, 1, 0, 1, 1, 44);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2117, 'NARUC_OPERATIONS_ACCOUNT', 175, 1, 0, 1, 1, 45);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2382, 'NARUC_SPECIAL_MAINT_NOTES', 175, 1, 0, 1, 1, 46);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2383, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 175, 1, 0, 1, 1, 47);
";

                #endregion

                #region INDICATOR

                case "INDICATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1192, 'INSTRUMENT_UOM', 176, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1277, 'LOOP_POWER', 176, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1281, 'INDICATR_TYP', 176, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1376, 'NEMA_ENCLOSURE', 176, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1377, 'OUTPUT_TP', 176, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1430, 'ON_SCADA', 176, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1942, 'OWNED_BY', 176, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1943, 'INSTRUMENT_RANGE', 176, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2118, 'NARUC_MAINTENANCE_ACCOUNT', 176, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2119, 'NARUC_OPERATIONS_ACCOUNT', 176, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2384, 'NARUC_SPECIAL_MAINT_NOTES', 176, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2385, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 176, 1, 0, 1, 1, 12);
";

                #endregion

                #region INSTRUMENT SWITCH

                case "INSTRUMENT SWITCH":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (978, 'OUTPUT_TP', 177, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1204, 'INST-SW_TYP', 177, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1226, 'NEMA_ENCLOSURE', 177, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1331, 'ON_SCADA', 177, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1539, 'LOOP_POWER', 177, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1561, 'INSTRUMENT_UOM', 177, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1944, 'OWNED_BY', 177, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1945, 'INSTRUMENT_RANGE', 177, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2120, 'NARUC_MAINTENANCE_ACCOUNT', 177, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2121, 'NARUC_OPERATIONS_ACCOUNT', 177, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2386, 'NARUC_SPECIAL_MAINT_NOTES', 177, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2387, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 177, 1, 0, 1, 1, 12);
";

                #endregion

                #region KIT (SAFETY, REPAIR, HAZWOPR)

                case "KIT (SAFETY, REPAIR, HAZWOPR)":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1124, 'RETEST_REQUIRED', 178, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1397, 'KIT_TYP', 178, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1946, 'OWNED_BY', 178, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2122, 'NARUC_MAINTENANCE_ACCOUNT', 178, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2123, 'NARUC_OPERATIONS_ACCOUNT', 178, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2388, 'NARUC_SPECIAL_MAINT_NOTES', 178, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2389, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 178, 1, 0, 1, 1, 7);
";

                #endregion

                #region LAB EQUIPMENT

                case "LAB EQUIPMENT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1208, 'LABEQ_TYP', 179, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1511, 'RETEST_REQUIRED', 179, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1774, 'OWNED_BY', 179, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1775, 'SPECIAL_MAINT_NOTES', 179, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2124, 'NARUC_MAINTENANCE_ACCOUNT', 179, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2125, 'NARUC_OPERATIONS_ACCOUNT', 179, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2390, 'NARUC_SPECIAL_MAINT_NOTES', 179, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2391, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 179, 1, 0, 1, 1, 8);
";

                #endregion

                #region LEAK MONITOR

                case "LEAK MONITOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (992, 'LK-MON_LOC_TYPE', 180, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1113, 'LK-MON_TYP', 180, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1304, 'LK-MON_DEPLOYMENT_TYPE', 180, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1498, 'LK-MON_TYPE', 180, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1566, 'LK-MON_DATA_RETRIEVE_METHOD', 180, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2126, 'NARUC_MAINTENANCE_ACCOUNT', 180, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2127, 'NARUC_OPERATIONS_ACCOUNT', 180, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2392, 'NARUC_SPECIAL_MAINT_NOTES', 180, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2393, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 180, 1, 0, 1, 1, 9);
";

                #endregion

                #region MANHOLE

                case "MANHOLE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (839, 'DEPENDENCY_DRIVER_1', 181, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (847, 'DEPENDENCY_DRIVER_2', 181, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (855, 'LINED_DATE', 181, 4, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (875, 'MH_STEP_MATERIAL', 181, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (879, 'MH_CONE_INSERT', 181, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (909, 'MH_PONDING', 181, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (963, 'MH_TROUGH', 181, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1027, 'MH_LID_TP', 181, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1071, 'WASTE_WATER_TP', 181, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1078, 'MH_COVER_SIZE', 181, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1108, 'MH_DROP_TP', 181, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1161, 'ASSET_LOCATION', 181, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1183, 'SURFACE_COVER_LOC_TP', 181, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1205, 'MH_CONE_MATERIAL', 181, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1221, 'MH_ADJUSTING_RING_MATL', 181, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1323, 'MH_SIZE', 181, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1387, 'MH_PIPE_SEAL_TP', 181, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1398, 'SPECIAL_MAINT_NOTES_MH', 181, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1416, 'MH_HAS_STEPS', 181, 5, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1454, 'MH_TYP', 181, 5, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1472, 'SURFACE_COVER', 181, 5, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1507, 'MATERIAL_OF_CONSTRUCTION_MH', 181, 5, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1514, 'LINED', 181, 5, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1529, 'MH_CASTING_MATERIAL', 181, 5, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1554, 'MH_CONE', 181, 5, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1578, 'MH_COVER_MATERIAL', 181, 5, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2128, 'NARUC_MAINTENANCE_ACCOUNT', 181, 1, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2129, 'NARUC_OPERATIONS_ACCOUNT', 181, 1, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2394, 'NARUC_SPECIAL_MAINT_NOTES', 181, 1, 0, 1, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2395, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 181, 1, 0, 1, 1, 30);
";

                #endregion

                #region MIXER

                case "MIXER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (834, 'BHP_RATING', 182, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1052, 'FLOW_UOM', 182, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1079, 'MIXR_TYP', 182, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1583, 'APPLICATION_MIXR', 182, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1786, 'OWNED_BY', 182, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1787, 'RPM OPERATING', 182, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1788, 'SPECIAL_MAINT_NOTES', 182, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2130, 'NARUC_MAINTENANCE_ACCOUNT', 182, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2131, 'NARUC_OPERATIONS_ACCOUNT', 182, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2396, 'NARUC_SPECIAL_MAINT_NOTES', 182, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2397, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 182, 1, 0, 1, 1, 11);
";

                #endregion

                #region MODEM

                case "MODEM":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (826, 'ANNUAL_COST', 140, 3, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1060, 'COMM-MOD_TYP', 140, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1182, 'STANDBY_POWER_TP', 140, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1214, 'COMMUNICATION_TP', 140, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1296, 'APPLICATION_COMM-MOD', 140, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1298, 'BAUD_RATE', 140, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1881, 'OWNED_BY', 140, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2046, 'NARUC_MAINTENANCE_ACCOUNT', 140, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2047, 'NARUC_OPERATIONS_ACCOUNT', 140, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2312, 'NARUC_SPECIAL_MAINT_NOTES', 140, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2313, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 140, 1, 0, 1, 1, 11);
";

                #endregion

                #region MOTOR

                case "MOTOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (853, 'HP_RATING', 183, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (885, 'MOT_VOLTAGE_RUNNING', 183, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (942, 'MOT_TYP', 183, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (953, 'MOT_SERVICE_FACTOR', 183, 5, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (965, 'MOT_BEARING_TP_FREE_END', 183, 5, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1145, 'MOT_COUPLING_TP', 183, 5, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1153, 'MOT_LUBE_TP_COUP_END', 183, 5, 0, 1, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1166, 'VOLT_RATING', 183, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1193, 'RPM_RATING', 183, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1210, 'MOT_LUBE_TP_FREE_END', 183, 5, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1238, 'INSULATION_CLASS', 183, 5, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1247, 'DUTY_CYCLE', 183, 5, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1249, 'MOT_HOLLOW_SHAFT', 183, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1275, 'MOT_NAMEPLATE_DESIGN', 183, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1373, 'MOT_ENCLOSURE_TP', 183, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1400, 'MOT_BEARING_TP_COUP_END', 183, 5, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1466, 'MOT_CODE', 183, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1475, 'MOT_ANTI_REVERSE', 183, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1510, 'MOT_INVERTER_DUTY', 183, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1519, 'ROTATION_DIRECTION', 183, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1567, 'ORIENTATION', 183, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1572, 'APPLICATION_MOT', 183, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1604, 'OWNED_BY', 183, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1605, 'FULL_LOAD_AMPS', 183, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1606, 'RPM_OPERATING', 183, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1607, 'MOT_FRAME_TP', 183, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1608, 'TEMPERATURE_RISE', 183, 2, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1609, 'MOT_EXCITATION_VOLTAGE', 183, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1610, 'MOT_BEARING_FREE_END', 183, 1, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1611, 'MOT_BEARING_COUP_END', 183, 1, 0, 1, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1612, 'MOT_CATALOG_NUMBER', 183, 1, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1613, 'SPECIAL_MAINT_NOTES_DIST', 183, 5, 0, 1, 1, 32);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2132, 'NARUC_MAINTENANCE_ACCOUNT', 183, 1, 0, 1, 1, 33);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2133, 'NARUC_OPERATIONS_ACCOUNT', 183, 1, 0, 1, 1, 34);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2398, 'NARUC_SPECIAL_MAINT_NOTES', 183, 1, 0, 1, 1, 35);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2399, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 183, 1, 0, 1, 1, 36);
";

                #endregion

                #region MOTOR CONTACTOR

                case "MOTOR CONTACTOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (815, 'AMP_RATING', 146, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1099, 'VOLT_RATING', 146, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1343, 'CONTACTR_TYP', 146, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1367, 'CONTACTOR_TP', 146, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1746, 'OWNED_BY', 146, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1747, 'INTERRUPTING_CAPACITY', 146, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1748, 'SPECIAL_MAINT_NOTES', 146, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2058, 'NARUC_MAINTENANCE_ACCOUNT', 146, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2059, 'NARUC_OPERATIONS_ACCOUNT', 146, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2324, 'NARUC_SPECIAL_MAINT_NOTES', 146, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2325, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 146, 1, 0, 1, 1, 11);
";

                #endregion

                #region MOTOR STARTER

                case "MOTOR STARTER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (817, 'AMP_RATING', 184, 2, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (850, 'HP_RATING', 184, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (959, 'STR_TP', 184, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (982, 'STR_OVERLOAD_TP', 184, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1015, 'MOTSTR_TYP', 184, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1219, 'VOLT_RATING', 184, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1378, 'STR_NEMA_SIZE', 184, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1739, 'OWNED_BY', 184, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1740, 'FUSE_SIZE', 184, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1741, 'SPECIAL_MAINT_NOTES', 184, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2134, 'NARUC_MAINTENANCE_ACCOUNT', 184, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2135, 'NARUC_OPERATIONS_ACCOUNT', 184, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2400, 'NARUC_SPECIAL_MAINT_NOTES', 184, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2401, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 184, 1, 0, 1, 1, 14);
";

                #endregion

                #region NETWORK ROUTER

                case "NETWORK ROUTER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (823, 'ANNUAL_COST', 142, 3, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (947, 'BAUD_RATE', 142, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1121, 'COMMUNICATION_TP', 142, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1266, 'COMM-RTR_TYP', 142, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1290, 'STANDBY_POWER_TP', 142, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1534, 'APPLICATION_COMM-RTR', 142, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1882, 'OWNED_BY', 142, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2050, 'NARUC_MAINTENANCE_ACCOUNT', 142, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2051, 'NARUC_OPERATIONS_ACCOUNT', 142, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2316, 'NARUC_SPECIAL_MAINT_NOTES', 142, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2317, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 142, 1, 0, 1, 1, 11);
";

                #endregion

                #region NETWORK SWITCH

                case "NETWORK SWITCH":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (824, 'ANNUAL_COST', 143, 3, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (900, 'COMM-SW_TYP', 143, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1048, 'APPLICATION_COMM-SW', 143, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1156, 'COMMUNICATION_TP', 143, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1276, 'STANDBY_POWER_TP', 143, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1491, 'BAUD_RATE', 143, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1883, 'OWNED_BY', 143, 1, 0, 1, 0, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2052, 'NARUC_MAINTENANCE_ACCOUNT', 143, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2053, 'NARUC_OPERATIONS_ACCOUNT', 143, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2318, 'NARUC_SPECIAL_MAINT_NOTES', 143, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2319, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 143, 1, 0, 1, 1, 11);
";

                #endregion

                #region NON POTABLE WATER TANK

                case "NON POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (925, 'TNK_PRESSURE_RATING', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1007, 'UNDERGROUND', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1090, 'LOCATION', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1160, 'TNK_STATE_INSPECTION_REQ', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1360, 'APPLICATION_TNK-WNON', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1392, 'TNK_MATERIAL', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1500, 'TNK_AUTO_REFILL', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1503, 'TNK-WNON_TYP', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1664, 'OWNED_BY', 223, 1, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1665, 'TNK_VOLUME', 223, 2, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1666, 'TNK_SIDE_LENGTH', 223, 2, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1667, 'TNK_DIAMETER', 223, 2, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1668, 'SPECIAL_MAINT_NOTES_DIST', 223, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2229, 'NARUC_MAINTENANCE_ACCOUNT', 223, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2230, 'NARUC_OPERATIONS_ACCOUNT', 223, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2475, 'NARUC_SPECIAL_MAINT_NOTES', 223, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2476, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 223, 1, 0, 1, 1, 23);
";

                #endregion

                #region OPERATOR COMPUTER TERMINAL

                case "OPERATOR COMPUTER TERMINAL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (881, 'HMI_MANUFACTURER', 186, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1112, 'OPERATING_SYSTEMS', 186, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1152, 'STANDBY_POWER_TP', 186, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1222, 'RAID', 186, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1410, 'NETWORK_SCHEME', 186, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1412, 'APPLICATION_OIT', 186, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1589, 'OIT_TYP', 186, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2136, 'NARUC_MAINTENANCE_ACCOUNT', 186, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2137, 'NARUC_OPERATIONS_ACCOUNT', 186, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2402, 'NARUC_SPECIAL_MAINT_NOTES', 186, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2403, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 186, 1, 0, 1, 1, 11);
";

                #endregion

                #region PC

                case "PC":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (868, 'NETWORK_SCHEME', 187, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (884, 'HMI_MANUFACTURER', 187, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (929, 'APPLICATION_PC', 187, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (997, 'OPERATING_SYSTEMS', 187, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1122, 'PC_TYP', 187, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1198, 'RAID', 187, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1371, 'STANDBY_POWER_TP', 187, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1947, 'OWNED_BY', 187, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1948, 'RAM_MEMORY', 187, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1949, 'HMI_SOFTWARE', 187, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1950, 'SOFTWARE_LICENSE #', 187, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2138, 'SPECIAL_MAINT_NOTES', 187, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2139, 'NARUC_MAINTENANCE_ACCOUNT', 187, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2140, 'NARUC_OPERATIONS_ACCOUNT', 187, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2404, 'NARUC_SPECIAL_MAINT_NOTES', 187, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2405, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 187, 1, 0, 1, 1, 15);
";

                #endregion

                #region PDM TOOL

                case "PDM TOOL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (958, 'RETEST_REQUIRED', 188, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1134, 'PDMTOOL_TYP', 188, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2141, 'NARUC_MAINTENANCE_ACCOUNT', 188, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2142, 'NARUC_OPERATIONS_ACCOUNT', 188, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2406, 'NARUC_SPECIAL_MAINT_NOTES', 188, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2407, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 188, 1, 0, 1, 1, 6);
";

                #endregion

                #region PHASE CONVERTER

                case "PHASE CONVERTER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1312, 'PHASECON_TYP', 189, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2143, 'NARUC_MAINTENANCE_ACCOUNT', 189, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2144, 'NARUC_OPERATIONS_ACCOUNT', 189, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2408, 'NARUC_SPECIAL_MAINT_NOTES', 189, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2409, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 189, 1, 0, 1, 1, 5);
";

                #endregion

                #region PLANT VALVE

                case "PLANT VALVE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (859, 'NORMAL_SYS_PRESSURE', 199, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (863, 'NUMBER_OF_TURNS', 199, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (927, 'MATERIAL_OF_CONSTRUCTION_PVLV', 199, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (939, 'VLV_VALVE_SIZE', 199, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1017, 'ACTUATOR_TP', 199, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1059, 'BYPASS_VALVE', 199, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1148, 'JOINT_TP', 199, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1200, 'VLV_VALVE_TP', 199, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1225, 'AUTOMATED_ACTUATED', 199, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1263, 'VLV_SWITCH', 199, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1338, 'OPEN_DIRECTION', 199, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1358, 'VLV_ACTUATOR_MANUF', 199, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1396, 'GEAR_TP', 199, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1427, 'PRESSURE_CLASS', 199, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1447, 'APPLICATION_PVLV', 199, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1544, 'NORMAL_POSITION', 199, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1563, 'PVLV_TYP', 199, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1590, 'VLV_FAIL_POSITION', 199, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1954, 'OWNED_BY', 199, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1955, 'OPEN/CLOSE SWITCHES', 199, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2167, 'NARUC_MAINTENANCE_ACCOUNT', 199, 1, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2168, 'NARUC_OPERATIONS_ACCOUNT', 199, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2428, 'NARUC_SPECIAL_MAINT_NOTES', 199, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2429, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 199, 1, 0, 1, 1, 22);
";

                #endregion

                #region POTABLE WATER TANK

                case "POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (828, 'APPLICATION_TNK-WPOT', 224, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (877, 'TNK-WPOT_TYP', 224, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (961, 'UNDERGROUND', 224, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1135, 'TNK_MATERIAL', 224, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1235, 'TNK_AUTO_REFILL', 224, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1250, 'TNK_STATE_INSPECTION_REQ', 224, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1354, 'TNK_PRESSURE_RATING', 224, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1535, 'LOCATION', 224, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1669, 'OWNED_BY', 224, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1670, 'TNK_VOLUME', 224, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1671, 'TNK_SIDE_LENGTH', 224, 2, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1672, 'TNK_DIAMETER', 224, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1673, 'SPECIAL_MAINT_NOTES_DIST', 224, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2231, 'NARUC_MAINTENANCE_ACCOUNT', 224, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2232, 'NARUC_OPERATIONS_ACCOUNT', 224, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2477, 'NARUC_SPECIAL_MAINT_NOTES', 224, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2478, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 224, 1, 0, 1, 1, 17);
";

                #endregion

                #region POWER BREAKER

                case "POWER BREAKER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (797, 'AMP_RATING', 200, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (924, 'PWRBRKR_TYP', 200, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1117, 'BREAKER_TP', 200, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1356, 'VOLT_RATING', 200, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1956, 'OWNED_BY', 200, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2169, 'NARUC_MAINTENANCE_ACCOUNT', 200, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2170, 'NARUC_OPERATIONS_ACCOUNT', 200, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2430, 'NARUC_SPECIAL_MAINT_NOTES', 200, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2431, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 200, 1, 0, 1, 1, 9);
";

                #endregion

                #region POWER CONDITIONER

                case "POWER CONDITIONER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (798, 'AMP_RATING', 201, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1199, 'ADJUSTABLE', 201, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1335, 'PWRCOND_TYP', 201, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1389, 'VOLT_RATING', 201, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1957, 'OWNED_BY', 201, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2171, 'NARUC_MAINTENANCE_ACCOUNT', 201, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2172, 'NARUC_OPERATIONS_ACCOUNT', 201, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2432, 'NARUC_SPECIAL_MAINT_NOTES', 201, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2433, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 201, 1, 0, 1, 1, 9);
";

                #endregion

                #region POWER DISCONNECT

                case "POWER DISCONNECT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (796, 'AMP_CURRRATING', 202, 2, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (801, 'AMP_RATING', 202, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (892, 'PWRDISC_TYP', 202, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1251, 'VOLT_RATING', 202, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1958, 'OWNED_BY', 202, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1959, 'FUSE_SIZE', 202, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2173, 'NARUC_MAINTENANCE_ACCOUNT', 202, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2174, 'NARUC_OPERATIONS_ACCOUNT', 202, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2434, 'NARUC_SPECIAL_MAINT_NOTES', 202, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2435, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 202, 1, 0, 1, 1, 10);
";

                #endregion

                #region POWER FEEDER CABLE

                case "POWER FEEDER CABLE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (799, 'AMP_RATING', 203, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (919, 'WIRE_INSULATION_TP', 203, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1012, 'PWRFEEDR_TYP', 203, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1522, 'VOLT_RATING', 203, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1960, 'OWNED_BY', 203, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1961, 'WIRE_SIZE', 203, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2175, 'NARUC_MAINTENANCE_ACCOUNT', 203, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2176, 'NARUC_OPERATIONS_ACCOUNT', 203, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2436, 'NARUC_SPECIAL_MAINT_NOTES', 203, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2437, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 203, 1, 0, 1, 1, 10);
";

                #endregion

                #region POWER MONITOR

                case "POWER MONITOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (802, 'AMP_RATING', 204, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1264, 'VOLT_RATING', 204, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1294, 'PWRMON_TYP', 204, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1523, 'ADJUSTABLE', 204, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1962, 'OWNED_BY', 204, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1963, 'SYSTEM_MONITORED', 204, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2177, 'NARUC_MAINTENANCE_ACCOUNT', 204, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2178, 'NARUC_OPERATIONS_ACCOUNT', 204, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2438, 'NARUC_SPECIAL_MAINT_NOTES', 204, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2439, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 204, 1, 0, 1, 1, 10);
";

                #endregion

                #region POWER PANEL

                case "POWER PANEL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (800, 'AMP_RATING', 205, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (860, 'NUMBER_OF_BREAKERS', 205, 2, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (874, 'FUSED', 205, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1211, 'VOLT_RATING', 205, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1442, 'PWRPNL_TYP', 205, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1964, 'OWNED_BY', 205, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1965, 'FUSE_SIZE', 205, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2179, 'NARUC_MAINTENANCE_ACCOUNT', 205, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2180, 'NARUC_OPERATIONS_ACCOUNT', 205, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2440, 'NARUC_SPECIAL_MAINT_NOTES', 205, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2441, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 205, 1, 0, 1, 1, 11);
";

                #endregion

                #region POWER RELAY

                case "POWER RELAY":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (809, 'AMP_RATING', 206, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1429, 'VOLT_RATING', 206, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1449, 'PWRRELAY_TYP', 206, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1520, 'BREAKER_NAME', 206, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1557, 'PHASE_ID', 206, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1966, 'OWNED_BY', 206, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1967, 'STYLE_NUMBER', 206, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2181, 'NARUC_MAINTENANCE_ACCOUNT', 206, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2182, 'NARUC_OPERATIONS_ACCOUNT', 206, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2442, 'NARUC_SPECIAL_MAINT_NOTES', 206, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2443, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 206, 1, 0, 1, 1, 11);
";

                #endregion

                #region POWER SURGE PROTECTION

                case "POWER SURGE PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (810, 'AMP_RATING', 207, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (989, 'VOLT_RATING', 207, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1002, 'SYSTEM_PROTECTED', 207, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1109, 'PWRSURG_TYP', 207, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1968, 'OWNED_BY', 207, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2183, 'NARUC_MAINTENANCE_ACCOUNT', 207, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2184, 'NARUC_OPERATIONS_ACCOUNT', 207, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2444, 'NARUC_SPECIAL_MAINT_NOTES', 207, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2445, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 207, 1, 0, 1, 1, 9);
";

                #endregion

                #region POWER TRANSFER SWITCH

                case "POWER TRANSFER SWITCH":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (819, 'AMP_RATING', 227, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (983, 'TRAN-SW_TYP', 227, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1092, 'VOLT_RATING', 227, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1123, 'PROGRAMMABLE', 227, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1792, 'OWNED_BY', 227, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1793, 'SPECIAL_MAINT_NOTES', 227, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2237, 'NARUC_MAINTENANCE_ACCOUNT', 227, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2238, 'NARUC_OPERATIONS_ACCOUNT', 227, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2483, 'NARUC_SPECIAL_MAINT_NOTES', 227, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2484, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 227, 1, 0, 1, 1, 10);
";

                #endregion

                #region PRESSURE DAMPER

                case "PRESSURE DAMPER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1287, 'PRESDMP_TYP', 197, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2161, 'OWNED_BY', 197, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2162, 'SPECIAL_MAINT_NOTES', 197, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2163, 'NARUC_MAINTENANCE_ACCOUNT', 197, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2164, 'NARUC_OPERATIONS_ACCOUNT', 197, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2424, 'NARUC_SPECIAL_MAINT_NOTES', 197, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2425, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 197, 1, 0, 1, 1, 7);
";

                #endregion

                #region PRINTER

                case "PRINTER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1028, 'PRNTR_TYP', 198, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1234, 'STANDBY_POWER_TP', 198, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1394, 'APPLICATION_PRNTR', 198, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1425, 'NETWORK_SCHEME', 198, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1487, 'OPERATING_SYSTEMS', 198, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1582, 'RAID', 198, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1951, 'OWNED_BY', 198, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1952, 'RAM_MEMORY', 198, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1953, 'SOFTWARE LICENSE #', 198, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2165, 'NARUC_MAINTENANCE_ACCOUNT', 198, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2166, 'NARUC_OPERATIONS_ACCOUNT', 198, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2426, 'NARUC_SPECIAL_MAINT_NOTES', 198, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2427, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 198, 1, 0, 1, 1, 13);
";

                #endregion

                #region PUMP CENTRIFUGAL

                case "PUMP CENTRIFUGAL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (830, 'BHP_RATING', 190, 2, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (901, 'LUBE_TP_2', 190, 5, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (980, 'PMP_BEARING_TP_FREE_END', 190, 5, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1016, 'PMP_DISCHARGE_SIZE', 190, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1018, 'ROTATION_DIRECTION', 190, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1087, 'ORIENTATION', 190, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1270, 'PMP_MATERIAL', 190, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1301, 'PMP_INLET_SIZE', 190, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1307, 'PMP_STAGES', 190, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1359, 'PMP_BEARING_TP_COUP_END', 190, 5, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1403, 'PMP_IMPELLER_MATL', 190, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1404, 'PMP_SEAL_TP', 190, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1446, 'LUBE_TP', 190, 5, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1452, 'APPLICATION_PMP-CENT', 190, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1471, 'PMP-CENT_TYP', 190, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1492, 'FLOW_UOM', 190, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1512, 'RPM_RATING', 190, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1614, 'OWNED_BY', 190, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1615, 'FLOW_RATING', 190, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1616, 'PMP_TDH_RATING', 190, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1617, 'FLOW_MAXIMUM', 190, 2, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1618, 'PMP_IMPELLER_SIZE', 190, 2, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1619, 'PMP_EFICIENCY', 190, 2, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1620, 'PMP_SHUT_OFF_HEAD', 190, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1621, 'PMP_NPSH_RATING', 190, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1623, 'BEARINGNUM_FREE_END', 190, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1624, 'SPECIAL_MAINT_NOTES_DIST', 190, 5, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1689, 'PMP-BEARING#', 190, 1, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1857, 'HORSE_POWER', 190, 1, 0, 0, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1858, 'VOLTAGE', 190, 1, 0, 0, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2145, 'NARUC_MAINTENANCE_ACCOUNT', 190, 1, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2146, 'NARUC_OPERATIONS_ACCOUNT', 190, 1, 0, 1, 1, 32);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2410, 'NARUC_SPECIAL_MAINT_NOTES', 190, 1, 0, 1, 1, 33);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2411, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 190, 1, 0, 1, 1, 34);
";

                #endregion

                #region PUMP GRINDER

                case "PUMP GRINDER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (833, 'BHP_RATING', 191, 2, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (870, 'PMP_STAGES', 191, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (917, 'LUBE_TP_2', 191, 5, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (933, 'ROTATION_DIRECTION', 191, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (968, 'RPM_RATING', 191, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (993, 'LUBE_TP', 191, 5, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1061, 'PMP_BEARING_TP_FREE_END', 191, 5, 0, 1, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1072, 'PMP_BEARING_TP_COUP_END', 191, 5, 0, 1, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1114, 'PMP-GRND_TYP', 191, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1138, 'ORIENTATION', 191, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1255, 'PMP_DISCHARGE_SIZE', 191, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1295, 'PMP_SEAL_TP', 191, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1299, 'FLOW_UOM', 191, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1324, 'PMP_MATERIAL', 191, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1432, 'APPLICATION_PMP-GRND', 191, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1495, 'PMP_IMPELLER_MATL', 191, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1555, 'PMP_INLET_SIZE', 191, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1625, 'OWNED_BY', 191, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1626, 'FLOW_RATING', 191, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1627, 'PMP_TDH_RATING', 191, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1628, 'FLOW_MAXIMUM', 191, 2, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1629, 'PMP_EFICIENCY', 191, 2, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1630, 'PMP_SHUT_OFF_HEAD', 191, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1631, 'PMP_NPSH_RATING', 191, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1634, 'SPECIAL_MAINT_NOTES_DIST', 191, 5, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1796, 'PMP_IMPELLER_SIZE', 191, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1797, 'PMP_BEARINGTP-UPPER/INNER', 191, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1798, 'PMP_BEARING#(UPPER/INNER)', 191, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1799, 'PMP_BEARINGTP-LOWER/OUTER', 191, 1, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1800, 'PMP-BEARING#(LOWER/OUTER)', 191, 1, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2147, 'NARUC_MAINTENANCE_ACCOUNT', 191, 1, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2148, 'NARUC_OPERATIONS_ACCOUNT', 191, 1, 0, 1, 1, 32);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2412, 'NARUC_SPECIAL_MAINT_NOTES', 191, 1, 0, 1, 1, 33);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2413, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 191, 1, 0, 1, 1, 34);
";

                #endregion

                #region PUMP POSITIVE DISPLACEMENT

                case "PUMP POSITIVE DISPLACEMENT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (832, 'BHP_RATING', 192, 2, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1224, 'PMP_IMPELLER_MATL', 192, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1321, 'PMP_INLET_SIZE', 192, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1328, 'PMP_SEAL_TP', 192, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1332, 'PMP_DISCHARGE_SIZE', 192, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1346, 'LUBE_TP', 192, 5, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1368, 'LUBE_TP_2', 192, 5, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1383, 'ROTATION_DIRECTION', 192, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1405, 'RPM_RATING', 192, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1409, 'PMP_BEARING_TP_FREE_END', 192, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1411, 'PMP-PD_TYP', 192, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1438, 'PMP_MATERIAL', 192, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1478, 'ORIENTATION', 192, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1508, 'PMP_BEARING_TP_COUP_END', 192, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1532, 'PMP_STAGES', 192, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1547, 'APPLICATION_PMP-PD', 192, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1587, 'FLOW_UOM', 192, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1635, 'OWNED_BY', 192, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1636, 'FLOW_RATING', 192, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1637, 'PMP_TDH_RATING', 192, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1638, 'FLOW_MAXIMUM', 192, 2, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1639, 'PMP_IMPELLER_SIZE', 192, 2, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1640, 'PMP_EFICIENCY', 192, 2, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1641, 'PMP_SHUT_OFF_HEAD', 192, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1642, 'PMP_NPSH_RATING', 192, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1645, 'SPECIAL_MAINT_NOTES_DIST', 192, 5, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1859, 'PMP-BEARINGTP-UPPER/INNER', 192, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1860, 'PMP-BEARING#(UPPER/INNER)', 192, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1861, 'PMP-BEARINGTP-LOWER/OUTER', 192, 1, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1862, 'PMP-BEARING#(LOWER/OUTER)', 192, 1, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2149, 'NARUC_MAINTENANCE_ACCOUNT', 192, 1, 0, 1, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2150, 'NARUC_OPERATIONS_ACCOUNT', 192, 1, 0, 1, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2414, 'NARUC_SPECIAL_MAINT_NOTES', 192, 1, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2415, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 192, 1, 0, 1, 1, 32);
";

                #endregion

                #region RECORDER

                case "RECORDER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1010, 'INSTRUMENT_UOM', 208, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1038, 'ON_SCADA', 208, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1049, 'OUTPUT_TP', 208, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1141, 'NEMA_ENCLOSURE', 208, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1271, 'RECORDER_TYP', 208, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1549, 'LOOP_POWER', 208, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1969, 'OWNED_BY', 208, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1970, 'INSTRUMENT_RANGE', 208, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2185, 'NARUC_MAINTENANCE_ACCOUNT', 208, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2186, 'NARUC_OPERATIONS_ACCOUNT', 208, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2446, 'NARUC_SPECIAL_MAINT_NOTES', 208, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2533, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 208, 1, 0, 1, 1, 11);
";

                #endregion

                #region RESPIRATOR

                case "RESPIRATOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (964, 'PPE-RESP_TYP', 196, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1417, 'GAS_MITIGATED', 196, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1443, 'RESPIRATOR_RATING', 196, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1794, 'OWNED_BY', 196, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1795, 'SPECIAL_MAINT_NOTES', 196, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2159, 'NARUC_MAINTENANCE_ACCOUNT', 196, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2160, 'NARUC_OPERATIONS_ACCOUNT', 196, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2422, 'NARUC_SPECIAL_MAINT_NOTES', 196, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2423, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 196, 1, 0, 1, 1, 9);
";

                #endregion

                #region RTU - PLC

                case "RTU - PLC":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (891, 'APPLICATION_RTU-PLC', 209, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (944, 'COMM3_DEVICE', 209, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (956, 'COMM6_TP', 209, 5, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (976, 'COMM1_TP', 209, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (977, 'COMM5_DEVICE', 209, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (988, 'RTU-PLC_TYP', 209, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1067, 'END_NODE', 209, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1076, 'COMM4_DEVICE', 209, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1082, 'COMM2_TP', 209, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1118, 'STANDBY_POWER_TP', 209, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1242, 'REMOTE_IO', 209, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1379, 'COMM2_DEVICE', 209, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1489, 'VOLT_RATING_RTU-PLC', 209, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1506, 'COMM1_DEVICE', 209, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1521, 'COMM4_TP', 209, 5, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1524, 'COMM6_DEVICE', 209, 5, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1525, 'COMM5_TP', 209, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1558, 'COMM3_TP', 209, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1646, 'OWNED_BY', 209, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1647, 'CPU_BATTERY', 209, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1648, 'SPECIAL_MAINT_NOTES_DIST', 209, 5, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2187, 'NARUC_MAINTENANCE_ACCOUNT', 209, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2188, 'NARUC_OPERATIONS_ACCOUNT', 209, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2447, 'NARUC_SPECIAL_MAINT_NOTES', 209, 1, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2448, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 209, 1, 0, 1, 1, 25);
";

                #endregion

                #region SAFETY SHOWER

                case "SAFETY SHOWER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1348, 'SAF-SHWR_TYP', 211, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1450, 'RETEST_REQUIRED', 211, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1971, 'OWNED_BY', 211, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2194, 'NARUC_MAINTENANCE_ACCOUNT', 211, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2195, 'NARUC_OPERATIONS_ACCOUNT', 211, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2451, 'NARUC_SPECIAL_MAINT_NOTES', 211, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2452, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 211, 1, 0, 1, 1, 7);
";

                #endregion

                #region SCADA RADIO

                case "SCADA RADIO":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (822, 'ANNUAL_COST', 141, 3, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (896, 'BAUD_RATE', 141, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (908, 'COMMUNICATION_TP', 141, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (928, 'STANDBY_POWER_TP', 141, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1127, 'APPLICATION_COMM-RAD', 141, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1351, 'ANTENNA_TP', 141, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1445, 'COMM-RAD_TYP', 141, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1768, 'OWNED_BY', 141, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1769, 'FCC_LICENSE#', 141, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1770, 'POWER_WATTS', 141, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1771, 'TRANSMIT_FREQUENCY', 141, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1772, 'RECEIVE_FREQUENCY', 141, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1773, 'SPECIAL_MAINT_NOTES', 141, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2048, 'NARUC_MAINTENANCE_ACCOUNT', 141, 1, 0, 1, 0, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2049, 'NARUC_OPERATIONS_ACCOUNT', 141, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2314, 'NARUC_SPECIAL_MAINT_NOTES', 141, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2315, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 141, 1, 0, 1, 1, 17);
";

                #endregion

                #region SCADA SYSTEM GEN

                case "SCADA SYSTEM GEN":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1230, 'SCADASYS_TYP', 212, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1972, 'SCADA_HMI AND DATABASE', 212, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1973, 'OWNED_BY', 212, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2196, 'SPECIAL_MAINT_NOTES', 212, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2197, 'NARUC_MAINTENANCE_ACCOUNT', 212, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2198, 'NARUC_OPERATIONS_ACCOUNT', 212, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2453, 'NARUC_SPECIAL_MAINT_NOTES', 212, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2454, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 212, 1, 0, 1, 1, 7);
";

                #endregion

                #region SCALE

                case "SCALE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (995, 'ON_SCADA', 213, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1106, 'INSTRUMENT_UOM', 213, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1240, 'LOOP_POWER', 213, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1433, 'NEMA_ENCLOSURE', 213, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1530, 'SCALE_TYP', 213, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1585, 'OUTPUT_TP', 213, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1974, 'OWNED_BY', 213, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1975, 'INSTRUMENT_RANGE', 213, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2199, 'NARUC_MAINTENANCE_ACCOUNT', 213, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2200, 'NARUC_OPERATIONS_ACCOUNT', 213, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2455, 'NARUC_SPECIAL_MAINT_NOTES', 213, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2456, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 213, 1, 0, 1, 1, 12);
";

                #endregion

                #region SCREEN

                case "SCREEN":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (878, 'AUTO_WASH', 215, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (897, 'LOCATION', 215, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1171, 'SCREEN_TYP', 215, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1527, 'APPLICATION_SCREEN', 215, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1976, 'OWNED_BY', 215, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1977, 'RPM_OPERATING', 215, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2203, 'NARUC_MAINTENANCE_ACCOUNT', 215, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2204, 'NARUC_OPERATIONS_ACCOUNT', 215, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2459, 'NARUC_SPECIAL_MAINT_NOTES', 215, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2460, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 215, 1, 0, 1, 1, 10);
";

                #endregion

                #region SCRUBBER

                case "SCRUBBER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (994, 'MEDIA_2_TP_SCRBBR', 214, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1022, 'MATERIAL_OF_CONSTRUCTION', 214, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1058, 'MEDIA_1_TP_SCRBBR', 214, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1070, 'SCRBBR_TYP', 214, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1095, 'LOCATION', 214, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1254, 'MEDIA_REGENERATION_REQD', 214, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1269, 'APPLICATION_SCRBBR', 214, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1305, 'WASH_TP', 214, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1810, 'OWNED_BY', 214, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1811, 'SURFACE_AREA_SQFT', 214, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1812, 'MEDIA_1_DEPTH', 214, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1813, 'MEDIA_2_DEPTH', 214, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1814, 'FLOW_NORMAL_RANGE', 214, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1815, 'BACK_WASH_RATE GPM/SQFT', 214, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1817, 'SPECIAL_MAINT_NOTES', 214, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2201, 'NARUC_MAINTENANCE_ACCOUNT', 214, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2202, 'NARUC_OPERATIONS_ACCOUNT', 214, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2457, 'NARUC_SPECIAL_MAINT_NOTES', 214, 1, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2458, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 214, 1, 0, 1, 1, 19);
";

                #endregion

                #region SECONDARY CONTAINMENT

                case "SECONDARY CONTAINMENT":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1097, 'UNDERGROUND', 147, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1286, 'CONTAIN_TYP', 147, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1291, 'LOCATION', 147, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1326, 'APPLICATION_CONTAIN', 147, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1406, 'TNK_STATE_INSPECTION_REQ', 147, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1886, 'OWNED_BY', 147, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1887, 'TNK_VOLUME (GAL)', 147, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2060, 'NARUC_MAINTENANCE_ACCOUNT', 147, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2061, 'NARUC_OPERATIONS_ACCOUNT', 147, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2326, 'NARUC_SPECIAL_MAINT_NOTES', 147, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2327, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 147, 1, 0, 1, 1, 11);
";

                #endregion

                #region SECURITY SYSTEM

                case "SECURITY SYSTEM":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (940, 'ACTION_TAKEN_UPON_ALARM', 216, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1217, 'RETEST_REQUIRED', 216, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1258, 'BACKUP_POWER', 216, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1340, 'SECSYS_TYP', 216, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1978, 'OWNED_BY', 216, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2205, 'NARUC_MAINTENANCE_ACCOUNT', 216, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2206, 'NARUC_OPERATIONS_ACCOUNT', 216, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2461, 'NARUC_SPECIAL_MAINT_NOTES', 216, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2462, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 216, 1, 0, 1, 1, 9);
";

                #endregion

                #region SERVER

                case "SERVER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (882, 'OPERATING_SYSTEMS', 217, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1006, 'SERVR_TYP', 217, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1031, 'HMI_MANUFACTURER', 217, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1189, 'STANDBY_POWER_TP', 217, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1322, 'RAID', 217, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1574, 'NETWORK_SCHEME', 217, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1581, 'APPLICATION_SERVR', 217, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1979, 'OWNED_BY', 217, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1980, 'RAM_MEMORY', 217, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1981, 'HMI_SOFTWARE', 217, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1982, 'SOFTWARE_LICENSE #', 217, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2207, 'NARUC_MAINTENANCE_ACCOUNT', 217, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2208, 'NARUC_OPERATIONS_ACCOUNT', 217, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2463, 'NARUC_SPECIAL_MAINT_NOTES', 217, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2464, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 217, 1, 0, 1, 1, 14);
";

                #endregion

                #region SOFTENER

                case "SOFTENER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (867, 'MATERIAL_OF_CONSTRUCTION', 232, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (946, 'WASH_TP', 232, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1188, 'LOCATION', 232, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1209, 'APPLICATION_TRT-SOFT', 232, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1228, 'MEDIA_2_TP_TRT-SOFT', 232, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1236, 'TRT-SOFT_TYP', 232, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1315, 'MEDIA_REGENERATION_REQD', 232, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1515, 'MEDIA_1_TP_TRT-SOFT', 232, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1835, 'OWNED_BY', 232, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1836, 'SURFACE_AREA_SQFT', 232, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1837, 'MEDIA_1_DEPTH', 232, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1838, 'MEDIA_2_DEPTH', 232, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1839, 'FLOW_NORMAL_RANGE', 232, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1840, 'BACKWASH_RATE', 232, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1841, 'SPECIAL_MAINT_NOTES', 232, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2247, 'NARUC_MAINTENANCE_ACCOUNT', 232, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2248, 'NARUC_OPERATIONS_ACCOUNT', 232, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2493, 'NARUC_SPECIAL_MAINT_NOTES', 232, 1, 0, 1, 0, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2494, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 232, 1, 0, 1, 1, 19);
";

                #endregion

                #region STREET VALVE

                case "STREET VALVE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (843, 'DEPENDENCY_DRIVER_1', 218, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (844, 'DEPENDENCY_DRIVER_2', 218, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (856, 'NORMAL_SYS_PRESSURE', 218, 2, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (861, 'NUMBER_OF_TURNS', 218, 2, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (876, 'OPEN_DIRECTION', 218, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (886, 'PRESSURE_CLASS', 218, 5, 0, 1, 1, 32);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (904, 'SPECIAL_MAINT_NOTES_DIST', 218, 5, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (921, 'APPLICATION_SVLV', 218, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (930, 'SURFACE_COVER', 218, 5, 0, 1, 1, 30);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1003, 'VLV_OPER_NUT_SIZE', 218, 5, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1037, 'BYPASS_VALVE', 218, 5, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1040, 'GEAR_TP', 218, 5, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1047, 'ON_SCADA', 218, 5, 0, 1, 1, 36);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1102, 'SURFACE_COVER_LOC_TP', 218, 5, 0, 1, 1, 31);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1136, 'VLV_SEAT_TP', 218, 5, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1146, 'NORMAL_POSITION', 218, 5, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1162, 'ACCESS_TP', 218, 5, 0, 1, 1, 27);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1180, 'VLV_VALVE_SIZE', 218, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1285, 'JOINT_TP', 218, 5, 0, 1, 1, 33);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1349, 'SVLV_TYP', 218, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1381, 'EAM_PIPE_SIZE', 218, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1459, 'OPERATING_NUT_TP', 218, 5, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1460, 'PIPE_MATERIAL', 218, 5, 0, 1, 1, 35);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1476, 'VLV_SPECIAL_V_BOX_MARKING', 218, 5, 0, 1, 1, 29);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1501, 'ACTUATOR_TP', 218, 5, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1577, 'VLV_VALVE_TP', 218, 5, 0, 1, 1, 37);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1985, 'OWNED_BY', 218, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1986, 'SPECIAL_MAINT_NOTES_DETAILS', 218, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1987, 'STREET_VALVE_TYPE', 218, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1988, 'SUBDIVISION', 218, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1989, 'PRESSURE_ZONE', 218, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1990, 'PRESSURE_ZONE_HGL', 218, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1991, 'MAP_PAGE', 218, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1992, 'BOOK_PAGE', 218, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1993, 'TORQUE_LIMIT', 218, 1, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1994, 'VLV_DEPTH_TOP_OF_MAIN', 218, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1995, 'VLV_TOP_VALVE_NUT_DEPTH', 218, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1996, 'VALVE_BOX_MARKING', 218, 1, 0, 1, 1, 28);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1997, 'PIPE_CHANNEL_SIZE', 218, 1, 0, 1, 1, 34);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1998, 'INSTALLATION_ WO#', 218, 1, 0, 1, 1, 38);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1999, 'SKETCH_#', 218, 1, 0, 1, 1, 39);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2000, 'HISTORICAL_ID', 218, 1, 0, 1, 1, 40);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2001, 'GEOACCURACY_GIS_DATASOURCETYPE', 218, 1, 0, 1, 1, 41);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2209, 'NARUC_MAINTENANCE_ACCOUNT', 218, 1, 0, 1, 1, 42);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2210, 'NARUC_OPERATIONS_ACCOUNT', 218, 1, 0, 1, 1, 43);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2465, 'NARUC_SPECIAL_MAINT_NOTES', 218, 1, 0, 1, 1, 44);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2466, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 218, 1, 0, 1, 1, 45);
";

                #endregion

                #region TELEPHONE

                case "TELEPHONE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (821, 'ANNUAL_COST', 144, 3, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1013, 'COMM-TEL_TYP', 144, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1164, 'COMMUNICATION_TP', 144, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1244, 'STANDBY_POWER_TP', 144, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1273, 'APPLICATION_COMM-TEL', 144, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1375, 'ANTENNA_TP', 144, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1517, 'BAUD_RATE', 144, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1884, 'OWNED_BY', 144, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1885, 'POWER (WATTS)', 144, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2054, 'NARUC_MAINTENANCE_ACCOUNT', 144, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2055, 'NARUC_OPERATIONS_ACCOUNT', 144, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2320, 'NARUC_SPECIAL_MAINT_NOTES', 144, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2321, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 144, 1, 0, 1, 1, 13);
";

                #endregion

                #region TOOL

                case "TOOL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (999, 'TOOL_TYP', 226, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1005, 'CAPACITY_UOM_E', 226, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1229, 'RETEST_REQUIRED', 226, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1789, 'OWNED_BY', 226, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1790, 'CAPACITY_RATING', 226, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1791, 'SPECIAL_MAINT_NOTES', 226, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2235, 'NARUC_MAINTENANCE_ACCOUNT', 226, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2236, 'NARUC_OPERATIONS_ACCOUNT', 226, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2481, 'NARUC_SPECIAL_MAINT_NOTES', 226, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2482, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 226, 1, 0, 1, 1, 10);
";

                #endregion

                #region TRANSFORMER

                case "TRANSFORMER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (818, 'AMP_RATING', 239, 2, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (871, 'XFMR_TYP', 239, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (889, 'MOUNTING_XFMR', 239, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (945, 'INSULATION_CLASS', 239, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (966, 'PHASES', 239, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1104, 'XFMR_PCBS', 239, 5, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1170, 'VOLT_RATING_SECONDARY', 239, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1190, 'VOLT_RATING', 239, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1241, 'NEMA_ENCLOSURE', 239, 5, 0, 1, 1, 19);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1314, 'APPLICATION_XFMR', 239, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1426, 'XFMR_WINDING_SEC', 239, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1499, 'XFMR_WINDING_PRI', 239, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1818, 'OWNED_BY', 239, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1819, 'AMPS_SECONDARY', 239, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1820, 'TEMPERATURE_RISE', 239, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1821, 'IMPEDANCE_%', 239, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1822, 'KVA_RATED', 239, 1, 1, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1823, 'OIL_CAPACITY_GAL', 239, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1825, 'OIL_TYPE', 239, 1, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1826, 'WEIGHT_LBS', 239, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1827, 'TAP_RANGE_TAP', 239, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1828, 'BIL_RATING', 239, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1829, 'SPECIAL_MAINT_NOTES', 239, 1, 0, 1, 1, 23);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1984, 'CONTAINS_PCB''S', 239, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2261, 'NARUC_MAINTENANCE_ACCOUNT', 239, 1, 0, 1, 1, 24);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2262, 'NARUC_OPERATIONS_ACCOUNT', 239, 1, 0, 1, 1, 25);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2507, 'NARUC_SPECIAL_MAINT_NOTES', 239, 1, 0, 1, 1, 26);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2508, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 239, 1, 0, 1, 1, 27);
";

                #endregion

                #region TRANSMITTER

                case "TRANSMITTER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (918, 'ON_SCADA', 240, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1353, 'XMTR_TYP', 240, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1386, 'INSTRUMENT_UOM', 240, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1414, 'OUTPUT_TP', 240, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1494, 'NEMA_ENCLOSURE', 240, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1543, 'LOOP_POWER', 240, 5, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1749, 'OWNED_BY', 240, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1750, 'INSTRUMENT_RANGE', 240, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1751, 'SPECIAL_MAINT_NOTE', 240, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2263, 'NARUC_MAINTENANCE_ACCOUNT', 240, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2264, 'NARUC_OPERATIONS_ACCOUNT', 240, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2509, 'NARUC_SPECIAL_MAINT_NOTES', 240, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2510, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 240, 1, 0, 1, 1, 13);
";

                #endregion

                #region UNINTERUPTED POWER SUPPLY

                case "UNINTERUPTED POWER SUPPLY":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (820, 'AMP_RATING', 235, 2, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (906, 'VOLT_RATING', 235, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1329, 'UPS_TYP', 235, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2251, 'OWNED_BY', 235, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2252, 'SPECIAL_MAINT_NOTE', 235, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2253, 'NARUC_MAINTENANCE_ACCOUNT', 235, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2254, 'NARUC_OPERATIONS_ACCOUNT', 235, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2499, 'NARUC_SPECIAL_MAINT_NOTES', 235, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2500, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 235, 1, 0, 1, 1, 9);
";

                #endregion

                #region UV SANITIZER

                case "UV SANITIZER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1303, 'TRT-UV_TYP', 234, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2265, 'OWNED_BY', 234, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2266, 'PEAK_PROCESS_FLOW (GPM)', 234, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2267, 'RETENTION_TIME (SECONDS)', 234, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2268, 'NUMBER_LAMPS/MODULES', 234, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2269, 'NUMBER_OF_UV_MODULES', 234, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2270, 'SPECIAL_MAINT_NOTE', 234, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2271, 'SPECIAL_MAINT_NOTE_DETAILS', 234, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2272, 'NARUC_MAINTENANCE_ACCOUNT', 234, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2273, 'NARUC_OPERATIONS_ACCOUNT', 234, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2497, 'NARUC_SPECIAL_MAINT_NOTES', 234, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2498, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 234, 1, 0, 1, 1, 12);
";

                #endregion

                #region UV-SOUND

                case "UV-SOUND":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2523, 'UV_SOUND_TYP', 242, 1, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2524, 'APPLICATION', 242, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2525, 'ANALYZER', 242, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2526, 'WQ_SNSR_PCKGE', 242, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2527, 'ALM_BUOY_SYS', 242, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2528, 'FLTING_SLR_PNL_SYS', 242, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2529, 'INT_ALG_CNTRL_SRVCS', 242, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2530, 'RANGE', 242, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2531, 'NARUC_SPECIAL_MAINT_NOTES', 242, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2532, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 242, 1, 0, 1, 1, 10);
";

                #endregion

                #region VEHICLE

                case "VEHICLE":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (883, 'VEH_TYP', 236, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1044, 'CAPACITY_UOM', 236, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1542, 'RETEST_REQUIRED', 236, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1854, 'OWNED_BY', 236, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1855, 'CAPACITY_RATING', 236, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1856, 'SPECIAL_MAINT_NOTES', 236, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2255, 'NARUC_MAINTENANCE_ACCOUNT', 236, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2256, 'NARUC_OPERATIONS_ACCOUNT', 236, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2501, 'NARUC_SPECIAL_MAINT_NOTES', 236, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2502, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 236, 1, 0, 1, 1, 10);
";

                #endregion

                #region VOC STRIPPER

                case "VOC STRIPPER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1032, 'MEDIA_REGENERATION_REQD', 233, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1055, 'MEDIA_1_TP_TRT-STRP', 233, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1140, 'MEDIA_2_TP_TRT-STRP', 233, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1142, 'MATERIAL_OF_CONSTRUCTION', 233, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1202, 'LOCATION', 233, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1318, 'TRT-STRP_TYP', 233, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1325, 'WASH_TP', 233, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1413, 'APPLICATION_TRT-STRP', 233, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2002, 'OWNED_BY', 233, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2003, 'SURFACE_AREA', 233, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2004, 'MEDIA_1_DEPTH', 233, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2005, 'FLOW_NORMAL_RANGE', 233, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2006, 'BACKWASH_RATE', 233, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2007, 'SPECIAL_MAINT_NOTES', 233, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2249, 'NARUC_MAINTENANCE_ACCOUNT', 233, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2250, 'NARUC_OPERATIONS_ACCOUNT', 233, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2495, 'NARUC_SPECIAL_MAINT_NOTES', 233, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2496, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 233, 1, 0, 1, 1, 18);
";

                #endregion

                #region WASTE TANK

                case "WASTE TANK":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1080, 'TNK_PRESSURE_RATING', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1115, 'TNK_AUTO_REFILL', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1116, 'TNK_MATERIAL', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1175, 'UNDERGROUND', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1216, 'LOCATION', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1308, 'TNK-WSTE_TYP', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1357, 'TNK_STATE_INSPECTION_REQ', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1497, 'APPLICATION_TNK-WSTE', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1674, 'OWNED_BY', 225, 1, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1675, 'TNK_VOLUME', 225, 2, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1676, 'TNK_SIDE_LENGTH', 225, 2, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1677, 'TNK_DIAMETER', 225, 2, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1678, 'SPECIAL_MAINT_NOTES_DIST', 225, 5, 0, 1, 1, 0);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2233, 'NARUC_MAINTENANCE_ACCOUNT', 225, 1, 0, 1, 1, 20);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2234, 'NARUC_OPERATIONS_ACCOUNT', 225, 1, 0, 1, 1, 21);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2479, 'NARUC_SPECIAL_MAINT_NOTES', 225, 1, 0, 1, 1, 22);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2480, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 225, 1, 0, 1, 1, 23);
";

                #endregion

                #region WATER HEATER

                case "WATER HEATER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (808, 'AMP_RATING', 174, 2, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1129, 'ENERGY_TP', 174, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1292, 'OUTPUT_UOM', 174, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1310, 'HVAC-WH_TYP', 174, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1336, 'VOLT_RATING', 174, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1355, 'DUTY_CYCLE', 174, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1468, 'APPLICATION_HVAC-WH', 174, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1919, 'OWNED_BY', 174, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1920, 'OUTPUT_VALUE', 174, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2108, 'NARUC_MAINTENANCE_ACCOUNT', 174, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2109, 'NARUC_OPERATIONS_ACCOUNT', 174, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2380, 'NARUC_SPECIAL_MAINT_NOTES', 174, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2381, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 174, 1, 0, 1, 1, 13);
";

                #endregion

                #region WATER QUALITY ANALYZER

                case "WATER QUALITY ANALYZER":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (938, 'NEMA_ENCLOSURE', 238, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (979, 'OUTPUT_TP', 238, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1020, 'WQ_TEMPARATURE', 238, 5, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1023, 'VOLT_RATING', 238, 5, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1120, 'STANDBY_POWER_TP', 238, 5, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1125, 'WQANLZR_TYP', 238, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1155, 'TRANSMITTER', 238, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1274, 'COMM_PROTOCOL', 238, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1382, 'ON_SCADA', 238, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1470, 'LOOP_POWER', 238, 5, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1830, 'OWNED_BY', 238, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1831, 'HIGH_ALARM_SETPOINT', 238, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1832, 'LOW_ALARM_SETPOINT', 238, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1833, 'MAX_PRESSURE', 238, 1, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1834, 'SPECIAL_MAINT_NOTES', 238, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2259, 'NARUC_MAINTENANCE_ACCOUNT', 238, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2260, 'NARUC_OPERATIONS_ACCOUNT', 238, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2505, 'NARUC_SPECIAL_MAINT_NOTES', 238, 1, 0, 1, 1, 18);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2506, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 238, 1, 0, 1, 1, 19);
";

                #endregion

                #region WATER TREATMENT CONTACTOR

                case "WATER TREATMENT CONTACTOR":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (937, 'MEDIA_2_TP_TRT-CONT', 230, 5, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (949, 'MEDIA_1_TP_TRT-CONT', 230, 5, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1041, 'TRT-CONT_TYP', 230, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1128, 'LOCATION', 230, 5, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1212, 'MEDIA_REGENERATION_REQD', 230, 5, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1252, 'WASH_TP', 230, 5, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1347, 'MATERIAL_OF_CONSTRUCTION', 230, 5, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1458, 'APPLICATION_TRT-CONT', 230, 5, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1848, 'OWNED_BY', 230, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1849, 'SURFACE_AREA_SQFT', 230, 1, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1850, 'MEDIA_1_DEPTH', 230, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1851, 'MEDIA_2_DEPTH', 230, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1852, 'FLOW_NORMAL_RANGE', 230, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1853, 'BACKWASH_RATE (GPM/SQFT)', 230, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2243, 'NARUC_MAINTENANCE_ACCOUNT', 230, 1, 0, 1, 1, 15);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2244, 'NARUC_OPERATIONS_ACCOUNT', 230, 1, 0, 1, 1, 16);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2489, 'NARUC_SPECIAL_MAINT_NOTES', 230, 1, 0, 1, 1, 17);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2490, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 230, 1, 0, 1, 1, 18);
";

                #endregion

                #region WELL

                case "WELL":
                    return @"
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (864, 'WELL_CAPACITY_RATING', 237, 2, 0, 1, 1, 10);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (865, 'WELL_PERMIT_LAST_DATE', 237, 4, 0, 1, 1, 4);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1033, 'WELL_TYP', 237, 5, 0, 1, 1, 1);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1801, 'OWNED_BY', 237, 1, 0, 1, 1, 2);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1802, 'PERMIT#', 237, 1, 0, 1, 1, 3);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1804, 'PERMIT_DURATION (YRS)', 237, 1, 0, 1, 1, 5);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1805, 'DEPTH_IN (FT)', 237, 1, 0, 1, 1, 6);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1806, 'STATIC_WATER_LEVEL', 237, 1, 0, 1, 1, 7);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1807, 'DIAMETER_TOP (IN)', 237, 1, 0, 1, 1, 8);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1808, 'DIAMETER BOTTOM (IN)', 237, 1, 0, 1, 1, 9);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (1809, 'SPECIAL_MAINT_NOTES', 237, 1, 0, 1, 1, 11);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2257, 'NARUC_MAINTENANCE_ACCOUNT', 237, 1, 0, 1, 1, 12);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2258, 'NARUC_OPERATIONS_ACCOUNT', 237, 1, 0, 1, 1, 13);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2503, 'NARUC_SPECIAL_MAINT_NOTES', 237, 1, 0, 1, 1, 14);
INSERT INTO EquipmentCharacteristicFields (Id, FieldName, EquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (2504, 'NARUC_SPECIAL_MAINT_NOTE_DETAILS', 237, 1, 0, 1, 1, 15);
";

                #endregion

                default:
                    throw new InvalidOperationException(
                        $"{typeof(EquipmentCharacteristicFields).Name} for EquipmentType '{equipmentType}' have not been scripted.");
            }
        }
    }
}
