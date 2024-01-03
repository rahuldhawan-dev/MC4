using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.Metadata;
using MMSINC.Testing;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class AjaxFileUploadTest
    {
        #region Fields

        private AjaxFileUpload _target;
        private readonly Type _targetType = typeof(AjaxFileUpload);

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AjaxFileUpload();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorSetsBinaryDataToNull()
        {
            Assert.IsNull(new AjaxFileUpload().BinaryData);
        }

        [TestMethod]
        public void TestConstructorSetsFileNameToNull()
        {
            Assert.IsNull(new AjaxFileUpload().FileName);
        }

        [TestMethod]
        public void TestHasCorrectModelBinderAttribute()
        {
            var attr = _targetType.GetCustomAttributes<ModelBinderAttribute>(true).Single();
            Assert.AreSame(typeof(AjaxFileUploadModelBinder), attr.BinderType);
        }

        [TestMethod]
        public void TestHasBinaryDataReturnsFalseIfBinaryDataIsNull()
        {
            _target.BinaryData = null;
            Assert.IsFalse(_target.HasBinaryData);
        }

        [TestMethod]
        public void TestHasBinaryDataReturnsFalseIfBinaryDataIsEmpty()
        {
            _target.BinaryData = new byte[] { };
            Assert.IsFalse(_target.HasBinaryData);
        }

        [TestMethod]
        public void TestHasBinaryDataReturnsTrueIfBinaryDataIsNotNullOrEmpty()
        {
            _target.BinaryData = new byte[] {1};
            Assert.IsTrue(_target.HasBinaryData);
        }

        [TestMethod]
        public void TestValidationFailsIfDoesNotHaveBinaryDataButHasKeySet()
        {
            _target.BinaryData = null;
            _target.Key = Guid.NewGuid();
            ValidationAssert.ModelStateHasError(_target, "Key", "Invalid upload key.");
        }

        [TestMethod]
        public void TestValidationFailsIfBinaryDataIsNullOrEmptyAndKeyIsNotSet()
        {
            _target.BinaryData = null;
            _target.Key = Guid.Empty;
            ValidationAssert.ModelStateHasError(_target, "BinaryData", "An empty file was uploaded.");

            _target.BinaryData = new byte[] { };
            ValidationAssert.ModelStateHasError(_target, "BinaryData", "An empty file was uploaded.");
        }

        [TestMethod]
        public void TestValidationPassesIfBinaryDataIsNullAndDoesNotHaveKey()
        {
            _target.BinaryData = null;
            _target.Key = Guid.NewGuid();
            ValidationAssert.ModelStateHasError(_target, "Key", "Invalid upload key.");
        }

        #endregion
    }
}
