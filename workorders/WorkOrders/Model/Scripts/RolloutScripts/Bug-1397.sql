DBCC CHECKIDENT(AssetTypes, RESEED, 8);

DECLARE @EquipmentAssetTypeID int;
DECLARE @EquipmentWorkCategoryID int;
DECLARE @OMAccountingTypeID int;

INSERT INTO AssetTypes (Description) VALUES ('Equipment');
SELECT @EquipmentAssetTypeID = @@IDENTITY;

INSERT INTO WorkCategories (Description) VALUES ('Equipment');
SELECT @EquipmentWorkCategoryID = @@IDENTITY;

SELECT @OMAccountingTypeID = AccountingTypeID FROM AccountingTypes WHERE Description = 'O&M';

INSERT INTO [WorkDescriptions] (Description, AssetTypeID, TimeToComplete, WorkCategoryID, AccountingTypeID, FirstRestorationAccountingCodeID, FirstRestorationCostBreakdown, FirstRestorationProductCodeID, ShowBusinessUnit, ShowApprovalAccounting, EditOnly)
VALUES ('Pump Repair', @EquipmentAssetTypeID, 2, @EquipmentWorkCategoryID, @OMAccountingTypeID, 1, 100, 1, 0, 0, 0);
INSERT INTO [WorkDescriptions] (Description, AssetTypeID, TimeToComplete, WorkCategoryID, AccountingTypeID, FirstRestorationAccountingCodeID, FirstRestorationCostBreakdown, FirstRestorationProductCodeID, ShowBusinessUnit, ShowApprovalAccounting, EditOnly)
VALUES ('Line Stop Repair', @EquipmentAssetTypeID, 2, @EquipmentWorkCategoryID, @OMAccountingTypeID, 1, 100, 1, 0, 0, 0);
INSERT INTO [WorkDescriptions] (Description, AssetTypeID, TimeToComplete, WorkCategoryID, AccountingTypeID, FirstRestorationAccountingCodeID, FirstRestorationCostBreakdown, FirstRestorationProductCodeID, ShowBusinessUnit, ShowApprovalAccounting, EditOnly)
VALUES ('Saw Repair', @EquipmentAssetTypeID, 2, @EquipmentWorkCategoryID, @OMAccountingTypeID, 1, 100, 1, 0, 0, 0);
INSERT INTO [WorkDescriptions] (Description, AssetTypeID, TimeToComplete, WorkCategoryID, AccountingTypeID, FirstRestorationAccountingCodeID, FirstRestorationCostBreakdown, FirstRestorationProductCodeID, ShowBusinessUnit, ShowApprovalAccounting, EditOnly)
VALUES ('Vehicle Repair', @EquipmentAssetTypeID, 2, @EquipmentWorkCategoryID, @OMAccountingTypeID, 1, 100, 1, 0, 0, 0);
INSERT INTO [WorkDescriptions] (Description, AssetTypeID, TimeToComplete, WorkCategoryID, AccountingTypeID, FirstRestorationAccountingCodeID, FirstRestorationCostBreakdown, FirstRestorationProductCodeID, ShowBusinessUnit, ShowApprovalAccounting, EditOnly)
VALUES ('Misc repair', @EquipmentAssetTypeID, 2, @EquipmentWorkCategoryID, @OMAccountingTypeID, 1, 100, 1, 0, 0, 0);

INSERT INTO OperatingCenterAssetTypes (AssetTypeID, OperatingCenterID)
SELECT
	(SELECT AssetTypeID FROM AssetTypes WHERE Description = 'Equipment'),
	OperatingCenterID
FROM
	OperatingCenters
WHERE
	WorkOrdersEnabled = 1
