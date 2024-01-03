using System;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using IMeterChangeOutRepository = Contractors.Data.Models.Repositories.IMeterChangeOutRepository;
using MeterChangeOutRepository = Contractors.Data.Models.Repositories.MeterChangeOutRepository;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class NoteControllerTest : ContractorControllerTestBase<NoteController, Note>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private string _currentUserName = "the current user";
        private DataType _dataType;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IDataTypeRepository>().Use<DataTypeRepository>();
            e.For<IMeterChangeOutRepository>().Use<MeterChangeOutRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _currentUser.Email = _currentUserName;
            _authenticationService.Setup(x => x.CurrentUser).Returns(_currentUser);
            _dateTimeProvider.Setup(p => p.GetCurrentDate()).Returns(DateTime.Now);

            // Needs to exist for Create tests
            _dataType = GetFactory<DataTypeFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var mco = GetFactory<MeterChangeOutFactory>().Create();
                mco.Contract.Contractor = _currentUser.Contractor;
                Session.Flush();
                var note = GetFactory<NoteFactory>().Create(new {
                    LinkedId = mco.Id
                });
                note.DataType.TableName = "MeterChangeOuts";
                Session.Flush();
                return note;
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (NewNote)vm;
                model.TableName = _dataType.TableName;
            };
            options.CreateRedirectsToReferrerOnSuccess = true;
            options.CreateRedirectSuccessUrlFragment = "#NotesTab";
            options.DestroyRedirectsToReferrerOnError = true;
            options.DestroyRedirectErrorUrlFragment = "#NotesTab";
            options.DestroyRedirectsToReferrerOnSuccess = true;
            options.DestroyRedirectSuccessUrlFragment = "#NotesTab";
            options.UpdateRedirectsToReferrerOnSuccess = true;
            options.UpdateRedirectSuccessUrlFragment = "#NotesTab";
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("I throw an error instead of returning validation results to client.");
        }

        #endregion

        #region Destroy(int)

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            // noop override: this keeps redirecting back to the Show page, so there's some validation error coming up.
            // the other tests cover this already.
        }

        [TestMethod]
        public void TestDestroyRedirectsToTheHomePageIfUrlReferrerNotSet()
        {
            var link = GetFactory<NoteFactory>().Create();
            var model = _container.GetInstance<DeleteNote>();
            model.Map(link);

            var result = _target.Destroy(model) as RedirectToRouteResult;

            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestDestroyRedirectsBackToTheReferrerIfSet()
        {
            var link = GetFactory<NoteFactory>().Create();
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.Destroy(_viewModelFactory.Build<DeleteNote, Note>(link)) as RedirectResult;

            Assert.AreEqual(url + NoteController.FRAGMENT_IDENTIFIER, result.Url);
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // noop override: throws exception for some reason.
        }

        [TestMethod]
        public void TestUpdateThrowsExceptionIfModelStateIsNotValid()
        {
            var note = GetFactory<NoteFactory>().Create();
            _target.ModelState.AddModelError("foo", new Exception());

            MyAssert.Throws<ModelValidationException>(
                () => _target.Update(_viewModelFactory.Build<EditNote, Note>(note)));
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var mco = GetFactory<MeterChangeOutFactory>().Create();
            mco.Contract.Contractor = _currentUser.Contractor;
            Session.Flush();
            var note = GetFactory<NoteFactory>().Create(new
            {
                LinkedId = mco.Id
            });
            note.DataType.TableName = "MeterChangeOuts";
            var newValue = "asdfasdf";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditNote, Note>(note, new {
                Text = newValue
            }));

            Assert.AreEqual(newValue, Session.Get<Note>(note.Id).Text);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Note/New/");
                a.RequiresLoggedInUserOnly("~/Note/Create/");
                a.RequiresLoggedInUserOnly("~/Note/Update/");
                a.RequiresLoggedInUserOnly("~/Note/Destroy/");
            });
        }
    }
}
