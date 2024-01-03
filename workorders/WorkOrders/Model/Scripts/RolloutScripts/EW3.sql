-- enable work orders
UPDATE [tblOpCntr] SET [WorkOrdersEnabled] = 1 WHERE [OpCntr] = 'EW3';

-- add restoration type costs
SET IDENTITY_INSERT [dbo].[RestorationTypeCosts] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[RestorationTypeCosts]([RestorationTypeCostID], [OperatingCenterID], [RestorationTypeID], [Cost])
SELECT '17', '17', '1', '12' UNION ALL
SELECT '18', '17', '2', '12' UNION ALL
SELECT '19', '17', '3', '15' UNION ALL
SELECT '20', '17', '4', '27' UNION ALL
SELECT '21', '17', '5', '27' UNION ALL
SELECT '22', '17', '6', '15' UNION ALL
SELECT '23', '17', '7', '5' UNION ALL
SELECT '24', '17', '8', '15'
COMMIT;
RAISERROR (N'[dbo].[RestorationTypeCosts]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[RestorationTypeCosts] OFF;

-- configure asset types
INSERT INTO [OperatingCenterAssetTypes] Values(17, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(17, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(17, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(17, 4)

-- materials changes:
update materials set description = [size] + ' ' + cast([Description] as varchar)

UPDATE [Materials] SET [PartNumber] = 'W0020H0H', [Description] = 'Corp,  3/4" Taper X CF' WHERE [MaterialID] = 1;
UPDATE [Materials] SET [PartNumber] = 'W0020101', [Description] = 'Corp, 1" Taper X CF' WHERE [MaterialID] = 2;
UPDATE [Materials] SET [PartNumber] = 'W0020202', [Description] = 'Corp, 2" Taper X CF' WHERE [MaterialID] = 3;
UPDATE [Materials] SET [PartNumber] = 'W0220202', [Description] = 'Ball/Curb Stop, 2" FIP X FIP' WHERE [MaterialID] = 4;
UPDATE [Materials] SET [PartNumber] = 'W0170101', [Description] = 'Ball/Curb Stop, 1" PC X PC' WHERE [MaterialID] = 5;
UPDATE [Materials] SET [PartNumber] = 'W0200H0H', [Description] = 'Ball/Curb Stop,  3/4" CF X CF' WHERE [MaterialID] = 6;
UPDATE [Materials] SET [PartNumber] = 'W0200101', [Description] = 'Ball/Curb Stop, 1" CF X CF' WHERE [MaterialID] = 7;
UPDATE [Materials] SET [PartNumber] = 'W0220H0H', [Description] = 'Ball/Curb Stop,  3/4" FIP X FI' WHERE [MaterialID] = 8;
UPDATE [Materials] SET [PartNumber] = 'W0220101', [Description] = 'Ball/Curb Stop, 1" FIP X FIP' WHERE [MaterialID] = 9;
UPDATE [Materials] SET [PartNumber] = 'W0410202', [Description] = 'Coupling,  2" PC X MIP' WHERE [MaterialID] = 10;
UPDATE [Materials] SET [PartNumber] = 'W0420202', [Description] = 'Coupling,  2" PC X FIP' WHERE [MaterialID] = 11;
UPDATE [Materials] SET [PartNumber] = 'W0430202', [Description] = 'Coupling,  2" PC X PC' WHERE [MaterialID] = 12;
UPDATE [Materials] SET [PartNumber] = 'W0670H0H', [Description] = 'Coupling,   3/4" TNA X IPC' WHERE [MaterialID] = 13;
UPDATE [Materials] SET [PartNumber] = 'W0680101', [Description] = 'Coupling,  1" TNA X PC' WHERE [MaterialID] = 14;
UPDATE [Materials] SET [PartNumber] = 'W0680202', [Description] = 'Coupling,  2" TNA X PC' WHERE [MaterialID] = 15;
UPDATE [Materials] SET [PartNumber] = 'W0480202', [Description] = 'Coupling,  2" CC X FIP' WHERE [MaterialID] = 16;
UPDATE [Materials] SET [PartNumber] = 'W0490202', [Description] = 'Coupling,  2" CC X MIP' WHERE [MaterialID] = 17;
UPDATE [Materials] SET [PartNumber] = '0500202', [Description] = '2" COUPLING CC X CC' WHERE [MaterialID] = 18;
UPDATE [Materials] SET [PartNumber] = 'W0540202', [Description] = 'Coupling,  2" CF X CF' WHERE [MaterialID] = 19;
UPDATE [Materials] SET [PartNumber] = 'W0510202', [Description] = 'Coupling,  2" TNA X CC' WHERE [MaterialID] = 20;
UPDATE [Materials] SET [PartNumber] = '0640202', [Description] = '2" COMPRESSION X FIP' WHERE [MaterialID] = 21;
UPDATE [Materials] SET [PartNumber] = 'W0630202', [Description] = 'Coupling,  2" TNA X FIP' WHERE [MaterialID] = 22;
UPDATE [Materials] SET [PartNumber] = 'W1020201', [Description] = 'YBR, 2" X 1" FIP X CF 2WY' WHERE [MaterialID] = 23;
UPDATE [Materials] SET [PartNumber] = 'W0990201', [Description] = 'YBR, 2" X 1" MIP X CF 3WY' WHERE [MaterialID] = 24;
UPDATE [Materials] SET [PartNumber] = '8800G0G', [Description] = '5/8" METER' WHERE [MaterialID] = 25;
UPDATE [Materials] SET [PartNumber] = '8800101', [Description] = '1" METER' WHERE [MaterialID] = 26;
UPDATE [Materials] SET [PartNumber] = '8801F1F', [Description] = '1 1/2" METER' WHERE [MaterialID] = 27;
UPDATE [Materials] SET [PartNumber] = '8800202', [Description] = '2" METER' WHERE [MaterialID] = 28;
UPDATE [Materials] SET [PartNumber] = '8800303', [Description] = '3" METER' WHERE [MaterialID] = 29;
UPDATE [Materials] SET [PartNumber] = '8800404', [Description] = '4" METER' WHERE [MaterialID] = 30;
UPDATE [Materials] SET [PartNumber] = '8800606', [Description] = '6" METER' WHERE [MaterialID] = 31;
UPDATE [Materials] SET [PartNumber] = 'W0440202', [Description] = 'Coupling,  2" MC X CC' WHERE [MaterialID] = 32;
UPDATE [Materials] SET [PartNumber] = '0750202', [Description] = '2" SCREW LOC PAC' WHERE [MaterialID] = 33;
UPDATE [Materials] SET [PartNumber] = 'W0441F1F', [Description] = 'Coupling,  1 1/2" MC X CC' WHERE [MaterialID] = 34;
UPDATE [Materials] SET [PartNumber] = 'W1600G0G', [Description] = 'Setter,  5/8" K style X CC' WHERE [MaterialID] = 36;
UPDATE [Materials] SET [PartNumber] = 'W1660101', [Description] = 'Setter, 1" X CC' WHERE [MaterialID] = 37;
UPDATE [Materials] SET [PartNumber] = '2002000', [Description] = '20" TILE' WHERE [MaterialID] = 38;
UPDATE [Materials] SET [PartNumber] = 'W2042000', [Description] = 'Frame & Lid, 20"' WHERE [MaterialID] = 39;
UPDATE [Materials] SET [PartNumber] = 'W2300000', [Description] = 'Curb Box, Top CI Screw' WHERE [MaterialID] = 40;
UPDATE [Materials] SET [PartNumber] = 'W2310000', [Description] = 'Curb Box, Bottom CI Screw' WHERE [MaterialID] = 41;
UPDATE [Materials] SET [PartNumber] = 'W2380000', [Description] = 'Box, Valve Top CI SL' WHERE [MaterialID] = 42;
UPDATE [Materials] SET [PartNumber] = 'W2390000', [Description] = 'Box, Valve Bottom CI SL' WHERE [MaterialID] = 43;
UPDATE [Materials] SET [PartNumber] = 'W7000404', [Description] = 'Tee,  4" X 4" MJ' WHERE [MaterialID] = 44;
UPDATE [Materials] SET [PartNumber] = 'W7000604', [Description] = 'Tee,  6" X 4" MJ' WHERE [MaterialID] = 45;
UPDATE [Materials] SET [PartNumber] = 'W7000606', [Description] = 'Tee,  6" X 6" MJ' WHERE [MaterialID] = 46;
UPDATE [Materials] SET [PartNumber] = 'W7000804', [Description] = 'Tee,  8" X 4" MJ' WHERE [MaterialID] = 47;
UPDATE [Materials] SET [PartNumber] = 'W7000806', [Description] = 'Tee,  8" X 6" MJ' WHERE [MaterialID] = 48;
UPDATE [Materials] SET [PartNumber] = 'W7000808', [Description] = 'Tee,  8" X 8" MJ' WHERE [MaterialID] = 49;
UPDATE [Materials] SET [PartNumber] = 'W7001006', [Description] = 'Tee, 10" X  6" MJ' WHERE [MaterialID] = 50;
UPDATE [Materials] SET [PartNumber] = 'W7001008', [Description] = 'Tee, 10" X  8" MJ' WHERE [MaterialID] = 51;
UPDATE [Materials] SET [PartNumber] = 'W7001010', [Description] = 'Tee, 10" X 10" MJ' WHERE [MaterialID] = 52;
UPDATE [Materials] SET [PartNumber] = 'W7001202', [Description] = 'Tee, 12" X  2" MJ' WHERE [MaterialID] = 53;
UPDATE [Materials] SET [PartNumber] = 'W7001204', [Description] = 'Tee, 12" X  4" MJ' WHERE [MaterialID] = 54;
UPDATE [Materials] SET [PartNumber] = 'W7001206', [Description] = 'Tee, 12" X  6" MJ' WHERE [MaterialID] = 55;
UPDATE [Materials] SET [PartNumber] = 'W7001208', [Description] = 'Tee, 12" X  8" MJ' WHERE [MaterialID] = 56;
UPDATE [Materials] SET [PartNumber] = 'W7001210', [Description] = 'Tee, 12" X 10" MJ' WHERE [MaterialID] = 57;
UPDATE [Materials] SET [PartNumber] = 'W7001212', [Description] = 'Tee, 12" X 12" MJ' WHERE [MaterialID] = 58;
UPDATE [Materials] SET [PartNumber] = 'W7001604', [Description] = 'Tee, 16" X  4" MJ' WHERE [MaterialID] = 59;
UPDATE [Materials] SET [PartNumber] = 'W7001606', [Description] = 'Tee, 16" X  6" MJ' WHERE [MaterialID] = 60;
UPDATE [Materials] SET [PartNumber] = 'W7001608', [Description] = 'Tee, 16" X  8" MJ' WHERE [MaterialID] = 61;
UPDATE [Materials] SET [PartNumber] = 'W7001610', [Description] = 'Tee, 16" X 10" MJ' WHERE [MaterialID] = 62;
UPDATE [Materials] SET [PartNumber] = 'W7001612', [Description] = 'Tee, 16" X 12" MJ' WHERE [MaterialID] = 63;
UPDATE [Materials] SET [PartNumber] = 'W7001616', [Description] = 'Tee, 16" X 16" MJ' WHERE [MaterialID] = 64;
UPDATE [Materials] SET [PartNumber] = 'W7001808', [Description] = 'Tee, 18" X  8" MJ' WHERE [MaterialID] = 65;
UPDATE [Materials] SET [PartNumber] = 'W7002006', [Description] = 'Tee, 20" X  6" MJ' WHERE [MaterialID] = 66;
UPDATE [Materials] SET [PartNumber] = 'W7002008', [Description] = 'Tee, 20" X  8" MJ' WHERE [MaterialID] = 67;
UPDATE [Materials] SET [PartNumber] = 'W7002012', [Description] = 'Tee, 20" X 12" MJ' WHERE [MaterialID] = 68;
UPDATE [Materials] SET [PartNumber] = 'W7002016', [Description] = 'Tee, 20" X 16" MJ' WHERE [MaterialID] = 69;
UPDATE [Materials] SET [PartNumber] = 'W7002020', [Description] = 'Tee, 20" X 20" MJ' WHERE [MaterialID] = 70;
UPDATE [Materials] SET [PartNumber] = 'W7002406', [Description] = 'Tee, 24" X  6" MJ' WHERE [MaterialID] = 71;
UPDATE [Materials] SET [PartNumber] = 'W7002408', [Description] = 'Tee, 24" X  8" MJ' WHERE [MaterialID] = 72;
UPDATE [Materials] SET [PartNumber] = 'W7002412', [Description] = 'Tee, 24" X 12" MJ' WHERE [MaterialID] = 73;
UPDATE [Materials] SET [PartNumber] = 'W7002416', [Description] = 'Tee, 24" X 16" MJ' WHERE [MaterialID] = 74;
UPDATE [Materials] SET [PartNumber] = 'W7002424', [Description] = 'Tee, 24" X 24" MJ' WHERE [MaterialID] = 75;
UPDATE [Materials] SET [PartNumber] = 'W7003006', [Description] = 'Tee, 30" X  6" MJ' WHERE [MaterialID] = 76;
UPDATE [Materials] SET [PartNumber] = 'W7003606', [Description] = 'Tee, 36" X  6" MJ' WHERE [MaterialID] = 77;
UPDATE [Materials] SET [PartNumber] = 'W7003608', [Description] = 'Tee, 36" X  8" MJ' WHERE [MaterialID] = 78;
UPDATE [Materials] SET [PartNumber] = 'W7003630', [Description] = 'Tee, 36" X 30" MJ' WHERE [MaterialID] = 79;
UPDATE [Materials] SET [PartNumber] = 'W7040606', [Description] = 'Tee,  6" X 6" Anchor MJ' WHERE [MaterialID] = 80;
UPDATE [Materials] SET [PartNumber] = '7040804', [Description] = '8" X 4" MJ ANCHOR TEE' WHERE [MaterialID] = 81;
UPDATE [Materials] SET [PartNumber] = 'W7040806', [Description] = 'Tee,  8" X 6" Anchor MJ' WHERE [MaterialID] = 82;
UPDATE [Materials] SET [PartNumber] = 'W7040808', [Description] = 'Tee,  8" X 8" Anchor MJ' WHERE [MaterialID] = 83;
UPDATE [Materials] SET [PartNumber] = 'W7041006', [Description] = 'Tee, 10" X  6" Anchor MJ' WHERE [MaterialID] = 84;
UPDATE [Materials] SET [PartNumber] = 'W7041206', [Description] = 'Tee, 12" X  6" Anchor MJ' WHERE [MaterialID] = 85;
UPDATE [Materials] SET [PartNumber] = 'W7041606', [Description] = 'Tee, 16" X  6" Anchor MJ' WHERE [MaterialID] = 86;
UPDATE [Materials] SET [PartNumber] = 'W7042406', [Description] = 'Tee, 24" X  6" Anchor MJ' WHERE [MaterialID] = 87;
UPDATE [Materials] SET [PartNumber] = 'W7010404', [Description] = 'Tee,  4" X 4" FLG' WHERE [MaterialID] = 88;
UPDATE [Materials] SET [PartNumber] = 'W7010604', [Description] = 'Tee,  6" X 4" FLG' WHERE [MaterialID] = 89;
UPDATE [Materials] SET [PartNumber] = 'W7010808', [Description] = 'Tee,  8" X 8" FLG' WHERE [MaterialID] = 90;
UPDATE [Materials] SET [PartNumber] = 'W7021208', [Description] = 'Tee, 12" X  8" RJ' WHERE [MaterialID] = 91;
UPDATE [Materials] SET [PartNumber] = '7021210', [Description] = '12" X 10" TR-FLEX TEE' WHERE [MaterialID] = 92;
UPDATE [Materials] SET [PartNumber] = 'W7021212', [Description] = 'Tee, 12" X 12" RJ' WHERE [MaterialID] = 93;
UPDATE [Materials] SET [PartNumber] = 'W7021606', [Description] = 'Tee, 16" X  6" RJ' WHERE [MaterialID] = 94;
UPDATE [Materials] SET [PartNumber] = 'W7021608', [Description] = 'Tee, 16" X  8" RJ' WHERE [MaterialID] = 95;
UPDATE [Materials] SET [PartNumber] = 'W7021610', [Description] = 'Tee, 16" X 10" RJ' WHERE [MaterialID] = 96;
UPDATE [Materials] SET [PartNumber] = 'W7021612', [Description] = 'Tee, 16" X 12" RJ' WHERE [MaterialID] = 97;
UPDATE [Materials] SET [PartNumber] = 'W7021616', [Description] = 'Tee, 16" X 16" RJ' WHERE [MaterialID] = 98;
UPDATE [Materials] SET [PartNumber] = 'W7022012', [Description] = 'Tee, 20" X 12" RJ' WHERE [MaterialID] = 99;
UPDATE [Materials] SET [PartNumber] = 'W7022020', [Description] = 'Tee, 20" X 20" RJ' WHERE [MaterialID] = 100;
UPDATE [Materials] SET [PartNumber] = 'W7022406', [Description] = 'Tee, 24" X  6" RJ' WHERE [MaterialID] = 101;
UPDATE [Materials] SET [PartNumber] = 'W7022408', [Description] = 'Tee, 24" X  8" RJ' WHERE [MaterialID] = 102;
UPDATE [Materials] SET [PartNumber] = 'W7022412', [Description] = 'Tee, 24" X 12" RJ' WHERE [MaterialID] = 103;
UPDATE [Materials] SET [PartNumber] = 'W7022416', [Description] = 'Tee, 24" X 16" RJ' WHERE [MaterialID] = 104;
UPDATE [Materials] SET [PartNumber] = 'W7022420', [Description] = 'Tee, 24" X 20" RJ' WHERE [MaterialID] = 105;
UPDATE [Materials] SET [PartNumber] = 'W7023006', [Description] = 'Tee, 30" X  6" RJ' WHERE [MaterialID] = 106;
UPDATE [Materials] SET [PartNumber] = 'W7023008', [Description] = 'Tee, 30" X  8" RJ' WHERE [MaterialID] = 107;
UPDATE [Materials] SET [PartNumber] = 'W7023012', [Description] = 'Tee, 30" X 12" RJ' WHERE [MaterialID] = 108;
UPDATE [Materials] SET [PartNumber] = 'W7023016', [Description] = 'Tee, 30" X 16" RJ' WHERE [MaterialID] = 109;
UPDATE [Materials] SET [PartNumber] = 'W7023020', [Description] = 'Tee, 30" X 20" RJ' WHERE [MaterialID] = 110;
UPDATE [Materials] SET [PartNumber] = 'W7023024', [Description] = 'Tee, 30" X 24" RJ' WHERE [MaterialID] = 111;
UPDATE [Materials] SET [PartNumber] = 'W7023030', [Description] = 'Tee, 30" X 30" RJ' WHERE [MaterialID] = 112;
UPDATE [Materials] SET [PartNumber] = 'W7023608', [Description] = 'Tee, 36" X  8" RJ' WHERE [MaterialID] = 113;
UPDATE [Materials] SET [PartNumber] = 'W7100604', [Description] = 'Cross,  6" X 4" MJ' WHERE [MaterialID] = 114;
UPDATE [Materials] SET [PartNumber] = 'W7100606', [Description] = 'Cross,  6" X 6" MJ' WHERE [MaterialID] = 115;
UPDATE [Materials] SET [PartNumber] = 'W7100806', [Description] = 'Cross,  8" X 6" MJ' WHERE [MaterialID] = 116;
UPDATE [Materials] SET [PartNumber] = 'W7100808', [Description] = 'Cross,  8" X 8" MJ' WHERE [MaterialID] = 117;
UPDATE [Materials] SET [PartNumber] = 'W7101006', [Description] = 'Cross, 10" X  6" MJ' WHERE [MaterialID] = 118;
UPDATE [Materials] SET [PartNumber] = 'W7101010', [Description] = 'Cross, 10" X 10" MJ' WHERE [MaterialID] = 119;
UPDATE [Materials] SET [PartNumber] = 'W7101204', [Description] = 'Cross, 12" X  4" MJ' WHERE [MaterialID] = 120;
UPDATE [Materials] SET [PartNumber] = 'W7101206', [Description] = 'Cross, 12" X  6" MJ' WHERE [MaterialID] = 121;
UPDATE [Materials] SET [PartNumber] = 'W7101208', [Description] = 'Cross, 12" X  8" MJ' WHERE [MaterialID] = 122;
UPDATE [Materials] SET [PartNumber] = 'W7101212', [Description] = 'Cross, 12" X 12" MJ' WHERE [MaterialID] = 123;
UPDATE [Materials] SET [PartNumber] = 'W7101608', [Description] = 'Cross, 16" X  8" MJ' WHERE [MaterialID] = 124;
UPDATE [Materials] SET [PartNumber] = 'W7101610', [Description] = 'Cross, 16" X 10" MJ' WHERE [MaterialID] = 125;
UPDATE [Materials] SET [PartNumber] = 'W7101612', [Description] = 'Cross, 16" X 12" MJ' WHERE [MaterialID] = 126;
UPDATE [Materials] SET [PartNumber] = 'W7102412', [Description] = 'Cross, 24" X 12" MJ' WHERE [MaterialID] = 127;
UPDATE [Materials] SET [PartNumber] = 'W7102416', [Description] = 'Cross, 24" X 16" MJ' WHERE [MaterialID] = 128;
UPDATE [Materials] SET [PartNumber] = 'W7200404', [Description] = 'Sleeve,  4" X 4" Tap MJ CI/DI' WHERE [MaterialID] = 129;
UPDATE [Materials] SET [PartNumber] = 'W7200604', [Description] = 'Sleeve,  6" X 4" Tap MJ CI/DI' WHERE [MaterialID] = 130;
UPDATE [Materials] SET [PartNumber] = 'W7200606', [Description] = 'Sleeve,  6" X 6" Tap MJ CI/DI' WHERE [MaterialID] = 131;
UPDATE [Materials] SET [PartNumber] = 'W7200804', [Description] = 'Sleeve,  8" X 4" Tap MJ CI/DI' WHERE [MaterialID] = 132;
UPDATE [Materials] SET [PartNumber] = 'W7200806', [Description] = 'Sleeve,  8" X 6" Tap MJ CI/DI' WHERE [MaterialID] = 133;
UPDATE [Materials] SET [PartNumber] = 'W7200808', [Description] = 'Sleeve,  8" X 8" Tap MJ CI/DI' WHERE [MaterialID] = 134;
UPDATE [Materials] SET [PartNumber] = 'W7201004', [Description] = 'Sleeve, 10" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 135;
UPDATE [Materials] SET [PartNumber] = 'W7201006', [Description] = 'Sleeve, 10" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 136;
UPDATE [Materials] SET [PartNumber] = 'W7201008', [Description] = 'Sleeve, 10" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 137;
UPDATE [Materials] SET [PartNumber] = 'W7201010', [Description] = 'Sleeve, 10" X 10" Tap MJ CI/DI' WHERE [MaterialID] = 138;
UPDATE [Materials] SET [PartNumber] = 'W7201204', [Description] = 'Sleeve, 12" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 139;
UPDATE [Materials] SET [PartNumber] = 'W7201206', [Description] = 'Sleeve, 12" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 140;
UPDATE [Materials] SET [PartNumber] = 'W7201208', [Description] = 'Sleeve, 12" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 141;
UPDATE [Materials] SET [PartNumber] = 'W7201210', [Description] = 'Sleeve, 12" X 10" Tap MJ CI/DI' WHERE [MaterialID] = 142;
UPDATE [Materials] SET [PartNumber] = 'W7201212', [Description] = 'Sleeve, 12" X 12" Tap MJ CI/DI' WHERE [MaterialID] = 143;
UPDATE [Materials] SET [PartNumber] = 'W7201604', [Description] = 'Sleeve, 16" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 144;
UPDATE [Materials] SET [PartNumber] = 'W7201606', [Description] = 'Sleeve, 16" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 145;
UPDATE [Materials] SET [PartNumber] = 'W7201608', [Description] = 'Sleeve, 16" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 146;
UPDATE [Materials] SET [PartNumber] = 'W7201612', [Description] = 'Sleeve, 16" X 12" Tap MJ CI/DI' WHERE [MaterialID] = 147;
UPDATE [Materials] SET [PartNumber] = 'W7201616', [Description] = 'Sleeve, 16" X 16" Tap MJ CI/DI' WHERE [MaterialID] = 148;
UPDATE [Materials] SET [PartNumber] = 'W7201804', [Description] = 'Sleeve, 18" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 149;
UPDATE [Materials] SET [PartNumber] = 'W7201806', [Description] = 'Sleeve, 18" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 150;
UPDATE [Materials] SET [PartNumber] = 'W7201808', [Description] = 'Sleeve, 18" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 151;
UPDATE [Materials] SET [PartNumber] = 'W7201814', [Description] = 'Sleeve, 18" X 14" Tap MJ CI/DI' WHERE [MaterialID] = 152;
UPDATE [Materials] SET [PartNumber] = 'W7202004', [Description] = 'Sleeve, 20" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 153;
UPDATE [Materials] SET [PartNumber] = 'W7202006', [Description] = 'Sleeve, 20" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 154;
UPDATE [Materials] SET [PartNumber] = 'W7202008', [Description] = 'Sleeve, 20" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 155;
UPDATE [Materials] SET [PartNumber] = 'W7202012', [Description] = 'Sleeve, 20" X 12" Tap MJ CI/DI' WHERE [MaterialID] = 156;
UPDATE [Materials] SET [PartNumber] = 'W7202016', [Description] = 'Sleeve, 20" X 16" Tap MJ CI/DI' WHERE [MaterialID] = 157;
UPDATE [Materials] SET [PartNumber] = 'W7202404', [Description] = 'Sleeve, 24" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 158;
UPDATE [Materials] SET [PartNumber] = 'W7202406', [Description] = 'Sleeve, 24" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 159;
UPDATE [Materials] SET [PartNumber] = 'W7202408', [Description] = 'Sleeve, 24" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 160;
UPDATE [Materials] SET [PartNumber] = 'W7202412', [Description] = 'Sleeve, 24" X 12" Tap MJ CI/DI' WHERE [MaterialID] = 161;
UPDATE [Materials] SET [PartNumber] = 'W7202416', [Description] = 'Sleeve, 24" X 16" Tap MJ CI/DI' WHERE [MaterialID] = 162;
UPDATE [Materials] SET [PartNumber] = 'W7203004', [Description] = 'Sleeve, 30" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 163;
UPDATE [Materials] SET [PartNumber] = 'W7203006', [Description] = 'Sleeve, 30" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 164;
UPDATE [Materials] SET [PartNumber] = 'W7203008', [Description] = 'Sleeve, 30" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 165;
UPDATE [Materials] SET [PartNumber] = 'W7203604', [Description] = 'Sleeve, 36" X  4" Tap MJ CI/DI' WHERE [MaterialID] = 166;
UPDATE [Materials] SET [PartNumber] = 'W7203606', [Description] = 'Sleeve, 36" X  6" Tap MJ CI/DI' WHERE [MaterialID] = 167;
UPDATE [Materials] SET [PartNumber] = 'W7203608', [Description] = 'Sleeve, 36" X  8" Tap MJ CI/DI' WHERE [MaterialID] = 168;
UPDATE [Materials] SET [PartNumber] = 'W7203612', [Description] = 'Sleeve, 36" X 12" Tap MJ CI/DI' WHERE [MaterialID] = 169;
UPDATE [Materials] SET [PartNumber] = 'W7203616', [Description] = 'Sleeve, 36" X 16" Tap MJ CI/DI' WHERE [MaterialID] = 170;
UPDATE [Materials] SET [PartNumber] = 'W7220404', [Description] = 'Sleeve,  4" X 4" Tap MJ AC' WHERE [MaterialID] = 171;
UPDATE [Materials] SET [PartNumber] = 'W7220604', [Description] = 'Sleeve,  6" X 4" Tap MJ AC' WHERE [MaterialID] = 172;
UPDATE [Materials] SET [PartNumber] = 'W7220606', [Description] = 'Sleeve,  6" X 6" Tap MJ AC' WHERE [MaterialID] = 173;
UPDATE [Materials] SET [PartNumber] = 'W7220804', [Description] = 'Sleeve,  8" X 4" Tap MJ AC' WHERE [MaterialID] = 174;
UPDATE [Materials] SET [PartNumber] = 'W7220806', [Description] = 'Sleeve,  8" X 6" Tap MJ AC' WHERE [MaterialID] = 175;
UPDATE [Materials] SET [PartNumber] = 'W7220808', [Description] = 'Sleeve,  8" X 8" Tap MJ AC' WHERE [MaterialID] = 176;
UPDATE [Materials] SET [PartNumber] = 'W7221006', [Description] = 'Sleeve, 10" X  6" Tap MJ AC' WHERE [MaterialID] = 177;
UPDATE [Materials] SET [PartNumber] = 'W7221008', [Description] = 'Sleeve, 10" X  8" Tap MJ AC' WHERE [MaterialID] = 178;
UPDATE [Materials] SET [PartNumber] = 'W7221204', [Description] = 'Sleeve, 12" X  4" Tap MJ AC' WHERE [MaterialID] = 179;
UPDATE [Materials] SET [PartNumber] = 'W7221206', [Description] = 'Sleeve, 12" X  6" Tap MJ AC' WHERE [MaterialID] = 180;
UPDATE [Materials] SET [PartNumber] = 'W7221208', [Description] = 'Sleeve, 12" X  8" Tap MJ AC' WHERE [MaterialID] = 181;
UPDATE [Materials] SET [PartNumber] = 'W7221212', [Description] = 'Sleeve, 12" X 12" Tap MJ AC' WHERE [MaterialID] = 182;
UPDATE [Materials] SET [PartNumber] = 'W7231604', [Description] = 'Sleeve, 16" X  4" Tap FLG FAB' WHERE [MaterialID] = 183;
UPDATE [Materials] SET [PartNumber] = 'W7231606', [Description] = 'Sleeve, 16" X  6" Tap FLG FAB' WHERE [MaterialID] = 184;
UPDATE [Materials] SET [PartNumber] = 'W7231608', [Description] = 'Sleeve, 16" X  8" Tap FLG FAB' WHERE [MaterialID] = 185;
UPDATE [Materials] SET [PartNumber] = '7231610', [Description] = '16" X 10" FABR TAP SLV' WHERE [MaterialID] = 186;
UPDATE [Materials] SET [PartNumber] = 'W7231612', [Description] = 'Sleeve, 16" X 12" Tap FLG FAB' WHERE [MaterialID] = 187;
UPDATE [Materials] SET [PartNumber] = '7231812', [Description] = '18" X 12" FABR TAP SLV' WHERE [MaterialID] = 188;
UPDATE [Materials] SET [PartNumber] = '7231816', [Description] = '18" X 16" FABR TAP SLV' WHERE [MaterialID] = 189;
UPDATE [Materials] SET [PartNumber] = 'W7232006', [Description] = 'Sleeve, 20" X  6" Tap FLG FAB' WHERE [MaterialID] = 190;
UPDATE [Materials] SET [PartNumber] = 'W7232008', [Description] = 'Sleeve, 20" X  8" Tap FLG FAB' WHERE [MaterialID] = 191;
UPDATE [Materials] SET [PartNumber] = 'W7232406', [Description] = 'Sleeve, 24" X  6" Tap FLG FAB' WHERE [MaterialID] = 192;
UPDATE [Materials] SET [PartNumber] = '7233010', [Description] = '30" X 10" FABR TAP SLV' WHERE [MaterialID] = 193;
UPDATE [Materials] SET [PartNumber] = 'W738160H', [Description] = 'Saddle, Tap 16" X   3/4" SRV P' WHERE [MaterialID] = 194;
UPDATE [Materials] SET [PartNumber] = 'W7381601', [Description] = 'Saddle, Tap 16" X  1" SRV PSC' WHERE [MaterialID] = 195;
UPDATE [Materials] SET [PartNumber] = 'W7381604', [Description] = 'Saddle, Tap 16" X  4" SRV PSC' WHERE [MaterialID] = 196;
UPDATE [Materials] SET [PartNumber] = 'W7381606', [Description] = 'Saddle, Tap 16" X  6" SRV PSC' WHERE [MaterialID] = 197;
UPDATE [Materials] SET [PartNumber] = 'W7381608', [Description] = 'Saddle, Tap 16" X  8" SRV PSC' WHERE [MaterialID] = 198;
UPDATE [Materials] SET [PartNumber] = 'W7381612', [Description] = 'Saddle, Tap 16" X 12" SRV PSC' WHERE [MaterialID] = 199;
UPDATE [Materials] SET [PartNumber] = '7381616', [Description] = ' 16" TAP SADDLE LJ PIPE' WHERE [MaterialID] = 200;
UPDATE [Materials] SET [PartNumber] = 'W738200H', [Description] = 'Saddle, Tap 20" X   3/4" SRV P' WHERE [MaterialID] = 201;
UPDATE [Materials] SET [PartNumber] = 'W7382001', [Description] = 'Saddle, Tap 20" X  1" SRV PSC' WHERE [MaterialID] = 202;
UPDATE [Materials] SET [PartNumber] = 'W7382004', [Description] = 'Saddle, Tap 20" X  4" SRV PSC' WHERE [MaterialID] = 203;
UPDATE [Materials] SET [PartNumber] = 'W7382006', [Description] = 'Saddle, Tap 20" X  6" SRV PSC' WHERE [MaterialID] = 204;
UPDATE [Materials] SET [PartNumber] = 'W7382008', [Description] = 'Saddle, Tap 20" X  8" SRV PSC' WHERE [MaterialID] = 205;
UPDATE [Materials] SET [PartNumber] = 'W7382012', [Description] = 'Saddle, Tap 20" X 12" SRV PSC' WHERE [MaterialID] = 206;
UPDATE [Materials] SET [PartNumber] = 'W7382016', [Description] = 'Saddle, Tap 20" X 16" SRV PSC' WHERE [MaterialID] = 207;
UPDATE [Materials] SET [PartNumber] = 'W738240H', [Description] = 'Saddle, Tap 24" X   3/4" SRV P' WHERE [MaterialID] = 208;
UPDATE [Materials] SET [PartNumber] = 'W7382401', [Description] = 'Saddle, Tap 24" X  1" SRV PSC' WHERE [MaterialID] = 209;
UPDATE [Materials] SET [PartNumber] = 'W7382402', [Description] = 'Saddle, Tap 24" X  2" SRV PSC' WHERE [MaterialID] = 210;
UPDATE [Materials] SET [PartNumber] = 'W7382404', [Description] = 'Saddle, Tap 24" X  4" SRV PSC' WHERE [MaterialID] = 211;
UPDATE [Materials] SET [PartNumber] = 'W7382406', [Description] = 'Saddle, Tap 24" X  6" SRV PSC' WHERE [MaterialID] = 212;
UPDATE [Materials] SET [PartNumber] = 'W7382408', [Description] = 'Saddle, Tap 24" X  8" SRV PSC' WHERE [MaterialID] = 213;
UPDATE [Materials] SET [PartNumber] = 'W7382412', [Description] = 'Saddle, Tap 24" X 12" SRV PSC' WHERE [MaterialID] = 214;
UPDATE [Materials] SET [PartNumber] = 'W738300H', [Description] = 'Saddle, Tap 30" X   3/4" SRV P' WHERE [MaterialID] = 215;
UPDATE [Materials] SET [PartNumber] = 'W7383001', [Description] = 'Saddle, Tap 30" X  1" SRV PSC' WHERE [MaterialID] = 216;
UPDATE [Materials] SET [PartNumber] = 'W7383002', [Description] = 'Saddle, Tap 30" X  2" SRV PSC' WHERE [MaterialID] = 217;
UPDATE [Materials] SET [PartNumber] = 'W7383004', [Description] = 'Saddle, Tap 30" X  4" SRV PSC' WHERE [MaterialID] = 218;
UPDATE [Materials] SET [PartNumber] = 'W7383006', [Description] = 'Saddle, Tap 30" X  6" SRV PSC' WHERE [MaterialID] = 219;
UPDATE [Materials] SET [PartNumber] = 'W7383008', [Description] = 'Saddle, Tap 30" X  8" SRV PSC' WHERE [MaterialID] = 220;
UPDATE [Materials] SET [PartNumber] = 'W7383012', [Description] = 'Saddle, Tap 30" X 12" SRV PSC' WHERE [MaterialID] = 221;
UPDATE [Materials] SET [PartNumber] = 'W7383016', [Description] = 'Saddle, Tap 30" X 16" SRV PSC' WHERE [MaterialID] = 222;
UPDATE [Materials] SET [PartNumber] = 'W738360H', [Description] = 'Saddle, Tap 36" X   3/4" SRV P' WHERE [MaterialID] = 223;
UPDATE [Materials] SET [PartNumber] = 'W7383601', [Description] = 'Saddle, Tap 36" X  1" SRV PSC' WHERE [MaterialID] = 224;
UPDATE [Materials] SET [PartNumber] = 'W7383602', [Description] = 'Saddle, Tap 36" X  2" SRV PSC' WHERE [MaterialID] = 225;
UPDATE [Materials] SET [PartNumber] = 'W7383604', [Description] = 'Saddle, Tap 36" X  4" SRV PSC' WHERE [MaterialID] = 226;
UPDATE [Materials] SET [PartNumber] = 'W7383606', [Description] = 'Saddle, Tap 36" X  6" SRV PSC' WHERE [MaterialID] = 227;
UPDATE [Materials] SET [PartNumber] = 'W7383608', [Description] = 'Saddle, Tap 36" X  8" SRV PSC' WHERE [MaterialID] = 228;
UPDATE [Materials] SET [PartNumber] = 'W744020H', [Description] = 'Saddle, Tap  2" X  3/4" SRV' WHERE [MaterialID] = 229;
UPDATE [Materials] SET [PartNumber] = 'W7440201', [Description] = 'Saddle, Tap  2" X 1" SRV' WHERE [MaterialID] = 230;
UPDATE [Materials] SET [PartNumber] = 'W7442C0H', [Description] = 'Saddle, Tap  2 1/4" X  3/4" SR' WHERE [MaterialID] = 231;
UPDATE [Materials] SET [PartNumber] = 'W7442C01', [Description] = 'Saddle, Tap  2 1/4" X 1" SRV' WHERE [MaterialID] = 232;
UPDATE [Materials] SET [PartNumber] = 'W744040H', [Description] = 'Saddle, Tap  4" X  3/4" SRV' WHERE [MaterialID] = 233;
UPDATE [Materials] SET [PartNumber] = 'W7440401', [Description] = 'Saddle, Tap  4" X 1" SRV' WHERE [MaterialID] = 234;
UPDATE [Materials] SET [PartNumber] = 'W744060H', [Description] = 'Saddle, Tap  6" X  3/4" SRV' WHERE [MaterialID] = 235;
UPDATE [Materials] SET [PartNumber] = 'W7440601', [Description] = 'Saddle, Tap  6" X 1" SRV' WHERE [MaterialID] = 236;
UPDATE [Materials] SET [PartNumber] = 'W744080H', [Description] = 'Saddle, Tap  8" X  3/4" SRV' WHERE [MaterialID] = 237;
UPDATE [Materials] SET [PartNumber] = 'W7440801', [Description] = 'Saddle, Tap  8" X 1" SRV' WHERE [MaterialID] = 238;
UPDATE [Materials] SET [PartNumber] = 'W7440802', [Description] = 'Saddle, Tap  8" X 2" SRV' WHERE [MaterialID] = 239;
UPDATE [Materials] SET [PartNumber] = 'W7441001', [Description] = 'Saddle, Tap 10" X  1" SRV' WHERE [MaterialID] = 240;
UPDATE [Materials] SET [PartNumber] = 'W7441002', [Description] = 'Saddle, Tap 10" X  2" SRV' WHERE [MaterialID] = 241;
UPDATE [Materials] SET [PartNumber] = 'W744120H', [Description] = 'Saddle, Tap 12" X  3/4" SRV' WHERE [MaterialID] = 242;
UPDATE [Materials] SET [PartNumber] = 'W7441201', [Description] = 'Saddle, Tap 12" X 1" SRV' WHERE [MaterialID] = 243;
UPDATE [Materials] SET [PartNumber] = 'W744160H', [Description] = 'Saddle, Tap 16" X   3/4" SRV' WHERE [MaterialID] = 244;
UPDATE [Materials] SET [PartNumber] = 'W7441601', [Description] = 'Saddle, Tap 16" X  1" SRV' WHERE [MaterialID] = 245;
UPDATE [Materials] SET [PartNumber] = 'W7441801', [Description] = 'Saddle, Tap 18" X  1" SRV' WHERE [MaterialID] = 246;
UPDATE [Materials] SET [PartNumber] = 'W7442001', [Description] = 'Saddle, Tap 20" X  1" SRV' WHERE [MaterialID] = 247;
UPDATE [Materials] SET [PartNumber] = 'W7442401', [Description] = 'Saddle, Tap 24" X  1" SRV' WHERE [MaterialID] = 248;
UPDATE [Materials] SET [PartNumber] = 'W7443601', [Description] = 'Saddle, Tap 36" X  1" SRV' WHERE [MaterialID] = 249;
UPDATE [Materials] SET [PartNumber] = 'W7443602', [Description] = 'Saddle, Tap 36" X  2" SRV' WHERE [MaterialID] = 250;
UPDATE [Materials] SET [PartNumber] = 'W8040404', [Description] = 'Valve,  4" Tap MJ OR' WHERE [MaterialID] = 251;
UPDATE [Materials] SET [PartNumber] = 'W8040606', [Description] = 'Valve,  6" Tap MJ OR' WHERE [MaterialID] = 252;
UPDATE [Materials] SET [PartNumber] = 'W8040808', [Description] = 'Valve,  8" Tap MJ OR' WHERE [MaterialID] = 253;
UPDATE [Materials] SET [PartNumber] = 'W8041010', [Description] = 'Valve, 10" Tap MJ OR' WHERE [MaterialID] = 254;
UPDATE [Materials] SET [PartNumber] = 'W8041212', [Description] = 'Valve, 12" Tap MJ OR' WHERE [MaterialID] = 255;
UPDATE [Materials] SET [PartNumber] = 'W8041616', [Description] = 'Valve, 16" Tap MJ OR' WHERE [MaterialID] = 256;
UPDATE [Materials] SET [PartNumber] = 'W8050404', [Description] = 'Valve,  4" Tap MJ OL' WHERE [MaterialID] = 257;
UPDATE [Materials] SET [PartNumber] = 'W8050606', [Description] = 'Valve,  6" Tap MJ OL' WHERE [MaterialID] = 258;
UPDATE [Materials] SET [PartNumber] = 'W8050808', [Description] = 'Valve,  8" Tap MJ OL' WHERE [MaterialID] = 259;
UPDATE [Materials] SET [PartNumber] = 'W8051010', [Description] = 'Valve, 10" Tap MJ OL' WHERE [MaterialID] = 260;
UPDATE [Materials] SET [PartNumber] = 'W8051212', [Description] = 'Valve, 12" Tap MJ OL' WHERE [MaterialID] = 261;
UPDATE [Materials] SET [PartNumber] = 'W8000404', [Description] = 'Valve,  4" Gate MJ OR' WHERE [MaterialID] = 262;
UPDATE [Materials] SET [PartNumber] = 'W8000606', [Description] = 'Valve,  6" Gate MJ OR' WHERE [MaterialID] = 263;
UPDATE [Materials] SET [PartNumber] = 'W8000808', [Description] = 'Valve,  8" Gate MJ OR' WHERE [MaterialID] = 264;
UPDATE [Materials] SET [PartNumber] = 'W8010404', [Description] = 'Valve,  4" Gate MJ OL' WHERE [MaterialID] = 265;
UPDATE [Materials] SET [PartNumber] = 'W8010606', [Description] = 'Valve,  6" Gate MJ OL' WHERE [MaterialID] = 266;
UPDATE [Materials] SET [PartNumber] = 'W8010808', [Description] = 'Valve,  8" Gate MJ OL' WHERE [MaterialID] = 267;
UPDATE [Materials] SET [PartNumber] = 'W8011010', [Description] = 'Valve, 10" Gate MJ OL' WHERE [MaterialID] = 268;
UPDATE [Materials] SET [PartNumber] = 'W8440606', [Description] = 'Valve,  6" Butterfly MJ OR' WHERE [MaterialID] = 269;
UPDATE [Materials] SET [PartNumber] = 'W8441010', [Description] = 'Valve, 10" Butterfly MJ OR' WHERE [MaterialID] = 270;
UPDATE [Materials] SET [PartNumber] = 'W8441212', [Description] = 'Valve, 12" Butterfly MJ OR' WHERE [MaterialID] = 271;
UPDATE [Materials] SET [PartNumber] = 'W8441616', [Description] = 'Valve, 16" Butterfly MJ OR' WHERE [MaterialID] = 272;
UPDATE [Materials] SET [PartNumber] = 'W8441818', [Description] = 'Valve, 18" Butterfly MJ OR' WHERE [MaterialID] = 273;
UPDATE [Materials] SET [PartNumber] = 'W8442020', [Description] = 'Valve, 20" Butterfly MJ OR' WHERE [MaterialID] = 274;
UPDATE [Materials] SET [PartNumber] = 'W8442424', [Description] = 'Valve, 24" Butterfly MJ OR' WHERE [MaterialID] = 275;
UPDATE [Materials] SET [PartNumber] = 'W8443030', [Description] = 'Valve, 30" Butterfly MJ OR' WHERE [MaterialID] = 276;
UPDATE [Materials] SET [PartNumber] = 'W8443636', [Description] = 'Valve, 36" Butterfly MJ OR' WHERE [MaterialID] = 277;
UPDATE [Materials] SET [PartNumber] = 'W8450606', [Description] = 'Valve,  6" Butterfly MJ OL' WHERE [MaterialID] = 278;
UPDATE [Materials] SET [PartNumber] = 'W8451010', [Description] = 'Valve, 10" Butterfly MJ OL' WHERE [MaterialID] = 279;
UPDATE [Materials] SET [PartNumber] = 'W8451212', [Description] = 'Valve, 12" Butterfly MJ OL' WHERE [MaterialID] = 280;
UPDATE [Materials] SET [PartNumber] = 'W8451616', [Description] = 'Valve, 16" Butterfly MJ OL' WHERE [MaterialID] = 281;
UPDATE [Materials] SET [PartNumber] = 'W8451818', [Description] = 'Valve, 18" Butterfly MJ OL' WHERE [MaterialID] = 282;
UPDATE [Materials] SET [PartNumber] = 'W8452020', [Description] = 'Valve, 20" Butterfly MJ OL' WHERE [MaterialID] = 283;
UPDATE [Materials] SET [PartNumber] = 'W8452424', [Description] = 'Valve, 24" Butterfly MJ OL' WHERE [MaterialID] = 284;
UPDATE [Materials] SET [PartNumber] = 'W8453030', [Description] = 'Valve, 30" Butterfly MJ OL' WHERE [MaterialID] = 285;
UPDATE [Materials] SET [PartNumber] = 'W8453636', [Description] = 'Valve, 36" Butterfly MJ OL' WHERE [MaterialID] = 286;
UPDATE [Materials] SET [PartNumber] = 'W8570202', [Description] = 'Valve,  2" Detect Check FLG' WHERE [MaterialID] = 287;
UPDATE [Materials] SET [PartNumber] = 'W8570404', [Description] = 'Valve,  4" Detect Check FLG' WHERE [MaterialID] = 288;
UPDATE [Materials] SET [PartNumber] = 'W8570606', [Description] = 'Valve,  6" Check Detect FLG' WHERE [MaterialID] = 289;
UPDATE [Materials] SET [PartNumber] = 'W8570808', [Description] = 'Valve,  8" Check Detect FLG' WHERE [MaterialID] = 290;
UPDATE [Materials] SET [PartNumber] = 'W8571010', [Description] = 'Valve, 10" Check Detect FLG' WHERE [MaterialID] = 291;
UPDATE [Materials] SET [PartNumber] = 'W8571212', [Description] = 'Valve, 12" Check Detect FLG' WHERE [MaterialID] = 292;
UPDATE [Materials] SET [PartNumber] = 'W3000404', [Description] = 'Sleeve,  4" Solid MJ LP' WHERE [MaterialID] = 293;
UPDATE [Materials] SET [PartNumber] = 'W3000606', [Description] = 'Sleeve,  6" Solid MJ LP' WHERE [MaterialID] = 294;
UPDATE [Materials] SET [PartNumber] = 'W3000808', [Description] = 'Sleeve,  8" Solid MJ LP' WHERE [MaterialID] = 295;
UPDATE [Materials] SET [PartNumber] = 'W3001010', [Description] = 'Sleeve, 10" Solid MJ LP' WHERE [MaterialID] = 296;
UPDATE [Materials] SET [PartNumber] = 'W3001212', [Description] = 'Sleeve, 12" Solid MJ LP' WHERE [MaterialID] = 297;
UPDATE [Materials] SET [PartNumber] = 'W3001616', [Description] = 'Sleeve, 16" Solid MJ LP' WHERE [MaterialID] = 298;
UPDATE [Materials] SET [PartNumber] = 'W3001818', [Description] = 'Sleeve, 18" Solid MJ LP' WHERE [MaterialID] = 299;
UPDATE [Materials] SET [PartNumber] = 'W3002020', [Description] = 'Sleeve, 20" Solid MJ LP' WHERE [MaterialID] = 300;
UPDATE [Materials] SET [PartNumber] = 'W3002424', [Description] = 'Sleeve, 24" Solid MJ LP' WHERE [MaterialID] = 301;
UPDATE [Materials] SET [PartNumber] = 'W3003030', [Description] = 'Sleeve, 30" Solid MJ LP' WHERE [MaterialID] = 302;
UPDATE [Materials] SET [PartNumber] = 'W3003636', [Description] = 'Sleeve, 36" Solid MJ LP' WHERE [MaterialID] = 303;
UPDATE [Materials] SET [PartNumber] = 'W3040404', [Description] = 'Sleeve,  4" MJ DP' WHERE [MaterialID] = 304;
UPDATE [Materials] SET [PartNumber] = 'W3040606', [Description] = 'Sleeve,  6" MJ DP' WHERE [MaterialID] = 305;
UPDATE [Materials] SET [PartNumber] = 'W3040808', [Description] = 'Sleeve,  8" MJ DP' WHERE [MaterialID] = 306;
UPDATE [Materials] SET [PartNumber] = 'W3041010', [Description] = 'Sleeve, 10" MJ DP' WHERE [MaterialID] = 307;
UPDATE [Materials] SET [PartNumber] = 'W3041212', [Description] = 'Sleeve, 12" MJ DP' WHERE [MaterialID] = 308;
UPDATE [Materials] SET [PartNumber] = 'W3041616', [Description] = 'Sleeve, 16" MJ DP' WHERE [MaterialID] = 309;
UPDATE [Materials] SET [PartNumber] = 'W3250404', [Description] = 'Bend,  4" MJ 90°' WHERE [MaterialID] = 310;
UPDATE [Materials] SET [PartNumber] = 'W3250606', [Description] = 'Bend,  6" MJ 90°' WHERE [MaterialID] = 311;
UPDATE [Materials] SET [PartNumber] = 'W3250808', [Description] = 'Bend,  8" MJ 90°' WHERE [MaterialID] = 312;
UPDATE [Materials] SET [PartNumber] = 'W3251010', [Description] = 'Bend, 10" MJ 90°' WHERE [MaterialID] = 313;
UPDATE [Materials] SET [PartNumber] = 'W3251212', [Description] = 'Bend, 12" MJ 90°' WHERE [MaterialID] = 314;
UPDATE [Materials] SET [PartNumber] = 'W3251616', [Description] = 'Bend, 16" MJ 90°' WHERE [MaterialID] = 315;
UPDATE [Materials] SET [PartNumber] = 'W3252020', [Description] = 'Bend, 20" MJ 90°' WHERE [MaterialID] = 316;
UPDATE [Materials] SET [PartNumber] = 'W3252424', [Description] = 'Bend, 24" MJ 90°' WHERE [MaterialID] = 317;
UPDATE [Materials] SET [PartNumber] = 'W3253030', [Description] = 'Bend, 30" MJ 90°' WHERE [MaterialID] = 318;
UPDATE [Materials] SET [PartNumber] = 'W3253636', [Description] = 'Bend, 36" MJ 90°' WHERE [MaterialID] = 319;
UPDATE [Materials] SET [PartNumber] = 'W3260404', [Description] = 'Bend,  4" MJ 45°' WHERE [MaterialID] = 320;
UPDATE [Materials] SET [PartNumber] = 'W3260606', [Description] = 'Bend,  6" MJ 45°' WHERE [MaterialID] = 321;
UPDATE [Materials] SET [PartNumber] = 'W3260808', [Description] = 'Bend,  8" MJ 45°' WHERE [MaterialID] = 322;
UPDATE [Materials] SET [PartNumber] = 'W3261010', [Description] = 'Bend, 10" MJ 45°' WHERE [MaterialID] = 323;
UPDATE [Materials] SET [PartNumber] = 'W3261212', [Description] = 'Bend, 12" MJ 45°' WHERE [MaterialID] = 324;
UPDATE [Materials] SET [PartNumber] = 'W3261616', [Description] = 'Bend, 16" MJ 45°' WHERE [MaterialID] = 325;
UPDATE [Materials] SET [PartNumber] = 'W3261818', [Description] = 'Bend, 18" MJ 45°' WHERE [MaterialID] = 326;
UPDATE [Materials] SET [PartNumber] = 'W3262020', [Description] = 'Bend, 20" MJ 45°' WHERE [MaterialID] = 327;
UPDATE [Materials] SET [PartNumber] = 'W3262424', [Description] = 'Bend, 24" MJ 45°' WHERE [MaterialID] = 328;
UPDATE [Materials] SET [PartNumber] = 'W3263030', [Description] = 'Bend, 30" MJ 45°' WHERE [MaterialID] = 329;
UPDATE [Materials] SET [PartNumber] = 'W3263636', [Description] = 'Bend, 36" MJ 45°' WHERE [MaterialID] = 330;
UPDATE [Materials] SET [PartNumber] = 'W3270404', [Description] = 'Bend,  4" MJ 22 1/2°' WHERE [MaterialID] = 331;
UPDATE [Materials] SET [PartNumber] = 'W3270606', [Description] = 'Bend,  6" MJ 22 1/2°' WHERE [MaterialID] = 332;
UPDATE [Materials] SET [PartNumber] = 'W3270808', [Description] = 'Bend,  8" MJ 22 1/2°' WHERE [MaterialID] = 333;
UPDATE [Materials] SET [PartNumber] = 'W3271010', [Description] = 'Bend, 10" MJ 22 1/2°' WHERE [MaterialID] = 334;
UPDATE [Materials] SET [PartNumber] = 'W3271212', [Description] = 'Bend, 12" MJ 22 1/2°' WHERE [MaterialID] = 335;
UPDATE [Materials] SET [PartNumber] = 'W3271616', [Description] = 'Bend, 16" MJ 22 1/2°' WHERE [MaterialID] = 336;
UPDATE [Materials] SET [PartNumber] = 'W3272020', [Description] = 'Bend, 20" MJ 22 1/2°' WHERE [MaterialID] = 337;
UPDATE [Materials] SET [PartNumber] = 'W3272424', [Description] = 'Bend, 24" MJ 22 1/2°' WHERE [MaterialID] = 338;
UPDATE [Materials] SET [PartNumber] = 'W3273030', [Description] = 'Bend, 30" MJ 22 1/2°' WHERE [MaterialID] = 339;
UPDATE [Materials] SET [PartNumber] = 'W3273636', [Description] = 'Bend, 36" MJ 22 1/2°' WHERE [MaterialID] = 340;
UPDATE [Materials] SET [PartNumber] = 'W3280404', [Description] = 'Bend,  4" MJ 11 1/4°' WHERE [MaterialID] = 341;
UPDATE [Materials] SET [PartNumber] = 'W3280606', [Description] = 'Bend,  6" MJ 11 1/4°' WHERE [MaterialID] = 342;
UPDATE [Materials] SET [PartNumber] = 'W3280808', [Description] = 'Bend,  8" MJ 11 1/4°' WHERE [MaterialID] = 343;
UPDATE [Materials] SET [PartNumber] = 'W3281010', [Description] = 'Bend, 10" MJ 11 1/4°' WHERE [MaterialID] = 344;
UPDATE [Materials] SET [PartNumber] = 'W3281212', [Description] = 'Bend, 12" MJ 11 1/4°' WHERE [MaterialID] = 345;
UPDATE [Materials] SET [PartNumber] = 'W3281616', [Description] = 'Bend, 16" MJ 11 1/4°' WHERE [MaterialID] = 346;
UPDATE [Materials] SET [PartNumber] = 'W3282020', [Description] = 'Bend, 20" MJ 11 1/4°' WHERE [MaterialID] = 347;
UPDATE [Materials] SET [PartNumber] = 'W3282424', [Description] = 'Bend, 24" MJ 11 1/4°' WHERE [MaterialID] = 348;
UPDATE [Materials] SET [PartNumber] = 'W3283030', [Description] = 'Bend, 30" MJ 11 1/4°' WHERE [MaterialID] = 349;
UPDATE [Materials] SET [PartNumber] = 'W3283636', [Description] = 'Bend, 36" MJ 11 1/4°' WHERE [MaterialID] = 350;
UPDATE [Materials] SET [PartNumber] = 'W3353636', [Description] = 'Bend, 36" RJ 90°' WHERE [MaterialID] = 351;
UPDATE [Materials] SET [PartNumber] = 'W3360606', [Description] = 'Bend,  6" RJ 45°' WHERE [MaterialID] = 352;
UPDATE [Materials] SET [PartNumber] = 'W3360808', [Description] = 'Bend,  8" RJ 45°' WHERE [MaterialID] = 353;
UPDATE [Materials] SET [PartNumber] = 'W3361212', [Description] = 'Bend, 12" RJ 45°' WHERE [MaterialID] = 354;
UPDATE [Materials] SET [PartNumber] = 'W3361616', [Description] = 'Bend, 16" RJ 45°' WHERE [MaterialID] = 355;
UPDATE [Materials] SET [PartNumber] = 'W3362020', [Description] = 'Bend, 20" RJ 45°' WHERE [MaterialID] = 356;
UPDATE [Materials] SET [PartNumber] = 'W3362424', [Description] = 'Bend, 24" RJ 45°' WHERE [MaterialID] = 357;
UPDATE [Materials] SET [PartNumber] = 'W3363030', [Description] = 'Bend, 30" RJ 45°' WHERE [MaterialID] = 358;
UPDATE [Materials] SET [PartNumber] = 'W3363636', [Description] = 'Bend, 36" RJ 45°' WHERE [MaterialID] = 359;
UPDATE [Materials] SET [PartNumber] = 'W3370606', [Description] = 'Bend,  6" RJ 22 1/2°' WHERE [MaterialID] = 360;
UPDATE [Materials] SET [PartNumber] = 'W3371212', [Description] = 'Bend, 12" RJ 22 1/2°' WHERE [MaterialID] = 361;
UPDATE [Materials] SET [PartNumber] = 'W3371616', [Description] = 'Bend, 16" RJ 22 1/2°' WHERE [MaterialID] = 362;
UPDATE [Materials] SET [PartNumber] = 'W3372020', [Description] = 'Bend, 20" RJ 22 1/2°' WHERE [MaterialID] = 363;
UPDATE [Materials] SET [PartNumber] = 'W3372424', [Description] = 'Bend, 24" RJ 22 1/2°' WHERE [MaterialID] = 364;
UPDATE [Materials] SET [PartNumber] = 'W3373030', [Description] = 'Bend, 30" RJ 22 1/2°' WHERE [MaterialID] = 365;
UPDATE [Materials] SET [PartNumber] = 'W3373636', [Description] = 'Bend, 36" RJ 22 1/2°' WHERE [MaterialID] = 366;
UPDATE [Materials] SET [PartNumber] = 'W3380606', [Description] = 'Bend,  6" RJ 11 1/4°' WHERE [MaterialID] = 367;
UPDATE [Materials] SET [PartNumber] = 'W3381212', [Description] = 'Bend, 12" RJ 11 1/4°' WHERE [MaterialID] = 368;
UPDATE [Materials] SET [PartNumber] = 'W3381616', [Description] = 'Bend, 16" RJ 11 1/4°' WHERE [MaterialID] = 369;
UPDATE [Materials] SET [PartNumber] = 'W3382020', [Description] = 'Bend, 20" RJ 11 1/4°' WHERE [MaterialID] = 370;
UPDATE [Materials] SET [PartNumber] = 'W3382424', [Description] = 'Bend, 24" RJ 11 1/4°' WHERE [MaterialID] = 371;
UPDATE [Materials] SET [PartNumber] = 'W3383030', [Description] = 'Bend, 30" RJ 11 1/4°' WHERE [MaterialID] = 372;
UPDATE [Materials] SET [PartNumber] = 'W3383636', [Description] = 'Bend, 36" RJ 11 1/4°' WHERE [MaterialID] = 373;
UPDATE [Materials] SET [PartNumber] = 'W3600406', [Description] = 'Offset,  4" X  6" MJ BE X PE' WHERE [MaterialID] = 374;
UPDATE [Materials] SET [PartNumber] = 'W3600412', [Description] = 'Offset,  4" X 12" MJ BE X PE' WHERE [MaterialID] = 375;
UPDATE [Materials] SET [PartNumber] = 'W3600612', [Description] = 'Offset,  6" X 12" MJ BE X PE' WHERE [MaterialID] = 376;
UPDATE [Materials] SET [PartNumber] = 'W3600618', [Description] = 'Offset,  6" X 18" MJ BE X PE' WHERE [MaterialID] = 377;
UPDATE [Materials] SET [PartNumber] = 'W3600624', [Description] = 'Offset,  6" X 24" MJ BE X PE' WHERE [MaterialID] = 378;
UPDATE [Materials] SET [PartNumber] = 'W3600606', [Description] = 'Offset,  6" X  6" MJ BE X PE' WHERE [MaterialID] = 379;
UPDATE [Materials] SET [PartNumber] = 'W3600806', [Description] = 'Offset,  8" X  6" MJ BE X PE' WHERE [MaterialID] = 380;
UPDATE [Materials] SET [PartNumber] = 'W3600812', [Description] = 'Offset,  8" X 12" MJ BE X PE' WHERE [MaterialID] = 381;
UPDATE [Materials] SET [PartNumber] = 'W3600818', [Description] = 'Offset,  8" X 18" MJ BE X PE' WHERE [MaterialID] = 382;
UPDATE [Materials] SET [PartNumber] = 'W3601012', [Description] = 'Offset, 10" X 12" MJ BE X PE' WHERE [MaterialID] = 383;
UPDATE [Materials] SET [PartNumber] = 'W3601218', [Description] = 'Offset, 12" X 18" MJ BE X PE' WHERE [MaterialID] = 384;
UPDATE [Materials] SET [PartNumber] = 'W3601612', [Description] = 'Offset, 16" X 12" MJ BE X PE' WHERE [MaterialID] = 385;
UPDATE [Materials] SET [PartNumber] = 'W3601618', [Description] = 'Offset, 16" X 18" MJ BE X PE' WHERE [MaterialID] = 386;
UPDATE [Materials] SET [PartNumber] = 'W3660604', [Description] = 'Reducer,  6" X 4" MJ' WHERE [MaterialID] = 387;
UPDATE [Materials] SET [PartNumber] = 'W3660804', [Description] = 'Reducer,  8" X 4" MJ' WHERE [MaterialID] = 388;
UPDATE [Materials] SET [PartNumber] = 'W3660806', [Description] = 'Reducer,  8" X 6" MJ' WHERE [MaterialID] = 389;
UPDATE [Materials] SET [PartNumber] = 'W3661006', [Description] = 'Reducer, 10" X 6" MJ' WHERE [MaterialID] = 390;
UPDATE [Materials] SET [PartNumber] = 'W3661008', [Description] = 'Reducer, 10" X 8" MJ' WHERE [MaterialID] = 391;
UPDATE [Materials] SET [PartNumber] = 'W3661204', [Description] = 'Reducer, 12" X  4" MJ' WHERE [MaterialID] = 392;
UPDATE [Materials] SET [PartNumber] = 'W3661206', [Description] = 'Reducer, 12" X  6" MJ' WHERE [MaterialID] = 393;
UPDATE [Materials] SET [PartNumber] = 'W3661208', [Description] = 'Reducer, 12" X  8" MJ' WHERE [MaterialID] = 394;
UPDATE [Materials] SET [PartNumber] = 'W3661210', [Description] = 'Reducer, 12" X 10" MJ' WHERE [MaterialID] = 395;
UPDATE [Materials] SET [PartNumber] = 'W3661606', [Description] = 'Reducer, 16" X  6" MJ' WHERE [MaterialID] = 396;
UPDATE [Materials] SET [PartNumber] = 'W3661608', [Description] = 'Reducer, 16" X  8" MJ' WHERE [MaterialID] = 397;
UPDATE [Materials] SET [PartNumber] = 'W3661610', [Description] = 'Reducer, 16" X 10" MJ' WHERE [MaterialID] = 398;
UPDATE [Materials] SET [PartNumber] = 'W3661612', [Description] = 'Reducer, 16" X 12" MJ' WHERE [MaterialID] = 399;
UPDATE [Materials] SET [PartNumber] = 'W3662012', [Description] = 'Reducer, 20" X 12" MJ' WHERE [MaterialID] = 400;
UPDATE [Materials] SET [PartNumber] = 'W3662016', [Description] = 'Reducer, 20" X 16" MJ' WHERE [MaterialID] = 401;
UPDATE [Materials] SET [PartNumber] = 'W3662412', [Description] = 'Reducer, 24" X 12" MJ' WHERE [MaterialID] = 402;
UPDATE [Materials] SET [PartNumber] = 'W3663024', [Description] = 'Reducer, 30" X 24" MJ' WHERE [MaterialID] = 403;
UPDATE [Materials] SET [PartNumber] = 'W3721612', [Description] = 'Reducer, 16" X 12" RJ' WHERE [MaterialID] = 404;
UPDATE [Materials] SET [PartNumber] = 'W7700402', [Description] = 'Cap,  4" X 2" MJ' WHERE [MaterialID] = 405;
UPDATE [Materials] SET [PartNumber] = 'W7700404', [Description] = 'Cap,  4" MJ' WHERE [MaterialID] = 406;
UPDATE [Materials] SET [PartNumber] = 'W7700602', [Description] = 'Cap,  6" X 2" MJ' WHERE [MaterialID] = 407;
UPDATE [Materials] SET [PartNumber] = 'W7700606', [Description] = 'Cap,  6" MJ' WHERE [MaterialID] = 408;
UPDATE [Materials] SET [PartNumber] = 'W7700802', [Description] = 'Cap,  8" X 2" MJ' WHERE [MaterialID] = 409;
UPDATE [Materials] SET [PartNumber] = 'W7700808', [Description] = 'Cap,  8" MJ' WHERE [MaterialID] = 410;
UPDATE [Materials] SET [PartNumber] = 'W7701202', [Description] = 'Cap, 12" X 2" MJ' WHERE [MaterialID] = 411;
UPDATE [Materials] SET [PartNumber] = 'W7701212', [Description] = 'Cap, 12" MJ' WHERE [MaterialID] = 412;
UPDATE [Materials] SET [PartNumber] = 'W7701602', [Description] = 'Cap, 16" X 2" MJ' WHERE [MaterialID] = 413;
UPDATE [Materials] SET [PartNumber] = 'W7701616', [Description] = 'Cap, 16" MJ' WHERE [MaterialID] = 414;
UPDATE [Materials] SET [PartNumber] = 'W7701818', [Description] = 'Cap, 18" MJ' WHERE [MaterialID] = 415;
UPDATE [Materials] SET [PartNumber] = 'W7702020', [Description] = 'Cap, 20" MJ' WHERE [MaterialID] = 416;
UPDATE [Materials] SET [PartNumber] = 'W7750402', [Description] = 'Plug,  4" DI MJ  W/2" TAP' WHERE [MaterialID] = 417;
UPDATE [Materials] SET [PartNumber] = 'W7750404', [Description] = 'Plug,  4" DI MJ' WHERE [MaterialID] = 418;
UPDATE [Materials] SET [PartNumber] = 'W7750602', [Description] = 'Plug,  6" DI MJ W/2" TAP' WHERE [MaterialID] = 419;
UPDATE [Materials] SET [PartNumber] = 'W7750604', [Description] = 'Plug,  6" DI MJ W/4" Tap' WHERE [MaterialID] = 420;
UPDATE [Materials] SET [PartNumber] = 'W7750606', [Description] = 'Plug,  6" DI MJ' WHERE [MaterialID] = 421;
UPDATE [Materials] SET [PartNumber] = 'W7750802', [Description] = 'Plug,  8" DI MJ W/2" TAP' WHERE [MaterialID] = 422;
UPDATE [Materials] SET [PartNumber] = 'W7750808', [Description] = 'Plug,  8" DI MJ' WHERE [MaterialID] = 423;
UPDATE [Materials] SET [PartNumber] = 'W7751202', [Description] = 'Plug, 12" DI MJ W/2" TAP' WHERE [MaterialID] = 424;
UPDATE [Materials] SET [PartNumber] = 'W7751204', [Description] = 'Plug, 12" DI MJ W/4" Tap' WHERE [MaterialID] = 425;
UPDATE [Materials] SET [PartNumber] = 'W7751212', [Description] = 'Plug, 12" DI MJ' WHERE [MaterialID] = 426;
UPDATE [Materials] SET [PartNumber] = 'W7751602', [Description] = 'Plug, 16" DI MJ W/2" TAP' WHERE [MaterialID] = 427;
UPDATE [Materials] SET [PartNumber] = 'W7751616', [Description] = 'Plug, 16" DI MJ' WHERE [MaterialID] = 428;
UPDATE [Materials] SET [PartNumber] = 'W775182F', [Description] = 'Plug, 18" DI MJ W/2 1/2" TAP' WHERE [MaterialID] = 429;
UPDATE [Materials] SET [PartNumber] = 'W7752020', [Description] = 'Plug, 20" DI MJ' WHERE [MaterialID] = 430;
UPDATE [Materials] SET [PartNumber] = 'W7770402', [Description] = 'Plug,  4" X 2" DI SJ' WHERE [MaterialID] = 431;
UPDATE [Materials] SET [PartNumber] = 'W7770404', [Description] = 'Plug,  4" DI SJ' WHERE [MaterialID] = 432;
UPDATE [Materials] SET [PartNumber] = 'W7770602', [Description] = 'Plug,  6" X 2" DI SJ' WHERE [MaterialID] = 433;
UPDATE [Materials] SET [PartNumber] = 'W7770606', [Description] = 'Plug,  6" DI SJ' WHERE [MaterialID] = 434;
UPDATE [Materials] SET [PartNumber] = 'W7770802', [Description] = 'Plug,  8" X 2" DI SJ W/2" TAP' WHERE [MaterialID] = 435;
UPDATE [Materials] SET [PartNumber] = 'W7770808', [Description] = 'Plug,  8" DI SJ' WHERE [MaterialID] = 436;
UPDATE [Materials] SET [PartNumber] = 'W7771202', [Description] = 'Plug, 12" X 2" DI SJ W/2" TAP' WHERE [MaterialID] = 437;
UPDATE [Materials] SET [PartNumber] = 'W7771602', [Description] = 'Plug, 16" X 2" DI SJ W/2" TAP' WHERE [MaterialID] = 438;
UPDATE [Materials] SET [PartNumber] = 'W7771616', [Description] = 'Plug, 16" DI SJ' WHERE [MaterialID] = 439;
UPDATE [Materials] SET [PartNumber] = 'W3780606', [Description] = 'Gland,  6" Retainer RJ' WHERE [MaterialID] = 440;
UPDATE [Materials] SET [PartNumber] = 'W3780808', [Description] = 'Gland,  8" Retainer RJ' WHERE [MaterialID] = 441;
UPDATE [Materials] SET [PartNumber] = 'W3781212', [Description] = 'Gland, 12" Retainer RJ' WHERE [MaterialID] = 442;
UPDATE [Materials] SET [PartNumber] = '7790402', [Description] = '4" X 2" PUSH IN PLUG' WHERE [MaterialID] = 443;
UPDATE [Materials] SET [PartNumber] = '7790602', [Description] = '6" X 2" PUSH IN PLUG' WHERE [MaterialID] = 444;
UPDATE [Materials] SET [PartNumber] = '7790802', [Description] = '8" X 2" PUSH IN PLUG' WHERE [MaterialID] = 445;
UPDATE [Materials] SET [PartNumber] = '7791202', [Description] = '12" X 2" PUSH IN PLUG' WHERE [MaterialID] = 446;
UPDATE [Materials] SET [PartNumber] = 'W7901616', [Description] = 'Adapter, 16" LJB X MJB LCP' WHERE [MaterialID] = 447;
UPDATE [Materials] SET [PartNumber] = 'W7902424', [Description] = 'Adapter, 24" LJB X MJB LCP' WHERE [MaterialID] = 448;
UPDATE [Materials] SET [PartNumber] = 'W7903030', [Description] = 'Adapter, 30" LJB X MJB LCP' WHERE [MaterialID] = 449;
UPDATE [Materials] SET [PartNumber] = 'W4000335', [Description] = 'Pipe,  3" DI SJ CL 350' WHERE [MaterialID] = 450;
UPDATE [Materials] SET [PartNumber] = 'W4000435', [Description] = 'Pipe,  4" DI SJ CL 350' WHERE [MaterialID] = 451;
UPDATE [Materials] SET [PartNumber] = 'W4000650', [Description] = 'Pipe,  6" DI SJ CL 50' WHERE [MaterialID] = 452;
UPDATE [Materials] SET [PartNumber] = 'W4000852', [Description] = 'Pipe,  8" DI SJ CL 52' WHERE [MaterialID] = 453;
UPDATE [Materials] SET [PartNumber] = 'W4001052', [Description] = 'Pipe, 10" DI SJ CL 52' WHERE [MaterialID] = 454;
UPDATE [Materials] SET [PartNumber] = 'W4001252', [Description] = 'Pipe, 12" DI SJ CL 52' WHERE [MaterialID] = 455;
UPDATE [Materials] SET [PartNumber] = 'W4001652', [Description] = 'Pipe, 16" DI SJ CL 52' WHERE [MaterialID] = 456;
UPDATE [Materials] SET [PartNumber] = 'W4001852', [Description] = 'Pipe, 18" DI SJ CL 52' WHERE [MaterialID] = 457;
UPDATE [Materials] SET [PartNumber] = 'W4002052', [Description] = 'Pipe, 20" DI SJ CL 52' WHERE [MaterialID] = 458;
UPDATE [Materials] SET [PartNumber] = 'W4002452', [Description] = 'Pipe, 24" DI SJ CL 52' WHERE [MaterialID] = 459;
UPDATE [Materials] SET [PartNumber] = 'W4003052', [Description] = 'Pipe, 30" DI SJ CL 52' WHERE [MaterialID] = 460;
UPDATE [Materials] SET [PartNumber] = 'W4003652', [Description] = 'Pipe, 36" DI SJ CL 52' WHERE [MaterialID] = 461;
UPDATE [Materials] SET [PartNumber] = 'W4030852', [Description] = 'Pipe,  8" DI RJ CL 52' WHERE [MaterialID] = 462;
UPDATE [Materials] SET [PartNumber] = 'W4031252', [Description] = 'Pipe, 12" DI RJ CL 52' WHERE [MaterialID] = 463;
UPDATE [Materials] SET [PartNumber] = 'W4031652', [Description] = 'Pipe, 16" DI RJ CL 52' WHERE [MaterialID] = 464;
UPDATE [Materials] SET [PartNumber] = 'W4032052', [Description] = 'Pipe, 20" DI RJ CL 52' WHERE [MaterialID] = 465;
UPDATE [Materials] SET [PartNumber] = 'W4032425', [Description] = 'Pipe, 24" DI RJ CL 250' WHERE [MaterialID] = 466;
UPDATE [Materials] SET [PartNumber] = 'W4033052', [Description] = 'Pipe, 30" DI RJ CL 52' WHERE [MaterialID] = 467;
UPDATE [Materials] SET [PartNumber] = 'W4033652', [Description] = 'Pipe, 36" DI RJ CL 52' WHERE [MaterialID] = 468;
UPDATE [Materials] SET [PartNumber] = 'W4270H0H', [Description] = 'Pipe,   3/4" PE IPS' WHERE [MaterialID] = 469;
UPDATE [Materials] SET [PartNumber] = 'W4270101', [Description] = 'Pipe,  1" PE IPS' WHERE [MaterialID] = 470;
UPDATE [Materials] SET [PartNumber] = 'W4270202', [Description] = 'Pipe,  2" PE IPS' WHERE [MaterialID] = 471;
UPDATE [Materials] SET [PartNumber] = '4210H0H', [Description] = '3/4" COPPER PIPE' WHERE [MaterialID] = 472;
UPDATE [Materials] SET [PartNumber] = '4210101', [Description] = '1" COPPER PIPE' WHERE [MaterialID] = 473;
UPDATE [Materials] SET [PartNumber] = '4220202', [Description] = '2" COPPER PIPE' WHERE [MaterialID] = 474;
UPDATE [Materials] SET [PartNumber] = 'W5184016', [Description] = 'Hydrant, Asbury 4''6" Bury' WHERE [MaterialID] = 475;
UPDATE [Materials] SET [PartNumber] = 'W5184046', [Description] = 'Hydrant, LB 4''6" Bury' WHERE [MaterialID] = 476;
UPDATE [Materials] SET [PartNumber] = 'W5184026', [Description] = 'Hydrant, Bradley 4''6" Bury' WHERE [MaterialID] = 477;
UPDATE [Materials] SET [PartNumber] = 'W5184056', [Description] = 'Hydrant, Holmdel 4''6" Bury' WHERE [MaterialID] = 478;
UPDATE [Materials] SET [PartNumber] = 'W5184036', [Description] = 'Hydrant, OG 4''6" Bury' WHERE [MaterialID] = 479;
UPDATE [Materials] SET [PartNumber] = '6400000', [Description] = ' EA ELECTRIC HORIZ MOTOR' WHERE [MaterialID] = 480;
UPDATE [Materials] SET [PartNumber] = '6410000', [Description] = ' EA RIGHT ANGLE DRIVE' WHERE [MaterialID] = 481;
UPDATE [Materials] SET [PartNumber] = 'W8700404', [Description] = 'Sleeve,  4" Hydro Stop' WHERE [MaterialID] = 482;
UPDATE [Materials] SET [PartNumber] = 'W8700606', [Description] = 'Sleeve,  6" Hydro Stop' WHERE [MaterialID] = 483;
UPDATE [Materials] SET [PartNumber] = 'W8700808', [Description] = 'Sleeve,  8" Hydro Stop' WHERE [MaterialID] = 484;
UPDATE [Materials] SET [PartNumber] = 'W8701010', [Description] = 'Sleeve, 10" Hydro Stop' WHERE [MaterialID] = 485;
UPDATE [Materials] SET [PartNumber] = 'W8701212', [Description] = 'Sleeve, 12" Hydro Stop' WHERE [MaterialID] = 486;
UPDATE [Materials] SET [PartNumber] = 'W3780404', [Description] = 'Gland,  4" Retainer RJ' WHERE [MaterialID] = 487;
UPDATE [Materials] SET [PartNumber] = 'W7440602', [Description] = 'Saddle, Tap  6" X 2" SRV' WHERE [MaterialID] = 488;
UPDATE [Materials] SET [PartNumber] = 'W2002430', [Description] = 'Box, Meter Plastic 24" X 30"' WHERE [MaterialID] = 489;
UPDATE [Materials] SET [PartNumber] = 'W2042400', [Description] = 'Frame & Lid, 24"' WHERE [MaterialID] = 490;
UPDATE [Materials] SET [PartNumber] = 'W2003036', [Description] = 'Box, Meter Plastic 30" X 36"' WHERE [MaterialID] = 491;
UPDATE [Materials] SET [PartNumber] = 'W2043600', [Description] = 'Frame & Lid, 36"' WHERE [MaterialID] = 492;
UPDATE [Materials] SET [PartNumber] = '1570202', [Description] = '2" SETTER' WHERE [MaterialID] = 493;
UPDATE [Materials] SET [PartNumber] = 'W8150202', [Description] = 'Valve,  2" Gate FIP OL' WHERE [MaterialID] = 494;
UPDATE [Materials] SET [PartNumber] = 'W8150202W', [Description] = 'Valve,  2" Gate FIP OL Wheel' WHERE [MaterialID] = 495;
UPDATE [Materials] SET [PartNumber] = '0160101', [Description] = '1" BALL / CURB STOP, 1" PC X M' WHERE [MaterialID] = 496;
UPDATE [Materials] SET [PartNumber] = 'W0160H0H', [Description] = 'Ball/Curb Stop,  3/4" PC X FIP' WHERE [MaterialID] = 497;
UPDATE [Materials] SET [PartNumber] = 'W0170H0H', [Description] = 'Ball/Curb Stop,  3/4" PC X PC' WHERE [MaterialID] = 498;
UPDATE [Materials] SET [PartNumber] = 'W0180101', [Description] = 'Ball/Curb Stop, 1" PC X CF' WHERE [MaterialID] = 499;
UPDATE [Materials] SET [PartNumber] = 'W0200202', [Description] = 'Ball/Curb Stop, 2" CF X CF' WHERE [MaterialID] = 500;
UPDATE [Materials] SET [PartNumber] = 'W0210H0HM', [Description] = 'Ball/Curb Stop,  3/4" CC X CC' WHERE [MaterialID] = 501;
UPDATE [Materials] SET [PartNumber] = 'W0230101', [Description] = 'Ball/Curb Stop, 1" CF X FIP' WHERE [MaterialID] = 502;
UPDATE [Materials] SET [PartNumber] = 'W0230H0H', [Description] = 'Ball/Curb Stop,  3/4" CF X FIP' WHERE [MaterialID] = 503;
UPDATE [Materials] SET [PartNumber] = 'W0410101', [Description] = 'Coupling,  1" PC X MIP' WHERE [MaterialID] = 504;
UPDATE [Materials] SET [PartNumber] = 'W041010H', [Description] = 'Coupling,  1" X  3/4" PC X MIP' WHERE [MaterialID] = 505;
UPDATE [Materials] SET [PartNumber] = 'W0410H0H', [Description] = 'Coupling,   3/4" PC X MIP' WHERE [MaterialID] = 506;
UPDATE [Materials] SET [PartNumber] = 'W0412F2F', [Description] = 'Coupling,  2 1/2" PC X MIP' WHERE [MaterialID] = 507;
UPDATE [Materials] SET [PartNumber] = 'W0420101', [Description] = 'Coupling,  1" PC X FIP' WHERE [MaterialID] = 508;
UPDATE [Materials] SET [PartNumber] = 'W0420H0H', [Description] = 'Coupling,   3/4" PC X FIP' WHERE [MaterialID] = 509;
UPDATE [Materials] SET [PartNumber] = 'W0421F1F', [Description] = 'Coupling,  1 1/2" PC X FIP' WHERE [MaterialID] = 510;
UPDATE [Materials] SET [PartNumber] = 'W0430101', [Description] = 'Coupling,  1" PC X PC' WHERE [MaterialID] = 511;
UPDATE [Materials] SET [PartNumber] = 'W0430H0H', [Description] = 'Coupling,   3/4" PC X PC' WHERE [MaterialID] = 512;
UPDATE [Materials] SET [PartNumber] = 'W0450101', [Description] = 'Coupling,  1" PC X CF' WHERE [MaterialID] = 513;
UPDATE [Materials] SET [PartNumber] = 'W0450H0H', [Description] = 'Coupling,   3/4" PC X CF' WHERE [MaterialID] = 514;
UPDATE [Materials] SET [PartNumber] = 'W0460H0H', [Description] = 'Coupling,   3/4" PC X IPC' WHERE [MaterialID] = 515;
UPDATE [Materials] SET [PartNumber] = 'W0470101', [Description] = 'Coupling,  1" PC X CC' WHERE [MaterialID] = 516;
UPDATE [Materials] SET [PartNumber] = 'W0470H0H', [Description] = 'Coupling,   3/4" PC X CC' WHERE [MaterialID] = 517;
UPDATE [Materials] SET [PartNumber] = 'W0480101', [Description] = 'Coupling,  1" CC X FIP' WHERE [MaterialID] = 518;
UPDATE [Materials] SET [PartNumber] = 'W0490101', [Description] = 'Coupling,  1" CC X MIP' WHERE [MaterialID] = 519;
UPDATE [Materials] SET [PartNumber] = 'W0500101', [Description] = 'Coupling,  1" CF X CF 3PT UN' WHERE [MaterialID] = 520;
UPDATE [Materials] SET [PartNumber] = 'W0500H0H', [Description] = 'Coupling,   3/4" CF X CF 3PT U' WHERE [MaterialID] = 521;
UPDATE [Materials] SET [PartNumber] = 'W051010H', [Description] = 'Coupling,  1" X  3/4" TNA X CC' WHERE [MaterialID] = 522;
UPDATE [Materials] SET [PartNumber] = 'W0530H01', [Description] = 'Coupling,   3/4" X 1" CF X MIP' WHERE [MaterialID] = 523;
UPDATE [Materials] SET [PartNumber] = 'W0540101', [Description] = 'Coupling,  1" CF X CF' WHERE [MaterialID] = 524;
UPDATE [Materials] SET [PartNumber] = 'W0540H0H', [Description] = 'Coupling,   3/4" CF X CF' WHERE [MaterialID] = 525;
UPDATE [Materials] SET [PartNumber] = 'W0790101', [Description] = 'Coupling,  1" CC X CC' WHERE [MaterialID] = 526;
UPDATE [Materials] SET [PartNumber] = 'W079010H', [Description] = 'Coupling,  1" X  3/4" CC X CC' WHERE [MaterialID] = 527;
UPDATE [Materials] SET [PartNumber] = 'W0790202', [Description] = 'Coupling,  2" CC X CC' WHERE [MaterialID] = 528;
UPDATE [Materials] SET [PartNumber] = 'W0790H0H', [Description] = 'Coupling,   3/4" CC X CC' WHERE [MaterialID] = 529;
UPDATE [Materials] SET [PartNumber] = '0940201', [Description] = '2" X 1" YBR, 2" X 1" CF X CF 2' WHERE [MaterialID] = 530;
UPDATE [Materials] SET [PartNumber] = 'W0961F01', [Description] = 'YBR, 1 1/2" X 1" CC X CC 2WY' WHERE [MaterialID] = 531;
UPDATE [Materials] SET [PartNumber] = '124010H', [Description] = '1" X 3/4" BEND, 1" X 3/4" BR P' WHERE [MaterialID] = 532;
UPDATE [Materials] SET [PartNumber] = 'W6220H0H', [Description] = 'Bend,   3/4" X  3/4" TNA X CC ' WHERE [MaterialID] = 533;
UPDATE [Materials] SET [PartNumber] = 'W1550101', [Description] = 'Resetter, 1"' WHERE [MaterialID] = 534;
UPDATE [Materials] SET [PartNumber] = 'W1550G04', [Description] = 'Resetter,  5/8" X  4" H' WHERE [MaterialID] = 535;
UPDATE [Materials] SET [PartNumber] = 'W1550G06', [Description] = 'Resetter,  5/8" X  6" H' WHERE [MaterialID] = 536;
UPDATE [Materials] SET [PartNumber] = 'W1550G07', [Description] = 'Resetter,  5/8" X  7" H' WHERE [MaterialID] = 537;
UPDATE [Materials] SET [PartNumber] = 'W1550G09', [Description] = 'Resetter,  5/8" X  9" H' WHERE [MaterialID] = 538;
UPDATE [Materials] SET [PartNumber] = 'W1550G0H', [Description] = 'Resetter,  5/8" X   3/4" H' WHERE [MaterialID] = 539;
UPDATE [Materials] SET [PartNumber] = 'W1550G12', [Description] = 'Resetter,  5/8" X 12" H' WHERE [MaterialID] = 540;
UPDATE [Materials] SET [PartNumber] = 'W1550G15', [Description] = 'Resetter,  5/8" X 15" H' WHERE [MaterialID] = 541;
UPDATE [Materials] SET [PartNumber] = 'W1550G18', [Description] = 'Resetter,  5/8" X 18" H' WHERE [MaterialID] = 542;
UPDATE [Materials] SET [PartNumber] = 'W1550G21', [Description] = 'Resetter,  5/8" X 21" H' WHERE [MaterialID] = 543;
UPDATE [Materials] SET [PartNumber] = 'W1550G24', [Description] = 'Resetter,  5/8" X 24" H' WHERE [MaterialID] = 544;
UPDATE [Materials] SET [PartNumber] = 'W1550G36', [Description] = 'Resetter,  5/8" X 36" H' WHERE [MaterialID] = 545;
UPDATE [Materials] SET [PartNumber] = 'W1550H0H', [Description] = 'Resetter,  3/4"' WHERE [MaterialID] = 546;
UPDATE [Materials] SET [PartNumber] = 'W1580101', [Description] = 'Setter, 1" Meter (inside horn)' WHERE [MaterialID] = 547;
UPDATE [Materials] SET [PartNumber] = 'W1580G0G', [Description] = 'Setter,  5/8" Meter  (inside h' WHERE [MaterialID] = 548;
UPDATE [Materials] SET [PartNumber] = 'W1580G0H', [Description] = 'Setter,  5/8" X  3/4" Meter  (' WHERE [MaterialID] = 549;
UPDATE [Materials] SET [PartNumber] = 'W1590101', [Description] = 'Horn, 1" Meter K' WHERE [MaterialID] = 550;
UPDATE [Materials] SET [PartNumber] = 'W1590G0G', [Description] = 'Horn,  5/8" Meter K' WHERE [MaterialID] = 551;
UPDATE [Materials] SET [PartNumber] = 'W1600101', [Description] = 'Setter, 1" K style X CC' WHERE [MaterialID] = 552;
UPDATE [Materials] SET [PartNumber] = 'W1600G01', [Description] = 'Setter,  5/8" X 1" K style X C' WHERE [MaterialID] = 553;
UPDATE [Materials] SET [PartNumber] = 'W1600G0F', [Description] = 'Setter,  5/8" X  1/2" K style ' WHERE [MaterialID] = 554;
UPDATE [Materials] SET [PartNumber] = 'W1600G0H', [Description] = 'Setter,  5/8" X  3/4" K style ' WHERE [MaterialID] = 555;
UPDATE [Materials] SET [PartNumber] = 'W1610101', [Description] = 'Setter, 1" K style X IPC' WHERE [MaterialID] = 556;
UPDATE [Materials] SET [PartNumber] = 'W1610G01', [Description] = 'Setter,  5/8" X 1" K style X I' WHERE [MaterialID] = 557;
UPDATE [Materials] SET [PartNumber] = '1610G0H', [Description] = '5/8" X 3/4" SETTER, 5/8" X 3/4' WHERE [MaterialID] = 558;
UPDATE [Materials] SET [PartNumber] = 'W1620101', [Description] = 'Setter, 1" K style X PC' WHERE [MaterialID] = 559;
UPDATE [Materials] SET [PartNumber] = 'W1620G01', [Description] = 'Setter,  5/8" X 1" K style X P' WHERE [MaterialID] = 560;
UPDATE [Materials] SET [PartNumber] = 'W1660202', [Description] = 'Setter, 2" X CC' WHERE [MaterialID] = 561;
UPDATE [Materials] SET [PartNumber] = 'W1660G0H', [Description] = 'Setter,  5/8" X  3/4" X CC' WHERE [MaterialID] = 562;
UPDATE [Materials] SET [PartNumber] = 'W1670202', [Description] = 'Setter, 2" FIP X FIP' WHERE [MaterialID] = 563;
UPDATE [Materials] SET [PartNumber] = 'W1780G0H', [Description] = 'Valve,   5/8" X  3/4" Yoke Ang' WHERE [MaterialID] = 564;
UPDATE [Materials] SET [PartNumber] = 'W1900202', [Description] = 'Flange,  2" Meter FIP' WHERE [MaterialID] = 565;
UPDATE [Materials] SET [PartNumber] = 'W1901F1F', [Description] = 'Flange,  1 1/2" Meter FIP' WHERE [MaterialID] = 566;
UPDATE [Materials] SET [PartNumber] = 'W1921F1F', [Description] = 'Flange,  1 1/2" Meter Adapt X ' WHERE [MaterialID] = 567;
UPDATE [Materials] SET [PartNumber] = 'W1930202', [Description] = 'Flange,  2" Meter Adapt X IPC' WHERE [MaterialID] = 568;
UPDATE [Materials] SET [PartNumber] = '1931F1F', [Description] = '1 1/2" FLANGE, 1 1/2" METER AD' WHERE [MaterialID] = 569;
UPDATE [Materials] SET [PartNumber] = 'W2001500', [Description] = 'Box, Meter Plastic 15"' WHERE [MaterialID] = 570;
UPDATE [Materials] SET [PartNumber] = 'W2002030', [Description] = 'Box, Meter Plastic 20" X 30"' WHERE [MaterialID] = 571;
UPDATE [Materials] SET [PartNumber] = 'W2003636', [Description] = 'Box, Meter Plastic 36" X 36"' WHERE [MaterialID] = 572;
UPDATE [Materials] SET [PartNumber] = 'W2041500', [Description] = 'Frame & Lid, 15"' WHERE [MaterialID] = 573;
UPDATE [Materials] SET [PartNumber] = 'W2041800', [Description] = 'Frame & Lid, 18"' WHERE [MaterialID] = 574;
UPDATE [Materials] SET [PartNumber] = 'W2160000', [Description] = 'Curb Box, Top PL SL' WHERE [MaterialID] = 575;
UPDATE [Materials] SET [PartNumber] = 'W2170000', [Description] = 'Curb Box, Bottom PL SL' WHERE [MaterialID] = 576;
UPDATE [Materials] SET [PartNumber] = 'W2290000', [Description] = 'Curb Box, Complete CI Screw' WHERE [MaterialID] = 577;
UPDATE [Materials] SET [PartNumber] = 'W2400000', [Description] = 'Box, Valve Complete PL SL' WHERE [MaterialID] = 578;
UPDATE [Materials] SET [PartNumber] = 'W2850606', [Description] = 'Coupling,  6" FLEX Bolted DI' WHERE [MaterialID] = 579;
UPDATE [Materials] SET [PartNumber] = 'W3351212', [Description] = 'Bend, 12" RJ 90°' WHERE [MaterialID] = 580;
UPDATE [Materials] SET [PartNumber] = 'W3352020', [Description] = 'Bend, 20" RJ 90°' WHERE [MaterialID] = 581;
UPDATE [Materials] SET [PartNumber] = 'W3352424', [Description] = 'Bend, 24" RJ 90°' WHERE [MaterialID] = 582;
UPDATE [Materials] SET [PartNumber] = 'W3600808', [Description] = 'Offset,  8" X  8" MJ BE X PE' WHERE [MaterialID] = 583;
UPDATE [Materials] SET [PartNumber] = 'W3662420', [Description] = 'Reducer, 24" X 20" MJ' WHERE [MaterialID] = 584;
UPDATE [Materials] SET [PartNumber] = 'W3663016', [Description] = 'Reducer, 30" X 16" MJ' WHERE [MaterialID] = 585;
UPDATE [Materials] SET [PartNumber] = 'W3700604', [Description] = 'Reducer,  6" X 4" FLG' WHERE [MaterialID] = 586;
UPDATE [Materials] SET [PartNumber] = 'W3760606', [Description] = 'Gland,  6" Retainer DI' WHERE [MaterialID] = 587;
UPDATE [Materials] SET [PartNumber] = 'W3762424', [Description] = 'Gland, 24" Retainer DI' WHERE [MaterialID] = 588;
UPDATE [Materials] SET [PartNumber] = 'W3761010', [Description] = 'Gland, 10" Retainer DI' WHERE [MaterialID] = 589;
UPDATE [Materials] SET [PartNumber] = 'W3761616', [Description] = 'Gland, 16" Retainer DI' WHERE [MaterialID] = 590;
UPDATE [Materials] SET [PartNumber] = 'W3761818', [Description] = 'Gland, 18" Retainer DI' WHERE [MaterialID] = 591;
UPDATE [Materials] SET [PartNumber] = 'W3762020', [Description] = 'Gland, 20" Retainer DI' WHERE [MaterialID] = 592;
UPDATE [Materials] SET [PartNumber] = 'W3811616', [Description] = 'Ring, 16" Gripper RJ' WHERE [MaterialID] = 593;
UPDATE [Materials] SET [PartNumber] = 'W3812424', [Description] = 'Ring, 24" Gripper RJ' WHERE [MaterialID] = 594;
UPDATE [Materials] SET [PartNumber] = 'W3850404', [Description] = 'Gasket,  4" Field Lok SJ' WHERE [MaterialID] = 595;
UPDATE [Materials] SET [PartNumber] = 'W3850606', [Description] = 'Gasket,  6" Field Lok SJ' WHERE [MaterialID] = 596;
UPDATE [Materials] SET [PartNumber] = 'W3850808', [Description] = 'Gasket,  8" Field Lok SJ' WHERE [MaterialID] = 597;
UPDATE [Materials] SET [PartNumber] = 'W3851010', [Description] = 'Gasket, 10" Field Lok SJ' WHERE [MaterialID] = 598;
UPDATE [Materials] SET [PartNumber] = 'W3851212', [Description] = 'Gasket, 12" Field Lok SJ' WHERE [MaterialID] = 599;
UPDATE [Materials] SET [PartNumber] = 'W3851616', [Description] = 'Gasket, 16" Field Lok SJ' WHERE [MaterialID] = 600;
UPDATE [Materials] SET [PartNumber] = 'W3900H0H', [Description] = 'Bolt, 3/4" Eye' WHERE [MaterialID] = 601;
UPDATE [Materials] SET [PartNumber] = 'W4000451', [Description] = 'Pipe,  4" DI SJ CL 51' WHERE [MaterialID] = 602;
UPDATE [Materials] SET [PartNumber] = 'W4000635', [Description] = 'Pipe,  6" DI SJ CL 350' WHERE [MaterialID] = 603;
UPDATE [Materials] SET [PartNumber] = 'W4000835', [Description] = 'Pipe,  8" DI SJ CL 350' WHERE [MaterialID] = 604;
UPDATE [Materials] SET [PartNumber] = 'W4001035', [Description] = 'Pipe, 10" DI SJ CL 350' WHERE [MaterialID] = 605;
UPDATE [Materials] SET [PartNumber] = 'W4001235', [Description] = 'Pipe, 12" DI SJ CL 350' WHERE [MaterialID] = 606;
UPDATE [Materials] SET [PartNumber] = 'W4001635', [Description] = 'Pipe, 16" DI SJ CL 350' WHERE [MaterialID] = 607;
UPDATE [Materials] SET [PartNumber] = 'W4002050', [Description] = 'Pipe, 20" DI SJ CL 50' WHERE [MaterialID] = 608;
UPDATE [Materials] SET [PartNumber] = 'W4002450', [Description] = 'Pipe, 24" DI SJ CL 50' WHERE [MaterialID] = 609;
UPDATE [Materials] SET [PartNumber] = 'W4022C2C', [Description] = 'Pipe,  2 1/4" CI' WHERE [MaterialID] = 610;
UPDATE [Materials] SET [PartNumber] = 'W4030635', [Description] = 'Pipe,  6" DI RJ CL 350' WHERE [MaterialID] = 611;
UPDATE [Materials] SET [PartNumber] = 'W4030835', [Description] = 'Pipe,  8" DI RJ CL 350' WHERE [MaterialID] = 612;
UPDATE [Materials] SET [PartNumber] = 'W4031235', [Description] = 'Pipe, 12" DI RJ CL 350' WHERE [MaterialID] = 613;
UPDATE [Materials] SET [PartNumber] = 'W4031635', [Description] = 'Pipe, 16" DI RJ CL 350' WHERE [MaterialID] = 614;
UPDATE [Materials] SET [PartNumber] = 'W4032050', [Description] = 'Pipe, 20" DI RJ CL 50' WHERE [MaterialID] = 615;
UPDATE [Materials] SET [PartNumber] = 'W4220202', [Description] = 'Pipe,  2" Copper Type K' WHERE [MaterialID] = 616;
UPDATE [Materials] SET [PartNumber] = 'W4271F1F', [Description] = 'Pipe,  1 1/2" PE IPS' WHERE [MaterialID] = 617;
UPDATE [Materials] SET [PartNumber] = 'W5183016', [Description] = 'Hydrant, Lake 4''6"Bury' WHERE [MaterialID] = 618;
UPDATE [Materials] SET [PartNumber] = 'W7221010', [Description] = 'Sleeve, 10" X 10" Tap MJ AC' WHERE [MaterialID] = 619;
UPDATE [Materials] SET [PartNumber] = 'W7230806', [Description] = 'Sleeve,  8" X 6" Tap FLG FAB' WHERE [MaterialID] = 620;
UPDATE [Materials] SET [PartNumber] = 'W7241006', [Description] = 'Sleeve, 10" X  6" Tap FAB SS C' WHERE [MaterialID] = 621;
UPDATE [Materials] SET [PartNumber] = 'W7231204', [Description] = 'Sleeve, 12" X  4" Tap FLG FAB' WHERE [MaterialID] = 622;
UPDATE [Materials] SET [PartNumber] = 'W7231206', [Description] = 'Sleeve, 12" X  6" Tap FLG FAB' WHERE [MaterialID] = 623;
UPDATE [Materials] SET [PartNumber] = 'W7231208', [Description] = 'Sleeve, 12" X  8" Tap FLG FAB' WHERE [MaterialID] = 624;
UPDATE [Materials] SET [PartNumber] = 'W7231616', [Description] = 'Sleeve, 16" X 16" Tap FLG FAB' WHERE [MaterialID] = 625;
UPDATE [Materials] SET [PartNumber] = 'W7240808', [Description] = 'Sleeve,  8" X 8" Tap FAB SS CI' WHERE [MaterialID] = 626;
UPDATE [Materials] SET [PartNumber] = 'W7440301', [Description] = 'Saddle, Tap  3" X 1" SRV' WHERE [MaterialID] = 627;
UPDATE [Materials] SET [PartNumber] = 'W7440402', [Description] = 'Saddle, Tap  4" X 2" SRV' WHERE [MaterialID] = 628;
UPDATE [Materials] SET [PartNumber] = '7440602', [Description] = '6" X 2" SADDLE, TAP SRV' WHERE [MaterialID] = 629;
UPDATE [Materials] SET [PartNumber] = 'W744061F', [Description] = 'Saddle, Tap  6" X 1 1/2" SRV' WHERE [MaterialID] = 630;
UPDATE [Materials] SET [PartNumber] = 'W744081F', [Description] = 'Saddle, Tap  8" X 1 1/2" SRV' WHERE [MaterialID] = 631;
UPDATE [Materials] SET [PartNumber] = 'W7441202', [Description] = 'Saddle, Tap 12" X 2" SRV' WHERE [MaterialID] = 632;
UPDATE [Materials] SET [PartNumber] = 'W7441602', [Description] = 'Saddle, Tap 16" X  2" SRV' WHERE [MaterialID] = 633;
UPDATE [Materials] SET [PartNumber] = 'W7441802', [Description] = 'Saddle, Tap 18" X  2" SRV' WHERE [MaterialID] = 634;
UPDATE [Materials] SET [PartNumber] = '7442F01', [Description] = '2 1/2" X 1" SADDLE, TAP SRV' WHERE [MaterialID] = 635;
UPDATE [Materials] SET [PartNumber] = 'W7700303', [Description] = 'Cap,  3" MJ' WHERE [MaterialID] = 636;
UPDATE [Materials] SET [PartNumber] = 'W7700604', [Description] = 'Cap,  6" X 4" MJ' WHERE [MaterialID] = 637;
UPDATE [Materials] SET [PartNumber] = 'W7751604', [Description] = 'Plug, 16" DI MJ W/4" Tap' WHERE [MaterialID] = 638;
UPDATE [Materials] SET [PartNumber] = 'W7771212', [Description] = 'Plug, 12" DI SJ' WHERE [MaterialID] = 639;
UPDATE [Materials] SET [PartNumber] = 'W7802424', [Description] = 'Plug, 24" DI RJ' WHERE [MaterialID] = 640;
UPDATE [Materials] SET [PartNumber] = 'W8011212', [Description] = 'Valve, 12" Gate MJ OL' WHERE [MaterialID] = 641;
UPDATE [Materials] SET [PartNumber] = 'W8051616', [Description] = 'Valve, 16" Tap MJ OL' WHERE [MaterialID] = 642;
UPDATE [Materials] SET [PartNumber] = 'W8090606', [Description] = 'Valve,  6" Gate FLG OL' WHERE [MaterialID] = 643;
UPDATE [Materials] SET [PartNumber] = 'W4170420', [Description] = 'Pipe,  4" PVC CL 200' WHERE [MaterialID] = 644;
UPDATE [Materials] SET [PartNumber] = 'W4170620', [Description] = 'Pipe,  6" PVC CL 200' WHERE [MaterialID] = 645;
UPDATE [Materials] SET [PartNumber] = 'W4170820', [Description] = 'Pipe,  8" PVC CL 200' WHERE [MaterialID] = 646;
UPDATE [Materials] SET [PartNumber] = 'W4171020', [Description] = 'Pipe, 10" PVC CL 200' WHERE [MaterialID] = 647;
UPDATE [Materials] SET [PartNumber] = 'W4171220', [Description] = 'Pipe, 12" PVC CL 200' WHERE [MaterialID] = 648;
UPDATE [Materials] SET [PartNumber] = '9001515', [Description] = '15" PIPE, PVC' WHERE [MaterialID] = 649;
UPDATE [Materials] SET [PartNumber] = 'W7070404', [Description] = 'WYE,  4" MJ' WHERE [MaterialID] = 650;
UPDATE [Materials] SET [PartNumber] = '9040804', [Description] = '8" X 4" TEE / WYE' WHERE [MaterialID] = 651;
UPDATE [Materials] SET [PartNumber] = 'W7070806', [Description] = 'WYE,  8" X 6" MJ' WHERE [MaterialID] = 652;
UPDATE [Materials] SET [PartNumber] = 'S9060404', [Description] = 'Bend,  4" PVC Sewer Gask 22 1/' WHERE [MaterialID] = 653;
UPDATE [Materials] SET [PartNumber] = 'S9060606', [Description] = 'Bend,  6" PVC Sewer Gask 22 1/' WHERE [MaterialID] = 654;
UPDATE [Materials] SET [PartNumber] = 'S9080404', [Description] = 'Bend,  4" PVC Sewer Gask 45°' WHERE [MaterialID] = 655;
UPDATE [Materials] SET [PartNumber] = 'S9080606', [Description] = 'Bend,  6" PVC Sewer Gask 45°' WHERE [MaterialID] = 656;
UPDATE [Materials] SET [PartNumber] = 'S9100404', [Description] = 'Bend,  4" PVC Sewer Gask 90°' WHERE [MaterialID] = 657;
UPDATE [Materials] SET [PartNumber] = 'S9120404', [Description] = 'Adapter,  4" PVC Sewer Screw/P' WHERE [MaterialID] = 658;
UPDATE [Materials] SET [PartNumber] = 'S9120606', [Description] = 'Adapter,  6" PVC Sewer Screw/P' WHERE [MaterialID] = 659;
UPDATE [Materials] SET [PartNumber] = 'S9220404', [Description] = 'Saddle,  4" X 4" PVC Sewer' WHERE [MaterialID] = 660;
UPDATE [Materials] SET [PartNumber] = 'S9220604', [Description] = 'Saddle,  6" X 4" PVC Sewer' WHERE [MaterialID] = 661;
UPDATE [Materials] SET [PartNumber] = 'S9220804', [Description] = 'Saddle,  8" X 4" PVC Sewer' WHERE [MaterialID] = 662;
UPDATE [Materials] SET [PartNumber] = 'S9220806', [Description] = 'Saddle,  8" X 6" PVC Sewer' WHERE [MaterialID] = 663;
UPDATE [Materials] SET [PartNumber] = 'S9221004', [Description] = 'Saddle, 10" X 4" PVC Sewer' WHERE [MaterialID] = 664;
UPDATE [Materials] SET [PartNumber] = 'S9221204', [Description] = 'Saddle, 12" X 4" PVC Sewer' WHERE [MaterialID] = 665;
UPDATE [Materials] SET [PartNumber] = 'S9230404', [Description] = 'Saddle,  4" WYE FLEX Sewer' WHERE [MaterialID] = 666;
UPDATE [Materials] SET [PartNumber] = 'S9240404', [Description] = 'Coupling,  4" Fernco Sewer' WHERE [MaterialID] = 667;
UPDATE [Materials] SET [PartNumber] = 'S9240606', [Description] = 'Coupling,  6" Fernco Sewer' WHERE [MaterialID] = 668;
UPDATE [Materials] SET [PartNumber] = 'S9240808', [Description] = 'Coupling,  8" Fernco Sewer' WHERE [MaterialID] = 669;
UPDATE [Materials] SET [PartNumber] = 'S9241010', [Description] = 'Coupling, 10" Fernco Sewer' WHERE [MaterialID] = 670;
UPDATE [Materials] SET [PartNumber] = 'S9241212', [Description] = 'Coupling, 12" Fernco Sewer' WHERE [MaterialID] = 671;
UPDATE [Materials] SET [PartNumber] = 'S9260404', [Description] = 'Coupling,  4" Fernco Clay X CI' WHERE [MaterialID] = 672;
UPDATE [Materials] SET [PartNumber] = 'S9260808', [Description] = 'Coupling,  8" Fernco Clay X CI' WHERE [MaterialID] = 673;
UPDATE [Materials] SET [PartNumber] = 'S9280604', [Description] = 'Coupling,  6" X 4"  Fernco ACX' WHERE [MaterialID] = 674;
UPDATE [Materials] SET [PartNumber] = 'S9280808', [Description] = 'Coupling,  8" Fernco ACXCI/PVC' WHERE [MaterialID] = 675;
UPDATE [Materials] SET [PartNumber] = 'S9300404', [Description] = 'Coupling,  4" Fernco CI/PVC' WHERE [MaterialID] = 676;
UPDATE [Materials] SET [PartNumber] = 'S9300808', [Description] = 'Coupling,  8" Fernco CI/PVC' WHERE [MaterialID] = 677;
UPDATE [Materials] SET [PartNumber] = 'W5183014', [Description] = 'Hydrant, Lake 3''6" Bury' WHERE [MaterialID] = 678;
UPDATE [Materials] SET [PartNumber] = 'W5183015', [Description] = 'Hydrant, Lake 4'' Bury' WHERE [MaterialID] = 679;
UPDATE [Materials] SET [PartNumber] = 'W3852020', [Description] = 'Gasket, 20" Field Lok SJ' WHERE [MaterialID] = 680;
UPDATE [Materials] SET [PartNumber] = 'W3852424', [Description] = 'Gasket, 24" Field Lok SJ' WHERE [MaterialID] = 681;
UPDATE [Materials] SET [PartNumber] = 'W3853030', [Description] = 'Gasket, 30" Field Lok SJ' WHERE [MaterialID] = 682;
UPDATE [Materials] SET [PartNumber] = 'W3853636', [Description] = 'Gasket, 36" Field Lok SJ' WHERE [MaterialID] = 683;

SET IDENTITY_INSERT [dbo].[Materials] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '684', 'S9000404', 'Pipe,  4" PVC Sewer' UNION ALL
SELECT '685', 'S9000606', 'Pipe,  6" PVC Sewer' UNION ALL
SELECT '686', 'S9000808', 'Pipe,  8" PVC Sewer' UNION ALL
SELECT '687', 'S9001010', 'Pipe, 10" PVC Sewer' UNION ALL
SELECT '688', 'S9001212', 'Pipe, 12" PVC Sewer' UNION ALL
SELECT '689', 'S9001515', 'Pipe, 15" PVC Sewer' UNION ALL
SELECT '690', 'S9012121', 'Pipe, 21" PVC Sewer Culvert' UNION ALL
SELECT '691', 'S9014242', 'Pipe, 42" PVC Sewer Culvert' UNION ALL
SELECT '692', 'S9040404', 'TEE/WYE,  4" Sewer' UNION ALL
SELECT '693', 'S9040604', 'TEE/WYE,  6" X 4" Sewer' UNION ALL
SELECT '694', 'S9040606', 'TEE/WYE,  6" Sewer' UNION ALL
SELECT '695', 'S9040804', 'TEE/WYE,  8" X 4" Sewer' UNION ALL
SELECT '696', 'S9040806', 'TEE/WYE,  8" X 6" Sewer' UNION ALL
SELECT '697', 'S9040808', 'TEE/WYE,  8" Sewer' UNION ALL
SELECT '698', 'S9041006', 'TEE/WYE, 10" X 6" Sewer' UNION ALL
SELECT '699', 'S9041206', 'TEE/WYE, 12" X 6" Sewer' UNION ALL
SELECT '700', 'S9060808', 'Bend,  8" PVC Sewer Gask 22 1/2°' UNION ALL
SELECT '701', 'S9070404', 'Bend,  4" PVC Sewer Glue 22 1/2°' UNION ALL
SELECT '702', 'S9080808', 'Bend,  8" PVC Sewer Gask 45°' UNION ALL
SELECT '703', 'S9090404', 'Bend,  4" PVC Sewer Glue 45°' UNION ALL
SELECT '704', 'S9100303', 'Bend,  3" PVC Sewer Gask 90°' UNION ALL
SELECT '705', 'S9100606', 'Bend,  6" PVC Sewer Gask 90°' UNION ALL
SELECT '706', 'S9100808', 'Bend,  8" PVC Sewer Gask 90°' UNION ALL
SELECT '707', 'S9110404', 'Bend,  4" PVC Sewer Glue 90°' UNION ALL
SELECT '708', 'S9140404', 'Cap,  4" PVC Sewer' UNION ALL
SELECT '709', 'S9140606', 'Cap,  6" PVC Sewer' UNION ALL
SELECT '710', 'S9140808', 'Cap,  8" PVC Sewer' UNION ALL
SELECT '711', 'S9160404', 'Plug,  4" PVC Sewer' UNION ALL
SELECT '712', 'S9160606', 'Plug,  6" PVC Sewer' UNION ALL
SELECT '713', 'S9180604', 'Coupling,  6" X  4" PVC Sewer SJ' UNION ALL
SELECT '714', 'S9180606', 'Coupling,  6" PVC Sewer SJ' UNION ALL
SELECT '715', 'S9180808', 'Coupling,  8" PVC Sewer SJ' UNION ALL
SELECT '716', 'S9181212', 'Coupling, 12" PVC Sewer SJ' UNION ALL
SELECT '717', 'S9200006', 'Saddle,  Multi X 6" Sewer' UNION ALL
SELECT '718', 'S9240202', 'Coupling,  2" Fernco Sewer' UNION ALL
SELECT '719', 'S9240604', 'Coupling,  6" X  4" Fernco Sewer' UNION ALL
SELECT '720', 'S9240806', 'Coupling,  8" X  6" Fernco Sewer' UNION ALL
SELECT '721', 'S9241515', 'Coupling, 15" Fernco Sewer' UNION ALL
SELECT '722', 'S9260406', 'Coupling,  4" X  6" Fernco Clay X CI/PVC' UNION ALL
SELECT '723', 'S9260604', 'Coupling,  6" X  4" Fernco Clay X CI/PVC' UNION ALL
SELECT '724', 'S9260606', 'Coupling,  6" Fernco Clay X CI/PVC' UNION ALL
SELECT '725', 'S9260804', 'Coupling,  8" X  4" Fernco Clay X CI/PVC' UNION ALL
SELECT '726', 'S9260806', 'Coupling,  8" X  6" Fernco Clay X CI/PVC' UNION ALL
SELECT '727', 'S9260808S', 'Coupling,  8" Fernco Clay X CI/PVC Shear' UNION ALL
SELECT '728', 'S9261010', 'Coupling, 10" Fernco Clay X CI/PVC' UNION ALL
SELECT '729', 'S9261010S', 'Coupling, 10" Fernco Clay X CI/PVC Shear' UNION ALL
SELECT '730', 'S9261212', 'Coupling, 12" Fernco Clay X CI/PVC' UNION ALL
SELECT '731', 'S9261212S', 'Coupling, 12" Fernco Clay X CI/PVC Shear' UNION ALL
SELECT '732', 'S9280404', 'Coupling,  4" Fernco ACXCI/PVC' UNION ALL
SELECT '733', 'S9280606', 'Coupling,  6" Fernco ACXCI/PVC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '734', 'S9280804', 'Coupling,  8" X  4" Fernco ACXCI/PVC' UNION ALL
SELECT '735', 'S9280806', 'Coupling,  8" X  6" Fernco ACXCI/PVC' UNION ALL
SELECT '736', 'S9280808S', 'Coupling,  8" Fernco ACXCI/PVC Shear' UNION ALL
SELECT '737', 'S9281010', 'Coupling, 10" Fernco ACXCI/PVC' UNION ALL
SELECT '738', 'S9281212', 'Coupling, 12" Fernco ACXCI/PVC' UNION ALL
SELECT '739', 'S9281212S', 'Coupling, 12" Fernco ACXCI/PVC Shear' UNION ALL
SELECT '740', 'S9300604', 'Coupling,  6" X  4" Fernco CI/PVC' UNION ALL
SELECT '741', 'S9300606', 'Coupling,  6" Fernco CI/PVC' UNION ALL
SELECT '742', 'S9300804', 'Coupling,  8" X  4" Fernco CI/PVC' UNION ALL
SELECT '743', 'S9300806', 'Coupling,  8" X  6" Fernco CI/PVC' UNION ALL
SELECT '744', 'S9300808S', 'Coupling,  8" Fernco CI/PVC Shear' UNION ALL
SELECT '745', 'S9301010', 'Coupling, 10" Fernco CI/PVC' UNION ALL
SELECT '746', 'S9301010S', 'Coupling, 10" Fernco CI/PVC Shear' UNION ALL
SELECT '747', 'S9301212', 'Coupling, 12" Fernco CI/PVC' UNION ALL
SELECT '748', 'S9301212S', 'Coupling, 12" Fernco CI/PVC Shear' UNION ALL
SELECT '749', 'S9301515', 'Coupling, 15" Fernco CI/PVC' UNION ALL
SELECT '750', 'W0010101', 'Corp, 1" Taper X MIP' UNION ALL
SELECT '751', 'W001010H', 'Corp, 1" X  3/4" Taper X MIP' UNION ALL
SELECT '752', 'W001011C', 'Corp, 1" X 1 1/4" Taper X MIP' UNION ALL
SELECT '753', 'W0010202', 'Corp, 2" Taper X MIP' UNION ALL
SELECT '754', 'W0010G0H', 'Corp,  5/8" X 3/4" Taper X MIP' UNION ALL
SELECT '755', 'W0010H01', 'Corp,  3/4" X 1" Taper X MIP' UNION ALL
SELECT '756', 'W0010H0H', 'Corp,  3/4" Taper X MIP' UNION ALL
SELECT '757', 'W0011C1C', 'Corp, 1 1/4" Taper X MIP' UNION ALL
SELECT '758', 'W0011F02', 'Corp, 1 1/2" X 2" Taper X MIP' UNION ALL
SELECT '759', 'W0011F1F', 'Corp, 1 1/2" Taper X MIP' UNION ALL
SELECT '760', 'W0020G0G', 'Corp,  5/8" Taper X CF' UNION ALL
SELECT '761', 'W0021C1C', 'Corp, 1 1/4" Taper X CF' UNION ALL
SELECT '762', 'W0021F1F', 'Corp, 1 1/2" Taper X CF' UNION ALL
SELECT '763', 'W0030101', 'Corp, 1" Taper X PC' UNION ALL
SELECT '764', 'W0030H0H', 'Corp,  3/4" Taper X PC' UNION ALL
SELECT '765', 'W0031C1C', 'Corp, 1 1/4" Taper X PC' UNION ALL
SELECT '766', 'W0040101', 'Corp, 1" Taper X FIP' UNION ALL
SELECT '767', 'W0040202', 'Corp, 2" Taper X FIP' UNION ALL
SELECT '768', 'W0040H01', 'Corp,  3/4" X 1" Taper X FIP' UNION ALL
SELECT '769', 'W0040H0H', 'Corp,  3/4" Taper X FIP' UNION ALL
SELECT '770', 'W0050101', 'Corp, 1" Taper X CC' UNION ALL
SELECT '771', 'W0050202', 'Corp, 2" Taper X CC' UNION ALL
SELECT '772', 'W0050G0G', 'Corp,  5/8" Taper X CC' UNION ALL
SELECT '773', 'W0050G0H', 'Corp,  5/8" X 3/4" Taper X CC' UNION ALL
SELECT '774', 'W0050H01', 'Corp,  3/4" X 1" Taper X CC' UNION ALL
SELECT '775', 'W0050H0H', 'Corp,  3/4" Taper X CC' UNION ALL
SELECT '776', 'W0051F1F', 'Corp, 1 1/2" Taper X CC' UNION ALL
SELECT '777', 'W0051F1F', 'Corp, 1 1/2" Taper X CC' UNION ALL
SELECT '778', 'W0060101', 'Corp, 1" Taper X SPT' UNION ALL
SELECT '779', 'W0060G0G', 'Corp,  5/8" Taper X SPT' UNION ALL
SELECT '780', 'W0060H0H', 'Corp,  3/4" Taper X SPT' UNION ALL
SELECT '781', 'W0070101', 'Corp, 1" MIP X MIP' UNION ALL
SELECT '782', 'W0070202', 'Corp, 2" MIP X MIP' UNION ALL
SELECT '783', 'W0070H0H', 'Corp,  3/4" MIP X MIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 2.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '784', 'W0071F1F', 'Corp, 1 1/2" MIP X MIP' UNION ALL
SELECT '785', 'W0080101', 'Corp, 1" MIP X PC' UNION ALL
SELECT '786', 'W0080H0H', 'Corp,  3/4" MIP X PC' UNION ALL
SELECT '787', 'W0090101', 'Corp, 1" X 1" MIP X CC' UNION ALL
SELECT '788', 'W0090H0H', 'Corp,  3/4" MIP X CC' UNION ALL
SELECT '789', 'W0091F1F', 'Corp, 1 1/2" X 1 1/2" MIP X CC' UNION ALL
SELECT '790', 'W0100101', 'Corp, 1" MIP X CF' UNION ALL
SELECT '791', 'W0100202', 'Corp, 2" MIP X CF' UNION ALL
SELECT '792', 'W0100H0H', 'Corp,  3/4" MIP X CF' UNION ALL
SELECT '793', 'W0101F1F', 'Corp, 1 1/2" MIP X CF' UNION ALL
SELECT '794', 'W0110101', 'Corp, 1" MIP X FIP' UNION ALL
SELECT '795', 'W0120202', 'Valve,  2" Ball Meter FIP X FLG' UNION ALL
SELECT '796', 'W0130202', 'Ball/Curb Stop, 2" MIP X CC' UNION ALL
SELECT '797', 'W0140101', 'Valve,  1" Ball Meter FIP X MC' UNION ALL
SELECT '798', 'W014010H', 'Valve,  1" X 3/4" Ball Meter FIP X MC' UNION ALL
SELECT '799', 'W0140202', 'Valve,  2" Ball Meter FIP X MC' UNION ALL
SELECT '800', 'W0140H0H', 'Valve,   3/4" Ball Meter FIP X MC' UNION ALL
SELECT '801', 'W0141F1F', 'Valve,  1 1/2" Ball Meter FIP X MC' UNION ALL
SELECT '802', 'W0150101', 'Valve,  1" BR Gate FIP X FIP' UNION ALL
SELECT '803', 'W0150202', 'Valve,  2" BR Gate FIP X FIP' UNION ALL
SELECT '804', 'W0150303', 'Valve,  3" BR Gate FIP X FIP' UNION ALL
SELECT '805', 'W0150F0F', 'Valve,   1/2" BR Gate FIP X FIP' UNION ALL
SELECT '806', 'W0150H0H', 'Valve,   3/4" BR Gate FIP X FIP' UNION ALL
SELECT '807', 'W0151C1C', 'Valve,  1 1/4" BR Gate FIP X FIP' UNION ALL
SELECT '808', 'W0151F1F', 'Valve,  1 1/2" BR Gate FIP X FIP' UNION ALL
SELECT '809', 'W0152F2F', 'Valve,  2 1/2" BR Gate FIP X FIP' UNION ALL
SELECT '810', 'W0160101', 'Ball/Curb Stop, 1" PC X FIP' UNION ALL
SELECT '811', 'W0160202', 'Ball/Curb Stop, 2" PC X FIP' UNION ALL
SELECT '812', 'W0160F0F', 'Ball/Curb Stop,  1/2" PC X FIP' UNION ALL
SELECT '813', 'W0161F1F', 'Ball/Curb Stop, 1 1/2" PC X FIP' UNION ALL
SELECT '814', 'W0170202', 'Ball/Curb Stop, 2" PC X PC' UNION ALL
SELECT '815', 'W0171C1C', 'Ball/Curb Stop, 1 1/4" PC X PC' UNION ALL
SELECT '816', 'W0180202', 'Ball/Curb Stop, 2" PC X CF' UNION ALL
SELECT '817', 'W0180H0H', 'Ball/Curb Stop,  3/4" PC X CF' UNION ALL
SELECT '818', 'W0190101', 'Ball/Curb Stop, 1" CF X MC' UNION ALL
SELECT '819', 'W019010H', 'Ball/Curb Stop, 1" X  3/4" CF X MC' UNION ALL
SELECT '820', 'W0190H0H', 'Ball/Curb Stop,  3/4" CF X MC' UNION ALL
SELECT '821', 'W0200101A', 'Ball/Curb Stop, 1" CF X CF 360°' UNION ALL
SELECT '822', 'W0200202M', 'Ball/Curb Stop, 2" CF X CF Minn' UNION ALL
SELECT '823', 'W0200H0HA', 'Ball/Curb Stop,  3/4" CF X CF 360°' UNION ALL
SELECT '824', 'W0201F1F', 'Ball/Curb Stop, 1 1/2" CF X CF' UNION ALL
SELECT '825', 'W0210101', 'Ball/Curb Stop, 1" CC X CC' UNION ALL
SELECT '826', 'W0210101A', 'Ball/Curb Stop, 1" CC X CC 360°' UNION ALL
SELECT '827', 'W0210101M', 'Ball/Curb Stop, 1" CC X CC Minn' UNION ALL
SELECT '828', 'W021010C', 'Ball/Curb Stop, 1" X  1/4" CC X CC' UNION ALL
SELECT '829', 'W021011C', 'Ball/Curb Stop, 1" X 1 1/4" CC X CC' UNION ALL
SELECT '830', 'W0210202', 'Ball/Curb Stop, 2" CC X CC' UNION ALL
SELECT '831', 'W021021C', 'Ball/Curb Stop, 2" X 1 1/4" CC X CC' UNION ALL
SELECT '832', 'W0210F0F', 'Ball/Curb Stop,  1/2" CC X CC' UNION ALL
SELECT '833', 'W0210H0H', 'Ball/Curb Stop,  3/4" CC X CC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 3.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '834', 'W0211C1C', 'Ball/Curb Stop, 1 1/4" CC X CC' UNION ALL
SELECT '835', 'W0211F1F', 'Ball/Curb Stop, 1 1/2" CC X CC' UNION ALL
SELECT '836', 'W0220202WH', 'Ball/Curb Stop, 2" FIP X FIP w/Handle' UNION ALL
SELECT '837', 'W022020HD', 'Ball/Curb Stop, 2" FIP X FIP w/Drain' UNION ALL
SELECT '838', 'W0220F0F', 'Ball/Curb Stop,  1/2" FIP X FIP' UNION ALL
SELECT '839', 'W0220H0HD', 'Ball/Curb Stop,  3/4" FIP X FIP w/Drain' UNION ALL
SELECT '840', 'W0221C1C', 'Ball/Curb Stop, 1 1/4" FIP X FIP' UNION ALL
SELECT '841', 'W0221C1CM', 'Ball/Curb Stop, 1 1/4" FIP X FIP Minn' UNION ALL
SELECT '842', 'W0221F1F', 'Ball/Curb Stop, 1 1/2" FIP X FIP' UNION ALL
SELECT '843', 'W0221F1FM', 'Ball/Curb Stop, 1 1/2" FIP X FIP Minn' UNION ALL
SELECT '844', 'W0230202', 'Ball/Curb Stop, 2" CF X FIP' UNION ALL
SELECT '845', 'W0231F1F', 'Ball/Curb Stop, 1 1/2" CF X FIP' UNION ALL
SELECT '846', 'W0240202', 'Ball/Curb Stop, 2" IPC X IPC' UNION ALL
SELECT '847', 'W0240H0H', 'Ball/Curb Stop,  3/4" IPC X IPC' UNION ALL
SELECT '848', 'W0241F1F', 'Ball/Curb Stop, 1 1/2" IPC X IPC' UNION ALL
SELECT '849', 'W0250101', 'Ball/Curb Stop, 1" CC X FIP' UNION ALL
SELECT '850', 'W0250202', 'Ball/Curb Stop, 2" CC X FIP' UNION ALL
SELECT '851', 'W0250H01', 'Ball/Curb Stop,  3/4" X 1" CC X FIP' UNION ALL
SELECT '852', 'W0250H0H', 'Ball/Curb Stop,  3/4" CC X FIP' UNION ALL
SELECT '853', 'W0250H0HD', 'Ball/Curb Stop,  3/4" CC X FIP w/Drain' UNION ALL
SELECT '854', 'W0251F1F', 'Ball/Curb Stop, 1 1/2" CC X FIP' UNION ALL
SELECT '855', 'W0260101', 'Valve,  1" Angle Meter CC' UNION ALL
SELECT '856', 'W026010H', 'Valve,  1" X 3/4" Angle Meter CC' UNION ALL
SELECT '857', 'W0260202', 'Valve,  2" Angle Meter CC' UNION ALL
SELECT '858', 'W0260G01', 'Valve,   5/8" X 1" Angle Meter CC' UNION ALL
SELECT '859', 'W0260G0H', 'Valve,   5/8" X  3/4" Angle Meter CC' UNION ALL
SELECT '860', 'W0260H0H', 'Valve,   3/4" Angle Meter CC' UNION ALL
SELECT '861', 'W0261F1F', 'Valve,  1 1/2" Angle Meter CC' UNION ALL
SELECT '862', 'W0270101', 'Valve,  1" Angle Meter IPC' UNION ALL
SELECT '863', 'W027010H', 'Valve,  1" X 3/4" Angle Meter IPC' UNION ALL
SELECT '864', 'W0270202', 'Valve,  2" Angle Meter IPC' UNION ALL
SELECT '865', 'W0271F1F', 'Valve,  1 1/2" Angle Meter IPC' UNION ALL
SELECT '866', 'W0280101', 'Valve,  1" Angle Meter FIP' UNION ALL
SELECT '867', 'W028010H', 'Valve,  1" X 3/4" Angle Meter FIP' UNION ALL
SELECT '868', 'W0280202', 'Valve,  2" Angle Meter FIP' UNION ALL
SELECT '869', 'W0280H0G', 'Valve,   3/4" X  5/8" Angle Meter FIP' UNION ALL
SELECT '870', 'W0280H0H', 'Valve,   3/4" Angle Meter FIP' UNION ALL
SELECT '871', 'W0290101', 'Valve,  1" Angle Meter CF' UNION ALL
SELECT '872', 'W029010H', 'Valve,  1" X 3/4" Angle Meter CF' UNION ALL
SELECT '873', 'W0290H0H', 'Valve,   3/4" Angle Meter CF' UNION ALL
SELECT '874', 'W0320101', 'Valve,  1" FLG Press Reduction' UNION ALL
SELECT '875', 'W0320202', 'Valve,  2" FLG Press Reduction' UNION ALL
SELECT '876', 'W0320404', 'Valve,  4" FLG Press Reduction' UNION ALL
SELECT '877', 'W0320H0H', 'Valve,   3/4" FLG Press Reduction' UNION ALL
SELECT '878', 'W0330101', 'Valve,  1" Check FLG' UNION ALL
SELECT '879', 'W0330200', 'Valve,  2" Check FLG' UNION ALL
SELECT '880', 'W03302CA', 'Valve,  2" Check 1/4" & 1/8" ports' UNION ALL
SELECT '881', 'W0330303', 'Valve,  3" Check FLG' UNION ALL
SELECT '882', 'W0330C0C', 'Valve,   1/4" Check FLG' UNION ALL
SELECT '883', 'W0330H0H', 'Valve,   3/4" Check FLG'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 4.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '884', 'W0331C1C', 'Valve,  1 1/4" Check FLG' UNION ALL
SELECT '885', 'W0331F1F', 'Valve,  1 1/2" Check FLG' UNION ALL
SELECT '886', 'W0340202', 'Valve,  2" Check PVC' UNION ALL
SELECT '887', 'W0350101', 'Valve,  1" Air Release' UNION ALL
SELECT '888', 'W0350202', 'Valve,  2" Air Release' UNION ALL
SELECT '889', 'W0350404', 'Valve,  4" Air Release' UNION ALL
SELECT '890', 'W0350606', 'Valve,  6" Air Release' UNION ALL
SELECT '891', 'W0350H0H', 'Valve,   3/4" Air Release' UNION ALL
SELECT '892', 'W0360101', 'Coupling,  1" IPC X CC' UNION ALL
SELECT '893', 'W0360103', 'Coupling,  1" X 3" IPC X CC' UNION ALL
SELECT '894', 'W0360105', 'Coupling,  1" X 5" IPC X CC' UNION ALL
SELECT '895', 'W036010H', 'Coupling,  1" X  3/4" IPC X CC' UNION ALL
SELECT '896', 'W0360205', 'Coupling,  2" X 5" IPC X CC' UNION ALL
SELECT '897', 'W0360F0H', 'Coupling,   1/2" X 3/4" IPC X CC' UNION ALL
SELECT '898', 'W0360H01', 'Coupling,   3/4" X 1" IPC X CC' UNION ALL
SELECT '899', 'W0360H05', 'Coupling,   3/4" X 5" IPC X CC' UNION ALL
SELECT '900', 'W0361F05', 'Coupling,  1 1/2" X 5" IPC X CC' UNION ALL
SELECT '901', 'W0370H0H', 'Valve,   3/4" Angle Meter PVCC X PC' UNION ALL
SELECT '902', 'W0380H0H', 'Coupling,   3/4" PVCC X PC' UNION ALL
SELECT '903', 'W0390G01', 'Coupling,   5/8" X 1" MP X CC' UNION ALL
SELECT '904', 'W0410H0F', 'Coupling,   3/4" X  1/2" PC X MIP' UNION ALL
SELECT '905', 'W0410H0G', 'Coupling,   3/4" X  5/8" PC X MIP' UNION ALL
SELECT '906', 'W0411C01', 'Coupling,  1 1/4" X 1" PC X MIP' UNION ALL
SELECT '907', 'W0411C1C', 'Coupling,  1 1/4" PC X MIP' UNION ALL
SELECT '908', 'W0411F1F', 'Coupling,  1 1/2" PC X MIP' UNION ALL
SELECT '909', 'W042010H', 'Coupling,  1" X  3/4" PC X FIP' UNION ALL
SELECT '910', 'W0420H01', 'Coupling,   3/4" X 1" PC X FIP' UNION ALL
SELECT '911', 'W0420H0F', 'Coupling,   3/4" X  1/2" PC X FIP' UNION ALL
SELECT '912', 'W0420H0G', 'Coupling,   3/4" X  5/8" PC X FIP' UNION ALL
SELECT '913', 'W0421C1C', 'Coupling,  1 1/4" PC X FIP' UNION ALL
SELECT '914', 'W0430H0F', 'Coupling,   3/4" X  1/2" PC X PC' UNION ALL
SELECT '915', 'W0430H0F', 'Coupling,   3/4" X  1/2" PC X PC' UNION ALL
SELECT '916', 'W0430H0G', 'Coupling,   3/4" X  5/8" PC X PC' UNION ALL
SELECT '917', 'W0431C1C', 'Coupling,  1 1/4" PC X PC' UNION ALL
SELECT '918', 'W0431F1F', 'Coupling,  1 1/2" PC X PC' UNION ALL
SELECT '919', 'W0432F2F', 'Coupling,  2 1/2" PC X PC' UNION ALL
SELECT '920', 'W0440101', 'Coupling,  1" MC X CC' UNION ALL
SELECT '921', 'W0440H01', 'Coupling,   3/4" X 1" MC X CC' UNION ALL
SELECT '922', 'W0440H0H', 'Coupling,   3/4" X  3/4" MC X CC' UNION ALL
SELECT '923', 'W045010H', 'Coupling,  1" X  3/4" PC X CF' UNION ALL
SELECT '924', 'W0450202', 'Coupling,  2" PC X CF' UNION ALL
SELECT '925', 'W0450H01', 'Coupling,   3/4" X 1" PC X CF' UNION ALL
SELECT '926', 'W0450H0F', 'Coupling,   3/4" X  1/2" PC X CF' UNION ALL
SELECT '927', 'W0450H0G', 'Coupling,   3/4" X  5/8" PC X CF' UNION ALL
SELECT '928', 'W0451C1C', 'Coupling,  1 1/4" PC X CF' UNION ALL
SELECT '929', 'W0451F1F', 'Coupling,  1 1/2" PC X CF' UNION ALL
SELECT '930', 'W0452F2F', 'Coupling,  2 1/2" PC X CF' UNION ALL
SELECT '931', 'W0460101', 'Coupling,  1" PC X IPC' UNION ALL
SELECT '932', 'W046010H', 'Coupling,  1" X  3/4" PC X IPC' UNION ALL
SELECT '933', 'W0460202', 'Coupling,  2" PC X IPC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 5.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '934', 'W0460H0F', 'Coupling,   3/4" X  1/2" PC X IPC' UNION ALL
SELECT '935', 'W0460H0G', 'Coupling,   3/4" X  5/8" PC X IPC' UNION ALL
SELECT '936', 'W0461C1C', 'Coupling,  1 1/4" PC X IPC' UNION ALL
SELECT '937', 'W0461F1F', 'Coupling,  1 1/2" PC X IPC' UNION ALL
SELECT '938', 'W0462F2F', 'Coupling,  2 1/2" PC X IPC' UNION ALL
SELECT '939', 'W047010H', 'Coupling,  1" X  3/4" PC X CC' UNION ALL
SELECT '940', 'W047010H', 'Coupling,  1" X  3/4" PC X CC' UNION ALL
SELECT '941', 'W0470202', 'Coupling,  2" PC X CC' UNION ALL
SELECT '942', 'W0470H01', 'Coupling,   3/4" X 1" PC X CC' UNION ALL
SELECT '943', 'W0470H0F', 'Coupling,   3/4" X  1/2" PC X CC' UNION ALL
SELECT '944', 'W0470H0G', 'Coupling,   3/4" X  5/8" PC X CC' UNION ALL
SELECT '945', 'W0471C1C', 'Coupling,  1 1/4" PC X CC' UNION ALL
SELECT '946', 'W0471F1F', 'Coupling,  1 1/2" PC X CC' UNION ALL
SELECT '947', 'W0472F2F', 'Coupling,  2 1/2" PC X CC' UNION ALL
SELECT '948', 'W048010H', 'Coupling,  1" X  3/4" CC X FIP' UNION ALL
SELECT '949', 'W048011C', 'Coupling,  1" X 1 1/4" CC X FIP' UNION ALL
SELECT '950', 'W0480F0F', 'Coupling,   1/2" CC X FIP' UNION ALL
SELECT '951', 'W0480F0H', 'Coupling,   1/2" X 3/4" CC X FIP' UNION ALL
SELECT '952', 'W0480G0H', 'Coupling,   5/8" X  3/4" CC X FIP' UNION ALL
SELECT '953', 'W0480H01', 'Coupling,   3/4" X 1" CC X FIP' UNION ALL
SELECT '954', 'W0480H0F', 'Coupling,   3/4" X  1/2" CC X FIP' UNION ALL
SELECT '955', 'W0480H0H', 'Coupling,   3/4" CC X FIP' UNION ALL
SELECT '956', 'W0481C1C', 'Coupling,  1 1/4" CC X FIP' UNION ALL
SELECT '957', 'W0481F1F', 'Coupling,  1 1/2" CC X FIP' UNION ALL
SELECT '958', 'W049010H', 'Coupling,  1" X  3/4" CC X MIP' UNION ALL
SELECT '959', 'W049021F', 'Coupling,  2" X 1 1/2" CC X MIP' UNION ALL
SELECT '960', 'W0490F0F', 'Coupling,   1/2" CC X MIP' UNION ALL
SELECT '961', 'W0490F0H', 'Coupling,   1/2" X 3/4" CC X MIP' UNION ALL
SELECT '962', 'W0490G0H', 'Coupling,   5/8" X  3/4" CC X MIP' UNION ALL
SELECT '963', 'W0490H01', 'Coupling,   3/4" X 1" CC X MIP' UNION ALL
SELECT '964', 'W0490H02', 'Coupling,   3/4" X 2" CC X MIP' UNION ALL
SELECT '965', 'W0490H0F', 'Coupling,   3/4" X  1/2" CC X MIP' UNION ALL
SELECT '966', 'W0490H0H', 'Coupling,   3/4" CC X MIP' UNION ALL
SELECT '967', 'W0491C01', 'Coupling,  1 1/4" X 1" CC X MIP' UNION ALL
SELECT '968', 'W0491C1C', 'Coupling,  1 1/4" CC X MIP' UNION ALL
SELECT '969', 'W0491F01', 'Coupling,  1 1/2" X 1" CC X MIP' UNION ALL
SELECT '970', 'W0491F1F', 'Coupling,  1 1/2" CC X MIP' UNION ALL
SELECT '971', 'W0500202', 'Coupling,  2" CF X CF 3PT UN' UNION ALL
SELECT '972', 'W0500H0F', 'Coupling,   3/4" X  1/2" CF X CF 3PT UN' UNION ALL
SELECT '973', 'W0501F1F', 'Coupling,  1 1/2" CF X CF 2 Part Union' UNION ALL
SELECT '974', 'W0510101', 'Coupling,  1" TNA X CC' UNION ALL
SELECT '975', 'W0510G0H', 'Coupling,   5/8" X  3/4" TNA X CC' UNION ALL
SELECT '976', 'W0510H01', 'Coupling,   3/4" X 1" TNA X CC' UNION ALL
SELECT '977', 'W0510H0H', 'Coupling,   3/4" TNA X CC' UNION ALL
SELECT '978', 'W0510H0H', 'Coupling,   3/4" X  3/4" TNA X CC' UNION ALL
SELECT '979', 'W0511C1C', 'Coupling,  1 1/4" TNA X CC' UNION ALL
SELECT '980', 'W0511F1F', 'Coupling,  1 1/2" TNA X CC' UNION ALL
SELECT '981', 'W052010H', 'Coupling,  1" X  3/4" CF X FIP' UNION ALL
SELECT '982', 'W0520202', 'Coupling,  2" CF X FIP' UNION ALL
SELECT '983', 'W0520F0F', 'Coupling,   1/2" CF X FIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 6.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '984', 'W0520H01', 'Coupling,   3/4" X 1" CF X FIP' UNION ALL
SELECT '985', 'W0520H0F', 'Coupling,   3/4" X  1/2" CF X FIP' UNION ALL
SELECT '986', 'W0521C1C', 'Coupling,  1 1/4" CF X FIP' UNION ALL
SELECT '987', 'W0521F1F', 'Coupling,  1 1/2" CF X FIP' UNION ALL
SELECT '988', 'W0522F2F', 'Coupling,  2 1/2" CF X FIP' UNION ALL
SELECT '989', 'W0530101', 'Coupling,  1" CF X MIP' UNION ALL
SELECT '990', 'W053010H', 'Coupling,  1" X  3/4" CF X MIP' UNION ALL
SELECT '991', 'W0530202', 'Coupling,  2" CF X MIP' UNION ALL
SELECT '992', 'W0530H0F', 'Coupling,   3/4" X  1/2" CF X MIP' UNION ALL
SELECT '993', 'W0530H0H', 'Coupling,   3/4" CF X MIP' UNION ALL
SELECT '994', 'W0531C1C', 'Coupling,  1 1/4" CF X MIP' UNION ALL
SELECT '995', 'W0531F1F', 'Coupling,  1 1/2" CF X MIP' UNION ALL
SELECT '996', 'W0532F2F', 'Coupling,  2 1/2" CF X MIP' UNION ALL
SELECT '997', 'W054010H', 'Coupling,  1" X  3/4" CF X CF' UNION ALL
SELECT '998', 'W0540H0F', 'Coupling,   3/4" X  1/2" CF X CF' UNION ALL
SELECT '999', 'W0541C1C', 'Coupling,  1 1/4" CF X CF' UNION ALL
SELECT '1000', 'W0541F1F', 'Coupling,  1 1/2" CF X CF' UNION ALL
SELECT '1001', 'W0542F2F', 'Coupling,  2 1/2" CF X CF' UNION ALL
SELECT '1002', 'W0550101', 'Coupling,  1" CF X LFA' UNION ALL
SELECT '1003', 'W055010G', 'Coupling,  1" X  5/8" CF X LFA' UNION ALL
SELECT '1004', 'W055010H', 'Coupling,  1" X  3/4" CF X LFA' UNION ALL
SELECT '1005', 'W0550202', 'Coupling,  2" CF X LFA' UNION ALL
SELECT '1006', 'W0550G0H', 'Coupling,   5/8" X  3/4" CF X LFA' UNION ALL
SELECT '1007', 'W0550H01', 'Coupling,   3/4" X 1" CF X LFA' UNION ALL
SELECT '1008', 'W0550H0G', 'Coupling,   3/4" X  5/8" CF X LFA' UNION ALL
SELECT '1009', 'W0550H0H', 'Coupling,   3/4" CF X LFA' UNION ALL
SELECT '1010', 'W0551C1C', 'Coupling,  1 1/4" CF X LFA' UNION ALL
SELECT '1011', 'W0551F1F', 'Coupling,  1 1/2" CF X LFA' UNION ALL
SELECT '1012', 'W0560101', 'Coupling,  1" CF X LC' UNION ALL
SELECT '1013', 'W0560202', 'Coupling,  2" CF X LC' UNION ALL
SELECT '1014', 'W0560H0G', 'Coupling,   3/4" X  5/8" CF X LC' UNION ALL
SELECT '1015', 'W0560H0H', 'Coupling,   3/4" CF X LC' UNION ALL
SELECT '1016', 'W0561C1C', 'Coupling,  1 1/4" CF X LC' UNION ALL
SELECT '1017', 'W0561F1F', 'Coupling,  1 1/2" CF X LC' UNION ALL
SELECT '1018', 'W0570101', 'Coupling,  1" CF X TNA' UNION ALL
SELECT '1019', 'W057010H', 'Coupling,  1" X  3/4" CF X TNA' UNION ALL
SELECT '1020', 'W0570202', 'Coupling,  2" CF X TNA' UNION ALL
SELECT '1021', 'W0570H01', 'Coupling,   3/4" X 1" CF X TNA' UNION ALL
SELECT '1022', 'W0570H0H', 'Coupling,   3/4" CF X TNA' UNION ALL
SELECT '1023', 'W0571C1C', 'Coupling,  1 1/4" CF X TNA' UNION ALL
SELECT '1024', 'W0571F1F', 'Coupling,  1 1/2" CF X TNA' UNION ALL
SELECT '1025', 'W0580H01', 'Coupling,   3/4" X 1" FIP X MP' UNION ALL
SELECT '1026', 'W0590101', 'Coupling,  1" CF X CF 2PT UN' UNION ALL
SELECT '1027', 'W0590202', 'Coupling,  2" CF X CF 2PT UN' UNION ALL
SELECT '1028', 'W0590H0F', 'Coupling,   3/4" X  1/2" CF X CF 2PT UN' UNION ALL
SELECT '1029', 'W0590H0H', 'Coupling,   3/4" CF X CF 2PT UN' UNION ALL
SELECT '1030', 'W0591C1C', 'Coupling,  1 1/4" CF X CF 2PT UN' UNION ALL
SELECT '1031', 'W0591F1F', 'Coupling,  1 1/2" CF X CF 2PT UN' UNION ALL
SELECT '1032', 'W0600101', 'Coupling,  1" MIP X LC' UNION ALL
SELECT '1033', 'W0600202', 'Coupling,  2" MIP X LC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 7.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1034', 'W0600H0G', 'Coupling,   3/4" X  5/8" MIP X LC' UNION ALL
SELECT '1035', 'W0600H0H', 'Coupling,   3/4" MIP X LC' UNION ALL
SELECT '1036', 'W0601C1C', 'Coupling,  1 1/4" MIP X LC' UNION ALL
SELECT '1037', 'W0601F1F', 'Coupling,  1 1/2" MIP X LC' UNION ALL
SELECT '1038', 'W0610101', 'Coupling,  1" LC X FIP' UNION ALL
SELECT '1039', 'W0610202', 'Coupling,  2" LC X FIP' UNION ALL
SELECT '1040', 'W0610H0H', 'Coupling,   3/4" LC X FIP' UNION ALL
SELECT '1041', 'W0611C1C', 'Coupling,  1 1/4" LC X FIP' UNION ALL
SELECT '1042', 'W0611F1F', 'Coupling,  1 1/2" LC X FIP' UNION ALL
SELECT '1043', 'W0620202', 'Coupling,  2" IPC X FIP' UNION ALL
SELECT '1044', 'W0620H0H', 'Coupling,   3/4" IPC X FIP' UNION ALL
SELECT '1045', 'W0621F1F', 'Coupling,  1 1/2" IPC X FIP' UNION ALL
SELECT '1046', 'W0630101', 'Coupling,  1" TNA X FIP' UNION ALL
SELECT '1047', 'W0630H0H', 'Coupling,   3/4" TNA X FIP' UNION ALL
SELECT '1048', 'W0631C1C', 'Coupling,  1 1/4" TNA X FIP' UNION ALL
SELECT '1049', 'W0631F1F', 'Coupling,  1 1/2" TNA X FIP' UNION ALL
SELECT '1050', 'W0650101', 'Coupling,  1" CF X CC' UNION ALL
SELECT '1051', 'W065011C', 'Coupling,  1" X 1 1/4" CF X CC' UNION ALL
SELECT '1052', 'W0650202', 'Coupling,  2" CF X CC' UNION ALL
SELECT '1053', 'W0650F0H', 'Coupling,   1/2" X 3/4" CF X CC' UNION ALL
SELECT '1054', 'W0650G0H', 'Coupling,   5/8" X  3/4" CF X CC' UNION ALL
SELECT '1055', 'W0650H01', 'Coupling,   3/4" X 1" CF X CC' UNION ALL
SELECT '1056', 'W0650H0H', 'Coupling,   3/4" CF X CC' UNION ALL
SELECT '1057', 'W0650H1C', 'Coupling,   3/4" X 1 1/4" CF X CC' UNION ALL
SELECT '1058', 'W0660101', 'Coupling,  1" LC X CC' UNION ALL
SELECT '1059', 'W0660202', 'Coupling,  2" LC X CC' UNION ALL
SELECT '1060', 'W0660G0G', 'Coupling,   5/8" LC X CC' UNION ALL
SELECT '1061', 'W0660G0H', 'Coupling,   5/8" X  3/4" LC X CC' UNION ALL
SELECT '1062', 'W0660H0H', 'Coupling,   3/4" LC X CC' UNION ALL
SELECT '1063', 'W0660H1F', 'Coupling,   3/4" X 1 1/2" LC X CC' UNION ALL
SELECT '1064', 'W0661C1F', 'Coupling,  1 1/4" X 1 1/2" LC X CC' UNION ALL
SELECT '1065', 'W0661F1F', 'Coupling,  1 1/2" LC X CC' UNION ALL
SELECT '1066', 'W068010H', 'Coupling,  1" X  3/4" TNA X PC' UNION ALL
SELECT '1067', 'W0680H01', 'Coupling,   3/4" X 1" TNA X PC' UNION ALL
SELECT '1068', 'W0680H0H', 'Coupling,   3/4" TNA X PC' UNION ALL
SELECT '1069', 'W0681F1F', 'Coupling,  1 1/2" TNA X PC' UNION ALL
SELECT '1070', 'W0690101', 'Coupling,  1" MIP X IPC' UNION ALL
SELECT '1071', 'W0690202', 'Coupling,  2" MIP X IPC' UNION ALL
SELECT '1072', 'W0690F0F', 'Coupling,   1/2" MIP X IPC' UNION ALL
SELECT '1073', 'W0690H0H', 'Coupling,   3/4" MIP X IPC' UNION ALL
SELECT '1074', 'W0691C1C', 'Coupling,  1 1/4" MIP X IPC' UNION ALL
SELECT '1075', 'W0700H0H', 'Coupling,   3/4" PVCC X MIP' UNION ALL
SELECT '1076', 'W0710202', 'Coupling,  2" PVCC X CC' UNION ALL
SELECT '1077', 'W0720101', 'Coupling,  1" CF X IPC' UNION ALL
SELECT '1078', 'W0720H01', 'Coupling,   3/4" X 1" CF X IPC' UNION ALL
SELECT '1079', 'W0720H0H', 'Coupling,   3/4" CF X IPC' UNION ALL
SELECT '1080', 'W0730101', 'Coupling,  1" IPC X IPC' UNION ALL
SELECT '1081', 'W0730202', 'Coupling,  2" IPC X IPC' UNION ALL
SELECT '1082', 'W0730F0F', 'Coupling,   1/2" IPC X IPC' UNION ALL
SELECT '1083', 'W0730G0G', 'Coupling,   5/8" IPC X IPC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 8.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1084', 'W0730H0H', 'Coupling,   3/4" IPC X IPC' UNION ALL
SELECT '1085', 'W0731C01', 'Coupling,  1 1/4" X 1" IPC X IPC' UNION ALL
SELECT '1086', 'W0731C1C', 'Coupling,  1 1/4" IPC X IPC' UNION ALL
SELECT '1087', 'W0731F1F', 'Coupling,  1 1/2" IPC X IPC' UNION ALL
SELECT '1088', 'W0740101', 'Coupling,  1" RIC X MIP' UNION ALL
SELECT '1089', 'W0740H0H', 'Coupling,   3/4" RIC X MIP' UNION ALL
SELECT '1090', 'W0760101', 'Coupling,  1" IPC X MIP' UNION ALL
SELECT '1091', 'W0760202', 'Coupling,  2" IPC X MIP' UNION ALL
SELECT '1092', 'W0760H0H', 'Coupling,   3/4" IPC X MIP' UNION ALL
SELECT '1093', 'W0761F1F', 'Coupling,  1 1/2" IPC X MIP' UNION ALL
SELECT '1094', 'W0770101', 'Coupling,  1" MC X MIP' UNION ALL
SELECT '1095', 'W0770102', 'Coupling,  1" X 2" MC X MIP' UNION ALL
SELECT '1096', 'W077011F', 'Coupling,  1" X 1 1/2" MC X MIP' UNION ALL
SELECT '1097', 'W077012F', 'Coupling,  1" X 2 1/2" MC X MIP' UNION ALL
SELECT '1098', 'W077012G', 'Coupling,  1" X 2 5/8" MC X MIP' UNION ALL
SELECT '1099', 'W0770202', 'Coupling,  2" MC X MIP' UNION ALL
SELECT '1100', 'W0770F0H', 'Coupling,   1/2" X 3/4" MC X MIP' UNION ALL
SELECT '1101', 'W0770G0F', 'Coupling,   5/8" X  1/2" MC X MIP' UNION ALL
SELECT '1102', 'W0770G0G', 'Coupling,   5/8" MC X MIP' UNION ALL
SELECT '1103', 'W0770G0H', 'Coupling,   5/8" X  3/4" MC X MIP' UNION ALL
SELECT '1104', 'W0770H02', 'Coupling,   3/4" X 2" MC X MIP' UNION ALL
SELECT '1105', 'W0770H03', 'Coupling,   3/4" X 3" MC X MIP' UNION ALL
SELECT '1106', 'W0770H0H', 'Coupling,   3/4" MC X MIP' UNION ALL
SELECT '1107', 'W0770H1F', 'Coupling,   3/4" X 1 1/2" MC X MIP' UNION ALL
SELECT '1108', 'W0770H1G', 'Coupling,   3/4" X 1 5/8" MC X MIP' UNION ALL
SELECT '1109', 'W0770H2C', 'Coupling,   3/4" X 2 1/4" MC X MIP' UNION ALL
SELECT '1110', 'W0770H2F', 'Coupling,   3/4" X 2 1/2" MC X MIP' UNION ALL
SELECT '1111', 'W0770H2G', 'Coupling,   3/4" X 2 5/8" MC X MIP' UNION ALL
SELECT '1112', 'W0770H2J', 'Coupling,   3/4" X 2 3/16" MC X MIP' UNION ALL
SELECT '1113', 'W0771F1F', 'Coupling,  1 1/2" MC X MIP' UNION ALL
SELECT '1114', 'W0780101', 'Bend,  1" Swivel TNA X CF 45°' UNION ALL
SELECT '1115', 'W0780F0H', 'Bend,   1/2" X  3/4" Swivel TNA X CF 45°' UNION ALL
SELECT '1116', 'W0780H0H', 'Bend,   3/4" X  3/4" Swivel TNA X CF 45°' UNION ALL
SELECT '1117', 'W079011C', 'Coupling,  1" X 1 1/4" CC X CC' UNION ALL
SELECT '1118', 'W079021F', 'Coupling,  2" X 1 1/2" CC X CC' UNION ALL
SELECT '1119', 'W0790F0H', 'Coupling,   1/2" X 3/4" CC X CC' UNION ALL
SELECT '1120', 'W0790F0H', 'Coupling,   1/2" X 3/4" CC X CC' UNION ALL
SELECT '1121', 'W0790H0F', 'Coupling,   3/4" X  1/2" CC X CC' UNION ALL
SELECT '1122', 'W0791C01', 'Coupling,  1 1/4" X 1" CC X CC' UNION ALL
SELECT '1123', 'W0791C1C', 'Coupling,  1 1/4" CC X CC' UNION ALL
SELECT '1124', 'W0791F01', 'Coupling,  1 1/2" X 1" CC X CC' UNION ALL
SELECT '1125', 'W0791F1F', 'Coupling,  1 1/2" CC X CC' UNION ALL
SELECT '1126', 'W0800101', 'Coupling,  1" BR FIP X FIP' UNION ALL
SELECT '1127', 'W0800202', 'Coupling,  2" BR FIP X FIP' UNION ALL
SELECT '1128', 'W0800H01', 'Coupling,   3/4" X 1" BR FIP X FIP' UNION ALL
SELECT '1129', 'W0800H0H', 'Coupling,   3/4" BR FIP X FIP' UNION ALL
SELECT '1130', 'W0801C1C', 'Coupling,  1 1/4" BR FIP X FIP' UNION ALL
SELECT '1131', 'W0801F1F', 'Coupling,  1 1/2" BR FIP X FIP' UNION ALL
SELECT '1132', 'W0802F2F', 'Coupling,  2 1/2" BR FIP X FIP' UNION ALL
SELECT '1133', 'W0810101', 'Coupling,  1" GALV FIP X FIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 9.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1134', 'W0810202', 'Coupling,  2" GALV FIP X FIP' UNION ALL
SELECT '1135', 'W0810303', 'Coupling,  3" GALV FIP X FIP' UNION ALL
SELECT '1136', 'W0810H0H', 'Coupling,   3/4" GALV FIP X FIP' UNION ALL
SELECT '1137', 'W0811F1F', 'Coupling,  1 1/2" GALV FIP X FIP' UNION ALL
SELECT '1138', 'W0812F2F', 'Coupling,  2 1/2" GALV FIP X FIP' UNION ALL
SELECT '1139', 'W082010H', 'Reducer,  1" X  3/4" BR' UNION ALL
SELECT '1140', 'W0820201', 'Reducer,  2" X 1" BR' UNION ALL
SELECT '1141', 'W082020H', 'Reducer,  2" X  3/4" BR' UNION ALL
SELECT '1142', 'W082021C', 'Reducer,  2" X 1 1/4" BR' UNION ALL
SELECT '1143', 'W082021F', 'Reducer,  2" X 1 1/2" BR' UNION ALL
SELECT '1144', 'W0820H0B', 'Reducer,   3/4" X 3/8" BR' UNION ALL
SELECT '1145', 'W0821C01', 'Reducer,  1 1/4" X 1" BR' UNION ALL
SELECT '1146', 'W0821C01', 'Reducer,  1 1/4" X 1" BR' UNION ALL
SELECT '1147', 'W0821C0H', 'Reducer,  1 1/4" X  3/4" BR' UNION ALL
SELECT '1148', 'W0821C0H', 'Reducer,  1 1/4" X  3/4" BR' UNION ALL
SELECT '1149', 'W0821F1C', 'Reducer,  1 1/2" X 1 1/4" BR' UNION ALL
SELECT '1150', 'W0822F1F', 'Reducer,  2 1/2" X 1 1/2" BR' UNION ALL
SELECT '1151', 'W0830101', 'Bend,  1" TNA X CF 90°' UNION ALL
SELECT '1152', 'W0830H0H', 'Bend,   3/4" X  3/4" TNA X CF 90°' UNION ALL
SELECT '1153', 'W0831F1F', 'Bend,  1 1/2" TNA X CF 90°' UNION ALL
SELECT '1154', 'W0840101', 'Bend,  1" X 1" TNA X CF 45°' UNION ALL
SELECT '1155', 'W084010H', 'Bend,  1" X  3/4" TNA X CF 45°' UNION ALL
SELECT '1156', 'W0840202', 'Bend,  2" TNA X CF 45°' UNION ALL
SELECT '1157', 'W0840G0H', 'Bend,   5/8" X 3/4" TNA X CF 45°' UNION ALL
SELECT '1158', 'W0840H01', 'Bend,   3/4" X 1" TNA X CF 45°' UNION ALL
SELECT '1159', 'W0840H01', 'Bend,   3/4" X 1" TNA X CF 45°' UNION ALL
SELECT '1160', 'W0840H0H', 'Bend,   3/4" X  3/4" TNA X CF 45°' UNION ALL
SELECT '1161', 'W0841F1F', 'Bend,  1 1/2" TNA X CF 45°' UNION ALL
SELECT '1162', 'W0850101', 'Bend,  1" Swivel CF X TNA 90°' UNION ALL
SELECT '1163', 'W0850202', 'Bend,  2" Swivel CF X TNA 90°' UNION ALL
SELECT '1164', 'W0850F0H', 'Bend,   1/2" X  3/4" Corp Swivel TNA X CF 90°' UNION ALL
SELECT '1165', 'W0850H0H', 'Bend,   3/4" X  3/4" Swivel TNA X CF 90°' UNION ALL
SELECT '1166', 'W0860H0H', 'Coupling,   3/4" Swivel TNA X FIP 90°' UNION ALL
SELECT '1167', 'W0870101', 'Coupling,  1" MC X FIP' UNION ALL
SELECT '1168', 'W0870H0H', 'Coupling,   3/4" MC X FIP' UNION ALL
SELECT '1169', 'W0880201', 'Reducer,  2" X 1" GALV' UNION ALL
SELECT '1170', 'W088021C', 'Reducer,  2" X 1 1/4" GALV' UNION ALL
SELECT '1171', 'W0881F1C', 'Reducer,  1 1/2" X 1 1/4" GALV' UNION ALL
SELECT '1172', 'W0890101', 'Bend,  1" TNA X PC 90°.' UNION ALL
SELECT '1173', 'W091010H', 'UBR, 1" X 3/4" CF X MIP' UNION ALL
SELECT '1174', 'W0920101', 'YBR, 1" X 1" MIP X CC 2WY' UNION ALL
SELECT '1175', 'W092010H', 'YBR, 1" X  3/4" MIP X CC 2WY' UNION ALL
SELECT '1176', 'W0920201', 'YBR, 2" X 1" MIP X CC 2WY' UNION ALL
SELECT '1177', 'W0920H0H', 'YBR,  3/4" MIP X CC 2WY' UNION ALL
SELECT '1178', 'W0930201', 'YBR, 2" X 1" FIP X CC 3WY' UNION ALL
SELECT '1179', 'W0940101', 'YBR, 1" X 1 " CF X CF 2WY' UNION ALL
SELECT '1180', 'W094010H', 'YBR, 1" X  3/4" CF X CF 2WY' UNION ALL
SELECT '1181', 'W0941F01', 'YBR, 1 1/2" X 1 " CF X CF 2WY' UNION ALL
SELECT '1182', 'W0941F1F', 'YBR, 1 1/2" X 1 1/2" CF X CF 2WY' UNION ALL
SELECT '1183', 'W095010H', 'YBR, 1" X  3/4" CF X CF 3WY'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 10.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1184', 'W0950201', 'YBR, 2" X 1" CF X CF 3WY' UNION ALL
SELECT '1185', 'W0950H0H', 'YBR,  3/4" CF X CF 3WY' UNION ALL
SELECT '1186', 'W0951F01', 'YBR, 1 1/2" X 1" CF X CF 3WY' UNION ALL
SELECT '1187', 'W0951F1F', 'YBR, 1 1/2" X 1 1/2" CF X CF 3WY' UNION ALL
SELECT '1188', 'W0960102', 'YBR, 1" X 2" CC X CC 2WY' UNION ALL
SELECT '1189', 'W0960201', 'YBR, 2" X 1" CC X CC 2WY' UNION ALL
SELECT '1190', 'W0960H01', 'YBR,  3/4" X 1" CC X CC 2WY' UNION ALL
SELECT '1191', 'W0960H0H', 'YBR,  3/4" CC X CC 2WY' UNION ALL
SELECT '1192', 'W0970101', 'YBR, 1" CC X CC 3WY' UNION ALL
SELECT '1193', 'W097010H', 'YBR, 1" X  3/4" CC X CC 3WY' UNION ALL
SELECT '1194', 'W0970201', 'YBR, 2" X 1" CC X CC 3WY' UNION ALL
SELECT '1195', 'W097020F', 'YBR, 2" X  1/2" CC X CC 3WY' UNION ALL
SELECT '1196', 'W0970H0H', 'YBR,  3/4" CC X CC 3WY' UNION ALL
SELECT '1197', 'W0971F01', 'YBR, 1 1/2" X 1" CC X CC 3WY' UNION ALL
SELECT '1198', 'W0980201', 'YBR, 2" X 1" MIP X CC 3WY' UNION ALL
SELECT '1199', 'W0981F01', 'YBR, 1 1/2" X 1" MIP X CC 3WY' UNION ALL
SELECT '1200', 'W100010H', 'YBR, 1" X  3/4" MIP X CF 2WY' UNION ALL
SELECT '1201', 'W1000201', 'YBR, 2" X 1" MIP X CF 2WY' UNION ALL
SELECT '1202', 'W101010G', 'UBR, 1" X 5/8" CC X MIP' UNION ALL
SELECT '1203', 'W101010H', 'UBR, 1" X 3/4" CC X MIP' UNION ALL
SELECT '1204', 'W1010H0H', 'UBR,  3/4" CC X MIP' UNION ALL
SELECT '1205', 'W1030201', 'YBR, 2" X 1" FIP X FIP 4WY' UNION ALL
SELECT '1206', 'W104010H', 'YBR, 1" X  3/4" MIP X PC 3WY' UNION ALL
SELECT '1207', 'W1050100', 'Nipple, 1" CL GALV' UNION ALL
SELECT '1208', 'W1050200', 'Nipple, 2" CL GALV' UNION ALL
SELECT '1209', 'W1050203', 'Nipple, 2" X  3" GALV' UNION ALL
SELECT '1210', 'W1050204', 'Nipple, 2" X  4" GALV' UNION ALL
SELECT '1211', 'W1050206', 'Nipple, 2" X  6" GALV' UNION ALL
SELECT '1212', 'W1050209', 'Nipple, 2" X  9" GALV' UNION ALL
SELECT '1213', 'W1050212', 'Nipple, 2" X 12" GALV' UNION ALL
SELECT '1214', 'W1050218', 'Nipple, 2" X 18" GALV' UNION ALL
SELECT '1215', 'W1050236', 'Nipple, 2" X 36" GALV' UNION ALL
SELECT '1216', 'W1050300', 'Nipple, 3" CL GALV' UNION ALL
SELECT '1217', 'W1050306', 'Nipple, 3" X  6" GALV' UNION ALL
SELECT '1218', 'W105034F', 'Nipple, 3" X  4 1/2" GALV' UNION ALL
SELECT '1219', 'W1050406', 'Nipple, 4" X  6" GALV' UNION ALL
SELECT '1220', 'W1050410', 'Nipple, 4" X 10" GALV' UNION ALL
SELECT '1221', 'W1050F00', 'Nipple,  1/2" CL GALV' UNION ALL
SELECT '1222', 'W1050H00', 'Nipple,  3/4" CL GALV' UNION ALL
SELECT '1223', 'W1051C00', 'Nipple, 1 1/4" CL GALV' UNION ALL
SELECT '1224', 'W1051C03', 'Nipple, 1 1/4" X  3" GALV' UNION ALL
SELECT '1225', 'W1051F00', 'Nipple, 1 1/2" CL GALV' UNION ALL
SELECT '1226', 'W1051F03', 'Nipple, 1 1/2" X  3" GALV' UNION ALL
SELECT '1227', 'W1051F06', 'Nipple, 1 1/2" X  6" GALV' UNION ALL
SELECT '1228', 'W1051F4F', 'Nipple, 1 1/2" X  4 1/2" GALV' UNION ALL
SELECT '1229', 'W1051F5F', 'Nipple, 1 1/2" X  5 1/2" GALV' UNION ALL
SELECT '1230', 'W1052F00', 'Nipple, 2 1/2" CL GALV' UNION ALL
SELECT '1231', 'W1060100', 'Nipple, 1" CL BR' UNION ALL
SELECT '1232', 'W1060101', 'Nipple, 1" X  1" BR' UNION ALL
SELECT '1233', 'W1060102', 'Nipple, 1" X  2" BR'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 11.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1234', 'W1060103', 'Nipple, 1" X  3" BR' UNION ALL
SELECT '1235', 'W1060104', 'Nipple, 1" X  4" BR' UNION ALL
SELECT '1236', 'W1060105', 'Nipple, 1" X  5" BR' UNION ALL
SELECT '1237', 'W1060106', 'Nipple, 1" X  6" BR' UNION ALL
SELECT '1238', 'W1060107', 'Nipple, 1" X  7" BR' UNION ALL
SELECT '1239', 'W1060108', 'Nipple, 1" X  8" BR' UNION ALL
SELECT '1240', 'W1060109', 'Nipple, 1" X  9" BR' UNION ALL
SELECT '1241', 'W1060110', 'Nipple, 1" X 10" BR' UNION ALL
SELECT '1242', 'W1060111', 'Nipple, 1" X 11" BR' UNION ALL
SELECT '1243', 'W1060112', 'Nipple, 1" X 12" BR' UNION ALL
SELECT '1244', 'W1060114', 'Nipple, 1" X 14" BR' UNION ALL
SELECT '1245', 'W106012F', 'Nipple, 1" X  2 1/2" BR' UNION ALL
SELECT '1246', 'W1060200', 'Nipple, 2" CL BR' UNION ALL
SELECT '1247', 'W1060202', 'Nipple, 2" X  2" BR' UNION ALL
SELECT '1248', 'W1060203', 'Nipple, 2" X  3" BR' UNION ALL
SELECT '1249', 'W1060204', 'Nipple, 2" X  4" BR' UNION ALL
SELECT '1250', 'W1060205', 'Nipple, 2" X  5" BR' UNION ALL
SELECT '1251', 'W1060206', 'Nipple, 2" X  6" BR' UNION ALL
SELECT '1252', 'W1060207', 'Nipple, 2" X  7" BR' UNION ALL
SELECT '1253', 'W1060208', 'Nipple, 2" X  8" BR' UNION ALL
SELECT '1254', 'W1060209', 'Nipple, 2" X  9" BR' UNION ALL
SELECT '1255', 'W1060210', 'Nipple, 2" X 10" BR' UNION ALL
SELECT '1256', 'W1060211', 'Nipple, 2" X 11" BR' UNION ALL
SELECT '1257', 'W1060212', 'Nipple, 2" X 12" BR' UNION ALL
SELECT '1258', 'W1060214', 'Nipple, 2" X 14" BR' UNION ALL
SELECT '1259', 'W1060215', 'Nipple, 2" X 15" BR' UNION ALL
SELECT '1260', 'W1060216', 'Nipple, 2" X 16" BR' UNION ALL
SELECT '1261', 'W1060218', 'Nipple, 2" X 18" BR' UNION ALL
SELECT '1262', 'W1060224', 'Nipple, 2" X 24" BR' UNION ALL
SELECT '1263', 'W106022F', 'Nipple, 2" X  2 1/2" BR' UNION ALL
SELECT '1264', 'W1060230', 'Nipple, 2" X 30" BR' UNION ALL
SELECT '1265', 'W1060236', 'Nipple, 2" X 36" BR' UNION ALL
SELECT '1266', 'W106023F', 'Nipple, 2" X  3 1/2" BR' UNION ALL
SELECT '1267', 'W1060300', 'Nipple, 3" CL BR' UNION ALL
SELECT '1268', 'W1060303', 'Nipple, 3" X  3" BR' UNION ALL
SELECT '1269', 'W1060304', 'Nipple, 3" X  4" BR' UNION ALL
SELECT '1270', 'W1060306', 'Nipple, 3" X  6" BR' UNION ALL
SELECT '1271', 'W1060406', 'Nipple, 4" X  6" BR' UNION ALL
SELECT '1272', 'W1060C06', 'Nipple,  1/4" X  6" BR' UNION ALL
SELECT '1273', 'W1060F00', 'Nipple,  1/2" CL BR' UNION ALL
SELECT '1274', 'W1060F02', 'Nipple,  1/2" X  2" BR' UNION ALL
SELECT '1275', 'W1060F03', 'Nipple,  1/2" X  3" BR' UNION ALL
SELECT '1276', 'W1060F04', 'Nipple,  1/2" X  4" BR' UNION ALL
SELECT '1277', 'W1060F05', 'Nipple,  1/2" X  5" BR' UNION ALL
SELECT '1278', 'W1060F06', 'Nipple,  1/2" X  6" BR' UNION ALL
SELECT '1279', 'W1060F0H', 'Nipple,  1/2" X   3/4" BR' UNION ALL
SELECT '1280', 'W1060F2F', 'Nipple,  1/2" X  2 1/2" BR' UNION ALL
SELECT '1281', 'W1060H00', 'Nipple,  3/4" CL BR' UNION ALL
SELECT '1282', 'W1060H01', 'Nipple,  3/4" X  1" BR' UNION ALL
SELECT '1283', 'W1060H02', 'Nipple,  3/4" X  2" BR'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 12.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1284', 'W1060H03', 'Nipple,  3/4" X  3" BR' UNION ALL
SELECT '1285', 'W1060H04', 'Nipple,  3/4" X  4" BR' UNION ALL
SELECT '1286', 'W1060H05', 'Nipple,  3/4" X  5" BR' UNION ALL
SELECT '1287', 'W1060H06', 'Nipple,  3/4" X  6" BR' UNION ALL
SELECT '1288', 'W1060H07', 'Nipple,  3/4" X  7" BR' UNION ALL
SELECT '1289', 'W1060H08', 'Nipple,  3/4" X  8" BR' UNION ALL
SELECT '1290', 'W1060H09', 'Nipple,  3/4" X  9" BR' UNION ALL
SELECT '1291', 'W1060H0H', 'Nipple,  3/4" X   3/4" BR' UNION ALL
SELECT '1292', 'W1060H10', 'Nipple,  3/4" X 10" BR' UNION ALL
SELECT '1293', 'W1060H11', 'Nipple,  3/4" X 11" BR' UNION ALL
SELECT '1294', 'W1060H12', 'Nipple,  3/4" X 12" BR' UNION ALL
SELECT '1295', 'W1060H13', 'Nipple,  3/4" X 13" BR' UNION ALL
SELECT '1296', 'W1060H1F', 'Nipple,  3/4" X  1 1/2" BR' UNION ALL
SELECT '1297', 'W1060H2F', 'Nipple,  3/4" X  2 1/2" BR' UNION ALL
SELECT '1298', 'W1060H3F', 'Nipple,  3/4" X  3 1/2" BR' UNION ALL
SELECT '1299', 'W1060H4F', 'Nipple,  3/4" X  4 1/2" BR' UNION ALL
SELECT '1300', 'W1060H5F', 'Nipple,  3/4" X  5 1/2" BR' UNION ALL
SELECT '1301', 'W1061C00', 'Nipple, 1 1/4" CL BR' UNION ALL
SELECT '1302', 'W1061C02', 'Nipple, 1 1/4" X  2" BR' UNION ALL
SELECT '1303', 'W1061C03', 'Nipple, 1 1/4" X  3" BR' UNION ALL
SELECT '1304', 'W1061C04', 'Nipple, 1 1/4" X  4" BR' UNION ALL
SELECT '1305', 'W1061C05', 'Nipple, 1 1/4" X  5" BR' UNION ALL
SELECT '1306', 'W1061C06', 'Nipple, 1 1/4" X  6" BR' UNION ALL
SELECT '1307', 'W1061C1C', 'Nipple, 1 1/4" X  1 1/4" BR' UNION ALL
SELECT '1308', 'W1061F00', 'Nipple, 1 1/2" CL BR' UNION ALL
SELECT '1309', 'W1061F00', 'Nipple, 1 1/2" CL BR' UNION ALL
SELECT '1310', 'W1061F02', 'Nipple, 1 1/2" X  2" BR' UNION ALL
SELECT '1311', 'W1061F03', 'Nipple, 1 1/2" X  3" BR' UNION ALL
SELECT '1312', 'W1061F04', 'Nipple, 1 1/2" X  4" BR' UNION ALL
SELECT '1313', 'W1061F05', 'Nipple, 1 1/2" X  5" BR' UNION ALL
SELECT '1314', 'W1061F06', 'Nipple, 1 1/2" X  6" BR' UNION ALL
SELECT '1315', 'W1061F07', 'Nipple, 1 1/2" X  7" BR' UNION ALL
SELECT '1316', 'W1061F08', 'Nipple, 1 1/2" X  8" BR' UNION ALL
SELECT '1317', 'W1061F09', 'Nipple, 1 1/2" X  9" BR' UNION ALL
SELECT '1318', 'W1061F10', 'Nipple, 1 1/2" X 10" BR' UNION ALL
SELECT '1319', 'W1061F11', 'Nipple, 1 1/2" X 11" BR' UNION ALL
SELECT '1320', 'W1061F12', 'Nipple, 1 1/2" X 12" BR' UNION ALL
SELECT '1321', 'W1061F13', 'Nipple, 1 1/2" X 13" BR' UNION ALL
SELECT '1322', 'W1061F14', 'Nipple, 1 1/2" X 14" BR' UNION ALL
SELECT '1323', 'W1061F18', 'Nipple, 1 1/2" X 18" BR' UNION ALL
SELECT '1324', 'W1061F1F', 'Nipple, 1 1/2" X  1 1/2" BR' UNION ALL
SELECT '1325', 'W1061F24', 'Nipple, 1 1/2" X 24" BR' UNION ALL
SELECT '1326', 'W1061F2F', 'Nipple, 1 1/2" X  2 1/2" BR' UNION ALL
SELECT '1327', 'W1062F00', 'Nipple, 2 1/2" CL BR' UNION ALL
SELECT '1328', 'W1062F06', 'Nipple, 2 1/2" X  6" BR' UNION ALL
SELECT '1329', 'W1062F12', 'Nipple, 2 1/2" X 12" BR' UNION ALL
SELECT '1330', 'W1062F2F', 'Nipple, 2 1/2" X  2 1/2" BR' UNION ALL
SELECT '1331', 'W1070112', 'Nipple, 1" X 12" PVC' UNION ALL
SELECT '1332', 'W1070212', 'Nipple, 2" X 12" PVC' UNION ALL
SELECT '1333', 'W1070312', 'Nipple, 3" X 12" PVC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 13.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1334', 'W1070402', 'Nipple, 4" X  2" PVC' UNION ALL
SELECT '1335', 'W1070412', 'Nipple, 4" X 12" PVC' UNION ALL
SELECT '1336', 'W1070F00', 'Nipple,  1/2" PVC' UNION ALL
SELECT '1337', 'W1070H08', 'Nipple,  3/4" X  8" PVC' UNION ALL
SELECT '1338', 'W1070H12', 'Nipple,  3/4" X 12" PVC' UNION ALL
SELECT '1339', 'W1071C12', 'Nipple, 1 1/4" X 12" PVC' UNION ALL
SELECT '1340', 'W1071F12', 'Nipple, 1 1/2" X 12" PVC' UNION ALL
SELECT '1341', 'W1080101', 'Bend,  1" CF for Iron Yoke' UNION ALL
SELECT '1342', 'W1080H01', 'Bend,   3/4" X 1" CF for Iron Yoke' UNION ALL
SELECT '1343', 'W1090101', 'Union, 1" BR' UNION ALL
SELECT '1344', 'W1090202', 'Union, 2" BR' UNION ALL
SELECT '1345', 'W1090F0F', 'Union,  1/2" BR' UNION ALL
SELECT '1346', 'W1090H01', 'Union,  3/4" X 1" BR' UNION ALL
SELECT '1347', 'W1090H0H', 'Union,  3/4" BR' UNION ALL
SELECT '1348', 'W1091C1C', 'Union, 1 1/4" BR' UNION ALL
SELECT '1349', 'W1091F1F', 'Union, 1 1/2" BR' UNION ALL
SELECT '1350', 'W1100404', 'Union, 4" GALV' UNION ALL
SELECT '1351', 'W1101C1C', 'Union, 1 1/4" GALV' UNION ALL
SELECT '1352', 'W1102F2F', 'Union, 2 1/2" GALV' UNION ALL
SELECT '1353', 'W1110100', 'Bushing, 1" BR' UNION ALL
SELECT '1354', 'W111010C', 'Bushing, 1" X 1/4" BR' UNION ALL
SELECT '1355', 'W111010F', 'Bushing, 1" X 1/2" BR' UNION ALL
SELECT '1356', 'W111010H', 'Bushing, 1" X 3/4" BR' UNION ALL
SELECT '1357', 'W1110201', 'Bushing, 2" X 1" BR' UNION ALL
SELECT '1358', 'W111020H', 'Bushing, 2" X  3/4" BR' UNION ALL
SELECT '1359', 'W111021C', 'Bushing, 2" X 1 1/4" BR' UNION ALL
SELECT '1360', 'W111021F', 'Bushing, 2" X 1 1/2" BR' UNION ALL
SELECT '1361', 'W1110402', 'Bushing, 4" X 2" BR' UNION ALL
SELECT '1362', 'W1110F0H', 'Bushing,  1/2" X 3/4" BR' UNION ALL
SELECT '1363', 'W1110H0C', 'Bushing,  3/4" X 1/4" BR' UNION ALL
SELECT '1364', 'W1110H0F', 'Bushing,  3/4" X 1/2" BR' UNION ALL
SELECT '1365', 'W1111C00', 'Bushing, 1 1/4" BR' UNION ALL
SELECT '1366', 'W1111C01', 'Bushing, 1 1/4" X 1" BR' UNION ALL
SELECT '1367', 'W1111C0H', 'Bushing, 1 1/4" X  3/4"  BR' UNION ALL
SELECT '1368', 'W1111F00', 'Bushing, 1 1/2" BR' UNION ALL
SELECT '1369', 'W1111F01', 'Bushing, 1 1/2" X 1" BR' UNION ALL
SELECT '1370', 'W1111F0F', 'Bushing, 1 1/2" X  1/2" BR' UNION ALL
SELECT '1371', 'W1111F0H', 'Bushing, 1 1/2" X  3/4" BR' UNION ALL
SELECT '1372', 'W1111F1C', 'Bushing, 1 1/2" X 1 1/4" BR' UNION ALL
SELECT '1373', 'W1112F02', 'Bushing, 2 1/2" X 2" BR' UNION ALL
SELECT '1374', 'W1120201', 'Bushing, 2" X 1" GALV' UNION ALL
SELECT '1375', 'W112021F', 'Bushing, 2" X 1 1/2" GALV' UNION ALL
SELECT '1376', 'W1120302', 'Bushing, 3" X 2" GALV' UNION ALL
SELECT '1377', 'W1120402', 'Bushing, 4" X 2" GALV' UNION ALL
SELECT '1378', 'W1120403', 'Bushing, 4" X 3" GALV' UNION ALL
SELECT '1379', 'W1122F01', 'Bushing, 2 1/2" X 1" GALV' UNION ALL
SELECT '1380', 'W1122F1F', 'Bushing, 2 1/2" X 1 1/2" GALV' UNION ALL
SELECT '1381', 'W1130101', 'Plug,  1" GALV MIP' UNION ALL
SELECT '1382', 'W1130202', 'Plug,  2" GALV MIP' UNION ALL
SELECT '1383', 'W1130303', 'Plug,  3" GALV MIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 14.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1384', 'W1130404', 'Plug,  4" GALV MIP' UNION ALL
SELECT '1385', 'W1130606', 'Plug,  6" GALV MIP' UNION ALL
SELECT '1386', 'W1131C1C', 'Plug,  1 1/4" GALV MIP' UNION ALL
SELECT '1387', 'W1131F1F', 'Plug,  1 1/2" GALV MIP' UNION ALL
SELECT '1388', 'W1132F2F', 'Plug,  2 1/2" GALV MIP' UNION ALL
SELECT '1389', 'W1133F3F', 'Plug,  3 1/2" GALV MIP' UNION ALL
SELECT '1390', 'W114010H', 'UBR, 1" X 3/4" PC X MIP' UNION ALL
SELECT '1391', 'W1150101', 'Plug,  1" Insert BR Taper Thread' UNION ALL
SELECT '1392', 'W1150G0G', 'Plug,   5/8" Insert BR Taper Thread' UNION ALL
SELECT '1393', 'W1150H0H', 'Plug,   3/4" Insert BR Taper Thread' UNION ALL
SELECT '1394', 'W1160101', 'Plug,  1" BR CORP' UNION ALL
SELECT '1395', 'W1160F0F', 'Plug,   1/2" BR CORP' UNION ALL
SELECT '1396', 'W1160G0G', 'Plug,   5/8" BR CORP' UNION ALL
SELECT '1397', 'W1160H0H', 'Plug,   3/4" BR CORP' UNION ALL
SELECT '1398', 'W1170101', 'Plug,  1" BR MIP' UNION ALL
SELECT '1399', 'W1170202', 'Plug,  2" BR MIP' UNION ALL
SELECT '1400', 'W1170F0F', 'Plug,   1/2" BR MIP' UNION ALL
SELECT '1401', 'W1170G0G', 'Plug,   5/8" BR MIP' UNION ALL
SELECT '1402', 'W1170H0H', 'Plug,   3/4" BR MIP' UNION ALL
SELECT '1403', 'W1171C1C', 'Plug,  1 1/4" BR MIP' UNION ALL
SELECT '1404', 'W1171F1F', 'Plug,  1 1/2" BR MIP' UNION ALL
SELECT '1405', 'W1180H0H', 'Bend,   3/4" X  3/4" CF X IPC 90°' UNION ALL
SELECT '1406', 'W1200101', 'Bend,  1" BR FIP 90°' UNION ALL
SELECT '1407', 'W120010F', 'Bend,  1" X  1/2" BR FIP 90°' UNION ALL
SELECT '1408', 'W1200201', 'Bend,  2" X  1" BR FIP 90°' UNION ALL
SELECT '1409', 'W1200202', 'Bend,  2" BR FIP 90°' UNION ALL
SELECT '1410', 'W120021F', 'Bend,  2" X  1 1/2" BR FIP 90°' UNION ALL
SELECT '1411', 'W1200F02', 'Bend,   1/2" X 2" BR FIP 90°' UNION ALL
SELECT '1412', 'W1200F0F', 'Bend,   1/2" X  1/2" BR FIP 90°' UNION ALL
SELECT '1413', 'W1200H0C', 'Bend,   3/4" X  1/4" BR FIP X FIP 90°' UNION ALL
SELECT '1414', 'W1200H0F', 'Bend,   3/4" X  1/2" BR FIP 90°' UNION ALL
SELECT '1415', 'W1200H0H', 'Bend,   3/4" X  3/4" BR FIP 90°' UNION ALL
SELECT '1416', 'W1201C01', 'Bend,  1 1/4" X 1" BR FIP 90°' UNION ALL
SELECT '1417', 'W1201C1C', 'Bend,  1 1/4" BR FIP 90°' UNION ALL
SELECT '1418', 'W1201F1F', 'Bend,  1 1/2" BR FIP 90°' UNION ALL
SELECT '1419', 'W1202F0C', 'Bend,  2 1/2" X  1/4" BR FIP 90°' UNION ALL
SELECT '1420', 'W1210101', 'Bend,  1" BR FIP 45°' UNION ALL
SELECT '1421', 'W121010C', 'Bend,  1" X  1/4" BR FIP 45°' UNION ALL
SELECT '1422', 'W1210202', 'Bend,  2" BR FIP 45°' UNION ALL
SELECT '1423', 'W1210F0F', 'Bend,   1/2" X  1/2" BR FIP 45°' UNION ALL
SELECT '1424', 'W1210H0H', 'Bend,   3/4" X  3/4" BR FIP 45°' UNION ALL
SELECT '1425', 'W1211C1C', 'Bend,  1 1/4" BR FIP 45°' UNION ALL
SELECT '1426', 'W1211F1F', 'Bend,  1 1/2" BR FIP 45°' UNION ALL
SELECT '1427', 'W1220101', 'Bend,  1" BR FIP X MIP 90°' UNION ALL
SELECT '1428', 'W1220202', 'Bend,  2" BR FIP X MIP 90°' UNION ALL
SELECT '1429', 'W1220F0F', 'Bend,   1/2" X  1/2" BR FIP X MIP 90°' UNION ALL
SELECT '1430', 'W1220H0H', 'Bend,   3/4" X  3/4" BR FIP X MIP 90°' UNION ALL
SELECT '1431', 'W1221C1C', 'Bend,  1 1/4" BR FIP X MIP 90°' UNION ALL
SELECT '1432', 'W1221F1F', 'Bend,  1 1/2" BR FIP X MIP 90°' UNION ALL
SELECT '1433', 'W1222F2F', 'Bend,  2 1/2" BR FIP X MIP 90°'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 15.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1434', 'W1230202', 'Bend,  2" PC X PC 90°' UNION ALL
SELECT '1435', 'W1230H01', 'Bend,   3/4" X 1" PC X PC 90°' UNION ALL
SELECT '1436', 'W1231F1F', 'Bend,  1 1/2" PC X PC 90°' UNION ALL
SELECT '1437', 'W124010H', 'Bend,  1" X  3/4" BR PC X CC 90°' UNION ALL
SELECT '1438', 'W1250101', 'Bend,  1" PC X FIP 90°' UNION ALL
SELECT '1439', 'W1260101', 'Bend,  1" MIP X CC 90°' UNION ALL
SELECT '1440', 'W126010H', 'Bend,  1" X  3/4" MIP X CC 90°' UNION ALL
SELECT '1441', 'W1260202', 'Bend,  2" MIP X CC 90°' UNION ALL
SELECT '1442', 'W1260H0H', 'Bend,   3/4" X  3/4" MIP X CC 90°' UNION ALL
SELECT '1443', 'W1270H0H', 'Bend,   3/4" X  3/4" MIP X IPC 90°' UNION ALL
SELECT '1444', 'W1290101', 'Bend,  1" CC X CC 90°' UNION ALL
SELECT '1445', 'W129010H', 'Bend,  1" X  3/4" CC X CC 90°' UNION ALL
SELECT '1446', 'W1290202', 'Bend,  2" CC X CC 90°' UNION ALL
SELECT '1447', 'W1290H0H', 'Bend,   3/4" X  3/4" CC X CC 90°' UNION ALL
SELECT '1448', 'W1291F1F', 'Bend,  1 1/2" CC X CC 90°' UNION ALL
SELECT '1449', 'W1300101', 'Bend,  1" CF X MIP 90°' UNION ALL
SELECT '1450', 'W1300H0H', 'Bend,   3/4" X  3/4" CF X MIP 90°' UNION ALL
SELECT '1451', 'W1301F1F', 'Bend,  1 1/2" CF X MIP 90°' UNION ALL
SELECT '1452', 'W1310101', 'Bend,  1" CF X MIP 45°' UNION ALL
SELECT '1453', 'W1310H0H', 'Bend,   3/4" X  3/4" CF X MIP 45°' UNION ALL
SELECT '1454', 'W1320101', 'Bend,  1" CF X CF 90°' UNION ALL
SELECT '1455', 'W1320202', 'Bend,  2" CF X CF 90°' UNION ALL
SELECT '1456', 'W1320H0H', 'Bend,   3/4" X  3/4" CF X CF 90°' UNION ALL
SELECT '1457', 'W1330202', 'Bend,  2" GALV 90°' UNION ALL
SELECT '1458', 'W1330303', 'Bend,  3" GALV 90°' UNION ALL
SELECT '1459', 'W1330404', 'Bend,  4" GALV 90°' UNION ALL
SELECT '1460', 'W1331F1F', 'Bend,  1 1/2" GALV 90°' UNION ALL
SELECT '1461', 'W1332F02', 'Bend,  2 1/2" X 2" GALV 90°' UNION ALL
SELECT '1462', 'W1332F2F', 'Bend,  2 1/2" GALV 90°' UNION ALL
SELECT '1463', 'W1340202', 'Bend,  2" GALV 45°' UNION ALL
SELECT '1464', 'W1340303', 'Bend,  3" GALV 45°' UNION ALL
SELECT '1465', 'W1340404', 'Bend,  4" GALV 45°' UNION ALL
SELECT '1466', 'W1340606', 'Bend,  6" GALV 45°' UNION ALL
SELECT '1467', 'W1341C1C', 'Bend,  1 1/4" GALV 45°' UNION ALL
SELECT '1468', 'W1341F1F', 'Bend,  1 1/2" GALV 45°' UNION ALL
SELECT '1469', 'W1342F2F', 'Bend,  2 1/2" GALV 45°' UNION ALL
SELECT '1470', 'W1350101', 'Bend,  1" CF X FIP 90°' UNION ALL
SELECT '1471', 'W135010H', 'Bend,  1" X  3/4" CF X FIP 90°' UNION ALL
SELECT '1472', 'W1350H0H', 'Bend,   3/4" X  3/4" CF X FIP 90°' UNION ALL
SELECT '1473', 'W1351C1C', 'Bend,  1 1/4" CF X FIP 90°' UNION ALL
SELECT '1474', 'W1360101', 'Bend,  1" LFA X CF 45°' UNION ALL
SELECT '1475', 'W1360202', 'Bend,  2" LFA X CF 45°' UNION ALL
SELECT '1476', 'W1360G0H', 'Bend,   5/8" X 3/4" LFA X CF 45°' UNION ALL
SELECT '1477', 'W1360H0G', 'Bend,   3/4" X  5/8" LFA X CF 45°' UNION ALL
SELECT '1478', 'W1360H0H', 'Bend,   3/4" X  3/4" LFA X CF 45°' UNION ALL
SELECT '1479', 'W1370101', 'Tee,  1" BR FIP X FIP' UNION ALL
SELECT '1480', 'W137010H', 'Tee,  1" X  3/4" BR FIP X FIP' UNION ALL
SELECT '1481', 'W1370201', 'Tee,  2" X 1" BR FIP X FIP' UNION ALL
SELECT '1482', 'W1370202', 'Tee,  2" BR FIP X FIP' UNION ALL
SELECT '1483', 'W137020H', 'Tee,  2" X  3/4" BR FIP X FIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 16.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1484', 'W1370404', 'Tee,  4" BR FIP X FIP' UNION ALL
SELECT '1485', 'W1370H0H', 'Tee,   3/4" BR FIP X FIP' UNION ALL
SELECT '1486', 'W1371C1C', 'Tee,  1 1/4" BR FIP X FIP' UNION ALL
SELECT '1487', 'W1371F1F', 'Tee,  1 1/2" BR FIP X FIP' UNION ALL
SELECT '1488', 'W1380101', 'Tee,  1" BR PC X PC' UNION ALL
SELECT '1489', 'W138010H', 'Tee,  1" X  3/4" BR PC X PC' UNION ALL
SELECT '1490', 'W1380H01', 'Tee,   3/4" X 1" BR PC X PC' UNION ALL
SELECT '1491', 'W1380H0H', 'Tee,   3/4" BR PC X PC' UNION ALL
SELECT '1492', 'W1390101', 'Tee,  1" BR CF' UNION ALL
SELECT '1493', 'W139010H', 'Tee,  1" X  3/4" BR CF' UNION ALL
SELECT '1494', 'W1390201', 'Tee,  2" X 1" BR CF' UNION ALL
SELECT '1495', 'W1390202', 'Tee,  2" BR CF' UNION ALL
SELECT '1496', 'W1390H01', 'Tee,   3/4" X 1" BR CF' UNION ALL
SELECT '1497', 'W1390H0H', 'Tee,   3/4" BR CF' UNION ALL
SELECT '1498', 'W1391F1F', 'Tee,  1 1/2" BR CF' UNION ALL
SELECT '1499', 'W1400101', 'Tee,  1" BR CC X CC' UNION ALL
SELECT '1500', 'W140010H', 'Tee,  1" X  3/4" BR CC X CC' UNION ALL
SELECT '1501', 'W140011C', 'Tee,  1" X 1" X 1 1/4" BR CC X CC' UNION ALL
SELECT '1502', 'W1400201', 'Tee,  2" X 1" BR CC X CC' UNION ALL
SELECT '1503', 'W1400202', 'Tee,  2" BR CC X CC' UNION ALL
SELECT '1504', 'W140020H', 'Tee,  2" X  3/4" BR CC X CC' UNION ALL
SELECT '1505', 'W140021F', 'Tee,  2" X 1 1/2" BR CC X CC' UNION ALL
SELECT '1506', 'W1400H01', 'Tee,   3/4" X 1" BR CC X CC' UNION ALL
SELECT '1507', 'W1400H0H', 'Tee,   3/4" BR CC X CC' UNION ALL
SELECT '1508', 'W1401F1F', 'Tee,  1 1/2" BR CC X CC' UNION ALL
SELECT '1509', 'W1410101', 'Tee,  1" BR CC X FIP' UNION ALL
SELECT '1510', 'W1410201', 'Tee,  2" X 1" BR CC X FIP' UNION ALL
SELECT '1511', 'W1410201', 'Tee,  2" X 1" BR CC X FIP' UNION ALL
SELECT '1512', 'W1410H01', 'Tee,   3/4" X 1" BR CC X FIP' UNION ALL
SELECT '1513', 'W1430H01', 'Tee,   3/4" X 1" BR MIP X MIP X CF' UNION ALL
SELECT '1514', 'W1430H0H', 'Tee,   3/4" BR MIP X MIP X CF' UNION ALL
SELECT '1515', 'W1440202', 'Tee,  2" GALV' UNION ALL
SELECT '1516', 'W1440303', 'Tee,  3" GALV' UNION ALL
SELECT '1517', 'W1440402', 'Tee,  4" X 2" GALV' UNION ALL
SELECT '1518', 'W1441C0H', 'Tee,  1 1/4" X 3/4" GALV' UNION ALL
SELECT '1519', 'W1441C1C', 'Tee,  1 1/4" GALV' UNION ALL
SELECT '1520', 'W1441F1F', 'Tee,  1 1/2" GALV' UNION ALL
SELECT '1521', 'W1442F2F', 'Tee,  2 1/2" GALV' UNION ALL
SELECT '1522', 'W1460101', 'Bend,  1" BR CC X CF 90°' UNION ALL
SELECT '1523', 'W146010H', 'Bend,  1" X  3/4" BR CC X CF 90°' UNION ALL
SELECT '1524', 'W1460206', 'Bend,  2" X  6" BR CC X CF 90°' UNION ALL
SELECT '1525', 'W1460212', 'Bend,  2" X 12" BR CC X CF 90°' UNION ALL
SELECT '1526', 'W1460H0H', 'Bend,   3/4" X  3/4" BR CC X CF 90°' UNION ALL
SELECT '1527', 'W1470H01', 'Bend,   3/4" X 1" BR MC X PC 90°' UNION ALL
SELECT '1528', 'W1480101', 'Bend,  1" CC X FIP 90°' UNION ALL
SELECT '1529', 'W1480202', 'Bend,  2" CC X FIP 90°' UNION ALL
SELECT '1530', 'W1480H0H', 'Bend,   3/4" X  3/4" CC X FIP 90°' UNION ALL
SELECT '1531', 'W1481F1F', 'Bend,  1 1/2" CC X FIP 90°' UNION ALL
SELECT '1532', 'W1490H0H', 'Bend,   3/4" X  3/4" PC X MIP 90°' UNION ALL
SELECT '1533', 'W1510H0H', 'Bend,   3/4" X  3/4" CC X FIP 45°'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 17.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1534', 'W1520101', 'Bend,  1" CF X FIP 45°' UNION ALL
SELECT '1535', 'W1520G0H', 'Bend,   5/8" X 3/4" CF X FIP 45°' UNION ALL
SELECT '1536', 'W1520H0H', 'Bend,   3/4" X  3/4" CF X FIP 45°' UNION ALL
SELECT '1537', 'W1521F1F', 'Bend,  1 1/2" CF X FIP 45°' UNION ALL
SELECT '1538', 'W1530G0H', 'Bend,   5/8" X 3/4" LFA X CF 90°' UNION ALL
SELECT '1539', 'W1530H0H', 'Bend,   3/4" X  3/4" LFA X CF 90°' UNION ALL
SELECT '1540', 'W1540101', 'Idler, 1" Meter' UNION ALL
SELECT '1541', 'W1540G0G', 'Idler,  5/8" Meter' UNION ALL
SELECT '1542', 'W1540G0H', 'Idler,  5/8" X 3/4" Meter' UNION ALL
SELECT '1543', 'W1540H0H', 'Idler,  3/4" Meter' UNION ALL
SELECT '1544', 'W1550107', 'Resetter, 1" X  7" H' UNION ALL
SELECT '1545', 'W1550110', 'Resetter, 1" X 10" H' UNION ALL
SELECT '1546', 'W1550112', 'Resetter, 1" X 12" H' UNION ALL
SELECT '1547', 'W1550115', 'Resetter, 1" X 15" H' UNION ALL
SELECT '1548', 'W1550118', 'Resetter, 1" X 18" H' UNION ALL
SELECT '1549', 'W1550207', 'Resetter, 2" X  7" H' UNION ALL
SELECT '1550', 'W1550212', 'Resetter, 2" X 12" H' UNION ALL
SELECT '1551', 'W1550213', 'Resetter, 2" X 13" H' UNION ALL
SELECT '1552', 'W1550214', 'Resetter, 2" X 14" H' UNION ALL
SELECT '1553', 'W1550215', 'Resetter, 2" X 15" H' UNION ALL
SELECT '1554', 'W1550218', 'Resetter, 2" X 18" H' UNION ALL
SELECT '1555', 'W1550224', 'Resetter, 2" X 24" H' UNION ALL
SELECT '1556', 'W1550G30', 'Resetter,  5/8" X 30" H' UNION ALL
SELECT '1557', 'W1550H07', 'Resetter,  3/4" X  7" H' UNION ALL
SELECT '1558', 'W1550H09', 'Resetter,  3/4" X  9" H' UNION ALL
SELECT '1559', 'W1550H12', 'Resetter,  3/4" X 12" H' UNION ALL
SELECT '1560', 'W1550H24', 'Resetter,  3/4" X 24" H' UNION ALL
SELECT '1561', 'W1551F07', 'Resetter, 1 1/2" X  7" H' UNION ALL
SELECT '1562', 'W1551F13', 'Resetter, 1 1/2" X 13" H' UNION ALL
SELECT '1563', 'W1551F15', 'Resetter, 1 1/2" X 15" H' UNION ALL
SELECT '1564', 'W1551F18', 'Resetter, 1 1/2" X 18" H' UNION ALL
SELECT '1565', 'W1551F24', 'Resetter, 1 1/2" X 24" H' UNION ALL
SELECT '1566', 'W1551F27', 'Resetter, 1 1/2" X 27" H' UNION ALL
SELECT '1567', 'W155GF04', 'Resetter,  5/8" X   1/2" X  4" H' UNION ALL
SELECT '1568', 'W155GF12', 'Resetter,  5/8" X   1/2" X 12" H' UNION ALL
SELECT '1569', 'W155GF18', 'Resetter,  5/8" X   1/2" X 18" H' UNION ALL
SELECT '1570', 'W155GF24', 'Resetter,  5/8" X   1/2" X 24" H' UNION ALL
SELECT '1571', 'W155GH04', 'Resetter,  5/8" X   3/4" X  4" H' UNION ALL
SELECT '1572', 'W155GH07', 'Resetter,  5/8" X   3/4" X  7" H' UNION ALL
SELECT '1573', 'W155GH09', 'Resetter,  5/8" X   3/4" X  9" H' UNION ALL
SELECT '1574', 'W155GH12', 'Resetter,  5/8" X   3/4" X 12" H' UNION ALL
SELECT '1575', 'W155GH15', 'Resetter,  5/8" X   3/4" X 15" H' UNION ALL
SELECT '1576', 'W155GH18', 'Resetter,  5/8" X   3/4" X 18" H' UNION ALL
SELECT '1577', 'W155GH24', 'Resetter,  5/8" X   3/4" X 24" H' UNION ALL
SELECT '1578', 'W155GH30', 'Resetter,  5/8" X   3/4" X 30" H' UNION ALL
SELECT '1579', 'W155GH36', 'Resetter,  5/8" X   3/4" X 36" H' UNION ALL
SELECT '1580', 'W1560202', 'Setter, 2" W/by-pass' UNION ALL
SELECT '1581', 'W1561F1F', 'Setter, 1 1/2" W/by-pass' UNION ALL
SELECT '1582', 'W1570101', 'Yoke, 1" Bar' UNION ALL
SELECT '1583', 'W1570G0G', 'Yoke,  5/8" Bar'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 18.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1584', 'W1570G0H', 'Yoke,  5/8" X 3/4" Bar' UNION ALL
SELECT '1585', 'W1570H0H', 'Yoke,  3/4" Bar' UNION ALL
SELECT '1586', 'W1580H0H', 'Setter,  3/4" Meter  (inside horn)' UNION ALL
SELECT '1587', 'W1590G0H', 'Horn,  5/8" X 3/4" Meter K' UNION ALL
SELECT '1588', 'W1590H0H', 'Horn,  3/4" Meter K' UNION ALL
SELECT '1589', 'W1600H0H', 'Setter,  3/4" K style X CC' UNION ALL
SELECT '1590', 'W1610G0G', 'Setter,  5/8" K style X IPC' UNION ALL
SELECT '1591', 'W1610G0H', 'Setter,  5/8" X  3/4" K style X IPC' UNION ALL
SELECT '1592', 'W1610H0H', 'Setter,  3/4" K style X IPC' UNION ALL
SELECT '1593', 'W1620G0G', 'Setter,  5/8" K style X PC' UNION ALL
SELECT '1594', 'W1620H0H', 'Setter,  3/4" K style X PC' UNION ALL
SELECT '1595', 'W1630101', 'Setter, 1" w/check' UNION ALL
SELECT '1596', 'W1630G0G', 'Setter,  5/8" w/check' UNION ALL
SELECT '1597', 'W1630G0H', 'Setter,  5/8" X  3/4" w/check' UNION ALL
SELECT '1598', 'W1630H0H', 'Setter,  3/4"  w/check' UNION ALL
SELECT '1599', 'W1640101', 'Setter, 1" PC X PC' UNION ALL
SELECT '1600', 'W1640G03', 'Setter,  5/8" X 3" PC X PC' UNION ALL
SELECT '1601', 'W1640G0H', 'Setter,  5/8" X  3/4" PC X PC' UNION ALL
SELECT '1602', 'W1640H18', 'Setter,  3/4" X 18" PC X PC' UNION ALL
SELECT '1603', 'W1650G0H', 'Setter,  5/8" X  3/4" CC Vertical' UNION ALL
SELECT '1604', 'W1650H0H', 'Setter,  3/4" CC Vertical' UNION ALL
SELECT '1605', 'W166010H', 'Setter, 1" X  3/4" X CC' UNION ALL
SELECT '1606', 'W1660G0G', 'Setter,  5/8" X CC' UNION ALL
SELECT '1607', 'W1660H0H', 'Setter,  3/4" CC' UNION ALL
SELECT '1608', 'W1661F1F', 'Setter, 1 1/2" X CC' UNION ALL
SELECT '1609', 'W1670101', 'Setter, 1" FIP X FIP' UNION ALL
SELECT '1610', 'W1670202', 'Setter, 2" FIP x FIP' UNION ALL
SELECT '1611', 'W1670G0G', 'Setter,  5/8" FIP X FIP' UNION ALL
SELECT '1612', 'W1670G0H', 'Setter,  5/8" X  3/4" FIP X FIP' UNION ALL
SELECT '1613', 'W1670H0H', 'Setter,  3/4" FIP X FIP' UNION ALL
SELECT '1614', 'W1671F1F', 'Setter, 1 1/2" FIP X FIP' UNION ALL
SELECT '1615', 'W1680101', 'Setter, 1" C X I' UNION ALL
SELECT '1616', 'W1680110', 'Setter, 1" X 10" C X I' UNION ALL
SELECT '1617', 'W1680215', 'Setter, 2" X 15" C X I' UNION ALL
SELECT '1618', 'W1680G0G', 'Setter,  5/8" C X I' UNION ALL
SELECT '1619', 'W1680G0H', 'Setter,  5/8" X  3/4" C X I' UNION ALL
SELECT '1620', 'W1680H07', 'Setter,  3/4" X 7" C X I' UNION ALL
SELECT '1621', 'W1680H0H', 'Setter,  3/4" C X I' UNION ALL
SELECT '1622', 'W1681F1F', 'Setter, 1 1/2" C X I' UNION ALL
SELECT '1623', 'W1690101', 'Wheel, 1" Expansion Setter' UNION ALL
SELECT '1624', 'W1690G0G', 'Wheel,  5/8" Expansion Setter' UNION ALL
SELECT '1625', 'W1690G0H', 'Wheel,  5/8" X 3/4"  Expansion Setter' UNION ALL
SELECT '1626', 'W1690H0H', 'Wheel,  3/4" Expansion Setter' UNION ALL
SELECT '1627', 'W1700101', 'Adapter,  1" X 1" Meter' UNION ALL
SELECT '1628', 'W1700102', 'Adapter,  1" X 2" Meter' UNION ALL
SELECT '1629', 'W170011C', 'Adapter,  1" X 1 1/4" Meter' UNION ALL
SELECT '1630', 'W170011F', 'Adapter,  1" X 1 1/2" Meter' UNION ALL
SELECT '1631', 'W1700202', 'Adapter,  2" Meter' UNION ALL
SELECT '1632', 'W1700F0H', 'Adapter,   1/2" X 3/4" Meter' UNION ALL
SELECT '1633', 'W1700G01', 'Adapter,   5/8" X 1" Meter'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 19.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1634', 'W1700G0G', 'Adapter,   5/8" X  5/8" Meter' UNION ALL
SELECT '1635', 'W1700G0H', 'Adapter,   5/8" X  3/4" Meter' UNION ALL
SELECT '1636', 'W1700H01', 'Adapter,   3/4" X 1" Meter' UNION ALL
SELECT '1637', 'W1700H0H', 'Adapter,   3/4" X  3/4" Meter' UNION ALL
SELECT '1638', 'W1710101', 'Coupling,  1" Yoke MC X CF' UNION ALL
SELECT '1639', 'W1710H0H', 'Coupling,   3/4" Yoke MC X CF' UNION ALL
SELECT '1640', 'W1730101', 'Bend,  1" Yoke Coupling MC X CC' UNION ALL
SELECT '1641', 'W1730G0H', 'Bend,   5/8" X 3/4" Yoke Coupling MC X CC' UNION ALL
SELECT '1642', 'W1730G0H', 'Bend,   5/8" X 3/4" Yoke Coupling MC X CC' UNION ALL
SELECT '1643', 'W1730H01', 'Bend,   3/4" X 1" Yoke Coupling MC X CC' UNION ALL
SELECT '1644', 'W1750101', 'Bend,  1" Yoke MC X PC' UNION ALL
SELECT '1645', 'W1760404', 'Clamp, Thrust  4"' UNION ALL
SELECT '1646', 'W1760606', 'Clamp, Thrust  6"' UNION ALL
SELECT '1647', 'W1760808', 'Clamp, Thrust  8"' UNION ALL
SELECT '1648', 'W1761010', 'Clamp, Thrust 10"' UNION ALL
SELECT '1649', 'W1761212', 'Clamp, Thrust 12"' UNION ALL
SELECT '1650', 'W1761414', 'Clamp, Thrust 14"' UNION ALL
SELECT '1651', 'W1761616', 'Clamp, Thrust 16"' UNION ALL
SELECT '1652', 'W1761818', 'Clamp, Thrust 18"' UNION ALL
SELECT '1653', 'W1762020', 'Clamp, Thrust 20"' UNION ALL
SELECT '1654', 'W1762424', 'Clamp, Thrust 24"' UNION ALL
SELECT '1655', 'W1763030', 'Clamp, Thrust 30"' UNION ALL
SELECT '1656', 'W1770101', 'Bend,  1" Yoke Coupling MC X FIP' UNION ALL
SELECT '1657', 'W1770H0H', 'Bend,   3/4" X  3/4" Yoke Coupling MC X FIP' UNION ALL
SELECT '1658', 'W1780H01', 'Valve,   3/4" X 1" Yoke Angle MC X CC' UNION ALL
SELECT '1659', 'W1780H0H', 'Valve,   3/4" Yoke Angle MC X CC' UNION ALL
SELECT '1660', 'W1800101', 'Valve,  1" Yoke Angle MC X FIP' UNION ALL
SELECT '1661', 'W1800H01', 'Valve,   3/4" X 1" Yoke Angle MC X FIP' UNION ALL
SELECT '1662', 'W1800H0H', 'Valve,   3/4" Yoke Angle MC X FIP' UNION ALL
SELECT '1663', 'W1820G0H', 'Yoke,  5/8" X 3/4" Dual Setting' UNION ALL
SELECT '1664', 'W1830101', 'Setter, 1" CF' UNION ALL
SELECT '1665', 'W1830202', 'Setter, 2" CF' UNION ALL
SELECT '1666', 'W1830G0G', 'Setter,  5/8" CF' UNION ALL
SELECT '1667', 'W1830H0H', 'Setter,  3/4" CF' UNION ALL
SELECT '1668', 'W1841220', 'Box, Meter 12" X 20" Poly Composite' UNION ALL
SELECT '1669', 'W1841324', 'Box, Meter 13" X 24" Poly Composite' UNION ALL
SELECT '1670', 'W1841728', 'Box, Meter 17" X 28" Poly Composite' UNION ALL
SELECT '1671', 'W1841730', 'Box, Meter 17" X 30" Poly Composite' UNION ALL
SELECT '1672', 'W1851801', 'Box, Meter Single W/Setting 18" X 1"' UNION ALL
SELECT '1673', 'W185180G0H', 'Box, Meter Single W/Setting 18" X  5/8" X 3/4"' UNION ALL
SELECT '1674', 'W18618110F', 'Frame, MB 18" X 11 1/2" No Lid' UNION ALL
SELECT '1675', 'W1871414', 'Lid, 14" X 14" Meter Fabricated' UNION ALL
SELECT '1676', 'W1871422', 'Lid, 14" X 22" Meter Fabricated' UNION ALL
SELECT '1677', 'W1871800', 'Lid, 18" Meter Fabricated' UNION ALL
SELECT '1678', 'W1872442', 'Lid, 24" X 42" Meter Fabricated' UNION ALL
SELECT '1679', 'W1881730', 'Lid, 17" X 30" Rectangular' UNION ALL
SELECT '1680', 'W1890202', 'Flange,  2" Meter X CC 90°' UNION ALL
SELECT '1681', 'W1900101', 'Flange,  1" Meter FIP' UNION ALL
SELECT '1682', 'W1900303', 'Flange,  3" Meter FIP' UNION ALL
SELECT '1683', 'W1900404', 'Flange,  4" Meter FIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 20.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1684', 'W1900606', 'Flange,  6" Meter FIP' UNION ALL
SELECT '1685', 'W1900G0H', 'Flange,   5/8" X 3/4" Meter FIP' UNION ALL
SELECT '1686', 'W1900H0H', 'Flange,   3/4" Meter FIP' UNION ALL
SELECT '1687', 'W1910202', 'Flange,  2" Meter MIP' UNION ALL
SELECT '1688', 'W1911F1F', 'Flange,  1 1/2" Meter MIP' UNION ALL
SELECT '1689', 'W1920202', 'Flange,  2" Meter Adapt X CC' UNION ALL
SELECT '1690', 'W1930303', 'Flange,  3" Meter Adapt X IPC' UNION ALL
SELECT '1691', 'W1930404', 'Flange,  4" Meter Adapt X IPC' UNION ALL
SELECT '1692', 'W1930606', 'Flange,  6" Meter Adapt X IPC' UNION ALL
SELECT '1693', 'W1931F1F', 'Flange,  1 1/2" Meter Adapt X IPC' UNION ALL
SELECT '1694', 'W196180G0H', 'Box, Meter Dual Thermal W/Setting 18" X 5/8" X 3/4"' UNION ALL
SELECT '1695', 'W1971414', 'Box, Meter 14" X 14" ST' UNION ALL
SELECT '1696', 'W1971422', 'Box, Meter 14" X 22" ST' UNION ALL
SELECT '1697', 'W1972442', 'Box, Meter 24" X 42" ST' UNION ALL
SELECT '1698', 'W1973030', 'Box, Meter 30" X 30" ST' UNION ALL
SELECT '1699', 'W1981818', 'Monitor Cover, 18" MB' UNION ALL
SELECT '1700', 'W1982020', 'Monitor Cover, 20" MB' UNION ALL
SELECT '1701', 'W1982420', 'Monitor Cover, 24" X 20" MB' UNION ALL
SELECT '1702', 'W1983020', 'Monitor Cover, 30" X 20" MB' UNION ALL
SELECT '1703', 'W1983620', 'Monitor Cover, 36" X 20" MB' UNION ALL
SELECT '1704', 'W1991017', 'Box, Meter Concrete 10" X 17"' UNION ALL
SELECT '1705', 'W1991220', 'Box, Meter Concrete 12" X 20"' UNION ALL
SELECT '1706', 'W1991324', 'Box, Meter Concrete 13" X 24"' UNION ALL
SELECT '1707', 'W1991500', 'Box, Meter Concrete 15"' UNION ALL
SELECT '1708', 'W1991728', 'Box, Meter Concrete 17" X 28"' UNION ALL
SELECT '1709', 'W1991818', 'Box, Meter Concrete 18" X 18"' UNION ALL
SELECT '1710', 'W1991824', 'Box, Meter Concrete 18" X 24"' UNION ALL
SELECT '1711', 'W2001419', 'Box, Meter Plastic 14" X 19"' UNION ALL
SELECT '1712', 'W2001730', 'Box, Meter Plastic 17" X 30"' UNION ALL
SELECT '1713', 'W2001812', 'Box, Meter Plastic 18" X 12"' UNION ALL
SELECT '1714', 'W2001814', 'Box, Meter Plastic 18" X 14"' UNION ALL
SELECT '1715', 'W2001818', 'Box, Meter Plastic 18" X 18"' UNION ALL
SELECT '1716', 'W2001824', 'Box, Meter Plastic 18" X 24"' UNION ALL
SELECT '1717', 'W2001828', 'Box, Meter Plastic 18" X 28"' UNION ALL
SELECT '1718', 'W2001830', 'Box, Meter Plastic 18" X 30"' UNION ALL
SELECT '1719', 'W2001836', 'Box, Meter Plastic 18" X 36"' UNION ALL
SELECT '1720', 'W2001848', 'Box, Meter Plastic 18" X 48"' UNION ALL
SELECT '1721', 'W2002012', 'Box, Meter Plastic 20" X 12"' UNION ALL
SELECT '1722', 'W2002020', 'Box, Meter Plastic 20" X 20"' UNION ALL
SELECT '1723', 'W2002024', 'Box, Meter Plastic 20" X 24"' UNION ALL
SELECT '1724', 'W2002048', 'Box, Meter Plastic 20" X 48"' UNION ALL
SELECT '1725', 'W2002130', 'Box, Meter Plastic 21" X 30"' UNION ALL
SELECT '1726', 'W2002412', 'Box, Meter Plastic 24" X 12"' UNION ALL
SELECT '1727', 'W2002418', 'Box, Meter Plastic 24" X 18"' UNION ALL
SELECT '1728', 'W2002419', 'Box, Meter Plastic 24" X 19"' UNION ALL
SELECT '1729', 'W2002436', 'Box, Meter Plastic 24" X 36"' UNION ALL
SELECT '1730', 'W2003024', 'Box, Meter Plastic 30" X 24"' UNION ALL
SELECT '1731', 'W2003030', 'Box, Meter Plastic 30" X 30"' UNION ALL
SELECT '1732', 'W2003648', 'Box, Meter Plastic 36" X 48"' UNION ALL
SELECT '1733', 'W2003660', 'Box, Meter Plastic 36" X 60"'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 21.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1734', 'W2011217', 'Box w/lid, Meter 12" X 17" Single' UNION ALL
SELECT '1735', 'W2011218', 'Box w/lid, Meter 12" X 18" Single' UNION ALL
SELECT '1736', 'W2011622', 'Box w/lid, Meter 16" X 22" Single' UNION ALL
SELECT '1737', 'W2032000', 'Frame & Lid, 20" Double Lid' UNION ALL
SELECT '1738', 'W2041100', 'Frame & Lid, 11"' UNION ALL
SELECT '1739', 'W20420001H', 'Frame & Lid, 20" w/1 TP Hole' UNION ALL
SELECT '1740', 'W20420002H', 'Frame & Lid, 20" w/2 TP Hole' UNION ALL
SELECT '1741', 'W20421001H', 'Frame & Lid, 21" w/1 TP Hole' UNION ALL
SELECT '1742', 'W20421002H', 'Frame & Lid, 21" w/2 TP Hole' UNION ALL
SELECT '1743', 'W2042700', 'Frame & Lid, 27"' UNION ALL
SELECT '1744', 'W2043000', 'Frame & Lid, 30"' UNION ALL
SELECT '1745', 'W2043624', 'Frame & Lid, 36" X 24"' UNION ALL
SELECT '1746', 'W2062024', 'Ring, 20" X 24" Extension Light' UNION ALL
SELECT '1747', 'W2062030', 'Ring, 20" X 30" Extension Light' UNION ALL
SELECT '1748', 'W2062036', 'Ring, 20" X 36" Extension Light' UNION ALL
SELECT '1749', 'W2062036', 'Ring, 20" X 36" Extension Light' UNION ALL
SELECT '1750', 'W2063020', 'Ring, 30" X 20" Extension Light' UNION ALL
SELECT '1751', 'W2063618', 'Ring, 36" X 18" Extension Light' UNION ALL
SELECT '1752', 'W2063624', 'Ring, Meter Regular 36" X 24"' UNION ALL
SELECT '1753', 'W2071818', 'Lid, Meter 18" Concrete' UNION ALL
SELECT '1754', 'W2072020', 'Lid, Meter 20" Concrete' UNION ALL
SELECT '1755', 'W2076000', 'Lid, Meter 60" Concrete' UNION ALL
SELECT '1756', 'W2082448', 'Vault, 24" X 48" w/setting' UNION ALL
SELECT '1757', 'W2082748', 'Vault, 27" X 48" w/setting' UNION ALL
SELECT '1758', 'W2083648', 'Vault, 36" X 48" w/setting' UNION ALL
SELECT '1759', 'W2090027', 'Riser, 27" Meter Box' UNION ALL
SELECT '1760', 'W2092002', 'Riser, 20" X 2" Meter Box' UNION ALL
SELECT '1761', 'W2092004', 'Riser, 20" X 4" Meter Box' UNION ALL
SELECT '1762', 'W2092406', 'Riser, 24" X 6" Meter Box' UNION ALL
SELECT '1763', 'W2092406', 'Riser, 24" X 6" Meter Box' UNION ALL
SELECT '1764', 'W2101100', 'Lid, 11"' UNION ALL
SELECT '1765', 'W210110FNL', 'Lid, 11 1/2" Meter for F/L Non locking' UNION ALL
SELECT '1766', 'W2101500', 'Lid, 15" Only for F/L' UNION ALL
SELECT '1767', 'W2102000NL', 'Lid, 20" Meter for F/L Non Locking' UNION ALL
SELECT '1768', 'W2113620', 'Ring, Extension MB 36" X 20"' UNION ALL
SELECT '1769', 'W2122424', 'Lid, 24" MB Flat hinged HD' UNION ALL
SELECT '1770', 'W2131800', 'Lid, 18" MB Flat hinged' UNION ALL
SELECT '1771', 'W2132424', 'Lid, 24" MB Flat hinged' UNION ALL
SELECT '1772', 'W2133030', 'Lid, 30" MB Flat hinged' UNION ALL
SELECT '1773', 'W2150000', 'Curb Box, Complete PL SL' UNION ALL
SELECT '1774', 'W2152F00', 'Curb Box,  2 1/2" Complete PL SL' UNION ALL
SELECT '1775', 'W2162F00', 'Curb Box,  2 1/2" Top PL SL' UNION ALL
SELECT '1776', 'W2180101', 'Curb Box,  1" Complete CI SL' UNION ALL
SELECT '1777', 'W2180202', 'Curb Box,  2" Complete CI SL' UNION ALL
SELECT '1778', 'W2181003', 'Curb Box, 10" X 3" Complete CI SL' UNION ALL
SELECT '1779', 'W2181C1C', 'Curb Box,  1 1/4" Complete CI SL' UNION ALL
SELECT '1780', 'W2181F02', 'Curb Box,  1 1/2" X 2" Complete CI SL' UNION ALL
SELECT '1781', 'W2182F2F', 'Curb Box,  2 1/2" Complete CI SL' UNION ALL
SELECT '1782', 'W2190101', 'Curb Box,  1" Top CI SL' UNION ALL
SELECT '1783', 'W2190202', 'Curb Box,  2" Top CI SL'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 22.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1784', 'W2191C1C', 'Curb Box,  1 1/4" Top CI SL' UNION ALL
SELECT '1785', 'W2191F1F', 'Curb Box,  1 1/2" Top CI SL' UNION ALL
SELECT '1786', 'W2192F2F', 'Curb Box, Top 2 1/2" CI SL' UNION ALL
SELECT '1787', 'W2212F00', 'Extension, 2 1/2" Curb Box PL SL' UNION ALL
SELECT '1788', 'W2220101', 'Extension, 1" Curb Box  CI SL' UNION ALL
SELECT '1789', 'W2231F60', 'Curb Box,  1 1/2" CI SL Minn. Thread' UNION ALL
SELECT '1790', 'W2250001', 'Lid, Curb Box (Plastic Box)' UNION ALL
SELECT '1791', 'W2250004', 'Lid, Curb Box (Edison) 2 hole Lid 1" Upper Section' UNION ALL
SELECT '1792', 'W2250101', 'Lid, Curb Box 1"' UNION ALL
SELECT '1793', 'W2250202', 'Lid, Curb Box 2"' UNION ALL
SELECT '1794', 'W2251C1C', 'Lid, Curb Box 1 1/4"' UNION ALL
SELECT '1795', 'W2251F1F', 'Lid, Curb Box 1 1/2"' UNION ALL
SELECT '1796', 'W2260000', 'Curb Box, Complete PL Screw' UNION ALL
SELECT '1797', 'W2262F2F', 'Curb Box,  2 1/2" Complete PL Screw' UNION ALL
SELECT '1798', 'W2270000', 'Curb Box, Top PL Screw' UNION ALL
SELECT '1799', 'W2280000', 'Curb Box, Bottom PL Screw' UNION ALL
SELECT '1800', 'W2292F2F', 'Curb Box,  2 1/2" Complete CI Screw' UNION ALL
SELECT '1801', 'W2332F2F', 'Extension, 2 1/2" Curb CI Screw' UNION ALL
SELECT '1802', 'W2340000', 'Extension, Curb Big Base' UNION ALL
SELECT '1803', 'W2352008', 'Riser, 20" X  8" Monitor Cover' UNION ALL
SELECT '1804', 'W2360101', 'Rod, 1" Curb Box' UNION ALL
SELECT '1805', 'W2371F1F', 'Valve Box Riser,  1 1/2"' UNION ALL
SELECT '1806', 'W2385C10', 'Box Valve, Top 5 1/4" x 10" CI SL' UNION ALL
SELECT '1807', 'W2405C48', 'Box, 5 1/4" X 48" Valve Complete PL SL' UNION ALL
SELECT '1808', 'W2410000', 'Box, Valve Top PL SL' UNION ALL
SELECT '1809', 'W2420000', 'Box, Valve Bottom PL SL' UNION ALL
SELECT '1810', 'W2430000', 'Box, Valve Complete CI 22 Screw 39"-60"' UNION ALL
SELECT '1811', 'W2434C4C', 'Box, Valve Complete 4 1/4" CI Screw' UNION ALL
SELECT '1812', 'W2435C5C', 'Box, Valve Complete 5 1/4" CI Screw' UNION ALL
SELECT '1813', 'W2440612', 'Box, Valve  6" X 12" Top CI Screw' UNION ALL
SELECT '1814', 'W2441012', 'Box, Valve 10" X 12" Top CI Screw' UNION ALL
SELECT '1815', 'W2441212', 'Box, Valve 12" X 12" Top CI Screw' UNION ALL
SELECT '1816', 'W2442C24', 'Box, Valve  2 1/4" X 24" Valve Complete PL SL' UNION ALL
SELECT '1817', 'W2445C00', 'Box, Valve Top CI Screw' UNION ALL
SELECT '1818', 'W2445C16', 'Box, Valve  5 1/4" X 16" Valve Complete PL SL' UNION ALL
SELECT '1819', 'W2445C24', 'Box, Valve  5 1/4" X 24" Top CI Screw' UNION ALL
SELECT '1820', 'W2445C36', 'Box, Valve  5 1/4" X 36" Top CI Screw' UNION ALL
SELECT '1821', 'W2445C5C', 'Box, Valve  5 1/4" Top CI Screw' UNION ALL
SELECT '1822', 'W2445CWL', 'Box, Valve Top 5 1/4" CI Screw with lid' UNION ALL
SELECT '1823', 'W2450000', 'Box, Valve Bottom CI Screw' UNION ALL
SELECT '1824', 'W2455C13', 'Box, 5 1/4" X 13" Valve Bottom CI Screw' UNION ALL
SELECT '1825', 'W2455C24', 'Box, 5 1/4" X 24" Valve Bottom CI Screw' UNION ALL
SELECT '1826', 'W2455C36', 'Box, 5 1/4" X 36" Valve Bottom CI Screw' UNION ALL
SELECT '1827', 'W2465C5C', 'Extension, 5 1/4" Valve Box PL SL' UNION ALL
SELECT '1828', 'W2474C24', 'Extension, 4 1/4" X 24" Valve Box CI SL' UNION ALL
SELECT '1829', 'W2480000', 'Box, Valve Large Base PL Screw' UNION ALL
SELECT '1830', 'W249060H', 'Riser, Valve Box Screw  6" X  3/4"' UNION ALL
SELECT '1831', 'W2491000', 'Riser, Valve Box Screw  0-10"' UNION ALL
SELECT '1832', 'W2492400', 'Riser, Valve Box Screw 24"' UNION ALL
SELECT '1833', 'W2495C01', 'Riser, Valve Box Screw  5 1/4" X  1"'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 23.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1834', 'W2495C02', 'Riser, Valve Box Screw  5 1/4" X  2"' UNION ALL
SELECT '1835', 'W2495C06', 'Riser, Valve Box Screw  5 1/4" X  6"' UNION ALL
SELECT '1836', 'W2495C08', 'Riser, Valve Box Screw  5 1/4" X  8"' UNION ALL
SELECT '1837', 'W2495C12', 'Riser, Valve Box Screw  5 1/4" X 12"' UNION ALL
SELECT '1838', 'W2500612', 'Riser,  6" X 12" Valve Box SL' UNION ALL
SELECT '1839', 'W2505C01', 'Riser,  5 1/4" X  1" Valve Box SL' UNION ALL
SELECT '1840', 'W2505C02', 'Riser,  5 1/4" X  2" Valve Box SL' UNION ALL
SELECT '1841', 'W2505C08', 'Riser,  5 1/4" X  8" Valve Box SL' UNION ALL
SELECT '1842', 'W2505C10', 'Riser,  5 1/4" X 10" Valve Box SL' UNION ALL
SELECT '1843', 'W2505C12', 'Riser,  5 1/4" X 12" Valve Box SL' UNION ALL
SELECT '1844', 'W2505C1F', 'Riser,  5 1/4" X  1 1/2" Valve Box SL' UNION ALL
SELECT '1845', 'W2505C24', 'Riser,  5 1/4" X 24" Valve Box SL' UNION ALL
SELECT '1846', 'W2515C5C', 'Box, Valve Complete CI SL' UNION ALL
SELECT '1847', 'W2550606', 'Lid, Valve Box  6" CI' UNION ALL
SELECT '1848', 'W2550808', 'Lid, Valve Box  8" CI' UNION ALL
SELECT '1849', 'W2550909', 'Lid, Valve Box  9" CI' UNION ALL
SELECT '1850', 'W2551010', 'Lid, Valve Box 10" CI' UNION ALL
SELECT '1851', 'W2551212', 'Lid, Valve Box 12" CI' UNION ALL
SELECT '1852', 'W2555C5C', 'Lid, Valve Box  5 1/4" CI' UNION ALL
SELECT '1853', 'W2555C5C', 'Lid, Valve Box  5 1/4" CI' UNION ALL
SELECT '1854', 'W2555C5C', 'Lid, Valve Box  5 1/4" CI' UNION ALL
SELECT '1855', 'W2565C00', 'Base, 5 1/4" VB CI Round' UNION ALL
SELECT '1856', 'W2570630', 'Box, 6" X 30" Valve Bottom Split GALV' UNION ALL
SELECT '1857', 'W2585C18', 'Box, 5 1/4" X 18" Middle CI Screw' UNION ALL
SELECT '1858', 'W2585C24', 'Box, 5 1/4" X 24" Middle CI Screw' UNION ALL
SELECT '1859', 'W2594C18', 'Extension, 4 1/4" X 18" Valve Box CI Screw' UNION ALL
SELECT '1860', 'W2594C24', 'Extension, 4 1/4" X 24" Valve Box CI Screw' UNION ALL
SELECT '1861', 'W2595C12', 'Extension, 5 1/4" X 12" Valve Box CI Screw' UNION ALL
SELECT '1862', 'W2595C18', 'Extension, 5 1/4" X 18" Valve Box CI Screw' UNION ALL
SELECT '1863', 'W2595C24', 'Extension, 5 1/4" X 24" Valve Box CI Screw' UNION ALL
SELECT '1864', 'W2605C00', 'Lid,  5 1/4" Valve Box Locking' UNION ALL
SELECT '1865', 'W2620612', 'Box, 6" X 12"Valve Top GALV' UNION ALL
SELECT '1866', 'W2620812', 'Box, 8" X 12" Valve Top GALV' UNION ALL
SELECT '1867', 'W2630630', 'Box, 6" X 30" Valve Bottom  GALV' UNION ALL
SELECT '1868', 'W2630830', 'Box, 8" X 30" Valve Bottom  GALV' UNION ALL
SELECT '1869', 'W2640000', 'Ring, Valve Box Alignment' UNION ALL
SELECT '1870', 'W2770303', 'Coupling, Transition  3" DI X AC' UNION ALL
SELECT '1871', 'W2770404', 'Coupling, Transition  4" DI X AC' UNION ALL
SELECT '1872', 'W2770606', 'Coupling, Transition  6" DI X AC' UNION ALL
SELECT '1873', 'W2770806', 'Coupling, Transition  8" X 6" DI X AC' UNION ALL
SELECT '1874', 'W2770808', 'Coupling, Transition  8" DI X AC' UNION ALL
SELECT '1875', 'W2771010', 'Coupling, Transition 10" DI X AC' UNION ALL
SELECT '1876', 'W2771212', 'Coupling, Transition 12" DI X AC' UNION ALL
SELECT '1877', 'W2771414', 'Coupling, Transition 14" DI X AC' UNION ALL
SELECT '1878', 'W2771616', 'Coupling, Transition 16" DI X AC' UNION ALL
SELECT '1879', 'W2771818', 'Coupling, Transition 18" DI X AC' UNION ALL
SELECT '1880', 'W2772020', 'Coupling, Transition 20" DI X AC' UNION ALL
SELECT '1881', 'W2772424', 'Coupling, Transition 24" DI X AC' UNION ALL
SELECT '1882', 'W2780303', 'Coupling, Transition  3" DI X PVC' UNION ALL
SELECT '1883', 'W2780404', 'Coupling, Transition  4" DI X PVC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 24.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1884', 'W2780604', 'Coupling, Transition  6" X 4" DI X PVC' UNION ALL
SELECT '1885', 'W2780606', 'Coupling, Transition  6" DI X PVC' UNION ALL
SELECT '1886', 'W2780808', 'Coupling, Transition  8" DI X PVC' UNION ALL
SELECT '1887', 'W2781010', 'Coupling, Transition 10" DI X PVC' UNION ALL
SELECT '1888', 'W2781212', 'Coupling, Transition 12" DI X PVC' UNION ALL
SELECT '1889', 'W2781616', 'Coupling, Transition 16" DI X PVC' UNION ALL
SELECT '1890', 'W2782020', 'Coupling, Transition 20" DI X PVC' UNION ALL
SELECT '1891', 'W2790404', 'Coupling, Transition  4" DI X CI' UNION ALL
SELECT '1892', 'W2790606', 'Coupling, Transition  6" DI X CI' UNION ALL
SELECT '1893', 'W2790808', 'Coupling, Transition  8" DI X CI' UNION ALL
SELECT '1894', 'W2791010', 'Coupling, Transition 10" DI X CI' UNION ALL
SELECT '1895', 'W2791212', 'Coupling, Transition 12" DI X CI' UNION ALL
SELECT '1896', 'W2791414', 'Coupling, Transition 14" DI X CI' UNION ALL
SELECT '1897', 'W2791616', 'Coupling, Transition 16" DI X CI' UNION ALL
SELECT '1898', 'W2792424', 'Coupling, Transition 24" DI X CI' UNION ALL
SELECT '1899', 'W2830101', 'Coupling,  1" FLEX Bolted ST LP' UNION ALL
SELECT '1900', 'W2840606', 'Coupling,  6" FLEX Bolted DI LP' UNION ALL
SELECT '1901', 'W2850202', 'Coupling,  2" FLEX Bolted DI' UNION ALL
SELECT '1902', 'W285022C', 'Coupling,  2" X 2 1/4" FLEX Bolted DI' UNION ALL
SELECT '1903', 'W2850303', 'Coupling,  3" FLEX Bolted DI' UNION ALL
SELECT '1904', 'W2850403', 'Coupling,  4" X  3" FLEX Bolted DI' UNION ALL
SELECT '1905', 'W2850404', 'Coupling,  4" FLEX Bolted DI' UNION ALL
SELECT '1906', 'W2850604', 'Coupling,  6" X  4" FLEX Bolted DI' UNION ALL
SELECT '1907', 'W2850806', 'Coupling,  8" X  6" FLEX Bolted DI' UNION ALL
SELECT '1908', 'W2850808', 'Coupling,  8" FLEX Bolted DI' UNION ALL
SELECT '1909', 'W2851010', 'Coupling, 10" FLEX Bolted DI' UNION ALL
SELECT '1910', 'W2851212', 'Coupling, 12" FLEX Bolted DI' UNION ALL
SELECT '1911', 'W2851414', 'Coupling, 14" FLEX Bolted DI' UNION ALL
SELECT '1912', 'W2851616', 'Coupling, 16" FLEX Bolted DI' UNION ALL
SELECT '1913', 'W2851F1F', 'Coupling,  1 1/2" FLEX Bolted DI' UNION ALL
SELECT '1914', 'W2852020', 'Coupling, 20" FLEX Bolted DI' UNION ALL
SELECT '1915', 'W2852424', 'Coupling, 24" FLEX Bolted DI' UNION ALL
SELECT '1916', 'W2852C2C', 'Coupling,  2 1/4" FLEX Bolted DI' UNION ALL
SELECT '1917', 'W2852F2F', 'Coupling,  2 1/2" FLEX Bolted DI' UNION ALL
SELECT '1918', 'W2853030', 'Coupling, 30" FLEX Bolted DI' UNION ALL
SELECT '1919', 'W2853636', 'Coupling, 36" FLEX Bolted DI' UNION ALL
SELECT '1920', 'W2854242', 'Coupling, 42" FLEX Bolted DI' UNION ALL
SELECT '1921', 'W2854848', 'Coupling, 48" FLEX Bolted DI' UNION ALL
SELECT '1922', 'W2856060', 'Coupling, 60" FLEX Bolted DI' UNION ALL
SELECT '1923', 'W2860303', 'Coupling,  3" FLEX Bolted AC' UNION ALL
SELECT '1924', 'W2860404', 'Coupling,  4" FLEX Bolted AC' UNION ALL
SELECT '1925', 'W2860606', 'Coupling,  6" FLEX Bolted AC' UNION ALL
SELECT '1926', 'W2860806', 'Coupling,  8" X  6" FLEX Bolted AC' UNION ALL
SELECT '1927', 'W2860808', 'Coupling,  8" FLEX Bolted AC' UNION ALL
SELECT '1928', 'W2861010', 'Coupling, 10" FLEX Bolted AC' UNION ALL
SELECT '1929', 'W2861212', 'Coupling, 12" FLEX Bolted AC' UNION ALL
SELECT '1930', 'W2861616', 'Coupling, 16" FLEX Bolted AC' UNION ALL
SELECT '1931', 'W2861818', 'Coupling, 18" FLEX Bolted AC' UNION ALL
SELECT '1932', 'W2862020', 'Coupling, 20" FLEX Bolted AC' UNION ALL
SELECT '1933', 'W2870101', 'Coupling,  1" FLEX Bolted ST SP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 25.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1934', 'W2870202', 'Coupling,  2" FLEX Bolted ST SP' UNION ALL
SELECT '1935', 'W2870303', 'Coupling,  3" FLEX Bolted ST SP' UNION ALL
SELECT '1936', 'W2870404', 'Coupling,  4" FLEX Bolted ST SP' UNION ALL
SELECT '1937', 'W2870606', 'Coupling,  6" FLEX Bolted ST SP' UNION ALL
SELECT '1938', 'W2870808', 'Coupling,  8" FLEX Bolted ST SP' UNION ALL
SELECT '1939', 'W2871010', 'Coupling, 10" FLEX Bolted ST SP' UNION ALL
SELECT '1940', 'W2871212', 'Coupling, 12" FLEX Bolted ST SP' UNION ALL
SELECT '1941', 'W2871616', 'Coupling, 16" FLEX Bolted ST SP' UNION ALL
SELECT '1942', 'W2872020', 'Coupling, 20" FLEX Bolted ST SP' UNION ALL
SELECT '1943', 'W2872424', 'Coupling, 24" FLEX Bolted ST SP' UNION ALL
SELECT '1944', 'W2880202', 'Coupling,  2" FLEX Bolted PVC' UNION ALL
SELECT '1945', 'W2880303', 'Coupling,  3" FLEX Bolted PVC' UNION ALL
SELECT '1946', 'W2880404', 'Coupling,  4" FLEX Bolted PVC' UNION ALL
SELECT '1947', 'W2880606', 'Coupling,  6" FLEX Bolted PVC' UNION ALL
SELECT '1948', 'W2880808', 'Coupling,  8" FLEX Bolted PVC' UNION ALL
SELECT '1949', 'W2881010', 'Coupling, 10" FLEX Bolted PVC' UNION ALL
SELECT '1950', 'W2881212', 'Coupling, 12" FLEX Bolted PVC' UNION ALL
SELECT '1951', 'W2890202', 'Coupling,  2" Trans DI X IPS' UNION ALL
SELECT '1952', 'W2890402', 'Coupling,  4" X  2" Trans DI X IPS' UNION ALL
SELECT '1953', 'W2890404', 'Coupling,  4" X  4" Trans DI X IPS' UNION ALL
SELECT '1954', 'W2890602', 'Coupling,  6" X  2" Trans DI X IPS' UNION ALL
SELECT '1955', 'W2890606', 'Coupling,  6" Trans DI X IPS' UNION ALL
SELECT '1956', 'W2890802', 'Coupling,  8" X  2" Trans DI X IPS' UNION ALL
SELECT '1957', 'W2890808', 'Coupling,  8" Trans DI X IPS' UNION ALL
SELECT '1958', 'W2891010', 'Coupling, 10" Trans DI X IPS' UNION ALL
SELECT '1959', 'W2891212', 'Coupling, 12" Trans DI X IPS' UNION ALL
SELECT '1960', 'W2891616', 'Coupling, 16" Trans DI X IPS' UNION ALL
SELECT '1961', 'W2892020', 'Coupling, 20" Trans DI X IPS' UNION ALL
SELECT '1962', 'W2892424', 'Coupling, 24" Trans DI X IPS' UNION ALL
SELECT '1963', 'W2892F2F', 'Coupling,  2 1/2" Trans DI X IPS' UNION ALL
SELECT '1964', 'W2893030', 'Coupling, 30" Trans DI X IPS' UNION ALL
SELECT '1965', 'W2900606', 'Coupling,  6" Trans AC X PVC' UNION ALL
SELECT '1966', 'W2900808', 'Coupling,  8" Trans AC X PVC' UNION ALL
SELECT '1967', 'W2920202', 'Coupling,  2" Trans one bolt' UNION ALL
SELECT '1968', 'W2920303', 'Coupling,  3" Trans one bolt' UNION ALL
SELECT '1969', 'W2920404', 'Coupling,  4" Trans one bolt' UNION ALL
SELECT '1970', 'W2920606', 'Coupling,  6" Trans one bolt' UNION ALL
SELECT '1971', 'W2920808', 'Coupling,  8" Trans one bolt' UNION ALL
SELECT '1972', 'W2921010', 'Coupling, 10" Trans one bolt' UNION ALL
SELECT '1973', 'W2921212', 'Coupling, 12" Trans one bolt' UNION ALL
SELECT '1974', 'W2921414', 'Coupling, 14" Trans one bolt' UNION ALL
SELECT '1975', 'W2921616', 'Coupling, 16" Trans one bolt' UNION ALL
SELECT '1976', 'W2921818', 'Coupling, 18" Trans one bolt' UNION ALL
SELECT '1977', 'W2922020', 'Coupling, 20" Trans one bolt' UNION ALL
SELECT '1978', 'W2922424', 'Coupling, 24" Trans one bolt' UNION ALL
SELECT '1979', 'W2980808', 'Casing,  8" Pipe ST' UNION ALL
SELECT '1980', 'W2981212', 'Casing, 12" Pipe ST' UNION ALL
SELECT '1981', 'W2981616', 'Casing, 16" Pipe  ST' UNION ALL
SELECT '1982', 'W2982020', 'Casing, 20" Pipe ST' UNION ALL
SELECT '1983', 'W2982424', 'Casing, 24" Pipe ST'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 26.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '1984', 'W2983030', 'Casing, 30" Pipe ST' UNION ALL
SELECT '1985', 'W2983636', 'Casing, 36" Pipe ST' UNION ALL
SELECT '1986', 'W2984444', 'Casing, 44" Pipe ST' UNION ALL
SELECT '1987', 'W2984848', 'Casing, 48" Pipe ST' UNION ALL
SELECT '1988', 'W2991010', 'Casing, 10" Pipe PVC' UNION ALL
SELECT '1989', 'W2991212', 'Casing, 12" Pipe PVC' UNION ALL
SELECT '1990', 'W2991616', 'Casing, 16" Pipe PVC' UNION ALL
SELECT '1991', 'W2991818', 'Casing, 18" Pipe PVC' UNION ALL
SELECT '1992', 'W2992020', 'Casing, 20" Pipe PVC' UNION ALL
SELECT '1993', 'W2992424', 'Casing, 24" Pipe PVC' UNION ALL
SELECT '1994', 'W2993030', 'Casing, 30" Pipe PVC' UNION ALL
SELECT '1995', 'W3000202', 'Sleeve,  2" Solid MJ LP' UNION ALL
SELECT '1996', 'W3000303', 'Sleeve,  3" Solid MJ LP' UNION ALL
SELECT '1997', 'W3001414', 'Sleeve, 14" Solid MJ LP' UNION ALL
SELECT '1998', 'W3001415', 'Sleeve, 14" X 15" Solid MJ LP' UNION ALL
SELECT '1999', 'W3002C2C', 'Sleeve,  2 1/4" Solid MJ LP' UNION ALL
SELECT '2000', 'W3003624', 'Sleeve, 36" X 24"  Solid MJ LP' UNION ALL
SELECT '2001', 'W3004242', 'Sleeve, 42" Solid MJ LP' UNION ALL
SELECT '2002', 'W3004824', 'Sleeve, 48" X 24" Solid MJ LP' UNION ALL
SELECT '2003', 'W3004848', 'Sleeve, 48" Solid MJ LP' UNION ALL
SELECT '2004', 'W3005454', 'Sleeve, 54" Solid MJ LP' UNION ALL
SELECT '2005', 'W3010303', 'Sleeve,  3" Solid MJ SP' UNION ALL
SELECT '2006', 'W3010404', 'Sleeve,  4" Solid MJ SP' UNION ALL
SELECT '2007', 'W3010606', 'Sleeve,  6" Solid MJ SP' UNION ALL
SELECT '2008', 'W3010808', 'Sleeve,  8" Solid MJ SP' UNION ALL
SELECT '2009', 'W3011010', 'Sleeve, 10" Solid MJ SP' UNION ALL
SELECT '2010', 'W3011212', 'Sleeve, 12" Solid MJ SP' UNION ALL
SELECT '2011', 'W3011414', 'Sleeve, 14" Solid MJ SP' UNION ALL
SELECT '2012', 'W3011616', 'Sleeve, 16" Solid MJ SP' UNION ALL
SELECT '2013', 'W3011818', 'Sleeve, 18" Solid MJ SP' UNION ALL
SELECT '2014', 'W3012020', 'Sleeve, 20" Solid MJ SP' UNION ALL
SELECT '2015', 'W3012424', 'Sleeve, 24" Solid MJ SP' UNION ALL
SELECT '2016', 'W3013636', 'Sleeve, 36" Solid MJ SP' UNION ALL
SELECT '2017', 'W3014848', 'Sleeve, 48" Solid MJ SP' UNION ALL
SELECT '2018', 'W3020101', 'Coupling,  1" PVC Slip' UNION ALL
SELECT '2019', 'W3020202', 'Coupling,  2" PVC Slip' UNION ALL
SELECT '2020', 'W3020202', 'Coupling,  2" PVC Slip' UNION ALL
SELECT '2021', 'W3020303', 'Coupling,  3" PVC Slip' UNION ALL
SELECT '2022', 'W3020404', 'Coupling,  4" PVC Slip' UNION ALL
SELECT '2023', 'W3020606', 'Coupling,  6" PVC Slip' UNION ALL
SELECT '2024', 'W3020808', 'Coupling,  8" PVC Slip' UNION ALL
SELECT '2025', 'W3020H0H', 'Coupling,   3/4" PVC Slip' UNION ALL
SELECT '2026', 'W3021212', 'Coupling, 12" PVC Slip' UNION ALL
SELECT '2027', 'W3021C1C', 'Coupling,  1 1/4" PVC Slip' UNION ALL
SELECT '2028', 'W3021F1F', 'Coupling,  1 1/2" PVC Slip' UNION ALL
SELECT '2029', 'W3031010', 'Sleeve, 10" Solid MJ FB LP' UNION ALL
SELECT '2030', 'W3031212', 'Sleeve, 12" Solid MJ FB LP' UNION ALL
SELECT '2031', 'W3031414', 'Sleeve, 14" Solid MJ FB LP' UNION ALL
SELECT '2032', 'W3031616', 'Sleeve, 16" Solid MJ FB LP' UNION ALL
SELECT '2033', 'W3031818', 'Sleeve, 18" Solid MJ FB LP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 27.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2034', 'W3032020', 'Sleeve, 20" Solid MJ FB LP' UNION ALL
SELECT '2035', 'W3032424', 'Sleeve, 24" Solid MJ FB LP' UNION ALL
SELECT '2036', 'W3033030', 'Sleeve, 30" Solid MJ FB LP' UNION ALL
SELECT '2037', 'W3033636', 'Sleeve, 36" Solid MJ FB LP' UNION ALL
SELECT '2038', 'W3034242', 'Sleeve, 42" Solid MJ FB LP' UNION ALL
SELECT '2039', 'W3034848', 'Sleeve, 48" Solid MJ FB LP' UNION ALL
SELECT '2040', 'W3040202', 'Sleeve,  2" MJ DP' UNION ALL
SELECT '2041', 'W3040303', 'Sleeve,  3" MJ DP' UNION ALL
SELECT '2042', 'W3041414', 'Sleeve, 14" MJ DP' UNION ALL
SELECT '2043', 'W3041818', 'Sleeve, 18" MJ DP' UNION ALL
SELECT '2044', 'W3042020', 'Sleeve, 20" MJ DP' UNION ALL
SELECT '2045', 'W3042424', 'Sleeve, 24" MJ DP' UNION ALL
SELECT '2046', 'W3043030', 'Sleeve, 30" MJ DP' UNION ALL
SELECT '2047', 'W3043636', 'Sleeve, 36" MJ DP' UNION ALL
SELECT '2048', 'W3044242', 'Sleeve, 42" MJ DP' UNION ALL
SELECT '2049', 'W3050202', 'Sleeve,  2" Solid MJ FB SP' UNION ALL
SELECT '2050', 'W3050404', 'Sleeve,  4" Solid MJ FB SP' UNION ALL
SELECT '2051', 'W3050606', 'Sleeve,  6" Solid MJ FB SP' UNION ALL
SELECT '2052', 'W3050808', 'Sleeve,  8" Solid MJ FB SP' UNION ALL
SELECT '2053', 'W3061414', 'Sleeve, 14" TR FLEX' UNION ALL
SELECT '2054', 'W3061616', 'Sleeve, 16" TR FLEX' UNION ALL
SELECT '2055', 'W3062020', 'Sleeve, 20" TR FLEX' UNION ALL
SELECT '2056', 'W3062424', 'Sleeve, 24" TR FLEX' UNION ALL
SELECT '2057', 'W3070202', 'Sleeve,  2" RJ TLSCP' UNION ALL
SELECT '2058', 'W3070404', 'Sleeve,  4" RJ TLSCP' UNION ALL
SELECT '2059', 'W3070606', 'Sleeve,  6" RJ TLSCP' UNION ALL
SELECT '2060', 'W3070808', 'Sleeve,  8" RJ TLSCP' UNION ALL
SELECT '2061', 'W3071212', 'Sleeve, 12" RJ TLSCP' UNION ALL
SELECT '2062', 'W3071414', 'Sleeve, 14" RJ TLSCP' UNION ALL
SELECT '2063', 'W3071616', 'Sleeve, 16" RJ TLSCP' UNION ALL
SELECT '2064', 'W3072020', 'Sleeve, 20" RJ TLSCP' UNION ALL
SELECT '2065', 'W3090202', 'Sleeve,  2" Split MJ' UNION ALL
SELECT '2066', 'W3090303', 'Sleeve,  3" Split MJ' UNION ALL
SELECT '2067', 'W3090404', 'Sleeve,  4" Split MJ' UNION ALL
SELECT '2068', 'W3090606', 'Sleeve,  6" Split MJ' UNION ALL
SELECT '2069', 'W3090808', 'Sleeve,  8" Split MJ' UNION ALL
SELECT '2070', 'W3091010', 'Sleeve, 10" Split MJ' UNION ALL
SELECT '2071', 'W3091212', 'Sleeve, 12" Split MJ' UNION ALL
SELECT '2072', 'W3091414', 'Sleeve, 14" Split MJ' UNION ALL
SELECT '2073', 'W3091616', 'Sleeve, 16" Split MJ' UNION ALL
SELECT '2074', 'W3091818', 'Sleeve, 18" Split MJ' UNION ALL
SELECT '2075', 'W3092020', 'Sleeve, 20" Split MJ' UNION ALL
SELECT '2076', 'W3092424', 'Sleeve, 24" Split MJ' UNION ALL
SELECT '2077', 'W3100808', 'Sleeve,  8" Split MJ For ST' UNION ALL
SELECT '2078', 'W3101212', 'Sleeve, 12" Split MJ For ST' UNION ALL
SELECT '2079', 'W3101616', 'Sleeve, 16" Split MJ For ST' UNION ALL
SELECT '2080', 'W3102020', 'Sleeve, 20" Split MJ For ST' UNION ALL
SELECT '2081', 'W3102424', 'Sleeve, 24" Split MJ For ST' UNION ALL
SELECT '2082', 'W3103030', 'Sleeve, 30" Split MJ For ST' UNION ALL
SELECT '2083', 'W3103636', 'Sleeve, 36" Split MJ For ST'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 28.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2084', 'W3110202', 'Sleeve,  2" Trans CI X ST' UNION ALL
SELECT '2085', 'W3110404', 'Sleeve,  4" Trans CI X ST' UNION ALL
SELECT '2086', 'W3110606', 'Sleeve,  6" Trans CI X ST' UNION ALL
SELECT '2087', 'W3110808', 'Sleeve,  8" Trans CI X ST' UNION ALL
SELECT '2088', 'W3112C2C', 'Sleeve,  2 1/4" Trans CI X ST' UNION ALL
SELECT '2089', 'W3121616', 'Sleeve, 16" Solid SJ' UNION ALL
SELECT '2090', 'W3130404', 'Sleeve,  4" MJ Split over bell' UNION ALL
SELECT '2091', 'W3130606', 'Sleeve,  6" MJ Split over bell' UNION ALL
SELECT '2092', 'W3130808', 'Sleeve,  8" MJ Split over bell' UNION ALL
SELECT '2093', 'W3131010', 'Sleeve, 10" MJ Split over bell' UNION ALL
SELECT '2094', 'W3131212', 'Sleeve, 12" MJ Split over bell' UNION ALL
SELECT '2095', 'W3131414', 'Sleeve, 14" MJ Split over bell' UNION ALL
SELECT '2096', 'W3131616', 'Sleeve, 16" MJ Split over bell' UNION ALL
SELECT '2097', 'W3131818', 'Sleeve, 18" MJ Split over bell' UNION ALL
SELECT '2098', 'W3132020', 'Sleeve, 20" MJ Split over bell' UNION ALL
SELECT '2099', 'W3132424', 'Sleeve, 24" MJ Split over bell' UNION ALL
SELECT '2100', 'W3150606', 'Sleeve,  6" Split Repair Clamp LP' UNION ALL
SELECT '2101', 'W3150808', 'Sleeve,  8" Split Repair Clamp LP' UNION ALL
SELECT '2102', 'W3160606', 'Sleeve,  6" Split Repair Clamp SP' UNION ALL
SELECT '2103', 'W3160808', 'Sleeve,  8" Split Repair Clamp SP' UNION ALL
SELECT '2104', 'W3210606', 'Bend,  6" RJ  1 Bolt 90°' UNION ALL
SELECT '2105', 'W3210808', 'Bend,  8" RJ  1 Bolt 90°' UNION ALL
SELECT '2106', 'W3211212', 'Bend, 12" RJ  1 Bolt 90°' UNION ALL
SELECT '2107', 'W3220606', 'Bend,  6" RJ  1 Bolt 45°' UNION ALL
SELECT '2108', 'W3220808', 'Bend,  8" RJ  1 Bolt 45°' UNION ALL
SELECT '2109', 'W3221212', 'Bend, 12" RJ  1 Bolt 45°' UNION ALL
SELECT '2110', 'W3230606', 'Bend,  6" RJ  1 Bolt 22 1/2°' UNION ALL
SELECT '2111', 'W3230808', 'Bend,  8" RJ  1 Bolt 22 1/2°' UNION ALL
SELECT '2112', 'W3231212', 'Bend, 12" RJ  1 Bolt 22 1/2°' UNION ALL
SELECT '2113', 'W3240606', 'Bend,  6" RJ  1 Bolt 11 1/4°' UNION ALL
SELECT '2114', 'W3241212', 'Bend, 12" RJ  1 Bolt 11 1/4°' UNION ALL
SELECT '2115', 'W3250202', 'Bend,  2" MJ 90°' UNION ALL
SELECT '2116', 'W3250303', 'Bend,  3" MJ 90°' UNION ALL
SELECT '2117', 'W3251414', 'Bend, 14" MJ 90°' UNION ALL
SELECT '2118', 'W3252C2C', 'Bend,  2 1/4" MJ 90°' UNION ALL
SELECT '2119', 'W3260202', 'Bend,  2" MJ 45°' UNION ALL
SELECT '2120', 'W3260303', 'Bend,  3" MJ 45°' UNION ALL
SELECT '2121', 'W3261414', 'Bend, 14" MJ 45°' UNION ALL
SELECT '2122', 'W3262C2C', 'Bend,  2 1/4" MJ 45°' UNION ALL
SELECT '2123', 'W3262F2F', 'Bend,  2 1/2" MJ 45°' UNION ALL
SELECT '2124', 'W3264242', 'Bend, 42" MJ 45°' UNION ALL
SELECT '2125', 'W3270202', 'Bend,  2" MJ 22 1/2°' UNION ALL
SELECT '2126', 'W3270303', 'Bend,  3" MJ 22 1/2°' UNION ALL
SELECT '2127', 'W3271414', 'Bend, 14" MJ 22 1/2°' UNION ALL
SELECT '2128', 'W3271818', 'Bend, 18" MJ 22 1/2°' UNION ALL
SELECT '2129', 'W3272C2C', 'Bend,  2 1/4" MJ 22 1/2°' UNION ALL
SELECT '2130', 'W3274242', 'Bend, 42" MJ 22 1/2°' UNION ALL
SELECT '2131', 'W3280303', 'Bend,  3" MJ 11 1/4°' UNION ALL
SELECT '2132', 'W3281414', 'Bend, 14" MJ 11 1/4°' UNION ALL
SELECT '2133', 'W3281818', 'Bend, 18" MJ 11 1/4°'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 29.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2134', 'W3282C2C', 'Bend,  2 1/4" MJ 11 1/4°' UNION ALL
SELECT '2135', 'W3290606', 'Bend,  6" MJ Anchor 90°' UNION ALL
SELECT '2136', 'W3300303', 'Bend,  3" SJ 90°' UNION ALL
SELECT '2137', 'W3300404', 'Bend,  4" SJ 90°' UNION ALL
SELECT '2138', 'W3300606', 'Bend,  6" SJ 90°' UNION ALL
SELECT '2139', 'W3300808', 'Bend,  8" SJ 90°' UNION ALL
SELECT '2140', 'W3301010', 'Bend, 10" SJ 90°' UNION ALL
SELECT '2141', 'W3301212', 'Bend, 12" SJ 90°' UNION ALL
SELECT '2142', 'W3301414', 'Bend, 14" SJ 90°' UNION ALL
SELECT '2143', 'W3301616', 'Bend, 16" SJ 90°' UNION ALL
SELECT '2144', 'W3301818', 'Bend, 18" SJ 90°' UNION ALL
SELECT '2145', 'W3302020', 'Bend, 20" SJ 90°' UNION ALL
SELECT '2146', 'W3302424', 'Bend, 24" SJ 90°' UNION ALL
SELECT '2147', 'W3302C2C', 'Bend,  2 1/4" SJ 90°' UNION ALL
SELECT '2148', 'W3303030', 'Bend, 30" SJ 90°' UNION ALL
SELECT '2149', 'W3303636', 'Bend, 36" SJ 90°' UNION ALL
SELECT '2150', 'W3310404', 'Bend,  4" SJ 45°' UNION ALL
SELECT '2151', 'W3310606', 'Bend,  6" SJ 45°' UNION ALL
SELECT '2152', 'W3310808', 'Bend,  8" SJ 45°' UNION ALL
SELECT '2153', 'W3311010', 'Bend, 10" SJ 45°' UNION ALL
SELECT '2154', 'W3311212', 'Bend, 12" SJ 45°' UNION ALL
SELECT '2155', 'W3311414', 'Bend, 14" SJ 45°' UNION ALL
SELECT '2156', 'W3311616', 'Bend, 16" SJ 45°' UNION ALL
SELECT '2157', 'W3311818', 'Bend, 18" SJ 45°' UNION ALL
SELECT '2158', 'W3312020', 'Bend, 20" SJ 45°' UNION ALL
SELECT '2159', 'W3312424', 'Bend, 24" SJ 45°' UNION ALL
SELECT '2160', 'W3312C2C', 'Bend,  2 1/4" SJ 45°' UNION ALL
SELECT '2161', 'W3313030', 'Bend, 30" SJ 45°' UNION ALL
SELECT '2162', 'W3313636', 'Bend, 36" SJ 45°' UNION ALL
SELECT '2163', 'W3320303', 'Bend,  3" SJ 22 1/2°' UNION ALL
SELECT '2164', 'W3320404', 'Bend,  4" SJ 22 1/2°' UNION ALL
SELECT '2165', 'W3320606', 'Bend,  6" SJ 22 1/2°' UNION ALL
SELECT '2166', 'W3320808', 'Bend,  8" SJ 22 1/2°' UNION ALL
SELECT '2167', 'W3321010', 'Bend, 10" SJ 22 1/2°' UNION ALL
SELECT '2168', 'W3321212', 'Bend, 12" SJ 22 1/2°' UNION ALL
SELECT '2169', 'W3321414', 'Bend, 14" SJ 22 1/2°' UNION ALL
SELECT '2170', 'W3321616', 'Bend, 16" SJ 22 1/2°' UNION ALL
SELECT '2171', 'W3321818', 'Bend, 18" SJ 22 1/2°' UNION ALL
SELECT '2172', 'W3322020', 'Bend, 20" SJ 22 1/2°' UNION ALL
SELECT '2173', 'W3322424', 'Bend, 24" SJ 22 1/2°' UNION ALL
SELECT '2174', 'W3322C2C', 'Bend,  2 1/4" SJ 22 1/2°' UNION ALL
SELECT '2175', 'W3323030', 'Bend, 30" SJ 22 1/2°' UNION ALL
SELECT '2176', 'W3323636', 'Bend, 36" SJ 22 1/2°' UNION ALL
SELECT '2177', 'W3324242', 'Bend, 42" SJ 22 1/2°' UNION ALL
SELECT '2178', 'W3326060', 'Bend, 60" SJ 22 1/2°' UNION ALL
SELECT '2179', 'W3330404', 'Bend,  4" SJ 11 1/4°' UNION ALL
SELECT '2180', 'W3330606', 'Bend,  6" SJ 11 1/4°' UNION ALL
SELECT '2181', 'W3330808', 'Bend,  8" SJ 11 1/4°' UNION ALL
SELECT '2182', 'W3331010', 'Bend, 10" SJ 11 1/4°' UNION ALL
SELECT '2183', 'W3331212', 'Bend, 12" SJ 11 1/4°'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 30.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2184', 'W3331414', 'Bend, 14" SJ 11 1/4°' UNION ALL
SELECT '2185', 'W3331616', 'Bend, 16" SJ 11 1/4°' UNION ALL
SELECT '2186', 'W3331818', 'Bend, 18" SJ 11 1/4°' UNION ALL
SELECT '2187', 'W3332020', 'Bend, 20" SJ 11 1/4°' UNION ALL
SELECT '2188', 'W3332C2C', 'Bend,  2 1/4" SJ 11 1/4°' UNION ALL
SELECT '2189', 'W3333030', 'Bend, 30" SJ 11 1/4°' UNION ALL
SELECT '2190', 'W3333636', 'Bend, 36" SJ 11 1/4°' UNION ALL
SELECT '2191', 'W3334242', 'Bend, 42" SJ 11 1/4°' UNION ALL
SELECT '2192', 'W3335454', 'Bend, 54" SJ 11 1/4°' UNION ALL
SELECT '2193', 'W3350606', 'Bend,  6" RJ 90°' UNION ALL
SELECT '2194', 'W3350808', 'Bend,  8" RJ 90°' UNION ALL
SELECT '2195', 'W3351616', 'Bend, 16" RJ 90°' UNION ALL
SELECT '2196', 'W3353030', 'Bend, 30" RJ 90°' UNION ALL
SELECT '2197', 'W3354848', 'Bend, 48" RJ 90°' UNION ALL
SELECT '2198', 'W3355454', 'Bend, 54" RJ 90°' UNION ALL
SELECT '2199', 'W3364242', 'Bend, 42" RJ 45°' UNION ALL
SELECT '2200', 'W3364848', 'Bend, 48" RJ 45°' UNION ALL
SELECT '2201', 'W3365454', 'Bend, 54" RJ 45°' UNION ALL
SELECT '2202', 'W3370808', 'Bend,  8" RJ 22 1/2°' UNION ALL
SELECT '2203', 'W3374242', 'Bend, 42" RJ 22 1/2°' UNION ALL
SELECT '2204', 'W3374848', 'Bend, 48" RJ 22 1/2°' UNION ALL
SELECT '2205', 'W3375454', 'Bend, 54" RJ 22 1/2°' UNION ALL
SELECT '2206', 'W3380808', 'Bend,  8" RJ 11 1/4°' UNION ALL
SELECT '2207', 'W3384242', 'Bend, 42" RJ 11 1/4°' UNION ALL
SELECT '2208', 'W3384848', 'Bend, 48" RJ 11 1/4°' UNION ALL
SELECT '2209', 'W3385454', 'Bend, 54" RJ 11 1/4°' UNION ALL
SELECT '2210', 'W3390606', 'Bend,  6" RJ  5 5/8°' UNION ALL
SELECT '2211', 'W3390808', 'Bend,  8" RJ  5 5/8°' UNION ALL
SELECT '2212', 'W3391212', 'Bend, 12" RJ  5 5/8°' UNION ALL
SELECT '2213', 'W3391616', 'Bend, 16" RJ  5 5/8°' UNION ALL
SELECT '2214', 'W3392020', 'Bend, 20" RJ  5 5/8°' UNION ALL
SELECT '2215', 'W3392424', 'Bend, 24" RJ  5 5/8°' UNION ALL
SELECT '2216', 'W3393030', 'Bend, 30" RJ  5 5/8°' UNION ALL
SELECT '2217', 'W3393636', 'Bend, 36" RJ  5 5/8°' UNION ALL
SELECT '2218', 'W3394848', 'Bend, 48" RJ  5 5/8°' UNION ALL
SELECT '2219', 'W3395454', 'Bend, 54" RJ  5 5/8°' UNION ALL
SELECT '2220', 'W3410202', 'Bend,  2" FLG 90°' UNION ALL
SELECT '2221', 'W3410303', 'Bend,  3" FLG 90°' UNION ALL
SELECT '2222', 'W3410404', 'Bend,  4" FLG 90°' UNION ALL
SELECT '2223', 'W3410606', 'Bend,  6" FLG 90°' UNION ALL
SELECT '2224', 'W3410808', 'Bend,  8" FLG 90°' UNION ALL
SELECT '2225', 'W3411006', 'Bend, 10" X 6" FLG 90°' UNION ALL
SELECT '2226', 'W3411008', 'Bend, 10" X 8" FLG 90°' UNION ALL
SELECT '2227', 'W3411010', 'Bend, 10" FLG 90°' UNION ALL
SELECT '2228', 'W3411208', 'Bend, 12" X  8" FLG 90°' UNION ALL
SELECT '2229', 'W3411210', 'Bend, 12" X 10" FLG 90°' UNION ALL
SELECT '2230', 'W3411212', 'Bend, 12" FLG 90°' UNION ALL
SELECT '2231', 'W3411414', 'Bend, 14" FLG 90°' UNION ALL
SELECT '2232', 'W3411616', 'Bend, 16" FLG 90°' UNION ALL
SELECT '2233', 'W3411818', 'Bend, 18" FLG 90°'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 31.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2234', 'W3412020', 'Bend, 20" FLG 90°' UNION ALL
SELECT '2235', 'W3412424', 'Bend, 24" FLG 90°' UNION ALL
SELECT '2236', 'W3420202', 'Bend,  2" FLG 45°' UNION ALL
SELECT '2237', 'W3420404', 'Bend,  4" FLG 45°' UNION ALL
SELECT '2238', 'W3420606', 'Bend,  6" FLG 45°' UNION ALL
SELECT '2239', 'W3420808', 'Bend,  8" FLG 45°' UNION ALL
SELECT '2240', 'W3421010', 'Bend, 10" FLG 45°' UNION ALL
SELECT '2241', 'W3421212', 'Bend, 12" FLG 45°' UNION ALL
SELECT '2242', 'W3421414', 'Bend, 14" FLG 45°' UNION ALL
SELECT '2243', 'W3421616', 'Bend, 16" FLG 45°' UNION ALL
SELECT '2244', 'W3421818', 'Bend, 18" FLG 45°' UNION ALL
SELECT '2245', 'W3422020', 'Bend, 20" FLG 45°' UNION ALL
SELECT '2246', 'W3422424', 'Bend, 24" FLG 45°' UNION ALL
SELECT '2247', 'W3430202', 'Bend,  2" FLG 22 1/2°' UNION ALL
SELECT '2248', 'W3430404', 'Bend,  4" FLG 22 1/2°' UNION ALL
SELECT '2249', 'W3430606', 'Bend,  6" FLG 22 1/2°' UNION ALL
SELECT '2250', 'W3430808', 'Bend,  8" FLG 22 1/2°' UNION ALL
SELECT '2251', 'W3431010', 'Bend, 10" FLG 22 1/2°' UNION ALL
SELECT '2252', 'W3431212', 'Bend, 12" FLG 22 1/2°' UNION ALL
SELECT '2253', 'W3431414', 'Bend, 14" FLG 22 1/2°' UNION ALL
SELECT '2254', 'W3431616', 'Bend, 16" FLG 22 1/2°' UNION ALL
SELECT '2255', 'W3431818', 'Bend, 18" FLG 22 1/2°' UNION ALL
SELECT '2256', 'W3432020', 'Bend, 20" FLG 22 1/2°' UNION ALL
SELECT '2257', 'W3432424', 'Bend, 24" FLG 22 1/2°' UNION ALL
SELECT '2258', 'W3440202', 'Bend,  2" FLG 11 1/4°' UNION ALL
SELECT '2259', 'W3440404', 'Bend,  4" FLG 11 1/4°' UNION ALL
SELECT '2260', 'W3440808', 'Bend,  8" FLG 11 1/4°' UNION ALL
SELECT '2261', 'W3441010', 'Bend, 10" FLG 11 1/4°' UNION ALL
SELECT '2262', 'W3441212', 'Bend, 12" FLG 11 1/4°' UNION ALL
SELECT '2263', 'W3441414', 'Bend, 14" FLG 11 1/4°' UNION ALL
SELECT '2264', 'W3441616', 'Bend, 16" FLG 11 1/4°' UNION ALL
SELECT '2265', 'W3441818', 'Bend, 18" FLG 11 1/4°' UNION ALL
SELECT '2266', 'W3442020', 'Bend, 20" FLG 11 1/4°' UNION ALL
SELECT '2267', 'W3442424', 'Bend, 24" FLG 11 1/4°' UNION ALL
SELECT '2268', 'W3460808', 'Bend,  8" Welded 22 1/2°' UNION ALL
SELECT '2269', 'W3470808', 'Bend,  8" Welded 45°' UNION ALL
SELECT '2270', 'W3471010', 'Bend, 10" Welded 45°' UNION ALL
SELECT '2271', 'W3471212', 'Bend, 12" Welded 45°' UNION ALL
SELECT '2272', 'W3471616', 'Bend, 16" Welded 45°' UNION ALL
SELECT '2273', 'W3490808', 'Bend,  8" MJ X FLG 45°' UNION ALL
SELECT '2274', 'W3492424', 'Bend, 24" MJ X FLG 45°' UNION ALL
SELECT '2275', 'W3500606', 'Bend,  6" MJ X FLG 90°' UNION ALL
SELECT '2276', 'W3500808', 'Bend,  8" MJ X FLG 90°' UNION ALL
SELECT '2277', 'W3501212', 'Bend, 12" MJ X FLG 90°' UNION ALL
SELECT '2278', 'W3510404', 'Bend,  4" MJ X PE 45°' UNION ALL
SELECT '2279', 'W3510606', 'Bend,  6" MJ X PE 45°' UNION ALL
SELECT '2280', 'W3510808', 'Bend,  8" MJ X PE 45°' UNION ALL
SELECT '2281', 'W3511010', 'Bend, 10" MJ X PE 45°' UNION ALL
SELECT '2282', 'W3511212', 'Bend, 12" MJ X PE 45°' UNION ALL
SELECT '2283', 'W3520404', 'Bend,  4" MJ X PE 90°'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 32.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2284', 'W3520606', 'Bend,  6" MJ X PE 90°' UNION ALL
SELECT '2285', 'W3520808', 'Bend,  8" MJ X PE 90°' UNION ALL
SELECT '2286', 'W3520808', 'Bend,  8" MJ X PE 90°' UNION ALL
SELECT '2287', 'W3521010', 'Bend, 10" MJ X PE 90°' UNION ALL
SELECT '2288', 'W3521212', 'Bend, 12" MJ X PE 90°' UNION ALL
SELECT '2289', 'W3530404', 'Bend,  4" MJ X PE 22 1/2°' UNION ALL
SELECT '2290', 'W3530606', 'Bend,  6" MJ X PE 22 1/2°' UNION ALL
SELECT '2291', 'W3530808', 'Bend,  8" MJ X PE 22 1/2°' UNION ALL
SELECT '2292', 'W3531212', 'Bend, 12" MJ X PE 22 1/2°' UNION ALL
SELECT '2293', 'W3540606', 'Bend,  6" MJ X PE 11 1/4°' UNION ALL
SELECT '2294', 'W3540808', 'Bend,  8" MJ X PE 11 1/4°' UNION ALL
SELECT '2295', 'W3541212', 'Bend, 12" MJ X PE 11 1/4°' UNION ALL
SELECT '2296', 'W3541616', 'Bend, 16" MJ X PE 11 1/4°' UNION ALL
SELECT '2297', 'W3580606', 'Offset,  6" X  6" SJ' UNION ALL
SELECT '2298', 'W3580612', 'Offset,  6" X 12" SJ' UNION ALL
SELECT '2299', 'W3580618', 'Offset,  6" X 18" SJ' UNION ALL
SELECT '2300', 'W3580806', 'Offset,  8" X  6" SJ' UNION ALL
SELECT '2301', 'W3580812', 'Offset,  8" X 12" SJ' UNION ALL
SELECT '2302', 'W3580818', 'Offset,  8" X 18" SJ' UNION ALL
SELECT '2303', 'W3581206', 'Offset, 12" X  6" SJ' UNION ALL
SELECT '2304', 'W3581212', 'Offset, 12" X 12" SJ' UNION ALL
SELECT '2305', 'W3581218', 'Offset, 12" X 18" SJ' UNION ALL
SELECT '2306', 'W3581612', 'Offset, 16" X 12" SJ' UNION ALL
SELECT '2307', 'W3581618', 'Offset, 16" X 18" SJ' UNION ALL
SELECT '2308', 'W3600404', 'Offset,  4" X  4" MJ BE X PE' UNION ALL
SELECT '2309', 'W3600418', 'Offset,  4" X 18" MJ BE X PE' UNION ALL
SELECT '2310', 'W3600424', 'Offset,  4" X 24" MJ BE X PE' UNION ALL
SELECT '2311', 'W3600824', 'Offset,  8" X 24" MJ BE X PE' UNION ALL
SELECT '2312', 'W3601018', 'Offset, 10" X 18" MJ BE X PE' UNION ALL
SELECT '2313', 'W3601206', 'Offset, 12" X  6" MJ BE X PE' UNION ALL
SELECT '2314', 'W3601212', 'Offset, 12" X 12" MJ BE X PE' UNION ALL
SELECT '2315', 'W3601216', 'Offset, 12" X 16" MJ BE X PE' UNION ALL
SELECT '2316', 'W3601224', 'Offset, 12" X 24" MJ BE X PE' UNION ALL
SELECT '2317', 'W3601616', 'Offset, 16" X 16" MJ BE X PE' UNION ALL
SELECT '2318', 'W3601624', 'Offset, 16" X 24" MJ BE X PE' UNION ALL
SELECT '2319', 'W3602012', 'Offset, 20" X 12" MJ BE X PE' UNION ALL
SELECT '2320', 'W3602018', 'Offset, 20" X 18" MJ BE X PE' UNION ALL
SELECT '2321', 'W3602024', 'Offset, 20" X 24" MJ BE X PE' UNION ALL
SELECT '2322', 'W3610406', 'Offset,  4" X  6" MJ BE X BE' UNION ALL
SELECT '2323', 'W3610412', 'Offset,  4" X 12" MJ BE X BE' UNION ALL
SELECT '2324', 'W3610418', 'Offset,  4" X 18" MJ BE X BE' UNION ALL
SELECT '2325', 'W3610424', 'Offset,  4" X 24" MJ BE X BE' UNION ALL
SELECT '2326', 'W3610606', 'Offset,  6" X  6" MJ BE X BE' UNION ALL
SELECT '2327', 'W3610612', 'Offset,  6" X 12" MJ BE X BE' UNION ALL
SELECT '2328', 'W3610618', 'Offset,  6" X 18" MJ BE X BE' UNION ALL
SELECT '2329', 'W3610624', 'Offset,  6" X 24" MJ BE X BE' UNION ALL
SELECT '2330', 'W3610806', 'Offset,  8" X  6" MJ BE X BE' UNION ALL
SELECT '2331', 'W3610812', 'Offset,  8" X 12" MJ BE X BE' UNION ALL
SELECT '2332', 'W3610818', 'Offset,  8" X 18" MJ BE X BE' UNION ALL
SELECT '2333', 'W3610824', 'Offset,  8" X 24" MJ BE X BE'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 33.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2334', 'W3611012', 'Offset, 10" X 12" MJ BE X BE' UNION ALL
SELECT '2335', 'W3611018', 'Offset, 10" X 18" MJ BE X BE' UNION ALL
SELECT '2336', 'W3611024', 'Offset, 10" X 24" MJ BE X BE' UNION ALL
SELECT '2337', 'W3611212', 'Offset, 12" X 12" MJ BE X BE' UNION ALL
SELECT '2338', 'W3611218', 'Offset, 12" X 18" MJ BE X BE' UNION ALL
SELECT '2339', 'W3611224', 'Offset, 12" X 24" MJ BE X BE' UNION ALL
SELECT '2340', 'W3611612', 'Offset, 16" X 12" MJ BE X BE' UNION ALL
SELECT '2341', 'W3611618', 'Offset, 16" X 18" MJ BE X BE' UNION ALL
SELECT '2342', 'W3611624', 'Offset, 16" X 24" MJ BE X BE' UNION ALL
SELECT '2343', 'W3612012', 'Offset, 20" X 12" MJ BE X BE' UNION ALL
SELECT '2344', 'W3612018', 'Offset, 20" X 18" MJ BE X BE' UNION ALL
SELECT '2345', 'W3612024', 'Offset, 20" X 24" MJ BE X BE' UNION ALL
SELECT '2346', 'W3660302', 'Reducer,  3" X 2" MJ' UNION ALL
SELECT '2347', 'W3660402', 'Reducer,  4" X 2" MJ' UNION ALL
SELECT '2348', 'W3660403', 'Reducer,  4" X 3" MJ' UNION ALL
SELECT '2349', 'W3660602', 'Reducer,  6" X 2" MJ' UNION ALL
SELECT '2350', 'W3660603', 'Reducer,  6" X 3" MJ' UNION ALL
SELECT '2351', 'W3660803', 'Reducer,  8" X 3" MJ' UNION ALL
SELECT '2352', 'W3661004', 'Reducer, 10" X 4" MJ' UNION ALL
SELECT '2353', 'W3661406', 'Reducer, 14" X  6" MJ' UNION ALL
SELECT '2354', 'W3661412', 'Reducer, 14" X 12" MJ' UNION ALL
SELECT '2355', 'W3661614', 'Reducer, 16" X 14" MJ' UNION ALL
SELECT '2356', 'W3661812', 'Reducer, 18" X 12" MJ' UNION ALL
SELECT '2357', 'W3661816', 'Reducer, 18" X 16" MJ' UNION ALL
SELECT '2358', 'W3662008', 'Reducer, 20" X  8" MJ' UNION ALL
SELECT '2359', 'W3662010', 'Reducer, 20" X 10" MJ' UNION ALL
SELECT '2360', 'W3662018', 'Reducer, 20" X 18" MJ' UNION ALL
SELECT '2361', 'W3662408', 'Reducer, 24" X  8"  MJ' UNION ALL
SELECT '2362', 'W3662416', 'Reducer, 24" X 16" MJ' UNION ALL
SELECT '2363', 'W3662418', 'Reducer, 24" X 18" MJ' UNION ALL
SELECT '2364', 'W3663008', 'Reducer, 30" X  8" MJ' UNION ALL
SELECT '2365', 'W3663012', 'Reducer, 30" X 12" MJ' UNION ALL
SELECT '2366', 'W3663020', 'Reducer, 30" X 20" MJ' UNION ALL
SELECT '2367', 'W3663620', 'Reducer, 36" X 20" MJ' UNION ALL
SELECT '2368', 'W3663624', 'Reducer, 36" X 24" MJ' UNION ALL
SELECT '2369', 'W3663630', 'Reducer, 36" X 30" MJ' UNION ALL
SELECT '2370', 'W3664224', 'Reducer, 42" X 24" MJ' UNION ALL
SELECT '2371', 'W3664842', 'Reducer, 48" X 42" MJ' UNION ALL
SELECT '2372', 'W3670604', 'Reducer,  6" X 4" MJ LEB' UNION ALL
SELECT '2373', 'W3670804', 'Reducer,  8" X 4" MJ LEB' UNION ALL
SELECT '2374', 'W3670806', 'Reducer,  8" X 6" MJ LEB' UNION ALL
SELECT '2375', 'W3671008', 'Reducer, 10" X 8" MJ LEB' UNION ALL
SELECT '2376', 'W3671206', 'Reducer, 12" X  6" MJ LEB' UNION ALL
SELECT '2377', 'W3671208', 'Reducer, 12" X  8" MJ LEB' UNION ALL
SELECT '2378', 'W3671210', 'Reducer, 12" X 10" MJ LEB' UNION ALL
SELECT '2379', 'W3671612', 'Reducer, 16" X 12" MJ LEB' UNION ALL
SELECT '2380', 'W3672006', 'Reducer, 20" X  6" MJ LEB' UNION ALL
SELECT '2381', 'W3672406', 'Reducer, 24" X  6"  MJ LEB' UNION ALL
SELECT '2382', 'W3690604', 'Reducer,  6" X 4" MJ SEB' UNION ALL
SELECT '2383', 'W3690804', 'Reducer,  8" X 4" MJ SEB'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 34.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2384', 'W3690806', 'Reducer,  8" X 6" MJ SEB' UNION ALL
SELECT '2385', 'W3691006', 'Reducer, 10" X 6" MJ SEB' UNION ALL
SELECT '2386', 'W3691008', 'Reducer, 10" X 8" MJ SEB' UNION ALL
SELECT '2387', 'W3691206', 'Reducer, 12" X  6" MJ SEB' UNION ALL
SELECT '2388', 'W3691208', 'Reducer, 12" X  8" MJ SEB' UNION ALL
SELECT '2389', 'W3691210', 'Reducer, 12" X 10" MJ SEB' UNION ALL
SELECT '2390', 'W3691612', 'Reducer, 16" X 12" MJ SEB' UNION ALL
SELECT '2391', 'W3692006', 'Reducer, 20" X  6"  MJ SEB' UNION ALL
SELECT '2392', 'W3692406', 'Reducer, 24" X  6"  MJ SEB' UNION ALL
SELECT '2393', 'W3700402', 'Reducer,  4" X 2" FLG' UNION ALL
SELECT '2394', 'W3700403', 'Reducer,  4" X 3" FLG' UNION ALL
SELECT '2395', 'W3700603', 'Reducer,  6" X 3" FLG' UNION ALL
SELECT '2396', 'W3700804', 'Reducer,  8" X 4" FLG' UNION ALL
SELECT '2397', 'W3700806', 'Reducer,  8" X 6" FLG' UNION ALL
SELECT '2398', 'W3701006', 'Reducer, 10" X 6" FLG' UNION ALL
SELECT '2399', 'W3701008', 'Reducer, 10" X 8" FLG' UNION ALL
SELECT '2400', 'W3701204', 'Reducer, 12" X  4" FLG' UNION ALL
SELECT '2401', 'W3701206', 'Reducer, 12" X  6" FLG' UNION ALL
SELECT '2402', 'W3701208', 'Reducer, 12" X  8" FLG' UNION ALL
SELECT '2403', 'W3701210', 'Reducer, 12" X 10" FLG' UNION ALL
SELECT '2404', 'W3701610', 'Reducer, 16" X 10" FLG' UNION ALL
SELECT '2405', 'W3702406', 'Reducer, 24" X  6"  FLG' UNION ALL
SELECT '2406', 'W3702412', 'Reducer, 24" X 12" FLG' UNION ALL
SELECT '2407', 'W3710604', 'Reducer,  6" X 4" PE X PE' UNION ALL
SELECT '2408', 'W3710804', 'Reducer,  8" X 4" PE X PE' UNION ALL
SELECT '2409', 'W3710806', 'Reducer,  8" X 6" PE X PE' UNION ALL
SELECT '2410', 'W3712006', 'Reducer, 20" X  6"  PE X PE' UNION ALL
SELECT '2411', 'W3712406', 'Reducer, 24" X  6"  PE X PE' UNION ALL
SELECT '2412', 'W3720806', 'Reducer,  8" X 6" RJ' UNION ALL
SELECT '2413', 'W3721206', 'Reducer, 12" X  8" RJ' UNION ALL
SELECT '2414', 'W3721208', 'Reducer, 12" X  8" RJ' UNION ALL
SELECT '2415', 'W3721608', 'Reducer, 16" X  8" RJ' UNION ALL
SELECT '2416', 'W3722008', 'Reducer, 20" X  8" RJ' UNION ALL
SELECT '2417', 'W3722012', 'Reducer, 20" X 12" RJ' UNION ALL
SELECT '2418', 'W3722016', 'Reducer, 20" X 16" RJ' UNION ALL
SELECT '2419', 'W3722018', 'Reducer, 20" X 18" RJ' UNION ALL
SELECT '2420', 'W3722412', 'Reducer, 24" X 12" RJ' UNION ALL
SELECT '2421', 'W3722416', 'Reducer, 24" X 16" RJ' UNION ALL
SELECT '2422', 'W3722420', 'Reducer, 24" X 20" RJ' UNION ALL
SELECT '2423', 'W3723016', 'Reducer, 30" X 16" RJ' UNION ALL
SELECT '2424', 'W3723024', 'Reducer, 30" X 24" RJ' UNION ALL
SELECT '2425', 'W3723620', 'Reducer, 36" X 20" RJ' UNION ALL
SELECT '2426', 'W3730402', 'Reducer,  4" X 2" SJ' UNION ALL
SELECT '2427', 'W3730403', 'Reducer,  4" X 3" SJ' UNION ALL
SELECT '2428', 'W3730602', 'Reducer,  6" X 2" SJ' UNION ALL
SELECT '2429', 'W3730603', 'Reducer,  6" X 3" SJ' UNION ALL
SELECT '2430', 'W3730604', 'Reducer,  6" X 4" SJ' UNION ALL
SELECT '2431', 'W3730803', 'Reducer,  8" X 3" SJ' UNION ALL
SELECT '2432', 'W3730804', 'Reducer,  8" X 4" SJ' UNION ALL
SELECT '2433', 'W3730806', 'Reducer,  8" X 6" SJ'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 35.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2434', 'W3731004', 'Reducer, 10" X 4" SJ' UNION ALL
SELECT '2435', 'W3731006', 'Reducer, 10" X 6" SJ' UNION ALL
SELECT '2436', 'W3731008', 'Reducer, 10" X 8" SJ' UNION ALL
SELECT '2437', 'W3731204', 'Reducer, 12" X  4" SJ' UNION ALL
SELECT '2438', 'W3731206', 'Reducer, 12" X  6" SJ' UNION ALL
SELECT '2439', 'W3731208', 'Reducer, 12" X  8" SJ' UNION ALL
SELECT '2440', 'W3731210', 'Reducer, 12" X 10" SJ' UNION ALL
SELECT '2441', 'W3731606', 'Reducer, 16" X  6" SJ' UNION ALL
SELECT '2442', 'W3731608', 'Reducer, 16" X  8" SJ' UNION ALL
SELECT '2443', 'W3731610', 'Reducer, 16" X 10" SJ' UNION ALL
SELECT '2444', 'W3731612', 'Reducer, 16" X 12" SJ' UNION ALL
SELECT '2445', 'W3732008', 'Reducer, 20" X  8" SJ' UNION ALL
SELECT '2446', 'W3732012', 'Reducer, 20" X 12" SJ' UNION ALL
SELECT '2447', 'W3732016', 'Reducer, 20" X 16" SJ' UNION ALL
SELECT '2448', 'W3732412', 'Reducer, 24" X 12" SJ' UNION ALL
SELECT '2449', 'W3732416', 'Reducer, 24" X 16" SJ' UNION ALL
SELECT '2450', 'W3733630', 'Reducer, 36" X 30" SJ' UNION ALL
SELECT '2451', 'W3740806', 'Reducer,  8" X 6" FLG X SJ' UNION ALL
SELECT '2452', 'W3760303', 'Gland,  3" Retainer DI' UNION ALL
SELECT '2453', 'W3760404', 'Gland,  4" Retainer DI' UNION ALL
SELECT '2454', 'W3760808', 'Gland,  8" Retainer DI' UNION ALL
SELECT '2455', 'W3761212', 'Gland, 12" Retainer DI' UNION ALL
SELECT '2456', 'W3761414', 'Gland, 14" Retainer DI' UNION ALL
SELECT '2457', 'W3763030', 'Gland, 30" Retainer DI' UNION ALL
SELECT '2458', 'W3763636', 'Gland, 36" Retainer DI' UNION ALL
SELECT '2459', 'W3764848', 'Gland, 48" Retainer DI' UNION ALL
SELECT '2460', 'W3780202', 'Gland,  2" Retainer RJ' UNION ALL
SELECT '2461', 'W3780303', 'Gland,  3" Retainer RJ' UNION ALL
SELECT '2462', 'W3781010', 'Gland, 10" Retainer RJ' UNION ALL
SELECT '2463', 'W3781414', 'Gland, 14" Retainer RJ' UNION ALL
SELECT '2464', 'W3781616', 'Gland, 16" Retainer RJ' UNION ALL
SELECT '2465', 'W3781818', 'Gland, 18" Retainer RJ' UNION ALL
SELECT '2466', 'W3782020', 'Gland, 20" Retainer RJ' UNION ALL
SELECT '2467', 'W3782424', 'Gland, 24" Retainer RJ' UNION ALL
SELECT '2468', 'W3783030', 'Gland, 30" Retainer RJ' UNION ALL
SELECT '2469', 'W3783636', 'Gland, 36" Retainer RJ' UNION ALL
SELECT '2470', 'W3784242', 'Gland, 42" Retainer RJ' UNION ALL
SELECT '2471', 'W3790303', 'Flange,  3" UNI' UNION ALL
SELECT '2472', 'W3790404', 'Flange,  4" UNI' UNION ALL
SELECT '2473', 'W3790606', 'Flange,  6" UNI' UNION ALL
SELECT '2474', 'W3790808', 'Flange,  8" UNI' UNION ALL
SELECT '2475', 'W3791010', 'Flange, 10" UNI' UNION ALL
SELECT '2476', 'W3791212', 'Flange, 12" UNI' UNION ALL
SELECT '2477', 'W3791414', 'Flange, 14" UNI' UNION ALL
SELECT '2478', 'W3791616', 'Flange, 16" UNI' UNION ALL
SELECT '2479', 'W3791818', 'Flange, 18" UNI' UNION ALL
SELECT '2480', 'W3792424', 'Flange, 24" UNI' UNION ALL
SELECT '2481', 'W3800404', 'Flange,  4" Companion' UNION ALL
SELECT '2482', 'W3800602', 'Flange,  6" X 2" Companion' UNION ALL
SELECT '2483', 'W3800606', 'Flange,  6" Companion'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 36.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2484', 'W3800808', 'Flange,  8" Companion' UNION ALL
SELECT '2485', 'W3801010', 'Flange, 10" Companion' UNION ALL
SELECT '2486', 'W3810606', 'Ring,  6" Gripper RJ' UNION ALL
SELECT '2487', 'W3810808', 'Ring,  8" Gripper RJ' UNION ALL
SELECT '2488', 'W3811212', 'Ring, 12" Gripper RJ' UNION ALL
SELECT '2489', 'W3811818', 'Ring, 18" Gripper RJ' UNION ALL
SELECT '2490', 'W3812020', 'Ring, 20" Gripper RJ' UNION ALL
SELECT '2491', 'W3813030', 'Ring, 30" Gripper RJ' UNION ALL
SELECT '2492', 'W3813636', 'Ring, 36" Gripper RJ' UNION ALL
SELECT '2493', 'W3820402', 'Flange,  4" X 2" Blind' UNION ALL
SELECT '2494', 'W3820404', 'Flange,  4" Blind' UNION ALL
SELECT '2495', 'W3820602', 'Flange,  6" X 2" Blind' UNION ALL
SELECT '2496', 'W3820606', 'Flange,  6" Blind' UNION ALL
SELECT '2497', 'W3820808', 'Flange,  8" Blind' UNION ALL
SELECT '2498', 'W3821010', 'Flange, 10" Blind' UNION ALL
SELECT '2499', 'W3821212', 'Flange, 12" Blind' UNION ALL
SELECT '2500', 'W3821616', 'Flange, 16" Blind' UNION ALL
SELECT '2501', 'W3840404', 'Gland,  4" Gripper MJ' UNION ALL
SELECT '2502', 'W3840606', 'Gland,  6" Gripper MJ' UNION ALL
SELECT '2503', 'W3840808', 'Gland,  8" Gripper MJ' UNION ALL
SELECT '2504', 'W3841010', 'Gland, 10" Gripper MJ' UNION ALL
SELECT '2505', 'W3841212', 'Gland, 12" Gripper MJ' UNION ALL
SELECT '2506', 'W3850202', 'Gasket,  2" Field Lok SJ' UNION ALL
SELECT '2507', 'W3850303', 'Gasket,  3" Field Lok SJ' UNION ALL
SELECT '2508', 'W3851414', 'Gasket, 14" Field Lok SJ' UNION ALL
SELECT '2509', 'W3851818', 'Gasket, 18" Field Lok SJ' UNION ALL
SELECT '2510', 'W3861615', 'Clamp, 16" X 15" SS FCRC AC' UNION ALL
SELECT '2511', 'W3872030', 'Clamp, 20" X 30" SS FCRC ST' UNION ALL
SELECT '2512', 'W3880404', 'Gasket,  4" Field Lok MJ' UNION ALL
SELECT '2513', 'W3880606', 'Gasket,  6" Field Lok MJ' UNION ALL
SELECT '2514', 'W3880808', 'Gasket,  8" Field Lok MJ' UNION ALL
SELECT '2515', 'W3881010', 'Gasket, 10" Field Lok MJ' UNION ALL
SELECT '2516', 'W3881212', 'Gasket, 12" Field Lok MJ' UNION ALL
SELECT '2517', 'W3881616', 'Gasket, 16" Field Lok MJ' UNION ALL
SELECT '2518', 'W3910H0H', 'LUG DUC 3/4' UNION ALL
SELECT '2519', 'W3920H0H', 'Rod,  3/4" Threaded' UNION ALL
SELECT '2520', 'W3931200', 'Clamp, 12" AC SS FCRC' UNION ALL
SELECT '2521', 'W3940303', 'Clamp,  3" Socket' UNION ALL
SELECT '2522', 'W3940404', 'Clamp,  4" Socket' UNION ALL
SELECT '2523', 'W3940606', 'Clamp,  6" Socket' UNION ALL
SELECT '2524', 'W3940808', 'Clamp,  8" Socket' UNION ALL
SELECT '2525', 'W3941010', 'Clamp, 10" Socket' UNION ALL
SELECT '2526', 'W3941212', 'Clamp, 12" Socket' UNION ALL
SELECT '2527', 'W3950404', 'Clamp,  4" BJ' UNION ALL
SELECT '2528', 'W3950606', 'Clamp,  6" BJ' UNION ALL
SELECT '2529', 'W3950808', 'Clamp,  8" BJ' UNION ALL
SELECT '2530', 'W3951010', 'Clamp, 10" BJ' UNION ALL
SELECT '2531', 'W3951212', 'Clamp, 12" BJ' UNION ALL
SELECT '2532', 'W3951616', 'Clamp, 16" BJ' UNION ALL
SELECT '2533', 'W3952020', 'Clamp, 20" BJ'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 37.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2534', 'W3952424', 'Clamp, 24" BJ' UNION ALL
SELECT '2535', 'W3953030', 'Clamp, 30" BJ' UNION ALL
SELECT '2536', 'W3953636', 'Clamp, 36" BJ' UNION ALL
SELECT '2537', 'W3954242', 'Clamp, 42" BJ' UNION ALL
SELECT '2538', 'W3960103', 'Clamp,  1" X 3" SS PSRC' UNION ALL
SELECT '2539', 'W3960106', 'Clamp,  1" X 6" SS PSRC' UNION ALL
SELECT '2540', 'W3960203', 'Clamp,  2" X  3" SS PSRC' UNION ALL
SELECT '2541', 'W3960206', 'Clamp,  2" X  6" SS PSRC' UNION ALL
SELECT '2542', 'W3960H03', 'Clamp,   3/4" X 3" SS PSRC' UNION ALL
SELECT '2543', 'W3960H06', 'Clamp,   3/4" X 6" SS PSRC' UNION ALL
SELECT '2544', 'W3961C06', 'Clamp,  1 1/4" X 6" SS PSRC' UNION ALL
SELECT '2545', 'W3961F06', 'Clamp,  1 1/2" X 6" SS PSRC' UNION ALL
SELECT '2546', 'W3962F06', 'Clamp,  2 1/2" X 6" SS PSRC' UNION ALL
SELECT '2547', 'W3970103', 'Clamp,  1" X 3" SS FCRC' UNION ALL
SELECT '2548', 'W3970106', 'Clamp,  1" X 6" SS FCRC' UNION ALL
SELECT '2549', 'W3970202', 'Clamp,  2" X  2" SS FCRC' UNION ALL
SELECT '2550', 'W3970206', 'Clamp,  2" X  6" SS FCRC' UNION ALL
SELECT '2551', 'W3970212', 'Clamp,  2" X 12" SS FCRC' UNION ALL
SELECT '2552', 'W397027F', 'Clamp,  2" X  7 1/2" SS FCRC' UNION ALL
SELECT '2553', 'W3970303', 'Clamp,  3" X  3" SS FCRC' UNION ALL
SELECT '2554', 'W3970306', 'Clamp,  3" X  6" SS FCRC' UNION ALL
SELECT '2555', 'W3970404', 'Clamp,  4" X  4" SS FCRC' UNION ALL
SELECT '2556', 'W3970406', 'Clamp,  4" X  6" SS FCRC' UNION ALL
SELECT '2557', 'W3970408', 'Clamp,  4" X  8" ST FCRC' UNION ALL
SELECT '2558', 'W3970412', 'Clamp,  4" X 12" SS FCRC' UNION ALL
SELECT '2559', 'W3970606', 'Clamp,  6" X  6" SS FCRC' UNION ALL
SELECT '2560', 'W3970608', 'Clamp,  6" X  8" SS FCRC' UNION ALL
SELECT '2561', 'W3970612', 'Clamp,  6" X 12" SS FCRC' UNION ALL
SELECT '2562', 'W397061201', 'Clamp,  6" X 12" SS FCRC CI/DI 1" Tap' UNION ALL
SELECT '2563', 'W3970808', 'Clamp,  8" X  8" SS FCRC' UNION ALL
SELECT '2564', 'W3970812', 'Clamp,  8" X 12" SS FCRC' UNION ALL
SELECT '2565', 'W3970815', 'Clamp,  8" X 15" SS FCRC' UNION ALL
SELECT '2566', 'W3970820', 'Clamp,  8" X 20" SS FCRC' UNION ALL
SELECT '2567', 'W3970H03', 'Clamp,   3/4" X 3" SS FCRC' UNION ALL
SELECT '2568', 'W3970H06', 'Clamp,   3/4" X 6" SS FCRC' UNION ALL
SELECT '2569', 'W3971010', 'Clamp, 10" X 10" SS FCRC' UNION ALL
SELECT '2570', 'W3971012', 'Clamp, 10" X 12" SS FCRC' UNION ALL
SELECT '2571', 'W3971015', 'Clamp, 10" X 15" SS FCRC' UNION ALL
SELECT '2572', 'W3971020', 'Clamp, 10" X 20" SS FCRC' UNION ALL
SELECT '2573', 'W3971200', 'Clamp, 12" SS FCRC' UNION ALL
SELECT '2574', 'W3971208', 'Clamp, 12" X  8" SS FCRC' UNION ALL
SELECT '2575', 'W3971212', 'Clamp, 12" X 12" SS FCRC' UNION ALL
SELECT '2576', 'W397121202', 'Clamp, 12" X 12" SS FCRC CI 2" Tap' UNION ALL
SELECT '2577', 'W3971215', 'Clamp, 12" X 15" SS FCRC' UNION ALL
SELECT '2578', 'W39712150H', 'Clamp, 12" X 15" X  3/4" SS FCRC' UNION ALL
SELECT '2579', 'W3971216', 'Clamp, 12" X 16" SS FCRC' UNION ALL
SELECT '2580', 'W3971222', 'Clamp, 12" X 22" SS FCRC' UNION ALL
SELECT '2581', 'W3971230', 'Clamp, 12" X 30" SS FCRC' UNION ALL
SELECT '2582', 'W3971414', 'Clamp, 14" SS FCRC' UNION ALL
SELECT '2583', 'W3971415', 'Clamp, 14" X 15" SS FCRC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 38.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2584', 'W3971616', 'Clamp, 16" SS FCRC' UNION ALL
SELECT '2585', 'W3971618', 'Clamp, 16" X 18" SS FCRC' UNION ALL
SELECT '2586', 'W3971620', 'Clamp, 16" X 20" SS FCRC CI/DI' UNION ALL
SELECT '2587', 'W3971624', 'Clamp, 16" X 24" SS FCRC' UNION ALL
SELECT '2588', 'W3971630', 'Clamp, 16" X 30" SS FCRC CI/DI' UNION ALL
SELECT '2589', 'W3971632', 'Clamp, 16" X 32" SS FCRC' UNION ALL
SELECT '2590', 'W3971818', 'Clamp, 18" SS FCRC' UNION ALL
SELECT '2591', 'W3971C03', 'Clamp,  1 1/4" X 3" SS FCRC' UNION ALL
SELECT '2592', 'W3971F03', 'Clamp,  1 1/2" X 3" SS FCRC' UNION ALL
SELECT '2593', 'W3971F06', 'Clamp,  1 1/2" X 6" SS FCRC' UNION ALL
SELECT '2594', 'W3972020', 'Clamp, 20" SS FCRC' UNION ALL
SELECT '2595', 'W3972024', 'Clamp, 20" X 24" SS FCRC' UNION ALL
SELECT '2596', 'W3972420', 'Clamp, 24" X 20" SS FCRC CI/DI' UNION ALL
SELECT '2597', 'W3972424', 'Clamp, 24" SS FCRC' UNION ALL
SELECT '2598', 'W3972C06', 'Clamp,  2 1/4" X  6" SS FCRC' UNION ALL
SELECT '2599', 'W3972C10', 'Clamp,  2 1/4" X 10" SS FCRC' UNION ALL
SELECT '2600', 'W3972C12', 'Clamp,  2 1/4" X 12" SS FCRC' UNION ALL
SELECT '2601', 'W3972C7F', 'Clamp,  2 1/4" X  7 1/2" SS FCRC' UNION ALL
SELECT '2602', 'W3972F03', 'Clamp,  2 1/2" X 3" SS FCRC' UNION ALL
SELECT '2603', 'W3972F06', 'Clamp,  2 1/2" X 6" SS FCRC' UNION ALL
SELECT '2604', 'W3973030', 'Clamp, 30" SS FCRC' UNION ALL
SELECT '2605', 'W3973624', 'Clamp, 36" X 24" SS FCRC' UNION ALL
SELECT '2606', 'W3973636', 'Clamp, 36" SS FCRC' UNION ALL
SELECT '2607', 'W3974230', 'Clamp, 42" X 30" SS FCRC' UNION ALL
SELECT '2608', 'W398060801', 'Clamp,  6" X  8" X 1" SS FCRC TAP' UNION ALL
SELECT '2609', 'W39806080H', 'Clamp,  6" X  8" X  3/4" SS FCRC Tap' UNION ALL
SELECT '2610', 'W398061202', 'Clamp,  6" X 12" X 2" SS FCRC TAP' UNION ALL
SELECT '2611', 'W39806120H', 'Clamp,  6" X 12" X  3/4" SS FCRC Tap' UNION ALL
SELECT '2612', 'W39806121F', 'Clamp,  6" X 12" X 1 1/2" SS FCRC TAP' UNION ALL
SELECT '2613', 'W3980801', 'Clamp,  8" X  8" X 1" SS FCRC TAP' UNION ALL
SELECT '2614', 'W398081201', 'Clamp,  8" X 12" X 1" SS FCRC Tap' UNION ALL
SELECT '2615', 'W398081202', 'Clamp,  8" X 12" X 2" SS FCRC Tap' UNION ALL
SELECT '2616', 'W39808120H', 'Clamp,  8" X 12" X  3/4" SS FCRC TAP' UNION ALL
SELECT '2617', 'W39808121F', 'Clamp,  8" X 12" X 1 1/2" SS FCRC TAP' UNION ALL
SELECT '2618', 'W3981201', 'Clamp, 12" X 16" X 1" SS FCRC Tap' UNION ALL
SELECT '2619', 'W4000235', 'Pipe,  2" DI SJ CL 350' UNION ALL
SELECT '2620', 'W4000350', 'Pipe,  3" DI SJ CL 50' UNION ALL
SELECT '2621', 'W4000351', 'Pipe,  3" DI SJ CL 51' UNION ALL
SELECT '2622', 'W4000352', 'Pipe,  3" DI SJ CL 52' UNION ALL
SELECT '2623', 'W4000352', 'Pipe,  3" DI SJ CL 52' UNION ALL
SELECT '2624', 'W4000450', 'Pipe,  4" DI SJ CL 50' UNION ALL
SELECT '2625', 'W4000452', 'Pipe,  4" DI SJ CL 52' UNION ALL
SELECT '2626', 'W4000454', 'Pipe,  4" DI SJ CL 54' UNION ALL
SELECT '2627', 'W4000651', 'Pipe,  6" DI SJ CL 51' UNION ALL
SELECT '2628', 'W4000654', 'Pipe,  6" DI SJ CL 54' UNION ALL
SELECT '2629', 'W4000656', 'Pipe,  6" DI SJ CL 56' UNION ALL
SELECT '2630', 'W4000850', 'Pipe,  8" DI SJ CL 50' UNION ALL
SELECT '2631', 'W4000851', 'Pipe,  8" DI SJ CL 51' UNION ALL
SELECT '2632', 'W4000853', 'Pipe,  8" DI SJ CL 53' UNION ALL
SELECT '2633', 'W4000854', 'Pipe,  8" DI SJ CL 54'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 39.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2634', 'W4000856', 'Pipe,  8" DI SJ CL 56' UNION ALL
SELECT '2635', 'W4001050', 'Pipe, 10" DI SJ CL 50' UNION ALL
SELECT '2636', 'W4001051', 'Pipe, 10" DI SJ CL 51' UNION ALL
SELECT '2637', 'W4001054', 'Pipe, 10" DI SJ CL 54' UNION ALL
SELECT '2638', 'W4001056', 'Pipe, 10" DI SJ CL 56' UNION ALL
SELECT '2639', 'W4001250', 'Pipe, 12" DI SJ CL 50' UNION ALL
SELECT '2640', 'W4001251', 'Pipe, 12" DI SJ CL 51' UNION ALL
SELECT '2641', 'W4001253', 'Pipe, 12" DI SJ CL 53' UNION ALL
SELECT '2642', 'W4001254', 'Pipe, 12" DI SJ CL 54' UNION ALL
SELECT '2643', 'W4001256', 'Pipe, 12" DI SJ CL 56' UNION ALL
SELECT '2644', 'W4001435', 'Pipe, 14" DI SJ CL 350' UNION ALL
SELECT '2645', 'W4001450', 'Pipe, 14" DI SJ CL 50' UNION ALL
SELECT '2646', 'W4001452', 'Pipe, 14" DI SJ CL 52' UNION ALL
SELECT '2647', 'W4001453', 'Pipe, 14" DI SJ CL 53' UNION ALL
SELECT '2648', 'W4001454', 'Pipe, 14" DI SJ CL 54' UNION ALL
SELECT '2649', 'W4001650', 'Pipe, 16" DI SJ CL 50' UNION ALL
SELECT '2650', 'W4001651', 'Pipe, 16" DI SJ CL 51' UNION ALL
SELECT '2651', 'W4001653', 'Pipe, 16" DI SJ CL 53' UNION ALL
SELECT '2652', 'W4001654', 'Pipe, 16" DI SJ CL 54' UNION ALL
SELECT '2653', 'W4001656', 'Pipe, 16" DI SJ CL 56' UNION ALL
SELECT '2654', 'W4001830', 'Pipe, 18" DI SJ CL 300' UNION ALL
SELECT '2655', 'W4001835', 'Pipe, 18" DI SJ CL 350' UNION ALL
SELECT '2656', 'W4001850', 'Pipe, 18" DI SJ CL 50' UNION ALL
SELECT '2657', 'W4001853', 'Pipe, 18" DI SJ CL 53' UNION ALL
SELECT '2658', 'W4001854', 'Pipe, 18" DI SJ CL 54' UNION ALL
SELECT '2659', 'W4002025', 'Pipe, 20" DI SJ CL 250' UNION ALL
SELECT '2660', 'W4002030', 'Pipe, 20" DI SJ CL 300' UNION ALL
SELECT '2661', 'W4002035', 'Pipe, 20" DI SJ CL 350' UNION ALL
SELECT '2662', 'W4002051', 'Pipe, 20" DI SJ CL 51' UNION ALL
SELECT '2663', 'W4002053', 'Pipe, 20" DI SJ CL 53' UNION ALL
SELECT '2664', 'W4002054', 'Pipe, 20" DI SJ CL 54' UNION ALL
SELECT '2665', 'W4002056', 'Pipe, 20" DI SJ CL 56' UNION ALL
SELECT '2666', 'W4002425', 'Pipe, 24" DI SJ CL 250' UNION ALL
SELECT '2667', 'W4002435', 'Pipe, 24" DI SJ CL 350' UNION ALL
SELECT '2668', 'W4002451', 'Pipe, 24" DI SJ CL 51' UNION ALL
SELECT '2669', 'W4002453', 'Pipe, 24" DI SJ CL 53' UNION ALL
SELECT '2670', 'W4002454', 'Pipe, 24" DI SJ CL 54' UNION ALL
SELECT '2671', 'W4002456', 'Pipe, 24" DI SJ CL 56' UNION ALL
SELECT '2672', 'W4003020', 'Pipe, 30" DI SJ CL 200' UNION ALL
SELECT '2673', 'W4003025', 'Pipe, 30" DI SJ CL 250' UNION ALL
SELECT '2674', 'W4003035', 'Pipe, 30" DI SJ CL 350' UNION ALL
SELECT '2675', 'W4003050', 'Pipe, 30" DI SJ CL 50' UNION ALL
SELECT '2676', 'W4003051', 'Pipe, 30" DI SJ CL 51' UNION ALL
SELECT '2677', 'W4003053', 'Pipe, 30" DI SJ CL 53' UNION ALL
SELECT '2678', 'W4003054', 'Pipe, 30" DI SJ CL 54' UNION ALL
SELECT '2679', 'W4003056', 'Pipe, 30" DI SJ CL 56' UNION ALL
SELECT '2680', 'W4003620', 'Pipe, 36" DI SJ CL 200' UNION ALL
SELECT '2681', 'W4003625', 'Pipe, 36" DI SJ CL 250' UNION ALL
SELECT '2682', 'W4003635', 'Pipe, 36" DI SJ CL 350' UNION ALL
SELECT '2683', 'W4003651', 'Pipe, 36" DI SJ CL 51'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 40.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2684', 'W4003653', 'Pipe, 36" DI SJ CL 53' UNION ALL
SELECT '2685', 'W4003654', 'Pipe, 36" DI SJ CL 54' UNION ALL
SELECT '2686', 'W4003656', 'Pipe, 36" DI SJ CL 56' UNION ALL
SELECT '2687', 'W4004215', 'Pipe, 42" DI SJ CL 150' UNION ALL
SELECT '2688', 'W4004220', 'Pipe, 42" DI SJ CL 200' UNION ALL
SELECT '2689', 'W4004225', 'Pipe, 42" DI SJ CL 250' UNION ALL
SELECT '2690', 'W4004235', 'Pipe, 42" DI SJ CL 350' UNION ALL
SELECT '2691', 'W4004251', 'Pipe, 42" DI SJ CL 51' UNION ALL
SELECT '2692', 'W4004252', 'Pipe, 42" DI SJ CL 52' UNION ALL
SELECT '2693', 'W4004253', 'Pipe, 42" DI SJ CL 53' UNION ALL
SELECT '2694', 'W4004254', 'Pipe, 42" DI SJ CL 54' UNION ALL
SELECT '2695', 'W4004256', 'Pipe, 42" DI SJ CL 56' UNION ALL
SELECT '2696', 'W4004835', 'Pipe, 48" DI SJ CL 350' UNION ALL
SELECT '2697', 'W4004851', 'Pipe, 48" DI SJ CL 51' UNION ALL
SELECT '2698', 'W4004852', 'Pipe, 48" DI SJ CL 52' UNION ALL
SELECT '2699', 'W4004853', 'Pipe, 48" DI SJ CL 53' UNION ALL
SELECT '2700', 'W4004854', 'Pipe, 48" DI SJ CL 54' UNION ALL
SELECT '2701', 'W4004856', 'Pipe, 48" DI SJ CL 56' UNION ALL
SELECT '2702', 'W4005452', 'Pipe, 54" DI SJ CL 52' UNION ALL
SELECT '2703', 'W4006020', 'Pipe, 60" DI SJ CL 200' UNION ALL
SELECT '2704', 'W4006030', 'Pipe, 60" DI SJ CL 300' UNION ALL
SELECT '2705', 'W4006052', 'Pipe, 60" DI SJ CL 52' UNION ALL
SELECT '2706', 'W4030352', 'Pipe,  3" DI RJ CL 52' UNION ALL
SELECT '2707', 'W4030650', 'Pipe,  6" DI RJ CL 50' UNION ALL
SELECT '2708', 'W4030652', 'Pipe,  6" DI RJ CL 52' UNION ALL
SELECT '2709', 'W4030850', 'Pipe,  8" DI RJ CL 50' UNION ALL
SELECT '2710', 'W4030856', 'Pipe,  8" DI RJ CL 56' UNION ALL
SELECT '2711', 'W4031035', 'Pipe, 10" DI RJ CL 350' UNION ALL
SELECT '2712', 'W4031250', 'Pipe, 12" DI RJ CL 50' UNION ALL
SELECT '2713', 'W4031650', 'Pipe, 16" DI RJ CL 50' UNION ALL
SELECT '2714', 'W4031651', 'Pipe, 16" DI RJ CL 51' UNION ALL
SELECT '2715', 'W4031656', 'Pipe, 16" DI RJ CL 56' UNION ALL
SELECT '2716', 'W4031835', 'Pipe, 18" DI RJ CL 350' UNION ALL
SELECT '2717', 'W4032030', 'Pipe, 20" DI RJ CL 300' UNION ALL
SELECT '2718', 'W4032035', 'Pipe, 20" DI RJ CL 350' UNION ALL
SELECT '2719', 'W4032435', 'Pipe, 24" DI RJ CL 350' UNION ALL
SELECT '2720', 'W4033020', 'Pipe, 30" DI RJ CL 200' UNION ALL
SELECT '2721', 'W4033035', 'Pipe, 30" DI RJ CL 350' UNION ALL
SELECT '2722', 'W4033620', 'Pipe, 36" DI RJ CL 200' UNION ALL
SELECT '2723', 'W4034220', 'Pipe, 42" DI RJ CL 200' UNION ALL
SELECT '2724', 'W4034235', 'Pipe, 42" DI RJ CL 350' UNION ALL
SELECT '2725', 'W4034252', 'Pipe, 42" DI RJ CL 52' UNION ALL
SELECT '2726', 'W4034253', 'Pipe, 42" DI RJ CL 53' UNION ALL
SELECT '2727', 'W4090435', 'Pipe,  4" DI MJ CL 350' UNION ALL
SELECT '2728', 'W4090635', 'Pipe,  6" DI MJ CL 350' UNION ALL
SELECT '2729', 'W4090835', 'Pipe,  8" DI MJ CL 350' UNION ALL
SELECT '2730', 'W4090850', 'Pipe,  8" DI MJ CL 50' UNION ALL
SELECT '2731', 'W4091035', 'Pipe, 10" DI MJ CL 350' UNION ALL
SELECT '2732', 'W4091235', 'Pipe, 12" DI MJ CL 350' UNION ALL
SELECT '2733', 'W4091435', 'Pipe, 14" DI MJ CL 350'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 41.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2734', 'W4091635', 'Pipe, 16" DI MJ CL 350' UNION ALL
SELECT '2735', 'W4120835', 'Pipe,  8" DI Ball Joint CL 350' UNION ALL
SELECT '2736', 'W4121235', 'Pipe, 12" DI Ball Joint CL 350' UNION ALL
SELECT '2737', 'W4122035', 'Pipe, 20" DI Ball Joint CL 350' UNION ALL
SELECT '2738', 'W4122435', 'Pipe, 24" DI Ball Joint CL 350' UNION ALL
SELECT '2739', 'W4122460', 'Pipe, 24" DI Ball Joint CL 60' UNION ALL
SELECT '2740', 'W4150408', 'Wrap,  4" to 8" Plastic for Pipe' UNION ALL
SELECT '2741', 'W4151012', 'Wrap, 10" to 12" Plastic for Pipe' UNION ALL
SELECT '2742', 'W4151416', 'Wrap, 14" to 16" Plastic for Pipe' UNION ALL
SELECT '2743', 'W4151624', 'Wrap, 16" to 24" Plastic for Pipe' UNION ALL
SELECT '2744', 'W4151824', 'Wrap, 18" to 24" Plastic for Pipe' UNION ALL
SELECT '2745', 'W4153036', 'Wrap, 30" to 36" Plastic for Pipe' UNION ALL
SELECT '2746', 'W4153036', 'Wrap, 30" to 36" Plastic for Pipe' UNION ALL
SELECT '2747', 'W4154248', 'Wrap, 42" to 48" Plastic for Pipe' UNION ALL
SELECT '2748', 'W4170115', 'Pipe,  1" PVC CL 150' UNION ALL
SELECT '2749', 'W4170202', 'Pipe,  2" PVC' UNION ALL
SELECT '2750', 'W4170215', 'Pipe,  2" PVC CL 150' UNION ALL
SELECT '2751', 'W4170315', 'Pipe,  3" PVC CL 150' UNION ALL
SELECT '2752', 'W4170320', 'Pipe,  3" PVC CL 200' UNION ALL
SELECT '2753', 'W4170340', 'Pipe,  3" PVC Sch 40' UNION ALL
SELECT '2754', 'W4170415', 'Pipe,  4" PVC CL 150' UNION ALL
SELECT '2755', 'W4170425', 'Pipe,  4" PVC CL 250' UNION ALL
SELECT '2756', 'W4170440', 'Pipe,  4" PVC Sch 40' UNION ALL
SELECT '2757', 'W4170615', 'Pipe,  6" PVC CL 150' UNION ALL
SELECT '2758', 'W4170625', 'Pipe,  6" PVC CL 250' UNION ALL
SELECT '2759', 'W4170640', 'Pipe,  6" PVC Sch 40' UNION ALL
SELECT '2760', 'W4170815', 'Pipe,  8" PVC CL 150' UNION ALL
SELECT '2761', 'W4170825', 'Pipe,  8" PVC CL 250' UNION ALL
SELECT '2762', 'W4170H0H', 'Pipe,   3/4" PVC' UNION ALL
SELECT '2763', 'W4171015', 'Pipe, 10" PVC CL 150' UNION ALL
SELECT '2764', 'W4171215', 'Pipe, 12" PVC CL 150' UNION ALL
SELECT '2765', 'W4171415', 'Pipe, 14" PVC CL 150' UNION ALL
SELECT '2766', 'W4171420', 'Pipe, 14" PVC CL 200' UNION ALL
SELECT '2767', 'W4171615', 'Pipe, 16" PVC CL 150' UNION ALL
SELECT '2768', 'W4171815', 'Pipe, 18" PVC CL 150' UNION ALL
SELECT '2769', 'W4171C1C', 'Pipe,  1 1/4" PVC' UNION ALL
SELECT '2770', 'W4171F40', 'Pipe,  1 1/2" PVC Sch 40' UNION ALL
SELECT '2771', 'W4172015', 'Pipe, 20" PVC CL 150' UNION ALL
SELECT '2772', 'W4172415', 'Pipe, 24" PVC CL 150' UNION ALL
SELECT '2773', 'W4172420', 'Pipe, 24" PVC CL 200' UNION ALL
SELECT '2774', 'W4172F15', 'Pipe,  2 1/2" PVC CL 150' UNION ALL
SELECT '2775', 'W4172F15', 'Pipe,  2 1/2" PVC CL 150' UNION ALL
SELECT '2776', 'W4172F15', 'Pipe,  2 1/2" PVC CL 150' UNION ALL
SELECT '2777', 'W4172F15', 'Pipe,  2 1/2" PVC CL 150' UNION ALL
SELECT '2778', 'W4172F15', 'Pipe,  2 1/2" PVC CL 150' UNION ALL
SELECT '2779', 'W4173615', 'Pipe, 36" PVC CL 150' UNION ALL
SELECT '2780', 'W4173620', 'Pipe, 36" PVC CL 200' UNION ALL
SELECT '2781', 'W4180225', 'Pipe,  2" PVC RJ CL 250' UNION ALL
SELECT '2782', 'W4180625', 'Pipe,  6" PVC RJ CL 250' UNION ALL
SELECT '2783', 'W4180825', 'Pipe,  8" PVC RJ CL 250'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 42.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2784', 'W4210101', 'Pipe,  1" Copper Type L' UNION ALL
SELECT '2785', 'W4210202', 'Pipe,  2" Copper Type L' UNION ALL
SELECT '2786', 'W4210F0F', 'Pipe,   1/2" Copper Type L' UNION ALL
SELECT '2787', 'W4210H0H', 'Pipe,   3/4" Copper Type L' UNION ALL
SELECT '2788', 'W4211C1C', 'Pipe,  1 1/4" Copper Type L' UNION ALL
SELECT '2789', 'W4211F1F', 'Pipe,  1 1/2" Copper Type L' UNION ALL
SELECT '2790', 'W4220101', 'Pipe,  1" Copper Type K' UNION ALL
SELECT '2791', 'W4220F0F', 'Pipe,   1/2" Copper Type K' UNION ALL
SELECT '2792', 'W4220H0H', 'Pipe,   3/4" Copper Type K' UNION ALL
SELECT '2793', 'W4221C1C', 'Pipe,  1 1/4" Copper Type K' UNION ALL
SELECT '2794', 'W4221F1F', 'Pipe,  1 1/2" Copper Type K' UNION ALL
SELECT '2795', 'W4230202', 'Pipe,  2" Copper Rigid' UNION ALL
SELECT '2796', 'W4231F1F', 'Pipe,  1 1/2" Copper RIGID' UNION ALL
SELECT '2797', 'W4260101', 'Pipe,  1" PE CTS' UNION ALL
SELECT '2798', 'W4260202', 'Pipe,  2" PE CTS' UNION ALL
SELECT '2799', 'W4260H0H', 'Pipe,   3/4" PE CTS' UNION ALL
SELECT '2800', 'W4261F1F', 'Pipe,  1 1/2" PE CTS' UNION ALL
SELECT '2801', 'W4271C1C', 'Pipe,  1 1/4" PE IPS' UNION ALL
SELECT '2802', 'W4280101', 'Pipe,  1" PB CTS' UNION ALL
SELECT '2803', 'W4280202', 'Pipe,  2" PB CTS' UNION ALL
SELECT '2804', 'W4280F0F', 'Pipe,   1/2" PB CTS' UNION ALL
SELECT '2805', 'W4280H0H', 'Pipe,   3/4" PB CTS' UNION ALL
SELECT '2806', 'W4281F1F', 'Pipe,  1 1/2" PB CTS' UNION ALL
SELECT '2807', 'W4320808', 'Pipe,  8" ST' UNION ALL
SELECT '2808', 'W4321212', 'Pipe, 12" ST' UNION ALL
SELECT '2809', 'W4321616', 'Pipe, 16" ST' UNION ALL
SELECT '2810', 'W4321818', 'Pipe, 18" ST' UNION ALL
SELECT '2811', 'W4322020', 'Pipe, 20" ST' UNION ALL
SELECT '2812', 'W4322424', 'Pipe, 24" ST' UNION ALL
SELECT '2813', 'W4323030', 'Pipe, 30" ST' UNION ALL
SELECT '2814', 'W4330202', 'Pipe,  2" GALV ST' UNION ALL
SELECT '2815', 'W4330202', 'Pipe,  2" GALV ST' UNION ALL
SELECT '2816', 'W4330303', 'Pipe,  3" GALV ST' UNION ALL
SELECT '2817', 'W4330404', 'Pipe,  4" GALV ST' UNION ALL
SELECT '2818', 'W4331F1F', 'Pipe,  1 1/2" GALV ST' UNION ALL
SELECT '2819', 'W4360202', 'Pipe,  2" BR' UNION ALL
SELECT '2820', 'W4361F1F', 'Pipe,  1 1/2" BR' UNION ALL
SELECT '2821', 'W4383636', 'Pipe, 36" Concrete' UNION ALL
SELECT '2822', 'W4420808', 'Pipe,  8" ST Cement Lined' UNION ALL
SELECT '2823', 'W4422020', 'Pipe, 20" ST Cement Lined' UNION ALL
SELECT '2824', 'W4500404', 'Adapter,  4" MJS X MJS' UNION ALL
SELECT '2825', 'W4500606', 'Adapter,  6" MJS X MJS' UNION ALL
SELECT '2826', 'W4500808', 'Adapter,  8" MJS X MJS' UNION ALL
SELECT '2827', 'W4501010', 'Adapter, 10" MJS X MJS' UNION ALL
SELECT '2828', 'W4501212', 'Adapter, 12" MJS X MJS' UNION ALL
SELECT '2829', 'W4501616', 'Adapter, 16" MJS X MJS' UNION ALL
SELECT '2830', 'W4501616', 'Adapter, 16" MJS X MJS' UNION ALL
SELECT '2831', 'W4502020', 'Adapter, 20" MJS X MJS' UNION ALL
SELECT '2832', 'W4950404', 'Post, Indicator' UNION ALL
SELECT '2833', 'W5053001', 'Hydrant, Coronado WB 1-2 1/2"'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 43.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2834', 'W5053002', 'Hydrant, Coronado WB 2-2 1/2"' UNION ALL
SELECT '2835', 'W5053030', 'Elbow, HYD Bury 6" X 16"' UNION ALL
SELECT '2836', 'W5053030', 'Elbow, HYD Bury 6" X 16"' UNION ALL
SELECT '2837', 'W5053050', 'Extension, HYD Spool 6" X  4"' UNION ALL
SELECT '2838', 'W5053051', 'Extension, HYD Spool 6" X  6"' UNION ALL
SELECT '2839', 'W5053052', 'Extension, HYD Spool 6" X 12"' UNION ALL
SELECT '2840', 'W5053053', 'Extension, HYD Spool 6" X 18"' UNION ALL
SELECT '2841', 'W5053080', 'Valve, Hydrant Breakoff Check' UNION ALL
SELECT '2842', 'W5054001', 'Hydrant, Monterey WB 1 2 1/2"' UNION ALL
SELECT '2843', 'W5054002', 'Hydrant, Monterey WB 2 2 1/2"' UNION ALL
SELECT '2844', 'W5055001', 'Hydrant, Rose Mead WB 2-2 1/2"' UNION ALL
SELECT '2845', 'W5055002', 'Hydrant, Rose Mead Steamer Head' UNION ALL
SELECT '2846', 'W5055003', 'Hydrant, Rose Mead Wharf Head' UNION ALL
SELECT '2847', 'W5055030', 'Elbow, HYD Bury 6" X 42" 8-hole' UNION ALL
SELECT '2848', 'W5055051', 'Extension, HYD Spool 6" X  6" 8-hole' UNION ALL
SELECT '2849', 'W5055052', 'Extension, HYD Spool 6" X 12" 8-hole' UNION ALL
SELECT '2850', 'W5055053', 'Elbow, HYD Bury 6" X 42" 6-hole' UNION ALL
SELECT '2851', 'W5055054', 'Extension, HYD Spool 6" X  6" 6-hole' UNION ALL
SELECT '2852', 'W5055055', 'Extension, HYD Spool 6" X 12" 6-hole' UNION ALL
SELECT '2853', 'W5055056', 'Extension, HYD Spool 6" X  4" 6-hole' UNION ALL
SELECT '2854', 'W5055101', 'Hydrant, Village WB 1 2 1/2"' UNION ALL
SELECT '2855', 'W5055102', 'Hydrant, Village WB 2 2 1/2"' UNION ALL
SELECT '2856', 'W5055130', 'Elbow, HYD Bury 6" X 42"' UNION ALL
SELECT '2857', 'W5056001', 'Hydrant, Sacremento WB 1 2 1/2"' UNION ALL
SELECT '2858', 'W5056002', 'Hydrant, Sacremento WB Wharf Head' UNION ALL
SELECT '2859', 'W5056003', 'Hydrant, Sacremento WB 2 1/2" X 4' UNION ALL
SELECT '2860', 'W5056030', 'Elbow, HYD Bury 6" X 36"' UNION ALL
SELECT '2861', 'W5056050', 'Extension, HYD Spool 6" X  4"' UNION ALL
SELECT '2862', 'W5056101', 'Hydrant, Larkfield 1-2 1/2"' UNION ALL
SELECT '2863', 'W5056102', 'Hydrant, Larkfield 2-2 1/2"' UNION ALL
SELECT '2864', 'W5091401', 'Hydrant, Champaign Clow' UNION ALL
SELECT '2865', 'W5091402', 'Hydrant, Champaigne US Pipe' UNION ALL
SELECT '2866', 'W5091403', 'Hydrant, Champaigne Iowa' UNION ALL
SELECT '2867', 'W5091404', 'Hydrant, Champaign Mueller 3'' Bury' UNION ALL
SELECT '2868', 'W5091405', 'Hydrant, Champaign Mueller 3''6" Bury' UNION ALL
SELECT '2869', 'W5091406', 'Hydrant, Champaign Mueller 4'' Bury' UNION ALL
SELECT '2870', 'W5091407', 'Hydrant, Champaign Mueller 4''6" Bury' UNION ALL
SELECT '2871', 'W5091408', 'Hydrant, Champaign Mueller 5'' Bury' UNION ALL
SELECT '2872', 'W5091409', 'Hydrant, Champaign Mueller 5''6" Bury' UNION ALL
SELECT '2873', 'W5091410', 'Hydrant, Champaign Mueller 6'' Bury' UNION ALL
SELECT '2874', 'W5091501', 'Hydrant, Alton' UNION ALL
SELECT '2875', 'W5092440', 'Hydrant, Streator 4'' Bury' UNION ALL
SELECT '2876', 'W5092446', 'Hydrant, Streator 4''6" Bury' UNION ALL
SELECT '2877', 'W5092450', 'Hydrant, Streator 5'' Bury' UNION ALL
SELECT '2878', 'W5092456', 'Hydrant, Streator 5''6" Bury' UNION ALL
SELECT '2879', 'W5092460', 'Hydrant, Streator 6'' Bury' UNION ALL
SELECT '2880', 'W5092501', 'Hydrant, Cairo 2''6" Bury' UNION ALL
SELECT '2881', 'W5092502', 'Hydrant, Cairo 3'' Bury' UNION ALL
SELECT '2882', 'W5092503', 'Hydrant, Cairo 3''6" Bury' UNION ALL
SELECT '2883', 'W5092504', 'Hydrant, Cairo 4'' Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 44.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2884', 'W5092505', 'Hydrant, Cairo 4''6" Bury' UNION ALL
SELECT '2885', 'W5093401', 'Hydrant, Sterling' UNION ALL
SELECT '2886', 'W5093501', 'Hydrant, Interurban 3'' Bury' UNION ALL
SELECT '2887', 'W5093502', 'Hydrant, Interurban 3''6" Bury' UNION ALL
SELECT '2888', 'W5093503', 'Hydrant, Interurban 4'' Bury' UNION ALL
SELECT '2889', 'W5093504', 'Hydrant, Interurban 4''6" Bury' UNION ALL
SELECT '2890', 'W5093505', 'Hydrant, Interurban 5'' Bury' UNION ALL
SELECT '2891', 'W5094440', 'Hydrant, Pontiac 4'' Bury' UNION ALL
SELECT '2892', 'W5094446', 'Hydrant, Pontiac 4''6" Bury' UNION ALL
SELECT '2893', 'W5094450', 'Hydrant, Pontiac 5'' Bury' UNION ALL
SELECT '2894', 'W5094456', 'Hydrant, Pontiac 5''6" Bury' UNION ALL
SELECT '2895', 'W5094460', 'Hydrant, Pontiac 6'' Bury' UNION ALL
SELECT '2896', 'W5095501', 'Hydrant, Pekin 4'' Bury' UNION ALL
SELECT '2897', 'W5095502', 'Hydrant, Pekin 5'' Bury' UNION ALL
SELECT '2898', 'W5095503', 'Hydrant, Pekin 6'' Bury' UNION ALL
SELECT '2899', 'W5096501', 'Hydrant, Peoria 4'' Bury' UNION ALL
SELECT '2900', 'W5096502', 'Hydrant, Peoria 4''6" Bury' UNION ALL
SELECT '2901', 'W5096503', 'Hydrant, Peoria 5'' Bury' UNION ALL
SELECT '2902', 'W5096504', 'Hydrant, Peoria 5''6" Bury' UNION ALL
SELECT '2903', 'W5096505', 'Hydrant, Peoria 6'' Bury' UNION ALL
SELECT '2904', 'W5096506', 'Hydrant, Peoria 6''6" Bury' UNION ALL
SELECT '2905', 'W5097701', 'Hydrant, Lincoln' UNION ALL
SELECT '2906', 'W5098601', 'Hydrant, C-M Water 5''6" Bury' UNION ALL
SELECT '2907', 'W5098602', 'Hydrant, C-M Water 5'' Bury' UNION ALL
SELECT '2908', 'W5098603', 'Hydrant, C-M Water 4'' Bury' UNION ALL
SELECT '2909', 'W5100000', 'Hydrant, Flush/Sample/Blowoff' UNION ALL
SELECT '2910', 'W5101001', 'Hydrant, Kokomo 5'' Bury' UNION ALL
SELECT '2911', 'W5101002', 'Hydrant, Jeffersonville 5 1/4'' Bury' UNION ALL
SELECT '2912', 'W5101003', 'Hydrant, Kokomo 4''6" Bury OL Yellow' UNION ALL
SELECT '2913', 'W5101101', 'Hydrant, Greenwood 5 1/4'' Bury' UNION ALL
SELECT '2914', 'W5101102', 'Hydrant, Kokomo 3''6" Bury OL Yellow' UNION ALL
SELECT '2915', 'W5101103', 'Hydrant, Kokomo 4'' Bury OL Yellow' UNION ALL
SELECT '2916', 'W5101501', 'Hydrant, Muncie' UNION ALL
SELECT '2917', 'W5102501', 'Hydrant, Richmond' UNION ALL
SELECT '2918', 'W5104001', 'Hydrant, Summitville' UNION ALL
SELECT '2919', 'W5104501', 'Hydrant, Wabash 5'' Bury' UNION ALL
SELECT '2920', 'W5104502', 'Hydrant, Wabash 4''6" Bury' UNION ALL
SELECT '2921', 'W5104601', 'Hydrant, Warsaw 5'' Bury' UNION ALL
SELECT '2922', 'W5104602', 'Hydrant, Warsaw 4'' Bury' UNION ALL
SELECT '2923', 'W5104701', 'Hydrant, West Lafayette 5'' Bury' UNION ALL
SELECT '2924', 'W5104702', 'Hydrant, West Lafayette 4'' Bury' UNION ALL
SELECT '2925', 'W5104801', 'Hydrant, Winchester' UNION ALL
SELECT '2926', 'W5105001', 'Hydrant, Crawfordsville' UNION ALL
SELECT '2927', 'W5105501', 'Hydrant, Johnson City' UNION ALL
SELECT '2928', 'W5105801', 'Hydrant, Mooresville 6'' Bury' UNION ALL
SELECT '2929', 'W5105802', 'Hydrant, Mooresville 5'' Bury' UNION ALL
SELECT '2930', 'W5105803', 'Hydrant, Mooresville 4'' Bury' UNION ALL
SELECT '2931', 'W5106001', 'Hydrant, Noblesville' UNION ALL
SELECT '2932', 'W5106002', 'Hydrant, Noblesville 4''6" Bury OL 3Way' UNION ALL
SELECT '2933', 'W5106003', 'Hydrant, Noblesville 5'' Bury OL 3Way'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 45.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2934', 'W5106004', 'Hydrant, Noblesville 5''6" Bury OL 3Way' UNION ALL
SELECT '2935', 'W5106501', 'Hydrant, Shelbyville' UNION ALL
SELECT '2936', 'W5107001', 'Hydrant, Wabash Valley' UNION ALL
SELECT '2937', 'W5107501', 'Hydrant, Southern Ind.' UNION ALL
SELECT '2938', 'W5108001', 'Hydrant, Newburgh' UNION ALL
SELECT '2939', 'W5108501', 'Hydrant, Seymour' UNION ALL
SELECT '2940', 'W5109001', 'Hydrant, Northwest Mueller' UNION ALL
SELECT '2941', 'W5109002', 'Hydrant, Northwest US Pipe' UNION ALL
SELECT '2942', 'W5109003', 'Hydrant, Northwest Kennedy' UNION ALL
SELECT '2943', 'W5110201', 'Hydrant, Quaad Cities' UNION ALL
SELECT '2944', 'W5110202', 'Hydrant, Quaad Cities' UNION ALL
SELECT '2945', 'W5110302', 'Hydrant, Clinton 4''6" Bury' UNION ALL
SELECT '2946', 'W5110303', 'Hydrant, Clinton 5'' bury' UNION ALL
SELECT '2947', 'W5110304', 'Hydrant, Clinton 5''6" Bury' UNION ALL
SELECT '2948', 'W5110305', 'Hydrant, Clinton 6'' Bury' UNION ALL
SELECT '2949', 'W5110306', 'Hydrant, Clinton 4'' Bury' UNION ALL
SELECT '2950', 'W5110307', 'Hydrant, Clinton 6''6" Bury' UNION ALL
SELECT '2951', 'W5110308', 'Hydrant, Clinton 7'' Bury' UNION ALL
SELECT '2952', 'W5120201', 'Hydrant, Bourbon/Owen Co 3''6" Bury' UNION ALL
SELECT '2953', 'W5120202', 'Hydrant, Scott Co 3''6" Bury' UNION ALL
SELECT '2954', 'W5120203', 'Hydrant, Georgetown/Clark 3''6" Bury' UNION ALL
SELECT '2955', 'W5120204', 'Hydrant, Fayette Co 3''6" Bury' UNION ALL
SELECT '2956', 'W5130201', 'Hydrant, Bel Air 4'' Bury' UNION ALL
SELECT '2957', 'W5130202', 'Hydrant, Bel Air 4''6" Bury' UNION ALL
SELECT '2958', 'W5130203', 'Hydrant, Bel Air 5'' Bury' UNION ALL
SELECT '2959', 'W5130204', 'Hydrant, Bel Air 6'' Bury' UNION ALL
SELECT '2960', 'W5130205', 'Hydrant, Bel Air 6''6" Bury' UNION ALL
SELECT '2961', 'W5130206', 'Hydrant, Bel Air 7'' Bury' UNION ALL
SELECT '2962', 'W5130207', 'Hydrant, Bel Air 8''6" Bury' UNION ALL
SELECT '2963', 'W5130208', 'Hydrant, Bel Air 9'' Bury' UNION ALL
SELECT '2964', 'W5160050', 'Hydrant, Michigan 5'' Bury' UNION ALL
SELECT '2965', 'W5160055', 'Hydrant, Michigan 5''6" Bury' UNION ALL
SELECT '2966', 'W5160060', 'Hydrant, Michigan 6'' Bury' UNION ALL
SELECT '2967', 'W5160070', 'Hydrant, Michigan Red 7'' Bury' UNION ALL
SELECT '2968', 'W5160071', 'Hydrant, Michigan Yellow 7'' Bury' UNION ALL
SELECT '2969', 'W5170201', 'Hydrant, St Louis 3''6" Bury' UNION ALL
SELECT '2970', 'W5170202', 'Hydrant, St Louis 4'' Bury' UNION ALL
SELECT '2971', 'W5170203', 'Hydrant, St Louis 4''6" Bury' UNION ALL
SELECT '2972', 'W5170204', 'Hydrant, St Louis 3''6" Bury MJ' UNION ALL
SELECT '2973', 'W5170205', 'Hydrant, St Louis 4'' Bury MJ' UNION ALL
SELECT '2974', 'W5170206', 'Hydrant, St Louis 4''6" Bury' UNION ALL
SELECT '2975', 'W5170207', 'Hydrant, St Louis 4''6" Bury MJ' UNION ALL
SELECT '2976', 'W5170301', 'Hydrant, St Joseph 5'' Bury' UNION ALL
SELECT '2977', 'W5170302', 'Hydrant, St Joseph 4''6" Bury' UNION ALL
SELECT '2978', 'W5170303', 'Hydrant, St Joseph 4'' Bury' UNION ALL
SELECT '2979', 'W5170304', 'Hydrant, St Joseph 4''6" Bury' UNION ALL
SELECT '2980', 'W5170305', 'Hydrant, St Joseph 5'' Bury' UNION ALL
SELECT '2981', 'W5170306', 'Hydrant, St Joseph 5''6"Bury' UNION ALL
SELECT '2982', 'W5170307', 'Hydrant, St Joseph 6'' Bury' UNION ALL
SELECT '2983', 'W5170401', 'Hydrant, Parkville'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 46.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '2984', 'W5170601', 'Hydrant, Warrensburg 5'' Bury' UNION ALL
SELECT '2985', 'W5170602', 'Hydrant, Warrensburg 3''6" Bury' UNION ALL
SELECT '2986', 'W5170603', 'Hydrant, Warrensburg 4'' Bury' UNION ALL
SELECT '2987', 'W5170604', 'Hydrant, Warrensburg 4''6" Bury' UNION ALL
SELECT '2988', 'W5170605', 'Hydrant, Warrensburg 5'' Bury' UNION ALL
SELECT '2989', 'W5170801', 'Hydrant, Brunswick 4'' Bury' UNION ALL
SELECT '2990', 'W5170802', 'Hydrant, Brunswick 4''6" Bury' UNION ALL
SELECT '2991', 'W5170803', 'Hydrant, Brunswick 5'' Bury' UNION ALL
SELECT '2992', 'W5170804', 'Hydrant, Brunswick 5''6" Bury' UNION ALL
SELECT '2993', 'W5170901', 'Hydrant, St Charles 3'' Bury MJ' UNION ALL
SELECT '2994', 'W5170902', 'Hydrant, St Charles 3''6" Bury MJ' UNION ALL
SELECT '2995', 'W5170903', 'Hydrant, St Charles 4'' Bury MJ' UNION ALL
SELECT '2996', 'W5170904', 'Hydrant, St Charles 5'' Bury MJ' UNION ALL
SELECT '2997', 'W5170905', 'Hydrant, St Charles 6'' Bury MJ' UNION ALL
SELECT '2998', 'W5170906', 'Hydrant, St Charles 3'' Bury' UNION ALL
SELECT '2999', 'W5170907', 'Hydrant, St Charles 4'' Bury' UNION ALL
SELECT '3000', 'W5170908', 'Hydrant, St Charles 4''6" Bury' UNION ALL
SELECT '3001', 'W5170909', 'Hydrant, St Charles 5'' Bury' UNION ALL
SELECT '3002', 'W5170910', 'Hydrant, St Charles 6'' Bury' UNION ALL
SELECT '3003', 'W5171001', 'Hydrant, Mexico 5'' Bury' UNION ALL
SELECT '3004', 'W5171002', 'Hydrant, Mexico' UNION ALL
SELECT '3005', 'W5171003', 'Hydrant, Mexico 4''6" Bury' UNION ALL
SELECT '3006', 'W5171101', 'Hydrant, Joplin 3'' Bury J' UNION ALL
SELECT '3007', 'W5171102', 'Hydrant, Joplin 3''6" Bury J' UNION ALL
SELECT '3008', 'W5171103', 'Hydrant, Joplin 4'' Bury J' UNION ALL
SELECT '3009', 'W5171104', 'Hydrant, Joplin 4''6" Bury J' UNION ALL
SELECT '3010', 'W5171105', 'Hydrant, Joplin 5'' Bury J' UNION ALL
SELECT '3011', 'W5171106', 'Hydrant, Joplin 5''6" Bury J' UNION ALL
SELECT '3012', 'W5171201', 'Hydrant, Jefferson' UNION ALL
SELECT '3013', 'W5171501', 'Hydrant, Warren County 3'' Bury MJ' UNION ALL
SELECT '3014', 'W5171502', 'Hydrant, Warren County 3''6" Bury MJ' UNION ALL
SELECT '3015', 'W5171503', 'Hydrant, Warren County 4'' Bury MJ' UNION ALL
SELECT '3016', 'W5171504', 'Hydrant, Warren County 5'' Bury MJ' UNION ALL
SELECT '3017', 'W5171505', 'Hydrant, Warren County 6'' Bury MJ' UNION ALL
SELECT '3018', 'W5171506', 'Hydrant, Warren County 3'' Bury' UNION ALL
SELECT '3019', 'W5171507', 'Hydrant, Warren County 4'' Bury' UNION ALL
SELECT '3020', 'W5171508', 'Hydrant, Warren County 4''6" Bury' UNION ALL
SELECT '3021', 'W5171509', 'Hydrant, Warren County 5'' Bury' UNION ALL
SELECT '3022', 'W5171510', 'Hydrant, Warren County 6'' Bury' UNION ALL
SELECT '3023', 'W5171601', 'Hydrant, Jefferson City 4'' Bury' UNION ALL
SELECT '3024', 'W5171602', 'Hydrant, Jefferson City 4''6" Bury' UNION ALL
SELECT '3025', 'W5171603', 'Hydrant, Jefferson City 5'' Bury' UNION ALL
SELECT '3026', 'W5171604', 'Hydrant, Jefferson City 5''6" Bury' UNION ALL
SELECT '3027', 'W5171605', 'Hydrant, Jefferson City 6'' Bury' UNION ALL
SELECT '3028', 'W5181011', 'Hydrant, Pal 2'' Bury' UNION ALL
SELECT '3029', 'W5181012', 'Hydrant, Pal 2''6" Bury' UNION ALL
SELECT '3030', 'W5181013', 'Hydrant, Pal 3'' Bury' UNION ALL
SELECT '3031', 'W5181014', 'Hydrant, Pal 3''6" Bury' UNION ALL
SELECT '3032', 'W5181015', 'Hydrant, Pal 4'' Bury' UNION ALL
SELECT '3033', 'W5181016', 'Hydrant, Pal 4''6" Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 47.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3034', 'W5181017', 'Hydrant, Pal 5'' Bury' UNION ALL
SELECT '3035', 'W5181018', 'Hydrant, Pal 5''6" Bury' UNION ALL
SELECT '3036', 'W5181019', 'Hydrant, Pal 6'' Bury' UNION ALL
SELECT '3037', 'W5181066', 'Hydrant, Delran 4''6" Bury' UNION ALL
SELECT '3038', 'W5182013', 'Hydrant, PVL 3'' Bury' UNION ALL
SELECT '3039', 'W5182014', 'Hydrant, PVL 3''6" Bury' UNION ALL
SELECT '3040', 'W5182015', 'Hydrant, PVL 4'' Bury' UNION ALL
SELECT '3041', 'W5182016', 'Hydrant, PVL 4''6" Bury' UNION ALL
SELECT '3042', 'W5182017', 'Hydrant, PVL 5'' Bury' UNION ALL
SELECT '3043', 'W5182023', 'Hydrant, OC 3'' Bury' UNION ALL
SELECT '3044', 'W5182024', 'Hydrant, OC 3''6" Bury' UNION ALL
SELECT '3045', 'W5182025', 'Hydrant, OC 4'' Bury' UNION ALL
SELECT '3046', 'W5182026', 'Hydrant, OC 4''6" Bury' UNION ALL
SELECT '3047', 'W5182036', 'Hydrant, Marm 4''6" Bury' UNION ALL
SELECT '3048', 'W5182046', 'Hydrant, RG 4''6" Bury' UNION ALL
SELECT '3049', 'W5183011', 'Hydrant, Lake 2'' Bury' UNION ALL
SELECT '3050', 'W5183012', 'Hydrant, Lake 2''6" Bury' UNION ALL
SELECT '3051', 'W5183013', 'Hydrant, Lake 3'' Bury' UNION ALL
SELECT '3052', 'W5183017', 'Hydrant, Lake 5'' Bury' UNION ALL
SELECT '3053', 'W5183018', 'Hydrant, Lake 5''6"Bury' UNION ALL
SELECT '3054', 'W5183019', 'Hydrant, Lake 6'' Bury' UNION ALL
SELECT '3055', 'W5183021', 'Hydrant, OB 2'' Bury' UNION ALL
SELECT '3056', 'W5183022', 'Hydrant, OB 2''6" Bury' UNION ALL
SELECT '3057', 'W5183023', 'Hydrant, OB 3'' Bury' UNION ALL
SELECT '3058', 'W5183024', 'Hydrant, OB 3''6" Bury' UNION ALL
SELECT '3059', 'W5183025', 'Hydrant, OB 4'' Bury' UNION ALL
SELECT '3060', 'W5183026', 'Hydrant, OB 4''6" Bury' UNION ALL
SELECT '3061', 'W5183027', 'Hydrant, OB 5'' Bury' UNION ALL
SELECT '3062', 'W5183028', 'Hydrant, OB 5''6" Bury' UNION ALL
SELECT '3063', 'W5183029', 'Hydrant, OB 6'' Bury' UNION ALL
SELECT '3064', 'W5184013', 'Hydrant, Asbury 3'' Bury' UNION ALL
SELECT '3065', 'W5184014', 'Hydrant, Asbury 3''6" Bury' UNION ALL
SELECT '3066', 'W5184015', 'Hydrant, Asbury 4'' Bury' UNION ALL
SELECT '3067', 'W5184017', 'Hydrant, Asbury 5'' Bury' UNION ALL
SELECT '3068', 'W5184023', 'Hydrant, Bradley 3'' Bury' UNION ALL
SELECT '3069', 'W5184025', 'Hydrant, Bradley 4'' Bury' UNION ALL
SELECT '3070', 'W5184033', 'Hydrant, OG 3'' Bury' UNION ALL
SELECT '3071', 'W5184035', 'Hydrant, OG 4'' Bury' UNION ALL
SELECT '3072', 'W5184041', 'Hydrant, LB 2'' Bury' UNION ALL
SELECT '3073', 'W5184042', 'Hydrant, LB 2''6" Bury' UNION ALL
SELECT '3074', 'W5184043', 'Hydrant, LB 3'' Bury' UNION ALL
SELECT '3075', 'W5184044', 'Hydrant, LB 3''6" Bury' UNION ALL
SELECT '3076', 'W5184045', 'Hydrant, LB 4'' Bury' UNION ALL
SELECT '3077', 'W5184047', 'Hydrant, LB 5'' Bury' UNION ALL
SELECT '3078', 'W5184048', 'Hydrant, LB 5''6" Bury' UNION ALL
SELECT '3079', 'W5184051', 'Hydrant, Holmdel 2'' Bury' UNION ALL
SELECT '3080', 'W5184052', 'Hydrant, Holmdel 2''6" Bury' UNION ALL
SELECT '3081', 'W5184053', 'Hydrant, Holmdel 3'' Bury' UNION ALL
SELECT '3082', 'W5184054', 'Hydrant, Holmdel 3''6" Bury' UNION ALL
SELECT '3083', 'W5184055', 'Hydrant, Holmdel 4'' Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 48.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3084', 'W5184057', 'Hydrant, Holmdel 5'' Bury' UNION ALL
SELECT '3085', 'W5184058', 'Hydrant, Holmdel 5''6" Bury' UNION ALL
SELECT '3086', 'W5184059', 'Hydrant, Holmdel 6'' Bury' UNION ALL
SELECT '3087', 'W5185012', 'Hydrant, Florham 2''6" Bury' UNION ALL
SELECT '3088', 'W5185013', 'Hydrant, Florham 3'' Bury' UNION ALL
SELECT '3089', 'W5185014', 'Hydrant, Florham 3''6" Bury' UNION ALL
SELECT '3090', 'W5185015', 'Hydrant, Florham 4'' Bury' UNION ALL
SELECT '3091', 'W5185016', 'Hydrant, Florham 4''6" Bury' UNION ALL
SELECT '3092', 'W5185017', 'Hydrant, Florham 5'' Bury' UNION ALL
SELECT '3093', 'W5185018', 'Hydrant, Florham 5''6" Bury' UNION ALL
SELECT '3094', 'W5185019', 'Hydrant, Florham 6'' Bury' UNION ALL
SELECT '3095', 'W5185026', 'Hydrant, Mendham B 4''6" Bury' UNION ALL
SELECT '3096', 'W5185027', 'Hydrant, Mendham B 5'' Bury' UNION ALL
SELECT '3097', 'W5185036', 'Hydrant, Mendham T 4''6"Bury' UNION ALL
SELECT '3098', 'W5185037', 'Hydrant, Mendham T 5'' Bury' UNION ALL
SELECT '3099', 'W5185046', 'Hydrant, Bernardsville 4''6" Bury' UNION ALL
SELECT '3100', 'W5185047', 'Hydrant, Bernardsville 5'' Bury' UNION ALL
SELECT '3101', 'W5185056', 'Hydrant, Berkeley H 4''6" Bury' UNION ALL
SELECT '3102', 'W5185057', 'Hydrant, Berkeley H 5'' Bury' UNION ALL
SELECT '3103', 'W5185066', 'Hydrant, Summit 4''6" Bury' UNION ALL
SELECT '3104', 'W5185067', 'Hydrant, Summit 5'' Bury' UNION ALL
SELECT '3105', 'W5185076', 'Hydrant, Maplewood 4''6" Bury' UNION ALL
SELECT '3106', 'W5185077', 'Hydrant, Maplewood 5'' Bury' UNION ALL
SELECT '3107', 'W5185085', 'Hydrant, Little Falls 4'' Bury' UNION ALL
SELECT '3108', 'W5185086', 'Hydrant, Little Falls 4''6" Bury' UNION ALL
SELECT '3109', 'W5185087', 'Hydrant, Little Falls 5'' Bury' UNION ALL
SELECT '3110', 'W5185103', 'Hydrant, W. Caldwell 3'' Bury' UNION ALL
SELECT '3111', 'W5185105', 'Hydrant, W. Caldwell 4'' Bury' UNION ALL
SELECT '3112', 'W5185106', 'Hydrant, W. Caldwell 4''6" Bury' UNION ALL
SELECT '3113', 'W5185107', 'Hydrant, W. Caldwell 5'' Bury' UNION ALL
SELECT '3114', 'W5186025', 'Hydrant, Wash Boro 4'' Bury' UNION ALL
SELECT '3115', 'W5186026', 'Hydrant, Wash Boro 4''6" Bury' UNION ALL
SELECT '3116', 'W5186027', 'Hydrant, Wash Boro 5'' Bury' UNION ALL
SELECT '3117', 'W5186035', 'Hydrant, Wash Twp 4'' Bury' UNION ALL
SELECT '3118', 'W5186036', 'Hydrant, Wash Twp 4''6" Bury' UNION ALL
SELECT '3119', 'W5186037', 'Hydrant, Wash Twp 5'' Bury' UNION ALL
SELECT '3120', 'W5186045', 'Hydrant, French Twp 4'' Bury' UNION ALL
SELECT '3121', 'W5186046', 'Hydrant, French Twp 4''6" Bury' UNION ALL
SELECT '3122', 'W5186047', 'Hydrant, French Twp 5'' Bury' UNION ALL
SELECT '3123', 'W5186053', 'Hydrant, Belvidere 3'' Bury' UNION ALL
SELECT '3124', 'W5186054', 'Hydrant, Belvidere 3''6" Bury' UNION ALL
SELECT '3125', 'W5186055', 'Hydrant, Belvidere 4'' Bury' UNION ALL
SELECT '3126', 'W5186056', 'Hydrant, Belvidere 4''6" Bury' UNION ALL
SELECT '3127', 'W5186057', 'Hydrant, Belvidere 5'' Bury' UNION ALL
SELECT '3128', 'W5186058', 'Hydrant, Belvidere 5''6" Bury' UNION ALL
SELECT '3129', 'W5186059', 'Hydrant, Belvidere 6'' Bury' UNION ALL
SELECT '3130', 'W5186071', 'Hydrant, Oxford 2'' Bury' UNION ALL
SELECT '3131', 'W5186072', 'Hydrant, Oxford 2''6" Bury' UNION ALL
SELECT '3132', 'W5186073', 'Hydrant, Oxford 3'' Bury' UNION ALL
SELECT '3133', 'W5186074', 'Hydrant, Oxford 3''6" Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 49.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3134', 'W5186075', 'Hydrant, Oxford 4'' Bury' UNION ALL
SELECT '3135', 'W5186076', 'Hydrant, Oxford 4''6" Bury' UNION ALL
SELECT '3136', 'W5186077', 'Hydrant, Oxford 5'' Bury' UNION ALL
SELECT '3137', 'W5186078', 'Hydrant, Oxford 5''6" Bury' UNION ALL
SELECT '3138', 'W5186079', 'Hydrant, Oxford 6'' Bury' UNION ALL
SELECT '3139', 'W5190201', 'Hydrant, Clovis 5 1/4" W/3''6" Bury' UNION ALL
SELECT '3140', 'W5190209', 'Hydrant, Clovis 4 1/2" W/3''6" Bury' UNION ALL
SELECT '3141', 'W5200002', 'Hydrant, #1 (No Marking) 4''6" Bury' UNION ALL
SELECT '3142', 'W5200003', 'Hydrant, #2 (Yellow Band) 4''6" Bury' UNION ALL
SELECT '3143', 'W5200004', 'Hydrant, #3 (Green Band) 4''6" Bury' UNION ALL
SELECT '3144', 'W5200005', 'Hydrant, 4''6" Bury (Elizabeth City Specs)' UNION ALL
SELECT '3145', 'W5220000', 'Hydrant, Ashtabula 4'' Bury' UNION ALL
SELECT '3146', 'W5220001', 'Hydrant, Ashtabula 5'' Bury' UNION ALL
SELECT '3147', 'W5220002', 'Hydrant, Ashtabula 6'' Bury' UNION ALL
SELECT '3148', 'W5220003', 'Hydrant, Ashtabula 4''6" Bury' UNION ALL
SELECT '3149', 'W5220100', 'Hydrant, Tiffin 4''6" Bury' UNION ALL
SELECT '3150', 'W5220101', 'Hydrant, Tiffin 5'' Bury' UNION ALL
SELECT '3151', 'W5220102', 'Hydrant, Tiffin 4'' Bury' UNION ALL
SELECT '3152', 'W5220103', 'Hydrant, Tiffin 5''6" Bury' UNION ALL
SELECT '3153', 'W5220104', 'Hydrant, Tiffin 6'' Bury' UNION ALL
SELECT '3154', 'W5220601', 'Hydrant, Lawrence County 4'' Bury 2.5" Nozzel' UNION ALL
SELECT '3155', 'W5220602', 'Hydrant, Lawrence County 4.5" Nozzel' UNION ALL
SELECT '3156', 'W5224001', 'Hydrant, Marion 5'' Bury' UNION ALL
SELECT '3157', 'W5224002', 'Hydrant, Marion 4''6" Bury' UNION ALL
SELECT '3158', 'W5225001', 'Hydrant, Franklin 5'' Bury' UNION ALL
SELECT '3159', 'W5230201', 'Hydrant, Paradise Valley 5 1/4" w/4''6" Bury' UNION ALL
SELECT '3160', 'W5236201', 'Hydrant, Sun City 5 1/4" w/4'' 6" Bury' UNION ALL
SELECT '3161', 'W5237101', 'Hydrant, Mohave 5 1/4" w/4''6" Bury' UNION ALL
SELECT '3162', 'W5241101', 'Hydrant, Munhall 3''6" Bury' UNION ALL
SELECT '3163', 'W5241102', 'Hydrant, Munhall 4'' Bury' UNION ALL
SELECT '3164', 'W5241103', 'Hydrant, Munhall 4''6" Bury' UNION ALL
SELECT '3165', 'W5241104', 'Hydrant, Munhall 5'' Bury' UNION ALL
SELECT '3166', 'W5241105', 'Hydrant, Munhall 5''6" Bury' UNION ALL
SELECT '3167', 'W5241106', 'Hydrant, Bethel Park 3''6" Bury' UNION ALL
SELECT '3168', 'W5241107', 'Hydrant, Bethel Park 4'' Bury' UNION ALL
SELECT '3169', 'W5241108', 'Hydrant, Bethel Park 4''6" Bury' UNION ALL
SELECT '3170', 'W5241109', 'Hydrant, Bethel Park 5'' Bury' UNION ALL
SELECT '3171', 'W5241110', 'Hydrant, Bethel Park 5''6" Bury' UNION ALL
SELECT '3172', 'W5241111', 'Hydrant, Homestead 3''6" Bury' UNION ALL
SELECT '3173', 'W5241112', 'Hydrant, Homestead 4'' Bury' UNION ALL
SELECT '3174', 'W5241113', 'Hydrant, Homestead 4''6" Bury' UNION ALL
SELECT '3175', 'W5241114', 'Hydrant, Homestead 5'' Bury' UNION ALL
SELECT '3176', 'W5241115', 'Hydrant, Peters Twp 3''6" Bury' UNION ALL
SELECT '3177', 'W5241116', 'Hydrant, Peters Twp 4'' Bury' UNION ALL
SELECT '3178', 'W5241117', 'Hydrant, Peters Twp 4''6" Bury' UNION ALL
SELECT '3179', 'W5241118', 'Hydrant, Peters Twp 5'' Bury' UNION ALL
SELECT '3180', 'W5241119', 'Hydrant, Union Twp 3''6" Bury' UNION ALL
SELECT '3181', 'W5241120', 'Hydrant, Union Twp 4'' Bury' UNION ALL
SELECT '3182', 'W5241121', 'Hydrant, Union Twp 4''6" Bury' UNION ALL
SELECT '3183', 'W5241122', 'Hydrant, Union Twp 5'' Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 50.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3184', 'W5242101', 'Hydrant, Peters Twp 4''6" Bury' UNION ALL
SELECT '3185', 'W5242102', 'Hydrant, McDonald 4''6" Bury' UNION ALL
SELECT '3186', 'W5242103', 'Hydrant, So Fayette 4''6" Bury OR' UNION ALL
SELECT '3187', 'W5242104', 'Hydrant, Canonsburg 4''6" Bury OL' UNION ALL
SELECT '3188', 'W5242201', 'Hydrant, Elizabeth Twp. 4'' Bury' UNION ALL
SELECT '3189', 'W5242202', 'Hydrant, Elizabeth Twp. 4''6" Bury' UNION ALL
SELECT '3190', 'W5242203', 'Hydrant, Elizabeth Twp. 5'' Bury' UNION ALL
SELECT '3191', 'W5242204', 'Hydrant, Mon City 4'' Bury' UNION ALL
SELECT '3192', 'W5242205', 'Hydrant, Mon City 4''6" Bury' UNION ALL
SELECT '3193', 'W5242206', 'Hydrant, Mon City 5'' Bury' UNION ALL
SELECT '3194', 'W5242207', 'Hydrant, Jefferson 4'' Bury' UNION ALL
SELECT '3195', 'W5242208', 'Hydrant, Jefferson 4''6" Bury' UNION ALL
SELECT '3196', 'W5242209', 'Hydrant, Jefferson 5'' Bury' UNION ALL
SELECT '3197', 'W5242210', 'Hydrant, Valley 4'' Bury' UNION ALL
SELECT '3198', 'W5242211', 'Hydrant, Valley 4''6" Bury' UNION ALL
SELECT '3199', 'W5242212', 'Hydrant, Valley 5'' Bury' UNION ALL
SELECT '3200', 'W5242301', 'Hydrant, Uniontown 3'' Bury' UNION ALL
SELECT '3201', 'W5242302', 'Hydrant, Uniontown 4'' Bury' UNION ALL
SELECT '3202', 'W5242303', 'Hydrant, Connellsville 4'' Bury' UNION ALL
SELECT '3203', 'W5242501', 'Hydrant, Brownsville Cal 4''' UNION ALL
SELECT '3204', 'W5242502', 'Hydrant, Redstone 4'' Bury' UNION ALL
SELECT '3205', 'W5243101', 'Hydrant, Elwood 4'' Bury' UNION ALL
SELECT '3206', 'W5243102', 'Hydrant, New Castle 4'' Bury' UNION ALL
SELECT '3207', 'W5243301', 'Hydrant, Butler NST 4'' Bury' UNION ALL
SELECT '3208', 'W5243302', 'Hydrant, Butler NST 5'' Bury' UNION ALL
SELECT '3209', 'W5243303', 'Hydrant, Butler 4'' Bury' UNION ALL
SELECT '3210', 'W5243304', 'Hydrant, Butler 5'' Bury' UNION ALL
SELECT '3211', 'W5243305', 'Hydrant, Kittanning 4'' Bury' UNION ALL
SELECT '3212', 'W5243306', 'Hydrant, Kittanning 5'' Bury' UNION ALL
SELECT '3213', 'W5244101', 'Hydrant, Indiana 3''6" Bury' UNION ALL
SELECT '3214', 'W5244102', 'Hydrant, Indiana 4'' Bury' UNION ALL
SELECT '3215', 'W5244103', 'Hydrant, Indiana 5'' Bury' UNION ALL
SELECT '3216', 'W5244104', 'Hydrant, Punxsutawney 4'' Bury' UNION ALL
SELECT '3217', 'W5244105', 'Hydrant, Punxsutawney 5'' Bury' UNION ALL
SELECT '3218', 'W5244301', 'Hydrant, Clairion 5'' Bury' UNION ALL
SELECT '3219', 'W5244501', 'Hydrant, Warren 4''6" Bury' UNION ALL
SELECT '3220', 'W5244502', 'Hydrant, Warren 5'' Bury' UNION ALL
SELECT '3221', 'W5244601', 'Hydrant, Kane 4 1/2" 4''6" Bury' UNION ALL
SELECT '3222', 'W5244602', 'Hydrant, Kane 4 1/2" 5'' Bury' UNION ALL
SELECT '3223', 'W5244603', 'Hydrant, Kane 5 1/4" 4''6" Bury' UNION ALL
SELECT '3224', 'W5245101', 'Hydrant, East Norriton 3''6" Bury' UNION ALL
SELECT '3225', 'W5245102', 'Hydrant, East Norriton 4'' Bury' UNION ALL
SELECT '3226', 'W5245103', 'Hydrant, East Norriton 4''6" Bury' UNION ALL
SELECT '3227', 'W5245104', 'Hydrant, East Norriton 5'' Bury' UNION ALL
SELECT '3228', 'W5245105', 'Hydrant, Norristown 3''6" Bury' UNION ALL
SELECT '3229', 'W5245106', 'Hydrant, Norristown 4'' Bury' UNION ALL
SELECT '3230', 'W5245107', 'Hydrant, Norristown 4''6" Bury' UNION ALL
SELECT '3231', 'W5245108', 'Hydrant, Norriston 5'' Bury' UNION ALL
SELECT '3232', 'W5245201', 'Hydrant, Yardley 5'' Bury' UNION ALL
SELECT '3233', 'W5245301', 'Hydrant, Abington 4'' Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 51.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3234', 'W5245302', 'Hydrant, Abington 5'' Bury' UNION ALL
SELECT '3235', 'W5245401', 'Hydrant, Susquehanna 5'' Bury' UNION ALL
SELECT '3236', 'W5245501', 'Hydrant, Bangor 4'' Bury' UNION ALL
SELECT '3237', 'W5245502', 'Hydrant, Bangor 4''6" Bury' UNION ALL
SELECT '3238', 'W5245503', 'Hydrant, Washington Twp 4'' Bury' UNION ALL
SELECT '3239', 'W5245504', 'Hydrant, Washington Twp 4''6"' UNION ALL
SELECT '3240', 'W5245601', 'Hydrant, Pen Argyl 4'' Bury' UNION ALL
SELECT '3241', 'W5245602', 'Hydrant, Pen Argyl 4''6" Bury' UNION ALL
SELECT '3242', 'W5245603', 'Hydrant, Pen Argyl 5'' Bury' UNION ALL
SELECT '3243', 'W5245604', 'Hydrant, Nazareth 4'' Bury' UNION ALL
SELECT '3244', 'W5245605', 'Hydrant, Nazareth 4''6" Bury' UNION ALL
SELECT '3245', 'W5245606', 'Hydrant, Nazareth 5'' Bury' UNION ALL
SELECT '3246', 'W5245607', 'Hydrant, NST 4'' Bury' UNION ALL
SELECT '3247', 'W5245608', 'Hydrant, NST 4''6" Bury' UNION ALL
SELECT '3248', 'W5245609', 'Hydrant, NST 5'' Bury' UNION ALL
SELECT '3249', 'W5245610', 'Hydrant, NST 5''6" Bury' UNION ALL
SELECT '3250', 'W5245611', 'Hydrant, Easton Suburban 4''6" Bury' UNION ALL
SELECT '3251', 'W5245612', 'Hydrant, Easton Suburban 5'' Bury' UNION ALL
SELECT '3252', 'W5245701', 'Hydrant, Pocono 4'' Bury' UNION ALL
SELECT '3253', 'W5245702', 'Hydrant, Pocono 4''6" Bury' UNION ALL
SELECT '3254', 'W5245703', 'Hydrant, Pocono 5'' Bury' UNION ALL
SELECT '3255', 'W5245704', 'Hydrant, Pocono 5''6" Bury' UNION ALL
SELECT '3256', 'W5245705', 'Hydrant, Pocono 6'' Bury' UNION ALL
SELECT '3257', 'W5245901', 'Hydrant, Exeter Twp 4''6" Bury' UNION ALL
SELECT '3258', 'W5245902', 'Hydrant, Exeter Twp 5'' Bury' UNION ALL
SELECT '3259', 'W5245903', 'Hydrant, Amity Twp 4''6" Bury' UNION ALL
SELECT '3260', 'W5245904', 'Hydrant, Amity Twp 5'' Bury' UNION ALL
SELECT '3261', 'W5246101', 'Hydrant, Quick Connect 4'' Bury' UNION ALL
SELECT '3262', 'W5246102', 'Hydrant, Quick Connect 4''6" Bury' UNION ALL
SELECT '3263', 'W5246103', 'Hydrant, Quick Connect 5'' Bury' UNION ALL
SELECT '3264', 'W5246301', 'Hydrant, Heidelberg 5'' Public' UNION ALL
SELECT '3265', 'W5246302', 'Hydrant, Heidelberg 5'' Private' UNION ALL
SELECT '3266', 'W5246303', 'Hydrant, Reading 3''6" Public' UNION ALL
SELECT '3267', 'W5246304', 'Hydrant, Reading 4'' Public' UNION ALL
SELECT '3268', 'W5246305', 'Hydrant, Reading 5'' Public' UNION ALL
SELECT '3269', 'W5246306', 'Hydrant, Quick Connect 3''6" Public' UNION ALL
SELECT '3270', 'W5246307', 'Hydrant, Quick Connect 4'' Public' UNION ALL
SELECT '3271', 'W5246308', 'Hydrant, Quick Connect 4''6" Public' UNION ALL
SELECT '3272', 'W5246401', 'Hydrant, Royersford 4''6" Bury' UNION ALL
SELECT '3273', 'W5246402', 'Hydrant, Royersford 5'' Bury' UNION ALL
SELECT '3274', 'W5246501', 'Hydrant, Coatesville 3'' Bury' UNION ALL
SELECT '3275', 'W5246502', 'Hydrant, Coatesville 3''6" Bury' UNION ALL
SELECT '3276', 'W5246503', 'Hydrant, Coatesville 4'' Bury' UNION ALL
SELECT '3277', 'W5246504', 'Hydrant, Coatesville 4''6" Bury' UNION ALL
SELECT '3278', 'W5246505', 'Hydrant, Coatesville, Grip 4''6" Bury' UNION ALL
SELECT '3279', 'W5246506', 'Hydrant, Coatesville 5'' Bury' UNION ALL
SELECT '3280', 'W5246507', 'Hydrant, Coatesville 5''6" Bury' UNION ALL
SELECT '3281', 'W5246508', 'Hydrant, Coatesville 6'' Bury' UNION ALL
SELECT '3282', 'W5246509', 'Hydrant, Coatesville 6''6" Bury' UNION ALL
SELECT '3283', 'W5246510', 'Hydrant, Coatesville 7'' Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 52.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3284', 'W5246511', 'Hydrant, Caln 3'' Bury' UNION ALL
SELECT '3285', 'W5246512', 'Hydrant, Caln 3''6" Bury' UNION ALL
SELECT '3286', 'W5246513', 'Hydrant, Caln 4'' Bury' UNION ALL
SELECT '3287', 'W5246514', 'Hydrant, Caln 4''6" Bury' UNION ALL
SELECT '3288', 'W5246515', 'Hydrant, Caln 5'' Bury' UNION ALL
SELECT '3289', 'W5246516', 'Hydrant, Caln 5''6" Bury' UNION ALL
SELECT '3290', 'W5246517', 'Hydrant, Caln 6'' Bury' UNION ALL
SELECT '3291', 'W5246518', 'Hydrant, Caln 6''5" Bury' UNION ALL
SELECT '3292', 'W5246519', 'Hydrant, Caln 7'' Bury' UNION ALL
SELECT '3293', 'W5246801', 'Hydrant, Bushkill 5'' Bury' UNION ALL
SELECT '3294', 'W5247101', 'Hydrant, Lewisburg 4'' Bury' UNION ALL
SELECT '3295', 'W5247102', 'Hydrant, Lewisburg STD 4''6" Bury' UNION ALL
SELECT '3296', 'W5247103', 'Hydrant, Lewisburg 4''6" Bury' UNION ALL
SELECT '3297', 'W5247104', 'Hydrant, Lewisburg 5'' Bury' UNION ALL
SELECT '3298', 'W5247105', 'Hydrant, Milton 4'' Bury' UNION ALL
SELECT '3299', 'W5247106', 'Hydrant, Milton 4''6" Bury' UNION ALL
SELECT '3300', 'W5247107', 'Hydrant, Milton STD 4''6" Bury' UNION ALL
SELECT '3301', 'W5247108', 'Hydrant, Milton 5'' Bury' UNION ALL
SELECT '3302', 'W5247109', 'Hydrant, N Umberland 4'' Bury' UNION ALL
SELECT '3303', 'W5247110', 'Hydrant, N Umberland STD 4''6" Bury' UNION ALL
SELECT '3304', 'W5247111', 'Hydrant, N Umberland 4''6" Bury' UNION ALL
SELECT '3305', 'W5247112', 'Hydrant, N Umberland 5'' Bury' UNION ALL
SELECT '3306', 'W5247201', 'Hydrant, Osceola Mills 4''6" Bury' UNION ALL
SELECT '3307', 'W5247202', 'Hydrant, Osceola Mills 5'' Bury' UNION ALL
SELECT '3308', 'W5247203', 'Hydrant, Philipsburg 4''6" Bury' UNION ALL
SELECT '3309', 'W5247204', 'Hydrant, Philipsburg 5'' Bury' UNION ALL
SELECT '3310', 'W5247301', 'Hydrant, Berwick 5'' Bury' UNION ALL
SELECT '3311', 'W5247401', 'Hydrant, Frackville 5'' Bury' UNION ALL
SELECT '3312', 'W5249101', 'Hydrant, Wilkes Barre 5''6" Bury' UNION ALL
SELECT '3313', 'W5249102', 'Hydrant, Pittston 5''6" Bury' UNION ALL
SELECT '3314', 'W5249103', 'Hydrant, Moosic 5''6" Bury' UNION ALL
SELECT '3315', 'W5249104', 'Hydrant, Nanticoke 5''6" Bury' UNION ALL
SELECT '3316', 'W5249105', 'Hydrant, S. Abington 5''6" Bury' UNION ALL
SELECT '3317', 'W5249106', 'Hydrant, Berwick 5''6" Bury' UNION ALL
SELECT '3318', 'W5249107', 'Hydrant, Mocanaqua 5''6" Bury' UNION ALL
SELECT '3319', 'W5249108', 'Hydrant, Trucksville 5''6" Bury' UNION ALL
SELECT '3320', 'W5260201', 'Hydrant, Chattanooga NTP' UNION ALL
SELECT '3321', 'W5260202', 'Hydrant, Chattanooga TP' UNION ALL
SELECT '3322', 'W5270101', 'Hydrant, Alexandria 3''6" Bury' UNION ALL
SELECT '3323', 'W5270102', 'Hydrant, Alexandria 4''6" Bury' UNION ALL
SELECT '3324', 'W5270201', 'Hydrant, Hopewell 3'' Bury' UNION ALL
SELECT '3325', 'W5270202', 'Hydrant, Hopewell 3''6" Bury' UNION ALL
SELECT '3326', 'W5270203', 'Hydrant, Prince George 3'' Bury' UNION ALL
SELECT '3327', 'W5270204', 'Hydrant, Prince George 3''6" Bury' UNION ALL
SELECT '3328', 'W5270205', 'Hydrant, Fort Lee 3''6" Bury' UNION ALL
SELECT '3329', 'W5270301', 'Hydrant, Prince William 4''6" Bury' UNION ALL
SELECT '3330', 'W5281101', 'Hydrant, Weston 3''6" Bury' UNION ALL
SELECT '3331', 'W5281201', 'Hydrant, Gassaway 4'' Bury' UNION ALL
SELECT '3332', 'W5281401', 'Hydrant, Webster Springs 4'' Bury' UNION ALL
SELECT '3333', 'W5282201', 'Hydrant, Princeton 3''6" Bury'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 53.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3334', 'W5282202', 'Hydrant, Princeton 4'' Bury' UNION ALL
SELECT '3335', 'W5282203', 'Hydrant, Princeton 4''6" Bury' UNION ALL
SELECT '3336', 'W5282204', 'Hydrant, Princeton 5'' Bury' UNION ALL
SELECT '3337', 'W5282301', 'Hydrant, Oak Hill 4'' Bury' UNION ALL
SELECT '3338', 'W5282401', 'Hydrant, Hinton 4'' Bury' UNION ALL
SELECT '3339', 'W5282402', 'Hydrant, Hinton 5'' Bury' UNION ALL
SELECT '3340', 'W5283001', 'Hydrant, Charleston 3'' Bury' UNION ALL
SELECT '3341', 'W5283002', 'Hydrant, Charleston 3''6" Bury' UNION ALL
SELECT '3342', 'W5283003', 'Hydrant, Charleston 4'' Bury' UNION ALL
SELECT '3343', 'W5283004', 'Hydrant, Charleston 4''6" Bury' UNION ALL
SELECT '3344', 'W5283005', 'Hydrant, Charleston 5'' Bury' UNION ALL
SELECT '3345', 'W5283006', 'Hydrant, Charleston 5''6" Bury' UNION ALL
SELECT '3346', 'W5283007', 'Hydrant, Charleston 6'' Bury' UNION ALL
SELECT '3347', 'W5284001', 'Hydrant, Huntington 3''6" Bury' UNION ALL
SELECT '3348', 'W5284002', 'Hydrant, Huntington 4'' Bury' UNION ALL
SELECT '3349', 'W5284003', 'Hydrant, Huntington 4''6" Bury' UNION ALL
SELECT '3350', 'W5284004', 'Hydrant, Huntington 5'' Bury' UNION ALL
SELECT '3351', 'W5284005', 'Hydrant, Huntington 5''6" Bury' UNION ALL
SELECT '3352', 'W5284006', 'Hydrant, Huntington 6'' Bury' UNION ALL
SELECT '3353', 'W5284701', 'Hydrant, Salt Rock 3''6" Bury' UNION ALL
SELECT '3354', 'W5284702', 'Hydrant, Salt Rock 4'' Bury' UNION ALL
SELECT '3355', 'W5284703', 'Hydrant, Salt Rock 4''6" Bury' UNION ALL
SELECT '3356', 'W5284704', 'Hydrant, Salt Rock 5'' Bury' UNION ALL
SELECT '3357', 'W5300001', 'Hydrant, 4''6" Bury OL (Mount Holly)' UNION ALL
SELECT '3358', 'W5381016', 'Hydrant, LI OL 4''6" Bury' UNION ALL
SELECT '3359', 'W5381026', 'Hydrant, LI OR 4''6" Bury' UNION ALL
SELECT '3360', 'W5540202', 'Hydrant, 2" Blow-off Assembly' UNION ALL
SELECT '3361', 'W5590624', 'Hydrant, Anchor Piece 6" X 24"' UNION ALL
SELECT '3362', 'W5590636', 'Hydrant, Anchor Piece 6" X 36"' UNION ALL
SELECT '3363', 'W5600606', 'Extension, Hydrant  6"' UNION ALL
SELECT '3364', 'W5601212', 'Extension, Hydrant 12"' UNION ALL
SELECT '3365', 'W5601818', 'Extension, Hydrant 18"' UNION ALL
SELECT '3366', 'W5602424', 'Extension, Hydrant 24"' UNION ALL
SELECT '3367', 'W5603030', 'Extension, Hydrant 30"' UNION ALL
SELECT '3368', 'W5603636', 'Extension, Hydrant 36"' UNION ALL
SELECT '3369', 'W5604242', 'Extension, Hydrant 42"' UNION ALL
SELECT '3370', 'W5604848', 'Extension, Hydrant 48"' UNION ALL
SELECT '3371', 'W5605C24', 'Extension, 5 1/4" X 24" Hydrant' UNION ALL
SELECT '3372', 'W5610604', 'Extension, 6" X  4" Wet Barrel' UNION ALL
SELECT '3373', 'W5610612', 'Extension, 6" X 12" Wet Barrel' UNION ALL
SELECT '3374', 'W5610618', 'Extension, 6" X 18" Wet Barrel' UNION ALL
SELECT '3375', 'W600010F', 'Bushing, 1" X 1/2" Slip X FIP PVC' UNION ALL
SELECT '3376', 'W600010H', 'Bushing, 1" X 3/4" Slip X FIP PVC' UNION ALL
SELECT '3377', 'W6000H0C', 'Bushing,  3/4" X 1/4" Slip X FIP PVC' UNION ALL
SELECT '3378', 'W601010H', 'Bushing, 1" X 3/4" PVC Taper X Taper' UNION ALL
SELECT '3379', 'W602010F', 'Bushing, 1" X 1/2" Slip PVC' UNION ALL
SELECT '3380', 'W602010H', 'Bushing, 1" X 3/4" Slip PVC' UNION ALL
SELECT '3381', 'W6020201', 'Bushing, 2" X 1" Slip PVC' UNION ALL
SELECT '3382', 'W602021C', 'Bushing, 2" X 1 1/4" Slip PVC' UNION ALL
SELECT '3383', 'W602021F', 'Bushing, 2" X 1 1/2" Slip PVC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 54.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3384', 'W6020302', 'Bushing, 3" X 2" Slip PVC' UNION ALL
SELECT '3385', 'W6020402', 'Bushing, 4" X 2" Slip PVC' UNION ALL
SELECT '3386', 'W6020403', 'Bushing, 4" X 3" Slip PVC' UNION ALL
SELECT '3387', 'W6030101', 'Ring,  1" Two part CF coupling' UNION ALL
SELECT '3388', 'W6050101', 'Stiffener, 1" PE pipe' UNION ALL
SELECT '3389', 'W6050202', 'Stiffener, 2" PE pipe' UNION ALL
SELECT '3390', 'W6050H0H', 'Stiffener,  3/4" PE pipe' UNION ALL
SELECT '3391', 'W6080202', 'Tee,  2" CS X CSX CS' UNION ALL
SELECT '3392', 'W6081F1F', 'Tee,  1 1/2" CS X CSX CS' UNION ALL
SELECT '3393', 'W6090202', 'Coupling,  2" CS X MIP' UNION ALL
SELECT '3394', 'W6091F1F', 'Coupling,  1 1/2" CS X MIP' UNION ALL
SELECT '3395', 'W6100202', 'Coupling,  2" CS X FIP' UNION ALL
SELECT '3396', 'W6101F1F', 'Coupling,  1 1/2" CS X FIP' UNION ALL
SELECT '3397', 'W6110202', 'Union, 2" CS X CS' UNION ALL
SELECT '3398', 'W6111F1F', 'Union, 1 1/2" CS X CS' UNION ALL
SELECT '3399', 'W6120202', 'Coupling,  2" CS X CS' UNION ALL
SELECT '3400', 'W6121F1F', 'Coupling,  1 1/2" CS X CS' UNION ALL
SELECT '3401', 'W6130202', 'Bend,  2" CS X CS 45°' UNION ALL
SELECT '3402', 'W6131F1F', 'Bend,  1 1/2" CS X CS 45°' UNION ALL
SELECT '3403', 'W6140202', 'Bend,  2" CS X CS 90°' UNION ALL
SELECT '3404', 'W6141F1F', 'Bend,  1 1/2" CS X CS 90°' UNION ALL
SELECT '3405', 'W6160H0H', 'Cross,   3/4" BR FIP X FIP' UNION ALL
SELECT '3406', 'W6161C1C', 'Cross,  1 1/4" BR FIP X FIP' UNION ALL
SELECT '3407', 'W6190101', 'Bend,  1" SPT X CF 45°' UNION ALL
SELECT '3408', 'W619010H', 'Bend,  1" X  3/4" SPT X CF 45°' UNION ALL
SELECT '3409', 'W6190G0H', 'Bend,   5/8" X 3/4" SPT X CF 45°' UNION ALL
SELECT '3410', 'W6190H01', 'Bend,   3/4" X 1" SPT X CF 45°' UNION ALL
SELECT '3411', 'W6190H0H', 'Bend,   3/4" X  3/4" SPT X CF 45°' UNION ALL
SELECT '3412', 'W6220101', 'Bend,  1" TNA X CC Swivel 90°' UNION ALL
SELECT '3413', 'W6220202', 'Bend,  2" TNA X CC Swivel 90°' UNION ALL
SELECT '3414', 'W6221F1F', 'Bend,  1 1/2" TNA X CC Swivel 90°' UNION ALL
SELECT '3415', 'W6230101', 'Bend,  1" TNA X CC Swivel 45°' UNION ALL
SELECT '3416', 'W6230202', 'Bend,  2" TNA X CC Swivel 45°' UNION ALL
SELECT '3417', 'W6230G0H', 'Bend,   5/8" X 3/4" TNA X CC Swivel  45°' UNION ALL
SELECT '3418', 'W6230H0H', 'Bend,   3/4" X  3/4" TNA X CC Swivel 45°' UNION ALL
SELECT '3419', 'W6231F1F', 'Bend,  1 1/2" TNA X CC Swivel 45°' UNION ALL
SELECT '3420', 'W6240101', 'Bend,  1" CC X MIP 45°' UNION ALL
SELECT '3421', 'W6240H0H', 'Bend,   3/4" X  3/4" CC X MIP 45°' UNION ALL
SELECT '3422', 'W6270101', 'Coupling,  1" TNA X MIP' UNION ALL
SELECT '3423', 'W6270202', 'Coupling,  2" TNA X MIP' UNION ALL
SELECT '3424', 'W6270H0H', 'Coupling,   3/4" TNA X MIP' UNION ALL
SELECT '3425', 'W6271F1F', 'Coupling,  1 1/2" TNA X MIP' UNION ALL
SELECT '3426', 'W6280101', 'Coupling,  1" MIT X FIP' UNION ALL
SELECT '3427', 'W6300101', 'Coupling,  1" FIP X FIP ST HD' UNION ALL
SELECT '3428', 'W6300202', 'Coupling,  2" FIP X FIP ST HD' UNION ALL
SELECT '3429', 'W6300303', 'Coupling,  3" FIP X FIP ST HD' UNION ALL
SELECT '3430', 'W6300404', 'Coupling,  4" FIP X FIP ST HD' UNION ALL
SELECT '3431', 'W6300606', 'Coupling,  6" FIP X FIP ST HD' UNION ALL
SELECT '3432', 'W6301F1F', 'Coupling,  1 1/2" FIP X FIP ST HD' UNION ALL
SELECT '3433', 'W6302F2F', 'Coupling,  2 1/2" FIP X FIP ST HD'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 55.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3434', 'W6330101', 'Coupling,  1" Insulated TNA X CC' UNION ALL
SELECT '3435', 'W6330H0H', 'Coupling,   3/4" Insulated TNA X CC' UNION ALL
SELECT '3436', 'W6340101', 'Coupling,  1" Insulated TNA X CF' UNION ALL
SELECT '3437', 'W6340H0H', 'Coupling,   3/4" Insulated TNA X CF' UNION ALL
SELECT '3438', 'W6360201', 'Bushing, 2" X 1" Taper X Taper' UNION ALL
SELECT '3439', 'W640010H', 'Coupling,  1" X  3/4" GALV IPC X IPC' UNION ALL
SELECT '3440', 'W6400H0H', 'Coupling,   3/4" GALV IPC X IPC' UNION ALL
SELECT '3441', 'W6430101', 'Coupling,  1" GALV CC X MIP' UNION ALL
SELECT '3442', 'W6430202', 'Coupling,  2" GALV CC X MIP' UNION ALL
SELECT '3443', 'W6430F0F', 'Coupling,   1/2" GALV CC X MIP' UNION ALL
SELECT '3444', 'W6430H0H', 'Coupling,   3/4" GALV CC X MIP' UNION ALL
SELECT '3445', 'W6431C1C', 'Coupling,  1 1/4" GALV CC X MIP' UNION ALL
SELECT '3446', 'W6431F1F', 'Coupling,  1 1/2" GALV CC X MIP' UNION ALL
SELECT '3447', 'W6440101', 'Coupling,  1" GALV CC X FIP' UNION ALL
SELECT '3448', 'W6440202', 'Coupling,  2" GALV CC X FIP' UNION ALL
SELECT '3449', 'W6440F0F', 'Coupling,   1/2" GALV CC X FIP' UNION ALL
SELECT '3450', 'W6440H0H', 'Coupling,   3/4" GALV CC X FIP' UNION ALL
SELECT '3451', 'W6441C1C', 'Coupling,  1 1/4" GALV CC X FIP' UNION ALL
SELECT '3452', 'W6441F1F', 'Coupling,  1 1/2" GALV CC X FIP' UNION ALL
SELECT '3453', 'W6450101', 'Coupling,  1" GALV CC X CC' UNION ALL
SELECT '3454', 'W6450202', 'Coupling,  2" GALV CC X CC' UNION ALL
SELECT '3455', 'W6450F0F', 'Coupling,   1/2" GALV CC X CC' UNION ALL
SELECT '3456', 'W6450H0H', 'Coupling,   3/4" GALV CC X CC' UNION ALL
SELECT '3457', 'W6451C1C', 'Coupling,  1 1/4" GALV CC X CC' UNION ALL
SELECT '3458', 'W6451F1F', 'Coupling,  1 1/2" GALV CC X CC' UNION ALL
SELECT '3459', 'W6460201', 'Tee,  2" X 1" CC X FTT' UNION ALL
SELECT '3460', 'W6610101', 'Coupling,  1" Slip X FIP PVC' UNION ALL
SELECT '3461', 'W6610202', 'Coupling,  2" Slip X FIP PVC' UNION ALL
SELECT '3462', 'W6610404', 'Coupling,  4" Slip X FIP PVC' UNION ALL
SELECT '3463', 'W6610H0H', 'Coupling,   3/4" Slip X FIP PVC' UNION ALL
SELECT '3464', 'W6611C1C', 'Coupling,  1 1/4" Slip X FIP PVC' UNION ALL
SELECT '3465', 'W6611F1F', 'Coupling,  1 1/2" Slip X FIP PVC' UNION ALL
SELECT '3466', 'W6630101', 'Bend,  1" PVC 90°' UNION ALL
SELECT '3467', 'W6630101', 'Bend,  1" PVC 90°' UNION ALL
SELECT '3468', 'W6630202', 'Bend,  2" PVC 90°' UNION ALL
SELECT '3469', 'W6630303', 'Bend,  3" PVC 90°' UNION ALL
SELECT '3470', 'W6630404', 'Bend,  4" PVC 90°' UNION ALL
SELECT '3471', 'W6630606', 'Bend,  6" PVC 90°' UNION ALL
SELECT '3472', 'W6630H0H', 'Bend,   3/4" X  3/4" PVC 90°' UNION ALL
SELECT '3473', 'W6631C1C', 'Bend,  1 1/4" PVC 90°' UNION ALL
SELECT '3474', 'W6631F1F', 'Bend,  1 1/2" PVC 90°' UNION ALL
SELECT '3475', 'W6640101', 'Tee,  1" PVC' UNION ALL
SELECT '3476', 'W6640202', 'Tee,  2" PVC' UNION ALL
SELECT '3477', 'W6640303', 'Tee,  3" PVC' UNION ALL
SELECT '3478', 'W6640404', 'Tee,  4" PVC' UNION ALL
SELECT '3479', 'W6640606', 'Tee,  6" X 6" PVC' UNION ALL
SELECT '3480', 'W6640H0H', 'Tee,   3/4" X  3/4" PVC' UNION ALL
SELECT '3481', 'W6641C1C', 'Tee,  1 1/4" PVC' UNION ALL
SELECT '3482', 'W6641F1F', 'Tee,  1 1/2" PVC' UNION ALL
SELECT '3483', 'W6642F2F', 'Tee,  2 1/2" PVC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 56.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3484', 'W6650101', 'Coupling,  1" PVC Comp' UNION ALL
SELECT '3485', 'W6650202', 'Coupling,  2" PVC Comp' UNION ALL
SELECT '3486', 'W6650H0H', 'Coupling,   3/4" PVC Comp' UNION ALL
SELECT '3487', 'W6650H0H', 'Coupling,   3/4" PVC Comp' UNION ALL
SELECT '3488', 'W6651F1F', 'Coupling,  1 1/2" PVC Comp' UNION ALL
SELECT '3489', 'W6660202', 'Bend,  2" PVC SJ 90°' UNION ALL
SELECT '3490', 'W6660202', 'Bend,  2" PVC SJ 90°' UNION ALL
SELECT '3491', 'W6660303', 'Bend,  3" PVC SJ 90°' UNION ALL
SELECT '3492', 'W6660404', 'Bend,  4" PVC SJ 90°' UNION ALL
SELECT '3493', 'W6662F2F', 'Bend,  2 1/2" X 2 1/2" PVC SJ 90°' UNION ALL
SELECT '3494', 'W6672F2F', 'Bend,  2 1/2" X 2 1/2" PVC SJ 45°' UNION ALL
SELECT '3495', 'W6680404', 'Bend,  4" PVC SJ 22 1/2°' UNION ALL
SELECT '3496', 'W672010H', 'Bushing, 1" X 3/4" PVC Taper X Taper' UNION ALL
SELECT '3497', 'W6770202', 'Adapter,  2" SJ X MIP PVC' UNION ALL
SELECT '3498', 'W6781616', 'Adapter, 16" LJB X MJS RJ' UNION ALL
SELECT '3499', 'W6782020', 'Adapter, 20" LJB X MJS RJ ECP' UNION ALL
SELECT '3500', 'W6782424', 'Adapter, 24" LJB X MJS RJ ECP' UNION ALL
SELECT '3501', 'W6791616', 'Adapter, 16" LJS X MJS RJ ECP' UNION ALL
SELECT '3502', 'W6792020', 'Adapter, 20" LJS X MJS RJ ECP' UNION ALL
SELECT '3503', 'W6792424', 'Adapter, 24" LJS X MJS RJ ECP' UNION ALL
SELECT '3504', 'W6894242', 'Adapter, 42" LJB X FLG ECP' UNION ALL
SELECT '3505', 'W6903636', 'Adapter, 36" LJB X MJB ECP' UNION ALL
SELECT '3506', 'W6904242', 'Adapter, 42" LJB X MJB ECP' UNION ALL
SELECT '3507', 'W6904848', 'Adapter, 48" LJB X MJB ECP' UNION ALL
SELECT '3508', 'W6914848', 'Adapter, 48" LJB X MJS ECP' UNION ALL
SELECT '3509', 'W6954848', 'Adapter, 48" LJS X MJS ECP' UNION ALL
SELECT '3510', 'W6974848', 'Adapter, 48" LJB X LJS ECP Full Bevel' UNION ALL
SELECT '3511', 'W6986012', 'Tee, 60" X 12" LJS X MJS' UNION ALL
SELECT '3512', 'W6991616', 'Tee, 16" LJB X MJS' UNION ALL
SELECT '3513', 'W6992020', 'Tee, 20" LJB X MJS' UNION ALL
SELECT '3514', 'W6992424', 'Tee, 24" LJB X MJS' UNION ALL
SELECT '3515', 'W6993030', 'Tee, 30" LJB X MJS' UNION ALL
SELECT '3516', 'W6993636', 'Tee, 36" LJB X MJS' UNION ALL
SELECT '3517', 'W6994242', 'Tee, 42" LJB X MJS' UNION ALL
SELECT '3518', 'W6995454', 'Tee, 54" LJB X MJS' UNION ALL
SELECT '3519', 'W6996012', 'Tee, 60" X 12" LJB X MJS' UNION ALL
SELECT '3520', 'W7000202', 'Tee,  2" X 2" MJ' UNION ALL
SELECT '3521', 'W7000204', 'Tee,  2" X 4" MJ' UNION ALL
SELECT '3522', 'W7000208', 'Tee,  2" X 8" MJ' UNION ALL
SELECT '3523', 'W7000303', 'Tee,  3" X 3" MJ' UNION ALL
SELECT '3524', 'W7000402', 'Tee,  4" X 2" MJ' UNION ALL
SELECT '3525', 'W7000403', 'Tee,  4" X 3" MJ' UNION ALL
SELECT '3526', 'W7000602', 'Tee,  6" X 2" MJ' UNION ALL
SELECT '3527', 'W7000603', 'Tee,  6" X 3" MJ' UNION ALL
SELECT '3528', 'W7000802', 'Tee,  8" X 2" MJ' UNION ALL
SELECT '3529', 'W7000803', 'Tee,  8" X 3" MJ' UNION ALL
SELECT '3530', 'W7001004', 'Tee, 10" X  4" MJ' UNION ALL
SELECT '3531', 'W7001412', 'Tee, 14" X 12" MJ' UNION ALL
SELECT '3532', 'W7001414', 'Tee, 14" X 14" MJ' UNION ALL
SELECT '3533', 'W7001806', 'Tee, 18" X  6" MJ'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 57.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3534', 'W7001816', 'Tee, 18" X 16" MJ' UNION ALL
SELECT '3535', 'W7001818', 'Tee, 18" X 18" MJ' UNION ALL
SELECT '3536', 'W7002004', 'Tee, 20" X  4" MJ' UNION ALL
SELECT '3537', 'W7002010', 'Tee, 20" X 10" MJ' UNION ALL
SELECT '3538', 'W7002024', 'Tee, 20" X 24" MJ' UNION ALL
SELECT '3539', 'W7002404', 'Tee, 24" X  4" MJ' UNION ALL
SELECT '3540', 'W7002410', 'Tee, 24" X 10" MJ' UNION ALL
SELECT '3541', 'W7002418', 'Tee, 24" X 18" MJ' UNION ALL
SELECT '3542', 'W7002420', 'Tee, 24" X 20" MJ' UNION ALL
SELECT '3543', 'W7003004', 'Tee, 30" X  4" MJ' UNION ALL
SELECT '3544', 'W7003008', 'Tee, 30" X  8" MJ' UNION ALL
SELECT '3545', 'W7003010', 'Tee, 30" X 10" MJ' UNION ALL
SELECT '3546', 'W7003012', 'Tee, 30" X 12" MJ' UNION ALL
SELECT '3547', 'W7003016', 'Tee, 30" X 16" MJ' UNION ALL
SELECT '3548', 'W7003020', 'Tee, 30" X 20" MJ' UNION ALL
SELECT '3549', 'W7003024', 'Tee, 30" X 24" MJ' UNION ALL
SELECT '3550', 'W7003030', 'Tee, 30" X 30" MJ' UNION ALL
SELECT '3551', 'W7003612', 'Tee, 36" X 12" MJ' UNION ALL
SELECT '3552', 'W7003620', 'Tee, 36" X 20" MJ' UNION ALL
SELECT '3553', 'W7003624', 'Tee, 36" X 24" MJ' UNION ALL
SELECT '3554', 'W7004824', 'Tee, 48" X 24" MJ' UNION ALL
SELECT '3555', 'W7004836', 'Tee, 48" X 36" MJ' UNION ALL
SELECT '3556', 'W7010606', 'Tee,  6" X 6" FLG' UNION ALL
SELECT '3557', 'W7010804', 'Tee,  8" X 4" FLG' UNION ALL
SELECT '3558', 'W7010806', 'Tee,  8" X 6" FLG' UNION ALL
SELECT '3559', 'W7011006', 'Tee, 10" X  6" FLG' UNION ALL
SELECT '3560', 'W7011008', 'Tee, 10" X  8" FLG' UNION ALL
SELECT '3561', 'W7011010', 'Tee, 10" X 10" FLG' UNION ALL
SELECT '3562', 'W7011204', 'Tee, 12" X  4" FLG' UNION ALL
SELECT '3563', 'W7011206', 'Tee, 12" X  6" FLG' UNION ALL
SELECT '3564', 'W7011208', 'Tee, 12" X  8" FLG' UNION ALL
SELECT '3565', 'W7021206', 'Tee, 12" X  6" RJ' UNION ALL
SELECT '3566', 'W7021816', 'Tee, 18" X 16" RJ' UNION ALL
SELECT '3567', 'W7021818', 'Tee, 18" X 18" RJ' UNION ALL
SELECT '3568', 'W7022006', 'Tee, 20" X  6" RJ' UNION ALL
SELECT '3569', 'W7022008', 'Tee, 20" X  8" RJ' UNION ALL
SELECT '3570', 'W7022016', 'Tee, 20" X 16" RJ' UNION ALL
SELECT '3571', 'W7022424', 'Tee, 24" X 24" RJ' UNION ALL
SELECT '3572', 'W7023624', 'Tee, 36" X 24" RJ' UNION ALL
SELECT '3573', 'W7023636', 'Tee, 36" X 36" RJ' UNION ALL
SELECT '3574', 'W7024220', 'Tee, 42" X 20" RJ' UNION ALL
SELECT '3575', 'W7024224', 'Tee, 42" X 24" RJ' UNION ALL
SELECT '3576', 'W7024236', 'Tee, 42" X 36" RJ' UNION ALL
SELECT '3577', 'W7030302', 'Tee,  3" X 2" Tap MJ X FIP' UNION ALL
SELECT '3578', 'W7030402', 'Tee,  4" X 2" Tap MJ X FIP' UNION ALL
SELECT '3579', 'W7030402', 'Tee,  4" X 2" Tap MJ X FIP' UNION ALL
SELECT '3580', 'W7030602', 'Tee,  6" X 2" Tap MJ X FIP' UNION ALL
SELECT '3581', 'W7030802', 'Tee,  8" X 2" Tap MJ X FIP' UNION ALL
SELECT '3582', 'W7030802', 'Tee,  8" X 2" Tap MJ X FIP' UNION ALL
SELECT '3583', 'W7031002', 'Tee, 10" X  2" Tap MJ X FIP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 58.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3584', 'W7031202', 'Tee, 12" X  2" Tap MJ X FIP' UNION ALL
SELECT '3585', 'W7041004', 'Tee, 10" X  4" Anchor MJ' UNION ALL
SELECT '3586', 'W7041008', 'Tee, 10" X  8" Anchor MJ' UNION ALL
SELECT '3587', 'W7041208', 'Tee, 12" X  8" Anchor MJ' UNION ALL
SELECT '3588', 'W7041406', 'Tee, 14" X  6" Anchor MJ' UNION ALL
SELECT '3589', 'W7041608', 'Tee, 16" X  8" Anchor MJ' UNION ALL
SELECT '3590', 'W7042006', 'Tee, 20" X  6" Anchor MJ' UNION ALL
SELECT '3591', 'W7042424', 'Tee, 24" X 24" Anchor MJ' UNION ALL
SELECT '3592', 'W7050404', 'Tee,  4" X 4" MJ X FLG' UNION ALL
SELECT '3593', 'W7050604', 'Tee,  6" X 4" MJ X FLG' UNION ALL
SELECT '3594', 'W7050606', 'Tee,  6" X 6" MJ X FLG' UNION ALL
SELECT '3595', 'W7050804', 'Tee,  8" X 4" MJ X FLG' UNION ALL
SELECT '3596', 'W7050806', 'Tee,  8" X 6" MJ X FLG' UNION ALL
SELECT '3597', 'W7050808', 'Tee,  8" X 8" MJ X FLG' UNION ALL
SELECT '3598', 'W7051006', 'Tee, 10" X  6" MJ X FLG' UNION ALL
SELECT '3599', 'W7051008', 'Tee, 10" X  8" MJ X FLG' UNION ALL
SELECT '3600', 'W7051010', 'Tee, 10" X 10" MJ X FLG' UNION ALL
SELECT '3601', 'W7051204', 'Tee, 12" X  4" MJ X FLG' UNION ALL
SELECT '3602', 'W7051206', 'Tee, 12" X  6" MJ X FLG' UNION ALL
SELECT '3603', 'W7051208', 'Tee, 12" X  8" MJ X FLG' UNION ALL
SELECT '3604', 'W7051212', 'Tee, 12" X 12" MJ X FLG' UNION ALL
SELECT '3605', 'W7060404', 'Tee,  4" X 4" SJ' UNION ALL
SELECT '3606', 'W7060603', 'Tee,  6" X 3" SJ' UNION ALL
SELECT '3607', 'W7060604', 'Tee,  6" X 4" SJ' UNION ALL
SELECT '3608', 'W7060606', 'Tee,  6" X 6" SJ' UNION ALL
SELECT '3609', 'W7060803', 'Tee,  8" X 3" SJ' UNION ALL
SELECT '3610', 'W7060804', 'Tee,  8" X 4" SJ' UNION ALL
SELECT '3611', 'W7060806', 'Tee,  8" X 6" SJ' UNION ALL
SELECT '3612', 'W7060808', 'Tee,  8" X 8" SJ' UNION ALL
SELECT '3613', 'W7061004', 'Tee, 10" X  4" SJ' UNION ALL
SELECT '3614', 'W7061006', 'Tee, 10" X  6" SJ' UNION ALL
SELECT '3615', 'W7061008', 'Tee, 10" X  8" SJ' UNION ALL
SELECT '3616', 'W7061010', 'Tee, 10" X 10" SJ' UNION ALL
SELECT '3617', 'W7061204', 'Tee, 12" X  4" SJ' UNION ALL
SELECT '3618', 'W7061206', 'Tee, 12" X  6" SJ' UNION ALL
SELECT '3619', 'W7061208', 'Tee, 12" X  8" SJ' UNION ALL
SELECT '3620', 'W7061212', 'Tee, 12" X 12" SJ' UNION ALL
SELECT '3621', 'W7061604', 'Tee, 16" X  4" SJ' UNION ALL
SELECT '3622', 'W7061606', 'Tee, 16" X  6" SJ' UNION ALL
SELECT '3623', 'W7061608', 'Tee, 16" X  8" SJ' UNION ALL
SELECT '3624', 'W7061612', 'Tee, 16" X 12" SJ' UNION ALL
SELECT '3625', 'W7061616', 'Tee, 16" X 16" SJ' UNION ALL
SELECT '3626', 'W7062006', 'Tee, 20" X  6" SJ' UNION ALL
SELECT '3627', 'W7062008', 'Tee, 20" X  8" SJ' UNION ALL
SELECT '3628', 'W7062012', 'Tee, 20" X 12" SJ' UNION ALL
SELECT '3629', 'W7062016', 'Tee, 20" X 16" SJ' UNION ALL
SELECT '3630', 'W7062020', 'Tee, 20" X 20" SJ' UNION ALL
SELECT '3631', 'W7062406', 'Tee, 24" X  6" SJ' UNION ALL
SELECT '3632', 'W7063006', 'Tee, 30" X  6" SJ' UNION ALL
SELECT '3633', 'W7063008', 'Tee, 30" X  8" SJ'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 59.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3634', 'W7063012', 'Tee, 30" X 12" SJ' UNION ALL
SELECT '3635', 'W7063024', 'Tee, 30" X 24" SJ' UNION ALL
SELECT '3636', 'W7063030', 'Tee, 30" X 30" SJ' UNION ALL
SELECT '3637', 'W7063608', 'Tee, 36" X  8" SJ' UNION ALL
SELECT '3638', 'W7063620', 'Tee, 36" X 20" SJ' UNION ALL
SELECT '3639', 'W7063630', 'Tee, 36" X 30" SJ' UNION ALL
SELECT '3640', 'W7064236', 'Tee, 42" X 36" SJ' UNION ALL
SELECT '3641', 'W7070606', 'WYE,  6" MJ' UNION ALL
SELECT '3642', 'W7070808', 'WYE,  8" MJ' UNION ALL
SELECT '3643', 'W7071212', 'WYE, 12" MJ' UNION ALL
SELECT '3644', 'W7100403', 'Cross,  4" X 3" MJ' UNION ALL
SELECT '3645', 'W7100404', 'Cross,  4" X 4" MJ' UNION ALL
SELECT '3646', 'W7100804', 'Cross,  8" X 4" MJ' UNION ALL
SELECT '3647', 'W7101004', 'Cross, 10" X  4" MJ' UNION ALL
SELECT '3648', 'W7101008', 'Cross, 10" X  8" MJ' UNION ALL
SELECT '3649', 'W7101210', 'Cross, 12" X 10" MJ' UNION ALL
SELECT '3650', 'W7101406', 'Cross, 14" X  6" MJ' UNION ALL
SELECT '3651', 'W7101412', 'Cross, 14" X 12" MJ' UNION ALL
SELECT '3652', 'W7101606', 'Cross, 16" X  6" MJ' UNION ALL
SELECT '3653', 'W7101616', 'Cross, 16" X 16" MJ' UNION ALL
SELECT '3654', 'W7102008', 'Cross, 20" X  8" MJ' UNION ALL
SELECT '3655', 'W7102012', 'Cross, 20" X 12" MJ' UNION ALL
SELECT '3656', 'W7102016', 'Cross, 20" X 16" MJ' UNION ALL
SELECT '3657', 'W7102020', 'Cross, 20" X 20" MJ' UNION ALL
SELECT '3658', 'W7102408', 'Cross, 24" X  8" MJ' UNION ALL
SELECT '3659', 'W7102424', 'Cross, 24" X 24" MJ' UNION ALL
SELECT '3660', 'W7110404', 'Cross,  4" X 4" FLG' UNION ALL
SELECT '3661', 'W7110606', 'Cross,  6" X 6" FLG' UNION ALL
SELECT '3662', 'W7110806', 'Cross,  8" X 6" FLG' UNION ALL
SELECT '3663', 'W7110808', 'Cross,  8" X 8" FLG' UNION ALL
SELECT '3664', 'W7111212', 'Cross, 12" X 12" FLG' UNION ALL
SELECT '3665', 'W7120404', 'Cross,  4" X 4" SJ' UNION ALL
SELECT '3666', 'W7120604', 'Cross,  6" X 4" SJ' UNION ALL
SELECT '3667', 'W7120606', 'Cross,  6" X 6" SJ' UNION ALL
SELECT '3668', 'W7120806', 'Cross,  8" X 6" SJ' UNION ALL
SELECT '3669', 'W7120808', 'Cross,  8" X 8" SJ' UNION ALL
SELECT '3670', 'W7121006', 'Cross, 10" X  6" SJ' UNION ALL
SELECT '3671', 'W7121008', 'Cross, 10" X  8" SJ' UNION ALL
SELECT '3672', 'W7121010', 'Cross, 10" X 10" SJ' UNION ALL
SELECT '3673', 'W7121206', 'Cross, 12" X  6" SJ' UNION ALL
SELECT '3674', 'W7121208', 'Cross, 12" X  8" SJ' UNION ALL
SELECT '3675', 'W7121212', 'Cross, 12" X 12" SJ' UNION ALL
SELECT '3676', 'W7121606', 'Cross, 16" X  6" SJ' UNION ALL
SELECT '3677', 'W7121608', 'Cross, 16" X  8" SJ' UNION ALL
SELECT '3678', 'W7121612', 'Cross, 16" X 12" SJ' UNION ALL
SELECT '3679', 'W7121616', 'Cross, 16" X 16" SJ' UNION ALL
SELECT '3680', 'W7122016', 'Cross, 20" X 16" SJ' UNION ALL
SELECT '3681', 'W7130303', 'Cross,  3" GALV' UNION ALL
SELECT '3682', 'W7131C1C', 'Cross,  1 1/4" GALV' UNION ALL
SELECT '3683', 'W7160806', 'Tee,  8" X 6" SJ X PE'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 60.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3684', 'W7161206', 'Tee, 12" X  6" SJ X PE' UNION ALL
SELECT '3685', 'W7181206', 'Sleeve, 12" X  6" Tap FAB MJ Outlet PE pipe' UNION ALL
SELECT '3686', 'W7190201', 'Saddle,  2" X 1" Fasttap PL' UNION ALL
SELECT '3687', 'W719020H', 'Saddle,  2" X  3/4" Fasttap PL' UNION ALL
SELECT '3688', 'W7190301', 'Saddle,  3" X 1" Fasttap PL' UNION ALL
SELECT '3689', 'W7190401', 'Saddle,  4" X 1" Fasttap PL' UNION ALL
SELECT '3690', 'W7190601', 'Saddle,  6" X 1" Fasttap PL' UNION ALL
SELECT '3691', 'W7190606', 'Saddle,  6" X 6" Fasttap PL' UNION ALL
SELECT '3692', 'W7200403', 'Sleeve,  4" X 3" Tap MJ CI/DI' UNION ALL
SELECT '3693', 'W7201202', 'Sleeve, 12" X  2" Tap MJ CI/DI' UNION ALL
SELECT '3694', 'W7201408', 'Sleeve, 14" X  8" Tap MJ CI/DI' UNION ALL
SELECT '3695', 'W7201412', 'Sleeve, 14" X 12" Tap MJ CI/DI' UNION ALL
SELECT '3696', 'W7201414', 'Sleeve, 14" X 14" Tap MJ CI/DI' UNION ALL
SELECT '3697', 'W7201816', 'Sleeve, 18" X 16" Tap MJ CI/DI' UNION ALL
SELECT '3698', 'W7202010', 'Sleeve, 20" X 10" Tap MJ CI/DI' UNION ALL
SELECT '3699', 'W7202020', 'Sleeve, 20" X 20" Tap MJ CI/DI' UNION ALL
SELECT '3700', 'W7202424', 'Sleeve, 24" X 24" Tap MJ CI/DI' UNION ALL
SELECT '3701', 'W7203016', 'Sleeve, 30" X 16" Tap MJ CI/DI' UNION ALL
SELECT '3702', 'W7203024', 'Sleeve, 30" X 24" Tap MJ CI/DI' UNION ALL
SELECT '3703', 'W7205420', 'Sleeve, 54" X 20" Tap MJ CI/DI' UNION ALL
SELECT '3704', 'W7221004', 'Sleeve, 10" X  4" Tap MJ AC' UNION ALL
SELECT '3705', 'W7221210', 'Sleeve, 12" X 10" Tap MJ AC' UNION ALL
SELECT '3706', 'W7221604', 'Sleeve, 16" X  4" Tap MJ AC' UNION ALL
SELECT '3707', 'W7221606', 'Sleeve, 16" X  6" Tap MJ AC' UNION ALL
SELECT '3708', 'W7221608', 'Sleeve, 16" X  8" Tap MJ AC' UNION ALL
SELECT '3709', 'W7222004', 'Sleeve, 20" X  4" Tap MJ AC' UNION ALL
SELECT '3710', 'W7222006', 'Sleeve, 20" X  6" Tap MJ AC' UNION ALL
SELECT '3711', 'W7222008', 'Sleeve, 20" X  8" Tap MJ AC' UNION ALL
SELECT '3712', 'W7222406', 'Sleeve, 24" X  6" Tap MJ AC' UNION ALL
SELECT '3713', 'W7222408', 'Sleeve, 24" X  8" Tap MJ AC' UNION ALL
SELECT '3714', 'W7223006', 'Sleeve, 30" X  6" Tap MJ AC' UNION ALL
SELECT '3715', 'W7223008', 'Sleeve, 30" X  8" Tap MJ AC' UNION ALL
SELECT '3716', 'W7223606', 'Sleeve, 36" X  6" Tap MJ AC' UNION ALL
SELECT '3717', 'W7223608', 'Sleeve, 36" X  8" Tap MJ AC' UNION ALL
SELECT '3718', 'W7223612', 'Sleeve, 36" X 12" Tap MJ AC' UNION ALL
SELECT '3719', 'W7224206', 'Sleeve, 42" X  6" Tap MJ AC' UNION ALL
SELECT '3720', 'W7224208', 'Sleeve, 42" X  8" Tap MJ AC' UNION ALL
SELECT '3721', 'W7224212', 'Sleeve, 42" X 12" Tap MJ AC' UNION ALL
SELECT '3722', 'W7230404', 'Sleeve,  4" X 4" Tap FLG FAB' UNION ALL
SELECT '3723', 'W7230604', 'Sleeve,  6" X 4" Tap FLG FAB' UNION ALL
SELECT '3724', 'W7230606', 'Sleeve,  6" X 6" Tap FLG FAB' UNION ALL
SELECT '3725', 'W7230804', 'Sleeve,  8" X 4" Tap FLG FAB' UNION ALL
SELECT '3726', 'W7230808', 'Sleeve,  8" X 8" Tap FLG FAB' UNION ALL
SELECT '3727', 'W7231004', 'Sleeve, 10" X  4" Tap FLG FAB' UNION ALL
SELECT '3728', 'W7231008', 'Sleeve, 10" X  8" Tap FLG FAB' UNION ALL
SELECT '3729', 'W7231210', 'Sleeve, 12" X 10" Tap FLG FAB' UNION ALL
SELECT '3730', 'W7231212', 'Sleeve, 12" X 12" Tap FLG FAB' UNION ALL
SELECT '3731', 'W7231408', 'Sleeve, 14" X  8" Tap FLG FAB' UNION ALL
SELECT '3732', 'W7231808', 'Sleeve, 18" X  8" Tap FLG FAB' UNION ALL
SELECT '3733', 'W7232004', 'Sleeve, 20" X  4" Tap FLG FAB'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 61.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3734', 'W7232012', 'Sleeve, 20" X 12" Tap FLG FAB' UNION ALL
SELECT '3735', 'W7232016', 'Sleeve, 20" X 16" Tap FLG FAB' UNION ALL
SELECT '3736', 'W7232404', 'Sleeve, 20" X 20" Tap FLG FAB' UNION ALL
SELECT '3737', 'W7232408', 'Sleeve, 24" X  8" Tap FLG FAB' UNION ALL
SELECT '3738', 'W7232412', 'Sleeve, 24" X 12" Tap FLG FAB' UNION ALL
SELECT '3739', 'W7233002', 'Sleeve, 30" X  2" Tap FLG FAB' UNION ALL
SELECT '3740', 'W7233006', 'Sleeve, 30" X  6" Tap FLG FAB' UNION ALL
SELECT '3741', 'W7233012', 'Sleeve, 30" X 12" Tap FLG FAB' UNION ALL
SELECT '3742', 'W7233606', 'Sleeve, 36" X  6" Tap FLG FAB' UNION ALL
SELECT '3743', 'W7233608', 'Sleeve, 36" X  8" Tap FLG FAB' UNION ALL
SELECT '3744', 'W7233612', 'Sleeve, 36" X 12" Tap FLG FAB' UNION ALL
SELECT '3745', 'W7234812', 'Sleeve, 48" X 12" Tap FLG FAB' UNION ALL
SELECT '3746', 'W7240404', 'Sleeve,  4" X 4" Tap FAB SS CI/DI' UNION ALL
SELECT '3747', 'W7240604', 'Sleeve,  6" X 4" Tap FAB SS CI/DI' UNION ALL
SELECT '3748', 'W7240606', 'Sleeve,  6" X 6" Tap FAB SS CI/DI' UNION ALL
SELECT '3749', 'W7240804', 'Sleeve,  8" X 4" Tap FAB SS CI/DI' UNION ALL
SELECT '3750', 'W7240806', 'Sleeve,  8" X 6" Tap FAB SS CI/DI' UNION ALL
SELECT '3751', 'W7241004', 'Sleeve, 10" X  4" Tap FAB SS CI/DI' UNION ALL
SELECT '3752', 'W7241008', 'Sleeve, 10" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3753', 'W7241010', 'Sleeve, 10" X 10" Tap FAB SS CI/DI' UNION ALL
SELECT '3754', 'W7241204', 'Sleeve, 12" X  4" Tap FAB SS CI/DI' UNION ALL
SELECT '3755', 'W7241206', 'Sleeve, 12" X  6" Tap FAB SS CI/DI' UNION ALL
SELECT '3756', 'W7241208', 'Sleeve, 12" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3757', 'W7241210', 'Sleeve, 12" X 10" Tap FAB SS CI/DI' UNION ALL
SELECT '3758', 'W7241212', 'Sleeve, 12" X 12" Tap FAB SS CI/DI' UNION ALL
SELECT '3759', 'W7241406', 'Sleeve, 14" X 16" Tap FAB SS CI/DI' UNION ALL
SELECT '3760', 'W7241408', 'Sleeve, 14" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3761', 'W7241414', 'Sleeve, 14" X 14" Tap FAB SS CI/DI' UNION ALL
SELECT '3762', 'W7241604', 'Sleeve, 16" X  4" Tap FAB SS CI/DI' UNION ALL
SELECT '3763', 'W7241606', 'Sleeve, 16" X  6" Tap FAB SS CI/DI' UNION ALL
SELECT '3764', 'W7241608', 'Sleeve, 16" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3765', 'W7241610', 'Sleeve, 16" X 10" Tap FAB SS CI/DI' UNION ALL
SELECT '3766', 'W7241612', 'Sleeve, 16" X 12" Tap FAB SS CI/DI' UNION ALL
SELECT '3767', 'W7241616', 'Sleeve, 16" X 16" Tap FAB SS CI/DI' UNION ALL
SELECT '3768', 'W7242004', 'Sleeve, 20" X  4" Tap FAB SS CI/DI' UNION ALL
SELECT '3769', 'W7242006', 'Sleeve, 20" X  6" Tap FAB SS CI/DI' UNION ALL
SELECT '3770', 'W7242008', 'Sleeve, 20" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3771', 'W7242012', 'Sleeve, 20" X 12" Tap FAB SS CI/DI' UNION ALL
SELECT '3772', 'W7242404', 'Sleeve, 24" X  4" Tap FAB SS CI/DI' UNION ALL
SELECT '3773', 'W7242406', 'Sleeve, 24" X  6" Tap FAB SS CI/DI' UNION ALL
SELECT '3774', 'W7242408', 'Sleeve, 24" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3775', 'W7242412', 'Sleeve, 24" X 12" Tap FAB SS CI/DI' UNION ALL
SELECT '3776', 'W7242416', 'Sleeve, 24" X 16" Tap FAB SS CI/DI' UNION ALL
SELECT '3777', 'W7243608', 'Sleeve, 36" X  8" Tap FAB SS CI/DI' UNION ALL
SELECT '3778', 'W7250404', 'Sleeve,  4" X 4" Tap FAB SS AC' UNION ALL
SELECT '3779', 'W7250604', 'Sleeve,  6" X 4" Tap FAB SS AC' UNION ALL
SELECT '3780', 'W7250606', 'Sleeve,  6" X 6" Tap FAB SS AC' UNION ALL
SELECT '3781', 'W7250804', 'Sleeve,  8" X 4" Tap FAB SS AC' UNION ALL
SELECT '3782', 'W7250806', 'Sleeve,  8" X 6" Tap FAB SS AC' UNION ALL
SELECT '3783', 'W7250808', 'Sleeve,  8" X 8" Tap FAB SS AC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 62.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3784', 'W7251004', 'Sleeve, 10" X  4" Tap FAB SS AC' UNION ALL
SELECT '3785', 'W7251006', 'Sleeve, 10" X  6" Tap FAB SS AC' UNION ALL
SELECT '3786', 'W7251008', 'Sleeve, 10" X  8" Tap FAB SS AC' UNION ALL
SELECT '3787', 'W7251010', 'Sleeve, 10" X 10" Tap FAB SS AC' UNION ALL
SELECT '3788', 'W7251204', 'Sleeve, 12" X  4" Tap FAB SS AC' UNION ALL
SELECT '3789', 'W7251206', 'Sleeve, 12" X  6" Tap FAB SS AC' UNION ALL
SELECT '3790', 'W7251208', 'Sleeve, 12" X  8" Tap FAB SS AC' UNION ALL
SELECT '3791', 'W7251210', 'Sleeve, 12" X 10" Tap FAB SS AC' UNION ALL
SELECT '3792', 'W7251212', 'Sleeve, 12" X 12" Tap FAB SS AC' UNION ALL
SELECT '3793', 'W7251408', 'Sleeve, 14" X  8" Tap FAB SS AC' UNION ALL
SELECT '3794', 'W7251412', 'Sleeve, 14" X 12" Tap FAB SS AC' UNION ALL
SELECT '3795', 'W7251604', 'Sleeve, 16" X  4" Tap FAB SS AC' UNION ALL
SELECT '3796', 'W7251606', 'Sleeve, 16" X  6" Tap FAB SS AC' UNION ALL
SELECT '3797', 'W7251608', 'Sleeve, 16" X  8" Tap FAB SS AC' UNION ALL
SELECT '3798', 'W7251612', 'Sleeve, 16" X 12" Tap FAB SS AC' UNION ALL
SELECT '3799', 'W7251616', 'Sleeve, 16" X 16" Tap FAB SS AC' UNION ALL
SELECT '3800', 'W7252012', 'Sleeve, 20" X 12" Tap FAB SS AC' UNION ALL
SELECT '3801', 'W7252408', 'Sleeve, 24" X  8" Tap FAB SS AC' UNION ALL
SELECT '3802', 'W7252412', 'Sleeve, 24" X 12" Tap FAB SS AC' UNION ALL
SELECT '3803', 'W7253012', 'Sleeve, 30" X 12" Tap FAB SS AC' UNION ALL
SELECT '3804', 'W7253608', 'Sleeve, 36" X  8" Tap FAB SS AC' UNION ALL
SELECT '3805', 'W7260402', 'Sleeve,  4" X 2" Tap FAB SS PVC' UNION ALL
SELECT '3806', 'W7260404', 'Sleeve,  4" X 4" Tap FAB SS PVC' UNION ALL
SELECT '3807', 'W7260504', 'Sleeve,  5" X 4" Tap FAB SS PVC' UNION ALL
SELECT '3808', 'W7260602', 'Sleeve,  6" X 2" Tap FAB SS PVC' UNION ALL
SELECT '3809', 'W7260604', 'Sleeve,  6" X 4" Tap FAB SS PVC' UNION ALL
SELECT '3810', 'W7260606', 'Sleeve,  6" X 6" Tap FAB SS PVC' UNION ALL
SELECT '3811', 'W7260802', 'Sleeve,  8" X 2" Tap FAB SS PVC' UNION ALL
SELECT '3812', 'W7260804', 'Sleeve,  8" X 4" Tap FAB SS PVC' UNION ALL
SELECT '3813', 'W7260806', 'Sleeve,  8" X 6" Tap FAB SS PVC' UNION ALL
SELECT '3814', 'W7260808', 'Sleeve,  8" X 8" Tap FAB SS PVC' UNION ALL
SELECT '3815', 'W7261004', 'Sleeve, 10" X  4" Tap FAB SS PVC' UNION ALL
SELECT '3816', 'W7261006', 'Sleeve, 10" X  6" Tap FAB SS PVC' UNION ALL
SELECT '3817', 'W7261008', 'Sleeve, 10" X  8" Tap FAB SS PVC' UNION ALL
SELECT '3818', 'W7261010', 'Sleeve, 10" X 10" Tap FAB SS PVC' UNION ALL
SELECT '3819', 'W7261202', 'Sleeve, 12" X  2" Tap FAB SS PVC' UNION ALL
SELECT '3820', 'W7261204', 'Sleeve, 12" X  4" Tap FAB SS PVC' UNION ALL
SELECT '3821', 'W7261206', 'Sleeve, 12" X  6" Tap FAB SS PVC' UNION ALL
SELECT '3822', 'W7261208', 'Sleeve, 12" X  8" Tap FAB SS PVC' UNION ALL
SELECT '3823', 'W7261210', 'Sleeve, 12" X 10" Tap FAB SS PVC' UNION ALL
SELECT '3824', 'W7261212', 'Sleeve, 12" X 12" Tap FAB SS PVC' UNION ALL
SELECT '3825', 'W7261408', 'Sleeve, 14" X  8" Tap FAB SS PVC' UNION ALL
SELECT '3826', 'W7261414', 'Sleeve, 14" Tap FAB SS PVC' UNION ALL
SELECT '3827', 'W7270606', 'Sleeve,  6" X 6" Tap FAB SS IPS' UNION ALL
SELECT '3828', 'W7280606', 'Sleeve,  6" X 6" Tap AC' UNION ALL
SELECT '3829', 'W7280804', 'Sleeve,  8" X 4" Tap AC' UNION ALL
SELECT '3830', 'W7280806', 'Sleeve,  8" X 6" Tap AC' UNION ALL
SELECT '3831', 'W7280808', 'Sleeve,  8" X 8" Tap AC' UNION ALL
SELECT '3832', 'W7281204', 'Sleeve, 12" X  4" Tap AC' UNION ALL
SELECT '3833', 'W7281206', 'Sleeve, 12" X  6" Tap AC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 63.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3834', 'W7281208', 'Sleeve, 12" X  8" Tap AC' UNION ALL
SELECT '3835', 'W7281604', 'Sleeve, 16" X  4" Tap FAB AC' UNION ALL
SELECT '3836', 'W7281606', 'Sleeve, 16" X  6" Tap AC' UNION ALL
SELECT '3837', 'W7281608', 'Sleeve, 16" X  8" Tap AC' UNION ALL
SELECT '3838', 'W7281612', 'Sleeve, 16" X 12" Tap AC' UNION ALL
SELECT '3839', 'W7282004', 'Sleeve, 20" X  4" Tap AC' UNION ALL
SELECT '3840', 'W7282008', 'Sleeve, 20" X  8" Tap AC' UNION ALL
SELECT '3841', 'W7290604', 'Sleeve,  6" X 4" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3842', 'W7290606', 'Sleeve,  6" X 6" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3843', 'W7290804', 'Sleeve,  8" X 4" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3844', 'W7290806', 'Sleeve,  8" X 6" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3845', 'W7290808', 'Sleeve,  8" X 8" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3846', 'W7291004', 'Sleeve, 10" X  4" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3847', 'W7291006', 'Sleeve, 10" X  6" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3848', 'W7291008', 'Sleeve, 10" X  8" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3849', 'W7291010', 'Sleeve, 10" X 10" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3850', 'W7291204', 'Sleeve, 12" X  4" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3851', 'W7291206', 'Sleeve, 12" X  6" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3852', 'W7291208', 'Sleeve, 12" X  8" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3853', 'W7291210', 'Sleeve, 12" X 10" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3854', 'W7291212', 'Sleeve, 12" X 12" Tap FAB MJ Outlet CI/DI' UNION ALL
SELECT '3855', 'W7300401', 'Saddle,  4" X 1" CI' UNION ALL
SELECT '3856', 'W7300402', 'Saddle,  4" X 2" CI' UNION ALL
SELECT '3857', 'W731020H', 'Saddle,  2" X  3/4" PVC' UNION ALL
SELECT '3858', 'W7310301', 'Saddle,  3" X 1" PVC' UNION ALL
SELECT '3859', 'W731030H', 'Saddle,  3" X  3/4" PVC' UNION ALL
SELECT '3860', 'W7310401', 'Saddle,  4" X 1" PVC' UNION ALL
SELECT '3861', 'W731040H', 'Saddle,  4" X  3/4" PVC' UNION ALL
SELECT '3862', 'W731050H', 'Saddle,  5" X 3/4" PVC' UNION ALL
SELECT '3863', 'W7310601', 'Saddle,  6" X 1" PVC' UNION ALL
SELECT '3864', 'W731060H', 'Saddle,  6" X  3/4" PVC' UNION ALL
SELECT '3865', 'W7310801', 'Saddle,  8" X 1" PVC' UNION ALL
SELECT '3866', 'W731080H', 'Saddle,  8" X  3/4" PVC' UNION ALL
SELECT '3867', 'W7312F01', 'Saddle,  2 1/2" X 1" PVC' UNION ALL
SELECT '3868', 'W7312F0H', 'Saddle,  2 1/2" X  3/4" PVC' UNION ALL
SELECT '3869', 'W732020H', 'Saddle,  2" X  3/4" SRV DBL STP CI' UNION ALL
SELECT '3870', 'W7320401', 'Saddle,  4" X 1" SRV DBL STP CI' UNION ALL
SELECT '3871', 'W7320402', 'Saddle,  4" X 2" SRV DBL STP CI' UNION ALL
SELECT '3872', 'W7320601', 'Saddle,  6" X 1" SRV DBL STP CI' UNION ALL
SELECT '3873', 'W7320602', 'Saddle,  6" X 2" SRV DBL STP CI' UNION ALL
SELECT '3874', 'W7320801', 'Saddle,  8" X 1" SRV DBL STP CI' UNION ALL
SELECT '3875', 'W7320802', 'Saddle,  8" X 2" SRV DBL STP CI' UNION ALL
SELECT '3876', 'W7321002', 'Saddle, 10" X 2" SRV DBL STP CI' UNION ALL
SELECT '3877', 'W7321202', 'Saddle, 12" X 2" SRV DBL STP CI' UNION ALL
SELECT '3878', 'W7330602', 'Saddle,  6" X 2" SRV DBL STP STL' UNION ALL
SELECT '3879', 'W7330802', 'Saddle,  8" X 2" SRV DBL STP STL' UNION ALL
SELECT '3880', 'W7331202', 'Saddle, 12" X 2" SRV DBL STP STL' UNION ALL
SELECT '3881', 'W7350201', 'Saddle,  2" X 1" SRV PVC IPS' UNION ALL
SELECT '3882', 'W735020H', 'Saddle,  2" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3883', 'W7350301', 'Saddle,  3" X 1" SRV PVC IPS'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 64.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3884', 'W735030H', 'Saddle,  3" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3885', 'W7350401', 'Saddle,  4" X 1" SRV PVC IPS' UNION ALL
SELECT '3886', 'W735040H', 'Saddle,  4" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3887', 'W7350601', 'Saddle,  6" X 1" SRV PVC IPS' UNION ALL
SELECT '3888', 'W735060H', 'Saddle,  6" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3889', 'W7350801', 'Saddle,  8" X 1" SRV PVC IPS' UNION ALL
SELECT '3890', 'W7350802', 'Saddle,  8" X 2" SRV PVC IPS' UNION ALL
SELECT '3891', 'W735080H', 'Saddle,  8" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3892', 'W7351001', 'Saddle, 10" X 1" SRV PVC IPS' UNION ALL
SELECT '3893', 'W735100H', 'Saddle, 10" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3894', 'W7351201', 'Saddle, 12" X 1" SRV PVC IPS' UNION ALL
SELECT '3895', 'W735120H', 'Saddle, 12" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3896', 'W7352C01', 'Saddle,  2 1/4" X 1" SRV PVC IPS' UNION ALL
SELECT '3897', 'W7352C0H', 'Saddle,  2 1/4" X  3/4" SRV PVC IPS' UNION ALL
SELECT '3898', 'W736020H', 'Saddle,  2" X  3/4" SRV PVC' UNION ALL
SELECT '3899', 'W736020H', 'Saddle,  3" X  3/4" SRV PVC' UNION ALL
SELECT '3900', 'W7360401', 'Sleeve,  4" X 1" SRV PVC' UNION ALL
SELECT '3901', 'W7360402', 'Sleeve,  4" X 2" SRV PVC' UNION ALL
SELECT '3902', 'W736040H', 'Saddle,  4" X  3/4" SRV PVC' UNION ALL
SELECT '3903', 'W7360601', 'Saddle,  6" X 1" SRV PVC' UNION ALL
SELECT '3904', 'W7360602', 'Saddle,  6" X 2" SRV PVC' UNION ALL
SELECT '3905', 'W736060H', 'Saddle,  6" X  3/4" SRV PVC' UNION ALL
SELECT '3906', 'W7360801', 'Saddle,  8" X 1" SRV PVC' UNION ALL
SELECT '3907', 'W7360802', 'Saddle,  8" X 2" SRV PVC' UNION ALL
SELECT '3908', 'W736080H', 'Saddle,  8" X  3/4" SRV PVC' UNION ALL
SELECT '3909', 'W7361001', 'Saddle, 10" X 1" SRV PVC' UNION ALL
SELECT '3910', 'W7361002', 'Saddle, 10" X 2" SRV PVC' UNION ALL
SELECT '3911', 'W736100H', 'Saddle, 10" X  3/4" SRV PVC' UNION ALL
SELECT '3912', 'W7361201', 'Saddle, 12" X 1" SRV PVC' UNION ALL
SELECT '3913', 'W7361202', 'Saddle, 12" X 2" SRV PVC' UNION ALL
SELECT '3914', 'W736120H', 'Saddle, 12" X  3/4" SRV PVC' UNION ALL
SELECT '3915', 'W7361401', 'Sleeve, 14" X  1" SRV PVC' UNION ALL
SELECT '3916', 'W736140H', 'Sleeve, 14" X   3/4" SRV PVC' UNION ALL
SELECT '3917', 'W7361601', 'Sleeve, 16" X  1" SRV PVC' UNION ALL
SELECT '3918', 'W7361801', 'Sleeve, 18" X  1" SRV PVC' UNION ALL
SELECT '3919', 'W736180H', 'Sleeve, 18" X   3/4" SRV PVC' UNION ALL
SELECT '3920', 'W7361F0H', 'Saddle,  1 1/2" X  3/4" SRV PVC' UNION ALL
SELECT '3921', 'W7362001', 'Sleeve, 20" X  1" SRV PVC' UNION ALL
SELECT '3922', 'W736200H', 'Sleeve, 20" X   3/4" SRV PVC' UNION ALL
SELECT '3923', 'W7362401', 'Sleeve, 24" X  1" SRV PVC' UNION ALL
SELECT '3924', 'W736240H', 'Sleeve, 24" X   3/4" SRV PVC' UNION ALL
SELECT '3925', 'W7370402', 'Sleeve,  4" X 2" SRV AC' UNION ALL
SELECT '3926', 'W7381602', 'Saddle, Tap 16" X  2" SRV PSC' UNION ALL
SELECT '3927', 'W738161F', 'Saddle, Tap 16" X  1 1/2" SRV PSC' UNION ALL
SELECT '3928', 'W7382002', 'Saddle, Tap 20" X  2" SRV PSC' UNION ALL
SELECT '3929', 'W738201F', 'Saddle, Tap 20" X  1 1/2" SRV PSC' UNION ALL
SELECT '3930', 'W7382416', 'Saddle, Tap 24" X 16" SRV PSC' UNION ALL
SELECT '3931', 'W7383020', 'Saddle, Tap 30" X 20" SRV PSC' UNION ALL
SELECT '3932', 'W7383612', 'Saddle, Tap 36" X 12" SRV PSC' UNION ALL
SELECT '3933', 'W7383616', 'Saddle, Tap 36" X 16" SRV PSC'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 65.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3934', 'W7384801', 'Saddle, Tap 48" X  1" SRV PSC' UNION ALL
SELECT '3935', 'W7384802', 'Saddle, Tap 48" X  2" SRV PSC' UNION ALL
SELECT '3936', 'W7384808', 'Saddle, Tap 48" X  8" SRV PSC' UNION ALL
SELECT '3937', 'W7386002', 'Saddle, Tap 60" X  2" SRV PSC' UNION ALL
SELECT '3938', 'W7386006', 'Saddle, Tap 60" X  6" SRV PSC' UNION ALL
SELECT '3939', 'W7386012', 'Saddle, Tap 60" X 12" SRV PSC' UNION ALL
SELECT '3940', 'W7386024', 'Saddle, Tap 60" X 24" SRV PSC' UNION ALL
SELECT '3941', 'W7390604', 'Saddle, Tap  6" X 4" DI/CI' UNION ALL
SELECT '3942', 'W7391206', 'Saddle, Tap 12" X 6" DI/CI' UNION ALL
SELECT '3943', 'W7391404', 'Saddle, Tap 14" X 4" DI/CI' UNION ALL
SELECT '3944', 'W7391406', 'Saddle, Tap 14" X 6" DI/CI' UNION ALL
SELECT '3945', 'W7391408', 'Saddle, Tap 14" X 8" DI/CI' UNION ALL
SELECT '3946', 'W7391604', 'Saddle, Tap 16" X  4" DI/CI' UNION ALL
SELECT '3947', 'W7391606', 'Saddle, Tap 16" X  6" DI/CI' UNION ALL
SELECT '3948', 'W7391608', 'Saddle, Tap 16" X  8" DI/CI' UNION ALL
SELECT '3949', 'W7391612', 'Saddle, Tap 16" X 12" DI/CI' UNION ALL
SELECT '3950', 'W7391806', 'Saddle, Tap 18" X  6" DI/CI' UNION ALL
SELECT '3951', 'W7392004', 'Saddle, Tap 20" X  4" DI/CI' UNION ALL
SELECT '3952', 'W7392006', 'Saddle, Tap 20" X  6" DI/CI' UNION ALL
SELECT '3953', 'W7392008', 'Saddle, Tap 20" X  8" DI/CI' UNION ALL
SELECT '3954', 'W7392012', 'Saddle, Tap 20" X 12" DI/CI' UNION ALL
SELECT '3955', 'W739202F', 'Saddle, Tap 20" X  2 1/2" DI/CI' UNION ALL
SELECT '3956', 'W7392404', 'Saddle, Tap 24" X  4" DI/CI' UNION ALL
SELECT '3957', 'W7392406', 'Saddle, Tap 24" X  6" DI/CI' UNION ALL
SELECT '3958', 'W7392408', 'Saddle, Tap 24" X  8" DI/CI' UNION ALL
SELECT '3959', 'W7392412', 'Saddle, Tap 24" X 12" DI/CI' UNION ALL
SELECT '3960', 'W7393004', 'Saddle, Tap 30" X  4" DI/CI' UNION ALL
SELECT '3961', 'W7393006', 'Saddle, Tap 30" X  6" DI/CI' UNION ALL
SELECT '3962', 'W7393008', 'Saddle, Tap 30" X  8" DI/CI' UNION ALL
SELECT '3963', 'W7393012', 'Saddle, Tap 30" X 12" DI/CI' UNION ALL
SELECT '3964', 'W739302F', 'Saddle, Tap 30" X  2 1/2" DI/CI' UNION ALL
SELECT '3965', 'W7393606', 'Saddle, Tap 36" X  6" DI/CI' UNION ALL
SELECT '3966', 'W7393608', 'Saddle, Tap 36" X  8" DI/CI' UNION ALL
SELECT '3967', 'W7393612', 'Saddle, Tap 36" X 12" DI/CI' UNION ALL
SELECT '3968', 'W7394206', 'Saddle, Tap 42" X  6" DI/CI' UNION ALL
SELECT '3969', 'W7394208', 'Saddle, Tap 42" X  8" DI/CI' UNION ALL
SELECT '3970', 'W7394212', 'Saddle, Tap 42" X 12" DI/CI' UNION ALL
SELECT '3971', 'W7394808', 'Saddle, Tap 48" X  8" DI/CI' UNION ALL
SELECT '3972', 'W7400401', 'Saddle, Self Tapping  4" X 1" PVC' UNION ALL
SELECT '3973', 'W7400601', 'Saddle, Self Tapping  6" X 1" PVC' UNION ALL
SELECT '3974', 'W7400801', 'Saddle, Self Tapping  8" X 1" PVC' UNION ALL
SELECT '3975', 'W7401001', 'Saddle, Self Tapping 10" X 1" PVC' UNION ALL
SELECT '3976', 'W744010H', 'Saddle, Tap  1" X 3/4" SRV' UNION ALL
SELECT '3977', 'W7440202', 'Saddle, Tap  2" SRV' UNION ALL
SELECT '3978', 'W744021C', 'Saddle, Tap  2" X 1 1/4" SRV' UNION ALL
SELECT '3979', 'W744021F', 'Saddle, Tap  2" X 1 1/2" SRV' UNION ALL
SELECT '3980', 'W7440302', 'Saddle, Tap  3" X 2" SRV' UNION ALL
SELECT '3981', 'W7440303', 'Saddle, Tap  3" X 3" SRV' UNION ALL
SELECT '3982', 'W744030H', 'Saddle, Tap  3" X  3/4" SRV' UNION ALL
SELECT '3983', 'W744031C', 'Saddle, Tap  3" X 1 1/4" SRV'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 66.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '3984', 'W744041C', 'Saddle, Tap  4" X 1 1/4" SRV' UNION ALL
SELECT '3985', 'W744041F', 'Saddle, Tap  4" X 1 1/2" SRV' UNION ALL
SELECT '3986', 'W744050H', 'Saddle, Tap  5" X  3/4" SRV' UNION ALL
SELECT '3987', 'W7440603', 'Saddle, Tap  6" X 3" SRV' UNION ALL
SELECT '3988', 'W744061C', 'Saddle, Tap  6" X 1 1/4" SRV' UNION ALL
SELECT '3989', 'W7440803', 'Saddle, Tap  8" X 3" SRV' UNION ALL
SELECT '3990', 'W744081C', 'Saddle, Tap  8" X 1 1/4" SRV' UNION ALL
SELECT '3991', 'W744100H', 'Saddle, Tap 10" X   3/4" SRV' UNION ALL
SELECT '3992', 'W744101F', 'Saddle, Tap 10" X  1 1/2" SRV' UNION ALL
SELECT '3993', 'W744121C', 'Saddle, Tap 12" X 1 1/4" SRV' UNION ALL
SELECT '3994', 'W744121F', 'Saddle, Tap 12" X 1 1/2" SRV' UNION ALL
SELECT '3995', 'W7441401', 'Saddle, Tap 14" X 1" SRV' UNION ALL
SELECT '3996', 'W7441402', 'Saddle, Tap 14" X 2" SRV' UNION ALL
SELECT '3997', 'W744140H', 'Saddle, Tap 14" X  3/4" SRV' UNION ALL
SELECT '3998', 'W744141F', 'Saddle, Tap 14" X 1 1/2" SRV' UNION ALL
SELECT '3999', 'W744160H', 'Saddle, Tap 16" X   3/4" SRV' UNION ALL
SELECT '4000', 'W744161F', 'Saddle, Tap 16" X  1 1/2" SRV' UNION ALL
SELECT '4001', 'W7441C01', 'Saddle, Tap  1 1/4" X 1" SRV' UNION ALL
SELECT '4002', 'W7441C0H', 'Saddle, Tap  1 1/4" X  3/4" SRV' UNION ALL
SELECT '4003', 'W7441F01', 'Saddle, Tap  1 1/2" X 1" SRV' UNION ALL
SELECT '4004', 'W7441F0H', 'Saddle, Tap  1 1/2" X  3/4" SRV' UNION ALL
SELECT '4005', 'W7442002', 'Saddle, Tap 20" X  2" SRV' UNION ALL
SELECT '4006', 'W744200H', 'Saddle, Tap 20" X   3/4" SRV' UNION ALL
SELECT '4007', 'W7442402', 'Saddle, Tap 24" X  2" SRV' UNION ALL
SELECT '4008', 'W744240H', 'Saddle, Tap 24" X   3/4" SRV' UNION ALL
SELECT '4009', 'W7442F01', 'Saddle, Tap  2 1/2" X 1" SRV' UNION ALL
SELECT '4010', 'W7442F0H', 'Saddle, Tap  2 1/2" X  3/4" SRV' UNION ALL
SELECT '4011', 'W7443002', 'Saddle, Tap 30" X  2" SRV' UNION ALL
SELECT '4012', 'W7443F01', 'Saddle, Tap  3 1/2" X 1" SRV' UNION ALL
SELECT '4013', 'W7443F0H', 'Saddle, Tap  3 1/2" X  3/4" SRV' UNION ALL
SELECT '4014', 'W7444801', 'Saddle, Tap 48" X  1" SRV' UNION ALL
SELECT '4015', 'W745020H', 'Saddle,  2" X  3/4" SRV BCB' UNION ALL
SELECT '4016', 'W7452F0H', 'Saddle,  2 1/2" X  3/4" SRV BCB' UNION ALL
SELECT '4017', 'W7470402', 'Saddle,  4" X 2" SSSC AC' UNION ALL
SELECT '4018', 'W7471202', 'Saddle, 12" X 2" SSSC AC' UNION ALL
SELECT '4019', 'W7471602', 'Saddle, 16" X 2" SSSC AC' UNION ALL
SELECT '4020', 'W7480401', 'Saddle,  4" X 1" SSSC ST' UNION ALL
SELECT '4021', 'W7480601', 'Saddle,  6" X 1" SSSC ST' UNION ALL
SELECT '4022', 'W7480602', 'Saddle,  6" X 2" SSSC ST' UNION ALL
SELECT '4023', 'W748061F', 'Saddle,  6" X 1 1/2" SSSC ST' UNION ALL
SELECT '4024', 'W7480801', 'Saddle,  8" X 1" SSSC ST' UNION ALL
SELECT '4025', 'W7480802', 'Saddle,  8" X 2" SSSC ST' UNION ALL
SELECT '4026', 'W748081F', 'Saddle,  8" X 1 1/2" SSSC ST' UNION ALL
SELECT '4027', 'W7481001', 'Saddle, 10" X 1" SSSC ST' UNION ALL
SELECT '4028', 'W7481002', 'Saddle, 10" X 2" SSSC ST' UNION ALL
SELECT '4029', 'W748101F', 'Saddle, 10" X 1 1/2" SSSC ST' UNION ALL
SELECT '4030', 'W7481201', 'Saddle, 12" X 1" SSSC ST' UNION ALL
SELECT '4031', 'W7481202', 'Saddle, 12" X 2" SSSC ST' UNION ALL
SELECT '4032', 'W748121F', 'Saddle, 12" X 1 1/2" SSSC ST' UNION ALL
SELECT '4033', 'W7610303', 'Cap,  3" End Flexbolt'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 67.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4034', 'W7610404', 'Cap,  4" End Flexbolt' UNION ALL
SELECT '4035', 'W7610606', 'Cap,  6" End Flexbolt' UNION ALL
SELECT '4036', 'W7610808', 'Cap,  8" End Flexbolt' UNION ALL
SELECT '4037', 'W7611010', 'Cap, 10" End Flexbolt' UNION ALL
SELECT '4038', 'W7611212', 'Cap, 12" End Flexbolt' UNION ALL
SELECT '4039', 'W7611616', 'Cap, 16" End Flexbolt' UNION ALL
SELECT '4040', 'W7612020', 'Cap, 20" End Flexbolt' UNION ALL
SELECT '4041', 'W7612424', 'Cap, 24" End Flexbolt' UNION ALL
SELECT '4042', 'W7670101', 'Cap,  1" GALV' UNION ALL
SELECT '4043', 'W7670202', 'Cap,  2" GALV' UNION ALL
SELECT '4044', 'W7671C1C', 'Cap,  1 1/4" GALV' UNION ALL
SELECT '4045', 'W7671F1F', 'Cap,  1 1/2" GALV' UNION ALL
SELECT '4046', 'W7681212', 'Cap, 12" SJ' UNION ALL
SELECT '4047', 'W7681616', 'Cap, 16" SJ' UNION ALL
SELECT '4048', 'W7690101', 'Cap,  1" BR' UNION ALL
SELECT '4049', 'W7690202', 'Cap,  2" BR' UNION ALL
SELECT '4050', 'W7690H0H', 'Cap,   3/4" BR' UNION ALL
SELECT '4051', 'W7691C1C', 'Cap,  1 1/4" BR' UNION ALL
SELECT '4052', 'W7691F1F', 'Cap,  1 1/2" BR' UNION ALL
SELECT '4053', 'W7700201', 'Cap,  2" X 1" MJ' UNION ALL
SELECT '4054', 'W7700202', 'Cap,  2" MJ' UNION ALL
SELECT '4055', 'W770020H', 'Cap,  2" X  3/4" MJ' UNION ALL
SELECT '4056', 'W7700302', 'Cap,  3" X 2" MJ' UNION ALL
SELECT '4057', 'W7701002', 'Cap, 10" X 2" MJ' UNION ALL
SELECT '4058', 'W7701010', 'Cap, 10" MJ' UNION ALL
SELECT '4059', 'W7701402', 'Cap, 14" X 2" MJ' UNION ALL
SELECT '4060', 'W7701414', 'Cap, 14" MJ' UNION ALL
SELECT '4061', 'W7701802', 'Cap, 18" X 2" MJ' UNION ALL
SELECT '4062', 'W770182F', 'Cap, 18" X 2 1/2" MJ' UNION ALL
SELECT '4063', 'W770182H', 'Cap, 18" X 2 3/4" MJ' UNION ALL
SELECT '4064', 'W7702002', 'Cap, 20" X 2" MJ' UNION ALL
SELECT '4065', 'W7702402', 'Cap, 24" X 2" MJ' UNION ALL
SELECT '4066', 'W7702404', 'Cap, 24" X 4" MJ' UNION ALL
SELECT '4067', 'W7702406', 'Cap, 24" X 6" MJ' UNION ALL
SELECT '4068', 'W7702424', 'Cap, 24" MJ' UNION ALL
SELECT '4069', 'W7702C01', 'Cap,  2 1/4" X 1" MJ' UNION ALL
SELECT '4070', 'W7702C2C', 'Cap,  2 1/4" MJ' UNION ALL
SELECT '4071', 'W7703002', 'Cap, 30" X 2" MJ' UNION ALL
SELECT '4072', 'W7703030', 'Cap, 30" MJ' UNION ALL
SELECT '4073', 'W7703602', 'Cap, 36" X 2" MJ' UNION ALL
SELECT '4074', 'W7703636', 'Cap, 36" MJ' UNION ALL
SELECT '4075', 'W7704242', 'Cap, 42" MJ' UNION ALL
SELECT '4076', 'W7704848', 'Cap, 48" MJ' UNION ALL
SELECT '4077', 'W7710808', 'Cap,  8" RJ' UNION ALL
SELECT '4078', 'W7711616', 'Cap, 16" RJ' UNION ALL
SELECT '4079', 'W7712002', 'Cap, 20" X 2" RJ Tap' UNION ALL
SELECT '4080', 'W7712020', 'Cap, 20" RJ' UNION ALL
SELECT '4081', 'W7712424', 'Cap, 24" RJ' UNION ALL
SELECT '4082', 'W7713002', 'Cap, 30" X 2" RJ Tap' UNION ALL
SELECT '4083', 'W7713030', 'Cap, 30" RJ'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 68.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4084', 'W7713636', 'Cap, 36" RJ' UNION ALL
SELECT '4085', 'W7714242', 'Cap, 42" RJ' UNION ALL
SELECT '4086', 'W7714848', 'Cap, 48" RJ' UNION ALL
SELECT '4087', 'W7715454', 'Cap, 54" RJ' UNION ALL
SELECT '4088', 'W7730412', 'Coupling,  4" X 12" Anchor DI MJ' UNION ALL
SELECT '4089', 'W7730601', 'Coupling,  6" X  1" Anchor DI MJ' UNION ALL
SELECT '4090', 'W7730602', 'Coupling,  6" X  2" Anchor DI MJ' UNION ALL
SELECT '4091', 'W7730613', 'Coupling,  6" X 13" Anchor DI MJ' UNION ALL
SELECT '4092', 'W7730813', 'Coupling,  8" X 13" Anchor DI MJ' UNION ALL
SELECT '4093', 'W7731212', 'Coupling, 12" X 12" Anchor DI MJ' UNION ALL
SELECT '4094', 'W7731212', 'Coupling, 12" X 12" Anchor DI MJ' UNION ALL
SELECT '4095', 'W7740606', 'Offset, Hydrant  6" X  6" Anchor' UNION ALL
SELECT '4096', 'W7740613', 'Offset, Hydrant  6" X 13" Anchor' UNION ALL
SELECT '4097', 'W7740636', 'Coupling,  6" X 36" Anchor W/OFFSET' UNION ALL
SELECT '4098', 'W7740812', 'Coupling,  8" X 12" Anchor W/OFFSET' UNION ALL
SELECT '4099', 'W775020H', 'Plug,  2" DI MJ W/3/4" Tap' UNION ALL
SELECT '4100', 'W7750302', 'Plug,  3" DI MJ  W/2" Tap' UNION ALL
SELECT '4101', 'W7750303', 'Plug,  3" DI MJ' UNION ALL
SELECT '4102', 'W7750804', 'Plug,  8" DI MJ W/4" Tap' UNION ALL
SELECT '4103', 'W7750806', 'Plug,  8" DI MJ W/6" Tap' UNION ALL
SELECT '4104', 'W7751002', 'Plug, 10" DI MJ W/2" Tap' UNION ALL
SELECT '4105', 'W7751010', 'Plug, 10" DI MJ' UNION ALL
SELECT '4106', 'W7751402', 'Plug, 14" DI MJ W/2" Tap' UNION ALL
SELECT '4107', 'W7751414', 'Plug, 14" DI MJ' UNION ALL
SELECT '4108', 'W7751818', 'Plug, 18" DI MJ' UNION ALL
SELECT '4109', 'W7752002', 'Plug, 20" DI MJ W/2" Tap' UNION ALL
SELECT '4110', 'W7752006', 'Plug, 20" DI MJ W/6" Tap' UNION ALL
SELECT '4111', 'W7752402', 'Plug, 24" DI MJ W/2" Tap' UNION ALL
SELECT '4112', 'W7752406', 'Plug, 24" DI MJ W/6" Tap' UNION ALL
SELECT '4113', 'W7752424', 'Plug, 24" DI MJ' UNION ALL
SELECT '4114', 'W7752C2C', 'Plug,  2 1/4" DI MJ' UNION ALL
SELECT '4115', 'W7753002', 'Plug, 30" DI MJ W/2" Tap' UNION ALL
SELECT '4116', 'W7753030', 'Plug, 30" DI MJ' UNION ALL
SELECT '4117', 'W7753602', 'Plug, 36" DI MJ W/2" Tap' UNION ALL
SELECT '4118', 'W7753636', 'Plug, 36" DI MJ' UNION ALL
SELECT '4119', 'W7754202', 'Plug, 42" DI MJ W/2" Tap' UNION ALL
SELECT '4120', 'W7754242', 'Plug, 42" DI MJ' UNION ALL
SELECT '4121', 'W7754848', 'Plug, 48" DI MJ' UNION ALL
SELECT '4122', 'W7771001', 'Plug, 10" X 1" DI SJ W/1" TAP' UNION ALL
SELECT '4123', 'W7771002', 'Plug, 10" X 2" DI SJ W/2" TAP' UNION ALL
SELECT '4124', 'W7771010', 'Plug, 10" DI SJ' UNION ALL
SELECT '4125', 'W7772002', 'Plug, 20" DI SJ W/2" Tap' UNION ALL
SELECT '4126', 'W7772424', 'Plug, 24" DI SJ' UNION ALL
SELECT '4127', 'W7773030', 'Plug, 30" DI SJ' UNION ALL
SELECT '4128', 'W7774242', 'Plug, 42" DI SJ' UNION ALL
SELECT '4129', 'W7780202', 'Cap,  2" PVC SJ' UNION ALL
SELECT '4130', 'W7780303', 'Cap,  3" PVC SJ' UNION ALL
SELECT '4131', 'W7780404', 'Cap,  4" PVC SJ' UNION ALL
SELECT '4132', 'W7780606', 'Cap,  6" PVC SJ' UNION ALL
SELECT '4133', 'W7801616', 'Plug, 16" DI RJ'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 69.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4134', 'W7802002', 'Plug, 20" DI RJ W/2" Tap' UNION ALL
SELECT '4135', 'W7802402', 'Plug, 24" X 2" DI RJ W/2" Tap' UNION ALL
SELECT '4136', 'W7803030', 'Plug, 30" DI RJ' UNION ALL
SELECT '4137', 'W7803636', 'Plug, 36" DI RJ' UNION ALL
SELECT '4138', 'W7804242', 'Plug, 42" DI RJ' UNION ALL
SELECT '4139', 'W7804848', 'Plug, 48" DI RJ' UNION ALL
SELECT '4140', 'W7805454', 'Plug, 54" DI RJ' UNION ALL
SELECT '4141', 'W7823030', 'Adapter, 30" LJB X MJS RJ LCP' UNION ALL
SELECT '4142', 'W7823636', 'Adapter, 36" LJB X MJS RJ LCP' UNION ALL
SELECT '4143', 'W7824242', 'Adapter, 42" LJB X MJS RJ LCP' UNION ALL
SELECT '4144', 'W7825454', 'Adapter, 54" LJB X MJS RJ LCP' UNION ALL
SELECT '4145', 'W7840404', 'Adapter,  4" FLG X FLEX Bolted DI' UNION ALL
SELECT '4146', 'W7840606', 'Adapter,  6" FLG X FLEX Bolted DI' UNION ALL
SELECT '4147', 'W7840808', 'Adapter,  8" FLG X FLEX Bolted DI' UNION ALL
SELECT '4148', 'W7841010', 'Adapter, 10" FLG X FLEX Bolted DI' UNION ALL
SELECT '4149', 'W7841212', 'Adapter, 12" FLG X FLEX Bolted DI' UNION ALL
SELECT '4150', 'W7860303', 'Adapter,  3" FLG X PE' UNION ALL
SELECT '4151', 'W7860404', 'Adapter,  4" FLG X PE' UNION ALL
SELECT '4152', 'W7860606', 'Adapter,  6" FLG X PE' UNION ALL
SELECT '4153', 'W7860618', 'Adapter,  6" X 18" FLG X PE' UNION ALL
SELECT '4154', 'W7860618', 'Adapter,  6" X 18" FLG X PE' UNION ALL
SELECT '4155', 'W7860624', 'Adapter,  6" X 24" FLG X PE' UNION ALL
SELECT '4156', 'W7860808', 'Adapter,  8" FLG X PE' UNION ALL
SELECT '4157', 'W7861010', 'Adapter, 10" FLG X PE' UNION ALL
SELECT '4158', 'W7861212', 'Adapter, 12" FLG X PE' UNION ALL
SELECT '4159', 'W7861224', 'Adapter, 12" X 24" FLG X PE' UNION ALL
SELECT '4160', 'W7861636', 'Adapter, 16" X 36" FLG X PE' UNION ALL
SELECT '4161', 'W7861648', 'Adapter, 16" X 48" FLG X PE' UNION ALL
SELECT '4162', 'W7861660', 'Adapter, 16" X 60" FLG X PE' UNION ALL
SELECT '4163', 'W7862020', 'Adapter, 20" FLG X PE' UNION ALL
SELECT '4164', 'W7862424', 'Adapter, 24" FLG X PE' UNION ALL
SELECT '4165', 'W7863030', 'Adapter, 30" FLG X PE' UNION ALL
SELECT '4166', 'W7863636', 'Adapter, 36" FLG X PE' UNION ALL
SELECT '4167', 'W7864848', 'Adapter, 48" FLG X PE' UNION ALL
SELECT '4168', 'W7866060', 'Adapter, 60" FLG X PE' UNION ALL
SELECT '4169', 'W7870606', 'Adapter,  6" FLG X MJ' UNION ALL
SELECT '4170', 'W7870808', 'Adapter,  8" FLG X MJ' UNION ALL
SELECT '4171', 'W7871616', 'Adapter, 16" FLG X MJ' UNION ALL
SELECT '4172', 'W7881212', 'Adapter, 12" PE X FLG' UNION ALL
SELECT '4173', 'W7902020', 'Adapter, 20" LJB X MJB LCP' UNION ALL
SELECT '4174', 'W7903636', 'Adapter, 36" LJB X MJB LCP' UNION ALL
SELECT '4175', 'W7904242', 'Adapter, 42" LJB X MJB LCP' UNION ALL
SELECT '4176', 'W7904848', 'Adapter, 48" LJB X MJB LCP' UNION ALL
SELECT '4177', 'W7911616', 'Adapter, 16" LJB X MJS LCP' UNION ALL
SELECT '4178', 'W7912020', 'Adapter, 20" LJB X MJS LCP' UNION ALL
SELECT '4179', 'W7912424', 'Adapter, 24" LJB X MJS LCP' UNION ALL
SELECT '4180', 'W7913030', 'Adapter, 30" LJB X MJS LCP' UNION ALL
SELECT '4181', 'W7913636', 'Adapter, 36" LJB X MJS LCP' UNION ALL
SELECT '4182', 'W7914242', 'Adapter, 42" LJB X MJS LCP' UNION ALL
SELECT '4183', 'W7914848', 'Adapter, 48" LJB X MJS LCP'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 70.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4184', 'W7921616', 'Adapter, 16" LJB X FLG LCP' UNION ALL
SELECT '4185', 'W7924242', 'Adapter, 42" LJB X FLG LCP' UNION ALL
SELECT '4186', 'W7933030', 'Adapter, 30" LJB X LJS LCP 1/2 Bevel' UNION ALL
SELECT '4187', 'W7933636', 'Adapter, 36" LJB X LJS LCP 1/2 Bevel' UNION ALL
SELECT '4188', 'W7934242', 'Adapter, 42" LJB X LJS LCP 1/2 Bevel' UNION ALL
SELECT '4189', 'W7934848', 'Adapter, 48" LJB X LJS LCP 1/2 Bevel' UNION ALL
SELECT '4190', 'W7941616', 'Adapter, 16" LJS X MJB LCP' UNION ALL
SELECT '4191', 'W7942020', 'Adapter, 20" LJS X MJB LCP' UNION ALL
SELECT '4192', 'W7942424', 'Adapter, 24" LJS X MJB LCP' UNION ALL
SELECT '4193', 'W7943030', 'Adapter, 30" LJS X MJB LCP' UNION ALL
SELECT '4194', 'W7943636', 'Adapter, 36" LJS X MJB LCP' UNION ALL
SELECT '4195', 'W7944242', 'Adapter, 42" LJS X MJB LCP' UNION ALL
SELECT '4196', 'W7944848', 'Adapter, 48" LJS X MJB LCP' UNION ALL
SELECT '4197', 'W7951612', 'Adapter, 16" X 12" LJS X MJS LCP' UNION ALL
SELECT '4198', 'W7951616', 'Adapter, 16" LJS X MJS LCP' UNION ALL
SELECT '4199', 'W7952020', 'Adapter, 20" LJS X MJS LCP' UNION ALL
SELECT '4200', 'W7952424', 'Adapter, 24" LJS X MJS LCP' UNION ALL
SELECT '4201', 'W7953030', 'Adapter, 30" LJS X MJS LCP' UNION ALL
SELECT '4202', 'W7953636', 'Adapter, 36" LJS X MJS LCP' UNION ALL
SELECT '4203', 'W7954242', 'Adapter, 42" LJS X MJS LCP' UNION ALL
SELECT '4204', 'W7954848', 'Adapter, 48" LJS X MJS LCP' UNION ALL
SELECT '4205', 'W7973636', 'Adapter, 36" LJB X LJS LCP Full Bevel' UNION ALL
SELECT '4206', 'W7974242', 'Adapter, 42" LJB X LJS LCP Full Bevel' UNION ALL
SELECT '4207', 'W7974848', 'Adapter, 48" LJB X LJS LCP Full Bevel' UNION ALL
SELECT '4208', 'W7984848', 'Adapter, 48" LJB ECP X LJS LCP' UNION ALL
SELECT '4209', 'W8000202', 'Valve,  2" Gate MJ OR' UNION ALL
SELECT '4210', 'W8000303', 'Valve,  3" Gate MJ OR' UNION ALL
SELECT '4211', 'W8001010', 'Valve, 10" Gate MJ OR' UNION ALL
SELECT '4212', 'W8001212', 'Valve, 12" Gate MJ OR' UNION ALL
SELECT '4213', 'W8001414', 'Valve, 14" Gate MJ OR' UNION ALL
SELECT '4214', 'W8001616', 'Valve, 16" Gate MJ OR' UNION ALL
SELECT '4215', 'W8002020', 'Valve, 20" Gate MJ OR' UNION ALL
SELECT '4216', 'W8002424', 'Valve, 24" Gate MJ OR' UNION ALL
SELECT '4217', 'W8002C2C', 'Valve,  2 1/4" Gate MJ OR' UNION ALL
SELECT '4218', 'W8003030', 'Valve, 30" Gate MJ OR' UNION ALL
SELECT '4219', 'W8010202', 'Valve,  2" Gate MJ OL' UNION ALL
SELECT '4220', 'W8010303', 'Valve,  3" Gate MJ OL' UNION ALL
SELECT '4221', 'W8011414', 'Valve, 14" Gate MJ OL' UNION ALL
SELECT '4222', 'W8011616', 'Valve, 16" Gate MJ OL' UNION ALL
SELECT '4223', 'W8011818', 'Valve, 18" Gate MJ OL' UNION ALL
SELECT '4224', 'W8012020', 'Valve, 20" Gate MJ OL' UNION ALL
SELECT '4225', 'W8012424', 'Valve, 24" Gate MJ OL' UNION ALL
SELECT '4226', 'W8040202W', 'Valve,  2" Tap MJ OR Wheel' UNION ALL
SELECT '4227', 'W8041414', 'Valve, 14" Tap MJ OR' UNION ALL
SELECT '4228', 'W8042020', 'Valve, 20" Tap MJ OR' UNION ALL
SELECT '4229', 'W8043030', 'Valve, 30" Tap MJ OR' UNION ALL
SELECT '4230', 'W8050202', 'Valve,  2" Tap MJ OL' UNION ALL
SELECT '4231', 'W8052020', 'Valve, 20" Tap MJ OL' UNION ALL
SELECT '4232', 'W8052424', 'Valve, 24" Tap MJ OL' UNION ALL
SELECT '4233', 'W8061616', 'Valve, 16" Tap MJ HORZ OL'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 71.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4234', 'W8062020', 'Valve, 20" Tap MJ HORZ OL' UNION ALL
SELECT '4235', 'W8062424', 'Valve, 24" Tap MJ HORZ OL' UNION ALL
SELECT '4236', 'W8071616', 'Valve, 16" Tap MJ HORZ OR' UNION ALL
SELECT '4237', 'W8072020', 'Valve, 20" Tap MJ HORZ OR' UNION ALL
SELECT '4238', 'W8072424', 'Valve, 24" Tap MJ HORZ OR' UNION ALL
SELECT '4239', 'W8080404', 'Valve,  4" Gate FLG OR' UNION ALL
SELECT '4240', 'W8080606', 'Valve,  6" Gate FLG OR' UNION ALL
SELECT '4241', 'W8080808', 'Valve,  8" Gate FLG OR' UNION ALL
SELECT '4242', 'W8081010', 'Valve, 10" Gate FLG OR' UNION ALL
SELECT '4243', 'W8081212', 'Valve, 12" Gate FLG OR' UNION ALL
SELECT '4244', 'W8081414', 'Valve, 14" Gate FLG OR' UNION ALL
SELECT '4245', 'W8081616', 'Valve, 16" Gate FLG OR' UNION ALL
SELECT '4246', 'W8082020', 'Valve, 20" Gate FLG OR' UNION ALL
SELECT '4247', 'W8082424', 'Valve, 24" Gate FLG OR' UNION ALL
SELECT '4248', 'W8090202', 'Valve,  2" Gate FLG OL' UNION ALL
SELECT '4249', 'W8090303', 'Valve,  3" Gate FLG OL' UNION ALL
SELECT '4250', 'W8090404', 'Valve,  4" Gate FLG OL' UNION ALL
SELECT '4251', 'W8090404W', 'Valve,  4" Gate FLG OL Wheel' UNION ALL
SELECT '4252', 'W8090808', 'Valve,  8" Gate FLG OL' UNION ALL
SELECT '4253', 'W8091010', 'Valve, 10" Gate FLG OL' UNION ALL
SELECT '4254', 'W8091212', 'Valve, 12" Gate FLG OL' UNION ALL
SELECT '4255', 'W8091414', 'Valve, 14" Gate FLG OL' UNION ALL
SELECT '4256', 'W8091616', 'Valve, 16" Gate FLG OL' UNION ALL
SELECT '4257', 'W8092020', 'Valve, 20" Gate FLG OL' UNION ALL
SELECT '4258', 'W8092424', 'Valve, 24" Gate FLG OL' UNION ALL
SELECT '4259', 'W8120303', 'Valve,  3" Gate FLG OS&Y OR' UNION ALL
SELECT '4260', 'W8140202', 'Valve,  2" Gate FIP OR' UNION ALL
SELECT '4261', 'W8140202W', 'Valve,  2" Gate FIP OR Wheel' UNION ALL
SELECT '4262', 'W8152C2C', 'Valve,  2 1/4" Gate FIP OL' UNION ALL
SELECT '4263', 'W8152F2F', 'Valve,  2 1/2" Gate FIP OL' UNION ALL
SELECT '4264', 'W8180202', 'Valve,  2" Gate SJ OL' UNION ALL
SELECT '4265', 'W8180202W', 'Valve,  2" Gate SJ OL Wheel' UNION ALL
SELECT '4266', 'W8180303', 'Valve,  3" Gate SJ OL' UNION ALL
SELECT '4267', 'W8180404', 'Valve,  4" Gate SJ OL' UNION ALL
SELECT '4268', 'W8180606', 'Valve,  6" Gate SJ OL' UNION ALL
SELECT '4269', 'W8180808', 'Valve,  8" Gate SJ OL' UNION ALL
SELECT '4270', 'W8181010', 'Valve, 10" Gate SJ OL' UNION ALL
SELECT '4271', 'W8181212', 'Valve, 12" Gate SJ OL' UNION ALL
SELECT '4272', 'W8182F2F', 'Valve,  2 1/2" Gate SJ OL' UNION ALL
SELECT '4273', 'W8190404', 'Valve,  4" Gate FLG X MJ OR' UNION ALL
SELECT '4274', 'W8190606', 'Valve,  6" Gate FLG X MJ OR' UNION ALL
SELECT '4275', 'W8190808', 'Valve,  8" Gate FLG X MJ OR' UNION ALL
SELECT '4276', 'W8191010', 'Valve, 10" Gate FLG X MJ OR' UNION ALL
SELECT '4277', 'W8191212', 'Valve, 12" Gate FLG X MJ OR' UNION ALL
SELECT '4278', 'W8192020', 'Valve, 20" Gate FLG X MJ OR' UNION ALL
SELECT '4279', 'W8201212', 'Valve, 12" Gate FLG X MJ OL' UNION ALL
SELECT '4280', 'W8231616', 'Valve, 16" Gate MJ HORZ OR' UNION ALL
SELECT '4281', 'W8232020', 'Valve, 20" Gate MJ HORZ OR' UNION ALL
SELECT '4282', 'W8232424', 'Valve, 24" Gate MJ HORZ OR' UNION ALL
SELECT '4283', 'W8233030', 'Valve, 30" Gate MJ HORZ OR'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 72.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4284', 'W8240404', 'Valve,  4" Gate SJ OR' UNION ALL
SELECT '4285', 'W8240606', 'Valve,  6" Gate SJ OR' UNION ALL
SELECT '4286', 'W8240808', 'Valve,  8" Gate SJ OR' UNION ALL
SELECT '4287', 'W8241212', 'Valve, 12" Gate SJ OR' UNION ALL
SELECT '4288', 'W8260404', 'Valve,  4" Tap SJ OL' UNION ALL
SELECT '4289', 'W8320404', 'Valve,  4" Gate MJ Cut-in OR' UNION ALL
SELECT '4290', 'W8320606', 'Valve,  6" Gate MJ Cut-in OR' UNION ALL
SELECT '4291', 'W8320808', 'Valve,  8" Gate MJ Cut-in OR' UNION ALL
SELECT '4292', 'W8321010', 'Valve, 10" Gate MJ Cut-in OR' UNION ALL
SELECT '4293', 'W8321212', 'Valve, 12" Gate MJ Cut-in OR' UNION ALL
SELECT '4294', 'W8330404', 'Valve,  4" Gate MJ Cut-in OL' UNION ALL
SELECT '4295', 'W8330606', 'Valve,  6" Gate MJ Cut-in OL' UNION ALL
SELECT '4296', 'W8330808', 'Valve,  8" Gate MJ Cut-in OL' UNION ALL
SELECT '4297', 'W8331010', 'Valve, 10" Gate MJ Cut-in OL' UNION ALL
SELECT '4298', 'W8331212', 'Valve, 12" Gate MJ Cut-in OL' UNION ALL
SELECT '4299', 'W8360404', 'Sleeve,  4" Cut-In MJ' UNION ALL
SELECT '4300', 'W8360606', 'Sleeve,  6" Cut-in MJ' UNION ALL
SELECT '4301', 'W8360808', 'Sleeve,  8" Cut-in MJ' UNION ALL
SELECT '4302', 'W8361010', 'Sleeve, 10" Cut-In MJ' UNION ALL
SELECT '4303', 'W8361212', 'Sleeve, 12" Cut-In MJ' UNION ALL
SELECT '4304', 'W8391616', 'Valve, 16" Gate FLG HORZ OR' UNION ALL
SELECT '4305', 'W8431010', 'Valve, 10" Butterfly FLG OR' UNION ALL
SELECT '4306', 'W8431212', 'Valve, 12" Butterfly FLG OR' UNION ALL
SELECT '4307', 'W8431616', 'Valve, 16" Butterfly FLG OR' UNION ALL
SELECT '4308', 'W8432020', 'Valve, 20" Butterfly FLG OR' UNION ALL
SELECT '4309', 'W8432424', 'Valve, 24" Butterfly FLG OR' UNION ALL
SELECT '4310', 'W8433030', 'Valve, 30" Butterfly FLG OR' UNION ALL
SELECT '4311', 'W8433636', 'Valve, 36" Butterfly FLG OR' UNION ALL
SELECT '4312', 'W8434848', 'Valve, 48" Butterfly FLG OR' UNION ALL
SELECT '4313', 'W8434848', 'Valve, 48" Butterfly FLG OR' UNION ALL
SELECT '4314', 'W8440808', 'Valve,  8" Butterfly MJ OR' UNION ALL
SELECT '4315', 'W8441414', 'Valve, 14" Butterfly MJ OR' UNION ALL
SELECT '4316', 'W8444848', 'Valve, 48" Butterfly MJ OR' UNION ALL
SELECT '4317', 'W8450404', 'Valve,  4" Butterfly MJ OL' UNION ALL
SELECT '4318', 'W8450808', 'Valve,  8" Butterfly MJ OL' UNION ALL
SELECT '4319', 'W8451414', 'Valve, 14" Butterfly MJ OL' UNION ALL
SELECT '4320', 'W8454242', 'Valve, 42" Butterfly MJ OL' UNION ALL
SELECT '4321', 'W8454848', 'Valve, 48" Butterfly MJ OL' UNION ALL
SELECT '4322', 'W8491010', 'Valve, 10" Butterfly FLG OL' UNION ALL
SELECT '4323', 'W8491212', 'Valve, 12" Butterfly FLG OL' UNION ALL
SELECT '4324', 'W8491616', 'Valve, 16" Butterfly FLG OL' UNION ALL
SELECT '4325', 'W8491818', 'Valve, 18" Butterfly FLG OL' UNION ALL
SELECT '4326', 'W8492020', 'Valve, 20" Butterfly FLG OL' UNION ALL
SELECT '4327', 'W8492424', 'Valve, 24" Butterfly FLG OL' UNION ALL
SELECT '4328', 'W8501616', 'Valve, 16" Butterfly SJ OL' UNION ALL
SELECT '4329', 'W8502020', 'Valve, 20" Butterfly SJ OL' UNION ALL
SELECT '4330', 'W8502424', 'Valve, 24" Butterfly SJ OL' UNION ALL
SELECT '4331', 'W8503030', 'Valve, 30" Butterfly SJ OL' UNION ALL
SELECT '4332', 'W8503636', 'Valve, 36" Butterfly SJ OL' UNION ALL
SELECT '4333', 'W8570303', 'Valve,  3" Check Detect FLG'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 73.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4334', 'W8620404', 'Valve,  4" Check FLG' UNION ALL
SELECT '4335', 'W8620606', 'Valve,  6" Check FLG' UNION ALL
SELECT '4336', 'W8621212', 'Valve, 12" Check FLG' UNION ALL
SELECT '4337', 'W8640202', 'Valve,  2" Spring Check' UNION ALL
SELECT '4338', 'W8710606', 'Valve,  6" Insertion'
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 74.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[Materials] OFF;


---------------------OPERATING CENTER STOCKED MATERIALS
delete from operatingcenterstockedmaterials;
dbcc checkident([OperatingCenterStockedMaterials], RESEED, 0);

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '1', '17', '756' UNION ALL
SELECT '2', '17', '3' UNION ALL
SELECT '3', '17', '762' UNION ALL
SELECT '4', '17', '770' UNION ALL
SELECT '5', '17', '814' UNION ALL
SELECT '6', '17', '6' UNION ALL
SELECT '7', '17', '825' UNION ALL
SELECT '8', '17', '9' UNION ALL
SELECT '9', '17', '4' UNION ALL
SELECT '10', '17', '8' UNION ALL
SELECT '11', '17', '503' UNION ALL
SELECT '12', '17', '849' UNION ALL
SELECT '13', '17', '850' UNION ALL
SELECT '14', '17', '854' UNION ALL
SELECT '15', '17', '10' UNION ALL
SELECT '16', '17', '11' UNION ALL
SELECT '17', '17', '16' UNION ALL
SELECT '18', '17', '963' UNION ALL
SELECT '19', '17', '970' UNION ALL
SELECT '20', '17', '1002' UNION ALL
SELECT '21', '17', '15' UNION ALL
SELECT '22', '17', '1070' UNION ALL
SELECT '23', '17', '526' UNION ALL
SELECT '24', '17', '527' UNION ALL
SELECT '25', '17', '528' UNION ALL
SELECT '26', '17', '529' UNION ALL
SELECT '27', '17', '1125' UNION ALL
SELECT '28', '17', '1126' UNION ALL
SELECT '29', '17', '1129' UNION ALL
SELECT '30', '17', '1139' UNION ALL
SELECT '31', '17', '1232' UNION ALL
SELECT '32', '17', '1233' UNION ALL
SELECT '33', '17', '1234' UNION ALL
SELECT '34', '17', '1235' UNION ALL
SELECT '35', '17', '1236' UNION ALL
SELECT '36', '17', '1237' UNION ALL
SELECT '37', '17', '1247' UNION ALL
SELECT '38', '17', '1248' UNION ALL
SELECT '39', '17', '1250' UNION ALL
SELECT '40', '17', '1282' UNION ALL
SELECT '41', '17', '1283' UNION ALL
SELECT '42', '17', '1284' UNION ALL
SELECT '43', '17', '1285' UNION ALL
SELECT '44', '17', '1286' UNION ALL
SELECT '45', '17', '1287' UNION ALL
SELECT '46', '17', '1310' UNION ALL
SELECT '47', '17', '1311' UNION ALL
SELECT '48', '17', '1312' UNION ALL
SELECT '49', '17', '1313' UNION ALL
SELECT '50', '17', '1314'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '51', '17', '1324' UNION ALL
SELECT '52', '17', '1343' UNION ALL
SELECT '53', '17', '1347' UNION ALL
SELECT '54', '17', '1356' UNION ALL
SELECT '55', '17', '1359' UNION ALL
SELECT '56', '17', '1369' UNION ALL
SELECT '57', '17', '1427' UNION ALL
SELECT '58', '17', '1430' UNION ALL
SELECT '59', '17', '1432' UNION ALL
SELECT '60', '17', '1439' UNION ALL
SELECT '61', '17', '1441' UNION ALL
SELECT '62', '17', '1442' UNION ALL
SELECT '63', '17', '1444' UNION ALL
SELECT '64', '17', '1447' UNION ALL
SELECT '65', '17', '1479' UNION ALL
SELECT '66', '17', '1485' UNION ALL
SELECT '67', '17', '1487' UNION ALL
SELECT '68', '17', '1528' UNION ALL
SELECT '69', '17', '1529' UNION ALL
SELECT '70', '17', '1530' UNION ALL
SELECT '71', '17', '1540' UNION ALL
SELECT '72', '17', '1542' UNION ALL
SELECT '73', '17', '534' UNION ALL
SELECT '74', '17', '539' UNION ALL
SELECT '75', '17', '37' UNION ALL
SELECT '76', '17', '561' UNION ALL
SELECT '77', '17', '1606' UNION ALL
SELECT '78', '17', '1608' UNION ALL
SELECT '79', '17', '1645' UNION ALL
SELECT '80', '17', '1646' UNION ALL
SELECT '81', '17', '1647' UNION ALL
SELECT '82', '17', '1648' UNION ALL
SELECT '83', '17', '1649' UNION ALL
SELECT '84', '17', '1651' UNION ALL
SELECT '85', '17', '1653' UNION ALL
SELECT '86', '17', '1654' UNION ALL
SELECT '87', '17', '565' UNION ALL
SELECT '88', '17', '566' UNION ALL
SELECT '89', '17', '571' UNION ALL
SELECT '90', '17', '572' UNION ALL
SELECT '91', '17', '39' UNION ALL
SELECT '92', '17', '492' UNION ALL
SELECT '93', '17', '1774' UNION ALL
SELECT '94', '17', '1787' UNION ALL
SELECT '95', '17', '1790' UNION ALL
SELECT '96', '17', '1807' UNION ALL
SELECT '97', '17', '1818' UNION ALL
SELECT '98', '17', '1824' UNION ALL
SELECT '99', '17', '1825' UNION ALL
SELECT '100', '17', '1846'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 2.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '101', '17', '1852' UNION ALL
SELECT '102', '17', '1855' UNION ALL
SELECT '103', '17', '1872' UNION ALL
SELECT '104', '17', '1875' UNION ALL
SELECT '105', '17', '1901' UNION ALL
SELECT '106', '17', '1903' UNION ALL
SELECT '107', '17', '1905' UNION ALL
SELECT '108', '17', '1908' UNION ALL
SELECT '109', '17', '1909' UNION ALL
SELECT '110', '17', '1910' UNION ALL
SELECT '111', '17', '1953' UNION ALL
SELECT '112', '17', '1955' UNION ALL
SELECT '113', '17', '1957' UNION ALL
SELECT '114', '17', '1959' UNION ALL
SELECT '115', '17', '1960' UNION ALL
SELECT '116', '17', '1964' UNION ALL
SELECT '117', '17', '310' UNION ALL
SELECT '118', '17', '311' UNION ALL
SELECT '119', '17', '312' UNION ALL
SELECT '120', '17', '313' UNION ALL
SELECT '121', '17', '314' UNION ALL
SELECT '122', '17', '315' UNION ALL
SELECT '123', '17', '317' UNION ALL
SELECT '124', '17', '320' UNION ALL
SELECT '125', '17', '321' UNION ALL
SELECT '126', '17', '322' UNION ALL
SELECT '127', '17', '323' UNION ALL
SELECT '128', '17', '324' UNION ALL
SELECT '129', '17', '325' UNION ALL
SELECT '130', '17', '328' UNION ALL
SELECT '131', '17', '331' UNION ALL
SELECT '132', '17', '332' UNION ALL
SELECT '133', '17', '333' UNION ALL
SELECT '134', '17', '334' UNION ALL
SELECT '135', '17', '335' UNION ALL
SELECT '136', '17', '336' UNION ALL
SELECT '137', '17', '338' UNION ALL
SELECT '138', '17', '341' UNION ALL
SELECT '139', '17', '342' UNION ALL
SELECT '140', '17', '343' UNION ALL
SELECT '141', '17', '344' UNION ALL
SELECT '142', '17', '345' UNION ALL
SELECT '143', '17', '346' UNION ALL
SELECT '144', '17', '348' UNION ALL
SELECT '145', '17', '2309' UNION ALL
SELECT '146', '17', '2310' UNION ALL
SELECT '147', '17', '376' UNION ALL
SELECT '148', '17', '378' UNION ALL
SELECT '149', '17', '381' UNION ALL
SELECT '150', '17', '2311'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 3.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '151', '17', '2314' UNION ALL
SELECT '152', '17', '2316' UNION ALL
SELECT '153', '17', '2324' UNION ALL
SELECT '154', '17', '389' UNION ALL
SELECT '155', '17', '391' UNION ALL
SELECT '156', '17', '2372' UNION ALL
SELECT '157', '17', '2373' UNION ALL
SELECT '158', '17', '2374' UNION ALL
SELECT '159', '17', '2375' UNION ALL
SELECT '160', '17', '2376' UNION ALL
SELECT '161', '17', '2377' UNION ALL
SELECT '162', '17', '2452' UNION ALL
SELECT '163', '17', '587' UNION ALL
SELECT '164', '17', '2454' UNION ALL
SELECT '165', '17', '589' UNION ALL
SELECT '166', '17', '2455' UNION ALL
SELECT '167', '17', '590' UNION ALL
SELECT '168', '17', '592' UNION ALL
SELECT '169', '17', '588' UNION ALL
SELECT '170', '17', '2457' UNION ALL
SELECT '171', '17', '2471' UNION ALL
SELECT '172', '17', '2472' UNION ALL
SELECT '173', '17', '2473' UNION ALL
SELECT '174', '17', '2474' UNION ALL
SELECT '175', '17', '2475' UNION ALL
SELECT '176', '17', '2476' UNION ALL
SELECT '177', '17', '2478' UNION ALL
SELECT '178', '17', '595' UNION ALL
SELECT '179', '17', '596' UNION ALL
SELECT '180', '17', '597' UNION ALL
SELECT '181', '17', '598' UNION ALL
SELECT '182', '17', '599' UNION ALL
SELECT '183', '17', '600' UNION ALL
SELECT '184', '17', '2518' UNION ALL
SELECT '185', '17', '2558' UNION ALL
SELECT '186', '17', '2559' UNION ALL
SELECT '187', '17', '2560' UNION ALL
SELECT '188', '17', '2561' UNION ALL
SELECT '189', '17', '2563' UNION ALL
SELECT '190', '17', '2564' UNION ALL
SELECT '191', '17', '2569' UNION ALL
SELECT '192', '17', '2571' UNION ALL
SELECT '193', '17', '2575' UNION ALL
SELECT '194', '17', '2579' UNION ALL
SELECT '195', '17', '2622' UNION ALL
SELECT '196', '17', '2625' UNION ALL
SELECT '197', '17', '452' UNION ALL
SELECT '198', '17', '2630' UNION ALL
SELECT '199', '17', '2635' UNION ALL
SELECT '200', '17', '2639'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 4.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '201', '17', '2649' UNION ALL
SELECT '202', '17', '608' UNION ALL
SELECT '203', '17', '609' UNION ALL
SELECT '204', '17', '2672' UNION ALL
SELECT '205', '17', '2784' UNION ALL
SELECT '206', '17', '2785' UNION ALL
SELECT '207', '17', '2787' UNION ALL
SELECT '208', '17', '2797' UNION ALL
SELECT '209', '17', '2798' UNION ALL
SELECT '210', '17', '2799' UNION ALL
SELECT '211', '17', '3143' UNION ALL
SELECT '212', '17', '3422' UNION ALL
SELECT '213', '17', '3423' UNION ALL
SELECT '214', '17', '3424' UNION ALL
SELECT '215', '17', '45' UNION ALL
SELECT '216', '17', '3530' UNION ALL
SELECT '217', '17', '54' UNION ALL
SELECT '218', '17', '57' UNION ALL
SELECT '219', '17', '61' UNION ALL
SELECT '220', '17', '67' UNION ALL
SELECT '221', '17', '68' UNION ALL
SELECT '222', '17', '116' UNION ALL
SELECT '223', '17', '121' UNION ALL
SELECT '224', '17', '122' UNION ALL
SELECT '225', '17', '123' UNION ALL
SELECT '226', '17', '131' UNION ALL
SELECT '227', '17', '134' UNION ALL
SELECT '228', '17', '143' UNION ALL
SELECT '229', '17', '3722' UNION ALL
SELECT '230', '17', '3723' UNION ALL
SELECT '231', '17', '3724' UNION ALL
SELECT '232', '17', '3725' UNION ALL
SELECT '233', '17', '620' UNION ALL
SELECT '234', '17', '3727' UNION ALL
SELECT '235', '17', '3728' UNION ALL
SELECT '236', '17', '622' UNION ALL
SELECT '237', '17', '623' UNION ALL
SELECT '238', '17', '624' UNION ALL
SELECT '239', '17', '3730' UNION ALL
SELECT '240', '17', '3740' UNION ALL
SELECT '241', '17', '230' UNION ALL
SELECT '242', '17', '627' UNION ALL
SELECT '243', '17', '488' UNION ALL
SELECT '244', '17', '630' UNION ALL
SELECT '245', '17', '239' UNION ALL
SELECT '246', '17', '631' UNION ALL
SELECT '247', '17', '241' UNION ALL
SELECT '248', '17', '3994' UNION ALL
SELECT '249', '17', '633' UNION ALL
SELECT '250', '17', '4020'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 5.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '251', '17', '4021' UNION ALL
SELECT '252', '17', '4024' UNION ALL
SELECT '253', '17', '4027' UNION ALL
SELECT '254', '17', '4030' UNION ALL
SELECT '255', '17', '4033' UNION ALL
SELECT '256', '17', '4034' UNION ALL
SELECT '257', '17', '4048' UNION ALL
SELECT '258', '17', '4050' UNION ALL
SELECT '259', '17', '4051' UNION ALL
SELECT '260', '17', '4052' UNION ALL
SELECT '261', '17', '432' UNION ALL
SELECT '262', '17', '434' UNION ALL
SELECT '263', '17', '436' UNION ALL
SELECT '264', '17', '4124' UNION ALL
SELECT '265', '17', '639' UNION ALL
SELECT '266', '17', '439' UNION ALL
SELECT '267', '17', '4126' UNION ALL
SELECT '268', '17', '4165' UNION ALL
SELECT '269', '17', '4210' UNION ALL
SELECT '270', '17', '262' UNION ALL
SELECT '271', '17', '263' UNION ALL
SELECT '272', '17', '264' UNION ALL
SELECT '273', '17', '4211' UNION ALL
SELECT '274', '17', '4212' UNION ALL
SELECT '275', '17', '4214' UNION ALL
SELECT '276', '17', '251' UNION ALL
SELECT '277', '17', '252' UNION ALL
SELECT '278', '17', '253' UNION ALL
SELECT '279', '17', '254' UNION ALL
SELECT '280', '17', '255' UNION ALL
SELECT '281', '17', '256' UNION ALL
SELECT '282', '14', '684' UNION ALL
SELECT '283', '14', '685' UNION ALL
SELECT '284', '14', '686' UNION ALL
SELECT '285', '14', '687' UNION ALL
SELECT '286', '14', '688' UNION ALL
SELECT '287', '14', '689' UNION ALL
SELECT '288', '14', '692' UNION ALL
SELECT '289', '14', '695' UNION ALL
SELECT '290', '14', '696' UNION ALL
SELECT '291', '14', '653' UNION ALL
SELECT '292', '14', '654' UNION ALL
SELECT '293', '14', '655' UNION ALL
SELECT '294', '14', '656' UNION ALL
SELECT '295', '14', '657' UNION ALL
SELECT '296', '14', '658' UNION ALL
SELECT '297', '14', '659' UNION ALL
SELECT '298', '14', '660' UNION ALL
SELECT '299', '14', '661' UNION ALL
SELECT '300', '14', '662'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 6.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '301', '14', '663' UNION ALL
SELECT '302', '14', '664' UNION ALL
SELECT '303', '14', '665' UNION ALL
SELECT '304', '14', '666' UNION ALL
SELECT '305', '14', '667' UNION ALL
SELECT '306', '14', '719' UNION ALL
SELECT '307', '14', '668' UNION ALL
SELECT '308', '14', '669' UNION ALL
SELECT '309', '14', '670' UNION ALL
SELECT '310', '14', '671' UNION ALL
SELECT '311', '14', '672' UNION ALL
SELECT '312', '14', '673' UNION ALL
SELECT '313', '14', '732' UNION ALL
SELECT '314', '14', '675' UNION ALL
SELECT '315', '14', '676' UNION ALL
SELECT '316', '14', '677' UNION ALL
SELECT '317', '14', '750' UNION ALL
SELECT '318', '14', '753' UNION ALL
SELECT '319', '14', '756' UNION ALL
SELECT '320', '14', '759' UNION ALL
SELECT '321', '14', '799' UNION ALL
SELECT '322', '14', '801' UNION ALL
SELECT '323', '14', '803' UNION ALL
SELECT '324', '14', '809' UNION ALL
SELECT '325', '14', '810' UNION ALL
SELECT '326', '14', '497' UNION ALL
SELECT '327', '14', '498' UNION ALL
SELECT '328', '14', '499' UNION ALL
SELECT '329', '14', '7' UNION ALL
SELECT '330', '14', '500' UNION ALL
SELECT '331', '14', '6' UNION ALL
SELECT '332', '14', '833' UNION ALL
SELECT '333', '14', '9' UNION ALL
SELECT '334', '14', '8' UNION ALL
SELECT '335', '14', '502' UNION ALL
SELECT '336', '14', '503' UNION ALL
SELECT '337', '14', '504' UNION ALL
SELECT '338', '14', '505' UNION ALL
SELECT '339', '14', '10' UNION ALL
SELECT '340', '14', '506' UNION ALL
SELECT '341', '14', '908' UNION ALL
SELECT '342', '14', '508' UNION ALL
SELECT '343', '14', '11' UNION ALL
SELECT '344', '14', '509' UNION ALL
SELECT '345', '14', '510' UNION ALL
SELECT '346', '14', '511' UNION ALL
SELECT '347', '14', '12' UNION ALL
SELECT '348', '14', '512' UNION ALL
SELECT '349', '14', '920' UNION ALL
SELECT '350', '14', '32'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 7.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '351', '14', '513' UNION ALL
SELECT '352', '14', '514' UNION ALL
SELECT '353', '14', '515' UNION ALL
SELECT '354', '14', '516' UNION ALL
SELECT '355', '14', '517' UNION ALL
SELECT '356', '14', '518' UNION ALL
SELECT '357', '14', '16' UNION ALL
SELECT '358', '14', '519' UNION ALL
SELECT '359', '14', '17' UNION ALL
SELECT '360', '14', '520' UNION ALL
SELECT '361', '14', '521' UNION ALL
SELECT '362', '14', '522' UNION ALL
SELECT '363', '14', '990' UNION ALL
SELECT '364', '14', '524' UNION ALL
SELECT '365', '14', '525' UNION ALL
SELECT '366', '14', '526' UNION ALL
SELECT '367', '14', '527' UNION ALL
SELECT '368', '14', '528' UNION ALL
SELECT '369', '14', '529' UNION ALL
SELECT '370', '14', '531' UNION ALL
SELECT '371', '14', '1198' UNION ALL
SELECT '372', '14', '1437' UNION ALL
SELECT '373', '14', '1447' UNION ALL
SELECT '374', '14', '534' UNION ALL
SELECT '375', '14', '535' UNION ALL
SELECT '376', '14', '536' UNION ALL
SELECT '377', '14', '537' UNION ALL
SELECT '378', '14', '538' UNION ALL
SELECT '379', '14', '539' UNION ALL
SELECT '380', '14', '540' UNION ALL
SELECT '381', '14', '541' UNION ALL
SELECT '382', '14', '542' UNION ALL
SELECT '383', '14', '543' UNION ALL
SELECT '384', '14', '544' UNION ALL
SELECT '385', '14', '545' UNION ALL
SELECT '386', '14', '546' UNION ALL
SELECT '387', '14', '547' UNION ALL
SELECT '388', '14', '548' UNION ALL
SELECT '389', '14', '549' UNION ALL
SELECT '390', '14', '550' UNION ALL
SELECT '391', '14', '551' UNION ALL
SELECT '392', '14', '552' UNION ALL
SELECT '393', '14', '553' UNION ALL
SELECT '394', '14', '554' UNION ALL
SELECT '395', '14', '555' UNION ALL
SELECT '396', '14', '556' UNION ALL
SELECT '397', '14', '557' UNION ALL
SELECT '398', '14', '1591' UNION ALL
SELECT '399', '14', '559' UNION ALL
SELECT '400', '14', '560'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 8.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '401', '14', '37' UNION ALL
SELECT '402', '14', '561' UNION ALL
SELECT '403', '14', '1606' UNION ALL
SELECT '404', '14', '562' UNION ALL
SELECT '405', '14', '563' UNION ALL
SELECT '406', '14', '565' UNION ALL
SELECT '407', '14', '566' UNION ALL
SELECT '408', '14', '1689' UNION ALL
SELECT '409', '14', '567' UNION ALL
SELECT '410', '14', '568' UNION ALL
SELECT '411', '14', '1693' UNION ALL
SELECT '412', '14', '570' UNION ALL
SELECT '413', '14', '571' UNION ALL
SELECT '414', '14', '572' UNION ALL
SELECT '415', '14', '573' UNION ALL
SELECT '416', '14', '574' UNION ALL
SELECT '417', '14', '39' UNION ALL
SELECT '418', '14', '492' UNION ALL
SELECT '419', '14', '575' UNION ALL
SELECT '420', '14', '576' UNION ALL
SELECT '421', '14', '577' UNION ALL
SELECT '422', '14', '40' UNION ALL
SELECT '423', '14', '41' UNION ALL
SELECT '424', '14', '42' UNION ALL
SELECT '425', '14', '43' UNION ALL
SELECT '426', '14', '578' UNION ALL
SELECT '427', '14', '579' UNION ALL
SELECT '428', '14', '293' UNION ALL
SELECT '429', '14', '294' UNION ALL
SELECT '430', '14', '295' UNION ALL
SELECT '431', '14', '296' UNION ALL
SELECT '432', '14', '297' UNION ALL
SELECT '433', '14', '298' UNION ALL
SELECT '434', '14', '299' UNION ALL
SELECT '435', '14', '300' UNION ALL
SELECT '436', '14', '301' UNION ALL
SELECT '437', '14', '304' UNION ALL
SELECT '438', '14', '305' UNION ALL
SELECT '439', '14', '306' UNION ALL
SELECT '440', '14', '307' UNION ALL
SELECT '441', '14', '308' UNION ALL
SELECT '442', '14', '310' UNION ALL
SELECT '443', '14', '311' UNION ALL
SELECT '444', '14', '312' UNION ALL
SELECT '445', '14', '313' UNION ALL
SELECT '446', '14', '314' UNION ALL
SELECT '447', '14', '315' UNION ALL
SELECT '448', '14', '320' UNION ALL
SELECT '449', '14', '321' UNION ALL
SELECT '450', '14', '322'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 9.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '451', '14', '323' UNION ALL
SELECT '452', '14', '324' UNION ALL
SELECT '453', '14', '325' UNION ALL
SELECT '454', '14', '326' UNION ALL
SELECT '455', '14', '327' UNION ALL
SELECT '456', '14', '328' UNION ALL
SELECT '457', '14', '331' UNION ALL
SELECT '458', '14', '332' UNION ALL
SELECT '459', '14', '333' UNION ALL
SELECT '460', '14', '334' UNION ALL
SELECT '461', '14', '335' UNION ALL
SELECT '462', '14', '336' UNION ALL
SELECT '463', '14', '337' UNION ALL
SELECT '464', '14', '338' UNION ALL
SELECT '465', '14', '341' UNION ALL
SELECT '466', '14', '342' UNION ALL
SELECT '467', '14', '343' UNION ALL
SELECT '468', '14', '345' UNION ALL
SELECT '469', '14', '346' UNION ALL
SELECT '470', '14', '347' UNION ALL
SELECT '471', '14', '348' UNION ALL
SELECT '472', '14', '580' UNION ALL
SELECT '473', '14', '581' UNION ALL
SELECT '474', '14', '582' UNION ALL
SELECT '475', '14', '354' UNION ALL
SELECT '476', '14', '355' UNION ALL
SELECT '477', '14', '356' UNION ALL
SELECT '478', '14', '357' UNION ALL
SELECT '479', '14', '361' UNION ALL
SELECT '480', '14', '362' UNION ALL
SELECT '481', '14', '363' UNION ALL
SELECT '482', '14', '364' UNION ALL
SELECT '483', '14', '370' UNION ALL
SELECT '484', '14', '371' UNION ALL
SELECT '485', '14', '379' UNION ALL
SELECT '486', '14', '377' UNION ALL
SELECT '487', '14', '583' UNION ALL
SELECT '488', '14', '384' UNION ALL
SELECT '489', '14', '387' UNION ALL
SELECT '490', '14', '388' UNION ALL
SELECT '491', '14', '389' UNION ALL
SELECT '492', '14', '390' UNION ALL
SELECT '493', '14', '391' UNION ALL
SELECT '494', '14', '392' UNION ALL
SELECT '495', '14', '393' UNION ALL
SELECT '496', '14', '394' UNION ALL
SELECT '497', '14', '395' UNION ALL
SELECT '498', '14', '397' UNION ALL
SELECT '499', '14', '399' UNION ALL
SELECT '500', '14', '402'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 10.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '501', '14', '584' UNION ALL
SELECT '502', '14', '585' UNION ALL
SELECT '503', '14', '586' UNION ALL
SELECT '504', '14', '587' UNION ALL
SELECT '505', '14', '588' UNION ALL
SELECT '506', '14', '487' UNION ALL
SELECT '507', '14', '440' UNION ALL
SELECT '508', '14', '441' UNION ALL
SELECT '509', '14', '2462' UNION ALL
SELECT '510', '14', '442' UNION ALL
SELECT '511', '14', '2464' UNION ALL
SELECT '512', '14', '2465' UNION ALL
SELECT '513', '14', '2466' UNION ALL
SELECT '514', '14', '593' UNION ALL
SELECT '515', '14', '594' UNION ALL
SELECT '516', '14', '595' UNION ALL
SELECT '517', '14', '596' UNION ALL
SELECT '518', '14', '597' UNION ALL
SELECT '519', '14', '598' UNION ALL
SELECT '520', '14', '599' UNION ALL
SELECT '521', '14', '600' UNION ALL
SELECT '522', '14', '601' UNION ALL
SELECT '523', '14', '602' UNION ALL
SELECT '524', '14', '603' UNION ALL
SELECT '525', '14', '604' UNION ALL
SELECT '526', '14', '605' UNION ALL
SELECT '527', '14', '606' UNION ALL
SELECT '528', '14', '607' UNION ALL
SELECT '529', '14', '608' UNION ALL
SELECT '530', '14', '609' UNION ALL
SELECT '531', '14', '610' UNION ALL
SELECT '532', '14', '611' UNION ALL
SELECT '533', '14', '612' UNION ALL
SELECT '534', '14', '613' UNION ALL
SELECT '535', '14', '614' UNION ALL
SELECT '536', '14', '615' UNION ALL
SELECT '537', '14', '2784' UNION ALL
SELECT '538', '14', '2785' UNION ALL
SELECT '539', '14', '2787' UNION ALL
SELECT '540', '14', '616' UNION ALL
SELECT '541', '14', '470' UNION ALL
SELECT '542', '14', '471' UNION ALL
SELECT '543', '14', '469' UNION ALL
SELECT '544', '14', '617' UNION ALL
SELECT '545', '14', '678' UNION ALL
SELECT '546', '14', '679' UNION ALL
SELECT '547', '14', '618' UNION ALL
SELECT '548', '14', '44' UNION ALL
SELECT '549', '14', '45' UNION ALL
SELECT '550', '14', '46'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 11.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '551', '14', '47' UNION ALL
SELECT '552', '14', '48' UNION ALL
SELECT '553', '14', '49' UNION ALL
SELECT '554', '14', '52' UNION ALL
SELECT '555', '14', '54' UNION ALL
SELECT '556', '14', '55' UNION ALL
SELECT '557', '14', '56' UNION ALL
SELECT '558', '14', '58' UNION ALL
SELECT '559', '14', '60' UNION ALL
SELECT '560', '14', '61' UNION ALL
SELECT '561', '14', '62' UNION ALL
SELECT '562', '14', '63' UNION ALL
SELECT '563', '14', '64' UNION ALL
SELECT '564', '14', '66' UNION ALL
SELECT '565', '14', '71' UNION ALL
SELECT '566', '14', '74' UNION ALL
SELECT '567', '14', '75' UNION ALL
SELECT '568', '14', '97' UNION ALL
SELECT '569', '14', '98' UNION ALL
SELECT '570', '14', '99' UNION ALL
SELECT '571', '14', '101' UNION ALL
SELECT '572', '14', '104' UNION ALL
SELECT '573', '14', '80' UNION ALL
SELECT '574', '14', '82' UNION ALL
SELECT '575', '14', '85' UNION ALL
SELECT '576', '14', '86' UNION ALL
SELECT '577', '14', '115' UNION ALL
SELECT '578', '14', '116' UNION ALL
SELECT '579', '14', '117' UNION ALL
SELECT '580', '14', '123' UNION ALL
SELECT '581', '14', '129' UNION ALL
SELECT '582', '14', '130' UNION ALL
SELECT '583', '14', '131' UNION ALL
SELECT '584', '14', '132' UNION ALL
SELECT '585', '14', '133' UNION ALL
SELECT '586', '14', '134' UNION ALL
SELECT '587', '14', '136' UNION ALL
SELECT '588', '14', '139' UNION ALL
SELECT '589', '14', '140' UNION ALL
SELECT '590', '14', '141' UNION ALL
SELECT '591', '14', '143' UNION ALL
SELECT '592', '14', '145' UNION ALL
SELECT '593', '14', '146' UNION ALL
SELECT '594', '14', '147' UNION ALL
SELECT '595', '14', '148' UNION ALL
SELECT '596', '14', '159' UNION ALL
SELECT '597', '14', '168' UNION ALL
SELECT '598', '14', '171' UNION ALL
SELECT '599', '14', '173' UNION ALL
SELECT '600', '14', '174'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 12.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '601', '14', '175' UNION ALL
SELECT '602', '14', '176' UNION ALL
SELECT '603', '14', '177' UNION ALL
SELECT '604', '14', '619' UNION ALL
SELECT '605', '14', '180' UNION ALL
SELECT '606', '14', '181' UNION ALL
SELECT '607', '14', '182' UNION ALL
SELECT '608', '14', '620' UNION ALL
SELECT '609', '14', '3726' UNION ALL
SELECT '610', '14', '622' UNION ALL
SELECT '611', '14', '623' UNION ALL
SELECT '612', '14', '624' UNION ALL
SELECT '613', '14', '183' UNION ALL
SELECT '614', '14', '184' UNION ALL
SELECT '615', '14', '185' UNION ALL
SELECT '616', '14', '625' UNION ALL
SELECT '617', '14', '3732' UNION ALL
SELECT '618', '14', '626' UNION ALL
SELECT '619', '14', '230' UNION ALL
SELECT '620', '14', '627' UNION ALL
SELECT '621', '14', '234' UNION ALL
SELECT '622', '14', '628' UNION ALL
SELECT '623', '14', '233' UNION ALL
SELECT '624', '14', '236' UNION ALL
SELECT '625', '14', '488' UNION ALL
SELECT '626', '14', '235' UNION ALL
SELECT '627', '14', '630' UNION ALL
SELECT '628', '14', '238' UNION ALL
SELECT '629', '14', '239' UNION ALL
SELECT '630', '14', '237' UNION ALL
SELECT '631', '14', '631' UNION ALL
SELECT '632', '14', '240' UNION ALL
SELECT '633', '14', '241' UNION ALL
SELECT '634', '14', '243' UNION ALL
SELECT '635', '14', '632' UNION ALL
SELECT '636', '14', '245' UNION ALL
SELECT '637', '14', '633' UNION ALL
SELECT '638', '14', '246' UNION ALL
SELECT '639', '14', '634' UNION ALL
SELECT '640', '14', '232' UNION ALL
SELECT '641', '14', '4009' UNION ALL
SELECT '642', '14', '636' UNION ALL
SELECT '643', '14', '405' UNION ALL
SELECT '644', '14', '406' UNION ALL
SELECT '645', '14', '407' UNION ALL
SELECT '646', '14', '637' UNION ALL
SELECT '647', '14', '408' UNION ALL
SELECT '648', '14', '409' UNION ALL
SELECT '649', '14', '410' UNION ALL
SELECT '650', '14', '411'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 13.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '651', '14', '412' UNION ALL
SELECT '652', '14', '413' UNION ALL
SELECT '653', '14', '414' UNION ALL
SELECT '654', '14', '416' UNION ALL
SELECT '655', '14', '417' UNION ALL
SELECT '656', '14', '418' UNION ALL
SELECT '657', '14', '419' UNION ALL
SELECT '658', '14', '420' UNION ALL
SELECT '659', '14', '421' UNION ALL
SELECT '660', '14', '422' UNION ALL
SELECT '661', '14', '423' UNION ALL
SELECT '662', '14', '424' UNION ALL
SELECT '663', '14', '425' UNION ALL
SELECT '664', '14', '426' UNION ALL
SELECT '665', '14', '427' UNION ALL
SELECT '666', '14', '638' UNION ALL
SELECT '667', '14', '428' UNION ALL
SELECT '668', '14', '4113' UNION ALL
SELECT '669', '14', '433' UNION ALL
SELECT '670', '14', '434' UNION ALL
SELECT '671', '14', '435' UNION ALL
SELECT '672', '14', '436' UNION ALL
SELECT '673', '14', '437' UNION ALL
SELECT '674', '14', '639' UNION ALL
SELECT '675', '14', '438' UNION ALL
SELECT '676', '14', '640' UNION ALL
SELECT '677', '14', '264' UNION ALL
SELECT '678', '14', '265' UNION ALL
SELECT '679', '14', '266' UNION ALL
SELECT '680', '14', '267' UNION ALL
SELECT '681', '14', '268' UNION ALL
SELECT '682', '14', '641' UNION ALL
SELECT '683', '14', '257' UNION ALL
SELECT '684', '14', '258' UNION ALL
SELECT '685', '14', '259' UNION ALL
SELECT '686', '14', '260' UNION ALL
SELECT '687', '14', '261' UNION ALL
SELECT '688', '14', '642' UNION ALL
SELECT '689', '14', '643' UNION ALL
SELECT '690', '14', '269' UNION ALL
SELECT '691', '14', '278' UNION ALL
SELECT '692', '14', '279' UNION ALL
SELECT '693', '14', '280' UNION ALL
SELECT '694', '14', '281' UNION ALL
SELECT '695', '14', '283' UNION ALL
SELECT '696', '14', '284' UNION ALL
SELECT '697', '14', '287' UNION ALL
SELECT '698', '14', '288' UNION ALL
SELECT '699', '14', '289' UNION ALL
SELECT '700', '14', '290'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 14.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '701', '14', '483' UNION ALL
SELECT '702', '14', '485' UNION ALL
SELECT '703', '14', '486' UNION ALL
SELECT '704', '10', '750' UNION ALL
SELECT '705', '10', '753' UNION ALL
SELECT '706', '10', '756' UNION ALL
SELECT '707', '10', '2' UNION ALL
SELECT '708', '10', '799' UNION ALL
SELECT '709', '10', '5' UNION ALL
SELECT '710', '10', '7' UNION ALL
SELECT '711', '10', '6' UNION ALL
SELECT '712', '10', '9' UNION ALL
SELECT '713', '10', '8' UNION ALL
SELECT '714', '10', '10' UNION ALL
SELECT '715', '10', '11' UNION ALL
SELECT '716', '10', '12' UNION ALL
SELECT '717', '10', '920' UNION ALL
SELECT '718', '10', '32' UNION ALL
SELECT '719', '10', '16' UNION ALL
SELECT '720', '10', '17' UNION ALL
SELECT '721', '10', '971' UNION ALL
SELECT '722', '10', '521' UNION ALL
SELECT '723', '10', '20' UNION ALL
SELECT '724', '10', '19' UNION ALL
SELECT '725', '10', '22' UNION ALL
SELECT '726', '10', '1198' UNION ALL
SELECT '727', '10', '24' UNION ALL
SELECT '728', '10', '37' UNION ALL
SELECT '729', '10', '1606' UNION ALL
SELECT '730', '10', '563' UNION ALL
SELECT '731', '10', '1689' UNION ALL
SELECT '732', '10', '567' UNION ALL
SELECT '733', '10', '568' UNION ALL
SELECT '734', '10', '1693' UNION ALL
SELECT '735', '10', '571' UNION ALL
SELECT '736', '10', '572' UNION ALL
SELECT '737', '10', '39' UNION ALL
SELECT '738', '10', '490' UNION ALL
SELECT '739', '10', '40' UNION ALL
SELECT '740', '10', '41' UNION ALL
SELECT '741', '10', '42' UNION ALL
SELECT '742', '10', '43' UNION ALL
SELECT '743', '10', '293' UNION ALL
SELECT '744', '10', '294' UNION ALL
SELECT '745', '10', '295' UNION ALL
SELECT '746', '10', '296' UNION ALL
SELECT '747', '10', '297' UNION ALL
SELECT '748', '10', '1997' UNION ALL
SELECT '749', '10', '1998' UNION ALL
SELECT '750', '10', '298'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 15.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '751', '10', '299' UNION ALL
SELECT '752', '10', '300' UNION ALL
SELECT '753', '10', '301' UNION ALL
SELECT '754', '10', '1999' UNION ALL
SELECT '755', '10', '302' UNION ALL
SELECT '756', '10', '2000' UNION ALL
SELECT '757', '10', '303' UNION ALL
SELECT '758', '10', '304' UNION ALL
SELECT '759', '10', '305' UNION ALL
SELECT '760', '10', '306' UNION ALL
SELECT '761', '10', '307' UNION ALL
SELECT '762', '10', '308' UNION ALL
SELECT '763', '10', '310' UNION ALL
SELECT '764', '10', '311' UNION ALL
SELECT '765', '10', '312' UNION ALL
SELECT '766', '10', '313' UNION ALL
SELECT '767', '10', '314' UNION ALL
SELECT '768', '10', '315' UNION ALL
SELECT '769', '10', '316' UNION ALL
SELECT '770', '10', '320' UNION ALL
SELECT '771', '10', '321' UNION ALL
SELECT '772', '10', '322' UNION ALL
SELECT '773', '10', '323' UNION ALL
SELECT '774', '10', '324' UNION ALL
SELECT '775', '10', '325' UNION ALL
SELECT '776', '10', '326' UNION ALL
SELECT '777', '10', '327' UNION ALL
SELECT '778', '10', '328' UNION ALL
SELECT '779', '10', '331' UNION ALL
SELECT '780', '10', '332' UNION ALL
SELECT '781', '10', '333' UNION ALL
SELECT '782', '10', '334' UNION ALL
SELECT '783', '10', '335' UNION ALL
SELECT '784', '10', '336' UNION ALL
SELECT '785', '10', '337' UNION ALL
SELECT '786', '10', '338' UNION ALL
SELECT '787', '10', '341' UNION ALL
SELECT '788', '10', '342' UNION ALL
SELECT '789', '10', '343' UNION ALL
SELECT '790', '10', '344' UNION ALL
SELECT '791', '10', '345' UNION ALL
SELECT '792', '10', '346' UNION ALL
SELECT '793', '10', '347' UNION ALL
SELECT '794', '10', '348' UNION ALL
SELECT '795', '10', '2195' UNION ALL
SELECT '796', '10', '355' UNION ALL
SELECT '797', '10', '356' UNION ALL
SELECT '798', '10', '357' UNION ALL
SELECT '799', '10', '362' UNION ALL
SELECT '800', '10', '364'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 16.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '801', '10', '369' UNION ALL
SELECT '802', '10', '374' UNION ALL
SELECT '803', '10', '375' UNION ALL
SELECT '804', '10', '379' UNION ALL
SELECT '805', '10', '376' UNION ALL
SELECT '806', '10', '377' UNION ALL
SELECT '807', '10', '378' UNION ALL
SELECT '808', '10', '380' UNION ALL
SELECT '809', '10', '381' UNION ALL
SELECT '810', '10', '382' UNION ALL
SELECT '811', '10', '384' UNION ALL
SELECT '812', '10', '387' UNION ALL
SELECT '813', '10', '388' UNION ALL
SELECT '814', '10', '389' UNION ALL
SELECT '815', '10', '392' UNION ALL
SELECT '816', '10', '393' UNION ALL
SELECT '817', '10', '394' UNION ALL
SELECT '818', '10', '395' UNION ALL
SELECT '819', '10', '396' UNION ALL
SELECT '820', '10', '397' UNION ALL
SELECT '821', '10', '398' UNION ALL
SELECT '822', '10', '399' UNION ALL
SELECT '823', '10', '401' UNION ALL
SELECT '824', '10', '590' UNION ALL
SELECT '825', '10', '487' UNION ALL
SELECT '826', '10', '440' UNION ALL
SELECT '827', '10', '441' UNION ALL
SELECT '828', '10', '2462' UNION ALL
SELECT '829', '10', '442' UNION ALL
SELECT '830', '10', '2464' UNION ALL
SELECT '831', '10', '2465' UNION ALL
SELECT '832', '10', '2466' UNION ALL
SELECT '833', '10', '2467' UNION ALL
SELECT '834', '10', '2468' UNION ALL
SELECT '835', '10', '2469' UNION ALL
SELECT '836', '10', '595' UNION ALL
SELECT '837', '10', '596' UNION ALL
SELECT '838', '10', '597' UNION ALL
SELECT '839', '10', '598' UNION ALL
SELECT '840', '10', '599' UNION ALL
SELECT '841', '10', '600' UNION ALL
SELECT '842', '10', '680' UNION ALL
SELECT '843', '10', '681' UNION ALL
SELECT '844', '10', '451' UNION ALL
SELECT '845', '10', '603' UNION ALL
SELECT '846', '10', '604' UNION ALL
SELECT '847', '10', '605' UNION ALL
SELECT '848', '10', '606' UNION ALL
SELECT '849', '10', '607' UNION ALL
SELECT '850', '10', '2661'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 17.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '851', '10', '2667' UNION ALL
SELECT '852', '10', '2674' UNION ALL
SELECT '853', '10', '2682' UNION ALL
SELECT '854', '10', '612' UNION ALL
SELECT '855', '10', '2711' UNION ALL
SELECT '856', '10', '613' UNION ALL
SELECT '857', '10', '614' UNION ALL
SELECT '858', '10', '2718' UNION ALL
SELECT '859', '10', '2738' UNION ALL
SELECT '860', '10', '2784' UNION ALL
SELECT '861', '10', '2785' UNION ALL
SELECT '862', '10', '2787' UNION ALL
SELECT '863', '10', '616' UNION ALL
SELECT '864', '10', '469' UNION ALL
SELECT '865', '10', '3064' UNION ALL
SELECT '866', '10', '475' UNION ALL
SELECT '867', '10', '3068' UNION ALL
SELECT '868', '10', '477' UNION ALL
SELECT '869', '10', '3070' UNION ALL
SELECT '870', '10', '479' UNION ALL
SELECT '871', '10', '3074' UNION ALL
SELECT '872', '10', '476' UNION ALL
SELECT '873', '10', '3081' UNION ALL
SELECT '874', '10', '478' UNION ALL
SELECT '875', '10', '44' UNION ALL
SELECT '876', '10', '45' UNION ALL
SELECT '877', '10', '46' UNION ALL
SELECT '878', '10', '47' UNION ALL
SELECT '879', '10', '48' UNION ALL
SELECT '880', '10', '49' UNION ALL
SELECT '881', '10', '3530' UNION ALL
SELECT '882', '10', '50' UNION ALL
SELECT '883', '10', '51' UNION ALL
SELECT '884', '10', '54' UNION ALL
SELECT '885', '10', '55' UNION ALL
SELECT '886', '10', '56' UNION ALL
SELECT '887', '10', '57' UNION ALL
SELECT '888', '10', '58' UNION ALL
SELECT '889', '10', '59' UNION ALL
SELECT '890', '10', '60' UNION ALL
SELECT '891', '10', '61' UNION ALL
SELECT '892', '10', '62' UNION ALL
SELECT '893', '10', '63' UNION ALL
SELECT '894', '10', '64' UNION ALL
SELECT '895', '10', '65' UNION ALL
SELECT '896', '10', '66' UNION ALL
SELECT '897', '10', '67' UNION ALL
SELECT '898', '10', '68' UNION ALL
SELECT '899', '10', '69' UNION ALL
SELECT '900', '10', '70'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 18.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '901', '10', '71' UNION ALL
SELECT '902', '10', '73' UNION ALL
SELECT '903', '10', '74' UNION ALL
SELECT '904', '10', '75' UNION ALL
SELECT '905', '10', '90' UNION ALL
SELECT '906', '10', '80' UNION ALL
SELECT '907', '10', '82' UNION ALL
SELECT '908', '10', '84' UNION ALL
SELECT '909', '10', '85' UNION ALL
SELECT '910', '10', '86' UNION ALL
SELECT '911', '10', '114' UNION ALL
SELECT '912', '10', '115' UNION ALL
SELECT '913', '10', '116' UNION ALL
SELECT '914', '10', '117' UNION ALL
SELECT '915', '10', '121' UNION ALL
SELECT '916', '10', '122' UNION ALL
SELECT '917', '10', '123' UNION ALL
SELECT '918', '10', '126' UNION ALL
SELECT '919', '10', '129' UNION ALL
SELECT '920', '10', '130' UNION ALL
SELECT '921', '10', '131' UNION ALL
SELECT '922', '10', '132' UNION ALL
SELECT '923', '10', '133' UNION ALL
SELECT '924', '10', '134' UNION ALL
SELECT '925', '10', '135' UNION ALL
SELECT '926', '10', '136' UNION ALL
SELECT '927', '10', '137' UNION ALL
SELECT '928', '10', '138' UNION ALL
SELECT '929', '10', '139' UNION ALL
SELECT '930', '10', '140' UNION ALL
SELECT '931', '10', '141' UNION ALL
SELECT '932', '10', '143' UNION ALL
SELECT '933', '10', '144' UNION ALL
SELECT '934', '10', '145' UNION ALL
SELECT '935', '10', '146' UNION ALL
SELECT '936', '10', '147' UNION ALL
SELECT '937', '10', '148' UNION ALL
SELECT '938', '10', '150' UNION ALL
SELECT '939', '10', '151' UNION ALL
SELECT '940', '10', '155' UNION ALL
SELECT '941', '10', '156' UNION ALL
SELECT '942', '10', '159' UNION ALL
SELECT '943', '10', '160' UNION ALL
SELECT '944', '10', '161' UNION ALL
SELECT '945', '10', '164' UNION ALL
SELECT '946', '10', '165' UNION ALL
SELECT '947', '10', '166' UNION ALL
SELECT '948', '10', '168' UNION ALL
SELECT '949', '10', '171' UNION ALL
SELECT '950', '10', '173'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 19.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '951', '10', '174' UNION ALL
SELECT '952', '10', '175' UNION ALL
SELECT '953', '10', '176' UNION ALL
SELECT '954', '10', '179' UNION ALL
SELECT '955', '10', '180' UNION ALL
SELECT '956', '10', '181' UNION ALL
SELECT '957', '10', '182' UNION ALL
SELECT '958', '10', '623' UNION ALL
SELECT '959', '10', '183' UNION ALL
SELECT '960', '10', '187' UNION ALL
SELECT '961', '10', '202' UNION ALL
SELECT '962', '10', '204' UNION ALL
SELECT '963', '10', '206' UNION ALL
SELECT '964', '10', '209' UNION ALL
SELECT '965', '10', '210' UNION ALL
SELECT '966', '10', '211' UNION ALL
SELECT '967', '10', '212' UNION ALL
SELECT '968', '10', '213' UNION ALL
SELECT '969', '10', '216' UNION ALL
SELECT '970', '10', '217' UNION ALL
SELECT '971', '10', '219' UNION ALL
SELECT '972', '10', '220' UNION ALL
SELECT '973', '10', '221' UNION ALL
SELECT '974', '10', '224' UNION ALL
SELECT '975', '10', '225' UNION ALL
SELECT '976', '10', '228' UNION ALL
SELECT '977', '10', '3946' UNION ALL
SELECT '978', '10', '3957' UNION ALL
SELECT '979', '10', '230' UNION ALL
SELECT '980', '10', '229' UNION ALL
SELECT '981', '10', '234' UNION ALL
SELECT '982', '10', '628' UNION ALL
SELECT '983', '10', '233' UNION ALL
SELECT '984', '10', '236' UNION ALL
SELECT '985', '10', '488' UNION ALL
SELECT '986', '10', '235' UNION ALL
SELECT '987', '10', '238' UNION ALL
SELECT '988', '10', '239' UNION ALL
SELECT '989', '10', '237' UNION ALL
SELECT '990', '10', '240' UNION ALL
SELECT '991', '10', '241' UNION ALL
SELECT '992', '10', '243' UNION ALL
SELECT '993', '10', '632' UNION ALL
SELECT '994', '10', '245' UNION ALL
SELECT '995', '10', '633' UNION ALL
SELECT '996', '10', '246' UNION ALL
SELECT '997', '10', '634' UNION ALL
SELECT '998', '10', '247' UNION ALL
SELECT '999', '10', '4007' UNION ALL
SELECT '1000', '10', '232'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 20.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '1001', '10', '405' UNION ALL
SELECT '1002', '10', '406' UNION ALL
SELECT '1003', '10', '407' UNION ALL
SELECT '1004', '10', '637' UNION ALL
SELECT '1005', '10', '408' UNION ALL
SELECT '1006', '10', '409' UNION ALL
SELECT '1007', '10', '410' UNION ALL
SELECT '1008', '10', '4058' UNION ALL
SELECT '1009', '10', '411' UNION ALL
SELECT '1010', '10', '412' UNION ALL
SELECT '1011', '10', '413' UNION ALL
SELECT '1012', '10', '414' UNION ALL
SELECT '1013', '10', '415' UNION ALL
SELECT '1014', '10', '416' UNION ALL
SELECT '1015', '10', '4066' UNION ALL
SELECT '1016', '10', '4067' UNION ALL
SELECT '1017', '10', '4072' UNION ALL
SELECT '1018', '10', '417' UNION ALL
SELECT '1019', '10', '418' UNION ALL
SELECT '1020', '10', '419' UNION ALL
SELECT '1021', '10', '420' UNION ALL
SELECT '1022', '10', '421' UNION ALL
SELECT '1023', '10', '422' UNION ALL
SELECT '1024', '10', '4103' UNION ALL
SELECT '1025', '10', '423' UNION ALL
SELECT '1026', '10', '4104' UNION ALL
SELECT '1027', '10', '4105' UNION ALL
SELECT '1028', '10', '424' UNION ALL
SELECT '1029', '10', '425' UNION ALL
SELECT '1030', '10', '638' UNION ALL
SELECT '1031', '10', '429' UNION ALL
SELECT '1032', '10', '4112' UNION ALL
SELECT '1033', '10', '4113' UNION ALL
SELECT '1034', '10', '431' UNION ALL
SELECT '1035', '10', '433' UNION ALL
SELECT '1036', '10', '436' UNION ALL
SELECT '1037', '10', '437' UNION ALL
SELECT '1038', '10', '639' UNION ALL
SELECT '1039', '10', '439' UNION ALL
SELECT '1040', '10', '4177' UNION ALL
SELECT '1041', '10', '4190' UNION ALL
SELECT '1042', '10', '4191' UNION ALL
SELECT '1043', '10', '4192' UNION ALL
SELECT '1044', '10', '4193' UNION ALL
SELECT '1045', '10', '262' UNION ALL
SELECT '1046', '10', '263' UNION ALL
SELECT '1047', '10', '264' UNION ALL
SELECT '1048', '10', '4211' UNION ALL
SELECT '1049', '10', '4212' UNION ALL
SELECT '1050', '10', '265'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 21.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '1051', '10', '266' UNION ALL
SELECT '1052', '10', '267' UNION ALL
SELECT '1053', '10', '268' UNION ALL
SELECT '1054', '10', '251' UNION ALL
SELECT '1055', '10', '252' UNION ALL
SELECT '1056', '10', '253' UNION ALL
SELECT '1057', '10', '254' UNION ALL
SELECT '1058', '10', '255' UNION ALL
SELECT '1059', '10', '256' UNION ALL
SELECT '1060', '10', '257' UNION ALL
SELECT '1061', '10', '258' UNION ALL
SELECT '1062', '10', '259' UNION ALL
SELECT '1063', '10', '260' UNION ALL
SELECT '1064', '10', '261' UNION ALL
SELECT '1065', '10', '270' UNION ALL
SELECT '1066', '10', '271' UNION ALL
SELECT '1067', '10', '272' UNION ALL
SELECT '1068', '10', '273' UNION ALL
SELECT '1069', '10', '274' UNION ALL
SELECT '1070', '10', '275' UNION ALL
SELECT '1071', '10', '276' UNION ALL
SELECT '1072', '10', '277' UNION ALL
SELECT '1073', '10', '280' UNION ALL
SELECT '1074', '10', '284' UNION ALL
SELECT '1075', '10', '287' UNION ALL
SELECT '1076', '10', '288' UNION ALL
SELECT '1077', '10', '289' UNION ALL
SELECT '1078', '10', '290' UNION ALL
SELECT '1079', '10', '291' UNION ALL
SELECT '1080', '10', '292' UNION ALL
SELECT '1081', '10', '4334' UNION ALL
SELECT '1082', '10', '482' UNION ALL
SELECT '1083', '10', '483' UNION ALL
SELECT '1084', '10', '484' UNION ALL
SELECT '1085', '10', '485' UNION ALL
SELECT '1086', '10', '486'
COMMIT;
RAISERROR (N'[dbo].[OperatingCenterStockedMaterials]: Insert Batch: 22.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] OFF;
