use [McProd]
GO

ALTER TABLE [WorkOrders] ADD [MarkoutToBeCalled] smalldatetime null;
ALTER TABLE [WorkOrders] ADD [MarkoutTypeNeededID] int null;

ALTER TABLE [WorkOrders]  ADD CONSTRAINT [FK_WorkOrders_MarkoutTypes_MarkoutTypeNeededID] FOREIGN KEY (
[MarkoutTypeNeededID]
) REFERENCES [MarkoutTypes] (
[MarkoutTypeID]
)
GO
