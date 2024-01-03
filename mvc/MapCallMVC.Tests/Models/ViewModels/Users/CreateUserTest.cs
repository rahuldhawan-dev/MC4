using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCallMVC.Models.ViewModels.Users;
using Moq;
using MMSINC.Authentication;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels.Users
{
    [TestClass]
    public class CreateUserTest : BaseUserViewModelTest<CreateUser>
    {
        #region Fields

        private Mock<IMembershipHelper> _membershipHelper;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            // UserName needs to be explicitly set in init because
            // it doesn't map automatically.
            _viewModel.UserName = "Some username";
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _membershipHelper = new Mock<IMembershipHelper>();
            e.For<IMembershipHelper>().Use(_membershipHelper.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSetDefaultsSetsHasAccessToTrue()
        {
            _viewModel.HasAccess = false;
            _viewModel.SetDefaults();
            Assert.IsTrue(_viewModel.HasAccess);
        }

        [TestMethod]
        public void TestSetDefaultsSetsIsUserAdminToFalse()
        {
            _viewModel.IsUserAdmin = true;
            _viewModel.SetDefaults();
            Assert.IsFalse(_viewModel.IsUserAdmin);
        }

        #region Mapping

        [TestMethod]
        public void TestMapToEntityTrimsUserName()
        {
            _viewModel.UserName = "   WHOA   ";
            _vmTester.MapToEntity();
            Assert.AreEqual("WHOA", _entity.UserName);
        }

        #endregion

        #region Validation
        
        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.UserName);
        }

        [TestMethod]
        public void TestUsernameMustBeUnique()
        {
            // This needs to check two places, the Users table and the Membership table.
            var existingUser = GetEntityFactory<User>().Create(new { UserName = "taken name" });
            _viewModel.UserName = "taken name";
            ValidationAssert.ModelStateHasError(x => x.UserName, "This username is already taken.");

            _membershipHelper.Setup(x => x.UserExists("taken membership username")).Returns(true);
            _viewModel.UserName = "taken membership username";
            ValidationAssert.ModelStateHasError(x => x.UserName, "This username is already taken by Membership Provider.");

            _viewModel.UserName = "free to use name";
            ValidationAssert.ModelStateIsValid(x => x.UserName);
        }

        #endregion

        #endregion
    }
}
