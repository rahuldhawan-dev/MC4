using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.Page;
using MapCall.Controls.HR;

namespace MapCall.Modules.Engineering
{
    public partial class ProjectsRPLookups : DataElementRolePage
    {
        #region Constants

        protected struct LookupKeys
        {
            public const string TABLE_NAME = "TableName",
                                PRIMARY_KEY = "PrimaryKey",
                                DESCRIPTION = "Description";
        }

        protected struct SqlCommands
        {
            public const string SELECT = "Select * FROM [{0}] ",
                                 INSERT = "INSERT INTO [{0}] VALUES(@{1}, @VariableScore, @PriorityWeightedScore);",
                                 UPDATE = "UPDATE [{0}] SET {1} = @{1}, VariableScore = @VariableScore, PriorityWeightedScore = @PriorityWeightedScore WHERE [{2}] = @{2}",
                                 DELETE = "DELETE [{0}] WHERE [{1}] = @{1}";
        }

        private IList<LookupTable> LOOKUP_TABLES = new List<LookupTable> {};
        //    new LookupTable {PrimaryKey = "PipeDiameterID", TableName = "PipeDiameters", Description = "Diameter" }, 
        //    new LookupTable {PrimaryKey = "PipeMaterialID", TableName = "PipeMaterials"}, 
        //    new LookupTable {PrimaryKey = "PipeDecadeID", TableName = "PipeDecades"},
        //    new LookupTable {PrimaryKey = "PipeInternalLiningTypeID", TableName = "PipeInternalLiningTypes"},
        //    new LookupTable {PrimaryKey = "NonBreakComplaintFrequencyID", TableName = "NonBreakComplaintFrequencies"},
        //    new LookupTable {PrimaryKey = "PredominantNonBreakComplaintTypeID", TableName = "PredominantNonBreakComplaintTypes"},
        //    new LookupTable {PrimaryKey = "RepairDifficultyTypeID", TableName = "RepairDifficultyTypes"},
        //    new LookupTable {PrimaryKey = "PredominantBreakTypeID", TableName = "PredominantBreakTypes"},
        //    new LookupTable {PrimaryKey = "PredominantBreakCauseID", TableName = "PredominantBreakCauses"},
        //    new LookupTable {PrimaryKey = "HighCostFactorID", TableName = "HighCostFactors"},
        //    new LookupTable {PrimaryKey = "PressureClassAdequacyID", TableName = "PressureClassAdequacies"},
        //    new LookupTable {PrimaryKey = "MainBreakFrequencyID", TableName = "MainBreakFrequencies"}
        //};

        #endregion
        
        #region Properties

        protected override MMSINC.Utilities.Permissions.IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.Projects; }
        }

        public string TableName
        {
            get 
            { 
                var tableName = ViewState[LookupKeys.TABLE_NAME];
                return (tableName != null) ? tableName.ToString() : string.Empty;

            }
            set { ViewState[LookupKeys.TABLE_NAME] = value; }
        }

        public string PrimaryKey
        {
            get
            {
                var primaryKey = ViewState[LookupKeys.PRIMARY_KEY];
                return (primaryKey != null) ? primaryKey.ToString() : string.Empty;
            }
            set { ViewState[LookupKeys.PRIMARY_KEY] = value; }
        }
        
        public string Description
        {
            get
            {
                var description = ViewState[LookupKeys.DESCRIPTION];
                return (description != null) ? description.ToString() : string.Empty;
            }
            set { ViewState[LookupKeys.DESCRIPTION] = value; }
        }
        
        #endregion

        #region Private Methods

        private void BindTableDropDownList()
        {
            ddlTableName.DataSource = LOOKUP_TABLES;
            ddlTableName.DataTextField = "TableName";
            ddlTableName.DataBind();
        }

        private bool IsValidTable(string tableName)
        {
            return LOOKUP_TABLES.Any(table => table.TableName == tableName);
        }

        private void InitializeDataSource()
        {
            if (!IsValidTable(TableName))
                throw new InvalidOperationException(
                    "An attempt has been made to access a table for editing that isn't allowed");

            dsTable.SelectCommand = String.Format(SqlCommands.SELECT, TableName);
            
            dsTable.UpdateCommand = String.Format(SqlCommands.UPDATE, TableName, Description, PrimaryKey);
            dsTable.UpdateParameters.Clear();
            dsTable.UpdateParameters.Add(new Parameter(PrimaryKey));
            dsTable.UpdateParameters.Add(new Parameter(Description));
            dsTable.UpdateParameters.Add(new Parameter("VariableScore"));
            dsTable.UpdateParameters.Add(new Parameter("PriorityWeightedScore"));

            dsTable.InsertCommand = String.Format(SqlCommands.INSERT, TableName, Description);
            dsTable.InsertParameters.Clear();
            dsTable.InsertParameters.Add(new Parameter(Description));
            dsTable.InsertParameters.Add(new Parameter("VariableScore"));
            dsTable.InsertParameters.Add(new Parameter("PriorityWeightedScore"));

            dsTable.DeleteCommand = String.Format(SqlCommands.DELETE, TableName, PrimaryKey);
            dsTable.DeleteParameters.Clear();
            dsTable.DeleteParameters.Add(new Parameter(PrimaryKey));
        }

        private void InitializeGridView()
        {
            // Assumption made on description field. If it isn't the default Description = "Description"
            // Then we make it a decimal for validation
            gvTable.DataKeyNames = new[] { PrimaryKey };
            gvTable.Columns.Clear();
            dvTable.Fields.Clear();

            gvTable.AutoGenerateEditButton = CanEdit;
            gvTable.AutoGenerateDeleteButton = CanDelete;
            
            gvTable.Columns.Add(new MMSINC.BoundField { DataField = PrimaryKey, HeaderText = PrimaryKey, ReadOnly = true, InsertVisible = false});
            gvTable.Columns.Add(new MMSINC.BoundField { DataField = Description, HeaderText = Description, MaxLength = 50, Required = true, SqlDataType = (Description == "Description") ? SqlDbType.VarChar : SqlDbType.Decimal });
            gvTable.Columns.Add(new MMSINC.BoundField { DataField = "VariableScore", HeaderText = "Variable Score", SqlDataType = SqlDbType.Decimal, Required = true, });
            gvTable.Columns.Add(new MMSINC.BoundField { DataField = "PriorityWeightedScore", HeaderText = "Priority Weighted Score", SqlDataType = SqlDbType.Decimal, Required = true });

            dvTable.Fields.Add(new MMSINC.BoundField { DataField = Description, HeaderText = Description, MaxLength = 50, Required = true, SqlDataType = (Description == "Description") ? SqlDbType.VarChar : SqlDbType.Decimal });
            dvTable.Fields.Add(new MMSINC.BoundField { DataField = "VariableScore", HeaderText = "Variable Score", SqlDataType = SqlDbType.Decimal, Required = true, });
            dvTable.Fields.Add(new MMSINC.BoundField { DataField = "PriorityWeightedScore", HeaderText = "Priority Weighted Score", SqlDataType = SqlDbType.Decimal, Required = true });
            dvTable.Visible = CanAdd;
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BindTableDropDownList();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!CanView)
            {
                pnlAll.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
                return;
            }

            if (IsPostBack && !String.IsNullOrWhiteSpace(TableName))
            {
                InitializeDataSource();
                // we don't want to init if we already have the setup.
                if (!gvTable.DataKeyNames.Contains(PrimaryKey))
                    InitializeGridView();
            }
        }

        protected void btnSelectTable_Click(object sender, EventArgs e)
        {
            // setup which table and fields we're using
            var table = LOOKUP_TABLES.First(x => x.TableName == ddlTableName.SelectedValue);
            if (table == null)
                throw new InvalidOperationException("An attempt has been made to access a table for editing that isn't allowed");

            PrimaryKey = table.PrimaryKey;
            TableName = table.TableName;
            Description = table.Description;

            InitializeDataSource();
            InitializeGridView();
            gvTable.EditIndex = -1;
            gvTable.DataBind();
            dvTable.DataBind();
        }

        protected void gvTable_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            dvTable.Visible = (gvTable.EditIndex <= 0 && gvTable.Rows.Count > 0 && CanAdd);
            
            var deleteButton = (from LinkButton b in e.Row.Cells[0].Controls.OfType<LinkButton>()
                                where b.CommandName == DataControlCommands.DeleteCommandName
                                select b).FirstOrDefault();
            if (deleteButton != null)
                deleteButton.OnClientClick = "return confirm('Are you sure you want to delete the record');";
        }

        #endregion
    }

    public class LookupTable
    {
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string Description { get; set; }
    
        public LookupTable()
        {
            Description = "Description";
        } 
    }
}