using System;
using System.Collections.Generic;
using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class SpoilRepositoryTest : ContractorsControllerTestBase<Spoil>
    {
        #region Private Members

        private SpoilRepository _target;

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
            _target = _container.GetInstance<SpoilRepository>();
        }

        #endregion

        private void TestAllAccess(Func<IEnumerable<Spoil>> getActualFunc)
        {
            var goodWorkOrder = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor
            });

            var goodSpoil = GetFactory<SpoilFactory>().Create(new { WorkOrder = goodWorkOrder });
            var badSpoil = GetFactory<SpoilFactory>().Create();

            var actual = getActualFunc().ToArray();
            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.Any(x => x.Id == goodSpoil.Id));
            Assert.IsFalse(actual.Any(x => x.Id == badSpoil.Id));
        }

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToSpoilStorageLocationsInOperatingCentersThatTheCurrentUsersContractorHasAccessTo()
        {
            TestAllAccess(() => _target.GetAll());
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToSpoilStorageLocationsInOperatingCentersThatTheCurrentUsersContractorHasAccessTo()
        {
            TestAllAccess(() => _target.Search(Restrictions.Conjunction()).List<Spoil>());
        }
    }
}
