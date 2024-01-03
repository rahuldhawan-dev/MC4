
insert into OperatingCenterStockedMaterials(OperatingCenterID, MaterialID)
	select
		OperatingCenterID,
		MaterialID 
	from
		Materials M, OperatingCenters OC
	where 
		M.PartNumber in (1405339, 1405468, 1405422, 1406351)
	and		
		OC.OperatingCenterCode in ('EW1', 'EW2', 'EW4', 'LWC')
	
