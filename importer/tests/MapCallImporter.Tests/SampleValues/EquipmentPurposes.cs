using System;

namespace MapCallImporter.SampleValues
{
    public static class EquipmentPurposes
    {
        public static string GetInsertQuery(string equipmentType)
        {
            switch (equipmentType)
            {
                #region ADJUSTABLE SPEED DRIVE

                case "ADJUSTABLE SPEED DRIVE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (310, 'General', 'ASDG', 121);
";

                #endregion

                #region AERATOR

                case "AERATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (311, 'General', 'AERG', 228);
";

                #endregion

                #region AIR COMPRESSOR

                case "AIR COMPRESSOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (312, 'General', 'ACPG', 145);
";

                #endregion

                #region AIR/ VACUUM TANK

                case "AIR/ VACUUM TANK":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (313, 'General', 'AVTG', 222);
";

                #endregion

                #region AM WATER NARUC ACCOUNT

                case "AM WATER NARUC ACCOUNT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (314, 'General', 'NARG', 185);
";

                #endregion

                #region AMIDATACOLL

                case "AMIDATACOLL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (315, 'General', 'ADCG', 241);
";

                #endregion

                #region ARC FLASH PROTECTION

                case "ARC FLASH PROTECTION":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (316, 'General', 'AFPG', 193);
";

                #endregion

                #region BATTERY

                case "BATTERY":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (317, 'General', 'BATG', 123);
";

                #endregion

                #region BATTERY CHARGER

                case "BATTERY CHARGER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (318, 'General', 'BTCG', 124);
";

                #endregion

                #region BLOW OFF VALVE

                case "BLOW OFF VALVE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (319, 'General', 'BOVG', 219);
";

                #endregion

                #region BLOWER

                case "BLOWER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (320, 'General', 'BLOG', 125);
";

                #endregion

                #region BOILER

                case "BOILER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (321, 'General', 'BLRG', 126);
";

                #endregion

                #region BURNER

                case "BURNER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (322, 'General', 'BRNG', 127);
";

                #endregion

                #region CALIBRATION DEVICE

                case "CALIBRATION DEVICE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (323, 'General', 'CBDG', 128);
";

                #endregion

                #region CATHODIC PROTECTION

                case "CATHODIC PROTECTION":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (324, 'General', 'CPTG', 129);
";

                #endregion

                #region CHEMICAL DRY FEEDER

                case "CHEMICAL DRY FEEDER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (325, 'General', 'CDFG', 132);
";

                #endregion

                #region CHEMICAL GAS FEEDER

                case "CHEMICAL GAS FEEDER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (326, 'General', 'CGDG', 133);
";

                #endregion

                #region CHEMICAL GENERATORS

                case "CHEMICAL GENERATORS":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (327, 'General', 'CGNG', 130);
";

                #endregion

                #region CHEMICAL LIQUID FEEDER

                case "CHEMICAL LIQUID FEEDER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (328, 'General', 'CLFG', 134);
";

                #endregion

                #region CHEMICAL PIPING

                case "CHEMICAL PIPING":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (329, 'General', 'CPPG', 131);
";

                #endregion

                #region CHEMICAL TANK

                case "CHEMICAL TANK":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (330, 'General', 'CTKG', 220);
";

                #endregion

                #region CLARIFIER

                case "CLARIFIER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (331, 'General', 'CFRG', 229);
";

                #endregion

                #region CLEAN OUT

                case "CLEAN OUT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (332, 'General', 'CLOG', 137);
";

                #endregion

                #region COLLECTION SYSTEM GENERAL

                case "COLLECTION SYSTEM GENERAL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (334, 'General', 'CSGG', 138);
";

                #endregion

                #region CONTROL PANEL

                case "CONTROL PANEL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (333, 'General', 'CTPG', 135);
";

                #endregion

                #region CONTROLLER

                case "CONTROLLER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (335, 'General', 'CTRG', 136);
";

                #endregion

                #region CONVEYOR

                case "CONVEYOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (336, 'General', 'CVRG', 148);
";

                #endregion

                #region COOLING TOWER

                case "COOLING TOWER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (337, 'General', 'CLTG', 172);
";

                #endregion

                #region DAM

                case "DAM":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (338, 'General', 'DAMG', 149);
";

                #endregion

                #region DEFIBRILLATOR

                case "DEFIBRILLATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (339, 'General', 'DFBG', 122);
";

                #endregion

                #region DISTRIBUTION SYSTEM

                case "DISTRIBUTION SYSTEM":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (340, 'General', 'DSSG', 150);
";

                #endregion

                #region DISTRIBUTION TOOL

                case "DISTRIBUTION TOOL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (341, 'General', 'DSTG', 151);
";

                #endregion

                #region ELEVATOR

                case "ELEVATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (342, 'General', 'EVTG', 152);
";

                #endregion

                #region EMERGENCY GENERATOR

                case "EMERGENCY GENERATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (343, 'General', 'EMGG', 163);
";

                #endregion

                #region EMERGENCY LIGHT

                case "EMERGENCY LIGHT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (344, 'General', 'EMLG', 153);
";

                #endregion

                #region ENGINE

                case "ENGINE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (345, 'General', 'ENGG', 154);
";

                #endregion

                #region EYEWASH

                case "EYEWASH":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (346, 'General', 'EYEG', 155);
";

                #endregion

                #region FACILITY AND GROUNDS

                case "FACILITY AND GROUNDS":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (347, 'General', 'FGDG', 156);
";

                #endregion

                #region FALL PROTECTION

                case "FALL PROTECTION":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (348, 'General', 'FPTG', 194);
";

                #endregion

                #region FILTER

                case "FILTER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (349, 'General', 'FLTG', 231);
";

                #endregion

                #region FIRE ALARM

                case "FIRE ALARM":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (350, 'General', 'FARG', 157);
";

                #endregion

                #region FIRE EXTINGUISHER

                case "FIRE EXTINGUISHER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (351, 'General', 'FETG', 158);
";

                #endregion

                #region FIRE SUPPRESSION

                case "FIRE SUPPRESSION":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (352, 'General', 'FSPG', 159);
";

                #endregion

                #region FIREWALL

                case "FIREWALL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (353, 'General', 'FWLG', 139);
";

                #endregion

                #region FLOATATION DEVICE

                case "FLOATATION DEVICE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (354, 'General', 'FLDG', 195);
";

                #endregion

                #region FLOW METER (NON PREMISE)

                case "FLOW METER (NON PREMISE)":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (355, 'General', 'FMNG', 160);
";

                #endregion

                #region FLOW WEIR

                case "FLOW WEIR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (356, 'General', 'FLWG', 161);
";

                #endregion

                #region FUEL TANK

                case "FUEL TANK":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (357, 'General', 'FTKG', 221);
";

                #endregion

                #region GAS DETECTOR

                case "GAS DETECTOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (358, 'General', 'GDTG', 210);
";

                #endregion

                #region GEARBOX

                case "GEARBOX":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (359, 'General', 'GRBG', 162);
";

                #endregion

                #region GRAVITY SEWER MAIN

                case "GRAVITY SEWER MAIN":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (360, 'General', 'GSMG', 164);
";

                #endregion

                #region GRINDER

                case "GRINDER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (361, 'General', 'GRNG', 165);
";

                #endregion

                #region HEAT EXCHANGER

                case "HEAT EXCHANGER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (362, 'General', 'HXCG', 170);
";

                #endregion

                #region HOIST

                case "HOIST":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (363, 'General', 'HOIG', 166);
";

                #endregion

                #region HVAC CHILLER

                case "HVAC CHILLER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (364, 'General', 'HVCG', 167);
";

                #endregion

                #region HVAC COMBINATION UNIT

                case "HVAC COMBINATION UNIT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (365, 'General', 'HCUG', 168);
";

                #endregion

                #region HVAC DEHUMIDIFIER

                case "HVAC DEHUMIDIFIER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (366, 'General', 'HVDG', 169);
";

                #endregion

                #region HVAC HEATER

                case "HVAC HEATER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (367, 'General', 'HVHG', 171);
";

                #endregion

                #region HVAC VENTILATOR

                case "HVAC VENTILATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (368, 'General', 'HVVG', 173);
";

                #endregion

                #region HYDRANT

                case "HYDRANT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (369, 'General', 'HYDG', 175);
";

                #endregion

                #region INDICATOR

                case "INDICATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (370, 'General', 'INDG', 176);
";

                #endregion

                #region INSTRUMENT SWITCH

                case "INSTRUMENT SWITCH":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (371, 'General', 'INSG', 177);
";

                #endregion

                #region KIT (SAFETY, REPAIR, HAZWOPR)

                case "KIT (SAFETY, REPAIR, HAZWOPR)":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (372, 'General', 'KITG', 178);
";

                #endregion

                #region LAB EQUIPMENT

                case "LAB EQUIPMENT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (373, 'General', 'LABG', 179);
";

                #endregion

                #region LEAK MONITOR

                case "LEAK MONITOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (374, 'General', 'LKMG', 180);
";

                #endregion

                #region MANHOLE

                case "MANHOLE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (375, 'General', 'MNHG', 181);
";

                #endregion

                #region MIXER

                case "MIXER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (376, 'General', 'MXRG', 182);
";

                #endregion

                #region MODEM

                case "MODEM":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (377, 'General', 'MDMG', 140);
";

                #endregion

                #region MOTOR

                case "MOTOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (378, 'General', 'MTRG', 183);
";

                #endregion

                #region MOTOR CONTACTOR

                case "MOTOR CONTACTOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (379, 'General', 'MTCG', 146);
";

                #endregion

                #region MOTOR STARTER

                case "MOTOR STARTER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (380, 'General', 'MTSG', 184);
";

                #endregion

                #region NETWORK ROUTER

                case "NETWORK ROUTER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (381, 'General', 'NWRG', 142);
";

                #endregion

                #region NETWORK SWITCH

                case "NETWORK SWITCH":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (382, 'General', 'NWSG', 143);
";

                #endregion

                #region NON POTABLE WATER TANK

                case "NON POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (383, 'General', 'NPTG', 223);
";

                #endregion

                #region OPERATOR COMPUTER TERMINAL

                case "OPERATOR COMPUTER TERMINAL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (384, 'General', 'OCTG', 186);
";

                #endregion

                #region PC

                case "PC":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (385, 'General', 'PCGG', 187);
";

                #endregion

                #region PDM TOOL

                case "PDM TOOL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (386, 'General', 'PDMG', 188);
";

                #endregion

                #region PHASE CONVERTER

                case "PHASE CONVERTER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (387, 'General', 'PCVG', 189);
";

                #endregion

                #region PLANT VALVE

                case "PLANT VALVE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (388, 'General', 'PLVG', 199);
";

                #endregion

                #region POTABLE WATER TANK

                case "POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (389, 'General', 'PWTG', 224);
";

                #endregion

                #region POWER BREAKER

                case "POWER BREAKER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (390, 'General', 'PBKG', 200);
";

                #endregion

                #region POWER CONDITIONER

                case "POWER CONDITIONER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (391, 'General', 'PCDG', 201);
";

                #endregion

                #region POWER DISCONNECT

                case "POWER DISCONNECT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (392, 'General', 'PDCG', 202);
";

                #endregion

                #region POWER FEEDER CABLE

                case "POWER FEEDER CABLE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (393, 'General', 'PFCG', 203);
";

                #endregion

                #region POWER MONITOR

                case "POWER MONITOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (394, 'General', 'PMTG', 204);
";

                #endregion

                #region POWER PANEL

                case "POWER PANEL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (395, 'General', 'PWPG', 205);
";

                #endregion

                #region POWER RELAY

                case "POWER RELAY":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (396, 'General', 'PWRG', 206);
";

                #endregion

                #region POWER SURGE PROTECTION

                case "POWER SURGE PROTECTION":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (397, 'General', 'PSPG', 207);
";

                #endregion

                #region POWER TRANSFER SWITCH

                case "POWER TRANSFER SWITCH":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (398, 'General', 'PTSG', 227);
";

                #endregion

                #region PRESSURE DAMPER

                case "PRESSURE DAMPER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (399, 'General', 'PDPG', 197);
";

                #endregion

                #region PRINTER

                case "PRINTER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (400, 'General', 'PRNG', 198);
";

                #endregion

                #region PUMP CENTRIFUGAL

                case "PUMP CENTRIFUGAL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (401, 'General', 'PCTG', 190);
";

                #endregion

                #region PUMP GRINDER

                case "PUMP GRINDER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (402, 'General', 'PGRG', 191);
";

                #endregion

                #region PUMP POSITIVE DISPLACEMENT

                case "PUMP POSITIVE DISPLACEMENT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (403, 'General', 'PPDG', 192);
";

                #endregion

                #region RECORDER

                case "RECORDER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (404, 'General', 'RCDG', 208);
";

                #endregion

                #region RESPIRATOR

                case "RESPIRATOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (405, 'General', 'RESG', 196);
";

                #endregion

                #region RTU - PLC

                case "RTU - PLC":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (406, 'General', 'RTUG', 209);
";

                #endregion

                #region SAFETY SHOWER

                case "SAFETY SHOWER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (407, 'General', 'SSRG', 211);
";

                #endregion

                #region SCADA RADIO

                case "SCADA RADIO":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (408, 'General', 'SRDG', 141);
";

                #endregion

                #region SCADA SYSTEM GEN

                case "SCADA SYSTEM GEN":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (409, 'General', 'SSGG', 212);
";

                #endregion

                #region SCALE

                case "SCALE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (410, 'General', 'SCLG', 213);
";

                #endregion

                #region SCREEN

                case "SCREEN":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (411, 'General', 'SCRG', 215);
";

                #endregion

                #region SCRUBBER

                case "SCRUBBER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (412, 'General', 'SCBG', 214);
";

                #endregion

                #region SECONDARY CONTAINMENT

                case "SECONDARY CONTAINMENT":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (413, 'General', 'SCTG', 147);
";

                #endregion

                #region SECURITY SYSTEM

                case "SECURITY SYSTEM":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (414, 'General', 'SECG', 216);
";

                #endregion

                #region SERVER

                case "SERVER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (415, 'General', 'SERG', 217);
";

                #endregion

                #region SOFTENER

                case "SOFTENER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (416, 'General', 'SOFG', 232);
";

                #endregion

                #region STREET VALVE

                case "STREET VALVE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (417, 'General', 'STVG', 218);
";

                #endregion

                #region TELEPHONE

                case "TELEPHONE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (418, 'General', 'TPNG', 144);
";

                #endregion

                #region TOOL

                case "TOOL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (419, 'General', 'TOLG', 226);
";

                #endregion

                #region TRANSFORMER

                case "TRANSFORMER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (420, 'General', 'TRNG', 239);
";

                #endregion

                #region TRANSMITTER

                case "TRANSMITTER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (421, 'General', 'XTRG', 240);
";

                #endregion

                #region UNINTERUPTED POWER SUPPLY

                case "UNINTERUPTED POWER SUPPLY":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (422, 'General', 'UPSG', 235);
";

                #endregion

                #region UV SANITIZER

                case "UV SANITIZER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (423, 'General', 'UVSG', 234);
";

                #endregion

                #region UV-SOUND

                case "UV-SOUND":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (424, 'General', 'USNG', 242);
";

                #endregion

                #region VEHICLE

                case "VEHICLE":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (425, 'General', 'VEHG', 236);
";

                #endregion

                #region VOC STRIPPER

                case "VOC STRIPPER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (426, 'General', 'VOCG', 233);
";

                #endregion

                #region WASTE TANK

                case "WASTE TANK":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (427, 'General', 'WTKG', 225);
";

                #endregion

                #region WATER HEATER

                case "WATER HEATER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (428, 'General', 'WTHG', 174);
";

                #endregion

                #region WATER QUALITY ANALYZER

                case "WATER QUALITY ANALYZER":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (429, 'General', 'WQAG', 238);
";

                #endregion

                #region WATER TREATMENT CONTACTOR

                case "WATER TREATMENT CONTACTOR":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (430, 'General', 'WTCG', 230);
";

                #endregion

                #region WELL

                case "WELL":
                    return @"
INSERT INTO EquipmentPurposes (EquipmentPurposeId, Description, Abbreviation, EquipmentTypeId) VALUES (431, 'General', 'WELG', 237);
";

                #endregion

                default:
                    throw new ArgumentException($"No EqupmentTypes scripted for EquipmentType '{equipmentType}'",
                        nameof(equipmentType));
            }
        }
    }
}