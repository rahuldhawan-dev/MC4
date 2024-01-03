using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Integration
{
    /// <summary>
    /// Summary description for WorkOrdersRepositoryTest
    /// </summary>
    [TestClass]
    public class WorkOrdersRepositoryTest : EventFiringTestClass
    {
        #region Private Members

        private IUser _siteuser;
        private ISecurityService _securityService;
        
        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void WorkOrdersRepositoryTestInitialize()
        {
            _mocks
                .DynamicMock(out _siteuser)
                .DynamicMock(out _securityService);
        }

        [TestCleanup]
        public void WorkOrdersRepositoryTestCleanup()
        {
            
        }

        #endregion

        [TestMethod]
        public void TestSecurityServicePropertyReturnsMockedValueIfPresent()
        {
            _mocks.ReplayAll();
            // Set the private variable using reflection
            typeof(TestWorkOrdersRepository).SetHiddenStaticFieldValueByName(
                "_securityService", _securityService);
            
            // Get the private variable using reflection
            var expected =
                typeof(TestWorkOrdersRepository).GetHiddenStaticPropertyValueByName(
                    "SecurityService");
            
            Assert.AreSame(_securityService, expected);
        }

        // TODO: SECURITY SERVICE, ANYONE?
        //[TestMethod]
        //public void TestSecurityServicePropertyGetsSecurityServiceSingletonInstance()
        //{
        //    _mocks.ReplayAll();

        //    typeof(TestWorkOrdersRepository).SetHiddenStaticFieldValueByName(
        //        "_securityService", null);
        //    var expected =
        //        typeof(TestWorkOrdersRepository).GetHiddenStaticPropertyValueByName(
        //            "SecurityService");

        //    Assert.AreSame(SecurityService.Instance,expected);
        //}

        // TODO: SECURITY SERVICE, ANYONE?
        //[TestMethod]
        //public void TestCannotInsertEntityWhenUserIsNull()
        //{
        //    var order = new WorkOrder();
        //    WorkOrdersRepository<WorkOrder>.SetSiteUser(null);

        //    MyAssert.Throws(() => WorkOrdersRepository<WorkOrder>.Insert(order),
        //        typeof(UnauthorizedAccessException),
        //        "Attempting to insert an entity when the current user has not been set should throw an exception.");
        //    MyAssert.Throws(() => WorkOrderRepository.Insert(order),
        //        typeof(UnauthorizedAccessException),
        //        "Attempting to insert an entity when the current user has not been set should throw an exception.");
        //}
    }

    internal class TestWorkOrdersRepository : WorkOrdersRepository<WorkOrder>
    {
        
    }
}
