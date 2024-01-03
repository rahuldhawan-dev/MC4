using System.Web.Mvc;
using System.Web.Mvc.Html;

using Contractors.Helpers;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace Contractors.Tests.Helpers
{
    [TestClass]
    public class LinkHelperTest
    {
        #region Private Members

        private HtmlHelper _target;
        private Mock<IAuthenticationService<ContractorUser>> _mockAuthenticationService;
        private IContainer _container;

        #endregion

        [TestInitialize]
        public void LinkHelperTestInitialize()
        {
            _target = new HtmlHelper(new ViewContext(),
                                     new Mock<IViewDataContainer>().Object);
            _mockAuthenticationService = new Mock<IAuthenticationService<ContractorUser>>();
            _container = new Container();
            _container.Inject(_mockAuthenticationService.Object);
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void LinkHelperTestCleanup()
        {
            _mockAuthenticationService.VerifyAll();
            _container.Dispose();
        }

        [TestMethod]
        public void TestAdminActionLinkReturnsLinkIfCurrentUserIsAdmin()
        {
            string linkText = "linkText",
                   actionName = "actionName",
                   controllerName = "controllerName";

            _mockAuthenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(true);

            Assert.AreEqual(_target.ActionLink(linkText, actionName, controllerName).ToString(),
                            _target.AdminActionLink(linkText, actionName, controllerName).ToString());
        }

        [TestMethod]
        public void TestAdminActionLinkReturnsEmptyStringIfCurrentUserIsNotAdmin()
        {
            string linkText = "linkText",
                   actionName = "actionName",
                   controllerName = "controllerName";
            

            _mockAuthenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(false);

            Assert.AreEqual(string.Empty,
                            _target.AdminActionLink(linkText, actionName, controllerName).ToString());
        }

        [TestMethod]
        public void TestAdminActionMenuItemReturnsListItemWithLinkIfCurrentUserIsAdmin()
        {
            string linkText = "linkText",
                   actionName = "actionName",
                   controllerName = "controllerName";
            
            _mockAuthenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(true);

            Assert.AreEqual(
                string.Format(LinkHelper.LIST_ITEM_FORMAT, _target.ActionLink(linkText, actionName, controllerName)),
                _target.AdminActionMenuItem(linkText, actionName, controllerName).ToString());
        }

        [TestMethod]
        public void TestAdminActionMenuItemReturnsEmptyStringIfCurrentUserIsNotAdmin()
        {
            string linkText = "linkText",
                   actionName = "actionName",
                   controllerName = "controllerName";
            
            _mockAuthenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(false);

            Assert.AreEqual(string.Empty,
                _target.AdminActionMenuItem(linkText, actionName, controllerName).ToString());
        }
    }
}
