use MCProd
GO
ALTER TABLE [WorkDescriptions] ADD
	Revisit bit NOT NULL 
CONSTRAINT DF_WorkDescriptions_Revisit DEFAULT 0

ALTER TABLE [WorkOrders] ADD [OriginalOrderNumber] int null
GO

ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_WorkOrders_OriginalOrderNumber] FOREIGN KEY (
[OriginalOrderNumber]
) REFERENCES [WorkOrders] (
[WorkOrderID]
)
GO
