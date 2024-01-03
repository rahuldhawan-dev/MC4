using MapCall.Common.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing.Data;
using StructureMap;
using MMSINC.Authentication;
using Moq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Utilities;
using MMSINC.Testing.ClassExtensions;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.Production.Models.ViewModels.SystemDeliveryEntries;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    public abstract class SystemDeliveryEntryViewModelTest<TViewModel> : ViewModelTestBase<SystemDeliveryEntry, TViewModel> where TViewModel : SystemDeliveryEntryViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;
        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void SystemDeliveryEntryViewModelTestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WeekOf);
            _vmTester.CanMapBothWays(x => x.IsValidated);
            _vmTester.CanMapBothWays(x => x.SystemDeliveryType);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop they're lists
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }

        [TestMethod]
        public virtual void TestIsSplitWeekReturnsCorrectlyWhenWeekIsSplitBetweenTwoMonths()
        {
            //Week of 7/26 starts on Monday 7/26 and ends on 8/1 - Good Test case
            var dateTime = new DateTime(2021, 7, 26);
            var otherDateTime = new DateTime(2021, 8, 9);

            Assert.IsTrue(_viewModel.IsSplitWeek(dateTime));
            Assert.IsFalse(_viewModel.IsSplitWeek(otherDateTime));
        }

        #endregion
    }

    [TestClass]
    public class CreateSystemDeliveryEntryTest : SystemDeliveryEntryViewModelTest<CreateSystemDeliveryEntryViewModel>
    {
        #region Tests
        
        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.WeekOf);
            ValidationAssert.PropertyIsRequired(x => x.Facilities);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenters);
        }
        
        [TestMethod]
        public void TestMapToEntityCreatesEntryListWithDailyValuesPreFilledToZero()
        {
            var sysDelType = GetEntityFactory<SystemDeliveryType>().Create(new {Description = "Not Water"});
            var entryType = GetEntityFactory<SystemDeliveryEntryType>().CreateList(4);
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = sysDelType, PointOfEntry = true});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = entryType[1], IsEnabled = true});

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            _entity.Facilities.Add(facility);
            _entity.WeekOf = new DateTime(2021, 8, 2);

            Session.SaveOrUpdate(facility);
            Session.SaveOrUpdate(_entity);

            _viewModel.Facilities = new[] { facility.Id };
            _viewModel.MapToEntity(_entity);

            Assert.AreNotEqual(0, _entity.FacilityEntries.Count);
            Assert.AreEqual(decimal.Zero, _entity.FacilityEntries.First().EntryValue);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsCorrectSupplierFacilityToNewEntriesList()
        {
            var sysDelType = GetEntityFactory<SystemDeliveryType>().Create(new {Description = "Not Water"});
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = sysDelType, PointOfEntry = true});
            var transferFacility = GetEntityFactory<Facility>().Create();
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {Facility = facility, SupplierFacility = transferFacility, IsEnabled = true, SystemDeliveryEntryType = systemDeliveryEntryType.FirstOrDefault(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)});

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);

            _entity.Facilities.Add(facility);
            _entity.WeekOf = new DateTime(2021, 8, 2);

            Session.SaveOrUpdate(facility);
            Session.SaveOrUpdate(_entity);

            _viewModel.Facilities = new[] { facility.Id };
            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(transferFacility.Id, _entity.FacilityEntries.First().SupplierFacility.Id);
            Assert.AreEqual(transferFacility.Description, _entity.FacilityEntries.First().SupplierFacility.Description);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsEntriesForFacilityWhenWasteWaterFromFacilityConfiguration()
        {
            var sysdelType = GetFactory<SystemDeliveryTypeFactory>().CreateAll();
            var facility = GetEntityFactory<Facility>()
               .Create(new { SystemDeliveryType = sysdelType[1], PointOfEntry = true });
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>()
               .CreateList(7, new { SystemDeliveryType = sysdelType[1] });
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = facility,
                    IsEnabled = true,
                    SystemDeliveryEntryType = systemDeliveryEntryType.FirstOrDefault(x =>
                        x.Id == SystemDeliveryEntryType.Indices.WASTEWATER_COLLECTED)
                });

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);

            _entity.Facilities.Add(facility);
            _entity.SystemDeliveryType = facility.SystemDeliveryType;
            _entity.WeekOf = new DateTime(2021, 8, 2);

            Session.SaveOrUpdate(facility);
            Session.SaveOrUpdate(_entity);
            
            _viewModel.Facilities = new[] { facility.Id };
            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(
                systemDeliveryEntryType
                   .FirstOrDefault(x => x.Id == SystemDeliveryEntryType.Indices.WASTEWATER_COLLECTED).Id,
                _entity.FacilityEntries.First().SystemDeliveryEntryType.Id);
        }

        #region SetDefaults

        [TestMethod]
        public void TestValidationFailsIfWeekOfNotAMonday()
        {
            _viewModel.WeekOf = new DateTime(2020, 12, 13);
            ValidationAssert.ModelStateHasNonPropertySpecificError("Week Of Date must be a Monday and must not be in the future.");
        }

        [TestMethod]
        public void TestValidationFailsIfMondayInTheFuture()
        {
            _viewModel.WeekOf = new DateTime(2030, 11, 28);
            ValidationAssert.ModelStateHasNonPropertySpecificError("Week Of Date must be a Monday and must not be in the future.");
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class EditSystemDeliveryEntryTest : SystemDeliveryEntryViewModelTest<EditSystemDeliveryEntryViewModel>
    {
        #region Fields

        private Mock<IEquipmentRepository> _mockEquipmentRepo;
        private Mock<IFacilityRepository> _mockFacilityRepo;
        private Mock<IEmployeeRepository> _mockEmployeeRepo;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IUserRepository> _userRepo;
        private new User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _mockEquipmentRepo = e.For<IEquipmentRepository>().Mock();
            _mockFacilityRepo = e.For<IFacilityRepository>().Mock();
            _mockEmployeeRepo = e.For<IEmployeeRepository>().Mock();
            _authServ = new Mock<IAuthenticationService<User>>();
            _userRepo = new Mock<IUserRepository>();
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEquipmentRepo = new Mock<IEquipmentRepository>();
            _mockFacilityRepo = new Mock<IFacilityRepository>();
            _mockEmployeeRepo = new Mock<IEmployeeRepository>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _userRepo = new Mock<IUserRepository>();
            _user = GetFactory<AdminUserFactory>().Create();
            _userRepo.Setup(x => x.Find(_user.Id)).Returns(_user);
            _user.IsAdmin = true;
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserId).Returns(_user.Id);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(_user.IsAdmin);
            _container.Inject(_authServ.Object);
            _container.Inject(_mockEquipmentRepo.Object);
            _container.Inject(_mockFacilityRepo.Object);
            _container.Inject(_mockEmployeeRepo.Object);
            _container.Inject(_userRepo.Object);
        }

        #endregion

        #region MapToEntity

        [TestMethod]
        public void Test_MapToEntity_MapsValuesToTheCorrectFacilityWhenCreatingEntriesForMultipleOperatingCenters()
        {
            // Arrange
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create();

            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "Thing1", 
                OperatingCenterName = "A"
            });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "Thing2", 
                OperatingCenterName = "B"
            });
            var operatingCenter3 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "Thing3",
                OperatingCenterName = "C"
            });

            var facility1 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter1 });
            var facility2 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter2 });
            var facility3 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter3 });

            _mockEmployeeRepo.Setup(x => x.Find(_user.Employee.Id)).Returns(_user.Employee);
            _mockFacilityRepo.Setup(x => x.Find(facility1.Id)).Returns(facility1);
            _mockFacilityRepo.Setup(x => x.Find(facility2.Id)).Returns(facility2);
            _mockFacilityRepo.Setup(x => x.Find(facility3.Id)).Returns(facility3);
            var list = new List<CreateSystemDeliveryFacilityEntry>();

            list.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                OperatingCenterId = facility1.OperatingCenter.Id,
                OperatingCenterDescription = facility1.OperatingCenter.Description,
                FacilityId = facility1.Id,
                FacilityName = facility1.FacilityName,
                FacilityIdWithFacilityName = facility1.FacilityId,
                SystemDeliveryType = systemDeliveryType.Id,
                SystemDeliveryEntryType = systemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EnteredBy = _user.Employee.Id,
                EntryDate = DateTime.Now,
                EntryValue = (decimal)1.2,
                WeeklyTotal = 700M,
            });

            list.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                OperatingCenterId = facility2.OperatingCenter.Id,
                OperatingCenterDescription = facility2.OperatingCenter.Description,
                FacilityId = facility2.Id,
                FacilityName = facility2.FacilityName,
                FacilityIdWithFacilityName = facility2.FacilityId,
                SystemDeliveryType = systemDeliveryType.Id,
                SystemDeliveryEntryType = systemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EnteredBy = _user.Employee.Id,
                EntryDate = DateTime.Now,
                EntryValue = (decimal)1.3
            });

            list.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                OperatingCenterId = facility3.OperatingCenter.Id,
                OperatingCenterDescription = facility3.OperatingCenter.Description,
                FacilityId = facility3.Id,
                FacilityName = facility3.FacilityName,
                FacilityIdWithFacilityName = facility3.FacilityId,
                SystemDeliveryType = systemDeliveryType.Id,
                SystemDeliveryEntryType = systemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EnteredBy = _user.Employee.Id,
                EntryDate = DateTime.Now,
                EntryValue = (decimal)1.4
            });

            // Act
            _entity.Facilities.Clear();
            _entity.Facilities.AddRange(new List<Facility> { facility1, facility2, facility3 });
            _entity.OperatingCenters.AddRange(new List<OperatingCenter>
                { operatingCenter1, operatingCenter2, operatingCenter3 });
            _viewModel.FacilityEntries = list;
            _viewModel.Facilities = new[] { facility1.Id, facility2.Id, facility3.Id };
            _viewModel.OperatingCenters = new[]
                { facility1.OperatingCenter.Id, facility2.OperatingCenter.Id, facility3.OperatingCenter.Id };
            _entity.FacilityEntries = new HashSet<SystemDeliveryFacilityEntry>();

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(_viewModel.FacilityEntries.Count, _entity.FacilityEntries.Count);
            Assert.AreEqual(_viewModel.FacilityEntries.First().EntryDate, _entity.FacilityEntries.First().EntryDate);
            Assert.AreEqual(_viewModel.FacilityEntries.First().EntryValue, _entity.FacilityEntries.First().EntryValue);
            Assert.AreEqual(_viewModel.FacilityEntries.First().FacilityId, _entity.FacilityEntries.First().Facility.Id);
            Assert.AreEqual(_viewModel.FacilityEntries.First().OperatingCenterDescription, _entity.FacilityEntries.First().Facility.OperatingCenter.Description);
            Assert.AreEqual(_viewModel.FacilityEntries.First().WeeklyTotal, _entity.FacilityEntries.First().WeeklyTotal);

            Assert.AreEqual(_viewModel.FacilityEntries.Last().EntryDate, _entity.FacilityEntries.Last().EntryDate);
            Assert.AreEqual(_viewModel.FacilityEntries.Last().EntryValue, _entity.FacilityEntries.Last().EntryValue);
            Assert.AreEqual(_viewModel.FacilityEntries.Last().FacilityId, _entity.FacilityEntries.Last().Facility.Id);
            Assert.AreEqual(_viewModel.FacilityEntries.Last().OperatingCenterDescription, _entity.FacilityEntries.Last().Facility.OperatingCenter.Description);
        }

        [TestMethod]
        public void MapToEntityAddsEntriesToCollection()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create();
            var list = new List<CreateSystemDeliveryFacilityEntry>();

            _mockEmployeeRepo.Setup(x => x.Find(_user.Employee.Id)).Returns(_user.Employee);

            list.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = systemDeliveryType.Id,
                SystemDeliveryEntryType = systemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EnteredBy = _user.Employee.Id,
                EntryDate = DateTime.Now,
                EntryValue = (decimal)1.2,
                IsInjection = true
            });
            _entity.Facilities.Clear();
            _entity.Facilities.Add(facility);
            _viewModel.FacilityEntries = list;
            _viewModel.Facilities = new[] {facility.Id};
            _entity.FacilityEntries = new HashSet<SystemDeliveryFacilityEntry>();

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(_viewModel.FacilityEntries.Count, _entity.FacilityEntries.Count);
            Assert.AreEqual(true, _entity.FacilityEntries.First().IsInjection);
            Assert.AreEqual(_viewModel.FacilityEntries.First().EntryValue, _entity.FacilityEntries.First().EntryValue);
        }

        [TestMethod]
        public void MapToEntityAddsTransferEntryWhenItsATransferFromADeliveredWaterFacility()
        {
            var facility = GetEntityFactory<Facility>().Create();
            var transferFacility = GetEntityFactory<Facility>().Create();
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {Facility = facility, SupplierFacility = transferFacility, IsEnabled = true, SystemDeliveryEntryType = systemDeliveryEntryType.FirstOrDefault(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)});
            var transferFacilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {Facility = transferFacility, SupplierFacility = transferFacility, IsEnabled = true, SystemDeliveryEntryType = systemDeliveryEntryType.FirstOrDefault(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM)});
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            transferFacility.FacilitySystemDeliveryEntryTypes.Add(transferFacilityEntryType);
            var list = new List<CreateSystemDeliveryFacilityEntry>();

            Session.SaveOrUpdate(transferFacility);
            Session.SaveOrUpdate(facility);

            _mockFacilityRepo.Setup(x => x.Find(transferFacility.Id)).Returns(transferFacility);
            _mockEmployeeRepo.Setup(x => x.Find(_user.Employee.Id)).Returns(_user.Employee);

            list.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = systemDeliveryType.Id,
                SystemDeliveryEntryType = systemDeliveryEntryType.FirstOrDefault(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO).Id,
                SystemDeliveryEntry = _entity.Id,
                SupplierFacility = transferFacility.Id,
                SupplierFacilityDesc = transferFacility.Description,
                EnteredBy = _user.Employee.Id,
                EntryValue = (decimal)3.14
            });
            _entity.Facilities.Add(facility);
            _viewModel.FacilityEntries = list;
            _viewModel.Facilities = new[] {facility.Id};
            _entity.FacilityEntries = new HashSet<SystemDeliveryFacilityEntry>();

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(2, _entity.FacilityEntries.Count);
            Assert.AreEqual(_entity.FacilityEntries.First().SupplierFacility.Id, transferFacility.Id);
            Assert.AreEqual(-_entity.FacilityEntries.First().EntryValue, _entity.FacilityEntries.Last().EntryValue);
        }

        /// <summary>
        /// Test related to bug https://americanwater.atlassian.net/browse/MC-5732
        /// </summary>
        [TestMethod]
        public void
            Test_MapToEntity_SumsValueCorrectlyWhenTransferringWaterFromOneFacilityToMultipleFacilities()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var transferFromFacility = GetEntityFactory<Facility>().Create();
            
            var transferToFacility1 = GetEntityFactory<Facility>().Create();
            transferToFacility1.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility1,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryType.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                }));
            
            var transferToFacility2 = GetEntityFactory<Facility>().Create();
            transferToFacility2.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility2,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryType.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                }));
            
            var transferToFacility3 = GetEntityFactory<Facility>().Create();
            transferToFacility2.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility3,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryType.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                }));
            
            var entryList = new List<CreateSystemDeliveryFacilityEntry> {
                new CreateSystemDeliveryFacilityEntry(_container) {
                    FacilityId = transferToFacility1.Id,
                    SystemDeliveryType = systemDeliveryType.Id,
                    SystemDeliveryEntryType = systemDeliveryEntryType.First(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO).Id,
                    SystemDeliveryEntry = _entity.Id,
                    EnteredBy = _user.Employee.Id,
                    EntryValue = 100
                },
                new CreateSystemDeliveryFacilityEntry(_container) {
                    FacilityId = transferToFacility2.Id,
                    SystemDeliveryType = systemDeliveryType.Id,
                    SystemDeliveryEntryType = systemDeliveryEntryType.First(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO).Id,
                    SystemDeliveryEntry = _entity.Id,
                    EnteredBy = _user.Employee.Id,
                    EntryValue = 200
                },
                new CreateSystemDeliveryFacilityEntry(_container) {
                    FacilityId = transferToFacility3.Id,
                    SystemDeliveryType = systemDeliveryType.Id,
                    SystemDeliveryEntryType = systemDeliveryEntryType.First(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO).Id,
                    SystemDeliveryEntry = _entity.Id,
                    EnteredBy = _user.Employee.Id,
                    EntryValue = 300
                }
            };

            _entity.Facilities.AddRange(new List<Facility> {
                transferToFacility1,
                transferToFacility2,
                transferToFacility3
            });
            _viewModel.FacilityEntries = entryList;
            _viewModel.Facilities = new[] { transferToFacility1.Id, transferToFacility2.Id, transferToFacility3.Id };
            _entity.FacilityEntries = new HashSet<SystemDeliveryFacilityEntry>();

            _viewModel.MapToEntity(_entity);

            // All good if a System Delivery Facility Entry exists that has a System Delivery Entry Type of
            // "Transferred From" and an Entry Value of -600 which is the sum of all the "Transfer To" entries
            var expression = _entity.FacilityEntries.Any(x =>
                x.EntryValue == -600 &&
                x.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            Assert.IsTrue(expression);
        }
        
        [TestMethod]
        public void Test_MapToEntity_ReMapsFacilityEntriesToEntity_WhenFacilitiesListIsChanged()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create();
            var entryList = new List<CreateSystemDeliveryFacilityEntry>();
            var facility = GetEntityFactory<Facility>().Create();
            var otherFacility = GetEntityFactory<Facility>().Create();
            var facilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = systemDeliveryEntryType, IsEnabled = true, MaximumValue = 1000.00m});
            otherFacility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryEntryType);
            var facilityEntries = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(3);
            _mockEmployeeRepo.Setup(x => x.Find(_user.Employee.Id)).Returns(_user.Employee);
            entryList.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = systemDeliveryType.Id,
                SystemDeliveryEntryType = systemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EnteredBy = _user.Employee.Id,
                EntryValue = (decimal)3.14
            });
            _entity.Facilities.Add(facility);
            _entity.FacilityEntries = facilityEntries;
            
            Session.SaveOrUpdate(otherFacility);
            Session.SaveOrUpdate(_entity);

            _viewModel.Facilities = new[] {otherFacility.Id};
            _viewModel.FacilityEntries = entryList;

            Assert.AreEqual(_entity.FacilityEntries.First().Facility.Id, facilityEntries.First().Facility.Id);

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(_entity.FacilityEntries.First().Facility.Id, otherFacility.Id);
        }

        #endregion

        #region Map

        [TestMethod]
        public void MapAddsExistingEntriesToEntryList()
        {
            var sysDelType = GetEntityFactory<SystemDeliveryType>().Create(new {Description = "Not Water"});
            var entryType = GetEntityFactory<SystemDeliveryEntryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = sysDelType, PointOfEntry = true});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = entryType, IsEnabled = true, MaximumValue = 1000.00m});
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            
            _entity.Facilities.Add(facility);
            _mockFacilityRepo.Setup(x => x.Find(facility.Id)).Returns(facility);

            Session.SaveOrUpdate(facility);

            var entryList = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(5, new {SystemDeliveryEntryType = entryType});
            _entity.FacilityEntries = entryList;

            _viewModel.Map(_entity);

            Assert.AreEqual(_viewModel.FacilityEntries.Count, _entity.FacilityEntries.Count);
            Assert.AreEqual(_viewModel.Original.Id, _entity.Id);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsEntryValueOutOfRangeForFacilitySystemDeliveryConfiguration()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryType});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {Facility = facility, MinimumValue = (decimal)2.00, MaximumValue = (decimal)10.00, IsEnabled = true});
            _viewModel.FacilityEntries.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = facility.SystemDeliveryType.Id,
                SystemDeliveryEntryType = facilityEntryType.SystemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EntryValue = (decimal)1.2
            });
            _viewModel.FacilityEntries.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = facility.SystemDeliveryType.Id,
                SystemDeliveryEntryType = facilityEntryType.SystemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EntryValue = (decimal)10.2
            });

            _mockFacilityRepo.Setup(x => x.Find(facility.Id)).Returns(facility);
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            _entity.Facilities.Add(facility);

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "Value not within range, please correct.");
        }

        [TestMethod]
        public void TestValidationDoesNotFailEntryValueOutOfRangeForFacilitySystemDeliveryConfiguration()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create() ?? throw new ArgumentNullException("GetEntityFactory<SystemDeliveryType>().Create()");
            var facility = GetEntityFactory<Facility>().Create(new { SystemDeliveryType = systemDeliveryType });
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { Facility = facility, MinimumValue = (decimal)-2.00, MaximumValue = (decimal)10.00, IsEnabled = true });
            _viewModel.FacilityEntries.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = facility.SystemDeliveryType.Id,
                SystemDeliveryEntryType = facilityEntryType.SystemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EntryValue = (decimal)-1.2
            });
            _viewModel.FacilityEntries.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                FacilityId = facility.Id,
                SystemDeliveryType = facility.SystemDeliveryType.Id,
                SystemDeliveryEntryType = facilityEntryType.SystemDeliveryEntryType.Id,
                SystemDeliveryEntry = _entity.Id,
                EntryValue = (decimal)10.00
            });

            _mockFacilityRepo.Setup(x => x.Find(facility.Id)).Returns(facility);
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            _entity.Facilities.Add(facility);

            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.Facilities);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenters);
        }

        #endregion
    }

    [TestClass]
    public class ValidateSystemDeliveryEntryViewModelTest : MapCallMvcInMemoryDatabaseTestBase<SystemDeliveryEntry>
    {
        #region Fields

        private ViewModelTester<ValidateSystemDeliveryEntryViewModel, SystemDeliveryEntry> _vmTester;
        private ValidateSystemDeliveryEntryViewModel _viewModel;
        private SystemDeliveryEntry _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private User _user;
        
        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            _viewModel = _viewModelFactory.Build<ValidateSystemDeliveryEntryViewModel, SystemDeliveryEntry>(_entity);
            _vmTester = new ViewModelTester<ValidateSystemDeliveryEntryViewModel, SystemDeliveryEntry>(_viewModel, _entity);
            _authServ = new Mock<IAuthenticationService<User>>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsIsValidatedToTrue()
        {
            _entity.IsValidated = null;
            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(true, _entity.IsValidated);
        }
        
        #endregion
    }

    [TestClass]
    public class AddSystemDeliveryEquipmentEntryReversalTest : MapCallMvcInMemoryDatabaseTestBase<SystemDeliveryEntry>
    {
        #region Fields

        private ViewModelTester<AddSystemDeliveryEquipmentEntryReversal, SystemDeliveryEntry> _vmTester;
        private AddSystemDeliveryEquipmentEntryReversal _viewModel;
        private SystemDeliveryEntry _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<SystemDeliveryEntry>().Create();
            _viewModel = _viewModelFactory.Build<AddSystemDeliveryEquipmentEntryReversal, SystemDeliveryEntry>(_entity);
            _vmTester = new ViewModelTester<AddSystemDeliveryEquipmentEntryReversal, SystemDeliveryEntry>(_viewModel, _entity);
            _authServ = new Mock<IAuthenticationService<User>>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityAddsReversalsToEntry()
        {
            var entryList = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(3,
                new { SystemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create() });
            _entity.FacilityEntries = entryList;
            var actualEntryList = entryList.ToList(); // We can't access a set via index so converting to list for test
            actualEntryList[0].IsBeingAdjusted = true;
            actualEntryList[0].AdjustedEntryValue = 22.14m;
            actualEntryList[0].EntryValue = 30.14m;
            actualEntryList[0].AdjustmentComment = "monday test comment";
            actualEntryList[1].IsBeingAdjusted = true;
            actualEntryList[1].AdjustedEntryValue = 10.14m;
            actualEntryList[1].EntryValue = 20.14m;
            actualEntryList[1].AdjustmentComment = "tuesday test comment";
            actualEntryList[2].IsBeingAdjusted = true;
            actualEntryList[2].AdjustedEntryValue = 2.14m;
            actualEntryList[2].EntryValue = 5.14m;
            actualEntryList[2].AdjustmentComment = "sunday test comment";
            foreach (var entry in entryList.ToList())
            {
                _viewModel.FacilityEntries.Add(_viewModelFactory
                   .Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(entry));
            }

            _viewModel.MapToEntity(_entity);

            var resultList = _entity.FacilityEntries.ToList();

            Assert.AreEqual(1, resultList[0].Adjustments.Count);
            Assert.AreEqual(1, resultList[1].Adjustments.Count);
            Assert.AreEqual(1, resultList[2].Adjustments.Count);

            Assert.AreEqual(resultList[0].Id, resultList[0].Adjustments[0].SystemDeliveryFacilityEntry.Id);
            Assert.AreEqual(resultList[0].EntryDate, resultList[0].Adjustments[0].AdjustedDate);
            Assert.AreEqual(resultList[0].AdjustedEntryValue, resultList[0].Adjustments[0].AdjustedEntryValue);
            Assert.AreEqual(expected: resultList[0].AdjustmentComment, actual: resultList[0].Adjustments[0].Comment);

            Assert.AreEqual(resultList[1].Id, resultList[1].Adjustments[0].SystemDeliveryFacilityEntry.Id);
            Assert.AreEqual(resultList[1].EntryDate, resultList[1].Adjustments[0].AdjustedDate);
            Assert.AreEqual(resultList[1].AdjustedEntryValue, resultList[1].Adjustments[0].AdjustedEntryValue);
            Assert.AreEqual(expected: resultList[1].AdjustmentComment, actual: resultList[1].Adjustments[0].Comment);

            Assert.AreEqual(resultList[2].Id, resultList[2].Adjustments[0].SystemDeliveryFacilityEntry.Id);
            Assert.AreEqual(resultList[2].EntryDate, resultList[2].Adjustments[0].AdjustedDate);
            Assert.AreEqual(resultList[2].AdjustedEntryValue, resultList[2].Adjustments[0].AdjustedEntryValue);
            Assert.AreEqual(expected: resultList[2].AdjustmentComment, actual: resultList[2].Adjustments[0].Comment);
        }

        [TestMethod]
        public void TestMapToEntityAddsAdjustmentForTransferEntry()
        {
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(4);
            var transferFacility = GetEntityFactory<Facility>().Create();
            var transferEntry = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new {SupplierFacility = transferFacility, EntryValue = 100m, SystemDeliveryEntryType = systemDeliveryEntryTypes[2]});
            var supplierEntry = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new {Facility = transferFacility, EntryValue = -100m, SystemDeliveryEntryType = systemDeliveryEntryTypes[3]});

            _entity.FacilityEntries.Add(transferEntry);
            _entity.FacilityEntries.Add(supplierEntry);
            
            var entryList = new List<SystemDeliveryFacilityEntry> { transferEntry };
            entryList[0].IsBeingAdjusted = true;
            entryList[0].AdjustedEntryValue = 22.14m;

            foreach (var entry in entryList.ToList())
            {
                _viewModel.FacilityEntries.Add(_viewModelFactory
                   .Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(entry));
            }

            _viewModel.MapToEntity(_entity);

            var resultList = _entity.FacilityEntries.ToList();

            Assert.AreEqual(resultList.Count(), 2);
            
            // not certain how this test was passing before, but MapToEntity changes the EntryValue, and this is properly checking that now
            Assert.AreEqual(_viewModel.FacilityEntries[0].AdjustedEntryValue, resultList[0].EntryValue);
            Assert.AreEqual((_viewModel.FacilityEntries[0].AdjustedEntryValue * -1), resultList[1].EntryValue);
            // this was added to assert the adjustment was added with the right value
            Assert.AreEqual(_viewModel.FacilityEntries[0].EntryValue, resultList[0].Adjustments.First().OriginalEntryValue);
        }

        [TestMethod]
        public void TestValidationFailsEntryValueOutOfRangeForFacilitySystemDeliveryConfiguration()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryType});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {Facility = facility, MinimumValue = (decimal)2.00, MaximumValue = (decimal)10.00, IsEnabled = true});
            var entry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {Facility = facility, facilityEntryType.SystemDeliveryEntryType});
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);

            _entity.FacilityEntries.Add(entry);
            _entity.Facilities.Add(facility);

            entry.IsBeingAdjusted = true;
            entry.AdjustedEntryValue = 1.00m;

            _viewModel.FacilityEntries.Add(_viewModelFactory
               .Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(entry));

            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "Value not within range, please correct.");
        }

        [TestMethod]
        public void Test_MapToEntity_AddsAdjustmentToTheCorrectEntryTypeWhenMoreThanOneFacilitySystemDeliveryEntryTypeExistsForAFacility()
        {
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(4);
            // See TORO WATER TREATMENT PLANT SITE - CA40-5949 System Delivery tab as an example 'Transfer To' setup
            var transferToFacility = GetEntityFactory<Facility>().Create();
            // See HIDDEN HILLS WTP SITE - CA40-5910 System Delivery tab as an example 'Transfer From' setup
            var transferFromFacility = GetEntityFactory<Facility>().Create();
            var transferToFacilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                });
            
            // The following two Facility System Delivery Entry Types are the heart of the test. The bug this test covers
            // existed when an adjustment was made to an entry for a facility that had two FSDET's and the adjustment
            // was made against the wrong one because the Linq expression used to determine which entry received
            // the adjustment didn't take into account that there may be more than one FSDET.
            var deliveredWaterFacilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER)
                });
            var transferFromFacilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferFromFacility,
                    SupplierFacility = transferToFacility,
                    IsEnabled = false,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM)
                });

            transferToFacility.FacilitySystemDeliveryEntryTypes.Add(transferToFacilitySystemDeliveryEntryType);
            transferFromFacility.FacilitySystemDeliveryEntryTypes.Add(deliveredWaterFacilitySystemDeliveryEntryType);
            transferFromFacility.FacilitySystemDeliveryEntryTypes.Add(transferFromFacilitySystemDeliveryEntryType);
            
            var transferToSystemDeliveryFacilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new {
                    Facility = transferToFacility,
                    EntryValue = 100m,
                    SupplierFacility = transferFromFacility,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                });
            
            var deliveredWaterSystemDeliveryFacilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new {
                    Facility = transferFromFacility,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER)
                });
            
            var transferFromSystemDeliveryFacilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new {
                    Facility = transferFromFacility,
                    EntryValue = -100m,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM)
                });

            _entity.FacilityEntries.Add(transferToSystemDeliveryFacilityEntry);
            _entity.FacilityEntries.Add(deliveredWaterSystemDeliveryFacilityEntry);
            _entity.FacilityEntries.Add(transferFromSystemDeliveryFacilityEntry);
            _entity.Facilities.Add(transferToFacility);
            
            // Make an adjustment to a 'Transfer To' entry type which will create an adjustment to its 
            // corresponding 'Transfer From' entry type
            var entryList = new List<SystemDeliveryFacilityEntry> { transferToSystemDeliveryFacilityEntry };
            entryList[0].IsBeingAdjusted = true;
            entryList[0].AdjustedEntryValue = 22.14m;

            foreach (var entry in entryList.ToList())
            {
                _viewModel.FacilityEntries.Add(_viewModelFactory
                   .Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(entry));
            }

            _viewModel.MapToEntity(_entity);

            var resultList = _entity.FacilityEntries.ToList();
            
            // Was the adjustment applied to the correct entry?
            Assert.AreEqual((_viewModel.FacilityEntries[0].AdjustedEntryValue * -1), resultList[2].EntryValue);
        }
        
        [TestMethod]
        public void
            Test_MapToEntity_SumsAdjustmentValueCorrectlyWhenTransferringWaterFromOneFacilityToMultipleFacilities()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var transferFromFacility = GetEntityFactory<Facility>().Create();
            
            var transferToFacility1 = GetEntityFactory<Facility>().Create();
            transferToFacility1.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility1,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                }));
            
            var transferToFacility2 = GetEntityFactory<Facility>().Create();
            transferToFacility2.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility2,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                }));
            
            var transferToFacility3 = GetEntityFactory<Facility>().Create();
            transferToFacility3.FacilitySystemDeliveryEntryTypes.Add(GetEntityFactory<FacilitySystemDeliveryEntryType>()
               .Create(new {
                    Facility = transferToFacility3,
                    SupplierFacility = transferFromFacility,
                    IsEnabled = true,
                    SystemDeliveryEntryType =
                        systemDeliveryEntryTypes.FirstOrDefault(x =>
                            x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                }));

            _entity.FacilityEntries.Add(GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = transferToFacility1,
                SupplierFacility = transferFromFacility,
                EnteredBy = _user.Employee,
                EntryValue = 100m,
                SystemDeliveryEntry = _entity,
                SystemDeliveryType = systemDeliveryType,
                SystemDeliveryEntryType =
                    systemDeliveryEntryTypes.FirstOrDefault(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
            }));
            _entity.FacilityEntries.Add(GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = transferToFacility2,
                SupplierFacility = transferFromFacility,
                EnteredBy = _user.Employee,
                EntryValue = 200m,
                SystemDeliveryEntry = _entity,
                SystemDeliveryType = systemDeliveryType,
                SystemDeliveryEntryType =
                    systemDeliveryEntryTypes.FirstOrDefault(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
            }));
            _entity.FacilityEntries.Add(GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = transferToFacility3,
                SupplierFacility = transferFromFacility,
                EnteredBy = _user.Employee,
                EntryValue = 300m,
                SystemDeliveryEntry = _entity,
                SystemDeliveryType = systemDeliveryType,
                SystemDeliveryEntryType =
                    systemDeliveryEntryTypes.FirstOrDefault(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
            }));
            _entity.FacilityEntries.Add(GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = transferFromFacility,
                EnteredBy = _user.Employee,
                EntryValue = -600m,
                SystemDeliveryEntry = _entity,
                SystemDeliveryType = systemDeliveryType,
                SystemDeliveryEntryType =
                    systemDeliveryEntryTypes.FirstOrDefault(x =>
                        x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM)
            }));
            
            _entity.Facilities.AddRange(new List<Facility> {
                transferToFacility1,
                transferToFacility2,
                transferToFacility3
            });

            var adjustedEntryList = _entity.FacilityEntries.Where(x =>
                x.EntryValue < 300m && 
                x.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO);

            foreach (var entry in adjustedEntryList)
            {
                entry.IsBeingAdjusted = true;
                entry.AdjustedEntryValue = entry.EntryValue - 50m;
                
                _viewModel.FacilityEntries.Add(_viewModelFactory
                   .Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(entry));
            }

            _viewModel.MapToEntity(_entity);
            
            // All good if a System Delivery Facility Entry exists that has a System Delivery Entry Type of
            // "Transferred From" and an Entry Value of -500 which is the sum of all the "Transfer To" entries
            var expression = _entity.FacilityEntries.Any(x =>
                x.EntryValue == -500 &&
                x.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            Assert.IsTrue(expression);
        }

        #endregion
    }
}
