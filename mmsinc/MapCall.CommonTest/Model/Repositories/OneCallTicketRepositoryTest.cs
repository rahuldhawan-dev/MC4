using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class OneCallTicketRepositoryTest : InMemoryDatabaseTest<OneCallTicket, OneCallTicketRepository>
    {
        #region Tests

        [TestMethod]
        public void TestSaveThrowsNotSupportedException()
        {
            MyAssert.Throws<NotSupportedException>(() => Repository.Save((OneCallTicket)null));
            MyAssert.Throws<NotSupportedException>(() => Repository.Save((IEnumerable<OneCallTicket>)null));
        }

        [TestMethod]
        public void TestDeleteThrowsNotSupportedException()
        {
            MyAssert.Throws<NotSupportedException>(() => Repository.Delete(null));
        }

        [TestMethod]
        public void TestFindByRequestNumberFindsByRequestNumber()
        {
            var expected = GetEntityFactory<OneCallTicket>().Create(new {RequestNumber = "123456789"});
            var unexpected = GetEntityFactory<OneCallTicket>().Create(new {RequestNumber = "434143341"});

            var result = Repository.FindByRequestNumber("123456789");
            Assert.AreSame(expected, result);
        }

        #endregion
    }
}
