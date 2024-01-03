using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class StateRepositoryTest : InMemoryDatabaseTest<State, StateRepository>
    {
        #region Tests

        [TestMethod]
        public void TestFindByAbbreviationReturnsStateByAbbreviation()
        {
            var state = GetEntityFactory<State>().Create(new {Abbreviation = "QQ"});
            var otherState = GetEntityFactory<State>().Create(new {Abbreviation = "XX"});
            Assert.AreSame(state, Repository.FindByAbbreviation("QQ"));
            Assert.AreSame(otherState, Repository.FindByAbbreviation("XX"));
        }

        [TestMethod]
        public void TestFindByAbbreviationThrowsAnExceptionIfBySomeActOfGodStatesSuddenlyDoNotHaveUniqueAbbreviations()
        {
            var state = GetEntityFactory<State>().Create(new {Abbreviation = "QQ"});
            var otherState = GetEntityFactory<State>().Create(new {Abbreviation = "QQ"});
            MyAssert.Throws(() => Repository.FindByAbbreviation("QQ"));
        }

        #endregion
    }
}
