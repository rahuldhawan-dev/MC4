using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class IncidentInvestigationRootCauseLevel3TypeControllerTest : MapCallMvcControllerTestBase<IncidentInvestigationRootCauseLevel3TypeController, IncidentInvestigationRootCauseLevel3Type>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/IncidentInvestigationRootCauseLevel3Type/ByLevel2/");
            });
        }

        #endregion

        #region ByLevel2

        [TestMethod]
        public void TestByLevel2ReturnsOnlyResultsWithAMatchingLevel2()
        {
            var good = GetEntityFactory<IncidentInvestigationRootCauseLevel3Type>().Create();
            var bad = GetEntityFactory<IncidentInvestigationRootCauseLevel3Type>().Create();

            var result = _target.ByLevel2(good.IncidentInvestigationRootCauseLevel2Type.Id);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(good.Id, data.Single().Id);
        }

        #endregion
    }
}
