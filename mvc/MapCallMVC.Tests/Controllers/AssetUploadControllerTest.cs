using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.AssetUploads;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Testing.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class AssetUploadControllerTest : MapCallMvcControllerTestBase<AssetUploadController, AssetUpload>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAssetUploadFileService>().Use(new Mock<IAssetUploadFileService>().Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateAssetUpload)vm;
                model.FileUpload = new MMSINC.Metadata.AjaxFileUpload {
                    BinaryData = TestFiles.GetExcel2007File(),
                    FileName = "some.file"
                };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/AssetUpload/New");
                a.RequiresSiteAdminUser("~/AssetUpload/Create");
                a.RequiresSiteAdminUser("~/AssetUpload/Show");
                a.RequiresSiteAdminUser("~/AssetUpload/Search");
                a.RequiresSiteAdminUser("~/AssetUpload/Index");
            });
        }
    }
}