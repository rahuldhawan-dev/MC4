USE [McProd]
GO

INSERT INTO OperatingCenterStockedMaterials (OperatingCenterID, MaterialID)
SELECT
	(SELECT OperatingCenterID FROM OperatingCenters WHERE OperatingCenterCode = 'EW1'),
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W6220H0H')
INSERT INTO OperatingCenterStockedMaterials (OperatingCenterID, MaterialID)
SELECT
	(SELECT OperatingCenterID FROM OperatingCenters WHERE OperatingCenterCode = 'EW2'),
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W6220H0H')
INSERT INTO OperatingCenterStockedMaterials (OperatingCenterID, MaterialID)
SELECT
	(SELECT OperatingCenterID FROM OperatingCenters WHERE OperatingCenterCode = 'EW3'),
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W6220H0H')
INSERT INTO OperatingCenterStockedMaterials (OperatingCenterID, MaterialID)
SELECT
	(SELECT OperatingCenterID FROM OperatingCenters WHERE OperatingCenterCode = 'EW4'),
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W6220H0H')
INSERT INTO OperatingCenterStockedMaterials (OperatingCenterID, MaterialID)
SELECT
	(SELECT OperatingCenterID FROM OperatingCenters WHERE OperatingCenterCode = 'LWC'),
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W6220H0H')
