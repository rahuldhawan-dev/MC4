using MMSINC.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    [TestClass]
    public class EntityBoundFieldTest
    {
        [TestMethod]
        public void TestConstructorSetsItemTemplateTypeToDropDownListItemTemplate()
        {
            var x = new EntityBoundField();
            // TODO: Assert.IsInstanceOfType
            Assert.IsTrue(typeof(DropDownListItemTemplate) == x.ItemTemplate.GetType());
        }

        [TestMethod]
        public void TestConstructorSetsEditItemTemplateTypeToDropDownListEditItemTemplate()
        {
            var x = new EntityBoundField();
            // TODO: Assert.IsInstanceOfType
            Assert.IsTrue(typeof(DropDownListEditTemplate) == x.EditItemTemplate.GetType());
        }

        [TestMethod]
        public void TestProperties()
        {
            var testString = "Test";
            var x = new EntityBoundField {
                DataField = testString,
                TypeName = testString,
                SelectMethod = testString,
                DataValueField = testString,
                DataTextField = testString,
                SelectedValue = testString,
                SelectedValueField = testString
            };

            Assert.AreEqual(x.DataField, testString);
            Assert.AreEqual(x.TypeName, testString);
            Assert.AreEqual(x.SelectMethod, testString);
            Assert.AreEqual(x.DataValueField, testString);
            Assert.AreEqual(x.DataTextField, testString);
            Assert.AreEqual(x.SelectedValue, testString);
            Assert.AreEqual(x.SelectedValueField, testString);
        }
    }
}
