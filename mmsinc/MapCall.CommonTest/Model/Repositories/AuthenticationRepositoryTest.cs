using System;
using System.Linq;
using System.Runtime.InteropServices;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class AuthenticationRepositoryTest : InMemoryDatabaseTest<User, AuthenticationRepository>
    {
        #region Fields

        #endregion

        #region Tests

        //[TestMethod]
        //public void TestGetUserGetsUserWhenMultipleUsersHaveTheSameEmployeeId()
        //{
        //    // Due to a mapping change in UserMap and EmployeeMap, attempting to select a 
        //    // User is causing an error when multiple users have the same EmployeeId(EmpNum in tblPermissions)
        //    // and a matching Employee record exists.

        //    var employee = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "12345"});
        //    var user1 = GetFactory<UserFactory>().Create(new { UserName = "user1", EmployeeId = "12345"});
        //    var user2 = GetFactory<UserFactory>().Create(new { UserName = "user2", EmployeeId = "12345"});

        //    // Clear/Evict everything otherwise NHibernate won't do the full query that causes the error
        //    // unless it's uising one of its own Proxy entity objects.
        //    Session.Clear();
        //    MyAssert.DoesNotThrow(() => Repository.GetUser("user1"));
        //}

        #endregion
    }
}
