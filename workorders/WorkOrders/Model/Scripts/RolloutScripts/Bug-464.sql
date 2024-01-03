USE [McProd]
GO

ALTER TABLE [WorkOrders] ADD OfficeAssignmentID int NULL;
ALTER TABLE [WorkOrders] ADD OfficeAssignedOn smalldatetime NULL;

ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblPermissions_OfficeAssignmentID] FOREIGN KEY (
	[OfficeAssignmentID]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO
