using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SmartCoverAlertTest : MapCallMvcInMemoryDatabaseTestBase<SmartCoverAlert>
    {
        #region Tests

        [TestMethod]
        public void TestMostRecentWorkOrder()
        {
            var target = GetEntityFactory<SmartCoverAlert>().Create();
            var wo1 = GetEntityFactory<WorkOrder>().Create(new {
                SmartCoverAlert = target
            });
            var wo2 = GetEntityFactory<WorkOrder>().Create(new {
                SmartCoverAlert = target
            });
            Session.Flush();
            Session.Clear();

            target = Session.Load<SmartCoverAlert>(target.Id);

            Assert.AreEqual(wo2.Id, target.MostRecentWorkOrder.Id);
        }

        #endregion
    }
}
