using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Documents;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;
using UserFactory = MapCall.Common.Testing.Data.UserFactory;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCall.CommonTest.Model.TestData
{
    [TestClass]
    public class FactoryTests : InMemoryDatabaseTest<User>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authenticationService;

        #endregion


        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use(ctx => ctx.GetInstance<InMemoryDocumentService>());
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(FacilityFactory).Assembly);
        }

        // ReSharper disable once UnusedField.Compiler
        private System.Data.SQLite.SQLiteException _doNotUseThisException;

        #endregion

        // Alphabetical Please

        #region AbsenceNotification

        [TestMethod]
        public void TestAbsenceNotificationFactorySetsDefaults()
        {
            var target = GetFactory<AbsenceNotificationFactory>().Build();

            MyAssert.AreClose(DateTime.Now, target.CreatedAt);
            Assert.IsFalse(target.HumanResourcesReviewed);
            Assert.IsNotNull(target.SubmittedBy);

            target = GetFactory<AbsenceNotificationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            MyAssert.AreClose(DateTime.Now, target.CreatedAt);
            Assert.IsFalse(target.HumanResourcesReviewed);
            Assert.IsNotNull(target.SubmittedBy);
        }

        #endregion

        #region ActionFactory

        [TestMethod]
        public void TestActionFactoryBuildsAndCreatesActionWithIdSetFromActionsEnum()
        {
            var factory = GetFactory<ActionFactory>();

            var built = factory.Build(new {Id = RoleActions.Edit});
            Assert.AreEqual((int)RoleActions.Edit, built.Id);

            var created = factory.Create(new {Id = RoleActions.Edit});
            Session.Flush();
            Assert.AreEqual((int)RoleActions.Edit, created.Id);
        }

        [TestMethod]
        public void TestActionFactorySetsNameFromActionsEnumIfOneIsNotProvided()
        {
            var factory = GetFactory<ActionFactory>();
            var app = factory.Create(new {
                Id = RoleActions.Edit
            });
            Assert.AreEqual("Edit", app.Name);
        }

        [TestMethod]
        public void TestActionFactoryCreateReturnsExistingDatabaseInstanceIfOneExistsForTheId()
        {
            var factory = GetFactory<ActionFactory>();
            var first = factory.Create(new {Id = RoleActions.Delete});
            var second = factory.Create(new {Id = RoleActions.Delete});
            Assert.AreSame(first, second);
            var third = factory.Create(new {Id = RoleActions.Read});
            Assert.AreNotSame(first, third);
        }

        [TestMethod]
        public void TestOtherActionFactoryMakesTheirAssignedActions()
        {
            Assert.AreEqual((int)RoleActions.Read, GetFactory<ReadActionFactory>().Create().Id, "Should be Read");
            Assert.AreEqual((int)RoleActions.Edit, GetFactory<EditActionFactory>().Create().Id, "Should be Edit");
            Assert.AreEqual((int)RoleActions.Add, GetFactory<AddActionFactory>().Create().Id, "Should be Add");
            Assert.AreEqual((int)RoleActions.Delete, GetFactory<DeleteActionFactory>().Create().Id, "Should be Delete");
            Assert.AreEqual((int)RoleActions.UserAdministrator, GetFactory<AdminActionFactory>().Create().Id,
                "Should be UserAdministrator.");
        }

        #endregion

        #region AddressFactory

        [TestMethod]
        public void TestAddressFactoryCreatesWithDefaults()
        {
            var addy = GetFactory<AddressFactory>().Create();
            Assert.AreEqual(AddressFactory.DEFAULT_ADDRESS_1, addy.Address1);
            Assert.AreEqual(AddressFactory.DEFAULT_ZIP_CODE, addy.ZipCode);
            Assert.IsNotNull(addy.Town);
        }

        #endregion

        #region ApplicationFactory

        [TestMethod]
        public void TestApplicationFactoryBuildsAndCreatesApplicationWithIdSetFromRoleApplicationsEnum()
        {
            var factory = GetFactory<ApplicationFactory>();

            var built = factory.Build(new {Id = RoleApplications.BPU});
            Assert.AreEqual((int)RoleApplications.BPU, built.Id);

            var created = factory.Create(new {Id = RoleApplications.BPU});
            Session.Flush();
            Assert.AreEqual((int)RoleApplications.BPU, created.Id);
        }

        [TestMethod]
        public void TestApplicationFactorySetsNameFromRoleApplicationsEnumIfOneIsNotProvided()
        {
            var factory = GetFactory<ApplicationFactory>();
            var app = factory.Create(new {
                Id = RoleApplications.BPU
            });
            Assert.AreEqual("BPU", app.Name);
        }

        [TestMethod]
        public void TestApplicationFactoryCreateReturnsExistingDatabaseInstanceIfOneExistsForTheId()
        {
            var factory = GetFactory<ApplicationFactory>();
            var first = factory.Create(new {Id = RoleApplications.BPU});
            var second = factory.Create(new {Id = RoleApplications.BPU});
            Assert.AreSame(first, second);
            var third = factory.Create(new {Id = RoleApplications.Events});
            Assert.AreNotSame(first, third);
        }

        #endregion

        #region AsBuiltImageFactory

        [TestMethod]
        public void TestAsBuiltImageFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<AsBuiltImageFactory>();
            var asBuiltImage = factory.Build();

            MyAssert.AreClose(Lambdas.GetNow(), asBuiltImage.CreatedAt);
            MyAssert.AreClose(Lambdas.GetNow(), asBuiltImage.CoordinatesModifiedOn.Value);

            asBuiltImage = factory.Create();

            Assert.AreNotEqual(0, asBuiltImage.Id);
            Assert.IsNotNull(asBuiltImage.Town);
            MyAssert.AreClose(Lambdas.GetNow(), asBuiltImage.CreatedAt);
            MyAssert.AreClose(Lambdas.GetNow(), asBuiltImage.CoordinatesModifiedOn.Value);
        }

        #endregion

        #region AssetInvestmentCategory

        [TestMethod]
        public void TestAssetInvestmentCategoryFactorySetsDefaults()
        {
            var target = GetFactory<AssetInvestmentCategoryFactory>().Build();

            Assert.AreEqual(AssetInvestmentCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(AssetInvestmentCategoryFactory.CREATED_BY, target.CreatedBy);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);

            target = GetFactory<AssetInvestmentCategoryFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetInvestmentCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(AssetInvestmentCategoryFactory.CREATED_BY, target.CreatedBy);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);
        }

        #endregion

        #region AssetReliabilityTechnologyUsedType

        [TestMethod]
        public void TestInfraredThermographyAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.INFRARED_THERMOGRAPHY, target.Id);
            Assert.AreEqual(InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestVibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.VIBRATION_ANALYSIS, target.Id);
            Assert.AreEqual(VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestMotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.MOTOR_WINDING_ANALYSIS_INSULATION_RESISTANCE, target.Id);
            Assert.AreEqual(MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestVisualInspectionAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<VisualInspectionAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(VisualInspectionAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<VisualInspectionAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.VISUAL_INSPECTION, target.Id);
            Assert.AreEqual(VisualInspectionAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestAirborneAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.AIRBORNE_ULTRASOUND, target.Id);
            Assert.AreEqual(AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestLaserAlignmentAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.LASER_ALIGNMENT, target.Id);
            Assert.AreEqual(LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestEarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.EARTH_GROUND_TESTING, target.Id);
            Assert.AreEqual(EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestElectricalTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.ELECTRICAL_TESTING, target.Id);
            Assert.AreEqual(ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestWireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.WIRE_TO_WATER_PUMP_PERFORMANCE, target.Id);
            Assert.AreEqual(WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestMotionAmplificationAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.MOTION_AMPLIFICATION, target.Id);
            Assert.AreEqual(MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.PROTECTIVE_RELAY_TESTING, target.Id);
            Assert.AreEqual(ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestOtherAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<OtherAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(OtherAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<OtherAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.OTHER, target.Id);
            Assert.AreEqual(OtherAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestBatteryTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<BatteryTestingAssetReliabilityTechnologyUsedTypeFactory>().Build();
            Assert.AreEqual(BatteryTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<BatteryTestingAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.BATTERY_TESTING, target.Id);
            Assert.AreEqual(BatteryTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestDynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory>().Build();

            Assert.AreEqual(DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.DYNAMIC_MOTOR_TESTING_ESA, target.Id);
            Assert.AreEqual(DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestMicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            var target = GetFactory<MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory>().Build();

            Assert.AreEqual(MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetReliabilityTechnologyUsedType.Indices.MICRO_OHMMETER_TESTING, target.Id);
            Assert.AreEqual(MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region AssetType

        [TestMethod]
        public void TestEquipmentAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentAssetTypeFactory>().Build();
            Assert.AreEqual(EquipmentAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EquipmentAssetTypeFactory>().Create();

            Assert.AreEqual(AssetType.Indices.EQUIPMENT, target.Id);
            Assert.AreEqual(EquipmentAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestFacilityAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<FacilityAssetTypeFactory>().Build();
            Assert.AreEqual(FacilityAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<FacilityAssetTypeFactory>().Create();

            Assert.AreEqual(AssetType.Indices.FACILITY, target.Id);
            Assert.AreEqual(FacilityAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestHydrantAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<HydrantAssetTypeFactory>().Build();

            Assert.AreEqual(HydrantAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<HydrantAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.HYDRANT, target.Id);
            Assert.AreEqual(HydrantAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestMainAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<MainAssetTypeFactory>().Build();

            Assert.AreEqual(MainAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<MainAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.MAIN, target.Id);
            Assert.AreEqual(MainAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestServiceAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<ServiceAssetTypeFactory>().Build();

            Assert.AreEqual(ServiceAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ServiceAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.SERVICE, target.Id);
            Assert.AreEqual(ServiceAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestSewerOpeningAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<SewerOpeningAssetTypeFactory>().Build();

            Assert.AreEqual(SewerOpeningAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<SewerOpeningAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.SEWER_OPENING, target.Id);
            Assert.AreEqual(SewerOpeningAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestSewerLateralAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<SewerLateralAssetTypeFactory>().Build();

            Assert.AreEqual(SewerLateralAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<SewerLateralAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.SEWER_LATERAL, target.Id);
            Assert.AreEqual(SewerLateralAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestSewerMainAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<SewerMainAssetTypeFactory>().Build();

            Assert.AreEqual(SewerMainAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<SewerMainAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.SEWER_MAIN, target.Id);
            Assert.AreEqual(SewerMainAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestStormCatchAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<StormCatchAssetTypeFactory>().Build();

            Assert.AreEqual(StormCatchAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<StormCatchAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.STORM_CATCH, target.Id);
            Assert.AreEqual(StormCatchAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestValveAssetTypeFactorySetsDefaults()
        {
            var target = GetFactory<ValveAssetTypeFactory>().Build();

            Assert.AreEqual(ValveAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ValveAssetTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AssetType.Indices.VALVE, target.Id);
            Assert.AreEqual(ValveAssetTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region AuditLogEntry

        [TestMethod]
        public void TestAuditLogEntryFactorySetsDefaults()
        {
            var target = GetFactory<AuditLogEntryFactory>().Build();
            var ts = TimeSpan.FromMinutes(10);

            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_AUDIT_ENTRY_TYPE, target.AuditEntryType);
            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_ENTITY_ID, target.EntityId);
            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_ENTITY_NAME, target.EntityName);
            MyAssert.AreClose(DateTime.Now, target.Timestamp, ts);
            Assert.IsNotNull(target.User);

            var hydrant = GetFactory<HydrantFactory>().Create();
            target = GetFactory<AuditLogEntryFactory>().Create(new {EntityId = hydrant.Id, EntityName = "Hydrants"});

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_AUDIT_ENTRY_TYPE, target.AuditEntryType);
            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_ENTITY_ID, target.EntityId);
            Assert.AreEqual("Hydrants", target.EntityName);
            MyAssert.AreClose(DateTime.Now, target.Timestamp, ts);
            Assert.IsNotNull(target.User);

            var valve = GetFactory<ValveFactory>().Create();
            target = GetFactory<AuditLogEntryFactory>().Create(new {EntityId = valve.Id, EntityName = "Valves"});

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_AUDIT_ENTRY_TYPE, target.AuditEntryType);
            Assert.AreEqual(AuditLogEntryFactory.DEFAULT_ENTITY_ID, target.EntityId);
            Assert.AreEqual("Valves", target.EntityName);
            MyAssert.AreClose(DateTime.Now, target.Timestamp, ts);
            Assert.IsNotNull(target.User);
        }

        #endregion

        #region BacterialWaterSample

        [TestMethod]
        public void TestBacterialWaterSampleFactorySetsDefaults()
        {
            var target = GetFactory<BacterialWaterSampleFactory>().Build();

            Assert.AreEqual(BacterialWaterSampleFactory.DEFAULT_CL2_FREE, target.Cl2Free);
            Assert.AreEqual(BacterialWaterSampleFactory.DEFAULT_CL2_TOTAL, target.Cl2Total);

            target = GetFactory<BacterialWaterSampleFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(BacterialWaterSampleFactory.DEFAULT_CL2_FREE, target.Cl2Free);
            Assert.AreEqual(BacterialWaterSampleFactory.DEFAULT_CL2_TOTAL, target.Cl2Total);
        }

        #endregion

        #region Bond

        [TestMethod]
        public void TestBondFactorySetsDefaults()
        {
            var target = GetFactory<BondFactory>().Build();

            Assert.IsNotNull(target.State);
            Assert.IsNotNull(target.County);

            target = GetFactory<BondFactory>().Create();

            Assert.IsNotNull(target.State);
            Assert.IsNotNull(target.County);
        }

        #endregion

        #region BondPurpose

        [TestMethod]
        public void TestRoadOpeningPermitBondPurposeSetsDefaults()
        {
            var target = GetFactory<RoadOpeningPermitBondPurposeFactory>().Build();
            Assert.AreEqual(RoadOpeningPermitBondPurposeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<RoadOpeningPermitBondPurposeFactory>().Create();
            Assert.AreEqual(RoadOpeningPermitBondPurposeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(BondPurpose.Indices.ROAD_OPENING_PERMIT, target.Id);
        }

        [TestMethod]
        public void TestPerformanceBondBondPurposeSetsDefaults()
        {
            var target = GetFactory<PerformanceBondPurposeFactory>().Build();
            Assert.AreEqual(PerformanceBondPurposeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<PerformanceBondPurposeFactory>().Create();
            Assert.AreEqual(PerformanceBondPurposeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(BondPurpose.Indices.PERFORMANCE_BOND, target.Id);
        }

        #endregion

        #region CompanyLaborCost

        [TestMethod]
        public void TestCompanyLaborCostFactorySetsDefaults()
        {
            var target = GetFactory<CompanyLaborCostFactory>().Build();

            Assert.AreEqual(CompanyLaborCostFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(CompanyLaborCostFactory.DEFAULT_UNIT, target.Unit);
            Assert.AreEqual(CompanyLaborCostFactory.DEFAULT_COST, target.Cost);

            target = GetFactory<CompanyLaborCostFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(CompanyLaborCostFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(CompanyLaborCostFactory.DEFAULT_UNIT, target.Unit);
            Assert.AreEqual(CompanyLaborCostFactory.DEFAULT_COST, target.Cost);
        }

        #endregion

        #region ClaimsRepresentative

        [TestMethod]
        public void TestClaimsRepresentativeSetsDefaults()
        {
            var regex = new Regex("^" + ClaimsRepresentativeFactory.DESCRIPTION + " #(\\d+)$");
            var target = GetFactory<ClaimsRepresentativeFactory>().Build();
            var match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<ClaimsRepresentativeFactory>().Create();
            match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region ClassLocation

        [TestMethod]
        public void TestClassLocationFactorySetsDefaults()
        {
            var target = GetFactory<ClassLocationFactory>().Build();

            Assert.AreEqual(ClassLocationFactory.DEFAULT_NAME, target.Name);

            target = GetFactory<ClassLocationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ClassLocationFactory.DEFAULT_NAME, target.Name);
        }

        #endregion

        #region ContactFactory

        [TestMethod]
        public void TestContactFactoryCreatesNewAddressByDefault()
        {
            var contact = GetFactory<ContactFactory>().Create();
            Assert.IsNotNull(contact.Address);
            Assert.AreNotEqual(0, contact.Address.Id);
        }

        #endregion

        #region ContactTypeFactory

        [TestMethod]
        public void TestContactTypeFactoryBuildsUniqueDefaultDescription()
        {
            var factory = GetFactory<ContactTypeFactory>();
            var one = factory.Build();
            var two = factory.Build();

            Assert.AreNotEqual(one.Description, two.Description);
        }

        [TestMethod]
        public void TestContactTypeFactorySaveReturnsExistingContactTypeForMatchingDescription()
        {
            var factory = GetFactory<ContactTypeFactory>();
            var one = factory.Create(new {Description = "Neat"});
            var two = factory.Create(new {Description = "Neat"});
            Assert.AreSame(one, two);
            var three = factory.Create(new {Description = "Not Neat"});
            Assert.AreNotSame(one, three);
        }

        #endregion

        #region Contractor

        [TestMethod]
        public void TestContractorFactorySetsDefaults()
        {
            var regex =
                new Regex("^" + ContractorFactory.DEFAULT_NAME + " #(\\d+)$");
            var target = GetFactory<ContractorFactory>().Build();
            var match = regex.Match(target.Name);

            Assert.IsNotNull(match);
            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<ContractorFactory>().Create();
            match = regex.Match(target.Name);

            Assert.IsNotNull(match);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region CoordinateFactory

        [TestMethod]
        public void TestCoordinateFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<CoordinateFactory>();
            var coordinate = factory.Build();

            Assert.AreEqual(CoordinateFactory.LATITUDE, coordinate.Latitude);
            Assert.AreEqual(CoordinateFactory.LONGITUDE, coordinate.Longitude);
            Assert.IsNotNull(coordinate.Icon);

            coordinate = factory.Create();

            Assert.AreNotEqual(0, coordinate.Id);
            Assert.AreEqual(CoordinateFactory.LATITUDE, coordinate.Latitude);
            Assert.AreEqual(CoordinateFactory.LONGITUDE, coordinate.Longitude);
            Assert.IsNotNull(coordinate.Icon);
        }

        #endregion

        #region CountyFactory

        [TestMethod]
        public void TestCountyFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<CountyFactory>();
            var county = factory.Build();

            Assert.IsNotNull(county.State);
            Assert.IsTrue(county.Enabled.Value);
            //Assert.IsTrue(county.State.Counties.Contains(county));

            county = factory.Create();

            Assert.IsNotNull(county.State);
            Assert.IsTrue(county.Enabled.Value);
            Assert.IsTrue(county.State.Counties.Contains(county));
        }

        [TestMethod]
        public void TestCountyFactoryDefaultNameIsAlwaysUnique()
        {
            var factory = GetFactory<CountyFactory>();
            Assert.AreNotEqual(factory.Build().Name, factory.Build().Name);
            Assert.AreNotEqual(factory.Create().Name, factory.Create().Name);
        }

        #endregion

        #region Crew

        [TestMethod]
        public void TestCrewFactorySetsDefaults()
        {
            var target = GetFactory<CrewFactory>().Build();

            Assert.AreEqual(CrewFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<CrewFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(CrewFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region CrewAssignment

        [TestMethod]
        public void TestCrewAssignmentFactorySetsDefaults()
        {
            var target = GetFactory<CrewAssignmentFactory>().Build();

            MyAssert.AreClose(Lambdas.GetNow(), target.AssignedFor);

            target = GetFactory<CrewAssignmentFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            MyAssert.AreClose(Lambdas.GetNow(), target.AssignedFor);
        }

        #endregion

        #region CustomerCoordinate

        [TestMethod]
        public void TestCustomerCoordinateFactorySetsDefaults()
        {
            var target = GetFactory<CustomerCoordinateFactory>().Build();

            Assert.AreEqual(CustomerCoordinateFactory.DEFAULT_SOURCE, target.Source);
            Assert.AreEqual(CustomerCoordinateFactory.DEFAULT_LATITUDE, target.Latitude);
            Assert.AreEqual(CustomerCoordinateFactory.DEFAULT_LONGITUDE, target.Longitude);
            Assert.IsNotNull(target.CustomerLocation);

            target = GetFactory<CustomerCoordinateFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(CustomerCoordinateFactory.DEFAULT_SOURCE, target.Source);
            Assert.AreEqual(CustomerCoordinateFactory.DEFAULT_LATITUDE, target.Latitude);
            Assert.AreEqual(CustomerCoordinateFactory.DEFAULT_LONGITUDE, target.Longitude);
            Assert.IsNotNull(target.CustomerLocation);
        }

        #endregion

        #region CustomerLocation

        [TestMethod]
        public void TestCustomerLocationFactorySetsDefaults()
        {
            var target = GetFactory<CustomerLocationFactory>().Build();

            Assert.AreEqual(CustomerLocationFactory.DEFAULT_ADDRESS, target.Address);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_CITY, target.City);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_PREMISE_NUMBER, target.PremiseNumber);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_STATE, target.State);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_ZIP, target.Zip);

            target = GetFactory<CustomerLocationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_ADDRESS, target.Address);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_CITY, target.City);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_PREMISE_NUMBER, target.PremiseNumber);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_STATE, target.State);
            Assert.AreEqual(CustomerLocationFactory.DEFAULT_ZIP, target.Zip);
        }

        #endregion

        #region CutoffSawQuestion

        [TestMethod]
        public void TestCutoffSawQuestionFactorySetsDefaults()
        {
            var target = GetFactory<CutoffSawQuestionFactory>().Build();

            Assert.AreEqual(CutoffSawQuestionFactory.DEFAULT_QUESTION, target.Question);
            Assert.AreEqual(CutoffSawQuestionFactory.DEFAULT_SORT_ORDER, target.SortOrder);
            Assert.IsTrue(target.IsActive);

            target = GetFactory<CutoffSawQuestionFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(CutoffSawQuestionFactory.DEFAULT_QUESTION, target.Question);
            Assert.AreEqual(CutoffSawQuestionFactory.DEFAULT_SORT_ORDER, target.SortOrder);
            Assert.IsTrue(target.IsActive);
        }

        #endregion

        #region CutoffSawQuestionnaire

        [TestMethod]
        public void TestCutoffSawQuestionnaireFactorySetsDefaults()
        {
            var target = GetFactory<CutoffSawQuestionnaireFactory>().Build();

            Assert.IsNotNull(target.LeadPerson);
            Assert.IsNotNull(target.SawOperator);
            MyAssert.AreClose(Lambdas.GetNow(), target.OperatedOn);

            target = GetFactory<CutoffSawQuestionnaireFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.IsNotNull(target.LeadPerson);
            Assert.IsNotNull(target.SawOperator);
            MyAssert.AreClose(Lambdas.GetNow(), target.OperatedOn);
        }

        #endregion

        #region DataTypeFactory

        [TestMethod]
        public void TestDataTypeFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<DataTypeFactory>();
            var dt = factory.Build();

            Assert.AreEqual(DataTypeFactory.NAME, dt.Name);
            Assert.AreEqual(DataTypeFactory.TABLE_ID, dt.TableID);

            dt = factory.Create();

            Assert.AreEqual(DataTypeFactory.NAME, dt.Name);
            Assert.AreEqual(DataTypeFactory.TABLE_ID, dt.TableID);
        }

        #endregion

        #region Department

        [TestMethod]
        public void TestDepartmentFactorySetsDefaults()
        {
            var obj = GetFactory<DepartmentFactory>().Build();

            Assert.AreEqual(DepartmentFactory.DESCRIPTION, obj.Description);

            obj = GetFactory<DepartmentFactory>().Create();

            Assert.AreNotEqual(0, obj.Id);
            Assert.AreEqual(DepartmentFactory.DESCRIPTION, obj.Description);
        }

        #endregion

        #region Division

        [TestMethod]
        public void TestDivisionFactorySetsDefaults()
        {
            var target = GetFactory<DivisionFactory>().Build();

            Assert.AreEqual(DivisionFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<DivisionFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(DivisionFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region DocumentDataFactory

        [TestMethod]
        public void TestDocumentDataFactoryCreateReturnsExistingInstanceIfOneWithMatchingHashExists()
        {
            _container.Inject<IDocumentService>(new InMemoryDocumentService());
            var data = GetFactory<DocumentDataFactory>().Create(new {BinaryData = new byte[] {1, 2, 3}});
            var shouldBeSameInstance =
                GetFactory<DocumentDataFactory>().Create(new {BinaryData = new byte[] {1, 2, 3}});
            Assert.AreSame(data, shouldBeSameInstance);
        }

        [TestMethod]
        public void TestDocumentDataFactoryBuildAlwaysReturnsAUniqueInstanceByDefault()
        {
            _container.Inject<IDocumentService>(new InMemoryDocumentService());
            var data = GetFactory<DocumentDataFactory>().Build();
            var diffData = GetFactory<DocumentDataFactory>().Build();
            Assert.AreNotSame(data, diffData);
            Assert.AreNotEqual(data.Hash, diffData.Hash);
        }

        #endregion

        #region DocumentFactory

        [TestMethod]
        public void TestDocumentFactorySetsUpDefaultValues()
        {
            _container.Inject<IDocumentService>(new InMemoryDocumentService());
            var factory = GetFactory<DocumentFactory>();
            var doc = factory.Build();

            Assert.AreEqual(doc.FileName, DocumentFactory.FILENAME);
            Assert.IsNotNull(doc.CreatedAt);
            Assert.IsNotNull(doc.UpdatedAt);
            Assert.IsNotNull(doc.CreatedBy);
            Assert.IsNotNull(doc.UpdatedBy);

            doc = factory.Create();

            Assert.AreEqual(doc.FileName, DocumentFactory.FILENAME);
            Assert.IsNotNull(doc.CreatedAt);
            Assert.IsNotNull(doc.UpdatedAt);
            Assert.IsNotNull(doc.CreatedBy);
            Assert.IsNotNull(doc.UpdatedBy);
        }

        #endregion

        #region DocumentTypeFactory

        [TestMethod]
        public void TestDocumentTypeFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<DocumentTypeFactory>();
            var dt = factory.Build();

            Assert.AreEqual(DocumentTypeFactory.NAME, dt.Name);
            Assert.IsNotNull(dt.DataType);

            dt = factory.Create();

            Assert.AreEqual(DocumentTypeFactory.NAME, dt.Name);
            Assert.IsNotNull(dt.DataType);
        }

        #endregion

        #region EmergencyPowerType

        [TestMethod]
        public void TestEmergencyPowerTypeFactorySetsDefaults()
        {
            var target = GetFactory<EmergencyPowerTypeFactory>().Build();

            Assert.AreEqual(EmergencyPowerTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EmergencyPowerTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EmergencyPowerTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region EmergencyResponsePlan

        [TestMethod]
        public void TestEmergencyResponsePlanFactorySetsDefaults()
        {
            var target = GetFactory<EmergencyResponsePlanFactory>().Build();

            Assert.AreEqual(EmergencyResponsePlanFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(EmergencyResponsePlanFactory.DEFAULT_TITLE, target.Title);

            target = GetFactory<EmergencyResponsePlanFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EmergencyResponsePlanFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(EmergencyResponsePlanFactory.DEFAULT_TITLE, target.Title);
        }

        #endregion

        #region Employee

        [TestMethod]
        public void TestEmployeeFactorySetsDefaults()
        {
            var target = GetFactory<EmployeeFactory>().Build();

            Assert.IsNotNull(target.FirstName);
            Assert.IsNotNull(target.LastName);
            Assert.IsNotNull(target.EmployeeId);

            target = GetFactory<EmployeeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.IsNotNull(target.FirstName);
            Assert.IsNotNull(target.LastName);
            Assert.IsNotNull(target.EmployeeId);

            GetFactory<EmployeeFactory>().BuildArray(100);
        }

        #endregion

        #region EmployeeDepartment

        [TestMethod]
        public void TestEmployeeDepartmentFactorySetsDefaults()
        {
            var target = GetFactory<EmployeeDepartmentFactory>().Build();

            Assert.AreEqual(EmployeeDepartmentFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EmployeeDepartmentFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EmployeeDepartmentFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region EndorsementStatus

        [TestMethod]
        public void TestEndorsementStatusFactorySetsDefaults()
        {
            var target = GetFactory<EndorsementStatusFactory>().Build();

            Assert.AreEqual(EndorsementStatusFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EndorsementStatusFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EndorsementStatusFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region Equipment

        [TestMethod]
        public void TestEquipmentFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentFactory>().Build();

            Assert.AreEqual(EquipmentFactory.DEFAULT_DESCRIPTION, target.Description);
            //Assert.AreEqual(EquipmentFactory.IDENTIFIER, target.Identifier);
            Assert.IsNotNull(target.Identifier);

            target = GetFactory<EquipmentFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentFactory.DEFAULT_DESCRIPTION, target.Description);
            //   Assert.AreEqual(EquipmentFactory.IDENTIFIER, target.Identifier);
            Assert.IsNotNull(target.Identifier);
        }

        #endregion

        #region EquipmentCategory

        [TestMethod]
        public void TestEquipmentCategoryFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentCategoryFactory>().Build();

            Assert.AreEqual(EquipmentCategoryFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EquipmentCategoryFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region EquipmentLifespan

        [TestMethod]
        public void TestChemicalFeedDryEquipmentLifespanFactorySetsDefaults()
        {
            var target = GetFactory<ChemicalFeedDryEquipmentLifespanFactory>().Build();

            Assert.AreEqual(ChemicalFeedDryEquipmentLifespanFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ChemicalFeedDryEquipmentLifespanFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentLifespan.Indices.CHEMICAL_FEED_DRY, target.Id);
            Assert.AreEqual(ChemicalFeedDryEquipmentLifespanFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestEngineEquipmentLifespanFactorySetsDefaults()
        {
            var target = GetFactory<EngineEquipmentLifespanFactory>().Build();

            Assert.AreEqual(EngineEquipmentLifespanFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EngineEquipmentLifespanFactory>().Create();
            Session.Flush();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentLifespan.Indices.ENGINE, target.Id);
            Assert.AreEqual(EngineEquipmentLifespanFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestGeneratorEquipmentLifespanFactorySetsDefaults()
        {
            var target = GetFactory<GeneratorEquipmentLifespanFactory>().Build();

            Assert.AreEqual(GeneratorEquipmentLifespanFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<GeneratorEquipmentLifespanFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentLifespan.Indices.GENERATOR, target.Id);
            Assert.AreEqual(GeneratorEquipmentLifespanFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region EquipmentModel

        [TestMethod]
        public void TestEquipmentModelFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentModelFactory>().Build();

            Assert.AreEqual(EquipmentModelFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EquipmentModelFactory>().Create(new
                {EquipmentManufacturer = GetFactory<EquipmentManufacturerFactory>().Create()});

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentModelFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region EquipmentStatus

        [TestMethod]
        public void TestInServiceEquipmentStatusFactory()
        {
            var target = GetFactory<InServiceEquipmentStatusFactory>().Create();

            Assert.AreEqual(EquipmentStatus.IN_SERVICE, target.Description);
            Assert.AreEqual(EquipmentStatus.Indices.IN_SERVICE, target.Id);
        }

        [TestMethod]
        public void TestOutOfServiceEquipmentStatusFactory()
        {
            var target = GetFactory<OutOfServiceEquipmentStatusFactory>().Create();

            Assert.AreEqual(EquipmentStatus.OUT_OF_SERVICE, target.Description);
            Assert.AreEqual(EquipmentStatus.Indices.OUT_OF_SERVICE, target.Id);
        }

        [TestMethod]
        public void TestPendingEquipmentStatusFactory()
        {
            var target = GetFactory<PendingEquipmentStatusFactory>().Create();

            Assert.AreEqual(EquipmentStatus.PENDING, target.Description);
            Assert.AreEqual(EquipmentStatus.Indices.PENDING, target.Id);
        }

        [TestMethod]
        public void TestRetiredEquipmentStatusFactory()
        {
            var target = GetFactory<RetiredEquipmentStatusFactory>().Create();

            Assert.AreEqual(EquipmentStatus.RETIRED, target.Description);
            Assert.AreEqual(EquipmentStatus.Indices.RETIRED, target.Id);
        }

        [TestMethod]
        public void TestPendingRetirementEquipmentStatusFactory()
        {
            var target = GetFactory<PendingRetirementEquipmentStatusFactory>().Create();

            Assert.AreEqual(EquipmentStatus.PENDING_RETIREMENT, target.Description);
            Assert.AreEqual(EquipmentStatus.Indices.PENDING_RETIREMENT, target.Id);
        }

        [TestMethod]
        public void TestCancelledEquipmentStatusFactory()
        {
            var target = GetFactory<CancelledEquipmentStatusFactory>().Create();

            Assert.AreEqual(EquipmentStatus.CANCELLED, target.Description);
            Assert.AreEqual(EquipmentStatus.Indices.CANCELLED, target.Id);
        }

        #endregion

        #region EquipmentSubCategory

        [TestMethod]
        public void TestEquipmentSubCategoryFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentSubCategoryFactory>().Build();

            Assert.AreEqual(EquipmentSubCategoryFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EquipmentSubCategoryFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentSubCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region EquipmentPurpose

        [TestMethod]
        public void TestEquipmentPurposeFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentPurposeFactory>().Build();

            Assert.AreEqual(EquipmentPurposeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(EquipmentPurposeFactory.DEFAULT_ABBREVIATION, target.Abbreviation);

            target = GetFactory<EquipmentPurposeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentPurposeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(EquipmentPurposeFactory.DEFAULT_ABBREVIATION, target.Abbreviation);
        }

        #endregion

        #region FacilityFactory

        [TestMethod]
        public void TestFacilityFactorySetsUpDefaultvalues()
        {
            var factory = GetEntityFactory<Facility>();
            var facility = factory.Build();

            Assert.IsNotNull(facility.FacilityId);
            Assert.IsNotNull(facility.OperatingCenter);
            Assert.IsNotNull(facility.Coordinate);

            facility = factory.Create();

            Assert.IsNotNull(facility.FacilityId);
            Assert.IsNotNull(facility.OperatingCenter);
            Assert.IsNotNull(facility.Coordinate);
        }

        #endregion

        #region FacilityOwner

        [TestMethod]
        public void TestFacilityOwnerFactorySetsDefaults()
        {
            var obj = GetFactory<AWFacilityOwnerFactory>().Build();

            Assert.AreEqual(AWFacilityOwnerFactory.DESCRIPTION, obj.Description);

            obj = GetFactory<AWFacilityOwnerFactory>().Create();

            Assert.AreNotEqual(0, obj.Id);
            Assert.AreEqual(AWFacilityOwnerFactory.DESCRIPTION, obj.Description);
        }

        #endregion

        #region FacilityStatus

        [TestMethod]
        public void TestFacilityStatusFactorySetsDefaults()
        {
            var obj = GetFactory<ActiveFacilityStatusFactory>().Build();
            Assert.AreEqual(ActiveFacilityStatusFactory.DESCRIPTION, obj.Description);
            obj = GetFactory<ActiveFacilityStatusFactory>().Create();
            Assert.AreNotEqual(0, obj.Id);

            var obj2 = GetFactory<InactiveFacilityStatusFactory>().Build();
            Assert.AreEqual(InactiveFacilityStatusFactory.DESCRIPTION, obj2.Description);
            obj2 = GetFactory<InactiveFacilityStatusFactory>().Create();
            Assert.AreNotEqual(0, obj2.Id);
        }

        #endregion

        #region FamilyMedicalLeaveActCase

        [TestMethod]
        public void TestFamilyMedicalLeaveActCaseFactorySetsDefaults()
        {
            var target = GetFactory<FamilyMedicalLeaveActCaseFactory>().Build();

            Assert.IsNotNull(target.Employee);
            Assert.IsFalse(target.CertificationExtended);
            Assert.IsFalse(target.SendPackage);
            Assert.IsFalse(target.ChronicCondition);

            target = GetFactory<FamilyMedicalLeaveActCaseFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.IsNotNull(target.Employee);
            Assert.IsFalse(target.CertificationExtended);
            Assert.IsFalse(target.SendPackage);
            Assert.IsFalse(target.ChronicCondition);
        }

        #endregion

        #region FEMAFloodRating

        [TestMethod]
        public void TestFEMAFloodRatingFactorySetsDefaults()
        {
            var obj = GetFactory<FEMAFloodRatingFactory>().Build();

            Assert.AreEqual(FEMAFloodRatingFactory.DESCRIPTION, obj.Description);

            obj = GetFactory<FEMAFloodRatingFactory>().Create();

            Assert.AreNotEqual(0, obj.Id);
            Assert.AreEqual(FEMAFloodRatingFactory.DESCRIPTION, obj.Description);
        }

        #endregion

        #region FilterMedia

        [TestMethod]
        public void TestFilterMediaFactorySetsDefaults()
        {
            var obj = GetFactory<FilterMediaFactory>().Build();

            Assert.IsNotNull(obj.FilterType);
            Assert.IsNotNull(obj.LevelControlMethod);
            Assert.IsNotNull(obj.Equipment);
            Assert.IsNotNull(obj.WashType);
            Assert.IsNotNull(obj.MediaType);
            Assert.IsNotNull(obj.Location);

            obj = GetFactory<FilterMediaFactory>().Create();

            Assert.AreNotEqual(0, obj.Id);
            Assert.IsNotNull(obj.FilterType);
            Assert.IsNotNull(obj.LevelControlMethod);
            Assert.IsNotNull(obj.Equipment);
            Assert.IsNotNull(obj.WashType);
            Assert.IsNotNull(obj.MediaType);
            Assert.IsNotNull(obj.Location);
        }

        #endregion

        #region FireDistrict

        [TestMethod]
        public void TestFireDistrictFactorySetsDefaults()
        {
            var obj = GetFactory<FireDistrictFactory>().Build();

            Assert.AreEqual(FireDistrictFactory.ADDRESS, obj.Address);
            Assert.AreEqual(FireDistrictFactory.ADDRESS_CITY, obj.AddressCity);
            Assert.AreEqual(FireDistrictFactory.ADDRESS_ZIP, obj.AddressZip);
            Assert.AreEqual(FireDistrictFactory.CONTACT, obj.Contact);
            Assert.AreEqual(FireDistrictFactory.DISTRICT_NAME, obj.DistrictName);
            Assert.AreEqual(FireDistrictFactory.FAX, obj.Fax);
            Assert.AreEqual(FireDistrictFactory.PHONE, obj.Phone);
            Assert.IsNotNull(obj.State);
            Assert.AreEqual(FireDistrictFactory.ABBREVIATION, obj.Abbreviation);
            Assert.AreEqual(FireDistrictFactory.PREMISE_NUMBER, obj.PremiseNumber);
            Assert.AreEqual(FireDistrictFactory.UTILITY_NAME, obj.UtilityName);
            Assert.AreEqual(FireDistrictFactory.UTILITY_DISTRICT, obj.UtilityDistrict);

            obj = GetFactory<FireDistrictFactory>().Create();

            Assert.AreNotEqual(0, obj.Id);
            Assert.AreEqual(FireDistrictFactory.ADDRESS, obj.Address);
            Assert.AreEqual(FireDistrictFactory.ADDRESS_CITY, obj.AddressCity);
            Assert.AreEqual(FireDistrictFactory.ADDRESS_ZIP, obj.AddressZip);
            Assert.AreEqual(FireDistrictFactory.CONTACT, obj.Contact);
            Assert.AreEqual(FireDistrictFactory.DISTRICT_NAME, obj.DistrictName);
            Assert.AreEqual(FireDistrictFactory.FAX, obj.Fax);
            Assert.AreEqual(FireDistrictFactory.PHONE, obj.Phone);
            Assert.IsNotNull(obj.State);
            Assert.AreEqual(FireDistrictFactory.ABBREVIATION, obj.Abbreviation);
            Assert.AreEqual(FireDistrictFactory.PREMISE_NUMBER, obj.PremiseNumber);
            Assert.AreEqual(FireDistrictFactory.UTILITY_NAME, obj.UtilityName);
            Assert.AreEqual(FireDistrictFactory.UTILITY_DISTRICT, obj.UtilityDistrict);
        }

        #endregion

        #region FireDistrictTown

        [TestMethod]
        public void TestFireDistrictTownFactorySetsDefaults()
        {
            var obj = GetFactory<FireDistrictTownFactory>().Build();

            Assert.IsFalse(obj.IsDefault);
            Assert.IsNotNull(obj.State);
            Assert.IsNotNull(obj.Town);
            Assert.IsNotNull(obj.FireDistrict);

            obj = GetFactory<FireDistrictTownFactory>().Create();

            Assert.AreNotEqual(0, obj.Id);
            Assert.IsFalse(obj.IsDefault);
            Assert.IsNotNull(obj.State);
            Assert.IsNotNull(obj.Town);
            Assert.IsNotNull(obj.FireDistrict);
        }

        #endregion

        #region FoundationalFilingPeriod

        [TestMethod]
        public void TestFoundationalFilingPeriodFactorySetsDefaults()
        {
            var target = GetFactory<FoundationalFilingPeriodFactory>().Build();

            Assert.AreEqual(FoundationalFilingPeriodFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<FoundationalFilingPeriodFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(FoundationalFilingPeriodFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region FuelType

        [TestMethod]
        public void TestFuelTypeFactorySetsDefaults()
        {
            var target = GetFactory<FuelTypeFactory>().Build();

            Assert.AreEqual(FuelTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<FuelTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(FuelTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region FunctionalLocation

        [TestMethod]
        public void TestFunctionalLocationFactorySetsDefaults()
        {
            var target = GetFactory<FunctionalLocationFactory>().Build();

            Assert.AreEqual(FunctionalLocationFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<FunctionalLocationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(FunctionalLocationFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestValveFunctionalLocationFactorySetsDefaults()
        {
            var target = GetFactory<ValveFunctionalLocationFactory>().Build();

            Assert.AreEqual(ValveFunctionalLocationFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ValveFunctionalLocationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ValveFunctionalLocationFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestEquipmentFunctionalLocationFactorySetsDefaults()
        {
            var target = GetFactory<EquipmentFunctionalLocationFactory>().Build();

            Assert.AreEqual(EquipmentFunctionalLocationFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<EquipmentFunctionalLocationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(EquipmentFunctionalLocationFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region GeneralLiabilityClaim

        [TestMethod]
        public void TestGeneralLiabilityClaimFactorySetsDefaults()
        {
            var target = GetFactory<GeneralLiabilityClaimFactory>().Build();

            Assert.AreEqual(GeneralLiabilityClaimFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.IsTrue(target.ClaimsRepresentative.ToString().StartsWith(ClaimsRepresentativeFactory.DESCRIPTION));
            Assert.AreEqual(GeneralLiabilityClaimFactory.DEFAULT_CLAIM_NUMBER, target.ClaimNumber);
            Assert.AreEqual(GeneralLiabilityClaimFactory.DEFAULT_REPORTED_BY, target.ReportedBy);

            target = GetFactory<GeneralLiabilityClaimFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(GeneralLiabilityClaimFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.IsTrue(target.ClaimsRepresentative.ToString().StartsWith(ClaimsRepresentativeFactory.DESCRIPTION));
            Assert.AreEqual(GeneralLiabilityClaimFactory.DEFAULT_CLAIM_NUMBER, target.ClaimNumber);
            Assert.AreEqual(GeneralLiabilityClaimFactory.DEFAULT_REPORTED_BY, target.ReportedBy);
        }

        #endregion

        #region HighCostFactor

        [TestMethod]
        public void TestHighCostFactorFactorySetsDefaults()
        {
            var target = GetFactory<HighCostFactorFactory>().Build();

            Assert.AreEqual(HighCostFactorFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<HighCostFactorFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(HighCostFactorFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region Hydrant

        [TestMethod]
        public void TestHydrantFactorySetsDefaults()
        {
            var target = GetFactory<HydrantFactory>().Build();

            Assert.AreEqual(HydrantFactory.DEFAULT_HYDRANT_NUMBER, target.HydrantNumber);

            target = GetFactory<HydrantFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(HydrantFactory.DEFAULT_HYDRANT_NUMBER, target.HydrantNumber);
        }

        #endregion

        #region HydrantInspection

        [TestMethod]
        public void TestHydrantInspectionFactorySetsDefaults()
        {
            var target = GetFactory<HydrantInspectionFactory>().Build();

            MyAssert.AreClose(Lambdas.GetNow(), target.DateInspected);

            target = GetFactory<HydrantInspectionFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            MyAssert.AreClose(Lambdas.GetNow(), target.DateInspected);
        }

        #endregion

        #region Interconnection

        [TestMethod]
        public void TestInterconnectionFactorySetsDefaults()
        {
            var target = GetFactory<InterconnectionFactory>().Build();

            Assert.IsNotNull(target.PurchaseSellTransfer);
            Assert.IsNotNull(target.OperatingStatus);
            Assert.IsNotNull(target.Category);
            Assert.IsNotNull(target.DeliveryMethod);
            Assert.IsNotNull(target.FlowControlMethod);
            Assert.IsNotNull(target.Direction);
            Assert.IsNotNull(target.Type);
            Assert.IsNotNull(target.Facility);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);
            Assert.AreEqual(InterconnectionFactory.DEP_DESIGNATION, target.DEPDesignation);
            Assert.AreEqual(InterconnectionFactory.PROGRAM_INTEREST_NUMBER, target.ProgramInterestNumber);
            Assert.AreEqual(InterconnectionFactory.PURCHASED_ACCOUNT_NUMBER, target.PurchasedWaterAccountNumber);
            Assert.AreEqual(InterconnectionFactory.SOLD_ACCOUNT_NUMBER, target.SoldWaterAccountNumber);
            Assert.IsTrue(target.DirectConnection.Value);
            Assert.AreEqual(InterconnectionFactory.INLET_CONNECTION_SIZE, target.InletConnectionSize);
            Assert.AreEqual(InterconnectionFactory.OUTLET_CONNECTION_SIZE, target.OutletConnectionSize);
            Assert.AreEqual(InterconnectionFactory.INLET_STATIC_PRESSURE, target.InletStaticPressure);
            Assert.AreEqual(InterconnectionFactory.OUTLET_STATIC_PRESSURE, target.OutletStaticPressure);
            Assert.AreEqual(InterconnectionFactory.MAXIMUM_FLOW_CAPACITY, target.MaximumFlowCapacity.Value);
            Assert.AreEqual(InterconnectionFactory.MAXIMUM_FLOW_CAPACITY_STRESSED_CONDITION,
                target.MaximumFlowCapacityStressedCondition.Value);
            Assert.IsTrue(target.DistributionPipingRestrictions.Value);
            Assert.AreEqual(InterconnectionFactory.WATER_QUALITY, target.WaterQuality);
            Assert.IsTrue(target.FluoridatedSupplyReceivingPurveyor);
            Assert.IsTrue(target.FluoridatedSupplyDeliveryPurveyor);
            Assert.IsTrue(target.ChloramineResidualReceivingPurveyor);
            Assert.IsTrue(target.ChloramineResidualDeliveryPurveyor);
            Assert.IsTrue(target.CorrosionInhibitorReceivingPurveyor);
            Assert.IsTrue(target.CorrosionInhibitorDeliveryPurveyor);
            Assert.AreEqual(InterconnectionFactory.REVERSIBLE_CAPACITY, target.ReversibleCapacity);
            Assert.IsTrue(target.AnnualTestRequired);
            Assert.IsTrue(target.Contract.Value);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MAX_SUMMER, target.ContractMaxSummer);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MIN_SUMMER, target.ContractMinSummer);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MAX_WINTER, target.ContractMaxWinter);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MIN_WINTER, target.ContractMinWinter);
            Assert.IsNotNull(target.InletPWSID);
            Assert.IsNotNull(target.OutletPWSID);

            target = GetFactory<InterconnectionFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.IsNotNull(target.PurchaseSellTransfer);
            Assert.IsNotNull(target.OperatingStatus);
            Assert.IsNotNull(target.Category);
            Assert.IsNotNull(target.DeliveryMethod);
            Assert.IsNotNull(target.FlowControlMethod);
            Assert.IsNotNull(target.Direction);
            Assert.IsNotNull(target.Type);
            Assert.IsNotNull(target.Facility);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);
            Assert.AreEqual(InterconnectionFactory.DEP_DESIGNATION, target.DEPDesignation);
            Assert.AreEqual(InterconnectionFactory.PROGRAM_INTEREST_NUMBER, target.ProgramInterestNumber);
            Assert.AreEqual(InterconnectionFactory.PURCHASED_ACCOUNT_NUMBER, target.PurchasedWaterAccountNumber);
            Assert.AreEqual(InterconnectionFactory.SOLD_ACCOUNT_NUMBER, target.SoldWaterAccountNumber);
            Assert.IsTrue(target.DirectConnection.Value);
            Assert.AreEqual(InterconnectionFactory.INLET_CONNECTION_SIZE, target.InletConnectionSize);
            Assert.AreEqual(InterconnectionFactory.OUTLET_CONNECTION_SIZE, target.OutletConnectionSize);
            Assert.AreEqual(InterconnectionFactory.INLET_STATIC_PRESSURE, target.InletStaticPressure);
            Assert.AreEqual(InterconnectionFactory.OUTLET_STATIC_PRESSURE, target.OutletStaticPressure);
            Assert.AreEqual(InterconnectionFactory.MAXIMUM_FLOW_CAPACITY, target.MaximumFlowCapacity.Value);
            Assert.AreEqual(InterconnectionFactory.MAXIMUM_FLOW_CAPACITY_STRESSED_CONDITION,
                target.MaximumFlowCapacityStressedCondition.Value);
            Assert.IsTrue(target.DistributionPipingRestrictions.Value);
            Assert.AreEqual(InterconnectionFactory.WATER_QUALITY, target.WaterQuality);
            Assert.IsTrue(target.FluoridatedSupplyReceivingPurveyor);
            Assert.IsTrue(target.FluoridatedSupplyDeliveryPurveyor);
            Assert.IsTrue(target.ChloramineResidualReceivingPurveyor);
            Assert.IsTrue(target.ChloramineResidualDeliveryPurveyor);
            Assert.IsTrue(target.CorrosionInhibitorReceivingPurveyor);
            Assert.IsTrue(target.CorrosionInhibitorDeliveryPurveyor);
            Assert.AreEqual(InterconnectionFactory.REVERSIBLE_CAPACITY, target.ReversibleCapacity);
            Assert.IsTrue(target.AnnualTestRequired);
            Assert.IsTrue(target.Contract.Value);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MAX_SUMMER, target.ContractMaxSummer);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MIN_SUMMER, target.ContractMinSummer);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MAX_WINTER, target.ContractMaxWinter);
            Assert.AreEqual(InterconnectionFactory.CONTRACT_MIN_WINTER, target.ContractMinWinter);
            Assert.IsNotNull(target.InletPWSID);
            Assert.IsNotNull(target.OutletPWSID);
        }

        #endregion

        #region JobCategory

        [TestMethod]
        public void TestJobCategoryFactorySetsDefaults()
        {
            var target = GetFactory<JobCategoryFactory>().Build();

            Assert.AreEqual(JobCategoryFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<JobCategoryFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(JobCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region JobObservation

        [TestMethod]
        public void TestJobObservationFactorySetsDefaults()
        {
            var target = GetFactory<JobObservationFactory>().Build();

            Assert.AreEqual(JobObservationFactory.DEFAULT_DESCRIPTION, target.TaskObserved);
            Assert.AreEqual(JobObservationFactory.DEFAULT_LOCATION, target.Address);

            target = GetFactory<JobObservationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(JobObservationFactory.DEFAULT_DESCRIPTION, target.TaskObserved);
            Assert.AreEqual(JobObservationFactory.DEFAULT_LOCATION, target.Address);
        }

        #endregion

        #region JobTitleCommonName

        [TestMethod]
        public void TestJobTitleCommonNameFactorySetsDefaults()
        {
            var target = GetFactory<JobTitleCommonNameFactory>().Build();

            Assert.AreEqual(JobTitleCommonNameFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<JobTitleCommonNameFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(JobTitleCommonNameFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region LargeServiceProject

        [TestMethod]
        public void TestLargeServiceProjectFactorySetsDefaults()
        {
            var target = GetFactory<LargeServiceProjectFactory>().Build();

            Assert.AreEqual(LargeServiceProjectFactory.DEFAULT_PROJECT_TITLE, target.ProjectTitle);
            Assert.AreEqual(LargeServiceProjectFactory.DEFAULT_PROJECT_ADDRESS, target.ProjectAddress);

            target = GetFactory<LargeServiceProjectFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(LargeServiceProjectFactory.DEFAULT_PROJECT_TITLE, target.ProjectTitle);
            Assert.AreEqual(LargeServiceProjectFactory.DEFAULT_PROJECT_ADDRESS, target.ProjectAddress);
        }

        #endregion

        #region LockoutForm

        [TestMethod]
        public void TestLockoutFormFactorySetsDefaults()
        {
            var target = GetFactory<LockoutFormFactory>().Build();

            Assert.AreEqual(ProductionWorkOrderFactory.DEFAULT_SAP_WORK_ORDER, target.ProductionWorkOrder.SAPWorkOrder);
            Assert.AreEqual(LockoutFormFactory.DEFAULT_REASON, target.ReasonForLockout);
            Assert.AreEqual(LockoutFormFactory.DEFAULT_LOCATION_OF_LOCKOUT_NOTES, target.LocationOfLockoutNotes);
            Assert.IsNotNull(target.OperatingCenter);

            target = GetFactory<LockoutFormFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ProductionWorkOrderFactory.DEFAULT_SAP_WORK_ORDER, target.ProductionWorkOrder.SAPWorkOrder);
            Assert.AreEqual(LockoutFormFactory.DEFAULT_REASON, target.ReasonForLockout);
            Assert.AreEqual(LockoutFormFactory.DEFAULT_LOCATION_OF_LOCKOUT_NOTES, target.LocationOfLockoutNotes);
            Assert.IsNotNull(target.OperatingCenter);
        }

        #endregion

        #region LockoutDevice

        [TestMethod]
        public void TestLockoutDeviceFactorySetsDefaults()
        {
            var regex = new Regex("^" + LockoutDeviceFactory.DEFAULT_DESCRIPTION + " (\\d+)$");
            var target = GetFactory<LockoutDeviceFactory>().Build();
            var match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<LockoutDeviceFactory>().Create();
            match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region LockoutDeviceColor

        [TestMethod]
        public void TestLockoutDeviceColorFactorySetsDefaults()
        {
            var regex = new Regex("^" + LockoutDeviceColorFactory.DEFAULT_DESCRIPTION + " (\\d+)$");
            var target = GetFactory<LockoutDeviceColorFactory>().Build();

            var match = regex.Match(target.Description);
            Assert.IsNotNull(match);

            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<LockoutDeviceColorFactory>().Create();
            match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region LockoutDeviceLocation

        [TestMethod]
        public void TestLockoutDeviceLocationFactorySetsDefaults()
        {
            var regex = new Regex("^" + LockoutDeviceLocationFactory.DEFAULT_DESCRIPTION + " (\\d+)$");
            var target = GetFactory<LockoutDeviceLocationFactory>().Build();
            var match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<LockoutDeviceLocationFactory>().Create();
            match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region LockoutReason

        [TestMethod]
        public void TestLockoutReasonFactorySetsDefaults()
        {
            var regex = new Regex("^" + LockoutReasonFactory.DEFAULT_DESCRIPTION + " (\\d+)$");
            var target = GetFactory<LockoutReasonFactory>().Build();
            var match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<LockoutReasonFactory>().Create();
            match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
        }

        #endregion

        #region LiabilityType

        [TestMethod]
        public void TestLiabilityTypeFactorySetsDefaults()
        {
            var regex = new Regex("^" + LiabilityTypeFactory.DEFAULT_DESCRIPTION + " #(\\d+)$");
            var target = GetFactory<LiabilityTypeFactory>().Build();
            var match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            var firstNumber = int.Parse(match.Groups[1].ToString());

            target = GetFactory<LiabilityTypeFactory>().Create();
            match = regex.Match(target.Description);

            Assert.IsNotNull(match);
            Assert.AreEqual(firstNumber + 1, int.Parse(match.Groups[1].ToString()));
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region MapFactory

        [TestMethod]
        public void TestMapIconFactoryCreateReturnsSameInstanceBasedOnFileName()
        {
            var factory = GetFactory<MapIconFactory>();
            var first = factory.Create(new {FileName = "SomeFile"});
            var sameAsFirst = factory.Create(new {FileName = "SomeFile"});
            var notSame = factory.Create(new {FileName = "SomethingElse"});
            Assert.AreSame(first, sameAsFirst);
            Assert.AreNotSame(sameAsFirst, notSame);
        }

        #endregion

        #region MapIconFactory

        [TestMethod]
        public void TestMapIconFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<MapIconFactory>();
            var icon = factory.Build();

            Assert.AreEqual(MapIconFactory.FILE_NAME, icon.FileName);
            Assert.AreEqual(MapIconFactory.HEIGHT, icon.Height);
            Assert.AreEqual(MapIconFactory.WIDTH, icon.Width);
            Assert.IsNotNull(icon.Offset);

            icon = factory.Create();

            Assert.AreNotEqual(0, icon.Id);
            Assert.AreEqual(MapIconFactory.FILE_NAME, icon.FileName);
            Assert.AreEqual(MapIconFactory.HEIGHT, icon.Height);
            Assert.AreEqual(MapIconFactory.WIDTH, icon.Width);
            Assert.IsNotNull(icon.Offset);
        }

        #endregion

        #region Material

        [TestMethod]
        public void TestMaterialFactorySetsDefaults()
        {
            var target = GetFactory<MaterialFactory>().Build();

            Assert.AreEqual(MaterialFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(MaterialFactory.DEFAULT_PART_NUMBER, target.PartNumber);

            target = GetFactory<MaterialFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(MaterialFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(MaterialFactory.DEFAULT_PART_NUMBER, target.PartNumber);
        }

        #endregion

        #region MainBreak

        [TestMethod]
        public void TestMainBreakFactorySetsDefaults()
        {
            var target = GetFactory<MainBreakFactory>().Build();

            Assert.AreEqual(MainBreakFactory.DEFAULT_DEPTH, target.Depth);
            Assert.AreEqual(MainBreakFactory.DEFAULT_SHUT_DOWN_TIME, target.ShutdownTime);
            Assert.AreEqual(MainBreakFactory.DEFAULT_BOIL_ALERT_ISSUED, target.BoilAlertIssued);
            Assert.AreEqual(MainBreakFactory.DEFAULT_CUSTOMERS_AFFECTED, target.CustomersAffected);

            target = GetFactory<MainBreakFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.IsNotNull(target.WorkOrder);
            Assert.AreNotEqual(0, target.WorkOrder.Id);
            Assert.AreEqual(MainBreakFactory.DEFAULT_DEPTH, target.Depth);
            Assert.AreEqual(MainBreakFactory.DEFAULT_SHUT_DOWN_TIME, target.ShutdownTime);
            Assert.AreEqual(MainBreakFactory.DEFAULT_BOIL_ALERT_ISSUED, target.BoilAlertIssued);
            Assert.AreEqual(MainBreakFactory.DEFAULT_CUSTOMERS_AFFECTED, target.CustomersAffected);
        }

        #endregion

        #region ModuleFactory

        [TestMethod]
        public void TestModuleFactoryBuildsAndCreatesApplicationWithIdSetFromRoleModulesEnum()
        {
            var factory = GetFactory<ModuleFactory>();

            var built = factory.Build(new {Id = RoleModules.FieldServicesAssets});
            Assert.AreEqual((int)RoleModules.FieldServicesAssets, built.Id);

            var created = factory.Create(new {Id = RoleModules.FieldServicesAssets});
            Session.Flush();
            Assert.AreEqual((int)RoleModules.FieldServicesAssets, created.Id);
        }

        [TestMethod]
        public void TestModuleFactorySetsNameFromRoleModulesEnumIfOneIsNotProvided()
        {
            var factory = GetFactory<ModuleFactory>();
            var app = factory.Create(new {
                Id = RoleModules.FieldServicesAssets
            });
            Assert.AreEqual("FieldServicesAssets", app.Name);
        }

        [TestMethod]
        public void TestModuleFactoryCreateReturnsExistingDatabaseInstanceIfOneExistsForTheId()
        {
            var factory = GetFactory<ModuleFactory>();
            var first = factory.Create(new {Id = RoleModules.FieldServicesAssets});
            var second = factory.Create(new {Id = RoleModules.FieldServicesAssets});
            Assert.AreSame(first, second);
            var third = factory.Create(new {Id = RoleModules.EventsEvents});
            Assert.AreNotSame(first, third);
        }

        #endregion

        #region NoteFactory

        [TestMethod]
        public void TestNoteFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<NoteFactory>();
            var note = factory.Build();

            Assert.AreEqual(note.Text, NoteFactory.TEXT);
            Assert.AreEqual(note.LinkedId, NoteFactory.LINKED_ID);
            Assert.AreEqual(note.CreatedBy, NoteFactory.CREATED_BY);
            Assert.IsNotNull(note.DataType);

            note = factory.Create();

            Assert.AreEqual(note.Text, NoteFactory.TEXT);
            Assert.AreEqual(note.LinkedId, NoteFactory.LINKED_ID);
            Assert.AreEqual(note.CreatedBy, NoteFactory.CREATED_BY);
            Assert.IsNotNull(note.DataType);
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

        [TestMethod]
        public void TestOperatingCenterFactoryCreateReturnsExistingInstanceBasedOnOpCode()
        {
            var factory = GetFactory<OperatingCenterFactory>();
            var one = factory.Create(new {OperatingCenterCode = "RD1"});
            var two = factory.Create(new {OperatingCenterCode = "RD1"});
            var three = factory.Create(new {OperatingCenterCode = "NOT1"});
            Assert.AreSame(one, two);
            Assert.AreNotSame(two, three);
        }

        [TestMethod]
        public void TestUniqueOperatingCenterFactoryAlwaysCreatesAUniquePredictableNewInstance()
        {
            var factory = GetFactory<UniqueOperatingCenterFactory>();
            var opc1 = factory.Create();
            var opc2 = factory.Create();
            Assert.AreNotEqual(opc1.OperatingCenterCode, opc2.OperatingCenterCode);
        }

        #endregion

        #region OrderType

        [TestMethod]
        public void TestOperationalOrderTypeFactoryReturnsWithTheCorrectId()
        {
            var factory = GetFactory<OperationalOrderTypeFactory>().Create();

            Assert.AreEqual(OrderType.Indices.OPERATIONAL_ACTIVITY_10, factory.Id);
        }

        [TestMethod]
        public void TestPlantMaintenanceOrderTypeFactoryReturnsWithTheCorrectId()
        {
            var factory = GetFactory<PlantMaintenanceOrderTypeFactory>().Create();

            Assert.AreEqual(OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11, factory.Id);
        }

        [TestMethod]
        public void TestCorrectiveActionOrderTypeFactoryReturnsWithTheCorrectId()
        {
            var factory = GetFactory<CorrectiveActionOrderTypeFactory>().Create();

            Assert.AreEqual(OrderType.Indices.CORRECTIVE_ACTION_20, factory.Id);
        }

        [TestMethod]
        public void TestRpCapitalOrderTypeFactoryReturnsWithTheCorrectId()
        {
            var factory = GetFactory<RpCapitalOrderTypeFactory>().Create();

            Assert.AreEqual(OrderType.Indices.RP_CAPITAL_40, factory.Id);
        }

        [TestMethod]
        public void TestRoutineOrderTypeFactoryReturnsWithTheCorrectId()
        {
            var factory = GetFactory<RoutineOrderTypeFactory>().Create();

            Assert.AreEqual(OrderType.Indices.ROUTINE_13, factory.Id);
        }

        #endregion

        #region OverallQualityRating

        [TestMethod]
        public void TestOverallQualityRatingFactorySetsDefaults()
        {
            var target = GetFactory<OverallQualityRatingFactory>().Build();

            Assert.AreEqual(OverallQualityRatingFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<OverallQualityRatingFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(OverallQualityRatingFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region OverallSafetyRating

        [TestMethod]
        public void TestOverallSafetyRatingFactorySetsDefaults()
        {
            var target = GetFactory<OverallSafetyRatingFactory>().Build();

            Assert.AreEqual(OverallSafetyRatingFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<OverallSafetyRatingFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(OverallSafetyRatingFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region PipeDataLookupType

        [TestMethod]
        public void TestPipeDataLookupTypeFactorySetsDefaults()
        {
            var target = GetFactory<PipeDataLookupTypeFactory>().Build();

            Assert.AreEqual(PipeDataLookupTypeFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<PipeDataLookupTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(PipeDataLookupTypeFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region PipeDataLookupValue

        [TestMethod]
        public void TestPipeDataLookupValueFactorySetsDefaults()
        {
            var target = GetFactory<PipeDataLookupValueFactory>().Build();

            Assert.AreEqual(PipeDataLookupValueFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<PipeDataLookupValueFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(PipeDataLookupValueFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region PipeDiameter

        [TestMethod]
        public void TestPipeDiameterFactorySetsDefaults()
        {
            var target = GetFactory<PipeDiameterFactory>().Build();

            Assert.AreEqual(PipeDiameterFactory.DIAMETER, target.Diameter);

            target = GetFactory<PipeDiameterFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(PipeDiameterFactory.DIAMETER, target.Diameter);
        }

        #endregion

        #region PipeMaterial

        [TestMethod]
        public void TestPipeMaterialFactorySetsDefaults()
        {
            var target = GetFactory<PipeMaterialFactory>().Build();

            Assert.AreEqual(PipeMaterialFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<PipeMaterialFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(PipeMaterialFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region PremiseType

        [TestMethod]
        public void TestPremiseTypeFactorySetsDefaults()
        {
            var target = GetFactory<PremiseTypeFactory>().Build();

            Assert.AreEqual(PremiseTypeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(PremiseTypeFactory.DEFAULT_ABBREVIATION, target.Abbreviation);

            target = GetFactory<PremiseTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(PremiseTypeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(PremiseTypeFactory.DEFAULT_ABBREVIATION, target.Abbreviation);
        }

        #endregion

        #region ProcessStage

        [TestMethod]
        public void TestProcessStageFactorySetsDefaults()
        {
            var target = GetFactory<ProcessStageFactory>().Build();

            Assert.AreEqual(ProcessStageFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ProcessStageFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ProcessStageFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region ProjectType

        [TestMethod]
        public void TestProjectTypeFactorySetsDefaults()
        {
            var target = GetFactory<RecurringProjectTypeFactory>().Build();

            Assert.AreEqual(RecurringProjectTypeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(HighCostFactorFactory.CREATED_BY, target.CreatedBy);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);

            target = GetFactory<RecurringProjectTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(RecurringProjectTypeFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(HighCostFactorFactory.CREATED_BY, target.CreatedBy);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);
        }

        #endregion

        #region PositionGroup

        [TestMethod]
        public void TestPositionGroupFactorySetsDefaults()
        {
            var target = GetFactory<PositionGroupFactory>().Build();

            Assert.AreEqual(PositionGroupFactory.BUSINESS_UNIT, target.BusinessUnit);

            target = GetFactory<PositionGroupFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(PositionGroupFactory.BUSINESS_UNIT, target.BusinessUnit);
        }

        #endregion

        #region Recurring Frequency Units

        [TestMethod]
        public void TestRecurringFrequencyUnitYearlySetsIdAndDescriptionProperly()
        {
            var target = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();

            Assert.AreEqual(RecurringFrequencyUnit.YEAR, target.Description);
            Assert.AreEqual(RecurringFrequencyUnit.Indices.YEAR, target.Id);
        }

        [TestMethod]
        public void TestRecurringFrequencyUnitYearlyReturnsSameInstanceAfterFirstIsCreated()
        {
            RecurringFrequencyUnit theOnly = null, theOther = null;

            theOnly = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();

            MyAssert.DoesNotCauseIncrease(() => theOther = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create(),
                () => _container.GetInstance<RepositoryBase<RecurringFrequencyUnit>>().GetAll().Count());

            Assert.AreSame(theOnly, theOther);
        }

        #endregion

        #region RecurringProject

        [TestMethod]
        public void TestRecurringProjectFactorySetsDefaults()
        {
            var target = GetFactory<RecurringProjectFactory>().Build();

            Assert.AreEqual(RecurringProjectFactory.PROJECT_TITLE, target.ProjectTitle);
            Assert.AreEqual(RecurringProjectFactory.DISTRICT, target.District);
            Assert.AreEqual(RecurringProjectFactory.NJAW_ESTIMATE, target.NJAWEstimate);
            Assert.IsNotNull(target.CreatedBy);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);

            target = GetFactory<RecurringProjectFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(RecurringProjectFactory.PROJECT_TITLE, target.ProjectTitle);
            Assert.AreEqual(RecurringProjectFactory.DISTRICT, target.District);
            Assert.AreEqual(RecurringProjectFactory.NJAW_ESTIMATE, target.NJAWEstimate);
            Assert.IsNotNull(target.CreatedBy);
            MyAssert.AreClose(Lambdas.GetNow(), target.CreatedAt);
        }

        #endregion

        #region RecurringProjectStatus

        [TestMethod]
        public void TestCompleteRecurringProjectStatusFactorySetsDefaults()
        {
            var target = GetFactory<CompleteRecurringProjectStatusFactory>().Build();

            Assert.AreEqual(CompleteRecurringProjectStatusFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<CompleteRecurringProjectStatusFactory>().Create();

            Assert.AreEqual(RecurringProjectStatus.Indices.COMPLETE, target.Id);
            Assert.AreEqual(CompleteRecurringProjectStatusFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        [TestMethod]
        public void TestProposedRecurringProjectStatusFactorySetsDefaults()
        {
            var target = GetFactory<ProposedRecurringProjectStatusFactory>().Build();

            Assert.AreEqual(ProposedRecurringProjectStatusFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ProposedRecurringProjectStatusFactory>().Create();

            Assert.AreEqual(RecurringProjectStatus.Indices.PROPOSED, target.Id);
            Assert.AreEqual(ProposedRecurringProjectStatusFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region RecurringProjectEndorsement

        [TestMethod]
        public void TestRecurringProjectEndorsementFactorySetsDefaults()
        {
            var target = GetFactory<RecurringProjectEndorsementFactory>().Build();

            Assert.AreEqual(RecurringProjectEndorsementFactory.COMMENT, target.Comment);
            MyAssert.AreClose(Lambdas.GetNow(), target.EndorsementDate);

            target = GetFactory<RecurringProjectEndorsementFactory>().Create();

            Assert.AreEqual(1, target.Id);
            Assert.AreEqual(RecurringProjectEndorsementFactory.COMMENT, target.Comment);
            MyAssert.AreClose(Lambdas.GetNow(), target.EndorsementDate);
        }

        #endregion

        #region RoadwayImprovementNotification

        [TestMethod]
        public void TestRoadwayImprovementNotificationFactorySetsDefaults()
        {
            var target = GetFactory<RoadwayImprovementNotificationFactory>().Build();
            var ts = TimeSpan.FromMinutes(10);

            Assert.AreEqual(RoadwayImprovementNotificationFactory.DEFAULT_DESCRIPTION, target.Description);
            MyAssert.AreClose(DateTime.Now.AddDays(10), target.ExpectedProjectStartDate, ts);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), target.DateReceived, ts);

            target = GetFactory<RoadwayImprovementNotificationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(RoadwayImprovementNotificationFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region RoleFactory

        [TestMethod]
        public void TestRoleFactoryDefaults()
        {
            var role = GetFactory<RoleFactory>().Create();
            Assert.IsNotNull(role.OperatingCenter);
            Assert.IsNotNull(role.Application);
            Assert.IsNotNull(role.Module);
            Assert.IsNotNull(role.User);
            Assert.IsNotNull(role.Action);
        }

        [TestMethod]
        public void TestRoleFactoryAddsRoleToUsersRoles()
        {
            var role = GetFactory<RoleFactory>().Build();

            Assert.IsTrue(role.User.Roles.Contains(role));

            role = GetFactory<RoleFactory>().Create();

            Assert.IsTrue(role.User.Roles.Any(r => r.Id == role.Id));
        }

        [TestMethod]
        public void TestRoleFactoryBuildSetsApplicationToModulesApplicationBecauseApplicationIsObsolete()
        {
            var role = GetFactory<RoleFactory>().Create();
            Assert.AreSame(role.Application, role.Module.Application);
        }

        [TestMethod]
        public void TestRoleFactoryBuildSetsOperatingCenterToUsersDefaultOperatingCenterIfNoOverride()
        {
            var role = GetFactory<RoleFactory>().Create();
            Assert.AreSame(role.OperatingCenter, role.User.DefaultOperatingCenter);

            var opc = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "BLUH"});
            role = GetFactory<RoleFactory>().Create(new {OperatingCenter = opc});
            Assert.AreNotSame(role.OperatingCenter, role.User.DefaultOperatingCenter);
        }

        [TestMethod]
        public void TestWildcardRoleFactorySetsOperatingCenterToNull()
        {
            Assert.IsNull(GetFactory<WildcardOpCenterRoleFactory>().Build().OperatingCenter);
            Assert.IsNull(GetFactory<WildcardOpCenterRoleFactory>().Create().OperatingCenter);
        }

        #endregion

        #region KilowattSensorMeasurementType

        [TestMethod]
        public void TestKilowattSensorMeasurementTypeFactorySetsDefaults()
        {
            var target = GetFactory<KilowattSensorMeasurementTypeFactory>().Build();

            Assert.AreEqual(SensorMeasurementType.Descriptions.KILOWATT, target.Description);

            target = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(SensorMeasurementType.Descriptions.KILOWATT, target.Description);
        }

        #endregion

        #region KilowattHoursSensorMeasurementType

        [TestMethod]
        // [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
        [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
        public void TestKilowattHoursSensorMeasurementTypeFactorySetsDefaults()
        {
            var target = GetFactory<KilowattHoursSensorMeasurementTypeFactory>().Build();

            Assert.AreEqual(SensorMeasurementType.Descriptions.KILOWATT_HOURS, target.Description);

            target = GetFactory<KilowattHoursSensorMeasurementTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(SensorMeasurementType.Descriptions.KILOWATT_HOURS, target.Description);
        }

        #endregion

        #region WattSensorMeasurementType

        [TestMethod]
        public void TestWattSensorMeasurementTypeFactorySetsDefaults()
        {
            var target = GetFactory<WattSensorMeasurementTypeFactory>().Build();

            Assert.AreEqual(SensorMeasurementType.Descriptions.WATT, target.Description);

            target = GetFactory<WattSensorMeasurementTypeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(SensorMeasurementType.Descriptions.WATT, target.Description);
        }

        #endregion

        #region Service

        [TestMethod]
        public void TestServiceFactorySetsDefaults()
        {
            var target = GetFactory<ServiceFactory>().Build();

            Assert.AreEqual(ServiceFactory.DEFAULT_SERVICE_NUMBER, target.ServiceNumber);

            target = GetFactory<ServiceFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ServiceFactory.DEFAULT_SERVICE_NUMBER, target.ServiceNumber);
        }

        #endregion

        #region ServiceInstallation

        [TestMethod]
        public void TestServiceInstallationFactorySetsDefaults()
        {
            var target = GetFactory<ServiceInstallationFactory>().Build();

            Assert.AreEqual(ServiceInstallationFactory.DEFAULT_METER_MANUFACTURER_SERIAL_NUMBER,
                target.MeterManufacturerSerialNumber);

            target = GetFactory<ServiceInstallationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ServiceInstallationFactory.DEFAULT_METER_MANUFACTURER_SERIAL_NUMBER,
                target.MeterManufacturerSerialNumber);
        }

        #endregion

        #region ServiceRestoration

        [TestMethod]
        public void TestServiceRestorationFactorySetsDefaults()
        {
            var target = GetFactory<ServiceRestorationFactory>().Build();

            Assert.AreEqual(ServiceRestorationFactory.DEFAULT_PURCHASE_ORDER_NUMBER, target.PurchaseOrderNumber);

            target = GetFactory<ServiceRestorationFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ServiceRestorationFactory.DEFAULT_PURCHASE_ORDER_NUMBER, target.PurchaseOrderNumber);
        }

        #endregion

        #region ServiceCategory

        [TestMethod]
        public void TestServiceCategoryFactorySetsDefaults()
        {
            var target = GetFactory<ServiceCategoryFactory>().Build();

            Assert.AreEqual(ServiceCategoryFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ServiceCategoryFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ServiceCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region ServiceMaterial

        [TestMethod]
        public void TestServiceMaterialFactorySetsDefaults()
        {
            var target = GetFactory<ServiceMaterialFactory>().Build();

            Assert.AreEqual(ServiceMaterialFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<ServiceMaterialFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ServiceMaterialFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region ServiceSize

        [TestMethod]
        public void TestServiceSizeFactorySetsDefaults()
        {
            var target = GetFactory<ServiceSizeFactory>().Build();

            Assert.AreEqual(ServiceSizeFactory.DEFAULT_SIZE, target.ServiceSizeDescription);

            target = GetFactory<ServiceSizeFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ServiceSizeFactory.DEFAULT_SIZE, target.ServiceSizeDescription);
        }

        #endregion

        #region SewerMainCleaning

        [TestMethod]
        public void TestSewerMainCleaningFactorySetsDefaults()
        {
            var target = GetFactory<SewerMainCleaningFactory>().Build();

            Assert.IsFalse(target.Overflow);

            target = GetFactory<SewerMainCleaningFactory>().Create();

            Assert.IsFalse(target.Overflow);
        }

        #endregion

        #region ScadaSignal

        [TestMethod]
        public void TestScadaSignalFactorySetsDefaults()
        {
            var target = GetFactory<ScadaSignalFactory>().Build();

            Assert.AreEqual(ScadaSignalFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_TAG_ID, target.TagId);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_TAG_NAME, target.TagName);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_ENGINEERING_UNITS, target.EngineeringUnits);

            target = GetFactory<ScadaSignalFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_TAG_ID, target.TagId);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_TAG_NAME, target.TagName);
            Assert.AreEqual(ScadaSignalFactory.DEFAULT_ENGINEERING_UNITS, target.EngineeringUnits);
        }

        #endregion

        #region StateFactory

        [TestMethod]
        public void TestStateFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<StateFactory>();
            var state = factory.Build();

            //Assert.AreEqual(StateFactory.DEFAULT_NAME, state.Name);
            Assert.AreEqual(StateFactory.DEFAULT_ABBREVIATION, state.Abbreviation);

            state = factory.Create();

            Assert.AreNotEqual(0, state.Id);
            //Assert.AreEqual(StateFactory.DEFAULT_NAME, state.Name);
            Assert.AreEqual(StateFactory.DEFAULT_ABBREVIATION, state.Abbreviation);
        }

        [TestMethod]
        public void TestStateFactoryDefaultNameIsAlwaysUnique()
        {
            var factory = GetFactory<StateFactory>();
            Assert.AreNotEqual(factory.Build().Name, factory.Build().Name);
            Assert.AreNotEqual(factory.Create(new {Abbreviation = "NJ"}).Name,
                factory.Create(new {Abbreviation = "QQ"}).Name);
        }

        #endregion

        #region StormWaterAssetFactory

        [TestMethod]
        public void TestStormWaterAssetFactoryBuildSetsUniqueAssetNumberByDefault()
        {
            var factory = GetFactory<StormWaterAssetFactory>();
            var first = factory.Build();
            var second = factory.Build();
            Assert.AreNotEqual(first.AssetNumber, second.AssetNumber);
        }

        [TestMethod]
        public void TestStormWaterAssetFactoryCreateSetsTownByDefault()
        {
            var result = GetFactory<StormWaterAssetFactory>().Create();
            Assert.IsNotNull(result.Town);
        }

        [TestMethod]
        public void TestStormWaterAssetFactoryCreateSetsOperatingCenterByDefault()
        {
            var result = GetFactory<StormWaterAssetFactory>().Create();
            Assert.IsNotNull(result.OperatingCenter);
        }

        [TestMethod]
        public void TestStormWaterAssetFactoryCreateCreatesCoordinateByDefault()
        {
            var result = GetFactory<StormWaterAssetFactory>().Create();
            Assert.IsNotNull(result.Coordinate);
        }

        [TestMethod]
        public void TestStormWaterAssetFactoryCreateSetsCreatedByByDefault()
        {
            var result = GetFactory<StormWaterAssetFactory>().Create();
            Assert.IsNotNull(result.CreatedBy);
        }

        [TestMethod]
        public void TestStormWaterAssetFactoryCreateListCreatesUniqueItemsInsteadOfCryingAndThrowingExceptions()
        {
            var result = GetFactory<StormWaterAssetFactory>().CreateList(10);
            Assert.AreEqual(10, result.Distinct().Count());
        }

        #endregion

        #region StormWaterAssetTypeFactory

        [TestMethod]
        public void TestSaveReturnsExistingInstanceThatMatchesSuspectsDescription()
        {
            var factory = GetFactory<StormWaterAssetTypeFactory>();
            var suspect = factory.Create(new {Description = "5'10, white, blue hoodie"});
            var match = factory.Create(new {Description = "5'10, white, blue hoodie"});
            Assert.AreSame(suspect, match);

            var wrongGuy = factory.Create(new {Description = "5'9, blue, only denim short shorts"});
            Assert.AreNotSame(suspect, wrongGuy);
        }

        #endregion

        #region TownFactory

        [TestMethod]
        public void TestTownFactorySetsUpDefaultValues()
        {
            var factory = GetFactory<TownFactory>();
            var town = factory.Build();

            //Assert.AreEqual(TownFactory.SHORT_NAME, town.ShortName);
            //Assert.AreEqual(TownFactory.FULL_NAME, town.FullName);
            //Assert.IsNotNull(town.State);
            //Assert.IsNotNull(town.County);

            town = factory.Create();

            //Assert.AreEqual(TownFactory.SHORT_NAME, town.ShortName);
            //Assert.AreEqual(TownFactory.FULL_NAME, town.FullName);
            Assert.IsNotNull(town.State);
            Assert.IsNotNull(town.County);
        }

        [TestMethod]
        public void TestTownFactoryDefaultNamesAreAlwaysUnique()
        {
            var factory = GetFactory<TownFactory>();
            Assert.AreNotEqual(factory.Build().ShortName, factory.Build().ShortName);
            Assert.AreNotEqual(factory.Create().ShortName, factory.Create().ShortName);
            var t1 = factory.Build();
            var t2 = factory.Build();
            Assert.AreNotEqual(t1.FullName, t2.FullName);
            Assert.AreNotEqual(factory.Create().FullName, factory.Create().FullName);
        }

        [TestMethod]
        public void TestTownFactorySetsUpCountyAndTownReferencesCorrectly()
        {
            var town = GetFactory<TownFactory>().Create();
            Assert.IsTrue(town.County.Towns.Contains(town), "County.Towns should include the town");
            Assert.AreSame(town.State, town.County.State, "States should be the same");

            // TODO: FIX THIS FOR DEFAULTS
            //       CountyFactory creates a State.
            //       StateFactory creates a State.
            //       Town has a State and a County for some reason.
            //       Town.State != Town.County.State when the factory builds it.
        }

        #endregion

        #region TownContactFactory

        [TestMethod]
        public void TestTownContactFactoryBuildAddsTownContactToTownsTownContacts()
        {
            var tc = GetFactory<TownContactFactory>().Build();
            Assert.IsTrue(tc.Town.TownContacts.Contains(tc));
        }

        #endregion

        #region TownSection

        [TestMethod]
        public void TestTownSectionFactorySetsDefaults()
        {
            var target = GetFactory<TownSectionFactory>().Build();

            MyAssert.Matches(new Regex("Section \\d+"), target.Description);

            target = GetFactory<TownSectionFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            MyAssert.Matches(new Regex("Section \\d+"), target.Description);
        }

        #endregion

        #region TrafficControlTicket

        [TestMethod]
        public void TestTrafficControlTicketFactorySetsDefaults()
        {
            var target = GetFactory<TrafficControlTicketFactory>().Build();

            Assert.AreEqual(TrafficControlTicketFactory.STREET_NUMBER, target.StreetNumber);
            //Assert.AreEqual("Accounting1", target.AccountingCode);

            target = GetFactory<TrafficControlTicketFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(TrafficControlTicketFactory.STREET_NUMBER, target.StreetNumber);
            //Assert.AreEqual("Accounting2", target.AccountingCode);
            MyAssert.IsInstanceOfType<Street>(target.Street);
            MyAssert.IsInstanceOfType<Town>(target.Town);
            MyAssert.IsInstanceOfType<OperatingCenter>(target.OperatingCenter);
            MyAssert.IsInstanceOfType<Coordinate>(target.Coordinate);
        }

        #endregion

        #region TrafficControlTicketCheck

        [TestMethod]
        public void TestTrafficControlTicketCheckFactorySetsDefaults()
        {
            var existing = GetFactory<TrafficControlTicketCheckFactory>().Build();

            var target = GetFactory<TrafficControlTicketCheckFactory>().Build();

            Assert.AreEqual(TrafficControlTicketCheckFactory.DEFAULT_AMOUNT, target.Amount);
            Assert.AreEqual(1, target.CheckNumber - existing.CheckNumber);

            target = GetFactory<TrafficControlTicketCheckFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(TrafficControlTicketCheckFactory.DEFAULT_AMOUNT, target.Amount);
            MyAssert.IsInstanceOfType<TrafficControlTicket>(target.TrafficControlTicket);
            Assert.AreEqual(2, target.CheckNumber - existing.CheckNumber);
        }

        #endregion

        #region TrainingContactHoursProgramCoordinator

        [TestMethod]
        public void TestTrainingContactHoursProgramCoordinatorFactorySetsDefaults()
        {
            var target = GetFactory<TrainingContactHoursProgramCoordinatorFactory>().Build();

            MyAssert.IsInstanceOfType<Employee>(target.ProgramCoordinator);

            target = GetFactory<TrainingContactHoursProgramCoordinatorFactory>().Create();

            MyAssert.IsInstanceOfType<Employee>(target.ProgramCoordinator);
            Assert.AreNotEqual(0, target.Id);
        }

        #endregion

        #region TrainingModule

        [TestMethod]
        public void TestTrainingModuleFactorySetsDefaults()
        {
            var target = GetFactory<TrainingModuleFactory>().Build();

            Assert.AreEqual(TrainingModuleFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(TrainingModuleFactory.DEFAULT_TITLE, target.Title);

            target = GetFactory<TrainingModuleFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(TrainingModuleFactory.DEFAULT_DESCRIPTION, target.Description);
            Assert.AreEqual(TrainingModuleFactory.DEFAULT_TITLE, target.Title);
        }

        #endregion

        #region TrainingModuleCategory

        [TestMethod]
        public void TestTrainingModuleCategoryFactorySetsDefaults()
        {
            var target = GetFactory<TrainingModuleCategoryFactory>().Build();

            Assert.AreEqual(TrainingModuleCategoryFactory.DEFAULT_DESCRIPTION, target.Description);

            target = GetFactory<TrainingModuleCategoryFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(TrainingModuleCategoryFactory.DEFAULT_DESCRIPTION, target.Description);
        }

        #endregion

        #region TrainingModuleRecurrantType

        [TestMethod]
        public void TestInitialTrainingModuleRecurrantTypeFactorySetsDefaults()
        {
            var target = GetFactory<InitialTrainingModuleRecurrantTypeFactory>().Build();
            Assert.AreEqual(target.Description, InitialTrainingModuleRecurrantTypeFactory.DEFAULT_DESCRIPTION);

            target = GetFactory<InitialTrainingModuleRecurrantTypeFactory>().Create();

            Assert.AreEqual(TrainingModuleRecurrantType.Indices.INITIAL, target.Id);
            Assert.AreEqual(target.Description, InitialTrainingModuleRecurrantTypeFactory.DEFAULT_DESCRIPTION);
        }

        [TestMethod]
        public void TestRecurringTrainingModuleRecurrantTypeFactorySetsDefaults()
        {
            var target = GetFactory<RecurringTrainingModuleRecurrantTypeFactory>().Build();
            Assert.AreEqual(target.Description, RecurringTrainingModuleRecurrantTypeFactory.DEFAULT_DESCRIPTION);

            target = GetFactory<RecurringTrainingModuleRecurrantTypeFactory>().Create();

            Assert.AreEqual(TrainingModuleRecurrantType.Indices.RECURRING, target.Id);
            Assert.AreEqual(target.Description, RecurringTrainingModuleRecurrantTypeFactory.DEFAULT_DESCRIPTION);
        }

        [TestMethod]
        public void TestInitialAndRecurringTrainingModuleRecurrantTypeFactorySetsDefaults()
        {
            var target = GetFactory<InitialAndRecurringTrainingModuleRecurrantTypeFactory>().Build();
            Assert.AreEqual(target.Description,
                InitialAndRecurringTrainingModuleRecurrantTypeFactory.DEFAULT_DESCRIPTION);

            target = GetFactory<InitialAndRecurringTrainingModuleRecurrantTypeFactory>().Create();

            Assert.AreEqual(TrainingModuleRecurrantType.Indices.INITIAL_RECURRING, target.Id);
            Assert.AreEqual(target.Description,
                InitialAndRecurringTrainingModuleRecurrantTypeFactory.DEFAULT_DESCRIPTION);
        }

        #endregion

        #region UserFactory

        [TestMethod]
        public void TestUserFactoryDefaultNameIsAlwaysUnique()
        {
            var factory = GetFactory<UserFactory>();
            Assert.AreNotEqual(factory.Build().UserName, factory.Build().UserName);
            Assert.AreNotEqual(factory.Create().UserName, factory.Create().UserName);
        }

        [TestMethod]
        public void TestUserFactoryDefaults()
        {
            var user = GetFactory<UserFactory>().Create();
            Assert.IsNotNull(user.DefaultOperatingCenter);
            Assert.IsTrue(user.HasAccess);
            Assert.IsFalse(user.IsAdmin);
        }

        [TestMethod]
        public void TestAdminUserFactoryDefaults()
        {
            var user = GetFactory<AdminUserFactory>().Create();
            Assert.IsNotNull(user.DefaultOperatingCenter);
            Assert.IsTrue(user.HasAccess);
            Assert.IsTrue(user.IsAdmin);
        }

        [TestMethod]
        public void TestAdminUserFactoryDefaultNameIsAlwaysUnique()
        {
            var factory = GetFactory<AdminUserFactory>();
            Assert.AreNotEqual(factory.Build().UserName, factory.Build().UserName);
            Assert.AreNotEqual(factory.Create().UserName, factory.Create().UserName);
        }

        [TestMethod]
        public void TestAdminUserFactorySetsIsAdminToTrue()
        {
            var factory = GetFactory<AdminUserFactory>();
            Assert.IsTrue(factory.Build().IsAdmin);
            Assert.IsTrue(factory.Create().IsAdmin);
        }

        #endregion

        #region Valve

        [TestMethod]
        public void TestValveFactorySetsDefaults()
        {
            var townSection = GetFactory<TownSectionFactory>().Create();
            var target = GetFactory<ValveFactory>().Build(new {TownSection = townSection});

            Assert.AreEqual(ValveFactory.DEFAULT_VALNUM, target.ValveNumber);
            Assert.IsFalse(target.BPUKPI);
            Assert.IsFalse(target.Critical);
            Assert.IsFalse(target.Traffic);

            target = GetFactory<ValveFactory>().Create();

            Assert.AreNotEqual(0, target.Id);
            Assert.AreEqual(ValveFactory.DEFAULT_VALNUM, target.ValveNumber);
            Assert.IsFalse(target.BPUKPI);
            Assert.IsFalse(target.Critical);
            Assert.IsFalse(target.Traffic);
        }

        #endregion

        #region ValveBilling

        [TestMethod]
        public void TestValvePublicBillingSetsIdProperly()
        {
            var target = GetFactory<PublicValveBillingFactory>().Create();

            Assert.AreEqual(ValveBilling.PUBLIC, target.Description);
            Assert.AreEqual(ValveBilling.Indices.PUBLIC, target.Id);
        }

        [TestMethod]
        public void TestValveCompanyBillingSetsIdProperly()
        {
            var target = GetFactory<CompanyValveBillingFactory>().Create();

            Assert.AreEqual(ValveBilling.COMPANY, target.Description);
            Assert.AreEqual(ValveBilling.Indices.COMPANY, target.Id);
        }

        [TestMethod]
        public void TestValveOAndMBillingSetsIdProperly()
        {
            var target = GetFactory<OAndMValveBillingFactory>().Create();

            Assert.AreEqual(ValveBilling.O_AND_M, target.Description);
            Assert.AreEqual(ValveBilling.Indices.O_AND_M, target.Id);
        }

        [TestMethod]
        public void TestValveMunicipalBillingSetsIdProperly()
        {
            var target = GetFactory<MunicipalValveBillingFactory>().Create();

            Assert.AreEqual(ValveBilling.MUNICIPAL, target.Description);
            Assert.AreEqual(ValveBilling.Indices.MUNICIPAL, target.Id);
        }

        //[TestMethod]
        //public void TestMultipleValveBillinsCanBeCreated()
        //{
        //    var mvb = GetFactory<MunicipalValveBillingFactory>().Create();
        //    var vob = GetFactory<OAndMValveBillingFactory>().Create();

        //    Assert.AreEqual(2, new RepositoryBase<ValveBilling>(Session).GetAll().Count());
        //}

        #endregion

        #region Valve Status

        [TestMethod]
        public void TestValveStatusActiveSetsIdAndDescriptionProperly()
        {
            var target = GetFactory<ActiveAssetStatusFactory>().Create();

            Assert.AreEqual(AssetStatus.ACTIVE, target.Description);
            Assert.AreEqual(AssetStatus.Indices.ACTIVE, target.Id);
        }

        [TestMethod]
        public void TestValveStatusPendingSetsIdAndDescriptionProperly()
        {
            var target = GetFactory<PendingAssetStatusFactory>().Create();

            Assert.AreEqual(AssetStatus.PENDING, target.Description);
            Assert.AreEqual(AssetStatus.Indices.PENDING, target.Id);
        }

        [TestMethod]
        public void TestValveStatusRetiredSetsIdAndDescriptionProperly()
        {
            var target = GetFactory<RetiredAssetStatusFactory>().Create();

            Assert.AreEqual(AssetStatus.RETIRED, target.Description);
            Assert.AreEqual(AssetStatus.Indices.RETIRED, target.Id);
        }

        //[TestMethod]
        //public void TestMultipleValveStatusCanBeCreated()
        //{
        //    var avs = GetFactory<ActiveAssetStatusFactory>().Create();
        //    var pvs = GetFactory<PendingAssetStatusFactory>().Create();
        //    var rvs = GetFactory<RetiredAssetStatusFactory>().Create();

        //    Assert.AreEqual(3, new AssetStatusFactory(Session).GetAll().Count());
        //}

        #endregion

        #region WorkersCompensationClaimStatus

        [TestMethod]
        public void TestWorkersCompensationClaimStatusNoClaimHasCorrectId()
        {
            var target = GetFactory<NoClaimWorkersCompensationClaimStatusFactory>().Create();

            Assert.AreEqual(WorkersCompensationClaimStatus.Indices.NO_CLAIM, target.Id);
        }

        [TestMethod]
        public void TestWorkersCompensationClaimStatusOpenHasCorrectId()
        {
            var target = GetFactory<OpenWorkersCompensationClaimStatusFactory>().Create();

            Assert.AreEqual(WorkersCompensationClaimStatus.Indices.OPEN, target.Id);
        }

        [TestMethod]
        public void TestWorkersCompensationClaimStatusClosedAcceptedHasCorrectId()
        {
            var target = GetFactory<ClosedAcceptedWorkersCompensationClaimStatusFactory>().Create();

            Assert.AreEqual(WorkersCompensationClaimStatus.Indices.CLOSED_ACCEPTED, target.Id);
        }

        [TestMethod]
        public void TestWorkersCompensationClaimStatusClosedDeniedHasCorrectId()
        {
            var target = GetFactory<ClosedDeniedWorkersCompensationClaimStatusFactory>().Create();

            Assert.AreEqual(WorkersCompensationClaimStatus.Indices.CLOSED_DENIED, target.Id);
        }

        #endregion

        #region WorkDescription

        [TestMethod]
        public void TestWaterMainBreakReplaceWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE, wd.Id);
        }

        [TestMethod]
        public void TestWaterMainBreakRepairWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR, wd.Id);
        }

        [TestMethod]
        public void TestSewerMainBreakReplaceWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescription.Indices.SEWER_MAIN_BREAK_REPLACE, wd.Id);
        }

        [TestMethod]
        public void TestSewerMainBreakRepairWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescription.Indices.SEWER_MAIN_BREAK_REPAIR, wd.Id);
        }

        [TestMethod]
        public void TestServiceLineRepairWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<ServiceLineRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescription.Indices.SERVICE_LINE_REPAIR, wd.Id);
        }

        [TestMethod]
        public void TestValveRepairWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<ValveRepairWorkDescriptionFactory>().Create();

            Assert.AreEqual((int)WorkDescription.Indices.VALVE_REPAIR, wd.Id);
        }

        [TestMethod]
        public void TestWaterMainBleedersWorkDescriptionHasCorrectId()
        {
            var wd = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            Assert.AreEqual((int)WorkDescription.Indices.WATER_MAIN_BLEEDERS, wd.Id);
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
            Assert.AreEqual(WorkOrderFactory.DEFAULT_STREET_OPENING_PERMIT_REQUIRED,
                workOrder.StreetOpeningPermitRequired);

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
            Assert.AreEqual(WorkOrderFactory.DEFAULT_STREET_OPENING_PERMIT_REQUIRED,
                workOrder.StreetOpeningPermitRequired);
        }

        [TestMethod]
        public void TestCompletedWorkOrderFactorySetsUpDefaultValues()
        {
            var order = GetFactory<CompletedWorkOrderFactory>().Create();

            MyAssert.AreClose(DateTime.Now, order.DateCompleted.Value);
        }

        [TestMethod]
        public void TestPlanningWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<AdminUserFactory>().Create();
            _authenticationService.Setup(x => x.CurrentUser).Returns(currentUser);
            var order = GetFactory<PlanningWorkOrderFactory>().Create();
            var planningOrders = _container.GetInstance<WorkOrderRepository>().PlanningOrders.List<WorkOrder>();
            Session.Flush();

            Assert.AreNotEqual(0, order.Id);
            Assert.AreEqual(1, planningOrders.Count);
            Assert.AreEqual(order, planningOrders[0]);
        }

        [TestMethod]
        public void TestSchedulingWorkOrderFactoryCreatesSchedulingWorkOrder()
        {
            // TODO: This test makes absolutely no sense. It tests that it "creates a scheduling work order"
            // and then it's asserting that it isn't returned by the repository? Why? -Ross 11/3/2023

            var currentUser = GetFactory<AdminUserFactory>().Create();
            _authenticationService.Setup(x => x.CurrentUser).Returns(currentUser);
            var order = GetFactory<SchedulingWorkOrderFactory>().Create(new {
                AssignedContractor = typeof(ContractorFactory)
            });
            Session.Flush();
            var schedulingOrders = _container.GetInstance<WorkOrderRepository>().SchedulingOrders.List<WorkOrder>();

            Assert.AreNotEqual(0, order.Id);
            Assert.AreEqual(0, schedulingOrders.Count);
        }

        [TestMethod]
        public void TestFinalizationWorkOrderFactoryCreateFinalizationWorkOrder()
        { 
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var contractor = GetFactory<ContractorFactory>().Create();
            contractor.OperatingCenters.Add(operatingCenter);
            Session.Save(contractor);
            Session.Flush();
            var currentUser = GetFactory<AdminUserFactory>().Create();
            _authenticationService.Setup(x => x.CurrentUser).Returns(currentUser);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                OperatingCenter = operatingCenter
            });
            Session.Flush();

            var finalizationOrders = _container.GetInstance<WorkOrderRepository>().FinalizationOrders.List<WorkOrder>();
            //  Session.Clear();

            // test will fail intermittantly without this:
            Thread.Sleep(1000);

            Assert.AreNotEqual(0, order.Id);
            Assert.AreEqual(1, finalizationOrders.Count);
            Assert.AreEqual(order, finalizationOrders[0]);
            Assert.IsNotNull(finalizationOrders[0].OperatingCenter);
        }

        [TestMethod]
        public void TestStreetOpeningPermitRequiredWorkOrderFactorySetsUpDefaultValues()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<StreetOpeningPermitRequiredWorkOrderFactory>().Create(
                    new {AssignedContractor = currentUser.Contractor});

            Assert.AreEqual(1, order.Id);
            Assert.IsTrue(order.StreetOpeningPermitRequired);
        }

        [TestMethod]
        public void TestStreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactorySetsUpDefaultValues()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory>().Create(
                    new {AssignedContractor = currentUser.Contractor});

            Assert.AreEqual(1, order.StreetOpeningPermits.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(-2), order.StreetOpeningPermits[0].DateIssued.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), order.StreetOpeningPermits[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void
            TestStreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactorySetsUpDefaultValues()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>()
                   .Create(
                        new {AssignedContractor = currentUser.Contractor});

            Assert.AreEqual((int)WorkOrderPriorityEnum.Emergency, order.Priority.Id);
            Assert.AreEqual(0, order.StreetOpeningPermits.Count);
        }

        [TestMethod]
        public void TestStreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactorySetsUpDefaultValues()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create(
                    new {AssignedContractor = currentUser.Contractor});

            Assert.AreEqual(1, order.StreetOpeningPermits.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), order.StreetOpeningPermits[0].DateIssued.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(1), order.StreetOpeningPermits[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void
            TestStreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory>().Create(
                    new {AssignedContractor = currentUser.Contractor});

            Assert.AreEqual(1, order.StreetOpeningPermits.Count);
            MyAssert.AreClose(DateTime.Today.AddDays(1), order.StreetOpeningPermits[0].DateIssued.Value);
            MyAssert.AreClose(DateTime.Today.AddDays(4), order.StreetOpeningPermits[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithNoMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<MarkoutRequirementRoutineWorkOrderFactory>().Create(
                    new {AssignedContractor = currentUser.Contractor});

            Assert.AreEqual((int)MarkoutRequirement.Indices.ROUTINE, order.MarkoutRequirement.Id);
            Assert.AreEqual(0, order.Markouts.Count);
        }

        [TestMethod]
        public void TestMarkoutRequirementEmergencyWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var order = GetFactory<MarkoutRequirementEmergencyWorkOrderFactory>().Create();

            Assert.AreEqual((int)MarkoutRequirement.Indices.EMERGENCY, order.MarkoutRequirement.Id);
        }

        [TestMethod]
        public void TestMarkoutRequirementEmergencyPermitRequiredWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var currentUser = GetFactory<ContractorUserFactory>().Create();
            var order =
                GetFactory<MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory>().Create(
                    new {AssignedContractor = currentUser.Contractor});
            Session.Flush();

            Assert.AreEqual(true, order.StreetOpeningPermitRequired);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithAnExpiredMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var order =
                GetFactory<MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory>().Create();

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(-10), order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(-1), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithAValidFutureMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var order =
                GetFactory<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>().Create();

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now.AddDays(1), order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(10), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementWithAValidMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var order =
                GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create();

            Assert.AreEqual(1, order.Markouts.Count);
            MyAssert.AreClose(DateTime.Now, order.Markouts[0].ReadyDate.Value);
            MyAssert.AreClose(DateTime.Now.AddDays(10), order.Markouts[0].ExpirationDate.Value);
        }

        [TestMethod]
        public void TestMarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactoryCreatePlanningWorkOrder()
        {
            var order =
                GetFactory<MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory>().Create();

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

        #region WorkOrderPurpose

        [TestMethod]
        public void TestVariousWorkOrderPurposeFactoriesSetIdsProperly()
        {
            Assert.AreEqual((int)WorkOrderPurpose.Indices.CUSTOMER,
                GetFactory<CustomerWorkOrderPurposeFactory>().Create().Id);
            Assert.AreEqual((int)WorkOrderPurpose.Indices.COMPLIANCE,
                GetFactory<ComplianceWorkOrderPurposeFactory>().Create().Id);
            Assert.AreEqual((int)WorkOrderPurpose.Indices.SAFETY,
                GetFactory<SafetyWorkOrderPurposeFactory>().Create().Id);
        }

        #endregion
    }
}
