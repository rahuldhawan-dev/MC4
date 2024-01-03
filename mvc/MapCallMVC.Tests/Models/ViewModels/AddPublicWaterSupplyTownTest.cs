using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AddPublicWaterSupplyTownTest : MapCallMvcInMemoryDatabaseTestBase<Town>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestAddPublicWaterSupplyMapToEntityAddsPublicWaterSupply()
        {
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var target = _viewModelFactory.BuildWithOverrides<AddPublicWaterSupplyTown, Town>(town, new {PublicWaterSupplyId = pws.Id});
            var entity = new Town();

            target.MapToEntity(entity);

            Assert.AreEqual(1, entity.PublicWaterSupplies.Count);
            Assert.AreSame(pws, entity.PublicWaterSupplies.First());
        }

        #endregion
    }
}
