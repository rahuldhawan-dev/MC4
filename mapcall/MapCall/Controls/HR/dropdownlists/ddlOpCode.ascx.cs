using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlOpCode : UserControl
    {
    #region Properties

        public string SelectedValue
        {
            get { return ddl_OpCode.SelectedValue; }
            set { ddl_OpCode.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_OpCode.SelectedIndex; }
            set { ddl_OpCode.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_OpCode.Width.ToString(); }
            set { ddl_OpCode.Width = Unit.Parse(value); }
        }
        public bool Required
        {
            get { return rfvddlOpCntr.Enabled; }
            set { rfvddlOpCntr.Enabled = value; }
        }

    #endregion

    #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    #endregion
    }
}