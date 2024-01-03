using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class CompletedWorkControllerTest : ContractorControllerTestBase<CompletedWorkController, MeterChangeOut, MeterChangeOutRepository>
    {
        private MeterChangeOutStatus _changedStatus;
        private MeterChangeOutContract _contract;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _contract = GetEntityFactory<MeterChangeOutContract>()
               .Create(new {Contractor = _currentUser.Contractor});
            // "Changed"is Id 3
            GetEntityFactory<MeterChangeOutStatus>().CreateList(2);
            _changedStatus = GetEntityFactory<MeterChangeOutStatus>()
               .Create(new {Description = "Changed"});
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<MeterChangeOut>().Create(new {
                Contract = _contract,
                ServiceStreetAddressCombined = "description 0",
                MeterChangeOutStatus = _changedStatus
            });
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/CompletedWork/Search");
                a.RequiresLoggedInUserOnly("~/CompletedWork/Index");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override because the base test will not return results for some reason.
            var entity0 = GetEntityFactory<MeterChangeOut>().Create(new
            {
                Contract = _contract,
                ServiceStreetAddressCombined = "description 0",
                MeterChangeOutStatus = _changedStatus
            });
            var entity1 = GetEntityFactory<MeterChangeOut>().Create(new
            {
                Contract = _contract,
                ServiceStreetAddressCombined = "description 1",
                MeterChangeOutStatus = _changedStatus
            });

            var search = new SearchCompletedWork();
            _target.ControllerContext = new ControllerContext();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchCompletedWork)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(entity0, resultModel[0]);
            Assert.AreSame(entity1, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<MeterChangeOut>().Create(new
            {
                Contract = _contract,
                ServiceStreetAddressCombined = "description 0",
                MeterChangeOutStatus = _changedStatus
            });
            var entity1 = GetEntityFactory<MeterChangeOut>().Create(new
            {
                Contract = _contract,
                ServiceStreetAddressCombined = "description 1",
                MeterChangeOutStatus = _changedStatus
            });

            var search = new SearchCompletedWork();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[
                    ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = _container.With(result).With(true).GetInstance<ExcelResultTester>())
            {
                helper.AreEqual(entity0.ServiceStreetAddressCombined, "ServiceStreetAddress");
                helper.AreEqual(entity1.ServiceStreetAddressCombined, "ServiceStreetAddress", 1);
            }
        }

        #endregion
    }
}