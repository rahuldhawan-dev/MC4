using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using System;
using System.Web.Mvc;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class InterconnectionMeterControllerTest : MapCallMvcControllerTestBase<InterconnectionMeterController, Interconnection>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<Interconnection>>(
                _container.GetInstance<RepositoryBase<Interconnection>>());
            _container.Inject<IRepository<Meter>>(
                _container.GetInstance<RepositoryBase<Meter>>());
            _container.Inject<IRepository<MeterProfile>>(
                _container.GetInstance<RepositoryBase<MeterProfile>>());
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to non-viewmodel parameter
            // modelstate should be cleared.
            _target.ModelState.AddModelError("oh", "no");
            var model = new CreateInterconnectionMeter();

            var result = _target.New(model);

            MvcAssert.IsViewWithNameAndModel(result, "New", model);
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop: Doesn't use ViewModel, also doesn't create a db record in a way that can be
            // automatically tested.
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop: Doesn't use ViewModel, also doesn't create a db record in a way that can be
            // automatically tested.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop: Doesn't use ViewModel, also doesn't create a db record in a way that can be
            // automatically tested.
        }

        [TestMethod]
        public void TestCreateLinksTheChosenInterconnectionToTheChosenMeter()
        {
            var interconnection = GetFactory<InterconnectionFactory>().Create();
            var meter = GetFactory<MeterFactory>().Create();

            _target.Create(new CreateInterconnectionMeter() {
                Interconnection = interconnection.Id,
                Meter = meter.Id
            });

            MyAssert.Contains(Session.Load<Interconnection>(interconnection.Id).Meters, meter);
        }

        [TestMethod]
        public void TestCreateRedirectsBackToTheReferrerIfSet()
        {
            var interconnection = GetFactory<InterconnectionFactory>().Create();
            var meter = GetFactory<MeterFactory>().Create();
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.Create(new CreateInterconnectionMeter()
            {
                Interconnection = interconnection.Id,
                Meter = meter.Id
            }) as RedirectResult;

            Assert.AreEqual(url + InterconnectionMeterController.FRAGMENT_IDENTIFIER, result.Url);
        }

        [TestMethod]
        public void TestCreateRedirectsToShowTheInterconnectionIfReferrerNotSet()
        {
            var interconnection = GetFactory<InterconnectionFactory>().Create();
            var meter = GetFactory<MeterFactory>().Create();

            var result = _target.Create(new CreateInterconnectionMeter()
            {
                Interconnection = interconnection.Id,
                Meter = meter.Id
            }) as RedirectToRouteResult;

            Assert.AreEqual("Interconnection", result.RouteValues["controller"]);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(interconnection.Id, result.RouteValues["id"]);
        }

        #endregion

        #region Destroy

        [TestMethod]
        public override void TestDestroyActuallyDeletesTheRecordAndOnlyTheRecord()
        {
            // noop: doesn't really delete so much as remove a reference to itself. Tested below.
            // also the model doesn't use ViewModel or an int param.
        }

        [TestMethod]
        public void TestDestoryRemovesTheMeterFromTheInterconnection()
        {
            var interconnection = GetFactory<InterconnectionFactory>().Create();
            var meter = GetFactory<MeterFactory>().Create();
            interconnection.Meters.Add(meter);
            Session.Save(interconnection);

            MyAssert.CausesDecrease(() => _target.Destroy(new DestroyInterconnectionMeter
            {
                InterconnectionId = interconnection.Id,
                MeterId = meter.Id
            }), () => Session.Load<Interconnection>(interconnection.Id).Meters.Count);
        }

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            // noop overide: tested below. Model doesn't have Id prop so this test can't run.
        }

        [TestMethod]
        public void TestDestroyRedirectsBackToTheReferrerIfSet()
        {
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));
            var interconnection = GetFactory<InterconnectionFactory>().Create();
            var meter = GetFactory<MeterFactory>().Create();
            interconnection.Meters.Add(meter);
            Session.Save(interconnection);

            var result = _target.Destroy(new DestroyInterconnectionMeter
            {
                InterconnectionId = interconnection.Id,
                MeterId = meter.Id
            }) as RedirectResult;

            Assert.AreEqual(url + InterconnectionMeterController.FRAGMENT_IDENTIFIER, result.Url);
        }

        [TestMethod]
        public void TestDestroyRedirectsToShowTheInterconnectionIfReferrerNotSet()
        {
            var interconnection = GetFactory<InterconnectionFactory>().Create();
            var meter = GetFactory<MeterFactory>().Create();
            interconnection.Meters.Add(meter);
            Session.Save(interconnection);

            var result = _target.Destroy(new DestroyInterconnectionMeter
            {
                InterconnectionId = interconnection.Id,
                MeterId = meter.Id
            }) as RedirectToRouteResult;

            Assert.AreEqual("Interconnection", result.RouteValues["controller"]);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(interconnection.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test me. Model doesn't have Id prop.");
        }

        [TestMethod]
        public override void TestDestroyReturnsNotFoundIfRecordCanNotBeFound()
        {
            // override needed because viewmodel is not ViewModel<TEntity>
            MvcAssert.IsNotFound(_target.Destroy(new DestroyInterconnectionMeter { InterconnectionId = -1 }));
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.ProductionFacilities;
                a.RequiresRole("~/InterconnectionMeter/New", module, RoleActions.Add);
                a.RequiresRole("~/InterconnectionMeter/Create", module, RoleActions.Add);
                a.RequiresRole("~/InterconnectionMeter/Destroy", module, RoleActions.Delete);
            });
        }

        #endregion
    }
}
