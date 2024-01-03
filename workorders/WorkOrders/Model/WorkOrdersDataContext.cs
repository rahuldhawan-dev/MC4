using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;
using MMSINC.Interface;

namespace WorkOrders.Model
{
    [Database(Name = "WorkOrdersTest")]
    public class WorkOrdersDataContext : DataContext, IDataContext
    {
        #region Extensibility Methods

        protected void OnCreated()
        {
        }

        #endregion

        // ReSharper disable UnusedPrivateMember
        #pragma warning disable 168

        #region AssetType

        private void InsertAssetType(AssetType instance)
        {
            throw new DomainLogicException(
                "Cannot create new AssetType objects in this context.");
        }

        private void UpdateAssetType(AssetType instance)
        {
            throw new DomainLogicException(
                "Cannot alter AssetType objects in this context.");
        }

        private void DeleteAssetType(AssetType instance)
        {
            throw new DomainLogicException(
                "Cannot delete AssetType objects in this context.");
        }

        #endregion

        #region Employee

        private void InsertEmployee(Employee instance)
        {
            throw new DomainLogicException(
                "Cannot create new Employee objects in this context.");
        }

        private void UpdateEmployee(Employee instance)
        {
            throw new DomainLogicException(
                "Cannot alter Employee objects in this context.");
        }

        private void DeleteEmployee(Employee instance)
        {
            throw new DomainLogicException(
                "Cannot delete Employee objects in this context.");
        }

        #endregion

        #region Hydrant

        //private void UpdateHydrant(Hydrant instance)
        //{
        //    throw new DomainLogicException("Cannot alter Hydrant objects within this context.");
        //}

        private void InsertHydrant(Hydrant instance)
        {
            throw new DomainLogicException("Cannot create new Hydrant objects within this context.");
        }

        private void DeleteHydrant(Hydrant instance)
        {
            throw new DomainLogicException("Cannot delete Hydrant objects within this context.");
        }

        #endregion

        #region LeakReporingSource

        private void InsertLeakReportingSource(LeakReportingSource instance)
        {
            throw new DomainLogicException(
                "Cannot create new LeakReporingSource objects within this context.");
        }

        private void UpdateLeakReportingSource(LeakReportingSource instance)
        {
            throw new DomainLogicException(
                "Cannot alter LeakReporingSource objects within this context.");
        }

        private void DeleteLeakReportingSource(LeakReportingSource instance)
        {
            throw new DomainLogicException(
                "Cannot delete LeakReportingSource objects within this context.");
        }

        #endregion

        #region MainCondition

        private void InsertMainCondition(MainCondition instance)
        {
            throw new DomainLogicException(
                "Cannot create new MainCondition objects within this context.");
        }

        private void UpdateMainCondition(MainCondition instance)
        {
            throw new DomainLogicException(
                "Cannot alter MainCondition objects within this context.");
        }

        private void DeleteMainCondition(MainCondition instance)
        {
            throw new DomainLogicException(
                "Cannot delete MainCondition objects within this context.");
        }

        #endregion

        #region MarkoutRequirement

        private void InsertMarkoutRequirement(MarkoutRequirement instance)
        {
            throw new DomainLogicException(
                "Cannot create new MarkoutRequirement objects in this context.");
        }

        private void UpdateMarkoutRequirement(MarkoutRequirement instance)
        {
            throw new DomainLogicException(
                "Cannot alter MarkoutRequirement objects in this context.");
        }

        private void DeleteMarkoutRequirement(MarkoutRequirement instance)
        {
            throw new DomainLogicException(
                "Cannot delete MarkoutRequirement objects in this context.");
        }

        #endregion

        #region MarkoutStatus

        private void InsertMarkoutStatus(MarkoutStatus instance)
        {
            throw new DomainLogicException(
                "Cannot create new MarkoutStatus objects within this context.");
        }

        private void UpdateMarkoutStatus(MarkoutStatus instance)
        {
            throw new DomainLogicException(
                "Cannot alter MarkoutStatus objects within this context.");
        }

        private void DeleteMarkoutStatus(MarkoutStatus instance)
        {
            throw new DomainLogicException(
                "Cannot delete MarkoutStatus objects within this context.");
        }

        #endregion

        #region OperatingCenter

        private void InsertOperatingCenter(OperatingCenter instance)
        {
            throw new DomainLogicException("Cannot create new OperatingCenter objects in this context.");
        }

        private void UpdateOperatingCenter(OperatingCenter instance)
        {
            throw new DomainLogicException("Cannot alter OperatingCenter objects in this context.");
        }

        private void DeleteOperatingCenter(OperatingCenter instance)
        {
            throw new DomainLogicException("Cannot delete OperatingCenter objects in this context.");
        }

        #endregion

        #region Street

        private void InsertStreet(Street instance)
        {
            throw new DomainLogicException(
                "Cannot create new Street objects within this context.");
        }

        private void UpdateStreet(Street instance)
        {
            throw new DomainLogicException(
                "Cannot alter Street objects within this context.");
        }

        private void DeleteStreet(Street instance)
        {
            throw new DomainLogicException(
                "Cannot delete Street objects within this context.");
        }

        #endregion

        #region Town

        private void InsertTown(Town instance)
        {
            throw new DomainLogicException(
                "Cannot create new Town objects within this context.");
        }

        private void UpdateTown(Town instance)
        {
            throw new DomainLogicException("Cannot alter Town objects within this context.");
        }

        private void DeleteTown(Town instance)
        {
            throw new DomainLogicException("Cannot delete Town objects within this context.");
        }

        #endregion

        #region TownSection

        private void InsertTownSection(TownSection instance)
        {
            throw new DomainLogicException(
                "Cannot create new TownSection objects within this context.");
        }

        private void UpdateTownSection(TownSection instance)
        {
            throw new DomainLogicException(
                "Cannot alter TownSection objects within this context.");
        }

        private void DeleteTownSection(TownSection instance)
        {
            throw new DomainLogicException(
                "Cannot delete TownSection objects within this context.");
        }

        #endregion

        #region WorkOrderPriority

        private void InsertWorkOrderPriority(WorkOrderPriority instance)
        {
            throw new DomainLogicException(
                "Cannot create new WorkOrderPriority objects in this context.");
        }

        private void UpdateWorkOrderPriority(WorkOrderPriority instance)
        {
            throw new DomainLogicException(
                "Cannot alter WorkOrderPriority objects in this context.");
        }

        private void DeleteWorkOrderPriority(WorkOrderPriority instance)
        {
            throw new DomainLogicException(
                "Cannot delete WorkOrderPriority objects in this context.");
        }

        #endregion

        #region WorkOrderPurpose

        private void InsertWorkOrderPurpose(WorkOrderPurpose instance)
        {
            throw new DomainLogicException(
                "Cannot create new WorkOrderPurpose objects in this context.");
        }

        private void UpdateWorkOrderPurpose(WorkOrderPurpose instance)
        {
            throw new DomainLogicException(
                "Cannot alter WorkOrderPurpose objects in this context.");
        }

        private void DeleteWorkOrderPurpose(WorkOrderPurpose instance)
        {
            throw new DomainLogicException(
                "Cannot delete WorkOrderPurpose objects in this context.");
        }

        #endregion

        #region WorkOrderRequester

        private void InsertWorkOrderRequester(WorkOrderRequester instance)
        {
            throw new DomainLogicException(
                "Cannot create new WorkOrderRequester objects within this context.");
        }

        private void UpdateWorkOrderRequester(WorkOrderRequester instance)
        {
            throw new DomainLogicException(
                "Cannot alter WorkOrderRequester objects within this context.");
        }

        private void DeleteWorkOrderRequester(WorkOrderRequester instance)
        {
            throw new DomainLogicException(
                "Cannot delete WorkOrderRequester objects within this context.");
        }

        #endregion

        #region Valve

        private void InsertValve(Valve instance)
        {
            throw new DomainLogicException(
                "Cannot create new Valve objects withinin this context.");
        }

        //private void UpdateValve(Valve instance)
        //{
        //    throw new DomainLogicException(
        //        "Cannot alter Valve objects within this context.");
        //}

        private void DeleteValve(Valve instance)
        {
            throw new DomainLogicException(
                "Cannot delete Valve objects within this context.");
        }

        #endregion

        #pragma warning restore 168
        // ReSharper restore UnusedPrivateMember

        #region Private Static Members

        private static readonly MappingSource mappingSource = new AttributeMappingSource();

        #endregion

        #region Properties

        public Table<AssetType> AssetTypes
        {
            get { return GetTable<AssetType>(); }
        }

        public Table<Coordinate> Coordinates
        {
            get { return GetTable<Coordinate>(); }
        }

        public Table<CrewAssignment> CrewAssignments
        {
            get { return GetTable<CrewAssignment>(); }
        }

        public Table<Crew> Crews
        {
            get { return GetTable<Crew>(); }
        }

        public Table<CustomerImpactRange> CustomerImpactRanges
        {
            get { return GetTable<CustomerImpactRange>(); }
        }

        public Table<DataType> DataTypes
        {
            get { return GetTable<DataType>(); }
        }

        public Table<DetectedLeak> DetectedLeaks
        {
            get { return GetTable<DetectedLeak>(); }
        }

        public Table<Document> Documents
        {
            get { return GetTable<Document>(); }
        }

        public Table<DocumentType> DocumentTypes
        {
            get { return GetTable<DocumentType>(); }
        }

        public Table<DocumentWorkOrder> DocumentsWorkOrders
        {
            get { return GetTable<DocumentWorkOrder>(); }
        }

        public Table<Employee> Employees
        {
            get { return GetTable<Employee>(); }
        }

        public Table<EmployeeWorkOrder> EmployeeWorkOrders
        {
            get { return GetTable<EmployeeWorkOrder>(); }
        }

        public Table<Hydrant> Hydrants
        {
            get { return GetTable<Hydrant>(); }
        }

        public Table<LeakReportingSource> LeakReportingSources
        {
            get { return GetTable<LeakReportingSource>(); }
        }

        public Table<LostWater> LostWaters
        {
            get { return GetTable<LostWater>(); }
        }

        public Table<MainBreak> MainBreaks
        {
            get { return GetTable<MainBreak>(); }
        }

        public Table<MainBreakValveOperation> MainBreakValveOperations
        {
            get { return GetTable<MainBreakValveOperation>(); }
        }

        public Table<MainCondition> MainConditions
        {
            get { return GetTable<MainCondition>(); }
        }

        public Table<MainFailureType> MainFailureTypes
        {
            get { return GetTable<MainFailureType>(); }
        }

        public Table<MarkoutRequirement> MarkoutRequirements
        {
            get { return GetTable<MarkoutRequirement>(); }
        }

        public Table<Markout> Markouts
        {
            get { return GetTable<Markout>(); }
        }

        public Table<MarkoutStatus> MarkoutStatuses
        {
            get { return GetTable<MarkoutStatus>(); }
        }

        public Table<Material> Materials
        {
            get { return GetTable<Material>(); }
        }

        public Table<MaterialsUsed> MaterialsUseds
        {
            get { return GetTable<MaterialsUsed>(); }
        }

        public Table<OperatingCenter> OperatingCenters
        {
            get { return GetTable<OperatingCenter>(); }
        }

        public Table<OperatingCenterSpoilRemovalCost> OperatingCenterSpoilTypes
        {
            get { return GetTable<OperatingCenterSpoilRemovalCost>(); }
        }

        public Table<OperatingCenterStockedMaterial>
            OperatingCenterStockedMaterials
        {
            get { return GetTable<OperatingCenterStockedMaterial>(); }
        }

        public Table<RepairTimeRange> RepairTimeRanges
        {
            get { return GetTable<RepairTimeRange>(); }
        }

        public Table<ReportViewing> ReportViewings
        {
            get { return GetTable<ReportViewing>(); }
        }

        public Table<RestorationAccountingCode> RestorationAccountingCodes
        {
            get { return GetTable<RestorationAccountingCode>(); }
        }

        public Table<RestorationMethod> RestorationMethods
        {
            get { return GetTable<RestorationMethod>(); }
        }

        public Table<RestorationMethodRestorationType> RestorationMethodsRestorationTypes
        {
            get { return GetTable<RestorationMethodRestorationType>(); }
        }

        public Table<RestorationProductCode> RestorationProductCodes
        {
            get { return GetTable<RestorationProductCode>(); }
        }

        public Table<Restoration> Restorations
        {
            get { return GetTable<Restoration>(); }
        }

        public Table<RestorationResponsePriority> RestorationResponsePriorities
        {
            get { return GetTable<RestorationResponsePriority>(); }
        }

        public Table<RestorationTypeCost> RestorationTypeCosts
        {
            get { return GetTable<RestorationTypeCost>(); }
        }

        public Table<RestorationType> RestorationTypes
        {
            get { return GetTable<RestorationType>(); }
        }

        public Table<SafetyMarker> SafetyMarkers
        {
            get { return GetTable<SafetyMarker>(); }
        }

        public Table<SpoilRemoval> SpoilRemovals
        {
            get { return GetTable<SpoilRemoval>(); }
        }

        public Table<Spoil> Spoils
        {
            get { return GetTable<Spoil>(); }
        }

        public Table<SpoilStorageLocation> SpoilStorageLocations
        {
            get { return GetTable<SpoilStorageLocation>(); }
        }

        public Table<SpoilFinalProcessingLocation> SpoilFinalProcessingLocations
        {
            get { return GetTable<SpoilFinalProcessingLocation>(); }
        }

        public Table<StockLocation> StockLocations
        {
            get { return GetTable<StockLocation>(); }
        }

        public Table<Street> Streets
        {
            get { return GetTable<Street>(); }
        }

        public Table<StreetOpeningPermit> StreetOpeningPermits
        {
            get { return GetTable<StreetOpeningPermit>(); }
        }

        public Table<Town> Towns
        {
            get { return GetTable<Town>(); }
        }

        public Table<TownSection> TownSections
        {
            get { return GetTable<TownSection>(); }
        }

        public Table<Valve> Valves
        {
            get { return GetTable<Valve>(); }
        }

        public Table<WorkAreaType> WorkAreaTypes
        {
            get { return GetTable<WorkAreaType>(); }
        }

        public Table<WorkOrderDescriptionChange> WorkOrderDescriptionChanges
        {
            get { return GetTable<WorkOrderDescriptionChange>(); }
        }

        public Table<WorkDescription> WorkDescriptions
        {
            get { return GetTable<WorkDescription>(); }
        }

        public Table<WorkOrderPriority> WorkOrderPriorities
        {
            get { return GetTable<WorkOrderPriority>(); }
        }

        public Table<WorkOrderPurpose> WorkOrderPurposes
        {
            get { return GetTable<WorkOrderPurpose>(); }
        }

        public Table<WorkOrderRequester> WorkOrderRequesters
        {
            get { return GetTable<WorkOrderRequester>(); }
        }

        public Table<WorkCategory> WorkCategories
        {
            get { return GetTable<WorkCategory>(); }
        }

        public Table<WorkOrder> WorkOrders
        {
            get { return GetTable<WorkOrder>(); }
        }

        public Table<OrcomOrderCompletion> OrcomOrderCompletions
        {
            get { return GetTable<OrcomOrderCompletion>(); }
        }

        public Table<ServiceSize> ServiceSizes
        {
            get { return GetTable<ServiceSize>(); }
        }

        public Table<MarkoutType> MarkoutTypes
        {
            get { return GetTable<MarkoutType>(); }
        }

        public static string ConnectionString
        {
            get
            {
#if DEBUG
                try
                {
#endif
                    return ConfigurationManager.ConnectionStrings["MCProd"]
                                               .ConnectionString;
#if DEBUG
                }
                catch (Exception e)
                {
                    var available = new List<string>();

                    foreach (ConnectionStringSettings connection in
                        ConfigurationManager.ConnectionStrings)
                    {
                        available.Add(connection.Name);
                    }

                    throw new Exception($"Could not find connection string named 'MCProd' in settings file '{AppDomain.CurrentDomain.SetupInformation.ConfigurationFile}'.  Available strings: '{string.Join("', '", available)}'", e);
                }
#endif
            }
        }

        #endregion

        #region Constructors

        public WorkOrdersDataContext() :
            base(ConnectionString, mappingSource)
        {
            OnCreated();
        }

        public WorkOrdersDataContext(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public WorkOrdersDataContext(IDbConnection connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public WorkOrdersDataContext(string connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public WorkOrdersDataContext(IDbConnection connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        #endregion
    }
}
