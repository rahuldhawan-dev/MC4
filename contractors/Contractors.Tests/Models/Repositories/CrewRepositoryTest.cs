using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
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
    public class CrewRepositoryTest : ContractorsControllerTestBase<Crew>
    {
        #region Fields

        private CrewRepository _target;

        #endregion

        #region Initializers

        [TestInitialize]
        public void CrewRepositoryTestInitialize()
        {
            _target = _container.GetInstance<CrewRepository>();
        }

        #endregion

        #region Test Methods

        #region Linq/Criteria Filtering

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToTheCrewsBelongingToTheContractorThatTheCurrentUserBelongsTo()
        {
            var expected = GetFactory<CrewFactory>().CreateArray(2, new {_currentUser.Contractor});
            // extras:
            GetFactory<CrewFactory>().CreateList(2, new {Contractor = GetFactory<ContractorFactory>().Create()});

            // .GetAll uses the Linq property (for now at least)
            var actual = _target.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToTheCrewsBelongingToTheContractorThatTheCurrentUserBelongsTo()
        {
            var expected = GetFactory<CrewFactory>().CreateArray(2, new {_currentUser.Contractor});
            // extras:
            GetFactory<CrewFactory>().CreateList(2, new {Contractor = GetFactory<ContractorFactory>().Create()});

            // .Search uses the Criteria property (for now at least)
            var actual = _target.Search(Restrictions.Conjunction()).List<Crew>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        #endregion

        #endregion
    }
}
