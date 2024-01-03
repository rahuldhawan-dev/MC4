using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlEmployees : UserControl
    {
        #region Private Members

        private bool _ActiveOnly;
        private bool _Disabled;

        #endregion

        #region Properties

        public string SelectedValue
        {
            get { return ddl_Employees.SelectedValue; }
            set { ddl_Employees.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_Employees.SelectedIndex; }
            set { ddl_Employees.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_Employees.Width.ToString(); }
            set { ddl_Employees.Width = Unit.Parse(value); }
        }
        public bool ActiveOnly
        {
            get
            {
                return _ActiveOnly;
            }
            set { _ActiveOnly = value;}
        }
        public bool Disabled
        {
            get { return _Disabled; }
            set { _Disabled = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ddl_Employees.DataSourceID = ActiveOnly ? "dsActiveEmployees" : "ds_Employees";
            if (Disabled)
                ddl_Employees.Enabled = false;
        }
    }
}