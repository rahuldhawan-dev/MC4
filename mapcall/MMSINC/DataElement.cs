using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.UserControl
{

    public sealed class DataElementRecordSavedEventArgs : EventArgs
    {

        #region Constructors
        public DataElementRecordSavedEventArgs(int recordId)
        {
            _recordId = recordId;
        }

        #endregion

        #region Fields

        private readonly int _recordId;

        #endregion

        #region Properties

        public int RecordId
        {
            get { return _recordId;  }
        }
        #endregion

    }

    /// <summary>
    /// DataElement Controls will have two main controls in them.
    /// 1. DetailsView1
    /// 2. SqlDataSource1
    /// </summary>
    public abstract class DataElement : System.Web.UI.UserControl
    {
    #region Virtual Control Declarations

        private DetailsView m_dvMain;
        private bool m_blDetailsViewSearched = false;
        protected DetailsView DetailsView1
        {
            get
            {
                if (!m_blDetailsViewSearched)
                {
                    m_dvMain = (DetailsView)Utility.GetFirstControlInstance(Page, "DetailsView1");
                    m_blDetailsViewSearched = true;
                }
                return m_dvMain;
            }
        }

        private LinkButton m_btnDelete;
        protected LinkButton btnDelete
        {
            get
            {
                if (m_btnDelete == null)
                    m_btnDelete = (LinkButton)Utility.GetFirstControlInstance(DetailsView1, "btnDelete");
                return m_btnDelete;
            }
        }

        private SqlDataSource m_sdsMain;
        private bool m_blDataSourceSearched = false;
        protected SqlDataSource SqlDataSource1
        {
            get
            {
                if (!m_blDataSourceSearched)
                {
                    m_sdsMain = (SqlDataSource)Utility.GetFirstControlInstanceIn(Page, "SqlDataSource1", "DataElement1");
                    m_blDataSourceSearched = true;
                }
                return m_sdsMain;
            }
            set
            {
                m_sdsMain = value;
                m_blDataSourceSearched = true;
            }
        }

        private Label m_lblResults;
        private bool m_blResultsLabelSearched = false;
        protected Label lblResults
        {
            get
            {
                if (!m_blResultsLabelSearched)
                {
                    m_lblResults = (Label)Utility.GetFirstControlInstance(Page, "lblResults");
                    m_blResultsLabelSearched = true;
                }
                return m_lblResults;
            }
        }

    #endregion

    #region Private Members

        // by default, allow only edit, not new or delete
        private bool m_blAllowNew = false, m_blAllowEdit = true, m_blAllowDelete = false;
        private DataElementLinksMode m_Mode = DataElementLinksMode.Top;

    #endregion

    #region Properties

        /// <summary>
        /// This should be wired to the DataElement_PreInit event. 
        /// Do not use this on PageLoad. It will not work properly.
        /// </summary>
        public bool AllowDelete
        {
            get { return m_blAllowDelete; }
            set { m_blAllowDelete = value; }
        }

        public bool AllowEdit
        {
            get { return m_blAllowEdit; }
            set { m_blAllowEdit = value; }
        }

        public bool AllowNew
        {
            get { return m_blAllowNew; }
            set { m_blAllowNew = value; }
        }

        public DataElementLinksMode LinksMode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        public int DataElementID { get; set; }
        public string DataElementName { get; set; }
        public string DataElementParameterName { get; set; }
        public string DataElementTableName { get; set; }

    #endregion

    #region Constructors

        public DataElement() : base()
        {

        }

    #endregion

    #region Events/Delegates

        public event EventHandler ItemInserted, ItemDeleted;
        public event EventHandler PreInit;
        public event EventHandler<DataElementRecordSavedEventArgs> RecordSaved;

    #endregion

    #region Exposed Methods

        /// <summary>
        /// Legacy. Use ChangeViewMode instead.
        /// </summary>
        /// <param name="dvm"></param>
        public void ChangeMode(DetailsViewMode dvm)
        {
            if (dvm == DetailsViewMode.Insert)
            {
                if (DetailsView1 != null)
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);
            }
        }
        public void ChangeViewMode(DetailsViewMode dvm)
        {
            if (DetailsView1 != null)
                DetailsView1.ChangeMode(dvm);
        }

    #endregion

    #region Event Handlers

        protected virtual void OnPreInit(EventArgs e)
        {
            if (PreInit != null)
                PreInit(this, e);
        }

        protected virtual void OnItemInserted(EventArgs e)
        {
            if (ItemInserted != null)
            {
                ItemInserted(this, e);
            }
        }

        protected virtual void OnItemDeleted(EventArgs e)
        {
            if (ItemDeleted != null)
                ItemDeleted(this, e);
        }

        protected virtual void OnRecordSaved(DataElementRecordSavedEventArgs e)
        {
            if (RecordSaved != null)
            {
                RecordSaved(this, e);
            }
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            // If lblResults previous text is never relevent, this step can
            // be removed entirely by setting lblResults.EnableViewState = false.
            if (lblResults != null)
                lblResults.Text = "";
        }

        protected virtual void DetailsView1_DataBound(object sender, EventArgs e)
        {
            if (DataElementID != 0)
            {
                if (SqlDataSource1 == null)
                    SqlDataSource1 = (SqlDataSource)Utility.GetFirstControlInstanceIn(Page, "SqlDataSource1", "Contract1");
                if (SqlDataSource1!=null)
                    SqlDataSource1.SelectParameters[0].DefaultValue = DataElementID.ToString();
            }
        }

        protected virtual void DetailsView1_PreRender(object sender, EventArgs e)
        {
            var lbtnDelete = Utility.GetFirstControlInstance(DetailsView1, "lbtnDelete");
            if (lbtnDelete != null)
                lbtnDelete.Visible = AllowDelete;
            var lbtnEdit = Utility.GetFirstControlInstance(DetailsView1, "lbtnEdit");
            if (lbtnEdit != null)
                lbtnEdit.Visible = AllowEdit;
        }
        protected virtual void DetailsView1_Canceled(Object sender, EventArgs e)
        {
            // TODO: This function currently just NOOPs.  Is it needed?
        }

        protected virtual void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (HandleSqlException(e, DetailsViewMode.Insert))
                return;

            if (SqlDataSource1 != null)
            {
                string strValue = ((IDbDataParameter)e.Command.Parameters[String.Format("@{0}", DataElementParameterName)]).Value.ToString();
                SqlDataSource1.SelectParameters[0].DefaultValue = strValue;
                Audit.Insert(
                    (int)AuditCategory.DataInsert,
                    Page.User.Identity.Name,
                    String.Format("Added {0}:{1}", DataElementName, strValue),
                    ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );
                if (!String.IsNullOrEmpty(strValue))
                {
                    this.DataElementID = Int32.Parse(strValue);
                    OnItemInserted(e);
                    OnRecordSaved(new DataElementRecordSavedEventArgs(this.DataElementID));
                }
            }
            if (DetailsView1 != null)
            {
                DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
                DetailsView1.DataBind();
            }

            if (lblResults != null)
                lblResults.Text = "Record Inserted";
        }
        protected virtual void SqlDataSource1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (HandleSqlException(e, DetailsViewMode.ReadOnly))
                return;

            if (lblResults != null)
                lblResults.Text = "Record Deleted";

            string strValue = ((IDbDataParameter) e.Command.Parameters[
                                     String.Format("@{0}", DataElementParameterName)]).Value.ToString();
           
            Audit.Insert(
                AuditCategory.DataDelete,
                String.Format("Deleted {0}:{1}", DataElementName, strValue),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );

            if (!String.IsNullOrEmpty(strValue))
            {
                DataElementID = Int32.Parse(strValue);
                OnItemDeleted(e);
            }
        }
        protected virtual void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (HandleSqlException(e, DetailsViewMode.Edit))
                return;

            if (lblResults != null)
                lblResults.Text = "Record Updated";

            Audit.Insert(
                AuditCategory.DataUpdate,
                String.Format("Updated {0}:{1}", DataElementName, ((IDbDataParameter)e.Command.Parameters[String.Format("@{0}", DataElementParameterName)]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );

            OnRecordSaved(new DataElementRecordSavedEventArgs(this.DataElementID));
        }
        protected virtual void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            string strValue = ((IDbDataParameter)e.Command.Parameters[String.Format("@{0}", DataElementName)]).Value.ToString();
            DataElementID = Int32.Parse(strValue);
            Audit.Insert(
                AuditCategory.DataUpdate,
                String.Format("Viewed {0}:{1}", DataElementName, strValue),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }

    #endregion

    #region Private Methods

        /// <summary>
        /// Checks the provided SqlDataSourceStatusEventArgs for a thrown exception.  If an exception
        /// occured, this function which change the DetailsViewMode to the mode value provided.
        /// </summary>
        /// <param name="args">
        /// SqlDataSourceStatusEventArgs that have been passed to an event handler method.
        /// </param>
        /// <param name="destMode">
        /// Mode to enter if an exception was thrown.
        /// </param>
        /// <returns>
        /// A boolean value indicating whether or not an exception was thrown by the Sql command.
        /// </returns>
        private bool HandleSqlException(SqlDataSourceStatusEventArgs args, DetailsViewMode destMode)
        {
            if (args.Exception != null)
            {
                // TODO: This needs to display a "friendly" message somehow.
                // display the error
                lblResults.Text = args.Exception.Message;
                lblResults.CssClass = "ErrorMessage";
                // stop the exception from bubbling
                args.ExceptionHandled = true;
                // hook into the DetailsView to change the mode back to what it
                // was (before the user clicked Update, Delete, Insert, etc.)
                DetailsView1.DataBinding += (object sender, EventArgs e) =>
                {
                    ChangeViewMode(destMode);
                };
                return true;
            }
            return false;
        }

    #endregion
    }

    /// <summary>
    /// Indicates whether a DataElement should draw its item links
    /// at the top or bottom of the table.
    /// </summary>
    public enum DataElementLinksMode
    {
        Top,
        Bottom
    }
}

namespace MMSINC.TemplateFields
{
    public class DropDownListEditTemplate : IBindableTemplate
    {
    #region Private Members

        private string fieldName;
        private DetailsView dv;
        private SqlDataSource sds;
        private DropDownList ddl;

    #endregion

    #region Constructors

        public DropDownListEditTemplate(string fieldName, DetailsView dv, string sqlCommandText, string sqlConnectionString)
        {
            this.fieldName = fieldName;
            this.dv = dv;
            sds = new SqlDataSource(sqlConnectionString, sqlCommandText);
            ddl = new DropDownList();
            ddl.DataTextField = "txt";
            ddl.DataValueField = "val";
            ddl.ID = "dv_ddl" + fieldName;
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add(new ListItem("--Select Here--", ""));
        }

    #endregion

    #region Exposed Methods

        public void InstantiateIn(Control container)
        {
            ddl.DataSource = sds;
            container.Controls.Add(ddl);
            ddl.DataBinding += new EventHandler(this.OnDataBindingDDL);
        }

        public void OnDataBindingDDL(object sender, EventArgs e)
        {
            DropDownList ddlx = (DropDownList)sender;
            if (dv.DataItem != null)
                ddlx.SelectedValue = DataBinder.Eval(dv.DataItem, fieldName).ToString();
        }

        public IOrderedDictionary ExtractValues(Control container)
        {
            OrderedDictionary od = new OrderedDictionary();
            od.Add(fieldName, ddl.SelectedValue);
            //od.Add("ProviderName", ((TextBox)(((GridEditFormItem)(container)).FindControl("MyTextBox"))).Text);
            return od;
        }

    #endregion
    }

    public class DropDownListItemTemplate : ITemplate
    {
    #region Private Members

        private string colname;

    #endregion

    #region Constructors

        public DropDownListItemTemplate(string colname)
        {
            this.colname = colname;
        }

    #endregion

    #region Exposed Methods

        public void InstantiateIn(Control container)
        {
            LiteralControl l = new LiteralControl();
            l.DataBinding += new EventHandler(this.OnDataBinding);
            container.Controls.Add(l);
        }

        public void OnDataBinding(object sender, EventArgs e)
        {
            LiteralControl l = (LiteralControl)sender;
            DetailsView dv = (DetailsView)l.NamingContainer;
            if (dv.DataItem != null)
                l.Text = DataBinder.Eval(dv.DataItem, colname + "_text").ToString();
            else
                l.Text = "Empty DataItem";
        }

    #endregion
    }

    public class LiteralTemplate : ITemplate
    {
    #region Private Members

        private string colname;

    #endregion

    #region Constructors

        public LiteralTemplate(string colname)
        {
            this.colname = colname;
        }

    #endregion

    #region Exposed Methods

        public void InstantiateIn(Control container)
        {
            LiteralControl l = new LiteralControl();
            l.DataBinding += new EventHandler(this.OnDataBinding);
            container.Controls.Add(l);
        }

        public void OnDataBinding(object sender, EventArgs e)
        {
            LiteralControl l = (LiteralControl)sender;
            DetailsView dv = (DetailsView)l.NamingContainer;
            if (dv.DataItem != null)
                l.Text = DataBinder.Eval(dv.DataItem, colname).ToString();
            else
                l.Text = "Empty DataItem";
        }

    #endregion
    }
}
