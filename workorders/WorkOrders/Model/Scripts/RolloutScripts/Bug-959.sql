USE MCProd
GO
ALTER TABLE Markouts Add CreatorID Int NULL
GO
ALTER TABLE [Markouts]  WITH NOCHECK ADD CONSTRAINT [FK_Markouts_tblPermissions_CreatorID] FOREIGN KEY (
[CreatorID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

ALTER TABLE [WorkOrders] ADD RequiredMarkoutNote text null;
