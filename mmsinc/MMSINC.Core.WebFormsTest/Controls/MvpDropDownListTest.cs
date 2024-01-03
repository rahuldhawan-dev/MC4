using System;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for MvpDropDownListTest
    /// </summary>
    [TestClass]
    public class MvpDropDownListTest
    {
        #region Private Methods

        private static ListItem[] GetSampleListItemArray()
        {
            return new[] {
                new ListItem("Foo", 1.ToString()),
                new ListItem("Bar", 2.ToString()),
                new ListItem("Baz", 3.ToString())
            };
        }

        private static MvpDropDownList GetSampleMvpDropDownListWithItems()
        {
            return
                new TestMvpDropDownListBuilder().WithItems(GetSampleListItemArray());
        }

        #endregion

        [TestMethod]
        public void TestSelectedDataKeyReflectsSelectedValue()
        {
            var target = GetSampleMvpDropDownListWithItems();

            target.SelectedValue = 1.ToString();
            Assert.AreEqual(target.SelectedValue, target.SelectedDataKey);
        }

        [TestMethod]
        public void TestSelect()
        {
            var target = GetSampleMvpDropDownListWithItems();
            MyAssert.Throws<InvalidOperationException>(
                () => target.SetSortDirection(SortDirection.Ascending));
        }
    }

    internal class TestMvpDropDownListBuilder : TestDataBuilder<MvpDropDownList>
    {
        #region Private Members

        private ListItem[] _items;

        #endregion

        #region Exposed Methods

        public override MvpDropDownList Build()
        {
            var ddl = new MvpDropDownList();
            if (_items != null && _items.Count() > 0)
                ddl.Items.AddRange(_items);
            return ddl;
        }

        public TestMvpDropDownListBuilder WithItems(ListItem[] items)
        {
            _items = items;
            return this;
        }

        #endregion
    }
}
