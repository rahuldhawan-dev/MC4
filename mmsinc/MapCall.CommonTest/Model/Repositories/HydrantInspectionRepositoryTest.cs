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
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class HydrantInspectionRepositoryTest : MapCallMvcSecuredRepositoryTestBase<HydrantInspection,
        HydrantInspectionRepository, User>
    {
        #region Constants

        private const RoleModules HYDRANT_INSPECTIONS_ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Fields

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        private TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider());
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Needs to exist
            GetFactory<ActiveAssetStatusFactory>().Create();
        }

        #endregion

        [TestMethod]
        public void TestLinqDoesNotReturnHydrantInspectionsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});

            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = HYDRANT_INSPECTIONS_ROLE_MODULE});
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

            var hydInsp1 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd1});
            var hydInsp2 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<HydrantInspectionRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(hydInsp1));
            Assert.IsFalse(result.Contains(hydInsp2));
        }

        [TestMethod]
        public void TestLinqReturnsAllHydrantInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});

            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = HYDRANT_INSPECTIONS_ROLE_MODULE});
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

            var hydInsp1 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd1});
            var hydInsp2 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd2});

            Repository = _container.GetInstance<HydrantInspectionRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(hydInsp1));
            Assert.IsTrue(result.Contains(hydInsp2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnHydrantInspectionsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = HYDRANT_INSPECTIONS_ROLE_MODULE});
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

            var hydInsp1 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd1});
            var hydInsp2 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<HydrantInspectionRepository>();
            var model = new EmptySearchSet<HydrantInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(hydInsp1));
            Assert.IsFalse(result.Contains(hydInsp2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllHydrantInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var hyd1 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr1});
            var hyd2 = GetFactory<HydrantFactory>().Create(new {OperatingCenter = opCntr2});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = HYDRANT_INSPECTIONS_ROLE_MODULE});
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

            var hydInsp1 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd1});
            var hydInsp2 = GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hyd2});

            Repository = _container.GetInstance<HydrantInspectionRepository>();
            var model = new EmptySearchSet<HydrantInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(hydInsp1));
            Assert.IsTrue(result.Contains(hydInsp2));
        }

        [TestMethod]
        public void TestSearchHydrantInspectionsForMapReturnsHydrantAssetCoordinatesFromHydrantInspections()
        {
            var search = new TestSearchHydrantInspection();

            var hydrant1 = GetFactory<HydrantFactory>().Create();
            var hydrant2 = GetFactory<HydrantFactory>().Create();
            var badHydrant = GetFactory<HydrantFactory>().Create();

            GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hydrant1});
            GetFactory<HydrantInspectionFactory>().Create(new {Hydrant = hydrant2});

            var result = Repository.SearchHydrantInspectionsForMap(search);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Id == hydrant1.Id));
            Assert.IsTrue(result.Any(x => x.Id == hydrant2.Id));
            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestMethod]
        public void TestSearchInspectionsCorrectlyMapsToViewModel()
        {
            var inspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = GetFactory<HydrantFactory>().Create(new {
                    Coordinate = typeof(CoordinateFactory)
                }),
                HydrantInspectionType = GetFactory<HydrantInspectionTypeFactory>().Create(),
                DateInspected = DateTime.Today,
                CreatedAt = DateTime.Today.AddHours(3),
                GallonsFlowed = 1,
                FullFlow = false,
                GPM = 2.0m,
                MinutesFlowed = 3.0m,
                ResidualChlorine = 4.0m,
                TotalChlorine = 5.0m,
                StaticPressure = 6.0m,
                Remarks = "Hhheheh", // http://knowyourmeme.com/memes/laughing-lizard-hhhehehe
                WorkOrderRequestOne = typeof(WorkOrderRequestFactory)
            });

            var search = new TestSearchHydrantInspection();
            Repository.SearchInspections(search);

            var result = search.Results.Single();
            Assert.AreEqual(inspection.Id, result.Id);
            Assert.AreEqual(inspection.Hydrant.Id, result.HydrantId);
            Assert.AreEqual(inspection.Hydrant.HydrantNumber, result.HydrantNumber);
            Assert.AreEqual(inspection.Hydrant.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(inspection.Hydrant.Town.ShortName, result.Town);
            Assert.AreEqual(inspection.Hydrant.Coordinate.Latitude, result.Latitude);
            Assert.AreEqual(inspection.Hydrant.Coordinate.Longitude, result.Longitude);
            Assert.AreEqual(inspection.DateInspected, result.DateInspected);
            Assert.AreEqual(inspection.HydrantInspectionType.Description, result.HydrantInspectionType);
            Assert.AreEqual(inspection.MinutesFlowed * inspection.GPM, result.GallonsFlowed);
            Assert.IsFalse(result.FullFlow);
            Assert.AreEqual(inspection.GPM, result.GPM);
            Assert.AreEqual(inspection.MinutesFlowed, result.MinutesFlowed);
            Assert.AreEqual(inspection.ResidualChlorine, result.ResidualChlorine);
            Assert.AreEqual(inspection.StaticPressure, result.StaticPressure);
            Assert.AreEqual(inspection.WorkOrderRequestOne.Description, result.WorkOrderRequestOne);
            Assert.AreEqual(inspection.Remarks, result.Remarks);
            Assert.AreEqual(inspection.InspectedBy.UserName, result.InspectedBy);
            Assert.AreEqual(inspection.CreatedAt, result.DateAdded);
        }

        [TestMethod]
        public void TestGetFlushingReport()
        {
            var hyd = GetFactory<HydrantFactory>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {OperatingCenter = hyd.OperatingCenter});

            // This should return 100.
            var inspValid = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hyd,
                DateInspected = new DateTime(2015, 1, 1),
                GPM = 10m,
                MinutesFlowed = 10m,
                BusinessUnit = "123456"
            });

            // This should return 144.
            var inspValid2 = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hyd,
                DateInspected = new DateTime(2015, 1, 14),
                GPM = 12m,
                MinutesFlowed = 12m,
                BusinessUnit = "123456"
            });

            // This should return 13.
            var inspValid3 = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hyd,
                DateInspected = new DateTime(2015, 1, 15),
                GPM = 0m,
                MinutesFlowed = 0m,
                GallonsFlowed = 13
            });

            // This should return 196
            var blowValid = GetEntityFactory<BlowOffInspection>().Create(new {
                Valve = valve,
                DateInspected = new DateTime(2015, 1, 28),
                GPM = 14m,
                MinutesFlowed = 14m,
                BusinessUnit = "123456"
            });

            var inspInvalid = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hyd,
                DateInspected = new DateTime(2015, 1, 14),
            });
            Assert.IsNull(inspInvalid.GPM);
            Assert.IsNull(inspInvalid.MinutesFlowed);

            var search = new TestSearchHydrantFlushing();
            search.OperatingCenter = hyd.OperatingCenter.Id;
            search.Year = 2015;
            
            Repository.GetFlushingReport(search);
            //MinutesFlowed * GPM
            Assert.AreEqual(440, search.Results.First().TotalGallons);
            Assert.AreEqual("123456", search.Results.First().BusinessUnit);
        }

        [TestMethod]
        public void TestGetFlushingReportReturnsItemsForMonthsNotFoundInQuery()
        {
            var hyd = GetFactory<HydrantFactory>().Create();

            // This should return 100.
            var inspValid = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = hyd,
                DateInspected = new DateTime(2015, 1, 1),
                GPM = 10m,
                MinutesFlowed = 10m
            });

            var search = new TestSearchHydrantFlushing();
            search.OperatingCenter = hyd.OperatingCenter.Id;
            search.Year = 2015;

            var result = Repository.GetFlushingReport(search).ToDictionary(x => x.Month, x => x);
            Assert.AreEqual(100, result[1].TotalGallons);
            Assert.AreEqual(0, result[2].TotalGallons);
            Assert.AreEqual(0, result[3].TotalGallons);
            Assert.AreEqual(0, result[4].TotalGallons);
            Assert.AreEqual(0, result[5].TotalGallons);
            Assert.AreEqual(0, result[6].TotalGallons);
            Assert.AreEqual(0, result[7].TotalGallons);
            Assert.AreEqual(0, result[8].TotalGallons);
            Assert.AreEqual(0, result[9].TotalGallons);
            Assert.AreEqual(0, result[10].TotalGallons);
            Assert.AreEqual(0, result[11].TotalGallons);
            Assert.AreEqual(0, result[12].TotalGallons);
            Assert.AreEqual(12, search.Count, "Count must be set.");
        }

        [TestMethod]
        public void TestGetDistinctYearsReturnsDistinctYears()
        {
            var hi1 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now});
            var hi2 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now.AddYears(1)});

            var results = Repository.GetDistinctYearsCompleted();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(hi1.DateInspected.Year, results.First());
            Assert.AreEqual(hi2.DateInspected.Year, results.Last());
        }

        [TestMethod]
        public void TestGetKPIHydrantsInspectedReport()
        {
            var op1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {ZoneStartYear = DateTime.Today.Year});
            var op2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var municipalBillingStatus = GetFactory<MunicipalHydrantBillingFactory>().Create();
            var publicBillingStatus = GetFactory<PublicHydrantBillingFactory>().Create();
            var hyd1 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, Zone = 1});
            var hyd2 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = op2, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus,
                InspectionFrequency = 1, InspectionFrequencyUnit = typeof(RecurringFrequencyUnitFactory)
            });
            var hyd3 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus
            });
            var hyd4 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus,
                InspectionFrequency = 1, InspectionFrequencyUnit = typeof(RecurringFrequencyUnitFactory)
            });
            var hi1 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now, Hydrant = hyd1});
            var hi2 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now, Hydrant = hyd2});
            var hi3 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = DateTime.Now.AddYears(1), Hydrant = hyd3});
            var hi4 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now, Hydrant = hyd4});
            var search = new TestSearchKPIHydrantInspectionsByMonth
                {Year = DateTime.Now.Year, OperatingCenter = new[] {op1.Id, op2.Id}};

            //new NHibernateQueryInterface.NHibernateQueryInterface(Session).ShowWindow();
            var result = Repository.GetKPIHydrantsInspectedReport(search).ToArray();

            Assert.AreEqual(5, result.Count());

            Assert.AreEqual(op1.OperatingCenterCode, result[0].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result[0].Year);
            Assert.AreEqual(2, result[0].Total);

            Assert.AreEqual(op1.OperatingCenterCode, result[1].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result[1].Year);
            Assert.AreEqual(2, result[1].Total);

            Assert.AreEqual(op2.OperatingCenterCode, result[2].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result[2].Year);
            Assert.AreEqual(1, result[2].Total);

            Assert.AreEqual(op2.OperatingCenterCode, result[3].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result[3].Year);
            Assert.AreEqual(1, result[3].Total);

            Assert.AreEqual("Total", result[4].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result[4].Year);
            Assert.AreEqual(3, result[4].Total);
        }

        [TestMethod]
        public void TestGetKPIHydrantsInspected()
        {
            var op1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {ZoneStartYear = DateTime.Today.Year});
            var op2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var municipalBillingStatus = GetFactory<MunicipalHydrantBillingFactory>().Create();
            var publicBillingStatus = GetFactory<PublicHydrantBillingFactory>().Create();
            var hyd1 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, Zone = 1});
            var hyd2 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = op2, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus,
                InspectionFrequency = 1, InspectionFrequencyUnit = typeof(RecurringFrequencyUnitFactory)
            });
            var hyd3 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus});
            var hi1 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now, Hydrant = hyd1});
            var hi2 = GetEntityFactory<HydrantInspection>().Create(new {DateInspected = DateTime.Now, Hydrant = hyd2});
            var hi3 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = DateTime.Now.AddYears(1), Hydrant = hyd3});
            var search = new TestSearchKPIHydrantInspectionsByMonth {Year = DateTime.Now.Year};

            var result = Repository.GetKPIHydrantsInspected(search);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(op1.OperatingCenterCode, result.First().OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result.First().Year);
            Assert.AreEqual(op2.OperatingCenterCode, result.Last().OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, result.Last().Year);
        }

        [TestMethod]
        public void TestGetKPIHydrantsInspectedDoesNotCountInspectionIfHydrantRetiredOrNotPublic()
        {
            var now = DateTime.Now;

            var op1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var op2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredHydrantStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var municipalBillingStatus = GetFactory<MunicipalHydrantBillingFactory>().Create();
            var publicBillingStatus = GetFactory<PublicHydrantBillingFactory>().Create();
            var hyd1 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = municipalBillingStatus});
            var hyd2 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op2, Status = retiredHydrantStatus, HydrantBilling = publicBillingStatus});
            var hyd3 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = municipalBillingStatus});
            var hi1 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = new DateTime(now.Year, 1, 1), Hydrant = hyd1});
            var hi11 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = new DateTime(now.Year, 2, 1), Hydrant = hyd1});
            var hi2 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = new DateTime(now.Year, 1, 1), Hydrant = hyd2});
            var hi3 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = DateTime.Now.AddYears(1), Hydrant = hyd3});
            var search = new TestSearchKPIHydrantInspectionsByMonth {Year = DateTime.Now.Year};

            var results = Repository.GetKPIHydrantsInspected(search).ToList();

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGetKPIHydrantsInspectedDoesNotCountInspectionIfAlreadyCountedInAPreviousMonth()
        {
            var now = DateTime.Now;
            var op1 = GetFactory<OperatingCenterFactory>().Create(new
                {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood", ZoneStartYear = DateTime.Today.Year});
            var op2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var activeHydrantStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredHydrantStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var municipalBillingStatus = GetFactory<MunicipalHydrantBillingFactory>().Create();
            var publicBillingStatus = GetFactory<PublicHydrantBillingFactory>().Create();
            var hyd1 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus, Zone = 1});
            var hyd2 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = op2, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus,
                InspectionFrequency = 1, InspectionFrequencyUnit = typeof(RecurringFrequencyUnitFactory)
            });
            var hyd3 = GetEntityFactory<Hydrant>().Create(new
                {OperatingCenter = op1, Status = activeHydrantStatus, HydrantBilling = publicBillingStatus});
            var hi1 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = new DateTime(now.Year, 1, 1), Hydrant = hyd1});
            var hi11 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = new DateTime(now.Year, 2, 1), Hydrant = hyd1});
            var hi2 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = new DateTime(now.Year, 1, 1), Hydrant = hyd2});
            var hi3 = GetEntityFactory<HydrantInspection>()
               .Create(new {DateInspected = DateTime.Now.AddYears(1), Hydrant = hyd3});
            var search = new TestSearchKPIHydrantInspectionsByMonth {Year = DateTime.Now.Year};

            var results = Repository.GetKPIHydrantsInspected(search).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(op1.OperatingCenterCode, results[0].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, results[0].Year);
            Assert.AreEqual(op2.OperatingCenterCode, results[1].OperatingCenter);
            Assert.AreEqual(DateTime.Now.Year, results[1].Year);
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
            var hydrant = GetFactory<HydrantFactory>().Create();
            var inspectType = GetFactory<HydrantInspectionTypeFactory>().Create();
            var inspectionGood1 = GetFactory<HydrantInspectionFactory>().Create(new {
                DateInspected = expectedStartDate, Hydrant = hydrant, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionGood2 = GetFactory<HydrantInspectionFactory>().Create(new {
                DateInspected = expectedEndDate, Hydrant = hydrant, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionBad = GetFactory<HydrantInspectionFactory>().Create(new {
                DateInspected = expectedEndDate.AddDays(1), Hydrant = hydrant, InspectedBy = user,
                HydrantInspectionType = inspectType
            });

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
            var hydrant = GetFactory<HydrantFactory>().Create();
            var inspectType = GetFactory<HydrantInspectionTypeFactory>().Create(new {Description = "A Description"});
            var inspectType2 = GetFactory<HydrantInspectionTypeFactory>().Create(new {Description = "B Description"});
            var inspectionGood1 = GetFactory<HydrantInspectionFactory>().Create(new {
                DateInspected = expectedStartDate, Hydrant = hydrant, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionGood2 = GetFactory<HydrantInspectionFactory>().Create(new {
                DateInspected = expectedStartDate.AddHours(1), Hydrant = hydrant, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionGood3 = GetFactory<HydrantInspectionFactory>().Create(new {
                DateInspected = expectedStartDate.AddHours(2), Hydrant = hydrant, InspectedBy = user,
                HydrantInspectionType = inspectType2
            });

            var result = Repository.GetInspectionProductivityReport(search).ToArray();
            Assert.AreEqual(2, result.Count());

            Assert.AreEqual(expectedStartDate, result[0].DateInspected);
            Assert.AreEqual("Hydrant", result[0].AssetType);
            Assert.AreEqual("A Description", result[0].InspectionType);
            Assert.AreEqual(2, result[0].Count);
            Assert.AreEqual(user.FullName, result[0].InspectedBy);
            Assert.IsNull(result[0].ValveOperated, "This is not used by the HydrantInspectionRepository.");
            Assert.IsNull(result[0].ValveSize, "This is not used by the HydrantInspectionRepository.");

            Assert.AreEqual(expectedStartDate, result[1].DateInspected);
            Assert.AreEqual("Hydrant", result[1].AssetType);
            Assert.AreEqual("B Description", result[1].InspectionType);
            Assert.AreEqual(1, result[1].Count);
            Assert.AreEqual(user.FullName, result[1].InspectedBy);
            Assert.IsNull(result[1].ValveOperated, "This is not used by the HydrantInspectionRepository.");
            Assert.IsNull(result[1].ValveSize, "This is not used by the HydrantInspectionRepository.");
        }

        [TestMethod]
        public void TestGetCountOfDistinctInspectionsForYearGetsDistinctCount()
        {
            var now = DateTime.Now;
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new
                {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood", ZoneStartYear = DateTime.Today.Year});
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new
                {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewbury", ZoneStartYear = DateTime.Today.Year});
            var hydrant1 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = opc1, InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(RecurringFrequencyUnitFactory)
            });
            var hydrant2 = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opc1});
            var hydrant3 = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opc1, Zone = 1});
            var hydrant4 = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opc1, Zone = 2});
            var hydrant5 = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opc2, Zone = 1});
            var hydrant6 = GetEntityFactory<Hydrant>().Create(new {
                OperatingCenter = opc2, InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(RecurringFrequencyUnitFactory)
            });
            var hydrantInspection1a = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant1, DateInspected = now});
            var hydrantInspection1b = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant1, DateInspected = now});
            var hydrantInspection2a = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant2, DateInspected = now});
            var hydrantInspection3a = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant3, DateInspected = now});
            var hydrantInspection4a = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant4, DateInspected = now});
            var hydrantInspection5a = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant5, DateInspected = now});
            var hydrantInspection6a = GetEntityFactory<HydrantInspection>()
               .Create(new {Hydrant = hydrant6, DateInspected = now});
            Session.Flush();
            Session.Clear();
            Assert.AreEqual(3, Repository.GetCountOfDistinctInspectionsForYear(now.Year, opc1.Id));
        }

        [TestMethod]
        public void TestGetFromPastMonthGetsMainBreaksFromPastMonth()
        {
            var now = DateTime.Now;
            var yestermonth = now.AddMonths(-1);
            _dateTimeProvider.SetNow(now);
            var pastMonth =
                GetEntityFactory<HydrantInspection>().Create(new {
                    DateInspected = yestermonth
                });
            var previousMonth =
                GetEntityFactory<HydrantInspection>().Create(new {
                    DateInspected = yestermonth.Date.AddDays(-1)
                });
            var tomonth =
                GetEntityFactory<HydrantInspection>().Create(new {
                    DateInspected = yestermonth.AddMonths(1)
                });

            var results = Repository.GetFromPastMonth();

            MyAssert.Contains(results, pastMonth, "Expected inspection was not in result");
            MyAssert.DoesNotContain(results, previousMonth, "Inspection from previousMonth was in result");
            MyAssert.DoesNotContain(results, tomonth, "Inspection from tomonth was in result");
        }

        #region GetHydrantInspectionsWithSapIssues

        [TestMethod]
        public void TestGetHydrantInspectionsWithSapIssuesReturnsHydrantInspectionsWithSapIssues()
        {
            var hydrantInspectionValid1 = GetFactory<HydrantInspectionFactory>()
               .Create(new {SAPErrorCode = "RETRY::Something went wrong"});
            var hydrantInspectionInvalid1 = GetFactory<HydrantInspectionFactory>().Create(new {SAPErrorCode = ""});
            var hydrantInspectionInvalid2 = GetFactory<HydrantInspectionFactory>().Create();

            var result = Repository.GetHydrantInspectionsWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region Test classes

        private class TestSearchHydrantInspection : SearchSet<HydrantInspectionSearchResultViewModel>,
            ISearchHydrantInspection
        {
            public int[] OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? FireDistrict { get; set; }
            public int? Route { get; set; }
            public int? HydrantSuffix { get; set; }
            public int? HydrantInspectionType { get; set; }
            public int? InspectedBy { get; set; }
            public int? HydrantTagStatus { get; set; }
            public bool? WorkOrderRequired { get; set; }
            public int? WorkOrderRequestOne { get; set; }
            public bool? SAPEquipmentOnly { get; set; }
            public int? SAPEquipmentId { get; set; }
            public int? FreeNoReadReason { get; set; }
            public int? TotalNoReadReason { get; set; }
        }

        private class TestSearchHydrantInspectionWorkOrder : SearchSet<HydrantInspectionWorkOrderReportItem>,
            ISearchHydrantInspectionWorkOrder
        {
            public int? OperatingCenter { get; set; }
            public int? Town { get; set; }
        }

        private class TestSearchHydrantFlushing : SearchSet<HydrantFlushingReportItem>, ISearchHydrantFlushingReport
        {
            public int? OperatingCenter { get; set; }

            public int? Year { get; set; }
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

        private class TestSearchKPIHydrantInspectionsByMonth : SearchSet<KPIHydrantsInspectedReportItem>,
            ISearchKPIHydrantInspectionReport
        {
            #region Properties

            public int[] OperatingCenter { get; set; }
            public int? Year { get; set; }

            #endregion
        }

        #endregion
    }
}
