using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class TapOrderControllerTest
        : ContractorControllerTestBase<TapOrderController, Service, ServiceRepository>
    {
        #region Fields

        private ContractorUser _user;
        private OperatingCenter _operatingCenter;
        private Contractor _contractor;

        #endregion

        #region Init/Cleanup

        protected override ContractorUser CreateUser()
        {
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _contractor = GetEntityFactory<Contractor>().Create();
            _contractor.OperatingCenters.Add(_operatingCenter);
            _user = GetFactory<ContractorUserFactory>().Create(new {
                Contractor = _contractor
            });
            return _user;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => a.RequiresLoggedInUserOnly("~/TapOrder/Show"));
        }
    }
}
