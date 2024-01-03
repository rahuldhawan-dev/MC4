using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        ValveInspectionRepositoryTest : MapCallMvcSecuredRepositoryTestBase<ValveInspection, ValveInspectionRepository,
            User>
    {
        #region Constants

        private const RoleModules VALVE_INSPECTIONS_ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Fields

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Needs to exist
            GetFactory<ActiveAssetStatusFactory>().Create();
        }

        #endregion

        private void SetupDataForMonthlyReports(int year, OperatingCenter operatingCenter)
        {
            var date = new DateTime(year, 1, 1);
            var user = GetEntityFactory<User>().Create();
            var valveZones = GetEntityFactory<ValveZone>().CreateList(7);
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var requestCancellationStatus = GetFactory<RequestCancellationAssetStatusFactory>().Create();
            var requestRetirementStatus = GetFactory<RequestRetirementAssetStatusFactory>().Create();
            var pendingStatus = GetFactory<PendingAssetStatusFactory>().Create();
            var publicBilling = GetFactory<PublicValveBillingFactory>().Create();
            var muniBilling = GetFactory<MunicipalValveBillingFactory>().Create();
            var valveSizeSmall = GetEntityFactory<ValveSize>().Create(new {Size = 2.0m});
            var valveSizeLarge = GetEntityFactory<ValveSize>().Create(new {Size = 12.0m});
            // the good
            var valve1 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date,
                Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0]
            });
            var valve2 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date,
                Status = requestCancellationStatus, ValveBilling = publicBilling, ValveZone = valveZones[0]
            });
            var valve3 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, ValveSize = valveSizeLarge, DateInstalled = date,
                Status = requestRetirementStatus, ValveBilling = publicBilling, ValveZone = valveZones[0]
            });
            var valve4 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, ValveSize = valveSizeSmall, DateInstalled = date,
                Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[0]
            });
            var vi1 = GetEntityFactory<ValveInspection>().BuildThenSave(new
                {InspectedBy = user, Valve = valve1, Inspected = true, DateInspected = date.AddDays(1)});
            var vi2 = GetEntityFactory<ValveInspection>().BuildThenSave(new
                {InspectedBy = user, Valve = valve2, Inspected = true, DateInspected = date.AddMonths(1).AddDays(1)});
            var vi3 = GetEntityFactory<ValveInspection>().BuildThenSave(new
                {InspectedBy = user, Valve = valve3, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1)});
            var vi4 = GetEntityFactory<ValveInspection>().BuildThenSave(new
                {InspectedBy = user, Valve = valve4, Inspected = true, DateInspected = date.AddMonths(2).AddDays(1)});

            // The Bad - these inspections should not be counted
            var valve5 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, DateInstalled = date, ValveSize = valveSizeSmall,
                ValveZone = valveZones[0], Status = pendingStatus, ValveBilling = publicBilling
            });
            var vi5 = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve5, Inspected = true, DateInspected = date.AddDays(1)
            }); // valve is not active or public
            var valve6 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, DateInstalled = date, ValveSize = valveSizeSmall,
                Status = activeStatus, ValveZone = valveZones[0], ValveBilling = muniBilling
            });
            var vi6 = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve6, Inspected = true, DateInspected = date.AddDays(1)
            }); // valve is not public
            var valve7 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, DateInstalled = date, ValveSize = valveSizeSmall,
                ValveBilling = publicBilling, ValveZone = valveZones[0], Status = pendingStatus
            });
            var vi7 = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve7, Inspected = true, DateInspected = date.AddDays(1)
            }); // valve is not active 
            var valve8 = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = operatingCenter, ValveSize = valveSizeLarge, DateInstalled = date.AddYears(1),
                Status = activeStatus, ValveBilling = publicBilling, ValveZone = valveZones[5]
            });
            var vi8 = GetEntityFactory<ValveInspection>().BuildThenSave(new
                {InspectedBy = user, Valve = valve8, Inspected = false, DateInspected = date.AddMonths(1).AddDays(13)});

            var vi1A = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve1, Inspected = false, DateInspected = date.AddDays(1)
            }); // not operated shouldn't be counted
            var vi1A1 = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve1, Inspected = false, DateInspected = date
            }); // not operated shouldn't be counted
            var vi1B = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve1, Inspected = true, DateInspected = date.AddDays(10)
            }); // we don't want to count valves twice if they were inspected twice
            var vi2A = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve2, Inspected = true, DateInspected = date.AddMonths(1).AddDays(13)
            }); // we don't want to count valves twice if they were inspected twice
            var vi3A = GetEntityFactory<ValveInspection>().BuildThenSave(new {
                InspectedBy = user, Valve = valve3, Inspected = false, DateInspected = date.AddMonths(2)
            }); // a valve inspection performed and not operated should not remove a valid one

            var valveInspections = new[] {vi1, vi2, vi3, vi4, vi5, vi6, vi7, vi8, vi1A, vi1A1, vi1B, vi2A, vi3A};
            foreach (var inspection in valveInspections)
            {
                Session.Refresh(inspection);
            }
        }

        [TestMethod]
        public void TestLinqDoesNotReturnValveInspectionsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var valve1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var valve2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = VALVE_INSPECTIONS_ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var valveInspection1 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve1});
            var valveInspection2 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<ValveInspectionRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valveInspection1));
            Assert.IsFalse(result.Contains(valveInspection2));
        }

        [TestMethod]
        public void TestLinqReturnsAllValveInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var valve1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var valve2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = VALVE_INSPECTIONS_ROLE_MODULE});
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

            var valveInspection1 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve1});
            var valveInspection2 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve2});

            Repository = _container.GetInstance<ValveInspectionRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valveInspection1));
            Assert.IsTrue(result.Contains(valveInspection2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnValveInspectionsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var valve1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var valve2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = VALVE_INSPECTIONS_ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var valveInspection1 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve1});
            var valveInspection2 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<ValveInspectionRepository>();
            var model = new EmptySearchSet<ValveInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(valveInspection1));
            Assert.IsFalse(result.Contains(valveInspection2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllValveInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var valve1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var valve2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = VALVE_INSPECTIONS_ROLE_MODULE});
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

            var valveInspection1 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve1});
            var valveInspection2 = GetFactory<ValveInspectionFactory>().Create(new {Valve = valve2});

            Repository = _container.GetInstance<ValveInspectionRepository>();
            var model = new EmptySearchSet<ValveInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(valveInspection1));
            Assert.IsTrue(result.Contains(valveInspection2));
        }

        [TestMethod]
        public void TestSearchInspectionsCorrectlyMapsToViewModel()
        {
            var valveInspection = new ValveInspection();

            var inspection = GetFactory<ValveInspectionFactory>().Create(new {
                Valve = GetFactory<ValveFactory>().Create(new {
                    Coordinate = typeof(CoordinateFactory),
                    FunctionalLocation = typeof(FunctionalLocationFactory)
                }),
                DateInspected = DateTime.Today,
                CreatedAt = DateTime.Today.AddHours(3),
                WorkOrderRequestOne = GetEntityFactory<ValveWorkOrderRequest>().Create(new {Description = "Covered"})
            });
            Session.Refresh(inspection);
            var search = new TestSearchValveInspection();
            Repository.SearchInspections(search);

            var result = search.Results.Single();
            Assert.AreEqual(inspection.Id, result.Id);
            Assert.AreEqual(inspection.Valve.Id, result.ValveId);
            Assert.AreEqual(inspection.Valve.ValveNumber, result.ValveNumber);
            Assert.AreEqual(inspection.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(inspection.Valve.Town.ShortName, result.Town);
            Assert.AreEqual(inspection.Valve.FunctionalLocation.Description, result.FunctionalLocation);
            Assert.AreEqual(inspection.Valve.SAPEquipmentId, result.SAPEquipmentId);
            Assert.AreEqual(inspection.Valve.Coordinate.Latitude, result.Latitude);
            Assert.AreEqual(inspection.Valve.Coordinate.Longitude, result.Longitude);
            Assert.AreEqual(inspection.DateInspected, result.DateInspected);
            Assert.AreEqual(inspection.Inspected, result.Inspected);
            Assert.AreEqual(inspection.Turns, result.Turns);
            Assert.AreEqual(inspection.Remarks, result.Remarks);
            Assert.AreEqual(inspection.InspectedBy.UserName, result.InspectedBy);
            Assert.AreEqual(inspection.CreatedAt, result.DateAdded);
        }

        [TestMethod]
        public void TestGetDistinctYearsReturnsDistinctYears()
        {
            var hi1 = GetEntityFactory<ValveInspection>().Create(new {DateInspected = DateTime.Now});
            var hi2 = GetEntityFactory<ValveInspection>().Create(new {DateInspected = DateTime.Now.AddYears(1)});

            var results = Repository.GetDistinctYearsCompleted();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(hi1.DateInspected.Year, results.First());
            Assert.AreEqual(hi2.DateInspected.Year, results.Last());
        }

        [TestMethod]
        public void TestGetInspectionProductivityReportOnlyCountsDatesWithinTheGivenRange()
        {
            var expectedStartDate = DateTime.Today;
            var expectedEndDate =
                expectedStartDate.AddDays(6); // The way the search works, 6 days + today == 7 days == 1 week.
            var search = new TestSearchInspectionProductivity();
            search.StartDate = expectedStartDate;
            search.Week = InspectionProductivityWeekSpan.OneWeek;

            var user = GetFactory<UserFactory>().Create();
            var valveSize = GetEntityFactory<ValveSize>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {ValveSize = valveSize});
            var inspectionGood1 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate, Valve = valve, InspectedBy = user});
            var inspectionGood2 = GetFactory<ValveInspectionFactory>()
               .Create(new {DateInspected = expectedEndDate, Valve = valve, InspectedBy = user});
            var inspectionBad = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedEndDate.AddDays(1), Valve = valve, InspectedBy = user});

            var result = Repository.GetInspectionProductivityReport(search).ToArray();
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(expectedStartDate, result[0].DateInspected);
            Assert.AreEqual(expectedEndDate, result[1].DateInspected);
        }

        [TestMethod]
        public void TestGetInspectionProductivityReportReturnsExpectedCounts()
        {
            var expectedStartDate = DateTime.Today;
            var search = new TestSearchInspectionProductivity();
            search.StartDate = expectedStartDate;
            search.Week = InspectionProductivityWeekSpan.OneWeek;

            var user = GetFactory<UserFactory>().Create();
            var valveSizeSmall = GetEntityFactory<ValveSize>().Create(new {Size = 2.0m});
            var valveSizeLarge = GetEntityFactory<ValveSize>().Create(new {Size = 12.0m});
            var valveSmall = GetFactory<ValveFactory>().Create(new {ValveSize = valveSizeSmall});
            var valveLarge = GetFactory<ValveFactory>().Create(new {ValveSize = valveSizeLarge});
            var valveNA = GetFactory<ValveFactory>().Create();
            var inspectionGood1 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate, Valve = valveSmall, InspectedBy = user,});
            var inspectionGood2 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate.AddHours(1), Valve = valveSmall, InspectedBy = user,});
            var inspectionGood3 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate.AddHours(2), Valve = valveSmall, InspectedBy = user,});
            var inspectionGood4 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate, Valve = valveLarge, InspectedBy = user,});
            var inspectionGood5 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate.AddHours(1), Valve = valveLarge, InspectedBy = user,});
            var inspectionGood6 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate.AddHours(2), Valve = valveLarge, InspectedBy = user,});
            var inspectionGood7 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate, Valve = valveNA, InspectedBy = user,});
            var inspectionGood8 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate.AddHours(1), Valve = valveNA, InspectedBy = user,});
            var inspectionGood9 = GetFactory<ValveInspectionFactory>().Create(new
                {DateInspected = expectedStartDate.AddHours(2), Valve = valveNA, InspectedBy = user,});

            var result = Repository.GetInspectionProductivityReport(search).ToArray();
            Assert.AreEqual(3, result.Count());

            Assert.AreEqual(expectedStartDate, result[0].DateInspected);
            Assert.AreEqual("Valve", result[0].AssetType);
            Assert.IsFalse(result[0].ValveOperated.Value);
            Assert.AreEqual("No", result[0].InspectionType, "Should be No when ValveOperated is false.");
            Assert.AreEqual(3, result[0].Count);
            Assert.AreEqual(user.FullName, result[0].InspectedBy);
            Assert.AreEqual("< 12", result[0].ValveSize);
            Assert.AreEqual(">= 12", result[1].ValveSize);
            Assert.AreEqual("N/A", result[2].ValveSize);
        }

        #region Inspection Monthly Counts

        [TestMethod]
        public void TestGetValveInspectionByMonthReportItemsReturnsProperReportItems()
        {
            var year = 2011;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ4"});
            SetupDataForMonthlyReports(year, operatingCenter);
            var operatingCenter2 = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ7"});
            SetupDataForMonthlyReports(year, operatingCenter2);

            var search = new TestSearchValveInspectionsByMonth
                {Year = year, OperatingCenter = new[] {operatingCenter.Id, operatingCenter2.Id}};

            var result = Repository.GetValveInspectionsByMonthReportItems(search).ToList();

            Assert.AreEqual(8, result.Count());

            Assert.AreEqual(1, result[0].Month);
            Assert.AreEqual(year, result[0].Year);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            Assert.AreEqual(operatingCenter.OperatingCenterCode, result[0].OperatingCenter);
            Assert.AreEqual(1, result[0].TotalDistinctValvesInspected);
            Assert.AreEqual(3, result[0].TotalRequired);
            Assert.AreEqual(3, result[0].TotalValves); // there's three small valves that could require an inspection

            Assert.AreEqual(2, result[1].Month);
            Assert.AreEqual(year, result[1].Year);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[1].SizeRange);
            Assert.AreEqual(1, result[1].TotalDistinctValvesInspected);
            Assert.AreEqual(3, result[1].TotalRequired);
            Assert.AreEqual(3, result[1].TotalValves);

            Assert.AreEqual(3, result[3].Month);
            Assert.AreEqual(year, result[3].Year);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[3].SizeRange);
            Assert.AreEqual(1, result[3].TotalDistinctValvesInspected);
            Assert.AreEqual(3, result[3].TotalRequired);
            Assert.AreEqual(3, result[3].TotalValves);

            Assert.AreEqual(3, result[2].Month);
            Assert.AreEqual(year, result[2].Year);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[2].SizeRange);
            Assert.AreEqual(1, result[2].TotalDistinctValvesInspected);
            Assert.AreEqual(1, result[2].TotalRequired);
            Assert.AreEqual(1, result[2].TotalValves); // there's just one large valve that could require an inspection
        }

        [TestMethod]
        public void TestGetValveInspectionByMonthReportReturnsProperReport()
        {
            var year = 2011;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            SetupDataForMonthlyReports(year, operatingCenter);
            var valves = _container.GetInstance<ValveRepository>().GetAll().Count();
            var search = new TestSearchValveInspectionsByMonth
                {Year = year, OperatingCenter = new[] {operatingCenter.Id}};

            var result = Repository.GetValveInspectionsByMonthReport(search).ToList();

            Assert.AreEqual(3, result.Count());
            //Small
            Assert.AreEqual(1, result[0].Jan);
            Assert.AreEqual(1, result[0].Feb);
            Assert.AreEqual(1, result[0].Mar);
            Assert.AreEqual(3, result[0].Total);
            Assert.AreEqual(3, result[0].TotalRequired);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            //Large
            Assert.AreEqual(0, result[1].Jan);
            Assert.AreEqual(0, result[1].Feb);
            Assert.AreEqual(1, result[1].Mar);
            Assert.AreEqual(1, result[1].Total);
            Assert.AreEqual(1, result[1].TotalRequired);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[1].SizeRange);
            //Total
            Assert.AreEqual(1, result[2].Jan);
            Assert.AreEqual(1, result[2].Feb);
            Assert.AreEqual(2, result[2].Mar);
            Assert.AreEqual(4, result[2].Total);
            Assert.AreEqual(4, result[2].TotalRequired);
            Assert.AreEqual("Total", result[2].SizeRange);
        }

        [TestMethod]
        public void TestGetValvesOperatedByMonthReportItemsReturnsProperItems()
        {
            var year = 2011;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            SetupDataForMonthlyReports(year, operatingCenter);
            var search = new TestSearchValvesOperatedByMonth
                {Year = year, OperatingCenter = new[] {operatingCenter.Id}};

            var result = Repository.GetValvesOperatedByMonthReportItems(search).ToList();

            Assert.AreEqual(7, result.Count());

            Assert.AreEqual(1, result[0].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            Assert.AreEqual(year, result[0].Year);
            Assert.AreEqual(5, result[0].TotalInspected);
            Assert.AreEqual(true, result[0].Operated);

            Assert.AreEqual(1, result[1].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[1].SizeRange);
            Assert.AreEqual(year, result[1].Year);
            Assert.AreEqual(2, result[1].TotalInspected);
            Assert.AreEqual(false, result[1].Operated);

            Assert.AreEqual(2, result[2].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[2].SizeRange);
            Assert.AreEqual(year, result[2].Year);
            Assert.AreEqual(1, result[2].TotalInspected);
            Assert.AreEqual(false, result[2].Operated);

            Assert.AreEqual(2, result[3].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[3].SizeRange);
            Assert.AreEqual(year, result[3].Year);
            Assert.AreEqual(2, result[3].TotalInspected);
            Assert.AreEqual(true, result[3].Operated);

            Assert.AreEqual(3, result[4].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[4].SizeRange);
            Assert.AreEqual(year, result[4].Year);
            Assert.AreEqual(1, result[4].TotalInspected);
            Assert.AreEqual(true, result[4].Operated);

            Assert.AreEqual(3, result[5].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[5].SizeRange);
            Assert.AreEqual(year, result[5].Year);
            Assert.AreEqual(1, result[5].TotalInspected);
            Assert.AreEqual(false, result[5].Operated);

            Assert.AreEqual(3, result[6].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[6].SizeRange);
            Assert.AreEqual(year, result[6].Year);
            Assert.AreEqual(1, result[6].TotalInspected);
            Assert.AreEqual(true, result[6].Operated);
        }

        [TestMethod]
        public void TestGetValvesOperatedByMonthReportReturnsProperReport()
        {
            var year = 2011;
            var date = new DateTime(year, 1, 2);
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            SetupDataForMonthlyReports(year, operatingCenter1);
            SetupDataForMonthlyReports(year, operatingCenter2);
            // Adding some with null valve sizes
            var valve1 = GetEntityFactory<Valve>()
               .Create(new {OperatingCenter = operatingCenter2, DateInstalled = date});
            var valve2 = GetEntityFactory<Valve>()
               .Create(new {OperatingCenter = operatingCenter2, DateInstalled = date});
            var valveInspection1 = GetEntityFactory<ValveInspection>()
               .Create(new {Valve = valve1, DateInspected = date.AddDays(1), Inspected = true});
            var valveInspection2 = GetEntityFactory<ValveInspection>()
               .Create(new {Valve = valve2, DateInspected = date.AddDays(1), Inspected = true});
            var search = new TestSearchValvesOperatedByMonth
                {Year = year, OperatingCenter = new[] {operatingCenter1.Id, operatingCenter2.Id}};

            var result = Repository.GetValvesOperatedByMonthReport(search).ToList();

            Assert.AreEqual(9, result.Count());
            Assert.AreEqual(10, result[0].Jan);
            Assert.AreEqual(4, result[0].Feb);
            Assert.AreEqual(2, result[0].Mar);
            Assert.AreEqual($"{operatingCenter1.OperatingCenterCode},{operatingCenter2.OperatingCenterCode}",
                result[0].OperatingCenter);
            Assert.AreEqual(16, result[0].Total);
            Assert.AreEqual(year, result[0].Year);
            Assert.AreEqual("YES", result[0].Operated);
            //ensure all the valve ranges
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[1].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[2].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[3].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[4].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, result[5].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_NULL, result[6].SizeRange);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_NULL, result[7].SizeRange);
            Assert.AreEqual("Total", result[8].SizeRange);

            Assert.AreEqual("Total", result[2].Operated);
            Assert.AreEqual("Total", result[5].Operated);
        }

        [TestMethod]
        public void TestGetRequiredValvesOperatedByMonthReportItemsReturnsProperItems()
        {
            var year = 2011;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            SetupDataForMonthlyReports(year, operatingCenter);
            var search = new TestSearchRequiredValvesOperatedByMonth
                {Year = year, OperatingCenter = new[] {operatingCenter.Id}};

            var result = Repository.GetRequiredValvesOperatedByMonthReportItems(search).ToList();

            Assert.AreEqual(6, result.Count());
            Assert.AreEqual(1, result[0].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            Assert.AreEqual(year, result[0].Year);
            Assert.AreEqual(2, result[0].TotalInspected);
            Assert.AreEqual(false, result[0].Operated);

            Assert.AreEqual(1, result[1].Month);
            Assert.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, result[0].SizeRange);
            Assert.AreEqual(year, result[1].Year);
            Assert.AreEqual(2, result[1].TotalInspected);
            Assert.AreEqual(true, result[1].Operated);
        }

        [TestMethod]
        public void TestGetRequiredValvesOperatedByMonthReportReturnsProperReport()
        {
            var year = 2011;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            SetupDataForMonthlyReports(year, operatingCenter);
            var search = new TestSearchRequiredValvesOperatedByMonth
                {Year = year, OperatingCenter = new[] {operatingCenter.Id}};

            var result = Repository.GetRequiredValvesOperatedByMonthReport(search).ToList();

            Assert.AreEqual(7, result.Count());
            Assert.AreEqual("Total", result[2].Operated);
            Assert.AreEqual("NO", result[4].Operated);
            Assert.AreEqual("Total", result[5].Operated);
            Assert.AreEqual("Total", result[6].SizeRange);
        }

        #endregion

        [TestMethod]
        public void TestSearchValveInspectionsForMapReturnsValveAssetCoordinatesFromValveInspections()
        {
            var search = new TestSearchValveInspection();

            var valve1 = GetFactory<ValveFactory>().Create();
            var valve2 = GetFactory<ValveFactory>().Create();
            var badValve = GetFactory<ValveFactory>().Create();

            GetFactory<ValveInspectionFactory>().Create(new {Valve = valve1});
            GetFactory<ValveInspectionFactory>().Create(new {Valve = valve2});

            var result = Repository.SearchValveInspectionsForMap(search);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Id == valve1.Id));
            Assert.IsTrue(result.Any(x => x.Id == valve2.Id));
            // ReSharper restore PossibleMultipleEnumeration
        }

        #region GetValveInspectionsWithSapIssues

        [TestMethod]
        public void TestGetValveInspectionsWithSapIssuesReturnsValveInspectionsWithSapIssues()
        {
            var valveInspectionValid1 = GetFactory<ValveInspectionFactory>()
               .Create(new {SAPErrorCode = "RETRY::Something went wrong"});
            var valveInspectionInvalid1 = GetFactory<ValveInspectionFactory>().Create(new {SAPErrorCode = ""});
            var valveInspectionInvalid2 = GetFactory<ValveInspectionFactory>().Create();

            var result = Repository.GetValveInspectionsWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region Test Classes

        private class TestSearchValveInspection : SearchSet<ValveInspectionSearchResultViewModel>,
            ISearchValveInspection
        {
            public int[] OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? ValveSuffix { get; set; }
            public int? Route { get; set; }
            public int? InspectedBy { get; set; }
            public bool? WorkOrderRequired { get; set; }
            public int? WorkOrderRequestOne { get; set; }
            public bool? SAPEquipmentOnly { get; set; }
            public int? SAPEquipmentId { get; set; }
            public int? ValveZone { get; set; }
        }

        private class TestSearchInspectionProductivity : SearchSet<InspectionProductivityReportItem>,
            ISearchInspectionProductivity
        {
            public DateTime? StartDate { get; set; }
            public InspectionProductivityWeekSpan? Week { get; set; }
            public int? OperatingCenter { get; set; }
            public int? InspectedBy { get; set; }

            public int GetDays()
            {
                if (Week.HasValue && Week.Value == InspectionProductivityWeekSpan.TwoWeeks)
                {
                    return 14;
                }

                // Return seven days by default rather than make Week a required field.
                return 7;
            }
        }

        private class BaseSearchValveInspectionsByMonth<T> : SearchSet<T>
            where T : class
        {
            #region Properties

            public int[] OperatingCenter { get; set; }
            public int? Year { get; set; }

            #endregion
        }

        private class TestSearchValveInspectionsByMonth :
            BaseSearchValveInspectionsByMonth<ValveInspectionsByMonthReportItem>,
            ISearchValveInspectionsByMonthReport { }

        private class TestSearchValvesOperatedByMonth :
            BaseSearchValveInspectionsByMonth<ValvesOperatedByMonthReportItem>, ISearchValvesOperatedByMonthReport { }

        private class TestSearchRequiredValvesOperatedByMonth :
            BaseSearchValveInspectionsByMonth<RequiredValvesOperatedByMonthReportItem>,
            ISearchRequiredValvesOperatedByMonthReport { }

        #endregion
    }
}
