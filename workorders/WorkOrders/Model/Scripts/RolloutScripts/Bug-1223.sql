--Please add the following part number to the material parts list for NJ5 and
--EW3.
--Part # W5181066, hydrant delran 4'0" bury
USE MCProd

INSERT INTO OperatingCenterStockedMaterials 
SELECT 
	(select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'EW3'),
	(Select MaterialID from Materials where PartNumber = 'W5181065')

INSERT INTO OperatingCenterStockedMaterials 
SELECT 
	(select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ5'),
	(Select MaterialID from Materials where PartNumber = 'W5181065')
	
	
select 
	* 
from 
	OperatingCenterStockedMaterials 
where 
	OperatingCenterID = (select operatingcenterID from OperatingCenters where OperatingCenterCode = 'NJ5')
and 
	MaterialID = (select materialID from materials where partNumber = 'W5181065')
	
select 
	* 
from 
	OperatingCenterStockedMaterials 
where 
	OperatingCenterID = (select operatingcenterID from OperatingCenters where OperatingCenterCode = 'EW3')
and 
	MaterialID = (select materialID from materials where partNumber = 'W5181065')