﻿namespace MapCallImporter.SampleValues
{
    public static class EquipmentTypes
    {
        public static string GetInsertQuery(string equipmentType)
        {
            return @"
            INSERT INTO EquipmentGroups (Code, Description) VALUES ('A', 'Accounting');
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (121, 'ADJSPD', 'ADJUSTABLE SPEED DRIVE', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (122, 'AED', 'DEFIBRILLATOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (123, 'BATT', 'BATTERY', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (124, 'BATTCHGR', 'BATTERY CHARGER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (125, 'BLWR', 'BLOWER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (126, 'BOILER', 'BOILER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (127, 'BURNER', 'BURNER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (128, 'CALIB', 'CALIBRATION DEVICE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (129, 'CATHODIC', 'CATHODIC PROTECTION', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (130, 'CHEM-GEN', 'CHEMICAL GENERATORS', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (131, 'CHEM-PIP', 'CHEMICAL PIPING', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (132, 'CHMF-DRY', 'CHEMICAL DRY FEEDER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (133, 'CHMF-GAS', 'CHEMICAL GAS FEEDER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (134, 'CHMF-LIQ', 'CHEMICAL LIQUID FEEDER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (135, 'CNTRLPNL', 'CONTROL PANEL', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (136, 'CNTRLR', 'CONTROLLER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (137, 'CO', 'CLEAN OUT', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (138, 'COLLSYS', 'COLLECTION SYSTEM GENERAL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (139, 'COMM-FWL', 'FIREWALL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (140, 'COMM-MOD', 'MODEM', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (141, 'COMM-RAD', 'SCADA RADIO', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (142, 'COMM-RTR', 'NETWORK ROUTER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (143, 'COMM-SW', 'NETWORK SWITCH', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (144, 'COMM-TEL', 'TELEPHONE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (145, 'COMP', 'AIR COMPRESSOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (146, 'CONTACTR', 'MOTOR CONTACTOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (147, 'CONTAIN', 'SECONDARY CONTAINMENT', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (148, 'CONVEYOR', 'CONVEYOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (149, 'DAM', 'DAM', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (150, 'DISTSYS', 'DISTRIBUTION SYSTEM', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (151, 'DISTTOOL', 'DISTRIBUTION TOOL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (152, 'ELEVATOR', 'ELEVATOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (153, 'ELIGHT', 'EMERGENCY LIGHT', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (154, 'ENG', 'ENGINE', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (155, 'EYEWASH', 'EYEWASH', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (156, 'FACILITY', 'FACILITY AND GROUNDS', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (157, 'FIRE-AL', 'FIRE ALARM', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (158, 'FIRE-EX', 'FIRE EXTINGUISHER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (159, 'FIRE-SUP', 'FIRE SUPPRESSION', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (160, 'FLO-MET', 'FLOW METER (NON PREMISE)', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (161, 'FLO-WEIR', 'FLOW WEIR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (162, 'GEARBOX', 'GEARBOX', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (163, 'GEN', 'EMERGENCY GENERATOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (164, 'GMAIN', 'GRAVITY SEWER MAIN', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (165, 'GRINDER', 'GRINDER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (166, 'HOIST', 'HOIST', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (167, 'HVAC-CHL', 'HVAC CHILLER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (168, 'HVAC-CMB', 'HVAC COMBINATION UNIT', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (169, 'HVAC-DHM', 'HVAC DEHUMIDIFIER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (170, 'HVAC-EXC', 'HEAT EXCHANGER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (171, 'HVAC-HTR', 'HVAC HEATER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (172, 'HVAC-TWR', 'COOLING TOWER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (173, 'HVAC-VNT', 'HVAC VENTILATOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (174, 'HVAC-WH', 'WATER HEATER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (175, 'HYD', 'HYDRANT', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (176, 'INDICATR', 'INDICATOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (177, 'INST-SW', 'INSTRUMENT SWITCH', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (178, 'KIT', 'KIT (SAFETY, REPAIR, HAZWOPR)', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (179, 'LABEQ', 'LAB EQUIPMENT', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (180, 'LK-MON', 'LEAK MONITOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (181, 'MH', 'MANHOLE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (182, 'MIXR', 'MIXER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (183, 'MOT', 'MOTOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (184, 'MOTSTR', 'MOTOR STARTER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (185, 'NARUC_EQ', 'AM WATER NARUC ACCOUNT', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (186, 'OIT', 'OPERATOR COMPUTER TERMINAL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (187, 'PC', 'PC', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (188, 'PDMTOOL', 'PDM TOOL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (189, 'PHASECON', 'PHASE CONVERTER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (190, 'PMP-CENT', 'PUMP CENTRIFUGAL', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (191, 'PMP-GRND', 'PUMP GRINDER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (192, 'PMP-PD', 'PUMP POSITIVE DISPLACEMENT', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (193, 'PPE-ARC', 'ARC FLASH PROTECTION', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (194, 'PPE-FALL', 'FALL PROTECTION', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (195, 'PPE-FLOT', 'FLOATATION DEVICE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (196, 'PPE-RESP', 'RESPIRATOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (197, 'PRESDMP', 'PRESSURE DAMPER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (198, 'PRNTR', 'PRINTER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (199, 'PVLV', 'PLANT VALVE', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (200, 'PWRBRKR', 'POWER BREAKER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (201, 'PWRCOND', 'POWER CONDITIONER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (202, 'PWRDISC', 'POWER DISCONNECT', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (203, 'PWRFEEDR', 'POWER FEEDER CABLE', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (204, 'PWRMON', 'POWER MONITOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (205, 'PWRPNL', 'POWER PANEL', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (206, 'PWRRELAY', 'POWER RELAY', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (207, 'PWRSURG', 'POWER SURGE PROTECTION', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (208, 'RECORDER', 'RECORDER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (209, 'RTU-PLC', 'RTU - PLC', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (210, 'SAFGASDT', 'GAS DETECTOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (211, 'SAF-SHWR', 'SAFETY SHOWER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (212, 'SCADASYS', 'SCADA SYSTEM GEN', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (213, 'SCALE', 'SCALE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (214, 'SCRBBR', 'SCRUBBER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (215, 'SCREEN', 'SCREEN', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (216, 'SECSYS', 'SECURITY SYSTEM', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (217, 'SERVR', 'SERVER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (218, 'SVLV', 'STREET VALVE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (219, 'SVLV-BO', 'BLOW OFF VALVE', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (220, 'TNK-CHEM', 'CHEMICAL TANK', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (221, 'TNK-FUEL', 'FUEL TANK', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (222, 'TNK-PVAC', 'AIR/ VACUUM TANK', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (223, 'TNK-WNON', 'NON POTABLE WATER TANK', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (224, 'TNK-WPOT', 'POTABLE WATER TANK', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (225, 'TNK-WSTE', 'WASTE TANK', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (226, 'TOOL', 'TOOL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (227, 'TRAN-SW', 'POWER TRANSFER SWITCH', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (228, 'TRT-AER', 'AERATOR', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (229, 'TRT-CLAR', 'CLARIFIER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (230, 'TRT-CONT', 'WATER TREATMENT CONTACTOR', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (231, 'TRT-FILT', 'FILTER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (232, 'TRT-SOFT', 'SOFTENER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (233, 'TRT-STRP', 'VOC STRIPPER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (234, 'TRT-UV', 'UV SANITIZER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (235, 'UPS', 'UNINTERUPTED POWER SUPPLY', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (236, 'VEH', 'VEHICLE', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (237, 'WELL', 'WELL', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (238, 'WQANLZR', 'WATER QUALITY ANALYZER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (239, 'XFMR', 'TRANSFORMER', 1, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (240, 'XMTR', 'TRANSMITTER', 0, 0, 1);
            INSERT INTO EquipmentTypes (Id, Abbreviation, Description, IsLockoutRequired, IsEligibleForRedTagPermit, EquipmentGroupId) VALUES (241, 'AMIDATACOLL', 'AMIDATACOLL', 0, 0, 1);
            ";
        }
    }
}
