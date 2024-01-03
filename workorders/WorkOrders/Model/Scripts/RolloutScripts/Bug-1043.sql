use [McProd]
GO

--- W0020H0H
-- NJ4
INSERT INTO [OperatingCenterStockedMaterials] (OperatingCenterID, MaterialID)
SELECT (SELECT OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ4'),
	   (SELECT MaterialID from Materials where PartNumber = 'W0020H0H');
-- NJ7
INSERT INTO [OperatingCenterStockedMaterials] (OperatingCenterID, MaterialID)
SELECT (SELECT OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ7'),
	   (SELECT MaterialID from Materials where PartNumber = 'W0020H0H');
--- W0020202
-- NJ4
INSERT INTO [OperatingCenterStockedMaterials] (OperatingCenterID, MaterialID)
SELECT (SELECT OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ4'),
	   (SELECT MaterialID from Materials where PartNumber = 'W0020202');
-- NJ7
INSERT INTO [OperatingCenterStockedMaterials] (OperatingCenterID, MaterialID)
SELECT (SELECT OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ7'),
	   (SELECT MaterialID from Materials where PartNumber = 'W0020202');
