ALTER TABLE [WorkOrders] ADD [CompletedByID] int null;

ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblPermissions_CompletedByID] FOREIGN KEY (
	[CompletedByID]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO
