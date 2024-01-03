using System;
using System.Reflection;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.CoreTest.Utilities.ObjectMapping.ObjectMappingTestClasses;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class AutoPropertyDescriptorTest
    {
        #region Fields

        private PrimaryObject _primaryObject;
        private SecondaryObject _secondaryObject;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _primaryObject = new PrimaryObject();
            _secondaryObject = new SecondaryObject();
        }

        private AutoPropertyDescriptor InitTarget(string primaryPropName, string secondaryPropertyName = null,
            MapDirections direction = MapDirections.BothWays)
        {
            return new AutoPropertyDescriptor(typeof(PrimaryObject).GetProperty(primaryPropName),
                typeof(SecondaryObject), secondaryPropertyName, direction);
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorThrowsArgumentNullExceptionIfPropertyInfoIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(
                () => new AutoPropertyDescriptor(null, typeof(SecondaryObject)));
        }

        [TestMethod]
        public void TestConstructorThrowsArgumentNullExceptionIfSecondaryTypeIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(
                () => new AutoPropertyDescriptor(new Mock<PropertyInfo>().Object, null));
        }

        [TestMethod]
        public void TestConstructorSetsDirectionProperty()
        {
            foreach (var d in EnumExtensions.GetValues<MapDirections>())
            {
                var target = InitTarget("StringProp", direction: d);
                Assert.AreEqual(d, target.Direction);
            }
        }

        [TestMethod]
        public void TestConstructorCreatesPrimaryAccessor()
        {
            var target = InitTarget("StringProp");
            Assert.IsNotNull(target.PrimaryAccessor);
        }

        [TestMethod]
        public void TestConstructorCreatesSecondaryAccessor()
        {
            var target = InitTarget("StringProp");
            Assert.IsNotNull(target.SecondaryAccessor);
        }

        [TestMethod]
        public void TestConstructorSetsValueConverterToNewInstance()
        {
            var target = InitTarget("StringProp");
            Assert.IsNotNull(target.ValueConverter);
        }

        #endregion

        #region CanMapToPrimary

        [TestMethod]
        public void TestCanMapToPrimaryReturnsTrue_DirectionIsBothWays_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.BothWays);
            Assert.IsTrue(target.CanMapToPrimary);
        }

        [TestMethod]
        public void TestCanMapToPrimaryReturnsTrue_DirectionIsToPrimary_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.ToPrimary);
            Assert.IsTrue(target.CanMapToPrimary);
        }

        [TestMethod]
        public void TestCanMapToPrimaryReturnsFalse_DirectionIsToSecondary_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.ToSecondary);
            Assert.IsFalse(target.CanMapToPrimary);
        }

        [TestMethod]
        public void TestCanMapToPrimaryReturnsFalse_DirectionIsNone_PrimaryHasSetter_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.None);
            Assert.IsFalse(target.CanMapToPrimary);
        }

        [TestMethod]
        public void TestCanMapToPrimaryReturnsFalse_DirectionIsBothWays_SecondaryAccessorIsNull()
        {
            var target = InitTarget("SecondaryWithNonPublicProperty", direction: MapDirections.BothWays);
            Assert.IsFalse(target.CanMapToPrimary);
        }

        [TestMethod]
        public void TestCanMapToPrimaryReturnsFalse_DirectionIsBothWays_SecondaryTypeIsNotAssignableToPrimaryType()
        {
            var target = InitTarget("StringProp", "IntProp");
            Assert.IsFalse(target.CanMapToPrimary);
        }

        #endregion

        #region CanMapToSecondary

        [TestMethod]
        public void TestCanMapToSecondaryReturnsTrue_DirectionIsBothWays_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.BothWays);
            Assert.IsTrue(target.CanMapToSecondary);
        }

        [TestMethod]
        public void TestCanMapToSecondaryReturnsTrue_DirectionIsToSecondary_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.ToSecondary);
            Assert.IsTrue(target.CanMapToSecondary);
        }

        [TestMethod]
        public void TestCanMapToSecondaryReturnsFalse_DirectionIsToPrimary_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.ToPrimary);
            Assert.IsFalse(target.CanMapToSecondary);
        }

        [TestMethod]
        public void TestCanMapToSecondaryReturnsFalse_DirectionIsNone_PrimaryHasSetter_SecondaryHasGetter()
        {
            var target = InitTarget("StringProp", direction: MapDirections.None);
            Assert.IsFalse(target.CanMapToSecondary);
        }

        [TestMethod]
        public void TestCanMapToSecondaryReturnsFalse_DirectionIsBothWays_SecondaryAccessorIsNull()
        {
            var target = InitTarget("SecondaryWithNonPublicProperty", direction: MapDirections.BothWays);
            Assert.IsFalse(target.CanMapToSecondary);
        }

        [TestMethod]
        public void TestCanMapToSecondaryRetrnsFalse_DirectionIsBothWays_SecondaryTypeIsNotAssignableToPrimaryType()
        {
            var target = InitTarget("StringProp", "IntProp");
            Assert.IsFalse(target.CanMapToSecondary);
        }

        #endregion

        #region MapToPrimary

        [TestMethod]
        public void TestMapToPrimary_DoesNotMapIfCanMapToPrimaryIsFalse()
        {
            var target = InitTarget("StringProp");
            target.SetHiddenFieldValueByName("_canConvertToPrimary", new Lazy<bool>(() => false));
            _primaryObject.StringProp = "wowsers";
            _secondaryObject.StringProp = "non wowsers";
            target.MapToPrimary(_primaryObject, _secondaryObject);
            Assert.AreEqual("wowsers", _primaryObject.StringProp);
        }

        [TestMethod]
        public void TestMapToPrimary_CanDoSimpleProperties()
        {
            var target = InitTarget("StringProp");
            _secondaryObject.StringProp = "wowsers";
            target.MapToPrimary(_primaryObject, _secondaryObject);
            Assert.AreEqual("wowsers", _primaryObject.StringProp);
        }

        [TestMethod]
        public void TestMapToPrimary_CanUseNullableGetterToSetValueIfNullableIsNull()
        {
            _primaryObject.IntOnPrimaryNullableIntOnSecondary = 111111;
            _secondaryObject.IntOnPrimaryNullableIntOnSecondary = null;

            var target = InitTarget("IntOnPrimaryNullableIntOnSecondary");
            target.MapToPrimary(_primaryObject, _secondaryObject);
            Assert.AreEqual(0, _primaryObject.IntOnPrimaryNullableIntOnSecondary,
                "Null nullable should be converted to default(T), so 0 in this case.");
        }

        #endregion

        #region MapToSecondary

        [TestMethod]
        public void TestMapToSecondary_DoesNotMapIfCanMapToSecondaryIsFalse()
        {
            var target = InitTarget("StringProp");
            target.SetHiddenFieldValueByName("_canConvertToSecondary", new Lazy<bool>(() => false));
            _primaryObject.StringProp = "non wowsers";
            _secondaryObject.StringProp = "wowsers";
            target.MapToPrimary(_primaryObject, _secondaryObject);
            Assert.AreEqual("wowsers", _secondaryObject.StringProp);
        }

        [TestMethod]
        public void TestMapToSecondary_CanDoSimpleProperties()
        {
            var target = InitTarget("StringProp");
            _primaryObject.StringProp = "wowsers";
            target.MapToSecondary(_primaryObject, _secondaryObject);
            Assert.AreEqual("wowsers", _secondaryObject.StringProp);
        }

        [TestMethod]
        public void TestMapToSecondary_CanUseNullableGetterToSetValueIfNullableIsNull()
        {
            _primaryObject.NullableIntOnPrimaryIntOnSecondary = null;
            _secondaryObject.NullableIntOnPrimaryIntOnSecondary = 10;

            var target = InitTarget("NullableIntOnPrimaryIntOnSecondary");
            target.MapToSecondary(_primaryObject, _secondaryObject);
            Assert.AreEqual(0, _secondaryObject.NullableIntOnPrimaryIntOnSecondary,
                "Null nullable should be converted to default(T), so 0 in this case.");
        }

        //[TestMethod]
        //public void TestMapToEntityCanSetValueOnEntityIfTypesDifferButEntityPropertyTypeIsAssignableFromViewModelPropertyType()
        //{
        //    var expected = new DerivedBaseClass();
        //    _viewModel.ClassProp = expected;
        //    MapToEntity();
        //    Assert.AreSame(expected, _entity.ClassProp);
        //}

        //[TestMethod]
        //public void TestMapToEntityCanNotSetValueOnEntityIfEntityPropertyTypeIsNotAssignableFromViewModelPropertyType()
        //{
        //    var expected = new BaseClass();
        //    _entity.DifferentClassProp = expected;
        //    _viewModel.DifferentClassProp = new DifferentClass();
        //    MapToEntity();
        //    Assert.AreSame(expected, _entity.DifferentClassProp);
        //}

        //[TestMethod]
        //public void TestMapToEntityCanNotSetValueOnEntityIfEntityPropertyTypeIsSubClassOfViewModelPropertyType()
        //{
        //    // Or to explain that name a bit better:
        //    // Make sure that mapping doesn't try to set BaseType on a property that's ChildOfBaseType.
        //    var expected = new DerivedBaseClass();
        //    _entity.SubClassProp = expected;
        //    _viewModel.SubClassProp = new BaseClass();
        //    MapToEntity();
        //    Assert.AreSame(expected, _entity.SubClassProp);
        //}

        #endregion

        #endregion
    }
}
