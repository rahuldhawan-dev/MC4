using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Admin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.Admin.Controllers
{
    [TestClass]
    public class NotificationPurposeControllerTest : MapCallMvcControllerTestBase<NotificationPurposeController, NotificationPurpose, IRepository<NotificationPurpose>>
    {
        #region Private Fields

        private Module _waterQualityModule;
        private Application _productionApplication;
        private NotificationPurpose _productionNotificationPurpose1;
        private NotificationPurpose _productionNotificationPurpose2;
        private NotificationPurpose _waterQualityNotificationPurpose;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {

            _productionApplication = GetEntityFactory<Application>().Create(new {
                Id = RoleApplications.Production
            });

            _waterQualityModule = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.WaterQualityGeneral,
                Application = GetEntityFactory<Application>().Create(new {
                    Id = RoleApplications.WaterQuality
                })
            });

            _waterQualityNotificationPurpose = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = _waterQualityModule
            });

            _productionNotificationPurpose1 = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = GetEntityFactory<Module>().Create(new {
                    Id = RoleModules.ProductionFacilities,
                    Application = _productionApplication
                })
            });

            _productionNotificationPurpose2 = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = GetEntityFactory<Module>().Create(new {
                    Id = RoleModules.ProductionEquipment,
                    Application = _productionApplication
                })
            });
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Admin/NotificationPurpose/ByApplication/");
                a.RequiresLoggedInUserOnly("~/Admin/NotificationPurpose/ByModule/");
            });
        }

        #endregion

        #region ByApplication

        [TestMethod]
        public void TestByApplicationReturnsNotificationPurposeRecordsFilteredByApplication()
        {
            var actionResult = (CascadingActionResult)_target.ByApplication(_productionApplication.Id);
            var notificationPurposes = ((IEnumerable<NotificationPurposeDisplayItem>)actionResult.Data).ToList();

            Assert.IsTrue(notificationPurposes[0].Id == _productionNotificationPurpose1.Id);
            Assert.IsTrue(notificationPurposes[1].Id == _productionNotificationPurpose2.Id);
            Assert.IsTrue(notificationPurposes.Count == 2);
        }

        #endregion

        #region ByModule
        
        [TestMethod]
        public void TestByModuleReturnsNotificationPurposeRecordsFilteredByModules()
        {
            var anotherModule = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.ProductionFacilities,
                Application = _productionApplication
            });

            var actionResult = (CascadingActionResult)_target.ByModule(new[] { _waterQualityModule.Id, anotherModule.Id });
            var notificationPurposes = ((IEnumerable<NotificationPurposeDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(notificationPurposes.Count, 2);
            Assert.IsTrue(notificationPurposes.Any(x => x.Id == _waterQualityNotificationPurpose.Id));
            Assert.IsTrue(notificationPurposes.Any(x => x.Id == _productionNotificationPurpose1.Id));
        }

        #endregion
    }
}
