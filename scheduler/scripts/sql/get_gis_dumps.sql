WITH WorkOrders_CTE (RowNumber, Id, ValveId, AccountCharged)
AS
(
    SELECT
        row_number() over (
            partition by ValveId
            order by CreatedOn desc
        ),
        WorkOrderId,
        ValveId,
        AccountCharged
    FROM WorkOrders
    WHERE WorkDescriptionId IN (71, 72, 118)
), TopWorkOrders_CTE (Id, ValveId, AccountCharged)
AS
(
    SELECT
        Id,
        ValveId,
        AccountCharged
    FROM
        WorkOrders_CTE
    WHERE RowNumber = 1
)
select valve.Id                                             as [Id],
valveControl.Id                                             as [ValveControl.Id],
valveControl.Description                                    as [ValveControl.Description],
valveSize.Id                                                as [ValveSize.Id],
valveSize.Size                                              as [ValveSize.Size],
left(convert(varchar, valve.DateInstalled, 126) + '.0000000', 27) + 'Z' as [DateInstalled],
assetStatus.AssetStatusID                                   as [AssetStatus.Id],
assetStatus.Description                                     as [AssetStatus.Description],
valveMake.Id                                                as [ValveMake.Id],
valveMake.Description                                       as [ValveMake.Description],
valveNormalPosition.Id                                      as [ValveNormalPosition.Id],
valveNormalPosition.Description                             as [ValveNormalPosition.Description],
valveOpenDirection.Id                                       as [ValveOpenDirection.Id],
valveOpenDirection.Description                              as [ValveOpenDirection.Description],
valve.Route                                                 as [Route],
valve.SAPEquipmentID                                        as [SAPEquipmentId],
valve.Stop                                                  as [Stop],
valve.Turns                                                 as [Turns],
valve.ValveNumber                                           as [ValveNumber],
valveType.Id                                                as [ValveType.Id],
valveType.Description                                       as [ValveType.Description],
left(convert(varchar, valve.LastUpdated, 126) + '00000', 27) + 'Z' as [LastUpdated],
[user].RecId                                                as [LastUpdatedBy.Id],
[user].UserName                                             as [LastUpdatedBy.UserName],
workOrder.Id                                                as [WorkOrderId],
workOrder.AccountCharged                                    as [WBSNumber],
COALESCE(state.StateId, townState.StateId)                  as [State.Id],
COALESCE(state.Abbreviation, townState.Abbreviation)        as [State.Abbreviation]
from Valves valve
left outer join ValveControls valveControl on valve.ValveControlsId=valveControl.Id
left outer join ValveSizes valveSize on valve.ValveSizeId=valveSize.Id
left outer join AssetStatuses assetStatus on valve.AssetStatusId=assetStatus.AssetStatusID
left outer join ValveManufacturers valveMake on valve.ValveMakeId=valveMake.Id
left outer join ValveNormalPositions valveNormalPosition on valve.NormalPositionId=valveNormalPosition.Id
left outer join ValveOpenDirections valveOpenDirection on valve.OpensId=valveOpenDirection.Id
left outer join ValveTypes valveType on valve.ValveTypeId=valveType.Id
left outer join tblPermissions [user] on valve.LastUpdatedById=[user].RecId
left outer join TopWorkOrders_CTE workOrder on valve.Id = workOrder.valveId
left outer join OperatingCenters operatingCenter on valve.OperatingCenterId=operatingCenter.OperatingCenterID
left outer join States state on operatingCenter.StateId=state.StateID
left outer join Towns town on valve.Town=town.TownID
left outer join States townState on town.StateId=townState.StateID
where state.Abbreviation IN ('CA', 'HI', 'IL', 'IA', 'KY', 'MD', 'NJ', 'NY', 'VA')
or townState.Abbreviation IN ('CA', 'HI', 'IL', 'IA', 'KY', 'MD', 'NJ', 'NY', 'VA')
or town.Town = 'JOPLIN'
FOR JSON PATH, ROOT('Valves');