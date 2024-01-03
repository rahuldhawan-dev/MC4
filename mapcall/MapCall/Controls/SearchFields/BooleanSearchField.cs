using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;

namespace MapCall.Controls.SearchFields
{
    public enum BooleanSearchType
    {
        CheckBox,
        DropDownList
    }

    public class BooleanSearchField : BaseSearchField
    {
        #region Fields

        private InternalHelper<BooleanSearchField> _helper;

        #endregion

        #region Properties

        private InternalHelper<BooleanSearchField> Helper
        {
            get
            {
                if (_helper == null)
                {
                    switch(SearchType)
                    {
                        case BooleanSearchType.CheckBox:
                            _helper = new CheckBoxHelper(this);
                            break;
                        case BooleanSearchType.DropDownList:
                            _helper = new DropDownHelper(this);
                            break;
                    }
                }
                return _helper;
            }
        }
        public BooleanSearchType SearchType { get; set; }

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


        #region Internal Helper Classes Ahoy!

        private sealed class CheckBoxHelper : InternalHelper<BooleanSearchField>
        {
            private readonly CheckBox _checkBox = new CheckBox();

            public CheckBoxHelper(BooleanSearchField parentOwner) : base(parentOwner)
            {
                _checkBox.ViewStateMode = ViewStateMode.Disabled;
            }

            public override void AddControlsToTemplate(Control template)
            {
                template.Controls.Add(_checkBox);
                _checkBox.ID = Owner.GetControlID();
                _checkBox.ClientIDMode = Owner.ClientIDMode;
            }

            public override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
            {
                // This is for backwards compatibility with DataField. It only applied the
                // filter if the checkbox is checked. 

                var value = _checkBox.Checked;
                if (value)
                {
                    builder.AddExpression(new FilterBuilderExpression(Owner.DataFieldName, DbType.Boolean,
                                                                      value));
                }
            }

            public override void SetValue(object value)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DropDownHelper : InternalHelper<BooleanSearchField>
        {
            private const string NO = "0";
            private const string YES = "1";

            private readonly DropDownList _dropDown = new DropDownList();

            public DropDownHelper(BooleanSearchField parentOwner) : base(parentOwner)
            {
                _dropDown.ViewStateMode = ViewStateMode.Disabled;
                _dropDown.Items.Add(new ListItem("-- Select Here --", string.Empty));
                _dropDown.Items.Add(new ListItem("Yes", YES));
                _dropDown.Items.Add(new ListItem("No", NO));
            }

            public override void AddControlsToTemplate(Control template)
            {
                template.Controls.Add(_dropDown);
                _dropDown.ID = Owner.GetControlID();
                _dropDown.ClientIDMode = Owner.ClientIDMode;
            }

            public override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
            {
                var value = _dropDown.SelectedValue;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                var boolVal = (value == YES ? true : false);
                builder.AddExpression(new FilterBuilderExpression(Owner.DataFieldName, DbType.Boolean,
                                                                  boolVal));

            }

            public override void SetValue(object value)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}