using System;
using System.Collections.Generic;
using MMSINC.CoreTest.Utilities.ObjectMapping.ObjectMappingTestClasses;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ObjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.CoreTest.Utilities.ObjectMapping
{
    [TestClass]
    public class AutoObjectMapperTest
    {
        #region Fields

        private AutoObjectMapper _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AutoObjectMapper(new Container(), typeof(PrimaryObject), typeof(SecondaryObject));
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorThrowsForNullEntityType()
        {
            MyAssert.Throws<ArgumentNullException>(() => new AutoObjectMapper(new Container(), null, typeof(object)));
        }

        [TestMethod]
        public void TestConstructorThrowsForNullViewModelType()
        {
            MyAssert.Throws<ArgumentNullException>(() => new AutoObjectMapper(new Container(), typeof(object), null));
        }

        #endregion

        [TestMethod]
        public void TestGetPropertyDescriptorsReturnsCachedDescriptors()
        {
            var privTarget = new PrivateObject(_target);
            var result1 = (IEnumerable<ObjectPropertyDescriptor>)privTarget.Invoke("GetPropertyDescriptors");
            var result2 = (IEnumerable<ObjectPropertyDescriptor>)privTarget.Invoke("GetPropertyDescriptors");
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreSame(result1, result2);
        }

        #region MapToPrimary

        [TestMethod]
        public void TestMapToPrimary_WorksCorrectlyWithInheritedPropertiesWithMapAttributes()
        {
            var primary = new DerivedMapAttributedPrimaryObject();
            var secondary = new MapAttributedSecondaryObject {
                BaseMapToPrimaryProperty = "sup",
                BaseMapToSecondaryProperty = "also sup"
            };
            var target = new AutoObjectMapper(new Container(), typeof(DerivedMapAttributedPrimaryObject),
                typeof(MapAttributedSecondaryObject));

            target.MapToPrimary(primary, secondary);

            Assert.AreEqual(secondary.BaseMapToPrimaryProperty, primary.BaseMapToPrimaryProperty,
                "Inherited property should map to primary.");
            Assert.IsNull(primary.BaseMapToSecondaryProperty,
                "Inherited property should only map to secondary, so this should not be getting set.");
        }

        [TestMethod]
        public void TestMapToPrimary_WorksCorrectlyWithOverriddenVirtualPropertiesWithMapAttributes()
        {
            var primary = new DerivedMapAttributedPrimaryObject();
            var secondary = new MapAttributedSecondaryObject {
                VirtualMappedProperty = "sup"
            };
            var target = new AutoObjectMapper(new Container(), typeof(DerivedMapAttributedPrimaryObject),
                typeof(MapAttributedSecondaryObject));

            target.MapToPrimary(primary, secondary);

            Assert.AreEqual("sup", primary.VirtualMappedProperty);
        }

        [TestMethod]
        public void TestMapToPrimary_WorksCorrectlyWithWithPropertiesThatOverrideTheBaseMapAttribute()
        {
            var primary = new DerivedMapAttributedPrimaryObject();
            var secondary = new MapAttributedSecondaryObject {
                OverrideMappedProperty = "sup"
            };
            var target = new AutoObjectMapper(new Container(), typeof(DerivedMapAttributedPrimaryObject),
                typeof(MapAttributedSecondaryObject));

            target.MapToPrimary(primary, secondary);

            Assert.AreEqual("sup", primary.OverrideMappedProperty);
        }

        #endregion

        #region MapToSecondary

        [TestMethod]
        public void TestMapToSecondary_WorksCorrectlyWithInheritedPropertiesWithMapAttributes()
        {
            var primary = new DerivedMapAttributedPrimaryObject {
                BaseMapToPrimaryProperty = "Primary",
                BaseMapToSecondaryProperty = "Secondary"
            };
            var secondary = new MapAttributedSecondaryObject();
            var target = new AutoObjectMapper(new Container(), typeof(DerivedMapAttributedPrimaryObject),
                typeof(MapAttributedSecondaryObject));

            target.MapToSecondary(primary, secondary);

            Assert.AreEqual(primary.BaseMapToSecondaryProperty, secondary.BaseMapToSecondaryProperty,
                "Inherited property should map to secondary.");
            Assert.IsNull(secondary.BaseMapToPrimaryProperty,
                "Inherited property should only map to primary, so this should not be getting set.");
        }

        [TestMethod]
        public void TestMapToSecondary_WorksCorrectlyWithOverriddenVirtualPropertiesWithMapAttributes()
        {
            var primary = new DerivedMapAttributedPrimaryObject {
                VirtualMappedProperty = "sup"
            };
            var secondary = new MapAttributedSecondaryObject();

            var target = new AutoObjectMapper(new Container(), typeof(DerivedMapAttributedPrimaryObject),
                typeof(MapAttributedSecondaryObject));
            target.MapToSecondary(primary, secondary);
            Assert.AreEqual("sup", secondary.VirtualMappedProperty);
        }

        [TestMethod]
        public void TestMapToSecondary_WorksCorrectlyWithWithPropertiesThatOverrideTheBaseMapAttribute()
        {
            var primary = new DerivedMapAttributedPrimaryObject {
                OverrideMappedProperty = "sup"
            };
            var secondary = new MapAttributedSecondaryObject();
            var target = new AutoObjectMapper(new Container(), typeof(DerivedMapAttributedPrimaryObject),
                typeof(MapAttributedSecondaryObject));

            target.MapToSecondary(primary, secondary);
            Assert.AreEqual("sup", secondary.OverrideMappedProperty,
                "This test will fail if the override attribute is not used. The base attribute has Direction = None.");
        }

        [TestMethod]
        public void TestMapToSecondary_ThrowsExceptionWhenMultipleMappersTryToSetValueOnTheSameSecondaryProperty()
        {
            var primary = new SomePrimaryObject {
                Property = "Expected",
                DuplicateMappedProperty = "Also maybe expected"
            };
            var secondary = new SomeSecondaryObject();
            var target = new AutoObjectMapper(new Container(), typeof(SomePrimaryObject), typeof(SomeSecondaryObject));

            MyAssert.Throws<ObjectMapperException>(() => target.MapToSecondary(primary, secondary));
        }

        #endregion

        [TestMethod]
        public void TestCorrectAttributeIsUsedWhenInheritingClassOverridesAutoMapAttribute()
        {
            var primary = new SonOfSomePrimaryObject {
                Property = "Expected",
                DuplicateMappedProperty = "Not expected because attribute."
            };
            var secondary = new SomeSecondaryObject();
            var target = new AutoObjectMapper(new Container(), typeof(SonOfSomePrimaryObject),
                typeof(SomeSecondaryObject));

            target.MapToSecondary(primary, secondary);

            Assert.AreEqual("Expected", secondary.Property);
        }

        [TestMethod]
        public void TestPropertiesAreFoundForParentsOfInterfaces()
        {
            var primary = new SonOfSomePrimaryObject {
                Property = "Expected",
                DuplicateMappedProperty = "Not expected because attribute."
            };
            var secondary = new SonOfSomeSecondaryObject();
            var target = new AutoObjectMapper(new Container(), typeof(SonOfSomePrimaryObject),
                typeof(ISonOfSomeSecondaryObject));

            target.MapToSecondary(primary, secondary);

            Assert.AreEqual("Expected", secondary.Property);
        }

        #endregion

        #region Models

        private class SomeSecondaryObject : ISomeSecondaryObject
        {
            public string Property { get; set; }
        }

        private class SonOfSomeSecondaryObject : SomeSecondaryObject, ISonOfSomeSecondaryObject { }

        private interface ISomeSecondaryObject
        {
            string Property { get; set; }
        }

        private interface ISonOfSomeSecondaryObject : ISomeSecondaryObject { }

        private class SomePrimaryObject
        {
            public string Property { get; set; }

            [AutoMap("Property")]
            public virtual string DuplicateMappedProperty { get; set; }
        }

        private class SonOfSomePrimaryObject : SomePrimaryObject
        {
            [AutoMap(MapDirections.None)]
            public override string DuplicateMappedProperty { get; set; }
        }

        #endregion
    }
}
