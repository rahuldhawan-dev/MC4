using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Permissions;
using MapCall.Common.Web;
using MapCall.SAP.Model.Repositories;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities.Pdf;
using MMSINC.Utilities.Permissions;
using StructureMap;
using StructureMap.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Web;
using WorkOrders.Model;
using WorkOrders.Presenters.Assets;
using WorkOrders.Presenters.CrewAssignments;
using WorkOrders.Presenters.Crews;
using WorkOrders.Presenters.Documents;
using WorkOrders.Presenters.Materials;
using WorkOrders.Presenters.OperatingCenterSpoilRemovalCosts;
using WorkOrders.Presenters.RestorationAccountingCodes;
using WorkOrders.Presenters.RestorationProductCodes;
using WorkOrders.Presenters.Restorations;
using WorkOrders.Presenters.RestorationTypeCosts;
using WorkOrders.Presenters.SpoilFinalProcessingLocations;
using WorkOrders.Presenters.SpoilRemovals;
using WorkOrders.Presenters.SpoilStorageLocations;
using WorkOrders.Presenters.StockLocations;
using WorkOrders.Presenters.WorkOrders;
using CrewAssignmentRepository = WorkOrders.Model.CrewAssignmentRepository;
using CrewRepository = WorkOrders.Model.CrewRepository;
using DocumentDataRepository = WorkOrders.Model.DocumentDataRepository;
using DocumentRepository = WorkOrders.Model.DocumentRepository;
using EmployeeRepository = WorkOrders.Model.EmployeeRepository;
using EquipmentRepository = WorkOrders.Model.EquipmentRepository;
using HydrantRepository = WorkOrders.Model.HydrantRepository;
using IDocumentDataRepository = WorkOrders.Model.IDocumentDataRepository;
using IDocumentRepository = WorkOrders.Model.IDocumentRepository;
using IEmployeeRepository = WorkOrders.Model.IEmployeeRepository;
using IOperatingCenterRepository = WorkOrders.Model.IOperatingCenterRepository;
using MainBreakRepository = WorkOrders.Model.MainBreakRepository;
using MaterialRepository = WorkOrders.Model.MaterialRepository;
using OperatingCenterRepository = WorkOrders.Model.OperatingCenterRepository;
using RestorationRepository = WorkOrders.Model.RestorationRepository;
using SewerOpeningRepository = WorkOrders.Model.SewerOpeningRepository;
using ValveRepository = WorkOrders.Model.ValveRepository;
using WorkOrderRepository = WorkOrders.Model.WorkOrderRepository;
using WorkOrderSpoilRemovalRepository = WorkOrders.Model.SpoilRemovalRepository;

namespace LINQTo271
{
    public class Global : MapCallHttpApplication
    {
        #region Constants

        public static readonly Regex DEV_MACHINE_REGEX = new Regex("^(njs-ws-tc(?:\\d+)-vm|njs-ls-\\w{4}|EC2AMAZ-\\w+|windows-\\w+)$",
            RegexOptions.Compiled | 
            RegexOptions.IgnoreCase |
            RegexOptions.Singleline);

        #endregion

        #region Private Methods

        public override IResourceConfiguration GetResourceConfiguration()
        {
            return new ResourceConfiguration {
                Site = MapCall.Common.Utility.Site.WorkOrders
            };
        }

        public override void RegisterDependencies(ConfigurationExpression i)
        {
            RegisterPresenters(i);
            RegisterModel(i);
            RegisterDataContext(i);
            RegisterUtilities(i);
        }

        private void RegisterUtilities(ConfigurationExpression i)
        {
            // IF there's any weird issues with roles, remove this line and put
            // back the uncommented out line. This is how MapCall proper uses
            // the RoleManager. RoleManager is designed to be re-used for the life
            // of the request.
            i.For<IRoleManager>().HttpContextScoped().Use<RoleManager>();
           // i.For<IRoleManager>().Use<RoleManager>();
           i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();
           i.For<IImageToPdfConverter>().Use<ImageToPdfConverter>();
        }

        private void RegisterDataContext(ConfigurationExpression i)
        {
            i.For<IDataContext>().Use(() => new WorkOrdersDataContext());
        }

        private void RegisterModel(ConfigurationExpression i)
        {
            i.RegisterRepository<WorkOrder, WorkOrderRepository>();
            i
                .RegisterRepository
                <AssetType, global::WorkOrders.Model.AssetTypeRepository>();
            i.RegisterRepository<Valve, ValveRepository>();
            i.RegisterRepository<Hydrant, HydrantRepository>();
            i.RegisterRepository<SewerOpening, SewerOpeningRepository>();
            i.RegisterRepository<StormCatch, StormCatchRepository>();
            i.RegisterRepository<Equipment, EquipmentRepository>();
            i.RegisterRepository<Crew, CrewRepository>();
            i.RegisterRepository<CrewAssignment, CrewAssignmentRepository>();
            i.RegisterRepository<Restoration, RestorationRepository>();
            i.RegisterRepository
                <RestorationTypeCost, RestorationTypeCostRepository>();
            i.RegisterRepository
                <OperatingCenterSpoilRemovalCost, OperatingCenterSpoilRemovalCostRepository>();
            i.RegisterRepository<SpoilRemoval, WorkOrderSpoilRemovalRepository>();
            i.RegisterRepository<StockLocation, global::WorkOrders.Model.StockLocationRepository>();
            i.RegisterRepository
                <SpoilStorageLocation, SpoilStorageLocationRepository>();
            i.RegisterRepository
                <SpoilFinalProcessingLocation, SpoilFinalProcessingLocationRepository>();
            i.RegisterRepository<Markout, MarkoutRepository>();
            i.For<IEmployeeRepository>().Use<EmployeeRepository>();
            i.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            i.RegisterRepository
                <WorkOrderDescriptionChange,
                    WorkOrderDescriptionChangeRepository>();

            i.RegisterRepository<OrcomOrderCompletion, OrcomOrderCompletionRepository>();

            i.For<IDocumentRepository>().Use<DocumentRepository>();
            i.For<IDocumentDataRepository>().Use<DocumentDataRepository>();

            i.For<IGISLayerUpdateRepository>().Use<GISLayerUpdateRepository>();
            i.For<IRepository<MapCall.Common.Model.Entities.SampleSite>>().Use<RepositoryBase<MapCall.Common.Model.Entities.SampleSite>>();
            i.RegisterRepository<Document, DocumentRepository>();
            i.RegisterRepository<MarkoutType, MarkoutTypeRepository>();
            i.RegisterRepository<BusinessUnit, BusinessUnitRepository>();
            i.RegisterRepository<ReportViewing, ReportViewingRepository>();
            i.RegisterRepository<MainBreak, MainBreakRepository>();
            i.RegisterRepository<OperatingCenterTown, OperatingCenterTownRepository>();
            i.RegisterRepository<OperatingCenterUser, OperatingCenterUserRepository>();
            i.RegisterRepository<MaterialsUsed, MaterialsUsedRepository>();
            i.RegisterRepository<Material, MaterialRepository>();
            i.RegisterRepository<MostRecentlyInstalledService, global::WorkOrders.Model.MostRecentlyInstalledServiceRepository>();
            i.RegisterRepository<WorkDescription, global::WorkOrders.Model.WorkDescriptionRepository>();
            i.RegisterRepository<RestorationAccountingCode, RestorationAccountingCodeRepository>();
            i.RegisterRepository<RestorationProductCode, RestorationProductCodeRepository>();
            i.RegisterRepository<StreetOpeningPermit, StreetOpeningPermitRepository>();
            i.For<IStreetOpeningPermitRepository>().Use<StreetOpeningPermitRepository>();
            i.For<ITapImageRepository>().Use<TapImageRepository>();
            i.For<IServiceRepository>().Use<ServiceRepository>();
            i.For<IWorkOrdersWorkOrderRepository>().Use<WorkOrderRepository>();
            i.For<IWorkOrderRepository>().Use<MapCall.Common.Model.Repositories.WorkOrderRepository>();
            i.For<IGeneralWorkOrderRepository>().Use<MapCall.Common.Model.Repositories.GeneralWorkOrderRepository>();
            i.For<ISAPWorkOrderRepository>().Use<SAPWorkOrderRepository>();
            i.For<ISAPNewServiceInstallationRepository>().Use<SAPNewServiceInstallationRepository>();

            i.For<IRepository<MapCall.Common.Model.Entities.AggregateRole>>().Use<RepositoryBase<MapCall.Common.Model.Entities.AggregateRole>>();

            i.For<ISAPHttpClient>().Use(() => new SAPHttpClient {
                UserName = ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUserName"),
                Password = ConfigurationManager.AppSettings.EnsureValue("SAPWebServicePassword"),
                BaseAddress = new Uri(ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUrl"))
            });
        }

        private void RegisterPresenters(ConfigurationExpression i)
        {
            i.RegisterResourcePresenter<WorkOrder, WorkOrderResourcePresenter>();
            i.RegisterDetailPresenter<WorkOrder, WorkOrderDetailPresenter>();
            i.RegisterListPresenter<WorkOrder, WorkOrderListPresenter>();
            i.RegisterSearchPresenter<WorkOrder, WorkOrderSearchPresenter>();
            i.RegisterRPCPresenter<WorkOrder, WorkOrderResourceRPCPresenter>();

            i.RegisterDetailPresenter<Asset, AssetLatLonPickerPresenter>();

            i.RegisterResourcePresenter<Crew, CrewResourcePresenter>();
            i.RegisterDetailPresenter<Crew, CrewDetailPresenter>();
            i.RegisterListPresenter<Crew, CrewsListPresenter>();

            i.RegisterRPCPresenter
                <CrewAssignment, CrewAssignmentResourceRPCPresenter>();
            i.RegisterListPresenter<CrewAssignment, CrewAssignmentsListPresenter>();
            i.For<ICrewAssignmentsReadOnlyPresenter>().Use<CrewAssignmentsReadOnlyPresenter>();

            i.RegisterRPCPresenter<Restoration, RestorationResourceRPCPresenter>();
            i.RegisterDetailPresenter<Restoration, RestorationDetailPresenter>();

            i.RegisterResourcePresenter
                <RestorationTypeCost, RestorationTypeCostResourcePresenter>();
            i.RegisterListPresenter
                <RestorationTypeCost, RestorationTypeCostListPresenter>();

            i.RegisterResourcePresenter
                <OperatingCenterSpoilRemovalCost,
                    OperatingCenterSpoilRemovalCostResourcePresenter>();
            i.RegisterListPresenter
                <OperatingCenterSpoilRemovalCost, OperatingCenterSpoilRemovalCostListPresenter>();

            i.RegisterResourcePresenter
                <SpoilRemoval, SpoilRemovalResourcePresenter>();
            i.RegisterListPresenter<SpoilRemoval, SpoilRemovalListPresenter>();
            i.RegisterSearchPresenter<SpoilRemoval, SpoilRemovalSearchPresenter>();

            i.RegisterResourcePresenter
                <StockLocation, StockLocationResourcePresenter>();
            i.RegisterListPresenter<StockLocation, StockLocationListPresenter>();
            i.RegisterSearchPresenter<StockLocation, StockLocationSearchPresenter>();

            i.RegisterResourcePresenter
                <SpoilStorageLocation, SpoilStorageLocationResourcePresenter>();
            i.RegisterListPresenter<SpoilStorageLocation, SpoilStorageLocationListPresenter>();
            i.RegisterSearchPresenter<SpoilStorageLocation, SpoilStorageLocationSearchPresenter>();

            i.RegisterResourcePresenter
                <SpoilFinalProcessingLocation, SpoilFinalProcessingLocationResourcePresenter>();
            i.RegisterListPresenter<SpoilFinalProcessingLocation, SpoilFinalProcessingLocationListPresenter>();
            i.RegisterSearchPresenter<SpoilFinalProcessingLocation, SpoilFinalProcessingLocationSearchPresenter>();

            i.RegisterResourcePresenter<CrewAssignment, CrewAssignmentResourcePresenter>();
            i.RegisterSearchPresenter<CrewAssignment, CrewAssignmentSearchPresenter>();
            i.For<ICrewAssignmentResourceViewPresenter>().Use<CrewAssignmentResourcePresenter>();

            i.RegisterRPCPresenter<Document, DocumentResourceRPCPresenter>();
            i.RegisterDetailPresenter<Document, DocumentDetailPresenter>();

            i.RegisterResourcePresenter<Material, MaterialResourcePresenter>();
            i.RegisterListPresenter<Material, MaterialListPresenter>();
            i.RegisterSearchPresenter<Material, MaterialSearchPresenter>();

            i.RegisterResourcePresenter<RestorationProductCode, RestorationProductCodeResourcePresenter>();
            i.RegisterSearchPresenter<RestorationProductCode, RestorationProductCodeSearchPresenter>();
            i.RegisterListPresenter<RestorationProductCode, RestorationProductCodeListPresenter>();

            i.RegisterResourcePresenter<RestorationAccountingCode, RestorationAccountingCodeResourcePresenter>();
            i.RegisterSearchPresenter<RestorationAccountingCode, RestorationAccountingCodeSearchPresenter>();
            i.RegisterListPresenter<RestorationAccountingCode, RestorationAccountingCodeListPresenter>();
        }

        public override void LogOutUser(bool redirectToLoginPage = true, bool includeReturnUrl = true)
        {
            base.LogOutUser(redirectToLoginPage, includeReturnUrl);

            if (redirectToLoginPage)
            {
                // Note the lack of an ~ in the redirect.
                HttpContext.Current.Response.Redirect("/login.aspx");
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #if DEBUG
            if (!Server.IsDevMachine())
            {
                throw new ServerException(
                    "This application cannot be run in DEBUG mode in the live or staging environment.  " +
                    "Please re-compile the project without debugging symbols, and re-deploy.");
            }
            #endif
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // clean up after StructureMap to prevent OOM errors
            //_container.ReleaseAndDisposeAllHttpScopedObjects();

            var disposableKeys = new List<object>();
            foreach (var key in HttpContext.Current.Items.Keys)
            {
                var disposable = HttpContext.Current.Items[key] as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                    disposableKeys.Add(key);
                }
            }

            disposableKeys.ForEach(key => HttpContext.Current.Items[key] = null);
        }

        #endregion
    }

    public static class ConfigurationExpressionExtensions
    {
        public static ConfigurationExpression RegisterResourcePresenter<TEntity, TPresenter>(this ConfigurationExpression i)
            where TEntity : class
            where TPresenter : IResourcePresenter<TEntity>
        {
            i.For<IResourcePresenter<TEntity>>().Use<TPresenter>();
            return i;
        }

        public static ConfigurationExpression RegisterDetailPresenter<TEntity, TPresenter>(this ConfigurationExpression i)
            where TEntity : class
            where TPresenter : IDetailPresenter<TEntity>
        {
            i.For<IDetailPresenter<TEntity>>().Use<TPresenter>();
            return i;
        }

        public static ConfigurationExpression RegisterListPresenter<TEntity, TPresenter>(this ConfigurationExpression i)
            where TEntity : class
            where TPresenter : IListPresenter<TEntity>
        {
            i.For<IListPresenter<TEntity>>().Use<TPresenter>();
            return i;
        }

        public static ConfigurationExpression RegisterSearchPresenter<TEntity, TPresenter>(this ConfigurationExpression i)
            where TEntity : class
            where TPresenter : ISearchPresenter<TEntity>
        {
            i.For<ISearchPresenter<TEntity>>().Use<TPresenter>();
            return i;
        }

        public static ConfigurationExpression RegisterRPCPresenter<TEntity, TPresenter>(this ConfigurationExpression i)
            where TEntity : class
            where TPresenter : IResourceRPCPresenter<TEntity>
        {
            i.For<IResourceRPCPresenter<TEntity>>().Use<TPresenter>();
            return i;
        }

        public static ConfigurationExpression RegisterRepository<TEntity, TPresenter>(this ConfigurationExpression i)
            where TEntity : class
            where TPresenter : MMSINC.Data.Linq.IRepository<TEntity>
        {
            i.For<MMSINC.Data.Linq.IRepository<TEntity>>().Use<TPresenter>();
            return i;
        }
    }

    internal static class ServerExtensions
    {
        #region Extension Methods

        public static bool IsDevMachine(this HttpServerUtility server)
        {
            return Global.DEV_MACHINE_REGEX.IsMatch(server.MachineName);
        }

        #endregion
    }
}
