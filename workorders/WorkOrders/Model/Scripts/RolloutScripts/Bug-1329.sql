
INSERT INTO WorkOrderPurposes VALUES('Hurricane Sandy')

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [WorkCategoryID], [TimeToComplete], [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])

SELECT 169, 'SERVICE OFF AT MAIN-STORM RESTORATION',		4, 4, 2,   2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 170, 'SERVICE OFF AT CURB STOP-STORM RESTORATION',	4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 171, 'SERVICE OFF AT METER PIT-STORM RESTORATION',	4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 172, 'VALVE TURNED OFF STORM RESTORATION',			1, 2, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 173, 'MAIN REPAIRED-STORM RESTORATION',				3, 3, 4,   2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 174, 'MAIN REPLACED - STORM RESTORATION',			3, 3, 8,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 175, 'HYDRANT TURNED OFF - STORM RESTORATION',		2, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 176, 'HYDRANT REPLACED - STORM RESTORATION',			2, 5, 4,   1,   1, 100, 1,   1, 1, 0, 0
SELECT 177, 'VALVE INSTALLATION - STORM RESTORATION',	1, 2, 4,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 178, 'VALVE REPLACEMENT - STORM RESTORATION',	1, 2, 4,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 179, 'CURB BOX LOCATE - STORM RESTORATION',		4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
SELECT 190, 'METER PIT LOCATE - STORM RESTORATION',		4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

update workdescriptions set [Description] = replace([description], 'REPAIRED', 'REPAIR') where workDescriptionId in (173,174,176)
update workdescriptions set [Description] = replace([description], 'REPLACED', 'REPLACE') where workDescriptionId in (173,174,176)

SET IDENTITY_INSERT [dbo].[Materials] ON;
insert into Materials(MaterialID,[Description], PartNumber, IsActive) Values(5557, 'Resetter 5/8" X 7"','1405179',1)
insert into Materials(MaterialID,[Description], PartNumber, IsActive) Values(5558, 'Resetter 5/8" X 12"','1405182',1)
insert into Materials(MaterialID,[Description], PartNumber, IsActive) Values(5559, 'Resetter 1" X 12"','1405166',1)
SET IDENTITY_INSERT [dbo].[Materials] OFF;

insert into OperatingCenterStockedMaterials(OperatingCenterID, MaterialID) Values(10, 5557)
insert into OperatingCenterStockedMaterials(OperatingCenterID, MaterialID) Values(10, 5558)
insert into OperatingCenterStockedMaterials(OperatingCenterID, MaterialID) Values(10, 5559)