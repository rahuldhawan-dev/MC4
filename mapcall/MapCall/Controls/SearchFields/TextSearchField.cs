using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;

namespace MapCall.Controls.SearchFields
{
    public class TextSearchField : BaseSearchField
    {
        #region Fields

        private TextBox _textBox;

        #endregion

        #region Properties

        /// <summary>
        /// Set to true if people should be able to enter * in a textbox
        /// and convert the sql to LIKE % % stuff. If no *'s are in the
        /// textbox value, then an exact match is performed.
        /// </summary>
        public bool AllowWildcardsAndExactMatches { get; set; }

        #endregion

        public override void AddControlsToTemplate(Control template)
        {
            if (_textBox != null)
            {
                throw new InvalidOperationException();
            }

            _textBox = new TextBox();
            _textBox.ClientIDMode = this.ClientIDMode;
            _textBox.ViewStateMode = ViewStateMode.Disabled;
            _textBox.ID = GetControlID();
            template.Controls.Add(_textBox);

            if (Required)
            {
                var req = GetRequiredFieldValidator();
                req.ControlToValidate = _textBox.ID;
                template.Controls.Add(req);
            }
        }

        public override void SetValue(object value)
        {
            _textBox.Text = (value == null ? string.Empty : value.ToString());
        }

        protected override void AddExpressions(IFilterBuilder builder)
        {
            object val;

            if (TryGetValue(out val))
            {
                var dataField = DataFieldName;
                var value = ((string)val).Trim();

                var exp = new FilterBuilderExpression();
                var formattedLeftSide = FilterBuilderParameter.GetFormattedQualifiedFieldName(dataField);
                var formattedRightSide = FilterBuilderParameter.GetParameterizedFormattedName(dataField);

                if (AllowWildcardsAndExactMatches)
                {
                    // If the value dooes not have "*" then 
                    // the search will do a basic "field = value" instead of a like search.
                    if (value.Contains("*"))
                    {
                        if (value == "*")
                        {
                            // This means they wanna search for everything, so we don't wanna add
                            // the expression at all. 
                            return;
                        }

                        value = value.Replace('*', '%');
                        exp.CustomFilterExpression = string.Format("{0} like @{1}", formattedLeftSide, formattedRightSide);
                    }

                }
                else
                {
                    value = "%" + value + "%";
                    exp.CustomFilterExpression = string.Format("{0} like @{1}", formattedLeftSide, formattedRightSide);
                }

                exp.AddParameter(dataField, DbType.String, value);
                builder.AddExpression(exp);

            }
        }

        private bool TryGetValue(out object value)
        {
            var v = _textBox.Text;
            if (!string.IsNullOrWhiteSpace(v))
            {
                // Let's make life a little easier by ignoring accidental trailing spaces

                value = v.Trim();
                return true;
            }
            value = null;
            return false;
        }
    }
}