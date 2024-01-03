using System;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.SampleValues
{
    public static class EquipmentModels
    {
        public static string GetInsertQuery(string equipmentType)
        {
            switch (equipmentType)
            {
                case "EMERGENCY GENERATOR":
                    return @"
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (1, '08VF157522', 2388);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (3, '154-2559', 2382);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (4, '3029TF150', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (5, '3029TF270', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (6, '3029TFT150', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (7, '3306B-DI', 2382);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (8, '3408B-SI', 2382);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (9, '4024HF285', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (10, '4039TF004', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (11, '4045HF285', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (12, '4045HF285H', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (13, '4045TF150', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (14, '4045TF250', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (15, '4045TF270E', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (17, '6061-A  LC', 2388);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (18, '6068HF285', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (19, '6068HF485T', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (20, '6068TF250', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (21, '6076AF010', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (22, '6076AF011', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (23, '6076TF010', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (24, '6081AF001', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (25, '6081TF001', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (26, '7084-7002', 2388);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (27, '7124-7002', 2388);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (28, '7125-7001', 2388);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (31, '91637305', 2388);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (32, 'C15', 2382);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (33, 'CH740EP', 2400);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (37, 'G743', 2386);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (38, 'G855', 2386);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (39, 'QSL9-G8', 2386);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (42, 'VTA-28-G7', 2386);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (81, 'DCA-15OU5J2', 2398);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (82, 'SR-4', 2382);";
                case "RTU - PLC":
                    return @"
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (84, '3305', 3414);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (85, 'CompactLogix L23', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (86, 'CompactLogix L24', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (87, 'CompactLogix L35', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (88, 'CompactLogix L36', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (89, 'CompactLogix L33', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (90, 'CompactLogix L32', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (91, 'CompactLogix L31', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (92, 'CompactLogix L30', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (93, 'CompactLogix L63', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (94, 'CompactLogix L43', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (95, 'CompactLogix L27', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (96, '3330', 3414);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (97, 'ControlWave', 3414);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (98, 'ControlWave Micro', 3414);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (99, 'Micrologix', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (100, 'SLC500', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (101, 'CompactLogix', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (102, 'M800', 3418);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (103, 'M110', 3418);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (104, 'PLC-5', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (106, 'ControlLogix', 3411);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (107, 'DirectLOGIC/Koyo', 3412);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (108, '3335', 3414);
INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (109, '3331', 3414);";
                case "TRANSMITTER":
                    return
                        "INSERT INTO EquipmentModels (EquipmentModelId, Description, EquipmentManufacturerId) VALUES (83, 'IGP20-T22D11F-M1L1', 3827);";
                default:
                    return null;
            }
        }
    }
}