using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.BoundFieldHelpers
{
    public class NumericBoundFieldHelper : BoundFieldHelper
    {
        #region Private Methods

        internal override BoundFieldControlHelper CreateEditableControl()
        {
            var ntb = CreateNumericTextBox();
            var ccv = CreateCompareValidator(ntb);

            var helper = new BoundFieldControlHelper(ntb);
            helper.Controls.Add(ccv);
            return helper;
        }

     
        private CompareValidator CreateCompareValidator(Control c)
        {
            var comp = new CompareValidator();
            BoundField.SetCommonValidatorAttributes(comp, c);
            comp.Operator = ValidationCompareOperator.DataTypeCheck;
            comp.Text = "Invalid number";

            switch (DataType)
            {
                case SqlDbType.Int:
                    comp.Type = ValidationDataType.Integer;
                    break;

                case SqlDbType.BigInt:
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Money:
                    comp.Type = ValidationDataType.Double; // There's no better option
                    break;

                default:
                    throw new NotSupportedException();
            }

            return comp;
        }

        private NumericTextBox CreateNumericTextBox()
        {
            var ntb = new NumericTextBox();
            ntb.ID = string.Format("num{0}", DataField); 
            
            switch(DataType)
            {
                case SqlDbType.Int:
                    ntb.NumericType = NumericTextBoxTypes.Integer;
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    ntb.NumericType = NumericTextBoxTypes.Decimal;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return ntb;
        }
        #endregion
    }
}