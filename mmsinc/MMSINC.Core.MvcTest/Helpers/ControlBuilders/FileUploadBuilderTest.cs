using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class FileUploadBuilderTest
    {
        #region Fields

        private FileUploadBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new FileUploadBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringRendersExpectedOutput()
        {
            var expected =
                "<input id=\"FileUpload_Key\" name=\"FileUpload.Key\" type=\"hidden\" value=\"\" />" +
                "<div class=\"file-upload\" data-file-inputname=\"FileUpload\" data-file-keyelement=\"FileUpload.Key\" data-file-url=\"/File/Create\" id=\"FileUpload\"></div>";

            _target.Name = "FileUpload";
            _target.Id = "FileUpload";
            _target.Url = "/File/Create";

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToHtmlStringCompletelyIgnoresWhateverYouSetAsNameWhenSettingTheDataFileInputNameAttribute()
        {
            // All file uploads go through the FileController.Create action that expects fields to be
            // named "FileUpload" without any nesting. This is due to the model used for that Create
            // action.

            var expected =
                "<input id=\"FileUpload_Key\" name=\"Something.Nested.Maybe.Key\" type=\"hidden\" value=\"\" />" +
                "<div class=\"file-upload\" data-file-inputname=\"FileUpload\" data-file-keyelement=\"Something.Nested.Maybe.Key\" data-file-url=\"/File/Create\" id=\"FileUpload\"></div>";

            _target.Name = "Something.Nested.Maybe";
            _target.Id = "FileUpload";
            _target.Url = "/File/Create";

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFileUploadIncludesAllowedExtensionsIfAny()
        {
            var expected =
                "<input id=\"FileUpload_Key\" name=\"FileUpload.Key\" type=\"hidden\" value=\"\" />" +
                "<div class=\"file-upload\" data-file-ext=\"bmp,jpeg,jpg,png,tif,tiff\" data-file-inputname=\"FileUpload\" data-file-keyelement=\"FileUpload.Key\" data-file-url=\"/File/Create\" id=\"FileUpload\"></div>";

            _target.Name = "FileUpload";
            _target.Id = "FileUpload";
            _target.Url = "/File/Create";

            _target.AllowedExtensions.Add("bmp");
            _target.AllowedExtensions.Add("jpeg");
            _target.AllowedExtensions.Add("png");
            _target.AllowedExtensions.Add("tiff");

            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestFileUploadIncludesOnCompleteIfSetInOptions()
        {
            var expected =
                "<input id=\"FileUpload_Key\" name=\"FileUpload.Key\" type=\"hidden\" value=\"\" />" +
                "<div class=\"file-upload\" data-file-inputname=\"FileUpload\" data-file-keyelement=\"FileUpload.Key\" data-file-oncomplete=\"Some.OnComplete\" data-file-url=\"/File/Create\" id=\"FileUpload\"></div>";

            _target.Name = "FileUpload";
            _target.Id = "FileUpload";
            _target.Url = "/File/Create";
            _target.OnComplete = "Some.OnComplete";

            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestFileUploadIncludesButtonTextIfSetInOptions()
        {
            var expected =
                "<input id=\"FileUpload_Key\" name=\"FileUpload.Key\" type=\"hidden\" value=\"\" />" +
                "<div class=\"file-upload\" data-file-buttontext=\"Click Me\" data-file-inputname=\"FileUpload\" data-file-keyelement=\"FileUpload.Key\" data-file-url=\"/File/Create\" id=\"FileUpload\"></div>";

            _target.Name = "FileUpload";
            _target.Id = "FileUpload";
            _target.Url = "/File/Create";
            _target.ButtonText = "Click Me";

            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestWithAddsAnonymousObjectAsHtmlAttributes()
        {
            var expected = new {
                mrs_dash = "yeah"
            };

            _target.With(expected);
            Assert.AreSame(expected.mrs_dash, _target.HtmlAttributes["mrs-dash"]);
        }

        [TestMethod]
        public void TestWithAddsDictionaryAsHtmlAttributes()
        {
            var expected = new Dictionary<string, object>();
            expected["something"] = "yay";

            _target.With(expected);
            Assert.AreSame("yay", _target.HtmlAttributes["something"]);
        }

        [TestMethod]
        public void TestWithAdds()
        {
            _target.With("neat", "cool");
            Assert.AreEqual("cool", _target.HtmlAttributes["neat"]);
        }

        [TestMethod]
        public void TestWithButtonTextSetsButtonTextPropertyAndReturnsSelf()
        {
            var result = _target.WithButtonText("Barf");
            Assert.AreSame(result, _target);
            Assert.AreSame("Barf", _target.ButtonText);
        }

        [TestMethod]
        public void TestWithOnCompleteSetsOnCompletePropertyAndReturnsSelf()
        {
            var result = _target.WithOnComplete("Barf");
            Assert.AreSame(result, _target);
            Assert.AreSame("Barf", _target.OnComplete);
        }

        [TestMethod]
        public void TestWithUrlSetsUrlPropertyAndReturnsSelf()
        {
            var result = _target.WithUrl("Barf");
            Assert.AreSame(result, _target);
            Assert.AreSame("Barf", _target.Url);
        }

        [TestMethod]
        public void TestWithIdSetsIdAndReturnsSelf()
        {
            var result = _target.WithId("Id");
            Assert.AreEqual("Id", _target.Id);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithNameSetsNameAndReturnsSelf()
        {
            var result = _target.WithName("Name");
            Assert.AreEqual("Name", _target.Name);
            Assert.AreSame(_target, result);
        }

        #endregion
    }
}
