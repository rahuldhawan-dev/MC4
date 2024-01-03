using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.Controls;
using MMSINC.UserControl;
using DropDownListEditTemplate = MMSINC.TemplateFields.DropDownListEditTemplate;
using DropDownListItemTemplate = MMSINC.TemplateFields.DropDownListItemTemplate;

//using MapCall.Controls.Maps;

namespace MapCall.Controls.HR
{
    /// <summary>
    /// This is the scaffold table.
    /// 
    /// TODO: Still missing some classes; table, foreignkey, etc.
    /// TODO: Coordinates - If you update a coordinate it simply creates a new, leaves the old one orphaned.
    /// </summary>
    [ControlBuilder(typeof(DataElementBuilder))]
    [ParseChildren(false)]
    public partial class DataElement : MMSINC.UserControl.DataElement
    {
    #region Constants

        private struct QueryFormatStrings
        {
            /// <summary>
            /// Requires 1 value, the name of the table from which to select.
            /// </summary>
            public const string SELECT_TOP_ONE_STAR     = "SELECT TOP 1 * FROM [{0}];";
            /// <summary>
            /// Requires 2 values, the column (or column list), and the table name.
            /// </summary>
            public const string SELECT_TOP_ONE_LIST     = "SELECT TOP 1 {0} FROM [{1}];";
            /// <summary>
            /// Requires 2 values, a string based query that affects the @@IDENTITY environment
            /// variable, and the name of the variable to set it to.
            /// </summary>
            public const string RUN_AND_GET_IDENTITY    = "{0};SELECT @{1} = @@IDENTITY;";
            /// <summary>
            /// Requires 2 values, the name of the LookupType to gather, and the name of the
            /// current DataElementTable.  This query will only return lookup values where
            /// Lookup.TableName is null, or where it matches the current DataElementTableName.
            /// </summary>
            public const string GET_LOOKUP_ITEM         = "SELECT [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = '{0}' AND ([TableName] = '{1}' OR [TableName] IS NULL) ORDER BY 2";
        }

        private const string DEFAULT_SQL_CONNECTION = "MCProd";

    #endregion

        #region Control Declarations

        protected internal MvpDetailsView DetailsView1;
        protected internal SqlDataSource SqlDataSource1;

        #endregion

        #region Private Members

        private bool testing = true;
        private bool _showKey;
        private string m_strSelectCommand;
        private SqlDataAdapter m_sdAdapter;
        private SqlCommandBuilder m_scBuilder;
        private SqlCommand m_scInsertCommand, m_scDeleteCommand, m_scUpdateCommand;
        private DataRowCollection m_drcForeignKeys;
        private bool m_blForeignKeysGathered = false;
        private ArrayList m_alElementFields;
        private string m_ConnectionString;
    #endregion

    #region Properties

        private string SelectCommandString
        {
            get
            {
                if (m_strSelectCommand == null)
                    m_strSelectCommand = GenerateSelectCommandString();
                return m_strSelectCommand;
            }
        }

        private SqlDataAdapter DataAdapter
        {
            get
            {
                if (m_sdAdapter == null)
                    m_sdAdapter = new SqlDataAdapter(SelectCommandString,
                        ConnStringSettings.ToString());
                return m_sdAdapter;
            }
        }
        private SqlCommandBuilder CommandBuilder
        {
            get
            {
                if (m_scBuilder == null)
                {
                    m_scBuilder = new SqlCommandBuilder(DataAdapter)
                                      {
                                          ConflictOption = ConflictOption.OverwriteChanges
                                      };
                }
                return m_scBuilder;
            }
        }

        private SqlCommand InsertCommand
        {
            get
            {
                // Is this throwing an error saying your db doesn't exist or is invalid?
                // Make sure the ConnectionString property is set to the correct value because
                // it defaults to MCProd.
                if (m_scInsertCommand == null)
                    m_scInsertCommand = CommandBuilder.GetInsertCommand(true);
                return m_scInsertCommand;
            }
        }
        private SqlCommand DeleteCommand
        {
            get
            {
                if (m_scDeleteCommand == null)
                    m_scDeleteCommand = CommandBuilder.GetDeleteCommand(true);
                return m_scDeleteCommand;
            }
        }
        private SqlCommand UpdateCommand
        {
            get
            {
                if (m_scUpdateCommand == null)
                    m_scUpdateCommand = CommandBuilder.GetUpdateCommand(true);
                return m_scUpdateCommand;
            }
        }

        private DataRowCollection ForeignKeys
        {
            get
            {
                if (!m_blForeignKeysGathered)
                {
                    m_drcForeignKeys = GatherForeignKeys();
                    m_blForeignKeysGathered = true;
                }
                return m_drcForeignKeys;
            }
        }

        public ArrayList ElementFields
        {
            get
            {
                if (m_alElementFields == null)
                    m_alElementFields = new ArrayList();
                return m_alElementFields;
            }
        }

        public bool ShowKey
        {
            get { return _showKey; }
            set { _showKey = value; }
        }

        // TODO: This is confusing. The property it's looking for is a database name, not an actual connection string.
        //       This should be renamed or set to be an actual connection string. 
        public string ConnectionString
        {
            get
            {
                if (m_ConnectionString==null)
                    m_ConnectionString = DEFAULT_SQL_CONNECTION;
                return m_ConnectionString;
            }
            set { m_ConnectionString = value; }
        }

        public ConnectionStringSettings ConnStringSettings
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionString];
            }
        }

    #endregion

    #region Constructors

        // Look up :DataElementField. Chat logs. Word.

        private bool _isDisposed;
        public override void Dispose()
        {
            base.Dispose();

            try
            {
                if (!_isDisposed) {
                    // I love this, because the property getter creates the object,
                    // and then we dispose it. -Ross 5/17/2011
                    if (CommandBuilder != null) { CommandBuilder.Dispose(); }
                    if (DataAdapter != null) { DataAdapter.Dispose(); }
                }
            }
            finally
            {
                _isDisposed = true;
            }

        }
    #endregion

    #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // This fun method comes from the MMSINC.Controls.DataElement class. 
            // I'm calling this after OnInit because originally this wasn't called
            // until the Init event handler was called. 
            //
            // -Ross 11/18/10
            base.OnPreInit(e);
            GatherDetailsViewFields();
            SetupDataSource();
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            DetailsView1.DataBind();
        }

    #endregion

    #region Private Methods

        private string GenerateSelectWithJoins()
        {
            string[] tmpFieldText;
            int lookupCounter = 1;
            StringBuilder sb = new StringBuilder();
            
            if (ForeignKeys != null && testing)
            {
                //0 Field, 1 Table, 2 fTable, 3 fField, 4 fField2
                sb.AppendFormat("SELECT Top 1 [{0}].*", DataElementTableName);
                foreach (DataRow dr in ForeignKeys)
                {
                    //Special Lookup Type - It is from the Lookup table, use the 3rd field as the value field
                    if (dr[2].ToString() == "Lookup")
                    {
                        sb.AppendFormat(", #{0}{1}.[LookupValue] as [{2}_text]", dr[2], lookupCounter, dr[0]);
                    }
                    else
                    {
                        //// HERE 
                        if (dr[4].ToString().Contains(","))
                        {
                            tmpFieldText = dr[4].ToString().Split(',');
                            sb.Append(", ");
                            for(int x=0;x<tmpFieldText.Length;x++)
                            {
                                if(x==tmpFieldText.Length-1)
                                    sb.AppendFormat("cast(#{0}{1}.[{2}] as varchar(255))", dr[2], lookupCounter, tmpFieldText[x]);
                                else
                                    sb.AppendFormat("cast(#{0}{1}.[{2}] as varchar(255)) + ', ' + ", dr[2], lookupCounter, tmpFieldText[x]);
                            }
                            sb.AppendFormat(" as [{0}_text]", dr[0]);
                        }
                        else
                            sb.AppendFormat(", #{0}{1}.[{2}] as [{3}_text]", dr[2], lookupCounter, dr[4], dr[0]);
                    }
                    lookupCounter++;
                }
                sb.AppendFormat("from [{0}] ", DataElementTableName);
                lookupCounter = 1;
                foreach (DataRow dr in ForeignKeys)
                {
                    sb.AppendFormat(" LEFT JOIN [{0}] as #{0}{1} on #{0}{1}.[{2}] = [{3}].[{4}]", dr[2], lookupCounter, dr[3], dr[1], dr[0].ToString());
                    lookupCounter++;
                }
                //This is the query to get the first field - select top 1 name from syscolumns where id in (select id from sysobjects where name = 'tblPWSID') and offset <> 2
                sb.AppendFormat(" Where [{0}].[{1}] = @DataElementID", DataElementTableName, DataElementParameterName, DataElementID);
            }
            else
            {
                sb.AppendFormat("SELECT Top 1 * from [{0}] ", DataElementTableName);
                sb.AppendFormat("{0} Where [{1}].[{2}] = @DataElementID", SelectCommandString, DataElementTableName, DataElementParameterName, DataElementID);
            }
            return sb.ToString();
        }

        private string GenerateSelectWithoutJoins()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" SELECT TOP 1 * FROM [{0}]", DataElementTableName);
            sb.AppendFormat(" WHERE [{0}].[{1}] = @DataElementID", DataElementTableName, DataElementParameterName, DataElementID);
            return sb.ToString();
        }

        /// <summary>
        /// Returns SP- getForeignKeyNames
        /// 0 - FieldName, 1 - Main TableName, 2 - Foreign TableName, 3-ForeignKeyID, 4-ForeignText Field(2nd field in foreign table)
        /// </summary>
        /// <returns></returns>
        private DataRowCollection GatherForeignKeys()
        {
            using (var sds = new SqlDataSource(ConnStringSettings.ToString(), "getForeignKeyNames"))
            {
                sds.SelectParameters.Add(new Parameter("tblName", TypeCode.String, DataElementTableName));
                sds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
                try
                {
                    using (var dv = (DataView)sds.Select(DataSourceSelectArguments.Empty))
                    {
                        // This can probably just use dv.Count instead, though not sure.
                        if (dv != null && dv.Table.Rows.Count > 0)
                        {
                            var drc = dv.Table.Rows;
                            return drc;
                        }
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private void AddManyToManyField(IDataElementField def)
        {
            var mtmf = new ManyToManyField(def);
            DetailsView1.Fields.Add(mtmf);
        }

        private void AddField(string strFieldName, string strHeaderName, string strCssClass, string strFilter, SqlDbType dbType, bool isRequiredField)
        {
            string[] tmpFieldText;
            var tmpFieldString = new StringBuilder();
            var isForeignKey = false;
            System.Web.UI.WebControls.BoundField bf;

            // If the current field is a foreign key //
            if (ForeignKeys != null)
            {
                foreach (DataRow dr in ForeignKeys)
                {
                    if (dr[0].ToString() == strFieldName)      // WE HAVE A FOREIGN KEY - ADD THE CUSTOM TEMPLATE FIELD
                    {
                        var tf = new TemplateField();
                        // should map to ControlStyle-CssClass
                        if (strCssClass != null)
                            tf.ControlStyle.CssClass = strCssClass;
                        tf.HeaderText = strHeaderName;
                        String sqlcommand;
                        //Special Lookup Type - It is from the Lookup table, use the 3rd field [LookupValue] as the value field
                        if (dr[2].ToString() == "Lookup")
                        {
                            sqlcommand = String.Format(QueryFormatStrings.GET_LOOKUP_ITEM, dr[0], DataElementTableName);
                        }
                        else if (strFieldName.Equals("coordinateid", StringComparison.OrdinalIgnoreCase))
                        {
                            // TODO: this is a bit sloppy.
                            DetailsView1.Fields.Add(new LatLonPickerField(ConnectionString));
                            isForeignKey = true;
                            continue;
                        }
                        else
                        {
                            //// HERE - place hack here.
                            tmpFieldString.Length = 0;
                            if (dr[4].ToString().Contains(","))                 //IF WE HAVE A COMMA SEPARATED LIST OF FIELDS
                            {
                                tmpFieldText = dr[4].ToString().Split(',');     // Throw into an array
                                for (var x = 0; x < tmpFieldText.Length; x++)   // loop
                                {
                                    if (x == tmpFieldText.Length - 1)           // handle last field
                                        tmpFieldString.AppendFormat("isNull(cast([{0}] as varchar(255)),'')", tmpFieldText[x]);
                                    else                                        // handle other fields
                                        tmpFieldString.AppendFormat("isNull(cast([{0}] as varchar(255)),'') + ', ' + ", tmpFieldText[x]);
                                }
                                // this adds extra filters to lookups
                                sqlcommand = (strFilter == null) ? String.Format("Select [{0}] as val, cast({1} as varchar(2000)) as txt from [{2}] order by {3}", dr[3], tmpFieldString, dr[2], dr[4]) :
                                    String.Format("Select [{0}] as val, cast({1} as varchar(2000)) as txt from [{2}] WHERE {3} order by {4}", dr[3], tmpFieldString, dr[2], strFilter, dr[4]);
                            }
                            else
                                sqlcommand = String.Format("Select [{0}] as val, cast([{1}] as varchar(2000)) as txt from [{2}] order by 2", dr[3], dr[4], dr[2]);
                        }

                        tf.ItemTemplate = new DropDownListItemTemplate(strFieldName);
                        tf.EditItemTemplate = new DropDownListEditTemplate(strFieldName, DetailsView1, sqlcommand, ConnStringSettings.ToString());
                        DetailsView1.Fields.Add(tf);

                        //Set this so we don't add the field again below.
                        isForeignKey = true;
                    }
                }
            }

            if (!isForeignKey)
            {
                switch (dbType)
                {
                    case SqlDbType.Bit:
                        // This shouldn't required a required field validation. There's always gonna be a value.
                        bf = new CheckBoxField();
                        break;
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.DateTime:
                        bf = new MMSINC.BoundField()
                        {
                            SqlDataType = SqlDbType.DateTime,
                            DataFormatString = "{0:d}",
                            HtmlEncode = false,
                            Required = isRequiredField 
                        };
                        break;
                    case SqlDbType.DateTime2:
                        bf = new MMSINC.BoundField()
                        {
                            SqlDataType = dbType,
                            DataFormatString = "{0:g}",
                            Required = isRequiredField 
                        };
                        break;
                    case SqlDbType.Text:
                    case SqlDbType.NVarChar:
                       var bField = new MMSINC.BoundField() 
                        {
                            SqlDataType = dbType,
                            TextMode = TextBoxMode.MultiLine,
                            Required = isRequiredField 
                        };
                        bf = bField;
                        break;
                    default:
                        bf = new MMSINC.BoundField();
                        break;
                }
                bf.DataField = strFieldName;
                bf.HeaderText = strHeaderName;
                if (strCssClass != null)
                    bf.ControlStyle.CssClass = strCssClass;
                DetailsView1.Fields.Add(bf);
            }
        }

        private void AddField(string strFieldName, SqlDbType dbType, bool isRequired)
        {
            AddField(strFieldName, strFieldName, null, null, dbType, isRequired);
        }

        private void AddField(DataElementField def)
        {
            if (!def.ManyToMany)
                AddField(def.DataFieldName, def.HeaderName, def.ControlCssClass, def.Filter, def.Type, def.Required);
            else
                AddManyToManyField(def);
        }

        private void GatherDetailsViewFields()
        {
            //Set DataKey
            if (DetailsView1.DataKeyNames.Length == 0)
                DetailsView1.DataKeyNames = new string[] { DataElementParameterName };

            //Set DetailsView1 Fields
            if (DetailsView1.Fields.Count == 0)
            {
                if (LinksMode == DataElementLinksMode.Top) {
                    AddLinkButtonFields();
                }
                
                if (ShowKey) {
                    //AddField(DetailsView1.DataKeyNames[0], SqlDbType.Int);
                    var keyfield = DetailsView1.DataKeyNames[0];
                    DetailsView1.Fields.Add(new MMSINC.BoundField
                                                {
                                                    DataField = keyfield,
                                                    HeaderText = keyfield,
                                                    SortExpression = keyfield,
                                                    InsertVisible = false,
                                                    ReadOnly = true
                                                });
                }
                if (ElementFields.Count == 0)
                {
                    //Loop through the parameters in the InsertCommand.Parameters collection
                    foreach (SqlParameter sp in InsertCommand.Parameters) {
                        AddField(sp.ParameterName.TrimStart('@'), sp.SqlDbType, sp.IsNullable);
                    }
                }
                else
                {
                    // TODO: Have Jason explain to me how this gathers these fields from the html
                    // This works because of the Attributes [ControlBuilder()] and [ParseChildren(false)]
                    // for this class.  The override method AddParsedSubObject() actually does the work.
                    // The class DataElementBuilder below this one helps define the object types that are
                    // passed up when the UML is parsed by Asp.NET.  The specific object type in question
                    // is MapCall.Controls.HR.DataElementField, which is a WebControl.  So, put simply,
                    // this doesn't gather the fields from the UML, because they've already been gathered
                    // by this point and stuck into the ArrayList ElementFields.
                    foreach (DataElementField field in ElementFields)
                        AddField(field);
                        //AddField(field.DataFieldName, field.HeaderName, field.ControlCssClass, field.Type);
                }

                if (LinksMode == DataElementLinksMode.Bottom)
                    AddLinkButtonFields();
            }
        }

        private void AddLinkButtonFields()
        {
            DetailsView1.Fields.Add(new LinksTemplateField(AllowEdit, AllowNew, AllowDelete));
        }

        private void SetupDataSource()
        {
            ConnectionStringSettings cs = ConnStringSettings;
            SqlDataSource1.ConnectionString = cs.ToString();
            SqlDataSource1.ProviderName = cs.ProviderName;
            //Wire up events - these are defined in the dataelement.cs file.
            SqlDataSource1.Deleted += SqlDataSource1_Deleted;
            SqlDataSource1.Updated += SqlDataSource1_Updated;
            SqlDataSource1.Inserted += SqlDataSource1_Inserted;

            //foreignKeys != null then we have to add the joins and _text columns
            SqlDataSource1.SelectCommand = (ForeignKeys != null ? GenerateSelectWithJoins() : GenerateSelectWithoutJoins());

            if (SqlDataSource1.UpdateCommand.Length == 0)
                SqlDataSource1.UpdateCommand = UpdateCommand.CommandText.Replace("@Original_", "@");
            SqlDataSource1.DeleteCommand = DeleteCommand.CommandText.Replace("@Original_", "@");
            SqlDataSource1.InsertCommand = string.Format(QueryFormatStrings.RUN_AND_GET_IDENTITY,
                InsertCommand.CommandText, DataElementParameterName);

            SqlDataSource1.SelectParameters.Add(new Parameter("DataElementID", TypeCode.Int32));
            SqlDataSource1.SelectParameters[0].DefaultValue = DataElementID.ToString();

            if (SqlDataSource1.UpdateParameters.Count == 0)
            {
                foreach (SqlParameter sp in UpdateCommand.Parameters)
                {
                    SqlDataSource1.UpdateParameters.Add(new Parameter(
                        new Regex("@(?:Original_)?").Replace(sp.ParameterName, "")));
                }
            }

            foreach (SqlParameter sp in DeleteCommand.Parameters)
            {
                SqlDataSource1.DeleteParameters.Add(new Parameter(sp.ParameterName.Replace("@", ""), sp.SqlDbType.GetTypeCode()));
            }

            foreach (SqlParameter sp in InsertCommand.Parameters)
            {
                SqlDataSource1.InsertParameters.Add(new Parameter(sp.ParameterName.Replace("@", "")));
            }
            var p = new Parameter(DataElementParameterName, TypeCode.Int32);
            p.Direction = ParameterDirection.Output;
            SqlDataSource1.InsertParameters.Add(p);
        }

        private string GenerateSelectCommandString()
        {
            if (ElementFields.Count == 0)
                return string.Format(QueryFormatStrings.SELECT_TOP_ONE_STAR, DataElementTableName);

            var sbColumns = new StringBuilder();
            foreach (DataElementField field in ElementFields)
                if (!field.ManyToMany)
                    sbColumns.AppendFormat("[{0}],", field.DataFieldName);

            sbColumns.AppendFormat("[{0}]", DataElementParameterName);
            return string.Format(QueryFormatStrings.SELECT_TOP_ONE_LIST, sbColumns.ToString(), DataElementTableName);
        }

    #endregion

    #region Override Methods

        /// <summary>
        /// This is what adds "<Element>" objects from the DataElementBuilder
        /// </summary>
        /// <param name="obj">
        /// Parsed sub-object being added.
        /// </param>
        protected override void AddParsedSubObject(object obj)
        {
            if (obj is DataElementField)
                ElementFields.Add(obj);
            else
                base.AddParsedSubObject(obj);
        }

    #endregion

    #region Exposed Methods

        public void SetDetailsViewMode(DetailsViewMode mode)
        {
            DetailsView1.ChangeMode(mode);
        }

    #endregion
    }

    /// <summary>
    /// Used to gather the specific fields to use for a DataElement control.
    /// </summary>
    internal class DataElementBuilder : ControlBuilder
    {
    #region Structs

        private struct Types
        {
            public static Type DataElementField = typeof(DataElementField);
        }

    #endregion

    #region Public Methods

        public override Type GetChildControlType(string tagName, IDictionary attribs)
        {
            if (tagName.ToLower() == "dataelementfield")
                return Types.DataElementField;
            return null;
        }

    #endregion
    }

    /// <summary>
    /// Used to represent a field within a DataElement control.
    /// </summary>
    public class DataElementField : WebControl, IDataElementField
    {
        #region Private Members

        private bool manyToMany = false;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the name of the field within its collection (probably a SQL table).
        /// </summary>
        public string DataFieldName
        {
            get;
            set;
        }

        /// <summary>
        /// The text to display to the user for the field.
        /// </summary>
        public string HeaderName
        {
            get;
            set;
        }

        public bool Required { get; set; }

        /// <summary>
        /// The type of control to use for user input on the field.
        /// </summary>
        public SqlDbType Type
        {
            get;
            set;
        }

        // TODO: this should be "ControlStyle-CssClass"
        // to match how asp:BoundFields work, but obviously
        // that can't be a member name, so we'll have to
        // figure out how to have a property that expects a
        // different name in the UML side of things than its
        // own real member name.
        public string ControlCssClass
        {
            get;
            set;
        }

        public string Filter
        {
            get;
            set;
        }

        public bool ManyToMany
        {
            get { return manyToMany; }
            set { manyToMany = value; }
        }

        #endregion

        #region Constructors

        public DataElementField()
        {
        }

        #endregion
    }

    public sealed class LinksTemplateField : TemplateField
    {
    #region Private Members

        private ITemplate m_tmplItem, m_tmplEdit, m_tmplInsert;
        private bool m_blAllowEdit = false, m_blAllowNew = false, m_blAllowDelete = false;

    #endregion

    #region Properties

        public override bool ShowHeader
        {
            // always return false, otherwise we'll
            // have an extra blank cell
            get { return false; }
        }

        public override ITemplate EditItemTemplate
        {
            get
            {
                if (m_tmplEdit == null)
                    m_tmplEdit = new EditItemLinksTemplate();
                return m_tmplEdit;
            }
        }
        public override ITemplate ItemTemplate
        {
            get
            {
                if (m_tmplItem == null)
                    m_tmplItem = new ItemLinksTemplate(AllowEdit, AllowNew, AllowDelete);
                return m_tmplItem;
            }
        }
        public override ITemplate InsertItemTemplate
        {
            get
            {
                if (m_tmplInsert == null)
                    m_tmplInsert = new InsertItemLinksTemplate();
                return m_tmplInsert;
            }
        }

        public bool AllowEdit
        {
            get { return m_blAllowEdit; }
        }
        public bool AllowNew
        {
            get { return m_blAllowNew; }
        }
        public bool AllowDelete
        {
            get { return m_blAllowDelete; }
        }

    #endregion

    #region Constructors

        public LinksTemplateField(bool blAllowEdit, bool blAllowNew, bool blAllowDelete) : base()
        {
            m_blAllowEdit = blAllowEdit;
            m_blAllowNew = blAllowNew;
            m_blAllowDelete = blAllowDelete;

            base.ItemTemplate = ItemTemplate;
            base.EditItemTemplate = EditItemTemplate;
            base.InsertItemTemplate = InsertItemTemplate;
        }

    #endregion
    }

    public abstract class LinksTemplate : ITemplate
    {
    #region Private Members

        private static Literal m_litSpacer = new Literal()
        {
            Text = "&nbsp;"
        };
        protected static LinkButton m_btnCancel = new LinkButton()
        {
            ID = "LinkButton2",
            Text = "Cancel",
            CommandName = "Cancel",
            CausesValidation = false
        };

    #endregion

    #region Properties

        protected abstract LinkButton[] Links
        {
            get;
        }

    #endregion

    #region Constructors

        public LinksTemplate()
        {
        }

    #endregion

    #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            // TODO: This is a hack, please clean it up.
            if (Links.Length > 0)
                container.Controls.Add(Links[0]);
            if (Links.Length > 1)
            {
                for (int i = 1, len = Links.Length; i < len; ++i)
                {
                    container.Controls.Add(m_litSpacer);
                    container.Controls.Add(Links[i]);
                }
            }
        }

    #endregion
    }

    /// <summary>
    /// Used to specify the links to display when editing an item.
    /// </summary>
    public class EditItemLinksTemplate : LinksTemplate
    {
    #region Private members

        private static LinkButton[] m_arLinks = new LinkButton[]
        {
            new LinkButton()
            {
                ID = "LinkButton1",
                Text = "Update",
                CommandName = "Update",
                CausesValidation = true
            },
            m_btnCancel
        };

    #endregion

    #region Properties

        protected override LinkButton[] Links
        {
            get { return m_arLinks; }
        }

    #endregion
    }

    /// <summary>
    /// Used to specify the links to display when viewing an item.
    /// </summary>
    public class ItemLinksTemplate : LinksTemplate
    {
    #region Private Members

        private bool m_blAllowEdit = false, m_blAllowNew = false, m_blAllowDelete = false;
        private LinkButton[] m_arLinks;
        private static LinkButton m_btnEdit = new LinkButton()
        {
            ID = "lbtnEdit",
            Text = "Edit",
            CommandName = "Edit",
            CausesValidation = false
        };
        private static LinkButton m_btnNew = new LinkButton()
        {
            ID = "lbtnNew",
            Text = "New",
            CommandName = "New",
            CausesValidation = false
        };
        private static LinkButton m_btnDelete = new LinkButton()
        {
            ID = "lbtnDelete",
            Text = "Delete",
            CommandName = "Delete",
            CausesValidation = false,
            OnClientClick = "return confirm('Are you sure you want to delete this record?');"
        };

    #endregion

    #region Properties

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

        public bool AllowDelete
        {
            get { return m_blAllowDelete; }
            set { m_blAllowDelete = value; }
        }

        protected override LinkButton[] Links
        {
            get { return m_arLinks; }
        }

    #endregion

    #region Constructors

        public ItemLinksTemplate(bool blAllowEdit, bool blAllowNew, bool blAllowDelete) : base()
        {
            m_blAllowEdit = blAllowEdit;
            m_blAllowNew = blAllowNew;
            m_blAllowDelete = blAllowDelete;

            GenerateLinksArray();
        }

        public ItemLinksTemplate() : base()
        {
            GenerateLinksArray();
        }

    #endregion

    #region Private Methods

        private void GenerateLinksArray()
        {
            int iLength = 0;
            if (AllowEdit)
                iLength++;
            if (AllowNew)
                iLength++;
            if (AllowDelete)
                iLength++;

            m_arLinks = new LinkButton[iLength];
            switch (iLength)
            {
                case 3:
                    Links[2] = m_btnDelete;
                    goto case 2;
                case 2:
                    Links[1] = (AllowEdit) ? ((AllowNew) ? m_btnNew : m_btnDelete) : m_btnDelete;
                    goto case 1;
                case 1:
                    Links[0] = (AllowEdit) ? m_btnEdit : (AllowNew) ? m_btnNew : m_btnDelete;
                    break;
            }
        }

    #endregion
    }

    /// <summary>
    /// Used to specify the links to display when inserting an item.
    /// </summary>
    public class InsertItemLinksTemplate : LinksTemplate
    {
    #region Private Members

        private static LinkButton[] m_arLinks = new LinkButton[]
        {
            new LinkButton()
            {
                ID = "LinkButton1",
                Text = "Insert",
                CommandName = "Insert",
                CausesValidation = true
            }
            // Cancel button does not good things on insert,
            // very badly, and with undesired results.
            // Disabled for the time being.
            //,
            //new LinkButton()
            //{
            //    ID = "LinkButton2",
            //    Text = "Cancel",
            //    CommandName = "Cancel",
            //    CausesValidation = false
            //}
        };

    #endregion

    #region Properties

        protected override LinkButton[] Links
        {
            get { return m_arLinks; }
        }

    #endregion
    }
}
