use [MCProd]
go

CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_8_1888777836__K20_K23_K35] ON [dbo].[WorkOrders] 
(
	[ORCOMServiceOrderNumber] ASC,
	[DateCompleted] ASC,
	[WorkDescriptionID] ASC
)
go

CREATE STATISTICS [_dta_stat_1888777836_23_35] ON [dbo].[WorkOrders]([DateCompleted], [WorkDescriptionID])
go

CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_8_1888777836__K48_K20_K23_K35] ON [dbo].[WorkOrders] 
(
	[OperatingCenterID] ASC,
	[ORCOMServiceOrderNumber] ASC,
	[DateCompleted] ASC,
	[WorkDescriptionID] ASC
)
go

CREATE STATISTICS [_dta_stat_1888777836_23_48_20_35] ON [dbo].[WorkOrders]([DateCompleted], [OperatingCenterID], [ORCOMServiceOrderNumber], [WorkDescriptionID])
go