BEGIN TRANSACTION
GO
ALTER TABLE dbo.WorkOrders ADD
	DistanceFromCrossStreet decimal(18, 0) NULL
GO
COMMIT