﻿#if DEBUG
using System;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
#endif
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for MvpGridViewClientHandlerTest
    /// </summary>
    [TestClass]
    public class MvpGridViewClientHandlerTest
    {
#if DEBUG

        [TestMethod]
        public void TestOnClientSelectReturnsGridViewOnClientSelect()
        {
            var expected = "Test String";
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithOnClientSelect(
                    expected).Build();

            Assert.AreEqual(expected, target.OnClientSelect);

            expected = null;
            target = new TestMvpGridViewClientHandlerBuilder().Build();

            Assert.AreEqual(expected, target.OnClientSelect);
        }

        [TestMethod]
        public void TestUseClientSelectReturnsTrueWhenAutoGenerateSelectButtonAndOnClientSelectIsNotNull()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateSelectButton(true).WithOnClientSelect(
                    String.Empty).Build();

            Assert.IsTrue(target.UseClientSelect);
        }

        [TestMethod]
        public void TestUseClientSelectReturnsFalseWhenAutoGenerateSelectButtonIsFalse()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateSelectButton(false).WithOnClientSelect(
                    String.Empty).Build();

            Assert.IsFalse(target.UseClientSelect);
        }

        [TestMethod]
        public void TestUseClientSelectReturnsFalseWhenOnClientSelectIsNull()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateSelectButton(true).Build();

            Assert.IsFalse(target.UseClientSelect);
        }

        [TestMethod]
        public void TestOnClientUpdateReturnsGridViewOnClientUpdate()
        {
            var expected = "Test String";
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithOnClientUpdate(
                    expected).Build();

            Assert.AreEqual(expected, target.OnClientUpdate);

            expected = null;
            target = new TestMvpGridViewClientHandlerBuilder().Build();

            Assert.AreEqual(expected, target.OnClientUpdate);
        }

        [TestMethod]
        public void TestUseClientUpdateReturnsTrueWhenAutoGenerateEditButtonAndOnClientUpdateIsNotNull()
        {
            var target = new TestMvpGridViewClientHandlerBuilder()
                        .WithAutoGenerateEditButton(true)
                        .WithOnClientUpdate(String.Empty)
                        .Build();

            Assert.IsTrue(target.UseClientUpdate);
        }

        [TestMethod]
        public void TestUseClientUpdateReturnsFalseWhenAutoGenerateEditButtonIsFalse()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateEditButton(false).WithOnClientUpdate(
                    String.Empty).Build();

            Assert.IsFalse(target.UseClientUpdate);
        }

        [TestMethod]
        public void TestUseClientUpdateReturnsFalseWhenOnClientUpdateIsNull()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateEditButton(true).Build();

            Assert.IsFalse(target.UseClientUpdate);
        }

        [TestMethod]
        public void TestOnClientDeleteReturnsGridViewOnClientDelete()
        {
            var expected = "Test String";
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithOnClientDelete(
                    expected).Build();

            Assert.AreEqual(expected, target.OnClientDelete);

            expected = null;
            target = new TestMvpGridViewClientHandlerBuilder().Build();

            Assert.AreEqual(expected, target.OnClientDelete);
        }

        [TestMethod]
        public void TestUseClientDeleteReturnsTrueWhenAutoGenerateDeleteButtonAndOnClientDeleteIsNotNull()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateDeleteButton(true)
                                                         .WithOnClientDelete(String.Empty).Build();

            Assert.IsTrue(target.UseClientDelete);
        }

        [TestMethod]
        public void TestUseClientDeleteReturnsFalseWhenAutoGenerateDeleteButtonIsFalse()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateDeleteButton(false).WithOnClientDelete(
                    String.Empty).Build();

            Assert.IsFalse(target.UseClientDelete);
        }

        [TestMethod]
        public void TestUseClientDeleteReturnsFalseWhenOnClientDeleteIsNull()
        {
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateDeleteButton(true).Build();

            Assert.IsFalse(target.UseClientDelete);
        }

        [TestMethod]
        public void TestAttachHandlersToRowAttachesClientSelectHandler()
        {
            var expected = "Test String";
            var btn = new LinkButton {CommandName = "Select"};
            var cell = new TableCell {Controls = {btn}};
            var args =
                new TestGridViewRowEventArgsBuilder().WithCell(cell).Build();
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateSelectButton(true).WithOnClientSelect(
                    expected).Build();

            target.AttachHandlersToRow(args);

            Assert.AreEqual(expected, btn.OnClientClick);
        }

        [TestMethod]
        public void TestAttachHandlersToRowAttachesClientDeleteHandler()
        {
            var expected = "Test String";
            var btn = new LinkButton {CommandName = "Delete"};
            var cell = new TableCell {Controls = {btn}};
            var args =
                new TestGridViewRowEventArgsBuilder().WithCell(cell).Build();
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateDeleteButton(true).WithOnClientDelete(
                    expected).Build();

            target.AttachHandlersToRow(args);

            Assert.AreEqual(expected, btn.OnClientClick);
        }

        [TestMethod]
        public void TestAttachHandlersToRowAttachesClientUpdateHandler()
        {
            var expected = "Test String";
            var btn = new LinkButton {CommandName = "Update"};
            var cell = new TableCell {Controls = {btn}};
            var args =
                new TestGridViewRowEventArgsBuilder().WithCell(cell).Build();
            var target =
                new TestMvpGridViewClientHandlerBuilder().WithAutoGenerateEditButton(true).WithOnClientUpdate(
                    expected).Build();

            target.AttachHandlersToRow(args);

            Assert.AreEqual(expected, btn.OnClientClick);
        }

#else
        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail(
                "The tests for MvpGridViewClientHandler must be run in debug mode.  Please change your configuration and re-run");
        }

#endif
    }

#if DEBUG
    internal class TestMvpGridViewClientHandlerBuilder : TestDataBuilder<MvpGridViewClientHandler>
    {
        #region Private Members

        private TestMvpGridViewBuilder _gridViewBuilder;

        #endregion

        #region Constructors

        internal TestMvpGridViewClientHandlerBuilder()
        {
            _gridViewBuilder = new TestMvpGridViewBuilder();
        }

        #endregion

        #region Exposed Methods

        public override MvpGridViewClientHandler Build()
        {
            return new MvpGridViewClientHandler(_gridViewBuilder.Build());
        }

        public TestMvpGridViewClientHandlerBuilder WithAutoGenerateSelectButton(bool autoGenerateSelectButton)
        {
            _gridViewBuilder =
                _gridViewBuilder.WithAutoGenerateSelectButton(
                    autoGenerateSelectButton);
            return this;
        }

        public TestMvpGridViewClientHandlerBuilder WithOnClientSelect(string onClientSelect)
        {
            _gridViewBuilder =
                _gridViewBuilder.WithOnClientSelect(onClientSelect);
            return this;
        }

        public TestMvpGridViewClientHandlerBuilder WithAutoGenerateDeleteButton(bool autoGenerateDeleteButton)
        {
            _gridViewBuilder =
                _gridViewBuilder.WithAutoGenerateDeleteButton(
                    autoGenerateDeleteButton);
            return this;
        }

        public TestMvpGridViewClientHandlerBuilder WithOnClientDelete(string onClientDelete)
        {
            _gridViewBuilder =
                _gridViewBuilder.WithOnClientDelete(onClientDelete);
            return this;
        }

        public TestMvpGridViewClientHandlerBuilder WithOnClientUpdate(string onClientUpdate)
        {
            _gridViewBuilder =
                _gridViewBuilder.WithOnClientUpdate(onClientUpdate);
            return this;
        }

        public TestMvpGridViewClientHandlerBuilder WithColumns(DataControlFieldCollection columns)
        {
            _gridViewBuilder = _gridViewBuilder.WithColumns(columns);
            return this;
        }

        public TestMvpGridViewClientHandlerBuilder WithColumn(DataControlField column)
        {
            _gridViewBuilder = _gridViewBuilder.WithColumn(column);
            return this;
        }

        #endregion

        public TestMvpGridViewClientHandlerBuilder WithAutoGenerateEditButton(bool autoGenerateEditButton)
        {
            _gridViewBuilder =
                _gridViewBuilder.WithAutoGenerateEditButton(
                    autoGenerateEditButton);
            return this;
        }
    }

    internal class TestGridViewRowEventArgsBuilder : TestDataBuilder<GridViewRowEventArgs>
    {
        #region Private Members

        private int _rowIndex = 0, _dataItemIndex = 0;
        private DataControlRowType _rowType = DataControlRowType.DataRow;
        private DataControlRowState _rowState = DataControlRowState.Normal;
        private TableCell[] _cells;

        #endregion

        #region Private Methods

        private void AddCells(TableRow row)
        {
            foreach (var cell in _cells)
                row.Cells.Add(cell);
        }

        #endregion

        #region Exposed Methods

        public override GridViewRowEventArgs Build()
        {
            var row = new GridViewRow(_rowIndex, _dataItemIndex, _rowType,
                _rowState);
            if (_cells != null)
                AddCells(row);
            return new GridViewRowEventArgs(row);
        }

        public TestGridViewRowEventArgsBuilder WithCells(TableCell[] cells)
        {
            _cells = cells;
            return this;
        }

        public TestGridViewRowEventArgsBuilder WithCell(TableCell cell)
        {
            _cells = new[] {cell};
            return this;
        }

        #endregion
    }
#endif
}
