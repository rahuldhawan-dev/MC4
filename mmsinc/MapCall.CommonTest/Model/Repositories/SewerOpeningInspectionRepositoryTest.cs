using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SewerOpeningInspectionRepositoryTest : MapCallMvcSecuredRepositoryTestBase<SewerOpeningInspection,
        SewerOpeningInspectionRepository, User>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateProvider;

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        private const RoleModules SEWER_OPENING_INSPECTIONS_ROLE_MODULES = RoleModules.FieldServicesAssets;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateProvider = e.For<IDateTimeProvider>().Mock<IDateTimeProvider>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLinqDoesNotReturnSewerOpeningInspectionsFromOtherOperatingCentersForUser()
        {
            var opc1 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opc2 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var sm1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc1 });
            var sm2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc2 });

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = SEWER_OPENING_INSPECTIONS_ROLE_MODULES });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opc1 });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opc1,
                User = user
            });

            Session.Save(user);

            var sewerIns1 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm1 });
            var sewerIns2 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerOpeningInspectionRepository>();
            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(sewerIns1));
            Assert.IsFalse(result.Contains(sewerIns2));
        }

        [TestMethod]
        public void TestLinqReturnsAllSewerOpeningInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opc1 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opc2 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var sm1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc1 });
            var sm2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc2 });

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = SEWER_OPENING_INSPECTIONS_ROLE_MODULES });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opc1 });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var sewerIns1 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm1 });
            var sewerIns2 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerOpeningInspectionRepository>();
            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(sewerIns1));
            Assert.IsTrue(result.Contains(sewerIns2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnSewerOpeningInspectionsFromOtherOperatingCentersForUser()
        {
            var opc1 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opc2 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var sm1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc1 });
            var sm2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc2 });

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = SEWER_OPENING_INSPECTIONS_ROLE_MODULES });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opc1 });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opc1,
                User = user
            });

            Session.Save(user);

            var sewerIns1 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm1 });
            var sewerIns2 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerOpeningInspectionRepository>();

            var model = new EmptySearchSet<SewerOpeningInspection>();
            var result = Repository.Search(model);
            Assert.IsTrue(result.Contains(sewerIns1));
            Assert.IsFalse(result.Contains(sewerIns2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllSewerOpeningInspectionsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opc1 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opc2 = GetEntityFactory<OperatingCenter>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var sm1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc1 });
            var sm2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc2 });

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = SEWER_OPENING_INSPECTIONS_ROLE_MODULES });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opc1 });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var sewerIns1 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm1 });
            var sewerIns2 = GetEntityFactory<SewerOpeningInspection>().Create(new { SewerOpening = sm2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerOpeningInspectionRepository>();
            var model = new EmptySearchSet<SewerOpeningInspection>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(sewerIns1));
            Assert.IsTrue(result.Contains(sewerIns2));
        }

        [TestMethod]
        public void TestSearchInspectionsCorrectlyMapsToViewModel()
        {
            var sewerOpeningInspection = new SewerOpeningInspection();

            var inspection = GetFactory<SewerOpeningInspectionFactory>().Create(new {
                SewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                    Coordinate = typeof(CoordinateFactory),
                    FunctionalLocation = typeof(FunctionalLocationFactory)
                }),
                DateInspected = DateTime.Today,
                CreatedAt = DateTime.Today.AddHours(3)
            });
            var search = new TestSearchSewerOpeningInspection();
            Repository.SearchInspections(search);

            var result = search.Results.Single();
            Assert.AreEqual(inspection.Id, result.Id);
            Assert.AreEqual(inspection.SewerOpening.Id, result.SewerOpeningId);
            Assert.AreEqual(inspection.SewerOpening.OpeningNumber, result.OpeningNumber);
            Assert.AreEqual(inspection.SewerOpening.OpeningSuffix, result.OpeningSuffix);
            Assert.AreEqual(inspection.SewerOpening.Route, result.Route);
            Assert.AreEqual(inspection.SewerOpening.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(inspection.SewerOpening.Town.ShortName, result.Town);
            Assert.AreEqual(inspection.SewerOpening.FunctionalLocation.Description, result.FunctionalLocation);
            Assert.AreEqual(inspection.SewerOpening.Coordinate.Latitude, result.Latitude);
            Assert.AreEqual(inspection.SewerOpening.Coordinate.Longitude, result.Longitude);
            Assert.AreEqual(inspection.DateInspected, result.DateInspected);
            Assert.AreEqual(inspection.Remarks, result.Remarks);
            Assert.AreEqual(inspection.InspectedBy.UserName, result.InspectedBy);
            Assert.AreEqual(inspection.CreatedAt, result.DateAdded);
        }

        #endregion

        private class TestSearchSewerOpeningInspection : SearchSet<SewerOpeningInspectionSearchResultViewModel>,
            ISearchSewerOpeningInspection
        {
            public int? OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? OpeningSuffix { get; set; }
            public int? InspectedBy { get; set; }
            public DateRange DateInspected { get; set; }
            public int? Route { get; set; }
        }
    }
}
