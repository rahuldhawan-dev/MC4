using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class ObjectPropertyDescriptorTest
    {
        #region Fields

        private TestObjectPropertyDescriptor _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestObjectPropertyDescriptor();
        }

        #endregion

        [TestMethod]
        public void TestMapToPrimaryDoesNotDoAnythingIfCanMapToPrimaryIsFalse()
        {
            _target.TestCanMapToPrimary = false;
            _target.MapToPrimary(new object(), new object());
            Assert.IsFalse(_target.MapToPrimaryCoreWasCalled);
        }

        [TestMethod]
        public void TestMapToSecondaryDoesNotDoAnythingIfCanMapToSecondaryIsFalse()
        {
            _target.TestCanMapToSecondary = false;
            _target.MapToSecondary(new object(), new object());
            Assert.IsFalse(_target.MapToSecondaryCoreWasCalled);
        }

        [TestMethod]
        public void TestMapToPrimaryConvertsValueFromSecondaryAccessorAndSetsItOnPrimaryAccessor()
        {
            var expectedSecondaryValue = "Secondary value";
            var expectedPrimaryValue = "Primary value was converted by converter probably";
            var primaryObject = new object();
            var secondaryObject = new object();

            _target.TestCanMapToPrimary = true;
            var secondaryAccessor = new Mock<IPropertyAccessor>();
            secondaryAccessor.Setup(x => x.GetValue(secondaryObject)).Returns(expectedSecondaryValue).Verifiable();
            _target.SetSecondaryAcessor(secondaryAccessor.Object);

            var primaryAccessor = new Mock<IPropertyAccessor>();
            primaryAccessor.Setup(x => x.SetValue(primaryObject, expectedPrimaryValue)).Verifiable();
            _target.SetPrimaryAccessor(primaryAccessor.Object);

            var converter = new Mock<IValueConverter>();
            converter.Setup(x => x.ToPrimary(expectedSecondaryValue)).Returns(expectedPrimaryValue).Verifiable();
            _target.TestValueConverter = converter.Object;

            _target.MapToPrimary(primaryObject, secondaryObject);
            Assert.IsTrue(_target.MapToPrimaryCoreWasCalled);
            secondaryAccessor.VerifyAll();
            primaryAccessor.VerifyAll();
            converter.VerifyAll();
        }

        [TestMethod]
        public void TestMapToSecondaryConvertsValueFromPrimaryAccessorAndSetsItOnSecondaryAccessor()
        {
            var expectedSecondaryValue = "Secondary value";
            var expectedPrimaryValue = "Primary value was converted by converter probably";
            var primaryObject = new object();
            var secondaryObject = new object();

            _target.TestCanMapToSecondary = true;

            var primaryAccessor = new Mock<IPropertyAccessor>();
            primaryAccessor.Setup(x => x.GetValue(primaryObject)).Returns(expectedPrimaryValue).Verifiable();
            _target.SetPrimaryAccessor(primaryAccessor.Object);

            var secondaryAccessor = new Mock<IPropertyAccessor>();
            secondaryAccessor.Setup(x => x.SetValue(secondaryObject, expectedSecondaryValue)).Verifiable();
            _target.SetSecondaryAcessor(secondaryAccessor.Object);

            var converter = new Mock<IValueConverter>();
            converter.Setup(x => x.ToSecondary(expectedPrimaryValue)).Returns(expectedSecondaryValue).Verifiable();
            _target.TestValueConverter = converter.Object;

            _target.MapToSecondary(primaryObject, secondaryObject);
            Assert.IsTrue(_target.MapToSecondaryCoreWasCalled);
            secondaryAccessor.VerifyAll();
            primaryAccessor.VerifyAll();
            converter.VerifyAll();
        }

        [TestMethod]
        public void TestMapToPrimaryThrowsObjectMapperExceptionWithInnerExceptionAsExceptionThrownByMapToPrimaryCore()
        {
            var primaryObject = new object();
            var secondaryObject = new object();

            var expectedException = new Exception();
            _target.ExceptionToThrow = expectedException;
            _target.TestCanMapToPrimary = true;
            try
            {
                _target.MapToPrimary(primaryObject, secondaryObject);
            }
            catch (ObjectMapperException ex)
            {
                Assert.AreSame(expectedException, ex.InnerException);
                return;
            }

            Assert.Fail("Exception was not thrown");
        }

        #region Test class

        private class TestObjectPropertyDescriptor : ObjectPropertyDescriptor
        {
            public IValueConverter TestValueConverter { get; set; }
            public bool TestCanMapToPrimary { get; set; }
            public bool TestCanMapToSecondary { get; set; }
            public bool MapToPrimaryCoreWasCalled { get; private set; }
            public bool MapToSecondaryCoreWasCalled { get; private set; }
            public Exception ExceptionToThrow { get; set; }

            public void SetPrimaryAccessor(IPropertyAccessor accessor)
            {
                PrimaryAccessor = accessor;
            }

            public void SetSecondaryAcessor(IPropertyAccessor accessor)
            {
                SecondaryAccessor = accessor;
            }

            public override string Name
            {
                get { return "Does not matter what this is."; }
            }

            public override bool CanMapToPrimary
            {
                get { return TestCanMapToPrimary; }
            }

            public override bool CanMapToSecondary
            {
                get { return TestCanMapToSecondary; }
            }

            public override IValueConverter ValueConverter
            {
                get { return TestValueConverter; }
            }

            protected internal override void MapToPrimaryCore(object primaryObject, object secondaryObject)
            {
                MapToPrimaryCoreWasCalled = true;

                if (ExceptionToThrow != null)
                {
                    throw ExceptionToThrow;
                }

                base.MapToPrimaryCore(primaryObject, secondaryObject);
            }

            protected internal override void MapToSecondaryCore(object primaryObject, object secondaryObject)
            {
                MapToSecondaryCoreWasCalled = true;
                if (ExceptionToThrow != null)
                {
                    throw ExceptionToThrow;
                }

                base.MapToSecondaryCore(primaryObject, secondaryObject);
            }
        }

        #endregion
    }
}
