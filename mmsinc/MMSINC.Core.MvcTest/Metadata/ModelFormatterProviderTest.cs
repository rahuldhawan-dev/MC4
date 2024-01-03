using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class ModelFormatterProviderTest
    {
        #region Fields

        private FakeMvcApplicationTester _app;
        private ModelFormatterProvider _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _app = new FakeMvcApplicationTester(new Container());
            _target = new ModelFormatterProvider();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRegisterFormatterThrowsArgumentNullExceptionIfTypeIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.RegisterFormatter(null, new TestFormatAttribute()));
            MyAssert.Throws<ArgumentNullException>(() =>
                _target.RegisterEditorFormatter(null, new TestFormatAttribute()));
        }

        [TestMethod]
        public void TestRegisterFormattedThrowsArgumentNUllExceptionIfFormatterIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.RegisterFormatter(typeof(object), null));
            MyAssert.Throws<ArgumentNullException>(() => _target.RegisterEditorFormatter(typeof(object), null));
        }

        [TestMethod]
        public void TestTryGetModelFormatterThrowsExceptionIfMetadataIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.TryGetModelFormatter(null, FormatMode.Display));
            MyAssert.Throws<ArgumentNullException>(() => _target.TryGetModelFormatter(null, FormatMode.Editor));
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsAttributeFromMetadataIfSet()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var attr = new TestFormatAttribute();
            attr.Process(metadata);
            var result = _target.TryGetModelFormatter(metadata, FormatMode.Display);
            Assert.AreSame(attr, result);

            result = _target.TryGetModelFormatter(metadata, FormatMode.Editor);
            Assert.AreSame(attr, result);
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsRegisteredFormatterForType()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var attr = new TestFormatAttribute();
            _target.RegisterFormatter(typeof(object), attr);
            var result = _target.TryGetModelFormatter(metadata, FormatMode.Display);
            Assert.AreSame(attr, result);
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsRegisteredEditorFormatterForType()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var attr = new TestFormatAttribute();
            _target.RegisterEditorFormatter(typeof(object), attr);
            var result = _target.TryGetModelFormatter(metadata, FormatMode.Editor);
            Assert.AreSame(attr, result);
        }

        [TestMethod]
        public void
            TestTryGetModelFormatterReturnsRegisteredDefaultFormatterForTypeForEditorModeIfNoEditorFormatterIsRegistered()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var displayAttr = new TestFormatAttribute();
            _target.RegisterFormatter(typeof(object), displayAttr);

            var result = _target.TryGetModelFormatter(metadata, FormatMode.Editor);
            Assert.AreSame(displayAttr, result);
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsNullForEditorModeIfNoRegisteredDefaultOrEditorFormatterExists()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var result = _target.TryGetModelFormatter(metadata, FormatMode.Editor);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsMetadataAttributeEvenIfADefaultInstanceIsRegistered()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var notExpected = new TestFormatAttribute();
            var expected = new TestFormatAttribute();
            expected.Process(metadata);
            _target.RegisterFormatter(typeof(object), notExpected);
            var result = _target.TryGetModelFormatter(metadata, FormatMode.Display);
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsNullIfNoFormatterIsFound()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            Assert.IsNull(_target.TryGetModelFormatter(metadata, FormatMode.Display));
        }

        [TestMethod]
        public void TestTryGetModelFormatterReturnsTheLastRegisteredFormatterForAType()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(object));
            var notExpected = new TestFormatAttribute();
            var expected = new TestFormatAttribute();
            _target.RegisterFormatter(typeof(object), notExpected);
            _target.RegisterFormatter(typeof(object), expected);
            var result = _target.TryGetModelFormatter(metadata, FormatMode.Display);
            Assert.AreSame(expected, result);
        }

        #endregion

        #region Test classes

        private class TestFormatAttribute : ModelFormatterAttribute
        {
            public override string FormatValue(object value)
            {
                throw new NotImplementedException();
            }
        }

        private class Model
        {
            [TestFormat]
            public string PropWithAttr { get; set; }

            [TestFormat]
            public DateTime PropWithAttrAndRegisteredDefault { get; set; }

            public bool PropWithDefault { get; set; }
        }

        #endregion
    }
}
