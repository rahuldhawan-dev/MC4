using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using SAP.DataTest.Model.Entities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Repositories;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPTechnicalMasterAccountRepositoryTest
    {
        #region Private Members

        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPTechnicalMasterAccountRepository _target;
        private IContainer _container;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());
            _target = _container.GetInstance<SAPTechnicalMasterAccountRepository>();
            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = _container.GetInstance<SearchSapTechnicalMaster>();

            var result = _target.Search(entity);

            Assert.AreEqual(SAPTechnicalMasterAccountRepository.ERROR_NO_SITE_CONNECTION, result.Items[0].SAPError);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestReturnsResultsForEquipmentSearch()
        {
            var SearchTechnicalMaster = _container.GetInstance<SAPTechnicalMasterAccountTest>()
                                                  .GetTechnicalMasterDataBasedOnEquipment();

            SAPTechnicalMasterAccountCollection actual = _target.Search(SearchTechnicalMaster);

            foreach (SAPTechnicalMasterAccount sapTechnicalMaster in actual)
            {
                Assert.AreEqual("Successful", sapTechnicalMaster.SAPError?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestReturnsResultsForPremiseSearch()
        {
            var SearchTechnicalMaster = _container.GetInstance<SAPTechnicalMasterAccountTest>()
                                                  .GetTechnicalMasterDataBasedOnPremiseNumber();

            SAPTechnicalMasterAccountCollection actual = _target.Search(SearchTechnicalMaster);

            foreach (SAPTechnicalMasterAccount sapTechnicalMaster in actual)
            {
                Assert.AreEqual("Successful", sapTechnicalMaster.SAPError?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestReturnsResultsForEquipmentAndPremise()
        {
            var SearchTechnicalMaster = _container.GetInstance<SAPTechnicalMasterAccountTest>()
                                                  .GetTechnicalMasterDataBasedOnEquipmentAndPremise();

            SAPTechnicalMasterAccountCollection actual = _target.Search(SearchTechnicalMaster);

            foreach (SAPTechnicalMasterAccount sapTechnicalMaster in actual)
            {
                Assert.AreEqual("Successful", sapTechnicalMaster.SAPError?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestReturnsResultsForEquipmentAndPremiseAndInstallation()
        {
            var SearchTechnicalMaster = _container.GetInstance<SAPTechnicalMasterAccountTest>()
                                                  .GetTechnicalMasterDataBasedOnEquipmentAndPremiseAndInstallation();

            SAPTechnicalMasterAccountCollection actual = _target.Search(SearchTechnicalMaster);

            foreach (SAPTechnicalMasterAccount sapTechnicalMaster in actual)
            {
                Assert.AreEqual("Successful", sapTechnicalMaster.SAPError?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchReturnsResultsForNoRecordFound()
        {
            var SearchTechnicalMaster = _container.GetInstance<SAPTechnicalMasterAccountTest>()
                                                  .GetTechnicalMasterIncorrectData();

            SAPTechnicalMasterAccountCollection actual = _target.Search(SearchTechnicalMaster);

            foreach (SAPTechnicalMasterAccount sapTechnicalMaster in actual)
            {
                Assert.AreEqual("Invalid Equipment Number", sapTechnicalMaster.SAPError?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchReturnsResultsForNull()
        {
            var SearchTechnicalMaster =
                _container.GetInstance<SAPTechnicalMasterAccountTest>().GetTechnicalMasterAllNULL();

            SAPTechnicalMasterAccountCollection actual = _target.Search(SearchTechnicalMaster);

            foreach (SAPTechnicalMasterAccount sapTechnicalMaster in actual)
            {
                Assert.AreEqual("Enter either Equipment or Premise", sapTechnicalMaster.SAPError?.ToString());
            }
        }
    }
}
