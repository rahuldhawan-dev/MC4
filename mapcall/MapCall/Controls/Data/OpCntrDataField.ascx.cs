using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using MMSINC.DataPages;

namespace MapCall.Controls.Data
{
    public partial class OpCntrDataField : UserControl, IDataField
    {
        #region Constants

        public struct SelectCommands
        {
            public const string MCPROD_ALL_OPCNTRS =
                "select OperatingCenterID, OperatingCenterCode + ' - ' + OperatingCenterName as OpCntr from OperatingCenters order by OperatingCenterCode";

            public const string MCPROD_NON_IL_OPCNTRS_ID =
                "select distinct OperatingCenterCode + ' - ' + OperatingCenterName as txt, OperatingCenterID as val from OperatingCenters where charindex('IL', OperatingCenterCode) = 0 order by 1";

            public const string MCPROD_NON_IL_OPCNTRS =
                "select distinct OperatingCenterCode + ' - ' + OperatingCenterName as txt, OperatingCenterCode as val from OperatingCenters where charindex('IL', OperatingCenterCode) = 0 order by 1";

        }

        public struct ServiceMethods
        {
            public const string BY_TEXT = "GetTownsByOperatingCenterText";
            public const string BY_ID = "GetTownsByOperatingCenterID";
        }

        #endregion

        #region Properties

        /// <summary>
        /// True will include the cascading town drop down list
        /// </summary>
        public bool UseTowns { get; set; }

        /// <summary>
        /// This will cause the search to be performed by 'NJ7' instead of 10
        /// </summary>
        public bool UseText { get; set; }

        /// <summary>
        /// Column name we are searching.
        /// </summary>
        public string DataFieldName { get; set; }

        /// <summary>
        /// Column name for searching the towns, used with UseTowns
        /// </summary>
        public string TownDataFieldName { get; set; }

        /// <summary>
        /// Display Text for the Form
        /// </summary>
        public string HeaderText {
            get
            {
                if (String.IsNullOrEmpty(lblHeaderText.Text))
                    lblHeaderText.Text = "OpCntr : ";
                return lblHeaderText.Text;
            }
            set
            {
                lblHeaderText.Text = value;
            }
        }


        /// <summary>
        /// Selected Value for Operating Center
        /// </summary>
        public string SelectedOperatingCenter
        {
            get { return ddlOpCntr.SelectedValue; }
        }

        public string SelectedTown
        {
            get { return ddlTown.SelectedValue; }
        }

        #endregion

        #region Private Methods

        private void SetupControls()
        {
            lblHeaderText.Text = HeaderText;
            if (UseText)
            {
                dsDataField.SelectCommand = SelectCommands.MCPROD_NON_IL_OPCNTRS;
                cddTowns.ServiceMethod = ServiceMethods.BY_TEXT;
            }
            else
            {
                dsDataField.SelectCommand = SelectCommands.MCPROD_NON_IL_OPCNTRS_ID;
                cddTowns.ServiceMethod = ServiceMethods.BY_ID;
            }
            dsDataField.ConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ToString();
            if (UseTowns)
                trTown.Visible = true;
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetupControls();
        }

        #endregion

        #region Exposed Methods
        // TODO: Account for [table].[field]

        public string FilterExpression()
        {
            if (UseTowns && !String.IsNullOrEmpty(ddlTown.SelectedValue))
            {
                return string.Format(" AND [{0}] = '{1}' AND [{2}] = '{3}'",
                                     DataFieldName,
                                     ddlOpCntr.SelectedValue,
                                     TownDataFieldName,
                                     ddlTown.SelectedValue);
            }
            if (!String.IsNullOrEmpty(ddlOpCntr.SelectedValue))
                return String.Format(" AND [{0}] = '{1}'",
                                 DataFieldName,
                                 ddlOpCntr.SelectedValue);
            return string.Empty;

        }

        public void FilterExpression(IFilterBuilder builder)
        {
            if (!String.IsNullOrEmpty(ddlOpCntr.SelectedValue))
                builder.AddExpression(
                    new FilterBuilderExpression(
                        DataFieldName,
                        DbType.String,
                        ddlOpCntr.SelectedValue));

            if (UseTowns && !String.IsNullOrEmpty(ddlTown.SelectedValue))
                builder.AddExpression(
                    new FilterBuilderExpression(
                        TownDataFieldName,
                        DbType.String,
                        ddlTown.SelectedValue));
        }

        #endregion

        protected void foo_DataBinding(object sender, EventArgs e)
        {
            
        }
    }
}