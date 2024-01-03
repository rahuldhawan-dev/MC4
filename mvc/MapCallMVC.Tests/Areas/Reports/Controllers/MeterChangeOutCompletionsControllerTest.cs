using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class MeterChangeOutCompletionsControllerTest : MapCallMvcControllerTestBase<MeterChangeOutCompletionsController, MeterChangeOut>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesMeterChangeOuts;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/MeterChangeOutCompletions/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Reports/MeterChangeOutCompletions/Index/", module, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var now = DateTime.Now;
            var statuses = GetEntityFactory<MeterChangeOutStatus>().CreateList(10);
            var crew = GetEntityFactory<ContractorMeterCrew>().Create();
            var changeouts = GetEntityFactory<MeterChangeOut>().CreateList(3, new {
                MeterChangeOutStatus = statuses[MeterChangeOutStatus.Indices.CHANGED-1],
                DateStatusChanged = now,
                CalledInByContractorMeterCrew = crew });
            var search = new SearchMeterChangeOutCompletions();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchMeterChangeOutCompletions)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);

            Assert.AreEqual(crew, resultModel[0].ContractorMeterCrew);
            Assert.AreEqual(3, resultModel[0].Changed);
            MyAssert.AreClose(now.BeginningOfDay(), resultModel[0].CompletionDate);
        }
        
        [TestMethod]
        public void TestSearchReturnsSearchViewWithDefaultSearchValues()
        {
            var result = _target.Search() as ViewResult;
            MvcAssert.IsViewNamed(result, "Search");
            Assert.IsInstanceOfType(result.Model, typeof(SearchMeterChangeOutCompletions));
            var now = DateTime.Now;
            var search = (SearchMeterChangeOutCompletions)result.Model;
            Assert.AreEqual(now.AddDays(-(int)now.DayOfWeek).BeginningOfDay(), search.DateStatusChanged.Start);
            Assert.AreEqual(now.AddDays(7 - (int)now.DayOfWeek).EndOfDay(), search.DateStatusChanged.End);
            Assert.AreEqual(RangeOperator.Between, search.DateStatusChanged.Operator);
            Assert.AreEqual(MeterChangeOutStatus.Indices.CHANGED , search.MeterChangeOutStatus[0]);
        }
    }
}