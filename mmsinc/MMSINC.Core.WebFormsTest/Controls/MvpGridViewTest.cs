using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for MvpGridViewTest
    /// </summary>
    [TestClass]
    public class MvpGridViewTest
    {
        #region Private Members

        private TestMvpGridView _target;
        private GridViewRow _headerRow;
        private GridViewRow _footerRow;

        #endregion

        #region Private Methods

        private static ObjectDataSource GetSampleObjectDataSource()
        {
            return new ObjectDataSource("MMSINCTest.Controls.TestObjectRepository", "SelectAllAsList");
        }

        private static void FireOnPreRender(MvpGridView gv)
        {
            var mi = gv.GetType().GetMethod("OnPreRender",
                BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(gv, new[] {
                EventArgs.Empty
            });
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MvpGridViewTestInitialize()
        {
            _headerRow = new GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            _footerRow = new GridViewRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
            _target = new TestMvpGridViewBuilder()
                     .WithDataSource(GetSampleObjectDataSource())
                     .WithDataKeyNames(new[] {"Value"})
                     .WithAccessibleHeader(true)
                     .WithHeaderRow(_headerRow)
                     .WithFooterRow(_footerRow);
        }

        #endregion

        [TestMethod]
        public void TestSelectedDataKeyReflectsSelectedValue()
        {
            _target.SelectedIndex = 0;
            Assert.AreEqual(_target.SelectedValue, _target.SelectedDataKey);
        }

        [TestMethod]
        public void TestUsesTHeadWhenUseTHeadAndUseAccessibleHeaderAreTrue()
        {
            _target.UseTHead = true;
            FireOnPreRender(_target);

            Assert.AreEqual(TableRowSection.TableHeader, _headerRow.TableSection);
        }

        [TestMethod]
        public void TestUseTHeadAndUseTFootAreFalseByDefault()
        {
            Assert.IsFalse(_target.UseTHead);
            Assert.IsFalse(_target.UseTFoot);
        }

        [TestMethod]
        public void TestDoesNotUseTHeadWhenUseTHeadIsFalseAndUseAccessibleHeaderIsTrue()
        {
            FireOnPreRender(_target);

            Assert.AreEqual(TableRowSection.TableBody, _headerRow.TableSection);
        }

        [TestMethod]
        public void TestDoesNotUseTHeadWhenUseTHeadIsTrueAndUseAccessibleHeaderIsFalse()
        {
            _target.UseAccessibleHeader = false;
            FireOnPreRender(_target);

            Assert.AreEqual(TableRowSection.TableBody, _headerRow.TableSection);
        }

        [TestMethod]
        public void TestUsesTFootWhenUseTFootAndUseAccessibleHeaderAreTrue()
        {
            _target.UseTFoot = true;
            FireOnPreRender(_target);

            Assert.AreEqual(TableRowSection.TableFooter, _footerRow.TableSection);
        }

        [TestMethod]
        public void TestDoesNotUseTFooWhenUseTFootIsFalseAndUseAccessibleHeaderIsTrue()
        {
            FireOnPreRender(_target);

            Assert.AreEqual(TableRowSection.TableBody, _footerRow.TableSection);
        }

        [TestMethod]
        public void TestDoesNotUseTFootWhenUseTFootIsTrueAndUseAccessibleHeaderIsFalse()
        {
            _target.UseAccessibleHeader = false;
            FireOnPreRender(_target);

            Assert.AreEqual(TableRowSection.TableBody, _footerRow.TableSection);
        }

        [TestMethod]
        public void TestOnPreRenderDoesNotThrowExceptionWhenUseAccessibleHeaderIsTrueAndHeaderOrFooterRowAreNull()
        {
            _target = new TestMvpGridViewBuilder()
                     .WithAccessibleHeader(true)
                     .WithHeaderRow(null)
                     .WithFooterRow(_footerRow);

            MyAssert.DoesNotThrow(() => FireOnPreRender(_target));

            _target = new TestMvpGridViewBuilder()
                     .WithAccessibleHeader(true)
                     .WithHeaderRow(_headerRow)
                     .WithFooterRow(null);

            MyAssert.DoesNotThrow(() => FireOnPreRender(_target));
        }

        [TestMethod]
        public void TestSetSortDirection()
        {
            var sortDir = SortDirection.Ascending;
            _target.SetSortDirection(sortDir);
            Assert.AreEqual(sortDir, _target.SortDirection);
            sortDir = SortDirection.Descending;
            _target.SetSortDirection(sortDir);
            Assert.AreEqual(sortDir, _target.SortDirection);
        }

        [TestMethod]
        public void TestSelectedIndexSet()
        {
            var selectedIndex = 0;
            _target.SelectedIndex = selectedIndex;
            Assert.AreEqual(selectedIndex, _target.SelectedIndex);
            _target.SetSortDirection(SortDirection.Descending);
            _target.SelectedIndex = selectedIndex;
            Assert.AreEqual(selectedIndex + 1, _target.SelectedIndex);
        }
    }

    internal class TestMvpGridViewBuilder : TestDataBuilder<TestMvpGridView>
    {
        #region Private Members

        private ObjectDataSource _dataSource;
        private string[] _dataKeyNames;

        private bool? _autoGenerateSelectButton,
                      _autoGenerateDeleteButton,
                      _autoGenerateEditButton;

        private string _onClientSelect, _onClientDelete, _onClientUpdate;
        private DataControlFieldCollection _columns;
        private bool? _useAccessibleHeader;
        private GridViewRow _headerRow, _footerRow;

        #endregion

        #region Private Methods

        private void AttachColumns(GridView gridView)
        {
            foreach (DataControlField column in _columns)
                gridView.Columns.Add(column);
        }

        #endregion

        #region Exposed Methods

        public override TestMvpGridView Build()
        {
            var gv = new TestMvpGridView();
            if (_dataSource != null)
                gv.DataSource = _dataSource;
            if (_dataKeyNames != null)
                gv.DataKeyNames = _dataKeyNames;
            if (_autoGenerateSelectButton != null)
                gv.AutoGenerateSelectButton = _autoGenerateSelectButton.Value;
            if (_onClientSelect != null)
                gv.OnClientSelect = _onClientSelect;
            if (_autoGenerateDeleteButton != null)
                gv.AutoGenerateDeleteButton = _autoGenerateDeleteButton.Value;
            if (_onClientDelete != null)
                gv.OnClientDelete = _onClientDelete;
            if (_autoGenerateEditButton != null)
                gv.AutoGenerateEditButton = _autoGenerateEditButton.Value;
            if (_onClientUpdate != null)
                gv.OnClientUpdate = _onClientUpdate;
            if (_columns != null)
                AttachColumns(gv);
            if (_useAccessibleHeader != null)
                gv.UseAccessibleHeader = _useAccessibleHeader.Value;
            if (_headerRow != null)
                gv.SetHeaderRow(_headerRow);
            if (_footerRow != null)
                gv.SetFooterRow(_footerRow);
            return gv;
        }

        public TestMvpGridViewBuilder WithAutoGenerateSelectButton(bool autoGenerateSelectButton)
        {
            _autoGenerateSelectButton = autoGenerateSelectButton;
            return this;
        }

        public TestMvpGridViewBuilder WithOnClientSelect(string onClientSelect)
        {
            _onClientSelect = onClientSelect;
            return this;
        }

        public TestMvpGridViewBuilder WithAutoGenerateDeleteButton(bool autoGenerateDeleteButton)
        {
            _autoGenerateDeleteButton = autoGenerateDeleteButton;
            return this;
        }

        public TestMvpGridViewBuilder WithOnClientDelete(string onClientDelete)
        {
            _onClientDelete = onClientDelete;
            return this;
        }

        public TestMvpGridViewBuilder WithOnClientUpdate(string update)
        {
            _onClientUpdate = update;
            return this;
        }

        public TestMvpGridViewBuilder WithAutoGenerateEditButton(bool b)
        {
            _autoGenerateEditButton = b;
            return this;
        }

        public TestMvpGridViewBuilder WithDataSource(ObjectDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestMvpGridViewBuilder WithDataKeyNames(string[] dataKeyNames)
        {
            _dataKeyNames = dataKeyNames;
            return this;
        }

        public TestMvpGridViewBuilder WithColumns(DataControlFieldCollection columns)
        {
            _columns = columns;
            return this;
        }

        public TestMvpGridViewBuilder WithColumn(DataControlField column)
        {
            _columns = new DataControlFieldCollection {column};
            return this;
        }

        public TestMvpGridViewBuilder WithAccessibleHeader(bool useAccessibleHeader)
        {
            _useAccessibleHeader = useAccessibleHeader;
            return this;
        }

        public TestMvpGridViewBuilder WithHeaderRow(GridViewRow headerRow)
        {
            _headerRow = headerRow;
            return this;
        }

        public TestMvpGridViewBuilder WithFooterRow(GridViewRow footerRow)
        {
            _footerRow = footerRow;
            return this;
        }

        #endregion
    }

    internal class TestMvpGridView : MvpGridView
    {
        #region Private Members

        private GridViewRow _headerRow, _footerRow;

        #endregion

        #region Properties

        public override GridViewRow HeaderRow
        {
            get
            {
                if (_headerRow == null)
                    return base.HeaderRow;
                return _headerRow;
            }
        }

        public override GridViewRow FooterRow
        {
            get
            {
                if (_footerRow == null)
                    return base.FooterRow;
                return _footerRow;
            }
        }

        #endregion

        #region Exposed Methods

        public void SetHeaderRow(GridViewRow headerRow)
        {
            _headerRow = headerRow;
        }

        public void SetFooterRow(GridViewRow footerRow)
        {
            _footerRow = footerRow;
        }

        #endregion
    }

    internal class TestObject
    {
        #region Properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }

    internal class TestObjectRepository
    {
        #region Private Static Members

        private static readonly List<TestObject> _items = new List<TestObject> {
            new TestObject {Name = "Foo", Value = 1.ToString()},
            new TestObject {Name = "Bar", Value = 2.ToString()},
            new TestObject {Name = "Baz", Value = 3.ToString()}
        };

        #endregion

        #region Exposed Static Methods

        public static List<TestObject> SelectAllAsList()
        {
            return _items;
        }

        #endregion
    }
}
