
-- This script will import the necessary data from the live site, for development and import.
-- You'll need to set the specific db you're using before running.

-- here we configure which operating centers' data we'll be grabbing:
-- TODO: This could be dynamic?
CREATE TABLE #tmpOpCntrs (OpCode char(3));
INSERT INTO #tmpOpCntrs SELECT 'nj3';
INSERT INTO #tmpOpCntrs SELECT 'nj4';
INSERT INTO #tmpOpCntrs SELECT 'nj5';
INSERT INTO #tmpOpCntrs SELECT 'nj7';
INSERT INTO #tmpOpCntrs SELECT 'ew3';
INSERT INTO #tmpOpCntrs SELECT 'ew1';
INSERT INTO #tmpOpCntrs SELECT 'ew4';
INSERT INTO #tmpOpCntrs SELECT 'lwc';
INSERT INTO #tmpOpCntrs SELECT 'ew2';
INSERT INTO #tmpOpCntrs SELECT 'nj6';
INSERT INTO #tmpOpCntrs SELECT 'nj9';
INSERT INTO #tmpOpCntrs SELECT 'nj8';


-----------------------------------------------------------------------------------------------------
--------------------------------------------TOWN SECTIONS--------------------------------------------
-----------------------------------------------------------------------------------------------------
/* WE ONLY NEED COORDINATES FOR THE SEWER MANHOLES AT THIS POINT */
SET IDENTITY_INSERT [Coordinates] ON 

INSERT INTO 
	[Coordinates] ([CoordinateID],[Latitude],[Longitude],[CustomIconID],[IconID],[AddressID])
SELECT
	[CoordinateID],[Latitude],[Longitude],[CustomIconID],[IconID],[AddressID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[Coordinates] C2
WHERE
	CoordinateID in (
		SELECT CoordinateID FROM [Fatman].[mapcall_2013_02_04].dbo.[SewerManholes] WHERE [TownID] in (Select TownID from Towns)
		UNION ALL 
		SELECT CoordinateID FROM [Fatman].[mapcall_2013_02_04].dbo.[StormWaterAssets] WHERE [TownID] in (Select TownID from Towns)
	)
AND
	NOT EXISTS (SELECT CoordinateID from Coordinates C where C.CoordinateID = C2.CoordinateID)
GO

SET IDENTITY_INSERT [Coordinates] OFF


--sewer manhole update
update sewermanholes set ManholeNumber = REPLACE(ManholeNumber, 'MAD', 'MHW')

--tblNJAWSizeServ
RAISERROR (N'Inserting service sizes...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [dbo].[tblNJAWSizeServ] ON;
INSERT INTO [dbo].[tblNJAWSizeServ]([Hyd], [Lat], [Main], [Meter], [RecID], [RecOrd], [Serv], [SizeServ], [Valve])
SELECT [Hyd], [Lat], [Main], [Meter], [RecID], [RecOrd], [Serv], [SizeServ], [Valve] FROM [Fatman].[mapcall_2013_02_04].dbo.[tblNJAWSizeServ] src where src.[RecId] not in (select [RecId] from [dbo].[tblNJAWSizeServ]);
SET IDENTITY_INSERT [dbo].[tblNJAWSizeServ] OFF;
GO

-----------------------------------------------------------------------------------------------------
--------------------------------OPERATING CENTER STOCKED MATERIALS-----------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting operating center stocked materials...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [OperatingCenterStockedMaterials] ON
GO

INSERT INTO [OperatingCenterStockedMaterials] ([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT
	[OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[OperatingCenterStockedMaterials] fattyMaterials
WHERE
	OperatingCenterID in (select operatingCenterID from OperatingCenters where OperatingCenterCode in (select OpCode from #tmpOpCntrs))


SET IDENTITY_INSERT [OperatingCenterStockedMaterials] OFF
GO

--ALTER TABLE dbo.Contractors CHECK CONSTRAINT FK_Contractors_Towns_CityID;


SET IDENTITY_INSERT dbo.ContractorsOperatingCenters ON;

INSERT INTO [ContractorsOperatingCenters] (ContractorOperatingCenterID, ContractorID, OperatingCenterID) 
  SELECT ContractorOperatingCenterID, ContractorID, OperatingCenterID FROM [Fatman].[mapcall_2013_02_04].dbo.ContractorsOperatingCenters

SET IDENTITY_INSERT dbo.ContractorsOperatingCenters OFF;
-- need to drop the temporary opcode table
DROP TABLE #tmpOpCntrs

-- THIS ISN'T THE REAL ONE FOR 1, It's really middletown, but i won't tell.
SET IDENTITY_INSERT dbo.SpoilStorageLocations ON;
INSERT INTO dbo.SpoilStorageLocations (SpoilStorageLocationID, Name, OperatingCenterID, TownID, StreetID) Values(1, 'Newman Springs Yard', 10, 64, 6970)
SET IDENTITY_INSERT dbo.SpoilStorageLocations OFF;
