using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.ClassExtensions
{
    [TestClass]
    public class ControllerExtensionsTest : MapCallMvcInMemoryDatabaseTestBase<OperatingCenter>
    {
        #region Private Members

        private TestController _target;
        private Mock<IAuthenticationRepository<User>> _authRepo;
        private Mock<IAuthenticationService<User>> _authServ;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _authRepo = new Mock<IAuthenticationRepository<User>>();
            _authServ = new Mock<IAuthenticationService<User>>();

            _container.Inject(_authRepo.Object);
            _container.Inject(_authServ.Object);
            _container.Inject<IRepository<OperatingCenter>>(_container.GetInstance<OperatingCenterRepository>());

            _target = _container.GetInstance<TestController>();
        }

        #endregion

        #region Tests

        // AddOperatingCenterDropDownData, GetAllOperatingCenters<TRepository, TEntity>
        [TestMethod]
        public void TestAddOperatingCenterDropDownDataAddsAllTheAvailableOperatingCentersAddIEnumerableDropDownItem()
        {
            UniqueOperatingCenterFactory.ResetOpCenterCodeNumberCount();
            var operatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(3);

            foreach (var opc in operatingCenters)
            {
                Console.WriteLine(opc.Id + " " + opc.OperatingCenterCode + " " + opc.OperatingCenterName);
            }

            _target.AddOperatingCenterDropDownData();
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"]).ToArray();

            foreach (var opc in actual)
            {
                Console.WriteLine(opc.Value + " " + opc.Text);
            }

            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(operatingCenters[i].Id.ToString(), item.Value);
                Assert.AreEqual(operatingCenters[i].Description, item.Text);
            });
        }

        // AddOperatingCenterDropDownDataForRoleAndAction, GetUserOperatingCentersFn

        [TestMethod]
        public void TestAddOperatingCenterDropDownDataForRoleAndActionOnlyReturnsAllOperatingCentersForSiteAdmin()
        {
            const RoleModules role = RoleModules.HumanResourcesUnion;
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            var validOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(4);

            _target.AddOperatingCenterDropDownDataForRoleAndAction(role, RoleActions.Edit);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"]).ToArray();

            Assert.AreEqual(validOperatingCenters.Count, actual.Count());
        }

        [TestMethod]
        public void TestAddOperatingCenterDropDownDataForRoleAndActionOnlyReturnsOperatingCentersForRoleAndAction()
        {
            const RoleModules roleModule = RoleModules.HumanResourcesUnion;
            var validOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);
            var invalidOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateList(3);

            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = validOperatingCenters[0] });
            _authServ.Setup(x => x.CurrentUser).Returns(user);

            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new { Id = roleModule, Application = application });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Edit });

            foreach (var op in validOperatingCenters)
            {
                GetFactory<RoleFactory>().Create(new {
                    Application = application,
                    Module = module,
                    Action = action ,
                    OperatingCenter = op,
                    User = user
                });
            }
            Session.Save(user);

            _target.AddOperatingCenterDropDownDataForRoleAndAction(roleModule, RoleActions.Edit);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"]).ToArray();

            Assert.AreEqual(validOperatingCenters.Count, actual.Count());
            actual.EachWithIndex((item, i) =>
            {
                Assert.AreEqual(validOperatingCenters[i].Id.ToString(), item.Value);
                Assert.AreEqual(validOperatingCenters[i].Description, item.Text);
            });
        }

        // Sort
        [TestMethod]
        public void TestOperatingCentersSortFnSortsProperly()
        {
            var fl = new State {Name = "Florida", Abbreviation = "FL"};
            var nj = new State {Name = "New Jersey", Abbreviation = "NJ"};
            var ny = new State {Name = "New York", Abbreviation = "NY"};
            var ew1 = new OperatingCenter { State = nj, OperatingCenterCode = "EW1" };
            var nj7 = new OperatingCenter { State = nj, OperatingCenterCode = "NJ7" };
            var ny1 = new OperatingCenter { State = ny, OperatingCenterCode = "NY1" };
            var fl1 = new OperatingCenter { State = fl, OperatingCenterCode = "FL1" };

            var opCntrs = ControllerExtensions.OperatingCentersSortFn(new List<OperatingCenter> { ew1, ny1, fl1, nj7 }.AsQueryable()).ToArray();
            
            Assert.AreEqual(fl1, opCntrs[0]);
            Assert.AreEqual(ew1, opCntrs[1]);
            Assert.AreEqual(nj7, opCntrs[2]);
            Assert.AreEqual(ny1, opCntrs[3]);
        }

        #endregion
    }

    internal class TestController : ControllerBaseWithPersistence<OperatingCenterRepository,OperatingCenter,User> {
        public TestController(ControllerBaseWithPersistenceArguments<OperatingCenterRepository, OperatingCenter, User> args) : base(args) {}
    }
}
