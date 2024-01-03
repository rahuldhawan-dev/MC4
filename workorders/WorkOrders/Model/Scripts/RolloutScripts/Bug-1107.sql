BEGIN TRAN

declare @materialId int
insert into dbo.Materials (PartNumber, Description) values ('W0821F01', 'Reducer, 1 1/2" X 1" BR')
set @materialId = (select @@IDENTITY)

insert into dbo.OperatingCenterStockedMaterials (OperatingCenterID, MaterialID) values ((select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'EW1'), @materialId)
insert into dbo.OperatingCenterStockedMaterials (OperatingCenterID, MaterialID) values ((select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'EW2'), @materialId)
insert into dbo.OperatingCenterStockedMaterials (OperatingCenterID, MaterialID) values ((select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'EW4'), @materialId)
insert into dbo.OperatingCenterStockedMaterials (OperatingCenterID, MaterialID) values ((select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'LWC'), @materialId)

ROLLBACK