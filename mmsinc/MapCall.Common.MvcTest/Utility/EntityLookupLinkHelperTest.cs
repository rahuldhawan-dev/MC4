using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCall.Common.MvcTest.Utility
{
    [TestClass, DoNotParallelize]
    public class EntityLookupLinkHelperTest
    {
        private Mock<IAuthenticationService> _authService;
        private FakeMvcHttpHandler _handler;
        private EntityLookupLinkHelper _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject(
                (_authService = new Mock<IAuthenticationService>()).Object);
            _handler = new FakeMvcHttpHandler(_container);

            // either of these changing will invalidate the tests
            Assert.IsTrue(typeof(EquipmentStatus).IsSubclassOf(typeof(EntityLookup)),
                "Pre-test check has failed, please examine the comments");
            Assert.IsTrue(typeof(EquipmentStatus).IsSubclassOf(typeof(ReadOnlyEntityLookup)),
                "Pre-test check has failed, please examine the comments");
            Assert.IsTrue(typeof(EquipmentPurpose).IsSubclassOf(typeof(ReadOnlyEntityLookup)),
                "Pre-test check has failed, please examine the comments");
            Assert.IsFalse(typeof(EquipmentPurpose).IsSubclassOf(typeof(EntityLookup)),
                "Pre-test check has failed, please examine the comments");

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        private void InitTarget(Type viewModelType, string property)
        {
            _target = new EntityLookupLinkHelper(_handler.CreateHtmlHelper<Town>(), viewModelType, property);
        }

        [TestMethod]
        public void TestRenderReturnsNullWhenCurrentUserIsNotAdmin()
        {
            InitTarget(typeof(TestViewModel), "EqiupmentStatus");
            _authService.Setup(x => x.CurrentUserIsAdmin).Returns(false);

            Assert.IsNull(_target.Render());
        }

        [TestMethod]
        public void TestRenderReturnsNullWhenPropertyIsNotReferenceToEntityLookup()
        {
            InitTarget(typeof(TestViewModel), "Identifier");
            _authService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            Assert.IsNull(_target.Render());
        }

        [TestMethod]
        public void TestRenderReturnsNullWhenPropertyIsReadOnlyEntityLookup()
        {
            InitTarget(typeof(TestViewModel), "EquipmentPurpose");
            _authService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            Assert.IsNull(_target.Render());
        }

        [TestMethod]
        public void TestRenderReturnsLinkWhenUserIsAdminAndPropertyIsEntityLookup()
        {
            InitTarget(typeof(TestViewModel), "EquipmentStatus");
            _authService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            Assert.AreEqual("<a class=\"link-button entity-lookup-settings\" href=\"/EquipmentStatus\"> </a>",
                _target.Render().ToString());
        }

        private class TestViewModel : ViewModel<Equipment>
        {
            public TestViewModel(IContainer container, Equipment entity) : base(container)
            {
                if (entity != null)
                {
                    Map(entity);
                }
            }
        }
    }
}
