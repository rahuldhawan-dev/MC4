using System.Web.Mvc;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.MvcTest.Controllers
{
    [TestClass]
    public class FileControllerTest : ControllerTestBase<FakeMvcApplication, FileController, User>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
        }

        [TestInitialize]
        public void FileControllerTestInitialize()
        {
            _target = _container.GetInstance<FileController>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // noop.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop override: returns json. tested below
        }

        [TestMethod]
        public void TestCreateReturnsSuccessFalseIfModelStateIsNotValid()
        {
            _target.ModelState.AddModelError("err", "err");
            var result = (JsonResult)_target.Create(null);
            dynamic model = result.Data;
            Assert.IsFalse(model.success);
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop override: returns json result, tested below.
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop: returns json result, tested below.
        }

        [TestMethod]
        public void TestCreateReturnsSuccessTrueWithKeyAndFileNameIfModelStateIsValid()
        {
            var expected = new FileController.UploadModel();
            expected.FileUpload = new AjaxFileUpload {
                FileName = "somefile.txt",
            };

            var result = (JsonResult)_target.Create(expected);
            dynamic model = result.Data;
            Assert.IsTrue(model.success);
            Assert.AreEqual(expected.FileUpload.FileName, model.fileName);
            Assert.AreEqual(expected.FileUpload.Key, model.key);
        }

        [TestMethod]
        public void TestCreateStoresUploadInFileService()
        {
            var expected = new FileController.UploadModel();
            expected.FileUpload = new AjaxFileUpload {
                FileName = "somefile.txt",
            };

            _target.Create(expected);

            var fileServ = _container.With(_target.TempData).GetInstance<FileUploadService>();
            Assert.AreSame(expected.FileUpload, fileServ.GetFile(expected.FileUpload.Key));
        }

        [TestMethod]
        public void TestCreateDoesNotRequireSecureForm()
        {
            Application.ControllerFactory.RegisterController(_container.GetInstance<FileController>());
            DoesNotRequireSecureForm("~/File/Create/");
        }

        #endregion
    }
}
