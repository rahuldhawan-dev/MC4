-- Useful for testing/development, to create tables that already exist in McProd
-- which are necessary for the scope of this project.  You'll need to set the
-- specific db you're using before running.
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Streets](
	[StreetID] [int] IDENTITY(1,1) NOT NULL,
	[FullStName] [varchar](50) NULL,
	[InactSt] [varchar](2) NULL CONSTRAINT [DF_Streets_InactSt]  DEFAULT (''),
	[StreetPrefix] [varchar](10) NULL,
	[StreetName] [varchar](30) NULL,
	[StreetSuffix] [varchar](10) NULL,
	[TownID] [int] NOT NULL
 CONSTRAINT [PK_Streets] PRIMARY KEY NONCLUSTERED 
(
	[StreetID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Towns](
	[Ab] [varchar](4) NULL,
	[Address] [varchar](50) NULL,
	[ContactName] [varchar](100) NULL,
	[County] [varchar](50) NULL,
	[DistrictID] [float] NULL,
	[EmergContact] [varchar](25) NULL,
	[EmergFax] [varchar](12) NULL,
	[EmergPhone] [varchar](12) NULL,
	[Fax] [varchar](12) NULL,
	[FD1Contact] [varchar](25) NULL,
	[FD1Fax] [varchar](12) NULL,
	[FD1Phone] [varchar](12) NULL,
	[Lat] [varchar](50) NULL,
	[Lon] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[TownID] [int] IDENTITY(1,1) NOT NULL,
	[State] [varchar](2) NULL,
	[Town] [varchar](50) NULL,
	[TownName] [varchar](50) NULL,
	[Zip] [varchar](10) NULL,
	[Link] [char](1) NULL CONSTRAINT [DF_Towns_Link]  DEFAULT ('L'),
	[AbbreviationTypeID] [int] NULL CONSTRAINT [DF_Towns_AbbreviationTypeID]  DEFAULT (1),
	[CountyID] [int] null,
 CONSTRAINT [PK_Towns] PRIMARY KEY CLUSTERED 
(
	[TownID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TownSections](
	[Abbreviation] [varchar](4) NULL,
	[TownSectionID] [int] IDENTITY(1,1) NOT NULL,
	[TownID] [int] NOT NULL,
	[Name] [varchar](30) NULL,
 CONSTRAINT [PK_TownSections] PRIMARY KEY CLUSTERED 
(
	[TownSectionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OperatingCenters](
	[CoInfo] [varchar](65) NULL,
	[CSNum] [varchar](12) NULL,
	[FaxNum] [varchar](12) NULL,
	[MailAdd] [varchar](30) NULL,
	[MailCo] [varchar](30) NULL,
	[MailCSZ] [varchar](30) NULL,
	[OperatingCenterCode] [varchar](4) NULL,
	[OperatingCenterName] [varchar](30) NULL,
	[OperatingCenterID] [int] IDENTITY(1,1) NOT NULL,
	[ServContactNum] [varchar](12) NULL,
	[HydInspFreq] [varchar](10) NULL CONSTRAINT [DF_OperatingCenters_HydInspFreq]  DEFAULT (1),
	[HydInspFreqUnit] [varchar](50) NULL CONSTRAINT [DF_OperatingCenters_HydInspFreqUnit]  DEFAULT ('Y'),
	[ValLgInspFreq] [varchar](10) NULL CONSTRAINT [DF_OperatingCenters_ValLgInspFreq]  DEFAULT (2),
	[ValLgInspFreqUnit] [varchar](50) NULL CONSTRAINT [DF_OperatingCenters_ValLgInspFreqUnit]  DEFAULT ('Y'),
	[ValSmInspFreq] [varchar](10) NULL CONSTRAINT [DF_OperatingCenters_ValSmInspFreq]  DEFAULT (4),
	[ValSmInspFreqUnit] [varchar](50) NULL CONSTRAINT [DF_OperatingCenters_ValSmInspFreqUnit]  DEFAULT ('Y'),
	[WorkOrdersEnabled] [bit] NOT NULL,
	[PermitsOMUserName] [varchar](50),
	[PermitsCapitalUserName] [varchar](50),
 CONSTRAINT [PK_OperatingCenters] PRIMARY KEY CLUSTERED 
(
	[OperatingCenterID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblPermissions](
	[Add1] [varchar](50) NULL,
	[CDCCode] [varchar](4) NULL,
	[CellNum] [varchar](12) NULL,
	[City] [varchar](50) NULL,
	[Company] [varchar](10) NULL,
	[EMail] [varchar](50) NULL,
	[EmpNum] [varchar](15) NULL,
	[FaxNum] [varchar](12) NULL,
	[FullName] [varchar](25) NULL,
	[Location] [varchar](3) NULL,
	[Inactive] [varchar](2) NULL,
	[OpCntr1] [varchar](4) NULL,
	[OpCntr2] [varchar](4) NULL,
	[OpCntr3] [varchar](4) NULL,
	[OpCntr4] [varchar](4) NULL,
	[OpCntr5] [varchar](4) NULL,
	[OpCntr6] [varchar](4) NULL,
	[Password] [varchar](20) NOT NULL,
	[PhoneNum] [varchar](12) NULL,
	[Region] [varchar](15) NULL,
	[St] [varchar](50) NULL,
	[UserInact] [char](1) NULL,
	[UserLevel] [char](4) NULL,
	[UserName] [varchar](20) NOT NULL,
	[Zip] [varchar](50) NULL,
	[FBUserName] [varchar](10) NULL,
	[FBPassWord] [varchar](10) NULL,
	[XYMUserName] [varchar](12) NULL,
	[XYMPassword] [varchar](12) NULL,
	[userID] [int] NULL,
	[uid] [varchar](500) NULL,
	[RecID] [int] IDENTITY(1,1) NOT NULL,
	[DefaultOperatingCenterID] int NOT NULL,
	[WorkBasket] [varchar](12) NULL,
 CONSTRAINT [PK_tblPermissions] PRIMARY KEY CLUSTERED 
(
	[RecID] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_tblPermissions_UserName_Unique] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblNJAWValves](
	[BillInfo] [varchar](16) NULL,
	[BlowOff] [varchar](3) NULL,
	[BPUKPI] [varchar](2) NULL,
	[Critical] [varchar](2) NULL,
	[CriticalNotes] [varchar](150) NULL,
	[CrossStreet] [varchar](30) NULL,
	[DateInst] [smalldatetime] NULL,
	[DateRetired] [smalldatetime] NULL,
	[DateTested] [datetime] NULL,
	[Elevation] [decimal](18, 9) NULL,
	[Initiator] [varchar](25) NULL,
	[InspFreq] [varchar](50) NULL,
	[InspFreqUnit] [varchar](50) NULL,
	[Lat] [float] NULL,
	[Lateral] [varchar](6) NULL,
	[Lon] [float] NULL,
	[Main] [varchar](6) NULL,
	[MapPage] [varchar](15) NULL,
	[NorPos] [varchar](25) NULL,
	[OpCntr] [varchar](4) NULL,
	[Opens] [varchar](6) NULL,
	[ObjectID] [int] NULL,
	[Path] [varchar](250) NULL,
	[PrintedLabel] [varchar](3) NULL,
	[RecID] [int] IDENTITY(1,1) NOT NULL,
	[Remarks] [ntext] NULL,
	[Route] [float] NULL,
	[SketchNum] [varchar](15) NULL,
	[StNum] [varchar](10) NULL,
	[StName] [int] NULL,
	[TaskRetire] [varchar](10) NULL,
	[Town] [int] NULL,
	[Traffic] [varchar](2) NULL,
	[Turns] [float] NULL,
	[TwnSection] [varchar](30) NULL,
	[TypeMain] [varchar](15) NULL,
	[ValCtrl] [varchar](25) NULL,
	[ValLoc] [varchar](150) NULL,
	[ValMake] [varchar](30) NULL,
	[ValNum] [varchar](15) NULL,
	[ValSuf] [float] NULL,
	[ValType] [varchar](25) NULL,
	[ValveSize] [varchar](10) NULL,
	[ValveStatus] [varchar](10) NULL,
	[WONum] [varchar](18) NULL,
	[DateAdded] [datetime] NULL,
	[ValveZone] [int] NULL,
	[ImageActionID] [int] NULL,
 CONSTRAINT [PK_tblNJAWValves] PRIMARY KEY CLUSTERED 
(
	[RecID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


GO
SET ANSI_PADDING OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblNJAWHydrant](
	[ActRet] [varchar](10) NULL,
	[BillInfo] [varchar](15) NULL,
	[BPUKPI] [varchar](2) NULL,
	[BranchLn] [varchar](10) NULL,
	[Critical] [varchar](2) NULL,
	[CriticalNotes] [varchar](150) NULL,
	[CrossStreet] [varchar](30) NULL,
	[DateInst] [smalldatetime] NULL,
	[DateRemoved] [smalldatetime] NULL,
	[DateTested] [datetime] NULL,
	[DEM] [varchar](3) NULL,
	[DepthBury] [varchar](10) NULL,
	[DirOpen] [varchar](7) NULL,
	[Elevation] [varchar](10) NULL,
	[FireD] [char](1) NULL,
	[Gradiant] [varchar](25) NULL,
	[HydMake] [varchar](30) NULL,
	[HydNum] [varchar](12) NOT NULL,
	[HydSize] [varchar](5) NULL,
	[HydStyle] [varchar](15) NULL,
	[HydSuf] [float] NULL,
	[Initiator] [varchar](25) NULL,
	[InspFreq] [varchar](10) NULL,
	[InspFreqUnit] [varchar](50) NULL,
	[Lat] [float] NULL,
	[LatSize] [varchar](10) NULL,
	[LatValNum] [varchar](10) NULL,
	[LinkNum] [varchar](10) NULL,
	[Location] [varchar](150) NULL,
	[Lon] [float] NULL,
	[MapPage] [varchar](15) NULL,
	[OpCntr] [varchar](4) NULL,
	[OutOfServ] [varchar](2) NULL,
	[RecID] [int] IDENTITY(1,1) NOT NULL,
	[Remarks] [ntext] NULL,
	[Route] [float] NULL,
	[SizeofMain] [varchar](10) NULL,
	[StName] [int] NULL,
	[StNum] [varchar](10) NULL,
	[Thread] [varchar](15) NULL,
	[Town] [int] NULL,
	[TwnSection] [varchar](30) NULL,
	[TypeMain] [varchar](15) NULL,
	[ValLoc] [varchar](40) NULL,
	[WONum] [varchar](18) NULL,
	[DateAdded] [datetime] NULL,
	[FireDistrictID] [int] NULL,
	[YearManufactured] [int] NULL,
	[ManufacturedUpdated] [datetime] NULL,
	[ManufacturedUpdatedBy] [varchar](50) NULL,
	[ClowTagged] [bit] NULL,
	[ObjectID] [int] NULL,
	[HydrantTagStatusID] [int] NULL,
	[PremiseNumber] [varchar](9) NULL,
	[BillingDate] [datetime] NULL,
	[ManufacturerID] [int] NULL,
	[HydrantModelID] [int] NULL,
	[FLRouteNumber] [float] NULL,
	[FLRouteSequence] [float] NULL,
	[HistoricalHydrantLocation] [varchar](150) NULL,
	[HistoricalValveLocation] [varchar](40) NULL,
	[BranchLnFt] [int] NULL,
	[BranchLnIn] [int] NULL,
	[DepthBuryFt] [int] NULL,
	[DepthBuryIn] [int] NULL,
 CONSTRAINT [PK_tblNJAWHydrant] PRIMARY KEY CLUSTERED 
(
	[RecID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Coordinates]    Script Date: 09/15/2009 15:23:29 ******/
/* 
	THIS TABLE IS A BIT NASTY. IT'S A DUPLICATE TABLE OF AWW.DBO.COORDINATES
	IT'S DUPLICATED HERE AND MANAGED THROUGH TRIGGERS TO STAY IN SYNC 
	WITH AWW SO THAT EVENTUALLY WHEN THE TABLES ARE MERGED THE IDS DON'T 
	CONFLICT.
	IT'S REFERENCED BY MANHOLES
 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coordinates](
	[CoordinateID] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[CustomIconID] [int] NULL,
	[IconID] [int] NULL,
	[AddressID] [int] NULL,
 CONSTRAINT [PK_Coorinates] PRIMARY KEY CLUSTERED 
(
	[CoordinateID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[AssetStatuses]    Script Date: 09/16/2009 11:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetStatuses](
	[AssetStatusID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_AssetStatuses] PRIMARY KEY CLUSTERED 
(
	[AssetStatusID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[SewerManholes]    Script Date: 09/16/2009 11:21:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SewerManholes](
	[SewerManholeID] [int] IDENTITY(1,1) NOT NULL,
	[OperatingCenterID] [int] NOT NULL,
	[TownID] [int] NOT NULL,
	[ManholeNumber] [varchar](50) NULL,
	[StreetID] [int] NULL,
	[IntersectingStreetID] [int] NULL,
	[TaskNumber] [nvarchar](50) NULL,
	[DateInstalled] [datetime] NULL,
	[DateRetired] [datetime] NULL,
	[MapPage] [nvarchar](50) NULL,
	[CoordinateID] [int] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL CONSTRAINT [DF_SewerManholes_CreatedOn]  DEFAULT (getdate()),
	[AssetStatusID] [int] NULL,
	[OldNumber] [varchar](50) NULL,
	[SewerManholeMaterialID] [int] NULL,
	[DistanceFromCrossStreet] [varchar](255) NULL,
	[IsEpoxyCoated] [bit] NULL,
	[Notes] [varchar](1000) NULL,
 CONSTRAINT [PK_SewerManholes] PRIMARY KEY CLUSTERED 
(
	[SewerManholeID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[SewerManholes]  WITH CHECK ADD  CONSTRAINT [FK_SewerManholes_AssetStatuses] FOREIGN KEY([AssetStatusID])
REFERENCES [dbo].[AssetStatuses] ([AssetStatusID])
GO
ALTER TABLE [dbo].[SewerManholes] CHECK CONSTRAINT [FK_SewerManholes_AssetStatuses]
GO
ALTER TABLE [dbo].[SewerManholes]  WITH CHECK ADD  CONSTRAINT [FK_SewerManholes_Coordinates] FOREIGN KEY([CoordinateID])
REFERENCES [dbo].[Coordinates] ([CoordinateID])
GO
ALTER TABLE [dbo].[SewerManholes] CHECK CONSTRAINT [FK_SewerManholes_Coordinates]
GO
ALTER TABLE [dbo].[SewerManholes]  WITH CHECK ADD  CONSTRAINT [FK_SewerManholes_Streets] FOREIGN KEY([StreetID])
REFERENCES [dbo].[Streets] ([StreetID])
GO
ALTER TABLE [dbo].[SewerManholes] CHECK CONSTRAINT [FK_SewerManholes_Streets]
GO
ALTER TABLE [dbo].[SewerManholes]  WITH CHECK ADD  CONSTRAINT [FK_SewerManholes_StreetsIntersecting] FOREIGN KEY([IntersectingStreetID])
REFERENCES [dbo].[Streets] ([StreetID])
GO
ALTER TABLE [dbo].[SewerManholes] CHECK CONSTRAINT [FK_SewerManholes_StreetsIntersecting]
GO
ALTER TABLE [dbo].[SewerManholes]  WITH CHECK ADD  CONSTRAINT [FK_SewerManholes_Towns] FOREIGN KEY([TownID])
REFERENCES [dbo].[Towns] ([TownID])
GO
ALTER TABLE [dbo].[SewerManholes] CHECK CONSTRAINT [FK_SewerManholes_Towns]
GO
ALTER TABLE [dbo].[SewerManholes]  WITH NOCHECK ADD  CONSTRAINT [FK_SewerManholes_OperatingCenters] FOREIGN KEY([OperatingCenterID])
REFERENCES [dbo].[OperatingCenters] ([OperatingCenterID])
GO
ALTER TABLE [dbo].[SewerManholes] CHECK CONSTRAINT [FK_SewerManholes_OperatingCenters]
GO


-------------------------------STORMWATER ASSETS-------------------------------

CREATE TABLE [dbo].[StormWaterAssetTypes](
	[StormWaterAssetTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_StormWaterAssetTypes] PRIMARY KEY CLUSTERED 
 (
 [StormWaterAssetTypeID] ASC
 ) ON [PRIMARY]
 ) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[StormWaterAssets]    Script Date: 10/11/2010 14:08:29 ******/

CREATE TABLE [dbo].[StormWaterAssets](
	[StormWaterAssetID] [int] IDENTITY(1,1) NOT NULL,
	[OperatingCenterID] [int] NOT NULL,
	[TownID] [int] NOT NULL,
	[StormWaterAssetTypeID] [int] NOT NULL,
	[AssetNumber] [varchar](50) NULL,
	[StreetID] [int] NULL,
	[IntersectingStreetID] [int] NULL,
	[TaskNumber] [nvarchar](50) NULL,
	[DateInstalled] [datetime] NULL,
	[DateRetired] [datetime] NULL,
	[MapPage] [nvarchar](50) NULL,
	[CoordinateID] [int] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL CONSTRAINT [DF_StormWaterAssets_CreatedOn]  DEFAULT (getdate()),
	[AssetStatusID] [int] NULL,
	[OldNumber] [varchar](50) NULL,
	[MaterialID] [int] NULL,
	[DistanceFromCrossStreet] [varchar](255) NULL,
	[IsEpoxyCoated] [bit] NULL,
	[Notes] [varchar](1000) NULL,
 CONSTRAINT [PK_StormWaterAssets] PRIMARY KEY CLUSTERED 
 (
 [StormWaterAssetID] ASC
 ) ON [PRIMARY]
 ) ON [PRIMARY]

GO

ALTER TABLE [dbo].[StormWaterAssets]  WITH CHECK ADD  CONSTRAINT [FK_StormWaterAssets_AssetStatuses] FOREIGN KEY([AssetStatusID])
REFERENCES [dbo].[AssetStatuses] ([AssetStatusID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_AssetStatuses]
GO
ALTER TABLE [dbo].[StormWaterAssets]  WITH NOCHECK ADD  CONSTRAINT [FK_StormWaterAssets_AssetType] FOREIGN KEY([StormWaterAssetTypeID])
REFERENCES [dbo].[StormWaterAssetTypes] ([StormWaterAssetTypeID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_AssetType]
GO
ALTER TABLE [dbo].[StormWaterAssets]  WITH NOCHECK ADD  CONSTRAINT [FK_StormWaterAssets_Coordinates] FOREIGN KEY([CoordinateID])
REFERENCES [dbo].[Coordinates] ([CoordinateID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_Coordinates]
GO
-- ALTER TABLE [dbo].[StormWaterAssets]  WITH CHECK ADD  CONSTRAINT [FK_StormWaterAssets_SewerManholeMaterials] FOREIGN KEY([MaterialID])
-- REFERENCES [dbo].[SewerManholeMaterials] ([SewerManholeMaterialID])
-- GO
-- ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_SewerManholeMaterials]
-- GO
ALTER TABLE [dbo].[StormWaterAssets]  WITH CHECK ADD  CONSTRAINT [FK_StormWaterAssets_Streets] FOREIGN KEY([StreetID])
REFERENCES [dbo].[Streets] ([StreetID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_Streets]
GO
ALTER TABLE [dbo].[StormWaterAssets]  WITH CHECK ADD  CONSTRAINT [FK_StormWaterAssets_StreetsIntersecting] FOREIGN KEY([IntersectingStreetID])
REFERENCES [dbo].[Streets] ([StreetID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_StreetsIntersecting]
GO
ALTER TABLE [dbo].[StormWaterAssets]  WITH CHECK ADD  CONSTRAINT [FK_StormWaterAssets_Towns] FOREIGN KEY([TownID])
REFERENCES [dbo].[Towns] ([TownID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_Towns]
GO
ALTER TABLE [dbo].[StormWaterAssets]  WITH NOCHECK ADD  CONSTRAINT [FK_StormWaterAssets_OperatingCenters] FOREIGN KEY([OperatingCenterID])
REFERENCES [dbo].[OperatingCenters] ([OperatingCenterID])
GO
ALTER TABLE [dbo].[StormWaterAssets] CHECK CONSTRAINT [FK_StormWaterAssets_OperatingCenters]


-----------------------------------------------------------------------------------------------------
----------------------------------------------DOCUMENTS----------------------------------------------
-----------------------------------------------------------------------------------------------------
 


--------------------------------DATA TYPES--------------------------------
CREATE TABLE [dbo].[DataType](
	[DataTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Data_Type] [varchar](255) NULL,
	[Table_Name] [varchar](255) NULL,
	[Table_ID] [varchar](255) NULL,
 CONSTRAINT [PK_DataType] PRIMARY KEY CLUSTERED 
(
	[DataTypeID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC dbo.sp_addextendedproperty @name=N'MS_Description', @value=N'This table contains the types of data elements in the system that documents and notes can be attached to. ' , @level0type=N'USER',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DataType', @level2type=N'COLUMN',@level2name=N'DataTypeID'


------------------------------DOCUMENT TYPES------------------------------
CREATE TABLE [dbo].[DocumentType](
	[DocumentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Document_Type] [varchar](50) NULL,
	[DataTypeID] [int] NULL,
 CONSTRAINT [PK_DocumentType] PRIMARY KEY CLUSTERED 
(
	[DocumentTypeID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[DocumentType]  WITH CHECK ADD  CONSTRAINT [FK_DocumentType_DataType] FOREIGN KEY([DataTypeID])
REFERENCES [dbo].[DataType] ([DataTypeID])
GO
ALTER TABLE [dbo].[DocumentType] CHECK CONSTRAINT [FK_DocumentType_DataType]


---------------------------------DOCUMENTS---------------------------------
CREATE TABLE [dbo].[Document](
	[documentID] [int] IDENTITY(1,1) NOT NULL,
	[documentTypeID] [int] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL CONSTRAINT [DF_Document_CreatedOn]  DEFAULT (getdate()),
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Description] [varchar](255) NULL,
	[BinaryData] [image] NULL,
	[File_Size] [int] NULL,
	[File_Name] [varchar](255) NULL,
	[CreatedByID] int null,
	[ModifiedByID] int null,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[documentID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [Document] WITH NOCHECK ADD CONSTRAINT [FK_Document_DocumentType_DocumentTypeID] FOREIGN KEY (
[DocumentTypeID]
) REFERENCES [DocumentType] (
[DocumentTypeID]
)
GO

ALTER TABLE [Document] WITH NOCHECK ADD CONSTRAINT [FK_Document_tblPermissions_CreatedByID] FOREIGN KEY (
[CreatedByID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

ALTER TABLE [Document] WITH NOCHECK ADD CONSTRAINT [FK_Document_tblPermissions_ModifiedByID] FOREIGN KEY (
[ModifiedByID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

/****** Object:  Table [dbo].[tblNJAWSizeServ]    Script Date: 06/09/2010 08:54:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblNJAWSizeServ](
	[Hyd] [varchar](2) NULL CONSTRAINT [DF_tblNJAWSizeServ_Hyd]  DEFAULT (''),
	[Lat] [varchar](2) NULL CONSTRAINT [DF_tblNJAWSizeServ_Lat]  DEFAULT (''),
	[Main] [varchar](2) NULL CONSTRAINT [DF_tblNJAWSizeServ_Main]  DEFAULT (''),
	[Meter] [varchar](2) NULL CONSTRAINT [DF_tblNJAWSizeServ_Meter]  DEFAULT (''),
	[RecID] [int] IDENTITY(1,1) NOT NULL,
	[RecOrd] [int] NULL,
	[Serv] [varchar](2) NULL CONSTRAINT [DF_tblNJAWSizeServ_Serv]  DEFAULT (''),
	[SizeServ] [varchar](10) NULL CONSTRAINT [DF_tblNJAWSizeServ_SizeServ]  DEFAULT (''),
	[Valve] [varchar](6) NULL CONSTRAINT [DF_tblNJAWSizeServ_Valve]  DEFAULT ('')
	CONSTRAINT [PK_tblNJAWSizeServ] PRIMARY KEY CLUSTERED 
	(
		[RecID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[BusinessUnits]    Script Date: 07/26/2011 16:23:36 ******/


CREATE TABLE [dbo].[BusinessUnits](
	[BusinessUnitID] [int] IDENTITY(1,1) NOT NULL,
	[BU] [char](6) NOT NULL,
	[OperatingCenterID] [int] NOT NULL,
	[DepartmentID] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Is271Visible] [bit] NOT NULL,
	[Area] [int] NULL,
	[EmployeeResponsible] [int] NULL,
	[AuthorizedStaffingLevelTotal] [int] NULL,
	[AuthorizedStaffingLevelManagement] [int] NULL,
	[AuthorizedStaffingLevelNonBargainingUnit] [int] NULL,
	[AuthorizedStaffingLevelBargainingUnit] [int] NULL,
 CONSTRAINT [PK_BusinessUnits] PRIMARY KEY CLUSTERED 
(
	[BusinessUnitID] ASC
) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[BusinessUnitID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BusinessUnits]  WITH CHECK ADD  CONSTRAINT [FK_BusinessUnits_tblOpCntr_OperatingCenterID] FOREIGN KEY([OperatingCenterID])
REFERENCES [dbo].[OperatingCenters] ([OperatingCenterID])
GO

ALTER TABLE [dbo].[BusinessUnits] CHECK CONSTRAINT [FK_BusinessUnits_tblOpCntr_OperatingCenterID]
GO

ALTER TABLE [dbo].[BusinessUnits] ADD  CONSTRAINT [DF_BusinessUnits_Is271Visible]  DEFAULT ('0') FOR [Is271Visible]
GO




CREATE TABLE OperatingCentersTowns(
	OperatingCenterID int not null,
	TownID int not null,
 CONSTRAINT [PK_OperatingCentersTowns] PRIMARY KEY CLUSTERED 
(
	[OperatingCenterID] ASC,
	[TownID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [OperatingCentersTowns]  ADD CONSTRAINT [FK_OperatingCentersTowns_OperatingCenters_OperatingCenterID] FOREIGN KEY (
[OperatingCenterID]
) REFERENCES [OperatingCenters] (
[OperatingCenterID]
)
GO

ALTER TABLE [OperatingCentersTowns]  ADD CONSTRAINT [FK_OperatingCentersTowns_Towns_TownID] FOREIGN KEY (
[TownID]
) REFERENCES [Towns] (
[TownID]
)
GO

CREATE TABLE OperatingCentersUsers(
	OperatingCenterID int not null,
	UserID int not null,
	CONSTRAINT [PK_OperatingCentersUsers] PRIMARY KEY CLUSTERED
( 
	OperatingCenterID ASC, 
	UserID ASC
) ON [PRIMARY] 
) ON [PRIMARY]
	

ALTER TABLE [OperatingCentersUsers]  ADD CONSTRAINT [FK_OperatingCenterUsers_OperatingCenters_OperatingCentersUsers] FOREIGN KEY (
[OperatingCenterID]
) REFERENCES [OperatingCenters] (
[OperatingCenterID]
)
GO

ALTER TABLE [OperatingCentersUsers]  ADD CONSTRAINT [FK_OperatingCentersUsers_tblPermissions_UserID] FOREIGN KEY (
[UserID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

--------------------------------------------------------------------------------
---------------------------------NOTIFICATIONS----------------------------------
--------------------------------------------------------------------------------

------------------------------------CONTACTS------------------------------------
CREATE TABLE [Contacts] (
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[MiddleInitial] [varchar](1) NULL,
	[BusinessPhone] [varchar](20) NULL,
	[Fax] [varchar](20) NULL,
	[Mobile] [varchar](20) NULL,
	[HomePhone] [varchar](20) NULL,
	[Address1] [varchar](255) NULL,
	[Address2] [varchar](255) NULL,
	[City] [varchar](255) NULL,
	[StateID] [int] NULL,
	[Zip] [varchar](10) NULL,
	[Email] [varchar](255) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED (
		[ContactID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [DF_Contacts_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
----------------------------------APPLICATIONS----------------------------------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Applications](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_AuthorizationApplication] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

------------------------------------MODULES-------------------------------------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Modules](
	[ModuleID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_AuthorizationModule] PRIMARY KEY CLUSTERED 
(
	[ModuleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [Modules]  ADD CONSTRAINT [FK_Modules_Applications_ApplicationID] FOREIGN KEY (
	[ApplicationID]
) REFERENCES [Applications] (
	[ApplicationID]
)
GO

-----------------------------NOTIFICATION PURPOSES------------------------------
CREATE TABLE [NotificationPurposes] (
	[NotificationPurposeID] int unique identity not null,
	[ModuleID] int not null,
	[Purpose] varchar(50) not null,
	CONSTRAINT [PK_NotificationPurposes] PRIMARY KEY CLUSTERED (
		[NotificationPurposeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [NotificationPurposes]  ADD CONSTRAINT [FK_NotificationPurposes_Modules_ModuleID] FOREIGN KEY (
	[ModuleID]
) REFERENCES [Modules] (
	[ModuleID]
)
GO

--------------------------NOTIFICATION CONFIGURATIONS---------------------------
CREATE TABLE [NotificationConfigurations] (
	[NotificationConfigurationID] int unique identity not null,
	[ContactID] int not null,
	[OperatingCenterID] int not null,
	[NotificationPurposeID] int not null,
	CONSTRAINT [PK_NotificationConfigurations] PRIMARY KEY CLUSTERED (
		[NotificationConfigurationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [NotificationConfigurations]  ADD CONSTRAINT [FK_NotificationConfigurations_Contacts_ContactID] FOREIGN KEY (
	[ContactID]
) REFERENCES [Contacts] (
	[ContactID]
)
GO

ALTER TABLE [NotificationConfigurations]  ADD CONSTRAINT [FK_NotificationConfigurations_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO

ALTER TABLE [NotificationConfigurations]  ADD CONSTRAINT [FK_NotificationConfigurations_NotificationPurposes_NotificationPurposeID] FOREIGN KEY (
	[NotificationPurposeID]
) REFERENCES [NotificationPurposes] (
	[NotificationPurposeID]
)
GO


/****** Object:  Table [dbo].[Contractors]    Script Date: 11/02/2011 15:31:48 ******/
CREATE TABLE [dbo].[Contractors](
	[ContractorID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[HouseNumber] [varchar](12) NULL,
	[ApartmentNumber] [varchar](12) NULL,
	[StreetID] [int] NULL,
	[CityID] [int] NULL,
	[StateID] [int] NULL,
	[Zip] [varchar](12) NULL,
	[Phone] [varchar](15) NULL,
	[IsUnionShop] [bit] NOT NULL,
	[IsBCPPartner] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[QualityControlContactID] [int] NULL,
	[SafetyContactID] [int] NULL,
	[VendorID] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ContractorsAccess] bit NULL
 CONSTRAINT [PK_Contractors] PRIMARY KEY CLUSTERED 
(
	[ContractorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- ALTER TABLE [dbo].[Contractors]  WITH NOCHECK ADD  CONSTRAINT [FK_Contractors_States] FOREIGN KEY([StateID])
-- REFERENCES [dbo].[States] ([stateID])
-- GO

-- ALTER TABLE [dbo].[Contractors] CHECK CONSTRAINT [FK_Contractors_States]
-- GO

ALTER TABLE [dbo].[Contractors]  WITH CHECK ADD  CONSTRAINT [FK_Contractors_Towns_CityID] FOREIGN KEY([CityID])
REFERENCES [dbo].[Towns] ([TownID])
GO

ALTER TABLE [dbo].[Contractors] CHECK CONSTRAINT [FK_Contractors_Towns_CityID]
GO

ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE dbo.Contractors ADD CONSTRAINT DF_Contractors_ContractorsAccess DEFAULT 0 FOR ContractorsAccess
GO

CREATE TABLE [dbo].[ContractorsOperatingCenters](
	[ContractorOperatingCenterID] [int] IDENTITY(1,1) NOT NULL,
	[ContractorID] [int] NOT NULL,
	[OperatingCenterID] [int] NOT NULL,
 CONSTRAINT [PK_ContractorsOperatingCenters] PRIMARY KEY CLUSTERED 
(
	[ContractorOperatingCenterID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO







/****** Object:  Table [dbo].[tblNJAWService]    Script Date: 01/09/2012 14:29:23 ******/
CREATE TABLE [dbo].[tblNJAWService](
	[OpCntr] [varchar](4) NULL,
	[PremNum] [varchar](9) NULL,
	[RecID] [int] IDENTITY(1,1) NOT NULL,
	[ServNum] [float] NULL,
	[Town] [int] NULL,
 CONSTRAINT [PK_tblNJAWService] PRIMARY KEY CLUSTERED 
(
	[RecID] ASC
) ON [PRIMARY]
)

GO

ALTER TABLE [dbo].[tblNJAWService]  WITH CHECK ADD  CONSTRAINT [FK_tblNJAWService_Towns_Town] FOREIGN KEY([Town])
REFERENCES [dbo].[Towns] ([TownID])
GO

ALTER TABLE [dbo].[tblNJAWService] CHECK CONSTRAINT [FK_tblNJAWService_Towns_Town]
GO


/****** Object:  Table [dbo].[TapImages]    Script Date: 01/09/2012 14:25:26 ******/
CREATE TABLE [dbo].[TapImages](
	[TapImageID] [int] IDENTITY(1,1) NOT NULL,
	[OperatingCenter] [nvarchar](50) NULL,
	[DateAdded] smalldatetime null,
	[FileList] [nvarchar](255) NULL,
	[fld] [nvarchar](30) NULL,
	[TownID] [int] NULL,
	[ServiceID] [int] NULL,
	[PremiseNumber] nvarchar(50) NULL,
	[ServiceNumber] nvarchar(50) NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TapImages]  WITH NOCHECK ADD  CONSTRAINT [FK_TapImages_tblNJAWService_ServiceID] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[tblNJAWService] ([RecID])
GO

ALTER TABLE [dbo].[TapImages] CHECK CONSTRAINT [FK_TapImages_tblNJAWService_ServiceID]
GO

ALTER TABLE [dbo].[TapImages]  WITH CHECK ADD  CONSTRAINT [FK_TapImages_Towns_TownID] FOREIGN KEY([TownID])
REFERENCES [dbo].[Towns] ([TownID])
GO

ALTER TABLE [dbo].[TapImages] CHECK CONSTRAINT [FK_TapImages_Towns_TownID]
GO


/****** Object:  Table [dbo].[ValveImages]    Script Date: 01/09/2012 14:32:47 ******/
CREATE TABLE [dbo].[ValveImages](
	[ValveImageID] [int] IDENTITY(1,1) NOT NULL,
	[FileList] [nvarchar](400) NULL,
	[fld] [nvarchar](50) NULL,
	[DateAdded] datetime null,
	[ValveID] [int] NULL,
	[TownID] int null,
 CONSTRAINT [PK_ValveImages] PRIMARY KEY NONCLUSTERED 
(
	[ValveImageID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[States]    Script Date: 12/07/2012 12:48:07 ******/
CREATE TABLE [dbo].[States](
	[stateID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Abbreviation] [varchar](50) NULL,
	[scadaTbl] [varchar](50) NULL,
	[RegionID] [int] NULL,
 CONSTRAINT [PK_StateTable] PRIMARY KEY CLUSTERED 
(
	[stateID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Counties]    Script Date: 12/07/2012 12:49:50 ******/
CREATE TABLE [dbo].[Counties](
	[CountyID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[SpecialID] [int] NULL,
	[stateID] [int] NULL,
	[countyEnabled] [bit] NULL,
 CONSTRAINT [PK_CountyTable] PRIMARY KEY CLUSTERED 
(
	[CountyID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

