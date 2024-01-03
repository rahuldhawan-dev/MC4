using System;
using System.Reflection;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    [TestClass]
    public class MvpBoundFieldTest
    {
        [TestMethod]
        public void TestIntializeDataCellAddsTextBox()
        {
            //DataControlFieldCell, DataControlRowState.Insert
            var cell = new DataControlFieldCell(null);
            var mvpBoundField = new MvpBoundField();
            var methodInfo =
                typeof(MvpBoundField).GetMethod("InitializeDataCell",
                    BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(mvpBoundField, new object[] {
                cell, DataControlRowState.Insert
            });
            Assert.IsNotNull(cell.Controls);
            Assert.IsInstanceOfType(cell.Controls[0], typeof(TextBox));
        }

        [TestMethod]
        public void TestInitializeDataCellWiresDatabindingOnAddedTextBox()
        {
            var cell = new DataControlFieldCell(null);
            var target = new TestMvpBoundField {DataField = "Test"};
            target.CallInitializeDataCell(cell, DataControlRowState.Insert);
            cell.Controls[0].DataBind();
            Assert.IsTrue(target.OnDataBindFieldCalled);
        }
    }

    internal class TestMvpBoundField : MvpBoundField
    {
        public bool OnDataBindFieldCalled { get; protected set; }

        protected override void OnDataBindField(object sender, EventArgs e)
        {
            OnDataBindFieldCalled = true;
        }

        public void CallInitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            InitializeDataCell(cell, rowState);
        }
    }
}
