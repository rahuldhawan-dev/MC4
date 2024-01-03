using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class MeterControllerTest : MapCallMvcControllerTestBase<MeterController, Meter>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IMeterRepository>().Use<MeterRepository>();
        }

        #endregion

        #region ByProfileId

        [TestMethod]
        public void TestByProfileIdReturnsACascadingResultWithMetersByTheChosenProfileId()
        {
            var profiles = GetFactory<MeterProfileFactory>().CreateArray(2);
            var meters = new[] {
                GetFactory<MeterFactory>().Create(new {Profile = profiles[0]}),
                GetFactory<MeterFactory>().Create(new {Profile = profiles[0]}),
                GetFactory<MeterFactory>().Create(new {Profile = profiles[1]}),
                GetFactory<MeterFactory>().Create(new {Profile = profiles[1]})
            };

            var result = _target.ByProfileId(profiles[0].Id) as CascadingActionResult;

            MyAssert.Contains(result.Data, meters[0]);
            MyAssert.Contains(result.Data, meters[1]);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Meter/ByProfileId/");
            });
        }
    }
}
