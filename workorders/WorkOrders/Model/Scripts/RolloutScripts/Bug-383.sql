use [McProd]
GO

ALTER TABLE [WorkOrderPurposes] ALTER COLUMN [Description] varchar(20);
GO

INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue 150-500');
INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue 500-1000');
INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue >1000');
