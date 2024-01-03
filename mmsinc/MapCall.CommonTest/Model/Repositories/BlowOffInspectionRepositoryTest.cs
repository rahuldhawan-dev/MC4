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
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class BlowOffInspectionRepositoryTest : MapCallMvcSecuredRepositoryTestBase<BlowOffInspection,
        BlowOffInspectionRepository, User>
    {
        #region Constants

        private const RoleModules HYDRANT_INSPECTIONS_ROLE_MODULE = RoleModules.FieldServicesAssets;

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
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        [TestMethod]
        public void TestLinqDoesNotReturnBlowOffInspectionsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

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

            var insp1 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val1});
            var insp2 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<BlowOffInspectionRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsFalse(result.Contains(insp2));
        }

        [TestMethod]
        public void TestLinqReturnsAllBlowOffInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});

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

            var insp1 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val1});
            var insp2 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val2});

            Repository = _container.GetInstance<BlowOffInspectionRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsTrue(result.Contains(insp2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnBlowOffInspectionsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});
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

            var insp1 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val1});
            var insp2 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<BlowOffInspectionRepository>();
            var model = new EmptySearchSet<BlowOffInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsFalse(result.Contains(insp2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllBlowOffInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var val1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr1});
            var val2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opCntr2});
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

            var insp1 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val1});
            var insp2 = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = val2});

            Repository = _container.GetInstance<BlowOffInspectionRepository>();
            var model = new EmptySearchSet<BlowOffInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsTrue(result.Contains(insp2));
        }

        [TestMethod]
        public void TestSearchInspectionsCorrectlyMapsToViewModel()
        {
            var inspection = GetFactory<BlowOffInspectionFactory>().Create(new {
                Valve = GetFactory<ValveFactory>().Create(new {
                    Coordinate = typeof(CoordinateFactory)
                }),
                HydrantInspectionType = GetFactory<HydrantInspectionTypeFactory>().Create(),
                DateInspected = DateTime.Today,
                CreatedAt = DateTime.Today.AddHours(3),
                GallonsFlowed = 1,
                FullFlow = true,
                GPM = 2.0m,
                MinutesFlowed = 3.0m,
                ResidualChlorine = 4.0m,
                TotalChlorine = 5.0m,
                StaticPressure = 6.0m,
                Remarks = "Hhheheh", // http://knowyourmeme.com/memes/laughing-lizard-hhhehehe
                WorkOrderRequestOne = typeof(WorkOrderRequestFactory)
            });

            var search = new TestSearchBlowOffInspection();
            Repository.SearchInspections(search);

            var result = search.Results.Single();
            Assert.AreEqual(inspection.Id, result.Id);
            Assert.AreEqual(inspection.Valve.Id, result.ValveId);
            Assert.AreEqual(inspection.Valve.ValveNumber, result.ValveNumber);
            Assert.AreEqual(inspection.Valve.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(inspection.Valve.Town.ShortName, result.Town);
            Assert.AreEqual(inspection.Valve.Coordinate.Latitude, result.Latitude);
            Assert.AreEqual(inspection.Valve.Coordinate.Longitude, result.Longitude);
            Assert.AreEqual(inspection.DateInspected, result.DateInspected);
            Assert.AreEqual(inspection.HydrantInspectionType.Description, result.HydrantInspectionType);
            Assert.AreEqual(inspection.GPM * inspection.MinutesFlowed, result.GallonsFlowed);
            Assert.IsTrue(result.FullFlow);
            Assert.AreEqual(inspection.ResidualChlorine, result.ResidualChlorine);
            Assert.AreEqual(inspection.StaticPressure, result.StaticPressure);
            Assert.AreEqual(inspection.WorkOrderRequestOne.Description, result.WorkOrderRequestOne);
            Assert.AreEqual(inspection.Remarks, result.Remarks);
            Assert.AreEqual(inspection.InspectedBy.UserName, result.InspectedBy);
            Assert.AreEqual(inspection.CreatedAt, result.DateAdded);
            Assert.AreEqual(inspection.MinutesFlowed, result.MinutesFlowed);
            //Assert.AreEqual(inspection.GPM, result.GPM);
        }

        [TestMethod]
        public void TestSearchBlowOffInspectionsForMapReturnsValveAssetCoordinatesFromBlowOffInspections()
        {
            var search = new TestSearchBlowOffInspection();

            var valve1 = GetFactory<ValveFactory>().Create();
            var valve2 = GetFactory<ValveFactory>().Create();
            var badValve = GetFactory<ValveFactory>().Create();

            GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve1,});
            GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve2});

            var result = Repository.SearchBlowOffInspectionsForMap(search);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Id == valve1.Id));
            Assert.IsTrue(result.Any(x => x.Id == valve2.Id));
            // ReSharper restore PossibleMultipleEnumeration
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
            var valve = GetFactory<ValveFactory>().Create();
            var inspectType = GetFactory<HydrantInspectionTypeFactory>().Create();
            var inspectionGood1 = GetFactory<BlowOffInspectionFactory>().Create(new {
                DateInspected = expectedStartDate, Valve = valve, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionGood2 = GetFactory<BlowOffInspectionFactory>().Create(new {
                DateInspected = expectedEndDate, Valve = valve, InspectedBy = user, HydrantInspectionType = inspectType
            });
            var inspectionBad = GetFactory<BlowOffInspectionFactory>().Create(new {
                DateInspected = expectedEndDate.AddDays(1), Valve = valve, InspectedBy = user,
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
            var valve = GetFactory<ValveFactory>().Create();
            var inspectType = GetFactory<HydrantInspectionTypeFactory>().Create(new {Description = "A Description"});
            var inspectType2 = GetFactory<HydrantInspectionTypeFactory>().Create(new {Description = "B Description"});
            var inspectionGood1 = GetFactory<BlowOffInspectionFactory>().Create(new {
                DateInspected = expectedStartDate, Valve = valve, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionGood2 = GetFactory<BlowOffInspectionFactory>().Create(new {
                DateInspected = expectedStartDate.AddHours(1), Valve = valve, InspectedBy = user,
                HydrantInspectionType = inspectType
            });
            var inspectionGood3 = GetFactory<BlowOffInspectionFactory>().Create(new {
                DateInspected = expectedStartDate.AddHours(2), Valve = valve, InspectedBy = user,
                HydrantInspectionType = inspectType2
            });

            var result = Repository.GetInspectionProductivityReport(search).ToArray();
            Assert.AreEqual(2, result.Count());

            Assert.AreEqual(expectedStartDate, result[0].DateInspected);
            Assert.AreEqual("BlowOff", result[0].AssetType);
            Assert.AreEqual("A Description", result[0].InspectionType);
            Assert.AreEqual(2, result[0].Count);
            Assert.AreEqual(user.FullName, result[0].InspectedBy);
            Assert.IsNull(result[0].ValveOperated, "This is not used by the BlowOffInspectionRepository.");
            Assert.IsNull(result[0].ValveSize, "This is not used by the BlowOffInspectionRepository.");

            Assert.AreEqual(expectedStartDate, result[1].DateInspected);
            Assert.AreEqual("BlowOff", result[1].AssetType);
            Assert.AreEqual("B Description", result[1].InspectionType);
            Assert.AreEqual(1, result[1].Count);
            Assert.AreEqual(user.FullName, result[1].InspectedBy);
            Assert.IsNull(result[1].ValveOperated, "This is not used by the BlowOffInspectionRepository.");
            Assert.IsNull(result[1].ValveSize, "This is not used by the BlowOffInspectionRepository.");
        }

        #region GetBlowOffInspectionsWithSapIssues

        [TestMethod]
        public void TestGetBlowOffInspectionsWithSapIssuesReturnsBlowOffInspectionsWithSapIssues()
        {
            var blowOffInspectionValid1 = GetFactory<BlowOffInspectionFactory>()
               .Create(new {SAPErrorCode = "RETRY::Something went wrong"});
            var blowOffInspectionInvalid1 = GetFactory<BlowOffInspectionFactory>().Create(new {SAPErrorCode = ""});
            var blowOffInspectionInvalid2 = GetFactory<BlowOffInspectionFactory>().Create();

            var result = Repository.GetBlowOffInspectionsWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region Test classes

        private class TestSearchBlowOffInspection : SearchSet<BlowOffInspectionSearchResultViewModel>,
            ISearchBlowOffInspection
        {
            public int[] OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? FireDistrict { get; set; }
            public int? ValveSuffix { get; set; }
            public int? HydrantInspectionType { get; set; }
            public int? InspectedBy { get; set; }
            public bool? WorkOrderRequired { get; set; }
            public int? WorkOrderRequestOne { get; set; }
            public int? FreeNoReadReason { get; set; }
            public int? TotalNoReadReason { get; set; }
            public bool? SAPEquipmentOnly { get; set; }
            public int? SAPEquipmentId { get; set; }
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

        //private class TestSearchBlowOffInspectionWorkOrder : SearchSet<BlowOffInspectionWorkOrderReportItem>, ISearchBlowOffInspectionWorkOrder
        //{
        //    public int? OperatingCenter { get; set; }
        //    public int? Town { get; set; }
        //}

        //private class TestSearchHydrantFlushing : SearchSet<HydrantFlushingReportItem>, ISearchHydrantFlushingReport
        //{
        //    public int? OperatingCenter { get; set; }

        //    public int? Year { get; set; }
        //}

        #endregion
    }
}
