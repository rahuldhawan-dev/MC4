using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Subtext.TestLibrary;

namespace MMSINC.Core.WebFormsTest.Controls
{
    [TestClass]
    public class ClientCapableCheckBoxFieldTest
    {
        #region Private Members

        HttpSimulator _simulator;
        MockRepository _mocks;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void DynamicBoundFieldTestInitialize()
        {
            _mocks = new MockRepository();
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void DynamicBoundFieldTestCleanup()
        {
            _mocks.VerifyAll();
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestInitializeCellAddsOnClientClickToInputAttributes()
        {
            var target = new ClientCapableCheckBoxField {
                OnClientClick = "alert('foo');",
                DataField = "Foo"
            };

            var cell = new DataControlFieldCell(null);
            target.InitializeCell(cell, DataControlCellType.DataCell, DataControlRowState.Insert, 0);
            var chkbox = (CheckBox)cell.Controls[0];
            Assert.AreEqual(target.OnClientClick, chkbox.InputAttributes["onclick"]);
        }

        [TestMethod]
        public void TestInitializeCellDoesNotAddOnClientClickToInputAttributesForNullOrEmptyOnClientClick()
        {
            var target = new ClientCapableCheckBoxField {
                DataField = "Foo"
            };

            var cell = new DataControlFieldCell(null);
            target.InitializeCell(cell, DataControlCellType.DataCell, DataControlRowState.Insert, 0);
            var chkbox = (CheckBox)cell.Controls[0];
            Assert.IsNull(chkbox.InputAttributes["onclick"]);

            target.OnClientClick = String.Empty;
            target.InitializeCell(cell, DataControlCellType.DataCell, DataControlRowState.Insert, 0);
            chkbox = (CheckBox)cell.Controls[0];
            Assert.IsNull(chkbox.InputAttributes["onclick"]);
        }

        [TestMethod]
        public void TestInitializeCellGeneratesProperControlID()
        {
            //"dvChkFoo_0"
            var rowIndex = 0;
            var dataField = "Foo";
            var idFormatString = "dvChk{0}_{1}";
            var onClientClick = "alert('foo');";

            var target = new ClientCapableCheckBoxField {
                OnClientClick = onClientClick,
                DataField = dataField
            };

            var cell = new DataControlFieldCell(null);
            target.InitializeCell(cell, DataControlCellType.DataCell, DataControlRowState.Insert, rowIndex);
            var chkbox = (CheckBox)cell.Controls[0];
            Assert.AreEqual(String.Format(idFormatString, dataField, rowIndex), chkbox.ID);
        }

        //[TestMethod]
        public void TestDataBindingAddsOnClientClickToInputAttributes()
        {
            var onDataBindField = typeof(ClientCapableCheckBoxField).GetMethod("OnDataBindField",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var parent = typeof(Control).GetField("_parent",
                BindingFlags.NonPublic | BindingFlags.Instance);
            using (_simulator.SimulateRequest())
            {
                var gvr = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal) {
                    DataItem = new {Foo = false}
                };
                var target = new ClientCapableCheckBoxField {
                    OnClientClick = "alert('foo');",
                    DataField = "Foo"
                };
                var chkbox = new CheckBox();
                parent.SetValue(chkbox, gvr);

                onDataBindField.Invoke(target, new object[] {chkbox, null});

                MyAssert.IsGreaterThan(chkbox.InputAttributes.Count, 0);
            }
        }

        //[TestMethod]
        public void TestDataBindingDoesNotAddNullorEmptyOnClientClickToInputAttributes()
        {
            var onDataBindField = typeof(ClientCapableCheckBoxField).GetMethod("OnDataBindField",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var parent = typeof(Control).GetField("_parent",
                BindingFlags.NonPublic | BindingFlags.Instance);
            using (_simulator.SimulateRequest())
            {
                var gvr = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal) {
                    DataItem = new {Foo = false}
                };
                var target = new ClientCapableCheckBoxField {
                    OnClientClick = String.Empty,
                    DataField = "Foo"
                };
                var chkbox = new CheckBox();
                parent.SetValue(chkbox, gvr);

                onDataBindField.Invoke(target, new object[] {chkbox, null});
                Assert.AreEqual(chkbox.InputAttributes.Count, 0);

                target = new ClientCapableCheckBoxField {
                    OnClientClick = String.Empty,
                    DataField = "Foo"
                };

                onDataBindField.Invoke(target, new object[] {chkbox, null});
                Assert.AreEqual(chkbox.InputAttributes.Count, 0);
            }
        }
    }
}
