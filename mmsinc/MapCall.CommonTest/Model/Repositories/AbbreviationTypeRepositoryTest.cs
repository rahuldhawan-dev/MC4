using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class AbbreviationTypeRepositoryTest : InMemoryDatabaseTest<AbbreviationType, AbbreviationTypeRepository>
    {
        #region Fields

        private AbbreviationType _town, _townSection;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _town = GetFactory<TownAbbreviationTypeFactory>().Create();
            _townSection = GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetTownAbbreviationTypeReturnsTheAbbreviationTypeThatHasADescriptionWithTheWordTown()
        {
            Assert.AreSame(_town, Repository.GetTownAbbreviationType());
        }

        [TestMethod]
        public void
            TestGetTownSectionAbbreviationTypeReturnsTheAbbreviationTypeThatHasADescriptionWithTheWordTownSection()
        {
            Assert.AreSame(_townSection, Repository.GetTownSectionAbbreviationType());
        }

        #endregion
    }
}
