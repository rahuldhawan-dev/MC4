using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class SelectListItemConverterTest
    {
        [TestMethod]
        public void TestCanConvertAGenericIEnumerableWithGetterAndSetterFuncs()
        {
            var expected = new[] {
                new TestModel {
                    IntegerProp = 1,
                    StringProp = "foo@bar.com"
                }
            };

            var result = SelectListItemConverter.Convert(expected, u => u.IntegerProp, u => u.StringProp).Single();

            Assert.AreEqual("1", result.Value);
            Assert.AreEqual("foo@bar.com", result.Text);
        }
        
        [TestMethod]
        public void TestCanConvertANonGenericIEnumerableWithGetterAndSetterStringsThatUseProperties()
        {
            var expected = new[] {
                new TestModel {
                    IntegerProp = 1,
                    StringProp = "foo@bar.com"
                }
            };

            var result = SelectListItemConverter.Convert(expected, "IntegerProp", "StringProp").Single();

            Assert.AreEqual("1", result.Value);
            Assert.AreEqual("foo@bar.com", result.Text);
        }

        [TestMethod]
        public void TestCanConvertANonGenericIEnumerableWithGetterAndSetterStringsThatUseFields()
        {
            var expected = new[] {
                new TestModel {
                    IntegerField = 1,
                    StringField = "foo@bar.com"
                }
            };

            var result = SelectListItemConverter.Convert(expected, "IntegerField", "StringField").Single();

            Assert.AreEqual("1", result.Value);
            Assert.AreEqual("foo@bar.com", result.Text);
        }

        [TestMethod]
        public void TestCanConvertFromEnum()
        {
            var result = SelectListItemConverter.ConvertFromEnumType(typeof(TestEnum)).Single();

            Assert.AreEqual("42", result.Value);
            Assert.AreEqual("Neato", result.Text);
        }

        #region Helper classes

        private class TestModel
        {
            public int IntegerProp { get; set; }
            public int IntegerField;
            public string StringProp { get; set; }
            public string StringField;
        }

        private enum TestEnum
        {
            Neato = 42
        }

        #endregion
    }
}
