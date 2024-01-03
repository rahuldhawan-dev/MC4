using System;
using System.Reflection;
using MMSINC.CoreTest.Utilities.ObjectMapping.ObjectMappingTestClasses;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.ClassExtensions.ObjectExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class AutoMapAttributeTest
    {
        #region Fields

        private AutoMapAttribute _target;
        private readonly PropertyInfo _mappedBothWaysProp = typeof(PrimaryObject).GetProperty("MappedBothWaysProp");
        private readonly Type _secondaryType = typeof(SecondaryObject);

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AutoMapAttribute();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestEmptyConstructorSetsDirectionToBothWays()
        {
            Assert.AreEqual(MapDirections.BothWays, new AutoMapAttribute().Direction);
        }

        [TestMethod]
        public void TestEmptyConstructorSetsSecondaryPropertyNameToNull()
        {
            Assert.IsNull(new AutoMapAttribute().SecondaryPropertyName);
        }

        [TestMethod]
        public void TestConstructorWithSecondaryPropNameParameterSetsValue()
        {
            Assert.AreEqual("Cool", new AutoMapAttribute("Cool").SecondaryPropertyName);
        }

        [TestMethod]
        public void TestConstructorWithDirectionSetsDirection()
        {
            var expected = MapDirections.None;
            Assert.AreEqual(expected, new AutoMapAttribute(expected).Direction);
            Assert.AreEqual(expected, new AutoMapAttribute("Prop", expected).Direction);
        }

        [TestMethod]
        public void TestCreatePropertyDescriptorReturnsPropertyDescriptorThatMapsToSecondaryType()
        {
            var result = _target.CreatePropertyDescriptor(new Container(), _mappedBothWaysProp, _secondaryType);
            Assert.AreSame(_secondaryType, new PrivateObject(result).GetField("_secondaryType"));
        }

        [TestMethod]
        public void TestCreatePropertyDescriptorReturnsPropertyDescriptorWithDirectionSet()
        {
            var target = new AutoMapAttribute(MapDirections.None);
            var result =
                (AutoPropertyDescriptor)target.CreatePropertyDescriptor(new Container(), _mappedBothWaysProp,
                    _secondaryType);
            Assert.AreEqual(MapDirections.None, result.Direction);
        }

        #endregion
    }
}
