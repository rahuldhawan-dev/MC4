using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class SearchUnionContractProposalViewModelTest : InMemoryDatabaseTest<UnionContractProposal, UnionContractProposalRepository>
    {
        #region Private Members

        private OperatingCenter _nj4, _nj7;
        private User _currentUser;
        private Local _nj4Local, _nj7Local;
        private UnionContract _nj4Contract, _nj7Contract;
        private UnionContractProposal _nj4Proposal, _nj7Proposal;
        private Mock<IAuthenticationService<User>> _authenticationService;

        #endregion

        #region Init/Cleanup

        private void SetupUser(User user)
        {
            _currentUser = user;
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(_currentUser);
            Repository = _container.GetInstance<UnionContractProposalRepository>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            _authenticationService = i.For<IAuthenticationService<User>>().Mock();
            i.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _nj4 = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ4"});
            _nj7 = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ7"});
            SetupUser(GetFactory<UserFactory>().Create());
            _nj4Local = GetFactory<LocalFactory>().Create(new {OperatingCenter = _nj4});
            _nj7Local = GetFactory<LocalFactory>().Create(new {OperatingCenter = _nj7});
            _nj4Contract = GetFactory<TestDataFactory<UnionContract>>().Create(new {
                OperatingCenter = _nj4, Local = _nj4Local
            });
            _nj7Contract = GetFactory<TestDataFactory<UnionContract>>().Create(new {
                OperatingCenter = _nj7, Local = _nj7Local
            });
            _nj4Proposal = GetFactory<TestDataFactory<UnionContractProposal>>().Create(new {Contract = _nj4Contract});
            _nj7Proposal = GetFactory<TestDataFactory<UnionContractProposal>>().Create(new {Contract = _nj7Contract});
        }

        #endregion

        [TestMethod]
        public void TestContractorCriteriaIsNotDuplicatedForRegularUsers()
        {
            GetFactory<RoleFactory>().Create(new {
                OperatingCenter = _nj4,
                User = _currentUser,
                Module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.HumanResourcesUnion})
            });
            GetFactory<RoleFactory>().Create(new {
                OperatingCenter = _nj7,
                User = _currentUser,
                Module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.HumanResourcesUnion})
            });

            var results = Repository.GetAll().ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.Contains(results, _nj7Proposal);

            var searchModel = new SearchUnionContractProposal();

            results = Repository.Search(searchModel).ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.Contains(results, _nj7Proposal);
          //  Assert.AreEqual(2, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));

            searchModel.OperatingCenter = _nj4.Id;

            results = Repository.Search(searchModel).ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.DoesNotContain(results, _nj7Proposal);
            //Assert.AreEqual(1, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));

            searchModel.OperatingCenter = null;
            searchModel.Contract = _nj7Contract.Id;

            results = Repository.Search(searchModel).ToArray();

            MyAssert.DoesNotContain(results, _nj4Proposal);
            MyAssert.Contains(results, _nj7Proposal);
         //   Assert.AreEqual(1, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));

            searchModel.Contract = null;
            searchModel.Local = _nj4Local.Id;

            results = Repository.Search(searchModel).ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.DoesNotContain(results, _nj7Proposal);
            // Assert.AreEqual(1, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));
        }

        [TestMethod]
        public void TestAdminCanSearchByOperatingCenterContractAndLocal()
        {
            SetupUser(GetFactory<AdminUserFactory>().Create());

            var searchModel = new SearchUnionContractProposal();

            var results = Repository.Search(searchModel).ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.Contains(results, _nj7Proposal);
            //Assert.AreEqual(2, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));

            searchModel.OperatingCenter = _nj4.Id;

            results = Repository.Search(searchModel).ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.DoesNotContain(results, _nj7Proposal);
           // Assert.AreEqual(1, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));
            
            searchModel.OperatingCenter = null;
            searchModel.Contract = _nj7Contract.Id;

            results = Repository.Search(searchModel).ToArray();

            MyAssert.DoesNotContain(results, _nj4Proposal);
            MyAssert.Contains(results, _nj7Proposal);
           // Assert.AreEqual(1, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));

            searchModel.Contract = null;
            searchModel.Local = _nj4Local.Id;

            results = Repository.Search(searchModel).ToArray();

            MyAssert.Contains(results, _nj4Proposal);
            MyAssert.DoesNotContain(results, _nj7Proposal);
          //  Assert.AreEqual(1, Repository.GetCountForCriterion(mapper.Map(), mapper.Aliases));
        }
    }
}
