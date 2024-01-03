using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class ServiceRepositoryTest : ContractorsControllerTestBase<Service, ServiceRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<MapCall.Common.Model.Repositories.IServiceRepository>().Mock();
            i.For<MapCall.Common.Model.Repositories.ITapImageRepository>()
             .Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Repository = _container.GetInstance<ServiceRepository>();
        }

        #endregion

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToServicesBelongingToOperatingCentersWhichTheContractorHasAccessTo()
        {
            var expected = SetupLinqCriteriaTests();

            // .GetAll uses the Linq property (for now at least)
            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToServicesBelongingToOperatingCentersWhichTheContractorHasAccessTo()
        {
            var expected = SetupLinqCriteriaTests();

            // .Search uses the Criteria property (for now at least)
            var actual = Repository.Search(Restrictions.Conjunction()).List<Service>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        private Service[] SetupLinqCriteriaTests()
        {
            var expectedOperatingCenters =
                GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);

            _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[0]);
            _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[1]);

            Session.Save(_currentUser.Contractor);

            var extraOperatingCenters =
                GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);

            var expected = new[] {
                GetEntityFactory<Service>().Create(new {
                    OperatingCenter = expectedOperatingCenters[0]
                }),
                GetEntityFactory<Service>().Create(new {
                    OperatingCenter = expectedOperatingCenters[1]
                }),
            };
            var extra = new[] {
                GetEntityFactory<Service>().Create(new {
                    OperatingCenter = extraOperatingCenters[0]
                }),
                GetEntityFactory<Service>().Create(new {
                    OperatingCenter = extraOperatingCenters[1]
                }),
            };

            return expected;
        }
    }
}