using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Subtext.TestLibrary;

namespace MMSINC.Core.WebFormsTest.ClassExtensions
{
    /// <summary>
    /// Summary description for ControlExtensionsTest
    /// </summary>
    [TestClass]
    public class ControlModeExtensionsTest
    {
        #region System.Web.UI.WebControls.DetailsViewMode

        [TestMethod]
        public void TestDetailsViewModeToMVPDetailViewModeConversion()
        {
            Assert.AreEqual(DetailViewMode.ReadOnly,
                ControlExtensions
                   .ToMVPDetailViewMode(DetailsViewMode.ReadOnly));
            Assert.AreEqual(DetailViewMode.Insert,
                ControlExtensions
                   .ToMVPDetailViewMode(DetailsViewMode.Insert));
            Assert.AreEqual(DetailViewMode.Edit,
                ControlExtensions
                   .ToMVPDetailViewMode(DetailsViewMode.Edit));
        }

        [TestMethod]
        public void TestMVPDetailViewModeToDetailsViewModeConversion()
        {
            Assert.AreEqual(DetailsViewMode.ReadOnly,
                ControlExtensions.ToDetailsViewMode(DetailViewMode.ReadOnly));
            Assert.AreEqual(DetailsViewMode.Insert,
                ControlExtensions.ToDetailsViewMode(DetailViewMode.Insert));
            Assert.AreEqual(DetailsViewMode.Edit,
                ControlExtensions.ToDetailsViewMode(DetailViewMode.Edit));
        }

        #endregion

        #region System.Web.UI.WebControls.FormViewMode

        [TestMethod]
        public void TestFormViewModeToMVPDetailViewModeConversion()
        {
            Assert.AreEqual(DetailViewMode.ReadOnly,
                ControlExtensions.ToMVPDetailViewMode(FormViewMode.ReadOnly));
            Assert.AreEqual(DetailViewMode.Insert,
                ControlExtensions.ToMVPDetailViewMode(FormViewMode.Insert));
            Assert.AreEqual(DetailViewMode.Edit,
                ControlExtensions.ToMVPDetailViewMode(FormViewMode.Edit));
        }

        [TestMethod]
        public void TestMVPDetailViewModeToFormViewModeConversion()
        {
            Assert.AreEqual(FormViewMode.ReadOnly,
                ControlExtensions.ToFormViewMode(DetailViewMode.ReadOnly));
            Assert.AreEqual(FormViewMode.Insert,
                ControlExtensions.ToFormViewMode(DetailViewMode.Insert));
            Assert.AreEqual(FormViewMode.Edit,
                ControlExtensions.ToFormViewMode(DetailViewMode.Edit));
        }

        #endregion
    }

    [TestClass]
    public class GeneralControlExtensionsTest : EventFiringTestClass
    {
        #region Private Members

        private Control _targetControl;
        private IControl _targetIControl;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _targetIControl)
               .DynamicMock(out _targetControl);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestFindControlReturnsResultOfFindControlOnTargetObjectWithGivenIDCastToGivenType()
        {
            var id = "txtFoo";
            var txtFoo = (TextBox)_mocks.CreateMock(typeof(TextBox));

            using (_mocks.Record())
            {
                SetupResult.For(_targetControl.FindControl(id)).Return(txtFoo);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(txtFoo,
                    ControlExtensions.FindControl<TextBox>(_targetControl, id));
            }
        }

        [TestMethod]
        public void TestFindIControlReturnsResultOfFindControlOnTargetObjectWithGivenIDCastToGivenType()
        {
            var id = "txtFoo";
            var txtFoo = (MvpTextBox)_mocks.CreateMock(typeof(MvpTextBox));

            using (_mocks.Record())
            {
                SetupResult.For(_targetControl.FindControl(id)).Return(txtFoo);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(txtFoo,
                    ControlExtensions.FindIControl<ITextBox>(_targetControl, id));
            }
        }

        [TestMethod]
        public void TestIControlFindIControlReturnsResultOfFindControlOnTargetObjectWithGivenIDCastToGivenType()
        {
            var id = "txtFoo";
            var txtFoo = (MvpTextBox)_mocks.CreateMock(typeof(MvpTextBox));

            using (_mocks.Record())
            {
                SetupResult.For(_targetIControl.FindControl(id)).Return(txtFoo);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(txtFoo,
                    ControlExtensions.FindIControl<ITextBox>(_targetIControl, id));
            }
        }
    }

    [TestClass]
    public class ListBoxControlExtensionsTest
    {
        #region Private Members

        private ListBox _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ListBoxControlExtensionsTestInitialize()
        {
            _target = new TestListBoxBuilder();
        }

        #endregion

        #region GetSelectedValue

        [TestMethod]
        public void TestGetSelectedValuesReturnsNullWhenListBoxHasNoItems()
        {
            _target = new TestListBoxBuilder().WithItems(null);

            Assert.IsNull(ControlExtensions.GetSelectedValues(_target, li => li));
        }

        [TestMethod]
        public void TestGetSelectedValuesReturnsNullWhenListBoxHasNoSelectedItems()
        {
            _target = new TestListBoxBuilder().WithItems(new[] {
                new ListItem("Foo"), new ListItem("Bar")
            });

            Assert.IsNull(ControlExtensions.GetSelectedValues(_target, li => li));
        }

        [TestMethod]
        public void TestGetSelectedValuesReturnsOnlySelectedItems()
        {
            var selected = new ListItem("Foo") {
                Selected = true
            };
            var notSelected = new ListItem("Bar") {
                Selected = false
            };
            _target = new TestListBoxBuilder().WithItems(new[] {
                selected, notSelected
            });

            var ret = ControlExtensions.GetSelectedValues(_target, li => li);

            Assert.AreEqual(1, ret.Count);
            Assert.AreSame(selected, ret[0]);
        }

        [TestMethod]
        public void TestGetSelectedValuesReturnsItemsTransformedAsSpecified()
        {
            var values = new[] {
                1, 2, 3
            };
            Func<int, ListItem> makeSelectedItem =
                i => new ListItem(i.ToString()) {
                    Selected = true
                };

            _target = new TestListBoxBuilder().WithItems(new[] {
                makeSelectedItem(values[0]),
                makeSelectedItem(values[1]),
                makeSelectedItem(values[2])
            });

            var ret = ControlExtensions.GetSelectedValues(_target, li => Int32.Parse(li.Value));

            Assert.AreEqual(3, ret.Count);

            for (var i = 0; i < ret.Count; ++i)
            {
                Assert.AreEqual(values[i], ret[i]);
            }
        }

        [TestMethod]
        public void TestGetSelectedValuesReturnsListOfIntByDefault()
        {
            var values = new[] {
                1, 2, 3
            };
            Func<int, ListItem> makeSelectedItem =
                i => new ListItem(i.ToString()) {
                    Selected = true
                };

            _target = new TestListBoxBuilder().WithItems(new[] {
                makeSelectedItem(values[0]),
                makeSelectedItem(values[1]),
                makeSelectedItem(values[2])
            });

            var ret = ControlExtensions.GetSelectedValues(_target);

            Assert.IsInstanceOfType(ret, typeof(List<int>));
            Assert.AreEqual(3, ret.Count);

            for (var i = 0; i < ret.Count; ++i)
            {
                Assert.AreEqual(values[i], ret[i]);
            }
        }

        #endregion
    }

    [TestClass]
    public class DropDownListExtensionsTest
    {
        #region Private Members

        private DropDownList _target;

        #endregion

        #region GetSelectedValue

        [TestMethod]
        public void TestGetSelectedValueReturnsNullWhenDropDownListHasNoItems()
        {
            _target = new TestDropDownListBuilder().WithItems(null);

            Assert.IsNull(ControlExtensions.GetSelectedValue(_target, li => li));
        }

        [TestMethod]
        public void TestGetSelectedValueReturnsSelectedItem()
        {
            var selected = new ListItem("Foo") {
                Selected = true
            };
            var notSelected = new ListItem("Bar") {
                Selected = false
            };
            _target = new TestDropDownListBuilder().WithItems(new[] {
                selected, notSelected
            });

            var ret = ControlExtensions.GetSelectedValue(_target, li => li);

            Assert.AreSame(selected, ret);
        }

        [TestMethod]
        public void TestGetSelectedValueReturnsItemTransformedAsSpecified()
        {
            var expected = 1;
            _target = new TestDropDownListBuilder().WithItems(new[] {
                new ListItem(expected.ToString()) {
                    Selected = true
                }
            });

            var ret = ControlExtensions.GetSelectedValue(_target, li => Int32.Parse(li.Value));

            Assert.AreEqual(expected, ret);
        }

        [TestMethod]
        public void TestGetSelectedValueReturnsNullableIntByDefault()
        {
            var expected = 1;
            _target = new TestDropDownListBuilder().WithItems(new[] {
                new ListItem(expected.ToString())
            });

            var ret = ControlExtensions.GetSelectedValue(_target);

            Assert.AreEqual(expected, ret);
        }

        #endregion

        #region GetBooleanValue

        [TestMethod]
        public void TestGetBooleanValueReturnsTrueWhenSelectedValueIsTruthy()
        {
            _target = new TestDropDownListBuilder().WithItems(new[] {
                new ListItem("Yes"),
                new ListItem("yes"),
                new ListItem("True"),
                new ListItem("true")
            });

            for (int i = 0, len = _target.Items.Count; i < len; ++i)
            {
                _target.Items[i].Selected = true;

                Assert.IsTrue(ControlExtensions.GetBooleanValue(_target).Value);
            }
        }

        [TestMethod]
        public void TestGetBooleanValueReturnsFalseWhenSelectedValueIsFalsey()
        {
            _target = new TestDropDownListBuilder().WithItems(new[] {
                new ListItem("No"),
                new ListItem("no"),
                new ListItem("False"),
                new ListItem("false"),
                new ListItem("not true"),
                new ListItem("sibilance")
            });

            for (int i = 0, len = _target.Items.Count; i < len; ++i)
            {
                _target.Items[i].Selected = true;

                Assert.IsFalse(ControlExtensions.GetBooleanValue(_target).Value);
            }
        }

        [TestMethod]
        public void TestGetBooleanValueReturnsNullWhenSelectedValueIsNullOrEmpty()
        {
            _target = new TestDropDownListBuilder().WithItems(new[] {
                new ListItem(""),
                new ListItem()
            });

            for (int i = 0, len = _target.Items.Count; i < len; ++i)
            {
                _target.Items[i].Selected = true;

                Assert.IsNull(ControlExtensions.GetBooleanValue(_target));
            }
        }

        [TestMethod]
        public void TestGetBooleanValueReturnsNullWhenNoSelectedValue()
        {
            _target = new TestDropDownListBuilder().WithItems(null);

            Assert.IsNull(ControlExtensions.GetBooleanValue(_target));
        }

        #endregion

        #region GetStringValue

        [TestMethod]
        public void TestGetStringValueReturnsValueOfSelectedItem()
        {
            var expected = "Foobar";
            _target = new TestDropDownListBuilder()
               .WithItems(new[] {
                    new ListItem(expected) {Selected = true},
                });

            Assert.AreEqual(expected, ControlExtensions.GetStringValue(_target));
        }

        [TestMethod]
        public void TestGetStringValueReturnsNullWhenNoSelectedItem()
        {
            _target = new TestDropDownListBuilder();

            Assert.IsNull(ControlExtensions.GetStringValue(_target));
        }

        #endregion
    }

    [TestClass]
    public class TextBoxExtensionsTest
    {
        #region Private Members

        private string _value;
        private TextBox _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void TextBoxExtensionsTestInitialize()
        {
            _value = "Foobar";
            _target = new TextBox {
                Text = _value
            };
        }

        #endregion

        #region GetValue

        [TestMethod]
        public void TestGetValueUsesSuppliedFuncToDeriveValue()
        {
            var called = false;
            Func<string, string> fn = str => {
                called = true;
                return str;
            };

            Assert.AreEqual(_value, ControlExtensions.GetValue(_target, fn));
            Assert.IsTrue(called);
        }

        #endregion

        #region GetIntValue and TryGetIntValue

        [TestMethod]
        public void TestGetIntValueReturnsIntegerValueOfTextBoxIfParsable()
        {
            var expected = 42;
            _target = new TextBox {
                Text = expected.ToString()
            };

            Assert.AreEqual(expected, ControlExtensions.GetIntValue(_target));
        }

        [TestMethod]
        public void TestGetIntValueReturnsStringIfTextBoxValueCannotBeParsed()
        {
            _target = new TextBox {
                Text = "not parsable"
            };

            Assert.AreEqual(0, ControlExtensions.GetIntValue(_target));

            _target = new TextBox();

            Assert.AreEqual(0, ControlExtensions.GetIntValue(_target));
        }

        [TestMethod]
        public void TestTryGetIntValueReturnsIntegerValueOfTextBoxIfParsable()
        {
            var expected = 138;
            _target = new TextBox {
                Text = expected.ToString()
            };

            Assert.AreEqual(expected, ControlExtensions.TryGetIntValue(_target).Value);
        }

        [TestMethod]
        public void TestTryGetIntValueReturnsNullWhenTextBoxValueCannotBeParsed()
        {
            _target = new TextBox {
                Text = "can't parse this" // naaaa na na na, na na, na na
            };

            Assert.IsNull(ControlExtensions.TryGetIntValue(_target));

            _target = new TextBox();

            Assert.IsNull(ControlExtensions.TryGetIntValue(_target));
        }

        #endregion

        #region GetDoubleValue and TryGetDoubleValue

        [TestMethod]
        public void TestGetDoubleValueReturnsDoubleValueOfTextBoxIfParsable()
        {
            var gigawatts = 1.21;
            _target = new TextBox {
                Text = gigawatts.ToString()
            };

            Assert.AreEqual(gigawatts, ControlExtensions.GetDoubleValue(_target));
        }

        [TestMethod]
        public void TestGetDoubleValueReturnsZeroWhenTextBoxValueCannotBeParsed()
        {
            _target = new TextBox {
                Text = "Hey!  I'm a number!"
            };

            Assert.AreEqual(0, ControlExtensions.GetDoubleValue(_target));

            _target = new TextBox();

            Assert.AreEqual(0, ControlExtensions.GetDoubleValue(_target));
        }

        [TestMethod]
        public void TestTryGetDoubleValueReturnsDoubleValueOfTextBoxIfParsable()
        {
            var gigawatts = 1.21;
            _target = new TextBox {
                Text = gigawatts.ToString()
            };

            Assert.AreEqual(gigawatts, ControlExtensions.TryGetDoubleValue(_target));
        }

        [TestMethod]
        public void TestTryGetDoubleValueReturnsNullWhenTextBoxValueCannotBeParsed()
        {
            _target = new TextBox {
                Text = "Hey!  I'm a number!"
            };

            Assert.IsNull(ControlExtensions.TryGetDoubleValue(_target));

            _target = new TextBox();

            Assert.IsNull(ControlExtensions.TryGetDoubleValue(_target));
        }

        #endregion

        #region GetDateTimeValue and TryGetDateTimeValue

        [TestMethod]
        public void TestGetDateTimeVlueReturnsDateTimeValueOfTextBoxIfParsable()
        {
            var expected = DateTime.Today;
            _target = new TextBox {
                Text = expected.ToString()
            };

            Assert.AreEqual(expected, ControlExtensions.GetDateTimeValue(_target));
        }

        [TestMethod]
        public void TestGetDateTimeValueReturnsMinimumDateTimeValueIfTextBoxValueNotParsable()
        {
            _target = new TextBox {
                Text = "yesterday"
            };

            Assert.AreEqual(DateTime.MinValue, ControlExtensions.GetDateTimeValue(_target));

            _target = new TextBox();

            Assert.AreEqual(DateTime.MinValue, ControlExtensions.GetDateTimeValue(_target));
        }

        [TestMethod]
        public void TestTryGetDateTimeReturnsDateTimeValueOfTextBoxIfParsable()
        {
            var expected = DateTime.Today;
            _target = new TextBox {
                Text = expected.ToString()
            };

            Assert.AreEqual(expected, ControlExtensions.TryGetDateTimeValue(_target).Value);
        }

        [TestMethod]
        public void TestTryGetDateTimeReturnsNullIfTextBoxNotParsable()
        {
            _target = new TextBox {
                Text = "yesterday"
            };

            Assert.IsNull(ControlExtensions.TryGetDateTimeValue(_target));

            _target = new TextBox();

            Assert.IsNull(ControlExtensions.TryGetDateTimeValue(_target));
        }

        #endregion
    }

    internal class TestListBoxBuilder : TestDataBuilder<ListBox>
    {
        #region Private Members

        private ListSelectionMode _mode = ListSelectionMode.Multiple;
        private ListItem[] _items;

        #endregion

        #region Exposed Methods

        public override ListBox Build()
        {
            var box = new ListBox {
                SelectionMode = _mode
            };
            if (_items != null)
                box.Items.AddRange(_items);
            return box;
        }

        public TestListBoxBuilder WithSelectionMode(ListSelectionMode selectionMode)
        {
            _mode = selectionMode;
            return this;
        }

        public TestListBoxBuilder WithItems(ListItem[] items)
        {
            _items = items;
            return this;
        }

        #endregion
    }

    internal class TestDropDownListBuilder : TestDataBuilder<DropDownList>
    {
        #region Private Members

        private ListItem[] _items;

        #endregion

        #region Exposed Methods

        public override DropDownList Build()
        {
            var ddl = new DropDownList();
            if (_items != null)
                ddl.Items.AddRange(_items);
            return ddl;
        }

        public TestDropDownListBuilder WithItems(ListItem[] items)
        {
            _items = items;
            return this;
        }

        #endregion
    }
}
