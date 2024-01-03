using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class GeneralWorkOrderRepositoryTest : MapCallMvcSecuredRepositoryTestBase<WorkOrder, TestGeneralWorkOrderRepository, User>
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
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCriteriaDoesNotReturnMoreThenThreeThousandWorkOrders()
        {
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.FieldServicesWorkManagement});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();
            var markoutReq = GetFactory<NoneMarkoutRequirementFactory>().Create();
            var street = GetEntityFactory<Street>().Create();
            var priority = GetFactory<RoutineWorkOrderPriorityFactory>().Create();
            var purpose = GetFactory<RevenueAbove1000WorkOrderPurposeFactory>().Create();
            var requestedBy = GetFactory<WorkOrderRequesterFactory>().Create();
            var workDescription = GetFactory<ValveBoxRepairWorkDescriptionFactory>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var valve = GetEntityFactory<Valve>().Create();
            var town = GetEntityFactory<Town>().Create();
            var createdBy = GetEntityFactory<User>().Create();

            // Creating this many objects takes several seconds, so create the least amount
            // of them needed for this test to pass.
            var workOrders = GetEntityFactory<WorkOrder>().CreateList(3001, new {
                Valve = valve,
                Town = town,
                CreatedBy = createdBy,
                AssetType = assetType,
                MarkoutRequirement = markoutReq,
                NearestCrossStreet = street,
                Priority = priority,
                Purpose = purpose,
                RequestedBy = requestedBy,
                Street = street,
                WorkDescription = workDescription,
                OperatingCenter = operatingCenter
            });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGeneralWorkOrderRepository>();

            var result = Repository.iCanHasCriteria().List<WorkOrder>();

            Assert.AreEqual(GeneralWorkOrderRepository.MAX_RESULTS, result.Count);
        }

         #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnFacilitiesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var woMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr1});
            var woNotAMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<TestGeneralWorkOrderRepository>();
            var model = new EmptySearchSet<WorkOrder>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(woMatch));
            Assert.IsFalse(result.Contains(woNotAMatch));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllFacilitiesIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var woMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr1});
            var woAlsoAMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<TestGeneralWorkOrderRepository>();
            var model = new EmptySearchSet<WorkOrder>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(woMatch));
            Assert.IsTrue(result.Contains(woAlsoAMatch));
        }

        #endregion 

        #endregion
    }

    public class TestGeneralWorkOrderRepository : GeneralWorkOrderRepository
    {
        public ICriteria iCanHasCriteria()
        {
            return Criteria;
        }

        public TestGeneralWorkOrderRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container, authenticationService, roleRepo, new DateTimeProvider() ) { }
    }
}
