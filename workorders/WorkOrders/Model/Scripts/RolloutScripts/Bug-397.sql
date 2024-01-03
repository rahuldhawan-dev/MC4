USE MCProd
GO
-- Rename
EXECUTE sp_rename N'dbo.WorkOrderDescriptionChanges.WorkDescriptionID', N'ToWorkDescriptionID', 'COLUMN' 
GO

-- Add Column
ALTER TABLE dbo.WorkOrderDescriptionChanges ADD
	FromWorkDescriptionID int NULL

-- FK
ALTER TABLE [WorkOrderDescriptionChanges] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrderDescriptionChanges_WorkDescriptions_FromWorkDescriptionID] FOREIGN KEY (
[FromWorkDescriptionID]
) REFERENCES [WorkDescriptions] (
[WorkDescriptionID]
)
GO
