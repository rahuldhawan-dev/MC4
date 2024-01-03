using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class TailgateTalkTest : MapCallMvcInMemoryDatabaseTestBase<TailgateTalk>
    {
        #region Private Members

        private ViewModelTester<TailgateTalkViewModel, TailgateTalk> _vmTester;
        private TailgateTalkViewModel _viewModel;
        private TailgateTalk _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        
        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<ITailgateTalkRepository>().Use<TailgateTalkRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<TailgateTalkFactory>().Create();
            _viewModel = _viewModelFactory.Build<TailgateTalkViewModel, TailgateTalk>(_entity);
            _vmTester = new ViewModelTester<TailgateTalkViewModel, TailgateTalk>(_viewModel, _entity);
        }
        
        #endregion

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.HeldOn);
            _vmTester.CanMapBothWays(x => x.TrainingTimeHours);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Topic);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HeldOn);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PresentedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TrainingTimeHours);
        }

        [TestMethod]
        public void TestTailgateTalkTopicCanMapBothWays()
        {
            var topic = GetEntityFactory<TailgateTalkTopic>().Create(new {Description = "Foo"});
            _entity.Topic = topic;

            _vmTester.MapToViewModel();

            Assert.AreEqual(topic.Id, _viewModel.Topic);

            _entity.Topic = null;
            _vmTester.MapToEntity();

            Assert.AreSame(topic, _entity.Topic);
        }

        [TestMethod]
        public void TestPresentedByCanMapBothWays()
        {
            var employee = GetFactory<EmployeeFactory>().Create(new {FullName = "Foo D. Bar"});
            _entity.PresentedBy = employee;

            _vmTester.MapToViewModel();

            Assert.AreEqual(employee.Id, _viewModel.PresentedBy);

            _entity.PresentedBy = null;
            _vmTester.MapToEntity();

            Assert.AreSame(employee, _entity.PresentedBy);
        }
    }
}