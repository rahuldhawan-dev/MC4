using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Common;
using MMSINC.Utilities.Permissions;
using MapCall.Common;
using MapCall.Controls.Data;
using MapCall.Controls.Documents;
using MapCall.Controls.HR;

namespace MMSINC.Page
{
    public abstract class DataElementPageWithDetailView : DataElementPage
    {
        #region Properties

        public abstract DetailsView DetailView { get; }
        public abstract SqlDataSource DataSource { get; }

        #endregion

        #region Exposed Methods

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            SetVisible(Panels.Detail);
            HideDocuments();
            HideNotes();
            if (DetailView != null)
                DetailView.ChangeMode(DetailsViewMode.Insert);
        }

        protected virtual void btnCancelInsert_Click(object sender, EventArgs e)
        {
            SetVisible(Panels.Search);
        }


        /// <remarks>
        /// 
        /// If you need to cancel an insert to the database, set the e.Cancel property to true here.
        /// Setting the Cancel property on the SqlDataSource's OnInserting event will cancel the command,
        /// but the DetailView will display nothing on the screen after posting back. 
        /// 
        /// </remarks>
        protected virtual void DetailView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            e.Values.CleanValues();
            var dv = (DetailsView)sender;
            var ds =
                (SqlDataSource)
                Utility.GetFirstControlInstance(Page, dv.DataSourceID);
            ds.InsertParameters["CreatedBy"].DefaultValue =
                Page.User.Identity.Name;

        }

        protected virtual void DetailView_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            e.NewValues.CleanValues();
        }

        protected virtual void DetailView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            SetVisible(Panels.Detail);
        }

        /// <summary>
        /// Make sure you're selecting @@IDENTITY INTO THE FIRST INSERT PARAMETER AS OUTPUT.
        /// Look AT SewerManhole.aspx for an example.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            // This exception will otherwise be eaten if we don't throw it, making debugging annoying.
            if (e.Exception != null && !e.ExceptionHandled)
            {
                throw e.Exception;
            }

            var id = int.Parse(e.Command.Parameters[0].Value.ToString());
            DataSource.SelectParameters[0].DefaultValue = id.ToString();
            DetailView.Visible = true;
            DetailView.DataBind();
            DetailView.ChangeMode(DetailsViewMode.ReadOnly);

            var Documents1 = (Documents)Utility.GetFirstControlInstance(Page, "Documents1");
            if (Documents1 != null)
            {
                if (DetailView != null)
                    Documents1.DataLinkID = id;
                Documents1.Visible = true;
            }

            var Notes1 = (Notes)Utility.GetFirstControlInstance(Page, "Notes1");
            if (Notes1 != null)
            {
                if (DetailView != null)
                    Notes1.DataLinkID = id;
                Notes1.Visible = true;
            }

            OnDataSaved(sender, e);
        }

        protected virtual void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            OnDataSaved(sender, e);
        }

        /// <summary>
        /// Method called after the SqlDataSource has either inserted or updated itself.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDataSaved(object sender, SqlDataSourceStatusEventArgs e)
        {
            // For inheritors. Nothing to see here.
        }

        #endregion
    }

    public abstract class DataElementPageWithRoles : DataElementPage
    {

    }


    public class DataElementRolePage : MvpPage
    {
        #region Private Members

        private bool? _canView, _canAdd, _canEdit, _canDelete, _canAdministrate;

        #endregion

        #region Properties

        protected virtual IModulePermissions ModulePermissions
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanView
        {
            get
            {
                if (_canView == null)
                    _canView =
                        IUser.CanRead(ModulePermissions).InAny();
                return _canView.Value || CanAdd || CanEdit || CanDelete || CanAdministrate;
            }
        }

        public bool CanAdd
        {
            get
            {
                if (_canAdd == null)
                    _canAdd = IUser.CanAdd(ModulePermissions).InAny();
                return _canAdd.Value || CanAdministrate;
            }
        }
        public bool CanEdit
        {
            get
            {
                if (_canEdit == null)
                    _canEdit =
                        IUser.CanEdit(ModulePermissions).InAny();
                return _canEdit.Value || CanAdministrate;
            }
        }
        public bool CanDelete
        {
            get
            {
                if (_canDelete == null)
                    _canDelete = IUser.CanDelete(ModulePermissions).InAny();
                return _canDelete.Value || CanAdministrate;
            }
        }
        public bool CanAdministrate
        {
            get
            {
                if (_canAdministrate == null)
                    _canAdministrate = IUser.CanAdministrate(ModulePermissions).InAny();
                return _canAdministrate.Value;
            }
        }

        public string Filter
        {
            get
            {
                if (this.ViewState["Filter"] != null)
                    return this.ViewState["Filter"].ToString();
                else
                    return null;
            }
            set { this.ViewState["Filter"] = value; }
        }

        #endregion
    }

    public class DataElementPage : DataElementRolePage
    {
        #region Event Handlers

        #region Button Click Event Handlers

        protected virtual void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
        protected virtual void btnExport_Click(object sender, EventArgs e)
        {
            SqlDataSource SqlDataSource1 = (SqlDataSource)Utility.GetFirstControlInstance(Page, "SqlDataSource1");
            if (SqlDataSource1 != null)
            {
                SqlDataSource1.FilterExpression = Filter;
                Utility.ExportToExcel(Page, SqlDataSource1);
            }
        }
        protected virtual void btnBackToResults_Click(object sender, EventArgs e)
        {
            SqlDataSource SqlDataSource1 = (SqlDataSource)Utility.GetFirstControlInstance(Page, "SqlDataSource1");
            if (SqlDataSource1 != null)
                SqlDataSource1.FilterExpression = Filter;

            var GridView1 = (GridView)Utility.GetFirstControlInstance(Page, "GridView1");
            if (GridView1 != null)
                GridView1.DataBind();

            Panel pnlSearch = (Panel)Utility.GetFirstControlInstance(Page, "pnlSearch");
            if (pnlSearch != null)
                pnlSearch.Visible = false;

            Panel pnlResults = (Panel)Utility.GetFirstControlInstance(Page, "pnlResults");
            if (pnlResults != null)
                pnlResults.Visible = true;

            Panel pnlDetail = (Panel)Utility.GetFirstControlInstance(Page, "pnlDetail");
            if (pnlDetail != null)
                pnlDetail.Visible = false;

            var Documents1 = (Documents)Utility.GetFirstControlInstance(Page, "Documents1");
            if (Documents1 != null)
            {
                Documents1.DataLinkID = 0;
                Documents1.Visible = false;
            }
            var Notes1 = (Notes)Utility.GetFirstControlInstance(Page, "Notes1");
            if (Notes1 != null)
            {
                Notes1.DataLinkID = 0;
                Notes1.Visible = false;
            }
            var hyperlinks1 =
                (Hyperlinks)
                Utility.GetFirstControlInstance(Page, "Hyperlinks1");
            if (hyperlinks1 != null)
            {
                hyperlinks1.DataLinkID = 0;
                hyperlinks1.Visible = false;
            }
        }
        protected virtual void btnBackToSearch_Click(object sender, EventArgs e)
        {
            Panel pnlSearch = (Panel)Utility.GetFirstControlInstance(Page, "pnlSearch");
            if (pnlSearch != null)
                pnlSearch.Visible = true;

            Panel pnlResults = (Panel)Utility.GetFirstControlInstance(Page, "pnlResults");
            if (pnlResults != null)
                pnlResults.Visible = false;

            Panel pnlDetail = (Panel)Utility.GetFirstControlInstance(Page, "pnlDetail");
            if (pnlDetail != null)
                pnlDetail.Visible = false;
        }
        protected virtual void btnSearch_Click(object sender, EventArgs e)
        {
            Panel pnlSearch = (Panel)Utility.GetFirstControlInstance(Page, "pnlSearch");
            if (pnlSearch != null)
                pnlSearch.Visible = false;

            Panel pnlResults = (Panel)Utility.GetFirstControlInstance(Page, "pnlResults");
            if (pnlResults != null)
                pnlResults.Visible = true;

            Panel pnlDetail = (Panel)Utility.GetFirstControlInstance(Page, "pnlDetail");
            if (pnlDetail != null)
                pnlDetail.Visible = false;

            var GridView1 = (GridView)Utility.GetFirstControlInstance(Page, "GridView1");
            if (GridView1 != null)
            {
                GridView1.PageIndex = 0;
                GridView1.DataBind();
            }

            var Documents1 = (Documents)Utility.GetFirstControlInstance(Page, "Documents1");
            if (Documents1 != null)
                Documents1.DataLinkID = 0;

            var Notes1 = (Notes)Utility.GetFirstControlInstance(Page, "Notes1");
            if (Notes1 != null)
                Notes1.DataLinkID = 0;
            var hyperlinks1 = (Hyperlinks)Utility.GetFirstControlInstance(Page, "Hyperlinks1");
            if (hyperlinks1 != null)
                hyperlinks1.DataLinkID = 0;
        }
        protected virtual void btnAdd_Click(object sender, EventArgs e)
        {
            SetVisible(Panels.Detail);
            HideDocuments();
            HideNotes();
            var hyperlinks1 =
                (Hyperlinks)
                Utility.GetFirstControlInstance(Page, "Hyperlinks1");
            if (hyperlinks1 != null)
            {
                hyperlinks1.DataLinkID = 0;
                hyperlinks1.Visible = false;
            }
            SetDataElementToInsert();
        }

        #endregion

        #region Other Event Handlers

        protected virtual void DataElement1_ItemInserted(object sender, EventArgs e)
        {
            SetVisible(Panels.Detail);

            var DataElement1 = (UserControl.DataElement)Utility.GetFirstControlInstance(Page, "DataElement1");
            var Documents1 = (Documents)Utility.GetFirstControlInstance(Page, "Documents1");
            if (Documents1 != null)
            {
                if (DataElement1 != null)
                    Documents1.DataLinkID = DataElement1.DataElementID;
                Documents1.Visible = true;
            }
            var Notes1 = (Notes)Utility.GetFirstControlInstance(Page, "Notes1");
            if (Notes1 != null)
            {
                if (DataElement1 != null)
                    Notes1.DataLinkID = DataElement1.DataElementID;
                Notes1.Visible = true;
            }
            var hyperlinks1 =
                (Hyperlinks)
                Utility.GetFirstControlInstance(Page, "Hyperlinks1");
            if (hyperlinks1 != null)
            {
                if (DataElement1 != null)
                    hyperlinks1.DataLinkID = DataElement1.DataElementID;
                hyperlinks1.Visible = true;
            }
        }
        protected virtual void Page_Init(object sender, EventArgs e)
        {
            var GridView1 = (GridView)Utility.GetFirstControlInstance(Page, "GridView1");
            GridView1.Sorting += new GridViewSortEventHandler(this.GridView1_Sorting);
            GridView1.PageIndexChanged += new EventHandler(this.GridView1_PageIndexChanged);
        }
        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
        protected void GridView1_Sorting(object sender, EventArgs e)
        {
            ApplyFilter();
        }
        protected void ddl_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("--Select Here--", ""));
        }

        #endregion

        #endregion

        #region Private Methods

        internal void ApplyFilter()
        {
            var SqlDataSource1 = (SqlDataSource)Utility.GetFirstControlInstance(Page, "SqlDataSource1");

            if (!String.IsNullOrEmpty(this.Filter) && SqlDataSource1 != null)
                SqlDataSource1.FilterExpression = Filter;
        }
        
        internal void SetVisible(Panels pnl)
        {
            var pnlSearch = (Panel)Utility.GetFirstControlInstance(Page, "pnlSearch");
            if (pnlSearch != null)
                pnlSearch.Visible = (pnl == Panels.Search);

            var pnlResults = (Panel)Utility.GetFirstControlInstance(Page, "pnlResults");
            if (pnlResults != null)
                pnlResults.Visible = (pnl == Panels.List);

            var pnlDetail = (Panel)Utility.GetFirstControlInstance(Page, "pnlDetail");
            if (pnlDetail != null)
                pnlDetail.Visible = (pnl == Panels.Detail);
        }

        internal void HideDocuments()
        {
            var Documents1 = (Documents)Utility.GetFirstControlInstance(Page, "Documents1");
            if (Documents1 != null)
            {
                Documents1.DataLinkID = 0;
                Documents1.Visible = false;
            }
        }

        internal void HideNotes()
        {
            var Notes1 = (Notes)Utility.GetFirstControlInstance(Page, "Notes1");
            if (Notes1 != null)
            {
                Notes1.DataLinkID = 0;
                Notes1.Visible = false;
            }

        }

        internal void SetDataElementToInsert()
        {
            var DataElement1 = (UserControl.DataElement)Utility.GetFirstControlInstance(Page, "DataElement1");
            if (DataElement1 != null)
            {
                DataElement1.DataElementID = 0;
                DataElement1.ChangeMode(DetailsViewMode.Insert);
            }
        }

        internal void CheckQueryString(DataElement element, Documents documents, Notes notes, Panel pnlDetail, Panel pnlSearch, Panel pnlResults)
        {
            var view = Request.QueryString["view"];
            if (IsPostBack || string.IsNullOrEmpty(view))
                return;

            int id;
            var parsed = int.TryParse(view, out id);

            if (parsed)
            {
                element.DataElementID = documents.DataLinkID = notes.DataLinkID = id;
                pnlDetail.Visible = true;
                pnlSearch.Visible = false;
                pnlResults.Visible = false;
                element.DataBind();
            }
        }

        protected int GetRecordCount(SqlDataSource dataSource)
        {
            var dv = (DataView)dataSource.Select(DataSourceSelectArguments.Empty);
            var count = dv.Count;
            dv.Dispose();
            return count;
        }

        #endregion

        internal enum Panels
        {
            Search,
            List,
            Detail
        }
    }


}