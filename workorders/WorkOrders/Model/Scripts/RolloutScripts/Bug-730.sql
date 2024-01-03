use [McProd]
GO

CREATE TABLE [RestorationResponsePriorities] (
	[RestorationResponsePriorityID] int unique identity not null,
	[Description] varchar(25) not null,
	CONSTRAINT [PK_RestorationResponsePriorities] PRIMARY KEY CLUSTERED (
		[RestorationResponsePriorityID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GRANT ALL ON [RestorationResponsePriorities] TO mcuser;
GRANT ALL ON [RestorationAccountingCodes] TO mcuser;
GRANT ALL ON [RestorationProductCodes] TO mcuser;

ALTER TABLE [Restorations] ADD [ResponsePriorityID] int null;

ALTER TABLE [Restorations]  ADD CONSTRAINT [FK_Restorations_RestorationResponsePriorities_ResponsePriorityID] FOREIGN KEY (
	[ResponsePriorityID]
) REFERENCES [RestorationResponsePriorities] (
	[RestorationResponsePriorityID]
)
GO

INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Emergency 5 day');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Emergency 24 hour');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Emergency 48 hour');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('On Demand OT/Holiday');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('On Demand Same Day');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Priority (10 days)');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Standard (30 days)');

UPDATE [Restorations] SET [ResponsePriorityID] = (SELECT [RestorationResponsePriorityID] FROM [RestorationResponsePriorities] WHERE [Description] = 'Standard (30 days)');

ALTER TABLE [WorkOrders] ADD BusinessUnit char(6) NULL

CREATE TABLE [Departments] (
[DepartmentID] int unique identity not null,
[Code] char(2) not null,
[Description] varchar(50) not null,
CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED (
[DepartmentID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[Departments] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[Departments]([DepartmentID], [Code], [Description])
SELECT 1, N'02', N'T&D' UNION ALL
SELECT 2, N'06', N'CFS' UNION ALL
SELECT 3, N'01', N'Production' UNION ALL
SELECT 4, N'03', N'FRCC' UNION ALL
SELECT 5, N'16', N'Maintenance Services' UNION ALL
SELECT 6, N'23', N'Fleet/Materials Mgmt'
COMMIT;
RAISERROR (N'[dbo].[Departments]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[Departments] OFF;


CREATE TABLE [BusinessUnits] (
[BusinessUnitID] int unique identity not null,
[Description] char(6) not null,
[OperatingCenterID] int not null,
[DepartmentID] int not null,
[Order] int not null,
CONSTRAINT [PK_BusinessUnits] PRIMARY KEY CLUSTERED (
[BusinessUnitID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [BusinessUnits]  ADD CONSTRAINT [FK_BusinessUnits_tblOpCntr_OperatingCenterID] FOREIGN KEY (
[OperatingCenterID]
) REFERENCES [tblOpCntr] (
[RecID]
)
GO

ALTER TABLE [BusinessUnits]  WITH NOCHECK ADD CONSTRAINT [FK_BusinessUnits_Departments_DepartmentID] FOREIGN KEY (
[DepartmentID]
) REFERENCES [Departments] (
[DepartmentID]
)
GO

SET IDENTITY_INSERT [dbo].[BusinessUnits] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[BusinessUnits]([BusinessUnitID], [Description], [OperatingCenterID], [DepartmentID], [Order])
SELECT 1, N'522602', 15, 1, 1 UNION ALL
SELECT 2, N'522602', 15, 1, 1 UNION ALL
SELECT 3, N'522606', 15, 2, 1 UNION ALL
SELECT 4, N'522502', 16, 1, 1 UNION ALL
SELECT 5, N'522506', 16, 2, 1 UNION ALL
SELECT 6, N'533002', 17, 1, 1 UNION ALL
SELECT 7, N'533006', 17, 2, 99 UNION ALL
SELECT 8, N'558202', 19, 1, 1 UNION ALL
SELECT 9, N'548102', 18, 1, 1 UNION ALL
SELECT 10, N'181202', 11, 1, 1 UNION ALL
SELECT 11, N'181206', 11, 2, 99 UNION ALL
SELECT 12, N'182202', 11, 1, 2 UNION ALL
SELECT 13, N'182206', 11, 2, 99 UNION ALL
SELECT 14, N'189102', 11, 1, 3 UNION ALL
SELECT 15, N'189106', 11, 2, 99 UNION ALL
SELECT 16, N'189116', 11, 5, 99 UNION ALL
SELECT 17, N'189202', 11, 1, 4 UNION ALL
SELECT 18, N'189206', 11, 2, 99 UNION ALL
SELECT 19, N'181902', 14, 1, 1 UNION ALL
SELECT 20, N'181906', 14, 2, 99 UNION ALL
SELECT 21, N'181102', 14, 1, 3 UNION ALL
SELECT 22, N'181106', 14, 2, 99 UNION ALL
SELECT 23, N'182302', 14, 1, 4 UNION ALL
SELECT 24, N'182306', 14, 2, 99 UNION ALL
SELECT 25, N'181302', 13, 1, 1 UNION ALL
SELECT 26, N'181306', 13, 2, 99 UNION ALL
SELECT 27, N'183102', 13, 1, 2 UNION ALL
SELECT 28, N'183106', 13, 2, 99 UNION ALL
SELECT 29, N'183202', 13, 1, 3 UNION ALL
SELECT 30, N'183206', 13, 2, 99 UNION ALL
SELECT 31, N'181502', 12, 1, 1 UNION ALL
SELECT 32, N'181506', 12, 2, 99 UNION ALL
SELECT 33, N'181702', 12, 1, 99 UNION ALL
SELECT 34, N'181706', 12, 2, 99 UNION ALL
SELECT 35, N'181802', 10, 1, 1 UNION ALL
SELECT 36, N'181806', 10, 2, 99
COMMIT;
RAISERROR (N'[dbo].[BusinessUnits]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[BusinessUnits] OFF;

-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ1'), '180102', (Select DepartmentID from Departments where [Code] = '02'), 1
-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ1'), '180103', (Select DepartmentID from Departments where [Code] = '03'), 99
-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ1'), '180123', (Select DepartmentID from Departments where [Code] = '23'), 99
-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ2'), '180106', (Select DepartmentID from Departments where [Code] = '06'), 99

GRANT ALL ON [BusinessUnits] TO MCUser
GRANT ALL ON [Departments] TO MCUser

