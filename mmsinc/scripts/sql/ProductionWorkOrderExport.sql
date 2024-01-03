SELECT this_.Id,
oc17_.OperatingCenterCode + ' - ' + oc17_.OperatingCenterName as OperatingCenter,
pp18_.Code + ' - ' + pp18_.Description as PlanningPlant,
cast(fac19_.RecordId as varchar) + ' - ' + fac19_.FacilityName as Facility,
this_.FunctionalLocation,
equipmentt52_.Abbreviation + ' - ' + equipmentt52_.[Description] as EquipmentClass,
e2_.Identifier as Equipment,
coord20_.CoordinateID as Coordinate,
pwop21_.[Description] as Priority,
(CASE WHEN DateCancelled IS NULL and DateCompleted IS NULL THEN 'True' ELSE 'False' END) as IsOpen,
pwd1_.[Description] as WorkDescription,
-- e.AirPermit,
-- e.HasLockoutRequirement,
-- e.HotWork,
-- e.IsConfinedSpace,
-- e.JobSafetyChecklist,

this_.CapitalizedFromId as CapitalizedFrom,
isnull(requestedb14_.First_Name + ' ', '') + isnull(requestedb14_.Middle_Name + ' ', '') + isnull(requestedb14_.Last_Name, '') as RequestedBy,
this_.Notes,
this_.DateReceived,
(CASE this_.BreakdownIndicator WHEN 1 THEN 'True' WHEN 0 THEN 'False' ELSE NULL END) as BreakdownIndicator,
this_.SAPWorkOrder,
this_.SAPErrorCode as SAPStatus,
this_.SAPNotificationNumber,
this_.WBSElement,
this_.CapitalizationReason,
this_.DateCompleted,
completedBy.UserName as CompletedBy,
this_.ApprovedOn,
approvedBy.UserName as ApprovedBy,
this_.MaterialsApprovedOn,
this_.MaterialsPlannedOn,
materialsApprovedBy.UserName as MaterialsApprovedBy,
this_.BasicStart,
this_.DateCancelled,
pwocr27_.[Description] as CancellationReason,
activityType.[Description] as PlantMaintenanceActivityTypeOverride,
isnull(copc23_.Code, '') + isnull(' - ' + copc23_.[Description], '')  as CorrectiveOrderProblemCode,
this_.OtherProblemNotes,
pwoac24_.[Description] as ActionCode,
pwofc25_.[Description] as FailureCode,
pwocc26_.[Description] as CauseCode,
(case production72_.RequiresSupervisorApproval when 1 then 'True' when 0 then 'False' else NULL end) as ProductionWOrkOrderRequiresSupervisorApproval,
(case when this_.MaterialsApprovedOn is not null and this_.MaterialsApprovedById is not null then 'True' else 'False' end) as MaterialsApproved,
-- e.Status,

isnull(ordertype28_.SAPCode, '') + ' - ' + isnull(ordertype28_.[Description], '') as OrderType,
-- e.Icon,

(case when oc17_.SAPEnabled = 1 and oc17_.IsContractedOperations <> 1 then 'True' else 'False' end) as SendToSap,
-- e.CanBeSupervisorApproved,
-- e.CanBeMaterialApproved,
-- e.CanBeCompleted,
-- e.CanBeCancelled,
-- e.CanBeMaterialPlanned,
-- e.CapitalizationCancelsOrder,
isnull(currentass5_.First_Name + ' ', '') + isnull(currentass5_.Middle_Name + ' ', '') + isnull(currentass5_.Last_Name, '') as CurrentlyAssignedEmployee

-- e.LockoutFormCreated

FROM ProductionWorkOrders this_
left outer join EmployeeAssignments currentass4_ on this_.Id=currentass4_.ProductionWorkOrderId
left outer join tblEmployee currentass5_ on currentass4_.AssignedToId=currentass5_.tblEmployeeID
left outer join tblPermissions currentass6_ on currentass5_.tblEmployeeID=currentass6_.EmployeeId
left outer join tblEmployee currentass7_ on currentass4_.AssignedById=currentass7_.tblEmployeeID
left outer join tblPermissions currentass8_ on currentass7_.tblEmployeeID=currentass8_.EmployeeId
left outer join ProductionWorkOrdersProductionPrerequisites ppp3_ on this_.Id=ppp3_.ProductionWorkOrderId
left outer join EmployeeAssignments assignment9_ on this_.Id=assignment9_.ProductionWorkOrderId
left outer join tblEmployee assignedto10_ on assignment9_.AssignedToId=assignedto10_.tblEmployeeID
left outer join tblPermissions assignedto11_ on assignedto10_.tblEmployeeID=assignedto11_.EmployeeId
left outer join tblEmployee assignedby12_ on assignment9_.AssignedById=assignedby12_.tblEmployeeID
left outer join tblPermissions assignedby13_ on assignedby12_.tblEmployeeID=assignedby13_.EmployeeId
left outer join ProductionWorkOrderMaterialUsed pwomateria16_ on this_.Id=pwomateria16_.ProductionWorkOrderId
left outer join OperatingCenters oc17_ on this_.OperatingCenterId=oc17_.OperatingCenterID
left outer join PlanningPlants pp18_ on this_.PlanningPlantId=pp18_.Id
left outer join tblFacilities fac19_ on this_.FacilityId=fac19_.RecordID
left outer join ProcessStages processsta47_ on fac19_.ProcessId=processsta47_.ProcessStageID
left outer join SAPEquipmentTypes eqt22_ on this_.EquipmentClassId=eqt22_.Id
left outer join Equipment e2_ on this_.EquipmentId=e2_.EquipmentID
left outer join FilterMedia filtermedi50_ on e2_.EquipmentID=filtermedi50_.EquipmentId
left outer join EquipmentLinks links51_ on e2_.EquipmentID=links51_.EquipmentId
left outer join EquipmentTypes equipmentt52_ on e2_.TypeID=equipmentt52_.EquipmentTypeID
left outer join EquipmentCategories equipmentc53_ on equipmentt52_.CategoryID=equipmentc53_.EquipmentCategoryID
left outer join EquipmentSubCategories equipments54_ on equipmentt52_.SubCategoryID=equipments54_.EquipmentSubCategoryID
left outer join EquipmentStatuses equipments55_ on e2_.StatusID=equipments55_.EquipmentStatusID
left outer join EquipmentModels equipmentm56_ on e2_.ModelID=equipmentm56_.EquipmentModelID
left outer join SAPEquipmentManufacturers sapequipme57_ on e2_.SAPEquipmentManufacturerId=sapequipme57_.Id
left outer join Generators generator58_ on e2_.EquipmentID=generator58_.EquipmentId
left outer join Coordinates coord20_ on this_.CoordinateId=coord20_.CoordinateID
left outer join MapIcon mapicon29_ on coord20_.IconId=mapicon29_.iconID
left outer join MapIconOffsets mapiconoff30_ on mapicon29_.OffsetId=mapiconoff30_.Id
left outer join ProductionWorkOrderPriorities pwop21_ on this_.PriorityId=pwop21_.Id
left outer join ProductionWorkDescriptions pwd1_ on this_.ProductionWorkDescriptionId=pwd1_.Id
left outer join OrderTypes ordertype28_ on pwd1_.OrderTypeId=ordertype28_.Id
left outer join tblEmployee requestedb14_ on this_.RequestedById=requestedb14_.tblEmployeeID
left outer join tblPermissions requestedb15_ on requestedb14_.tblEmployeeID=requestedb15_.EmployeeId
left outer join ProductionWorkOrderCancellationReasons pwocr27_ on this_.CancellationReasonId=pwocr27_.Id
left outer join CorrectiveOrderProblemCodes copc23_ on this_.CorrectiveOrderProblemCodeId=copc23_.Id
left outer join ProductionWorkOrderActionCodes pwoac24_ on this_.ActionCodeId=pwoac24_.Id
left outer join ProductionWorkOrderFailureCodes pwofc25_ on this_.FailureCodeId=pwofc25_.Id
left outer join ProductionWorkOrderCauseCodes pwocc26_ on this_.CauseCodeId=pwocc26_.Id
left outer join ProductionWorkOrderRequiresSupervisorApproval production72_ on this_.Id=production72_.ProductionWorkOrderId
left outer join tblPermissions completedBy on this_.CompletedById=completedBy.EmployeeId
left outer join tblPermissions approvedBy on this_.ApprovedById=approvedBy.EmployeeId
left outer join tblPermissions materialsApprovedBy on this_.MaterialsApprovedById=materialsApprovedBy.EmployeeId
left outer join PlantMaintenanceActivityTypes activityType on this_.PlantMaintenanceActivityTypeOverrideId=activityType.Id
WHERE (this_.DateReceived >= '2020-01-01 00:00:00' and this_.DateReceived < '2020-10-01 00:00:00')
