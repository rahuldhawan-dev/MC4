using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class NoteControllerTest : MapCallMvcControllerTestBase<NoteController, Note>
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
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _authenticationService.SetupGet(s => s.CurrentUser.UserName).Returns(_currentUserName);
            _dateTimeProvider.Setup(p => p.GetCurrentDate()).Returns(DateTime.Now);
            
            // Needs to exist for Create tests
            _dataType = GetFactory<DataTypeFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
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
        public void TestCreateThrowsModelValidationExceptionIfModelIsNotValid()
        {
            var dataType = GetFactory<DataTypeFactory>().Create();
            _target.ModelState.AddModelError("foo", new Exception());

            MyAssert.Throws<ModelValidationException>(
                () => _target.Create(new NewNote(_container) {
                    TableName = dataType.TableName,
                    LinkedId = 666
                }));
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Doesn't return validation results to client.");
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // noop: Throws exception. Tested below.
        }

        [TestMethod]
        public void TestUpdateThrowsExceptionIfModelStateIsNotValid()
        {
            var note = GetFactory<NoteFactory>().Create();
            _target.ModelState.AddModelError("foo", new Exception());

            MyAssert.Throws<ModelValidationException>(
                () => _target.Update(_viewModelFactory.Build<DeleteNote, Note>(note)));
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var note = GetFactory<NoteFactory>().Create();
            var newValue = "asdfasdf";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<DeleteNote, Note>(note, new {
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
