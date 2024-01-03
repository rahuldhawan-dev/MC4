begin tran
use MCProd
go

ALTER TABLE WorkOrders ADD AccountChargedNew varchar(30) Null
GO

UPDATE workorders SET AccountChargedNew = AccountCharged

ALTER TABLE workorders DROP COLUMN AccountCharged
GO

EXECUTE sp_rename N'dbo.workOrders.AccountChargedNew', N'AccountCharged', 'COLUMN'
GO

commit tran