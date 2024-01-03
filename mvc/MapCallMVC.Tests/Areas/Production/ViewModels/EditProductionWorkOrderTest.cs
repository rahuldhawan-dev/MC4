using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class EditProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<EditProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private EditProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private ProductionWorkDescription[] _workDescriptions;
        private OrderType[] _orderTypes;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<EditProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<EditProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);

            _orderTypes = GetFactory<OrderTypeFactory>().CreateAll().ToArray();
            _workDescriptions = CreateProductionWorkOrderTest.EnsureWorkDescriptions(GetEntityFactory<ProductionWorkDescription>(), _orderTypes);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsCreateWorkOrderToTrue()
        {
            var createWorkOrder = true;
            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _entity.SAPWorkOrder = null;
            _entity.SAPNotificationNumber = default(long?);
            _vmTester.MapToEntity();
            Assert.AreEqual(createWorkOrder, _viewModel.CreateWorkOrder);
        }

        [TestMethod]
        public void TestMapToEntitySetsFinalizeWorkOrderToTrueWhenNotPreviouslyApprovedAndSetToApproved()
        {
            _entity.OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false }); 
            _entity.SAPNotificationNumber = 1234L;
            _viewModel.ApprovedOn = DateTime.Now;

            _vmTester.MapToEntity();
            
            Assert.IsTrue(_viewModel.FinalizeWorkOrder);
        }

        [TestMethod]
        public void TestMapToEntitySetsProgressWorkOrderToTrue()
        {
            var progressWorkOrder = true;
            _entity.ApprovedOn = null;
            _entity.DateCancelled = null;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity.OperatingCenter = operatingCenter;

            _entity.SAPWorkOrder = "XYZ";
            _entity.SAPNotificationNumber = 1234L;
            _viewModel.ApprovedOn = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(progressWorkOrder, _viewModel.ProgressWorkOrder);
        }

        [TestMethod]
        public void TestMapToEntitySetsApprovedOn()
        {
            var expectedDate = DateTime.Today;
            _entity.ApprovedOn = null;
            _viewModel.ApprovedOn = expectedDate;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.ApprovedOn);
        }

        [TestMethod]
        public void TestMapToEntitySetsMaterialsApprovedOn()
        {
            var expectedDate = DateTime.Today;
            _entity.MaterialsApprovedOn = null;
            _viewModel.MaterialsApprovedOn = expectedDate;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.MaterialsApprovedOn);
        }

        [TestMethod]
        public void TestMapToEntitySetsDateCompleted()
        {
            var expectedDate = DateTime.Today;
            _entity.DateCompleted = null;
            _viewModel.DateCompleted = expectedDate;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.DateCompleted);
        }

        [TestMethod]
        public void TestMapToEntitySetsDateCancelled()
        {
            var expectedDate = DateTime.Today;
            _entity.DateCancelled = null;
            _viewModel.DateCancelled = expectedDate;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.DateCancelled);
        }

        [TestMethod]
        public void TestMapToEntitySetsCancelledBy()
        {
            _entity.DateCancelled = null;
            _entity.CancelledBy = null;
            _viewModel.DateCancelled = DateTime.Today; 
            _viewModel.CancellationReason = 1;

            _vmTester.MapToEntity();

            Assert.AreEqual(_user.UserName, _entity.CancelledBy.UserName);
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
            var hasLockoutRequirementPrerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var isConfinedSpacePrerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();

            _entity.ProductionWorkOrderProductionPrerequisites.Add(GetFactory<ProductionWorkOrderProductionPrerequisiteFactory>().Create(new {
                ProductionPrerequisite = hasLockoutRequirementPrerequisite
            }));

            _entity.ProductionWorkOrderProductionPrerequisites.Add(GetFactory<ProductionWorkOrderProductionPrerequisiteFactory>().Create(new {
                ProductionPrerequisite = isConfinedSpacePrerequisite
            }));            

            var prerequisites = GetFactory<ProductionPrerequisiteFactory>().CreateAll()
                                                                           .Where(x => x.Id != ProductionPrerequisite.Indices.RED_TAG_PERMIT)
                                                                           .Select(x => x.Id)
                                                                           .ToArray();
            _viewModel.Prerequisites = prerequisites;

            _vmTester.MapToEntity();

            CollectionAssert.AreEqual(prerequisites, _entity.ProductionWorkOrderProductionPrerequisites.Select(x => x.ProductionPrerequisite.Id).ToArray());
        }

        [TestMethod]
        public void TestMapToEntityMapsPrerequisitesAndDoesNotRemoveAnyThatWereNotInViewModel()
        {
            var hasLockoutRequirementPrerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var isConfinedSpacePrerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var airPermitPrerequisite = GetFactory<AirPermitProductionPrerequisiteFactory>().Create();

            _entity.ProductionWorkOrderProductionPrerequisites.Add(GetFactory<ProductionWorkOrderProductionPrerequisiteFactory>().Create(new {
                ProductionPrerequisite = hasLockoutRequirementPrerequisite
            }));

            _entity.ProductionWorkOrderProductionPrerequisites.Add(GetFactory<ProductionWorkOrderProductionPrerequisiteFactory>().Create(new {
                ProductionPrerequisite = isConfinedSpacePrerequisite
            }));

            var prerequisites = new[] { airPermitPrerequisite.Id };
            _viewModel.Prerequisites = prerequisites;

            _vmTester.MapToEntity();

            var expected = new[] {
                hasLockoutRequirementPrerequisite.Id, 
                isConfinedSpacePrerequisite.Id, 
                airPermitPrerequisite.Id
            };

            var actual = _entity.ProductionWorkOrderProductionPrerequisites
                                .Select(x => x.ProductionPrerequisite.Id)
                                .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotMapRedTagPermitPrerequisiteWhenEquipmentIsNull()
        {
            var redTagPermitPrerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();
            var hasLockoutRequirementPrerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var isConfinedSpacePrerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();

            _entity.ProductionWorkOrderProductionPrerequisites.Add(GetFactory<ProductionWorkOrderProductionPrerequisiteFactory>().Create(new {
                ProductionPrerequisite = hasLockoutRequirementPrerequisite
            }));

            _viewModel.Prerequisites = new[] { 
                redTagPermitPrerequisite.Id, 
                hasLockoutRequirementPrerequisite.Id,
                isConfinedSpacePrerequisite.Id
            };

            _viewModel.Equipment = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.ProductionWorkOrderProductionPrerequisites.Count);
            Assert.AreEqual(hasLockoutRequirementPrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[0].ProductionPrerequisite.Id);
            Assert.AreEqual(isConfinedSpacePrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[1].ProductionPrerequisite.Id);
        }

        [TestMethod]
        public void TestMapToEntityDoesMapRedTagPermitPrerequisiteWhenEquipmentIsNotNull()
        {
            var redTagPermitPrerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();
            var hasLockoutRequirementPrerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var isConfinedSpacePrerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();

            _entity.ProductionWorkOrderProductionPrerequisites.Add(GetFactory<ProductionWorkOrderProductionPrerequisiteFactory>().Create(new {
                ProductionPrerequisite = hasLockoutRequirementPrerequisite
            }));

            _viewModel.Prerequisites = new[] { 
                redTagPermitPrerequisite.Id, 
                hasLockoutRequirementPrerequisite.Id,
                isConfinedSpacePrerequisite.Id
            };

            _viewModel.Equipment = 1;

            _vmTester.MapToEntity();

            Assert.AreEqual(3, _entity.ProductionWorkOrderProductionPrerequisites.Count);
            Assert.AreEqual(hasLockoutRequirementPrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[0].ProductionPrerequisite.Id);
            Assert.AreEqual(redTagPermitPrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[1].ProductionPrerequisite.Id);
            Assert.AreEqual(isConfinedSpacePrerequisite.Id, _entity.ProductionWorkOrderProductionPrerequisites[2].ProductionPrerequisite.Id);
        }

        #endregion
    }
}
