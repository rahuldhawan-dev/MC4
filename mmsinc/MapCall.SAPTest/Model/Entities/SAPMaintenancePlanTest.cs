using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.SAPTest.Model.Entities
{
    [TestClass()]
    public class SAPMaintenancePlanTest
    {
        [TestInitialize]
        public void TestInitialize() { }

        #region Public Methods

        public SAPMaintenancePlanLookup GetTestMaintenancePlanLookUp()
        {
            var SapMaintenancePlanLookup = new SAPMaintenancePlanLookup();
            SapMaintenancePlanLookup.FunctionalLocation = ""; //NJADW-HW
            SapMaintenancePlanLookup.EquipmentType = "";
            SapMaintenancePlanLookup.SAPEquipmentID = "";
            SapMaintenancePlanLookup.MaintenancePlan = "700000026752"; //700000002500

            return SapMaintenancePlanLookup;
        }

        public SAPMaintenancePlanLookup GetMaintenancePlanLookUpForNULLTest()
        {
            var SapMaintenancePlanLookup = new SAPMaintenancePlanLookup();
            SapMaintenancePlanLookup.FunctionalLocation = "abc";
            SapMaintenancePlanLookup.EquipmentType = "";
            SapMaintenancePlanLookup.SAPEquipmentID = "";
            SapMaintenancePlanLookup.MaintenancePlan = "123";

            return SapMaintenancePlanLookup;
        }

        public SAPMaintenancePlanUpdate GetTestMaintenancePlanUpdate()
        {
            var SapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate();
            SAPAddRemoveItem[] SapAddRemoveItem = new SAPAddRemoveItem[1] {
                new SAPAddRemoveItem {
                    MaintenancePlan = "700000002500",
                    Item = "2501",
                    Action = "ADD",
                    Equipment = "5007506",
                    FunctionalLocation = "NJLKW-LK-MAINS"
                }
            };
            SapMaintenancePlanUpdate.SapAddRemoveItem = SapAddRemoveItem;
            return SapMaintenancePlanUpdate;
        }

        public SAPMaintenancePlanUpdate GetTestMaintenancePlanTestFixCall()
        {
            var SapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate();
            SAPFixCall SapFixCall = new SAPFixCall {
                MaintenancePlan = "700000002500",
                CallNumber = "1",
                PlanDate = DateTime.Now
            };
            SapMaintenancePlanUpdate.SapFixCall = SapFixCall;
            return SapMaintenancePlanUpdate;
        }

        public SAPMaintenancePlanUpdate GetTestMaintenancePlanTestManualCall()
        {
            var SapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate();
            SAPManualCall SapManualCall = new SAPManualCall {
                MaintenancePlan = "700000002500",
                ManualCallDate = DateTime.Now
            };
            SapMaintenancePlanUpdate.SapManualCall = SapManualCall;
            return SapMaintenancePlanUpdate;
        }

        public SAPMaintenancePlanUpdate GetTestMaintenancePlanTestSkipCall()
        {
            var SapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate();
            SAPSkipCall SapSkipCall = new SAPSkipCall {
                MaintenancePlan = "700000002500",
                CallNumber = "1"
            };
            SapMaintenancePlanUpdate.SapSkipCall = SapSkipCall;
            return SapMaintenancePlanUpdate;
        }

        #endregion
    }
}
