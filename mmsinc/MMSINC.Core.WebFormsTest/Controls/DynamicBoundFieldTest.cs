using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Subtext.TestLibrary;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for DynamicBoundFieldTest
    /// </summary>
    [TestClass]
    public class DynamicBoundFieldTest
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
        public void TestTextModePropertyDefaultsToSingleLine()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new DynamicBoundField();
                Assert.AreEqual(TextBoxMode.SingleLine,
                    target.TextMode, "TextMode for a DynamicBoundField should default to TextBoxMode.SingleLine.");
            }
        }

        [TestMethod]
        public void TestTextModePropertyPersistence()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new DynamicBoundField();
                TextBoxMode expected;

                expected = TextBoxMode.Password;
                target.TextMode = expected;
                Assert.AreEqual(expected,
                    target.TextMode);

                expected = TextBoxMode.MultiLine;
                target.TextMode = expected;
                Assert.AreEqual(expected,
                    target.TextMode);
            }
        }

        [TestMethod]
        public void TestTextBoxModeSettingReflectsDuringDataBind()
        {
            var onDataBindField = typeof(DynamicBoundField).GetMethod("OnDataBindField",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var parent = typeof(Control).GetField("_parent",
                BindingFlags.NonPublic | BindingFlags.Instance);
            using (_simulator.SimulateRequest())
            {
                var gvr = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal) {
                    DataItem = new {
                        Foo = "foo"
                    }
                };
                var target = new DynamicBoundField {
                    DataField = "Foo"
                };
                var txt = new TextBox();
                parent.SetValue(txt, gvr);
                TextBoxMode expected;

                expected = TextBoxMode.SingleLine;
                target.TextMode = expected;
                onDataBindField.Invoke(target, new object[] {txt, null});
                Assert.AreEqual(expected, txt.TextMode);

                expected = TextBoxMode.Password;
                target.TextMode = expected;
                onDataBindField.Invoke(target, new object[] {txt, null});
                Assert.AreEqual(expected, txt.TextMode);

                expected = TextBoxMode.MultiLine;
                target.TextMode = expected;
                onDataBindField.Invoke(target, new object[] {txt, null});
                Assert.AreEqual(expected, txt.TextMode);
            }
        }
    }
}
