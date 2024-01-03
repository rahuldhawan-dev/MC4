using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;

namespace MapCall.Modules.Contractors
{
    public partial class ContractorInventoriesMaterials : UserControl
    {
        #region Fields

        private readonly ParameterCollection _filterParameters = new ParameterCollection();
        protected SqlDataSource dsResults;
        protected MvpGridView gvResults;
        //private bool? canDelete, canEdit, canAdd;

        #endregion

        #region Properties

        /// <summary>
        /// Any parameters needed for the where clause. They're then added to the SelectParameters. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ParameterCollection FilterParameters { get { return _filterParameters; } }

        public MvpGridView ResultsGridView { get { return gvResults; } }
        public SqlDataSource ResultsDataSource { get { return dsResults; } }

        public bool CanDelete
        {
            get
            {
                if (ViewState["CanDelete"] == null)
                    ViewState["CanDelete"] = false;
                return (bool)ViewState["CanDelete"];
            }
            set
            {
                ViewState["CanDelete"] = value;
            }
        }

        public bool CanEdit
        {
            get
            {
                if (ViewState["CanEdit"] == null)
                    ViewState["CanEdit"] = false;
                return (bool)ViewState["CanEdit"];
            }
            set
            {
                ViewState["CanEdit"] = value;
            }
        }

        public bool CanAdd { 
            get
            {
                if (ViewState["CanAdd"] == null)
                    ViewState["CanAdd"] = false;
                return (bool)ViewState["CanAdd"];
            }
            set
            {
                ViewState["CanAdd"] = value;
            } 
        }

        public string OperatingCenterID
        {
            get
            {
                if (ViewState["OperatingCenterID"] == null)
                    ViewState["OperatingCenterID"] = string.Empty;
                return ViewState["OperatingCenterID"].ToString();
            }
            set
            {
                ViewState["OperatingCenterID"] = value;
            }
        }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Doing it this way instead of requiring the DataSourceId property and junk. 
            ResultsGridView.DataSource = ResultsDataSource;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsPostBack)
            {
                var selectParams = ResultsDataSource.SelectParameters;
                foreach (Parameter p in FilterParameters)
                {
                    selectParams.Add(p);
                }
                DataBind();
            }
        }

        protected void gvResults_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dsResults.DeleteParameters[0].DefaultValue = e.Keys[0].ToString();
            dsResults.Delete();
            gvResults.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // crap to rely on this to add the key
            // but it's the only way right now
            var insertParams = ResultsDataSource.InsertParameters;
            foreach (Parameter p in FilterParameters)
            {
                insertParams.Add(p);
            }
            dsResults.InsertParameters["MaterialID"].DefaultValue = materialPicker.SelectedValue;
            dsResults.InsertParameters["Quantity"].DefaultValue = txtQuantity.Text;
            dsResults.Insert();
            gvResults.DataBind();
        }

        #endregion

        #region Exposed Methods


        #endregion
    }
}