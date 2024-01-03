using System;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class UserMapTest : InMemoryDatabaseTest<User>
    {
        #region Tests

        [TestMethod]
        public void
            TestIsAdminIsReadingFromTheIsSiteAdministratorColumnAndNotTheStupidIsAdministratorColumnThatDoesntMeantWhatYoudThinkItMeans()
        {
            var adminUser = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            var regularUser = GetFactory<UserFactory>().Create(new {IsAdmin = false});

            var query = Session.CreateSQLQuery("select [IsSiteAdministrator] from [tblPermissions] where [RecId] = " +
                                               adminUser.Id);
            var result = Convert.ToBoolean(query.List<long>().Single());
            Assert.IsTrue(result);

            query = Session.CreateSQLQuery("select [IsSiteAdministrator] from [tblPermissions] where [RecId] = " +
                                           regularUser.Id);
            result = Convert.ToBoolean(query.List<long>().Single());
            Assert.IsFalse(result);
        }

        #endregion
    }
}
