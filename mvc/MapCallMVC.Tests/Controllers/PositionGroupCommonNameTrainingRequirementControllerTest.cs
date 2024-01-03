using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using System;
using System.Web.Mvc;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PositionGroupCommonNameTrainingRequirementControllerTest : MapCallMvcControllerTestBase<PositionGroupCommonNameTrainingRequirementController, PositionGroupCommonName>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<PositionGroupCommonName>>(_container.GetInstance<RepositoryBase<PositionGroupCommonName>>());
            _container.Inject<IRepository<TrainingRequirement>>(_container.GetInstance<RepositoryBase<TrainingRequirement>>());
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override because of New parameter
            _target.ModelState.AddModelError("uh", "oh");
            var model = new CreatePositionGroupCommonNameTrainingRequirement();

            var result = _target.New(model) as ViewResult;

            Assert.AreSame(model, result.Model);
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // override because viewmodel doesn't inherit from ViewModel<TEntity> for some reason.
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();

            _target.Create(new CreatePositionGroupCommonNameTrainingRequirement
            {
                TrainingRequirement = trainingRequirement.Id,
                PositionGroupCommonName = job.Id
            });

            MyAssert.Contains(Session.Load<TrainingRequirement>(trainingRequirement.Id).PositionGroupCommonNames, job);
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // override because viewmodel doesn't inherit from ViewModel<TEntity> for some reason.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // override because viewmodel doesn't inherit from ViewModel<TEntity> for some reason.
        }

        [TestMethod]
        public void TestCreateDoesNotLinkTheChosenPositionGroupCommonNameToTheChosenTrainingRequirementIfItIsAlreadyLink()
        {
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();
            trainingRequirement.PositionGroupCommonNames.Add(job);
            Session.Save(trainingRequirement);

            trainingRequirement = Session.Load<TrainingRequirement>(trainingRequirement.Id);

            Assert.IsTrue(trainingRequirement.PositionGroupCommonNames.Contains(job));
            Assert.AreEqual(1, trainingRequirement.PositionGroupCommonNames.Count);
            
            _target.Create(new CreatePositionGroupCommonNameTrainingRequirement {
                TrainingRequirement = trainingRequirement.Id,
                PositionGroupCommonName = job.Id
            });
            
            Assert.AreEqual(1, Session.Load<TrainingRequirement>(trainingRequirement.Id).PositionGroupCommonNames.Count);
            _target.AssertTempDataContainsMessage(PositionGroupCommonNameTrainingRequirementController.ALREADY_LINKED,
                PositionGroupCommonNameTrainingRequirementController.ERROR_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestCreateRedirectsBackToTheReffererIfSet()
        {
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();
            var url = "http://somesite.net";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.Create(new CreatePositionGroupCommonNameTrainingRequirement {
                TrainingRequirement = trainingRequirement.Id,
                PositionGroupCommonName = job.Id
            }) as RedirectResult;

            Assert.AreEqual(url + PositionGroupCommonNameTrainingRequirementController.FRAGMENT_IDENTIFIER, result.Url);
        }

        [TestMethod]
        public void TestCreateRedirectsToShowTheTrainingRequirementIfReferrerNotSet()
        {
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();

            var result = _target.Create(new CreatePositionGroupCommonNameTrainingRequirement {
                TrainingRequirement = trainingRequirement.Id,
                PositionGroupCommonName = job.Id
            }) as RedirectToRouteResult;

            Assert.AreEqual("TrainingRequirement", result.RouteValues["controller"]);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(trainingRequirement.Id, result.RouteValues["id"]);
        }

        #endregion

        #region Destroy

        [TestMethod]
        public override void TestDestroyActuallyDeletesTheRecordAndOnlyTheRecord()
        {
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();
            trainingRequirement.PositionGroupCommonNames.Add(job);
            Session.Save(trainingRequirement);

            MyAssert.CausesDecrease(() => _target.Destroy(new DestroyPositionGroupCommonNameTrainingRequirement {
                PositionGroupCommonNameId = job.Id,
                TrainingRequirementId = trainingRequirement.Id
            }), () => Session.Load<TrainingRequirement>(trainingRequirement.Id).PositionGroupCommonNames.Count);
        }

        [TestMethod]
        public void TestDestroyRedirectsBackToTheRefererrerIfSet()
        {
            var url = "http://somesite.net";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();
            trainingRequirement.PositionGroupCommonNames.Add(job);
            Session.Save(trainingRequirement);

            var result = _target.Destroy(new DestroyPositionGroupCommonNameTrainingRequirement {
                TrainingRequirementId = trainingRequirement.Id,
                PositionGroupCommonNameId = job.Id
            }) as RedirectResult;

            Assert.AreEqual(url + PositionGroupCommonNameTrainingRequirementController.FRAGMENT_IDENTIFIER, result.Url);
        }

        [TestMethod]
        public void TestDestroyRedirectsToShowTheTrainingRequirementIfReferrerNotSet()
        {
            var trainingRequirement = GetFactory<TrainingRequirementFactory>().Create();
            var job = GetFactory<PositionGroupCommonNameFactory>().Create();
            trainingRequirement.PositionGroupCommonNames.Add(job);
            Session.Save(trainingRequirement);

            var result = _target.Destroy(new DestroyPositionGroupCommonNameTrainingRequirement
            {
                TrainingRequirementId = trainingRequirement.Id,
                PositionGroupCommonNameId = job.Id
            }) as RedirectToRouteResult;

            Assert.AreEqual("TrainingRequirement", result.RouteValues["controller"]);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(trainingRequirement.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            // noop override: redirects to other pages and is handled by other tests.
        }

        [TestMethod]
        public override void TestDestroyReturnsNotFoundIfRecordCanNotBeFound()
        {
            // override needed because view model doesn't inherit ViewModel<TEntity>
            MvcAssert.IsNotFound(_target.Destroy(new DestroyPositionGroupCommonNameTrainingRequirement { TrainingRequirementId = -1 }));
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.OperationsTrainingModules;
                a.RequiresRole("~/PositionGroupCommonNameTrainingRequirement/New", module, RoleActions.Add);
                a.RequiresRole("~/PositionGroupCommonNameTrainingRequirement/Create", module, RoleActions.Add);
                a.RequiresRole("~/PositionGroupCommonNameTrainingRequirement/Destroy", module, RoleActions.Delete);
            });
        }
    }
}
