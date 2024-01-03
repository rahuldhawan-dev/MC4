using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;

namespace MapCall.Controls
{
    public partial class ddlMcProdOperatingCenter : UserControl, IDataField
    {
        #region Constructor

        public ddlMcProdOperatingCenter()
        {
            ShowDefault = true;
        }

        #endregion

        #region Properties

        public string SelectedValue
        {
            get { return ddlOpCntr.SelectedValue; }
            set { ddlOpCntr.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddlOpCntr.SelectedIndex; }
            set { ddlOpCntr.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddlOpCntr.Width.ToString(); }
            set { ddlOpCntr.Width = Unit.Parse(value); }
        }
        public bool Required
        {
            get { return rfvddlOpCntr.Enabled; }
            set { rfvddlOpCntr.Enabled = value; }
        }
        public ListItemCollection Items
        {
            get { return ddlOpCntr.Items; }
        }

        public string BaseRole { get; set; }

        public bool ShowDefault { get; set; }
        #endregion

        #region Event Handlers

        protected void ddlOpCntr_DataBinding(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            if (ddl.Items.Count == 0 && ShowDefault)
                ddl.Items.Add(new ListItem("--Select Here--", ""));
        }

        protected void ddlOpCntr_DataBound(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;

            //if (string.IsNullOrEmpty(BaseRole))
            //{
            //    ddl.Items.Clear();
            //    ddl.Items.Add(new ListItem("Security is not enabled for this page"));
            //    return;
            //}

        }

        #endregion


        #region IDataField Members

        public string FilterExpression()
        {
            // This is just here to not break backwards compatibility. It would just result in
            // there being two expressions added. 
            return string.Empty;
        }

        public void FilterExpression(IFilterBuilder builder)
        {
            if (String.IsNullOrWhiteSpace(DataFieldName))
            {
                throw new NullReferenceException("A DataMemberName must be set on this control.");
            }

            if (!string.IsNullOrEmpty(this.SelectedValue))
            {
                var exp = new FilterBuilderExpression(DataFieldName, DbType.Int32, this.SelectedValue);
                builder.AddExpression(exp);
            }
        }

        public string DataFieldName { get; set; }

        #endregion
    }
}