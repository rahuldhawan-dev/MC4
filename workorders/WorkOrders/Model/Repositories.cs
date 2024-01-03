namespace WorkOrders.Model
{
    public class CoordinateRepository : WorkOrdersRepository<Coordinate>
    {
    }

    public class DetectedLeakRepository : WorkOrdersRepository<DetectedLeak>
    {
    }

    public class EmployeeWorkOrderRepository : WorkOrdersRepository<EmployeeWorkOrder>
    {
    }

    public class LostWaterRepository : WorkOrdersRepository<LostWater>
    {
    }

    public class MainBreakValveOperationRepository : WorkOrdersRepository<MainBreakValveOperation>
    {
    }

    public class RestorationTypeRepository : WorkOrdersRepository<RestorationType>
    {
    }

    public class RestorationMethodRepository : WorkOrdersRepository<RestorationMethod>
    {
    }

    public class SafetyMarkerRepository : WorkOrdersRepository<SafetyMarker>
    {
    }

    public class WorkAreaTypeRepository : WorkOrdersRepository<WorkAreaType>
    {
    }

    public class WorkCategoryRepository : WorkOrdersRepository<WorkCategory>
    {
    }

    public class WorkOrderDescriptionChangeRepository : WorkOrdersRepository<WorkOrderDescriptionChange>
    {
    }

    public class StormCatchRepository : WorkOrdersRepository<StormCatch>
    {
    }

    public class EquipmentRepository : WorkOrdersRepository<Equipment>
    {
    }

    public class MainCrossingRepository : WorkOrdersRepository<MainCrossing> { }

    public class DepartmentRepository : WorkOrdersRepository<Department>
    {
        public struct Indices
        {
            public const short T_AND_D = 1,
                               CFS = 2,
                               PRODUCTION = 3,
                               FRCC = 4,
                               MAINTENANCE_SERVICES = 5,
                               FLEET_MATERIALS_MGMT = 6,
                               ADMINISTRATION = 7,
                               BUSINESS_DEVELOPMENT = 8,
                               COMMUNICATIONS = 9,
                               ENGINEERING = 10,
                               ENVIRONMENTAL_COMPLIANCE = 11,
                               FINANCE = 12,
                               HUMAN_RESOURCES = 13,
                               LEGAL = 14;
        }
    }
}
