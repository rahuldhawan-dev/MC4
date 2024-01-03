using System.Linq;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using RestorationRepository = Contractors.Data.Models.Repositories.RestorationRepository;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class RestorationRepositoryTest : ContractorsControllerTestBase<Restoration, RestorationRepository>
    {
        #region Private Members

        private OperatingCenter _currentOperatingCenter;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _currentOperatingCenter = GetFactory<OperatingCenterFactory>().Create();
            Repository = _container.GetInstance<RestorationRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLinqFiltersRestorationsCorrectly()
        {
            var contractor1 = _currentUser.Contractor;
            var contractor2 = GetFactory<ContractorFactory>().Create();

            var restoration = GetFactory<RestorationFactory>().Create();

            // 1. Restorations ASSIGNED to the contractor must be returned.
            restoration.AssignedContractor = null;
            restoration.CreatedByContractor = null;
            Assert.IsFalse(Repository.GetAll().Any());

            restoration.AssignedContractor = contractor1;
            Session.Flush();
            Assert.AreSame(restoration, Repository.GetAll().Single());

            // 2. Restorations CREATED by the contractor must be returned.
            restoration.AssignedContractor = null;
            restoration.CreatedByContractor = contractor1;
            Session.Flush();
            Assert.AreSame(restoration, Repository.GetAll().Single());

            // 3. Restorations CREATED by the contractor but ASSIGNED to a DIFFERENT contractor must NOT be returned.
            restoration.AssignedContractor = contractor1;
            restoration.CreatedByContractor = contractor1;
            Session.Flush();
            Assert.AreSame(restoration, Repository.GetAll().Single());

            restoration.AssignedContractor = contractor2;
            Session.Flush();
            Assert.IsFalse(Repository.GetAll().Any());
        }

        [TestMethod]
        public void TestCriteriaFiltersRestorationsCorrectly()
        {
            var contractor1 = _currentUser.Contractor;
            var contractor2 = GetFactory<ContractorFactory>().Create();

            var restoration = GetFactory<RestorationFactory>().Create();

            // 1. Restorations ASSIGNED to the contractor must be returned.
            restoration.AssignedContractor = null;
            restoration.CreatedByContractor = null;
            Assert.IsFalse(Repository.Search(new EmptySearchSet<Restoration>()).Any());

            restoration.AssignedContractor = contractor1;
            Session.Flush();
            Assert.AreSame(restoration, Repository.Search(new EmptySearchSet<Restoration>()).Single());

            // 2. Restorations CREATED by the contractor must be returned.
            restoration.AssignedContractor = null;
            restoration.CreatedByContractor = contractor1;
            Session.Flush();
            Assert.AreSame(restoration, Repository.Search(new EmptySearchSet<Restoration>()).Single());

            // 3. Restorations CREATED by the contractor but ASSIGNED to a DIFFERENT contractor must NOT be returned.
            restoration.AssignedContractor = contractor1;
            restoration.CreatedByContractor = contractor1;
            Session.Flush();
            Assert.AreSame(restoration, Repository.Search(new EmptySearchSet<Restoration>()).Single());

            restoration.AssignedContractor = contractor2;
            Session.Flush();
            Assert.IsFalse(Repository.Search(new EmptySearchSet<Restoration>()).Any());
        }
        #endregion
    }
}
