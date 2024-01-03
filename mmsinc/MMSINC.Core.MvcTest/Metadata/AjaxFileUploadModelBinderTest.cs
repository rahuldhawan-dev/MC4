using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class AjaxFileUploadModelBinderTest
    {
        #region Fields

        private string _expectedModelName = "APropertyOnAThing";
        private string _expectedFileName = "SomeFile.txt";

        private AjaxFileUploadModelBinder _target;
        private TestController _controller;
        private ControllerContext _controllerContext;
        private ModelBindingContext _bindingContext;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _request;
        private Mock<IValueProvider> _valueProvider;
        private byte[] _expectedBinaryData;
        private string _encodedExpectedBinaryData;
        private MemoryStream _expectedStream;
        private Mock<HttpPostedFileBase> _postedFile;
        private Mock<HttpFileCollectionBase> _requestFiles;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(e => e.For<IDateTimeProvider>().Use(new Mock<IDateTimeProvider>().Object));
            _expectedBinaryData = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 255};
            _encodedExpectedBinaryData = Convert.ToBase64String(_expectedBinaryData);
            _expectedStream = new MemoryStream(_expectedBinaryData);
            _valueProvider = new Mock<IValueProvider>();
            _httpContext = new Mock<HttpContextBase>();
            _request = new Mock<HttpRequestBase>();
            _httpContext.Setup(x => x.Request).Returns(_request.Object);
            _postedFile = new Mock<HttpPostedFileBase>();
            _postedFile.Setup(x => x.InputStream).Returns(_expectedStream);
            _requestFiles = new Mock<HttpFileCollectionBase>();
            _request.Setup(x => x.Files).Returns(_requestFiles.Object);

            _controller = _container.GetInstance<TestController>();
            _controllerContext = new ControllerContext {HttpContext = _httpContext.Object};
            _controller.ControllerContext = _controllerContext;
            _controllerContext.Controller = _controller;
            _bindingContext = new ModelBindingContext();
            _bindingContext.ValueProvider = _valueProvider.Object;
            _bindingContext.ModelName = _expectedModelName;
            _target = new AjaxFileUploadModelBinder();
        }

        private void InitForOctetStreamTesting()
        {
            _request.Setup(x => x.ContentType).Returns(AjaxFileUploadModelBinder.ContentTypes.APPLICATION_OCTET_STREAM);
            _request.Setup(x => x.InputStream).Returns(_expectedStream);
            ResetStream();
        }

        private void InitForMultipartFormData()
        {
            _request.Setup(x => x.ContentType).Returns(AjaxFileUploadModelBinder.ContentTypes.MULTIPART_FORMDATA);
            _requestFiles.Setup(x => x[_expectedModelName]).Returns(_postedFile.Object);

            var vpr = new ValueProviderResult(_postedFile.Object, null, CultureInfo.CurrentUICulture);
            _valueProvider.Setup(x => x.GetValue(_expectedModelName)).Returns(vpr);
            ResetStream();
        }

        private void InitForTempData(AjaxFileUpload tempDataModel)
        {
            _valueProvider.Setup(x => x.GetValue(_expectedModelName + ".Key"))
                          .Returns(new ValueProviderResult(tempDataModel.Key, tempDataModel.Key.ToString(),
                               CultureInfo.CurrentUICulture));

            var uploadItem = new FileUploadServiceItem {
                FileUpload = tempDataModel,
            };
            _controller.TempData[tempDataModel.Key.ToString()] = uploadItem;
        }

        private void InitForApplicationJson()
        {
            var binaryKey = _expectedModelName + "." + AjaxFileUploadModelBinder.BINARY_DATA_KEY;
            var fileNameKey = _expectedModelName + "." + AjaxFileUploadModelBinder.FILE_NAME_KEY;

            _request.Setup(x => x.ContentType).Returns("It doesn't actually matter what this is.");

            _valueProvider.Setup(x => x.GetValue(binaryKey))
                          .Returns(new ValueProviderResult(_encodedExpectedBinaryData, _encodedExpectedBinaryData,
                               CultureInfo.CurrentUICulture));

            _valueProvider.Setup(x => x.GetValue(fileNameKey))
                          .Returns(new ValueProviderResult(_expectedFileName, _encodedExpectedBinaryData,
                               CultureInfo.CurrentUICulture));
        }

        private void ResetStream()
        {
            _expectedStream.Seek(0, SeekOrigin.Begin);
        }

        private AjaxFileUpload Bind()
        {
            return (AjaxFileUpload)_target.BindModel(_controllerContext, _bindingContext);
        }

        #endregion

        #region Tests

        #region Binding with application/octet-stream

        [TestMethod]
        public void TestBindModelBindsForApplicationOctetStream()
        {
            InitForOctetStreamTesting();
            var result = Bind();
            Assert.IsNotNull(result.BinaryData);
            Assert.IsTrue(result.BinaryData.ByteArrayEquals(_expectedBinaryData));
        }

        [TestMethod]
        public void TestBindModelBindsFileNameFromValueProviderForOctetStreams()
        {
            InitForOctetStreamTesting();
            var expectedValueProviderResult =
                new ValueProviderResult(_expectedFileName, _expectedFileName, CultureInfo.CurrentUICulture);
            _valueProvider.Setup(x => x.GetValue("FileName")).Returns(expectedValueProviderResult);

            var result = Bind();
            Assert.AreEqual(_expectedFileName, result.FileName);
        }

        [TestMethod]
        public void TestBindModelWithOctetStreamSetsFileNameToNullWhenValueProviderDoesNotProvideValue()
        {
            InitForOctetStreamTesting();

            var expectedValueProviderResult = new ValueProviderResult(null, null, CultureInfo.CurrentUICulture);
            _valueProvider.Setup(x => x.GetValue("FileName")).Returns(expectedValueProviderResult).Verifiable();
            Assert.IsNull(Bind().FileName);

            ResetStream();
            _valueProvider.Setup(x => x.GetValue("FileName")).Returns((ValueProviderResult)null).Verifiable();
            Assert.IsNull(Bind().FileName);

            _valueProvider.VerifyAll();
        }

        #endregion

        #region Binding with multipart/form-data

        [TestMethod]
        public void TestBindModelBindsBinaryDataWithMultipartFormData()
        {
            InitForMultipartFormData();
            var result = Bind();
            Assert.IsNotNull(result.BinaryData);
            Assert.IsTrue(result.BinaryData.ByteArrayEquals(_expectedBinaryData));
        }

        [TestMethod]
        public void TestBindModelSetsFileNameFromHttpPostedFileForMultipartFormData()
        {
            InitForMultipartFormData();
            _postedFile.Setup(x => x.FileName).Returns(_expectedFileName);

            var result = Bind();
            Assert.AreEqual(_expectedFileName, result.FileName);
        }

        [TestMethod]
        public void TestBindModelSetsFileNameToNullIfHttpPostedFileHasNullOrEmptyFileName()
        {
            var baddies = new[] {string.Empty, null, "   "};
            foreach (var bad in baddies)
            {
                InitForMultipartFormData();
                _postedFile.Setup(x => x.FileName).Returns(bad);
                Assert.IsNull(Bind().FileName);
            }
        }

        #endregion

        #region Binding from application/json or otherwise any other content type

        [TestMethod]
        public void TestBindModelBindsWithApplicationJsonAndActualByteArrays()
        {
            InitForApplicationJson();
            var binaryKey = _expectedModelName + "." + AjaxFileUploadModelBinder.BINARY_DATA_KEY;

            _valueProvider.Setup(x => x.GetValue(binaryKey))
                          .Returns(new ValueProviderResult(_expectedBinaryData,
                               "some string? doesn't matter as far as I know.", CultureInfo.CurrentUICulture));

            var result = Bind();
            Assert.IsTrue(_expectedBinaryData.ByteArrayEquals(result.BinaryData));
        }

        [TestMethod]
        public void TestBindModelBindsWithApplicationJsonAndBase64EncodedBinaryData()
        {
            InitForApplicationJson();
            var result = Bind();
            Assert.IsTrue(_expectedBinaryData.ByteArrayEquals(result.BinaryData));
        }

        [TestMethod]
        public void TestBindModelBindsWithApplicationJsonAndSetsFileName()
        {
            InitForApplicationJson();
            Assert.AreEqual(_expectedFileName, Bind().FileName);
        }

        #endregion

        [TestMethod]
        public void TestBindModelRemovesDirectoryPathFromFileName()
        {
            InitForOctetStreamTesting();
            var fullPath = @"C:\Something\SomethingSomething\" + _expectedFileName;
            var expectedValueProviderResult = new ValueProviderResult(fullPath, fullPath, CultureInfo.CurrentUICulture);
            _valueProvider.Setup(x => x.GetValue("FileName")).Returns(expectedValueProviderResult);

            var result = Bind();
            Assert.AreEqual(_expectedFileName, result.FileName);
        }

        [TestMethod]
        public void TestBindModelBindsFromTempDataWhenKeyIsPresent()
        {
            var tempDataModel = new AjaxFileUpload {
                BinaryData = new byte[] {3},
                Key = Guid.NewGuid()
            };
            InitForTempData(tempDataModel);
            var result = Bind();

            Assert.AreNotSame(result, tempDataModel);
            Assert.AreEqual(tempDataModel.Key, result.Key);
            Assert.AreNotSame(tempDataModel.BinaryData, result.BinaryData);
            MyAssert.AreEqual(tempDataModel.BinaryData, result.BinaryData);
        }

        [TestMethod]
        public void TestBindModelReturnsNullIfNoUploadInformationIsFound()
        {
            Assert.IsNull(Bind());
        }

        #endregion

        #region Helper classes

        private class TestController : MMSINC.Controllers.ControllerBase
        {
            public TestController(ControllerBaseArguments args) : base(args) { }
        }

        #endregion
    }
}
