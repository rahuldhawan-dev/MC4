using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class ForgotPasswordVerifyContractorUserTest
    {
        private IContainer _container;
        private IViewModelFactory _viewModelFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _viewModelFactory = _container.GetInstance<ViewModelFactory>();
        }

        [TestMethod]
        public void TestMapNullsPasswordProperty()
        {
            var cu = new ContractorUser();
            cu.Password = "password";

            var target = _viewModelFactory.Build<ForgotPasswordVerifyContractorUser, ContractorUser>(cu);
            Assert.IsNull(target.Password);
        }

        [TestMethod]
        public void TestMapNullsPasswordAnswerProperty()
        {
             var cu = new ContractorUser();
            cu.PasswordAnswer = "answer";

            var target = _viewModelFactory.Build<ForgotPasswordVerifyContractorUser, ContractorUser>(cu);

            Assert.IsNull(target.PasswordAnswer);
        }
    }
}
