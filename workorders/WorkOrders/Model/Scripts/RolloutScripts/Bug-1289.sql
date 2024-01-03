-- WorkOrders.AccountCharged
BEGIN TRAN

ALTER TABLE WorkOrders ADD AccountChargedNew varchar(18) null
GO
update workorders set AccountChargedNew = AccountCharged
GO
ALTER TABLE WorkOrders DROP COLUMN AccountCharged
GO
EXECUTE sp_rename N'dbo.WorkOrders.AccountChargedNew', N'AccountCharged', 'COLUMN' 
GO

COMMIT TRAN