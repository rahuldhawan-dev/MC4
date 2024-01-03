using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.Controls;

// TODO: Come back and use DateTimePicker here so we can get rid of the Ajax Calender crap.

namespace MapCall.Controls.BoundFieldHelpers
{
    public class DateBoundFieldHelper : BoundFieldHelper
    {
        #region Constants

        private const string DATE_FORMAT = "{0:MM/dd/yyyy}";

        #endregion

        #region Properties

        public override string DataFormatString
        {
            get
            {
                if (DataType == SqlDbType.Date)
                {
                    return DATE_FORMAT;
                }
                return base.DataFormatString;
            }
        }

        /// <summary>
        /// Returns whether or not this field should show time along with the date.
        /// </summary>
        private bool ShowTime
        {
            get { return (DataType == SqlDbType.DateTime2 || DataType == SqlDbType.DateTime); }
        }

        #endregion

        #region Private Methods

        #endregion

        // This is directly copied from the Green.cs BoundField, with slight modifications. 
        internal override BoundFieldControlHelper CreateEditableControl()
        {
            var tb = new DateTimePicker();
            tb.ID = String.Format("txt{0}", DataField);
            tb.ShowMonthChangeDropDown = true;
            tb.ShowYearChangeDropDown = true;
            tb.ShowTimePicker = ShowTime;

            var rev = new RegularExpressionValidator();
            BoundField.SetCommonValidatorAttributes(rev, tb);
            rev.ErrorMessage = "Must be a valid date.";
            rev.SetFocusOnError = true;
            rev.ValidationExpression = @"^(?=\d)(?:(?:(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})|(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))|(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2}))($|\ (?=\d)))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$";
            rev.EnableClientScript = true;

            var helper = new BoundFieldControlHelper(tb);
            helper.Controls.Add(rev);

            return helper;
        }

        public override void OnControlDataBinding(object sender, EventArgs e)
        {
            if (sender is Literal)
            {
                //Call base so it can do its normal string formatting stuff.
                base.OnControlDataBinding(sender, e);
            }
            else if (sender is DateTimePicker)
            {
                var dtp = (DateTimePicker)sender;
                var value = GetValue(dtp);
                if (value is DateTime?)
                {
                    dtp.SelectedDate = (DateTime?)value;
                }
                else if (value is string)
                {
                    // If we have a DateTime BoundField, but it's bound to a string, 
                    // the value returned to us is a string. Very annoying!
                    var str = (string)value;
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        dtp.SelectedDate = DateTime.Parse(str);
                    }
                }
            }
        }

        protected override string FormatValue(Control boundControl)
        {
            var value = GetValue(boundControl);
            if (value is string)
            {
                return (string)value;
            }
            else
            {
                var dt = (DateTime?)GetValue(boundControl);
                // Got a null? Don't render anything.
                if (!dt.HasValue) { return string.Empty; }

                var formatted = dt.Value.ToShortDateString();
                if (ShowTime)
                {
                    formatted += " " + dt.Value.ToShortTimeString();
                }
                return formatted;
            }
        }

        public override bool ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, bool isEditable, bool includeReadOnly)
        {
            // Don't call base.

            // For some stupid reason, isEditable will be true if the field is set to ReadOnly
            // when the DetailsViewMode is Edit.
            if (isEditable && !Owner.ReadOnly)
            {
                var dtp = (DateTimePicker)cell.Controls[0];
                if (dtp.Enabled || includeReadOnly) // that read only check is in the asp:CheckBoxField code. 
                {
                    dictionary[DataField] = dtp.SelectedDate;
                }
            }
            else if (includeReadOnly)
            {
                var lit = (Literal)cell.Controls[0];
                var formattedValue = lit.Text;
                if (string.IsNullOrWhiteSpace(formattedValue))
                {
                    dictionary[DataField] = null;
                }
                else
                {
                    dictionary[DataField] = DateTime.Parse(formattedValue);
                }
            }

            return true;

        }
    }
}