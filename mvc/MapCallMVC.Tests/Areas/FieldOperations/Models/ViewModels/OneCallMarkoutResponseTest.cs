using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateOneCallMarkoutResponseTest : MapCallMvcInMemoryDatabaseTestBase<OneCallMarkoutResponse>
    {
        #region Fields

        private ViewModelTester<CreateOneCallMarkoutResponse, OneCallMarkoutResponse> _vmTester;
        private CreateOneCallMarkoutResponse _viewModel;
        private OneCallMarkoutResponse _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);

            _viewModel = new CreateOneCallMarkoutResponse(_container);
            _entity = new OneCallMarkoutResponse();
            _vmTester = new ViewModelTester<CreateOneCallMarkoutResponse, OneCallMarkoutResponse>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Comments);
            _vmTester.CanMapBothWays(x => x.ReqNotified);
            _vmTester.CanMapBothWays(x => x.Paint);
            _vmTester.CanMapBothWays(x => x.Flag);
            _vmTester.CanMapBothWays(x => x.Stake);
            _vmTester.CanMapBothWays(x => x.Over500Feet);
            _vmTester.CanMapBothWays(x => x.CrewMarkoutIsNeeded);
            _vmTester.CanMapBothWays(x => x.NumberOfCsmo);
            _vmTester.CanMapBothWays(x => x.NumberOfCsmoUnableToLocate);
            _vmTester.CanMapBothWays(x => x.TotalTimeSpentForCsmo);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OneCallMarkoutTicket);
            //ValidationAssert.PropertyIsRequired(_viewModel, x => x.CompletedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CompletedAt);
        }

        [TestMethod]
        public void TestOneCallMarkoutResponseStatusCanMapBothWays()
        {
            var response = GetEntityFactory<OneCallMarkoutResponseStatus>().Create(new {Description = "Foo"});
            _entity.OneCallMarkoutResponseStatus = response;

            _vmTester.MapToViewModel();

            Assert.AreEqual(response.Id, _viewModel.OneCallMarkoutResponseStatus);

            _entity.OneCallMarkoutResponseStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(response, _entity.OneCallMarkoutResponseStatus);
        }

        [TestMethod]
        public void TestOneCallMarkoutResponseTechniqueCanMapBothWays()
        {
            var technique = GetEntityFactory<OneCallMarkoutResponseTechnique>().Create(new {Description = "Foo"});
            _entity.OneCallMarkoutResponseTechnique = technique;

            _vmTester.MapToViewModel();

            Assert.AreEqual(technique.Id, _viewModel.OneCallMarkoutResponseTechnique);

            _entity.OneCallMarkoutResponseTechnique = null;
            _vmTester.MapToEntity();

            Assert.AreSame(technique, _entity.OneCallMarkoutResponseTechnique);
        }

        [TestMethod]
        public void TestMapToEntitySetsCompletedByToCurrentUser()
        {
            _entity.CompletedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CompletedBy);
        }

        #endregion
    }

    [TestClass]
    public class EditOneCallMarkoutResponseTest : MapCallMvcInMemoryDatabaseTestBase<OneCallMarkoutResponse>
    {
        #region Fields

        private ViewModelTester<EditOneCallMarkoutResponse, OneCallMarkoutResponse> _vmTester;
        private EditOneCallMarkoutResponse _viewModel;
        private OneCallMarkoutResponse _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditOneCallMarkoutResponse(_container);
            _entity = new OneCallMarkoutResponse();
            _vmTester = new ViewModelTester<EditOneCallMarkoutResponse, OneCallMarkoutResponse>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Comments);
            _vmTester.CanMapBothWays(x => x.ReqNotified);
            _vmTester.CanMapBothWays(x => x.Paint);
            _vmTester.CanMapBothWays(x => x.Flag);
            _vmTester.CanMapBothWays(x => x.Stake);
            _vmTester.CanMapBothWays(x => x.Over500Feet);
            _vmTester.CanMapBothWays(x => x.CrewMarkoutIsNeeded);
            _vmTester.CanMapBothWays(x => x.NumberOfCsmo);
            _vmTester.CanMapBothWays(x => x.NumberOfCsmoUnableToLocate);
            _vmTester.CanMapBothWays(x => x.TotalTimeSpentForCsmo);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            //ValidationAssert.PropertyIsRequired(_viewModel, x => x.OneCallMarkoutTicket);
            //ValidationAssert.PropertyIsRequired(_viewModel, x => x.CompletedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CompletedAt);
        }

        [TestMethod]
        public void TestOneCallMarkoutResponseStatusCanMapBothWays()
        {
            var response = GetEntityFactory<OneCallMarkoutResponseStatus>().Create(new { Description = "Foo" });
            _entity.OneCallMarkoutResponseStatus = response;

            _vmTester.MapToViewModel();

            Assert.AreEqual(response.Id, _viewModel.OneCallMarkoutResponseStatus);

            _entity.OneCallMarkoutResponseStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(response, _entity.OneCallMarkoutResponseStatus);
        }

        [TestMethod]
        public void TestOneCallMarkoutResponseTechniqueCanMapBothWays()
        {
            var technique = GetEntityFactory<OneCallMarkoutResponseTechnique>().Create(new { Description = "Foo" });
            _entity.OneCallMarkoutResponseTechnique = technique;

            _vmTester.MapToViewModel();

            Assert.AreEqual(technique.Id, _viewModel.OneCallMarkoutResponseTechnique);

            _entity.OneCallMarkoutResponseTechnique = null;
            _vmTester.MapToEntity();

            Assert.AreSame(technique, _entity.OneCallMarkoutResponseTechnique);
        }

        #endregion
    }
}
