using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCallMVC.Areas.Production.Models.ViewModels;
using System.Linq;
using MapCall.Common.Testing.Data;
using MMSINC.Testing;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class NonRevenueWaterEntryControllerTest : MapCallMvcControllerTestBase<NonRevenueWaterEntryController, NonRevenueWaterEntry, IRepository<NonRevenueWaterEntry>>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
                var entity = GetEntityFactory<NonRevenueWaterEntry>().Create(new { OperatingCenter = operatingCenter });
                return Repository.Find(entity.Id);
            };
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues["Month"] = 1;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.ProductionNonRevenueWaterUnbilledUsage;
                const string path = "~/Production/NonRevenueWaterEntry/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Show", role);
                a.RequiresRole(path + "Index", role);
                a.RequiresRole(path + "AddNonRevenueWaterAdjustment", role, RoleActions.Edit);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity,
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "Thing1",
                OperatingCenterName = "A"
            });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = "Thing2",
                OperatingCenterName = "B"
            });
            var entry1 = GetEntityFactory<NonRevenueWaterEntry>().Create(new { OperatingCenter = operatingCenter1 });
            var entry2 = GetEntityFactory<NonRevenueWaterEntry>().Create(new { OperatingCenter = operatingCenter2 });

            var search = new SearchNonRevenueWaterEntry { OperatingCenter = new int[] { operatingCenter1.Id, operatingCenter2.Id } };
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchNonRevenueWaterEntry)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
        }

        #endregion

        #region Children

        [TestMethod]
        public void TestAddNonRevenueWaterAdjustmentAddsAdjustmentToNonRevenueWaterEntry()
        {
            var entry = GetEntityFactory<NonRevenueWaterEntry>().Create();
            var adj = GetEntityFactory<NonRevenueWaterAdjustment>().Create();

            MyAssert.CausesIncrease(
                () => _target.AddNonRevenueWaterAdjustment(new AddNonRevenueWaterAdjustment(_container) {
                    Id = entry.Id,
                    Comments = adj.Comments,
                    BusinessUnit = adj.BusinessUnit,
                    TotalGallons = adj.TotalGallons
                }),_container.GetInstance<RepositoryBase<NonRevenueWaterAdjustment>>().GetAll().Count);
        }

        #endregion
    }
}