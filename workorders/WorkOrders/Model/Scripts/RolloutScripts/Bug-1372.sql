DECLARE @curMaterialID int;
DECLARE @curOpCenterID int;
DECLARE opCenters CURSOR FOR
SELECT DISTINCT
	OperatingCenterID
FROM
	OperatingCenterStockedMaterials;

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1410021', 'MTR BX,FR,RECSSD,18X11 1/2', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1410046', 'MTR BX,LID,RECSSD,TR,SM NUT,11 1/2', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1410023', 'MTR BX,FRM,RECSSD,20X11 1/2', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1409358', 'VLV_BOX,LID,DROP IN,CI,5 1/4X1 1/2H', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1409324', 'VLV_BOX,TOP,SL,NO FLG,CI,5 1/4X15H', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1411196', 'VLV_BOX,BOTTOM,SL,PL,5 1/4X48', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1410058', 'CURB_BOX,TOP/LID,SL,PL,2 1/2X26H', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters

INSERT INTO Materials (PartNumber, Description, IsActive) VALUES ('1410059', 'CURB_BOX,BOTTOM,SL,PL,2 1/2X36H', 1);
SELECT @curMaterialID = @@identity;

OPEN opCenters;

FETCH NEXT FROM opCenters INTO @curOpCenterID;

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO OperatingCenterStockedMaterials (MaterialID, OperatingCenterID) VALUES (@curMaterialID, @curOpCenterID);
	FETCH NEXT FROM opCenters INTO @curOpCenterID;
END

CLOSE opCenters
DEALLOCATE opCenters
