using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls.DropDowns;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCall.Controls;
using NHibernate.Engine;
using StructureMap;

namespace MapCall.Modules.Engineering
{
    public partial class ProjectsRP : TemplatedDetailsViewDataPageBase
    {
        #region Constants

        public struct Parameters
        {
            public const string PROJECT_ID = "@RPProjectID";
        }

        private const string NEW_PROJECT_ADDED_NOTIFICATION_TEMPLATE = "ProjectsRP New Record",
                             PROJECT_COMPLETED_NOTIFICATION_TEMPLATE = "ProjectsRP Completed Record";

        private const int PROJECT_STATUS_COMPLETE = 3;

        public struct Statuses
        {
            public const string SUBMITTED = "1",
                AP_APPROVED = "2",
                COMPLETE = "3",
                CANCELED = "6",
                REVIEWED = "8",
                DUPLICATE = "9",
                MANAGER_ENDORSED = "11",
                AP_ENDORSED = "12",
                MUNICIPAL_RELOCATION_APPROVED = "13",
                PROPOSED = "14";
        }

        #endregion

        #region Private Members

        private decimal variableScore = 0, priorityWeightedScore = 0, variableScoreCount = 0;

        #endregion

        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.Projects; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }
        
        #endregion

        #region Private methods

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            base.AddExpressionsToFilterBuilder(builder);
            opCntrField.FilterExpression(builder);
        }

        protected override void PerformExtendedDataSaving(SqlDataSourceStatusEventArgs e, DataRecordSavingEventArgs savingArgs)
        {
            base.PerformExtendedDataSaving(e, savingArgs);

            if (CurrentDataRecordId <= 0)
            {
                throw new ArgumentOutOfRangeException("CurrentDataRecordId has an invalid value. @RPProjectID parameter must be valid in order to properly save additional information.");
            }

            var deleteCommandOnly = (savingArgs.SaveType == DataRecordSaveTypes.Delete);
            var commands = new Queue<DbCommand>();

            QueueHighCostFactorCommands(commands, deleteCommandOnly);

            while (commands.Any())
            {
                commands.Dequeue().ExecuteNonQuery();
            }

        }

        private void QueueHighCostFactorCommands(Queue<DbCommand> queue, bool deleteOnly)
        {
            var deleteCommand =
                CreateTransactionCommand("DELETE FROM [RPProjectsHighCostFactors] WHERE RPProjectID = @RPProjectID");
            deleteCommand.Parameters.Add(new SqlParameter("@RPProjectID", CurrentDataRecordId));

            queue.Enqueue(deleteCommand);

            if (!deleteOnly)
            {
                var multi = FindCheckBoxList("mcblHighCostFactors");

                foreach (var item in multi.SelectedItems)
                {
                    var comm =
                        CreateTransactionCommand(
                            "INSERT INTO [RPProjectsHighCostFactors] VALUES(@RPProjectID, @HighCostFactorID)");
                    comm.Parameters.Add(new SqlParameter("@RPProjectID", CurrentDataRecordId));
                    comm.Parameters.Add(new SqlParameter("@HighCostFactorID", int.Parse(item.Value)));
                    queue.Enqueue(comm);
                }
            }
        }

        protected override void OnDetailsViewItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            // We need to update the StatusID if we need to update the StatusID
            // We know because it isn't equal to the HidStatusID field
            var ddl = DetailsView.FindIControl<IDropDownList>("ddlStatus");
            if (!String.IsNullOrWhiteSpace(ddl.SelectedValue) 
                    && ddl.SelectedValue != e.NewValues["StatusID"].ToString())
            {
                e.NewValues["StatusID"] = ddl.SelectedValue;
            }

            if (!IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.CapitalPlanning).InAny() &&
                !IUser.IsAdmin())
            {
                e.NewValues["RPProjectRegulatoryStatusId"] =
                    DetailsView.FindControl<MvpHiddenField>("hidRegulatoryStatus").Value;
            }

            base.OnDetailsViewItemUpdating(sender, e);
        }

        protected override void OnRecordSaved(DataRecordSavedEventArgs e)
        {
            if (e.SaveType == DataRecordSaveTypes.Insert)
            {
                SendNotificationEmail(e, NEW_PROJECT_ADDED_NOTIFICATION_TEMPLATE);
            }
            else
            {
                if (e.SaveType != DataRecordSaveTypes.Delete)
                {
                    if (e.GetValue("StatusID") != null && e.GetValue<int>("StatusID") == PROJECT_STATUS_COMPLETE)
                    {
                        if (e.GetOldValue("StatusID") == null || e.GetOldValue<int>("StatusID") != PROJECT_STATUS_COMPLETE)
                        {
                            SendNotificationEmail(e, PROJECT_COMPLETED_NOTIFICATION_TEMPLATE);
                        }
                    }
                }
            }
            base.OnRecordSaved(e);
        }

        private void SendNotificationEmail(DataRecordSavedEventArgs e, string template)
        {
            var tiny = new TinyTown(e.GetValue<int>("TownID"));
            var opCenterId = e.GetValue<int>("OperatingCenterID");
            var model = new ProjectsRPNotificationModel
            {
                Id = e.RecordId,
                AcceleratedAssetInvestmentCategory = GetDescription(e.GetValue<int>("AcceleratedAssetInvestmentCategoryID"), "AssetInvestmentCategories", "AssetInvestmentCategoryID"),
                District = e.GetValue<int?>("District"),
                EstimatedInServiceDate = e.GetValue<DateTime?>("EstimatedInServiceDate"),
                EstimatedProjectDuration = e.GetValue<int?>("EstimatedProjectDuration"),
                FoundationalFilingPeriod = GetDescription(e.GetValue<int?>("FoundationalFilingPeriodID"), "FoundationalFilingPeriods", "FoundationalFilingPeriodID"),
                HistoricProjectID = e.GetValue("HistoricProjectID"),
                NJAWEstimate = e.GetValue<int>("NJAWEstimate"),
                Justification = e.GetValue("Justification"),
                OperatingCenter = GetOperatingCenterCode(opCenterId),
                OriginationYear = e.GetValue("OriginationYear"),
                ProjectDescription = e.GetValue("ProjectDescription"),
                ProjectTitle = e.GetValue("ProjectTitle"),
                ProjectType = GetDescription(e.GetValue<int?>("ProjectTypeID"), "ProjectTypes", "ProjectTypeID"),
                ProposedLength = e.GetValue<int>("ProposedLength"),
                ProposedDiameter = GetDescription(e.GetValue<int?>("ProposedDiameterID"), "PipeDiameters", "PipeDiameterID", "Diameter"),
                ProposedPipeMaterial = GetDescription(e.GetValue<int?>("ProposedPipeMaterialID"), "PipeMaterials", "PipeMaterialID"),
                SecondaryAssetInvestmentCategory = GetDescription(e.GetValue<int?>("SecondaryAssetInvestmentCategoryID"), "AssetInvestmentCategories", "AssetInvestmentCategoryID"),
                Town = tiny.TownName,
                CreatedBy = Page.User.Identity.Name
            };

            DependencyResolver.Current.GetService<INotificationService>()
                .Notify(opCenterId, RoleModules.FieldServicesProjects, template, model);
        }
        
        private string GetOperatingCenterCode(int opCenterId)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var com = conn.CreateCommand())
            {
                conn.Open();
                com.CommandText = "select OperatingCenterCode from OperatingCenters where OperatingCenterID = @OperatingCenterID";
                com.Parameters.AddWithValue("OperatingCenterID", opCenterId);

                using (var reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader["OperatingCenterCode"] as string;
                    }
                }
            }
            return string.Empty;
        }

        private string GetDescription(int? value, string tableName, string primaryKeyFieldName, string textFieldName = "Description")
        {
            if (!value.HasValue) { return string.Empty; }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var com = conn.CreateCommand())
            {
                const string commandFormat = "select {2} from {0} where {1} = @ID";
                com.CommandText = string.Format(commandFormat, tableName, primaryKeyFieldName, textFieldName);
                com.Parameters.AddWithValue("ID", value);

                conn.Open();

                using (var reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var description = reader[textFieldName];
                        if (description != null)
                        {
                            return description.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }

        #endregion

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            Response.Redirect("~/Modules/mvc/ProjectManagement/RecurringProjects/Search");
        }

        protected void DetailsView_OnInit(object sender, EventArgs e)
        {
            if (DetailsView.CurrentMode == DetailsViewMode.Edit)
            {
                if (!IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.CapitalPlanning).InAny() &&
                    !IUser.IsAdmin())
                {
                    DetailsView.FindControl<DataSourceDropDownList>("ddlRegulatoryStatus").Enabled = false;
                }
            }
        }

        /// <summary>
        /// If for some reason this is databound again, make sure we reset the counted values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRPProjectsPipeDataLookupValues_DataBinding(object sender, EventArgs e)
        {
            variableScore = priorityWeightedScore = variableScoreCount = 0;
        }

        protected void gvRPProjectsPipeDataLookupValues_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var format = "N2";
            // Add current rows values to totals
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lblVariableScore = (Label)e.Row.FindControl("lblVariableScore");
                var rowVariableScore = Convert.ToDecimal(lblVariableScore.Text);
                variableScore += rowVariableScore;
                if (rowVariableScore > 0) variableScoreCount += 1;

                var lblPriorityWeightedScore = (Label)e.Row.FindControl("lblPriorityWeightedScore");
                priorityWeightedScore += Convert.ToDecimal(lblPriorityWeightedScore.Text);
            }

            // Set footer row values
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var lblVariableScore = (Label)e.Row.FindControl("lblVariableScore");
                lblVariableScore.Text = (variableScoreCount > 0)
                                            ? (variableScore / variableScoreCount).ToString(format)
                                            : variableScore.ToString(format);

                var lblPriorityWeightedScore = (Label)e.Row.FindControl("lblPriorityWeightedScore");
                lblPriorityWeightedScore.Text = (variableScoreCount > 0)
                    ? (priorityWeightedScore / variableScoreCount).ToString(format)
                    : priorityWeightedScore.ToString(format);

                var lblVariableScoreCount = (Label)e.Row.FindControl("lblVariableScoreCount");
                lblVariableScoreCount.Text = "Estimated Final Scores";
            }

            // Set Delete Confirm
            var deleteButton = (from LinkButton b in e.Row.Cells[0].Controls.OfType<LinkButton>()
                                where b.CommandName == DataControlCommands.DeleteCommandName
                                select b).FirstOrDefault();
            if (deleteButton != null)
                deleteButton.OnClientClick = "return confirm('Are you sure you want to delete the record');";
        }

        protected void dvLookups_Inserting(object sender, DetailsViewInsertEventArgs e)
        {
            e.Values.CleanValues();
            dsRPProjectsPipeDataLookupValues.InsertParameters["PipeDataLookupValueID"].DefaultValue =
                e.Values[0].ToString();
        }

        protected void gvRPProjectsPipeDataLookupValues_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            e.NewValues.CleanValues();
        }

        protected void dvRPProjectEndorsements_Inserting(object sender, DetailsViewInsertEventArgs e)
        {
            dsRPProjectEndorsements.InsertParameters["tblEmployeeID"].DefaultValue = Page.User.Identity.Name;
        }

        /// <summary>
        /// Complete BS that there isn't something built in for this already.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (DetailsView.CurrentMode == DetailsViewMode.Insert)
            {
                var txtOriginationYear = DetailsView.FindControl<MvpTextBox>("txtOriginationDate");
                if (txtOriginationYear != null)
                    txtOriginationYear.Text = DateTime.Now.Year.ToString();
            }
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    var status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                    e.Row.CssClass = status;
                }
            }
        }

        protected void ddlStatus_OnDataBinding(object sender, EventArgs e)
        {
            // maxing out technical debt here -- you sir are a jerk.
            var ddl = (DropDownList)sender;
            if (IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.Projects).InAny() || IUser.IsAdmin())
            {
                ddl.Items.Add(new ListItem {Value = Statuses.SUBMITTED, Text = "Submitted"});
                ddl.Items.Add(new ListItem {Value = Statuses.CANCELED, Text = "Canceled"});
                ddl.Items.Add(new ListItem {Value = Statuses.PROPOSED, Text = "Proposed"});
            }

            if (IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.LocalApproval).InAny() ||
                IUser.IsAdmin())
            {
                ddl.Items.Add(new ListItem {Value = Statuses.MANAGER_ENDORSED, Text = "Manager Endorsed"});
                ddl.Items.Add(new ListItem {Value = Statuses.REVIEWED, Text = "Reviewed"});
            }

            if (IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.AssetPlanningEndorsement).InAny() ||
                IUser.IsAdmin())
                ddl.Items.Add(new ListItem {Value = Statuses.AP_ENDORSED, Text = "AP Endorsed"});

            if (IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.AssetPlanningApproval).InAny() ||
                IUser.IsAdmin())
                ddl.Items.Add(new ListItem {Value = Statuses.AP_APPROVED, Text = "AP Approved"});

            if (IUser.CanEdit(Common.Utility.Permissions.Modules.FieldServices.CapitalPlanning).InAny() ||
                IUser.IsAdmin())
            {
                ddl.Items.Add(new ListItem {Value = Statuses.MUNICIPAL_RELOCATION_APPROVED, Text = "Municipal Relocation Approved"});
            }

            var statusText = DetailsView.FindIControl<IHiddenField>("hidStatus");
            var statusId = DetailsView.FindIControl<IHiddenField>("hidStatusID");

            var selectedListItem = new ListItem(statusText.Value, statusId.Value);
            if (!ddl.Items.Contains(selectedListItem))
            {
                ddl.Items.Add(selectedListItem);
            }
            ddl.SelectedValue = statusId.Value;
            /*

             */
        }

        #endregion

        #region Exposed Methods
        
        protected override string RenderGridViewToExcel(IGridView gv)
        {
            //  gv.AllowSorting = false;
            // Hides the "View" fields since they're useless in an Excel sheet. 
            foreach (DataControlField col in gv.Columns)
            {
                if (!col.Visible)
                {
                    col.Visible = true;
                }
                col.SortExpression = "";
                if (col is CommandField)
                {
                    col.Visible = false;
                }
                if (col is ViewLinkField)
                {
                    col.Visible = false;
                }
            }

            using (var sw = new StringWriter())
            {
                using (var htw = new HtmlTextWriter(sw))
                {
                    gv.RenderControl(htw);
                }
                return sw.ToString();
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// This is a little helper class for getting Town information. Hopefully we can replace this with
        /// a proper ORM someday. Or use the 271 one?
        /// </summary>
        public sealed class TinyTown
        {
            public string CountyName { get; private set; }
            public int District { get; private set; }
            public string State { get; private set; }
            public string TownName { get; private set; }

            public TinyTown(int townId)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
                using (var com = conn.CreateCommand())
                {
                    conn.Open();
                    com.CommandText = "select t.DistrictID, t.Town, c.Name as CountyName, s.Abbreviation from [Towns] t" +
                                      " join [Counties] c on c.CountyID = t.CountyID" +
                                      " join [States] s on s.StateID = t.StateID" +
                                      " where TownID = @TownID";
                    com.Parameters.AddWithValue("TownID", townId);

                    using (var reader = com.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new InvalidOperationException("Invalid TownID(" + townId + ")");
                        }
                        reader.Read();
                        // Doing this Convert.ToInt32 thing because the column is a float 
                        // at the moment for some reason.
                        District = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("DistrictID")));
                        State = reader.GetString(reader.GetOrdinal("Abbreviation"));
                        CountyName = reader.GetString(reader.GetOrdinal("CountyName"));
                        TownName = reader.GetString(reader.GetOrdinal("Town"));
                    }
                }
            }
        }

        #endregion
    }


    public class ProjectsRPNotificationModel
    {
        public int Id { get; set; }
        public string AcceleratedAssetInvestmentCategory { get; set; }
        public int? District { get; set; }
        public DateTime? EstimatedInServiceDate { get; set; }
        public int? EstimatedProjectDuration { get; set; }
        public string FoundationalFilingPeriod { get; set; }
        public string HistoricProjectID { get; set; }
        public string Justification { get; set; }
        public int NJAWEstimate { get; set; }
        public string OperatingCenter { get; set; }
        public string OriginationYear { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectType { get; set; }
        public int ProposedLength { get; set; }
        public string ProposedDiameter { get; set; }
        public string ProposedPipeMaterial { get; set; }
        public string SecondaryAssetInvestmentCategory { get; set; }
        public string Town { get; set; }
        public string CreatedBy { get; set; }
    }
}