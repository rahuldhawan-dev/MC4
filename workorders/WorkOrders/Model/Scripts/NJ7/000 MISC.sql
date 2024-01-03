-- there are some work orders where the valve/hydrant ids didn't match anything in nj7.  this will remove them, for now

delete from
	Restorations
where
	WorkOrderID in
		(select
			WorkOrderID
		from
			WorkOrders
		where
			(AssetTypeID = (select AssetTypeID from AssetTypes where Description = 'Hydrant')
		and
			[HydrantID] not in (select recid from tblNJAWHydrant))
		or
			(AssetTypeID = (select AssetTypeID from AssetTypes where Description = 'Valve')
		and
			[ValveID] not in (select recid from tblNJAWValves)))

delete from
	Markouts
where
	WorkOrderID in
		(select
			WorkOrderID
		from
			WorkOrders
		where
			(AssetTypeID = (select AssetTypeID from AssetTypes where Description = 'Hydrant')
		and
			[HydrantID] not in (select recid from tblNJAWHydrant))
		or
			(AssetTypeID = (select AssetTypeID from AssetTypes where Description = 'Valve')
		and
			[ValveID] not in (select recid from tblNJAWValves)))

delete from
	WorkOrders
where
	(AssetTypeID = (select AssetTypeID from AssetTypes where Description = 'Hydrant')
and
	[HydrantID] not in (select recid from tblNJAWHydrant))
or
	(AssetTypeID = (select AssetTypeID from AssetTypes where Description = 'Valve')
and
	[ValveID] not in (select recid from tblNJAWValves))
