using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.ComponentModel;
using System.Linq;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CreateProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<CreateProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private CreateProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IOperatingCenterRepository> _operatingCenterRepository;
        private ProductionWorkDescription[] _workDescriptions;
        private OrderType[] _orderTypes;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            _authServ = i.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = i.For<IDateTimeProvider>().Mock();
            _operatingCenterRepository = i.For<IOperatingCenterRepository>().Mock();
        }

        internal static ProductionWorkDescription[] EnsureWorkDescriptions(TestDataFactory<ProductionWorkDescription> workDescriptionFactory, OrderType[] orderTypes)
        {
            return new[] {
                workDescriptionFactory.Create(new { OrderType = orderTypes[0] }),
                workDescriptionFactory.Create(new { OrderType = orderTypes[1] }),
                workDescriptionFactory.Create(new { OrderType = orderTypes[2] }),
                workDescriptionFactory.Create(new { OrderType = orderTypes[3] }),
                workDescriptionFactory.Create(new { OrderType = orderTypes[4] })
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<CreateProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<CreateProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);
            
            _orderTypes = GetFactory<OrderTypeFactory>().CreateAll().ToArray();
            _workDescriptions = EnsureWorkDescriptions(GetEntityFactory<ProductionWorkDescription>(), _orderTypes);
        }

        #endregion

        #region Breakdown Indicator Test

        [TestMethod]
        public void TestBreakdownIndicatorForProductionWorkOrders()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var coordinateFacility = GetEntityFactory<Coordinate>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { PlanningPlant = planningPlant, Coordinate = coordinateFacility });
            var facilityFacilityArea = GetEntityFactory<FacilityFacilityArea>().Create();
            var equipmentType = GetEntityFactory<EquipmentType>().Create();
            var functionalLocation = GetEntityFactory<FunctionalLocation>().Create();

            var equipment = GetEntityFactory<Equipment>().Create(new {
                OperatingCenter = operatingCenter,
                Coordinate = coordinate,
                Facility = facility,
                FacilityFacilityArea = facilityFacilityArea,
                EquipmentType = equipmentType,
                FunctionalLocation = functionalLocation.Description
            });

            var target = new CreateProductionWorkOrder(_container, equipment);

            var targetCorrectiveProductionWorkOrder = new CreateProductionWorkOrder(_container, equipment);
            
            //Breakdown Indicator should be true for Corrective Production Work Orders
            var orderType = GetFactory<CorrectiveActionOrderTypeFactory>().Create();
            var productionWorkDescription = GetFactory<ProductionWorkDescriptionFactory>().Create(new { EquipmentType = equipmentType, OrderType = orderType, BreakdownIndicator = true});
            targetCorrectiveProductionWorkOrder.ProductionWorkDescription = productionWorkDescription.Id;
            var productionWorkOrder = new ProductionWorkOrder();
            Assert.AreEqual(true, targetCorrectiveProductionWorkOrder.MapToEntity(productionWorkOrder).BreakdownIndicator);

            //Breakdown Indicator should be false for Operational Production Work Orders
            orderType = GetFactory<OperationalOrderTypeFactory>().Create();
            productionWorkDescription = GetFactory<ProductionWorkDescriptionFactory>().Create(new { EquipmentType = equipmentType, OrderType = orderType, BreakdownIndicator = false});
            targetCorrectiveProductionWorkOrder.ProductionWorkDescription = productionWorkDescription.Id;
            productionWorkOrder = new ProductionWorkOrder();
            Assert.AreEqual(false, targetCorrectiveProductionWorkOrder.MapToEntity(productionWorkOrder).BreakdownIndicator);
        }
        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsDateReceived()
        {
            var expectedDate = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.DateReceived);
        }

        [TestMethod]
        public void TestAssignToSelfCreatesAndAddsAssignmentForSelf()
        {
            var expectedDate = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var employee = GetEntityFactory<Employee>().Create();
            _user.Employee = employee;
            _viewModel.AssignToSelf = true;

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.EmployeeAssignments.Count);
            var firstEmployeeAssignment = _entity.EmployeeAssignments.First();
            Assert.AreSame(employee, firstEmployeeAssignment.AssignedTo);
            Assert.AreSame(employee, firstEmployeeAssignment.AssignedBy);
            MyAssert.AreClose(expectedDate, firstEmployeeAssignment.AssignedFor);
            MyAssert.AreClose(expectedDate, firstEmployeeAssignment.AssignedOn);
        }

        [TestMethod]
        public void TestEquipmentIsRequiredForCorrectiveOperationalAndCapitalOrders()
        {
            var equipment = GetEntityFactory<Equipment>().Create();
            _viewModel.CorrectiveOrderProblemCode = GetEntityFactory<CorrectiveOrderProblemCode>().Create().Id;
            _viewModel.ProductionWorkDescription = _workDescriptions.Single(d =>
                d.OrderType.Id == OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11).Id;

            ValidationAssert.ModelStateIsValid(_viewModel);

            foreach (var typeId in new[] {
                         OrderType.Indices.CORRECTIVE_ACTION_20, OrderType.Indices.OPERATIONAL_ACTIVITY_10,
                         OrderType.Indices.RP_CAPITAL_40
                     })
            {
                _viewModel.Equipment = null;
                _viewModel.ProductionWorkDescription = _workDescriptions.Single(d => d.OrderType.Id == typeId).Id;

                ValidationAssert.PropertyIsRequired(_viewModel, x => x.Equipment);

                _viewModel.Equipment = equipment.Id;

                ValidationAssert.ModelStateIsValid(_viewModel);
            }
        }

        [TestMethod]
        public void TestMapToEntityMapsPrerequisites()
        {
            // Red Tag Permits handled in other test methods.
            var prerequisites = GetFactory<ProductionPrerequisiteFactory>().CreateAll()
                                                                           .Where(x => x.Id != ProductionPrerequisite.Indices.RED_TAG_PERMIT)
                                                                           .Select(x => x.Id)
                                                                           .ToArray();
            _viewModel.Prerequisites = prerequisites;

            _vmTester.MapToEntity();

            CollectionAssert.AreEqual(prerequisites, _entity.ProductionWorkOrderProductionPrerequisites.Select(x => x.ProductionPrerequisite.Id).ToArray());
        }

        [TestMethod]
        public void TestMapToEntityDoesNotMapRedTagPermitPrerequisiteWhenEquipmentIsNull()
        {
            var redTagPermitPrerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();
            var isConfinedSpacePrerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();

            _viewModel.Prerequisites = new int[] { redTagPermitPrerequisite.Id, isConfinedSpacePrerequisite.Id };
            _viewModel.Equipment = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.ProductionWorkOrderProductionPrerequisites.Count);
            Assert.AreEqual(isConfinedSpacePrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[0].ProductionPrerequisite.Id);
        }

        [TestMethod]
        public void TestMapToEntityDoesMapRedTagPermitPrerequisiteWhenEquipmentIsNotNull()
        {
            var redTagPermitPrerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();
            var isConfinedSpacePrerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();

            _viewModel.Prerequisites = new int[] { redTagPermitPrerequisite.Id, isConfinedSpacePrerequisite.Id };
            _viewModel.Equipment = GetEntityFactory<Equipment>().Create().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.ProductionWorkOrderProductionPrerequisites.Count);
            Assert.AreEqual(redTagPermitPrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[0].ProductionPrerequisite.Id);
            Assert.AreEqual(isConfinedSpacePrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[1].ProductionPrerequisite.Id);
        }

        [TestMethod]
        public void TestMapToEntityMapsCoordinatesFromFacilityWhenCoordinatesAreNull()
        {
            var facility = GetEntityFactory<Facility>().Create();
            _viewModel.Facility = facility.Id;
            _viewModel.Coordinate = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(facility.Id, _entity.Facility.Id);
            Assert.AreEqual(facility.Coordinate.Id, _entity.Coordinate.Id);
        }

        [TestMethod]
        public void TestMapToEntityMapsCoordinatesFromFacilityWhenCoordinatesAreNullAndFacilityCoordinateAreNull()
        {
            var facility = GetEntityFactory<Facility>().Create();
            facility.Coordinate = null;
            _viewModel.Facility = facility.Id;
            _viewModel.Coordinate = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(facility.Id, _entity.Facility.Id);
            Assert.IsNull(_entity.Facility.Coordinate);
            Assert.IsNull(_entity.Coordinate);
        }

        [TestMethod]
        public void TestMapToEntitySetsFacilityArea()
        {
            var facilityArea = GetEntityFactory<FacilityArea>().Create();
            var facilityFacilityArea = GetEntityFactory<FacilityFacilityArea>().Create(new { FacilityArea = facilityArea });
            _viewModel.FacilityFacilityArea = facilityFacilityArea.Id;

            _vmTester.MapToEntity();

            Assert.AreSame(facilityFacilityArea, _entity.FacilityFacilityArea);
            Assert.AreSame(facilityArea, _entity.FacilityFacilityArea.FacilityArea);
        }

        [TestMethod]
        public void TestCreateNewViewModelSetsProperties()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var coordinateFacility = GetEntityFactory<Coordinate>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { PlanningPlant = planningPlant, Coordinate = coordinateFacility });
            var facilityFacilityArea = GetEntityFactory<FacilityFacilityArea>().Create();
            var equipmentType = GetEntityFactory<EquipmentType>().Create();
            var functionalLocation = GetEntityFactory<FunctionalLocation>().Create();
            
            var equipment = GetEntityFactory<Equipment>().Create(new {
                OperatingCenter = operatingCenter,
                Coordinate = coordinate,
                Facility = facility,
                FacilityFacilityArea = facilityFacilityArea,
                EquipmentType = equipmentType,
                FunctionalLocation = functionalLocation.Description
            });

            var target = new CreateProductionWorkOrder(_container, equipment);
            
            Assert.AreEqual(operatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(planningPlant.Id, target.PlanningPlant);
            Assert.AreEqual(coordinate.Id, target.Coordinate);
            Assert.AreEqual(facility.Id, target.Facility);
            Assert.AreEqual(facilityFacilityArea.Id, target.FacilityFacilityArea);
            Assert.AreEqual(equipmentType.Id, target.EquipmentType);
            Assert.AreEqual(functionalLocation.Description, target.FunctionalLocation);
        }

        #endregion
    }
}
