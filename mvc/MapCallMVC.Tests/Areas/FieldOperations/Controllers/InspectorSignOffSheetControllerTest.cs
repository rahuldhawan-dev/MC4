using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class InspectorSignOffSheetControllerTest : MapCallMvcControllerTestBase<InspectorSignOffSheetController, Service, ServiceRepository>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Fields

        private User _user;
        private Mock<INotificationService> _noteServ;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return _user = GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _noteServ = e.For<INotificationService>().Mock();
            e.For<IImageToPdfConverter>().Mock();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/InspectorSignOffSheet/Show/", ROLE);
            });
        }
    }
}