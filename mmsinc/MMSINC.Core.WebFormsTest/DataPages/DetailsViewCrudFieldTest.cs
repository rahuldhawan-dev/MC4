using System;
using MMSINC.DataPages;
using MMSINC.DataPages.Permissions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DetailsViewCrudFieldTest
    /// </summary>
    [TestClass]
    public class DetailsViewCrudFieldTest
    {
        #region Fields

        private DetailsViewCrudField _target;
        private Mock<IDataPagePermissions> _mockPermissions;

        #endregion

        [TestInitialize]
        public void DetailsViewCrudFieldTestInitialize()
        {
            _mockPermissions = new Mock<IDataPagePermissions>();
            _target = new DetailsViewCrudField(_mockPermissions.Object);
        }

        [TestMethod]
        public void TestConstructorThrowsIfPermissionsIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(() => new DetailsViewCrudField(null));
        }

        [TestMethod]
        public void TestConstructorSetsPagePermissionsPropertyToPassedInArgument()
        {
            Assert.AreSame(_mockPermissions.Object, _target.PagePermissions);
        }

        [TestMethod]
        public void TestConstructorSetsShowHeaderPropertyToFalse()
        {
            Assert.IsFalse(_target.ShowHeader);
        }

        [TestMethod]
        public void TestInitSetsItemTemplateProperty()
        {
            var template = _target.ItemTemplate as DetailsViewCrudTemplate;

            Assert.IsNotNull(template);
            Assert.AreEqual(template.TemplateType, DetailsViewCrudTemplateType.ItemTemplate);
        }

        [TestMethod]
        public void TestInitSetsInsertItemTemplateProperty()
        {
            var template = _target.InsertItemTemplate as DetailsViewCrudTemplate;

            Assert.IsNotNull(template);
            Assert.AreEqual(template.TemplateType, DetailsViewCrudTemplateType.InsertTemplate);
        }

        [TestMethod]
        public void TestInitSetsEditItemTemplateProperty()
        {
            var template = _target.EditItemTemplate as DetailsViewCrudTemplate;

            Assert.IsNotNull(template);
            Assert.AreEqual(template.TemplateType, DetailsViewCrudTemplateType.EditTemplate);
        }
    }
}
