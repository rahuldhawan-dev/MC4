using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class ObjectMapperTest
    {
        #region Fields

        private TestObjectMapper _target;
        private List<Mock<ObjectPropertyDescriptor>> _descriptors;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _descriptors = new List<Mock<ObjectPropertyDescriptor>>();
            _target = new TestObjectMapper {
                TestDescriptors = _descriptors
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToPrimaryCallsMapToPrimaryOnEachDescriptorReturnedFromGetPropertyDescriptors()
        {
            var expectedPrimaryObject = new object();
            var expectedSecondaryObject = new object();

            var desc1 = new Mock<ObjectPropertyDescriptor>();
            desc1.Setup(x => x.CanMapToPrimary).Returns(true);
            _descriptors.Add(desc1);

            var desc2 = new Mock<ObjectPropertyDescriptor>();
            desc2.Setup(x => x.CanMapToPrimary).Returns(true);
            _descriptors.Add(desc2);

            _target.MapToPrimary(expectedPrimaryObject, expectedSecondaryObject);

            desc1.Verify(x => x.MapToPrimaryCore(expectedPrimaryObject, expectedSecondaryObject));
            desc2.Verify(x => x.MapToPrimaryCore(expectedPrimaryObject, expectedSecondaryObject));
        }

        [TestMethod]
        public void TestMapToSecondaryCallsMapToSecondaryOnEachDescriptorReturnedFromGetPropertyDescriptors()
        {
            var expectedPrimaryObject = new object();
            var expectedSecondaryObject = new object();

            var desc1 = new Mock<ObjectPropertyDescriptor>();
            desc1.Setup(x => x.CanMapToPrimary).Returns(true);
            _descriptors.Add(desc1);

            var desc2 = new Mock<ObjectPropertyDescriptor>();
            desc2.Setup(x => x.CanMapToPrimary).Returns(true);
            _descriptors.Add(desc2);

            _target.MapToPrimary(expectedPrimaryObject, expectedSecondaryObject);

            desc1.Verify(x => x.MapToPrimaryCore(expectedPrimaryObject, expectedSecondaryObject));
            desc2.Verify(x => x.MapToPrimaryCore(expectedPrimaryObject, expectedSecondaryObject));
        }

        [TestMethod]
        public void TestMapToPrimaryThrowsForNullPrimaryObject()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.MapToPrimary(null, new object()));
        }

        [TestMethod]
        public void TestMapToPrimaryThrowsForNullSecondaryObject()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.MapToPrimary(new object(), null));
        }

        [TestMethod]
        public void TestMapToSecondaryThrowsForNullPrimaryObject()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.MapToSecondary(null, new object()));
        }

        [TestMethod]
        public void TestMapToSecondaryThrowsForNullSecondaryObject()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.MapToSecondary(new object(), null));
        }

        #endregion

        #region TestMapper

        private class TestObjectMapper : ObjectMapperBase
        {
            public List<Mock<ObjectPropertyDescriptor>> TestDescriptors { get; set; }

            protected override IEnumerable<ObjectPropertyDescriptor> GetPropertyDescriptors()
            {
                return TestDescriptors.Select(x => x.Object);
            }
        }

        #endregion
    }
}
