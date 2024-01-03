using System;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class WorkOrderSearchTest : InMemoryDatabaseTest<WorkOrder>
    {
        #region Fields

        private Mock<IAuthenticationService<ContractorUser>> _authServ;
        private ContractorUser _currentUser;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<ContractorUser>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _currentUser = GetFactory<ContractorUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestValidationFailsIfQueryIsNullReturnsTrue()
        {
            Action<WorkOrderSearch> doTest = (search) => {
                Assert.IsTrue(search.QueryIsNull());
                ValidationAssert.ModelStateHasNonPropertySpecificError(search, "No search criteria chosen.");
            };
            doTest(new WorkOrderGeneralSearch());
            doTest(new WorkOrderPlanningSearch());
            doTest(new WorkOrderSchedulingSearch());
            doTest(new WorkOrderFinalizationSearch());
        }

        [TestMethod]
        public void TestValidationSucceedsIfOnlyIdIsSet()
        {
            Action<WorkOrderSearch> doTest = (search) => {
                search.Id = 1;
                ValidationAssert.ModelStateIsValid(search);
            };
            doTest(new WorkOrderGeneralSearch());
            doTest(new WorkOrderPlanningSearch());
            doTest(new WorkOrderSchedulingSearch());
            doTest(new WorkOrderFinalizationSearch());

            Action<WorkOrderSearch> doAnotherTest = (search) => {
                // All these models have OperatingCenter set. Adding an Id value should fail.
                search.Id = 1;
                ValidationAssert.ModelStateHasNonPropertySpecificError(search, "You must choose either Work Order Number or any combination of the other fields.");

                // Removing the Id value should then pass.
                search.Id = null;
                ValidationAssert.ModelStateIsValid(search);
            };
            doAnotherTest(new WorkOrderGeneralSearch() { OperatingCenter = 10 });
            doAnotherTest(new WorkOrderPlanningSearch() { OperatingCenter = 10 });
            doAnotherTest(new WorkOrderSchedulingSearch() { OperatingCenter = 10 });
            doAnotherTest(new WorkOrderFinalizationSearch() { OperatingCenter = 10 });
        }


        #endregion
    }
}
