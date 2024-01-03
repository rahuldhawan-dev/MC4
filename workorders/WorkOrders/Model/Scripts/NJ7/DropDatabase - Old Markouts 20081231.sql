use [WorkOrdersTest]
GO

/* USE THE FOLLOWING TO REMOVE ANY STRAGGLING DATA:
delete from coordinates
delete from detectedleaks
delete from individualmarkouts
delete from leakreportingsources where leakreportingsourceid > 4
delete from lostwater
delete from mainbreaks
delete from mainbreakvalveoperations
delete from markouts
delete from markoutrequirements where markoutrequirementid < 3
delete from materialsused
delete from safetymarkers
delete from tblNJAWValves
delete from workorders
delete from tblnjawtownnames where recid > 297
delete from tblnjawtwnsection where recid > 143
delete from tblnjawstreets where recid > 62648
delete from tblpermissions
delete from workorderdescriptionchanges
delete from workdescriptions where workdescriptionid > 94
delete from workorderpriorities where workorderpriorityid > 4
delete from workorderpurposes where workorderpurposeid > 4
delete from workorderrequesters where workorderrequesterid > 3
delete from mainconditions where mainconditionid > 13
delete from mainsizes where mainsizeid > 19
delete from markouttypes where markouttypeid > 3
delete from workareatypes where workareatypeid > 4
*/

/* USE THE FOLLOWING TO REMOVE THE ENTIRE DATABASE
 * YOU'LL NEED TO RUN IT A FEW TIMES
drop table [AssetTypes];
drop table [Coordinates];
drop table [Crews];
drop table [DetectedLeaks];
drop table [EmployeeWorkOrders];
drop table [IndividualMarkouts];
drop table [LeakReportingSources];
drop table [LostWater];
drop table [MainBreaks];
drop table [MainBreakValveOperations];
drop table [MainConditions];
drop table [MainFailureTypes];
drop table [MainSizes];
drop table [Markouts];
drop table [MarkoutStatuses];
drop table [MarkoutRequirements];
drop table [MarkoutTypes];
drop table [MaterialsUsed];
drop table [Materials];
drop table [OperatingCenterStockedMaterials];
drop table [RestorationMethods];
drop table [SafetyMarkers];
drop table [WorkAreaTypes];
drop table [WorkDescriptions];
drop table [WorkOrderDescriptionChanges];
drop table [WorkOrderPriorities];
drop table [WorkOrderPurposes];
drop table [WorkOrderRequesters];
drop table [WorkOrders];

drop table [tblPermissions]
drop table [tblNJAWValves]
drop table [tblNJAWHydrant]
drop table [tblNJAWStreets]
drop table [tblNJAWTownNames]
drop table [TblNJAWTwnSection]
drop table [tblOpCntr]
*/
