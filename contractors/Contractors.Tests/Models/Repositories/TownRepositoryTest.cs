using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class TownRepositoryTest : ContractorsControllerTestBase<Town, TownRepository>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TownRepositoryTestInitialize()
        {
            Repository = _container.GetInstance<TownRepository>();
        }

        #endregion

        [TestMethod]
        public void TestLinqPropertyOnlyAllowsAccessToTownsBelongingToOperatingCentersThatTheCurrentUserCanAccess()
        {
            var expectedOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extraOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var expected = GetFactory<TownFactory>().CreateArray(2);
            var extra = GetFactory<TownFactory>().CreateArray(2);

            for (var i = 0; i < expectedOperatingCenters.Count(); ++i)
            {
                expected[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = expected[i],
                        OperatingCenter =
                            expectedOperatingCenters[i]
                    });
                extra[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = extra[i],
                        OperatingCenter = extraOperatingCenters[i]
                    });
                _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[i]);
                Session.SaveOrUpdate(expected[i]);
                Session.SaveOrUpdate(extra[i]);
            }
            Session.SaveOrUpdate(_currentUser.Contractor);

            Session.Flush();
            Session.Clear();

            // .GetAll uses the Linq property (for now at least)
            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaPropertyOnlyAllowsAccessToTownsBelongingToOperatingCentersThatTheCurrentUserCanAccess()
        {
            var expectedOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extraOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var expected = GetFactory<TownFactory>().CreateArray(2);
            var extra = GetFactory<TownFactory>().CreateArray(2);

            for (var i = 0; i < expectedOperatingCenters.Count(); ++i)
            {
                expected[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = expected[i],
                        OperatingCenter =
                            expectedOperatingCenters[i]
                    });
                extra[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = extra[i],
                        OperatingCenter = extraOperatingCenters[i]
                    });
                _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[i]);
                Session.SaveOrUpdate(expected[i]);
                Session.SaveOrUpdate(extra[i]);
            }
            Session.SaveOrUpdate(_currentUser.Contractor);

            Session.Flush();
            Session.Clear();

            // .Search uses the Criteria property (for now at least)
            var actual = Repository.Search(Restrictions.Conjunction()).List<Town>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestGetByOperatingCenterIdShouldFindAllTownsForTheGivenOperatingCenter()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var towns = GetFactory<TownFactory>().CreateList(3);
            var invalidTown = GetFactory<TownFactory>().Build();
            towns.Each(x => { 
                x.OperatingCentersTowns.Add(new OperatingCenterTown {
                    OperatingCenter = operatingCenter,
                    Town = x
                });
                Session.SaveOrUpdate(x);
            });
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            Session.SaveOrUpdate(_currentUser.Contractor);
            Session.Flush();
            var actual =
                Repository.GetByOperatingCenterId(
                    operatingCenter.Id).ToArray();

            Assert.AreEqual(towns.Count(), actual.Length);
            Assert.IsFalse(actual.Contains(invalidTown));
            towns.Each(x => Assert.IsTrue(actual.Contains(x)));
        }
    }
}
