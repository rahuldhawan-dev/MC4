using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.BoundFieldHelpers
{

    /// <summary>
    /// Base class for creating helpers that do magic fun things for BoundField.
    /// 
    /// NOTE: This has no support for editable gridviews yet!
    /// </summary>
    public abstract class BoundFieldHelper
    {
        #region Properties

        public bool Required
        {
            get { return Owner.Required; }
        }
        public virtual BoundField Owner { get; set; }
        public string DataField { get { return Owner.DataField; } }
        public SqlDbType DataType { get { return Owner.DataType; } }
        public int MaxLength { get { return Owner.MaxLength; } }

        public virtual string DataFormatString { get { return string.Empty; } }

        #endregion

        #region Private Methods

        internal virtual Control CreateReadOnlyControl()
        {
            return new Literal();
        }

        /// <summary>
        /// Method called when an editable control is required to represent a field. 
        /// </summary>
        /// <returns></returns>
        internal abstract BoundFieldControlHelper CreateEditableControl();

        protected object GetValue(Control control)
        {
            var container = control.NamingContainer;
            var dataItem = DataBinder.GetDataItem(container);
            var value = DataBinder.Eval(dataItem, DataField);

            // Squashing the DBNull.Value here so we don't have to deal with it elsewhere.
            return (value == DBNull.Value ? null : value);
        }

        public virtual void OnControlDataBinding(object sender, EventArgs e)
        {
            // Not checking for type because if something's not ITextControl
            // we're gonna wanna know about it instead of it silently erroring.
            //if (Owner.HtmlEncode)
            //{
                
            //}
            ((ITextControl)sender).Text = FormatValue((Control)sender);
        }

        protected virtual string FormatValue(Control boundControl)
        {
            var value = GetValue(boundControl);
            if (Owner.ApplyFormatInEditMode || !Owner.IsInEditMode)
                return Owner.FormatValue(value, false);
            return (value == null) ? string.Empty : value.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Overriders should return true if they extract a value successfully. 
        /// It's only necessary to override this if there's not a TextBox control being used to store a value.
        /// </summary>
        public virtual bool ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, bool isEditable, bool includeReadOnly)
        {
            return false;
        }

        #endregion
    }

    internal sealed class BoundFieldControlHelper
    {
        // This is needed so a BoundFieldHelper can return a PlaceHolder or some some other parent
        // container that holds multiple controls, but we also need to know which control to put
        // the RequiredFieldValidator on. 
        public readonly Control ValidatableControl;
        public readonly IList<Control> Controls = new List<Control>();

        public BoundFieldControlHelper(Control validatableControl)
        {
            ValidatableControl = validatableControl;
            Controls.Add(validatableControl);
        }

        public void VerifyControlOrder()
        {
            if (Controls.First() != ValidatableControl)
            {
                throw new NotSupportedException(
                    "The first control in the Controls collection must be same as the ValidatableControl for compatibility with System.Web.UI.BoundField");
            }
        }

    }

}