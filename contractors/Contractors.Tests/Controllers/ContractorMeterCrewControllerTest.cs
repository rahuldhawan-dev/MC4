using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class ContractorMeterCrewControllerTest : ContractorControllerTestBase<ContractorMeterCrewController, ContractorMeterCrew>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<ContractorMeterCrewFactory>().Create(new { Contractor = _currentUser.Contractor });
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/Show/");
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/Index/");
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/Search/");
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/New/");
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/Create/");
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/Edit/");
                a.RequiresLoggedInUserOnly("~/ContractorMeterCrew/Update/");
            });
        }
    }
}