using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Documents;
using MMSINC.Utilities.Pdf;
using Moq;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Configuration
{
    [TestClass]
    public class MapCallDependenciesTest
    {
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(i => {
                MapCallDependencies.RegisterRepositories(i);
                i.For<IAuthenticationService<User>>().Use(new Mock<IAuthenticationService<User>>().Object);
                i.For<ISession>().Use(new Mock<ISession>().Object);
                i.For<IDateTimeProvider>().Use(new Mock<IDateTimeProvider>().Object);
                i.For<IDocumentService>().Use(new Mock<IDocumentService>().Object);
                i.For<IImageToPdfConverter>().Use(new Mock<IImageToPdfConverter>().Object);
            });
        }

        [TestMethod]
        public void TestRegisterRepositoriesRegistersSomeStuffWeNeed()
        {
            TestIsRegistered<IAsBuiltImageRepository, AsBuiltImageRepository>();
            TestIsRegistered<IAuditLogEntryRepository, AuditLogEntryRepository>();
            TestIsRegistered<IBacterialWaterSampleRepository, BacterialWaterSampleRepository>();
            TestIsRegistered<IBappTeamRepository, BappTeamRepository>();
            TestIsRegistered<IBodyOfWaterRepository, BodyOfWaterRepository>();
            TestIsRegistered<ICommercialDriversLicenseProgramStatusRepository,
                CommercialDriversLicenseProgramStatusRepository>();
            TestIsRegistered<IContractorRepository, ContractorRepository>();
            TestIsRegistered<IContactRepository, ContactRepository>();
            TestIsRegistered<IContractorLaborCostRepository, ContractorLaborCostRepository>();
            TestIsRegistered<ICountyRepository, CountyRepository>();
            TestIsRegistered<ICustomerLocationRepository, CustomerLocationRepository>();
            TestIsRegistered<ICustomerCoordinateRepository, CustomerCoordinateRepository>();
            TestIsRegistered<ICutoffSawQuestionRepository, CutoffSawQuestionRepository>();
            TestIsRegistered<IDataTypeRepository, DataTypeRepository>();
            TestIsRegistered<IDivisionRepository, DivisionRepository>();
            TestIsRegistered<IDocumentDataRepository, DocumentDataRepository>();
            TestIsRegistered<IDocumentRepository, DocumentRepository>();
            TestIsRegistered<IDocumentTypeRepository, DocumentTypeRepository>();
            TestIsSecurelyRegistered<IDriversLicenseRepository, DriversLicenseRepository, DriversLicense>();
            TestIsRegistered<IEmployeeRepository, EmployeeRepository>();
            TestIsRegistered<IEmployeeStatusRepository, EmployeeStatusRepository>();
            TestIsSecurelyRegistered<IEstimatingProjectRepository, EstimatingProjectRepository, EstimatingProject>();
            TestIsRegistered<IEquipmentCharacteristicFieldRepository, EquipmentCharacteristicFieldRepository>();
            TestIsRegistered<IEquipmentRepository, EquipmentRepository>();
            TestIsRegistered<IEquipmentModelRepository, EquipmentModelRepository>();
            TestIsRegistered<IFacilityRepository, FacilityRepository>();
            TestIsRegistered<IFireDistrictRepository, FireDistrictRepository>();
            TestIsRegistered<IFunctionalLocationRepository, FunctionalLocationRepository>();
            TestIsSecurelyRegistered<IGeneralLiabilityClaimRepository, GeneralLiabilityClaimRepository,
                GeneralLiabilityClaim>();
            TestIsSecurelyRegistered<IGrievanceRepository, GrievanceRepository, Grievance>();
            TestIsSecurelyRegistered<IHepatitisBVaccinationRepository, HepatitisBVaccinationRepository,
                HepatitisBVaccination>();
            TestIsRegistered<IIconSetRepository, IconSetRepository>();
            TestIsSecurelyRegistered<IIncidentRepository, IncidentRepository, Incident>();
            TestIsSecurelyRegistered<IJobSiteCheckListRepository, JobSiteCheckListRepository, JobSiteCheckList>();
            TestIsSecurelyRegistered<ILocalRepository, LocalRepository, Local>();
            TestIsRegistered<ILockoutDeviceRepository, LockoutDeviceRepository>();
            TestIsSecurelyRegistered<ILockoutFormRepository, LockoutFormRepository, LockoutForm>();
            TestIsRegistered<IMainCrossingRepository, MainCrossingRepository>();
            TestIsRegistered<IMainCrossingInspectionAssessmentRatingRepository,
                MainCrossingInspectionAssessmentRatingRepository>();
            TestIsRegistered<IMaterialRepository, MaterialRepository>();
            TestIsSecurelyRegistered<IMarkoutDamageRepository, MarkoutDamageRepository, MarkoutDamage>();
            TestIsSecurelyRegistered<IMedicalCertificateRepository, MedicalCertificateRepository, MedicalCertificate>();
            TestIsRegistered<IMeterRepository, MeterRepository>();
            TestIsRegistered<IPlanningPlantRepository, PlanningPlantRepository>();
            TestIsRegistered<IPositionRepository, PositionRepository>();
            TestIsRegistered<INotificationConfigurationRepository, NotificationConfigurationRepository>();
            TestIsRegistered<IOneCallTicketRepository, OneCallTicketRepository>();
            TestIsRegistered<IOperatingCenterRepository, OperatingCenterRepository>();
            TestIsRegistered<IRecurringFrequencyUnitRepository, RecurringFrequencyUnitRepository>();
            TestIsRegistered<IEquipmentManufacturerRepository, EquipmentManufacturerRepository>();
            TestIsRegistered<IEquipmentTypeRepository, EquipmentTypeRepository>();
            TestIsRegistered<ISensorRepository, SensorRepository>();
            TestIsRegistered<ISensorMeasurementTypeRepository, SensorMeasurementTypeRepository>();
            TestIsRegistered<IServiceRepository, ServiceRepository>();
            TestIsRegistered<IStateRepository, StateRepository>();
            TestIsRegistered<IStreetRepository, StreetRepository>();
            TestIsRegistered<ITailgateTalkRepository, TailgateTalkRepository>();
            TestIsRegistered<ITailgateTalkTopicRepository, TailgateTalkTopicRepository>();
            TestIsRegistered<ITapImageRepository, TapImageRepository>();
            TestIsRegistered<ITrainingContactHoursProgramCoordinatorRepository,
                TrainingContactHoursProgramCoordinatorRepository>();
            TestIsRegistered<ITrainingModuleRepository, TrainingModuleRepository>();
            TestIsRegistered<ITrainingRecordRepository, TrainingRecordRepository>();
            TestIsRegistered<ITrainingSessionRepository, TrainingSessionRepository>();
            TestIsRegistered<ITownRepository, TownRepository>();
            TestIsRegistered<IUserRepository, UserRepository>();
            TestIsRegistered<IValveRepository, ValveRepository>();
            TestIsRegistered<IValveNormalPositionRepository, ValveNormalPositionRepository>();
            TestIsRegistered<IValveOpenDirectionRepository, ValveOpenDirectionRepository>();
            TestIsRegistered<IValveImageRepository, ValveImageRepository>();
            TestIsRegistered<IVehicleRepository, VehicleRepository>();
            TestIsSecurelyRegistered<IViolationCertificateRepository, ViolationCertificateRepository,
                ViolationCertificate>();
            TestIsSecurelyRegistered<IUnionContractRepository, UnionContractRepository, UnionContract>();
            TestIsSecurelyRegistered<IUnionContractProposalRepository, UnionContractProposalRepository,
                UnionContractProposal>();
            TestIsRegistered<IWorkOrderRepository, WorkOrderRepository>();
            TestIsRegistered<IUserRepository, UserRepository>();
        }

        private void TestIsRegistered<TInterface, TRepository>()
        {
            MyAssert.DoesNotThrow<StructureMapException>(
                () => Assert.IsInstanceOfType(_container.GetInstance<TInterface>(), typeof(TRepository),
                    "Type did not match expectation."),
                "Couldn't find type registration.");
        }

        private void TestIsSecurelyRegistered<TInterface, TRepository, TEntity>()
        {
            TestIsRegistered<TInterface, TRepository>();

            MyAssert.DoesNotThrow<StructureMapException>(
                () => Assert.IsInstanceOfType(_container.GetInstance<IRepository<TEntity>>(), typeof(TRepository),
                    "Type did not match expectation."),
                "Couldn't find type registration.");
        }
    }
}
