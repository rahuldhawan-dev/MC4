using System;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for InputWorkOrderTest
    /// </summary>
    [TestClass]
    public class InputWorkOrderTest
    {
        #region Private Members

        private HttpSimulator _simulator;

        #endregion

        #region Constants

        //------------------WORK ORDER VALUES------------------//
        // TODO: Get real example value from Doug.
        private string TEST_SERVICE_NUMBER;
        private string TEST_PREMISE_NUMBER;
        private string TEST_CUSTOMER_ACCOUNT_NUMBER;
        private DateTime TEST_DATE_RECEIVED;
        private AssetType TEST_ASSET_TYPE;
        private MarkoutRequirement TEST_MARKOUT_REQUIREMENT;
        private Valve TEST_VALVE;
        private WorkDescription TEST_DESCRIPTION;
        private WorkOrderPriority TEST_PRIORITY;
        private WorkOrderPurpose TEST_PURPOSE;
        private WorkOrderRequester TEST_REQUESTED_BY;
        private string TEST_CUSTOMER_NAME;
        private string TEST_STREET_NUMBER;
        private Street TEST_STREET;
        private Street TEST_CROSS_STREET;
        private Town TEST_TOWN;
        private TownSection TEST_TOWN_SECTION;
        private string TEST_PHONE_NUMBER;

        //--------------------MARKOUT VALUES-------------------//
        private DateTime TEST_DATE_OF_REQUEST;
        private string TEST_MARKOUT_NUMBER;

        #endregion

        #region Properties

        protected WorkOrder WorkOrderTarget { get; set; }
        protected Markout MarkoutTarget { get; set; }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void InputWorkOrderTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void InputWorkOrderTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        #region Private Methods

        protected void InitializeSampleValues()
        {
            //------------------WORK ORDER VALUES------------------//
            TEST_SERVICE_NUMBER = "1234567890";
            TEST_PREMISE_NUMBER = "12345678";
            TEST_CUSTOMER_ACCOUNT_NUMBER = "12345678901";
            TEST_DATE_RECEIVED = DateTime.Now;
            TEST_ASSET_TYPE = AssetTypeRepository.Valve;
            TEST_MARKOUT_REQUIREMENT = MarkoutRequirementRepository.Emergency;
            TEST_VALVE = ValveTest.GetValidValve();
            TEST_DESCRIPTION = WorkDescriptionTest.GetValidWorkDescription();
            TEST_PRIORITY = WorkOrderPriorityRepository.Emergency;
            TEST_PURPOSE = WorkOrderPurposeRepository.Customer;
            TEST_REQUESTED_BY = WorkOrderRequesterRepository.Customer;
            TEST_CUSTOMER_NAME = "John Smith";
            TEST_STREET_NUMBER = "123";
            // BRIGHTON
            TEST_STREET = StreetIntegrationTest.ReferenceStreet;
            // WOODMERE
            TEST_CROSS_STREET = StreetIntegrationTest.ReferenceCrossStreet;
            // NEPTUNE
            TEST_TOWN = TownIntegrationTest.ReferenceTown;
            // SHARK RIVER HILLS
            TEST_TOWN_SECTION = TownSectionIntegrationTest.ReferenceTownSection;
            TEST_PHONE_NUMBER = "555-867-5309";

            //--------------------MARKOUT VALUES-------------------//
            TEST_DATE_OF_REQUEST = DateTime.Now;
            TEST_MARKOUT_NUMBER = "123456789";
        }

        #endregion

        [TestMethod]
        public void DoTest()
        {
            Assert.Inconclusive("This test needs to be re-written.");
            //using (_simulator.SimulateRequest())
            //{
            //    InitializeSampleValues();

            //    // user first creates the WorkOrder
            //    MyAssert.DoesNotThrow(InputAndSaveWorkOrder);
            //    ReloadAndVerifyWorkOrder();

            //    // user creates the Markout requirement
            //    MyAssert.DoesNotThrow(InputAndSaveMarkout);
            //    ReloadAndVerifyMarkout();

            //    // test cleanup
            //    MyAssert.DoesNotThrow(CleanUpAndDelete);
            //}
        }

        //---------------------WORK ORDER----------------------//
        private void InputAndSaveWorkOrder()
        {
            WorkOrderTarget = new WorkOrder {
                ServiceNumber = TEST_SERVICE_NUMBER,
                PremiseNumber = TEST_PREMISE_NUMBER,
                CustomerAccountNumber = TEST_CUSTOMER_ACCOUNT_NUMBER,
                DateReceived = TEST_DATE_RECEIVED,
                AssetType = TEST_ASSET_TYPE,
                MarkoutRequirement = TEST_MARKOUT_REQUIREMENT,
                Valve = TEST_VALVE,
                WorkDescription = TEST_DESCRIPTION,
                RequestedBy = TEST_REQUESTED_BY,
                Priority = TEST_PRIORITY,
                DrivenBy = TEST_PURPOSE,
                CustomerName = TEST_CUSTOMER_NAME,
                StreetNumber = TEST_STREET_NUMBER,
                Street = TEST_STREET,
                NearestCrossStreet = TEST_CROSS_STREET,
                Town = TEST_TOWN,
                TownSection = TEST_TOWN_SECTION,
                PhoneNumber = TEST_PHONE_NUMBER
            };
            WorkOrderRepository.Insert(WorkOrderTarget);
        }

        private void ReloadAndVerifyWorkOrder()
        {
            MyAssert.IsGreaterThan(WorkOrderTarget.WorkOrderID, 0,
                                   "Target WorkOrder object did not get an ID value");
            WorkOrderTarget =
                WorkOrderRepository.GetEntity(
                    WorkOrderTarget);
            MyAssert.IsNotNullButInstanceOfType(WorkOrderTarget,
                                                typeof(WorkOrder),
                                                "Error reloading target WorkOrder object from the database");


            Assert.AreEqual(TEST_ASSET_TYPE, WorkOrderTarget.AssetType);
            Assert.AreEqual(TEST_DESCRIPTION, WorkOrderTarget.WorkDescription);
            Assert.AreEqual(TEST_PREMISE_NUMBER, WorkOrderTarget.PremiseNumber,
                            "Error with reloaded PremiseNumber value");
            Assert.AreEqual(TEST_CUSTOMER_ACCOUNT_NUMBER,
                            WorkOrderTarget.CustomerAccountNumber,
                            "Error with reloaded CustomerAccountNumber value");
            Assert.AreEqual(TEST_DATE_RECEIVED,
                            WorkOrderTarget.DateReceived.Value,
                            "Error with reloaded DateReceived value");
            Assert.AreEqual(TEST_REQUESTED_BY, WorkOrderTarget.RequestedBy,
                            "Error with reloaded RequestedBy value");
            Assert.AreSame(TEST_PRIORITY, WorkOrderTarget.Priority,
                           "Error with reloaded Priority value");
            Assert.AreEqual(TEST_CUSTOMER_NAME, WorkOrderTarget.CustomerName,
                            "Error with reloaded CustomerName value");
            Assert.AreEqual(TEST_STREET_NUMBER, WorkOrderTarget.StreetNumber,
                            "Error with reloaded StreetNumber value");
            Assert.AreSame(TEST_STREET, WorkOrderTarget.Street,
                           "Error with reloaded Street value");
            Assert.AreSame(TEST_CROSS_STREET, WorkOrderTarget.NearestCrossStreet,
                           "Error with reloaded NearestCrossStreet value");
            Assert.AreSame(TEST_TOWN, WorkOrderTarget.Town,
                           "Error with reloaded Town value");
            Assert.AreSame(TEST_TOWN_SECTION, WorkOrderTarget.TownSection,
                           "Error with reloaded TownSection value");
            Assert.AreEqual(TEST_PHONE_NUMBER, WorkOrderTarget.PhoneNumber,
                            "Error with reloaded PhoneNumber value");
        }

        //-----------------------MARKOUT-----------------------//
        private void InputAndSaveMarkout()
        {
            MarkoutTarget = new Markout {
                WorkOrder = WorkOrderTarget,
                DateOfRequest = TEST_DATE_OF_REQUEST,
                MarkoutNumber = TEST_MARKOUT_NUMBER
            };
            MarkoutRepository.Insert(MarkoutTarget);
        }

        private void ReloadAndVerifyMarkout()
        {
            MyAssert.IsGreaterThan(MarkoutTarget.MarkoutID, 0,
                                   "Target Markout object did not get an ID value");
            MarkoutTarget =
                MarkoutRepository.GetEntity(
                    MarkoutTarget);
            MyAssert.IsNotNullButInstanceOfType(MarkoutTarget, typeof(Markout),
                                                "Error reloading target Markout object from database");

            Assert.AreSame(WorkOrderTarget, MarkoutTarget.WorkOrder,
                           "Attached WorkOrder on target does not match target WorkOrder");
            Assert.AreSame(WorkOrderTarget.Markouts[0], MarkoutTarget,
                           "Target WorkOrder's first Markout does not match target Markout");
            Assert.AreEqual(TEST_DATE_OF_REQUEST, MarkoutTarget.DateOfRequest,
                            "Error with reloaded DateOfRequest value");
            Assert.AreEqual(TEST_MARKOUT_NUMBER, MarkoutTarget.MarkoutNumber,
                            "Error with reloaded MarkoutNumber value");
        }

        private void CleanUpAndDelete()
        {
            MarkoutRepository.Delete(MarkoutTarget);
            WorkOrderRepository.Delete(WorkOrderTarget);
        }
    }
}
