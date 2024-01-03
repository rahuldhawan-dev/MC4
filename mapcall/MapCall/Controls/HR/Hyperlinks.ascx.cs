using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Controls;

namespace MapCall.Controls.HR
{
    public partial class Hyperlinks : UserControl, IDataLink
    {
        #region Private Members

        private string _userFullName;

        #endregion

        #region Properties

        public int DataLinkID
        {
            get
            {
                return ViewState["DataLinkID"] == null ? 0 : Int32.Parse(ViewState["DataLinkID"].ToString());
            }
            set { ViewState["DataLinkID"] = value.ToString(); }
        }
        public int DataTypeID
        {
            get
            {
                return ViewState["DataTypeID"] == null ? 0 : Int32.Parse(ViewState["DataTypeID"].ToString());
            }
            set { ViewState["DataTypeID"] = value.ToString(); }
        }

        public bool AllowDelete
        {
            get
            {
                return ViewState["AllowDelete"] == null
                           ? false
                           : (bool) ViewState["AllowDelete"];
            }
            set
            {
                ViewState["AllowDelete"] = value;
            }
        }

        public bool AllowEdit { get; set; }

        public bool AllowAdd { get; set; }

        public string UserFullName
        {
            get
            {
                if (String.IsNullOrEmpty(_userFullName))
                {
                    var currentUser = DependencyResolver.Current.GetService<IAuthenticationService<User>>().CurrentUser;
                    _userFullName = currentUser.FullName;
                }
                return _userFullName;
            }
        }

        #endregion

        #region Constructors

        public Hyperlinks()
        {
            AllowAdd = false;
            AllowEdit = false;
            AllowDelete = false;
        }

        #endregion

        #region Private Methods

        private void BindGridView()
        {
            dsHyperlinks.SelectParameters["DataTypeID"].DefaultValue = DataTypeID.ToString();
            dsHyperlinks.SelectParameters["DataLinkID"].DefaultValue = DataLinkID.ToString();
            gvHyperlinks.DataBind();
            gvHyperlinks.Visible = true;
        }

        #endregion

        #region Event Handlers

        protected void btnAddHyperlink_Click(object sender, EventArgs e)
        {
            dsHyperlinks.InsertParameters["CreatedBy"].DefaultValue =
                UserFullName;
            dsHyperlinks.InsertParameters["DataLinkID"].DefaultValue =
                DataLinkID.ToString();
            dsHyperlinks.InsertParameters["DataTypeID"].DefaultValue =
                DataTypeID.ToString();
            dsHyperlinks.Insert();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            
            var btnDelete = Utility.GetFirstControlInstance(e.Row,
                                                                   "btnDeleteHyperlink");
            if (btnDelete != null) btnDelete.Visible = AllowDelete;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAdd.Visible = AllowAdd;
            gvHyperlinks.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (DataTypeID != 0)
            {
                BindGridView();
            }
        }

        #endregion

        protected void dsHyperlinks_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            BindGridView();
            txtLinkURL.Text = "";
            txtLinkText.Text = "";
        }
    }
}