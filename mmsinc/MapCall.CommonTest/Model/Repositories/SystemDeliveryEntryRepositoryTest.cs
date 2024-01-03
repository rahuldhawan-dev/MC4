using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SystemDeliveryEntryRepositoryTest : MapCallMvcSecuredRepositoryTestBase<SystemDeliveryEntry, TestSystemDeliveryEntryRepository, User>
    {
        #region Fields

        private DateTime _now;
        
        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLinqDoesNotReturnSysDelEntriesFromOtherOperatingCentersForUser()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Production});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionSystemDeliveryEntry});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opcPrime});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opcPrime,
                User = user
            });

            Session.Save(user);

            var validSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var notValidSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();

            validSystemDeliveryEntry.OperatingCenters.Add(opcPrime);
            notValidSystemDeliveryEntry.OperatingCenters.Add(opcSecondary);

            Session.Save(validSystemDeliveryEntry);
            Session.Save(notValidSystemDeliveryEntry);
            Session.Flush();

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestSystemDeliveryEntryRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validSystemDeliveryEntry));
            Assert.IsFalse(result.Contains(notValidSystemDeliveryEntry));
        }

        [TestMethod]
        public void TestLinqReturnsAllTheGasMonitorsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Production});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionSystemDeliveryEntry});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var validSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var notValidSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();

            validSystemDeliveryEntry.OperatingCenters.Add(opcPrime);
            notValidSystemDeliveryEntry.OperatingCenters.Add(opcSecondary);

            Session.Save(validSystemDeliveryEntry);
            Session.Save(notValidSystemDeliveryEntry);
            Session.Flush();

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestSystemDeliveryEntryRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validSystemDeliveryEntry));
            Assert.IsTrue(result.Contains(notValidSystemDeliveryEntry));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnSystemDeliveryEntriesFromOtherOperatingCentersForUser()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Production});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionSystemDeliveryEntry});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opcPrime});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opcPrime,
                User = user
            });

            Session.Save(user);

            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcSecondary});
            var validSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var notValidSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var validEntryList = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(1, new {Facility = facility, SystemDeliveryEntry = validSystemDeliveryEntry});
            var notValidEntryList = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(1, new {Facility = facility2, SystemDeliveryEntry = notValidSystemDeliveryEntry});

            validSystemDeliveryEntry.FacilityEntries = validEntryList;
            validSystemDeliveryEntry.OperatingCenters.Add(opcSecondary);
            validSystemDeliveryEntry.OperatingCenters.Add(opcPrime);
            notValidSystemDeliveryEntry.FacilityEntries = notValidEntryList;

            Session.Save(validSystemDeliveryEntry);
            Session.Save(notValidSystemDeliveryEntry);
            Session.Flush();

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestSystemDeliveryEntryRepository>();

            var result = Repository.iCanHasCriteria().List<SystemDeliveryEntry>();

            Assert.IsTrue(result.Contains(validSystemDeliveryEntry));
            Assert.IsFalse(result.Contains(notValidSystemDeliveryEntry));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheSystemDeliveryEntriesIfUserHasMatchingRoleWithWildCardOperatingCenter()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Production});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionSystemDeliveryEntry});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcSecondary});
            var validSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var notValidSystemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create();
            var validEntryList = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(1, new {Facility = facility, SystemDeliveryEntry = validSystemDeliveryEntry});
            var notValidEntryList = GetEntityFactory<SystemDeliveryFacilityEntry>().CreateSet(1, new {Facility = facility2, SystemDeliveryEntry = notValidSystemDeliveryEntry});

            validSystemDeliveryEntry.FacilityEntries = validEntryList;
            validSystemDeliveryEntry.OperatingCenters.Add(opcSecondary);
            validSystemDeliveryEntry.OperatingCenters.Add(opcPrime);
            notValidSystemDeliveryEntry.FacilityEntries = notValidEntryList;

            Session.Save(validSystemDeliveryEntry);
            Session.Save(notValidSystemDeliveryEntry);
            Session.Flush();
            
            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestSystemDeliveryEntryRepository>();

            var result = Repository.iCanHasCriteria().List<SystemDeliveryEntry>();

            Assert.IsTrue(result.Contains(validSystemDeliveryEntry));
            Assert.IsTrue(result.Contains(notValidSystemDeliveryEntry));
        }

        [TestMethod]
        public void TestSearchSystemDeliveryEntriesShouldReturnCorrectEntriesBasedOffSearchCriteria()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opc});
            var otherFacility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opc2});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _now});
            var sysdel2 = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _now.AddDays(7)});
            var facilityEntry1 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = facility, SystemDeliveryEntry = sysdel1, SystemDeliveryEntryType = entryTypes[0], EnteredBy = employee, EntryDate = _now });
            var facilityEntry2 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = otherFacility, SystemDeliveryEntry = sysdel2, SystemDeliveryEntryType = entryTypes[1], EnteredBy = employee, EntryDate = _now });
            
            sysdel1.OperatingCenters.Add(opc);
            sysdel2.OperatingCenters.Add(opc2);
            sysdel1.Facilities.Add(facility);
            sysdel2.Facilities.Add(otherFacility);
            sysdel1.FacilityEntries.Add(facilityEntry1);
            sysdel2.FacilityEntries.Add(facilityEntry2);

            Session.Save(facilityEntry1);
            Session.Save(facilityEntry2);
            Session.Save(sysdel1);
            Session.Save(sysdel2);
            Session.Flush();

            // Act

            var search = new SearchSystemDeliveryEntry {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchSystemDeliveryEntries(search);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(sysdel1.Id, results.First().Id);
            Assert.AreEqual(opc.Description, results.First().OperatingCenter);
            Assert.AreEqual(facility.FacilityIdWithRegionalPlanningArea, results.First().Facility);
            Assert.AreEqual(entryTypes[0].Description, results.First().SystemDeliveryEntryType);
            Assert.AreEqual((false).ToString("yn"), results.First().Adjustment);
            Assert.AreEqual(employee.FullName, results.First().EnteredBy);
            Assert.AreEqual(_now.Date, results.First().EntryDate.Date);
            Assert.AreEqual(facilityEntry1.EntryValue, results.First().Value);
            Assert.IsTrue(results.Any(x => x.EntryDate.ToString() == facilityEntry1.EntryDate.ToString()));
        }

        [TestMethod]
        public void TestSearchSystemDeliveryEntriesShouldReturnCorrectValueIfAdjustmentWasEntered()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opc});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _now});
            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryEntryType = entryTypes[0],
                EnteredBy = employee,
                EntryDate = _now,
                EntryValue = 42.00m,
                HasBeenAdjusted = true,
                OriginalEntryValue = 3.14m,
                AdjustmentComment = "My name is Luka, I live on the second floor"
            });
            
            facilityEntry.Adjustments.Add(new SystemDeliveryFacilityEntryAdjustment {
                AdjustedDate = facilityEntry.EntryDate,
                DateTimeEntered = _now,
                Facility = facility,
                EnteredBy = employee,
                OriginalEntryValue = 3.14m,
                AdjustedEntryValue = 42.00m,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryFacilityEntry = facilityEntry,
                Comment = "My name is Luka, I live on the second floor"
            });

            sysdel1.OperatingCenters.Add(opc);
            sysdel1.Facilities.Add(facility);
            sysdel1.FacilityEntries.Add(facilityEntry);

            Session.Save(facilityEntry);
            Session.Save(sysdel1);
            Session.Flush();

            var search = new SearchSystemDeliveryEntry {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchSystemDeliveryEntries(search).ToList();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual((true).ToString("yn"), results.First().Adjustment);
            Assert.AreEqual(facilityEntry.Adjustments.First().AdjustedEntryValue, results.First().Value);
            Assert.AreEqual(facilityEntry.Adjustments.First().OriginalEntryValue, results.First().OriginalEntry);
            Assert.AreEqual(facilityEntry.Adjustments.First().Comment, results.First().Comment);
        }

        [TestMethod]
        public void TestSearchSystemDeliveryEntriesShouldReturnCorrectAdjustmentIfATransfer()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opc, PublicWaterSupply = publicWaterSupply});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _now});
            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO),
                EnteredBy = employee,
                EntryDate = _now,
                EntryValue = 1.00m,
                HasBeenAdjusted = true
            });
            
            sysdel1.OperatingCenters.Add(opc);
            sysdel1.Facilities.Add(facility);
            sysdel1.FacilityEntries.Add(facilityEntry);
            
            Session.Save(facilityEntry);
            Session.Save(sysdel1);
            Session.Flush();

            // Act

            var search = new SearchSystemDeliveryEntry {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchSystemDeliveryEntries(search);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual((true).ToString("yn"), results.First().Adjustment);
            Assert.AreEqual(publicWaterSupply.Description, results.First().PublicWaterSupply);
        }

        [TestMethod]
        public void TestSearchSystemDeliveryEntriesShouldReturnPurchaseSupplierIfAPurchase()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opc, PublicWaterSupply = publicWaterSupply});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var facilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = entryTypes[0], PurchaseSupplier = "the place that we bought water from apparently", IsEnabled = true});
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _now});
            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryEntryType = entryTypes[0],
                EnteredBy = employee,
                EntryDate = _now,
                EntryValue = 3.14m
            });
            
            facility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryEntryType);
            sysdel1.OperatingCenters.Add(opc);
            sysdel1.Facilities.Add(facility);
            sysdel1.FacilityEntries.Add(facilityEntry);

            Session.Flush();
            Session.Evict(sysdel1);

            // Act
            var search = new SearchSystemDeliveryEntry {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchSystemDeliveryEntries(search);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(facilitySystemDeliveryEntryType.PurchaseSupplier, results.First().PurchaseSupplier);
        }

        [TestMethod]
        public void TestSearchSystemDeliveryEntriesShouldNotBlowUpIfFacilityHasANullValueForPublicWaterSupplyWhenATransfer()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opc});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var facilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {SystemDeliveryEntryType = entryTypes[2], IsEnabled = true});
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = _now});
            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO),
                EnteredBy = employee,
                EntryDate = _now,
                EntryValue = 3.14m
            });
            
            facility.PublicWaterSupply = null;
            facility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryEntryType);
            sysdel1.OperatingCenters.Add(opc);
            sysdel1.Facilities.Add(facility);
            sysdel1.FacilityEntries.Add(facilityEntry);

            Session.Flush();
            Session.Evict(sysdel1);

            // Act
            var search = new SearchSystemDeliveryEntry {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchSystemDeliveryEntries(search);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(null, results.First().PublicWaterSupply);
        }

        [TestMethod]
        public void TestSearchPublicWaterSupplyForAllSystemDeliveryEntryTypes()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(); 

            var facility = GetEntityFactory<Facility>().Create(new {
                OperatingCenter = opc,
                PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create()
            });
            
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(10);
            var facilitySystemDeliveryEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new { SystemDeliveryEntryType = systemDeliveryEntryTypes[2], IsEnabled = true });
            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create(new { WeekOf = _now });
            var facilityEntries = new List<SystemDeliveryFacilityEntry>();

            foreach (var systemDeliveryEntryType in systemDeliveryEntryTypes)
            {
                var systemDeliveryFacilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                    SystemDeliveryEntryType = systemDeliveryEntryType,
                    Facility = facility,
                    SystemDeliveryEntry = systemDeliveryEntry,
                    EntryDate = _now
                });

                facilityEntries.Add(systemDeliveryFacilityEntry);
            }
            
            facility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryEntryType);
            systemDeliveryEntry.OperatingCenters.Add(opc);
            systemDeliveryEntry.Facilities.Add(facility);
            systemDeliveryEntry.FacilityEntries.AddRange(facilityEntries);
            
            Session.Flush();
            Session.Evict(systemDeliveryEntry);

            var search = new SearchSystemDeliveryEntry {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchSystemDeliveryEntries(search).ToList();
            
            for (int i = 0; i < facilityEntries.Count; i++)
            {
                Assert.AreEqual(facility.PublicWaterSupply.Description, results[i].PublicWaterSupply);
                Assert.AreEqual(facilityEntries[i].SystemDeliveryType.Description, results[i].SystemDeliveryType);
            }
        }

        [TestMethod]
        public void TestGetDataForSystemDeliveryEntryFileDumpReturnsCorrectData()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryType});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new {UpdatedAt = _now, IsValidated = true, WeekOf = _now.AddMonths(-1).Date});
            var sysdel2 = GetEntityFactory<SystemDeliveryEntry>().Create(new {UpdatedAt = _now.AddMonths(-3), IsValidated = true, WeekOf = _now.AddMonths(-3)});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType =
                    entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                BusinessUnit = 210100, IsEnabled = true
            });
            var facilityEntryTypeTwo = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType =
                    entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER),
                BusinessUnit = 110101, IsEnabled = true
            });
            
            var facilityEntry1 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                EnteredBy = employee,
                EntryDate = _now.AddMonths(-1).Date,
                EntryValue = 3.14m
            });
            var facilityEntry2 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel1,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER),
                EnteredBy = employee,
                EntryDate = _now.AddMonths(-1).Date,
                EntryValue = 3.14m
            });
            
            var facilityEntry3 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel2,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                EnteredBy = employee,
                EntryDate = _now,
                EntryValue = 3.14m
            });
            var facilityEntry4 = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntry = sysdel2,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER),
                EnteredBy = employee,
                EntryDate = _now,
                EntryValue = 3.14m
            });

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryTypeTwo);
            sysdel1.FacilityEntries.Add(facilityEntry1);
            sysdel1.FacilityEntries.Add(facilityEntry2);
            sysdel2.FacilityEntries.Add(facilityEntry3);
            sysdel2.FacilityEntries.Add(facilityEntry4);

            Session.Flush();

            var results = Repository.GetDataForSystemDeliveryEntryFileDump(_now.AddMonths(-1).GetBeginningOfMonth()).ToList();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(facility.FacilityName.ToUpper(), results[0].FacilityName);
            Assert.AreEqual(sysdel1.WeekOf.Month.ToString(), results[0].Month);
            Assert.AreEqual(sysdel1.WeekOf.Year.ToString(), results[0].Year);
            Assert.AreEqual("0000110101", results[0].BusinessUnit);
            Assert.AreEqual("SYSTEM DELIVERY", results[0].SystemDeliveryDescription);
            Assert.AreEqual("AS POSTED", results[0].AsPostedDescription);
            Assert.AreEqual("SYS_PURCHASE", results[0].EntryDescription);

            Assert.AreEqual(facility.FacilityName.ToUpper(), results[1].FacilityName);
            Assert.AreEqual(sysdel1.WeekOf.Month.ToString(), results[1].Month);
            Assert.AreEqual(sysdel1.WeekOf.Year.ToString(), results[1].Year);
            Assert.AreEqual("0000210100", results[1].BusinessUnit);
            Assert.AreEqual("SYSTEM DELIVERY", results[1].SystemDeliveryDescription);
            Assert.AreEqual("AS POSTED", results[1].AsPostedDescription);
            Assert.AreEqual("SYS_NORMAL", results[1].EntryDescription);
        }
        
        // To make this more clear, this test is testing that when sys del data is entered on a split week IE week of 5/31
        // that the repo method is pulling that in and calculating out the correct total for the month for the entry
        // this method is running for June data on July 4th. So the entry of 5/31 should be ignored for the June file.
        // and the correct total should be 73.20 not 85.40
        [TestMethod]
        public void TestGetDataReturnsLastSixWeeksOfData()
        {
            var now = new DateTime(2021, 7, 4);
            var entryMondayDate = new DateTime(2021, 5, 31);
            var followingWeek = entryMondayDate.AddWeeks(1);
            var entryValue = 12.20M;
            var employee = GetEntityFactory<Employee>().Create();
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryType});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var sysdel1 = GetEntityFactory<SystemDeliveryEntry>().Create(new
                {UpdatedAt = _now, IsValidated = true, WeekOf = entryMondayDate.Date});
            var sysdel2 = GetEntityFactory<SystemDeliveryEntry>().Create(new
                {UpdatedAt = _now, IsValidated = true, WeekOf = followingWeek.Date});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType =
                    entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                BusinessUnit = 210100, IsEnabled = true
            });
            var facilityEntryTypeTwo = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType =
                    entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER),
                BusinessUnit = 110101, IsEnabled = true
            });
            
            for (var i = 0; i <= 6; i++)
            {
                sysdel1.FacilityEntries.Add(GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                    Facility = facility,
                    SystemDeliveryEntry = sysdel1,
                    SystemDeliveryEntryType =
                        entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                    EnteredBy = employee,
                    EntryDate = entryMondayDate.AddDays(i),
                    EntryValue = entryValue
                }));
            }

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryTypeTwo);
            // sysdel1.FacilityEntries.Add(facilityEntry1);
            // sysdel1.FacilityEntries.Add(facilityEntry2);

            Session.Flush();

            var results = Repository.GetDataForSystemDeliveryEntryFileDump(now.AddMonths(-1).GetBeginningOfMonth())
                                    .ToList();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("73.20", results[0].TotalValue);
        }

        [TestMethod]
        public void TestGetDataForSystemDeliveryEntryFileDumpReturnsAsPostedDescription()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryType});
            // var equipment = GetEntityFactory<Equipment>().Create(new {Facility = facility});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);

            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create(new {UpdatedAt = _now, IsValidated = true, WeekOf = _now.AddMonths(-1).GetBeginningOfMonth().Date});

            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                BusinessUnit = 210100,
                IsEnabled = true
            });

            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER),
                SystemDeliveryEntry = systemDeliveryEntry,
                EntryDate = _now.AddMonths(-1).Date
            });

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            systemDeliveryEntry.FacilityEntries.Add(facilityEntry);

            Session.Flush();

            var results = Repository.GetDataForSystemDeliveryEntryFileDump(_now.AddMonths(-1).GetBeginningOfMonth()).ToList();

            Assert.AreEqual("AS POSTED", results[0].AsPostedDescription);
        }

        [TestMethod] 
        public void TestGetDataForSystemDeliveryEntryFileDumpReturnsEntriesForSpecifiedStates()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var operatingCenterInNJ = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = operatingCenterInNJ, SystemDeliveryType = systemDeliveryType});
            var entryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(5);
            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create(new {UpdatedAt = _now, IsValidated = true, WeekOf = _now.AddMonths(-1).Date});
            var facilityEntryType = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType =
                    entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                BusinessUnit = 210100, IsEnabled = true
            });
            
            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new {
                Facility = facility,
                SystemDeliveryEntryType = entryTypes.First(x => x.Id == SystemDeliveryEntryType.Indices.DELIVERED_WATER),
                SystemDeliveryEntry = systemDeliveryEntry,
                EntryDate = _now.AddMonths(-1).Date
            });

            facility.FacilitySystemDeliveryEntryTypes.Add(facilityEntryType);
            systemDeliveryEntry.FacilityEntries.Add(facilityEntry);
            systemDeliveryEntry.OperatingCenters.Add(operatingCenterInNJ);
            
            Session.Flush();
            
            var results = 
                Repository.GetDataForSystemDeliveryEntryFileDump(_now.AddMonths(-1).GetBeginningOfMonth(), 
                    State.Indices.NJ).ToList();
            
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestGetEntryIdsReturnsIds()
        {
            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create(new {UpdatedAt = _now, IsValidated = true, WeekOf = _now.AddMonths(-1).GetBeginningOfMonth().Date});
            
            Session.Save(systemDeliveryEntry);

            // passing an empty array because creating one requires additional setup which doesn't add value to this test.
            var results = Repository.GetEntryIds(_now.AddMonths(-1).GetBeginningOfMonth(), Array.Empty<int>()).ToList();
            
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(1, results[0]);
        }

        #endregion

        #region Helper class

        private class SearchSystemDeliveryEntry : SearchSet<SystemDeliveryEntry>
        {
            [SearchAlias("OperatingCenters", "critOc", "Id")]
            public int? OperatingCenter { get; set; }
        }

        #endregion
    }

    public class TestSystemDeliveryEntryRepository : SystemDeliveryEntryRepository
    {
        public ICriteria iCanHasCriteria()
        {
            return Criteria;
        }

        public TestSystemDeliveryEntryRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container, authenticationService, roleRepo) { }
    }
}
