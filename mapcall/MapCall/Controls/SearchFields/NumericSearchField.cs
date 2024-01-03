using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MapCall.Controls.Data;

namespace MapCall.Controls.SearchFields
{
    public enum NumericSearchType
    {
        Integer,
        Double,
        Range
    }

    public class NumericSearchField : BaseSearchField
    {

        #region Fields

        private InternalHelper<NumericSearchField> _helper;

        #endregion
        #region Properties

        public NumericSearchType SearchType { get; set; }

        private InternalHelper<NumericSearchField> Helper
        {
            get
            {
                if (_helper == null)
                {
                    switch (SearchType)
                    {
                        case NumericSearchType.Double:
                        case NumericSearchType.Integer:
                            _helper = new IntDoubHelper(this);
                            break;
                        case NumericSearchType.Range:
                            _helper = new RangeHelper(this);
                            break;

                    }
                }
                return _helper;
            }
        }

        #endregion

        public override void AddControlsToTemplate(Control template)
        {
            Helper.AddControlsToTemplate(template);
        }

        public override void SetValue(object value)
        {
            Helper.SetValue(value);
        }

        protected override void AddExpressions(IFilterBuilder builder)
        {
            Helper.AddExpressionsToFilterBuilder(builder);
        }


        private class IntDoubHelper : InternalHelper<NumericSearchField>
        {
            #region Fields

            private readonly NumericTextBox _numBox = new NumericTextBox();
            private readonly CompareValidator _validator = new CompareValidator();

            #endregion

            public IntDoubHelper(NumericSearchField parentOwner)
                : base(parentOwner)
            {
            }

            public override void AddControlsToTemplate(Control template)
            {
                _numBox.ClientIDMode = Owner.ClientIDMode;
                _numBox.ViewStateMode = ViewStateMode.Disabled;
                _numBox.ID = Owner.GetControlID();

                _validator.Text = "Please enter a number";
                _validator.ClientIDMode = ClientIDMode.AutoID;
                _validator.ControlToValidate = _numBox.ID;
                _validator.Display = ValidatorDisplay.Dynamic;

                // This has to be Double for both Integer and Decimal field types.
                // If it's set to Integer, you get this error on empty textboxes:
                // "The value '' of the ValueToCompare property of '' cannot be converted to type 'Integer'."
                _validator.Type = ValidationDataType.Double;

                switch (Owner.SearchType)
                {
                    case NumericSearchType.Double:
                        _numBox.NumericType = NumericTextBoxTypes.Decimal;
                        break;
                    case NumericSearchType.Integer:
                        _numBox.NumericType = NumericTextBoxTypes.Integer;
                        break;
                    default:
                        throw new NotSupportedException();
                }

                template.Controls.Add(_numBox);
                template.Controls.Add(_validator);
            }

            public override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
            {
                switch (Owner.SearchType)
                {
                    case NumericSearchType.Double:
                        if (_numBox.DecimalValue.HasValue)
                        {
                            builder.AddExpression(new FilterBuilderExpression(Owner.DataFieldName,
                                                                              DbType.Double,
                                                                              _numBox.DecimalValue));
                        }
                        break;
                    case NumericSearchType.Integer:
                        if (_numBox.IntegerValue.HasValue)
                        {
                            builder.AddExpression(new FilterBuilderExpression(Owner.DataFieldName,
                                                                              DbType.Int32,
                                                                              _numBox.IntegerValue));
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            public override void SetValue(object value)
            {
                switch(Owner.SearchType)
                {
                    case NumericSearchType.Double:
                        if (value == null)
                        {
                            _numBox.DecimalValue = null;
                        }
                        else
                        {
                            _numBox.DecimalValue = Decimal.Parse(value.ToString());
                        }
                        break;
                    case NumericSearchType.Integer:
                        if (value == null)
                        {
                            _numBox.IntegerValue = null;
                        }
                        else
                        {
                            _numBox.IntegerValue = Int32.Parse(value.ToString());
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private class RangeHelper : InternalHelper<NumericSearchField>
        {
            #region Fields

            /// <summary>
            /// Don't call this field directly. use the DateControl property getter. 
            /// This field is lazy-loaded.
            /// </summary>
            private number _numberControl;

            #endregion

            #region Properties

            private number NumberControl
            {
                get
                {
                    if (_numberControl == null)
                    {
                        _numberControl = TemplateControlHelper.CreateControl<number>("~/Controls/Data/number.ascx");
                    }
                    return _numberControl;
                }
            }

            #endregion

            public RangeHelper(NumericSearchField parentOwner)
                : base(parentOwner)
            {
            }

            public override void AddControlsToTemplate(Control template)
            {
                var n = NumberControl;
                n.ID = Owner.GetControlID();
                n.UseBetterControlNamingScheme = true;
                n.SelectedOperatorType = OperatorTypes.Between;
                n.ClientIDMode = Owner.ClientIDMode;
                template.Controls.Add(n);
            }

            public override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
            {
                _numberControl.FilterExpression(builder, Owner.DataFieldName);
            }

            public override void SetValue(object value)
            {
                throw new NotImplementedException();
            }
        }
    }
}