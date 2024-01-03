using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Admin.Controllers;
using MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.Admin.Controllers
{
    [TestClass]
    public class NotificationConfigurationControllerTest : MapCallMvcControllerTestBase<NotificationConfigurationController, NotificationConfiguration>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var waterQualityModule = GetEntityFactory<Module>().Create(new {
                    Id = RoleModules.WaterQualityGeneral,
                    Application = GetEntityFactory<Application>().Create(new {
                        Id = RoleApplications.WaterQuality
                    })
                });

                var notificationPurpose1 = GetEntityFactory<NotificationPurpose>().Create(new {
                    Module = waterQualityModule,
                    Purpose = "This is purpose #1"
                });

                var notificationPurpose2 = GetEntityFactory<NotificationPurpose>().Create(new {
                    Module = waterQualityModule,
                    Purpose = "This is purpose #2"
                });

                return GetEntityFactory<NotificationConfiguration>().Create(new {
                    OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                    Contact = GetEntityFactory<Contact>().Create(),
                    NotificationPurposes = new List<NotificationPurpose> {
                        notificationPurpose1,
                        notificationPurpose2
                    }
                });
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Search/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Show/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Index/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/New/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Create/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Edit/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Update/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationConfiguration/Destroy/");
            });
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var result = _target.New();
            MvcAssert.IsViewNamed(result, "New");
            Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(CreateNotificationConfigurations));
        }

        #endregion

        #region Create

        private CreateNotificationConfigurations BuildCreateNotificatonConfigurationsModel()
        {
            var model = _viewModelFactory.Build<CreateNotificationConfigurations>();
            model.Contacts = new[] { GetEntityFactory<Contact>().Create().Id };
            model.AppliesToAllOperatingCenters = true;
            model.NotificationPurposes = new[] { GetEntityFactory<NotificationPurpose>().Create().Id };
            return model;
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop because this uses ViewModelSet which the base testing stuff doesn't support.
        }

        [TestMethod]
        public void TestCreateRedirectsUserToSearchPageAfterSuccessfullySaving()
        {
            var model = BuildCreateNotificatonConfigurationsModel();
           
            var result = _target.Create(model);
           
            MvcAssert.RedirectsToRoute(result, new { action = "Search" });
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            var model = BuildCreateNotificatonConfigurationsModel();
            _target.ModelState.AddModelError("UH OH", "I made an oopsy");

            var result = _target.Create(model);

            MvcAssert.IsViewWithNameAndModel(result, "New", model);
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            var model = BuildCreateNotificatonConfigurationsModel();
            MyAssert.CausesIncrease(() => _target.Create(model), () => Repository.GetAll().Count(), 1);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var notificationConfiguration = GetEntityFactory<NotificationConfiguration>().Create();
            var expectedContact = GetEntityFactory<Contact>().Create();
            var expectedOperatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var expectedNotificationPurposes = GetEntityFactory<NotificationPurpose>().CreateList(2);

            _target.Update(_viewModelFactory.BuildWithOverrides<EditNotificationConfiguration, NotificationConfiguration>(notificationConfiguration, new {
                Contact = expectedContact.Id,
                OperatingCenter = expectedOperatingCenter.Id,
                NotificationPurposes = expectedNotificationPurposes.Select(x => x.Id).ToArray()
            }));

            var updatedNotificationConfiguration = Session.Get<NotificationConfiguration>(notificationConfiguration.Id);

            Assert.AreEqual(expectedContact, updatedNotificationConfiguration.Contact);
            Assert.AreEqual(expectedOperatingCenter, updatedNotificationConfiguration.OperatingCenter);
            Assert.AreEqual(expectedNotificationPurposes.Count, updatedNotificationConfiguration.NotificationPurposes.Count);
        }

        #endregion
    }
}
