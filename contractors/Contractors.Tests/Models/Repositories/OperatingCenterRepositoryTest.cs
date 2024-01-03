using System.Collections.Generic;
using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class OperatingCenterRepositoryTest : ContractorsControllerTestBase<OperatingCenter>
    {
        #region Private Members

        private OperatingCenterRepository _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void OperatingCenterRepositoryTestInitialize()
        {
            _target = _container.GetInstance<OperatingCenterRepository>();
        }

        #endregion

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToOperatingCentersThatTheCurrentUsersContractorHasAccessTo()
        {
            var expected = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2, new {
                Contractors = new List<Contractor>{_currentUser.Contractor}});
            var extra = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);

            expected.Each(oc => _currentUser.Contractor.OperatingCenters.Add(oc));
            Session.Save(_currentUser.Contractor);
            Session.Save(_currentUser);

            Session.Flush();
            Session.Clear();

            // .GetAll uses the Linq property (for now at least)
            var actual = _target.GetAll().ToArray();
            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToOperatingCentersThatTheCurrentUsersContractorHasAccessTo()
        {
            var expected = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extra = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);

            expected.Each(oc => _currentUser.Contractor.OperatingCenters.Add(oc));
            Session.Save(_currentUser.Contractor);
            Session.Save(_currentUser);

            Session.Flush();
            Session.Clear();

            // .Search uses the Criteria property (for now at least)
            var actual = _target.Search(Restrictions.Conjunction()).List<OperatingCenter>().ToArray();
            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }
    }
}
