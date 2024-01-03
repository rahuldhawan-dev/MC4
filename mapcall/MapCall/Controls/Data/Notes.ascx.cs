using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Controls;

namespace MapCall.Controls.Data
{
    public partial class Notes : UserControl, IDataLink
    {

        #region Fields

        private bool _allowAdd, _allowEdit, _allowDelete;

        #endregion

        #region Properties

        public int DataLinkID
        {
            get
            {
                if (ViewState["DataLinkID"] == null)
                    return 0;
                return Int32.Parse(ViewState["DataLinkID"].ToString());
            }
            set { ViewState["DataLinkID"] = value.ToString(); }
        }
        public int DataTypeID
        {
            get
            {
                return ViewState["DataTypeID"] == null ? 0 : Int32.Parse(this.ViewState["DataTypeID"].ToString());
            }
            set { ViewState["DataTypeID"] = value.ToString(); }
        }

        public bool AllowDelete
        {
            get { return (bool)ViewState["AllowDelete"]; }
            set { ViewState["AllowDelete"] = value; }
        }

        public bool AllowEdit
        {
            get { return (bool)ViewState["AllowEdit"]; }
            set { ViewState["AllowEdit"] = value; }
        }

        public bool AllowAdd
        {
            get
            {
                return _allowAdd;
            }
            set
            {
                _allowAdd = value;
                btnAddToggle.Visible = value;
            }
        }

        #endregion

        #region Private Methods

        private void loadGridView1()
        {
            SqlDataSource1.SelectParameters["DataTypeID"].DefaultValue =
                DataTypeID.ToString();
            SqlDataSource1.SelectParameters["DataLinkID"].DefaultValue =
                DataLinkID.ToString();
            GridView1.DataBind();
            GridView1.Visible = true;
        }

        #endregion

        #region Protected overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //  GridView1.Columns[0].ItemStyle.CssClass = "NoteCell";
            if (DataLinkID != 0 && DataTypeID != 0)
            {
                //loadGridView1();
            }
            else
            {
                GridView1.Visible = false;
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DataTypeID != 0)
            {
                loadGridView1();
            }
        }

        private static void VerifyDataProperty(string propName, int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(propName + " is invalid. Value must be greater than 0.");
            }
        }

        #endregion

        #region Event Handlers

        protected void btnAddNote_Click(object sender, EventArgs e)
        {
            VerifyDataProperty("DataTypeID", DataTypeID);
            VerifyDataProperty("DataLinkID", DataLinkID);

            var currentUser = DependencyResolver.Current.GetService<IAuthenticationService<User>>().CurrentUser;

            var insertParams = SqlDataSource1.InsertParameters;

            insertParams["Note"].DefaultValue = txtNote.Text;
            insertParams["DataLinkID"].DefaultValue = DataLinkID.ToString();
            insertParams["DataTypeID"].DefaultValue = DataTypeID.ToString();
            insertParams["CreatedBy"].DefaultValue = currentUser.FullName;
            SqlDataSource1.Insert();

            GridView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TODO: Fragile code here. Position instead of direct lookup.
                var ctrl = e.Row.Controls[1];
                var btnDelete = ctrl.FindControl("btnDeleteNote1");
                var btnEdit = ctrl.FindControl("btnEdit");
                if (btnDelete != null) btnDelete.Visible = AllowDelete;
                if (btnEdit != null) btnEdit.Visible = AllowEdit;
            }
        }

        #endregion

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            
        }
    }
}