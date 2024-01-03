use MCProd
GO

begin tran
GO

-- From the list in the bug, these three don't exist in the database yet, so add them first.
INSERT INTO [Materials] ([Description], [PartNumber], [IsActive]) VALUES ('20" Meter Pit Frame', '1410020', 1)
INSERT INTO [Materials] ([Description], [PartNumber], [IsActive]) VALUES ('20" Meter Pit Lid', '1410051', 1)
INSERT INTO [Materials] ([Description], [PartNumber], [IsActive]) VALUES ('8" x 8" Broken Pipe clamp, CI', '1409538', 1)

declare @materialId int 

set @materialId = (Select MaterialID from Materials where PartNumber = '1410020')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1410051')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1406265')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1406272')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1406273')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1409538')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1406278')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1407727')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1407728')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1407731')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL

	
set @materialId = (Select MaterialID from Materials where PartNumber = '1405226')
INSERT INTO OperatingCenterStockedMaterials 
	SELECT oc.[OperatingCenterID], @materialId as [MaterialId]
	FROM [OperatingCenters] oc
	LEFT JOIN [OperatingCenterStockedMaterials] ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID AND ocsm.MaterialID = @materialId
	WHERE oc.WorkOrdersEnabled = 1 AND ocsm.OperatingCenterID IS NULL



rollback tran