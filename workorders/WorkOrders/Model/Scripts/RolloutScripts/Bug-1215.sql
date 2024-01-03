use mcprod

insert into OperatingCenterStockedMaterials Values(
	(select operatingcenterID from OperatingCenters where OperatingCenterCode = 'EW3'),
	(select materialID from materials where partNumber = 'W2150000')
)
