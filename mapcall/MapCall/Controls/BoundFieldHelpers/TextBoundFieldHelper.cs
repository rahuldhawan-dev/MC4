using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.BoundFieldHelpers
{
    // TODO: Verify that the value is not longer than the MaxLength
    //       when binding. TextBox does not do this by default!

    // TODO: Trim text values maybe? Would prevent rows from having values like "VALUE" and "VALUE "

    public class TextBoundFieldHelper : BoundFieldHelper 
    {
        public TextBoundFieldOptions Options { get; set; }

        public TextBoundFieldHelper(TextBoundFieldOptions options)
        {
            Options = options;
        }

        internal override BoundFieldControlHelper CreateEditableControl()
        {
            var tb = new TextBox();
            tb.ID = string.Format("txt{0}", DataField);
            tb.MaxLength = MaxLength;

            switch(DataType)
            {
                case SqlDbType.NText:
                case SqlDbType.Text:
                    tb.TextMode = TextBoxMode.MultiLine;
                    break;

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                   // tb.MaxLength = MaxLength;
                    break;

                default:
                    throw new NotSupportedException("TextBoundFieldHelper does not support the DataType: " + DataType);

            }

            if (Options.RequiresMultiLineTextBox)
            {
                tb.TextMode = TextBoxMode.MultiLine;
            }

            return new BoundFieldControlHelper(tb);
        }

        public override void OnControlDataBinding(object sender, EventArgs e)
        {
            // Make sure the base is called as it'll set the initial text value
            // to the control.
            base.OnControlDataBinding(sender, e);

            if (DataType == SqlDbType.NText || DataType == SqlDbType.Text)
            {
                var tc = (ITextControl)sender;
                var text = tc.Text;

                if (sender is TextBox)
                {
                    // Why am I doing this?
                    // -Ross 8/18/2011
                    tc.Text = text;
                }
                else
                {
                    // We don't wanna add a bunch of empty pre tags when there's nothing to
                    // put in them. 
                    if (!string.IsNullOrWhiteSpace(tc.Text))
                    {
                        const string preFormat = "<pre class=\"BoundFieldTextArea\">{0}</pre>";
                        tc.Text = string.Format(preFormat, text);
                    }
                }
            }
        }
    }
}