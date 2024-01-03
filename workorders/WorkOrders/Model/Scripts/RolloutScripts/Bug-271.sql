use mcprod

-- Field for the SewerManholeID
ALTER TABLE dbo.WorkOrders ADD
	SewerManholeID int NULL
GO

-- Increase AssetTypes Description field's length
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AssetTypes
	(
	AssetTypeID int NOT NULL IDENTITY (1, 1),
	Description varchar(13) NOT NULL
	)  ON [PRIMARY]
GO
GRANT REFERENCES ON dbo.Tmp_AssetTypes TO MCUser  AS dbo
GO
GRANT SELECT ON dbo.Tmp_AssetTypes TO MCUser  AS dbo
GO
GRANT UPDATE ON dbo.Tmp_AssetTypes TO MCUser  AS dbo
GO
GRANT INSERT ON dbo.Tmp_AssetTypes TO MCUser  AS dbo
GO
GRANT DELETE ON dbo.Tmp_AssetTypes TO MCUser  AS dbo
GO
SET IDENTITY_INSERT dbo.Tmp_AssetTypes ON
GO
IF EXISTS(SELECT * FROM dbo.AssetTypes)
	 EXEC('INSERT INTO dbo.Tmp_AssetTypes (AssetTypeID, Description)
		SELECT AssetTypeID, Description FROM dbo.AssetTypes WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_AssetTypes OFF
GO
ALTER TABLE dbo.WorkDescriptions
	DROP CONSTRAINT FK_WorkDescriptions_AssetTypes_AssetTypeID
GO
DROP TABLE dbo.AssetTypes
GO
EXECUTE sp_rename N'dbo.Tmp_AssetTypes', N'AssetTypes', 'OBJECT' 
GO
ALTER TABLE dbo.AssetTypes ADD CONSTRAINT
	PK_AssetTypes PRIMARY KEY CLUSTERED 
	(
	AssetTypeID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.AssetTypes ADD CONSTRAINT
	UQ__AssetTypes__0D933B9C UNIQUE NONCLUSTERED 
	(
	AssetTypeID
	) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WorkDescriptions WITH NOCHECK ADD CONSTRAINT
	FK_WorkDescriptions_AssetTypes_AssetTypeID FOREIGN KEY
	(
	AssetTypeID
	) REFERENCES dbo.AssetTypes
	(
	AssetTypeID
	)
GO
COMMIT
GO

-- New Asset Types
INSERT INTO [AssetTypes] ([Description]) VALUES ('Sewer Manhole');
INSERT INTO [AssetTypes] ([Description]) VALUES ('Sewer Lateral');
INSERT INTO [AssetTypes] ([Description]) VALUES ('Sewer Main');
GO

--DBCC CHECKIDENT (AssetTypes, RESEED, 4)
--Select * from AssetTypes


-- Increase Description size for WorkCategories
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_WorkCategories
	(
	WorkCategoryID int NOT NULL IDENTITY (1, 1),
	Description varchar(35) NOT NULL
	)  ON [PRIMARY]
GO
GRANT REFERENCES ON dbo.Tmp_WorkCategories TO MCUser  AS dbo
GO
GRANT SELECT ON dbo.Tmp_WorkCategories TO MCUser  AS dbo
GO
GRANT UPDATE ON dbo.Tmp_WorkCategories TO MCUser  AS dbo
GO
GRANT INSERT ON dbo.Tmp_WorkCategories TO MCUser  AS dbo
GO
GRANT DELETE ON dbo.Tmp_WorkCategories TO MCUser  AS dbo
GO
SET IDENTITY_INSERT dbo.Tmp_WorkCategories ON
GO
IF EXISTS(SELECT * FROM dbo.WorkCategories)
	 EXEC('INSERT INTO dbo.Tmp_WorkCategories (WorkCategoryID, Description)
		SELECT WorkCategoryID, Description FROM dbo.WorkCategories WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_WorkCategories OFF
GO
ALTER TABLE dbo.WorkDescriptions
	DROP CONSTRAINT FK_WorkDescriptions_WorkCategories_WorkCategoryID
GO
DROP TABLE dbo.WorkCategories
GO
EXECUTE sp_rename N'dbo.Tmp_WorkCategories', N'WorkCategories', 'OBJECT' 
GO
ALTER TABLE dbo.WorkCategories ADD CONSTRAINT
	PK_WorkCategories PRIMARY KEY CLUSTERED 
	(
	WorkCategoryID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.WorkCategories ADD CONSTRAINT
	UQ__WorkCategories__1163CC80 UNIQUE NONCLUSTERED 
	(
	WorkCategoryID
	) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WorkDescriptions WITH NOCHECK ADD CONSTRAINT
	FK_WorkDescriptions_WorkCategories_WorkCategoryID FOREIGN KEY
	(
	WorkCategoryID
	) REFERENCES dbo.WorkCategories
	(
	WorkCategoryID
	)
GO
COMMIT
GO

-- INSERT NEW WorkCategories
--Select * from WorkCategories
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Main Repair')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Main Replacement')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Main Retirement')
INSERT INTO [WorkCategories] ([Description])
Values ('Install new Sewer main')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Main Cleaning')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Lateral Installation')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Lateral Leak - Repair')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Lateral Leak - Replace')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Lateral Leak - Retire')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Lateral Leak - Customer side')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Manhole Repair')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Manhole Replace')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Manhole Installation')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer Overflow Response')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer backup- Company side')
INSERT INTO [WorkCategories] ([Description])
Values ('Sewer backup - Customer side')
GO

-- NEW WorkDescriptions
--Select * from WorkDescriptions
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(29, 'SEWER MAIN BREAK-REPAIR',7,6,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(30, 'SEWER MAIN BREAK-REPLACE',7,6,1)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(31, 'SEWER MAIN RETIREMENT',7,4,3)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(32, 'SEWER MAIN INSTALLATION',7,8,1)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(33, 'SEWER MAIN CLEANING',7,8,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(34, 'SEWER LATERAL LINE INSTALLATION',6,6,1)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(35, 'SEWER LATERAL LEAK - REPAIR',6,4,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(36, 'SEWER LATERAL LEAK - REPLACE',6,4,1)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(37, 'SEWER LATERAL LEAK - RETIRE',6,4,3)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(38, 'SEWER LATERAL LEAK - CUSTOMER SIDE',6,2,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(39, 'SEWER MANHOLE REPAIR',5,4,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(40, 'SEWER MANHOLE REPLACE',5,8,1)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(41, 'SEWER MANHOLE INSTALLATION',5,8,1)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  VALUES(42, 'SEWER OVERFLOW RESPONSE',7,2,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  Values(43, 'SEWER BACKUP- COMPANY SIDE',6,2,2)
INSERT INTO [dbo].[WorkDescriptions]([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
  Values(44, 'SEWER BACKUP - CUSTOMER SIDE',7,2,2)
GO

/****** Object:  Table [dbo].[OperatingCenterAssetTypes]    Script Date: 09/24/2009 15:27:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperatingCenterAssetTypes](
	[OperatingCenterAssetTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OperatingCenterID] [int] NOT NULL,
	[AssetTypeID] [int] NOT NULL,
 CONSTRAINT [PK_OperatingCenterAssetTypes] PRIMARY KEY CLUSTERED 
(
	[OperatingCenterAssetTypeID] ASC
)  ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[OperatingCenterAssetTypes]  WITH CHECK ADD  CONSTRAINT [FK_OperatingCenterAssetTypes_AssetTypes] FOREIGN KEY([AssetTypeID])
REFERENCES [dbo].[AssetTypes] ([AssetTypeID])
GO
ALTER TABLE [dbo].[OperatingCenterAssetTypes] CHECK CONSTRAINT [FK_OperatingCenterAssetTypes_AssetTypes]
GO
ALTER TABLE [dbo].[OperatingCenterAssetTypes]  WITH CHECK ADD  CONSTRAINT [FK_OperatingCenterAssetTypes_tblOpCntr] FOREIGN KEY([OperatingCenterID])
REFERENCES [dbo].[tblOpCntr] ([RecID])
GO
ALTER TABLE [dbo].[OperatingCenterAssetTypes] CHECK CONSTRAINT [FK_OperatingCenterAssetTypes_tblOpCntr]
GO
GRANT all ON dbo.OperatingCenterAssetTypes TO MCUser  AS dbo
GO
INSERT INTO [OperatingCenterAssetTypes] Values(10, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(10, 4)

INSERT INTO [OperatingCenterAssetTypes] Values(14, 1)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 2)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 3)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 4)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 5)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 6)
INSERT INTO [OperatingCenterAssetTypes] Values(14, 7)
GO