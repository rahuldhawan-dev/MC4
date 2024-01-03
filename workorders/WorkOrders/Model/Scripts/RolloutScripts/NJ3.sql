-- turn the op center on
UPDATE tblOpCntr SET WorkOrdersEnabled = 1 WHERE OpCntr = 'NJ3';

-- spoil removal costs
INSERT INTO [OperatingCenterSpoilRemovalCosts] ([OperatingCenterID], [Cost])
VALUES (11, 75)

-- restoration type costs
SET IDENTITY_INSERT [dbo].[RestorationTypeCosts] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[RestorationTypeCosts]([RestorationTypeCostID], [OperatingCenterID], [RestorationTypeID], [Cost])
SELECT '49', '11', '1', '12' UNION ALL
SELECT '50', '11', '2', '12' UNION ALL
SELECT '51', '11', '3', '15' UNION ALL
SELECT '52', '11', '4', '27' UNION ALL
SELECT '53', '11', '5', '27' UNION ALL
SELECT '54', '11', '6', '15' UNION ALL
SELECT '55', '11', '7', '5' UNION ALL
SELECT '56', '11', '8', '15'
COMMIT;
RAISERROR (N'[dbo].[RestorationTypeCosts]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[RestorationTypeCosts] OFF;

-- operating center asset types
INSERT INTO [OperatingCenterAssetTypes] Values(11, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 4)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 5)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 6)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 7)

-- update to new descriptions
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  3/4" FIP X FIP' WHERE [MaterialID] = 8;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 183;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 184;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 185;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 187;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X  6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 190;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 191;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X  6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 192;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 16" X   3/4" SRV PSC' WHERE [MaterialID] = 194;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 20" X   3/4" SRV PSC' WHERE [MaterialID] = 201;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 24" X   3/4" SRV PSC' WHERE [MaterialID] = 208;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 30" X   3/4" SRV PSC' WHERE [MaterialID] = 215;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 36" X   3/4" SRV PSC' WHERE [MaterialID] = 223;
UPDATE [Materials] SET [Description] = 'Saddle, Tap  2 1/4" X  3/4" SRV' WHERE [MaterialID] = 231;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" CF X CF 3PT UN' WHERE [MaterialID] = 521;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" TNA X CC Swivel 90°' WHERE [MaterialID] = 533;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" Meter  (inside horn)' WHERE [MaterialID] = 548;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X  3/4" Meter  (inside horn)' WHERE [MaterialID] = 549;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X 1" K style X CC' WHERE [MaterialID] = 553;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X  1/2" K style X CC' WHERE [MaterialID] = 554;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X  3/4" K style X CC' WHERE [MaterialID] = 555;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X 1" K style X IPC' WHERE [MaterialID] = 557;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X 1" K style X PC' WHERE [MaterialID] = 560;
UPDATE [Materials] SET [Description] = 'Valve,   5/8" X  3/4" Yoke Angle MC X CC' WHERE [MaterialID] = 564;
UPDATE [Materials] SET [Description] = 'Flange,  1 1/2" Meter Adapt X CC' WHERE [MaterialID] = 567;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 620;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  6" Tap FAB SS CI/DI' WHERE [MaterialID] = 621;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 622;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 623;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 624;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 16" Tap FAB FLG CI/DI' WHERE [MaterialID] = 625;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 8" Tap FAB SS CI/DI' WHERE [MaterialID] = 626;
UPDATE [Materials] SET [Description] = 'Bend,  4" PVC Sewer Gask 22 1/2°' WHERE [MaterialID] = 653;
UPDATE [Materials] SET [Description] = 'Bend,  6" PVC Sewer Gask 22 1/2°' WHERE [MaterialID] = 654;
UPDATE [Materials] SET [Description] = 'Adapter,  4" PVC Sewer Screw/Plug' WHERE [MaterialID] = 658;
UPDATE [Materials] SET [Description] = 'Adapter,  6" PVC Sewer Screw/Plug' WHERE [MaterialID] = 659;
UPDATE [Materials] SET [Description] = 'Coupling,  4" Fernco Clay X CI/PVC' WHERE [MaterialID] = 672;
UPDATE [Materials] SET [Description] = 'Coupling,  8" Fernco Clay X CI/PVC' WHERE [MaterialID] = 673;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  4"  Fernco ACXCI/PVC' WHERE [MaterialID] = 674;
UPDATE [Materials] SET [Description] = 'Bend,  8" PVC Sewer Gask 22 1/2°' WHERE [MaterialID] = 700;
UPDATE [Materials] SET [Description] = 'Bend,  4" PVC Sewer Glue 22 1/2°' WHERE [MaterialID] = 701;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  4" PVC Sewer SJ' WHERE [MaterialID] = 713;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  4" Fernco Sewer' WHERE [MaterialID] = 719;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  6" Fernco Sewer' WHERE [MaterialID] = 720;
UPDATE [Materials] SET [Description] = 'Coupling,  4" X  6" Fernco Clay X CI/PVC' WHERE [MaterialID] = 722;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  4" Fernco Clay X CI/PVC' WHERE [MaterialID] = 723;
UPDATE [Materials] SET [Description] = 'Coupling,  6" Fernco Clay X CI/PVC' WHERE [MaterialID] = 724;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  4" Fernco Clay X CI/PVC' WHERE [MaterialID] = 725;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  6" Fernco Clay X CI/PVC' WHERE [MaterialID] = 726;
UPDATE [Materials] SET [Description] = 'Coupling,  8" Fernco Clay X CI/PVC Shear' WHERE [MaterialID] = 727;
UPDATE [Materials] SET [Description] = 'Coupling, 10" Fernco Clay X CI/PVC' WHERE [MaterialID] = 728;
UPDATE [Materials] SET [Description] = 'Coupling, 10" Fernco Clay X CI/PVC Shear' WHERE [MaterialID] = 729;
UPDATE [Materials] SET [Description] = 'Coupling, 12" Fernco Clay X CI/PVC' WHERE [MaterialID] = 730;
UPDATE [Materials] SET [Description] = 'Coupling, 12" Fernco Clay X CI/PVC Shear' WHERE [MaterialID] = 731;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  4" Fernco ACXCI/PVC' WHERE [MaterialID] = 734;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  6" Fernco ACXCI/PVC' WHERE [MaterialID] = 735;
UPDATE [Materials] SET [Description] = 'Coupling,  8" Fernco ACXCI/PVC Shear' WHERE [MaterialID] = 736;
UPDATE [Materials] SET [Description] = 'Coupling, 12" Fernco ACXCI/PVC Shear' WHERE [MaterialID] = 739;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  4" Fernco CI/PVC' WHERE [MaterialID] = 740;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  4" Fernco CI/PVC' WHERE [MaterialID] = 742;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  6" Fernco CI/PVC' WHERE [MaterialID] = 743;
UPDATE [Materials] SET [Description] = 'Coupling,  8" Fernco CI/PVC Shear' WHERE [MaterialID] = 744;
UPDATE [Materials] SET [Description] = 'Coupling, 10" Fernco CI/PVC Shear' WHERE [MaterialID] = 746;
UPDATE [Materials] SET [Description] = 'Coupling, 12" Fernco CI/PVC Shear' WHERE [MaterialID] = 748;
UPDATE [Materials] SET [Description] = 'Valve,  2" Ball Meter FIP X FLG' WHERE [MaterialID] = 795;
UPDATE [Materials] SET [Description] = 'Valve,  1" X 3/4" Ball Meter FIP X MC' WHERE [MaterialID] = 798;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" Ball Meter FIP X MC' WHERE [MaterialID] = 800;
UPDATE [Materials] SET [Description] = 'Valve,  1 1/2" Ball Meter FIP X MC' WHERE [MaterialID] = 801;
UPDATE [Materials] SET [Description] = 'Valve,   1/2" BR Gate FIP X FIP' WHERE [MaterialID] = 805;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" BR Gate FIP X FIP' WHERE [MaterialID] = 806;
UPDATE [Materials] SET [Description] = 'Valve,  1 1/4" BR Gate FIP X FIP' WHERE [MaterialID] = 807;
UPDATE [Materials] SET [Description] = 'Valve,  1 1/2" BR Gate FIP X FIP' WHERE [MaterialID] = 808;
UPDATE [Materials] SET [Description] = 'Valve,  2 1/2" BR Gate FIP X FIP' WHERE [MaterialID] = 809;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/2" PC X FIP' WHERE [MaterialID] = 813;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1" X  3/4" CF X MC' WHERE [MaterialID] = 819;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1" CF X CF 360°' WHERE [MaterialID] = 821;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 2" CF X CF Minn' WHERE [MaterialID] = 822;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  3/4" CF X CF 360°' WHERE [MaterialID] = 823;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1" CC X CC 360°' WHERE [MaterialID] = 826;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1" CC X CC Minn' WHERE [MaterialID] = 827;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1" X  1/4" CC X CC' WHERE [MaterialID] = 828;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1" X 1 1/4" CC X CC' WHERE [MaterialID] = 829;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 2" X 1 1/4" CC X CC' WHERE [MaterialID] = 831;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 2" FIP X FIP w/Handle' WHERE [MaterialID] = 836;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 2" FIP X FIP w/Drain' WHERE [MaterialID] = 837;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  1/2" FIP X FIP' WHERE [MaterialID] = 838;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  3/4" FIP X FIP w/Drain' WHERE [MaterialID] = 839;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/4" FIP X FIP' WHERE [MaterialID] = 840;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/4" FIP X FIP Minn' WHERE [MaterialID] = 841;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/2" FIP X FIP' WHERE [MaterialID] = 842;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/2" FIP X FIP Minn' WHERE [MaterialID] = 843;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/2" CF X FIP' WHERE [MaterialID] = 845;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  3/4" IPC X IPC' WHERE [MaterialID] = 847;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/2" IPC X IPC' WHERE [MaterialID] = 848;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  3/4" X 1" CC X FIP' WHERE [MaterialID] = 851;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop,  3/4" CC X FIP w/Drain' WHERE [MaterialID] = 853;
UPDATE [Materials] SET [Description] = 'Ball/Curb Stop, 1 1/2" CC X FIP' WHERE [MaterialID] = 854;
UPDATE [Materials] SET [Description] = 'Valve,  1" X 3/4" Angle Meter CC' WHERE [MaterialID] = 856;
UPDATE [Materials] SET [Description] = 'Valve,   5/8" X 1" Angle Meter CC' WHERE [MaterialID] = 858;
UPDATE [Materials] SET [Description] = 'Valve,   5/8" X  3/4" Angle Meter CC' WHERE [MaterialID] = 859;
UPDATE [Materials] SET [Description] = 'Valve,  1" X 3/4" Angle Meter IPC' WHERE [MaterialID] = 863;
UPDATE [Materials] SET [Description] = 'Valve,  1" X 3/4" Angle Meter FIP' WHERE [MaterialID] = 867;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" X  5/8" Angle Meter FIP' WHERE [MaterialID] = 869;
UPDATE [Materials] SET [Description] = 'Valve,  1" X 3/4" Angle Meter CF' WHERE [MaterialID] = 872;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" FLG Press Reduction' WHERE [MaterialID] = 877;
UPDATE [Materials] SET [Description] = 'Valve,  2" Check 1/4" & 1/8" ports' WHERE [MaterialID] = 880;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" IPC X CC' WHERE [MaterialID] = 897;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" X 5" IPC X CC' WHERE [MaterialID] = 900;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" Angle Meter PVCC X PC' WHERE [MaterialID] = 901;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X MIP' WHERE [MaterialID] = 904;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" PC X MIP' WHERE [MaterialID] = 905;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" X 1" PC X MIP' WHERE [MaterialID] = 906;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X FIP' WHERE [MaterialID] = 911;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" PC X FIP' WHERE [MaterialID] = 912;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X PC' WHERE [MaterialID] = 914;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X PC' WHERE [MaterialID] = 915;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" PC X PC' WHERE [MaterialID] = 916;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  3/4" MC X CC' WHERE [MaterialID] = 922;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X CF' WHERE [MaterialID] = 926;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" PC X CF' WHERE [MaterialID] = 927;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X IPC' WHERE [MaterialID] = 934;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" PC X IPC' WHERE [MaterialID] = 935;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" PC X CC' WHERE [MaterialID] = 943;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" PC X CC' WHERE [MaterialID] = 944;
UPDATE [Materials] SET [Description] = 'Coupling,  1" X 1 1/4" CC X FIP' WHERE [MaterialID] = 949;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" CC X FIP' WHERE [MaterialID] = 951;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" CC X FIP' WHERE [MaterialID] = 952;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CC X FIP' WHERE [MaterialID] = 954;
UPDATE [Materials] SET [Description] = 'Coupling,  2" X 1 1/2" CC X MIP' WHERE [MaterialID] = 959;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" CC X MIP' WHERE [MaterialID] = 961;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" CC X MIP' WHERE [MaterialID] = 962;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CC X MIP' WHERE [MaterialID] = 965;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" X 1" CC X MIP' WHERE [MaterialID] = 967;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" X 1" CC X MIP' WHERE [MaterialID] = 969;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CF X CF 3PT UN' WHERE [MaterialID] = 972;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" CF X CF 2 Part Union' WHERE [MaterialID] = 973;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" TNA X CC' WHERE [MaterialID] = 975;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  3/4" TNA X CC' WHERE [MaterialID] = 977;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  3/4" TNA X CC' WHERE [MaterialID] = 978;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CF X FIP' WHERE [MaterialID] = 985;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CF X MIP' WHERE [MaterialID] = 992;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CF X CF' WHERE [MaterialID] = 998;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" CF X LFA' WHERE [MaterialID] = 1006;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" CF X LFA' WHERE [MaterialID] = 1008;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" CF X LC' WHERE [MaterialID] = 1014;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CF X CF 2PT UN' WHERE [MaterialID] = 1028;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" CF X CF 2PT UN' WHERE [MaterialID] = 1029;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" CF X CF 2PT UN' WHERE [MaterialID] = 1030;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" CF X CF 2PT UN' WHERE [MaterialID] = 1031;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  5/8" MIP X LC' WHERE [MaterialID] = 1034;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" CF X CC' WHERE [MaterialID] = 1053;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" CF X CC' WHERE [MaterialID] = 1054;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 1 1/4" CF X CC' WHERE [MaterialID] = 1057;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" LC X CC' WHERE [MaterialID] = 1061;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 1 1/2" LC X CC' WHERE [MaterialID] = 1063;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" X 1 1/2" LC X CC' WHERE [MaterialID] = 1064;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" X 1" IPC X IPC' WHERE [MaterialID] = 1085;
UPDATE [Materials] SET [Description] = 'Coupling,  1" X 1 1/2" MC X MIP' WHERE [MaterialID] = 1096;
UPDATE [Materials] SET [Description] = 'Coupling,  1" X 2 1/2" MC X MIP' WHERE [MaterialID] = 1097;
UPDATE [Materials] SET [Description] = 'Coupling,  1" X 2 5/8" MC X MIP' WHERE [MaterialID] = 1098;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" MC X MIP' WHERE [MaterialID] = 1100;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  1/2" MC X MIP' WHERE [MaterialID] = 1101;
UPDATE [Materials] SET [Description] = 'Coupling,   5/8" X  3/4" MC X MIP' WHERE [MaterialID] = 1103;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 1 1/2" MC X MIP' WHERE [MaterialID] = 1107;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 1 5/8" MC X MIP' WHERE [MaterialID] = 1108;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 2 1/4" MC X MIP' WHERE [MaterialID] = 1109;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 2 1/2" MC X MIP' WHERE [MaterialID] = 1110;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 2 5/8" MC X MIP' WHERE [MaterialID] = 1111;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 2 3/16" MC X MIP' WHERE [MaterialID] = 1112;
UPDATE [Materials] SET [Description] = 'Bend,   1/2" X  3/4" Swivel TNA X CF 45°' WHERE [MaterialID] = 1115;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" Swivel TNA X CF 45°' WHERE [MaterialID] = 1116;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" CC X CC' WHERE [MaterialID] = 1119;
UPDATE [Materials] SET [Description] = 'Coupling,   1/2" X 3/4" CC X CC' WHERE [MaterialID] = 1120;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X  1/2" CC X CC' WHERE [MaterialID] = 1121;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" X 1" BR FIP X FIP' WHERE [MaterialID] = 1128;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" GALV FIP X FIP' WHERE [MaterialID] = 1136;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" GALV FIP X FIP' WHERE [MaterialID] = 1137;
UPDATE [Materials] SET [Description] = 'Coupling,  2 1/2" GALV FIP X FIP' WHERE [MaterialID] = 1138;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" TNA X CF 90°' WHERE [MaterialID] = 1152;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" TNA X CF 45°' WHERE [MaterialID] = 1157;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" TNA X CF 45°' WHERE [MaterialID] = 1160;
UPDATE [Materials] SET [Description] = 'Bend,   1/2" X  3/4" Corp Swivel TNA X CF 90°' WHERE [MaterialID] = 1164;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" Swivel TNA X CF 90°' WHERE [MaterialID] = 1165;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" Swivel TNA X FIP 90°' WHERE [MaterialID] = 1166;
UPDATE [Materials] SET [Description] = 'YBR, 1 1/2" X 1 1/2" CF X CF 2WY' WHERE [MaterialID] = 1182;
UPDATE [Materials] SET [Description] = 'YBR, 1 1/2" X 1 1/2" CF X CF 3WY' WHERE [MaterialID] = 1187;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X 1" CF for Iron Yoke' WHERE [MaterialID] = 1342;
UPDATE [Materials] SET [Description] = 'Plug,  1" Insert BR Taper Thread' WHERE [MaterialID] = 1391;
UPDATE [Materials] SET [Description] = 'Plug,   5/8" Insert BR Taper Thread' WHERE [MaterialID] = 1392;
UPDATE [Materials] SET [Description] = 'Plug,   3/4" Insert BR Taper Thread' WHERE [MaterialID] = 1393;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CF X IPC 90°' WHERE [MaterialID] = 1405;
UPDATE [Materials] SET [Description] = 'Bend,   1/2" X  1/2" BR FIP 90°' WHERE [MaterialID] = 1412;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  1/4" BR FIP X FIP 90°' WHERE [MaterialID] = 1413;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  1/2" BR FIP 90°' WHERE [MaterialID] = 1414;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" BR FIP 90°' WHERE [MaterialID] = 1415;
UPDATE [Materials] SET [Description] = 'Bend,  2 1/2" X  1/4" BR FIP 90°' WHERE [MaterialID] = 1419;
UPDATE [Materials] SET [Description] = 'Bend,   1/2" X  1/2" BR FIP 45°' WHERE [MaterialID] = 1423;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" BR FIP 45°' WHERE [MaterialID] = 1424;
UPDATE [Materials] SET [Description] = 'Bend,   1/2" X  1/2" BR FIP X MIP 90°' WHERE [MaterialID] = 1429;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" BR FIP X MIP 90°' WHERE [MaterialID] = 1430;
UPDATE [Materials] SET [Description] = 'Bend,  1" X  3/4" BR PC X CC 90°' WHERE [MaterialID] = 1437;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" MIP X CC 90°' WHERE [MaterialID] = 1442;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" MIP X IPC 90°' WHERE [MaterialID] = 1443;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CC X CC 90°' WHERE [MaterialID] = 1447;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CF X MIP 90°' WHERE [MaterialID] = 1450;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CF X MIP 45°' WHERE [MaterialID] = 1453;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CF X CF 90°' WHERE [MaterialID] = 1456;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CF X FIP 90°' WHERE [MaterialID] = 1472;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" LFA X CF 45°' WHERE [MaterialID] = 1476;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  5/8" LFA X CF 45°' WHERE [MaterialID] = 1477;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" LFA X CF 45°' WHERE [MaterialID] = 1478;
UPDATE [Materials] SET [Description] = 'Tee,  1" X 1" X 1 1/4" BR CC X CC' WHERE [MaterialID] = 1501;
UPDATE [Materials] SET [Description] = 'Tee,   3/4" X 1" BR MIP X MIP X CF' WHERE [MaterialID] = 1513;
UPDATE [Materials] SET [Description] = 'Bend,  1" X  3/4" BR CC X CF 90°' WHERE [MaterialID] = 1523;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" BR CC X CF 90°' WHERE [MaterialID] = 1526;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X 1" BR MC X PC 90°' WHERE [MaterialID] = 1527;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CC X FIP 90°' WHERE [MaterialID] = 1530;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" PC X MIP 90°' WHERE [MaterialID] = 1532;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CC X FIP 45°' WHERE [MaterialID] = 1533;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" CF X FIP 45°' WHERE [MaterialID] = 1535;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CF X FIP 45°' WHERE [MaterialID] = 1536;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" LFA X CF 90°' WHERE [MaterialID] = 1538;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" LFA X CF 90°' WHERE [MaterialID] = 1539;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   1/2" X  4" H' WHERE [MaterialID] = 1567;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   1/2" X 12" H' WHERE [MaterialID] = 1568;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   1/2" X 18" H' WHERE [MaterialID] = 1569;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   1/2" X 24" H' WHERE [MaterialID] = 1570;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X  4" H' WHERE [MaterialID] = 1571;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X  7" H' WHERE [MaterialID] = 1572;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X  9" H' WHERE [MaterialID] = 1573;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X 12" H' WHERE [MaterialID] = 1574;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X 15" H' WHERE [MaterialID] = 1575;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X 18" H' WHERE [MaterialID] = 1576;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X 24" H' WHERE [MaterialID] = 1577;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X 30" H' WHERE [MaterialID] = 1578;
UPDATE [Materials] SET [Description] = 'Resetter,  5/8" X   3/4" X 36" H' WHERE [MaterialID] = 1579;
UPDATE [Materials] SET [Description] = 'Setter,  3/4" Meter  (inside horn)' WHERE [MaterialID] = 1586;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X  3/4" K style X IPC' WHERE [MaterialID] = 1591;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X  3/4" CC Vertical' WHERE [MaterialID] = 1603;
UPDATE [Materials] SET [Description] = 'Setter,  5/8" X  3/4" FIP X FIP' WHERE [MaterialID] = 1612;
UPDATE [Materials] SET [Description] = 'Wheel,  5/8" X 3/4"  Expansion Setter' WHERE [MaterialID] = 1625;
UPDATE [Materials] SET [Description] = 'Bend,  1" Yoke Coupling MC X CC' WHERE [MaterialID] = 1640;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" Yoke Coupling MC X CC' WHERE [MaterialID] = 1641;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" Yoke Coupling MC X CC' WHERE [MaterialID] = 1642;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X 1" Yoke Coupling MC X CC' WHERE [MaterialID] = 1643;
UPDATE [Materials] SET [Description] = 'Bend,  1" Yoke Coupling MC X FIP' WHERE [MaterialID] = 1656;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" Yoke Coupling MC X FIP' WHERE [MaterialID] = 1657;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" X 1" Yoke Angle MC X CC' WHERE [MaterialID] = 1658;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" Yoke Angle MC X CC' WHERE [MaterialID] = 1659;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" X 1" Yoke Angle MC X FIP' WHERE [MaterialID] = 1661;
UPDATE [Materials] SET [Description] = 'Valve,   3/4" Yoke Angle MC X FIP' WHERE [MaterialID] = 1662;
UPDATE [Materials] SET [Description] = 'Yoke,  5/8" X 3/4" Dual Setting' WHERE [MaterialID] = 1663;
UPDATE [Materials] SET [Description] = 'Box, Meter 12" X 20" Poly Composite' WHERE [MaterialID] = 1668;
UPDATE [Materials] SET [Description] = 'Box, Meter 13" X 24" Poly Composite' WHERE [MaterialID] = 1669;
UPDATE [Materials] SET [Description] = 'Box, Meter 17" X 28" Poly Composite' WHERE [MaterialID] = 1670;
UPDATE [Materials] SET [Description] = 'Box, Meter 17" X 30" Poly Composite' WHERE [MaterialID] = 1671;
UPDATE [Materials] SET [Description] = 'Box, Meter Single W/Setting 18" X 1"' WHERE [MaterialID] = 1672;
UPDATE [Materials] SET [Description] = 'Box, Meter Single W/Setting 18" X  5/8" X 3/4"' WHERE [MaterialID] = 1673;
UPDATE [Materials] SET [Description] = 'Lid, 14" X 14" Meter Fabricated' WHERE [MaterialID] = 1675;
UPDATE [Materials] SET [Description] = 'Lid, 14" X 22" Meter Fabricated' WHERE [MaterialID] = 1676;
UPDATE [Materials] SET [Description] = 'Lid, 24" X 42" Meter Fabricated' WHERE [MaterialID] = 1678;
UPDATE [Materials] SET [Description] = 'Flange,   5/8" X 3/4" Meter FIP' WHERE [MaterialID] = 1685;
UPDATE [Materials] SET [Description] = 'Flange,  1 1/2" Meter Adapt X IPC' WHERE [MaterialID] = 1693;
UPDATE [Materials] SET [Description] = 'Box, Meter Dual Thermal W/Setting 18" X 5/8" X 3/4"' WHERE [MaterialID] = 1694;
UPDATE [Materials] SET [Description] = 'Box w/lid, Meter 12" X 17" Single' WHERE [MaterialID] = 1734;
UPDATE [Materials] SET [Description] = 'Box w/lid, Meter 12" X 18" Single' WHERE [MaterialID] = 1735;
UPDATE [Materials] SET [Description] = 'Box w/lid, Meter 16" X 22" Single' WHERE [MaterialID] = 1736;
UPDATE [Materials] SET [Description] = 'Ring, 20" X 24" Extension Light' WHERE [MaterialID] = 1746;
UPDATE [Materials] SET [Description] = 'Ring, 20" X 30" Extension Light' WHERE [MaterialID] = 1747;
UPDATE [Materials] SET [Description] = 'Ring, 20" X 36" Extension Light' WHERE [MaterialID] = 1748;
UPDATE [Materials] SET [Description] = 'Ring, 20" X 36" Extension Light' WHERE [MaterialID] = 1749;
UPDATE [Materials] SET [Description] = 'Ring, 30" X 20" Extension Light' WHERE [MaterialID] = 1750;
UPDATE [Materials] SET [Description] = 'Ring, 36" X 18" Extension Light' WHERE [MaterialID] = 1751;
UPDATE [Materials] SET [Description] = 'Lid, 11 1/2" Meter for F/L Non locking' WHERE [MaterialID] = 1765;
UPDATE [Materials] SET [Description] = 'Lid, 20" Meter for F/L Non Locking' WHERE [MaterialID] = 1767;
UPDATE [Materials] SET [Description] = 'Curb Box,  2 1/2" Complete PL SL' WHERE [MaterialID] = 1774;
UPDATE [Materials] SET [Description] = 'Curb Box, 10" X 3" Complete CI SL' WHERE [MaterialID] = 1778;
UPDATE [Materials] SET [Description] = 'Curb Box,  1 1/4" Complete CI SL' WHERE [MaterialID] = 1779;
UPDATE [Materials] SET [Description] = 'Curb Box,  1 1/2" X 2" Complete CI SL' WHERE [MaterialID] = 1780;
UPDATE [Materials] SET [Description] = 'Curb Box,  2 1/2" Complete CI SL' WHERE [MaterialID] = 1781;
UPDATE [Materials] SET [Description] = 'Extension, 2 1/2" Curb Box PL SL' WHERE [MaterialID] = 1787;
UPDATE [Materials] SET [Description] = 'Curb Box,  1 1/2" CI SL Minn. Thread' WHERE [MaterialID] = 1789;
UPDATE [Materials] SET [Description] = 'Lid, Curb Box (Edison) 2 hole Lid 1" Upper Section' WHERE [MaterialID] = 1791;
UPDATE [Materials] SET [Description] = 'Curb Box,  2 1/2" Complete PL Screw' WHERE [MaterialID] = 1797;
UPDATE [Materials] SET [Description] = 'Curb Box,  2 1/2" Complete CI Screw' WHERE [MaterialID] = 1800;
UPDATE [Materials] SET [Description] = 'Extension, 2 1/2" Curb CI Screw' WHERE [MaterialID] = 1801;
UPDATE [Materials] SET [Description] = 'Box Valve, Top 5 1/4" x 10" CI SL' WHERE [MaterialID] = 1806;
UPDATE [Materials] SET [Description] = 'Box, 5 1/4" X 48" Valve Complete PL SL' WHERE [MaterialID] = 1807;
UPDATE [Materials] SET [Description] = 'Box, Valve Complete CI 22 Screw 39"-60"' WHERE [MaterialID] = 1810;
UPDATE [Materials] SET [Description] = 'Box, Valve Complete 4 1/4" CI Screw' WHERE [MaterialID] = 1811;
UPDATE [Materials] SET [Description] = 'Box, Valve Complete 5 1/4" CI Screw' WHERE [MaterialID] = 1812;
UPDATE [Materials] SET [Description] = 'Box, Valve  6" X 12" Top CI Screw' WHERE [MaterialID] = 1813;
UPDATE [Materials] SET [Description] = 'Box, Valve 10" X 12" Top CI Screw' WHERE [MaterialID] = 1814;
UPDATE [Materials] SET [Description] = 'Box, Valve 12" X 12" Top CI Screw' WHERE [MaterialID] = 1815;
UPDATE [Materials] SET [Description] = 'Box, Valve  2 1/4" X 24" Valve Complete PL SL' WHERE [MaterialID] = 1816;
UPDATE [Materials] SET [Description] = 'Box, Valve  5 1/4" X 16" Valve Complete PL SL' WHERE [MaterialID] = 1818;
UPDATE [Materials] SET [Description] = 'Box, Valve  5 1/4" X 24" Top CI Screw' WHERE [MaterialID] = 1819;
UPDATE [Materials] SET [Description] = 'Box, Valve  5 1/4" X 36" Top CI Screw' WHERE [MaterialID] = 1820;
UPDATE [Materials] SET [Description] = 'Box, Valve  5 1/4" Top CI Screw' WHERE [MaterialID] = 1821;
UPDATE [Materials] SET [Description] = 'Box, Valve Top 5 1/4" CI Screw with lid' WHERE [MaterialID] = 1822;
UPDATE [Materials] SET [Description] = 'Box, 5 1/4" X 13" Valve Bottom CI Screw' WHERE [MaterialID] = 1824;
UPDATE [Materials] SET [Description] = 'Box, 5 1/4" X 24" Valve Bottom CI Screw' WHERE [MaterialID] = 1825;
UPDATE [Materials] SET [Description] = 'Box, 5 1/4" X 36" Valve Bottom CI Screw' WHERE [MaterialID] = 1826;
UPDATE [Materials] SET [Description] = 'Extension, 5 1/4" Valve Box PL SL' WHERE [MaterialID] = 1827;
UPDATE [Materials] SET [Description] = 'Extension, 4 1/4" X 24" Valve Box CI SL' WHERE [MaterialID] = 1828;
UPDATE [Materials] SET [Description] = 'Riser, Valve Box Screw  6" X  3/4"' WHERE [MaterialID] = 1830;
UPDATE [Materials] SET [Description] = 'Riser, Valve Box Screw  5 1/4" X  1"' WHERE [MaterialID] = 1833;
UPDATE [Materials] SET [Description] = 'Riser, Valve Box Screw  5 1/4" X  2"' WHERE [MaterialID] = 1834;
UPDATE [Materials] SET [Description] = 'Riser, Valve Box Screw  5 1/4" X  6"' WHERE [MaterialID] = 1835;
UPDATE [Materials] SET [Description] = 'Riser, Valve Box Screw  5 1/4" X  8"' WHERE [MaterialID] = 1836;
UPDATE [Materials] SET [Description] = 'Riser, Valve Box Screw  5 1/4" X 12"' WHERE [MaterialID] = 1837;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X  1" Valve Box SL' WHERE [MaterialID] = 1839;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X  2" Valve Box SL' WHERE [MaterialID] = 1840;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X  8" Valve Box SL' WHERE [MaterialID] = 1841;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X 10" Valve Box SL' WHERE [MaterialID] = 1842;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X 12" Valve Box SL' WHERE [MaterialID] = 1843;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X  1 1/2" Valve Box SL' WHERE [MaterialID] = 1844;
UPDATE [Materials] SET [Description] = 'Riser,  5 1/4" X 24" Valve Box SL' WHERE [MaterialID] = 1845;
UPDATE [Materials] SET [Description] = 'Box, 6" X 30" Valve Bottom Split GALV' WHERE [MaterialID] = 1856;
UPDATE [Materials] SET [Description] = 'Box, 5 1/4" X 18" Middle CI Screw' WHERE [MaterialID] = 1857;
UPDATE [Materials] SET [Description] = 'Box, 5 1/4" X 24" Middle CI Screw' WHERE [MaterialID] = 1858;
UPDATE [Materials] SET [Description] = 'Extension, 4 1/4" X 18" Valve Box CI Screw' WHERE [MaterialID] = 1859;
UPDATE [Materials] SET [Description] = 'Extension, 4 1/4" X 24" Valve Box CI Screw' WHERE [MaterialID] = 1860;
UPDATE [Materials] SET [Description] = 'Extension, 5 1/4" X 12" Valve Box CI Screw' WHERE [MaterialID] = 1861;
UPDATE [Materials] SET [Description] = 'Extension, 5 1/4" X 18" Valve Box CI Screw' WHERE [MaterialID] = 1862;
UPDATE [Materials] SET [Description] = 'Extension, 5 1/4" X 24" Valve Box CI Screw' WHERE [MaterialID] = 1863;
UPDATE [Materials] SET [Description] = 'Box, 6" X 30" Valve Bottom  GALV' WHERE [MaterialID] = 1867;
UPDATE [Materials] SET [Description] = 'Box, 8" X 30" Valve Bottom  GALV' WHERE [MaterialID] = 1868;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  3" DI X AC' WHERE [MaterialID] = 1870;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  4" DI X AC' WHERE [MaterialID] = 1871;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  6" DI X AC' WHERE [MaterialID] = 1872;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  8" X 6" DI X AC' WHERE [MaterialID] = 1873;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  8" DI X AC' WHERE [MaterialID] = 1874;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 10" DI X AC' WHERE [MaterialID] = 1875;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 12" DI X AC' WHERE [MaterialID] = 1876;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 14" DI X AC' WHERE [MaterialID] = 1877;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 16" DI X AC' WHERE [MaterialID] = 1878;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 18" DI X AC' WHERE [MaterialID] = 1879;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 20" DI X AC' WHERE [MaterialID] = 1880;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 24" DI X AC' WHERE [MaterialID] = 1881;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  3" DI X PVC' WHERE [MaterialID] = 1882;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  4" DI X PVC' WHERE [MaterialID] = 1883;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  6" X 4" DI X PVC' WHERE [MaterialID] = 1884;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  6" DI X PVC' WHERE [MaterialID] = 1885;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  8" DI X PVC' WHERE [MaterialID] = 1886;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 10" DI X PVC' WHERE [MaterialID] = 1887;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 12" DI X PVC' WHERE [MaterialID] = 1888;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 16" DI X PVC' WHERE [MaterialID] = 1889;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 20" DI X PVC' WHERE [MaterialID] = 1890;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  4" DI X CI' WHERE [MaterialID] = 1891;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  6" DI X CI' WHERE [MaterialID] = 1892;
UPDATE [Materials] SET [Description] = 'Coupling, Transition  8" DI X CI' WHERE [MaterialID] = 1893;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 10" DI X CI' WHERE [MaterialID] = 1894;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 12" DI X CI' WHERE [MaterialID] = 1895;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 14" DI X CI' WHERE [MaterialID] = 1896;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 16" DI X CI' WHERE [MaterialID] = 1897;
UPDATE [Materials] SET [Description] = 'Coupling, Transition 24" DI X CI' WHERE [MaterialID] = 1898;
UPDATE [Materials] SET [Description] = 'Coupling,  1" FLEX Bolted ST LP' WHERE [MaterialID] = 1899;
UPDATE [Materials] SET [Description] = 'Coupling,  6" FLEX Bolted DI LP' WHERE [MaterialID] = 1900;
UPDATE [Materials] SET [Description] = 'Coupling,  2" X 2 1/4" FLEX Bolted DI' WHERE [MaterialID] = 1902;
UPDATE [Materials] SET [Description] = 'Coupling,  4" X  3" FLEX Bolted DI' WHERE [MaterialID] = 1904;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  4" FLEX Bolted DI' WHERE [MaterialID] = 1906;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  6" FLEX Bolted DI' WHERE [MaterialID] = 1907;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" FLEX Bolted DI' WHERE [MaterialID] = 1913;
UPDATE [Materials] SET [Description] = 'Coupling,  2 1/4" FLEX Bolted DI' WHERE [MaterialID] = 1916;
UPDATE [Materials] SET [Description] = 'Coupling,  2 1/2" FLEX Bolted DI' WHERE [MaterialID] = 1917;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  6" FLEX Bolted AC' WHERE [MaterialID] = 1926;
UPDATE [Materials] SET [Description] = 'Coupling,  1" FLEX Bolted ST SP' WHERE [MaterialID] = 1933;
UPDATE [Materials] SET [Description] = 'Coupling,  2" FLEX Bolted ST SP' WHERE [MaterialID] = 1934;
UPDATE [Materials] SET [Description] = 'Coupling,  3" FLEX Bolted ST SP' WHERE [MaterialID] = 1935;
UPDATE [Materials] SET [Description] = 'Coupling,  4" FLEX Bolted ST SP' WHERE [MaterialID] = 1936;
UPDATE [Materials] SET [Description] = 'Coupling,  6" FLEX Bolted ST SP' WHERE [MaterialID] = 1937;
UPDATE [Materials] SET [Description] = 'Coupling,  8" FLEX Bolted ST SP' WHERE [MaterialID] = 1938;
UPDATE [Materials] SET [Description] = 'Coupling, 10" FLEX Bolted ST SP' WHERE [MaterialID] = 1939;
UPDATE [Materials] SET [Description] = 'Coupling, 12" FLEX Bolted ST SP' WHERE [MaterialID] = 1940;
UPDATE [Materials] SET [Description] = 'Coupling, 16" FLEX Bolted ST SP' WHERE [MaterialID] = 1941;
UPDATE [Materials] SET [Description] = 'Coupling, 20" FLEX Bolted ST SP' WHERE [MaterialID] = 1942;
UPDATE [Materials] SET [Description] = 'Coupling, 24" FLEX Bolted ST SP' WHERE [MaterialID] = 1943;
UPDATE [Materials] SET [Description] = 'Coupling,  4" X  2" Trans DI X IPS' WHERE [MaterialID] = 1952;
UPDATE [Materials] SET [Description] = 'Coupling,  4" X  4" Trans DI X IPS' WHERE [MaterialID] = 1953;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  2" Trans DI X IPS' WHERE [MaterialID] = 1954;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X  2" Trans DI X IPS' WHERE [MaterialID] = 1956;
UPDATE [Materials] SET [Description] = 'Coupling,  2 1/2" Trans DI X IPS' WHERE [MaterialID] = 1963;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" Split Repair Clamp LP' WHERE [MaterialID] = 2100;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" Split Repair Clamp LP' WHERE [MaterialID] = 2101;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" Split Repair Clamp SP' WHERE [MaterialID] = 2102;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" Split Repair Clamp SP' WHERE [MaterialID] = 2103;
UPDATE [Materials] SET [Description] = 'Clamp,  6" X 12" SS FCRC CI/DI 1" Tap' WHERE [MaterialID] = 2562;
UPDATE [Materials] SET [Description] = 'Clamp, 12" X 12" SS FCRC CI 2" Tap' WHERE [MaterialID] = 2576;
UPDATE [Materials] SET [Description] = 'Clamp, 12" X 15" X  3/4" SS FCRC' WHERE [MaterialID] = 2578;
UPDATE [Materials] SET [Description] = 'Clamp,  2 1/4" X  7 1/2" SS FCRC' WHERE [MaterialID] = 2601;
UPDATE [Materials] SET [Description] = 'Clamp,  6" X  8" X 1" SS FCRC TAP' WHERE [MaterialID] = 2608;
UPDATE [Materials] SET [Description] = 'Clamp,  6" X  8" X  3/4" SS FCRC Tap' WHERE [MaterialID] = 2609;
UPDATE [Materials] SET [Description] = 'Clamp,  6" X 12" X 2" SS FCRC TAP' WHERE [MaterialID] = 2610;
UPDATE [Materials] SET [Description] = 'Clamp,  6" X 12" X  3/4" SS FCRC Tap' WHERE [MaterialID] = 2611;
UPDATE [Materials] SET [Description] = 'Clamp,  6" X 12" X 1 1/2" SS FCRC TAP' WHERE [MaterialID] = 2612;
UPDATE [Materials] SET [Description] = 'Clamp,  8" X  8" X 1" SS FCRC TAP' WHERE [MaterialID] = 2613;
UPDATE [Materials] SET [Description] = 'Clamp,  8" X 12" X 1" SS FCRC Tap' WHERE [MaterialID] = 2614;
UPDATE [Materials] SET [Description] = 'Clamp,  8" X 12" X 2" SS FCRC Tap' WHERE [MaterialID] = 2615;
UPDATE [Materials] SET [Description] = 'Clamp,  8" X 12" X  3/4" SS FCRC TAP' WHERE [MaterialID] = 2616;
UPDATE [Materials] SET [Description] = 'Clamp,  8" X 12" X 1 1/2" SS FCRC TAP' WHERE [MaterialID] = 2617;
UPDATE [Materials] SET [Description] = 'Clamp, 12" X 16" X 1" SS FCRC Tap' WHERE [MaterialID] = 2618;
UPDATE [Materials] SET [Description] = 'Wrap,  4" to 8" Plastic for Pipe' WHERE [MaterialID] = 2740;
UPDATE [Materials] SET [Description] = 'Wrap, 10" to 12" Plastic for Pipe' WHERE [MaterialID] = 2741;
UPDATE [Materials] SET [Description] = 'Wrap, 14" to 16" Plastic for Pipe' WHERE [MaterialID] = 2742;
UPDATE [Materials] SET [Description] = 'Wrap, 16" to 24" Plastic for Pipe' WHERE [MaterialID] = 2743;
UPDATE [Materials] SET [Description] = 'Wrap, 18" to 24" Plastic for Pipe' WHERE [MaterialID] = 2744;
UPDATE [Materials] SET [Description] = 'Wrap, 30" to 36" Plastic for Pipe' WHERE [MaterialID] = 2745;
UPDATE [Materials] SET [Description] = 'Wrap, 30" to 36" Plastic for Pipe' WHERE [MaterialID] = 2746;
UPDATE [Materials] SET [Description] = 'Wrap, 42" to 48" Plastic for Pipe' WHERE [MaterialID] = 2747;
UPDATE [Materials] SET [Description] = 'Hydrant, Rose Mead Steamer Head' WHERE [MaterialID] = 2845;
UPDATE [Materials] SET [Description] = 'Elbow, HYD Bury 6" X 42" 8-hole' WHERE [MaterialID] = 2847;
UPDATE [Materials] SET [Description] = 'Extension, HYD Spool 6" X  6" 8-hole' WHERE [MaterialID] = 2848;
UPDATE [Materials] SET [Description] = 'Extension, HYD Spool 6" X 12" 8-hole' WHERE [MaterialID] = 2849;
UPDATE [Materials] SET [Description] = 'Elbow, HYD Bury 6" X 42" 6-hole' WHERE [MaterialID] = 2850;
UPDATE [Materials] SET [Description] = 'Extension, HYD Spool 6" X  6" 6-hole' WHERE [MaterialID] = 2851;
UPDATE [Materials] SET [Description] = 'Extension, HYD Spool 6" X 12" 6-hole' WHERE [MaterialID] = 2852;
UPDATE [Materials] SET [Description] = 'Extension, HYD Spool 6" X  4" 6-hole' WHERE [MaterialID] = 2853;
UPDATE [Materials] SET [Description] = 'Hydrant, Sacremento WB 1 2 1/2"' WHERE [MaterialID] = 2857;
UPDATE [Materials] SET [Description] = 'Hydrant, Sacremento WB Wharf Head' WHERE [MaterialID] = 2858;
UPDATE [Materials] SET [Description] = 'Hydrant, Sacremento WB 2 1/2" X 4' WHERE [MaterialID] = 2859;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 3'' Bury' WHERE [MaterialID] = 2867;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 3''6" Bury' WHERE [MaterialID] = 2868;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 4'' Bury' WHERE [MaterialID] = 2869;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 4''6" Bury' WHERE [MaterialID] = 2870;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 5'' Bury' WHERE [MaterialID] = 2871;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 5''6" Bury' WHERE [MaterialID] = 2872;
UPDATE [Materials] SET [Description] = 'Hydrant, Champaign Mueller 6'' Bury' WHERE [MaterialID] = 2873;
UPDATE [Materials] SET [Description] = 'Hydrant, Jeffersonville 5 1/4'' Bury' WHERE [MaterialID] = 2911;
UPDATE [Materials] SET [Description] = 'Hydrant, Kokomo 4''6" Bury OL Yellow' WHERE [MaterialID] = 2912;
UPDATE [Materials] SET [Description] = 'Hydrant, Kokomo 3''6" Bury OL Yellow' WHERE [MaterialID] = 2914;
UPDATE [Materials] SET [Description] = 'Hydrant, Kokomo 4'' Bury OL Yellow' WHERE [MaterialID] = 2915;
UPDATE [Materials] SET [Description] = 'Hydrant, West Lafayette 5'' Bury' WHERE [MaterialID] = 2923;
UPDATE [Materials] SET [Description] = 'Hydrant, West Lafayette 4'' Bury' WHERE [MaterialID] = 2924;
UPDATE [Materials] SET [Description] = 'Hydrant, Noblesville 4''6" Bury OL 3Way' WHERE [MaterialID] = 2932;
UPDATE [Materials] SET [Description] = 'Hydrant, Noblesville 5'' Bury OL 3Way' WHERE [MaterialID] = 2933;
UPDATE [Materials] SET [Description] = 'Hydrant, Noblesville 5''6" Bury OL 3Way' WHERE [MaterialID] = 2934;
UPDATE [Materials] SET [Description] = 'Hydrant, Bourbon/Owen Co 3''6" Bury' WHERE [MaterialID] = 2952;
UPDATE [Materials] SET [Description] = 'Hydrant, Georgetown/Clark 3''6" Bury' WHERE [MaterialID] = 2954;
UPDATE [Materials] SET [Description] = 'Hydrant, Michigan Yellow 7'' Bury' WHERE [MaterialID] = 2968;
UPDATE [Materials] SET [Description] = 'Hydrant, St Charles 3''6" Bury MJ' WHERE [MaterialID] = 2994;
UPDATE [Materials] SET [Description] = 'Hydrant, Warren County 3'' Bury MJ' WHERE [MaterialID] = 3013;
UPDATE [Materials] SET [Description] = 'Hydrant, Warren County 3''6" Bury MJ' WHERE [MaterialID] = 3014;
UPDATE [Materials] SET [Description] = 'Hydrant, Warren County 4'' Bury MJ' WHERE [MaterialID] = 3015;
UPDATE [Materials] SET [Description] = 'Hydrant, Warren County 5'' Bury MJ' WHERE [MaterialID] = 3016;
UPDATE [Materials] SET [Description] = 'Hydrant, Warren County 6'' Bury MJ' WHERE [MaterialID] = 3017;
UPDATE [Materials] SET [Description] = 'Hydrant, Warren County 4''6" Bury' WHERE [MaterialID] = 3020;
UPDATE [Materials] SET [Description] = 'Hydrant, Jefferson City 4'' Bury' WHERE [MaterialID] = 3023;
UPDATE [Materials] SET [Description] = 'Hydrant, Jefferson City 4''6" Bury' WHERE [MaterialID] = 3024;
UPDATE [Materials] SET [Description] = 'Hydrant, Jefferson City 5'' Bury' WHERE [MaterialID] = 3025;
UPDATE [Materials] SET [Description] = 'Hydrant, Jefferson City 5''6" Bury' WHERE [MaterialID] = 3026;
UPDATE [Materials] SET [Description] = 'Hydrant, Jefferson City 6'' Bury' WHERE [MaterialID] = 3027;
UPDATE [Materials] SET [Description] = 'Hydrant, Bernardsville 4''6" Bury' WHERE [MaterialID] = 3099;
UPDATE [Materials] SET [Description] = 'Hydrant, Little Falls 4''6" Bury' WHERE [MaterialID] = 3108;
UPDATE [Materials] SET [Description] = 'Hydrant, Clovis 5 1/4" W/3''6" Bury' WHERE [MaterialID] = 3139;
UPDATE [Materials] SET [Description] = 'Hydrant, Clovis 4 1/2" W/3''6" Bury' WHERE [MaterialID] = 3140;
UPDATE [Materials] SET [Description] = 'Hydrant, #1 (No Marking) 4''6" Bury' WHERE [MaterialID] = 3141;
UPDATE [Materials] SET [Description] = 'Hydrant, #2 (Yellow Band) 4''6" Bury' WHERE [MaterialID] = 3142;
UPDATE [Materials] SET [Description] = 'Hydrant, #3 (Green Band) 4''6" Bury' WHERE [MaterialID] = 3143;
UPDATE [Materials] SET [Description] = 'Hydrant, 4''6" Bury (Elizabeth City Specs)' WHERE [MaterialID] = 3144;
UPDATE [Materials] SET [Description] = 'Hydrant, Lawrence County 4'' Bury 2.5" Nozzel' WHERE [MaterialID] = 3154;
UPDATE [Materials] SET [Description] = 'Hydrant, Lawrence County 4.5" Nozzel' WHERE [MaterialID] = 3155;
UPDATE [Materials] SET [Description] = 'Hydrant, Paradise Valley 5 1/4" w/4''6" Bury' WHERE [MaterialID] = 3159;
UPDATE [Materials] SET [Description] = 'Hydrant, Sun City 5 1/4" w/4'' 6" Bury' WHERE [MaterialID] = 3160;
UPDATE [Materials] SET [Description] = 'Hydrant, Mohave 5 1/4" w/4''6" Bury' WHERE [MaterialID] = 3161;
UPDATE [Materials] SET [Description] = 'Hydrant, So Fayette 4''6" Bury OR' WHERE [MaterialID] = 3186;
UPDATE [Materials] SET [Description] = 'Hydrant, Canonsburg 4''6" Bury OL' WHERE [MaterialID] = 3187;
UPDATE [Materials] SET [Description] = 'Hydrant, Elizabeth Twp. 4'' Bury' WHERE [MaterialID] = 3188;
UPDATE [Materials] SET [Description] = 'Hydrant, Elizabeth Twp. 4''6" Bury' WHERE [MaterialID] = 3189;
UPDATE [Materials] SET [Description] = 'Hydrant, Elizabeth Twp. 5'' Bury' WHERE [MaterialID] = 3190;
UPDATE [Materials] SET [Description] = 'Hydrant, East Norriton 3''6" Bury' WHERE [MaterialID] = 3224;
UPDATE [Materials] SET [Description] = 'Hydrant, East Norriton 4''6" Bury' WHERE [MaterialID] = 3226;
UPDATE [Materials] SET [Description] = 'Hydrant, Washington Twp 4'' Bury' WHERE [MaterialID] = 3238;
UPDATE [Materials] SET [Description] = 'Hydrant, Easton Suburban 4''6" Bury' WHERE [MaterialID] = 3250;
UPDATE [Materials] SET [Description] = 'Hydrant, Easton Suburban 5'' Bury' WHERE [MaterialID] = 3251;
UPDATE [Materials] SET [Description] = 'Hydrant, Quick Connect 4''6" Bury' WHERE [MaterialID] = 3262;
UPDATE [Materials] SET [Description] = 'Hydrant, Quick Connect 3''6" Public' WHERE [MaterialID] = 3269;
UPDATE [Materials] SET [Description] = 'Hydrant, Quick Connect 4'' Public' WHERE [MaterialID] = 3270;
UPDATE [Materials] SET [Description] = 'Hydrant, Quick Connect 4''6" Public' WHERE [MaterialID] = 3271;
UPDATE [Materials] SET [Description] = 'Hydrant, Coatesville, Grip 4''6" Bury' WHERE [MaterialID] = 3278;
UPDATE [Materials] SET [Description] = 'Hydrant, Lewisburg STD 4''6" Bury' WHERE [MaterialID] = 3295;
UPDATE [Materials] SET [Description] = 'Hydrant, N Umberland STD 4''6" Bury' WHERE [MaterialID] = 3303;
UPDATE [Materials] SET [Description] = 'Hydrant, Osceola Mills 4''6" Bury' WHERE [MaterialID] = 3306;
UPDATE [Materials] SET [Description] = 'Hydrant, Wilkes Barre 5''6" Bury' WHERE [MaterialID] = 3312;
UPDATE [Materials] SET [Description] = 'Hydrant, Prince George 3''6" Bury' WHERE [MaterialID] = 3327;
UPDATE [Materials] SET [Description] = 'Hydrant, Prince William 4''6" Bury' WHERE [MaterialID] = 3329;
UPDATE [Materials] SET [Description] = 'Hydrant, Webster Springs 4'' Bury' WHERE [MaterialID] = 3332;
UPDATE [Materials] SET [Description] = 'Hydrant, 4''6" Bury OL (Mount Holly)' WHERE [MaterialID] = 3357;
UPDATE [Materials] SET [Description] = 'Extension, 5 1/4" X 24" Hydrant' WHERE [MaterialID] = 3371;
UPDATE [Materials] SET [Description] = 'Bushing, 1" X 1/2" Slip X FIP PVC' WHERE [MaterialID] = 3375;
UPDATE [Materials] SET [Description] = 'Bushing, 1" X 3/4" Slip X FIP PVC' WHERE [MaterialID] = 3376;
UPDATE [Materials] SET [Description] = 'Bushing,  3/4" X 1/4" Slip X FIP PVC' WHERE [MaterialID] = 3377;
UPDATE [Materials] SET [Description] = 'Bushing, 1" X 3/4" PVC Taper X Taper' WHERE [MaterialID] = 3378;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" SPT X CF 45°' WHERE [MaterialID] = 3409;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" SPT X CF 45°' WHERE [MaterialID] = 3411;
UPDATE [Materials] SET [Description] = 'Bend,  1 1/2" TNA X CC Swivel 90°' WHERE [MaterialID] = 3414;
UPDATE [Materials] SET [Description] = 'Bend,   5/8" X 3/4" TNA X CC Swivel  45°' WHERE [MaterialID] = 3417;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" TNA X CC Swivel 45°' WHERE [MaterialID] = 3418;
UPDATE [Materials] SET [Description] = 'Bend,  1 1/2" TNA X CC Swivel 45°' WHERE [MaterialID] = 3419;
UPDATE [Materials] SET [Description] = 'Bend,   3/4" X  3/4" CC X MIP 45°' WHERE [MaterialID] = 3421;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" FIP X FIP ST HD' WHERE [MaterialID] = 3432;
UPDATE [Materials] SET [Description] = 'Coupling,  2 1/2" FIP X FIP ST HD' WHERE [MaterialID] = 3433;
UPDATE [Materials] SET [Description] = 'Coupling,  1" Insulated TNA X CC' WHERE [MaterialID] = 3434;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" Insulated TNA X CC' WHERE [MaterialID] = 3435;
UPDATE [Materials] SET [Description] = 'Coupling,  1" Insulated TNA X CF' WHERE [MaterialID] = 3436;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" Insulated TNA X CF' WHERE [MaterialID] = 3437;
UPDATE [Materials] SET [Description] = 'Coupling,  1" X  3/4" GALV IPC X IPC' WHERE [MaterialID] = 3439;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" GALV IPC X IPC' WHERE [MaterialID] = 3440;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" GALV CC X MIP' WHERE [MaterialID] = 3445;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" GALV CC X MIP' WHERE [MaterialID] = 3446;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" GALV CC X FIP' WHERE [MaterialID] = 3451;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" GALV CC X FIP' WHERE [MaterialID] = 3452;
UPDATE [Materials] SET [Description] = 'Coupling,   3/4" Slip X FIP PVC' WHERE [MaterialID] = 3463;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/4" Slip X FIP PVC' WHERE [MaterialID] = 3464;
UPDATE [Materials] SET [Description] = 'Coupling,  1 1/2" Slip X FIP PVC' WHERE [MaterialID] = 3465;
UPDATE [Materials] SET [Description] = 'Bend,  2 1/2" X 2 1/2" PVC SJ 90°' WHERE [MaterialID] = 3493;
UPDATE [Materials] SET [Description] = 'Bend,  2 1/2" X 2 1/2" PVC SJ 45°' WHERE [MaterialID] = 3494;
UPDATE [Materials] SET [Description] = 'Bushing, 1" X 3/4" PVC Taper X Taper' WHERE [MaterialID] = 3496;
UPDATE [Materials] SET [Description] = 'Adapter, 48" LJB X LJS ECP Full Bevel' WHERE [MaterialID] = 3510;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  6" Tap FAB MJ Outlet PE pipe' WHERE [MaterialID] = 3685;
UPDATE [Materials] SET [Description] = 'Sleeve,  4" X 4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3722;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3723;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3724;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3725;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3726;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3727;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3728;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 10" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3729;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3730;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3731;
UPDATE [Materials] SET [Description] = 'Sleeve, 18" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3732;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X  4" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3733;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3734;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X 16" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3735;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X 20" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3736;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3737;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3738;
UPDATE [Materials] SET [Description] = 'Sleeve, 30" X  2" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3739;
UPDATE [Materials] SET [Description] = 'Sleeve, 30" X  6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3740;
UPDATE [Materials] SET [Description] = 'Sleeve, 30" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3741;
UPDATE [Materials] SET [Description] = 'Sleeve, 36" X  6" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3742;
UPDATE [Materials] SET [Description] = 'Sleeve, 36" X  8" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3743;
UPDATE [Materials] SET [Description] = 'Sleeve, 36" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3744;
UPDATE [Materials] SET [Description] = 'Sleeve, 48" X 12" Tap FAB FLG CI/DI' WHERE [MaterialID] = 3745;
UPDATE [Materials] SET [Description] = 'Sleeve,  4" X 4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3746;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3747;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 6" Tap FAB SS CI/DI' WHERE [MaterialID] = 3748;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3749;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 6" Tap FAB SS CI/DI' WHERE [MaterialID] = 3750;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3751;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3752;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X 10" Tap FAB SS CI/DI' WHERE [MaterialID] = 3753;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3754;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  6" Tap FAB SS CI/DI' WHERE [MaterialID] = 3755;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3756;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 10" Tap FAB SS CI/DI' WHERE [MaterialID] = 3757;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 12" Tap FAB SS CI/DI' WHERE [MaterialID] = 3758;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X 16" Tap FAB SS CI/DI' WHERE [MaterialID] = 3759;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3760;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X 14" Tap FAB SS CI/DI' WHERE [MaterialID] = 3761;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3762;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  6" Tap FAB SS CI/DI' WHERE [MaterialID] = 3763;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3764;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 10" Tap FAB SS CI/DI' WHERE [MaterialID] = 3765;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 12" Tap FAB SS CI/DI' WHERE [MaterialID] = 3766;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 16" Tap FAB SS CI/DI' WHERE [MaterialID] = 3767;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X  4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3768;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X  6" Tap FAB SS CI/DI' WHERE [MaterialID] = 3769;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3770;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X 12" Tap FAB SS CI/DI' WHERE [MaterialID] = 3771;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X  4" Tap FAB SS CI/DI' WHERE [MaterialID] = 3772;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X  6" Tap FAB SS CI/DI' WHERE [MaterialID] = 3773;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3774;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X 12" Tap FAB SS CI/DI' WHERE [MaterialID] = 3775;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X 16" Tap FAB SS CI/DI' WHERE [MaterialID] = 3776;
UPDATE [Materials] SET [Description] = 'Sleeve, 36" X  8" Tap FAB SS CI/DI' WHERE [MaterialID] = 3777;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  4" Tap FAB SS AC' WHERE [MaterialID] = 3784;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  6" Tap FAB SS AC' WHERE [MaterialID] = 3785;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  8" Tap FAB SS AC' WHERE [MaterialID] = 3786;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X 10" Tap FAB SS AC' WHERE [MaterialID] = 3787;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  4" Tap FAB SS AC' WHERE [MaterialID] = 3788;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  6" Tap FAB SS AC' WHERE [MaterialID] = 3789;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  8" Tap FAB SS AC' WHERE [MaterialID] = 3790;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 10" Tap FAB SS AC' WHERE [MaterialID] = 3791;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 12" Tap FAB SS AC' WHERE [MaterialID] = 3792;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X  8" Tap FAB SS AC' WHERE [MaterialID] = 3793;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X 12" Tap FAB SS AC' WHERE [MaterialID] = 3794;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  4" Tap FAB SS AC' WHERE [MaterialID] = 3795;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  6" Tap FAB SS AC' WHERE [MaterialID] = 3796;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X  8" Tap FAB SS AC' WHERE [MaterialID] = 3797;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 12" Tap FAB SS AC' WHERE [MaterialID] = 3798;
UPDATE [Materials] SET [Description] = 'Sleeve, 16" X 16" Tap FAB SS AC' WHERE [MaterialID] = 3799;
UPDATE [Materials] SET [Description] = 'Sleeve, 20" X 12" Tap FAB SS AC' WHERE [MaterialID] = 3800;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X  8" Tap FAB SS AC' WHERE [MaterialID] = 3801;
UPDATE [Materials] SET [Description] = 'Sleeve, 24" X 12" Tap FAB SS AC' WHERE [MaterialID] = 3802;
UPDATE [Materials] SET [Description] = 'Sleeve, 30" X 12" Tap FAB SS AC' WHERE [MaterialID] = 3803;
UPDATE [Materials] SET [Description] = 'Sleeve, 36" X  8" Tap FAB SS AC' WHERE [MaterialID] = 3804;
UPDATE [Materials] SET [Description] = 'Sleeve,  4" X 2" Tap FAB SS PVC' WHERE [MaterialID] = 3805;
UPDATE [Materials] SET [Description] = 'Sleeve,  4" X 4" Tap FAB SS PVC' WHERE [MaterialID] = 3806;
UPDATE [Materials] SET [Description] = 'Sleeve,  5" X 4" Tap FAB SS PVC' WHERE [MaterialID] = 3807;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 2" Tap FAB SS PVC' WHERE [MaterialID] = 3808;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 4" Tap FAB SS PVC' WHERE [MaterialID] = 3809;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 6" Tap FAB SS PVC' WHERE [MaterialID] = 3810;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 2" Tap FAB SS PVC' WHERE [MaterialID] = 3811;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 4" Tap FAB SS PVC' WHERE [MaterialID] = 3812;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 6" Tap FAB SS PVC' WHERE [MaterialID] = 3813;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 8" Tap FAB SS PVC' WHERE [MaterialID] = 3814;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  4" Tap FAB SS PVC' WHERE [MaterialID] = 3815;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  6" Tap FAB SS PVC' WHERE [MaterialID] = 3816;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  8" Tap FAB SS PVC' WHERE [MaterialID] = 3817;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X 10" Tap FAB SS PVC' WHERE [MaterialID] = 3818;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  2" Tap FAB SS PVC' WHERE [MaterialID] = 3819;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  4" Tap FAB SS PVC' WHERE [MaterialID] = 3820;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  6" Tap FAB SS PVC' WHERE [MaterialID] = 3821;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  8" Tap FAB SS PVC' WHERE [MaterialID] = 3822;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 10" Tap FAB SS PVC' WHERE [MaterialID] = 3823;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 12" Tap FAB SS PVC' WHERE [MaterialID] = 3824;
UPDATE [Materials] SET [Description] = 'Sleeve, 14" X  8" Tap FAB SS PVC' WHERE [MaterialID] = 3825;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 6" Tap FAB SS IPS' WHERE [MaterialID] = 3827;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 4" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3841;
UPDATE [Materials] SET [Description] = 'Sleeve,  6" X 6" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3842;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 4" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3843;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 6" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3844;
UPDATE [Materials] SET [Description] = 'Sleeve,  8" X 8" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3845;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  4" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3846;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  6" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3847;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X  8" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3848;
UPDATE [Materials] SET [Description] = 'Sleeve, 10" X 10" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3849;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  4" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3850;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  6" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3851;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X  8" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3852;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 10" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3853;
UPDATE [Materials] SET [Description] = 'Sleeve, 12" X 12" Tap FAB MJ Outlet CI/DI' WHERE [MaterialID] = 3854;
UPDATE [Materials] SET [Description] = 'Saddle,  2" X  3/4" SRV DBL STP CI' WHERE [MaterialID] = 3869;
UPDATE [Materials] SET [Description] = 'Saddle,  4" X 1" SRV DBL STP CI' WHERE [MaterialID] = 3870;
UPDATE [Materials] SET [Description] = 'Saddle,  4" X 2" SRV DBL STP CI' WHERE [MaterialID] = 3871;
UPDATE [Materials] SET [Description] = 'Saddle,  6" X 1" SRV DBL STP CI' WHERE [MaterialID] = 3872;
UPDATE [Materials] SET [Description] = 'Saddle,  6" X 2" SRV DBL STP CI' WHERE [MaterialID] = 3873;
UPDATE [Materials] SET [Description] = 'Saddle,  8" X 1" SRV DBL STP CI' WHERE [MaterialID] = 3874;
UPDATE [Materials] SET [Description] = 'Saddle,  8" X 2" SRV DBL STP CI' WHERE [MaterialID] = 3875;
UPDATE [Materials] SET [Description] = 'Saddle, 10" X 2" SRV DBL STP CI' WHERE [MaterialID] = 3876;
UPDATE [Materials] SET [Description] = 'Saddle, 12" X 2" SRV DBL STP CI' WHERE [MaterialID] = 3877;
UPDATE [Materials] SET [Description] = 'Saddle,  6" X 2" SRV DBL STP STL' WHERE [MaterialID] = 3878;
UPDATE [Materials] SET [Description] = 'Saddle,  8" X 2" SRV DBL STP STL' WHERE [MaterialID] = 3879;
UPDATE [Materials] SET [Description] = 'Saddle, 12" X 2" SRV DBL STP STL' WHERE [MaterialID] = 3880;
UPDATE [Materials] SET [Description] = 'Saddle,  2" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3882;
UPDATE [Materials] SET [Description] = 'Saddle,  3" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3884;
UPDATE [Materials] SET [Description] = 'Saddle,  4" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3886;
UPDATE [Materials] SET [Description] = 'Saddle,  6" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3888;
UPDATE [Materials] SET [Description] = 'Saddle,  8" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3891;
UPDATE [Materials] SET [Description] = 'Saddle, 10" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3893;
UPDATE [Materials] SET [Description] = 'Saddle, 12" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3895;
UPDATE [Materials] SET [Description] = 'Saddle,  2 1/4" X 1" SRV PVC IPS' WHERE [MaterialID] = 3896;
UPDATE [Materials] SET [Description] = 'Saddle,  2 1/4" X  3/4" SRV PVC IPS' WHERE [MaterialID] = 3897;
UPDATE [Materials] SET [Description] = 'Saddle,  1 1/2" X  3/4" SRV PVC' WHERE [MaterialID] = 3920;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 16" X  1 1/2" SRV PSC' WHERE [MaterialID] = 3927;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 20" X  1 1/2" SRV PSC' WHERE [MaterialID] = 3929;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 20" X  2 1/2" DI/CI' WHERE [MaterialID] = 3955;
UPDATE [Materials] SET [Description] = 'Saddle, Tap 30" X  2 1/2" DI/CI' WHERE [MaterialID] = 3964;
UPDATE [Materials] SET [Description] = 'Saddle, Self Tapping  4" X 1" PVC' WHERE [MaterialID] = 3972;
UPDATE [Materials] SET [Description] = 'Saddle, Self Tapping  6" X 1" PVC' WHERE [MaterialID] = 3973;
UPDATE [Materials] SET [Description] = 'Saddle, Self Tapping  8" X 1" PVC' WHERE [MaterialID] = 3974;
UPDATE [Materials] SET [Description] = 'Saddle, Self Tapping 10" X 1" PVC' WHERE [MaterialID] = 3975;
UPDATE [Materials] SET [Description] = 'Saddle, Tap  1 1/4" X  3/4" SRV' WHERE [MaterialID] = 4002;
UPDATE [Materials] SET [Description] = 'Saddle, Tap  1 1/2" X  3/4" SRV' WHERE [MaterialID] = 4004;
UPDATE [Materials] SET [Description] = 'Saddle, Tap  2 1/2" X  3/4" SRV' WHERE [MaterialID] = 4010;
UPDATE [Materials] SET [Description] = 'Saddle, Tap  3 1/2" X  3/4" SRV' WHERE [MaterialID] = 4013;
UPDATE [Materials] SET [Description] = 'Saddle,  2 1/2" X  3/4" SRV BCB' WHERE [MaterialID] = 4016;
UPDATE [Materials] SET [Description] = 'Coupling,  4" X 12" Anchor DI MJ' WHERE [MaterialID] = 4088;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  1" Anchor DI MJ' WHERE [MaterialID] = 4089;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X  2" Anchor DI MJ' WHERE [MaterialID] = 4090;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X 13" Anchor DI MJ' WHERE [MaterialID] = 4091;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X 13" Anchor DI MJ' WHERE [MaterialID] = 4092;
UPDATE [Materials] SET [Description] = 'Coupling, 12" X 12" Anchor DI MJ' WHERE [MaterialID] = 4093;
UPDATE [Materials] SET [Description] = 'Coupling, 12" X 12" Anchor DI MJ' WHERE [MaterialID] = 4094;
UPDATE [Materials] SET [Description] = 'Offset, Hydrant  6" X  6" Anchor' WHERE [MaterialID] = 4095;
UPDATE [Materials] SET [Description] = 'Offset, Hydrant  6" X 13" Anchor' WHERE [MaterialID] = 4096;
UPDATE [Materials] SET [Description] = 'Coupling,  6" X 36" Anchor W/OFFSET' WHERE [MaterialID] = 4097;
UPDATE [Materials] SET [Description] = 'Coupling,  8" X 12" Anchor W/OFFSET' WHERE [MaterialID] = 4098;
UPDATE [Materials] SET [Description] = 'Adapter,  4" FLG X FLEX Bolted DI' WHERE [MaterialID] = 4145;
UPDATE [Materials] SET [Description] = 'Adapter,  6" FLG X FLEX Bolted DI' WHERE [MaterialID] = 4146;
UPDATE [Materials] SET [Description] = 'Adapter,  8" FLG X FLEX Bolted DI' WHERE [MaterialID] = 4147;
UPDATE [Materials] SET [Description] = 'Adapter, 10" FLG X FLEX Bolted DI' WHERE [MaterialID] = 4148;
UPDATE [Materials] SET [Description] = 'Adapter, 12" FLG X FLEX Bolted DI' WHERE [MaterialID] = 4149;
UPDATE [Materials] SET [Description] = 'Adapter, 30" LJB X LJS LCP 1/2 Bevel' WHERE [MaterialID] = 4186;
UPDATE [Materials] SET [Description] = 'Adapter, 36" LJB X LJS LCP 1/2 Bevel' WHERE [MaterialID] = 4187;
UPDATE [Materials] SET [Description] = 'Adapter, 42" LJB X LJS LCP 1/2 Bevel' WHERE [MaterialID] = 4188;
UPDATE [Materials] SET [Description] = 'Adapter, 48" LJB X LJS LCP 1/2 Bevel' WHERE [MaterialID] = 4189;
UPDATE [Materials] SET [Description] = 'Adapter, 16" X 12" LJS X MJS LCP' WHERE [MaterialID] = 4197;
UPDATE [Materials] SET [Description] = 'Adapter, 36" LJB X LJS LCP Full Bevel' WHERE [MaterialID] = 4205;
UPDATE [Materials] SET [Description] = 'Adapter, 42" LJB X LJS LCP Full Bevel' WHERE [MaterialID] = 4206;
UPDATE [Materials] SET [Description] = 'Adapter, 48" LJB X LJS LCP Full Bevel' WHERE [MaterialID] = 4207;


-- insert new materials
SET IDENTITY_INSERT [dbo].[Materials] ON;
BEGIN TRANSACTION;
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4340,'M8920605','Meter,  6 HP PRCTS GAL EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4341,'M8940329','Meter,  3 HP C/F ET4BR7F8S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4342,'M8960116','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4343,'M8960243','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4344,'M8960G152','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4345,'M8960G84','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4346,'M8961F49','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4347,'W7170806','Sleeve,  8" X  6" Tap FAB MJ O');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4348,'W7171206','Sleeve, 12" X  6" Tap FAB MJ O');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4349,'W6770303','Adapter,  3" SJ X MIP PVC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4350,'W6770606','Adapter,  6" SJ X MIP PVC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4351,'W6913636','Adapter, 36" LJB X MJS ECP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4352,'W6953636','Adapter, 36" LJS X MJS ECP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4353,'W7004216','Tee, 42" X 16" MJ');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4354,'W7292412','Sleeve, 24" X 12" Tap FAB MJ O');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4355,'W7293012','Sleeve, 30" X 12" Tap FAB MJ O');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4356,'W7300801','Saddle,  8" X 1" CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4357,'W7301001','Saddle, 10" X 1" CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4358,'W730100H','Saddle, 10" X  3/4" CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4359,'W730120H','Saddle, 12" X  3/4" CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4360,'W7302F0H','Saddle,  2 1/2" X  3/4" CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4361,'W7310201','Saddle,  2" X 1" PVC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4362,'W7312C01','Saddle,  2 1/4" X 1" PVC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4363,'W7312C0H','Saddle,  2 1/4" X  3/4" PVC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4364,'W732030H','Saddle,  3" X  3/4" SRV DBL ST');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4365,'W7321402','Saddle, 14" X 2" SRV DBL STP C');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4366,'W7321602','Saddle, 16" X 2" SRV DBL STP C');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4367,'W7321802','Saddle, 18" X 2" SRV DBL STP C');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4368,'W7322002','Saddle, 20" X 2" SRV DBL STP C');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4369,'W736030H','Saddle,  3" X  3/4" SRV PVC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4370,'W7383024','Saddle, Tap 30" X 24" SRV PSC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4371,'W7394806','Saddle, Tap 48" X  6" DI/CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4372,'W7620808','Cap,  8" End Flexbolt AC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4373,'W7692F2F','Cap,  2 1/2" BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4374,'W7730624','Coupling,  6" X 24" Anchor DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4375,'W7730636','Coupling,  6" X 36" Anchor DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4376,'W7730648','Coupling,  6" X 48" Anchor DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4377,'W7730812','Coupling,  8" X 12" Anchor DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4378,'W7740612','Offset,  6" X 12" Anchor');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4379,'W7740624','Offset,  6" X 24" Anchor');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4380,'W7772020','Plug, 20" DI SJ');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4381,'W7802000','Plug, 20" DI RJ');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4382,'W7860818','Adapter,  8" X 18" FLG X PE');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4383,'W7873030','Adapter, 30" FLG X MJ');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4384,'W7973030','Adapter, 30" LJB X LJS LCP Ful');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4385,'W8200404','Valve,  4" Gate FLG X MJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4386,'W8200606','Valve,  6" Gate FLG X MJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4387,'W8200808','Valve,  8" Gate FLG X MJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4388,'W8201010','Valve, 10" Gate FLG X MJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4389,'W8223030','Valve, 30" Gate MJ HORZ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4390,'W8260606','Valve,  6" Tap SJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4391,'W8260808','Valve,  8" Tap SJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4392,'W8261212','Valve, 12" Tap SJ OL');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4393,'W8436060','Valve, 60" Butterfly FLG OR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4394,'W7210804','Sleeve,  8" X 4" Tap FAB MJ Ou');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4395,'W7231006','Sleeve, 10" X  6" Tap FAB FLG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4396,'W1230101','Bend,  1" PC X PC 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4397,'W1230H0H','Bend,   3/4" X  3/4" PC X PC 9');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4398,'W1240101','Bend,  1" PC X CC 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4399,'W1240H0H','Bend,   3/4" X  3/4" PC X CC 9');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4400,'W1261F1F','Bend,  1 1/2" MIP X CC 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4401,'W1280H0H','Bend,   3/4" X  3/4" CF X IPC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4402,'W1442F02','Tee,  2 1/2" X 2" GALV');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4403,'W1450101','Setter, 1" MIP Vertical');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4404,'W1460202','Bend,  2" BR CC X CF 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4405,'W1480H01','Bend,   3/4" X 1" CC X FIP 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4406,'W1490101','Bend,  1" PC X MIP 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4407,'W1500H0G','Bend,   3/4" X  5/8" CF X LC 9');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4408,'W1641F1F','Setter, 1 1/2" PC X PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4409,'W1700F0G','Adapter,   1/2" X 5/8" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4410,'W1701F02','Adapter,  1 1/2" X 2" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4411,'W1720202','Valve,  2" Check Angle');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4412,'W1750G0H','Bend,   5/8" X 3/4" MC X PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4413,'W1750H0H','Bend,   3/4" X  3/4" MC X PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4414,'W1780101','Valve,  1" Yoke Angle MC X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4415,'W1790101','Valve,  1" Yoke Angle MC X CF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4416,'W1790H0H','Valve,   3/4" X  3/4" Yoke Ang');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4417,'W1800G0H','Valve,   5/8" X  3/4" Yoke Ang');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4418,'W1810G0H07','Setter,  5/8" X  3/4" X 7" MIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4419,'W18620110F','Frame, MB 20" X 11 1/2" No Lid');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4420,'W1871500','Lid, 15" Meter Fabricated');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4421,'W1973818','Box, Meter 38" X 18" ST');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4422,'W1992424','Box, Meter Concrete 24" X 24"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4423,'W1993636','Box, Meter Concrete 36" X 36"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4424,'W2002036','Box, Meter Plastic 20" X 36"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4425,'W2003600','Box, Meter Plastic 36"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4426,'W210110F','Lid, 11 1/2" Only for F/L');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4427,'W2231C1C','Curb Box,  1 1/4" CI SL Minn.');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4428,'W2240101','Curb Box,  1" Complete SL Big');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4429,'W2370101','Valve Box Riser,  1"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4430,'W2370202','Valve Box Riser,  2"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4431,'W2370303','Valve Box Riser,  3"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4432,'W2370404','Valve Box Riser,  4"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4433,'W2530000','Box, Valve Complete PL Screw');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4434,'W2540000','Box, Valve Top PL Screw');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4435,'W2851818','Coupling, 18" FLEX Bolted DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4436,'W2870H0H','Coupling,   3/4" FLEX Bolted S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4437,'W2871C1C','Coupling,  1 1/4" FLEX Bolted');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4438,'W2871F1F','Coupling,  1 1/2" FLEX Bolted');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4439,'W3010202','Sleeve,  2" Solid MJ SP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4440,'W3251818','Bend, 18" MJ 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4441,'W3470404','Bend,  4" Welded 45°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4442,'W3800402','Flange,  4" X 2" Companion');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4443,'W393060612F','Clamp,  6" X 12 1/2" AC SS FCR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4444,'W393060615','Clamp,  6" X 15" AC SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4445,'W39306067F','Clamp,  6" X  7 1/2" AC SS FCR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4446,'W3970104','Clamp,  1" X 4" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4447,'W3970108','Clamp,  1" X 8" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4448,'W397020212','Clamp,  2" X 12" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4449,'W397020215','Clamp,  2" X 15" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4450,'W39702027F','Clamp,  2" X  7 1/2" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4451,'W3970208','Clamp,  2" X  8" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4452,'W397060612','Clamp,  6" X 12" SS FCRC CI/DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4453,'W397060615','Clamp,  6" X 15" SS FCRC CI/DI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4454,'W39706067F','Clamp,  6" X  7 1/2" SS FCRC C');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4455,'W3970H08','Clamp,   3/4" X 8" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4456,'W3971220','Clamp, 12" X 20" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4457,'W3971C04','Clamp,  1 1/4" X 4" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4458,'W3971F08','Clamp,  1 1/2" X 8" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4459,'W3971F1F','Clamp,  1 1/2" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4460,'W3972C08','Clamp,  2 1/4" X  8" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4461,'W3972F2F12F','Clamp,  2 1/2" X 12 1/2" SS FC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4462,'W3972F2F15','Clamp,  2 1/2" X 15" SS FCRC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4463,'W3972F2F7F','Clamp,  2 1/2" X  7 1/2" SS FC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4464,'W398080H','Clamp,  8" X  8" X  3/4" SS FC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4465,'W3981202','Clamp, 12" X  2" SS FCRC Tap');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4466,'W4000652','Pipe,  6" DI SJ CL 52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4467,'W4010418','Pipe,  4" X 18" FLG DI/CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4468,'W4012048','Pipe, 20" X 48" FLG DI/CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4469,'W4032452','Pipe, 24" DI RJ CL 52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4470,'W4170520','Pipe,  5" PVC SDR-21CL 200');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4471,'W5186038','Hydrant, Wash Twp 4''6" BURY w/');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4472,'W5225002','Hydrant, Franklin 5'' Bury 1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4473,'W5246520','Hydrant, Coatesville QC 4''6" B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4474,'W5246521','Hydrant, Coatesville QC 5''6" B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4475,'W5270206','Hydrant, Hopewell 4'' Bury');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4476,'W5282302','Hydrant, Oak Hill 3''6" Bury');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4477,'M8920606','Meter,  6 HP PRCTS GAL EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4478,'M8920607','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4479,'M8920608','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4480,'M8920609','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4481,'M8920610','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4482,'M8920611','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4483,'M8920612','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4484,'M8920613','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4485,'M8920614','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4486,'M8920615','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4487,'M8920616','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4488,'M8920617','Meter,  6 HP PRCTS GAL RW5G71');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4489,'M8920618','Meter,  6 HP PRCTS GAL RW5G71S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4490,'M8920619','Meter,  6 HP PRCTS C/F R75F72S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4491,'M8920620','Meter,  6 HP PRCTS C/F R75F72S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4492,'M8920621','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4493,'M8920622','Meter,  6 HP PRCTS C/F EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4494,'M8920623','Meter,  6 HP PRCTS C/F EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4495,'M8920624','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4496,'M8920625','Meter,  6 HP PRCTS C/F EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4497,'M8920626','Meter,  6 HP PRCTS GAL EP4D1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4498,'M8920801','Meter,  8 HP PRCTS C/F EP4E1R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4499,'M8920802','Meter,  8 HP PRCTS GAL EP4E1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4500,'M8920803','Meter,  8 HP PRCTS GAL EP4E1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4501,'M8920804','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4502,'M8920805','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4503,'M8920806','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4504,'M8920807','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4505,'M8920808','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4506,'M8920809','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4507,'M8920810','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4508,'M8920811','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4509,'M8920812','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4510,'M8920813','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4511,'M8920814','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4512,'M8920815','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4513,'M8920816','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4514,'M8920817','Meter,  8 HP PRCTS GAL RW5G81');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4515,'M8920818','Meter,  8 HP PRCTS GAL RW5G81S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4516,'M8920819','Meter,  8 HP PRCTS C/F R75F82');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4517,'M8920820','Meter,  8 HP PRCTS C/F R75F82S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4518,'M8920821','Meter,  8 HP PRCTS GAL R75G82');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4519,'M8920822','Meter,  8 HP PRCTS C/F EP4E1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4520,'M8920823','Meter,  8 HP PRCTS GAL EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4521,'M8920824','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4522,'M8920825','Meter,  8 HP PRCTS C/F EP4E1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4523,'M8921001','Meter, 10 HP PRCTS C/F EP4F1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4524,'M8921002','Meter, 10 HP PRCTS GAL EP4F1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4525,'M8921003','Meter, 10 HP PRCTS GAL EP4F1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4526,'M8921004','Meter, 10 HP PRCTS GAL EP4F1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4527,'M8921005','Meter, 10 HP PRCTS C/F EP4F1R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4528,'M8921006','Meter, 10 HP PRCTS C/F EP4F1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4529,'M8921007','Meter, 10 HP PRCTS C/F EP4F1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4530,'M8921008','Meter, 10 HP PRCTS GAL EP4F1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4531,'M8921009','Meter, 10 HP PRCTS C/F R75F92');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4532,'M8921010','Meter, 10 HP PRCTS C/F R75F92S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4533,'M8921011','Meter, 10 HP PRCTS GAL R75G92');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4534,'M8921012','Meter, 10 HP PRCTS GAL EP4F1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4535,'M8921013','Meter, 10 HP PRCTS GAL EP4F1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4536,'M8921014','Meter, 10 HP PRCTS GAL EP4F1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4537,'M8930201','Meter,  2 HP GAL EC2AR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4538,'M8930202','Meter,  2 HP GAL EC2ARWG1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4539,'M8930203','Meter,  2 HP GAL EC2ARWG1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4540,'M8930204','Meter,  2 HP GAL EC2ARWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4541,'M8930205','Meter,  2 HP C/F EC2AR7F7SA46');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4542,'M8930206','Meter,  2 HP GAL EC2AR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4543,'M8930207','Meter,  2 HP GAL EC2AR7G8S190');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4544,'M8930208','Meter,  2 HP GAL EC2AR7G8S227');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4545,'M8930209','Meter,  2 HP GAL EC2AR7G8S228');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4546,'M8930210','Meter,  2 HP GAL EC2AR7G8S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4547,'M8930211','Meter,  2 HP GAL EC2AR7G7S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4548,'M8930212','Meter,  2 HP C/F EC2ARWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4549,'M8930213','Meter,  2 HP GAL EC2AR7G8SG52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4550,'M8930214','Meter,  2 HP C/F EC2ARWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4551,'M8930215','Meter,  2 HP C/F EC2AR7F8S504');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4552,'M8940201','Meter,  2 HP C/F ET4AR8F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4553,'M8940202','Meter,  2 HP C/F ET4AR8FS514');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4554,'M8940203','Meter,  2 HP GAL ET4AR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4555,'M8940204','Meter,  2 HP GAL ET4AR8GS352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4556,'M8940205','Meter,  2 HP GAL ET4AR8GS788');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4557,'M8940206','Meter,  2 HP GAL ET4ARHG2S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4558,'M8940207','Meter,  2 HP C/F ET4ARWF1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4559,'M8940208','Meter,  2 HP C/F ET4ARWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4560,'M8940209','Meter,  2 HP C/F ET4ARWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4561,'M8940210','Meter,  2 HP C/F ET4AR7F8S617');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4562,'M8940211','Meter,  2 HP C/F ET4AR7F8S738');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4563,'M8940212','Meter,  2 HP C/F ET4AR7F8S780');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4564,'M8940213','Meter,  2 HP C/F ET4AR7F8S989');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4565,'M8940214','Meter,  2 HP C/F ET4AR7F8SG52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4566,'M8940215','Meter,  2 HP C/F ET4AR7F8S501');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4567,'M8940216','Meter,  2 HP C/F ET4AR7F8S503');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4568,'M8940217','Meter,  2 HP C/F ET4AR7F7S576');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4569,'M8940218','Meter,  2 HP C/F ET4AR7F7SA46');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4570,'M8940219','Meter,  2 HP GAL ET4AR7G8S554');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4571,'M8940220','Meter,  2 HP GAL ET4AR7G8S578');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4572,'M8940221','Meter,  2 HP GAL ET4AR7G8S780');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4573,'M8940222','Meter,  2 HP GAL ET4AR7G8S227');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4574,'M8940223','Meter,  2 HP GAL ET4AR7G8S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4575,'M8940224','Meter,  2 HP GAL ET4AR7G7S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4576,'M8940225','Meter,  2 HP GAL ET4AR7G7S431');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4577,'M8940226','Meter,  2 HP C/F RW5F11');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4578,'M8940227','Meter,  2 HP C/F 6WHL R75F12');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4579,'M8940228','Meter,  2 HP GAL R75G12');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4580,'M8940229','Meter,  2 HP C/F ET4AR7F8S505');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4581,'M8940230','Meter,  2 HP GAL ET4ARWG1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4582,'M8940301','Meter,  3 HP C/F ET4BR8F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4583,'M8940302','Meter,  3 HP GAL ET4BR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4584,'M8940303','Meter,  3 HP C/F ET4BRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4585,'M8940304','Meter,  3 HP C/F ET4BRWF1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4586,'M8940305','Meter,  3 HP GAL ET4BRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4587,'M8940306','Meter,  3 HP GAL ET4BRWG1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4588,'M8940307','Meter,  3 HP GAL ET4BRWG1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4589,'M8940308','Meter,  3 HP C/F ET4BR7F8S504');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4590,'M8940309','Meter,  3 HP C/F ET4BR7F8S569');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4591,'M8940310','Meter,  3 HP C/F ET4BR7F8S780');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4592,'M8940311','Meter,  3 HP C/F ET4BR7F8S825');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4593,'M8940312','Meter,  3 HP C/F ET4BR7F8S501');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4594,'M8940313','Meter,  3 HP C/F ET4BR7F8S503');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4595,'M8940314','Meter,  3 HP C/F ET4BR7F7S465');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4596,'M8940315','Meter,  3 HP C/F ET4BR7F7S576');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4597,'M8940316','Meter,  3 HP C/F ET4BR7F7SA46');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4598,'M8940317','Meter,  3 HP GAL ET4BR7G8S554');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4599,'M8940318','Meter,  3 HP GAL ET4BR7G8S578');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4600,'M8940319','Meter,  3 HP GAL ET4BR7G8S815');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4601,'M8940320','Meter,  3 HP GAL ET4BR7G8SG52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4602,'M8940321','Meter,  3 HP GAL ET4BR7G8S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4603,'M8940322','Meter,  3 HP GAL ET4BR7G8S501');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4604,'M8940323','Meter,  3 HP GAL ET4BR7G7S431');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4605,'M8940324','Meter,  3 HP C/F RW5F21');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4606,'M8940325','Meter,  3 HP GAL RW5G21');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4607,'M8940326','Meter,  3 HP C/F R75F22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4608,'M8940327','Meter,  3 HP C/F R75F22S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4609,'M8940328','Meter,  3 HP GAL R75G22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4610,'W5590600','Hydrant, Anchor Piece 6"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4611,'W5590612','Hydrant, Anchor Piece 6" X 12"');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4612,'W5605C06','Extension, 5 1/4" X  6" Hydran');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4613,'W5605C12','Extension, 5 1/4" X 12" Hydran');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4614,'W5880000','Safety Repair Kit');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4615,'W6200101','Bend,  1" IPC X CC 90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4616,'W622010H','Bend,  1" X  3/4" TNA X CC Swi');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4617,'M8940330','Meter,  3 HP C/F ET4BRWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4618,'M8940331','Meter,  3 HP C/F ET4BR7F8S507');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4619,'M8940332','Meter,  3 HP GAL ET4BR7G8S821');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4620,'M8940401','Meter,  4 HP C/F ET4CR8F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4621,'M8940402','Meter,  4 HP GAL ET4CR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4622,'M8940403','Meter,  4 HP C/F ET4CRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4623,'M8940404','Meter,  4 HP C/F ET4CRWF1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4624,'M8940405','Meter,  4 HP GAL ET4CRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4625,'M8940406','Meter,  4 HP GAL ET4CRWG1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4626,'M8940407','Meter,  4 HP GAL ET4CRWG1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4627,'M8940408','Meter,  4 HP C/F ET4CR7F8S504');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4628,'M8940409','Meter,  4 HP C/F ET4CR7F8S569');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4629,'M8940410','Meter,  4 HP C/F ET4CR7F8S635');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4630,'M8940411','Meter,  4 HP C/F ET4CR7F8S780');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4631,'M8940412','Meter,  4 HP C/F ET4CR7F8S788');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4632,'M8940413','Meter,  4 HP C/F ET4CR7F8S365');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4633,'M8940414','Meter,  4 HP C/F ET4CR7F8S503');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4634,'M8940415','Meter,  4 HP C/F ET4CR7F7S465');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4635,'M8940416','Meter,  4 HP C/F ET4CR7F7S576');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4636,'M8940417','Meter,  4 HP GAL ET4CR7G8S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4637,'M8940418','Meter,  4 HP GAL ET4CR7G8S439');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4638,'M8940419','Meter,  4 HP GAL ET4CR7G8S501');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4639,'M8940420','Meter,  4 HP GAL ET4CR7G8S554');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4640,'M8940421','Meter,  4 HP GAL ET4CR7G8S780');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4641,'M8940422','Meter,  4 HP GAL T4CR7G8S200');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4642,'M8940423','Meter,  4 HP GAL ET4CR7G8S227');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4643,'M8940424','Meter,  4 HP GAL ET4CR7G7S431');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4644,'M8940425','Meter,  4 HP C/F RW5F31');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4645,'M8940426','Meter,  4 HP C/F RW5F31SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4646,'M8940427','Meter,  4 HP GAL RW5G31');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4647,'M8940428','Meter,  4 HP C/F R75F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4648,'M8940429','Meter,  4 HP C/F R75F32S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4649,'M8940430','Meter,  4 HP GAL R75G32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4650,'M8940431','Meter,  4 HP GAL ET4CR7G8S1052');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4651,'M8940432','Meter,  4 HP C/F ET4CR7F8S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4652,'M8940433','Meter,  4 HP C/F ET4CR7F8S825');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4653,'M8940434','Meter,  4 HP C/F ET4CRWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4654,'M8940435','Meter,  4 HP C/F ET4CR7F8S507');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4655,'M8940436','Meter,  4 HP C/F ET4CR6F8S617');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4656,'M8940437','Meter,  4 HP GAL ET4CR7G8S821');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4657,'M8940601','Meter,  6 HP C/F ET4DRWF1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4658,'M8940602','Meter,  6 HP GAL ET4DRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4659,'M8940603','Meter,  6 HP GAL ET4DRWG1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4660,'M8940604','Meter,  6 HP C/F ET4DR7F8S365');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4661,'M8940605','Meter,  6 HP C/F ET4DR7F8S503');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4662,'M8940606','Meter,  6 HP GAL ET4DR7G8S227');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4663,'M8940607','Meter,  6 HP GAL ET4DR7G8S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4664,'M8940608','Meter,  6 HP GAL ET4DR7G8S554');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4665,'M8940609','Meter,  6 HP GAL ET4DR7G7S431');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4666,'M8940610','Meter,  6 HP C/F RW5F41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4667,'M8940611','Meter,  6 HP GAL RW5G41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4668,'M8940612','Meter,  6 HP C/F R75F42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4669,'M8940613','Meter,  6 HP C/F R75F42S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4670,'M8940614','Meter,  6 HP GAL R75G42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4671,'M8940615','Meter,  6 HP GAL ET4DR7G8S1052');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4672,'M8940616','Meter,  6 HP C/F ET4DR7F8S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4673,'M8940617','Meter,  6 HP C/F ET4DR6F8S977');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4674,'M8940618','Meter,  6 HP GAL ET4DRHG2S042');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4675,'M8940619','Meter,  6 HP C/F ET4DRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4676,'M8940620','Meter,  6 HP C/F ET4DRWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4677,'M8940621','Meter,  6 HP C/F ET4DR7F8S507');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4678,'M8940622','Meter,  6 HP C/F ET4DR6F8S617');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4679,'M8940623','Meter,  6 HP GAL ET4DR7G8S821');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4680,'M8940624','Meter,  6 HP GAL ET4DR6G7S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4681,'M8940625','Meter,  6 HP C/F ET4DR7F8S635');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4682,'M8940801','Meter,  8 HP GAL ET4ER8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4683,'M8940802','Meter,  8 HP GAL ET4ERWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4684,'M8940803','Meter,  8 HP GAL ET4ERWG1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4685,'M8940804','Meter,  8 HP GAL ET4ERWG1SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4686,'M8940805','Meter,  8 HP C/F ET4ER7F7S465');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4687,'M8940806','Meter,  8 HP GAL ET4ER7G8S352');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4688,'M8940807','Meter,  8 HP GAL ET4ER7G8S554');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4689,'M8940808','Meter,  8 HP GAL ET4ER7G8S821');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4690,'M8940809','Meter,  8 HP GAL ET4ER7G7S431');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4691,'M8940810','Meter,  8 HP C/F RW5F51');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4692,'M8940811','Meter,  8 HP C/F R75F52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4693,'M8940812','Meter,  8 HP C/F R75F52S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4694,'M8940813','Meter,  8 HP GAL R75G52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4695,'M8940814','Meter,  8 HP GAL ET4ER7G8S1052');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4696,'M8940815','Meter,  8 HP C/F ET4ER7F8S503');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4697,'M8940816','Meter,  8 HP C/F ET4ER7F8S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4698,'M8940817','Meter,  8 HP C/F ET4ERWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4699,'M8940818','Meter,  8 HP C/F ET4ERWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4700,'M8940819','Meter,  8 HP C/F ET4ER7F8S635');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4701,'M8941001','Meter, 10 HP C/F ET4FRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4702,'M8941002','Meter, 10 HP C/F RW5F01');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4703,'M8941003','Meter, 10 HP C/F ET4FRWF1SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4704,'M8941201','Meter, 12 HP C/F ET6JACR7F7S46');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4705,'M8941202','Meter, 12 HP GAL R75GA2');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4706,'M8941F01','Meter,  1 1/2 HP C/F ET4HRWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4707,'M8950H01','Meter,   3/4 SL T-10 GAL ED2D2');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4708,'M8950H02','Meter,   3/4 SL T-10 GAL ED2D2');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4709,'M8950H03','Meter,   3/4 SL T-10 GAL ED2D1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4710,'M8950H04','Meter,   3/4 SL T-10 C/F ED2D1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4711,'M8950H05','Meter,   3/4 SL T-10 GAL ED2D1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4712,'M8960101','Meter,  1 T-10 C/F ED2F21RDF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4713,'M8960102','Meter,  1 T-10 C/F ED2F21R8F1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4714,'M8960103','Meter,  1 T-10 GAL ED2F21R8G1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4715,'M8960104','Meter,  1 T-10 GAL ED2F21RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4716,'M8960105','Meter,  1 T-10 C/F ED2F21RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4717,'M8960106','Meter,  1 T-10 C/F ED2F21RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4718,'M8960107','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4719,'M8960108','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4720,'M8960109','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4721,'M8960110','Meter,  1 T-10 C/F ED2F21R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4722,'M89601100','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4723,'M89601101','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4724,'M8960111','Meter,  1 T-10 GAL ED2F21R6G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4725,'M896011102','Meter,  1 T-10 C/F ED2F21RWF8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4726,'M896011103','Meter,  1 T-10 C/F ED2F11RDF8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4727,'M896011104','Meter,  1 T-10 C/F ED2F11R6F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4728,'M896011105','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4729,'M896011106','Meter,  1 T-10 C/F ED2F11RDF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4730,'M896011107','Meter,  1 T-10 C/F ED2F11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4731,'M896011108','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4732,'M896011109','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4733,'M896011110','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4734,'M896011111','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4735,'M896011112','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4736,'M896011113','Meter,  1 T-10 C/F ED2F21RWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4737,'M896011114','Meter,  1 T-10 C/F ED2F21RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4738,'M896011115','Meter,  1 T-10 GAL ED2F21R8G1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4739,'M896011116','Meter,  1 T-10 GAL ED2F21RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4740,'M896011117','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4741,'M896011118','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4742,'M896011119','Meter,  1 T-10 C/F ED2F21R8F1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4743,'M896011120','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4744,'M896011121','Meter,  1 T-10 GAL ED2F2RWG1S8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4745,'M896011122','Meter,  1 T-10 C/F ED2F21RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4746,'M896011123','Meter,  1 T-10 C/F ED2F21RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4747,'M896011124','Meter,  1 T-10 C/F ED2F21R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4748,'M896011125','Meter,  1 T-10 C/F ED2F11RDF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4749,'M8960112','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4750,'M8960113','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4751,'M8960114','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4752,'M8960115','Meter,  1 T-10 C/F ED2F21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4753,'M8960117','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4754,'M8960118','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4755,'M8960119','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4756,'M8960120','Meter,  1 T-10 GAL ED2F21R7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4757,'M8960121','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4758,'M8960122','Meter,  1 T-10 GAL ED2F21R7G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4759,'M8960123','Meter,  1 T-10 C/F ED2F22R8F1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4760,'M8960124','Meter,  1 T-10 C/F ED2F22R8F1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4761,'M8960125','Meter,  1 T-10 GAL ED2F22R8G1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4762,'M8960126','Meter,  1 T-10 C/F ED2F11R8F1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4763,'M8960127','Meter,  1 T-10 C/F ED2F11R8F1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4764,'M8960128','Meter,  1 T-10 C/F ED2F11RDF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4765,'M8960129','Meter,  1 T-10 GAL ED2F11RDG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4766,'M8960130','Meter,  1 T-10 GAL ED2F11RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4767,'M8960131','Meter,  1 T-10 GAL ED2F11RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4768,'M8960132','Meter,  1 T-10 GAL ED2F11RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4769,'M8960133','Meter,  1 T-10 C/F ED2F11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4770,'M8960134','Meter,  1 T-10 C/F ED2F11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4771,'M8960135','Meter,  1 T-10 C/F ED2F11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4772,'M8960136','Meter,  1 T-10 C/F ED2F11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4773,'M8960137','Meter,  1 T-10 C/F ED2F11RWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4774,'M8960138','Meter,  1 T-10 C/F ED2F11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4775,'M8960139','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4776,'M8960140','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4777,'M8960141','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4778,'M8960142','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4779,'M8960143','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4780,'M8960144','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4781,'M8960145','Meter,  1 T-10 GAL ED2F11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4782,'M8960146','Meter,  1 T-10 C/F ED2F11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4783,'M8960147','Meter,  1 T-10 C/F ED2F11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4784,'M8960148','Meter,  1 T-10 C/F ED2F11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4785,'M8960149','Meter,  1 T-10 C/F ED2F11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4786,'M8960150','Meter,  1 T-10 C/F ED2F11R6F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4787,'M8960151','Meter,  1 T-10 C/F ED2F11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4788,'M8960152','Meter,  1 T-10 GAL ED2F11R6G6S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4789,'M8960153','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4790,'M8960154','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4791,'M8960155','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4792,'M8960156','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4793,'M8960157','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4794,'M8960158','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4795,'M8960159','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4796,'M8960160','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4797,'M8960161','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4798,'M8960162','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4799,'M8960163','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4800,'M8960164','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4801,'M8960165','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4802,'M8960166','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4803,'M8960167','Meter,  1 T-10 C/F ED2F11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4804,'M8960168','Meter,  1 T-10 C/F ED2F11R7F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4805,'M8960169','Meter,  1 T-10 C/F ED2F11R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4806,'M8960170','Meter,  1 T-10 C/F ED2F11R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4807,'M8960171','Meter,  1 T-10 C/F ED2F11R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4808,'M8960172','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4809,'M8960173','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4810,'M8960174','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4811,'M8960175','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4812,'M8960176','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4813,'M8960177','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4814,'M8960178','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4815,'M8960179','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4816,'M8960180','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4817,'M8960181','Meter,  1 T-10 GAL ED2F11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4818,'M8960182','Meter,  1 T-10 GAL ED2F11R7G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4819,'M8960183','Meter,  1 T-10 GAL RW8G31');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4820,'M8960184','Meter,  1 T-10 C/F R78F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4821,'M8960185','Meter,  1 T-10 C/F R78F32S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4822,'M8960186','Meter,  1 T-10 GAL R78G32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4823,'M8960187','Meter,  1 T-10 C/F RW2F31');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4824,'M8960188','Meter,  1 T-10 C/F RW2F31SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4825,'M8960189','Meter,  1 T-10 C/F RW2F31SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4826,'M8960190','Meter,  1 T-10 GAL RW2G31SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4827,'M8960191','Meter,  1 T-10 C/F R62F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4828,'M8960192','Meter,  1 T-10 C/F R62F32S690');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4829,'M8960193','Meter,  1 T-10 GAL R62G32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4830,'M8960194','Meter,  1 T-10 C/F R72F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4831,'M8960195','Meter,  1 T-10 C/F R72F32S342');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4832,'M8960196','Meter,  1 T-10 GAL R72G32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4833,'M8960197','Meter,  1 T-10 GAL ED2F21R7G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4834,'M8960198','Meter,  1 T-10 GAL ED2F21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4835,'M8960199','Meter,  1 T-10 C/F ED2F21R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4836,'M8960201','Meter,  2  T-10 C/F R78F52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4837,'M8960202','Meter,  2  T-10 GAL R78G52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4838,'M8960203','Meter,  2 T-10 C/F RD2F51');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4839,'M8960204','Meter,  2 T-10 GAL RD2G51');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4840,'M8960205','Meter,  2 T-10 C/F ED2J11R8F2S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4841,'M8960206','Meter,  2 T-10 GAL ED2J11R8G2');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4842,'M8960207','Meter,  2 T-10 GAL ED2J11R8G2S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4843,'M8960208','Meter,  2 T-10 GAL ED2J11R8G2S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4844,'M8960209','Meter,  2 T-10 C/F ED2J11RDF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4845,'M8960210','Meter,  2 T-10 GAL ED2J11RDG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4846,'M89602100','Meter,  2 T-10 C/F ED2J11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4847,'M89602101','Meter,  2 T-10 C/F ED2J11RHF2S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4848,'M89602102','Meter,  2 T-10 C/F ED2J11R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4849,'M89602103','Meter,  2 T-10 C/F ED2J11RDF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4850,'M89602104','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4851,'M89602105','Meter,  2 T-10 C/F ED2J11R8F2S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4852,'M8960211','Meter,  2 T-10 GAL ED2J11RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4853,'M8960212','Meter,  2 T-10 GAL ED2J11RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4854,'M8960213','Meter,  2 T-10 C/F ED2J11RWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4855,'M8960214','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4856,'M8960215','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4857,'M8960216','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4858,'M8960217','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4859,'M8960218','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4860,'M8960219','Meter,  2 T-10 GAL ED2J11RWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4861,'M8960220','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4862,'M8960221','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4863,'M8960222','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4864,'M8960223','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4865,'M8960224','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4866,'M8960225','Meter,  2 T-10 C/F ED2J11R6F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4867,'M8960226','Meter,  2 T-10 C/F ED2J11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4868,'M8960227','Meter,  2 T-10 C/F ED2J11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4869,'M8960228','Meter,  2 T-10 C/F ED2J11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4870,'M8960229','Meter,  2 T-10 C/F ED2J11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4871,'M8960230','Meter,  2 T-10 C/F ED2J11R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4872,'M8960231','Meter,  2 T-10 GAL ED2J11R6G7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4873,'M8960232','Meter,  2 T-10 GAL ED2J11R6G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4874,'M8960233','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4875,'M8960234','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4876,'M8960235','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4877,'M8960236','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4878,'M8960237','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4879,'M8960238','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4880,'M8960239','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4881,'M8960240','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4882,'M8960241','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4883,'M8960242','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4884,'M8960244','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4885,'M8960245','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4886,'M8960246','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4887,'M8960247','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4888,'M8960248','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4889,'M8960249','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4890,'M8960250','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4891,'M8960251','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4892,'M8960252','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4893,'M8960253','Meter,  2 T-10 C/F ED2J11R7F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4894,'M8960254','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4895,'M8960255','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4896,'M8960256','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4897,'M8960257','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4898,'M8960258','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4899,'M8960259','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4900,'M8960260','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4901,'M8960261','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4902,'M8960262','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4903,'M8960263','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4904,'M8960264','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4905,'M8960265','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4906,'M8960266','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4907,'M8960267','Meter,  2 T-10 GAL ED2J11R7G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4908,'M8960268','Meter,  2 T-10 GAL ED2J11R7G7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4909,'M8960269','Meter,  2 T-10 C/F R82F52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4910,'M8960270','Meter,  2 T-10 GAL R82G51');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4911,'M8960271','Meter,  2 T-10 C/F RW2F51');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4912,'M8960272','Meter,  2 T-10 GAL RW2G51');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4913,'M8960273','Meter,  2 T-10 GAL RW2G51SG59');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4914,'M8960274','Meter,  2 T-10 GAL RW2G51SG60');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4915,'M8960275','Meter,  2 T-10 C/F R62F52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4916,'M8960276','Meter,  2 T-10 C/F R62F52S690');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4917,'M8960277','Meter,  2 T-10 GAL R62G52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4918,'M8960278','Meter,  2 T-10 C/F R72F52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4919,'M8960279','Meter,  2 T-10 C/F R72F52S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4920,'M8960280','Meter,  2 T-10 C/F R72F52S503');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4921,'M8960281','Meter,  2 T-10 C/F R72F52SG54');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4922,'M8960282','Meter,  2 T-10 GAL R72G52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4923,'M8960283','Meter,  2 T-10 GAL R72G52SG52');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4924,'M8960284','Meter,  2 T-10 GAL R72G52SG54');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4925,'M8960285','Meter,  2 T-10 GAL ED2J21RDG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4926,'M8960286','Meter,  2 T-10 GAL ED2J21RWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4927,'M8960287','Meter,  2 T-10 C/F ED2J21R6F7S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4928,'M8960288','Meter,  2 T-10 C/F ED2J21R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4929,'M8960289','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4930,'M8960290','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4931,'M8960291','Meter,  2 T-10 GAL ED2J11R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4932,'M8960292','Meter,  2 T-10 GAL ED2J21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4933,'M8960293','Meter,  2 T-10 GAL ED2J21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4934,'M8960294','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4935,'M8960295','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4936,'M8960296','Meter,  2 T-10 GAL ED2J21R7G8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4937,'M8960297','Meter,  2 T-10 C/F ED2J11RWF1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4938,'M8960298','Meter,  2 T-10 C/F ED2J11R7F8S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4939,'M8960299','Meter,  2 T-10 GAL ED2J11RWG1S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4940,'M8960G01','Meter,   5/8 T-10 GAL ED2A21RE');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4941,'M8960G02','Meter,   5/8 T-10 GAL ED2A21RH');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4942,'M8960G03','Meter,   5/8 T-10 C/F ED2A21RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4943,'M8960G04','Meter,   5/8 T-10 GAL ED2A21RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4944,'M8960G05','Meter,   5/8 T-10 C/F ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4945,'M8960G06','Meter,   5/8 T-10 C/F ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4946,'M8960G07','Meter,   5/8 T-10 C/F ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4947,'M8960G08','Meter,   5/8 T-10 GAL ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4948,'M8960G09','Meter,   5/8 T-10 GAL ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4949,'M8960G10','Meter,   5/8 T-10 C/F ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4950,'M8960G100','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4951,'M8960G101','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4952,'M8960G102','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4953,'M8960G103','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4954,'M8960G104','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4955,'M8960G105','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4956,'M8960G106','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4957,'M8960G107','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4958,'M8960G108','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4959,'M8960G109','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4960,'M8960G11','Meter,   5/8 T-10 C/F ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4961,'M8960G110','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4962,'M8960G111','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4963,'M8960G112','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4964,'M8960G113','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4965,'M8960G114','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4966,'M8960G115','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4967,'M8960G116','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4968,'M8960G117','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4969,'M8960G118','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4970,'M8960G119','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4971,'M8960G12','Meter,   5/8 T-10 GAL ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4972,'M8960G120','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4973,'M8960G121','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4974,'M8960G122','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4975,'M8960G123','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4976,'M8960G124','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4977,'M8960G125','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4978,'M8960G126','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4979,'M8960G127','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4980,'M8960G128','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4981,'M8960G129','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4982,'M8960G13','Meter,   5/8 T-10 GAL ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4983,'M8960G130','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4984,'M8960G131','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4985,'M8960G132','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4986,'M8960G133','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4987,'M8960G134','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4988,'M8960G135','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4989,'M8960G136','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4990,'M8960G137','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4991,'M8960G138','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4992,'M8960G139','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4993,'M8960G14','Meter,   5/8 T-10 GAL ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4994,'M8960G140','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4995,'M8960G141','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4996,'M8960G142','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4997,'M8960G143','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4998,'M8960G144','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (4999,'M8960G145','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5000,'M8960G146','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5001,'M8960G147','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5002,'M8960G148','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5003,'M8960G149','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5004,'M8960G15','Meter,   5/8 T-10 GAL ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5005,'M8960G150','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5006,'M8960G151','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5007,'M8960G153','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5008,'M8960G154','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5009,'M8960G155','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5010,'M8960G156','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5011,'M8960G157','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5012,'M8960G158','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5013,'M8960G159','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5014,'M8960G16','Meter,   5/8 T-10 GAL ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5015,'M8960G160','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5016,'M8960G161','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5017,'M8960G162','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5018,'M8960G163','Meter,   5/8X1/2 T-10 GAL ED2A');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5019,'M8960G164','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5020,'M8960G165','Meter,   5/8X1/2 T-10 GAL ED2A');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5021,'M8960G166','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5022,'M8960G167','Meter,   5/8X1/2 T-10 C/F ED2A');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5023,'M8960G168','Meter,   5/8X3/4 T-10 CF ED2B2');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5024,'M8960G169','Meter,   5/8 T-10 GAL ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5025,'M8960G17','Meter,   5/8 T-10 C/F ED2A11R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5026,'M8960G170','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5027,'M8960G171','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5028,'M8960G172','Meter,   5/8 T-10 C/F ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5029,'M8960G173','Meter,   5/8 T-10 C/F ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5030,'M8960G174','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5031,'M8960G175','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5032,'M8960G176','Meter,   5/8 T-10 C/F ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5033,'M8960G177','Meter,   5/8 T-10 C/F ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5034,'M8960G178','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5035,'M8960G179','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5036,'M8960G18','Meter,   5/8 T-10 GAL ED2A11R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5037,'M8960G180','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5038,'M8960G181','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5039,'M8960G182','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5040,'M8960G184','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5041,'M8960G185','Meter,   5/8 T-10 GAL ED2A21R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5042,'M8960G186','Meter,   5/8X1/2 T-10 C/F ED2A');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5043,'M8960G187','Meter,   5/8 T-10 C/F ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5044,'M8960G188','Meter,   5/8 T-10 C/F ED2A21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5045,'M8960G189','Meter,   5/8 T-10 C/F ED2A21R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5046,'M8960G19','Meter,   5/8 T-10 C/F ED2A11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5047,'M8960G190','Meter,   5/8 T-10 GAL ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5048,'M8960G191','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5049,'M8960G192','Meter,   5/8 T-10 C/F ED2A21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5050,'M8960G193','Meter,   5/8 T-10 C/F ED2A21RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5051,'M8960G194','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5052,'M8960G195','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5053,'M8960G196','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5054,'M8960G197','Meter,   5/8X3/4 T-10 CF ED2B2');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5055,'M8960G198','Meter,   5/8 T-10 C/F ED2A11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5056,'M8960G199','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5057,'M8960G20','Meter,   5/8 T-10 C/F ED2A11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5058,'M8960G200','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5059,'M8960G201','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5060,'M8960G202','Meter,   5/8 T-10 C/F ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5061,'M8960G203','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5062,'M8960G204','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5063,'M8960G21','Meter,   5/8 T-10 C/F ED2A11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5064,'M8960G22','Meter,   5/8 T-10 C/F ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5065,'M8960G23','Meter,   5/8 T-10 C/F ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5066,'M8960G24','Meter,   5/8 T-10 C/F ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5067,'M8960G25','Meter,   5/8 T-10 C/F ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5068,'M8960G26','Meter,   5/8 T-10 GAL ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5069,'M8960G27','Meter,   5/8 T-10 GAL ED2A11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5070,'M8960G28','Meter,   5/8 T-10 C/F ED2A11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5071,'M8960G29','Meter,   5/8 T-10 C/F ED2A11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5072,'M8960G30','Meter,   5/8 T-10 C/F ED2A11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5073,'M8960G31','Meter,   5/8 T-10 C/F ED2A11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5074,'M8960G32','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5075,'M8960G33','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5076,'M8960G34','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5077,'M8960G35','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5078,'M8960G36','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5079,'M8960G37','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5080,'M8960G38','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5081,'M8960G39','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5082,'M8960G40','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5083,'M8960G41','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5084,'M8960G42','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5085,'M8960G43','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5086,'M8960G44','Meter,   5/8 T-10 C/F ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5087,'M8960G45','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5088,'M8960G46','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5089,'M8960G47','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5090,'M8960G48','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5091,'M8960G49','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5092,'M8960G50','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5093,'M8960G50','Meter,   5/8 T-10 GAL ED2A11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5094,'M8960G51','Meter,   5/8 T-10 C/F R78F12');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5095,'M8960G52','Meter,   5/8 T-10 GAL R78G12');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5096,'M8960G53','Meter,   5/8 T-10 GAL ED2A31RH');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5097,'M8960G54','Meter,   5/8 T-10 C/F ED2A31RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5098,'M8960G55','Meter,   5/8 T-10 GAL ED2A31RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5099,'M8960G56','Meter,   5/8 T-10 C/F ED2A31R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5100,'M8960G57','Meter,   5/8 T-10 C/F ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5101,'M8960G58','Meter,   5/8 T-10 C/F ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5102,'M8960G59','Meter,   5/8 T-10 C/F ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5103,'M8960G60','Meter,   5/8 T-10 C/F ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5104,'M8960G61','Meter,   5/8 T-10 C/F ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5105,'M8960G62','Meter,   5/8 T-10 C/F ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5106,'M8960G63','Meter,   5/8 T-10 GAL ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5107,'M8960G64','Meter,   5/8 T-10 GAL ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5108,'M8960G65','Meter,   5/8 T-10 GAL ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5109,'M8960G66','Meter,   5/8 T-10 GAL ED2A31R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5110,'M8960G67','Meter,   5/8 T-10 C/F RW2F11');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5111,'M8960G68','Meter,   5/8 T-10 C/F RW2F11SG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5112,'M8960G69','Meter,   5/8 T-10 GAL RW2G11SG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5113,'M8960G70','Meter,   5/8 T-10 C/F 6WHL R62');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5114,'M8960G71','Meter,   5/8 T-10 C/F 6WHL R72');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5115,'M8960G72','Meter,   5/8 T-10 GAL R72G12');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5116,'M8960G73','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5117,'M8960G74','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5118,'M8960G75','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5119,'M8960G76','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5120,'M8960G77','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5121,'M8960G78','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5122,'M8960G79','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5123,'M8960G80','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5124,'M8960G81','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5125,'M8960G82','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5126,'M8960G83','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5127,'M8960G85','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5128,'M8960G86','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5129,'M8960G87','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5130,'M8960G88','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5131,'M8960G89','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5132,'M8960G90','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5133,'M8960G91','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5134,'M8960G92','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5135,'M8960G93','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5136,'M8960G94','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5137,'M8960G95','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5138,'M8960G96','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5139,'M8960G97','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5140,'M8960G98','Meter,   5/8X3/4 T-10 C/F ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5141,'M8960G99','Meter,   5/8X3/4 T-10 GAL ED2B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5142,'M8960H01','Meter,   3/4 T-10 C/F ED2C21RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5143,'M8960H02','Meter,   3/4 T-10 C/F ED2C21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5144,'M8960H03','Meter,   3/4 T-10 C/F ED2C21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5145,'M8960H04','Meter,   3/4 T-10 GAL ED2C21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5146,'M8960H05','Meter,   3/4 T-10 GAL ED2C21R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5147,'M8960H06','Meter,   3/4 T-10 C/F ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5148,'M8960H07','Meter,   3/4 T-10 C/F ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5149,'M8960H08','Meter,   3/4 T-10 C/F ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5150,'M8960H09','Meter,   3/4 T-10 GAL ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5151,'M8960H10','Meter,   3/4 T-10 C/F ED2C22RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5152,'M8960H11','Meter,   3/4 T-10 C/F ED2C22R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5153,'M8960H12','Meter,   3/4 T-10 C/F ED2C22R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5154,'M8960H13','Meter,   3/4 T-10 C/F ED2C11R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5155,'M8960H14','Meter,   3/4 T-10 C/F ED2C11R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5156,'M8960H15','Meter,   3/4 T-10 GAL ED2C11R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5157,'M8960H16','Meter,   3/4 T-10 C/F ED2C11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5158,'M8960H17','Meter,   3/4 T-10 GAL ED2C11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5159,'M8960H18','Meter,   3/4 T-10 C/F ED2C11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5160,'M8960H19','Meter,   3/4 T-10 C/F ED2C11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5161,'M8960H20','Meter,   3/4 T-10 C/F ED2C11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5162,'M8960H21','Meter,   3/4 T-10 C/F ED2C11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5163,'M8960H22','Meter,   3/4 T-10 C/F ED2C11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5164,'M8960H23','Meter,   3/4 T-10 GAL ED2C11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5165,'M8960H24','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5166,'M8960H25','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5167,'M8960H26','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5168,'M8960H27','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5169,'M8960H28','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5170,'M8960H29','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5171,'M8960H29','Meter,   3/4 T-10 C/F ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5172,'M8960H30','Meter,   3/4 T-10 GAL ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5173,'M8960H31','Meter,   3/4 T-10 GAL ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5174,'M8960H32','Meter,   3/4 T-10 GAL ED2C11R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5175,'M8960H33','Meter,   3/4 T-10 GAL ED2C21RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5176,'M8960H34','Meter,   3/4 T-10 C/F R78F22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5177,'M8960H35','Meter,   3/4 T-10 GAL R78G22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5178,'M8960H36','Meter,   3/4 T-10 GAL RD2G21');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5179,'M8960H37','Meter,   3/4 T-10 C/F RW2F21');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5180,'M8960H38','Meter,   3/4 T-10 C/F RW2F21SG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5181,'M8960H39','Meter,   3/4 T-10 C/F R62F22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5182,'M8960H40','Meter,   3/4 T-10 C/F R72F22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5183,'M8960H41','Meter,   3/4 T-10 C/F R72F22S3');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5184,'M8960H42','Meter,   3/4 T-10 GAL R72G22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5185,'M8960H43','Meter,   3/4 T-10 GAL ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5186,'M8960H44','Meter,   3/4 T-10 GAL ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5187,'M8960H45','Meter,   3/4X3/4 T-10 C/F ED2C');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5188,'M8960H46','Meter,   3/4 T-10 GAL ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5189,'M8960H47','Meter,   3/4 T-10 GAL ED2C21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5190,'M8960H48','Meter,   3/4 T-10 C/F ED2C11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5191,'M8960H49','Meter,   3/4 T-10 C/F ED2C11RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5192,'M8960H50','Meter,   3/4 T-10 C/F ED2C11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5193,'M8960H51','Meter,   3/4 T-10 C/F ED2C21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5194,'M8960H52','Meter,   3/4 T-10 C/F ED2C21RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5195,'M8960H53','Meter,   3/4 T-10 C/F ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5196,'M8960H54','Meter,   3/4 T-10 C/F ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5197,'M8960H55','Meter,   3/4 T-10 C/F ED2C21R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5198,'M8960H56','Meter,   3/4 T-10 GAL ED2C21R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5199,'M8960H57','Meter,   3/4 T-10 C/F ED2C11R6');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5200,'M8960H58','Meter,   3/4 T-10 C/F ED2C11RD');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5201,'M8960H59','Meter,   3/4 T-10 C/F ED2C21R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5202,'M8961F01','Meter,  1 1/2 T-10 GAL RW8G41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5203,'M8961F02','Meter,  1 1/2 T-10 C/F R78F42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5204,'M8961F03','Meter,  1 1/2 T-10 C/F RD2F41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5205,'M8961F04','Meter,  1 1/2 T-10 GAL RD2G41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5206,'M8961F05','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5207,'M8961F06','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5208,'M8961F07','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5209,'M8961F08','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5210,'M8961F09','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5211,'M8961F10','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5212,'M8961F11','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5213,'M8961F12','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5214,'M8961F13','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5215,'M8961F14','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5216,'M8961F15','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5217,'M8961F16','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5218,'M8961F17','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5219,'M8961F18','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5220,'M8961F19','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5221,'M8961F20','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5222,'M8961F21','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5223,'M8961F22','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5224,'M8961F23','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5225,'M8961F24','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5226,'M8961F25','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5227,'M8961F26','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5228,'M8961F27','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5229,'M8961F28','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5230,'M8961F29','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5231,'M8961F30','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5232,'M8961F31','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5233,'M8961F32','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5234,'M8961F33','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5235,'M8961F34','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5236,'M8961F35','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5237,'M8961F36','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5238,'M8961F37','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5239,'M8961F38','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5240,'M8961F39','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5241,'M8961F40','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5242,'M8961F41','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5243,'M8961F42','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5244,'M8961F43','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5245,'M8961F44','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5246,'M8961F45','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5247,'M8961F46','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5248,'M8961F47','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5249,'M8961F48','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5250,'M8961F50','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5251,'M8961F51','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5252,'M8961F52','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5253,'M8961F53','Meter,  1 1/2 T-10 C/F RW2F41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5254,'M8961F54','Meter,  1 1/2 T-10 C/F RW2F41S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5255,'M8961F55','Meter,  1 1/2 T-10 C/F RW2F41S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5256,'M8961F56','Meter,  1 1/2 T-10 GAL RW2G41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5257,'M8961F57','Meter,  1 1/2 T-10 GAL RW2G41S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5258,'M8961F58','Meter,  1 1/2 T-10 C/F R62F42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5259,'M8961F59','Meter,  1 1/2 T-10 C/F R62F42S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5260,'M8961F60','Meter,  1 1/2 T-10 GAL R62G42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5261,'M8961F61','Meter,  1 1/2 T-10 C/F R72F42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5262,'M8961F62','Meter,  1 1/2 T-10 C/F R72F42S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5263,'M8961F63','Meter,  1 1/2 T-10 GAL R72G42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5264,'M8961F64','Meter,  1 1/2 T-10 GAL ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5265,'M8961F65','Meter,  1 1/2 T-10 GAL ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5266,'M8961F66','Meter,  1 1/2 T-10 C/F ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5267,'M8961F67','Meter,  1 1/2 T-10 GAL ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5268,'M8961F68','Meter,  1 1/2 T-10 GAL ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5269,'M8961F69','Meter,  1 1/2 T-10 GAL ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5270,'M8961F70','Meter,  1 1/2 T-10 GAL ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5271,'M8961F71','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5272,'M8961F72','Meter,  1 1/2 T-10 GAL ED2H12R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5273,'M8961F73','Meter,  1 1/2 T-10 C/F ED2H12R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5274,'M8961F74','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5275,'M8961F75','Meter,  1 1/2 T-10 GAL ED2H21R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5276,'M8961F76','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5277,'M8961F77','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5278,'M8961F78','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5279,'M8961F79','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5280,'M8961F80','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5281,'M8961F81','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5282,'M8961F82','Meter,  1 1/2 T-10 C/F ED2H11R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5283,'M8970101','Meter,  1 T-8 C/F R71F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5284,'M8970102','Meter,  1 T-8 C/F R71F32S016');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5285,'M8980301','Meter,  3 TRD TUR C/F RW3F21');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5286,'M8980302','Meter,  3 TRD TUR C/F RW3F21SG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5287,'M8980303','Meter,  3 TRD TUR C/F R73F22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5288,'M8980304','Meter,  3 TRD TUR C/F R73F22S0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5289,'M8980305','Meter,  3 TRD TUR GAL R73G22');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5290,'M8980401','Meter,  4 TRD TUR C/F RW3F31');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5291,'M8980402','Meter,  4 TRD TUR C/F RW3F31SG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5292,'M8980403','Meter,  4 TRD TUR C/F R63F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5293,'M8980404','Meter,  4 TRD TUR C/F R73F32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5294,'M8980405','Meter,  4 TRD TUR C/F R73F32S0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5295,'M8980406','Meter,  4 TRD TUR C/F R73F32SA');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5296,'M8980407','Meter,  4 TRD TUR GAL R73G32');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5297,'M8980601','Meter,  6 TRD TUR C/F RW3F41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5298,'M8980602','Meter,  6 TRD TUR GAL RW3G41');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5299,'M8980603','Meter,  6 TRD TUR C/F R73F42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5300,'M8980604','Meter,  6 TRD TUR C/F R73F42S0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5301,'M8980605','Meter,  6 TRD TUR C/F R73F42SA');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5302,'M8980606','Meter,  6 TRD TUR GAL R73G42');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5303,'M8980801','Meter,  8 TRD TUR C/F R73F52S0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5304,'M8990301','Meter,  3 TRU/FLO C/F EC3BR8FS');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5305,'M8990302','Meter,  3 TRU/FLO GAL EC3BR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5306,'M8990303','Meter,  3 TRU/FLO C/F EC3BRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5307,'M8990304','Meter,  3 TRU/FLO C/F EC3BRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5308,'M8990305','Meter,  3 TRU/FLO GAL EC3BRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5309,'M8990306','Meter,  3 TRU/FLO GAL EC3BRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5310,'M8990307','Meter,  3 TRU/FLO C/F  EC3BR7F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5311,'M8990308','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5312,'M8990309','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5313,'M8990310','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5314,'M8990311','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5315,'M8990312','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5316,'M8990313','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5317,'M8990314','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5318,'M8990315','Meter,  3 TRU/FLO C/F EC3BR7F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5319,'M8990316','Meter,  3 TRU/FLO C/F EC3BR7F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5320,'M8990317','Meter,  3 TRU/FLO GAL EC3BR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5321,'M8990318','Meter,  3 TRU/FLO GAL EC3BR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5322,'M8990319','Meter,  3 TRU/FLO GAL EC3BR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5323,'M8990320','Meter,  3 TRU/FLO GAL EC3BR7G7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5324,'M8990321','Meter,  3 TRU/FLO GAL EC3BR7G7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5325,'M8990322','Meter,  3 TRU/FLO GAL EC3BR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5326,'M8990323','Meter,  3 TRU/FLO GAL EC3BR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5327,'M8990324','Meter,  3 TRU/FLO GAL EC3BR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5328,'M8990325','Meter,  3 TRU/FLO C/F EC3BRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5329,'M8990326','Meter,  3 TRU/FLO C/F EC3BR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5330,'M8990401','Meter,  4 TRU/FLO C/F EC3CR8FS');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5331,'M8990402','Meter,  4 TRU/FLO GAL EC3CR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5332,'M8990403','Meter,  4 TRU/FLO C/F EC3CRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5333,'M8990404','Meter,  4 TRU/FLO GAL EC3CRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5334,'M8990405','Meter,  4 TRU/FLO GAL EC3CRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5335,'M8990406','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5336,'M8990407','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5337,'M8990408','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5338,'M8990409','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5339,'M8990410','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5340,'M8990411','Meter,  4 TRU/FLO C/F EC3CR7F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5341,'M8990412','Meter,  4 TRU/FLO C/F EC3CR7F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5342,'M8990413','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5343,'M8990414','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5344,'M8990415','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5345,'M8990416','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5346,'M8990417','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5347,'M8990418','Meter,  4 TRU/FLO GAL EC3CR7G7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5348,'M8990419','Meter,  4 TRU/FLO GAL EC3CR7G7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5349,'M8990420','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5350,'M8990421','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5351,'M8990422','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5352,'M8990423','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5353,'M8990424','Meter,  4 TRU/FLO C/F EC3CRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5354,'M8990425','Meter,  4 TRU/FLO GAL EC3CR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5355,'M8990426','Meter,  4 TRU/FLO C/F EC3CR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5356,'M8990601','Meter,  6 TRU/FLO GAL EC3DRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5357,'M8990602','Meter,  6 TRU/FLO GAL EC3DRWG1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5358,'M8990603','Meter,  6 TRU/FLO C/F EC3DR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5359,'M8990604','Meter,  6 TRU/FLO C/F EC3DR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5360,'M8990605','Meter,  6 TRU/FLO C/F EC3DR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5361,'M8990606','Meter,  6 TRU/FLO C/F EC3DR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5362,'M8990607','Meter,  6 TRU/FLO C/F EC3DR7F7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5363,'M8990608','Meter,  6 TRU/FLO GAL EC3DR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5364,'M8990609','Meter,  6 TRU/FLO GAL EC3DR7G7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5365,'M8990610','Meter,  6X8 TRU/FLO C/F EC3ER7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5366,'M8990611','Meter,  6 TRU/FLO GAL EC3DR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5367,'M8990612','Meter,  6 TRU/FLO GAL EC3DR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5368,'M8990613','Meter,  6 TRU/FLO GAL EC3DR7GE');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5369,'M8990614','Meter,  6 TRU/FLO C/F EC3DR7F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5370,'M8990615','Meter,  6 TRU/FLO GAL EC3DR7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5371,'M8990616','Meter,  6 TRU/FLO C/F EC3DRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5372,'M8990617','Meter,  6 TRU/FLO C/F EC3DRWF1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5373,'M8990801','Meter,  8 TRU/FLO GAL EC3ER7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5374,'M8990802','Meter,  8 TRU/FLO GAL EC3ER7G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5375,'S9020404','Bend,  4" PVC Sewer Gask  22 1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5376,'S9020606','Bend,  6" PVC Sewer Gask  22 1');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5377,'S9020808','Bend,  8" PVC Sewer Gask 22 1/');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5378,'S9030404','Bend,  4" PVC Sewer Gask  45°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5379,'S9030606','Bend,  6" PVC Sewer Gask  45°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5380,'S9030808','Bend,  8" PVC Sewer Gask 45° B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5381,'S9050606','Bend,  6" PVC Sewer Gask  90°');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5382,'S9050808','Bend,  8" PVC Sewer Gask 90° B');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5383,'S9261515','Coupling, 15" Fernco Clay X CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5384,'S9261818','Coupling, 18" Fernco Clay X CI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5385,'S9300504','Coupling,  5" X  4" Fernco CI/');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5386,'W0030202','Corp, 2" Taper X PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5387,'W0051C1C','Corp, 1 1/4" Taper X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5388,'W0090202','Corp, 2" MIP X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5389,'W0141C1C','Valve,  1 1/4" Ball Meter FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5390,'W0220101M','Ball/Curb Stop, 1" FIP X FIP M');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5391,'W0220202M','Ball/Curb Stop, 2" FIP X FIP M');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5392,'W0220H0HM','Ball/Curb Stop,  3/4" FIP X FI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5393,'W0230101M','Ball/Curb Stop, 1" CF X FIP MI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5394,'W0230202M','Ball/Curb Stop, 2" CF X FIP MI');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5395,'W0230H0HM','Ball/Curb Stop,  3/4" CF X FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5396,'W0250101M','Ball/Curb Stop, 1" CC X FIP Mi');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5397,'W0250202M','Ball/Curb Stop, 2" CC X FIP Mi');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5398,'W0250H0HM','Ball/Curb Stop,  3/4" CC X FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5399,'W0260H0G','Valve,   3/4" X  5/8" Angle Me');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5400,'W0281F1F','Valve,  1 1/2" Angle Meter FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5401,'W0300101','Valve,  1" Angle Meter PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5402,'W0300H0H','Valve,   3/4" Angle Meter PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5403,'W031021F','Valve,  2" X 1 1/2" Pressure R');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5404,'W0311F1C','Valve,  1 1/2" X 1 1/4" Pressu');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5405,'W0360H0H','Coupling,   3/4" IPC X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5406,'W043010H','Coupling,  1" X  3/4" PC X PC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5407,'W050010H','Coupling,  1" X  3/4" CF X CF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5408,'W0520101','Coupling,  1" CF X FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5409,'W0520H0H','Coupling,   3/4" CF X FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5410,'W0610G0H','Coupling,   5/8" X  3/4" LC X');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5411,'W0620101','Coupling,  1" IPC X FIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5412,'W0640G0H','Valve,   5/8" X 3/4" Yoke Angl');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5413,'W0660H0G','Coupling,   3/4" X  5/8" LC X');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5414,'W0710101','Coupling,  1" PVCC X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5415,'W0710H0H','Coupling,   3/4" PVCC X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5416,'W0790H01','Coupling,   3/4" X 1" CC X CC');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5417,'W0821F01','Reducer,  1 1/2" X 1" BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5418,'W0821F0H','Reducer,  1 1/2" X  3/4" BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5419,'W0822F02','Reducer,  2 1/2" X 2" BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5420,'W0910H0H','UBR,  3/4" CF X MIP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5421,'W0921F01','YBR, 1 1/2" X 1" MIP X CC 2WY');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5422,'W0940201','YBR, 2" X 1" CF X CF 2WY');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5423,'W096010H','YBR, 1" X  3/4" CC X CC 2WY');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5424,'W106011F','Nipple, 1" X  1 1/2" BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5425,'M8760001','Meter MIU, R900 V3 Wall 12510-');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5426,'M8760002','Meter MIU, R900 V3 Pit  6'' 125');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5427,'M8760003','Meter MIU, R900 V3 Pit 25'' 125');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5428,'M8760004','Meter MIU, Bracket, Direct Mou');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5429,'M8760009','Meter MIU, Antenna, R900i Pit');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5430,'M8760010','Meter MIU, Antenna, R900i Pit');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5431,'M8760032','Meter MIU, Receptacle wall ass');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5432,'M8760033','Meter MIU, Receptacle comp  6''');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5433,'M8760034','Meter MIU, Receptacle comp 25''');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5434,'M8760035','Meter MIU, Receptacle assy  6''');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5435,'M8760036','Meter MIU, Receptacle assy 25''');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5436,'M8760037','Meter MIU, R900 V3 25'' potted');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5437,'M8760042','Meter Strainer Kit,  8" HP F/S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5438,'M8760043','Meter Strainer Kit, 12" NEP HP');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5439,'M8760045','Meter Spacer Adapter Kit, for');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5440,'M8760046','Meter Trico/E  2" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5441,'M8760047','Meter Trico/E  3" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5442,'M8760048','Meter Trico/E  4" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5443,'M8760049','Meter Trico/E  6" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5444,'M8760050','Meter Trico/E  8" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5445,'M8760051','Meter Trico/E 10" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5446,'M8760052','Meter Trico/E 12" Register HPT');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5447,'M8760053','Meter MIU, R900 V3 Pit 6'' pott');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5448,'M8760055','Register,  5/8" CF Radio Read');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5449,'M8760056','Register, 2" CF T-10 20'' anten');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5450,'M8770303','Meter Strainer,  3" Fire Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5451,'M8770404','Meter Strainer,  4" Fire Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5452,'M8770808','Meter Strainer,  8" Fire Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5453,'M8771010','Meter Strainer, 10" Fire Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5454,'M8771212','Meter Strainer, 12" Fire Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5455,'M8780G00','Meter,   5/8 BFLOW DB GAL EB1A');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5456,'M8820202','Meter Strainer,  2" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5457,'M8820303','Meter Strainer,  3" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5458,'M8820404','Meter Strainer,  4" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5459,'M8820606','Meter Strainer,  6" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5460,'M8820808','Meter Strainer,  8" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5461,'M8821010','Meter Strainer, 10" Meter');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5462,'M8830001','Meter,   3/4 SRII ECR/WP 57505');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5463,'M8830002','Meter,   3/4 SRII ECR-DM 57506');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5464,'M8830003','Meter,   3/4 SRII TRPL 5750696');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5465,'M8830004','Meter,   5/8 SRII ECR/WP 57505');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5466,'M8830005','Meter,   5/8 SRII ECR  5750596');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5467,'M8830006','Meter,   5/8 SRII TRPL 5750596');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5468,'M8830007','Meter,   5/8X3/4 SRII ECR/WP 5');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5469,'M8830008','Meter,   5/8X3/4 SRII ECR-DM 5');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5470,'M8830009','Meter,   5/8X3/4 SRII TRPL 575');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5471,'M8830010','Meter,  1 SRII ECR/WP 57508968');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5472,'M8830011','Meter,  1 SRII ECR-DM 57508968');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5473,'M8830012','Meter,  1 SRII TRPL 5750896870');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5474,'M8830013','Meter,  1 1/2 SR ECR 508109086');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5475,'M8830014','Meter,  1 1/2 SR ECR/WP 508109');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5476,'M8830015','Meter,  1 1/2 SR TRPL 50810908');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5477,'M8830016','Meter,  2 SR ECR 5081290860002');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5478,'M8830017','Meter,  2 SR ECR/WP 5081290860');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5479,'M8830018','Meter,  2 SR TRPL 508129087090');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5480,'M8840001','Meter,  1 1/2 OMNI T2 T11XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5481,'M8840002','Meter,  2 OMNI T2 T21XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5482,'M8840003','Meter,  2 OMNI T T23XXXXF4AA0X');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5483,'M8840004','Meter,  3 OMNI T2 T31XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5484,'M8840005','Meter,  3 OMNI T2 T33XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5485,'M8840006','Meter,  4 OMNI T2 T41XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5486,'M8840007','Meter,  4 OMNI T2 T43XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5487,'M8840008','Meter,  6 OMNI T2 T61XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5488,'M8840009','Meter,  6 OMNI T2 T63XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5489,'M8850001','Meter,  1 1/2 OMNI C2 C11XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5490,'M8850002','Meter,  2 OMNI C2 C21XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5491,'M8850003','Meter,  2 OMNI C2 C23XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5492,'M8850004','Meter,  3 OMNI C2 C31XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5493,'M8850005','Meter,  3 OMNI C2 C33XXXXF4AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5494,'M8850006','Meter,  4 OMNI C2 C41XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5495,'M8850007','Meter,  4 OMNI C2 C43XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5496,'M8850008','Meter,  6 OMNI C2 C61XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5497,'M8850009','Meter,  6 OMNI C2 C63XXXXF3AA0');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5498,'M8860001','Meter,  1 1/2 OMNI T2 MMP T19X');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5499,'M8860002','Meter,  2 OMNI T2 MMP T29XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5500,'M8860003','Meter,  3 OMNI T2 MMP T39XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5501,'M8860004','Meter,  4 OMNI T2 MMP T49XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5502,'M8860005','Meter,  6 OMNI T2 MMP T69XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5503,'M8870301','Meter,  3 FIRE HYD C/F R83F8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5504,'M8870302','Meter,  3 FIRE HYD GAL R83G8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5505,'M8870303','Meter,  3 FIRE HYD C/F ET2BR8F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5506,'M8870304','Meter,  3 FIRE HYD C/F ET2BR8F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5507,'M8870305','Meter,  3 FIRE HYD C/F ET2BR8F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5508,'M8870306','Meter,  3 FIRE HYD GAL ET2BR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5509,'M8870307','Meter,  3 FIRE HYD GAL ET2BR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5510,'M8870308','Meter,  3 FIRE HYD GAL ET2BR8G');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5511,'M8880001','Meter,  1 1/2 OMNI C2 MMP C19X');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5512,'M8880002','Meter,  2 OMNI C2 MMP C29XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5513,'M8880003','Meter,  3 OMNI C2 MMP C39XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5514,'M8880004','Meter,  4 OMNI C2 MMP C49XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5515,'M8880005','Meter,  6 OMNI C2 MMP C69XXXXF');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5516,'M8890401','Meter,  4 HP FIRE GAL ET5C1RWG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5517,'M8890600','Meter,  6 HP FIRE GAL ET5D1RWG');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5518,'M8890601','Meter,  6 HP FIRE C/F ET5D1R7F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5519,'M8900001','Meter, Model 510R 1-Port 53961');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5520,'M8900002','Meter, Model 510R 2-Port 53961');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5521,'M8900003','Meter, Model 510R 1-Port 53961');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5522,'M8900004','Meter, Model 520R 1-Port 53961');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5523,'M8900005','Meter, Model 520R 2-Port 53961');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5524,'M8920401','Meter,  4 HP PRCTS C/F EP4C1R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5525,'M8920402','Meter,  4 HP PRCTS GAL EP4C1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5526,'M8920403','Meter,  4 HP PRCTS C/F EP4C1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5527,'M8920404','Meter,  4 HP PRCTS GAL EP4C1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5528,'M8920405','Meter,  4 HP PRCTS GAL EP4C1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5529,'M8920406','Meter,  4 HP PRCTS C/F EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5530,'M8920407','Meter,  4 HP PRCTS C/F EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5531,'M8920408','Meter,  4 HP PRCTS GAL EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5532,'M8920409','Meter,  4 HP PRCTS GAL EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5533,'M8920410','Meter,  4 HP PRCTS GAL EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5534,'M8920411','Meter,  4 HP PRCTS GAL EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5535,'M8920412','Meter,  4 HP PRCTS GAL RW5G61');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5536,'M8920413','Meter,  4 HP PRCTS GAL RW5G61S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5537,'M8920414','Meter,  4 HP PRCTS C/F R75F62');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5538,'M8920415','Meter,  4 HP PRCTS C/F R75F62S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5539,'M8920416','Meter,  4 HP PRCTS C/F R75F62S');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5540,'M8920417','Meter,  4 HP PRCTS GAL R75G62');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5541,'M8920418','Meter,  4 HP PRCTS C/F EP4C1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5542,'M8920419','Meter,  4 HP PRCTS GAL EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5543,'M8920420','Meter,  4 HP PRCTS C/F EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5544,'M8920421','Meter,  4 HP PRCTS GAL EP4C1R7');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5545,'M8920601','Meter,  6 HP PRCTS C/F EP4D1R8');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5546,'M8920602','Meter,  6 HP PRCTS GAL EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5547,'M8920603','Meter,  6 HP PRCTS C/F EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5548,'M8920604','Meter,  6 HP PRCTS GAL EP4D1RW');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5549,'W1061F1H','Nipple, 1 1/2" X  1 3/4" BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5550,'W1111C0F','Bushing, 1 1/4" X  1/2"  BR');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5551,'W1151F1F','Plug,  1 1/2" Insert BR Taper');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5552,'W1200G0H','Bend,   5/8" X 3/4" BR FIP X F');
INSERT INTO [Materials] ([MaterialID], [PartNumber], [Description]) VALUES (5553,'W1202F2F','Bend,  2 1/2" BR FIP 90°');
COMMIT;
RAISERROR (N'[dbo].[Materials]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[Materials] ON;

-- disable unused parts
DELETE FROM [Materials]
WHERE [MaterialID] IN (18,21,25,26,27,28,29,30,31,33,38,81,92,186,188,189,193,200,443,444,445,446,472,473,474,480,481,493,496,530,532,558,569,629,635,649,651);

-- insert new operating center stocked materials
INSERT INTO [OperatingCenterStockedMaterials] ([MaterialID], [OperatingCenterID])
SELECT
MaterialID,
(SELECT [RecID] FROM [tblOpCntr] WHERE [OpCntr] = 'NJ3')
FROM
Materials
WHERE
[PartNumber] IN
('W0220202',
'W0420202',
'W0430202',
'W0680101',
'W0480202',
'W0490202',
'W1660101',
'W2042000',
'W2380000',
'W2390000',
'W7000404',
'W7000604',
'W7000606',
'W7000804',
'W7000806',
'W7000808',
'W7001006',
'W7001204',
'W7001206',
'W7001208',
'W7001210',
'W7001212',
'W7001606',
'W7001608',
'W7001610',
'W7001612',
'W7001616',
'W7002408',
'W7002412',
'W7002416',
'W7002424',
'W7040606',
'W7040806',
'W7041006',
'W7041206',
'W7041606',
'W7042406',
'W7021616',
'W7100606',
'W7100808',
'W7101206',
'W7101208',
'W7101212',
'W7101608',
'W7101612',
'W7440201',
'W7440401',
'W7440601',
'W7440801',
'W7441001',
'W7441201',
'W8040404',
'W8040606',
'W8040808',
'W8041010',
'W8041212',
'W8041616',
'W8050404',
'W8050606',
'W8050808',
'W8051212',
'W8000404',
'W8000606',
'W8000808',
'W8010404',
'W8010606',
'W8010808',
'W3381616',
'W8440606',
'W8441010',
'W8441212',
'W8441616',
'W8443030',
'W8451010',
'W8451212',
'W8451616',
'W8452424',
'W8570202',
'W8570404',
'W8570606',
'W8570808',
'W8571010',
'W8571212',
'W3000404',
'W3000606',
'W3000808',
'W3001010',
'W3001212',
'W3001616',
'W3002424',
'W3003030',
'W3040404',
'W3040606',
'W3040808',
'W3041010',
'W3041212',
'W3041616',
'W3250404',
'W3250606',
'W3250808',
'W3251212',
'W3251616',
'W3260404',
'W3260606',
'W3260808',
'W3261212',
'W3261616',
'W3262020',
'W3262424',
'W3270404',
'W3270606',
'W3270808',
'W3271212',
'W3271616',
'W3272424',
'W3280606',
'W3280808',
'W3281212',
'W3281616',
'W3282424',
'W3360808',
'W3361616',
'W3371212',
'W3371616',
'W3600612',
'W3600618',
'W3600812',
'W3600818',
'W3660604',
'W3660804',
'W3660806',
'W3661006',
'W3661206',
'W3661208',
'W3661210',
'W3661606',
'W3661608',
'W3661612',
'W3662016',
'W7700402',
'W7700404',
'W7700602',
'W7700606',
'W7700802',
'W7700808',
'W7701202',
'W7701212',
'W7701616',
'W7750402',
'W7750404',
'W7750602',
'W7750604',
'W7750606',
'W7750802',
'W7750808',
'W7751202',
'W7751212',
'W7751616',
'W7770602',
'W7770606',
'W7770802',
'W7770808',
'W7771202',
'W7771616',
'W4000650',
'W4270H0H',
'W4270101',
'W4270202',
'W2043600',
'W0410101',
'W041010H',
'W0410H0H',
'W0420101',
'W0420H0H',
'W0430101',
'W0430H0H',
'W0470101',
'W0470H0H',
'W0480101',
'W0490101',
'W0790101',
'W079010H',
'W0790202',
'W0790H0H',
'W0961F01',
'W1550101',
'W1550G07',
'W1550G0H',
'W1590G0G',
'W1600G0H',
'W1660202',
'W1670202',
'W1900202',
'W1901F1F',
'W1921F1F',
'W1930202',
'W2002030',
'W2003636',
'W2041800',
'W2400000',
'W2850606',
'W3760606',
'W3762424',
'W3761010',
'W3761616',
'W3850404',
'W3850606',
'W3850808',
'W3851212',
'W3851616',
'W4002450',
'W4022C2C',
'W7240808',
'W7771212',
'W8011212',
'W8051616',
'S9060404',
'S9060606',
'S9080404',
'S9080606',
'S9120404',
'S9120606',
'S9260404',
'S9260808',
'S9280604',
'S9280808',
'S9300404',
'S9300808',
'W3852424',
'S9000404',
'S9000606',
'S9000808',
'S9001010',
'S9001212',
'S9001515',
'S9040404',
'S9040604',
'S9040606',
'S9040806',
'S9040808',
'S9041006',
'S9041206',
'S9080808',
'S9100606',
'S9160404',
'S9160606',
'S9180808',
'S9181212',
'S9200006',
'S9260406',
'S9260604',
'S9260606',
'S9260804',
'S9260806',
'S9261010',
'S9261212',
'S9280404',
'S9280606',
'S9280804',
'S9280806',
'S9281010',
'S9281212',
'S9300604',
'S9300606',
'S9300804',
'S9300806',
'S9301010',
'S9301212',
'S9301515',
'W0030101',
'W0050101',
'W0140101',
'W0140202',
'W0140H0H',
'W0141F1F',
'W0201F1F',
'W0250101',
'W0320202',
'W036010H',
'W0360H01',
'W042010H',
'W047010H',
'W047010H',
'W0480H01',
'W0480H0H',
'W049010H',
'W049021F',
'W0490H01',
'W0490H0H',
'W0491F1F',
'W0510101',
'W0510H01',
'W0510H0H',
'W0510H0H',
'W052010H',
'W053010H',
'W054010H',
'W0550101',
'W055010H',
'W0550G0H',
'W0550H01',
'W0570101',
'W057010H',
'W068010H',
'W0680H01',
'W0680H0H',
'W0740101',
'W0740H0H',
'W0800101',
'W0800202',
'W0800H0H',
'W0801C1C',
'W0801F1F',
'W0920201',
'W0980201',
'W0981F01',
'W1290101',
'W1290H0H',
'W1330202',
'W1370101',
'W1370H0H',
'W1371C1C',
'W1400101',
'W1480101',
'W1660G0G',
'W1661F1F',
'W1670202',
'W1671F1F',
'W1920202',
'W2001830',
'W2150000',
'W2770606',
'W2770808',
'W2771010',
'W2771212',
'W2850404',
'W2850604',
'W2850808',
'W2851212',
'W2920202',
'W2920404',
'W2920606',
'W2920808',
'W2921212',
'W3351616',
'W3601212',
'W3610612',
'W3610812',
'W3662416',
'W3760404',
'W3760808',
'W3761212',
'W3880404',
'W3880606',
'W3880808',
'W3881010',
'W3881212',
'W3881616',
'W3910H0H',
'W4000452',
'W4000850',
'W4001050',
'W4001250',
'W4001650',
'W4031650',
'W4170815',
'W4210101',
'W4210202',
'W4210H0H',
'W4211F1F',
'W5182013',
'W5182014',
'W5182015',
'W5182016',
'W5182017',
'W5182023',
'W5182024',
'W5182025',
'W5182026',
'W5182036',
'W7003020',
'W7003024',
'W7101616',
'W7240404',
'W7240604',
'W7240606',
'W7240804',
'W7240806',
'W7241008',
'W7241204',
'W7241206',
'W7241208',
'W7241212',
'W7241604',
'W7241606',
'W7241608',
'W7241612',
'W7250404',
'W7250604',
'W7250606',
'W7250804',
'W7250806',
'W7250808',
'W7251008',
'W7251204',
'W7251206',
'W7251208',
'W7251212',
'W7251604',
'W7251606',
'W7251608',
'W7251612',
'W7441F0H',
'W7442F01',
'W7701010',
'W7702424',
'W7751010',
'W7752424',
'W7772424',
'W8001010',
'W8001212',
'W8440808')
