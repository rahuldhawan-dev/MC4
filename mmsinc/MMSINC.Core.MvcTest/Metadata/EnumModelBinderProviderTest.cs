using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class EnumModelBinderProviderTest
    {
        #region Test enum

        private enum TestEnum
        {
            // No values!
        }

        #endregion

        #region Fields

        private EnumModelBinderProvider _target;

        #endregion

        #region Init/cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new EnumModelBinderProvider();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetBinderReturnsNullIfTypeIsNotAnEnum()
        {
            Assert.IsNull(_target.GetBinder(typeof(object)));
        }

        [TestMethod]
        public void TestGetBinderReturnsBinderForNullableEnum()
        {
            Assert.IsInstanceOfType(_target.GetBinder(typeof(TestEnum?)), typeof(EnumModelBinder));
        }

        [TestMethod]
        public void TestGetBinderReturnsBinderForEnum()
        {
            Assert.IsInstanceOfType(_target.GetBinder(typeof(TestEnum)), typeof(EnumModelBinder));
        }

        [TestMethod]
        public void TestGetBinderReturnsTheSameBinderForEnumAndNullableEnumOfSameType()
        {
            var enumBinder = _target.GetBinder(typeof(TestEnum));
            var nullableBinder = _target.GetBinder(typeof(TestEnum?));
            Assert.AreSame(enumBinder, nullableBinder);
        }

        #endregion
    }
}
