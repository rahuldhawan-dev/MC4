-----------------------------------------------------------------------------------------------------
---------------------------------------RESTORATION TYPE COSTS----------------------------------------
-----------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[RestorationTypeCosts] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[RestorationTypeCosts]([RestorationTypeCostID], [OperatingCenterID], [RestorationTypeID], [Cost])
------------------------------------NJ7------------------------------------
SELECT '1', '10', '1', '12' UNION ALL
SELECT '2', '10', '2', '12' UNION ALL
SELECT '3', '10', '3', '15' UNION ALL
SELECT '4', '10', '4', '27' UNION ALL
SELECT '5', '10', '5', '27' UNION ALL
SELECT '6', '10', '6', '15' UNION ALL
SELECT '7', '10', '7', '5' UNION ALL
SELECT '8', '10', '8', '15' UNION ALL
------------------------------------NJ4------------------------------------
SELECT '9', '14', '1', '12' UNION ALL
SELECT '10', '14', '2', '12' UNION ALL
SELECT '11', '14', '3', '15' UNION ALL
SELECT '12', '14', '4', '27' UNION ALL
SELECT '13', '14', '5', '27' UNION ALL
SELECT '14', '14', '6', '15' UNION ALL
SELECT '15', '14', '7', '5' UNION ALL
SELECT '16', '14', '8', '15' UNION ALL
------------------------------------EW3------------------------------------
SELECT '17', '17', '1', '12' UNION ALL
SELECT '18', '17', '2', '12' UNION ALL
SELECT '19', '17', '3', '15' UNION ALL
SELECT '20', '17', '4', '27' UNION ALL
SELECT '21', '17', '5', '27' UNION ALL
SELECT '22', '17', '6', '15' UNION ALL
SELECT '23', '17', '7', '5' UNION ALL
SELECT '24', '17', '8', '15' UNION ALL
------------------------------------EW1------------------------------------
SELECT '25', '15', '1', '12' UNION ALL
SELECT '26', '15', '2', '12' UNION ALL
SELECT '27', '15', '3', '15' UNION ALL
SELECT '28', '15', '4', '27' UNION ALL
SELECT '29', '15', '5', '27' UNION ALL
SELECT '30', '15', '6', '15' UNION ALL
SELECT '31', '15', '7', '5' UNION ALL
SELECT '32', '15', '8', '15' UNION ALL
------------------------------------EW4------------------------------------
SELECT '33', '19', '1', '12' UNION ALL
SELECT '34', '19', '2', '12' UNION ALL
SELECT '35', '19', '3', '15' UNION ALL
SELECT '36', '19', '4', '27' UNION ALL
SELECT '37', '19', '5', '27' UNION ALL
SELECT '38', '19', '6', '15' UNION ALL
SELECT '39', '19', '7', '5' UNION ALL
SELECT '40', '19', '8', '15' UNION ALL
------------------------------------LWC------------------------------------
SELECT '41', '18', '1', '12' UNION ALL
SELECT '42', '18', '2', '12' UNION ALL
SELECT '43', '18', '3', '15' UNION ALL
SELECT '44', '18', '4', '27' UNION ALL
SELECT '45', '18', '5', '27' UNION ALL
SELECT '46', '18', '6', '15' UNION ALL
SELECT '47', '18', '7', '5' UNION ALL
SELECT '48', '18', '8', '15' UNION ALL
------------------------------------NJ3------------------------------------
SELECT '49', '11', '1', '12' UNION ALL
SELECT '50', '11', '2', '12' UNION ALL
SELECT '51', '11', '3', '15' UNION ALL
SELECT '52', '11', '4', '27' UNION ALL
SELECT '53', '11', '5', '27' UNION ALL
SELECT '54', '11', '6', '15' UNION ALL
SELECT '55', '11', '7', '5' UNION ALL
SELECT '56', '11', '8', '15' UNION ALL
------------------------------------NJ5------------------------------------
SELECT '57', '13', '1', '12' UNION ALL
SELECT '58', '13', '2', '12' UNION ALL
SELECT '59', '13', '3', '15' UNION ALL
SELECT '60', '13', '4', '27' UNION ALL
SELECT '61', '13', '5', '27' UNION ALL
SELECT '62', '13', '6', '15' UNION ALL
SELECT '63', '13', '7', '5' UNION ALL
SELECT '64', '13', '8', '15' UNION ALL
------------------------------------NJ6------------------------------------
SELECT '65', '12', '1', '18' UNION ALL
SELECT '66', '12', '2', '18' UNION ALL
SELECT '67', '12', '3', '20' UNION ALL
SELECT '68', '12', '4', '30' UNION ALL
SELECT '69', '12', '5', '30' UNION ALL
SELECT '70', '12', '6', '10' UNION ALL
SELECT '71', '12', '7', '5' UNION ALL
SELECT '72', '12', '8', '10' UNION ALL
------------------------------------EW2------------------------------------
SELECT '73', '16', '1', '12' UNION ALL
SELECT '74', '16', '2', '12' UNION ALL
SELECT '75', '16', '3', '15' UNION ALL
SELECT '76', '16', '4', '27' UNION ALL
SELECT '77', '16', '5', '27' UNION ALL
SELECT '78', '16', '6', '15' UNION ALL
SELECT '79', '16', '7', '5' UNION ALL
SELECT '80', '16', '8', '15' UNION ALL
------------------------------------NJ9------------------------------------
SELECT 81, 26, 1, N'12' UNION ALL
SELECT 82, 26, 2, N'12' UNION ALL
SELECT 83, 26, 3, N'15' UNION ALL
SELECT 84, 26, 4, N'27' UNION ALL
SELECT 85, 26, 5, N'27' UNION ALL
SELECT 86, 26, 6, N'15' UNION ALL
SELECT 87, 26, 7, N'5' UNION ALL
SELECT 88, 26, 8, N'15' UNION ALL
------------------------------------NJ8------------------------------------
SELECT 89, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '1', '18' UNION ALL
SELECT 90, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '2', '18' UNION ALL
SELECT 91, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '3', '20' UNION ALL
SELECT 92, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '4', '30' UNION ALL
SELECT 93, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '5', '30' UNION ALL
SELECT 94, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '6', '10' UNION ALL
SELECT 95, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '7', '5' UNION ALL
SELECT 96, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), '8', '10' 

COMMIT;
RAISERROR (N'[dbo].[RestorationTypeCosts]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[RestorationTypeCosts] OFF;

-----------------------------------------------------------------------------------------------------
------------------------------------OPERATING CENTER ASSET TYPES-------------------------------------
-----------------------------------------------------------------------------------------------------

------------------------------------NJ7------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(10, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 4)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 9)

------------------------------------NJ4------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(14, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 4)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 5)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 6)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 7)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 9)

------------------------------------EW3------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(17, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(17, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(17, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(17, 4)

------------------------------------EW1------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(15, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(15, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(15, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(15, 4)

------------------------------------EW4------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(19, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(19, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(19, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(19, 4)

------------------------------------LWC------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(18, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(18, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(18, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(18, 4)

------------------------------------NJ3------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(11, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 4)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 5)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 6)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 7)
INSERT INTO [OperatingCenterAssetTypes] Values(11, 8)

------------------------------------NJ5------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(13, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(13, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(13, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(13, 4)

------------------------------------NJ6------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(12, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(12, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(12, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(12, 4)

------------------------------------EW2------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(16, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(16, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(16, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(16, 4)

------------------------------------NJ9------------------------------------
INSERT INTO [OperatingCenterAssetTypes] Values(26, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(26, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(26, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(26, 4)

------------------------------------NJ8------------------------------------
INSERT INTO [OperatingCenterAssetTypes] select (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), 1
INSERT INTO [OperatingCenterAssetTypes] select (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), 2
INSERT INTO [OperatingCenterAssetTypes] select (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), 3
INSERT INTO [OperatingCenterAssetTypes] select (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ8'), 4


-----------------------------------------------------------------------------------------------------
--------------------------------OPERATING CENTER SPOIL REMOVAL COSTS---------------------------------
-----------------------------------------------------------------------------------------------------
INSERT INTO [OperatingCenterSpoilRemovalCosts] ([OperatingCenterID], [Cost])
SELECT [OperatingCenterID], 75 FROM [OperatingCenters] WHERE [WorkOrdersEnabled] = 1


SET IDENTITY_INSERT [dbo].[SpoilStorageLocations] ON;

BEGIN TRANSACTION;
INSERT INTO [dbo].[SpoilStorageLocations]([SpoilStorageLocationID], [Name], [OperatingCenterID], [TownID], [StreetID])
SELECT 8, N'Oak Tree ', 15, 227, 42420 UNION ALL
SELECT 9, N'Oak Tree Road', 19, 227, 42420 UNION ALL
SELECT 15, N'well 4 Howell', 14, 189, 56342 
COMMIT;
RAISERROR (N'[dbo].[SpoilStorageLocations]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[SpoilStorageLocations] OFF;

UPDATE [operatingcenters] SET [PermitsOMUserName] = 'admin@mmsinc.com', [PermitsCapitalUserName] = 'admin@mmsinc.com'
