using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    public abstract class HydrantPaintingViewModelTestBase<TViewModel>
        : ViewModelTestBase<HydrantPainting, TViewModel>
        where TViewModel : HydrantPaintingViewModel
    {
        #region Private Members

        protected DateTime _now;
        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _authServ.Setup(x => x.CurrentUser).Returns(_user = GetEntityFactory<User>().Create());
        }

        #endregion

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.PaintedAt);
        }

        [TestMethod]
        public virtual void Test_MapToEntity_SetsChangeTrackingColumns()
        {
            var entity = _vmTester.MapToEntity();

            Assert.AreEqual(_now, entity.UpdatedAt);
            Assert.AreEqual(_user, entity.UpdatedBy);
        }
    }
}
