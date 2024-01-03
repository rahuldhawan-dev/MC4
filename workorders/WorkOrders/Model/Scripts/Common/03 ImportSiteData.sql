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
-----------------------------------------------VALVES------------------------------------------------
-----------------------------------------------------------------------------------------------------

RAISERROR (N'Inserting valves...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [tblNJAWValves] ON
GO

INSERT INTO [tblNJAWValves] ([BillInfo], [BlowOff], [BPUKPI], [Critical], [CriticalNotes], [CrossStreet],
[DateInst], [DateRetired], [DateTested], [Elevation], [Initiator], [InspFreq], [InspFreqUnit], [Lat],
[Lateral], [Lon], [Main], [MapPage], [NorPos], [OpCntr], [Opens], [ObjectID], [Path], [PrintedLabel],
[RecID], [Remarks], [Route], [SketchNum], [StNum], [StName], [TaskRetire], [Town], [Traffic], [Turns],
[TwnSection], [TypeMain], [ValCtrl], [ValLoc], [ValMake], [ValNum], [ValSuf], [ValType], [ValveSize],
[ValveStatus], [WONum], [DateAdded])
SELECT
	[BillInfo], [BlowOff], [BPUKPI], [Critical], [CriticalNotes], [CrossStreet], [DateInst],
[DateRetired], [DateTested], [Elevation], [Initiator], [InspFreq], [InspFreqUnit], [Lat], [Lateral],
[Lon], [Main], [MapPage], [NorPos], [OpCntr], [Opens], [ObjectID], [Path], [PrintedLabel], [RecID],
[Remarks], [Route], [SketchNum], [StNum], [StName], [TaskRetire], [Town], [Traffic], [Turns],
[TwnSection], [TypeMain], [ValCtrl], [ValLoc], [ValMake], [ValNum], [ValSuf], [ValType], [ValveSize],
[ValveStatus], [WONum], [DateAdded]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[tblNJAWValves]
WHERE
	[OpCntr] in (SELECT OpCode from #tmpOpCntrs);

SET IDENTITY_INSERT [tblNJAWValves] OFF
GO

-----------------------------------------------------------------------------------------------------
----------------------------------------------HYDRANTS-----------------------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting hydrants...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [tblNJAWHydrant] ON
GO

INSERT INTO [tblNJAWHydrant] ([ActRet], [BillInfo], [BPUKPI], [BranchLn],
[Critical], [CriticalNotes], [CrossStreet], [DateInst], [DateRemoved], [DateTested], [DEM],
[DepthBury], [DirOpen], [Elevation], [FireD], [Gradiant], [HydMake], [HydNum], [HydSize],
[HydStyle], [HydSuf], [Initiator], [InspFreq], [InspFreqUnit], [Lat], [LatSize], [LatValNum],
[LinkNum], [Location], [Lon], [MapPage], [OpCntr], [OutOfServ], [RecID], [Remarks], [Route],
[SizeofMain], [StName], [StNum], [Thread], [Town], [TwnSection], [TypeMain], [ValLoc], [WONum],
[DateAdded], [FireDistrictID])
SELECT
	[ActRet], [BillInfo], [BPUKPI], [BranchLn], [Critical], [CriticalNotes], [CrossStreet],
[DateInst], [DateRemoved], [DateTested], [DEM], [DepthBury], [DirOpen], [Elevation], [FireD],
[Gradiant], [HydMake], [HydNum], [HydSize], [HydStyle], [HydSuf], [Initiator], [InspFreq],
[InspFreqUnit], [Lat], [LatSize], [LatValNum], [LinkNum], [Location], [Lon], [MapPage], [OpCntr],
[OutOfServ], [RecID], [Remarks], [Route], [SizeofMain], [StName], [StNum], [Thread], [Town],
[TwnSection], [TypeMain], [ValLoc], [WONum], [DateAdded], [FireDistrictID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[tblNJAWHydrant]
WHERE
	[OpCntr] in (SELECT OpCode from #tmpOpCntrs);

SET IDENTITY_INSERT [tblNJAWHydrant] OFF
GO
-----------------------------------------------------------------------------------------------------
----------------------------------------------EMPLOYEES----------------------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting employees...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [tblPermissions] ON
GO

INSERT INTO [tblPermissions] ([Add1], [CDCCode], [CellNum], [City], [Company], [EMail], [EmpNum],
[FaxNum], [FullName], [Location], [Inactive], [OpCntr1], [OpCntr2], [OpCntr3], [OpCntr4], [OpCntr5],
[OpCntr6], [Password], [PhoneNum], [Region], [St], [UserInact], [UserLevel], [UserName], [Zip],
[FBUserName], [FBPassWord], [XYMUserName], [XYMPassword], [userID], [uid], [RecID], [DefaultOperatingCenterID])
SELECT
	[Add1], [CDCCode], [CellNum], [City], [Company], [EMail], [EmpNum], [FaxNum], [FullName],
[Location], [Inactive], [OpCntr1], [OpCntr2], [OpCntr3], [OpCntr4], [OpCntr5], [OpCntr6], [Password],
[PhoneNum], [Region], [St], [UserInact], [UserLevel], [UserName], [Zip], [FBUserName], [FBPassWord],
[XYMUserName], [XYMPassword], [userID], [uid], [RecID], [DefaultOperatingCenterID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[tblPermissions]

SET IDENTITY_INSERT [tblPermissions] OFF
GO

-----------------------------------------------------------------------------------------------------
------------------------------------------------TOWNS------------------------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting towns...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [Towns] ON
GO

INSERT INTO [Towns] ([Ab], [Address], [ContactName], [County], [DistrictID], [EmergContact], [EmergFax],
[EmergPhone], [Fax],
[FD1Contact], [FD1Fax], [FD1Phone], [Lat], [Lon],
[Phone], [TownID], [State],
[Town], [TownName], [Zip],
[Link], [AbbreviationTypeID])
SELECT
	Distinct [Ab], T.[Address], [ContactName], [County], [DistrictID], [EmergContact], [EmergFax], [EmergPhone],
[Fax], [FD1Contact],
[FD1Fax], [FD1Phone], [Lat], [Lon], [Phone], T.[TownID], T.[State],
[Town], [TownName], T.[Zip], [Link], [AbbreviationTypeID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[Towns] T
JOIN
	[Fatman].[mapcall_2013_02_04].dbo.[OperatingCentersTowns] OCT
ON
	OCT.TownID = T.TownID
JOIN
	[Fatman].[mapcall_2013_02_04].dbo.[OperatingCenters] OC
ON
	OC.OperatingCenterID = OCT.OperatingCenterID
WHERE
	OC.OperatingCenterCode in (select opCode from #tmpOpCntrs)
SET IDENTITY_INSERT [Towns] OFF
GO

-----------------------------------------------------------------------------------------------------
--------------------------------------------TOWN SECTIONS--------------------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting town sections...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [TownSections] ON
GO

INSERT INTO [TownSections] ([Abbreviation], [TownSectionID], [TownID], [Name])
SELECT
	Distinct a.[Abbreviation], a.[TownSectionID], a.[TownID], a.[Name]
FROM
	[Fatman].[mapcall_2013_02_04]].dbo.[TownSections] AS a
WHERE
	a.TownID IN (SELECT
				b.[TownID]
			FROM
				[Fatman].[mapcall_2013_02_04].dbo.[Towns] AS b
			JOIN
				[Fatman].[mapcall_2013_02_04].dbo.[OperatingCentersTowns] OCT
			ON
				OCT.TownID = b.TownID
			JOIN
				[Fatman].[mapcall_2013_02_04].dbo.[OperatingCenters] OC
			ON
				OC.OperatingCenterID = OCT.OperatingCenterID
			WHERE
				a.TownID = b.TownID
			AND
				OC.OperatingCenterCode in (select opCode from #tmpOpCntrs)
	)
SET IDENTITY_INSERT [TownSections] OFF
GO

-----------------------------------------------------------------------------------------------------
-----------------------------------------------STREETS-----------------------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting streets...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [Streets] ON
GO

INSERT INTO [Streets] ([StreetID], [FullStName], [InactSt], [StreetPrefix], [StreetName],
[StreetSuffix], [TownID])
SELECT
	s.[StreetID], s.[FullStName], s.[InactSt], s.[StreetPrefix], s.[StreetName], s.[StreetSuffix], s.[TownID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[Streets] as s
WHERE
	EXISTS (SELECT 1 FROM [Towns] as t WHERE t.[TownID] = s.[TownID])

SET IDENTITY_INSERT [Streets] OFF
GO

-----------------------------------------------------------------------------------------------------
---------------------------------------------OP CENTERS----------------------------------------------
-----------------------------------------------------------------------------------------------------
RAISERROR (N'Inserting operating centers...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [OperatingCenters] ON
GO

INSERT INTO [OperatingCenters] ([CSNum], [FaxNum], [MailAdd], [MailCo], [MailCSZ], [OperatingCenterCode],
	[OperatingCenterName],
	[ServContactNum], [HydInspFreq], [HydInspFreqUnit], [ValLgInspFreq], [ValLgInspFreqUnit],
	[ValSmInspFreq], [ValSmInspFreqUnit], [OperatingCenterID], [WorkOrdersEnabled])
SELECT
	[CSNum], [FaxNum], [MailAdd], [MailCo], [MailCSZ], [OperatingCenterCode],
	[OperatingCenterName], [ServContactNum],
	[HydInspFreq], [HydInspFreqUnit], [ValLgInspFreq], [ValLgInspFreqUnit], [ValSmInspFreq],
	[ValSmInspFreqUnit], [OperatingCenterID], 1
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[OperatingCenters] oc
WHERE
	oc.OperatingCenterCode
IN 
    (Select OpCode from #tmpOpCntrs)


SET IDENTITY_INSERT [OperatingCenters] OFF
GO

/* WE ONLY NEED COORDINATES FOR THE SEWER MANHOLES AT THIS POINT */
SET IDENTITY_INSERT [Coordinates] ON 

INSERT INTO 
	[Coordinates] ([CoordinateID],[Latitude],[Longitude],[CustomIconID],[IconID],[AddressID])
SELECT
	[CoordinateID],[Latitude],[Longitude],[CustomIconID],[IconID],[AddressID]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[Coordinates]
WHERE
	CoordinateID in (
		SELECT CoordinateID FROM [Fatman].[mapcall_2013_02_04].dbo.[SewerManholes] WHERE [TownID] in (Select TownID from Towns)
		UNION ALL 
		SELECT CoordinateID FROM [Fatman].[mapcall_2013_02_04].dbo.[StormWaterAssets] WHERE [TownID] in (Select TownID from Towns)
	)
GO

SET IDENTITY_INSERT [Coordinates] OFF


SET IDENTITY_INSERT [AssetStatuses] ON

INSERT INTO 
	[AssetStatuses] ([AssetStatusID], [Description]) 
SELECT 
	[AssetStatusID], [Description]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[AssetStatuses]

SET IDENTITY_INSERT [AssetStatuses] OFF

SET IDENTITY_INSERT [SewerManholes] ON

INSERT INTO [SewerManholes] (
	[SewerManholeID]
	,[OperatingCenterID]
	,[TownID]
	,[ManholeNumber]
	,[StreetID]
	,[IntersectingStreetID]
	,[TaskNumber]
	,[DateInstalled]
	,[DateRetired]
	,[MapPage]
	,[CoordinateID]
	,[CreatedBy]
	,[CreatedOn]
	,[AssetStatusID]
	,[OldNumber]
	,[SewerManholeMaterialID]
	,[DistanceFromCrossStreet]
	,[IsEpoxyCoated]
	,[Notes]
)
SELECT
	[SewerManholeID]
	,[OperatingCenterID]
	,[TownID]
	,[ManholeNumber]
	,[StreetID]
	,[IntersectingStreetID]
	,[TaskNumber]
	,[DateInstalled]
	,[DateRetired]
	,[MapPage]
	,[CoordinateID]
	,[CreatedBy]
	,[CreatedOn]
	,[AssetStatusID]
	,[OldNumber]
	,[SewerManholeMaterialID]
	,[DistanceFromCrossStreet]
	,[IsEpoxyCoated]
	,[Notes]
FROM
	[Fatman].[mapcall_2013_02_04].dbo.[SewerManholes]
WHERE
	[TownID] in (Select TownID from Towns)
GO

SET IDENTITY_INSERT [SewerManholes] OFF

--------------------------------StormWaterAssets--------------------------------
RAISERROR (N'Inserting storm water assets...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [dbo].[StormWaterAssetTypes] ON;

INSERT INTO [dbo].[StormWaterAssetTypes]([StormWaterAssetTypeID], [Description])
SELECT 1, N'Catch Basin' UNION ALL
SELECT 2, N'Stormwater Manhole'

SET IDENTITY_INSERT [dbo].[StormWaterAssetTypes] OFF;

SET IDENTITY_INSERT [dbo].[StormWaterAssets] ON;

INSERT INTO [StormWaterAssets]
		([StormWaterAssetID],
		[OperatingCenterID],
        [TownID], 
        [StormWaterAssetTypeID], 
        [AssetNumber], 
        [StreetID], 
        [IntersectingStreetID], 
        [TaskNumber], 
        [DateInstalled], 
        [DateRetired], 
        [MapPage], 
        [CoordinateID], 
        [CreatedBy], 
        [CreatedOn], 
        [AssetStatusID], 
        [OldNumber], 
        [MaterialID], 
        [DistanceFromCrossStreet], 
        [IsEpoxyCoated], 
        [Notes])
SELECT [StormWaterAssetID],
		[OperatingCenterID],
        [TownID], 
        [StormWaterAssetTypeID], 
        [AssetNumber], 
        [StreetID], 
        [IntersectingStreetID], 
        [TaskNumber], 
        [DateInstalled], 
        [DateRetired], 
        [MapPage], 
        [CoordinateID], 
        [CreatedBy], 
        [CreatedOn], 
        [AssetStatusID], 
        [OldNumber], 
        [MaterialID], 
        [DistanceFromCrossStreet], 
        [IsEpoxyCoated], 
        [Notes]
FROM
		[Fatman].[mapcall_2013_02_04].dbo.[StormWaterAssets]
WHERE
	[TownID] in (Select TownID from Towns)

SET IDENTITY_INSERT [dbo].[StormWaterAssets] OFF;


--tblNJAWSizeServ
RAISERROR (N'Inserting service sizes...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT [dbo].[tblNJAWSizeServ] ON;
INSERT INTO [dbo].[tblNJAWSizeServ]([Hyd], [Lat], [Main], [Meter], [RecID], [RecOrd], [Serv], [SizeServ], [Valve])
SELECT [Hyd], [Lat], [Main], [Meter], [RecID], [RecOrd], [Serv], [SizeServ], [Valve] FROM [Fatman].[mapcall_2013_02_04].dbo.[tblNJAWSizeServ]
SET IDENTITY_INSERT [dbo].[tblNJAWSizeServ] OFF;
GO

--Business Units
RAISERROR (N'Inserting business units...', 10, 1) WITH NOWAIT;

SET IDENTITY_INSERT dbo.BusinessUnits ON;
INSERT INTO [BusinessUnits]
           ([BusinessUnitID]
		   ,[BU]
           ,[OperatingCenterID]
           ,[DepartmentID]
           ,[Order]
           ,[Description]
           ,[Is271Visible]
           ,[Area]
           ,[EmployeeResponsible]
           ,[AuthorizedStaffingLevelTotal]
           ,[AuthorizedStaffingLevelManagement]
           ,[AuthorizedStaffingLevelNonBargainingUnit]
           ,[AuthorizedStaffingLevelBargainingUnit])
SELECT [BusinessUnitID]
      ,[BU]
      ,[OperatingCenterID]
      ,[DepartmentID]
      ,[Order]
      ,[Description]
      ,[Is271Visible]
      ,[Area]
      ,[EmployeeResponsible]
      ,[AuthorizedStaffingLevelTotal]
      ,[AuthorizedStaffingLevelManagement]
      ,[AuthorizedStaffingLevelNonBargainingUnit]
      ,[AuthorizedStaffingLevelBargainingUnit]
  FROM [Fatman].[mapcall_2013_02_04].[dbo].[BusinessUnits]
  WHERE
	OperatingCenterID in (select operatingCenterID from OperatingCenters where OperatingCenterCode in (select OpCode from #tmpOpCntrs))
GO


SET IDENTITY_INSERT dbo.BusinessUnits OFF;

--OperatingCentersTowns
RAISERROR (N'Inserting operating centers/towns...', 10, 1) WITH NOWAIT;

INSERT INTO [OperatingCentersTowns](OperatingCenterID, TownID) SELECT OperatingCenterID, TownID FROM [Fatman].[mapcall_2013_02_04].dbo.OperatingCentersTowns WHERE TownID in (select townID from towns) and OperatingCenterID in (select operatingCenterID from operatingCenters)

--OperatingCentersUsers
RAISERROR (N'Inserting operating centers/users...', 10, 1) WITH NOWAIT;

INSERT INTO [OperatingCentersUsers](OperatingCenterID, UserID) SELECT OperatingCenterID, UserID FROM [Fatman].[mapcall_2013_02_04].dbo.OperatingCentersUsers WHERE UserID in (select recid from tblPermissions) and OperatingCenterID in (select operatingCenterID from operatingCenters)

-- need to drop the temporary opcode table
DROP TABLE #tmpOpCntrs