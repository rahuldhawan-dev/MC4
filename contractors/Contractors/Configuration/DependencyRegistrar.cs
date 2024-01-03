using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using Contractors.Data.Library;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Configuration;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using MMSINC.Utilities.Permissions;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;
using StructureMap.Web;
using AsBuiltImageRepository = Contractors.Data.Models.Repositories.AsBuiltImageRepository;
using AssetTypeRepository = Contractors.Data.Models.Repositories.AssetTypeRepository;
using AuthenticationRepository = Contractors.Data.Models.Repositories.AuthenticationRepository;
using ContractorUserRepository = Contractors.Data.Models.Repositories.ContractorUserRepository;
using CountyRepository = Contractors.Data.Models.Repositories.CountyRepository;
using CrewRepository = Contractors.Data.Models.Repositories.CrewRepository;
using DocumentRepository = Contractors.Data.Models.Repositories.DocumentRepository;
using IAsBuiltImageRepository = Contractors.Data.Models.Repositories.IAsBuiltImageRepository;
using IAssetTypeRepository = Contractors.Data.Models.Repositories.IAssetTypeRepository;
using IContractorUserRepository = Contractors.Data.Models.Repositories.IContractorUserRepository;
using ICountyRepository = Contractors.Data.Models.Repositories.ICountyRepository;
using ICrewAssignmentRepository = Contractors.Data.Models.Repositories.ICrewAssignmentRepository;
using IDocumentRepository = Contractors.Data.Models.Repositories.IDocumentRepository;
using IMaterialRepository = Contractors.Data.Models.Repositories.IMaterialRepository;
using IMeterChangeOutRepository = Contractors.Data.Models.Repositories.IMeterChangeOutRepository;
using IMeterChangeOutStatusRepository = Contractors.Data.Models.Repositories.IMeterChangeOutStatusRepository;
using IOperatingCenterRepository = Contractors.Data.Models.Repositories.IOperatingCenterRepository;
using IRestorationMethodRepository = Contractors.Data.Models.Repositories.IRestorationMethodRepository;
using IServiceRepository = Contractors.Data.Models.Repositories.IServiceRepository;
using IStreetRepository = Contractors.Data.Models.Repositories.IStreetRepository;
using ITapImageRepository = Contractors.Data.Models.Repositories.ITapImageRepository;
using ITownRepository = Contractors.Data.Models.Repositories.ITownRepository;
using ITownSectionRepository = Contractors.Data.Models.Repositories.ITownSectionRepository;
using IValveImageRepository = Contractors.Data.Models.Repositories.IValveImageRepository;
using IWorkDescriptionRepository = Contractors.Data.Models.Repositories.IWorkDescriptionRepository;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using MaterialRepository = Contractors.Data.Models.Repositories.MaterialRepository;
using MeterChangeOutRepository = Contractors.Data.Models.Repositories.MeterChangeOutRepository;
using MeterChangeOutStatusRepository = Contractors.Data.Models.Repositories.MeterChangeOutStatusRepository;
using OperatingCenterRepository = Contractors.Data.Models.Repositories.OperatingCenterRepository;
using RestorationMethodRepository = Contractors.Data.Models.Repositories.RestorationMethodRepository;
using ServiceRepository = Contractors.Data.Models.Repositories.ServiceRepository;
using StreetRepository = Contractors.Data.Models.Repositories.StreetRepository;
using TapImageRepository = Contractors.Data.Models.Repositories.TapImageRepository;
using TownRepository = Contractors.Data.Models.Repositories.TownRepository;
using TownSectionRepository = Contractors.Data.Models.Repositories.TownSectionRepository;
using ValveImageRepository = Contractors.Data.Models.Repositories.ValveImageRepository;
using WorkDescriptionRepository = Contractors.Data.Models.Repositories.WorkDescriptionRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;
using CrewAssignmentRepository = Contractors.Data.Models.Repositories.CrewAssignmentRepository;
using MapCall.Common.Configuration;
using NHibernate.Event;
using MapCall.Common.Utility.Notifications;
using RazorEngine.Templating;
using MMSINC.Common;
using log4net;
using RestorationRepository = Contractors.Data.Models.Repositories.RestorationRepository;

namespace Contractors.Configuration
{
    public class DependencyRegistrar : DependencyRegistrar<ContractorUser, ContractorUser>
    {
        #region Type Registration

        protected override void RegisterUtilities(ConfigurationExpression i)
        {
            i.For<IViewModelFactory>().Use<ViewModelFactory>();
            i.For<IPermissionsObjectFactory>().Use<PermissionsObjectFactory>();
            i.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
            i.For<IAuthenticationService<ContractorUser>>()
             .HybridHttpOrThreadLocalScoped()
             .Use<AuthenticationService>();
            // they don't all need to be scoped like that
            i.For<IAuthenticationService<IAdministratedUser>>().Use(ctx =>
                ctx.GetInstance<IAuthenticationService<ContractorUser>>());
            i.For<IAuthenticationService>()
                .Use(ctx => ctx.GetInstance<IAuthenticationService<ContractorUser>>());

            i.For<IDateTimeProvider>()
                .Use(() => new DateTimeProvider());

            i.For<IDocumentService>()
                .Use(() => new DocumentService());
            i.For<HttpContextBase>().Use(ctx => new System.Web.HttpContextWrapper(HttpContext.Current));

            i.For<IHtmlToPdfConverter>().Use<HtmlToPdfConverter>();

            i.For<ISecureFormTokenService>()
             .Use<SecureFormTokenService<ContractorsSecureFormToken,
                  ContractorsSecureFormDynamicValue>>();

            i.For<IDisplayItemService>().Use<DisplayItemService>();
            i.For<ISmtpClientFactory>().Use<SmtpClientFactory>();
            i.For<IMailMessageFactory>().Use<MailMessageFactory>();
            i.For<INotificationService>().Use<NotificationService>();
            i.For<ITemplateService>().Use(_ => new TemplateService()).Singleton();
            i.For<INotifier>().Use<RazorNotifier>();
        }

        protected override void RegisterModels(ConfigurationExpression i)
        {
            ContractorsDependencies.RegisterRepositories(i);
        }

        protected override Action<NHibernate.Cfg.Configuration> ConfigureSessionFactory(IContext container)
        {
            return cfg => {
                base.ConfigureSessionFactory(container)(cfg);
                cfg.EventListeners.FlushEventListeners = new IFlushEventListener[]
                    {new FixedDefaultFlushEventListener()};
                cfg.SetListener(ListenerType.PostInsert, container.GetInstance<AuditInsertListener>());
                cfg.SetListener(ListenerType.PostUpdate, container.GetInstance<AuditUpdateListener>());
                cfg.SetListener(ListenerType.PostDelete, container.GetInstance<AuditDeleteListener>());
            };
        }

        #endregion
    }

    public static class ContractorsDependencies
    {
        public static void RegisterRepositories(ConfigurationExpression i)
        {
            i.For<IForgotPasswordRepository<ContractorUser>>().Use<AuthenticationRepository>();
            i.For<IAuthenticationRepository<ContractorUser>>().Use<AuthenticationRepository>();
            i.For<IAuthenticationLogRepository<ContractorsAuthenticationLog, ContractorUser>>()
             .Use<AuthenticationLogRepository>();

            i
               .RegisterRepository<IAssetTypeRepository, AssetTypeRepository, AssetType>()
               .RegisterRepository<IAsBuiltImageRepository, AsBuiltImageRepository, AsBuiltImage>()
               .RegisterRepository<IAuditLogEntryRepository, AuditLogEntryRepository, AuditLogEntry>()
               .RegisterRepository<Contractor>()
               .RegisterRepository<IContractorMeterCrewRepository, ContractorMeterCrewRepository, ContractorMeterCrew>()
               .RegisterRepository<IContractorUserRepository, ContractorUserRepository, ContractorUser>()
               .RegisterRepository<ICountyRepository, CountyRepository, County>()
               .RegisterRepository<ICrewAssignmentRepository, CrewAssignmentRepository, CrewAssignment>()
               .RegisterRepository<CustomerMeterLocation>()
               .RegisterRepository<CustomerSideSLReplacer>()
               .RegisterRepository<CustomerSideSLReplacementOfferStatus>()
               .RegisterRepository<IRepository<Crew>, CrewRepository, Crew>()
               .RegisterRepository<IDocumentRepository, DocumentRepository, Document>()
               .RegisterRepository<IDataTypeRepository, DataTypeRepository, DataType>()
               .RegisterRepository<IDocumentTypeRepository, DocumentTypeRepository, DocumentType>()
               .RegisterRepository<IDocumentDataRepository, Data.Models.Repositories.DocumentDataRepository,
                    DocumentData>()
               .RegisterRepository<DocumentLink>()
               .RegisterRepository<Employee>()
               .RegisterRepository<FlushingOfCustomerPlumbingInstructions>()
               .RegisterRepository<MapIcon>()
               .RegisterRepository<MainBreak>()
               .RegisterRepository<IRepository<Markout>, MarkoutRepository, Markout>()
               .RegisterRepository<MainBreakDisinfectionMethod>()
               .RegisterRepository<MainBreakFlushMethod>()
               .RegisterRepository<MainBreakMaterial>()
               .RegisterRepository<MainBreakSoilCondition>()
               .RegisterRepository<MainCondition>()
               .RegisterRepository<MainFailureType>()
               .RegisterRepository<MainType>()
               .RegisterRepository<MarkoutRequirement>()
               .RegisterRepository<MarkoutType>()
               .RegisterRepository<IMaterialRepository, MaterialRepository, Material>()
               .RegisterRepository<MaterialUsed>()
               .RegisterRepository<MeterChangeOutContract>()
               .RegisterRepository<MeterChangeOutStatus>()
               .RegisterRepository<MeterChangeOutWorkScope>()
               .RegisterRepository<MeterDirection>()
               .RegisterRepository<MeterScheduleTime>()
               .RegisterRepository<MeterSupplementalLocation>()
               .RegisterRepository<IMeterChangeOutRepository, MeterChangeOutRepository, MeterChangeOut>()
               .RegisterRepository<IMeterChangeOutStatusRepository, MeterChangeOutStatusRepository,
                    MeterChangeOutStatus>()
               .RegisterRepository<MiuInstallReasonCode>()
               .RegisterRepository<Note>()
               .RegisterRepository<IOperatingCenterRepository, OperatingCenterRepository, OperatingCenter>()
               .RegisterRepository<IRepository<Restoration>, RestorationRepository, Restoration>()
               .RegisterRepository<RestorationMethod>()
               .RegisterRepository<RestorationType>()
               .RegisterRepository<IRestorationMethodRepository, RestorationMethodRepository, RestorationMethod>()
               .RegisterRepository<RestorationResponsePriority>()
               .RegisterRepository<RestorationPriorityUpchargeType>()
               .RegisterRepository<ITokenRepository<ContractorsSecureFormToken, ContractorsSecureFormDynamicValue>,
                    ContractorsSecureFormTokenRepository, ContractorsSecureFormToken>()
               .RegisterRepository<IServiceInstallationRepository, ServiceInstallationRepository, ServiceInstallation>()
               .RegisterRepository<SAPWorkOrderStep>()
               .RegisterRepository<SAPWorkOrderPurpose>()
               .RegisterRepository<IServiceRepository, ServiceRepository, Service>()
               .RegisterRepository<ServiceCategory>()
               .RegisterRepository<ServiceInstallationPosition>()
               .RegisterRepository<ServiceInstallationReadType>()
               .RegisterRepository<ServiceInstallationReason>()
               .RegisterRepository<ServiceInstallationFirstActivity>()
               .RegisterRepository<ServiceInstallationSecondActivity>()
               .RegisterRepository<ServiceInstallationThirdActivity>()
               .RegisterRepository<ServiceInstallationWorkType>()
               .RegisterRepository<IServiceMaterialRepository, ServiceMaterialRepository, ServiceMaterial>()
               .RegisterRepository<ServiceMaterial>()
               .RegisterRepository<ServicePriority>()
               .RegisterRepository<ServiceRestorationContractor>()
               .RegisterRepository<ServiceSideType>()
               .RegisterRepository<ServiceSize>()
               .RegisterRepository<SmallMeterLocation>()
               .RegisterRepository<State>()
               .RegisterRepository<StockLocation>()
               .RegisterRepository<ISpoilRepository, SpoilRepository, Spoil>()
               .RegisterRepository<ISpoilStorageLocationRepository, SpoilStorageLocationRepository,
                    SpoilStorageLocation>()
               .RegisterRepository<IStreetRepository, StreetRepository, Street>()
               .RegisterRepository<StreetMaterial>()
               .RegisterRepository<IRepository<StreetOpeningPermit>, StreetOpeningPermitRepository,
                    StreetOpeningPermit>()
               .RegisterRepository<ITapImageRepository, TapImageRepository, TapImage>()
               .RegisterRepository<ITownRepository, TownRepository, Town>()
               .RegisterRepository<ITownSectionRepository, TownSectionRepository, TownSection>()
               .RegisterRepository<IRepository<TypeOfPlumbing>, RepositoryBase<TypeOfPlumbing>, TypeOfPlumbing>()
               .RegisterRepository<IValveImageRepository, ValveImageRepository, ValveImage>()
               .RegisterRepository<ValveNormalPosition>()
               .RegisterRepository<ValveOpenDirection>()
               .RegisterRepository<WaterServiceStatus>()
               .RegisterRepository<IWorkDescriptionRepository, WorkDescriptionRepository, WorkDescription>()
               .RegisterRepository<WorkOrderPriority>()
               .RegisterRepository<WorkOrderPurpose>()
               .RegisterRepository<IWorkOrderRepository, WorkOrderRepository, WorkOrder>()
               .RegisterRepository<WorkOrderRequester>()
               .RegisterRepository<IRepository<WorkOrderFlushingNoticeType>, RepositoryBase<WorkOrderFlushingNoticeType>
                    , WorkOrderFlushingNoticeType>()
               .RegisterRepository<INotificationConfigurationRepository, NotificationConfigurationRepository,
                    NotificationConfiguration>()
               .RegisterRepository<NotificationPurpose>()
               .RegisterRepository<Module>()
               .RegisterRepository<IRepository<PitcherFilterCustomerDeliveryMethod>,
                    RepositoryBase<PitcherFilterCustomerDeliveryMethod>, PitcherFilterCustomerDeliveryMethod>()
               .RegisterRepository<IRepository<ConsolidatedCustomerSideMaterial>,
                    RepositoryBase<ConsolidatedCustomerSideMaterial>, ConsolidatedCustomerSideMaterial>()
               .RegisterRepository<MeterLocation>()
               .RegisterRepository<Premise>();

            i.RegisterRepository<IIconSetRepository, IconSetRepository, IconSet>();
            i.For<MapCall.Common.Model.Repositories.IServiceRepository>().Use<DummyServiceRepository>();
            i.For<MapCall.Common.Model.Repositories.ITapImageRepository>()
             .Use<MapCall.Common.Model.Repositories.TapImageRepository>();
            i.For<MMSINC.Data.NHibernate.IRepository<WorkOrder>>()
             .Use<MMSINC.Data.NHibernate.RepositoryBase<WorkOrder>>();

            i.For<IImageToPdfConverter>().Use(() => new ImageToPdfConverter());
            i.For<ISAPWorkOrderRepository>().Use<SAPWorkOrderRepository>();
            i.For<ISAPDeviceRepository>().Use<SAPDeviceRepository>();
            i.For<ISAPNewServiceInstallationRepository>().Use<SAPNewServiceInstallationRepository>();
            i.For<ISAPHttpClient>().Use(() => new SAPHttpClient
            {
                UserName = ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUserName"),
                Password = ConfigurationManager.AppSettings.EnsureValue("SAPWebServicePassword"),
                BaseAddress = new Uri(ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUrl"))
            });
            i.For<ILog>()
             .AlwaysUnique()
             .Use(ctx => LogManager.GetLogger(ctx.ParentType == null ? ctx.RequestedName : ctx.ParentType.Name));
        }

        // Why do we need dummy repositories in production? What purpose does this serve? -Ross 12/18/2019
        private class DummyRepositoryBase<TEntity> : IRepository<TEntity>
        {
            public bool Exists(int id)
            {
                throw new NotImplementedException();
            }

            public void Delete(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public TEntity Save(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public void Save(IEnumerable<TEntity> entities)
            {
                throw new NotImplementedException();
            }

            public void Update(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public TEntity Find(int id)
            {
                throw new NotImplementedException();
            }

            public int GetIdentifier(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public IQueryable<TEntity> Linq { get; }
            public ICriteria Criteria { get; }

            public int GetCountForCriterion(ICriterion criterion, IDictionary<string, string> aliases = null,
                ICriterion additionalCriterion = null)
            {
                throw new NotImplementedException();
            }

            public int GetCountForCriteria(ICriteria criteria)
            {
                throw new NotImplementedException();
            }

            public int GetCountForSearchSet<T>(ISearchSet<T> search) where T : class
            {
                throw new NotImplementedException();
            }

            public ICriteria Search(ICriterion criterion, IDictionary<string, string> aliases = null,
                ICriterion additionalCriterion = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, ICriteria criteria,
                Action<ISearchMapper> searchMapperCallback = null, int? maxResults = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, IQueryOver query,
                Action<ISearchMapper> searchMapperCallback = null)
            {
                throw new NotImplementedException();
            }

            public IQueryable<TEntity> GetAll()
            {
                throw new NotImplementedException();
            }

            public IQueryable<TEntity> GetAllSorted()
            {
                throw new NotImplementedException();
            }

            public IQueryable<TEntity> GetAllSorted(Expression<Func<TEntity, object>> sort)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TAs> GetAllAs<TAs>(Expression<Func<TEntity, TAs>> expression)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriterion filter,
                string sort = null, bool sortAsc = true)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriteria criteria,
                string sort = null, bool sortAsc = true)
            {
                throw new NotImplementedException();
            }

            public TEntity Load(int id)
            {
                throw new NotImplementedException();
            }

            public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> p)
            {
                throw new NotImplementedException();
            }
            
            public bool Any(Expression<Func<TEntity, bool>> p)
            {
                throw new NotImplementedException();
            }

            public Dictionary<int, TEntity> FindManyByIds(IEnumerable<int> ids)
            {
                throw new NotImplementedException();
            }

            public void ClearSession()
            {
                throw new NotImplementedException();
            }
        }

        private class DummyServiceRepository : DummyRepositoryBase<Service>, MapCall.Common.Model.Repositories.IServiceRepository
        {
            public IEnumerable<AggregatedService> GetServicesRenewed(ISearchSet<Service> service)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Service> FindManyByServiceNumberAndPremiseNumber(string serviceNumber,
                string premiseNumber)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Service> FindManyByPremiseNumber(string premiseNumber)
            {
                throw new NotImplementedException();
            }

            public Service FindByPremiseNumberAndServiceNumber(string serviceNumber,
                string premiseNumber)
            {
                throw new NotImplementedException();
            }
            public Service FindByPremiseNumber(string premiseNumber)
            {
                throw new NotImplementedException();
            }

            public Service FindByOperatingCenterAndPremiseNumber(int operatingCenter,
                string premiseNumber, int id)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Service> FindByStreetId(int streetId)
            {
                throw new NotImplementedException();
            }

            public long? GetNextServiceNumber(string operatingCenterCode)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Service> FindByInstallationNumberAndOperatingCenterAndSampleSites(
                string installation, int operatingCenterId)
            {
                throw new NotImplementedException();
            }

            public bool AnyWithInstallationNumberAndOperatingCenterAndSampleSites(
                string installation, int operatingCenterId)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<int> GetDistinctYears()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<int> GetDistinctYearsRetired()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<MonthlyServicesInstalledByCategoryViewModel> GetMonthlyServicesInstalledByCategory(
                ISearchMonthlyServicesInstalledByCategory search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<MonthlyServicesInstalledByCategoryReportViewModel> GetMonthlyServicesInstalledByCategoryReport(
                ISearchMonthlyServicesInstalledByCategory search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<BPUReportForServiceReportItem> GetBPUReportForServices(ISearchBPUReportForServices search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<ServicesRetiredReportItem> GetServicesRetired(ISearchServicesRetired search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<ServicesRenewedSummaryReportItem> GetServicesRenewedSummary(ISearchServicesRenewedSummary search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<ServicesCompletedByCategoryReportItem> GetServicesCompletedByCategory(
                ISearchServicesCompletedByCategory search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TDPendingServicesKPIReportItem> GetTDPendingServicesKPI(ISearchTDPendingServicesKPI search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<OpenIssuedServicesReportItem> GetOpenIssuedServices(ISearchSet<OpenIssuedServicesReportItem> model)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<ServiceQualityAssuranceReportItem> GetServicesQualityAssuranceReport(ISearchServiceQAReport search)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Service> GetServicesWithoutPremiseLinked()
            {
                throw new NotImplementedException();
            }
        }
    }

    public static class StructureMapNHibernateExtensions
    {
        public static ConfigurationExpression RegisterRepository<TInterface, TRepository, TEntity>(this ConfigurationExpression i)
            where TInterface : IRepository<TEntity>
            where TRepository : RepositoryBase<TEntity>, TInterface
            where TEntity : class
        {
            i.For<TInterface>().Use<TRepository>();
            i.For<IRepository<TEntity>>().Use<TRepository>();
            return i;
        }

        public static ConfigurationExpression RegisterRepository<TInterface, TEntity>(this ConfigurationExpression i)
            where TInterface : IRepository<TEntity>
            where TEntity : class
        {
            i.For(typeof(TInterface)).Use(typeof(SecuredRepositoryBase<TEntity, ContractorUser>));
            i.For<IRepository<TEntity>>().Use<SecuredRepositoryBase<TEntity, ContractorUser>>();
            return i;
        }

        public static ConfigurationExpression RegisterRepository<TEntity>(this ConfigurationExpression i)
            where TEntity : class
        {
            return i.RegisterRepository<IRepository<TEntity>, TEntity>();
        }
    }
}
