using System;
using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.IListExtensions;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace Contractors.Tests
{
    #region AsBuiltImage

    public class AsBuiltImageFactory : TestDataFactory<AsBuiltImage>
    {
        #region Constructors

        static AsBuiltImageFactory()
        {
            Defaults(new {
                Town = typeof(TownFactory),
                CreatedAt = Lambdas.GetNow,
                CoordinatesModifiedOn = Lambdas.GetNow,
                FileName = "some file.tif",
                Directory = "SomeDirectory",
                OperatingCenter = typeof(UniqueOperatingCenterFactory)
            });
        }

        public AsBuiltImageFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    // AssetType
    public class AssetTypeFactory : StaticListEntityLookupFactory<AssetType, AssetTypeFactory>
    {
        #region Constructors

        public AssetTypeFactory(IContainer container) : base(container) {}

        #endregion
    }

    public class ValveAssetTypeFactory : AssetTypeFactory
    {
        #region Constructors

        static ValveAssetTypeFactory()
        {
            Defaults(new {Description = "Valve"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)AssetTypeEnum.Valve));
        }

        public ValveAssetTypeFactory(IContainer container) : base(container) {}

        #endregion
    }

    public class HydrantAssetTypeFactory : AssetTypeFactory
    {
        #region Constructors

        static HydrantAssetTypeFactory()
        {
            Defaults(new {Description = "Hydrant"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)AssetTypeEnum.Hydrant));
        }

        public HydrantAssetTypeFactory(IContainer container) : base(container) {}

        #endregion
    }

    public class MainAssetTypeFactory : AssetTypeFactory
    {
        #region Constructors

        static MainAssetTypeFactory()
        {
            Defaults(new {Description = "Main"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)AssetTypeEnum.Main));
        }

        public MainAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ServiceAssetTypeFactory : AssetTypeFactory
    {
        #region Constructors

        static ServiceAssetTypeFactory()
        {
            Defaults(new {Description = "Service"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)AssetTypeEnum.Service));
        }

        public ServiceAssetTypeFactory(IContainer container) : base(container) {}

        #endregion
    }

    public class SewerOpeningAssetTypeFactory : AssetTypeFactory
    {
        #region Constructors

        static SewerOpeningAssetTypeFactory()
        {
            Defaults(new {Description = "Sewer Opening"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)AssetTypeEnum.SewerOpening));
        }

        public SewerOpeningAssetTypeFactory(IContainer container) : base(container) {}

        #endregion
    }

    public class StormCatchAssetTypeFactory : AssetTypeFactory
    {
        #region Constructors

        static StormCatchAssetTypeFactory()
        {
            Defaults(new {Description = "Storm/Catch"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)AssetTypeEnum.StormCatch));
        }

        public StormCatchAssetTypeFactory(IContainer container) : base(container) {}

        #endregion
    }

    // Contractor
    public class ContractorFactory : TestDataFactory<Contractor>
    {
        #region Constants

        public const string DEFAULT_NAME = "Some Test Contractor";
        public const bool DEFAULT_IS_UNION_SHOP = false;
        public const bool DEFAULT_IS_BCP_PARNTER = false;
        public const bool DEFAULT_IS_ACTIVE = true;
        public const string DEFAULT_CREATED_BY = "mcadmin";

        #endregion

        #region Constructors

        static ContractorFactory()
        {
            Defaults(new {
                Name = DEFAULT_NAME,
                IsUnionShop = DEFAULT_IS_UNION_SHOP,
                IsBcpPartner = DEFAULT_IS_BCP_PARNTER,
                IsActive = DEFAULT_IS_ACTIVE,
                CreatedBy = DEFAULT_CREATED_BY, 
                ContractorsAccess = true
            });
        }

        public ContractorFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    #region ContractorMeterCrew

    public class ContractorMeterCrewFactory : TestDataFactory<ContractorMeterCrew>
    {
        static ContractorMeterCrewFactory()
        {
            Defaults(new
            {
                Contractor = typeof(ContractorFactory),
                Description = "Some crew"
            });
        }
        public ContractorMeterCrewFactory(IContainer container) : base(container) { }
    }

    #endregion


    // ContractorUser
    public class ContractorUserFactory : TestDataFactory<ContractorUser>
    {
        #region Constants

        public const string QUESTION = "What's the meaning of life, the universe, and everything?";
        public const string ANSWER = "42";
        
        #endregion
        
        #region Private Members

        private IContractorUserRepository _repository;

        #endregion
        
        #region Properties

        public IContractorUserRepository Repository => _repository ?? (_repository = _container.GetInstance<ContractorUserRepository>());

        #endregion

        #region Constructors

        static ContractorUserFactory()
        {
            var i = 0;
            Func<string> emailFn = () => string.Format("foo{0}@bar.baz", i++);
            Defaults(new {
                Email = emailFn,
                PasswordQuestion = QUESTION,
                PasswordAnswer = ANSWER,
                Password = "foo",
                Contractor = typeof (ContractorFactory),
                IsAdmin = false,
                IsActive = true 
            });
        }

        public ContractorUserFactory(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        protected override ContractorUser Save(ContractorUser entity)
        {
            entity.Email = entity.Email.SanitizeAndDowncase();
            entity.PasswordSalt = Guid.NewGuid();
            entity.Password = entity.Password.Salt(entity.PasswordSalt);
            entity.PasswordAnswer = entity
                .PasswordAnswer
                .SanitizeAndDowncase()
                .Salt(entity.PasswordSalt);
            return Repository.Save(entity);
        }

        #endregion  
    }

    public class AdminUserFactory : ContractorUserFactory
    {
        #region Constructors

        static AdminUserFactory()
        {
            Defaults(new {IsAdmin = true});
        }

        public AdminUserFactory(IContainer container) : base(container) {}

        #endregion
    }

    #region CountyFactory

    public class CountyFactory : TestDataFactory<County>
    {
        #region Constants

        public static string NAME = "Monmouth";

        #endregion

        #region Fields

        public static int _count;

        #endregion

        #region Constructors

        static CountyFactory()
        {
            Func<string> nameCreatorThing = () => {
                _count++;
                return String.Format("{0} {1}", NAME, _count);
            };
            Defaults(new
            {
                Name = nameCreatorThing,
                State = typeof(StateFactory),
                Enabled = true
            });
        }

        public CountyFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override County Create(object overrides = null)
        {
            var c = base.Create(overrides);
            if (!c.State.Counties.Contains(c))
            {
                c.State.Counties.Add(c);
            }
            return c;
        }

        protected override County Save(County entity)
        {
            // Prevent duplicate asset types from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<County>>();
            var existing = repo.GetAll().Where(x => x.Name == entity.Name).SingleOrDefault();
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    #endregion

    // Crew
    public class CrewFactory : TestDataFactory<Crew>
    {
        #region Constant

        public const string DEFAULT_DESCRIPTION = "Toms";

        #endregion

        #region Constructors

        static CrewFactory()
        {
            Defaults(new {
                Contractor = typeof(ContractorFactory),
                Description = DEFAULT_DESCRIPTION
            });
        }

        public CrewFactory(IContainer container) : base(container) {}

        #endregion
    }

    // Document
    public class DocumentFactory : TestDataFactory<Document>
    {
        #region Constants

        public const string FILE_NAME = "FileName";

        #endregion

        #region Constructors

        static DocumentFactory()
        {
            Defaults(new {
                FileName = FILE_NAME,
                CreatedAt = DateTime.Now.Date,
                DocumentData = typeof(DocumentDataFactory)
            });
        }

        public DocumentFactory(IContainer container) : base(container) {}

       #endregion
    }


    #region DocumentDataFactory

    public class DocumentDataFactory : TestDataFactory<DocumentData>
    {
        private static int _currentBinaryData = 0;

        public DocumentDataFactory(IContainer container) : base(container) { }

        static DocumentDataFactory()
        {
            // This func's for making sure DocumentDataFactory
            // creates a unique hash instance by default.
            Func<byte[]> incrementalBinaryData = () =>
            {
                _currentBinaryData++;
                return BitConverter.GetBytes(_currentBinaryData);
            };

            Defaults(new
            {
                BinaryData = incrementalBinaryData
            });
        }

        public override DocumentData Build(object overrides = null)
        {
            var data = base.Build(overrides);
            if (data.BinaryData != null)
            {
                var docServ = _container.GetInstance<IDocumentService>();
                data.Hash = docServ.Save(data.BinaryData);
                data.FileSize = data.BinaryData.Length;
            }
            return data;
        }

        protected override DocumentData Save(DocumentData entity)
        {
            // Hash is a unique field, so we need to return the same instance.
            var existing = Session.Query<DocumentData>().SingleOrDefault(x => x.Hash == entity.Hash);
            if (existing != null)
            {
                return existing;
            }
            return base.Save(entity);
        }
    }

    #endregion

    // DocumentType
    public class DocumentTypeFactory : TestDataFactory<DocumentType>
    {
        #region Constants

        public const string DOCUMENT_TYPE_NAME = "Image";
        public const int DATA_TYPE_ID = 127;

        #endregion

        #region Constructors

        static DocumentTypeFactory()
        {
            Defaults(new { 
                Name = DOCUMENT_TYPE_NAME,
                DataType = typeof(WorkOrdersDataTypeFactory)
            });
        }

        public DocumentTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    // Hydrant
    public class HydrantFactory : TestDataFactory<Hydrant>
    {
        #region Constants

        public const string DERFAULT_HYDRANT_NUMBER = "MEH-666";

        #endregion

        #region Constructors

        public HydrantFactory(IContainer container) : base(container) {}

        static HydrantFactory()
        {
            Defaults(new {
                HydrantNumber = DERFAULT_HYDRANT_NUMBER,
                Town = typeof(TownFactory),
                Status = typeof(ActiveAssetStatusFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                HydrantBilling = typeof(PublicHydrantBillingFactory)
            });
        }

        #endregion
    }

    // MainBreaks
    public class MainBreakFactory : TestDataFactory<MainBreak>
    {
        #region Constructors
        
        static MainBreakFactory()
        {
            Defaults(new {
                MainBreakMaterial = typeof(MainBreakMaterialFactory),
                MainCondition = typeof(MainConditionFactory),
                MainFailureType = typeof(MainFailureTypeFactory),
                Depth = 1.0m,
                MainBreakSoilCondition = typeof(MainBreakSoilConditionFactory),
                CustomersAffected = 2, 
                ShutdownTime = 3.0m,
                MainBreakDisinfectionMethod = typeof(MainBreakDisinfectionMethodFactory),
                MainBreakFlushMethod = typeof(MainBreakFlushMethodFactory), 
                ChlorineResidual = 0.2m,
                BoilAlertIssued = true,
                ServiceSize = typeof(ServiceSizeFactory),
                WorkOrder = typeof(WorkOrderFactory),
                ReplacedWith = typeof(MainBreakMaterialFactory),
                FootageReplaced = 3
            });
        }

        public MainBreakFactory(IContainer container) : base(container) {}

        #endregion
    }

    public class GenericMainBreakFactory : MainBreakFactory
    {
        #region Constructors

        static GenericMainBreakFactory()
        {
            Defaults(new{
                BoilAlertIssued = false,
                ChlorineResidual = 1.0m,
                CustomersAffected = 1,
                Depth = 1.0m,
                ShutdownTime = 1.0m,
                MainFailureType = typeof(MainFailureTypeFactory),
                ServiceSize = typeof(ServiceSizeFactory),
                MainCondition = typeof(MainConditionFactory),
                MainBreakMaterial = typeof(MainBreakMaterialFactory),
                MainBreakSoilCondition = typeof(MainBreakSoilConditionFactory),
                MainBreakDisinfectionMethod = typeof(MainBreakDisinfectionMethodFactory),
                MainBreakFlushMethod = typeof(MainBreakFlushMethodFactory)
            });
        }

        public GenericMainBreakFactory(IContainer container) : base(container) { }

        #endregion
    }

    #region Main Break Lookups

    public class ServiceSizeFactory : TestDataFactory<ServiceSize>
    {
        #region Constants

        public const string SIZE_SERV = "111";

        #endregion

        #region Constructors

        static ServiceSizeFactory()
        {
            Defaults(new{ ServiceSizeDescription = SIZE_SERV });
        }

        public ServiceSizeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #region ServiceMaterial

    public class ServiceMaterialFactory : TestDataFactory<ServiceMaterial>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "ServiceMaterial";

        #endregion

        #region Constructors

        static ServiceMaterialFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ServiceMaterialFactory(IContainer container) : base(container) {}

        #endregion
    }

    #endregion

    #endregion

    // Markout Factory
    public class MarkoutFactory : TestDataFactory<Markout>
    {
        #region Constants

        internal const string MARKOUT_NUMBER = "123ABC789";

        #endregion
        
        #region Constructors

        static MarkoutFactory()
        {
            Func<DateTime> dateFn = () => DateTime.Now;

            Defaults(new {
                DateOfRequest = dateFn,
                MarkoutNumber = MARKOUT_NUMBER,
                MarkoutType = typeof(MarkoutTypeFactory),
                WorkOrder = typeof(WorkOrderFactory) // I do not know if this is the correct factory to use. -Ross 10/4/2017
            });
        }

        public MarkoutFactory(IContainer container) : base(container) {}

        #endregion
    }

    #region MarkoutType

    //public class MarkoutTypeFactory : UniqueEntityLookupFactory<MarkoutType>
    //{
    //    #region Constants

    //    public const string DEFAULT_DESCRIPTION = "C TO C";

    //    #endregion

    //    #region Constructors

    //    static MarkoutTypeFactory()
    //    {
    //        Defaults(new { Description = DEFAULT_DESCRIPTION });
    //    }

    //    public MarkoutTypeFactory(IContainer container) : base(container) { }

    //    #endregion


    //    public override MarkoutType Build(object overrides = null)
    //    {
    //        var model = base.Build(overrides);
    //        model.SetPropertyValueByName("Id", 1);
    //        return model;
    //    }


    //}


    //public class NoneMarkoutTypeFactory : MarkoutTypeFactory
    //{
    //    #region Constants

    //    public const string DEFAULT_DESCRIPTION = "NOT LISTED";

    //    #endregion

    //    #region Constructors

    //    static NoneMarkoutTypeFactory()
    //    {
    //        Defaults(new { Description = DEFAULT_DESCRIPTION });
    //    }

    //    public NoneMarkoutTypeFactory(IContainer container) : base(container) { }

    //    #endregion

    //    public override MarkoutType Build(object overrides = null)
    //    {
    //        var model = base.Build(overrides);
    //        model.SetPropertyValueByName("Id", MarkoutType.INDICES.NONE);
    //        return model;
    //    }

    //}

    public class BaseMarkoutTypeFactory : StaticListEntityLookupFactory<MarkoutType, BaseMarkoutTypeFactory>
    {
        #region Constructors

        public BaseMarkoutTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MarkoutTypeFactory : BaseMarkoutTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "NOT LISTED";

        #endregion

        #region Constructors

        static MarkoutTypeFactory()
        {
            Defaults(new
            {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) =>
                a.SetPropertyValueByName("Id", 1)
            );
        }

        public MarkoutTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class NoneMarkoutTypeFactory : BaseMarkoutTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "NOT LISTED";

        #endregion

        #region Constructors

        static NoneMarkoutTypeFactory()
        {
            Defaults(new
            {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) =>
                a.SetPropertyValueByName("Id", (int)MarkoutType.Indices.NONE)
            );
        }

        public NoneMarkoutTypeFactory(IContainer container) : base(container) { }

        #endregion
    }
   

    #endregion

    // MarkoutRequirement
    public class MarkoutRequirementFactory : StaticListEntityLookupFactory<MarkoutRequirement, MarkoutRequirementFactory>
    {
        #region Constructors
        
        public MarkoutRequirementFactory(IContainer container) : base(container) { }

        #endregion
    }

    // MarkoutRequirementNone
    public class MarkoutRequirementNoneFactory : MarkoutRequirementFactory
    {
        static MarkoutRequirementNoneFactory()
        {
            Defaults(new {Description = "None"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.None));
        }

        public MarkoutRequirementNoneFactory(IContainer container) : base(container) { }
    }

    // MarkoutRequirementEmergency
    public class MarkoutRequirementEmergencyFactory : MarkoutRequirementFactory
    {
        static MarkoutRequirementEmergencyFactory()
        {
            Defaults(new { Description = "Emergency" });
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Emergency));
        }

        public MarkoutRequirementEmergencyFactory(IContainer container) : base(container) { }
    }

    // MarkoutRequirementRoutine
    public class MarkoutRequirementRoutineFactory : MarkoutRequirementFactory
    {
        static MarkoutRequirementRoutineFactory()
        {
            Defaults(new { Description = "Routine" });
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)MarkoutRequirementEnum.Routine));
        }

        public MarkoutRequirementRoutineFactory(IContainer container) : base(container) { }
    }

    // Material
    public class MaterialFactory : TestDataFactory<Material>
    {
        #region Constants

        public const string PART_NUMBER = "888888888",
                            DESCRIPTION = "Some part";

        #endregion

        #region Constructors

        static MaterialFactory()
        {
            Defaults(new {
                PartNumber = PART_NUMBER,
                Description = DESCRIPTION,
                IsActive = true
            });
        }

        public MaterialFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    // MaterialUsed
    public class MaterialUsedFactory : TestDataFactory<MaterialUsed>
    {
        #region Constructors

        static MaterialUsedFactory()
        {
            Defaults(new {
                WorkOrder = typeof(FinalizationWorkOrderFactory),
                Material = typeof(MaterialFactory),
                Quantity = 3,
            });
        }

        public MaterialUsedFactory(IContainer container) : base(container) { }

        #endregion
    }

    #region MeterChangeOut

    public class MeterChangeOutFactory : TestDataFactory<MeterChangeOut>
    {
        static MeterChangeOutFactory()
        {
            Defaults(new
            {
                Contract = typeof(MeterChangeOutContractFactory),
                CustomerName = "Some guy",
                ServiceStreetNumber = "123",
                ServiceStreet = "Some Street",
                ServiceCity = typeof(TownFactory)
            });
        }
        public MeterChangeOutFactory(IContainer container) : base(container) { }

        public override MeterChangeOut Build(object overrides = null)
        {
            var mco = base.Build(overrides);
            mco.Contract.MeterChangeOuts.AddIfMissing(mco);
            return mco;
        }
    }

    #endregion

    #region MeterChangeOutContract

    public class MeterChangeOutContractFactory : TestDataFactory<MeterChangeOutContract>
    {
        static MeterChangeOutContractFactory()
        {
            Defaults(new
            {
                Description = "Some contract",
                OperatingCenter = typeof(UniqueOperatingCenterFactory),
                Contractor = typeof(ContractorFactory)
            });
        }
        public MeterChangeOutContractFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterChangeOutStatus

    public class MeterChangeOutStatusFactory : UniqueEntityLookupFactory<MeterChangeOutStatus>
    {
        public MeterChangeOutStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterScheduleTime

    public class MeterScheduleTimeFactory : UniqueEntityLookupFactory<MeterScheduleTime>
    {
        public MeterScheduleTimeFactory(IContainer container) : base(container) { }
    }

    #endregion

    // OperatingCenter
    public class OperatingCenterFactory : MapCall.Common.Testing.Data.OperatingCenterFactory
    {
        public OperatingCenterFactory(IContainer container) : base(container) { }
    }

    public class RequisitionTypeFactory : TestDataFactory<RequisitionType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Paving";

        #endregion

        #region Constructors

        static RequisitionTypeFactory()
        {
            Defaults(new
            {
                Description = DEFAULT_DESCRIPTION,
            });
        }

        public RequisitionTypeFactory(IContainer container) : base(container)
        {

        }

        #endregion
    }

    public class RequisitionFactory : TestDataFactory<Requisition>
    {
        #region Constants

        public const string DEFAULT_SAP_NUMBER = "sapnumber4";
        public static readonly DateTime DEFAULT_CREATED_ON_DATE = DateTime.Today; 

        #endregion

        #region Constructors

        static RequisitionFactory()
        {
            Defaults(new {
                RequisitionType = typeof(RequisitionTypeFactory),
                WorkOrder = typeof(WorkOrderFactory),
                CreatedBy = typeof(UserFactory),
                CreatedAt = DEFAULT_CREATED_ON_DATE,
                SAPRequisitionNumber = DEFAULT_SAP_NUMBER
            });
        }

        public RequisitionFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override Requisition Save(Requisition entity)
        {
            if (!entity.WorkOrder.Requisitions.Contains(entity))
            {
                entity.WorkOrder.Requisitions.Add(entity);
            }
            return base.Save(entity);
        }

        #endregion
    }

    public class RestorationFactory : TestDataFactory<Restoration>
    {
        #region Constants

        public const decimal LINEAR_FEET_OF_CURB = 50m;

        #endregion

        #region Constructors

        static RestorationFactory()
        {
            Defaults(new
            {
                WorkOrder = typeof(WorkOrderFactory),
                ResponsePriority = typeof(RestorationResponsePriorityFactory),
                RestorationType = typeof(RestorationTypeFactory),
                LinearFeetOfCurb = LINEAR_FEET_OF_CURB,
                EightInchStabilizeBaseByCompanyForces = false,
                AssignedContractor = typeof(ContractorFactory)
            });
        }

        public RestorationFactory(IContainer container) : base(container) { }

        #endregion
    }


    public class RestorationTypeFactory : UniqueEntityLookupFactory<RestorationType>
    {
        #region Constructors

        static RestorationTypeFactory()
        {
            Defaults(new
            {
                //   Description = "RestorationType"
            });
        }

        public RestorationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class LinearRestorationTypeFactory : TestDataFactory<RestorationType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "SOMETHING SOMETHING CURB";

        #endregion
        
        #region Constructors

        static LinearRestorationTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public LinearRestorationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class RestorationTypeCostFactory : TestDataFactory<RestorationTypeCost>
    {
        #region Constants

        public const double DEFAULT_COST = 10.0;

        #endregion

        #region Constructors

        static RestorationTypeCostFactory()
        {
            Defaults(new {
                Cost = DEFAULT_COST,
                OperatingCenter = typeof(UniqueOperatingCenterFactory),
                RestorationType = typeof(RestorationTypeFactory)
            });
        }

        public RestorationTypeCostFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    // Inherited from TestDataFactory<Service> to address test for null ServiceNumber
    public class ServiceFactory : TestDataFactory<Service>
    {
        #region Constants

        public const string DEFAULT_PREMISE_NUMBER = "8568675309";

        #endregion

        #region Constructors

        static ServiceFactory()
        {
            Defaults(new {
                PremiseNumber = DEFAULT_PREMISE_NUMBER,
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                Initiator = typeof(UserFactory),
                UpdatedBy = typeof(UserFactory)
            });
        }

        public ServiceFactory(IContainer container) : base(container) { }

        #endregion
    }
    
    #region ServiceInstallation
    //public class MeterSupplementalLocation : EntityLookupTestDataFactory<MeterSupplementalLocation> { }
    public class ServiceInstallationFactory : TestDataFactory<ServiceInstallation>
    {
        #region Constants

        public const string DEFAULT_METER_MANUFACTURER_SERIAL_NUMBER = "12341234",
            DEFAULT_RFMIU = "123",
            DEFAULT_CURRENT_READ = "000001",
            DEFAULT_METER_LOCATION_INFORMATION = " These are some notes about the install.";

        #endregion

        #region Constructors

        static ServiceInstallationFactory()
        {
            Defaults(new
            {
                MeterManufacturerSerialNumber = DEFAULT_METER_MANUFACTURER_SERIAL_NUMBER,
                Register1RFMIU = DEFAULT_RFMIU,
                Register1CurrentRead = DEFAULT_CURRENT_READ,
                MeterLocationInformation = DEFAULT_METER_LOCATION_INFORMATION,
                WorkOrder = typeof(WorkOrderFactory),
                MeterLocation = typeof(EntityLookupTestDataFactory<MeterSupplementalLocation>),
                MeterPositionalLocation = typeof(EntityLookupTestDataFactory<SmallMeterLocation>),
                MeterDirectionalLocation = typeof(EntityLookupTestDataFactory<MeterDirection>),
                ReadingDevicePosition = typeof(EntityLookupTestDataFactory<SmallMeterLocation>),
                ReadingDeviceSupplemental = typeof(EntityLookupTestDataFactory<MeterSupplementalLocation>),
                ReadingDeviceDirectionalInformation = typeof(EntityLookupTestDataFactory<MeterDirection>),
                Register1ReadType = typeof(EntityLookupTestDataFactory<ServiceInstallationReadType>),
                Activity1 = typeof(EntityLookupTestDataFactory<ServiceInstallationFirstActivity>),
                ServiceFound = typeof(EntityLookupTestDataFactory<ServiceInstallationPosition>),
                ServiceLeft = typeof(EntityLookupTestDataFactory<ServiceInstallationPosition>),
                ServiceInstallationReason = typeof(EntityLookupTestDataFactory<ServiceInstallationReason>)
            });
        }

        public ServiceInstallationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    public class SpoilFactory : TestDataFactory<Spoil>
    {
        #region Constants

        public const decimal DEFAULT_QUANTITY = (decimal)2.5;

        #endregion

        #region Constructors

        static SpoilFactory()
        {
            Defaults(new {
                Quantity = DEFAULT_QUANTITY,
                SpoilStorageLocation = typeof(SpoilStorageLocationFactory),
                WorkOrder = typeof(WorkOrderFactory)
            });
        }

        public SpoilFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class SpoilStorageLocationFactory : TestDataFactory<SpoilStorageLocation>
    {
        #region Constants

        public const string NAME = "Some Spoil Storage Location";

        #endregion

        #region Constructors

        static SpoilStorageLocationFactory()
        {
            Defaults(new {
                Name = NAME, OperatingCenter = typeof(UniqueOperatingCenterFactory)
            });
        }

        public SpoilStorageLocationFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class SpoilFinalProcessingLocationFactory : TestDataFactory<SpoilFinalProcessingLocation>
    {
        #region Constants

        public const string NAME = "Some Spoil Storage Location";

        #endregion

        #region Constructors

        static SpoilFinalProcessingLocationFactory()
        {
            Defaults(new { Name = NAME, OperatingCenter = typeof(UniqueOperatingCenterFactory) });
        }

        public SpoilFinalProcessingLocationFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    #region State

    public class StateFactory : TestDataFactory<State>
    {
        #region Constants

        public const string DEFAULT_NAME = "New Jersey",
            DEFAULT_ABBREVIATION = "NJ";

        #endregion

        #region Fields

        public static int _count;

        #endregion

        #region Constructors

        static StateFactory()
        {
            Func<string> nameCreatorThing = () => {
                _count++;
                return String.Format("{0} {1}", DEFAULT_NAME, _count);
            };
            Defaults(new
            {
                Name = nameCreatorThing,
                Abbreviation = DEFAULT_ABBREVIATION
            });
        }

        public StateFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        protected override State Save(State entity)
        {
            // Prevent duplicate asset types from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<State>>();
            var existing = repo.GetAll().SingleOrDefault(x => x.Abbreviation == entity.Abbreviation);
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    #endregion

    // StockLocation
    public class StockLocationFactory : TestDataFactory<StockLocation>
    {
        #region Constants

        public const string DESCRIPTION = "Some Place";

        #endregion

        #region Constructors

        static StockLocationFactory()
        {
            Defaults(new {
                Description = DESCRIPTION, OperatingCenter = typeof(UniqueOperatingCenterFactory)
            });
        }

        public StockLocationFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    // SewerOpening
    public class SewerOpeningFactory : TestDataFactory<SewerOpening>
    {
        #region Constants

        public const string DERFAULT_OPENING_NUMBERT = "MEH-666";

        #endregion

        #region Constructors

        public SewerOpeningFactory(IContainer container) : base(container) {}

        static SewerOpeningFactory()
        {
            Defaults(new {
                OpeningNumber = DERFAULT_OPENING_NUMBERT,
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                SewerOpeningType = typeof(SewerOpeningTypeFactory)
            });
        }

        #endregion
    }

    // SewerOpeningType
    public class SewerOpeningTypeFactory : TestDataFactory<SewerOpeningType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "SewerOpeningType";

        #endregion

        #region Constructors

        public SewerOpeningTypeFactory(IContainer container) : base(container) { }

        static SewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        #endregion
    }


    // StormCatch
    public class StormCatchFactory : TestDataFactory<StormWaterAsset>
    {
        #region Constants

        public const string DEFAULT_ASSET_NUMBER = "MEH-666",
                            DEFAULT_CREATED_BY = "some guy";

        #endregion

        #region Constructors

        public StormCatchFactory(IContainer container) : base(container) {}

        static StormCatchFactory()
        {
            Defaults(new {
                AssetNumber = DEFAULT_ASSET_NUMBER,
                CreatedBy = DEFAULT_CREATED_BY,
                AssetType = typeof(StormWaterAssetTypeFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory)
            });
        }

        #endregion
    }

    // StreetOpeningPermit
    public class StreetOpeningPermitFactory : TestDataFactory<StreetOpeningPermit>
    {
        #region Constants

        public const string DEFAULT_STREET_OPENING_PERMIT_NUMBER = "1234";

        #endregion

        #region Constructors

        static StreetOpeningPermitFactory()
        {
            Func<DateTime> dateFn = () => DateTime.Now;
            Defaults(new {
                DateRequested = dateFn,
                StreetOpeningPermitNumber = DEFAULT_STREET_OPENING_PERMIT_NUMBER,
                WorkOrder = typeof(WorkOrderFactory)
            });
        }

        public StreetOpeningPermitFactory(IContainer container) : base(container) { }

        #endregion
    }

    #region TapImage

    public class TapImageFactory : TestDataFactory<TapImage>
    {
        #region Constructors

        static TapImageFactory()
        {
            Defaults(new
            {
                Town = typeof(TownFactory),
                OperatingCenter = typeof(UniqueOperatingCenterFactory),
                FileName = "some file.tif",
                Directory = "SomeDirectory",
            });
        }

        public TapImageFactory(IContainer container) : base(container) { }

        #endregion

        public override TapImage Create(object overrides = null)
        {
            var t = base.Create(overrides);
            if (t.Service != null && !t.Service.TapImages.Contains(t))
            {
                t.Service.TapImages.Add(t);
            }
            return t;
        }
    }

    #endregion

    #region ValveImage

    public class ValveImageFactory : TestDataFactory<ValveImage>
    {
        #region Constructors

        static ValveImageFactory()
        {
            Func<DateTime> createdAtFn = () => DateTime.Now;

            Defaults(new {
                Town = typeof(TownFactory),
                OperatingCenter = typeof(UniqueOperatingCenterFactory),
                FileName = "some file.tif",
                Directory = "SomeDirectory",
                CreatedAt = createdAtFn
            });
        }

        public ValveImageFactory(IContainer container) : base(container) { }

        #endregion

        public override ValveImage Create(object overrides = null)
        {
            var vi = base.Create(overrides);
            if (vi.Valve != null && !vi.Valve.ValveImages.Contains(vi))
            {
                vi.Valve.ValveImages.Add(vi);
            }
            return vi;
        }
    }

    #endregion

    // Town
    public class TownFactory : TestDataFactory<Town>
    {
        #region Constants

        public const string DEFAULT_TOWN_TEXT = "Default Town";

        #endregion

        #region Constructors

        static TownFactory()
        {
            Defaults(new {
                ShortName = DEFAULT_TOWN_TEXT,
                County = typeof(CountyFactory)
            });
        }

        public TownFactory(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        protected override Town Save(Town entity)
        {
            if (!entity.County.Towns.Contains(entity))
            {
                entity.County.Towns.Add(entity);
            }
            return base.Save(entity);
        }

        #endregion
    }

    //TownSection
    public class TownSectionFactory : TestDataFactory<TownSection>
    {
        #region Constants

        public const string DEFAULT_TOWN_SECTION_TEXT = "Default Town Section";

        #endregion
        
        #region Constructors
        
        static TownSectionFactory()
        {
            Defaults(new {
                Name = DEFAULT_TOWN_SECTION_TEXT,
                Town = typeof(TownFactory)
            });
        }

        public TownSectionFactory(IContainer container) : base(container) { }

        #endregion
    }

    // Valve
    public class ValveFactory : TestDataFactory<Valve>
    {
        #region Constants

        public const string DEFAULT_IDENTIFIER = "VHD-5000";

        #endregion

        #region Constructors

        static ValveFactory()
        {
            Defaults(new {
                ValveNumber = DEFAULT_IDENTIFIER,
                OperatingCenter = typeof(UniqueOperatingCenterFactory),
                Status = typeof(ActiveAssetStatusFactory),
                Town = typeof(TownFactory),
                ValveBilling = typeof(PublicValveBillingFactory)
            });
        }

        public ValveFactory(IContainer container) : base(container) {}

        #endregion
    }

    // WorkDescription
    public class WorkDescriptionFactory : StaticListEntityLookupFactory<WorkDescription, WorkDescriptionFactory>
    {
        #region Constructors

        static WorkDescriptionFactory()
        {
            Defaults(new {
                PlantMaintenanceActivityType = typeof(PlantMaintenanceActivityTypeFactory),
                IsActive = true,
                FirstRestorationAccountingCode = typeof(RestorationAccountingCodeFactory),
                FirstRestorationProductCode = typeof(EntityLookupTestDataFactory<RestorationProductCode>),
            });
        }
        
        public WorkDescriptionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ValveWorkDescriptionFactory : WorkDescriptionFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Some Valve Thing";

        #endregion

        #region Constructors

        static ValveWorkDescriptionFactory()
        {
            Defaults(new {
                AssetType = typeof(ValveAssetTypeFactory),
                Description = DEFAULT_DESCRIPTION,
            });
        }

        public ValveWorkDescriptionFactory(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class ValveBoxRepairWorkDescriptionFactory : ValveWorkDescriptionFactory
    {
        #region Constants

        public const string VALVE_BOX_DEFAULT_DESCRIPTION = "Valve Box Repair";
        
        #endregion

        #region Constructors

        static ValveBoxRepairWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = VALVE_BOX_DEFAULT_DESCRIPTION,
                    TimeToComplete = 15m
                });
            OnSaving(
                (a, s) =>
                    a.SetPropertyValueByName("Id",
                        (int)WorkDescriptionEnum.ValveBoxRepair));
        }

        public ValveBoxRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class FrozenHydrantWorkDescriptionFactory : WorkDescriptionFactory
    {
        #region Constants
            
        public const string DEFAULT_DESCRIPTION = "Frozen Hydrant";

        #endregion

        #region Constructors

        static FrozenHydrantWorkDescriptionFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION, AssetType = typeof (HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkDescriptionEnum.FrozenHydrant));
        }

        public FrozenHydrantWorkDescriptionFactory(IContainer container) : base(container) { }
        
        #endregion
    }

    public class MainBreakReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        #region Constants
        
        public const string DEFAULT_DESCRIPTION = "Water Main Break Replace";

        #endregion

        #region Constructors

        static MainBreakReplaceWorkDescriptionFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION, AssetType = typeof(MainAssetTypeFactory) });
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkDescriptionEnum.WaterMainBreakReplace));
        }

        public MainBreakReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CurbBoxRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "CURB BOX REPAIR";
        
        #endregion

        #region Constructors

        static CurbBoxRepairWorkDescriptionFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION, AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkDescriptionEnum.CurbBoxRepair));
        }

        public CurbBoxRepairWorkDescriptionFactory(IContainer container) : base(container)
        {
        }

        #endregion

    }

    public class ServiceLineRenewalWorkDescriptionFactory : WorkDescriptionFactory
    {
        public const string DEFAULT_DESCRIPTION = "SERVICE LINE REPAIR";

        static ServiceLineRenewalWorkDescriptionFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION, AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a,s) => a.SetPropertyValueByName("Id", (int)WorkDescriptionEnum.ServiceLineRenewal));
        }
        public ServiceLineRenewalWorkDescriptionFactory(IContainer container) : base(container) { }
    }

    public class ServiceLineInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public const string DEFAULT_DESCRIPTION = "SERVICE LINE INSTALLATION";

        static ServiceLineInstallationWorkDescriptionFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION, AssetType = typeof(ServiceAssetTypeFactory) });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION);
        }
        public ServiceLineInstallationWorkDescriptionFactory(IContainer container) : base(container) { }
    }

    public class ServiceLineRetireWorkDescriptionFactory : WorkDescriptionFactory
    {
        public const string DEFAULT_DESCRIPTION = "SERVICE LINE RETIRE";

        static ServiceLineRetireWorkDescriptionFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION, 
                AssetType = typeof(ServiceAssetTypeFactory)
            });
            OnSaving((a, s) => a.Id = (int)WorkDescriptionEnum.ServiceLineRetire);
        }

        public ServiceLineRetireWorkDescriptionFactory(IContainer container) : base(container) { }
    }

    public class ServiceLineRenewalCompanySideFactory : WorkDescriptionFactory
    {
        public const string DEFAULT_DESCRIPTION = "SERVICE LINE RENEWAL COMPANY SIDE";

        static ServiceLineRenewalCompanySideFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION, AssetType = typeof(ServiceAssetTypeFactory) });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
        }
        public ServiceLineRenewalCompanySideFactory(IContainer container) : base(container) { }
    }

   

    // WorkOrder

    // WorkOrderPriority
    public class WorkOrderPriorityFactory : TestDataFactory<WorkOrderPriority>
    {
        #region Constructors
        
        public WorkOrderPriorityFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override WorkOrderPriority Build(object overrides = null)
        {
            if (GetType() == typeof(WorkOrderPriorityFactory))
            {
                throw new InvalidOperationException("The base WorkOrderPriorityFactory should not be used directly. Please use one of the inherited factory classes instead");
            }
            return base.Build(overrides);
        }

        #endregion

        #region Private Methods

        protected override WorkOrderPriority Save(WorkOrderPriority entity)
        {
            var repository = _container.GetInstance<RepositoryBase<WorkOrderPriority>>();
            var ret = repository.Find(entity.Id);

            return ret ?? base.Save(entity);
        }

        #endregion
    }

    // EmergencyWorkOrderPriority
    public class EmergencyWorkOrderPriorityFactory : WorkOrderPriorityFactory
    {
        #region Constructors

        static EmergencyWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Emergency"});
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkOrderPriorityEnum.Emergency));
        }

        public EmergencyWorkOrderPriorityFactory(IContainer container) : base(container) { }

        #endregion
    }

    // HighPriorityWorkOrderPriority
    public class HighPriorityWorkOrderPriorityFactory : WorkOrderPriorityFactory
    {
        #region Constructors

        static HighPriorityWorkOrderPriorityFactory()
        {
            Defaults(new { Description = "High Priority" });
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkOrderPriorityEnum.HighPriority));
        }

        public HighPriorityWorkOrderPriorityFactory(IContainer container) : base(container) { }

        #endregion
    }

    // RoutineWorkOrderPriority
    public class RoutineWorkOrderPriorityFactory : WorkOrderPriorityFactory
    {
        #region Constructors

        static RoutineWorkOrderPriorityFactory()
        {
            Defaults(new { Description = "Routine" });
            OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkOrderPriorityEnum.Routine));
        }

        public RoutineWorkOrderPriorityFactory(IContainer container) : base(container) { }

        #endregion
    }

    //// WorkOrderRequester
    //public class WorkOrderRequesterFactory : TestDataFactory<WorkOrderRequester>
    //{
    //    #region Constants

    //    public const string DEFAULT_DESCRIPTION = "Company Forces";

    //    #endregion

    //    #region Constructors

    //    static WorkOrderRequesterFactory()
    //    {
    //        Defaults(new WorkOrderRequester {Description = DEFAULT_DESCRIPTION});
    //    }

    //    public WorkOrderRequesterFactory(IContainer container) : base(container) { }

    //    #endregion
    //}

    #region NoteFactory

    public class NoteFactory : TestDataFactory<Note>
    {
        public const string TEXT = "the note",
            CREATED_BY = "some guy";

        public const int LINKED_ID = 666;

        public NoteFactory(IContainer container) : base(container) { }

        static NoteFactory()
        {
            Defaults(new {
                Text = TEXT,
                CreatedBy = CREATED_BY,
                LinkedId = LINKED_ID,
                CreatedAt = Lambdas.GetNow,
                DataType = typeof(DataTypeFactory)
            });
        }
    }

    #endregion

    #region PitcherFilter

    public class BasePitcherFilterDeliveryMethodFactory : StaticListEntityLookupFactory<PitcherFilterCustomerDeliveryMethod, BasePitcherFilterDeliveryMethodFactory>
    {
        #region Constructors

        public BasePitcherFilterDeliveryMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PitcherFilterCustomerDeliveryMethodHandedToCustomerFactory : BasePitcherFilterDeliveryMethodFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Handed to Customer";

        #endregion

        #region Constructors

        static PitcherFilterCustomerDeliveryMethodHandedToCustomerFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
        }

        public PitcherFilterCustomerDeliveryMethodHandedToCustomerFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PitcherFilterCustomerDeliveryMethodLeftOnPorchFactory : BasePitcherFilterDeliveryMethodFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Left on Porch/Doorstep";

        #endregion

        #region Constructors

        static PitcherFilterCustomerDeliveryMethodLeftOnPorchFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
        }

        public PitcherFilterCustomerDeliveryMethodLeftOnPorchFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PitcherFilterCustomerDeliveryMethodOtherFactory : BasePitcherFilterDeliveryMethodFactory
    {
        #region Constants

          public const string DEFAULT_DESCRIPTION = "Other";

        #endregion

        #region Constructors
        static PitcherFilterCustomerDeliveryMethodOtherFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
        }
        public PitcherFilterCustomerDeliveryMethodOtherFactory(IContainer container) : base(container) { }       

        #endregion
    }

    #endregion
}
