
----------------------------RESTORATION PRODUCT CODES----------------------------
CREATE TABLE [RestorationProductCodes] (
	[RestorationProductCodeID] int unique identity not null,
	[Code] varchar(4) not null
	CONSTRAINT [PK_ProductCode] PRIMARY KEY CLUSTERED (
		[RestorationProductCodeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[RestorationProductCodes] ON;

INSERT INTO RestorationProductCodes(RestorationProductCodeID, Code) Values(1, 'TB04')

SET IDENTITY_INSERT [dbo].[RestorationProductCodes] OFF;


--------------------------RESTORATION ACCOUNTING CODES--------------------------
CREATE TABLE [RestorationAccountingCodes] (
	[RestorationAccountingCodeID] int unique identity not null,
	[Code] varchar(8) not null,
	[SubCode] varchar(2) null
	CONSTRAINT [PK_RestorationAccountingCodes] PRIMARY KEY CLUSTERED (
		[RestorationAccountingCodeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[RestorationAccountingCodes] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[RestorationAccountingCodes]([RestorationAccountingCodeID], [Code], [SubCode])
SELECT 1, N'105300', N'21' UNION ALL
SELECT 2, N'675650', N'24' UNION ALL
SELECT 3, N'675651', N'24' UNION ALL
SELECT 4, N'675652', N'24' UNION ALL
SELECT 5, N'675653', N'24' UNION ALL
SELECT 6, N'675654', N'24' UNION ALL
SELECT 7, N'675655', N'24' UNION ALL
SELECT 8, N'675656', N'24' UNION ALL
SELECT 9, N'675657', N'24' UNION ALL
SELECT 10, N'675658', N'24' UNION ALL
SELECT 11, N'675659', N'24' UNION ALL
SELECT 12, N'675660', N'24' UNION ALL
SELECT 13, N'675661', N'24' UNION ALL
SELECT 14, N'675662', N'24' UNION ALL
SELECT 15, N'675663', N'24' UNION ALL
SELECT 16, N'675664', N'24' UNION ALL
SELECT 17, N'675665', N'24' UNION ALL
SELECT 18, N'675666', N'24' UNION ALL
SELECT 19, N'675667', N'24' UNION ALL
SELECT 20, N'675668', N'24' UNION ALL
SELECT 21, N'675669', N'24' UNION ALL
SELECT 22, N'675670', N'24' UNION ALL
SELECT 23, N'675671', N'24' UNION ALL
SELECT 24, N'675672', N'24' UNION ALL
SELECT 25, N'675673', N'24' UNION ALL
SELECT 26, N'185300', NULL
COMMIT;
RAISERROR (N'[dbo].[RestorationAccountingCodes]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[RestorationAccountingCodes] OFF;


--------------------------------WORK CATEGORIES--------------------------------

SET IDENTITY_INSERT [dbo].[WorkCategories] ON;

INSERT INTO [dbo].[WorkCategories]([WorkCategoryID], [Description])
SELECT 48, N'Relocation' UNION ALL
SELECT 49, N'Restoration Investigation' UNION ALL
SELECT 50, N'Sewer Lift Station Repair' UNION ALL
SELECT 51, N'Storm/Catch Installation' UNION ALL
SELECT 52, N'Storm/Catch Repair' UNION ALL
SELECT 53, N'Storm/Catch Replace'

SET IDENTITY_INSERT [dbo].[WorkCategories] OFF;


------------------------------WORK ODER PURPOSES ------------------------------
INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Water Quality');


-------------------------------WORK DESCRIPTIONS-------------------------------
-- MODIFY TABLES

ALTER TABLE dbo.WorkDescriptions ADD
	[FirstRestorationAccountingCodeID] int NOT NULL CONSTRAINT DF_WorkDescriptions_FirstRestorationAccountingCodeID DEFAULT 1,
	[FirstRestorationCostBreakdown] tinyint NOT NULL CONSTRAINT DF_WorkDescriptions_FirstRestorationCostBreakdown DEFAULT 0,
	[FirstRestorationProductCodeID] int not null CONSTRAINT DF_WorkDescriptions_FirstRestorationProductCodeID DEFAULT 1,
	[SecondRestorationAccountingCodeID] int null,
	[SecondRestorationCostBreakdown] tinyint null,
	[SecondRestorationProductCodeID] int null,
	[ShowBusinessUnit] bit not null CONSTRAINT DF_WorkDescriptions_ShowBusinessUnit DEFAULT 0,
	[ShowApprovalAccounting] bit not null CONSTRAINT DF_WorkDescriptions_ShowApproval DEFAULT 0,
	[EditOnly] bit not null CONSTRAINT DF_WorkDescriptions_EditOnly DEFAULT 0

-- REMOVE THESE DEFAULT CONSTRAINTS
ALTER TABLE dbo.WorkDescriptions DROP CONSTRAINT DF_WorkDescriptions_FirstRestorationAccountingCodeID
ALTER TABLE dbo.WorkDescriptions DROP CONSTRAINT DF_WorkDescriptions_FirstRestorationCostBreakdown
ALTER TABLE dbo.WorkDescriptions DROP CONSTRAINT DF_WorkDescriptions_FirstRestorationProductCodeID
ALTER TABLE dbo.WorkDescriptions DROP CONSTRAINT DF_WorkDescriptions_ShowBusinessUnit
ALTER TABLE dbo.WorkDescriptions DROP CONSTRAINT DF_WorkDescriptions_ShowApproval
ALTER TABLE dbo.WorkDescriptions DROP CONSTRAINT DF_WorkDescriptions_EditOnly

ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationAccountingCodes_FirstRestorationAccountingCodeID] FOREIGN KEY (
	[FirstRestorationAccountingCodeID]
) REFERENCES [RestorationAccountingCodes] (
	[RestorationAccountingCodeID]
)
GO
ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationAccountingCodes_SecondRestorationAccountingCodeID] FOREIGN KEY (
	[SecondRestorationAccountingCodeID]
) REFERENCES [RestorationAccountingCodes] (
	[RestorationAccountingCodeID]
)
GO

ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationProductCodes_FirstRestorationProductCodeID] FOREIGN KEY (
	[FirstRestorationProductCodeID]
) REFERENCES [RestorationProductCodes] (
	[RestorationProductCodeID]
)
GO
ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationProductCodes_SecondRestorationProductCodeID] FOREIGN KEY (
	[SecondRestorationProductCodeID]
) REFERENCES [RestorationProductCodes] (
	[RestorationProductCodeID]
)
GO

-- ADD NEW WORK DESCRIPTIONS
SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly])
SELECT 120, 4, N'HYDRANT PAINT', 2, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 121, 28, N'BALL/CURB STOP REPLACE', 4, 3.00, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0 UNION ALL
SELECT 122, 17, N'VALVE BLOW OFF RETIREMENT', 1, 2.00, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
SELECT 123, 17, N'VALVE BLOW OFF BROKEN', 1, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 124, 48, N'WATER MAIN RELOCATION', 3, 8.00, 1, 1, 49, 1, 26, 51, 1, 0, 1, 0 UNION ALL
SELECT 125, 48, N'HYDRANT RELOCATION', 2, 8.00, 1, 1, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 126, 48, N'SERVICE RELOCATION', 4, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 127, 46, N'SEWER INVESTIGATION-MAIN', 7, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 128, 42, N'SEWER SERVICE OVERFLOW', 6, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 129, 34, N'SEWER INVESTIGATION-LATERAL', 6, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 130, 46, N'SEWER INVESTIGATION-MANHOLE', 5, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 131, 50, N'SEWER LIFT STATION REPAIR', 7, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 132, 28, N'CURB BOX REPLACE', 4, 1.00, 1, 1, 60, 1, 26, 40, 1, 1, 0, 0 UNION ALL
SELECT 133, 28, N'SERVICE LINE/VALVE BOX REPLACE', 4, 1.00, 1, 1, 60, 1, 26, 40, 1, 1, 0, 0

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

-----------UPDATE ACCOUNTING VALUES FOR WORK DESCRIPTIONS-----------------------
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 2
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 56, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 44, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 1 WHERE WorkDescriptionID = 3
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 4
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 5
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 9
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 14
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 18
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 19
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 20
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 21
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 22
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 23
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 24
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 25
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 26
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 27
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 28
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 29
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 50, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 50, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 30
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 26, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 31
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 32
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 34
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 35
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 36
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 1 WHERE WorkDescriptionID = 37
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 38
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 40
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 41
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 42
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 43
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 44
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 47
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 56, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 44, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 1 WHERE WorkDescriptionID = 49
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 50
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 54
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 56
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 57
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 58
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 60, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 40, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 59
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 26, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 60
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 61
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 62
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 64
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 65
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 66
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 67
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 68
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 69
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 70
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 57, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 43, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 71
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 26, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 72
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 73
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 74
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 75
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 26, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 76
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 78
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 49, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 51, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 80
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 60, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 40, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 81
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 82
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 49, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 51, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 83
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 26, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 84
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 85
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 86
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 87
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 88
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 60, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 40, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 89
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 26, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 90
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 91
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 92
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 60, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 40, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 93
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 94
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 95
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 96
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 97
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 98
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 99
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 57, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 43, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 100
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 101
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 60, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 40, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 102
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 103
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 104
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 105
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 106
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 107
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 108
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 109
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 110
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 111
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 0 WHERE WorkDescriptionID = 112
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 113
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 114
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 115
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 116
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 2, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 1, [ShowApprovalAccounting] = 0, [EditOnly] = 1 WHERE WorkDescriptionID = 117
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 100, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = NULL, [SecondRestorationCostBreakdown] = NULL, [SecondRestorationProductCodeID] = NULL, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 118
UPDATE [WorkDescriptions] SET [FirstRestorationAccountingCodeID] = 1, [FirstRestorationCostBreakdown] = 57, [FirstRestorationProductCodeID] = 1, [SecondRestorationAccountingCodeID] = 26, [SecondRestorationCostBreakdown] = 43, [SecondRestorationProductCodeID] = 1, [ShowBusinessUnit] = 0, [ShowApprovalAccounting] = 1, [EditOnly] = 0 WHERE WorkDescriptionID = 119

-- UPDATE WORK DESCRIPTION DESCRIPTIONS

UPDATE [WorkDescriptions] SET [Description] = 'WATER MAIN BLEEDERS' WHERE [WorkDescriptionID] = 2
UPDATE [WorkDescriptions] SET [Description] = 'CURB BOX REPAIR' WHERE [WorkDescriptionID] = 6
UPDATE [WorkDescriptions] SET [Description] = 'CURB BOX REPAIR' WHERE [WorkDescriptionID] = 7
UPDATE [WorkDescriptions] SET [Description] = 'CURB BOX REPAIR' WHERE [WorkDescriptionID] = 8
UPDATE [WorkDescriptions] SET [Description] = 'BALL/CURB STOP REPAIR' WHERE [WorkDescriptionID] = 9
UPDATE [WorkDescriptions] SET [Description] = 'EXCAVATE METER BOX/SETTER' WHERE [WorkDescriptionID] = 14
UPDATE [WorkDescriptions] SET [Description] = 'HYDRANT FROZEN' WHERE [WorkDescriptionID] = 19
UPDATE [WorkDescriptions] SET [Description] = 'HYDRANT FLUSHING' WHERE [WorkDescriptionID] = 24
UPDATE [WorkDescriptions] SET [Description] = 'HYDRANT INVESTIGATION' WHERE [WorkDescriptionID] = 25
UPDATE [WorkDescriptions] SET [Description] = 'METER BOX/SETTER INSTALLATION' WHERE [WorkDescriptionID] = 33
UPDATE [WorkDescriptions] SET [Description] = 'VALVE BLOW OFF INSTALLATION' WHERE [WorkDescriptionID] = 34
UPDATE [WorkDescriptions] SET [Description] = 'FIRE SERVICE INSTALLATION' WHERE [WorkDescriptionID] = 35
UPDATE [WorkDescriptions] SET [Description] = 'INTERIOR SETTING REPAIR' WHERE [WorkDescriptionID] = 39
UPDATE [WorkDescriptions] SET [Description] = 'SERVICE INVESTIGATION' WHERE [WorkDescriptionID] = 40
UPDATE [WorkDescriptions] SET [Description] = 'MAIN INVESTIGATION' WHERE [WorkDescriptionID] = 41
UPDATE [WorkDescriptions] SET [Description] = 'LEAK IN METER BOX, INLET' WHERE [WorkDescriptionID] = 42
UPDATE [WorkDescriptions] SET [Description] = 'LEAK IN METER BOX, OUTLET' WHERE [WorkDescriptionID] = 43
UPDATE [WorkDescriptions] SET [Description] = 'METER BOX/SETTER INSTALLATION' WHERE [WorkDescriptionID] = 47
UPDATE [WorkDescriptions] SET [Description] = 'METER BOX ADJUSTMENT/RESETTER' WHERE [WorkDescriptionID] = 50
UPDATE [WorkDescriptions] SET [Description] = 'SERVICE LINE REPAIR' WHERE [WorkDescriptionID] = 57
UPDATE [WorkDescriptions] SET [Description] = 'TEST SHUT DOWN' WHERE [WorkDescriptionID] = 62
UPDATE [WorkDescriptions] SET [Description] = 'VALVE BOX BLOW OFF REPAIR' WHERE [WorkDescriptionID] = 65
UPDATE [WorkDescriptions] SET [Description] = 'SERVICE LINE/VALVE BOX REPAIR' WHERE [WorkDescriptionID] = 66
UPDATE [WorkDescriptions] SET [Description] = 'VALVE INVESTIGATION' WHERE [WorkDescriptionID] = 67
UPDATE [WorkDescriptions] SET [Description] = 'VALVE BLOW OFF REPAIR' WHERE [WorkDescriptionID] = 70
UPDATE [WorkDescriptions] SET [Description] = 'FLUSHING-SERVICE' WHERE [WorkDescriptionID] = 78
UPDATE [WorkDescriptions] SET [Description] = 'SERVICE LINE FLOW TEST' WHERE [WorkDescriptionID] = 79
UPDATE [WorkDescriptions] SET [Description] = 'METER BOX/SETTER REPLACE' WHERE [WorkDescriptionID] = 81
UPDATE [WorkDescriptions] SET [Description] = 'SEWER LATERAL-INSTALLATION' WHERE [WorkDescriptionID] = 87
UPDATE [WorkDescriptions] SET [Description] = 'SEWER LATERAL-REPAIR' WHERE [WorkDescriptionID] = 88
UPDATE [WorkDescriptions] SET [Description] = 'SEWER LATERAL-REPLACE' WHERE [WorkDescriptionID] = 89
UPDATE [WorkDescriptions] SET [Description] = 'SEWER LATERAL-RETIRE' WHERE [WorkDescriptionID] = 90
UPDATE [WorkDescriptions] SET [Description] = 'SEWER LATERAL-CUSTOMER SIDE' WHERE [WorkDescriptionID] = 91
UPDATE [WorkDescriptions] SET [Description] = 'SEWER MAIN OVERFLOW' WHERE [WorkDescriptionID] = 95
UPDATE [WorkDescriptions] SET [Description] = 'SEWER BACKUP-COMPANY SIDE' WHERE [WorkDescriptionID] = 96
UPDATE [WorkDescriptions] SET [Description] = 'SEWER BACKUP-CUSTOMER SIDE' WHERE [WorkDescriptionID] = 97
UPDATE [WorkDescriptions] SET [Description] = 'MARKOUT-CREW' WHERE [WorkDescriptionID] = 99
UPDATE [WorkDescriptions] SET [Description] = 'SERVICE-TURN ON' WHERE [WorkDescriptionID] = 113
UPDATE [WorkDescriptions] SET [Description] = 'SERVICE-TURN OFF' WHERE [WorkDescriptionID] = 114
UPDATE [WorkDescriptions] SET [Description] = 'METER-OBTAIN READ' WHERE [WorkDescriptionID] = 115
UPDATE [WorkDescriptions] SET [Description] = 'METER-FINAL/START READ' WHERE [WorkDescriptionID] = 116
UPDATE [WorkDescriptions] SET [Description] = 'METER-REPAIR TOUCH PAD' WHERE [WorkDescriptionID] = 117
UPDATE [WorkDescriptions] SET [Description] = 'VALVE BLOW OFF REPLACEMENT' WHERE [WorkDescriptionID] = 119

-- UPDATE EXISTING ORDERS WHERE CHANGE 
UPDATE [WorkOrders] SET [WorkDescriptionID] = 5, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Estimates') WHERE [WorkDescriptionID] = 6
UPDATE [WorkOrders] SET [WorkDescriptionID] = 5, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Revenue 500-1000') WHERE [WorkDescriptionID] = 7
UPDATE [WorkOrders] SET [WorkDescriptionID] = 5, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Compliance') WHERE [WorkDescriptionID] = 8

UPDATE [WorkOrders] SET [WorkDescriptionID] = 9, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Estimates') WHERE [WorkDescriptionID] = 10
UPDATE [WorkOrders] SET [WorkDescriptionID] = 9, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Revenue 150-500') WHERE [WorkDescriptionID] = 11
UPDATE [WorkOrders] SET [WorkDescriptionID] = 9, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Compliance') WHERE [WorkDescriptionID] = 12

UPDATE [WorkOrders] SET [WorkDescriptionID] = 14, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Estimates') WHERE [WorkDescriptionID] = 15
UPDATE [WorkOrders] SET [WorkDescriptionID] = 14, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Revenue 150-500') WHERE [WorkDescriptionID] = 16
UPDATE [WorkOrders] SET [WorkDescriptionID] = 14, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Compliance') WHERE [WorkDescriptionID] = 17

UPDATE [WorkOrders] SET PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Water Quality') WHERE [WorkDescriptionID] = 24

UPDATE [WorkOrders] SET [WorkDescriptionID] = 38, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Compliance') WHERE [WorkDescriptionID] = 39

UPDATE [WorkOrders] SET [WorkDescriptionID] = 47, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Estimates') WHERE [WorkDescriptionID] = 48

UPDATE [WorkOrders] SET [WorkDescriptionID] = 50, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Estimates') WHERE [WorkDescriptionID] = 51
UPDATE [WorkOrders] SET [WorkDescriptionID] = 50, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Revenue 150-500') WHERE [WorkDescriptionID] = 52
UPDATE [WorkOrders] SET [WorkDescriptionID] = 50, PurposeID = (SELECT [WorkOrderPurposeID] FROM [WorkOrderPurposes] WHERE [Description] = 'Compliance') WHERE [WorkDescriptionID] = 53

UPDATE [WorkOrders] SET [WorkDescriptionID] = (Select top 1 WorkDescriptionID from WorkDescriptions where [Description] = 'SERVICE LINE FLOW TEST' AND WorkDescriptionID <> 79) WHERE [WorkDescriptionID] = 79

UPDATE [WorkOrders] SET [WorkDescriptionID] = (Select top 1 WorkDescriptionID from WorkDescriptions where [Description] = 'METER BOX/SETTER INSTALLATION' AND WorkDescriptionID <> 33) WHERE [WorkDescriptionID] = 33

UPDATE [WorkOrders] SET [WorkDescriptionID] = 103 WHERE [WorkDescriptionID] = 57

UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 5 WHERE ToWorkDescriptionID in (6,7,8)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 9 WHERE ToWorkDescriptionID in (10,11,12)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 14 WHERE ToWorkDescriptionID in (15,16,17)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 38 WHERE ToWorkDescriptionID in (39)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 47 WHERE ToWorkDescriptionID in (48)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 50 WHERE ToWorkDescriptionID in (51,52,53)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 18 WHERE ToWorkDescriptionID in (79)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = (Select top 1 WorkDescriptionID from WorkDescriptions where [Description] = 'METER BOX/SETTER INSTALLATION' AND WorkDescriptionID <> 33) WHERE ToWorkDescriptionID in (33)
UPDATE [WorkOrderDescriptionChanges] SET ToWorkDescriptionID = 103 WHERE ToWorkDescriptionID in (57)

UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 5 WHERE FromWorkDescriptionID in (6,7,8)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 9 WHERE FromWorkDescriptionID in (10,11,12)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 14 WHERE FromWorkDescriptionID in (15,16,17)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 38 WHERE FromWorkDescriptionID in (39)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 47 WHERE FromWorkDescriptionID in (48)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 50 WHERE FromWorkDescriptionID in (51,52,53)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 18 WHERE FromWorkDescriptionID in (79)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = (Select top 1 WorkDescriptionID from WorkDescriptions where [Description] = 'METER BOX/SETTER INSTALLATION' AND WorkDescriptionID <> 33) WHERE FromWorkDescriptionID in (33)
UPDATE [WorkOrderDescriptionChanges] SET FromWorkDescriptionID = 103 WHERE FromWorkDescriptionID in (57)

-- REMOVE MERGED WORK DESCRIPTIONS
DELETE WorkDescriptions where WorkDescriptionID in (6,7,8,10,11,12,15,16,17,51,52,53,79,39,33,48,57)
