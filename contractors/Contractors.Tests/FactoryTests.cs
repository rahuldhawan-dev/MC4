using System;
using System.Threading;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.ClassExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests
{
    [TestClass]
    public class FactoryTests : InMemoryDatabaseTest<ContractorUser>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton()
             .Use<InMemoryDocumentService>();
            e.For<IAuthenticationService<ContractorUser>>().Mock();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void FactoryTestInitialize()
        {
            _container.Inject<IDocumentService>(new InMemoryDocumentService());
        }

        #endregion

        #region AssetTypeFactory

        [TestMethod]
        public void TestValveAssetTypeFactoryCreatesValveAssetType()
        {
            var assetType = GetFactory<ValveAssetTypeFactory>().Build();

            Assert.AreEqual("Valve", assetType.Description);

            assetType = GetFactory<ValveAssetTypeFactory>().Create();

            Assert.AreEqual((int)AssetTypeEnum.Valve, assetType.Id);
        }

        [TestMethod]
        public void TestValveAssetTypeFactoryReturnsValveAssetInsteadOfCreatingIfItAlreadyExists()
        {
            var expected = GetFactory<ValveAssetTypeFactory>().Create();
            var actual = GetFactory<ValveAssetTypeFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestHydrantAssetTypeFactoryCreatesHydrantAssetType()
        {
            var assetType = GetFactory<HydrantAssetTypeFactory>().Build();

            Assert.AreEqual("Hydrant", assetType.Description);

            assetType = GetFactory<HydrantAssetTypeFactory>().Create();

            Assert.AreEqual((int)AssetTypeEnum.Hydrant, assetType.Id);
        }

        [TestMethod]
        public void TestHydrantAssetTypeFactoryReturnHydrantInsteadOfCreatingIfItAlreadyExists()
        {
            var expected = GetFactory<HydrantAssetTypeFactory>().Create();
            var actual = GetFactory<HydrantAssetTypeFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMainAssetTypeFactoryCreatesMainAssetType()
        {
            var assetType = GetFactory<MainAssetTypeFactory>().Build();

            Assert.AreEqual("Main", assetType.Description);

            assetType = GetFactory<MainAssetTypeFactory>().Create();

            Assert.AreEqual((int)AssetTypeEnum.Main, assetType.Id);
        }

        [TestMethod]
        public void TestMainAssetTypeFactoryReturnsMainAssetInsteadOfCreatingItIfItAlreadyExists()
        {
            var expected = GetFactory<MainAssetTypeFactory>().Create();
            var actual = GetFactory<MainAssetTypeFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestServiceAssetTypeFactoryCreatesServiceAssetType()
        {
            var assetType = GetFactory<ServiceAssetTypeFactory>().Build();

            Assert.AreEqual("Service", assetType.Description);

            assetType = GetFactory<ServiceAssetTypeFactory>().Create();

            Assert.AreEqual((int)AssetTypeEnum.Service, assetType.Id);
        }

        [TestMethod]
        public void TestServiceAssetTypeFactoryReturnsServiceInsteadOfCreatingItIfItAlreadyExists()
        {
            var expected = GetFactory<ServiceAssetTypeFactory>().Create();
            var actual = GetFactory<ServiceAssetTypeFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ContractorFactory

        [TestMethod]
        public void TestContractorFactorySetsUpDefaultValues()
        {
            var contractor = GetFactory<ContractorFactory>().Build();

            Assert.AreEqual(ContractorFactory.DEFAULT_NAME, contractor.Name);
            Assert.AreEqual(ContractorFactory.DEFAULT_IS_UNION_SHOP, contractor.IsUnionShop);
            Assert.AreEqual(ContractorFactory.DEFAULT_IS_BCP_PARNTER, contractor.IsBcpPartner);
            Assert.AreEqual(ContractorFactory.DEFAULT_IS_ACTIVE, contractor.IsActive);
            Assert.AreEqual(ContractorFactory.DEFAULT_CREATED_BY, contractor.CreatedBy);

            contractor = GetFactory<ContractorFactory>().Create();

            Assert.AreNotEqual(0, contractor.Id);
            Assert.AreEqual(ContractorFactory.DEFAULT_NAME, contractor.Name);
            Assert.AreEqual(ContractorFactory.DEFAULT_IS_UNION_SHOP, contractor.IsUnionShop);
            Assert.AreEqual(ContractorFactory.DEFAULT_IS_BCP_PARNTER, contractor.IsBcpPartner);
            Assert.AreEqual(ContractorFactory.DEFAULT_IS_ACTIVE, contractor.IsActive);
            Assert.AreEqual(ContractorFactory.DEFAULT_CREATED_BY, contractor.CreatedBy);
        }

        #endregion

        #region ContractorUserFactory/AdminUserFactory

        [TestMethod]
        public void TestContractorUserFactoryShouldSetContractorByDefault()
        {
            var factory = GetFactory<ContractorUserFactory>();

            var user = factory.Build();

            Assert.IsNotNull(user.Contractor);

            user = factory.Create();

            Assert.AreNotEqual(0, user.Id);
            Assert.IsNotNull(user.Contractor);
            Assert.AreNotEqual(0, user.Contractor.Id);
        }

        [TestMethod]
        public void TestContractorUserFactoryShouldAllowOverrideOfContractor()
        {
            var contractor = GetFactory<ContractorFactory>().Create();
            var user = GetFactory<ContractorUserFactory>().Build(new {Contractor = contractor});

            Assert.AreSame(contractor, user.Contractor);

            user = GetFactory<ContractorUserFactory>().Create(new {Contractor = contractor});

            Assert.AreSame(contractor, user.Contractor);
            Assert.AreNotEqual(0, contractor.Id);
        }

        [TestMethod]
        public void TestAdminUserFactoryShouldCreateAdmins()
        {
            var factory = GetFactory<AdminUserFactory>();
            var admin = factory.Build();

            Assert.IsTrue(admin.IsAdmin);

            admin = factory.Create();

            Assert.AreNotEqual(0, admin.Id);
            Assert.IsTrue(admin.IsAdmin);
        }

        #endregion

        #region CrewFactory

        [TestMethod]
        public void TestShouldCreateContractorFromFactoryByDefault()
        {
            var factory = GetFactory<CrewFactory>();
            var crew = factory.Build();

            Assert.IsNotNull(crew.Contractor);

            crew = factory.Create();

            Assert.IsNotNull(crew.Contractor);
            Assert.AreNotEqual(0, crew.Contractor.Id);
        }

        [TestMethod]
        public void TestShouldAllowOverrideOfContractor()
        {
            var factory = GetFactory<CrewFactory>();
            var contractor = GetFactory<ContractorFactory>().Create();
            var crew = factory.Build(new {Contractor = contractor});

            Assert.AreEqual(contractor, crew.Contractor);

            crew = factory.Create(new {Contractor = contractor});

            Assert.AreEqual(contractor, crew.Contractor);
        }

        #endregion

        #region CrewAssignmentFactory

        [TestMethod]
        public void TestCrewAssignmentFactoryAllowsOverrideOfCrewAndWorkOrder()
        {
            var factory = GetFactory<CrewAssignmentFactory>();
            var crew = GetFactory<CrewFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create();

            var assignment = factory.Build(new {
                Crew = crew,
                WorkOrder = workOrder
            });

            Assert.AreSame(crew, assignment.Crew);
            Assert.AreSame(workOrder, assignment.WorkOrder);
            Assert.IsFalse(assignment.IsOpen);

            assignment = factory.Create(new {
                Crew = crew,
                WorkOrder = workOrder
            });

            Assert.AreNotEqual(0, assignment.Id);
            Assert.AreSame(crew, assignment.Crew);
            Assert.AreSame(workOrder, assignment.WorkOrder);
            Assert.IsFalse(assignment.IsOpen);
        }

        [TestMethod]
        public void TestOpenCrewAssignmentFactoryBuildsCrewAssignmentWithStartDate()
        {
            var factory = GetFactory<OpenCrewAssignmentFactory>();
            var assignment = factory.Build();

            MyAssert.AreClose(DateTime.Now.AddDays(-1).Date, assignment.AssignedFor);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.AssignedOn);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.DateStarted.Value);
            Assert.IsNotNull(assignment.Crew);
            Assert.IsNotNull(assignment.WorkOrder);
            Assert.IsTrue(assignment.IsOpen);

            assignment = factory.Create();

            MyAssert.AreClose(DateTime.Now.AddDays(-1).Date, assignment.AssignedFor);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.AssignedOn);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.DateStarted.Value);
            Assert.IsNotNull(assignment.Crew);
            Assert.AreNotEqual(0, assignment.Crew.Id);
            Assert.IsNotNull(assignment.WorkOrder);
            Assert.AreNotEqual(0, assignment.WorkOrder.Id);
            Assert.IsTrue(assignment.IsOpen);
        }

        [TestMethod]
        public void TestClosedCrewAssignmentFactoryBuildsCrewAssignmentWithEndDate()
        {
            var factory = GetFactory<ClosedCrewAssignmentFactory>();
            var assignment = factory.Build();

            MyAssert.AreClose(DateTime.Now.AddDays(-1).Date, assignment.AssignedFor);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.AssignedOn);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.DateStarted.Value);
            MyAssert.AreClose(DateTime.Now, assignment.DateEnded.Value);
            Assert.IsNotNull(assignment.Crew);
            Assert.IsNotNull(assignment.WorkOrder);
            Assert.IsFalse(assignment.IsOpen);
            Assert.AreEqual(
                ClosedCrewAssignmentFactory.DEFAULT_EMPLOYEES_ON_JOB,
                assignment.EmployeesOnJob);

            assignment = factory.Create();

            MyAssert.AreClose(DateTime.Now.AddDays(-1).Date, assignment.AssignedFor);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.AssignedOn);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), assignment.DateStarted.Value);
            MyAssert.AreClose(DateTime.Now, assignment.DateEnded.Value);
            Assert.IsNotNull(assignment.Crew);
            Assert.AreNotEqual(0, assignment.Crew.Id);
            Assert.IsNotNull(assignment.WorkOrder);
            Assert.AreNotEqual(0, assignment.WorkOrder.Id);
            Assert.IsFalse(assignment.IsOpen);
            Assert.AreEqual(
                ClosedCrewAssignmentFactory.DEFAULT_EMPLOYEES_ON_JOB,
                assignment.EmployeesOnJob);
        }

        #endregion

        #region DocumentFactory

        [TestMethod]
        public void TestDocumentFactorySetsDefaultValues()
        {
            var document = GetFactory<DocumentFactory>().Build();

            Assert.AreEqual(DocumentFactory.FILE_NAME, document.FileName);
            Assert.IsNotNull(document.DocumentData);

            document = GetFactory<DocumentFactory>().Create();

            Assert.AreNotEqual(0, document.Id);
            Assert.AreEqual(DocumentFactory.FILE_NAME, document.FileName);
            Assert.IsNotNull(document.DocumentData);
        }

        #endregion

        #region DocumentTypeFactory

        [TestMethod]
        public void TestDocumentTypeFactorySetsDefaultValues()
        {
            var documentType = GetFactory<DocumentTypeFactory>().Build();

            Assert.AreEqual(DocumentTypeFactory.DOCUMENT_TYPE_NAME, documentType.Name);
            Assert.AreEqual("WorkOrders", documentType.DataType.TableName);

            documentType = GetFactory<DocumentTypeFactory>().Create();

            Assert.AreNotEqual(0, documentType.Id);
            Assert.AreEqual(DocumentTypeFactory.DOCUMENT_TYPE_NAME, documentType.Name);
            Assert.AreEqual("WorkOrders", documentType.DataType.TableName);
        }

        #endregion

        #region HydrantFactory

        [TestMethod]
        public void TestHydrantFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<HydrantFactory>();
            var valve = factory.Create();

            Assert.AreNotEqual(0, valve.Id);
        }

        #endregion

        #region MainBreakFactory

        #endregion

        #region MaterialFactory

        [TestMethod]
        public void TestMaterialFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<MaterialFactory>();
            var material = factory.Build();

            Assert.AreEqual(MaterialFactory.PART_NUMBER, material.PartNumber);
            Assert.AreEqual(MaterialFactory.DESCRIPTION, material.Description);

            material = factory.Create();

            Assert.AreNotEqual(0, material.Id);
            Assert.AreEqual(MaterialFactory.PART_NUMBER, material.PartNumber);
            Assert.AreEqual(MaterialFactory.DESCRIPTION, material.Description);
        }

        #endregion

        #region MaterialUsedFactory

        [TestMethod]
        public void TestMaterialUsedFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<MaterialUsedFactory>();
            var material = factory.Build();

            Assert.IsNotNull(material.WorkOrder);

            material = factory.Create();

            Assert.IsNotNull(material.WorkOrder);
        }

        #endregion

        #region MarkoutFactory

        [TestMethod]
        public void TestMarkoutFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<MarkoutFactory>();
            var markout = factory.Build();

            Assert.AreEqual(MarkoutFactory.MARKOUT_NUMBER, markout.MarkoutNumber);

            markout = factory.Create();

            Assert.IsNotNull(markout.MarkoutType);
            Assert.AreEqual(MarkoutFactory.MARKOUT_NUMBER, markout.MarkoutNumber);
        }

        [TestMethod]
        public void TestMarkoutFactoryAllowsOverrideOfMarkoutTypeAndWorkOrder()
        {
            var factory = GetFactory<MarkoutFactory>();
            var markoutType = GetFactory<MarkoutTypeFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create();

            var markout = factory.Build(new {
                MarkoutType = markoutType,
                WorkOrder = workOrder
            });

            Assert.AreSame(markoutType, markout.MarkoutType);
            Assert.AreSame(workOrder, markout.WorkOrder);

            markout = factory.Create(new {
                MarkoutType = markoutType,
                WorkOrder = workOrder
            });

            Assert.AreNotEqual(0, markout.Id);
            Assert.AreSame(markoutType, markout.MarkoutType);
            Assert.AreSame(workOrder, markout.WorkOrder);
        }

        #endregion

        #region MarkoutRequirementFactory

        [TestMethod]
        public void TestMarkoutRequirementNoneFactoryCreatesMarkoutRequirementNone()
        {
            var actual = GetFactory<MarkoutRequirementNoneFactory>().Build();

            Assert.AreEqual("None", actual.Description);

            actual = GetFactory<MarkoutRequirementNoneFactory>().Create();

            Assert.AreEqual("None", actual.Description);
            Assert.AreEqual((int)MarkoutRequirementEnum.None, actual.Id);
        }

        [TestMethod]
        public void TestMarkoutRequirementRoutineCreatesMarkoutRequirementRoutine()
        {
            var actual = GetFactory<MarkoutRequirementRoutineFactory>().Build();

            Assert.AreEqual("Routine", actual.Description);

            actual = GetFactory<MarkoutRequirementRoutineFactory>().Create();

            Assert.AreEqual("Routine", actual.Description);
            Assert.AreEqual((int)MarkoutRequirementEnum.Routine, actual.Id);
        }

        [TestMethod]
        public void TestMarkoutRequirementEmergencyCreatesMarkoutRequirementEmergency()
        {
            var actual = GetFactory<MarkoutRequirementEmergencyFactory>().Build();

            Assert.AreEqual("Emergency", actual.Description);

            actual = GetFactory<MarkoutRequirementEmergencyFactory>().Create();

            Assert.AreEqual("Emergency", actual.Description);
            Assert.AreEqual((int)MarkoutRequirementEnum.Emergency, actual.Id);
        }


        [TestMethod]
        public void TestMarkoutRequirementNoneFactoryReturnsMarkoutRequirementNoneInsteadOfCreatingIfItAlreadyExists()
        {
            var expected = GetFactory<MarkoutRequirementNoneFactory>().Create();
            var actual = GetFactory<MarkoutRequirementNoneFactory>().Create();

            Assert.AreEqual(expected, actual);
        }
        
        #endregion

        #region MarkoutTypeFactory

        [TestMethod]
        public void TestMarkoutTypeFactorySetsDefaultValues()
        {
            var markoutType = GetFactory<MarkoutTypeFactory>().Build();

            Assert.AreEqual(MarkoutTypeFactory.DEFAULT_DESCRIPTION, markoutType.Description);

            markoutType = GetFactory<MarkoutTypeFactory>().Create();

            Assert.AreNotEqual(0, markoutType.Id);
            Assert.AreEqual(MarkoutTypeFactory.DEFAULT_DESCRIPTION, markoutType.Description);
        }

        #endregion

        #region OperatingCenterFactory

        [TestMethod]
        public void TestOperatingCenterFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<OperatingCenterFactory>();
            var opCntr = factory.Build();

            Assert.AreEqual(OperatingCenterFactory.DEFAULT_OP_CNTR, opCntr.OperatingCenterCode);
            Assert.AreEqual(OperatingCenterFactory.DEFAULT_OP_CNTR_NAME, opCntr.OperatingCenterName);
            Assert.AreEqual(OperatingCenterFactory.DEFAULT_WORK_ORDERS_ENABLED, opCntr.WorkOrdersEnabled);

            opCntr = factory.Create();

            Assert.AreNotEqual(0, opCntr.Id);
            Assert.AreEqual(OperatingCenterFactory.DEFAULT_OP_CNTR, opCntr.OperatingCenterCode);
            Assert.AreEqual(OperatingCenterFactory.DEFAULT_OP_CNTR_NAME, opCntr.OperatingCenterName);
            Assert.AreEqual(OperatingCenterFactory.DEFAULT_WORK_ORDERS_ENABLED, opCntr.WorkOrdersEnabled);
        }

        #endregion

        #region RequisitionFactory

        [TestMethod]
        public void TestRequisitionFactoryCreatesAndSaves()
        {
            var factory = GetFactory<RequisitionFactory>();
            var result = factory.Create();

            Assert.IsTrue(result.WorkOrder.Requisitions.Contains(result));
            Assert.IsNotNull(result.RequisitionType);
        }

        #endregion

        #region RequisitionTypeFactory

        [TestMethod]
        public void TestRequisitionTypeFactoryBuildsAndCreates()
        {
            var factory = GetFactory<RequisitionTypeFactory>();
            var built = factory.Build();
            Assert.AreEqual(RequisitionTypeFactory.DEFAULT_DESCRIPTION,
                built.Description);

            var created = factory.Create();
            Assert.AreEqual(RequisitionTypeFactory.DEFAULT_DESCRIPTION,
                created.Description);
            Assert.AreNotEqual(0, created.Id);
        }
        
        #endregion

        #region Restoration

        [TestMethod]
        public void TestRestorationFactorySetsUpDefaultValues()
        {
            var restoration = GetFactory<RestorationFactory>().Build();

            Assert.IsNotNull(restoration.ResponsePriority);
            Assert.IsNotNull(restoration.RestorationType);
            
            restoration = GetFactory<RestorationFactory>().Create();

            Assert.AreNotEqual(0, restoration.Id);
            Assert.IsNotNull(restoration.ResponsePriority);
            Assert.IsNotNull(restoration.RestorationType);
            Assert.AreEqual(RestorationFactory.LINEAR_FEET_OF_CURB,
                restoration.LinearFeetOfCurb);
            Assert.IsFalse(restoration.EightInchStabilizeBaseByCompanyForces);
            //Assert.IsFalse(restoration.SawCutByCompanyForces);
        }
        
        #endregion

        #region RestorationType

        [TestMethod]
        public void TestRestorationTypeFactorySetsDefaultValues()
        {
            var restorationType = GetFactory<RestorationTypeFactory>().Build();

            //Assert.AreEqual(RestorationTypeFactory.DEFAULT_DESCRIPTION,
            //    restorationType.Description);
            Assert.AreEqual(RestorationMeasurementTypes.SquareFt,
                restorationType.MeasurementType);

            restorationType = GetFactory<RestorationTypeFactory>().Create();

            Assert.AreNotEqual(0, restorationType.Id);
            //Assert.AreEqual(RestorationTypeFactory.DEFAULT_DESCRIPTION,
            //    restorationType.Description);
            Assert.AreEqual(RestorationMeasurementTypes.SquareFt,
                restorationType.MeasurementType);
        }

        #endregion

        #region RestorationTypeCost

        [TestMethod]
        public void TestRestorationTypeCostFactorySetsDefaultValues()
        {
            var restorationTypeCost =
                GetFactory<RestorationTypeCostFactory>().Build();

            Assert.AreEqual(RestorationTypeCostFactory.DEFAULT_COST, restorationTypeCost.Cost);

            restorationTypeCost =
                GetFactory<RestorationTypeCostFactory>().Create();

            Assert.AreNotEqual(0, restorationTypeCost.Id);
            Assert.AreEqual(RestorationTypeCostFactory.DEFAULT_COST, restorationTypeCost.Cost);

        }

        #endregion

        #region RestorationResponsePriority

        [TestMethod]
        public void TestRestorationResponsePriorityFactorySetsDefaultValues()
        {
            var restorationResponsePriority = GetFactory<RestorationResponsePriorityFactory>().Build();

            Assert.AreEqual(RestorationResponsePriorityFactory.DEFAULT_DESCRIPTION,
                restorationResponsePriority.Description);

            restorationResponsePriority = GetFactory<RestorationResponsePriorityFactory>().Create();

            Assert.AreNotEqual(0, restorationResponsePriority.Id);
            Assert.AreEqual(RestorationResponsePriorityFactory.DEFAULT_DESCRIPTION,
                restorationResponsePriority.Description);
        }

        #endregion

        #region SewerOpeningFactory

        [TestMethod]
        public void TestSewerOpeningFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<SewerOpeningFactory>();
            var valve = factory.Create();

            Assert.AreNotEqual(0, valve.Id);
        }

        #endregion

        #region SpoilFactory

        [TestMethod]
        public void TestSpoilFactorySetsDefaultValues()
        {
            var spoil = GetFactory<SpoilFactory>().Build();
            Assert.AreEqual(SpoilFactory.DEFAULT_QUANTITY, spoil.Quantity);
            spoil = GetFactory<SpoilFactory>().Create();
            Assert.AreNotEqual(0, spoil.Id);
            Assert.IsNotNull(spoil.SpoilStorageLocation);
            Assert.IsNotNull(spoil.WorkOrder);
            Assert.AreEqual(SpoilFactory.DEFAULT_QUANTITY, spoil.Quantity);
        }

        #endregion

        #region SpoilStorageLocationFactory

        [TestMethod]
        public void TestSpoilStorageLocationFactorySetsDefaultValues()
        {
            var spoil = GetFactory<SpoilStorageLocationFactory>().Build();
            Assert.AreEqual(SpoilStorageLocationFactory.NAME, spoil.Name);
            spoil = GetFactory<SpoilStorageLocationFactory>().Create();
            Assert.AreNotEqual(0, spoil.Id);
            Assert.IsNotNull(spoil.OperatingCenter);
            Assert.AreEqual(SpoilStorageLocationFactory.NAME, spoil.Name);
        }

        #endregion

        #region SpoilFinalProcessingLocationFactory

        [TestMethod]
        public void TestSpoilFinalProcessingLocationFactorySetsDefaultValues()
        {
            var spoil = GetFactory<SpoilFinalProcessingLocationFactory>().Build();
            Assert.AreEqual(SpoilFinalProcessingLocationFactory.NAME, spoil.Name);
            spoil = GetFactory<SpoilFinalProcessingLocationFactory>().Create();
            Assert.AreNotEqual(0, spoil.Id);
            Assert.IsNotNull(spoil.OperatingCenter);
            Assert.AreEqual(SpoilFinalProcessingLocationFactory.NAME, spoil.Name);
        }

        #endregion

        #region StockLocationFactory

        [TestMethod]
        public void TestStockLocationFactorySetsUpDefaultViews()
        {
            var factory = GetFactory<StockLocationFactory>();
            var stockLocation = factory.Create();

            Assert.AreNotEqual(0, stockLocation.Id);
            Assert.IsNotNull(stockLocation.OperatingCenter);
            Assert.AreEqual(StockLocationFactory.DESCRIPTION, stockLocation.Description);
        }

        #endregion

        #region StormCatchFactory

        [TestMethod]
        public void TestStormCatchFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<StormCatchFactory>();
            var valve = factory.Create();

            Assert.AreNotEqual(0, valve.Id);
        }

        #endregion

        #region StreetOpeningPermitFactory

        [TestMethod]
        public void TestStreetOpeningPermitFactoryAllowsOverrideOfWorkOrder()
        {
            var factory = GetFactory<StreetOpeningPermitFactory>();
            var workOrder = GetFactory<WorkOrderFactory>().Create();

            var sop = factory.Build(new {WorkOrder = workOrder});

            Assert.AreSame(workOrder, sop.WorkOrder);

            sop = factory.Create(new {WorkOrder = workOrder});

            Assert.AreNotEqual(0, sop.Id);
            Assert.AreSame(workOrder, sop.WorkOrder);
        }
        
        #endregion

        #region TownFactory

        [TestMethod]
        public void TestTownFactorySetsDefaultValues()
        {
            var town = GetFactory<TownFactory>().Build();

            Assert.AreEqual(TownFactory.DEFAULT_TOWN_TEXT, town.ShortName);

            town = GetFactory<TownFactory>().Create();

            Assert.AreNotEqual(0, town.Id);
            Assert.AreEqual(TownFactory.DEFAULT_TOWN_TEXT, town.ShortName);
        }

        #endregion

        #region TownSectionFactory

        [TestMethod]
        public void TestTownSectionFactorySetsDefaultValues()
        {
            var townSection = GetFactory<TownSectionFactory>().Build();

            Assert.AreEqual(TownSectionFactory.DEFAULT_TOWN_SECTION_TEXT, townSection.Name);

            townSection = GetFactory<TownSectionFactory>().Create();

            Assert.AreNotEqual(0, townSection.Id);
            Assert.IsNotNull(townSection.Town);
            Assert.AreEqual(TownSectionFactory.DEFAULT_TOWN_SECTION_TEXT, townSection.Name);
        }

        [TestMethod]
        public void TestTownSectionFactoryAllowsOverrideOfTown()
        {
            var factory = GetFactory<TownSectionFactory>();
            var town = GetFactory<TownFactory>().Create();

            var townSection = factory.Build(new {Town = town});

            Assert.AreSame(town, townSection.Town);

            townSection = factory.Create(new {Town = town});

            Assert.AreNotEqual(0, townSection.Id);
            Assert.AreSame(town, townSection.Town);

        }

        #endregion

        #region ValveFactory

        [TestMethod]
        public void TestValveFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<ValveFactory>();
            var valve = factory.Create();

            Assert.AreNotEqual(0, valve.Id);
        }

        #endregion

        #region ValveImageFactory

        [TestMethod]
        public void TestValveImageFactorySetsDefaultValues()
        {
            var now = DateTime.Now;
            var valveImage = GetFactory<ValveImageFactory>().Build(new{ CreatedAt = now });
         
            MyAssert.AreClose(valveImage.CreatedAt, DateTime.Now, new TimeSpan(0,0,0,10));

            valveImage = GetFactory<ValveImageFactory>().Create(new { CreatedAt = now });

            Assert.AreNotEqual(0, valveImage.Id);
            MyAssert.AreClose(valveImage.CreatedAt, DateTime.Now, new TimeSpan(0,0,0,10));
        }

        #endregion

        #region WorkDescriptionFactory

        [TestMethod]
        public void TestValveWorkDescriptionFactorySetsDefaultValues()
        {
            var workDescription =
                GetFactory<ValveWorkDescriptionFactory>().Build();

            Assert.AreEqual(ValveWorkDescriptionFactory.DEFAULT_DESCRIPTION, workDescription.Description);
            Assert.IsNotNull(workDescription.AssetType);
            Assert.AreEqual("Valve", workDescription.AssetType.Description);

            workDescription = GetFactory<ValveWorkDescriptionFactory>().Create();

            Assert.AreEqual(ValveWorkDescriptionFactory.DEFAULT_DESCRIPTION, workDescription.Description);
            Assert.IsNotNull(workDescription.AssetType);
            Assert.AreEqual("Valve", workDescription.AssetType.Description);
        }

        [TestMethod]
        public void TestValveBoxRepairWorkDescriptionFactorySetsDefaultValues()
        {
            var workDescription = GetFactory<ValveBoxRepairWorkDescriptionFactory>().Build();

            Assert.AreEqual(ValveBoxRepairWorkDescriptionFactory.VALVE_BOX_DEFAULT_DESCRIPTION, workDescription.Description);
            Assert.IsNotNull(workDescription.AssetType);

            workDescription = GetFactory<ValveBoxRepairWorkDescriptionFactory>().Create();

            Assert.AreNotEqual(0, workDescription.Id, "Id is 0");
            Assert.AreEqual(ValveBoxRepairWorkDescriptionFactory.VALVE_BOX_DEFAULT_DESCRIPTION, workDescription.Description);
            Assert.IsNotNull(workDescription.AssetType);
            Assert.AreNotEqual(0, workDescription.AssetType.Id);
        }

        [TestMethod]
        public void TestValveBoxRepairWorkDescriptionFactoryReturnsCorrectDescriptionInsteadOfCreatingIfItAlreadyExists()
        {
            var expected = GetFactory<ValveBoxRepairWorkDescriptionFactory>().Create();
            var actual = GetFactory<ValveBoxRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMainBreakWorkDescriptionFactorySetsDefaultValues()
        {
            var wd = GetFactory<MainBreakRepairWorkDescriptionFactory>().Build();

            Assert.AreEqual(MainBreakRepairWorkDescriptionFactory.DEFAULT_DESCRIPTION, wd.Description);

            wd = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create();
            
            Assert.AreEqual((int)WorkDescriptionEnum.WaterMainBreakRepair, wd.Id);
        }

        [TestMethod]
        public void TestMainBreakWorkDescriptionFactoryReturnsCorrectDescriptionInsteadOfCreatingItIfItAlreadyExists()
        {
            var expected = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create();
            var actual = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFrozenHydrantWorkDescriptionFactorySetsDefaultValues()
        {
            var wd = GetFactory<FrozenHydrantWorkDescriptionFactory>().Build();

            Assert.AreEqual(FrozenHydrantWorkDescriptionFactory.DEFAULT_DESCRIPTION, wd.Description);

            wd = GetFactory<FrozenHydrantWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescriptionEnum.FrozenHydrant, wd.Id);
        }

        [TestMethod]
        public void TestFrozenHydrantWorkDescriptionFactoryReturnsCorrectDescriptionInsteadOfCreatingItIfItAlreadyExists()
        {
            var expected = GetFactory<FrozenHydrantWorkDescriptionFactory>().Create();
            var actual = GetFactory<FrozenHydrantWorkDescriptionFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCurbBoxRepairWorkDescriptionFactorySetsDefaultValues()
        {
            var wd = GetFactory<CurbBoxRepairWorkDescriptionFactory>().Build();

            Assert.AreEqual(
                CurbBoxRepairWorkDescriptionFactory.DEFAULT_DESCRIPTION,
                wd.Description);

            wd = GetFactory<CurbBoxRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescriptionEnum.CurbBoxRepair,
                wd.Id);
        }

        [TestMethod]
        public void TestCurbBoxRepairWorkDescriptionFactoryReturnsCorrectDescriptionsInsteadOfCreatingIfItAlreadyExists()
        {
            var expected =
                GetFactory<CurbBoxRepairWorkDescriptionFactory>().Create();
            var actual =
                GetFactory<CurbBoxRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual(expected, actual);
        }

        #endregion
        
        #region WorkOrderFactory

        [TestMethod]
        public void TestWorkOrderFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<WorkOrderFactory>();
            var workOrder = factory.Build();

            Assert.AreEqual(WorkOrderFactory.DEFAULT_STREET_NUMBER, workOrder.StreetNumber);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_ACCOUNT_CHARGED, workOrder.AccountCharged);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_PREMISE_NUMBER, workOrder.PremiseNumber);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_PHONE_NUMBER, workOrder.PhoneNumber);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_ORCOM_SERVICE_ORDER_NUMBER, workOrder.ORCOMServiceOrderNumber);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_NOTES, workOrder.Notes);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_LATITUDE, workOrder.Latitude);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_LONGITUDE, workOrder.Longitude);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_LOST_WATER, workOrder.LostWater);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_EXCAVATION_DATE, workOrder.ExcavationDate);
            MyAssert.AreClose(DateTime.Now, workOrder.DateReceived.Value, new TimeSpan(0, 1, 0));
            MyAssert.AreClose(DateTime.Now, workOrder.CreatedAt, new TimeSpan(0, 1, 0));
            Assert.IsNotNull(workOrder.CreatedBy);
            Assert.IsNotNull(workOrder.Town);
            Assert.IsNotNull(workOrder.RequestedBy);
            Assert.IsNotNull(workOrder.Priority);
            Assert.IsNotNull(workOrder.Purpose);
            Assert.IsNotNull(workOrder.AssetType);
            Assert.IsNotNull(workOrder.WorkDescription);
            Assert.IsNotNull(workOrder.MarkoutRequirement);
            Assert.IsNotNull(workOrder.OperatingCenter);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_TRAFFIC_CONTROL_REQUIRED, workOrder.TrafficControlRequired);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_STREET_OPENING_PERMIT_REQUIRED, workOrder.StreetOpeningPermitRequired);

            workOrder = factory.Create();

            Assert.AreEqual(WorkOrderFactory.DEFAULT_STREET_NUMBER, workOrder.StreetNumber);
            MyAssert.AreClose(DateTime.Now, workOrder.DateReceived.Value, new TimeSpan(0, 1, 0));
            MyAssert.AreClose(DateTime.Now, workOrder.CreatedAt, new TimeSpan(0, 1, 0));
            Assert.IsNotNull(workOrder.CreatedBy);
            Assert.AreNotEqual(0, workOrder.CreatedBy.Id);
            Assert.IsNotNull(workOrder.Town);
            Assert.AreNotEqual(0, workOrder.Town.Id);
            Assert.IsNotNull(workOrder.RequestedBy);
            Assert.AreNotEqual(0, workOrder.RequestedBy.Id);
            Assert.IsNotNull(workOrder.Priority);
            Assert.AreNotEqual(0, workOrder.Priority.Id);
            Assert.IsNotNull(workOrder.Purpose);
            Assert.AreNotEqual(0, workOrder.Purpose.Id);
            Assert.IsNotNull(workOrder.AssetType);
            Assert.AreNotEqual(0, workOrder.AssetType.Id);
            Assert.IsNotNull(workOrder.WorkDescription);
            Assert.AreNotEqual(0, workOrder.WorkDescription.Id);
            Assert.IsNotNull(workOrder.MarkoutRequirement);
            Assert.AreNotEqual(0, workOrder.MarkoutRequirement.Id);
            Assert.IsNotNull(workOrder.OperatingCenter);
            Assert.AreNotEqual(0, workOrder.OperatingCenter.Id);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_TRAFFIC_CONTROL_REQUIRED, workOrder.TrafficControlRequired);
            Assert.AreEqual(WorkOrderFactory.DEFAULT_STREET_OPENING_PERMIT_REQUIRED, workOrder.StreetOpeningPermitRequired);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithAnExpiredMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory>().Create(
                    new { AssignedContractor = currentUser.Contractor });

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(-10), order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithAValidFutureMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>().Create(
                    new { AssignedContractor = currentUser.Contractor });

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(1), order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(10), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithAValidMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create(
                    new { AssignedContractor = currentUser.Contractor });

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now, order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(10), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory>().Create(
                    new { AssignedContractor = currentUser.Contractor });

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(-14), order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(-10), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var order = GetFactory<MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory>().Create();

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now, order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(10), order.Markouts[0].ExpirationDate.Value);
        }

        #endregion

        #region WorkOrderPriorityFactory

        [TestMethod]
        public void TestWorkOrderPriorityFactoryThrowsExceptionOnBuildAndCreate()
        {
            MyAssert.Throws<InvalidOperationException>(() => GetFactory<WorkOrderPriorityFactory>().Build());

            MyAssert.Throws<InvalidOperationException>(() => GetFactory<WorkOrderPriorityFactory>().Create());
        }

        [TestMethod]
        public void TestEmergencyWorkOrderPriorityFactoryCreatesEmergencyWorkOrderPriority()
        {
            var workOrderPriority = GetFactory<EmergencyWorkOrderPriorityFactory>().Build();

            Assert.AreEqual("Emergency", workOrderPriority.Description);

            workOrderPriority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();

            Assert.AreEqual("Emergency", workOrderPriority.Description);
            Assert.AreEqual((int)WorkOrderPriorityEnum.Emergency, workOrderPriority.Id);
        }

        //HighPriority
        [TestMethod]
        public void TestHighPriorityWorkOrderPriorityFactoryCreatesHighPriorityWorkOrderPriority()
        {
            var workOrderPriority = GetFactory<HighPriorityWorkOrderPriorityFactory>().Build();

            Assert.AreEqual("High Priority", workOrderPriority.Description);

            workOrderPriority = GetFactory<HighPriorityWorkOrderPriorityFactory>().Create();

            Assert.AreEqual("High Priority", workOrderPriority.Description);
            Assert.AreEqual((int)WorkOrderPriorityEnum.HighPriority, workOrderPriority.Id);
        }

        //Routine
        [TestMethod]
        public void TestRoutineWorkOrderPriorityFactoryCreatesRoutineWorkOrderPriority()
        {
            var workOrderPriority = GetFactory<RoutineWorkOrderPriorityFactory>().Build();

            Assert.AreEqual("Routine", workOrderPriority.Description);

            workOrderPriority = GetFactory<RoutineWorkOrderPriorityFactory>().Create();

            Assert.AreEqual("Routine", workOrderPriority.Description);
            Assert.AreEqual((int)WorkOrderPriorityEnum.Routine, workOrderPriority.Id);
        }

        #endregion
    }
}
