use MCProd
go

BEGIN TRAN 

ALTER TABLE WorkOrders ADD AssignedContractorID int
GO

ALTER TABLE WorkOrders ADD AssignedToContractorOn smalldatetime null
GO

ALTER TABLE [WorkOrders]  WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_Contractors_AssignedContractorID] FOREIGN KEY (
[AssignedContractorID]
) REFERENCES [Contractors] (
[ContractorID]
)
GO

ROLLBACK TRAN