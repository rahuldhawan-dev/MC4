using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.BoundFieldHelpers
{
    public class BooleanBoundFieldHelper : BoundFieldHelper
    {
        #region Fields

        private readonly BoundFieldHelper _internalHelper;

        #endregion

        #region Properties

        public override BoundField Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                _internalHelper.Owner = value;
            }
        }

        #endregion

        #region Constructors

        public BooleanBoundFieldHelper(BooleanBoundFieldOptions options)
        {
            switch (options.ControlType)
            {
                case BooleanBoundFieldControlTypes.CheckBox:
                    _internalHelper = new CheckBoxBoundFieldHelper();
                    break;
                case BooleanBoundFieldControlTypes.RadioButtonList:
                    _internalHelper = new RadioButtonListBoundFieldHelper(options);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion



        internal override BoundFieldControlHelper CreateEditableControl()
        {
            return _internalHelper.CreateEditableControl();
        }

        public override void OnControlDataBinding(object sender, EventArgs e)
        {
            // Don't call base kthx.
            _internalHelper.OnControlDataBinding(sender, e);
        }

        public override bool ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, bool isEditable, bool includeReadOnly)
        {
            return _internalHelper.ExtractValuesFromCell(dictionary, cell, isEditable, includeReadOnly);
        }

        #region Helper Classes

        private sealed class CheckBoxBoundFieldHelper : BoundFieldHelper
        {
            #region Constants

            // Apparently input tags aren't self-closing. Seriously! 
            private const string FALSE_READONLY_CHECKBOX = @"<input type=""checkbox"" disabled=""disabled"">";
            private const string TRUE_READONLY_CHECKBOX = @"<input type=""checkbox"" disabled=""disabled"" checked=""checked"">";

            #endregion

            internal override BoundFieldControlHelper CreateEditableControl()
            {
                var cb = new CheckBox();
                cb.ID = string.Format("chk{0}", DataField);
                return new BoundFieldControlHelper(cb);
            }

            public override void OnControlDataBinding(object sender, EventArgs e)
            {
                // Don't call the base. We're not using the string formatted value here.
                if (sender is Literal)
                {
                    var lit = (Literal)sender;
                    lit.Text = (GetBoundValue(lit) ? TRUE_READONLY_CHECKBOX : FALSE_READONLY_CHECKBOX);
                }
                else if (sender is CheckBox)
                {
                    var c = (CheckBox)sender;
                    c.Checked = GetBoundValue(c);
                }
                else
                {
                    throw new NotSupportedException();
                }

            }

            public override bool ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, bool isEditable, bool includeReadOnly)
            {
                if (isEditable)
                {
                    var cBox = (CheckBox)cell.Controls[0];
                    if (cBox.Enabled || includeReadOnly) // that read only check is in the asp:CheckBoxField code. 
                    {
                        dictionary[DataField] = cBox.Checked;
                    }
                }
                else if (includeReadOnly)
                {
                    var lit = (Literal)cell.Controls[0];
                    dictionary[DataField] = (lit.Text == TRUE_READONLY_CHECKBOX);
                }

                return true;
            }

            private bool GetBoundValue(Control control)
            {
                var val = GetValue(control);

                if (val == null || val == DBNull.Value) { return false; }

                if (val is bool)
                {
                    return (bool)val;
                }

                throw new NotSupportedException("What type is this?: " + val.GetType().Name);
            }
        }

        private sealed class RadioButtonListBoundFieldHelper : BoundFieldHelper
        {
            #region Consts

            // These are for the RadioButtonList SelectedValue.
            // THESE AREN'T THE DISPLAY VALUES
            private const string NULL_VALUE = "";
            private const string TRUE_VALUE = "True";
            private const string FALSE_VALUE = "False";

            #endregion

            #region Fields

            private readonly BooleanBoundFieldOptions _options;

            #endregion

            public RadioButtonListBoundFieldHelper(BooleanBoundFieldOptions options)
            {
                _options = options;
            }

            internal override BoundFieldControlHelper CreateEditableControl()
            {
                var radical = new RadioButtonList();
                radical.ID = string.Format("rad{0}", DataField);
                radical.RepeatDirection = RepeatDirection.Horizontal; // Needs some css stylin'
                radical.Items.Add(new ListItem(_options.FalseValueText, FALSE_VALUE));
                radical.Items.Add(new ListItem(_options.TrueValueText, TRUE_VALUE));
                radical.Items.Add(new ListItem(_options.NullValueText, NULL_VALUE));
                return new BoundFieldControlHelper(radical);
            }

            public override void OnControlDataBinding(object sender, EventArgs e)
            {
                // Don't call the base. We're not using the string formatted value here.
                if (sender is Literal)
                {
                    var lit = (Literal)sender;

                    var val = GetBoundValue(lit);
                    if (!val.HasValue)
                    {
                        lit.Text = _options.NullValueText;
                    }
                    else if (val.Value)
                    {
                        lit.Text = _options.TrueValueText;
                    }
                    else
                    {
                        lit.Text = _options.FalseValueText;
                    }
                }
                else if (sender is RadioButtonList)
                {
                    var rad = (RadioButtonList)sender;
                    var val = GetBoundValue(rad);
                    if (!val.HasValue)
                    {
                        rad.SelectedValue = NULL_VALUE;
                    }
                    else if (val.Value)
                    {
                        rad.SelectedValue = TRUE_VALUE;
                    }
                    else
                    {
                        rad.SelectedValue = FALSE_VALUE;
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }

            }

            private object ExtractValueFromString(string value)
            {
                if (value == _options.NullValueText || value == NULL_VALUE)
                {
                    return null; // Dunno if this needs to be DBNull.Value instead
                }

                // Using StringComparison when comparing with the consts. This is because the parseed value will return
                // "True" and "False", but the capitalization could change through magic someday. 
                else if (value == _options.TrueValueText || TRUE_VALUE.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (value == _options.FalseValueText || FALSE_VALUE.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                throw new NotSupportedException();
            }

            public override bool ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, bool isEditable, bool includeReadOnly)
            {
                if (isEditable)
                {
                    var rad = (RadioButtonList)cell.Controls[0];
                    if (rad.Enabled || includeReadOnly)
                    {
                        var selected = rad.SelectedValue;
                        dictionary[DataField] = ExtractValueFromString(selected);
                    }
                }
                else
                {
                    if (includeReadOnly)
                    {
                        var lit = (Literal)cell.Controls[0];
                        dictionary[DataField] = ExtractValueFromString(lit.Text);
                    }
                }

                return true;
            }

            private bool? GetBoundValue(Control control)
            {
                var val = GetValue(control);

                if (val == null || val == DBNull.Value) { return null; }

                if (val is bool)
                {
                    return (bool)val;
                }

                throw new NotSupportedException("What type is this?: " + val.GetType().Name);
            }

        }

        #endregion
    }
}