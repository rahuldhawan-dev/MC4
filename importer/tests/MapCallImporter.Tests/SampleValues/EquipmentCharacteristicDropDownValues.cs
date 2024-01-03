using System;

namespace MapCallImporter.SampleValues
{
    public static class EquipmentCharacteristicDropDownValues
    {
        public static string GetInsertQuery(string equipmentType)
        {
            switch (equipmentType)
            {
                #region ADJUSTABLE SPEED DRIVE

                case "ADJUSTABLE SPEED DRIVE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5173, 'ELECTROLYTIC (ADJUSTABLE)', 1462);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5174, 'MAGNETIC (ADJUSTABLE)', 1462);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5175, 'VFD', 1462);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5176, 'WOUND ROTOR (ADJUSTABLE)', 1462);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5177, 'XX', 1462);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8460, '6 PWM', 1260);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8461, '12 PWM', 1260);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8462, '18 PWM', 1260);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8463, '24 PWM', 1260);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9988, '6V', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9989, '12V', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9990, '24V', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9991, '120', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9992, '120/208', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9993, '120/240', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9994, '208Y/120', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9995, '208', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9996, '230', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9997, '230/460', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9998, '240', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9999, '277', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10000, '277/480', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10001, '460', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10002, '480', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10003, '600', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10004, '2300', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10005, '2300/4160', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10006, '2400', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10007, '4160', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10008, '13KV', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10009, '25KV', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10010, '33KV', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10011, 'OTHER', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10012, 'LESS THEN 100', 1461);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10013, '90VDC OR LESS', 1461);
";

                #endregion

                #region AERATOR

                case "AERATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6243, '304', 1045);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6244, '316L', 1045);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7433, '304', 1168);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7434, '316L', 1168);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9110, 'COARSE BUBBLER AERATOR', 1391);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9111, 'FINE BUBBLER AERATOR', 1391);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9112, 'FLOATING AERATOR', 1391);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9113, 'SUBMERGED ROTARY AERATOR', 1391);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9114, 'SURFACE ROTARY AERATOR', 1391);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9115, 'XX', 1391);
";

                #endregion

                #region AIR COMPRESSOR

                case "AIR COMPRESSOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5402, 'WATER PROCESSING', 920);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5403, 'WASTE PROCESSING', 920);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5404, 'PLANT UTILITY', 920);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5405, 'ENVIRONMENTAL COMPLIANCE', 920);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5406, 'OTHER', 920);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6218, 'CENTRIFUGAL COMP', 1083);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6219, 'PORTABLE POSITIVE DISP', 1083);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6220, 'STATIONARY CENTRIFUGAL', 1083);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6221, 'STATIONARY POSITIVE DISP', 1083);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6222, 'XX', 1083);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6275, 'COUPLING', 1415);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6276, 'BELT', 1415);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6277, 'GEAR', 1415);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8745, '1', 1187);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8746, '2', 1187);
";

                #endregion

                #region AIR/ VACUUM TANK

                case "AIR/ VACUUM TANK":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5727, 'PRESSURE', 1313);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5728, 'VACUUM', 1313);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7121, 'INDOORS', 1063);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7122, 'OUTDOORS', 1063);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8917, 'Y', 1158);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8918, 'N', 1158);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8919, 'XX', 1158);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8960, 'STEEL', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8961, 'STAINLESS STEEL', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8962, 'PLASTIC', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8963, 'FIBERGLASS', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8964, 'OTHER', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8965, 'CONCRETE', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8966, 'XX', 1479);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8984, 'VACUUM', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8985, 'ATMOSPHERIC', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8986, 'LESS THAN 100PSIG', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8987, 'LESS THAN 2PSIG', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8988, 'GREATER THAN 100PSIG', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8989, 'LESS THAN 15PSIG', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8990, '15-100PSIG', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8991, 'XX', 1081);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9034, 'Y', 998);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9035, 'N', 998);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9036, 'XX', 998);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9049, 'TNK-PVAC*', 1119);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9050, 'XX', 1119);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9157, 'Y', 1435);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9158, 'N', 1435);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9159, 'XX', 1435);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10311, 'ALERT CUSTOMERS BEFORE', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10312, 'SPECIAL FLUSHING CONCERN', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10313, 'VALVE CLOSED/PLUGGED', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10314, 'OPEN SLOWLY', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10315, 'CLOSE SLOWLY', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10316, 'NO VEHICLE ACCESS', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10317, 'BACK-SIDE YARD', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10318, 'PUMPOUT REQUIRED', 1663);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10319, 'OTHER', 1663);
";

                #endregion

                #region AM WATER NARUC ACCOUNT

                case "AM WATER NARUC ACCOUNT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7642, '2600', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7643, '2200', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7644, '2215', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7645, '2210', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7646, '2205', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7647, '2100', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7648, '2115', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7649, '2120', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7650, '2110', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7651, '2105', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7652, '2135', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7653, '2125', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7654, '2130', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7655, '2400', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7656, '2415', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7657, '2425', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7658, '2440', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7659, '2420', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7660, '2435', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7661, '2430', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7662, '2410', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7663, '2405', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7664, '2300', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7665, '2315', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7666, '2310', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7667, '2305', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7668, '9000', 1137);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7669, '1600', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7670, '1601', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7671, '1500', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7672, '1515', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7673, '1510', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7674, '1520', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7675, '1505', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7676, '1200', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7677, '1210', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7678, '1215', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7679, '1205', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7680, '1100', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7681, '1105', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7682, '1400', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7683, '1415', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7684, '1420', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7685, '1425', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7686, '1410', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7687, '1405', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7688, '1300', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7689, '1305', 1316);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7690, '9000', 1316);
";

                #endregion

                #region AMIDATACOLL

                case "AMIDATACOLL":
                    return @"

";

                #endregion

                #region ARC FLASH PROTECTION

                case "ARC FLASH PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8385, '8 CAL/CM2', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8386, '40 CAL/CM2', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8387, 'GLOVE 00 500V', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8388, 'GLOVE 0 1000V', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8389, 'GLOVE 1 7500V', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8390, 'GLOVE 2 17,000V', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8391, 'OTHER', 1562);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8413, 'ARC GLOVES', 1541);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8414, 'ARC KIT', 1541);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8415, 'ARC SUIT', 1541);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8416, 'PPE-ARC*', 1541);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8417, 'XX', 1541);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8549, 'Y', 1227);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8550, 'N', 1227);
";

                #endregion

                #region BATTERY

                case "BATTERY":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5784, 'WET', 1265);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5785, 'DRY', 1265);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5786, 'MAINTAINED BATT', 1284);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5787, 'MAINTENANCE FREE BATT', 1284);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5788, 'XX', 1284);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5789, '6V', 1105);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5790, '12V', 1105);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5791, '24V', 1105);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5792, '48V', 1105);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5793, 'OTHER', 1105);
";

                #endregion

                #region BATTERY CHARGER

                case "BATTERY CHARGER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5794, 'FIXED CURRENT CHARGER', 1486);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5795, 'FIXED VOLTAGE CHARGER', 1486);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5796, 'XX', 1486);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10014, '6V', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10015, '12V', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10016, '24V', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10017, '120', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10018, '120/208', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10019, '120/240', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10020, '208Y/120', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10021, '208', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10022, '230', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10023, '230/460', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10024, '240', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10025, '277', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10026, '277/480', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10027, '460', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10028, '480', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10029, '600', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10030, '2300', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10031, '2300/4160', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10032, '2400', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10033, '4160', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10034, '13KV', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10035, '25KV', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10036, '33KV', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10037, 'OTHER', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10038, 'LESS THEN 100', 1579);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10039, '90VDC OR LESS', 1579);
";

                #endregion

                #region BLOW OFF VALVE

                case "BLOW OFF VALVE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5109, 'VALVE BOX', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5110, 'MANHOLE', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5111, 'VAULT', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5112, 'STOP BOX', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5113, 'PIT BOX', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5114, 'BUILDING', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5115, 'OTHER', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5116, 'XX', 1490);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5165, 'MANUAL', 1283);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5166, 'ELECTRIC', 1283);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5167, 'HYDRAULIC', 1283);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5168, 'PNEUMATIC', 1283);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5856, 'Y', 1231);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5857, 'N', 1231);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5858, 'XX', 1231);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6334, '2', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6335, '4', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6336, '6', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6337, '8', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6338, '10', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6339, '12', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6340, '14', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6341, '16', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6342, '18', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6343, '20', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6344, '24', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6345, '30', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6346, '36', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6347, '42', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6348, '48', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6349, '60', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6350, '72', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6351, 'OTHER', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6352, '54', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6353, '2.25', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6354, '2.5', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6355, '3', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6356, '5', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6357, '15', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6358, '21', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6359, '27', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6360, '0.625', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6361, '0.75', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6362, '1', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6363, '1.25', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6364, '1.5', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6365, '66', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6366, '0.25', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6367, '0.5', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6368, 'XX', 1565);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6701, 'WORM', 1344);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6702, 'SPUR', 1344);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6703, 'BEVEL', 1344);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6704, 'NON RISING STEM', 1344);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6705, 'RISING STEM', 1344);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7006, 'MJ', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7007, 'RESTRAINED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7008, 'LEAD', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7009, 'FLANGED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7010, 'BALL & SOCKET', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7011, 'OTHER', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7012, 'THREADED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7013, 'SOLDERED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7014, 'FUSED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7015, 'LEADITE', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7016, 'WELDED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7017, 'GLUED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7018, 'PUSH ON', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7019, 'GLUED/CEMENTED', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7020, 'COMPRESSION', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7021, 'FLARE', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7022, 'XX', 1036);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7803, 'OPEN', 1069);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7804, 'CLOSED', 1069);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7805, 'THROTTLING', 1069);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7806, 'XX', 1069);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7831, 'Y', 911);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7832, 'N', 911);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7835, 'LEFT', 923);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7836, 'RIGHT', 923);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7837, 'XX', 923);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7853, 'SQUARE', 1437);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7854, 'TEE', 1437);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7855, 'WHEEL', 1437);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7856, 'OTHER', 1437);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7857, 'XX', 1437);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7858, 'PENTAGON', 1437);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8091, 'PVC', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8092, 'HDPE', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8093, 'RCP', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8094, 'PCCP', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8095, 'AC', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8096, 'GALV', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8097, 'CU', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8098, 'ST', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8099, 'DI', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8100, 'OTH', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8101, 'BR', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8102, 'CIU', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8103, 'CIL', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8104, 'PE', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8105, 'LD', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8106, 'WI', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8107, 'UNK', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8108, 'CI', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8109, 'ABS', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8110, 'VCP', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8111, 'BFP', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8112, 'XX', 1464);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8436, '150', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8437, '200', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8438, '250', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8439, '300', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8440, '350', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8441, '175', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8442, 'XX', 913);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8704, 'ALERT CUSTOMERS BEFORE', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8705, 'SPECIAL FLUSHING CONCERN', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8706, 'VALVE CLOSED/PLUGGED', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8707, 'OPEN SLOWLY', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8708, 'CLOSE SLOWLY', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8709, 'NO VEHICLE ACCESS', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8710, 'BACK-SIDE YARD', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8711, 'PUMPOUT REQUIRED', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8712, 'OTHER', 1268);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8846, 'CONCRETE', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8847, 'ASPHALT', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8848, 'GRASS', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8849, 'LANDSCAPING', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8850, 'GRAVEL', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8851, 'SOIL', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8852, 'BRICK', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8853, 'OTHER', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8854, 'UNIMPROVED', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8855, 'XX', 1586);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8873, 'STREET', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8874, 'DRIVEWAY', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8875, 'PARKING', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8876, 'SIDEWALK', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8877, 'OTHER', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8878, 'UNPAVED', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8879, 'XX', 1496);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8909, 'BLOWOFF(A)', 1029);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8910, 'BLOWOFF(M)', 1029);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8911, 'XX', 1029);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9202, '2', 1179);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9203, '1.5', 1179);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9204, 'OTHER', 1179);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9205, 'XX', 1179);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9211, 'RESILIENT', 1257);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9212, 'STAINLESS STEEL', 1257);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9213, 'BRONZE', 1257);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9214, 'CAST IRON', 1257);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9215, 'OTHER', 1257);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9218, 'Y', 887);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9219, 'N', 887);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9264, 'LESS THAN 1', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9265, '0.25', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9266, '0.5', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9267, '66', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9268, '5', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9269, 'XX', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9290, '2', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9291, '4', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9292, '6', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9293, '8', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9294, '10', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9295, '12', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9296, '14', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9297, '16', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9298, '18', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9299, '20', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9300, '24', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9301, '30', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9302, '36', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9303, '42', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9304, '48', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9305, '60', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9306, '72', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9307, 'OTHER', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9308, '40', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9309, '54', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9310, '0.625', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9311, '0.75', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9312, '1', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9313, '1.25', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9314, '1.5', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9315, '2.25', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9316, '2.5', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9317, '3', 1150);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9324, 'AIR RELIEF', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9325, 'AIR/VAC RELIEF', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9326, 'ANGLE', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9327, 'BALL', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9328, 'BUTTERFLY', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9329, 'CHECK', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9330, 'CHECK/FOOT', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9331, 'CURB STOP', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9332, 'DOUBLE CHECK', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9355, 'GATE', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9356, 'GLOBE', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9357, 'OTHER', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9358, 'PLUG', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9359, 'PRES REGULATOR', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9360, 'PRES RELIEF', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9361, 'RPZ', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9362, 'SOLENOID', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9363, 'TAPPING', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9364, 'TELESCOPIC', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9365, 'VAC REGULATOR', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9366, 'VAC RELIEF', 1431);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9367, 'XX', 1431);
";

                #endregion

                #region BLOWER

                case "BLOWER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5190, 'WATER PROCESSING', 1407);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5191, 'WASTE PROCESSING', 1407);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5192, 'PLANT UTILITY', 1407);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5193, 'ENVIRONMENTAL COMPLIANCE', 1407);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5194, 'OTHER', 1407);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5827, 'CENTRIFUGAL', 1004);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5828, 'POSITIVE DISPLACEMENT', 1004);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5829, 'TURBINE BLWR', 1004);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5830, 'XX', 1004);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6278, 'COUPLING', 1165);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6279, 'BELT', 1165);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6280, 'GEAR', 1165);
";

                #endregion

                #region BOILER

                case "BOILER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5195, 'FREEZE PROTECTION', 1505);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5196, 'COMFORT', 1505);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5197, 'PROCESSING', 1505);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5198, 'OTHER', 1505);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5831, 'BOILER*', 1191);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5832, 'XX', 1191);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6283, 'CONTINUOUS', 1288);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6284, 'INTERMITTENT', 1288);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6512, 'ELECTRIC', 1084);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6513, 'LIQ FUEL', 1084);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6514, 'NAT GAS', 1084);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6515, 'OTHER', 1084);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7954, 'BTU/HR', 1556);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7955, 'TONS', 1556);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7956, 'CFM', 1556);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7957, 'OTHER', 1556);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7958, 'WATTS', 1556);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7959, 'KW', 1556);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10040, '6V', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10041, '12V', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10042, '24V', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10043, '120', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10044, '120/208', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10045, '120/240', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10046, '208Y/120', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10047, '208', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10048, '230', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10049, '230/460', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10050, '240', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10051, '277', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10052, '277/480', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10053, '460', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10054, '480', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10055, '600', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10056, '2300', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10057, '2300/4160', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10058, '2400', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10059, '4160', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10060, '13KV', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10061, '25KV', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10062, '33KV', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10063, 'OTHER', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10064, 'LESS THEN 100', 1444);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10065, '90VDC OR LESS', 1444);
";

                #endregion

                #region BURNER

                case "BURNER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5199, 'FREEZE PROTECTION', 1467);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5200, 'COMFORT', 1467);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5201, 'PROCESSING', 1467);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5202, 'OTHER', 1467);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5842, 'BURNER*', 1139);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5843, 'XX', 1139);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6281, 'CONTINUOUS', 1237);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6282, 'INTERMITTENT', 1237);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6516, 'ELECTRIC', 1248);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6517, 'LIQ FUEL', 1248);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6518, 'NAT GAS', 1248);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6519, 'OTHER', 1248);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7960, 'BTU/HR', 987);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7961, 'TONS', 987);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7962, 'CFM', 987);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7963, 'OTHER', 987);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7964, 'WATTS', 987);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7965, 'KW', 987);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10066, '6V', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10067, '12V', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10068, '24V', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10069, '120', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10070, '120/208', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10071, '120/240', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10072, '208Y/120', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10073, '208', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10074, '230', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10075, '230/460', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10076, '240', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10077, '277', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10078, '277/480', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10079, '460', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10080, '480', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10081, '600', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10082, '2300', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10083, '2300/4160', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10084, '2400', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10085, '4160', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10086, '13KV', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10087, '25KV', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10088, '33KV', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10089, 'OTHER', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10090, 'LESS THEN 100', 1088);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10091, '90VDC OR LESS', 1088);
";

                #endregion

                #region CALIBRATION DEVICE

                case "CALIBRATION DEVICE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5859, 'CALIB*', 1439);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5860, 'XX', 1439);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8565, 'Y', 880);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8566, 'N', 880);
";

                #endregion

                #region CATHODIC PROTECTION

                case "CATHODIC PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5203, 'TANK', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5204, 'CLARIFIER', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5205, 'FILTER', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5206, 'PIPING', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5207, '120', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5208, '208', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5209, '240', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5210, '480', 1537);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5903, 'GALVANIC (SACRIFICIAL)', 962);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5904, 'IMPRESSED CURRENT', 962);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5905, 'XX', 962);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10092, '120', 934);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10093, '208', 934);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10094, '240', 934);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10095, '480', 934);
";

                #endregion

                #region CHEMICAL DRY FEEDER

                case "CHEMICAL DRY FEEDER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5299, 'LIME (SOLID)', 899);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5300, 'POWERED ACT CARBON', 899);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5301, 'POTASIUM PERMANGANATE', 899);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5302, 'FERRIC SULPHATE', 899);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5921, 'FEEDBACK', 1552);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5922, 'FLOW PACING', 1552);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5923, 'OTHER', 1552);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5942, 'LB/HR', 1093);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5943, 'LPM', 1093);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5944, 'GPM', 1093);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5945, 'GPD', 1093);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5946, 'CUFT/HR', 1093);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5947, 'LB/DAY', 1093);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5970, 'STEEL', 873);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5971, 'STAINLESS STEEL', 873);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5972, 'PLASTIC', 873);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5973, 'FIBERGLASS', 873);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5974, 'OTHER', 873);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5985, 'LOSS OF WEIGHT', 1232);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5986, 'VOLUMETRIC DRY FEEDER', 1232);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5987, 'XX', 1232);
";

                #endregion

                #region CHEMICAL GAS FEEDER

                case "CHEMICAL GAS FEEDER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5303, 'CHLORINE DIOXIDE', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5304, 'CHLORINE (GAS)', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5305, 'OZONE', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5306, 'OXYGEN', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5307, 'WATER', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5308, 'WASH WATER', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5309, 'WASTE WATER', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5310, 'SLUDGE', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5311, 'RESIN', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5312, 'DIESEL', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5313, 'GASOLINE', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5314, 'LPG', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5315, 'CARBON DIOXIDE', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5316, 'SULFUR DIOXIDE', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5317, 'AMOMONIA (ANHYD)', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5318, 'AMMONIA (ANHYD)', 931);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5924, 'FEEDBACK', 907);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5925, 'FLOW PACING', 907);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5926, 'OTHER', 907);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5930, 'LB/HR', 1144);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5931, 'LPM', 1144);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5932, 'GPM', 1144);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5933, 'GPD', 1144);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5934, 'CUFT/HR', 1144);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5935, 'LB/DAY', 1144);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5975, 'STEEL', 1065);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5976, 'STAINLESS STEEL', 1065);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5977, 'PLASTIC', 1065);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5978, 'FIBERGLASS', 1065);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5979, 'OTHER', 1065);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5988, 'AMMONIATOR', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5989, 'CHLORINATOR', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5990, 'CHMF-GAS*', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5991, 'DIFFUSER', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5992, 'EDUCTOR', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5993, 'EVAPORATOR', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5994, 'RUPTURE DISC', 950);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5995, 'XX', 950);
";

                #endregion

                #region CHEMICAL GENERATORS

                case "CHEMICAL GENERATORS":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5211, 'ALUM(ACIDIC)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5212, 'ALUMINUM CHLORHYDRATE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5213, 'ALUMINUM SULPHATE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5214, 'AMMONIA (ANHYD)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5215, 'CHLORINE DIOXIDE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5216, 'CHLORINE(GAS)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5217, 'FERRIC CHLORIDE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5218, 'FERRIC SULPHATE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5219, 'HYDROFLOUROSILICIC ACID', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5220, 'HYDROGEN PEROXIDE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5221, 'LIME HYDRATED', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5222, 'LIME SOLID', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5223, 'MISC CORROSION INHIB', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5224, 'MISC SEQUESTERANT', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5225, 'OZONE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5226, 'OXYGEN', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5227, 'PHOSPHORIC ACID', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5228, 'POLYALUMINUM CHLORIDE (LOW)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5229, 'POLYALUMINUM CHLORIDE (HIGH)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5230, 'POLYALUMINUM CHLORIDE (MID)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5231, 'POLYMER ANIONIC', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5232, 'POLYMER CATIONIC', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5233, 'POLYMER NON-IONIC', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5234, 'POLYMER (FILTER AID)', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5235, 'POTASIUM PERMANGANATE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5236, 'POWERED ACT. CARBON', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5237, 'SODIUM FLOURIDE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5238, 'SODIUM HYDROXIDE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5239, 'SODIUM HYPOCHLORITE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5240, 'SULFUR DIOXIDE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5241, 'SULFURIC ACID', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5242, 'ZINC ORTHOPHOSPHATE', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5243, 'OTHER', 1034);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5906, 'CHLORINE DIOXIDE', 1366);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5907, 'OZONE', 1366);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5908, 'SODIUM HYPO', 1366);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5909, 'XX', 1366);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5918, 'FEEDBACK', 973);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5919, 'FLOW PACING', 973);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5920, 'OTHER', 973);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5948, 'LB/HR', 1195);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5949, 'LPM', 1195);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5950, 'GPM', 1195);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5951, 'GPD', 1195);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5952, 'CUFT/HR', 1195);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5953, 'LB/DAY', 1195);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5960, 'STEEL', 1370);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5961, 'STAINLESS STEEL', 1370);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5962, 'PLASTIC', 1370);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5963, 'FIBERGLASS', 1370);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5964, 'OTHER', 1370);
";

                #endregion

                #region CHEMICAL LIQUID FEEDER

                case "CHEMICAL LIQUID FEEDER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5319, 'ALUM (ACIDIC)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5320, 'ALUMINUM CHLORHYDRATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5321, 'ALUMINUM SULPHATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5322, 'AMMONIA (ANHYD)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5323, 'AMMONIUM HYDROXIDE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5324, 'FERRIC CHLORIDE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5325, 'FERRIC SULPHATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5326, 'HYDROFLOUROSILICIC ACID', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5327, 'HYDROGEN PEROXIDE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5328, 'LIME (HYDRATED)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5329, 'MISC CORROSION INHIB', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5330, 'MISC SEQUESTERANT', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5331, 'OTHER', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5332, 'PHOSPHORIC ACID', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5333, 'POLYALUMINUM CHLORIDE (HIGH)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5334, 'POLYALUMINUM CHLORIDE (LOW)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5335, 'POLYALUMINUM CHLORIDE (MID)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5336, 'POLYMER (FILTER AID)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5337, 'POLYMER ANIONIC', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5338, 'POLYMER CATIONIC', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5339, 'POLYMER NON-IONIC', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5340, 'POTASIUM PERMANGANATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5341, 'SODIUM FLOURIDE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5342, 'SODIUM HYDROXIDE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5343, 'SODIUM HYPOCHLORITE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5344, 'SULFUR DIOXIDE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5345, 'SULFURIC ACID', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5346, 'ZINC ORTHOPHOSPHATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5347, 'WATER', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5348, 'WASH WATER', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5349, 'WASTE WATER', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5350, 'SLUDGE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5351, 'RESIN', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5352, 'DIESEL', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5353, 'GASOLINE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5354, 'LPG', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5355, 'SODIUM ALUMINATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5356, 'CHLORINE (GAS)', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5357, 'POWERED ACT. CARBON', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5358, 'POWERED ACT CARBON', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5359, 'SODIUM BISULPHATE', 1319);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5927, 'FEEDBACK', 1253);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5928, 'FLOW PACING', 1253);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5929, 'OTHER', 1253);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5936, 'LB/HR', 1132);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5937, 'LPM', 1132);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5938, 'GPM', 1132);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5939, 'GPD', 1132);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5940, 'CUFT/HR', 1132);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5941, 'LB/DAY', 1132);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5980, 'STEEL', 1213);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5981, 'STAINLESS STEEL', 1213);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5982, 'PLASTIC', 1213);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5983, 'FIBERGLASS', 1213);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5984, 'OTHER', 1213);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5996, 'VOLUMETRIC LIQ FEEDER', 1094);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5997, 'XX', 1094);
";

                #endregion

                #region CHEMICAL PIPING

                case "CHEMICAL PIPING":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5244, 'ALUM (ACIDIC)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5245, 'ALUMINUM CHLORHYDRATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5246, 'ALUMINUM SULPHATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5247, 'AMMONIA (ANHYD)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5248, 'AMMONIUM HYDROXIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5249, 'FERRIC CHLORIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5250, 'FERRIC SULPHATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5251, 'HYDROFLOUROSILICIC ACID', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5252, 'HYDROGEN PEROXIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5253, 'LIME (HYDRATED)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5254, 'LIME (SOLID)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5255, 'MISC CORROSION INHIB', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5256, 'MISC SEQUESTERANT', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5257, 'OTHER', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5258, 'PHOSPHORIC ACID', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5259, 'POLYALUMINUM CHLORIDE (HIGH)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5260, 'POLYALUMINUM CHLORIDE (LOW)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5261, 'POLYALUMINUM CHLORIDE (MID)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5262, 'POLYMER (FILTER AID)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5263, 'POLYMER ANIONIC', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5264, 'POLYMER CATIONIC', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5265, 'POLYMER NON-IONIC', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5266, 'POTASIUM PERMANGANATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5267, 'POWERED ACT CARBON', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5268, 'SODIUM FLOURIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5269, 'SODIUM HYDROXIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5270, 'SODIUM HYPOCHLORITE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5271, 'SULFUR DIOXIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5272, 'SULFURIC ACID', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5273, 'ZINC ORTHOPHOSPHATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5274, 'CHLORINE (GAS)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5275, 'WATER', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5276, 'AMMONIA (AQUEOUS)', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5277, 'BRINE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5278, 'SLUDGE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5279, 'SODIUM BISULPHATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5280, 'BENTONITE CLAY', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5281, 'CARBON DIOXIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5282, 'CHLORAMINE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5283, 'CHLORINE DIOXIDE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5284, 'CITRIC ACID', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5285, 'DIESEL', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5286, 'GASOLINE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5287, 'HYDROCHLORIC ACID', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5288, 'LPG', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5289, 'MICROSAND', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5290, 'OXYGEN', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5291, 'OZONE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5292, 'POLYALUMINUM SULFATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5293, 'RESIN', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5294, 'SODA ASH', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5295, 'SODIUM ALUMINATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5296, 'SODIUM BICARBONATE', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5297, 'WASH WATER', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5298, 'WASTE WATER', 1465);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5910, 'FLEX WHIP', 1197);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5911, 'GASEOUS', 1197);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5912, 'LIQUID', 1197);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5913, 'SOLID', 1197);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5914, 'XX', 1197);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5915, 'FEEDBACK', 915);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5916, 'FLOW PACING', 915);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5917, 'OTHER', 915);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5954, 'LB/HR', 974);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5955, 'LPM', 974);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5956, 'GPM', 974);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5957, 'GPD', 974);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5958, 'CUFT/HR', 974);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5959, 'LB/DAY', 974);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5965, 'STEEL', 1473);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5966, 'STAINLESS STEEL', 1473);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5967, 'PLASTIC', 1473);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5968, 'FIBERGLASS', 1473);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5969, 'OTHER', 1473);
";

                #endregion

                #region CHEMICAL TANK

                case "CHEMICAL TANK":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5675, 'ALUM (ACIDIC)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5676, 'ALUMINUM CHLORHYDRATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5677, 'ALUMINUM SULPHATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5678, 'AMMONIA (ANHYD)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5679, 'AMMONIUM HYDROXIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5680, 'CHLORINE (GAS)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5681, 'CHLORINE DIOXIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5682, 'FERRIC CHLORIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5683, 'FERRIC SULPHATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5684, 'HYDROFLOUROSILICIC ACID', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5685, 'HYDROGEN PEROXIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5686, 'LIME (HYDRATED)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5687, 'LIME (SOLID)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5688, 'MISC CORROSION INHIB', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5689, 'MISC SEQUESTERANT', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5690, 'OZONE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5691, 'OXYGEN', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5692, 'PHOSPHORIC ACID', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5693, 'POLYALUMINUM CHLORIDE (LOW)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5694, 'POLYALUMINUM CHLORIDE (HIGH)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5695, 'POLYALUMINUM CHLORIDE (MID)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5696, 'POLYMER ANIONIC', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5697, 'POLYMER CATIONIC', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5698, 'POLYMER NON-IONIC', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5699, 'POLYMER (FILTER AID)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5700, 'POTASIUM PERMANGANATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5701, 'POWERED ACT CARBON', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5702, 'SODIUM FLOURIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5703, 'SODIUM HYDROXIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5704, 'SODIUM HYPOCHLORITE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5705, 'SULFUR DIOXIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5706, 'SULFURIC ACID', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5707, 'ZINC ORTHOPHOSPHATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5708, 'OTHER', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5709, 'CITRIC ACID', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5710, 'HYDROCHLORIC ACID', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5711, 'SODIUM ALUMINATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5712, 'AMMONIA (AQUEOUS)', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5713, 'BRINE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5714, 'BENTONITE CLAY', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5715, 'CARBON DIOXIDE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5716, 'CHLORAMINE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5717, 'MICROSAND', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5718, 'POLYALUMINUM SULFATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5719, 'RESIN', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5720, 'SODA ASH', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5721, 'SODIUM BICARBONATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5722, 'SODIUM BISULPHATE', 1245);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7129, 'INDOORS', 1185);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7130, 'OUTDOORS', 1185);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8923, 'Y', 1451);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8924, 'N', 1451);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8925, 'XX', 1451);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8946, 'STEEL', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8947, 'STAINLESS STEEL', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8948, 'PLASTIC', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8949, 'FIBERGLASS', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8950, 'OTHER', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8951, 'CONCRETE', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8952, 'XX', 1143);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8992, 'VACUUM', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8993, 'ATMOSPHERIC', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8994, 'LESS THAN 100PSIG', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8995, 'LESS THAN 2PSIG', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8996, 'GREATER THAN 100PSIG', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8997, 'LESS THAN 15PSIG', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8998, '15-100PSIG', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8999, 'XX', 1516);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9031, 'Y', 1056);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9032, 'N', 1056);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9033, 'XX', 1056);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9043, 'NON-PRESSURIZED CHEM', 1536);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9044, 'PRESSURIZED/BLADDER CHEM', 1536);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9045, 'XX', 1536);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9163, 'Y', 1091);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9164, 'N', 1091);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9165, 'XX', 1091);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10291, 'ALERT CUSTOMERS BEFORE', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10292, 'SPECIAL FLUSHING CONCERN', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10293, 'VALVE CLOSED/PLUGGED', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10294, 'OPEN SLOWLY', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10295, 'CLOSE SLOWLY', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10296, 'NO VEHICLE ACCESS', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10297, 'BACK-SIDE YARD', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10298, 'PUMPOUT REQUIRED', 1653);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10299, 'OTHER', 1653);
";

                #endregion

                #region CLARIFIER

                case "CLARIFIER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5766, 'Y', 1528);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5767, 'N', 1528);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7105, 'INDOORS', 1485);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7106, 'OUTDOORS', 1485);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7183, 'STEEL', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7184, 'STAINLESS STEEL', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7185, 'PLASTIC', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7186, 'FIBERGLASS', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7187, 'WOOD', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7188, 'CONCRETE', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7189, 'OTHER', 1408);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9116, 'ACTIFLOW', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9117, 'DAF', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9118, 'FLOCCULATOR', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9119, 'GRAVITY CLAR', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9120, 'PLATE', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9121, 'RAKE', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9122, 'SUPERPULSATOR', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9123, 'TUB', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9124, 'UPFLOW', 1317);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9125, 'XX', 1317);
";

                #endregion

                #region CLEAN OUT

                case "CLEAN OUT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5758, 'EASEMENT', 1345);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5759, 'COMPANY', 1345);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5760, 'PRIVATE', 1345);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5761, 'PUBLIC ROW', 1345);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6002, 'WYE', 1279);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6003, 'TEE', 1279);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6004, '4', 975);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6005, '6', 975);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6006, 'SINGLE', 1509);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6007, 'DOUBLE', 1509);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6008, 'NONE', 1509);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6009, 'LATSERV', 1172);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6010, 'MAINLINE', 1172);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6011, 'XX', 1172);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7218, 'ABS', 1540);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7219, 'BFP', 1540);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7220, 'OTH', 1540);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7221, 'PVC', 1540);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7222, 'VCP', 1540);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8687, 'ALERT CUSTOMER BEFORE', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8688, 'BACK-SIDE YARD', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8689, 'SPECIAL TRAFFIC CONCERN', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8690, 'RECURRING PESTS', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8691, 'USE LOW PRESSURE', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8692, 'OTHER', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8693, 'NO VEHICLE ACCESS', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8694, 'PUMPOUT REQUIRED', 1218);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8836, 'CONCRETE', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8837, 'ASPHALT', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8838, 'GRASS', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8839, 'LANDSCAPING', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8840, 'GRAVEL', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8841, 'SOIL', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8842, 'BRICK', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8843, 'OTHER', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8844, 'UNIMPROVED', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8845, 'XX', 1256);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8887, 'STREET', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8888, 'DRIVEWAY', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8889, 'PARKING', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8890, 'SIDEWALK', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8891, 'OTHER', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8892, 'UNPAVED', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8893, 'XX', 1261);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10136, 'COMBINED', 1575);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10137, 'IO EFFLUENT', 1575);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10138, 'RAW', 1575);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10139, 'RECLAIMED', 1575);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10140, 'RO EFFLUENT', 1575);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10141, 'STORM', 1575);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10142, 'TREATED', 1575);
";

                #endregion

                #region COLLECTION SYSTEM GENERAL

                case "COLLECTION SYSTEM GENERAL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6012, 'COLLSYS*', 935);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6013, 'XX', 935);
";

                #endregion

                #region CONTROL PANEL

                case "CONTROL PANEL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5998, 'CNTRLPNL*', 1569);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5999, 'XX', 1569);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9936, '6V', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9937, '12V', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9938, '24V', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9939, '120', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9940, '120/208', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9941, '120/240', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9942, '208Y/120', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9943, '208', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9944, '230', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9945, '230/460', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9946, '240', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9947, '277', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9948, '277/480', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9949, '460', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9950, '480', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9951, '600', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9952, '2300', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9953, '2300/4160', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9954, '2400', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9955, '4160', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9956, '13KV', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9957, '25KV', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9958, '33KV', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9959, 'OTHER', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9960, 'LESS THEN 100', 1103);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9961, '90VDC OR LESS', 1103);
";

                #endregion

                #region CONTROLLER

                case "CONTROLLER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5360, 'SINGLE VARIABLE CONTROLLER', 943);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5361, 'FULL PROCESS CONTROL', 943);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5362, 'DATA COLLECTOR', 943);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5363, 'DATA TRANSMITTER ONLY', 943);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5364, 'OTHER', 943);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5365, 'FULL PROCESS CONTROLLER', 943);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6000, 'CNTRLR*', 1066);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6001, 'XX', 1066);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6036, 'NONLICENSE RADIO', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6037, 'LICENSE RADIO', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6038, 'NONE', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6039, 'FIBER OPTIC', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6040, 'LEASE LINE MODEM', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6041, 'CELLULAR', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6042, 'OTHER', 1573);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6043, 'OTHER', 941);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6044, 'RS485', 941);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6045, 'RS232', 941);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6046, 'ETHERNET', 941);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6058, 'NONE', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6059, 'NONLICENSE RADIO', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6060, 'LICENSE RADIO', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6061, 'FIBER OPTIC', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6062, 'OTHER', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6063, 'LEASE LINE MODEM', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6064, 'CELLULAR', 1147);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6065, 'OTHER', 1570);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6066, 'RS485', 1570);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6067, 'RS232', 1570);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6068, 'ETHERNET', 1570);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6080, 'NONLICENSE RADIO', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6081, 'LICENSE RADIO', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6082, 'OTHER', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6083, 'NONE', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6084, 'FIBER OPTIC', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6085, 'LEASE LINE MODEM', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6086, 'CELLULAR', 1424);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6087, 'OTHER', 1440);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6088, 'RS485', 1440);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6089, 'RS232', 1440);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6090, 'ETHERNET', 1440);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6102, 'NONLICENSE RADIO', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6103, 'LICENSE RADIO', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6104, 'NONE', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6105, 'OTHER', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6106, 'CELLULAR', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6107, 'FIBER OPTIC', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6108, 'LEASE LINE MODEM', 1463);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6109, 'ETHERNET', 866);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6110, 'OTHER', 866);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6111, 'RS485', 866);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6112, 'RS232', 866);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6124, 'FIBER OPTIC', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6125, 'NONE', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6126, 'LEASE LINE MODEM', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6127, 'CELLULAR', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6128, 'LICENSE RADIO', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6129, 'NONLICENSE RADIO', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6130, 'OTHER', 1453);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6131, 'ETHERNET', 1309);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6132, 'OTHER', 1309);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6133, 'RS485', 1309);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6134, 'RS232', 1309);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6146, 'NONE', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6147, 'LEASE LINE MODEM', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6148, 'CELLULAR', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6149, 'LICENSE RADIO', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6150, 'NONLICENSE RADIO', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6151, 'FIBER OPTIC', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6152, 'OTHER', 1096);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6153, 'ETHERNET', 1206);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6154, 'OTHER', 1206);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6155, 'RS485', 1206);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6156, 'RS232', 1206);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6480, 'Y', 1553);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6481, 'N', 1553);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8531, 'Y', 1282);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8532, 'N', 1282);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8767, 'NONE', 981);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8768, 'GENERATOR', 981);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8769, 'BATTERY', 981);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8770, 'UPS', 981);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8771, 'OTHER', 981);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10096, '120VAC', 895);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10097, '24VDC', 895);
";

                #endregion

                #region CONVEYOR

                case "CONVEYOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6231, 'BELT CONVEYOR', 1077);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6232, 'SCREW CONVEYOR', 1077);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6233, 'XX', 1077);
";

                #endregion

                #region COOLING TOWER

                case "COOLING TOWER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5519, 'PROCESSING', 1001);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5520, 'COMFORT', 1001);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5521, 'FREEZE PROTECTION', 1001);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5522, 'VENTILATION', 1001);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5523, 'OTHER', 1001);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6492, 'ELECTRIC', 1130);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6493, 'LIQ FUEL', 1130);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6494, 'NAT GAS', 1130);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6495, 'OTHER', 1130);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6770, 'HVAC-TWR*', 1167);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6771, 'XX', 1167);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7990, 'BTU/HR', 1149);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7991, 'TONS', 1149);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7992, 'CFM', 1149);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7993, 'OTHER', 1149);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7994, 'WATTS', 1149);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7995, 'KW', 1149);
";

                #endregion

                #region DAM

                case "DAM":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6234, 'CONCRETE ARCH', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6235, 'CONCRETE GRAVITY', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6236, 'CONCRETE SLAB AND BUTTRESS', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6237, 'EARTHEN EMBANKMENT', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6238, 'ROCK EMBANKMENT', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6239, 'STEEL SHEET PILING', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6240, 'STONE MASONRY GRAVITY', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6241, 'TIMBER', 1380);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6242, 'XX', 1380);
";

                #endregion

                #region DEFIBRILLATOR

                case "DEFIBRILLATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5182, 'AED*', 1000);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5183, 'XX', 1000);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5782, 'Y', 1159);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5783, 'N', 1159);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8406, '8 CAL/CM2', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8407, '40 CAL/CM2', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8408, 'GLOVE 00 500V', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8409, 'GLOVE 0 1000V', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8410, 'GLOVE 1 7500V', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8411, 'GLOVE 2 17,000V', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8412, 'OTHER', 1551);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8563, 'Y', 1568);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8564, 'N', 1568);
";

                #endregion

                #region DISTRIBUTION SYSTEM

                case "DISTRIBUTION SYSTEM":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6245, 'DISTSYS*', 948);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6246, 'XX', 948);
";

                #endregion

                #region DISTRIBUTION TOOL

                case "DISTRIBUTION TOOL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6247, 'PLASTIC LOCATOR', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6248, 'MARKER BALL LOCATOR', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6249, 'STANDARD CORRELATOR', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6250, 'MULTI POINT CORRELATOR', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6251, 'LEAK AMPLIFIER', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6252, 'METAL DETECTOR', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6253, 'INDUCTIVE-CONDUCTIVE LOCATOR', 1177);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6254, 'ACCELEROMETER', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6255, 'HYDROPHONES', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6256, 'ACCELEROMETER & HYDROPHONES', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6257, 'PROBE', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6258, 'GROUND MIC', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6259, 'MAGNETIC CONTACT', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6260, 'PROBE & GROUND MIC', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6261, 'PROBE & MAGNETIC CONTACT', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6262, 'GROUND MIC & MAGNETIC CONTACT', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6263, 'PROBE, GROUND MIC & MAGNETIC', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6264, 'FERROUS METAL', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6265, 'GENERAL METAL', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6266, 'SPLIT BOX', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6267, 'TWO COMPONENT', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6268, 'VIBRATION', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6269, 'MARKER', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6270, 'LOCATION ONLY', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6271, 'LOCATION – MATERIAL', 1545);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6272, 'LEAK DETECTOR', 1184);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6273, 'LOCATOR', 1184);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6274, 'XX', 1184);
";

                #endregion

                #region ELEVATOR

                case "ELEVATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5871, 'CFM', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5872, 'GPM', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5873, 'LB H', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5874, 'OTHER', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5875, 'LB', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5876, 'KG', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5877, 'PSI', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5878, 'IN H2O', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5879, 'FT H2O', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5880, 'PPM', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5881, 'MOHMS', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5882, 'AMPS', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5883, 'VOLTS', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5884, 'KWH', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5885, 'RPM', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5886, 'HZ', 1402);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6474, 'ELEV ELEC', 1559);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6475, 'ELEV HYDR', 1559);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6476, 'XX', 1559);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8561, 'Y', 1483);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8562, 'N', 1483);
";

                #endregion

                #region EMERGENCY GENERATOR

                case "EMERGENCY GENERATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5499, 'EMERGENCY', 1110);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5500, 'BASE LOAD', 1110);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6714, 'Y', 1293);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6715, 'N', 1293);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6716, 'XX', 1293);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6717, 'Y', 1584);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6718, 'N', 1584);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6719, 'XX', 1584);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6720, 'ENGINE DRIVEN', 1215);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6721, 'HYDRO DRIVEN', 1215);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6722, 'SOLAR', 1215);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6723, 'WIND', 1215);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6724, 'XX', 1215);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6725, 'AC', 1469);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6726, 'DC', 1469);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8023, '3', 1339);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8024, '1', 1339);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8637, '1200', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8638, '1800', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8639, '3600', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8640, 'VARIABLE', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8641, 'OTHER', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8642, '600', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8643, '600/1200', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8644, '900', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8645, '900/1800', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8646, '3500', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8647, '1725', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8648, '1550', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8649, 'XX', 971);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8679, 'Y', 1420);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8680, 'N', 1420);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8681, 'XX', 1420);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9543, '6V', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9544, '12V', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9545, '24V', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9546, '120', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9547, '120/208', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9548, '120/240', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9549, '208Y/120', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9550, '208', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9551, '230', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9552, '230/460', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9553, '240', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9554, '277', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9555, '277/480', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9556, '460', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9557, '480', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9558, '600', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9559, '2300', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9560, '2300/4160', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9561, '2400', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9562, '4160', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9563, '13KV', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9564, '25KV', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9565, '33KV', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9566, 'OTHER', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9567, 'LESS THEN 100', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9568, '90VDC OR LESS', 905);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10226, 'TAXABLE', 1597);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10227, 'UNLEADED', 1597);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10228, 'GASOLINE', 1598);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10229, 'DIESEL', 1598);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10230, 'LPG', 1598);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10231, 'FUEL OIL', 1598);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10232, 'NG', 1598);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10233, 'OTHER', 1598);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10234, 'N/A', 1599);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10235, 'INTEGRAL', 1599);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10236, 'SEPARATE', 1599);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10237, 'ALERT CUSTOMERS BEFORE', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10238, 'SPECIAL FLUSHING CONCERN', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10239, 'VALVE CLOSED/PLUGGED', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10240, 'OPEN SLOWLY', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10241, 'CLOSE SLOWLY', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10242, 'NO VEHICLE ACCESS', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10243, 'BACK-SIDE YARD', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10244, 'PUMPOUT REQUIRED', 1603);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10245, 'OTHER', 1603);
";

                #endregion

                #region EMERGENCY LIGHT

                case "EMERGENCY LIGHT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5780, 'Y', 1341);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5781, 'N', 1341);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6477, 'STANDALONE ELIGHT', 1481);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6478, 'SYSTEMWIDE ELIGHT', 1481);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6479, 'XX', 1481);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8559, 'Y', 893);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8560, 'N', 893);
";

                #endregion

                #region ENGINE

                case "ENGINE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5461, 'PUMPING', 1075);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5462, 'EMERGENCY PUMPING', 1075);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5463, 'ELEC GENERATION', 1075);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5464, 'EMERGENCY ELEC GENERATION', 1075);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6520, 'GPH', 1513);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6521, 'CFH', 1513);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6522, 'DIESEL', 1457);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6523, 'GASOLINE', 1457);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6524, 'LPG', 1457);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6525, 'NATURAL GAS', 1457);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6526, 'XX', 1457);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8682, 'Y', 1272);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8683, 'N', 1272);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8684, 'XX', 1272);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10217, 'ALERT CUSTOMERS BEFORE', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10218, 'SPECIAL FLUSHING CONCERN', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10219, 'VALVE CLOSED/PLUGGED', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10220, 'OPEN SLOWLY', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10221, 'CLOSE SLOWLY', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10222, 'NO VEHICLE ACCESS', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10223, 'BACK-SIDE YARD', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10224, 'PUMPOUT REQUIRED', 1596);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10225, 'OTHER', 1596);
";

                #endregion

                #region EYEWASH

                case "EYEWASH":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6527, 'PERMANENT EYEWASH', 957);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6528, 'PORTABLE EYEWASH', 957);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6529, 'XX', 957);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8557, 'Y', 890);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8558, 'N', 890);
";

                #endregion

                #region FACILITY AND GROUNDS

                case "FACILITY AND GROUNDS":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5774, 'Y', 1395);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5775, 'N', 1395);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6530, 'Y', 1100);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6531, 'N', 1100);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6532, 'SCADA', 1181);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6533, 'DIAL OUT', 1181);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6534, 'NONE', 1181);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6535, 'OTHER', 1181);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6536, 'SCADA AND DIAL OUT', 1181);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6537, '8/7', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6538, 'WEEKDAY VISIT', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6539, '16/5', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6540, '16/7', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6541, 'AS NEEDED', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6542, '24/7', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6543, '24/5', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6544, '8/5', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6545, 'DAILY VISIT', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6546, 'WEEKLY VISIT', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6547, 'MONTHLY VISIT', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6548, 'OTHER', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6549, 'YEARLY VISIT', 1390);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6550, 'FACILITY*', 1399);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6551, 'XX', 1399);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6552, '4160', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6553, '13.2K', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6554, '>13.2K', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6555, 'OTHER', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6556, '<480', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6557, '480', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6558, '2300', 1201);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7825, 'Y', 914);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7826, 'N', 914);
";

                #endregion

                #region FALL PROTECTION

                case "FALL PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8392, '8 CAL/CM2', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8393, '40 CAL/CM2', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8394, 'GLOVE 00 500V', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8395, 'GLOVE 0 1000V', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8396, 'GLOVE 1 7500V', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8397, 'GLOVE 2 17,000V', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8398, 'OTHER', 1365);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8418, 'PPE-FALL*', 1333);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8419, 'XX', 1333);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8547, 'Y', 1401);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8548, 'N', 1401);
";

                #endregion

                #region FILTER

                case "FILTER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5737, 'WATER', 1194);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5738, 'WASTE WATER', 1194);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5739, 'SLUDGE', 1194);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5740, 'OTHER', 1194);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7113, 'INDOORS', 1053);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7114, 'OUTDOORS', 1053);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7197, 'STEEL', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7198, 'STAINLESS STEEL', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7199, 'PLASTIC', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7200, 'FIBERGLASS', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7201, 'WOOD', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7202, 'CONCRETE', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7203, 'OTHER', 1186);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7266, 'ANTHRACITE', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7267, 'CHOROSORB', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7268, 'CLOTH', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7269, 'DIATOMACEOUS EARTH', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7270, 'GARNITE', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7271, 'GRANULATED ACTIVATED CARBON', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7272, 'GRAVEL', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7273, 'GREENSAND', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7274, 'INTEGRAL MEDIA SUPPORT', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7275, 'MEMBRANE', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7276, 'MEMBRANE EMERSED', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7277, 'MEMBRANE PRESSURIZED', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7278, 'MEMBRANE TUBULAR', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7279, 'MEMBRANE PLATE', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7280, 'MEMBRANE HOLLOW', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7281, 'PLASTIC BLOCK SUPPORT', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7282, 'RAPID SAND', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7283, 'REVERSE OSMOSIS', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7284, 'SILICA', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7285, 'SLOW SAND', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7286, 'TILE BLOCK SUPPORT', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7287, 'WHEELER BOTTOMS SUPPORT', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7288, 'OTHER', 1203);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7325, 'ANTHRACITE', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7326, 'DIATOMACEOUS EARTH', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7327, 'GARNITE', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7328, 'GRANULATED ACTIVATED CARBON', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7329, 'GRAVEL', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7330, 'GREENSAND', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7331, 'INTEGRAL MEDIA SUPPORT', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7332, 'MEMBRANE', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7333, 'MEMBRANE EMERSED', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7334, 'MEMBRANE PRESSURIZED', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7335, 'MEMBRANE TUBULAR', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7336, 'MEMBRANE PLATE', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7337, 'MEMBRANE HOLLOW', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7338, 'PLASTIC BLOCK SUPPORT', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7339, 'RAPID SAND', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7340, 'REVERSE OSMOSIS', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7341, 'SILICA', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7342, 'SLOW SAND', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7343, 'TILE BLOCK SUPPORT', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7344, 'WHEELER BOTTOMS SUPPORT', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7345, 'OTHER', 903);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7360, 'ANTHRACITE', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7361, 'DIATOMACEOUS EARTH', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7362, 'GARNITE', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7363, 'GRANULATED ACTIVATED CARBON', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7364, 'GRAVEL', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7365, 'GREENSAND', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7366, 'INTEGRAL MEDIA SUPPORT', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7367, 'MEMBRANE', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7368, 'MEMBRANE EMERSED', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7369, 'MEMBRANE PRESSURIZED', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7370, 'MEMBRANE TUBULAR', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7371, 'MEMBRANE PLATE', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7372, 'MEMBRANE HOLLOW', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7373, 'PLASTIC BLOCK SUPPORT', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7374, 'RAPID SAND', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7375, 'REVERSE OSMOSIS', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7376, 'SILICA', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7377, 'SLOW SAND', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7378, 'TILE BLOCK SUPPORT', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7379, 'WHEELER BOTTOMS SUPPORT', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7380, 'OTHER', 1043);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7381, 'ANTHRACITE', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7382, 'DIATOMACEOUS EARTH', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7383, 'GARNITE', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7384, 'GRANULATED ACTIVATED CARBON', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7385, 'GRAVEL', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7386, 'GREENSAND', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7387, 'INTEGRAL MEDIA SUPPORT', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7388, 'MEMBRANE', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7389, 'MEMBRANE EMERSED', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7390, 'MEMBRANE PRESSURIZED', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7391, 'MEMBRANE TUBULAR', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7392, 'MEMBRANE PLATE', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7393, 'MEMBRANE HOLLOW', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7394, 'PLASTIC BLOCK SUPPORT', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7395, 'RAPID SAND', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7396, 'REVERSE OSMOSIS', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7397, 'SILICA', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7398, 'SLOW SAND', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7399, 'TILE BLOCK SUPPORT', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7400, 'WHEELER BOTTOMS SUPPORT', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7401, 'OTHER', 1233);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7402, 'ANTHRACITE', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7403, 'DIATOMACEOUS EARTH', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7404, 'GARNITE', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7405, 'GRANULATED ACTIVATED CARBON', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7406, 'GRAVEL', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7407, 'GREENSAND', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7408, 'INTEGRAL MEDIA SUPPORT', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7409, 'MEMBRANE', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7410, 'MEMBRANE EMERSED', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7411, 'MEMBRANE PRESSURIZED', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7412, 'MEMBRANE TUBULAR', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7413, 'MEMBRANE PLATE', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7414, 'MEMBRANE HOLLOW', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7415, 'PLASTIC BLOCK SUPPORT', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7416, 'RAPID SAND', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7417, 'REVERSE OSMOSIS', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7418, 'SILICA', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7419, 'SLOW SAND', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7420, 'TILE BLOCK SUPPORT', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7421, 'WHEELER BOTTOMS SUPPORT', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7422, 'OTHER', 1259);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7423, 'Y', 1588);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7424, 'N', 1588);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9128, 'CONVEYOR PRESS', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9129, 'GRAVITY', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9130, 'PLATE PRESS', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9131, 'PRESSURE', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9132, 'MEMBRANE-MF', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9133, 'MEMBRANE-NF', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9134, 'MEMBRANE-RO', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9135, 'MEMBRANE-UF', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9136, 'XX', 1434);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10114, 'SURFACE SCOUR', 1393);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10115, 'AIR SCOUR', 1393);
";

                #endregion

                #region FIRE ALARM

                case "FIRE ALARM":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5125, 'SIREN', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5126, 'STROBE', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5127, 'SIREN/STROBE', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5128, 'AUTO SHUTDOWN', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5129, 'AUTO SHUT/VENTILATION', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5130, 'OTHER', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5131, 'LIGHT BUILDING', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5132, 'SCADA ALARM', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5133, 'XX', 1024);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6569, 'FIRE CLASS A', 1009);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6570, 'FIRE CLASS B', 1009);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6571, 'FIRE CLASS C', 1009);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6572, 'FIRE CLASS ABC', 1009);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6573, 'OTHER', 1009);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6574, 'SINGLE DETECTOR', 985);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6575, 'SYSTEM', 985);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6576, 'XX', 985);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8545, 'Y', 967);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8546, 'N', 967);
";

                #endregion

                #region FIRE EXTINGUISHER

                case "FIRE EXTINGUISHER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6559, 'FIRE CLASS A', 1422);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6560, 'FIRE CLASS B', 1422);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6561, 'FIRE CLASS C', 1422);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6562, 'FIRE CLASS ABC', 1422);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6563, 'OTHER', 1422);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6577, 'GAS', 1085);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6578, 'POWDER', 1085);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6579, 'WATER', 1085);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6580, 'XX', 1085);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8543, 'Y', 954);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8544, 'N', 954);
";

                #endregion

                #region FIRE SUPPRESSION

                case "FIRE SUPPRESSION":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5134, 'SIREN', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5135, 'STROBE', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5136, 'SIREN/STROBE', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5137, 'AUTO SHUTDOWN', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5138, 'AUTO SHUT/VENTILATION', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5139, 'OTHER', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5140, 'LIGHT BUILDING', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5141, 'SCADA ALARM', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5142, 'XX', 1580);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6564, 'FIRE CLASS A', 910);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6565, 'FIRE CLASS B', 910);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6566, 'FIRE CLASS C', 910);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6567, 'FIRE CLASS ABC', 910);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6568, 'OTHER', 910);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6581, 'GAS SUP', 1448);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6582, 'WATER SUP', 1448);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6583, 'XX', 1448);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8541, 'Y', 1039);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8542, 'N', 1039);
";

                #endregion

                #region FIREWALL

                case "FIREWALL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5366, 'CONTROL AND DAQ', 1352);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5367, 'DAQ ONLY', 1352);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5368, 'CONTROL ONLY', 1352);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5369, 'SECURITY ONLY', 1352);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5370, 'NETWORK', 1352);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5371, 'ALARMING', 1352);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5807, '2400', 1246);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5808, '9600', 1246);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5809, '19200', 1246);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5810, '56K', 1246);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5811, 'OTHER', 1246);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6161, 'COMM-FWL*', 1384);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6162, 'XX', 1384);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6211, 'NONE', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6212, 'LEASE LINE', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6213, 'CELLULAR', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6214, 'LICENSE RADIO', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6215, 'NONLICENSE RADIO', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6216, 'NETWORK', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6217, 'WIRELESS', 888);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8772, 'NONE', 1455);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8773, 'GENERATOR', 1455);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8774, 'BATTERY', 1455);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8775, 'UPS', 1455);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8776, 'OTHER', 1455);
";

                #endregion

                #region FLOATATION DEVICE

                case "FLOATATION DEVICE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5772, 'Y', 1421);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5773, 'N', 1421);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8399, '8 CAL/CM2', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8400, '40 CAL/CM2', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8401, 'GLOVE 00 500V', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8402, 'GLOVE 0 1000V', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8403, 'GLOVE 1 7500V', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8404, 'GLOVE 2 17,000V', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8405, 'OTHER', 1488);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8420, 'PPE-FLOT*', 1334);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8421, 'XX', 1334);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8539, 'Y', 1243);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8540, 'N', 1243);
";

                #endregion

                #region FLOW METER (NON PREMISE)

                case "FLOW METER (NON PREMISE)":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5465, 'PLANT FINISHED WATER', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5466, 'PLANT BACKWASH', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5467, 'PLANT EFFLUENT', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5468, 'PLANT PROCESS', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5469, 'PLANT WASTE', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5470, 'PURCHASE FINISHED WATER', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5471, 'EMERG SALES', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5472, 'DISTRIBUTION ZONE', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5473, 'FUEL', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5474, 'BLOW-OFF', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5475, 'OTHER', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5476, 'SALES', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5477, 'PLANT INFLUENT', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5478, 'COLLECTION ZONE', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5479, 'RAW WATER (GROUND)', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5480, 'RAW WATER (SURFACE)', 1436);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5850, 'Y', 1342);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5851, 'N', 1342);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5852, 'XX', 1342);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5861, 'IN WATER', 1176);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5862, 'FT WATER', 1176);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5863, 'IN HG', 1176);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6019, 'PROFIBUS', 1320);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6020, 'HART', 1320);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6021, 'FOXCOM', 1320);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6022, 'MODBUS', 1320);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6023, 'RS232', 1320);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6584, 'COMPOUND', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6585, 'MAGNETIC', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6586, 'MASSFLOW', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6587, 'ORIFICE', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6588, 'OTHER MET', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6589, 'PARSHALL', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6590, 'PITOT TUBE', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6591, 'POSITIVE DISP', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6592, 'ROTOMETER', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6593, 'THERMAL DISPERSION', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6594, 'TURBINE/PROPELLER', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6595, 'ULTRASONIC DOPPLER', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6596, 'ULTRASONIC TIME OF TRANSIT', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6597, 'VCONE', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6598, 'VENTURI', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6599, 'VORTEX', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6600, 'XX', 1548);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6619, 'GPM', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6620, 'MGD', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6621, 'GPH', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6622, 'CFS', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6623, 'GPD', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6624, 'LB/HR', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6625, 'LB/DAY', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6626, 'OTHER', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6627, 'CFM', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6628, 'LPH', 1025);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6873, '2 WIRE', 1311);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6874, '4 WIRE', 1311);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7443, 'ALUMINUM', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7444, 'BRONZE', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7445, 'CAST IRON', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7446, 'DUCTILE IRON', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7447, 'PTFE', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7448, 'PVC', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7449, 'STAINLESS', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7450, 'OTHER', 1369);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7770, '10', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7771, '12', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7772, '14', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7773, '16', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7774, '18', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7775, '20', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7776, '24', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7777, '30', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7778, '36', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7779, '42', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7780, '48', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7781, '60', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7782, '72', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7783, '96', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7784, 'OTHER', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7785, 'LESS THAN 1', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7786, '1', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7787, '1.5', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7788, '2', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7789, '3', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7790, '4', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7791, '5', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7792, '6', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7793, '8', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7794, 'XX', 869);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7823, 'Y', 1419);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7824, 'N', 1419);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7907, '4-20MA', 1126);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7908, '1-5V', 1126);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7909, 'PULSE', 1126);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7910, 'CLOSED CONTACT', 1126);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7911, 'OTHER', 1126);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8741, 'IN TRANSMITTER', 1302);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8742, 'IN SCADA', 1302);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8743, 'OTHER', 1302);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8744, 'NA', 1302);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9065, 'Y', 1262);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9066, 'N', 1262);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9088, 'INVENTRON', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9089, 'ABB', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9090, 'BRISTOL BABCOCK', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9091, 'EMERSON', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9092, 'ENDRESS-HAUSER', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9093, 'FISCHER PORTER', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9094, 'FOXBORO', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9095, 'HACH', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9096, 'HOFFER', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9097, 'HONEYWELL', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9098, 'KROHNE', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9099, 'MARSH MCBIRNEY', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9100, 'MILLTRONICS', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9101, 'NEPTUNE', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9102, 'NUSONICS-MESA', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9103, 'ROSEMOUNT', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9104, 'SPARLING', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9105, 'SENSUS', 1098);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9106, 'SIEMENS', 1098);
";

                #endregion

                #region FLOW WEIR

                case "FLOW WEIR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5481, 'PLANT INFLUENT', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5482, 'PLANT PROCESS', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5483, 'PLANT WASTE', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5484, 'SALES', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5485, 'PURCHASE FINISHED WATER', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5486, 'EMERG SALES', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5487, 'DISTRIBUTION ZONE', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5488, 'COLLECTION ZONE', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5489, 'FUEL', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5490, 'BLOW-OFF', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5491, 'OTHER', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5492, 'PLANT FINISHED WATER', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5493, 'PLANT BACKWASH', 1330);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5853, 'Y', 1363);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5854, 'N', 1363);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5855, 'XX', 1363);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5864, 'IN WATER', 1057);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5865, 'FT WATER', 1057);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5866, 'IN HG', 1057);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6024, 'PROFIBUS', 1480);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6025, 'HART', 1480);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6026, 'FOXCOM', 1480);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6027, 'MODBUS', 1480);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6028, 'RS232', 1480);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6609, 'GPM', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6610, 'MGD', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6611, 'GPH', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6612, 'CFS', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6613, 'GPD', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6614, 'LB/HR', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6615, 'LB/DAY', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6616, 'OTHER', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6617, 'CFM', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6618, 'LPH', 1157);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6669, 'RECTANGULAR', 1133);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6670, 'V-NOTCH', 1133);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6671, 'XX', 1133);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6871, '2 WIRE', 1385);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6872, '4 WIRE', 1385);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7435, 'ALUMINUM', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7436, 'BRONZE', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7437, 'CAST IRON', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7438, 'DUCTILE IRON', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7439, 'PTFE', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7440, 'PVC', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7441, 'STAINLESS', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7442, 'OTHER', 1014);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7745, '10', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7746, '12', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7747, '14', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7748, '16', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7749, '18', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7750, '20', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7751, '24', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7752, '30', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7753, '36', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7754, '42', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7755, '48', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7756, '60', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7757, '72', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7758, '96', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7759, 'OTHER', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7760, 'LESS THAN 1', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7761, '1', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7762, '1.5', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7763, '2', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7764, '3', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7765, '4', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7766, '5', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7767, '6', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7768, '8', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7769, 'XX', 1306);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7821, 'Y', 1163);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7822, 'N', 1163);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7902, '4-20MA', 912);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7903, '1-5V', 912);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7904, 'PULSE', 912);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7905, 'CLOSED CONTACT', 912);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7906, 'OTHER', 912);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8737, 'IN TRANSMITTER', 1423);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8738, 'IN SCADA', 1423);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8739, 'OTHER', 1423);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8740, 'NA', 1423);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9067, 'Y', 969);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9068, 'N', 969);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9069, 'INVENTRON', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9070, 'ABB', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9071, 'BRISTOL BABCOCK', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9072, 'EMERSON', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9073, 'ENDRESS-HAUSER', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9074, 'FISCHER PORTER', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9075, 'FOXBORO', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9076, 'HACH', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9077, 'HOFFER', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9078, 'HONEYWELL', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9079, 'KROHNE', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9080, 'MARSH MCBIRNEY', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9081, 'MILLTRONICS', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9082, 'NEPTUNE', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9083, 'NUSONICS-MESA', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9084, 'ROSEMOUNT', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9085, 'SPARLING', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9086, 'SENSUS', 1526);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9087, 'SIEMENS', 1526);
";

                #endregion

                #region FUEL TANK

                case "FUEL TANK":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5723, 'GASOLINE', 872);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5724, 'DIESEL', 872);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5725, 'LPG', 872);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5726, 'FUEL OIL', 872);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7127, 'INDOORS', 1546);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7128, 'OUTDOORS', 1546);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8920, 'Y', 894);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8921, 'N', 894);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8922, 'XX', 894);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8953, 'STEEL', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8954, 'STAINLESS STEEL', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8955, 'PLASTIC', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8956, 'FIBERGLASS', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8957, 'OTHER', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8958, 'CONCRETE', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8959, 'XX', 1533);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9000, 'VACUUM', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9001, 'ATMOSPHERIC', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9002, 'LESS THAN 100PSIG', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9003, 'LESS THAN 2PSIG', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9004, 'GREATER THAN 100PSIG', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9005, 'LESS THAN 15PSIG', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9006, '15-100PSIG', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9007, 'XX', 1372);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9028, 'Y', 960);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9029, 'N', 960);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9030, 'XX', 960);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9046, 'NON-PRESSURIZED FUEL', 1062);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9047, 'PRESSURIZED/BLADDER FUEL', 1062);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9048, 'XX', 1062);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9160, 'Y', 1073);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9161, 'N', 1073);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9162, 'XX', 1073);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10300, 'ALERT CUSTOMERS BEFORE', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10301, 'SPECIAL FLUSHING CONCERN', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10302, 'VALVE CLOSED/PLUGGED', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10303, 'OPEN SLOWLY', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10304, 'CLOSE SLOWLY', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10305, 'NO VEHICLE ACCESS', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10306, 'BACK-SIDE YARD', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10307, 'PUMPOUT REQUIRED', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10308, 'OTHER', 1658);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10309, 'NG', 872);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10310, 'OTHER', 872);
";

                #endregion

                #region GAS DETECTOR

                case "GAS DETECTOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5143, 'SIREN', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5144, 'STROBE', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5145, 'SIREN/STROBE', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5146, 'AUTO SHUTDOWN', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5147, 'AUTO SHUT/VENTILATION', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5148, 'OTHER', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5149, 'LIGHT BUILDING', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5150, 'SCADA ALARM', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5151, 'XX', 1374);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5778, 'Y', 1504);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5779, 'N', 1504);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6685, 'CHLORINE', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6686, 'AMMONIA', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6687, 'OXYGEN', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6688, 'HYDROGEN', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6689, 'HYDROGEN SULFIDE', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6690, 'SULFUR DIOXIDE', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6691, 'CARBON MONOXIDE', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6692, 'DUST', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6693, 'OTHER', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6694, 'MULTI EXP-H2S-O2', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6695, 'XX', 1300);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8652, 'PERMANENT', 1223);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8653, 'PORTABLE', 1223);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8654, 'XX', 1223);
";

                #endregion

                #region GEARBOX

                case "GEARBOX":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5494, 'WATER PROCESSING', 1051);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5495, 'WASTE PROCESSING', 1051);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5496, 'PLANT UTILITY', 1051);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5497, 'ENVIRONMENTAL COMPLIANCE', 1051);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5498, 'OTHER', 1051);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6711, 'DIRECT COUPLED', 1026);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6712, 'FLUID COUPLED', 1026);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6713, 'XX', 1026);
";

                #endregion

                #region GRAVITY SEWER MAIN

                case "GRAVITY SEWER MAIN":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5762, 'EASEMENT', 1074);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5763, 'COMPANY', 1074);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5764, 'PRIVATE', 1074);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5765, 'PUBLIC ROW', 1074);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6404, '2', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6405, '4', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6406, '6', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6407, '8', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6408, '10', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6409, '12', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6410, '14', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6411, '16', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6412, '18', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6413, '20', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6414, '24', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6415, '30', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6416, '36', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6417, '42', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6418, '48', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6419, '60', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6420, '72', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6421, 'OTHER', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6422, '54', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6423, '2.25', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6424, '2.5', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6425, '3', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6426, '5', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6427, '15', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6428, '21', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6429, '27', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6430, '0.625', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6431, '0.75', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6432, '1', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6433, '1.25', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6434, '1.5', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6435, '66', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6436, '0.25', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6437, '0.5', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6438, 'XX', 986);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6601, 'EAST', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6602, 'NORTH', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6603, 'NORTH EAST', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6604, 'NORTH WEST', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6605, 'SOUTH', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6606, 'SOUTH EAST', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6607, 'SOUTH WEST', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6608, 'WEST', 1441);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6727, 'COLLECTOR', 1089);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6728, 'INTERCEPTOR', 1089);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6729, 'INVERTEDSIPHON-GM', 1089);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6730, 'OUTFALL', 1089);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6731, 'XX', 1089);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7082, 'YES', 932);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7083, 'NO', 932);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7084, 'SLIPLINING', 1220);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7085, 'FULLYSTRUCTURED', 1220);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7086, 'SEMISTRUCTURED', 1220);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7087, 'PIPEBURSTING', 1220);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8025, 'PVC', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8026, 'HDPE', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8027, 'RCP', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8028, 'PCCP', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8029, 'AC', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8030, 'GALV', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8031, 'CU', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8032, 'ST', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8033, 'DI', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8034, 'OTH', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8035, 'BR', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8036, 'CIU', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8037, 'CIL', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8038, 'PE', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8039, 'LD', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8040, 'WI', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8041, 'UNK', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8042, 'CI', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8043, 'ABS', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8044, 'VCP', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8045, 'BFP', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8046, 'XX', 1388);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8722, 'CLEANOUT INSTALLED', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8723, 'USE LOW PRESSURE', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8724, 'OTHER', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8725, 'NONE', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8726, 'ALERT CUSTOMER BEFORE', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8727, 'BACK-SIDE YARD', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8728, 'SPECIAL TRAFFIC CONCERN', 898);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8880, 'STREET', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8881, 'DRIVEWAY', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8882, 'PARKING', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8883, 'SIDEWALK', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8884, 'OTHER', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8885, 'UNPAVED', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8886, 'XX', 1564);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10122, 'COMBINED', 1239);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10123, 'IO EFFLUENT', 1239);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10124, 'RAW', 1239);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10125, 'RECLAIMED', 1239);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10126, 'RO EFFLUENT', 1239);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10127, 'STORM', 1239);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10128, 'TREATED', 1239);
";

                #endregion

                #region GRINDER

                case "GRINDER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6439, '2', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6440, '4', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6441, '6', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6442, '8', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6443, '10', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6444, '12', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6445, '14', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6446, '16', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6447, '18', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6448, '20', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6449, '24', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6450, '30', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6451, '36', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6452, '42', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6453, '48', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6454, '60', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6455, '72', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6456, 'OTHER', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6457, '54', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6458, '2.25', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6459, '2.5', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6460, '3', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6461, '5', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6462, '15', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6463, '21', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6464, '27', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6465, '0.625', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6466, '0.75', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6467, '1', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6468, '1.25', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6469, '1.5', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6470, '66', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6471, '0.25', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6472, '0.5', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6473, 'XX', 1151);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6732, 'ELECTRIC GRINDER', 1477);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6733, 'HYDRAULIC GRINDER', 1477);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6734, 'XX', 1477);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7635, 'PIPE', 1364);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7636, 'CHANNEL', 1364);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7807, '5', 1196);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7808, '7', 1196);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7809, '11', 1196);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7810, 'OTHER', 1196);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7899, 'HORIZONTAL', 1362);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7900, 'VERTICAL', 1362);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7901, 'XX', 1362);
";

                #endregion

                #region HEAT EXCHANGER

                case "HEAT EXCHANGER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6765, 'PLATE EXC', 1101);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6766, 'TUBE EXC', 1101);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6767, 'XX', 1101);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7167, 'ALUMINUM', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7168, 'BRONZE', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7169, 'CAST IRON', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7170, 'DUCTILE IRON', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7171, 'OTHER', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7172, 'PTFE', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7173, 'PVC', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7174, 'STAINLESS', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7175, 'STEEL', 1054);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7223, 'ALUMINUM', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7224, 'BRONZE', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7225, 'CAST IRON', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7226, 'STAINLESS', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7227, 'DUCTILE IRON', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7228, 'OTHER', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7229, 'PTFE', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7230, 'PVC', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7231, 'STEEL', 902);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7244, 'WATER', 1337);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7245, 'GLYCOL', 1337);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7246, 'AIR', 1337);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7247, 'EXHAUST', 1337);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7248, 'HEAT TRANSFER FLUID', 1337);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7249, 'OTHER', 1337);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7305, 'WATER', 952);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7306, 'AIR', 952);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7307, 'GLYCOL', 952);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7308, 'EXHAUST', 952);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7309, 'HEAT TRANSFER FLUID', 952);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7310, 'OTHER', 952);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7972, 'BTU/HR', 1571);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7973, 'TONS', 1571);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7974, 'CFM', 1571);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7975, 'OTHER', 1571);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7976, 'WATTS', 1571);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7977, 'KW', 1571);
";

                #endregion

                #region HOIST

                case "HOIST":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5867, 'LB', 1042);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5868, 'KG', 1042);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6756, 'CHAINFALL', 1267);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6757, 'GANTRY', 1267);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6758, 'XX', 1267);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8555, 'Y', 1064);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8556, 'N', 1064);
";

                #endregion

                #region HVAC CHILLER

                case "HVAC CHILLER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5501, 'COMFORT', 1297);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5502, 'PROCESSING', 1297);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5503, 'FREEZE PROTECTION', 1297);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5504, 'OTHER', 1297);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6285, 'CONTINUOUS', 970);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6286, 'INTERMITTENT', 970);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6500, 'ELECTRIC', 1173);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6501, 'LIQ FUEL', 1173);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6502, 'NAT GAS', 1173);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6503, 'OTHER', 1173);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6759, 'HVAC-CHL*', 916);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6760, 'XX', 916);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7948, 'BTU/HR', 1278);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7949, 'TONS', 1278);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7950, 'CFM', 1278);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7951, 'OTHER', 1278);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7952, 'WATTS', 1278);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7953, 'KW', 1278);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9780, '6V', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9781, '12V', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9782, '24V', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9783, '120', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9784, '120/208', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9785, '120/240', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9786, '208Y/120', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9787, '208', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9788, '230', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9789, '230/460', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9790, '240', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9791, '277', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9792, '277/480', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9793, '460', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9794, '480', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9795, '600', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9796, '2300', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9797, '2300/4160', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9798, '2400', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9799, '4160', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9800, '13KV', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9801, '25KV', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9802, '33KV', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9803, 'OTHER', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9804, 'LESS THEN 100', 1350);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9805, '90VDC OR LESS', 1350);
";

                #endregion

                #region HVAC COMBINATION UNIT

                case "HVAC COMBINATION UNIT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5505, 'PROCESSING', 926);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5506, 'COMFORT', 926);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5507, 'FREEZE PROTECTION', 926);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5508, 'VENTILATION', 926);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5509, 'OTHER', 926);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6287, 'CONTINUOUS', 1474);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6288, 'INTERMITTENT', 1474);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6508, 'ELECTRIC', 1482);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6509, 'LIQ FUEL', 1482);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6510, 'NAT GAS', 1482);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6511, 'OTHER', 1482);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6761, 'HVAC-CMB*', 1111);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6762, 'XX', 1111);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7942, 'BTU/HR', 1050);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7943, 'TONS', 1050);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7944, 'CFM', 1050);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7945, 'OTHER', 1050);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7946, 'WATTS', 1050);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7947, 'KW', 1050);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9806, '6V', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9807, '12V', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9808, '24V', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9809, '120', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9810, '120/208', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9811, '120/240', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9812, '208Y/120', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9813, '208', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9814, '230', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9815, '230/460', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9816, '240', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9817, '277', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9818, '277/480', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9819, '460', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9820, '480', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9821, '600', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9822, '2300', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9823, '2300/4160', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9824, '2400', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9825, '4160', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9826, '13KV', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9827, '25KV', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9828, '33KV', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9829, 'OTHER', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9830, 'LESS THEN 100', 1169);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9831, '90VDC OR LESS', 1169);
";

                #endregion

                #region HVAC DEHUMIDIFIER

                case "HVAC DEHUMIDIFIER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5510, 'PROCESSING', 1531);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5511, 'FREEZE PROTECTION', 1531);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5512, 'COMFORT', 1531);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5513, 'OTHER', 1531);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5514, 'VENTILATION', 1531);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6295, 'CONTINUOUS', 1538);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6296, 'INTERMITTENT', 1538);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6504, 'ELECTRIC', 1560);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6505, 'LIQ FUEL', 1560);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6506, 'NAT GAS', 1560);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6507, 'OTHER', 1560);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6763, 'HVAC-DHM*', 1086);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6764, 'XX', 1086);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7966, 'BTU/HR', 1154);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7967, 'TONS', 1154);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7968, 'CFM', 1154);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7969, 'OTHER', 1154);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7970, 'WATTS', 1154);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7971, 'KW', 1154);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9569, '6V', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9570, '12V', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9571, '24V', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9572, '120', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9573, '120/208', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9574, '120/240', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9575, '208Y/120', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9576, '208', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9577, '230', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9578, '230/460', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9579, '240', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9580, '277', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9581, '277/480', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9582, '460', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9583, '480', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9584, '600', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9585, '2300', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9586, '2300/4160', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9587, '2400', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9588, '4160', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9589, '13KV', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9590, '25KV', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9591, '33KV', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9592, 'OTHER', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9593, 'LESS THEN 100', 1030);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9594, '90VDC OR LESS', 1030);
";

                #endregion

                #region HVAC HEATER

                case "HVAC HEATER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5515, 'PROCESSING', 972);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5516, 'FREEZE PROTECTION', 972);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5517, 'COMFORT', 972);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5518, 'OTHER', 972);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6297, 'CONTINUOUS', 1550);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6298, 'INTERMITTENT', 1550);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6496, 'ELECTRIC', 922);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6497, 'LIQ FUEL', 922);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6498, 'NAT GAS', 922);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6499, 'OTHER', 922);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6768, 'HVAC-HTR*', 951);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6769, 'XX', 951);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7978, 'BTU/HR', 996);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7979, 'TONS', 996);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7980, 'CFM', 996);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7981, 'OTHER', 996);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7982, 'WATTS', 996);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7983, 'KW', 996);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9595, '6V', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9596, '12V', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9597, '24V', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9598, '120', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9599, '120/208', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9600, '120/240', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9601, '208Y/120', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9602, '208', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9603, '230', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9604, '230/460', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9605, '240', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9606, '277', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9607, '277/480', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9608, '460', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9609, '480', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9610, '600', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9611, '2300', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9612, '2300/4160', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9613, '2400', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9614, '4160', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9615, '13KV', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9616, '25KV', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9617, '33KV', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9618, 'OTHER', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9619, 'LESS THEN 100', 1131);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9620, '90VDC OR LESS', 1131);
";

                #endregion

                #region HVAC VENTILATOR

                case "HVAC VENTILATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5524, 'FREEZE PROTECTION', 1456);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5525, 'COMFORT', 1456);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5526, 'PROCESSING', 1456);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5527, 'OTHER', 1456);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5528, 'VENTILATION', 1456);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6293, 'CONTINUOUS', 1035);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6294, 'INTERMITTENT', 1035);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6488, 'ELECTRIC', 1327);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6489, 'LIQ FUEL', 1327);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6490, 'NAT GAS', 1327);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6491, 'OTHER', 1327);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6772, 'HVAC-VNT*', 1361);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6773, 'XX', 1361);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7996, 'BTU/HR', 1576);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7997, 'TONS', 1576);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7998, 'CFM', 1576);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7999, 'OTHER', 1576);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8000, 'WATTS', 1576);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8001, 'KW', 1576);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9631, '6V', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9632, '12V', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9633, '24V', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9634, '120', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9635, '120/208', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9636, '120/240', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9637, '208Y/120', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9638, '208', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9639, '230', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9640, '230/460', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9641, '240', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9642, '277', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9643, '277/480', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9644, '460', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9645, '480', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9646, '600', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9663, '2300', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9664, '2300/4160', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9665, '2400', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9666, '4160', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9667, '13KV', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9668, '25KV', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9669, '33KV', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9670, 'OTHER', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9671, 'LESS THEN 100', 1008);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9672, '90VDC OR LESS', 1008);
";

                #endregion

                #region HYDRANT

                case "HYDRANT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6299, '12', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6300, '14', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6301, '16', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6302, '18', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6303, '20', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6304, '24', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6305, '30', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6306, '36', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6307, '42', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6308, '48', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6309, '60', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6310, '72', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6311, 'OTHER', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6312, '54', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6313, '2.25', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6314, '2.5', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6315, '3', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6316, '5', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6317, '15', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6318, '21', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6319, '27', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6320, '0.625', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6321, '0.75', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6322, '1', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6323, '1.25', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6324, '1.5', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6325, '66', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6326, '0.25', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6327, '0.5', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6328, 'XX', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6329, '2', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6330, '4', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6331, '6', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6332, '8', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6333, '10', 1046);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6778, '4', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6779, '6', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6780, '8', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6781, '3', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6782, '10', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6783, '12', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6784, 'XX', 1289);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6785, 'Y', 1493);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6786, 'N', 1493);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6787, 'XX', 1493);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6788, 'XX', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6789, '2.5', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6790, '4', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6791, '4.25', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6792, '4.5', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6793, '4.75', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6794, '5.25', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6795, '5.5', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6796, '5', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6797, '6', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6798, '3', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6799, '3.25', 990);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6800, 'BREAKAWAY', 1280);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6801, 'FULL BARREL', 1280);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6802, 'XX', 1280);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6803, 'PRIVATE FIRE', 1484);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6804, 'PUBLIC FIRE', 1484);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6805, 'UNBILLED AW HYDRANT', 1484);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6806, 'XX', 1484);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6807, 'BONNETS', 984);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6808, 'BONNETS/CAPS', 984);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6809, 'RINGS', 984);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6810, 'CAPS', 984);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6811, 'MAIN SIZE', 1021);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6812, 'FLOW', 1021);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6813, 'GREEN', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6814, 'ORANGE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6815, 'RED', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6816, 'SILVER', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6817, 'SILVER/RED', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6818, 'YELLOW', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6819, 'YELLOW/RED', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6820, '4 NFPA RED', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6821, 'WHITE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6822, 'BLUE/WHITE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6823, '*NONE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6824, '2 NFPA GREEN', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6825, '1 NFPA LT BLUE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6826, '3 NFPA ORANGE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6827, 'Z-OTHER', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6828, 'BLUE', 1418);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6829, 'Y', 1518);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6830, 'N', 1518);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6831, 'NONE', 991);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6832, 'OTHER', 991);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6833, 'SPECIAL OPERATING NUT', 991);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6834, 'STRAPS/DOMES', 991);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6835, 'SPECIAL CAPS/NOZZLES', 991);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6836, '2 STEAMERS', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6837, 'OTHER', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6838, '1 SIDE PORT/1 STEAMER', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6839, '1 SIDE PORT/2 STEAMERS', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6840, '1 STEAMER', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6841, '1 SIDE PORT', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6842, 'XX', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6843, '2 SIDE/1 STEAMER', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6844, '2 SIDE PORTS', 1207);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6845, '2.5', 1019);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6846, 'OTHER', 1019);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6847, '2.25', 1019);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6848, '2.625', 1019);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6849, 'XX', 1019);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6850, '4', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6851, '5', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6852, '6', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6853, '4.5', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6854, '5.25', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6855, '3.5', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6856, 'XX', 1068);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6857, 'NST', 955);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6858, 'STORZ', 955);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6859, 'OTHER', 955);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6860, 'BLISS', 955);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6861, 'XX', 955);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6862, 'GREASE', 1174);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6863, 'OIL', 1174);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6864, 'DRY', 936);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6865, 'DRYN', 936);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6866, 'WET', 936);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6867, 'XX', 936);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6989, 'MJ', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6990, 'RESTRAINED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6991, 'LEAD', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6992, 'FLANGED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6993, 'BALL & SOCKET', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6994, 'OTHER', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6995, 'THREADED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6996, 'SOLDERED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6997, 'FUSED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6998, 'LEADITE', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6999, 'WELDED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7000, 'GLUED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7001, 'PUSH ON', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7002, 'GLUED/CEMENTED', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7003, 'COMPRESSION', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7004, 'FLARE', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7005, 'XX', 1107);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7838, 'LEFT', 1011);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7839, 'RIGHT', 1011);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7840, 'XX', 1011);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8069, 'PVC', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8070, 'HDPE', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8071, 'RCP', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8072, 'PCCP', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8073, 'AC', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8074, 'GALV', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8075, 'CU', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8076, 'ST', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8077, 'DI', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8078, 'OTH', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8079, 'BR', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8080, 'CIU', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8081, 'CIL', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8082, 'PE', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8083, 'LD', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8084, 'WI', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8085, 'UNK', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8086, 'CI', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8087, 'ABS', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8088, 'VCP', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8089, 'BFP', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8090, 'XX', 1178);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8448, '150', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8449, '200', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8450, '250', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8451, '300', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8452, '350', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8453, '175', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8454, 'XX', 1502);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8695, 'ALERT CUSTOMERS BEFORE', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8696, 'SPECIAL FLUSHING CONCERN', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8697, 'VALVE CLOSED/PLUGGED', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8698, 'OPEN SLOWLY', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8699, 'CLOSE SLOWLY', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8700, 'NO VEHICLE ACCESS', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8701, 'BACK-SIDE YARD', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8702, 'PUMPOUT REQUIRED', 1428);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8703, 'OTHER', 1428);
";

                #endregion

                #region INDICATOR

                case "INDICATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6868, 'ELECTRONIC INDICATOR', 1281);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6869, 'MECHANICAL INDICATOR', 1281);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6870, 'XX', 1281);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6911, 'INCHES', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6912, 'LBS', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6913, 'KG', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6914, 'PPM', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6915, 'PPB', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6916, 'MOHMS', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6917, 'AMPS', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6918, 'VOLTS', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6919, 'KWH', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6920, 'RPM', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6921, 'HZ', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6922, 'OTHER', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6935, 'DEG F', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6936, 'DEG C', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6937, 'PSI', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6938, 'IN H20', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6939, 'FT H20', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6940, 'FEET', 1192);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7141, 'Y', 1277);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7142, 'N', 1277);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7715, '1,2 INDOOR', 1376);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7716, '3 RAINTIGHT', 1376);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7717, '4 SPLASHPROOF', 1376);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7718, '6 SHORTTERM SUBMERSIBLE', 1376);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7719, '7 EXPLOSION PROOF', 1376);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7720, '4X STAINLESS SPLASHPROOF', 1376);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7813, 'Y', 1430);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7814, 'N', 1430);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7927, '4-20MA', 1377);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7928, '1-5V', 1377);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7929, 'PULSE', 1377);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7930, 'CLOSED CONTACT', 1377);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7931, 'OTHER', 1377);
";

                #endregion

                #region INSTRUMENT SWITCH

                case "INSTRUMENT SWITCH":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6923, 'INCHES', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6924, 'LBS', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6925, 'KG', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6926, 'PPM', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6927, 'PPB', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6928, 'MOHMS', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6929, 'AMPS', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6930, 'VOLTS', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6931, 'KWH', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6932, 'RPM', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6933, 'HZ', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6934, 'OTHER', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6941, 'DEG F', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6942, 'DEG C', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6943, 'PSI', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6944, 'IN H20', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6945, 'FT H20', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6946, 'FEET', 1561);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6965, 'DIFF PRESSURE SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6966, 'FLOW SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6967, 'LEVEL SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6968, 'LIMIT SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6969, 'OTHER SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6970, 'PRESSURE SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6971, 'PROXIMITY SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6972, 'TEMPERATURE SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6973, 'TORQUE SW', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6974, 'XX', 1204);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7139, 'Y', 1539);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7140, 'N', 1539);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7709, '1,2 INDOOR', 1226);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7710, '3 RAINTIGHT', 1226);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7711, '4 SPLASHPROOF', 1226);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7712, '6 SHORTTERM SUBMERSIBLE', 1226);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7713, '7 EXPLOSION PROOF', 1226);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7714, '4X STAINLESS SPLASHPROOF', 1226);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7815, 'Y', 1331);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7816, 'N', 1331);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7922, '4-20MA', 978);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7923, '1-5V', 978);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7924, 'PULSE', 978);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7925, 'CLOSED CONTACT', 978);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7926, 'OTHER', 978);
";

                #endregion

                #region KIT (SAFETY, REPAIR, HAZWOPR)

                case "KIT (SAFETY, REPAIR, HAZWOPR)":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7057, 'CHLORINE A KIT', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7058, 'CHLORINE B KIT', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7059, 'FIRST AID KIT', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7060, 'SAFETY KIT', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7061, 'SPILL KIT', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7062, 'TOOL KIT', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7063, 'XX', 1397);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8573, 'Y', 1124);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8574, 'N', 1124);
";

                #endregion

                #region LAB EQUIPMENT

                case "LAB EQUIPMENT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7064, 'LAB-AUTOCLAVE', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7065, 'LAB-BACT COUNT', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7066, 'LAB-COMPOSITE SAMPLER', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7067, 'LAB-DISHWASHER', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7068, 'LAB-HOT AIR OVEN', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7069, 'LAB-HOT PLATE', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7070, 'LAB-INCUBATOR', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7071, 'LAB-JAR TESTER', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7072, 'LAB-MAG STIRRER', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7073, 'LAB-MICROSCOPE', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7074, 'LAB-REFRIG', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7075, 'LAB-STILL', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7076, 'LAB-THERMOMETER', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7077, 'LAB-UV LAMP', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7078, 'LAB-WATER BATH', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7079, 'XX', 1208);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8575, 'Y', 1511);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8576, 'N', 1511);
";

                #endregion

                #region LEAK MONITOR

                case "LEAK MONITOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7088, 'AMI', 1566);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7089, 'MOBILE RADIO', 1566);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7090, 'FIXED', 1304);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7091, 'LIFT & SHIFT', 1304);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7092, 'REACTIVE', 1304);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7093, 'VALVE BOX', 992);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7094, 'METER PIT', 992);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7095, 'HOUSE', 992);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7096, 'HYDRANT CAP', 992);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7097, 'NOT DEPLOYED', 992);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7098, 'ACOUSTIC MON (DIST)', 1113);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7099, 'ACOUSTIC MON (TRANS)', 1113);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7100, 'XX', 1113);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7101, 'CORRELATING', 1498);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7102, 'NON-CORRELATING', 1498);
";

                #endregion

                #region MANHOLE

                case "MANHOLE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5754, 'EASEMENT', 1161);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5755, 'COMPANY', 1161);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5756, 'PRIVATE', 1161);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5757, 'PUBLIC ROW', 1161);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7080, 'YES', 1514);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7081, 'NO', 1514);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7232, 'BRICK', 1507);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7233, 'PRECAST CONCRETE', 1507);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7234, 'PVC', 1507);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7235, 'OTHER', 1507);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7451, 'BRICK', 1221);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7452, 'OTHER', 1221);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7453, 'NONE', 1221);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7454, 'ALUMINUM', 1529);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7455, 'CAST IRON', 1529);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7456, 'OTHER', 1529);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7457, 'OFF-SET', 1554);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7458, 'CONCENTRIC', 1554);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7459, 'FLAT TOP', 1554);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7460, 'OTHER', 1554);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7461, 'YES', 879);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7462, 'NO', 879);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7463, 'BRICK', 1205);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7464, 'PRECAST CONCRETE', 1205);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7465, 'OTHER', 1205);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7466, 'CAST IRON', 1578);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7467, 'ALUMINUM', 1578);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7468, 'CONCRETE', 1578);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7469, 'OTHER', 1578);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7470, '27.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7471, '27.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7472, '28', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7473, '28.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7474, '28.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7475, '28.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7476, '29', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7477, '29.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7478, '29.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7479, '29.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7480, '30', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7481, '30.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7482, '30.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7483, '30.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7484, '22.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7485, '22.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7486, '22.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7487, '23', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7488, '23.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7489, '23.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7490, '23.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7491, '24', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7492, '24.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7493, '24.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7494, '24.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7495, '25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7496, '25.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7497, '25.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7498, '25.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7499, '26', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7500, '26.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7501, '26.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7502, '26.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7503, '20', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7504, '20.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7505, '20.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7506, '20.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7507, '21', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7508, '21.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7509, '21.5', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7510, '21.75', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7511, '22', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7512, '27', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7513, '27.25', 1078);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7514, 'INSIDE', 1108);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7515, 'OUTSIDE', 1108);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7516, 'BOTH', 1108);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7517, 'NONE', 1108);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7518, 'YES', 1416);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7519, 'NO', 1416);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7520, 'OPEN PICK HOLE', 1027);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7521, 'CONCEALED WITH GASKET', 1027);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7522, 'CONCEALED WITHOUT GASKET', 1027);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7523, 'INFLOW PREVENTER INSERT', 1027);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7524, 'BOLT DOWN', 1027);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7525, 'OTHER', 1027);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7526, 'FLEXIBLE', 1387);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7527, 'MORTAR', 1387);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7528, 'NONE', 1387);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7529, 'OTHER', 1387);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7530, 'YES', 909);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7531, 'NO', 909);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7532, '4', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7533, '6', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7534, '24', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7535, '30', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7536, '36', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7537, '42', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7538, '48', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7539, '54', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7540, '60', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7541, '72', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7542, '99', 1323);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7543, 'CAST IRON', 875);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7544, 'PLASTIC', 875);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7545, 'OTHER', 875);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7546, 'NONE', 875);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7547, 'YES', 963);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7548, 'NO', 963);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7549, 'INSPECTION', 1454);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7550, 'INVERTEDSIPHON-MH', 1454);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7551, 'LAMPHOLE', 1454);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7552, 'STANDARD', 1454);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7553, 'XX', 1454);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8729, 'ALERT CUSTOMER BEFORE', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8730, 'BACK-SIDE YARD', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8731, 'SPECIAL TRAFFIC CONCERN', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8732, 'RECURRING PESTS', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8733, 'USE LOW PRESSURE', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8734, 'PUMPOUT REQUIRED', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8735, 'NO VEHICLE ACCESS', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8736, 'OTHER', 1398);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8826, 'CONCRETE', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8827, 'ASPHALT', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8828, 'GRASS', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8829, 'LANDSCAPING', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8830, 'GRAVEL', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8831, 'SOIL', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8832, 'BRICK', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8833, 'OTHER', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8834, 'UNIMPROVED', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8835, 'XX', 1472);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8894, 'STREET', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8895, 'DRIVEWAY', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8896, 'PARKING', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8897, 'SIDEWALK', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8898, 'OTHER', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8899, 'UNPAVED', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8900, 'XX', 1183);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10129, 'COMBINED', 1071);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10130, 'IO EFFLUENT', 1071);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10131, 'RAW', 1071);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10132, 'RECLAIMED', 1071);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10133, 'RO EFFLUENT', 1071);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10134, 'STORM', 1071);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10135, 'TREATED', 1071);
";

                #endregion

                #region MIXER

                case "MIXER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5534, 'WATER PROCESSING', 1583);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5535, 'WASTE PROCESSING', 1583);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5536, 'PLANT UTILITY', 1583);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5537, 'ENVIRONMENTAL COMPLIANCE', 1583);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5538, 'OTHER', 1583);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6659, 'GPM', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6660, 'MGD', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6661, 'GPH', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6662, 'CFS', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6663, 'GPD', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6664, 'LB/HR', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6665, 'LB/DAY', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6666, 'OTHER', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6667, 'CFM', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6668, 'LPH', 1052);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7554, 'ROTATING MIXER', 1079);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7555, 'STATIC MIXER', 1079);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7556, 'XX', 1079);
";

                #endregion

                #region MODEM

                case "MODEM":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5372, 'CONTROL AND DAQ', 1296);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5373, 'DAQ ONLY', 1296);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5374, 'CONTROL ONLY', 1296);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5375, 'SECURITY ONLY', 1296);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5376, 'NETWORK', 1296);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5377, 'ALARMING', 1296);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5797, '2400', 1298);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5798, '9600', 1298);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5799, '19200', 1298);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5800, '56K', 1298);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5801, 'OTHER', 1298);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6163, 'CELLULAR', 1060);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6164, 'FIBER OPTIC', 1060);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6165, 'WIRE', 1060);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6166, 'WIRELESS', 1060);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6167, 'XX', 1060);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6204, 'NONE', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6205, 'LEASE LINE', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6206, 'CELLULAR', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6207, 'LICENSE RADIO', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6208, 'NONLICENSE RADIO', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6209, 'NETWORK', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6210, 'WIRELESS', 1214);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8777, 'NONE', 1182);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8778, 'GENERATOR', 1182);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8779, 'BATTERY', 1182);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8780, 'UPS', 1182);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8781, 'OTHER', 1182);
";

                #endregion

                #region MOTOR

                case "MOTOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5539, 'CHEMICAL', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5540, 'DISTRIBUTION SYSTEM BOOSTER', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5541, 'FUEL', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5542, 'GRINDER', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5543, 'IN PLANT BOOSTER', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5544, 'IN PLANT TRANSFER', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5545, 'OTHER', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5546, 'PLANT TO DISTRIBUTION PUMP', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5547, 'PROCESS EQUIPMENT', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5548, 'RAW WATER (GROUND)', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5549, 'RAW WATER (SURFACE)', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5550, 'SAMPLE', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5551, 'SEWER LIFT', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5552, 'SLUDGE', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5553, 'SUMP', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5554, 'DITCH/TRASH', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5555, 'ENVIRONMENTAL COMPLIANCE', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5556, 'PLANT UTILITY', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5557, 'WASTE PROCESSING', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5558, 'WATER PROCESSING', 1572);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6289, 'CONTINUOUS', 1247);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6290, 'INTERMITTENT', 1247);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6975, 'A', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6976, 'E', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6977, 'B', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6978, 'F', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6979, 'H', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6980, 'N', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6981, 'OTHER', 1238);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7557, 'Y', 1475);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7558, 'N', 1475);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7559, 'ANTI FRICTION', 1400);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7560, 'SLEEVE', 1400);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7561, 'ANTI FRICTION', 965);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7562, 'SLEEVE', 965);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7563, 'A', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7564, 'B', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7565, 'C', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7566, 'D', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7567, 'E', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7568, 'F', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7569, 'G', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7570, 'H', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7571, 'I', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7572, 'J', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7573, 'K', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7574, 'L', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7575, 'M', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7576, 'N', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7577, 'O', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7578, 'P', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7579, 'Q', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7580, 'R', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7581, 'S', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7582, 'T', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7583, 'V', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7584, 'U', 1466);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7585, 'RIGID', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7586, 'FLEX', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7587, 'BELT', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7588, 'GEAR', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7589, 'GRID', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7590, 'SLIP', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7591, 'MAGNETIC', 1145);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7592, 'ODP', 1373);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7593, 'TEFC', 1373);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7594, 'TENV', 1373);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7595, 'XP', 1373);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7596, 'OTHER', 1373);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7597, 'Y', 1249);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7598, 'N', 1249);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7599, 'Y', 1510);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7600, 'N', 1510);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7601, 'OIL', 1153);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7602, 'GREASE', 1153);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7603, 'OTHER', 1153);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7604, 'OIL', 1210);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7605, 'GREASE', 1210);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7606, 'OTHER', 1210);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7607, 'A', 1275);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7608, 'B', 1275);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7609, 'C', 1275);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7610, 'D', 1275);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7611, '1', 953);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7612, '1.15', 953);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7613, '1.25', 953);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7614, '1.35', 953);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7615, 'OTHER', 953);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7616, 'DC', 942);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7617, 'SINGLE PHASE', 942);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7618, 'SUBMERSIBLE MOT', 942);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7619, 'THREE PHASE HORIZ', 942);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7620, 'THREE PHASE VERT', 942);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7621, 'XX', 942);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7622, '2300', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7623, '4160', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7624, 'OTHER', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7625, '120', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7626, '120/240', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7627, '230/460', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7628, '240', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7629, '480', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7630, '90VDC OR LESS', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7631, '208', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7632, 'XX', 885);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7893, 'HORIZONTAL', 1567);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7894, 'VERTICAL', 1567);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7895, 'XX', 1567);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8577, 'CW (FROM MOTOR FAN END)', 1519);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8578, 'CCW (FROM MOTOR FAN END)', 1519);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8624, '1200', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8625, '1800', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8626, '3600', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8627, 'VARIABLE', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8628, 'OTHER', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8629, '600', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8630, '600/1200', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8631, '900', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8632, '900/1800', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8633, '3500', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8634, '1725', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8635, '1550', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8636, 'XX', 1193);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9910, '6V', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9911, '12V', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9912, '24V', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9913, '120', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9914, '120/208', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9915, '120/240', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9916, '208Y/120', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9917, '208', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9918, '230', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9919, '230/460', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9920, '240', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9921, '277', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9922, '277/480', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9923, '460', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9924, '480', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9925, '600', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9926, '2300', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9927, '2300/4160', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9928, '2400', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9929, '4160', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9930, '13KV', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9931, '25KV', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9932, '33KV', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9933, 'OTHER', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9934, 'LESS THEN 100', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9935, '90VDC OR LESS', 1166);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10246, 'ALERT CUSTOMERS BEFORE', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10247, 'SPECIAL FLUSHING CONCERN', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10248, 'VALVE CLOSED/PLUGGED', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10249, 'OPEN SLOWLY', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10250, 'CLOSE SLOWLY', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10251, 'NO VEHICLE ACCESS', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10252, 'BACK-SIDE YARD', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10253, 'PUMPOUT REQUIRED', 1613);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10254, 'OTHER', 1613);
";

                #endregion

                #region MOTOR CONTACTOR

                case "MOTOR CONTACTOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6223, 'AIR', 1367);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6224, 'VACUUM', 1367);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6225, 'CONTACTR*', 1343);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6226, 'XX', 1343);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9962, '6V', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9963, '12V', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9964, '24V', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9965, '120', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9966, '120/208', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9967, '120/240', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9968, '208Y/120', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9969, '208', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9970, '230', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9971, '230/460', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9972, '240', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9973, '277', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9974, '277/480', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9975, '460', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9976, '480', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9977, '600', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9978, '2300', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9979, '2300/4160', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9980, '2400', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9981, '4160', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9982, '13KV', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9983, '25KV', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9984, '33KV', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9985, 'OTHER', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9986, 'LESS THEN 100', 1099);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9987, '90VDC OR LESS', 1099);
";

                #endregion

                #region MOTOR STARTER

                case "MOTOR STARTER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7633, 'MOTSTR*', 1015);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7634, 'XX', 1015);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8812, '00', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8813, '0', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8814, '1', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8815, '2', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8816, '3', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8817, '4', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8818, '5', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8819, '6', 1378);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8820, 'BI-METALLIC', 982);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8821, 'HEATERS', 982);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8822, 'FULL VOLTAGE', 959);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8823, 'REDUCED VOLTAGE', 959);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8824, 'SOFT START', 959);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8825, 'OTHER', 959);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9884, '2300', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9885, '2300/4160', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9886, '2400', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9887, '4160', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9888, '13KV', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9889, '25KV', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9890, '33KV', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9891, 'OTHER', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9892, 'LESS THEN 100', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9893, '90VDC OR LESS', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9894, '6V', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9895, '12V', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9896, '24V', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9897, '120', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9898, '120/208', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9899, '120/240', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9900, '208Y/120', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9901, '208', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9902, '230', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9903, '230/460', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9904, '240', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9905, '277', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9906, '277/480', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9907, '460', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9908, '480', 1219);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9909, '600', 1219);
";

                #endregion

                #region NETWORK ROUTER

                case "NETWORK ROUTER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5384, 'CONTROL AND DAQ', 1534);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5385, 'DAQ ONLY', 1534);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5386, 'CONTROL ONLY', 1534);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5387, 'SECURITY ONLY', 1534);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5388, 'NETWORK', 1534);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5389, 'ALARMING', 1534);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5817, '2400', 947);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5818, '9600', 947);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5819, '19200', 947);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5820, '56K', 947);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5821, 'OTHER', 947);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6170, 'COMM-RTR*', 1266);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6171, 'XX', 1266);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6190, 'NONE', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6191, 'LEASE LINE', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6192, 'CELLULAR', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6193, 'LICENSE RADIO', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6194, 'NONLICENSE RADIO', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6195, 'NETWORK', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6196, 'WIRELESS', 1121);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8757, 'NONE', 1290);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8758, 'GENERATOR', 1290);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8759, 'BATTERY', 1290);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8760, 'UPS', 1290);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8761, 'OTHER', 1290);
";

                #endregion

                #region NETWORK SWITCH

                case "NETWORK SWITCH":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5390, 'CONTROL AND DAQ', 1048);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5391, 'DAQ ONLY', 1048);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5392, 'CONTROL ONLY', 1048);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5393, 'SECURITY ONLY', 1048);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5394, 'NETWORK', 1048);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5395, 'ALARMING', 1048);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5822, '2400', 1491);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5823, '9600', 1491);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5824, '19200', 1491);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5825, '56K', 1491);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5826, 'OTHER', 1491);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6172, 'COMM-SW*', 900);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6173, 'XX', 900);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6183, 'NONE', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6184, 'LEASE LINE', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6185, 'CELLULAR', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6186, 'LICENSE RADIO', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6187, 'NONLICENSE RADIO', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6188, 'NETWORK', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6189, 'WIRELESS', 1156);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8762, 'NONE', 1276);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8763, 'GENERATOR', 1276);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8764, 'BATTERY', 1276);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8765, 'UPS', 1276);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8766, 'OTHER', 1276);
";

                #endregion

                #region NON POTABLE WATER TANK

                case "NON POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5729, 'WASH WATER', 1360);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5730, 'RAW WATER', 1360);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7125, 'INDOORS', 1090);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7126, 'OUTDOORS', 1090);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8914, 'Y', 1500);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8915, 'N', 1500);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8916, 'XX', 1500);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8967, 'STEEL', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8968, 'STAINLESS STEEL', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8969, 'PLASTIC', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8970, 'FIBERGLASS', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8971, 'OTHER', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8972, 'CONCRETE', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8973, 'XX', 1392);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8974, 'VACUUM', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8975, 'ATMOSPHERIC', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8976, 'LESS THAN 100PSIG', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8977, 'LESS THAN 2PSIG', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8978, 'GREATER THAN 100PSIG', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8979, 'LESS THAN 15PSIG', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8980, '15-100PSIG', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8981, 'XX', 925);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9037, 'Y', 1160);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9038, 'N', 1160);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9039, 'XX', 1160);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9051, 'NON-PRESSURIZED WNON', 1503);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9052, 'PRESSURIZED/BLADDER WNON', 1503);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9053, 'XX', 1503);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9154, 'Y', 1007);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9155, 'N', 1007);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9156, 'XX', 1007);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10320, 'ALERT CUSTOMERS BEFORE', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10321, 'SPECIAL FLUSHING CONCERN', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10322, 'VALVE CLOSED/PLUGGED', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10323, 'OPEN SLOWLY', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10324, 'CLOSE SLOWLY', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10325, 'NO VEHICLE ACCESS', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10326, 'BACK-SIDE YARD', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10327, 'PUMPOUT REQUIRED', 1668);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10328, 'OTHER', 1668);
";

                #endregion

                #region OPERATOR COMPUTER TERMINAL

                case "OPERATOR COMPUTER TERMINAL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5559, 'HMI', 1412);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5560, 'DATA STORAGE', 1412);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5561, 'OTHER', 1412);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6742, 'ICONICS GENESIS', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6743, 'INTELLUTION IFIX', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6744, 'INTELLUTION FIX32', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6745, 'WONDERWARE', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6746, 'TRANSDYNE', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6747, 'SURVALENT', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6748, 'OTHER', 881);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7739, 'STANDALONE', 1410);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7740, 'SCADA DOMAIN', 1410);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7741, 'OTHER', 1410);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7811, 'OIT*', 1589);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7812, 'XX', 1589);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7873, 'WINDOWS 2000', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7874, 'WINDOWS XP', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7875, 'WINDOWS 7', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7876, 'WINDOWS SERVER 2003', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7877, 'WINDOWS SERVER 2008', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7878, 'WINDOWS SERVER 2012', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7879, 'OTHER', 1112);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8519, 'NONE', 1222);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8520, '1', 1222);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8521, '2', 1222);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8522, '3', 1222);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8523, '4', 1222);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8524, '5', 1222);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8782, 'NONE', 1152);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8783, 'GENERATOR', 1152);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8784, 'BATTERY', 1152);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8785, 'UPS', 1152);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8786, 'OTHER', 1152);
";

                #endregion

                #region PC

                case "PC":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5562, 'HMI', 929);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5563, 'DATA STORAGE', 929);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5564, 'OTHER', 929);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5565, 'HMI AND STORAGE', 929);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6749, 'ICONICS GENESIS', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6750, 'INTELLUTION IFIX', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6751, 'INTELLUTION FIX32', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6752, 'WONDERWARE', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6753, 'TRANSDYNE', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6754, 'SURVALENT', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6755, 'OTHER', 884);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7742, 'STANDALONE', 868);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7743, 'SCADA DOMAIN', 868);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7744, 'OTHER', 868);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7880, 'WINDOWS 2000', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7881, 'WINDOWS XP', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7882, 'WINDOWS 7', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7883, 'WINDOWS SERVER 2003', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7884, 'WINDOWS SERVER 2008', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7885, 'WINDOWS SERVER 2012', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7886, 'OTHER', 997);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8002, 'PC*', 1122);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8003, 'XX', 1122);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8513, 'NONE', 1198);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8514, '1', 1198);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8515, '2', 1198);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8516, '3', 1198);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8517, '4', 1198);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8518, '5', 1198);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8787, 'NONE', 1371);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8788, 'GENERATOR', 1371);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8789, 'BATTERY', 1371);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8790, 'UPS', 1371);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8791, 'OTHER', 1371);
";

                #endregion

                #region PDM TOOL

                case "PDM TOOL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8004, 'IR EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8005, 'VIB EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8006, 'US EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8007, 'INSULATION TESTING EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8008, 'HIGH CURRENT TESTING EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8009, 'MULTIMETER', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8010, 'PWR QLY ANALYSIS EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8011, 'INSTR CALIB EQ', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8012, 'XX', 1134);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8567, 'Y', 958);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8568, 'N', 958);
";

                #endregion

                #region PHASE CONVERTER

                case "PHASE CONVERTER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8017, 'DIGITAL PHASE CONVERTER', 1312);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8018, 'ROTARY PHASE CONVERTER', 1312);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8019, 'STATIC PHASE CONVERTER', 1312);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8020, 'XX', 1312);
";

                #endregion

                #region PLANT VALVE

                case "PLANT VALVE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5169, 'MANUAL', 1017);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5170, 'ELECTRIC', 1017);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5171, 'HYDRAULIC', 1017);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5172, 'PNEUMATIC', 1017);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5616, 'AIR RELEASE', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5617, 'AIR/VACUUM BREAKER', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5618, 'ALTITUDE', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5619, 'BACKFLOW PREVENTION', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5620, 'BLOW-OFF', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5621, 'BYPASS', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5622, 'CHEMICAL FEED - GAS', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5623, 'CHEMICAL FEED - LIQUID', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5624, 'CLARIFIER', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5625, 'FILTER', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5626, 'FLOW CONTROL', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5627, 'IN PLANT', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5628, 'ISOLATION', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5629, 'PRESSURE REGULATION', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5630, 'PRESSURE RELIEF', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5631, 'PUMP DISCHARGE', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5632, 'PUMP SUCTION', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5633, 'RAW WATER', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5634, 'VACUUM BREAKER', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5635, 'OTHER', 1447);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5770, 'Y', 1225);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5771, 'N', 1225);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5844, 'Y', 1059);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5845, 'N', 1059);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5846, 'XX', 1059);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6706, 'WORM', 1396);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6707, 'SPUR', 1396);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6708, 'BEVEL', 1396);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6709, 'NON RISING STEM', 1396);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6710, 'RISING STEM', 1396);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7040, 'MJ', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7041, 'RESTRAINED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7042, 'LEAD', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7043, 'FLANGED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7044, 'BALL & SOCKET', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7045, 'OTHER', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7046, 'THREADED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7047, 'SOLDERED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7048, 'FUSED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7049, 'LEADITE', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7050, 'WELDED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7051, 'GLUED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7052, 'PUSH ON', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7053, 'GLUED/CEMENTED', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7054, 'COMPRESSION', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7055, 'FLARE', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7056, 'XX', 1148);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7236, 'BRASS', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7237, 'CAST IRON', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7238, 'DUCTILE IRON', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7239, 'OTHER', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7240, 'PTFE', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7241, 'PVC', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7242, 'STAINLESS', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7243, 'STEEL', 927);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7795, 'OPEN', 1544);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7796, 'CLOSED', 1544);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7797, 'THROTTLING', 1544);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7798, 'XX', 1544);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7844, 'LEFT', 1338);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7845, 'RIGHT', 1338);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7846, 'XX', 1338);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8428, 'XX', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8429, '150', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8430, '200', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8431, '250', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8432, '300', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8433, '350', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8434, '175', 1427);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8464, 'BACKFLOW W-TEST PORTS PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8465, 'ELECTRIC MOTOR PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8466, 'ELECTRIC SOLENOID ONLY PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8467, 'GAS REGULATOR PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8468, 'HYDRAULIC ONLY PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8469, 'HYDRAULIC W-PILOTS PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8470, 'MANUAL PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8471, 'PNEUMATIC PVLV', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8472, 'XX', 1563);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9175, 'SMITH', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9176, 'SUREFLOW', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9177, 'DEZURICK', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9178, 'BRAY', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9179, 'APOLLO', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9180, 'ASCO', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9181, 'AUMA', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9182, 'BECK', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9183, 'BIF', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9184, 'BIMBA', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9185, 'EIM COMPANY', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9186, 'EMERSON', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9187, 'GOLDEN ANDERSON', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9188, 'HALOGEN', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9189, 'INLINE INDUSTRIES', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9190, 'KEYSTONE', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9191, 'LIMITORQUE', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9192, 'PRATT', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9193, 'ROTORK', 1358);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9194, 'OPEN', 1590);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9195, 'CLOSED', 1590);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9196, 'LAST', 1590);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9197, 'OTHER', 1590);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9220, 'Y', 1263);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9221, 'N', 1263);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9222, 'LESS THAN 1', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9223, '0.25', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9224, '0.5', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9225, '66', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9226, '5', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9227, 'XX', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9228, '0.625', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9229, '0.75', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9230, '1', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9231, '1.25', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9232, '1.5', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9233, '2.25', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9234, '2.5', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9235, '3', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9236, '2', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9237, '4', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9238, '6', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9239, '8', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9240, '10', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9241, '12', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9242, '14', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9243, '16', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9244, '18', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9245, '20', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9246, '24', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9247, '30', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9248, '36', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9249, '42', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9250, '48', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9251, '60', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9252, '72', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9253, 'OTHER', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9254, '40', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9255, '54', 939);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9368, 'AIR RELIEF', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9369, 'AIR/VAC RELIEF', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9370, 'ANGLE', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9371, 'BALL', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9372, 'BUTTERFLY', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9373, 'CHECK', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9374, 'CHECK/FOOT', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9375, 'CURB STOP', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9376, 'DOUBLE CHECK', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9377, 'GATE', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9378, 'GLOBE', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9379, 'OTHER', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9380, 'PLUG', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9381, 'PRES REGULATOR', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9382, 'PRES RELIEF', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9383, 'RPZ', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9384, 'SOLENOID', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9385, 'TAPPING', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9386, 'TELESCOPIC', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9387, 'VAC REGULATOR', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9388, 'VAC RELIEF', 1200);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9389, 'XX', 1200);
";

                #endregion

                #region POTABLE WATER TANK

                case "POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7123, 'INDOORS', 1535);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7124, 'OUTDOORS', 1535);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8926, 'Y', 1235);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8927, 'N', 1235);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8928, 'XX', 1235);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8939, 'STEEL', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8940, 'STAINLESS STEEL', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8941, 'PLASTIC', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8942, 'FIBERGLASS', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8943, 'OTHER', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8944, 'CONCRETE', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8945, 'XX', 1135);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8982, 'VACUUM', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8983, 'ATMOSPHERIC', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9008, 'LESS THAN 100PSIG', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9009, 'LESS THAN 2PSIG', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9010, 'GREATER THAN 100PSIG', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9011, 'LESS THAN 15PSIG', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9012, '15-100PSIG', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9013, 'XX', 1354);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9040, 'Y', 1250);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9041, 'N', 1250);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9042, 'XX', 1250);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9054, 'NON-PRESSURIZED WPOT', 877);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9055, 'PRESSURIZED/BLADDER WPOT', 877);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9056, 'XX', 877);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9151, 'Y', 961);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9152, 'N', 961);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9153, 'XX', 961);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10329, 'ALERT CUSTOMERS BEFORE', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10330, 'SPECIAL FLUSHING CONCERN', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10331, 'VALVE CLOSED/PLUGGED', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10332, 'OPEN SLOWLY', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10333, 'CLOSE SLOWLY', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10334, 'NO VEHICLE ACCESS', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10335, 'BACK-SIDE YARD', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10336, 'PUMPOUT REQUIRED', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10337, 'OTHER', 1673);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10338, 'WATER', 828);
";

                #endregion

                #region POWER BREAKER

                case "POWER BREAKER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5837, 'AIR', 1117);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5838, 'VACUUM', 1117);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5839, 'OIL FILLED', 1117);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5840, 'GAS FILLED', 1117);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5841, 'MOLDED CASE', 1117);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8473, 'AIR (BREAKER)', 924);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8474, 'GAS (BREAKER)', 924);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8475, 'MOLDED CASE', 924);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8476, 'OIL (BREAKER)', 924);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8477, 'VACUUM (BREAKER)', 924);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8478, 'XX', 924);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9390, '6V', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9391, '12V', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9392, '24V', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9393, '120', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9394, '120/208', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9395, '120/240', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9396, '208Y/120', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9397, '208', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9398, '230', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9399, '230/460', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9400, '240', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9401, '277', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9402, '277/480', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9403, '460', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9404, '480', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9405, '600', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9406, '2300', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9407, '2300/4160', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9408, '2400', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9409, '4160', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9410, '13KV', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9411, '25KV', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9412, '33KV', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9413, 'OTHER', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9414, 'LESS THEN 100', 1356);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9415, '90VDC OR LESS', 1356);
";

                #endregion

                #region POWER CONDITIONER

                case "POWER CONDITIONER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5178, 'Y', 1199);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5179, 'N', 1199);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8479, 'HARMONIC FILTER CONDITIONING', 1335);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8480, 'ISOLATION TRANSFORMER', 1335);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8481, 'PFC CAPACITOR', 1335);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8482, 'POWER CONDITIONER', 1335);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8483, 'VOLTAGE REGULATOR', 1335);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8484, 'XX', 1335);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9442, '6V', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9443, '12V', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9444, '24V', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9445, '120', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9446, '120/208', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9447, '120/240', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9448, '208Y/120', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9449, '208', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9450, '230', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9451, '230/460', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9452, '240', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9453, '277', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9454, '277/480', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9455, '460', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9456, '480', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9457, '600', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9458, '2300', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9459, '2300/4160', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9460, '2400', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9461, '4160', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9462, '13KV', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9463, '25KV', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9464, '33KV', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9465, 'OTHER', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9466, 'LESS THEN 100', 1389);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9467, '90VDC OR LESS', 1389);
";

                #endregion

                #region POWER DISCONNECT

                case "POWER DISCONNECT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8485, 'LOADBREAK DISCONNECT', 892);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8486, 'NON-LOADBREAK DISCONNECT', 892);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8487, 'XX', 892);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9416, '6V', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9417, '12V', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9418, '24V', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9419, '120', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9420, '120/208', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9421, '120/240', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9422, '208Y/120', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9423, '208', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9424, '230', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9425, '230/460', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9426, '240', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9427, '277', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9428, '277/480', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9429, '460', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9430, '480', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9431, '600', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9432, '2300', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9433, '2300/4160', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9434, '2400', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9435, '4160', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9436, '13KV', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9437, '25KV', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9438, '33KV', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9439, 'OTHER', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9440, 'LESS THEN 100', 1251);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9441, '90VDC OR LESS', 1251);
";

                #endregion

                #region POWER FEEDER CABLE

                case "POWER FEEDER CABLE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8488, 'PWRFEEDR*', 1012);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8489, 'XX', 1012);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9517, '6V', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9518, '12V', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9519, '24V', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9520, '120', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9521, '120/208', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9522, '120/240', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9523, '208Y/120', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9524, '208', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9525, '230', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9526, '230/460', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9527, '240', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9528, '277', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9529, '277/480', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9530, '460', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9531, '480', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9532, '600', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9533, '2300', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9534, '2300/4160', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9535, '2400', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9536, '4160', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9537, '13KV', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9538, '25KV', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9539, '33KV', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9540, 'OTHER', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9541, 'LESS THEN 100', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9542, '90VDC OR LESS', 1522);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10146, 'THHN', 919);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10147, 'THHW', 919);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10148, 'THW', 919);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10149, 'OTHER', 919);
";

                #endregion

                #region POWER MONITOR

                case "POWER MONITOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5180, 'Y', 1523);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5181, 'N', 1523);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8490, 'PWRMON*', 1294);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8491, 'XX', 1294);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9491, '6V', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9492, '12V', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9493, '24V', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9494, '120', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9495, '120/208', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9496, '120/240', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9497, '208Y/120', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9498, '208', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9499, '230', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9500, '230/460', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9501, '240', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9502, '277', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9503, '277/480', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9504, '460', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9505, '480', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9506, '600', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9507, '2300', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9508, '2300/4160', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9509, '2400', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9510, '4160', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9511, '13KV', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9512, '25KV', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9513, '33KV', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9514, 'OTHER', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9515, 'LESS THEN 100', 1264);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9516, '90VDC OR LESS', 1264);
";

                #endregion

                #region POWER PANEL

                case "POWER PANEL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6672, 'Y', 874);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6673, 'N', 874);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8492, 'BREAKER PROTECTED', 1442);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8493, 'FUSE PROTECTED', 1442);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8494, 'XX', 1442);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9468, '6V', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9469, '12V', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9470, '24V', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9471, '120', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9472, '120/208', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9473, '120/240', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9474, '208Y/120', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9475, '208', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9476, '230', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9477, '230/460', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9478, '240', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9479, '277', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9480, '277/480', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9481, '460', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9482, '480', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9483, '600', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9484, '2300', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9485, '2300/4160', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9486, '2400', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9487, '4160', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9488, '13KV', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9489, '25KV', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9490, '33KV', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9673, 'OTHER', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9674, 'LESS THEN 100', 1211);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9675, '90VDC OR LESS', 1211);
";

                #endregion

                #region POWER RELAY

                case "POWER RELAY":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5833, 'MAIN', 1520);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5834, 'BUS TIE', 1520);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5835, 'INCOMING', 1520);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5836, 'OTHER', 1520);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8013, 'PHASE 1', 1557);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8014, 'PHASE 2', 1557);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8015, 'PHASE 3', 1557);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8016, 'GROUND', 1557);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8495, 'PWRRELAY*', 1449);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8496, 'XX', 1449);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9702, '6V', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9703, '12V', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9704, '24V', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9705, '120', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9706, '120/208', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9707, '120/240', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9708, '208Y/120', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9709, '208', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9710, '230', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9711, '230/460', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9712, '240', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9713, '277', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9714, '277/480', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9715, '460', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9716, '480', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9717, '600', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9718, '2300', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9719, '2300/4160', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9720, '2400', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9721, '4160', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9722, '13KV', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9723, '25KV', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9724, '33KV', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9725, 'OTHER', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9726, 'LESS THEN 100', 1429);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9727, '90VDC OR LESS', 1429);
";

                #endregion

                #region POWER SURGE PROTECTION

                case "POWER SURGE PROTECTION":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8497, 'LIGHTNING ARRESTOR', 1109);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8498, 'SURGE SUPPRESSOR', 1109);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8499, 'TVSS', 1109);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8500, 'XX', 1109);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8912, 'Y', 1002);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8913, 'N', 1002);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9676, '6V', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9677, '12V', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9678, '24V', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9679, '120', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9680, '120/208', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9681, '120/240', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9682, '208Y/120', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9683, '208', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9684, '230', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9685, '230/460', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9686, '240', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9687, '277', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9688, '277/480', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9689, '460', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9690, '480', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9691, '600', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9692, '2300', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9693, '2300/4160', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9694, '2400', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9695, '4160', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9696, '13KV', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9697, '25KV', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9698, '33KV', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9699, 'OTHER', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9700, 'LESS THEN 100', 989);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9701, '90VDC OR LESS', 989);
";

                #endregion

                #region POWER TRANSFER SWITCH

                case "POWER TRANSFER SWITCH":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8458, 'Y', 1123);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8459, 'N', 1123);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9107, 'AUTOMATIC TRANSFER', 983);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9108, 'MANUAL TRANSFER', 983);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9109, 'XX', 983);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9858, '6V', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9859, '12V', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9860, '24V', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9861, '120', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9862, '120/208', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9863, '120/240', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9864, '208Y/120', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9865, '208', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9866, '230', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9867, '230/460', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9868, '240', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9869, '277', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9870, '277/480', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9871, '460', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9872, '480', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9873, '600', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9874, '2300', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9875, '2300/4160', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9876, '2400', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9877, '4160', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9878, '13KV', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9879, '25KV', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9880, '33KV', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9881, 'OTHER', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9882, 'LESS THEN 100', 1092);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9883, '90VDC OR LESS', 1092);
";

                #endregion

                #region PRESSURE DAMPER

                case "PRESSURE DAMPER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8426, '*PRESDMP', 1287);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8427, 'XX', 1287);
";

                #endregion

                #region PRINTER

                case "PRINTER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5613, 'HMI', 1394);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5614, 'DATA STORAGE', 1394);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5615, 'OTHER', 1394);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7736, 'STANDALONE', 1425);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7737, 'SCADA DOMAIN', 1425);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7738, 'OTHER', 1425);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7866, 'WINDOWS 2000', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7867, 'WINDOWS XP', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7868, 'WINDOWS 7', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7869, 'WINDOWS SERVER 2003', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7870, 'WINDOWS SERVER 2008', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7871, 'WINDOWS SERVER 2012', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7872, 'OTHER', 1487);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8456, 'PRNTR*', 1028);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8457, 'XX', 1028);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8507, 'NONE', 1582);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8508, '1', 1582);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8509, '2', 1582);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8510, '3', 1582);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8511, '4', 1582);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8512, '5', 1582);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8797, 'NONE', 1234);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8798, 'GENERATOR', 1234);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8799, 'BATTERY', 1234);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8800, 'UPS', 1234);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8801, 'OTHER', 1234);
";

                #endregion

                #region PUMP CENTRIFUGAL

                case "PUMP CENTRIFUGAL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5566, 'PLANT TO DISTRIBUTION PUMP', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5567, 'DISTRIBUTION SYSTEM BOOSTER', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5568, 'CHEMICAL', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5569, 'SEWER LIFT', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5570, 'GRINDER', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5571, 'SLUDGE', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5572, 'FUEL', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5573, 'SAMPLE', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5574, 'DITCH/TRASH PUMPS', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5575, 'PROCESS EQUIPMENT', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5576, 'HVAC', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5577, 'DITCH/TRASH', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5578, 'RAW WATER (SURFACE)', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5579, 'RAW WATER (GROUND)', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5580, 'IN PLANT TRANSFER', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5581, 'IN PLANT BOOSTER', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5582, 'SUMP', 1452);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6629, 'GPM', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6630, 'MGD', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6631, 'GPH', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6632, 'CFS', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6633, 'GPD', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6634, 'LB/HR', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6635, 'LB/DAY', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6636, 'OTHER', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6637, 'CFM', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6638, 'LPH', 1492);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7151, 'WATER', 1446);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7152, 'OIL', 1446);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7153, 'GREASE', 1446);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7154, 'OTHER', 1446);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7155, 'WATER', 901);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7156, 'OIL', 901);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7157, 'GREASE', 901);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7158, 'OTHER', 901);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7890, 'HORIZONTAL', 1087);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7891, 'VERTICAL', 1087);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7892, 'XX', 1087);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8113, 'ANTI FRICTION', 1359);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8114, 'SLEEVE', 1359);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8123, 'ANTI FRICTION', 980);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8124, 'SLEEVE', 980);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8173, '10', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8174, '12', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8175, '14', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8176, '16', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8177, '18', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8178, '20', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8179, '24', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8180, '30', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8181, '36', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8182, '42', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8183, '48', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8184, 'OTHER', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8185, '1', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8186, '1.5', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8187, '2', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8188, '2.5', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8189, '3', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8190, '4', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8191, '5', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8192, '6', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8193, '8', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8194, 'LESS THAN 1', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8195, '1.25', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8196, 'XX', 1016);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8203, 'STAINLESS', 1403);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8204, 'BRONZE', 1403);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8205, 'PHENOLIC', 1403);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8206, 'CERAMIC', 1403);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8207, 'CAST IRON', 1403);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8208, 'OTHER', 1403);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8238, 'LESS THAN 1', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8239, '1', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8240, '1.25', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8241, '1.5', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8242, '2', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8243, '2.5', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8244, '3', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8245, '4', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8246, '5', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8247, '6', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8248, '8', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8249, '10', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8250, '12', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8251, '14', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8252, '16', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8253, '18', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8254, '20', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8255, '24', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8256, '30', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8257, '36', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8258, '42', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8259, '48', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8260, 'OTHER', 1301);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8302, 'ALUMINUM', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8303, 'BRONZE', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8304, 'CAST IRON', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8305, 'DUCTILE IRON', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8306, 'OTHER', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8307, 'TEFLON', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8308, 'PVC', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8309, 'STAINLESS', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8310, 'STEEL', 1270);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8313, 'PACKING', 1404);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8314, 'MECHANICAL', 1404);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8349, '3', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8350, '4', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8351, '5', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8352, '6', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8353, '7', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8354, '8', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8355, '9', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8356, '10', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8357, '11', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8358, '12', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8359, '13', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8360, '14', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8361, '15', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8362, '16', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8363, '1', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8364, '2', 1307);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8365, 'CENT VACUUM', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8366, 'DRUM', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8367, 'END SUCTION', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8368, 'NON-SUBMERGED TURBINE', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8369, 'SPLITCASE', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8370, 'SUBMERGED TURBINE', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8371, 'SUBMERSIBLE', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8372, 'XX', 1471);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8579, 'CW (FROM MOTOR FAN END)', 1018);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8580, 'CCW (FROM MOTOR FAN END)', 1018);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8611, '1200', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8612, '1800', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8613, '3600', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8614, 'VARIABLE', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8615, 'OTHER', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8616, '600', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8617, '600/1200', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8618, '900', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8619, '900/1800', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8620, '3500', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8621, '1725', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8622, '1550', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8623, 'XX', 1512);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10255, 'ALERT CUSTOMERS BEFORE', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10256, 'SPECIAL FLUSHING CONCERN', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10257, 'VALVE CLOSED/PLUGGED', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10258, 'OPEN SLOWLY', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10259, 'CLOSE SLOWLY', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10260, 'NO VEHICLE ACCESS', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10261, 'BACK-SIDE YARD', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10262, 'PUMPOUT REQUIRED', 1624);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10263, 'OTHER', 1624);
";


                #endregion

                #region PUMP GRINDER

                case "PUMP GRINDER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5583, 'CHEMICAL', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5584, 'DISTRIBUTION SYSTEM BOOSTER', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5585, 'DITCH/TRASH PUMPS', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5586, 'FUEL', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5587, 'GRINDER', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5588, 'IN PLANT BOOSTER', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5589, 'IN PLANT TRANSFER', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5590, 'PLANT TO DISTRIBUTION PUMP', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5591, 'RAW WATER (GROUND)', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5592, 'RAW WATER (SURFACE)', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5593, 'SAMPLE', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5594, 'SEWER LIFT', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5595, 'SLUDGE', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5596, 'SUMP', 1432);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6649, 'GPM', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6650, 'MGD', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6651, 'GPH', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6652, 'CFS', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6653, 'GPD', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6654, 'LB/HR', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6655, 'LB/DAY', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6656, 'OTHER', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6657, 'CFM', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6658, 'LPH', 1299);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7143, 'WATER', 993);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7144, 'OIL', 993);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7145, 'GREASE', 993);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7146, 'OTHER', 993);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7163, 'WATER', 917);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7164, 'OIL', 917);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7165, 'GREASE', 917);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7166, 'OTHER', 917);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7887, 'HORIZONTAL', 1138);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7888, 'VERTICAL', 1138);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7889, 'XX', 1138);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8115, 'ANTI FRICTION', 1072);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8116, 'SLEEVE', 1072);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8121, 'ANTI FRICTION', 1061);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8122, 'SLEEVE', 1061);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8125, '10', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8126, '12', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8127, '14', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8128, '16', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8129, '18', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8130, '20', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8131, '24', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8132, '30', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8133, '36', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8134, '42', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8135, '48', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8136, 'OTHER', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8137, '1', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8138, '1.5', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8139, '2', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8140, '2.5', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8141, '3', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8142, '4', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8143, '5', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8144, '6', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8145, '8', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8146, 'LESS THAN 1', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8147, '1.25', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8148, 'XX', 1255);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8197, 'STAINLESS', 1495);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8198, 'BRONZE', 1495);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8199, 'PHENOLIC', 1495);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8200, 'CERAMIC', 1495);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8201, 'CAST IRON', 1495);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8202, 'OTHER', 1495);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8261, 'LESS THAN 1', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8262, '1', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8263, '1.25', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8264, '1.5', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8265, '2', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8266, '2.5', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8267, '3', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8268, '4', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8269, '5', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8270, '6', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8271, '8', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8272, '10', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8273, '12', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8274, '14', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8275, '16', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8276, '18', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8277, '20', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8278, '24', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8279, '30', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8280, '36', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8281, '42', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8282, '48', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8283, 'OTHER', 1555);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8293, 'ALUMINUM', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8294, 'BRONZE', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8295, 'CAST IRON', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8296, 'DUCTILE IRON', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8297, 'OTHER', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8298, 'TEFLON', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8299, 'PVC', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8300, 'STAINLESS', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8301, 'STEEL', 1324);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8311, 'PACKING', 1295);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8312, 'MECHANICAL', 1295);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8330, '10', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8331, '11', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8332, '12', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8333, '13', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8334, '14', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8335, '15', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8336, '16', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8337, '1', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8338, '2', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8339, '3', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8340, '4', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8341, '5', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8342, '6', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8343, '7', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8344, '8', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8345, '9', 870);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8373, 'GRINDER END SUCTION', 1114);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8374, 'GRINDER SUBMERSIBLE', 1114);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8375, 'XX', 1114);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8581, 'CW (FROM MOTOR FAN END)', 933);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8582, 'CCW (FROM MOTOR FAN END)', 933);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8585, '1200', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8586, '1800', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8587, '3600', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8588, 'VARIABLE', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8589, 'OTHER', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8590, '600', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8591, '600/1200', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8592, '900', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8593, '900/1800', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8594, '3500', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8595, '1725', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8596, '1550', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8597, 'XX', 968);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10264, 'ALERT CUSTOMERS BEFORE', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10265, 'SPECIAL FLUSHING CONCERN', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10266, 'VALVE CLOSED/PLUGGED', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10267, 'OPEN SLOWLY', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10268, 'CLOSE SLOWLY', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10269, 'NO VEHICLE ACCESS', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10270, 'BACK-SIDE YARD', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10271, 'PUMPOUT REQUIRED', 1634);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10272, 'OTHER', 1634);
";

                #endregion

                #region PUMP POSITIVE DISPLACEMENT

                case "PUMP POSITIVE DISPLACEMENT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5597, 'CHEMICAL', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5598, 'DISTRIBUTION SYSTEM BOOSTER', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5599, 'DITCH/TRASH PUMPS', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5600, 'FUEL', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5601, 'GRINDER', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5602, 'IN PLANT BOOSTER', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5603, 'IN PLANT TRANSFER', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5604, 'PLANT TO DISTRIBUTION PUMP', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5605, 'RAW WATER (GROUND)', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5606, 'RAW WATER (SURFACE)', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5607, 'SAMPLE', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5608, 'SEWER LIFT', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5609, 'SLUDGE', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5610, 'SUMP', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5611, 'PROCESS EQUIPMENT', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5612, 'HVAC', 1547);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6639, 'GPM', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6640, 'MGD', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6641, 'GPH', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6642, 'CFS', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6643, 'GPD', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6644, 'LB/HR', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6645, 'LB/DAY', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6646, 'OTHER', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6647, 'CFM', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6648, 'LPH', 1587);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7147, 'WATER', 1346);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7148, 'OIL', 1346);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7149, 'GREASE', 1346);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7150, 'OTHER', 1346);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7159, 'WATER', 1368);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7160, 'OIL', 1368);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7161, 'GREASE', 1368);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7162, 'OTHER', 1368);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7896, 'HORIZONTAL', 1478);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7897, 'VERTICAL', 1478);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7898, 'XX', 1478);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8117, 'ANTI FRICTION', 1508);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8118, 'SLEEVE', 1508);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8119, 'ANTI FRICTION', 1409);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8120, 'SLEEVE', 1409);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8149, '10', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8150, '12', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8151, '14', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8152, '16', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8153, '18', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8154, '20', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8155, '24', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8156, '30', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8157, '36', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8158, '42', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8159, '48', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8160, 'OTHER', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8161, '1', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8162, '1.5', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8163, '2', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8164, '2.5', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8165, '3', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8166, '4', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8167, '5', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8168, '6', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8169, '8', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8170, 'LESS THAN 1', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8171, '1.25', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8172, 'XX', 1332);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8209, 'STAINLESS', 1224);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8210, 'BRONZE', 1224);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8211, 'PHENOLIC', 1224);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8212, 'CERAMIC', 1224);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8213, 'CAST IRON', 1224);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8214, 'OTHER', 1224);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8215, 'LESS THAN 1', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8216, '1', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8217, '1.25', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8218, '1.5', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8219, '2', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8220, '2.5', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8221, '3', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8222, '4', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8223, '5', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8224, '6', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8225, '8', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8226, '10', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8227, '12', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8228, '14', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8229, '16', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8230, '18', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8231, '20', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8232, '24', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8233, '30', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8234, '36', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8235, '42', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8236, '48', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8237, 'OTHER', 1321);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8284, 'ALUMINUM', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8285, 'BRONZE', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8286, 'CAST IRON', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8287, 'DUCTILE IRON', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8288, 'OTHER', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8289, 'TEFLON', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8290, 'PVC', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8291, 'STAINLESS', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8292, 'STEEL', 1438);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8315, 'PACKING', 1328);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8316, 'MECHANICAL', 1328);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8317, '3', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8318, '4', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8319, '5', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8320, '6', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8321, '7', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8322, '8', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8323, '9', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8324, '13', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8325, '14', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8326, '15', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8327, '16', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8328, '1', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8329, '2', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8346, '10', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8347, '11', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8348, '12', 1532);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8376, 'DIAPHRAGM', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8377, 'GEAR', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8378, 'LOBE', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8379, 'PD VACUUM', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8380, 'PERISTALTIC', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8381, 'PISTON', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8382, 'PROGRESSIVE CAVITY', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8383, 'SCREW', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8384, 'XX', 1411);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8583, 'CW (FROM MOTOR FAN END)', 1383);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8584, 'CCW (FROM MOTOR FAN END)', 1383);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8598, '1200', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8599, '1800', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8600, '3600', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8601, 'VARIABLE', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8602, 'OTHER', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8603, '600', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8604, '600/1200', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8605, '900', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8606, '900/1800', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8607, '3500', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8608, '1725', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8609, '1550', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8610, 'XX', 1405);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10273, 'ALERT CUSTOMERS BEFORE', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10274, 'SPECIAL FLUSHING CONCERN', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10275, 'VALVE CLOSED/PLUGGED', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10276, 'OPEN SLOWLY', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10277, 'CLOSE SLOWLY', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10278, 'NO VEHICLE ACCESS', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10279, 'BACK-SIDE YARD', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10280, 'PUMPOUT REQUIRED', 1645);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10281, 'OTHER', 1645);
";

                #endregion

                #region RECORDER

                case "RECORDER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6893, 'INCHES', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6894, 'LBS', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6895, 'KG', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6896, 'PPM', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6897, 'PPB', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6898, 'MOHMS', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6899, 'AMPS', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6900, 'VOLTS', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6901, 'KWH', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6902, 'RPM', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6903, 'HZ', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6904, 'OTHER', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6905, 'DEG F', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6906, 'DEG C', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6907, 'PSI', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6908, 'IN H20', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6909, 'FT H20', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6910, 'FEET', 1010);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7131, 'Y', 1549);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7132, 'N', 1549);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7727, '1,2 INDOOR', 1141);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7728, '3 RAINTIGHT', 1141);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7729, '4 SPLASHPROOF', 1141);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7730, '6 SHORTTERM SUBMERSIBLE', 1141);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7731, '7 EXPLOSION PROOF', 1141);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7732, '4X STAINLESS SPLASHPROOF', 1141);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7829, 'Y', 1038);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7830, 'N', 1038);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7917, '4-20MA', 1049);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7918, '1-5V', 1049);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7919, 'PULSE', 1049);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7920, 'CLOSED CONTACT', 1049);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7921, 'OTHER', 1049);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8525, 'DATALOGGER', 1271);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8526, 'ELECTRONIC CHART RECORDER', 1271);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8527, 'MECHANICAL CHART RECORDER', 1271);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8528, 'XX', 1271);
";

                #endregion

                #region RESPIRATOR

                case "RESPIRATOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6674, 'CHLORINE', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6675, 'AMMONIA', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6676, 'OXYGEN', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6677, 'HYDROGEN', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6678, 'HYDROGEN SULFIDE', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6679, 'SULFUR DIOXIDE', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6680, 'CARBON MONOXIDE', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6681, 'DUST', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6682, 'OTHER', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6683, 'MULTI EXP-H2S-O2', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6684, 'XX', 1417);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8422, 'A SUIT', 964);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8423, 'CARTRIDGE MASK', 964);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8424, 'SCBA', 964);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8425, 'XX', 964);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8533, 'NIOSH NFPA 1', 1443);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8534, 'NIOSH NFPA 2', 1443);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8535, 'NIOSH NFPA 3', 1443);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8536, 'NIOSH NFPA 4', 1443);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8537, 'NIOSH NFPA 5', 1443);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8538, 'OTHER', 1443);
";

                #endregion

                #region RTU - PLC

                case "RTU - PLC":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5636, 'SINGLE VARIABLE CONTROLLER', 891);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5637, 'FULL PROCESS CONTROLLER', 891);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5638, 'DATA COLLECTOR', 891);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5639, 'DATA TRANSMITTER ONLY', 891);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5640, 'REMOTE RACK', 891);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5641, 'OTHER', 891);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6029, 'NONLICENSE RADIO', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6030, 'LICENSE RADIO', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6031, 'NONE', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6032, 'FIBER OPTIC', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6033, 'LEASE LINE MODEM', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6034, 'CELLULAR', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6035, 'OTHER', 1506);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6047, 'OTHER', 976);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6048, 'RS485', 976);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6049, 'RS232', 976);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6050, 'ETHERNET', 976);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6051, 'NONE', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6052, 'NONLICENSE RADIO', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6053, 'LICENSE RADIO', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6054, 'FIBER OPTIC', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6055, 'OTHER', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6056, 'LEASE LINE MODEM', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6057, 'CELLULAR', 1379);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6069, 'OTHER', 1082);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6070, 'RS485', 1082);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6071, 'RS232', 1082);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6072, 'ETHERNET', 1082);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6073, 'NONLICENSE RADIO', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6074, 'LICENSE RADIO', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6075, 'OTHER', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6076, 'NONE', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6077, 'FIBER OPTIC', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6078, 'LEASE LINE MODEM', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6079, 'CELLULAR', 944);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6091, 'OTHER', 1558);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6092, 'RS485', 1558);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6093, 'RS232', 1558);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6094, 'ETHERNET', 1558);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6095, 'NONLICENSE RADIO', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6096, 'LICENSE RADIO', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6097, 'NONE', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6098, 'OTHER', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6099, 'CELLULAR', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6100, 'FIBER OPTIC', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6101, 'LEASE LINE MODEM', 1076);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6113, 'ETHERNET', 1521);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6114, 'OTHER', 1521);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6115, 'RS485', 1521);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6116, 'RS232', 1521);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6117, 'FIBER OPTIC', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6118, 'NONE', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6119, 'LEASE LINE MODEM', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6120, 'CELLULAR', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6121, 'LICENSE RADIO', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6122, 'NONLICENSE RADIO', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6123, 'OTHER', 977);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6135, 'ETHERNET', 1525);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6136, 'OTHER', 1525);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6137, 'RS485', 1525);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6138, 'RS232', 1525);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6139, 'NONE', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6140, 'LEASE LINE MODEM', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6141, 'CELLULAR', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6142, 'LICENSE RADIO', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6143, 'NONLICENSE RADIO', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6144, 'FIBER OPTIC', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6145, 'OTHER', 1524);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6157, 'ETHERNET', 956);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6158, 'OTHER', 956);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6159, 'RS485', 956);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6160, 'RS232', 956);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6482, 'Y', 1067);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6483, 'N', 1067);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8529, 'Y', 1242);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8530, 'N', 1242);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8650, 'RTU-PLC*', 988);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8651, 'XX', 988);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8807, 'NONE', 1118);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8808, 'GENERATOR', 1118);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8809, 'BATTERY', 1118);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8810, 'UPS', 1118);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8811, 'OTHER', 1118);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10098, '120VAC', 1489);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10099, '24VDC', 1489);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10282, 'ALERT CUSTOMERS BEFORE', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10283, 'SPECIAL FLUSHING CONCERN', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10284, 'VALVE CLOSED/PLUGGED', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10285, 'OPEN SLOWLY', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10286, 'CLOSE SLOWLY', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10287, 'NO VEHICLE ACCESS', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10288, 'BACK-SIDE YARD', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10289, 'PUMPOUT REQUIRED', 1648);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10290, 'OTHER', 1648);
";

                #endregion

                #region SAFETY SHOWER

                case "SAFETY SHOWER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8551, 'Y', 1450);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8552, 'N', 1450);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8655, 'FIXED SHWR', 1348);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8656, 'PORTABLE SHWR', 1348);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8657, 'XX', 1348);
";

                #endregion

                #region SCADA RADIO

                case "SCADA RADIO":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5184, 'YAGI', 1351);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5185, 'OMNI', 1351);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5186, 'OTHER', 1351);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5378, 'CONTROL AND DAQ', 1127);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5379, 'DAQ ONLY', 1127);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5380, 'CONTROL ONLY', 1127);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5381, 'SECURITY ONLY', 1127);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5382, 'NETWORK', 1127);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5383, 'ALARMING', 1127);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5802, '2400', 896);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5803, '9600', 896);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5804, '19200', 896);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5805, '56K', 896);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5806, 'OTHER', 896);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6168, 'COMM-RAD*', 1445);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6169, 'XX', 1445);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6197, 'NONE', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6198, 'LEASE LINE', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6199, 'CELLULAR', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6200, 'LICENSE RADIO', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6201, 'NONLICENSE RADIO', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6202, 'NETWORK', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6203, 'WIRELESS', 908);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8752, 'NONE', 928);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8753, 'GENERATOR', 928);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8754, 'BATTERY', 928);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8755, 'UPS', 928);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8756, 'OTHER', 928);
";

                #endregion

                #region SCADA SYSTEM GEN

                case "SCADA SYSTEM GEN":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8658, 'SCADASYS*', 1230);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8659, 'XX', 1230);
";

                #endregion

                #region SCALE

                case "SCALE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6875, 'DEG F', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6876, 'DEG C', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6877, 'PSI', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6878, 'IN H20', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6879, 'FT H20', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6880, 'FEET', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6881, 'INCHES', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6882, 'LBS', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6883, 'KG', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6884, 'PPM', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6885, 'PPB', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6886, 'MOHMS', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6887, 'AMPS', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6888, 'VOLTS', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6889, 'KWH', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6890, 'RPM', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6891, 'HZ', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6892, 'OTHER', 1106);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7133, 'Y', 1240);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7134, 'N', 1240);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7721, '1,2 INDOOR', 1433);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7722, '3 RAINTIGHT', 1433);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7723, '4 SPLASHPROOF', 1433);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7724, '6 SHORTTERM SUBMERSIBLE', 1433);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7725, '7 EXPLOSION PROOF', 1433);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7726, '4X STAINLESS SPLASHPROOF', 1433);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7833, 'Y', 995);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7834, 'N', 995);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7912, '4-20MA', 1585);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7913, '1-5V', 1585);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7914, 'PULSE', 1585);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7915, 'CLOSED CONTACT', 1585);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7916, 'OTHER', 1585);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8660, 'ELECTRONIC SCALE', 1530);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8661, 'MECHANICAL SCALE', 1530);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8662, 'XX', 1530);
";

                #endregion

                #region SCREEN

                case "SCREEN":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5647, 'WATER PROCESSING', 1527);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5648, 'WASTE PROCESSING', 1527);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5649, 'PLANT UTILITY', 1527);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5650, 'ENVIRONMENTAL COMPLIANCE', 1527);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5651, 'OTHER', 1527);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5652, 'WASTE WATER', 1527);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5768, 'Y', 878);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5769, 'N', 878);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7119, 'INDOORS', 897);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7120, 'OUTDOORS', 897);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8666, 'BAR', 1171);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8667, 'BASKET', 1171);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8668, 'ROTATING', 1171);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8669, 'XX', 1171);
";

                #endregion

                #region SCRUBBER

                case "SCRUBBER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5642, 'OTHER', 1269);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5643, 'SLUDGE', 1269);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5644, 'WASTEWATER', 1269);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5645, 'WATER', 1269);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5646, 'WASTE WATER', 1269);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7117, 'INDOORS', 1095);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7118, 'OUTDOORS', 1095);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7176, 'STEEL', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7177, 'STAINLESS STEEL', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7178, 'PLASTIC', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7179, 'FIBERGLASS', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7180, 'WOOD', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7181, 'CONCRETE', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7182, 'OTHER', 1022);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7250, 'ANTHRACITE', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7251, 'CAUSTIC', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7252, 'DRY MEDIA', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7253, 'GRANULATED ACTIVATED CARBON', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7254, 'INTEGRAL MEDIA SUPPORT', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7255, 'PLASTIC BLOCK SUPPORT', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7256, 'OTHER', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7257, 'CHOROSORB', 1058);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7311, 'ANTHRACITE', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7312, 'CAUSTIC', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7313, 'DRY MEDIA', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7314, 'GRANULATED ACTIVATED CARBON', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7315, 'INTREGRAL MEDIA SUPPORT', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7316, 'PLASTIC BLOCK SUPPORT', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7317, 'OTHER', 994);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7431, 'Y', 1254);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7432, 'N', 1254);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8663, 'DRY MEDIA', 1070);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8664, 'LIQUID MEDIA', 1070);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8665, 'XX', 1070);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10120, 'SURFACE SCOUR', 1305);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10121, 'AIR SCOUR', 1305);
";

                #endregion

                #region SECONDARY CONTAINMENT

                case "SECONDARY CONTAINMENT":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5407, 'HYDROFLOUROSILICIC ACID', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5408, 'HYDROGEN PEROXIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5409, 'LIME (HYDRATED)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5410, 'LIME (SOLID)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5411, 'LPG', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5412, 'MICROSAND', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5413, 'MISC CORROSION INHIB', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5414, 'MISC SEQUESTERANT', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5415, 'OTHER', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5416, 'OXYGEN', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5417, 'OZONE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5418, 'PHOSPHORIC ACID', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5419, 'POLYALUMINUM CHLORIDE (HIGH)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5420, 'POLYALUMINUM CHLORIDE (LOW)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5421, 'POLYALUMINUM CHLORIDE (MID)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5422, 'POLYALUMINUM SULFATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5423, 'POLYMER (FILTER AID)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5424, 'POLYMER ANIONIC', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5425, 'POLYMER CATIONIC', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5426, 'POLYMER NON-IONIC', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5427, 'POTASIUM PERMANGANATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5428, 'POWERED ACT CARBON', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5429, 'RESIN', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5430, 'SODA ASH', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5431, 'SODIUM ALUMINATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5432, 'SODIUM BICARBONATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5433, 'SODIUM BISULPHATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5434, 'SODIUM FLOURIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5435, 'SODIUM HYDROXIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5436, 'SODIUM HYPOCHLORITE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5437, 'SULFUR DIOXIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5438, 'SULFURIC ACID', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5439, 'WASH WATER', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5440, 'WASTE WATER', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5441, 'ZINC ORTHOPHOSPHATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5442, 'ALUM (ACIDIC)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5443, 'ALUMINUM CHLORHYDRATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5444, 'ALUMINUM SULPHATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5445, 'AMMONIA (ANHYD)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5446, 'BENTONITE CLAY', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5447, 'BRINE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5448, 'AMMONIA (AQUEOUS)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5449, 'AMMONIUM HYDROXIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5450, 'CARBON DIOXIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5451, 'CHLORAMINE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5452, 'CHLORINE (GAS)', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5453, 'CHLORINE DIOXIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5454, 'CITRIC ACID', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5455, 'DIESEL', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5456, 'FERRIC CHLORIDE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5457, 'FERRIC SULPHATE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5458, 'FUEL OIL', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5459, 'GASOLINE', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5460, 'HYDROCHLORIC ACID', 1326);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6227, 'CONTAIN-CONC', 1286);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6228, 'CONTAIN-METAL', 1286);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6229, 'CONTAIN-PL', 1286);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6230, 'XX', 1286);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7115, 'INDOORS', 1291);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7116, 'OUTDOORS', 1291);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9025, 'Y', 1406);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9026, 'N', 1406);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9027, 'XX', 1406);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9148, 'Y', 1097);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9149, 'N', 1097);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9150, 'XX', 1097);
";

                #endregion

                #region SECURITY SYSTEM

                case "SECURITY SYSTEM":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5152, 'SIREN', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5153, 'STROBE', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5154, 'SIREN/STROBE', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5155, 'AUTO SHUTDOWN', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5156, 'AUTO SHUT/VENTILATION', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5157, 'OTHER', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5158, 'LIGHT BUILDING', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5159, 'SCADA ALARM', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5160, 'XX', 940);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5776, 'Y', 1258);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5777, 'N', 1258);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8553, 'Y', 1217);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8554, 'N', 1217);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8670, 'BURGLAR ALARM', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8671, 'BURGLAR ALARM SCADA', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8672, 'CARD ACCESS', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8673, 'ELECTRONIC KEY', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8674, 'FENCING', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8675, 'SECURITY SOFTWARE', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8676, 'VIDEO', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8677, 'VIDEO SCADA', 1340);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8678, 'XX', 1340);
";

                #endregion

                #region SERVER

                case "SERVER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5653, 'HMI', 1581);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5654, 'DATA STORAGE', 1581);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5655, 'OTHER', 1581);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5656, 'HMI AND STORAGE', 1581);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6735, 'ICONICS GENESIS', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6736, 'INTELLUTION IFIX', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6737, 'INTELLUTION FIX32', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6738, 'WONDERWARE', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6739, 'TRANSDYNE', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6740, 'SURVALENT', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6741, 'OTHER', 1031);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7733, 'STANDALONE', 1574);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7734, 'SCADA DOMAIN', 1574);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7735, 'OTHER', 1574);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7859, 'WINDOWS 2000', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7860, 'WINDOWS XP', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7861, 'WINDOWS 7', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7862, 'WINDOWS SERVER 2003', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7863, 'WINDOWS SERVER 2008', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7864, 'WINDOWS SERVER 2012', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7865, 'OTHER', 882);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8501, 'NONE', 1322);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8502, '1', 1322);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8503, '2', 1322);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8504, '3', 1322);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8505, '4', 1322);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8506, '5', 1322);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8685, 'SERVR*', 1006);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8686, 'XX', 1006);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8802, 'NONE', 1189);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8803, 'GENERATOR', 1189);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8804, 'BATTERY', 1189);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8805, 'UPS', 1189);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8806, 'OTHER', 1189);
";

                #endregion

                #region SOFTENER

                case "SOFTENER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5741, 'WATER', 1209);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5742, 'WASTE WATER', 1209);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5743, 'SLUDGE', 1209);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5744, 'OTHER', 1209);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7111, 'INDOORS', 1188);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7112, 'OUTDOORS', 1188);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7204, 'STEEL', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7205, 'STAINLESS STEEL', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7206, 'PLASTIC', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7207, 'FIBERGLASS', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7208, 'WOOD', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7209, 'CONCRETE', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7210, 'OTHER', 867);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7289, 'ANTHRACITE', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7290, 'CAUSTIC', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7291, 'DRY MEDIA', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7292, 'GRANULATED ACTIVATED CARBON', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7293, 'INTEGRAL MEDIA SUPPORT', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7294, 'PLASTIC BLOCK SUPPORT', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7295, 'OTHER', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7296, 'CHOROSORB', 1515);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7346, 'ANTHRACITE', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7347, 'CAUSTIC', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7348, 'DRY MEDIA', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7349, 'GRANULATED ACTIVATED CARBON', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7350, 'INTEGRAL MEDIA SUPPORT', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7351, 'PLASTIC BLOCK SUPPORT', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7352, 'OTHER', 1228);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7429, 'Y', 1315);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7430, 'N', 1315);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9137, 'TRT-SOFT*', 1236);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9138, 'XX', 1236);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10116, 'SURFACE SCOUR', 946);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10117, 'AIR SCOUR', 946);
";

                #endregion

                #region STREET VALVE

                case "STREET VALVE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5117, 'VALVE BOX', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5118, 'MANHOLE', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5119, 'VAULT', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5120, 'STOP BOX', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5121, 'PIT BOX', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5122, 'BUILDING', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5123, 'OTHER', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5124, 'XX', 1162);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5161, 'MANUAL', 1501);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5162, 'ELECTRIC', 1501);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5163, 'HYDRAULIC', 1501);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5164, 'PNEUMATIC', 1501);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5657, 'BLOW-OFF', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5658, 'BOOSTER DISCHARGE', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5659, 'CROSSOVER', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5660, 'DIST DEAD END', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5661, 'DISTRIBUTION IN GRID', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5662, 'HYD AUX', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5663, 'OTHER', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5664, 'PLANT', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5665, 'PLANT DISCHARGE', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5666, 'RAW WATER', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5667, 'SERVICE LINE', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5668, 'TRANSMISSION', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5669, 'ZONE SEPARATION', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5670, 'PRESSURE REGULATION', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5671, 'BYPASS', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5672, 'FIRE SERVICE', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5673, 'PUMP DISCHARGE', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5674, 'XX', 921);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5847, 'Y', 1037);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5848, 'N', 1037);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5849, 'XX', 1037);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6369, '2', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6370, '4', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6371, '6', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6372, '8', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6373, '10', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6374, '12', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6375, '14', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6376, '16', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6377, '18', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6378, '20', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6379, '24', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6380, '30', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6381, '36', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6382, '42', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6383, '48', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6384, '60', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6385, '72', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6386, 'OTHER', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6387, '54', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6388, '2.25', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6389, '2.5', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6390, '3', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6391, '5', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6392, '15', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6393, '21', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6394, '27', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6395, '0.625', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6396, '0.75', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6397, '1', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6398, '1.25', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6399, '1.5', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6400, '66', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6401, '0.25', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6402, '0.5', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6403, 'XX', 1381);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6696, 'WORM', 1040);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6697, 'SPUR', 1040);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6698, 'BEVEL', 1040);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6699, 'NON RISING STEM', 1040);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6700, 'RISING STEM', 1040);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7023, 'MJ', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7024, 'RESTRAINED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7025, 'LEAD', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7026, 'FLANGED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7027, 'BALL & SOCKET', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7028, 'OTHER', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7029, 'THREADED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7030, 'SOLDERED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7031, 'FUSED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7032, 'LEADITE', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7033, 'WELDED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7034, 'GLUED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7035, 'PUSH ON', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7036, 'GLUED/CEMENTED', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7037, 'COMPRESSION', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7038, 'FLARE', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7039, 'XX', 1285);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7799, 'OPEN', 1146);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7800, 'CLOSED', 1146);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7801, 'THROTTLING', 1146);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7802, 'XX', 1146);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7827, 'Y', 1047);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7828, 'N', 1047);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7841, 'LEFT', 876);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7842, 'RIGHT', 876);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7843, 'XX', 876);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7847, 'SQUARE', 1459);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7848, 'TEE', 1459);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7849, 'WHEEL', 1459);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7850, 'OTHER', 1459);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7851, 'XX', 1459);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7852, 'PENTAGON', 1459);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8047, 'PVC', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8048, 'HDPE', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8049, 'RCP', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8050, 'PCCP', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8051, 'AC', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8052, 'GALV', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8053, 'CU', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8054, 'ST', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8055, 'DI', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8056, 'OTH', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8057, 'BR', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8058, 'CIU', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8059, 'CIL', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8060, 'PE', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8061, 'LD', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8062, 'WI', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8063, 'UNK', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8064, 'CI', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8065, 'ABS', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8066, 'VCP', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8067, 'BFP', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8068, 'XX', 1460);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8435, '150', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8443, '200', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8444, '250', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8445, '300', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8446, '350', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8447, '175', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8455, 'XX', 886);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8713, 'ALERT CUSTOMERS BEFORE', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8714, 'SPECIAL FLUSHING CONCERN', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8715, 'VALVE CLOSED/PLUGGED', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8716, 'OPEN SLOWLY', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8717, 'CLOSE SLOWLY', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8718, 'NO VEHICLE ACCESS', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8719, 'BACK-SIDE YARD', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8720, 'PUMPOUT REQUIRED', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8721, 'OTHER', 904);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8856, 'CONCRETE', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8857, 'ASPHALT', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8858, 'GRASS', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8859, 'LANDSCAPING', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8860, 'GRAVEL', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8861, 'SOIL', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8862, 'BRICK', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8863, 'OTHER', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8864, 'UNIMPROVED', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8865, 'XX', 930);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8866, 'STREET', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8867, 'DRIVEWAY', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8868, 'PARKING', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8869, 'SIDEWALK', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8870, 'OTHER', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8871, 'UNPAVED', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8872, 'XX', 1102);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8901, 'DIST (A)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8902, 'DIST (M)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8903, 'HYD AUX (M)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8904, 'SERV (A)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8905, 'SERV (M)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8906, 'TRANS (A)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8907, 'TRANS (M)', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8908, 'XX', 1349);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9198, '2', 1003);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9199, '1.5', 1003);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9200, 'OTHER', 1003);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9201, 'XX', 1003);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9206, 'RESILIENT', 1136);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9207, 'STAINLESS STEEL', 1136);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9208, 'BRONZE', 1136);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9209, 'CAST IRON', 1136);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9210, 'OTHER', 1136);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9216, 'Y', 1476);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9217, 'N', 1476);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9256, '0.625', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9257, '0.75', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9258, '1', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9259, '1.25', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9260, '1.5', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9261, '2.25', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9262, '2.5', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9263, '3', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9270, '2', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9271, '4', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9272, '6', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9273, '8', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9274, '10', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9275, '12', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9276, '14', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9277, '16', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9278, '18', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9279, '20', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9280, '24', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9281, '30', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9282, '36', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9283, '42', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9284, '48', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9285, '60', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9286, '72', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9287, 'OTHER', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9288, '40', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9289, '54', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9318, 'LESS THAN 1', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9319, '0.25', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9320, '0.5', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9321, '66', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9322, '5', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9323, 'XX', 1180);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9333, 'AIR RELIEF', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9334, 'AIR/VAC RELIEF', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9335, 'ANGLE', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9336, 'BALL', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9337, 'BUTTERFLY', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9338, 'CHECK', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9339, 'CHECK/FOOT', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9340, 'CURB STOP', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9341, 'DOUBLE CHECK', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9342, 'GATE', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9343, 'GLOBE', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9344, 'OTHER', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9345, 'PLUG', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9346, 'PRES REGULATOR', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9347, 'PRES RELIEF', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9348, 'RPZ', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9349, 'SOLENOID', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9350, 'TAPPING', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9351, 'TELESCOPIC', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9352, 'VAC REGULATOR', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9353, 'VAC RELIEF', 1577);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9354, 'XX', 1577);
";

                #endregion

                #region TELEPHONE

                case "TELEPHONE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5187, 'YAGI', 1375);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5188, 'OMNI', 1375);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5189, 'OTHER', 1375);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5396, 'CONTROL AND DAQ', 1273);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5397, 'DAQ ONLY', 1273);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5398, 'CONTROL ONLY', 1273);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5399, 'SECURITY ONLY', 1273);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5400, 'NETWORK', 1273);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5401, 'ALARMING', 1273);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5812, '2400', 1517);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5813, '9600', 1517);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5814, '19200', 1517);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5815, '56K', 1517);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5816, 'OTHER', 1517);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6174, 'COMM-TEL*', 1013);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6175, 'XX', 1013);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6176, 'NONE', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6177, 'LEASE LINE', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6178, 'CELLULAR', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6179, 'LICENSE RADIO', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6180, 'NONLICENSE RADIO', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6181, 'NETWORK', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6182, 'WIRELESS', 1164);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8747, 'NONE', 1244);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8748, 'GENERATOR', 1244);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8749, 'BATTERY', 1244);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8750, 'UPS', 1244);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8751, 'OTHER', 1244);
";

                #endregion

                #region TOOL

                case "TOOL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5887, 'CFM', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5888, 'GPM', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5889, 'LB H', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5890, 'OTHER', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5891, 'LB', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5892, 'KG', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5893, 'PSI', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5894, 'IN H2O', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5895, 'FT H2O', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5896, 'PPM', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5897, 'MOHMS', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5898, 'AMPS', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5899, 'VOLTS', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5900, 'KWH', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5901, 'RPM', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5902, 'HZ', 1005);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8569, 'Y', 1229);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8570, 'N', 1229);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9060, 'HANDTOOL', 999);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9061, 'WELDER', 999);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9062, 'XX', 999);
";

                #endregion

                #region TRANSFORMER

                case "TRANSFORMER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5749, 'MAIN FEED', 1314);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5750, 'SUBFEED', 1314);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5751, 'CONTROLS', 1314);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5752, 'OTHER', 1314);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5753, 'PT/CT', 1314);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6982, 'A', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6983, 'E', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6984, 'B', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6985, 'F', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6986, 'H', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6987, 'N', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6988, 'OTHER', 945);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7637, 'PAD/FLOOR', 889);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7638, 'POLE', 889);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7639, 'WALL', 889);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7640, 'MCC', 889);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7641, 'OTHER', 889);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7703, '1,2 INDOOR', 1241);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7704, '3 RAINTIGHT', 1241);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7705, '4 SPLASHPROOF', 1241);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7706, '6 SHORTTERM SUBMERSIBLE', 1241);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7707, '7 EXPLOSION PROOF', 1241);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7708, '4X STAINLESS SPLASHPROOF', 1241);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8021, '3', 966);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8022, '1', 966);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9832, '6V', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9833, '12V', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9834, '24V', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9835, '120', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9836, '120/208', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9837, '120/240', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9838, '208Y/120', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9839, '208', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9840, '230', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9841, '230/460', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9842, '240', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9843, '277', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9844, '277/480', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9845, '460', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9846, '480', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9847, '600', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9848, '2300', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9849, '2300/4160', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9850, '2400', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9851, '4160', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9852, '13KV', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9853, '25KV', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9854, '33KV', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9855, 'OTHER', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9856, 'LESS THEN 100', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9857, '90VDC OR LESS', 1190);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10100, '120', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10101, '120/240', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10102, '208Y/120', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10103, '208', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10104, '240', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10105, '277/480', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10106, '480', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10107, '2400', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10108, '4160', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10109, 'OTHER', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10110, 'DC', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10111, '13KV', 1170);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10185, 'Y', 1104);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10186, 'N', 1104);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10187, 'PASSIVE AIR COOLED(DRY)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10188, 'PASSIVE AIR COOLED(OIL FILLED)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10189, 'VOLTAGE TRANSFORMER(PT)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10190, 'XX', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10191, 'CURRENT TRANSFORMER(CT)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10192, 'FORCED AIR COOLED(DRY)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10193, 'FORCED AIR COOLED(OIL FILLED)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10194, 'NOT AW FORCED AIR(DRY)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10195, 'NOT AW FORCED AIR(OIL FILLED)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10196, 'NOT AW PASSIVE AIR(DRY)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10197, 'NOT AW PASSIVE AIR(OIL FILLED)', 871);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10198, 'WYE', 1499);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10199, 'DELTA', 1499);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10200, 'OTHER', 1499);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10201, 'WYE', 1426);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10202, 'DELTA', 1426);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10203, 'OTHER', 1426);
";

                #endregion

                #region TRANSMITTER

                case "TRANSMITTER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6947, 'INCHES', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6948, 'LBS', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6949, 'KG', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6950, 'PPM', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6951, 'PPB', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6952, 'MOHMS', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6953, 'AMPS', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6954, 'VOLTS', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6955, 'KWH', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6956, 'RPM', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6957, 'HZ', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6958, 'OTHER', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6959, 'DEG F', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6960, 'DEG C', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6961, 'PSI', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6962, 'IN H20', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6963, 'FT H20', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6964, 'FEET', 1386);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7137, 'Y', 1543);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7138, 'N', 1543);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7691, '1,2 INDOOR', 1494);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7692, '3 RAINTIGHT', 1494);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7693, '4 SPLASHPROOF', 1494);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7694, '6 SHORTTERM SUBMERSIBLE', 1494);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7695, '7 EXPLOSION PROOF', 1494);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7696, '4X STAINLESS SPLASHPROOF', 1494);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7819, 'Y', 918);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7820, 'N', 918);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7932, '4-20MA', 1414);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7933, '1-5V', 1414);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7934, 'PULSE', 1414);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7935, 'CLOSED CONTACT', 1414);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7936, 'OTHER', 1414);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10204, 'CURRENT TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10205, 'DENSITY TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10206, 'DEWPOINT TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10207, 'DIFF PRESSURE TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10208, 'FREQUENCY TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10209, 'LEVEL TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10210, 'OTHER TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10211, 'POWER TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10212, 'PRESSURE TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10213, 'SPEED TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10214, 'TEMPERATURE TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10215, 'VOLTAGE TX', 1353);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10216, 'XX', 1353);
";

                #endregion

                #region UNINTERUPTED POWER SUPPLY

                case "UNINTERUPTED POWER SUPPLY":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9166, 'UPS*', 1329);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9167, 'XX', 1329);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9728, '6V', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9729, '12V', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9730, '24V', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9731, '120', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9732, '120/208', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9733, '120/240', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9734, '208Y/120', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9735, '208', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9736, '230', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9737, '230/460', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9738, '240', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9739, '277', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9740, '277/480', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9741, '460', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9742, '480', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9743, '600', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9744, '2300', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9745, '2300/4160', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9746, '2400', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9747, '4160', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9748, '13KV', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9749, '25KV', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9750, '33KV', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9751, 'OTHER', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9752, 'LESS THEN 100', 906);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9753, '90VDC OR LESS', 906);
";

                #endregion

                #region UV SANITIZER

                case "UV SANITIZER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9142, 'SELFCLEANING UV', 1303);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9143, 'STANDARD UV', 1303);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9144, 'XX', 1303);
";

                #endregion

                #region UV-SOUND

                case "UV-SOUND":
                    return @"

";

                #endregion

                #region VEHICLE

                case "VEHICLE":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5869, 'LB', 1044);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5870, 'KG', 1044);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8571, 'Y', 1542);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8572, 'N', 1542);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9168, 'CAR', 883);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9169, 'CART', 883);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9170, 'FORKTRUCK', 883);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9171, 'TRUCK', 883);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9172, 'BACKHOE', 883);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9173, 'TRAILER', 883);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9174, 'XX', 883);
";

                #endregion

                #region VOC STRIPPER

                case "VOC STRIPPER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5745, 'WATER', 1413);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5746, 'WASTE WATER', 1413);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5747, 'SLUDGE', 1413);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5748, 'OTHER', 1413);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7109, 'INDOORS', 1202);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7110, 'OUTDOORS', 1202);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7211, 'STEEL', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7212, 'STAINLESS STEEL', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7213, 'PLASTIC', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7214, 'FIBERGLASS', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7215, 'WOOD', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7216, 'CONCRETE', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7217, 'OTHER', 1142);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7297, 'ANTHRACITE', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7298, 'CAUSTIC', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7299, 'DRY MEDIA', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7300, 'GRANULATED ACTIVATED CARBON', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7301, 'INTEGRAL MEDIA SUPPORT', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7302, 'PLASTIC BLOCK SUPPORT', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7303, 'OTHER', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7304, 'CHOROSORB', 1055);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7353, 'ANTHRACITE', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7354, 'CAUSTIC', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7355, 'DRY MEDIA', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7356, 'GRANULATED ACTIVATED CARBON', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7357, 'INTEGRAL MEDIA SUPPORT', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7358, 'PLASTIC BLOCK SUPPORT', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7359, 'OTHER', 1140);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7427, 'Y', 1032);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7428, 'N', 1032);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9139, 'PACKED AIR TOWER', 1318);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9140, 'TRAY TOWER', 1318);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9141, 'XX', 1318);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10118, 'SURFACE SCOUR', 1325);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10119, 'AIR SCOUR', 1325);
";

                #endregion

                #region WASTE TANK

                case "WASTE TANK":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5731, 'WASTE WATER', 1497);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5732, 'SLUDGE', 1497);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7103, 'INDOORS', 1216);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7104, 'OUTDOORS', 1216);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8929, 'Y', 1115);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8930, 'N', 1115);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8931, 'XX', 1115);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8932, 'STEEL', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8933, 'STAINLESS STEEL', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8934, 'PLASTIC', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8935, 'FIBERGLASS', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8936, 'OTHER', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8937, 'CONCRETE', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8938, 'XX', 1116);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9014, 'VACUUM', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9015, 'ATMOSPHERIC', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9016, 'LESS THAN 100PSIG', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9017, 'LESS THAN 2PSIG', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9018, 'GREATER THAN 100PSIG', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9019, 'LESS THAN 15PSIG', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9020, '15-100PSIG', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9021, 'XX', 1080);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9022, 'Y', 1357);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9023, 'N', 1357);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9024, 'XX', 1357);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9057, 'NON-PRESSURIZED WSTE', 1308);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9058, 'PRESSURIZED/BLADDER WSTE', 1308);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9059, 'XX', 1308);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9145, 'Y', 1175);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9146, 'N', 1175);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9147, 'XX', 1175);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10339, 'ALERT CUSTOMERS BEFORE', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10340, 'SPECIAL FLUSHING CONCERN', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10341, 'VALVE CLOSED/PLUGGED', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10342, 'OPEN SLOWLY', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10343, 'CLOSE SLOWLY', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10344, 'NO VEHICLE ACCESS', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10345, 'BACK-SIDE YARD', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10346, 'PUMPOUT REQUIRED', 1678);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10347, 'OTHER', 1678);
";

                #endregion

                #region WATER HEATER

                case "WATER HEATER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5529, 'PROCESSING', 1468);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5530, 'COMFORT', 1468);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5531, 'FREEZE PROTECTION', 1468);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5532, 'VENTILATION', 1468);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5533, 'OTHER', 1468);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6291, 'CONTINUOUS', 1355);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6292, 'INTERMITTENT', 1355);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6484, 'ELECTRIC', 1129);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6485, 'LIQ FUEL', 1129);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6486, 'NAT GAS', 1129);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6487, 'OTHER', 1129);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6774, 'ELEC FIRED', 1310);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6775, 'GAS FIRED', 1310);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6776, 'OIL FIRED', 1310);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6777, 'XX', 1310);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7984, 'BTU/HR', 1292);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7985, 'TONS', 1292);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7986, 'CFM', 1292);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7987, 'OTHER', 1292);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7988, 'WATTS', 1292);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7989, 'KW', 1292);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9621, '2300', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9622, '2300/4160', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9623, '2400', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9624, '4160', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9625, '13KV', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9626, '25KV', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9627, '33KV', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9628, 'OTHER', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9629, 'LESS THEN 100', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9630, '90VDC OR LESS', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9647, '6V', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9648, '12V', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9649, '24V', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9650, '120', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9651, '120/208', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9652, '120/240', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9653, '208Y/120', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9654, '208', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9655, '230', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9656, '230/460', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9657, '240', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9658, '277', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9659, '277/480', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9660, '460', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9661, '480', 1336);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9662, '600', 1336);
";

                #endregion

                #region WATER QUALITY ANALYZER

                case "WATER QUALITY ANALYZER":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6014, 'PROFIBUS', 1274);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6015, 'HART', 1274);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6016, 'FOXCOM', 1274);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6017, 'MODBUS', 1274);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (6018, 'RS232', 1274);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7135, 'Y', 1470);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7136, 'N', 1470);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7697, '1,2 INDOOR', 938);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7698, '3 RAINTIGHT', 938);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7699, '4 SPLASHPROOF', 938);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7700, '6 SHORTTERM SUBMERSIBLE', 938);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7701, '7 EXPLOSION PROOF', 938);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7702, '4X STAINLESS SPLASHPROOF', 938);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7817, 'Y', 1382);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7818, 'N', 1382);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7937, '4-20MA', 979);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7938, '1-5V', 979);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7939, 'PULSE', 979);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7940, 'CLOSED CONTACT', 979);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7941, 'OTHER', 979);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8792, 'NONE', 1120);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8793, 'GENERATOR', 1120);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8794, 'BATTERY', 1120);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8795, 'UPS', 1120);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (8796, 'OTHER', 1120);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9063, 'Y', 1155);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9064, 'N', 1155);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9754, '2300', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9755, '2300/4160', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9756, '2400', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9757, '4160', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9758, '13KV', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9759, '25KV', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9760, '33KV', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9761, 'OTHER', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9762, 'LESS THEN 100', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9763, '90VDC OR LESS', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9764, '6V', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9765, '12V', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9766, '24V', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9767, '120', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9768, '120/208', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9769, '120/240', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9770, '208Y/120', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9771, '208', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9772, '230', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9773, '230/460', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9774, '240', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9775, '277', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9776, '277/480', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9777, '460', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9778, '480', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9779, '600', 1023);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10150, 'Y', 1020);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10151, 'N', 1020);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10152, 'NH3 ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10153, 'NO3 ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10154, 'O3 ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10155, 'ORP ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10156, 'OTHER ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10157, 'PARTICLE COUNTER ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10158, 'PARTICLE COUNTER ANLYZR(L)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10159, 'PH ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10160, 'PH ANLYZR(H)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10161, 'PH ANLYZR(L)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10162, 'PO4 ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10163, 'SCD ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10164, 'SPECTROPHOTOMETER(L)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10165, 'TOC ANLYZR (C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10166, 'TUBIDITY ANLYZR (HI)(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10167, 'TURBIDITY ANLYZR (LO)(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10168, 'TURBIDITY ANLYZR (LO)(L)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10169, 'UV254 ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10170, 'XX', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10171, 'CHLORAMINE ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10172, 'CL2 AMP/PH/CONDUCT ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10173, 'CL2 AMPEROMETRIC ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10174, 'CL2 AMPEROMETRIC ANLYZR(L)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10175, 'CL2 AMPEROMETRIC/PH ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10176, 'CL2 COLOR/PH/COND ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10177, 'CL2 COLORIMETRIC ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10178, 'CL2 COLORIMETRIC ANLYZR(H)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10179, 'CL2 COLORMETRIC/PH ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10180, 'CONDUCT ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10181, 'CONDUCT ANLYZR(L)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10182, 'DO ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10183, 'FLUORIDE ANLYZR(C)', 1125);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10184, 'FLUORIDE ANLYZR(L)', 1125);
";

                #endregion

                #region WATER TREATMENT CONTACTOR

                case "WATER TREATMENT CONTACTOR":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5733, 'WATER', 1458);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5734, 'WASTE WATER', 1458);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5735, 'SLUDGE', 1458);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (5736, 'OTHER', 1458);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7107, 'INDOORS', 1128);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7108, 'OUTDOORS', 1128);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7190, 'STEEL', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7191, 'STAINLESS STEEL', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7192, 'PLASTIC', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7193, 'FIBERGLASS', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7194, 'WOOD', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7195, 'CONCRETE', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7196, 'OTHER', 1347);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7258, 'ANTHRACITE', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7259, 'CAUSTIC', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7260, 'DRY MEDIA', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7261, 'GRANULATED ACTIVATED CARBON', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7262, 'INTEGRAL MEDIA SUPPORT', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7263, 'PLASTIC BLOCK SUPPORT', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7264, 'OTHER', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7265, 'CHOROSORB', 949);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7318, 'ANTHRACITE', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7319, 'CAUSTIC', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7320, 'DRY MEDIA', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7321, 'GRANULATED ACTIVATED CARBON', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7322, 'INTEGRAL MEDIA SUPPORT', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7323, 'PLASTIC BLOCK SUPPORT', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7324, 'OTHER', 937);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7425, 'Y', 1212);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (7426, 'N', 1212);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9126, 'TRT-CONT*', 1041);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (9127, 'XX', 1041);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10112, 'SURFACE SCOUR', 1252);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10113, 'AIR SCOUR', 1252);
";

                #endregion

                #region WELL

                case "WELL":
                    return @"
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10143, 'HORIZONTAL BORE', 1033);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10144, 'VERTICAL BORE', 1033);
INSERT INTO EquipmentCharacteristicDropDownValues (Id, Value, FieldId) VALUES (10145, 'XX', 1033);
";

                #endregion

                default:
                    throw new InvalidOperationException(
                        $"{typeof(EquipmentCharacteristicDropDownValues).Name} for EquipmentType '{equipmentType}' have not been scripted.");
            }
        }
    }
}
