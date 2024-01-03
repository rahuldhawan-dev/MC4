using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls
{

    // TODO: Move this over to MapCall.Common or MMSINC.Core or something. 
    //       Also, it'll need ViewState/ControlState if it's used in 271
    //       due to the UpdatePanels   
    //
    // TODO: Have this output the same way the html5 input "number" element does.
    //       That way we'll have future-proofed this a bit. Properties it'd need
    //       are min/max and step. Min/max could be used with RangeValidator or something
    //       and maybe step/interval could figure out whether to use integer/decimal?
    //       Be nice if it wasn't a textbox though. 

    public enum NumericTextBoxTypes
    {
        Integer,
        Decimal 
    }

    public interface INumericTextBox : IPostBackDataHandler
    {
        #region Properties

        NumericTextBoxTypes NumericType { get; set; }
        int? IntegerValue { get; set; }
        decimal? DecimalValue { get; set; }

        #endregion
    }

    /// <summary>
    /// It's a numeric textbox!
    /// </summary>
    /// <remarks>
    /// Don't use the Text property directly. You should use the IntegerValue or DecimalValue
    /// properties instead. 
    /// </remarks>
    public class NumericTextBox : TextBox, INumericTextBox
    {
        #region Properties

        private static readonly Dictionary<NumericTextBoxTypes, string> _typeLookup;
        public NumericTextBoxTypes NumericType { get; set; }

        public int? IntegerValue
        {
            get
            {
                if (NumericType != NumericTextBoxTypes.Integer)
                {
                    throw new InvalidOperationException(
                        "NumericType must be set to Integer in order to get IntegerValue");
                }
                int value;
                if (int.TryParse(this.Text, out value))
                {
                    return value;
                }
                return null;
            }
            set {
                this.Text = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }

        public decimal? DecimalValue
        {
            get
            {
                if (NumericType != NumericTextBoxTypes.Decimal)
                {
                    throw new InvalidOperationException(
                        "NumericType must be set to Integer in order to get IntegerValue");
                }
                decimal value;
                if (decimal.TryParse(this.Text, out value))
                {
                    return value;
                }
                return null;
            }
            set {
                this.Text = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }

        #endregion

        #region Constructors

        static NumericTextBox()
        {
            _typeLookup = new Dictionary<NumericTextBoxTypes, string>(); 
            _typeLookup[NumericTextBoxTypes.Integer] = "number integer";
            _typeLookup[NumericTextBoxTypes.Decimal] = "number decimal";
        }

        public NumericTextBox()
        {
            NumericType = NumericTextBoxTypes.Integer;
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Have to use this because Page.ResolveClientUrl will not work if the control
            // is in a boundfield/template field.
            var scriptPath = VirtualPathUtility.ToAbsolute("~/Controls/NumericTextBox.js");
            // Don't use Page.ClientScript.RegisterClientScriptInclude. The script will never be loaded
            // if all the instances of NTB are in an UpdatePanel.
            ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "NumericTextBoxScript", scriptPath);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("rel", _typeLookup[NumericType]);
           
            base.Render(writer);
        }

    }
}