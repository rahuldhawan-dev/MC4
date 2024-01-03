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
    public class StreetRepositoryTest : ContractorsControllerTestBase<Street, StreetRepository>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void StreetRepositoryTestInitialize()
        {
            Repository = _container.GetInstance<StreetRepository>();
        }

        #endregion

        #region GetByTownId

        [TestMethod]
        public void GetByTownIdShouldFindAllStreetsForTheGivenTown()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var extraTown = GetFactory<TownFactory>().Create();
            var expected = GetFactory<StreetFactory>().CreateArray(3, new { Town = town });
            var extraStreet = GetFactory<StreetFactory>().Create(new {Town = extraTown});

            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            operatingCenter.OperatingCenterTowns.Add(new OperatingCenterTown {Town = town, OperatingCenter = operatingCenter});
            operatingCenter.OperatingCenterTowns.Add(new OperatingCenterTown {Town = extraTown, OperatingCenter = operatingCenter});
            Session.SaveOrUpdate(_currentUser.Contractor);
            Session.SaveOrUpdate(town);

            Session.Flush();
            Session.Clear();

            var actual = Repository.GetByTownId(town.Id).ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        #endregion

        #region Linq/Criteria Filtering

        [TestMethod]
        public void TestLinqPropertyOnlyAllowsAccessToTheStreetsBelongingToTheTownsBelongingToTheOperatingCentersThatTheCurrentUserCanAccess()
        {
            var expectedOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extraOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var expectedTowns = GetFactory<TownFactory>().CreateArray(2);
            var extraTowns = GetFactory<TownFactory>().CreateArray(2);
            var expected = new[] {
                GetFactory<StreetFactory>().Create(new {Town = expectedTowns[0]}),
                GetFactory<StreetFactory>().Create(new {Town = expectedTowns[1]})
            };
            var extra = new[] {
                GetFactory<StreetFactory>().Create(new {Town = extraTowns[0]}),
                GetFactory<StreetFactory>().Create(new {Town = extraTowns[1]})
            };

            for (var i = 0; i < expectedOperatingCenters.Count(); ++i)
            {
                _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[i]);
                expectedTowns[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = expectedTowns[i],
                        OperatingCenter =
                            expectedOperatingCenters[i]
                    });
                extraTowns[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = extraTowns[i],
                        OperatingCenter = extraOperatingCenters[i]
                    });
                Session.SaveOrUpdate(expectedTowns[i]);
                Session.SaveOrUpdate(extraTowns[i]);
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
        public void TestCriteriaPropertyOnlyAllowsAccessToTheStreetsBelongingToTheTownsBelongingToTheOperatingCentersThatTheCurrentUserCanAccess()
        {
            var expectedOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extraOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var expectedTowns = GetFactory<TownFactory>().CreateArray(2);
            var extraTowns = GetFactory<TownFactory>().CreateArray(2);
            var expected = new[] {
                GetFactory<StreetFactory>().Create(new {Town = expectedTowns[0]}),
                GetFactory<StreetFactory>().Create(new {Town = expectedTowns[1]})
            };
            var extra = new[] {
                GetFactory<StreetFactory>().Create(new {Town = extraTowns[0]}),
                GetFactory<StreetFactory>().Create(new {Town = extraTowns[1]})
            };

            for (var i = 0; i < expectedOperatingCenters.Count(); ++i)
            {
                _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[i]);
                expectedTowns[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = expectedTowns[i],
                        OperatingCenter =
                            expectedOperatingCenters[i]
                    });
                extraTowns[i].OperatingCentersTowns.Add(
                    new OperatingCenterTown {
                        Town = extraTowns[i],
                        OperatingCenter = extraOperatingCenters[i]
                    });
                Session.SaveOrUpdate(expectedTowns[i]);
                Session.SaveOrUpdate(extraTowns[i]);
            }
            Session.SaveOrUpdate(_currentUser.Contractor);

            Session.Flush();
            Session.Clear();

            // .Search uses the Criteria property (for now at least)
            var actual = Repository.Search(Restrictions.Conjunction()).List<Street>().ToArray();
            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        #endregion
    }
}
