using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FacilityKwhCostControllerTest : MapCallMvcControllerTestBase<FacilityKwhCostController, FacilityKwhCost>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IFacilityRepository>().Use<FacilityRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexRedirectsToShowForSingleResult = true;
            options.InitializeCreateViewModel = (vm) => {
                ((CreateFacilityKwhCost)vm).OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/FacilityKwhCost/Index");
                a.RequiresSiteAdminUser("~/FacilityKwhCost/Search");
                a.RequiresSiteAdminUser("~/FacilityKwhCost/Show");
                a.RequiresSiteAdminUser("~/FacilityKwhCost/New");
                a.RequiresSiteAdminUser("~/FacilityKwhCost/Create");
                a.RequiresSiteAdminUser("~/FacilityKwhCost/Edit");
                a.RequiresSiteAdminUser("~/FacilityKwhCost/Update");
            });
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<FacilityKwhCost>().Create();
            var expected = 6.66m;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditFacilityKwhCost, FacilityKwhCost>(eq, new {
                CostPerKwh = expected
            }));

            Assert.AreEqual(expected, Session.Get<FacilityKwhCost>(eq.Id).CostPerKwh);
        }

        #endregion
    }
}
