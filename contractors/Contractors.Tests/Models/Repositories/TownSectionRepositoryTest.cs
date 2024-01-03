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
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class TownSectionRepositoryTest : ContractorsControllerTestBase<TownSection, TownSectionRepository>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TownSectionRepositoryTestInitialize()
        {
            Repository = _container.GetInstance<TownSectionRepository>();
        }

        #endregion

        [TestMethod]
        public void TestLinqPropertyOnlyAllowsAccessToTheTownSectionsBelongingToTownsBelongingToTheOperatingCentersThatTheCurrentUserCanAccess()
        {
            var expectedOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extraOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var expectedTowns = GetFactory<TownFactory>().CreateArray(2);
            var extraTowns = GetFactory<TownFactory>().CreateArray(2);
            var expected = new[] {
                GetFactory<TownSectionFactory>().Create(new {Town = expectedTowns[0]}),
                GetFactory<TownSectionFactory>().Create(new {Town = expectedTowns[1]})
            };
            var extra = new[] {
                GetFactory<TownSectionFactory>().Create(new {Town = extraTowns[0]}),
                GetFactory<TownSectionFactory>().Create(new {Town = extraTowns[1]})
            };

            for (var i = 0; i < expectedOperatingCenters.Count(); ++i)
            {
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
                _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[i]);
                Session.SaveOrUpdate(expectedTowns[i]);
                Session.SaveOrUpdate(extraTowns[i]);
            }
            Session.SaveOrUpdate(_currentUser.Contractor);
            Session.Flush();
            // .GetAll uses the Linq property (for now at least)
            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaPropertyOnlyAllowsAccessToTheTownSectionsBelongingToTownsBelongingToTheOperatingCentersThatTheCurrentUserCanAccess()
        {
            var expectedOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var extraOperatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var expectedTowns = GetFactory<TownFactory>().CreateArray(2);
            var extraTowns = GetFactory<TownFactory>().CreateArray(2);
            var expected = new[] {
                GetFactory<TownSectionFactory>().Create(new {Town = expectedTowns[0]}),
                GetFactory<TownSectionFactory>().Create(new {Town = expectedTowns[1]})
            };
            var extra = new[] {
                GetFactory<TownSectionFactory>().Create(new {Town = extraTowns[0]}),
                GetFactory<TownSectionFactory>().Create(new {Town = extraTowns[1]})
            };

            for (var i = 0; i < expectedOperatingCenters.Count(); ++i)
            {
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
                _currentUser.Contractor.OperatingCenters.Add(expectedOperatingCenters[i]);
                Session.SaveOrUpdate(expectedTowns[i]);
                Session.SaveOrUpdate(extraTowns[i]);
            }
            Session.SaveOrUpdate(_currentUser.Contractor);
            Session.Flush();
            // .GetAll uses the Criteria property (for now at least)
            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void GetByTownIdShouldFindAllTownSectionsForTheGivenTown()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var townSections = GetFactory<TownSectionFactory>().CreateList(3, new {Town = town});
            var extraTownSection = GetFactory<TownSectionFactory>().Create(new { Town = GetFactory<TownFactory>().Create(), Name = "Not Me" });

            town.OperatingCentersTowns.Add(
                new OperatingCenterTown {
                    OperatingCenter = operatingCenter,
                    Town = town
                });
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
         //   Session.SaveOrUpdate(town);
          //  Session.SaveOrUpdate(_currentUser.Contractor);
            Session.Flush();
            var actual = Repository.GetByTownId(town.Id).ToArray();

            Assert.AreEqual(townSections.Count(),actual.Length);
            Assert.IsFalse(actual.Contains(extraTownSection));
            foreach(var ts in townSections)
            {
                Assert.IsTrue(actual.Contains(ts));
            }
        }
    }
}
