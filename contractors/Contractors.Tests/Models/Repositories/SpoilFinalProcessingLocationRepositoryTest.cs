using System;
using System.Collections.Generic;
using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class SpoilFinalProcessingLocationRepositoryTest : ContractorsControllerTestBase<SpoilFinalProcessingLocation>
    {
        #region Private Members

        private SpoilFinalProcessingLocationRepository _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _target = _container.GetInstance<SpoilFinalProcessingLocationRepository>();
        }

        #endregion

        private void TestAllAccess(Func<IEnumerable<SpoilFinalProcessingLocation>> getActualFunc)
        {
            var goodOpCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new
            {
                Contractors = new List<Contractor> { _currentUser.Contractor }
            });
            _currentUser.Contractor.OperatingCenters.Add(goodOpCenter);

            var badOpCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodStorageLocation = GetFactory<SpoilFinalProcessingLocationFactory>().Create(new { OperatingCenter = goodOpCenter });
            var badStorageLocation = GetFactory<SpoilFinalProcessingLocationFactory>().Create(new { OperatingCenter = badOpCenter });

            Session.Save(_currentUser.Contractor);
            Session.Save(_currentUser);

            Session.Flush();
            Session.Clear();

            // .GetAll uses the Linq property (for now at least)
            var actual = getActualFunc().ToArray();
            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.Any(x => x.Id == goodStorageLocation.Id));
            Assert.IsFalse(actual.Any(x => x.Id == badStorageLocation.Id));
        }

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToSpoilStorageLocationsInOperatingCentersThatTheCurrentUsersContractorHasAccessTo()
        {
            TestAllAccess(() => _target.GetAll());
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToSpoilStorageLocationsInOperatingCentersThatTheCurrentUsersContractorHasAccessTo()
        {
            TestAllAccess(() => _target.Search(Restrictions.Conjunction()).List<SpoilFinalProcessingLocation>());
        }
    }
}
