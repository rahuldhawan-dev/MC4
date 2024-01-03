using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.TestingTest.NHibernate
{
    [TestClass]
    public class InMemoryDatabaseTestTest : InMemoryDatabaseTest<TestUser, TestUserRepository>
    {
        #region Setup/Teardown

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(TestUserFactory).Assembly);
        }

        #endregion

        // [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
        [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
        [TestMethod]
        public void TestCreateAUserOrTwo()
        {
            var users = new[] {
                new TestUser {Email = "User1@site.com", SomeForeignId = 123},
                new TestUser {Email = "User2@site.com", SomeForeignId = 456}
            };

            foreach (var user in users)
            {
                Repository.Save(user);
            }

            Assert.AreEqual(2, Repository.Count());
        }

        [TestMethod]
        public void TestGetFactoryByFactoryType()
        {
            var factory = GetFactory<TestUserFactory>();

            Assert.IsInstanceOfType(factory, typeof(TestUserFactory));
            Assert.AreSame(factory, GetFactory<TestUserFactory>());
        }

        [TestMethod]
        public void TestGetFactoryByEntityType()
        {
            var factory = GetEntityFactory<TestUser>();

            Assert.IsInstanceOfType(factory, typeof(TestUserFactory));
            Assert.AreSame(factory, GetEntityFactory<TestUser>());
            Assert.AreSame(factory, GetFactory<TestUserFactory>());
        }
    }
}
