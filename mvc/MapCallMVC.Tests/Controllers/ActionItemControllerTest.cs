using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ActionItemControllerTest : MapCallMvcControllerTestBase<ActionItemController, ActionItem, IRepository<ActionItem>>
    {
        #region Fields

        private DataType _dataType;
        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            // Needs to exist for automatic tests.
            _dataType = GetFactory<DataTypeFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
            e.For<IActionItemTypeRepository>().Use<ActionItemTypeRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (NewActionItem)vm;
                model.TableName = _dataType.TableName;
            };

            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditActionItem)vm;
                model.Url = "http://www.internet!.com";
            };

            options.CreateRedirectsToReferrerOnError = true;
            options.CreateRedirectErrorUrlFragment = ActionItemController.FRAGMENT_IDENTIFIER;
            options.CreateRedirectsToReferrerOnSuccess = true;
            options.CreateRedirectSuccessUrlFragment = ActionItemController.FRAGMENT_IDENTIFIER;

            options.DestroyRedirectsToReferrerOnError = true;
            options.DestroyRedirectErrorUrlFragment = ActionItemController.FRAGMENT_IDENTIFIER;
            options.DestroyRedirectsToReferrerOnSuccess = true;
            options.DestroyRedirectSuccessUrlFragment = ActionItemController.FRAGMENT_IDENTIFIER;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string path = "~/ActionItem/";
                a.RequiresLoggedInUserOnly(path + "Create");
                a.RequiresLoggedInUserOnly(path + "New");
                a.RequiresLoggedInUserOnly(path + "Edit");
                a.RequiresLoggedInUserOnly(path + "CancelEdit");
                a.RequiresLoggedInUserOnly(path + "Update");
                a.RequiresLoggedInUserOnly(path + "Destroy");
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // Override needed due to Edit action taking a view model.

            var eq = GetEntityFactory<ActionItem>().Create();
            var vm = _viewModelFactory.Build<EditActionItem, ActionItem>(eq);
            var result = (ViewResult)_target.Edit(vm);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(eq.Id, ((EditActionItem)result.Model).Id);
        }

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // override because of lack of int param on Edit
            Assert.IsNotNull((HttpNotFoundResult)_target.Edit(new EditActionItem(_container) {
                Id = 404
            }));
        }

        [TestMethod]
        public void TestCancelEditRedirectsBackToSuppliedURL()
        {
            var url = "stuff";
            var result = (RedirectResult)_target.CancelEdit(url);

            Assert.AreEqual(url, result.Url);
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // override needed because it redirects to a supplied url.
            var eq = GetEntityFactory<ActionItem>().Create();
            var expected = "description field";
            var url = $"http://localhost:15765/Incident/Show/50";
            var result = (RedirectResult)_target.Update(
                _viewModelFactory.BuildWithOverrides<EditActionItem, ActionItem>(eq, new {
                    Note = expected,
                    Url = url
                }));

            Assert.AreEqual(url, result.Url);
            Assert.AreEqual(expected, Session.Get<ActionItem>(eq.Id).Note);
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            // override needed because it redirects to a supplied url.
            var eq = GetEntityFactory<ActionItem>().Create();
            var expected = "description field";
            var url = $"http://localhost:15765/Incident/Show/50";

            _target.ModelState.AddModelError("Error", "error!");
            var result = (RedirectResult)_target.Update(
                _viewModelFactory.BuildWithOverrides<EditActionItem, ActionItem>(eq, new {
                    Note = expected,
                    Url = url
                }));

            Assert.AreEqual(url, result.Url);
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateSavesANewRecord()
        {
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "YES" });
            var eq = GetEntityFactory<ActionItem>().Create(new { DataType = dataType });
            var expected = "These are the notes for the Action Item";
            var result = (RedirectToRouteResult)_target.Create(
                _viewModelFactory.BuildWithOverrides<NewActionItem, ActionItem>(eq, new {
                    Note = expected,
                    TableName = dataType.TableName
                }));

            Assert.AreEqual(expected, Session.Get<ActionItem>(eq.Id).Note);
        }

        [TestMethod]
        public void TestCreateSendsNotificationForNearMiss()
        {
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "NearMisses" });
            var eq = GetEntityFactory<ActionItem>().Create(new { DataType = dataType });
            var nearMiss = GetEntityFactory<NearMiss>().Create();
            var expected = "These are the notes for the Action Item";
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = (RedirectToRouteResult)_target.Create(
                _viewModelFactory.BuildWithOverrides<NewActionItem, ActionItem>(eq, new {
                    Note = expected,
                    TableName = dataType.TableName,
                    LinkedId = nearMiss.Id
                }));
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
            Assert.AreEqual("Near Miss Action Item Created", resultArgs.Purpose);
        }

        [TestMethod]
        public void TestCreateDoesNotSendNotificationForNonNearMiss()
        {
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "some table" });
            var eq = GetEntityFactory<ActionItem>().Create(new { DataType = dataType });
            var expected = "These are the notes for the Action Item";
            var result = (RedirectToRouteResult)_target.Create(
                _viewModelFactory.BuildWithOverrides<NewActionItem, ActionItem>(eq, new {
                    Note = expected,
                    TableName = dataType.TableName
                }));
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never());
        }
        
       
        [TestMethod]
        public void TestNewAddsDropDownDataForActionItemTypesByTableName()
        {
            var dataType = GetFactory<DataTypeFactory>().Create(new {TableName = "action item data table"});
            var expected = GetFactory<ActionItemTypeFactory>().CreateList(3, new {
                DataType = dataType
            });

            GetFactory<ActionItemTypeFactory>().CreateList(3);

            _target.New(new NewActionItem(_container) {
                TableName = "action item data table"
            });

            _target.AssertHasDropDownData(expected, dt => dt.Id, dt => dt.Description, "Type");
        }
        
        [TestMethod]
        public void TestNewAddsDropDownDataForActionItemTypesWithNullDataType()
        {
            var expected = GetFactory<ActionItemTypeFactory>().CreateList(2);

            GetFactory<ActionItemTypeFactory>().CreateList(2);

            _target.New(new NewActionItem(_container));

            _target.AssertHasDropDownData(expected, dt => dt.Id, dt => dt.Description, "Type");
        }

        #endregion
    }
}
