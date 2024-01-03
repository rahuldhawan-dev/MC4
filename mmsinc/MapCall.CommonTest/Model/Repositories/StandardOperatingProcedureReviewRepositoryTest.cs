using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class StandardOperatingProcedureReviewRepositoryTest : MapCallMvcSecuredRepositoryTestBase<
        StandardOperatingProcedureReview, StandardOperatingProcedureReviewRepository, User>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IDateTimeProvider>().Use(_ => new Mock<IDateTimeProvider>().Object);
        }

        protected override User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            Session.Save(user);
            return user;
        }

        #endregion

        #region Tests

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnReviewsTheUsersDidNotAnswer()
        {
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ManagementGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);

            var insp1 = GetFactory<StandardOperatingProcedureReviewFactory>().Create(new {AnsweredBy = user});
            var insp2 = GetFactory<StandardOperatingProcedureReviewFactory>().Create();
            Assert.AreNotEqual(insp1.AnsweredBy.Id, insp2.AnsweredBy.Id, "sanity");
            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<StandardOperatingProcedureReviewRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsFalse(result.Contains(insp2));
        }

        [TestMethod]
        public void TestLinqReturnsAllReviewsIfUserIsUserAdministrator()
        {
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ManagementGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.UserAdministrator});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);

            var insp1 = GetFactory<StandardOperatingProcedureReviewFactory>().Create(new {AnsweredBy = user});
            var insp2 = GetFactory<StandardOperatingProcedureReviewFactory>().Create();

            Repository = _container.GetInstance<StandardOperatingProcedureReviewRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsTrue(result.Contains(insp2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnReviewsTheUsersDidNotAnswer()
        {
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ManagementGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);

            var insp1 = GetFactory<StandardOperatingProcedureReviewFactory>().Create(new {AnsweredBy = user});
            var insp2 = GetFactory<StandardOperatingProcedureReviewFactory>().Create();

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<StandardOperatingProcedureReviewRepository>();

            var model = new EmptySearchSet<StandardOperatingProcedureReview>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsFalse(result.Contains(insp2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllRecordsIfUserIsUserAdministrator()
        {
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ManagementGeneral});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.UserAdministrator});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);

            var insp1 = GetFactory<StandardOperatingProcedureReviewFactory>().Create(new {AnsweredBy = user});
            var insp2 = GetFactory<StandardOperatingProcedureReviewFactory>().Create();

            Repository = _container.GetInstance<StandardOperatingProcedureReviewRepository>();

            var model = new EmptySearchSet<StandardOperatingProcedureReview>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(insp1));
            Assert.IsTrue(result.Contains(insp2));
        }

        #endregion

        [TestMethod]
        public void TestTest()
        {
            var pg = GetFactory<PositionGroupFactory>().Create(new {
                CommonName = typeof(PositionGroupCommonNameFactory)
            });
            var pgcn = pg.CommonName;
            var sop = GetFactory<StandardOperatingProcedureFactory>().Create();

            sop.PGCNRequirements.Add(new StandardOperatingProcedurePositionGroupCommonNameRequirement {
                PositionGroupCommonName = pgcn,
                StandardOperatingProcedure = sop,
                Frequency = 1,
                FrequencyUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create()
            });

            Session.Save(sop);
            Session.Flush();
            var employee = GetFactory<EmployeeFactory>().Create(new {PositionGroup = pg});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, Employee = employee});

            var result = Repository.GetStandardOperatingProcedureReviewsDueForUser(user);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TestGetStandardOperatingProcedureReviewsDueForUserThrowsExceptionWhenUserDoesNotHaveLinkedEmployee()
        {
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            user.Employee = null;

            MyAssert.Throws<StandardOperatingProcedureReviewException>(
                () => Repository.GetStandardOperatingProcedureReviewsDueForUser(user),
                $"User#{user.Id} does not have a linked employee record.");
        }

        [TestMethod]
        public void
            TestGetStandardOperatingProcedureReviewsDueForUserThrowsExceptionWhenEmployeeDoesNotHaveLinkedPositionGroup()
        {
            var employee = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "12456"});
            employee.PositionGroup = null;
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, Employee = employee});

            MyAssert.Throws<StandardOperatingProcedureReviewException>(
                () => Repository.GetStandardOperatingProcedureReviewsDueForUser(user),
                $"Unable to get reviews because the employee('{employee.EmployeeId}') does not have a position group set.");
        }

        [TestMethod]
        public void
            TestGetStandardOperatingProcedureReviewsDueForUserThrowsExceptionWhenPositionGroupDoesNotHaveCommonName()
        {
            var pg = GetFactory<PositionGroupFactory>().Create();
            pg.CommonName = null;

            var employee = GetFactory<EmployeeFactory>().Create(new {PositionGroup = pg});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, Employee = employee});

            MyAssert.Throws<StandardOperatingProcedureReviewException>(
                () => Repository.GetStandardOperatingProcedureReviewsDueForUser(user),
                $"Unable to get reviews because the employee('{employee.EmployeeId}') has a position group('{employee.PositionGroup}') without a common name.");
        }

        #endregion
    }
}
