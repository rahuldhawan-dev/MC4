using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class IncidentInvestigationRootCauseLevel2TypeControllerTest : MapCallMvcControllerTestBase<IncidentInvestigationRootCauseLevel2TypeController, IncidentInvestigationRootCauseLevel2Type>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/IncidentInvestigationRootCauseLevel2Type/ByLevel1/");
            });
        }

        #endregion

        #region ByLevel1

        [TestMethod]
        public void TestByLevel1ReturnsOnlyResultsWithAMatchingLevel1()
        {
            var good = GetEntityFactory<IncidentInvestigationRootCauseLevel2Type>().Create();
            var bad = GetEntityFactory<IncidentInvestigationRootCauseLevel2Type>().Create();

            var result = _target.ByLevel1(good.IncidentInvestigationRootCauseLevel1Type.Id);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(good.Id, data.Single().Id);
        }

        #endregion
    }
}
