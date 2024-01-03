use [McProd]
GO

CREATE NONCLUSTERED INDEX [IDX_Markouts_WorkOrderID_ExpirationDate] ON [dbo].[Markouts] 
(
	[WorkOrderID] ASC,
	[ExpirationDate] ASC
)

CREATE NONCLUSTERED INDEX [IDX_WorkOrders_SchedulingSearch] ON [dbo].[WorkOrders] 
(
	[MarkoutRequirementID] ASC,
	[OperatingCenterID] ASC,
	[AssetTypeID] ASC,
	[TownID] ASC,
	[PriorityID] ASC,
	[StreetOpeningPermitRequired] ASC,
	[DateCompleted] ASC
)
