use mcprod

insert into OperatingCenterStockedMaterials Values(
	(select operatingcenterID from OperatingCenters where OperatingCenterCode = 'EW3'),
	(select materialID from materials where partNumber = 'W5181066')
)

insert into OperatingCenterStockedMaterials Values(
	(select operatingcenterID from OperatingCenters where OperatingCenterCode = 'NJ5'),
	(select materialID from materials where partNumber = 'W5181066')
)

select 
	* 
from 
	OperatingCenterStockedMaterials 
where 
	OperatingCenterID = (select operatingcenterID from OperatingCenters where OperatingCenterCode = 'NJ5')
and 
	MaterialID = (select materialID from materials where partNumber = 'W5181066')
	
select 
	* 
from 
	OperatingCenterStockedMaterials 
where 
	OperatingCenterID = (select operatingcenterID from OperatingCenters where OperatingCenterCode = 'EW3')
and 
	MaterialID = (select materialID from materials where partNumber = 'W5181066')