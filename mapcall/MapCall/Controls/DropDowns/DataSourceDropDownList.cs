using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MapCall.Controls.Data;

namespace MapCall.Controls.DropDowns
{
    // TODO: Change this to inherit from DropDownList. The DataSource can be added
    //       directly to the ddl's child controls. The ddl just needs its CreateControlCollection
    //       method overridden and have it return a new ControlCollection. BAM
    //
    // NOTE: If one of these controls are referenced in a cascading dropdown
    //       then you need to refer to the control id as "_YourId"
    //
    // TODO: Discuss what should be done in terms of the cache and invalidating it
    //       when new values get added to it. 
    //
    //       We can set the CacheDependencyKey to a string and then have its value be
    //       DateTime.Now. That'll invalidate it when you change the value. We'd just
    //       need to figure out a way to trigger it. Either by just setting up a page
    //       with a button to trigger it, or certain pages could trigger the cache
    //       invalidation when new items get added(like a lookup table page).
    public class DataSourceDropDownList : DropDownList
    {
        #region Private Methods

        private readonly ListItem _emptyItem = new ListItem();
        private readonly McProdDataSource _dataSource = new McProdDataSource();
        private readonly RequiredFieldValidator _requiredFieldValidator = new RequiredFieldValidator();

        #endregion

        #region Properties

        /// <summary>
        /// The duration, in seconds, that the bound items should be cached for before going back to the database to retrieve them.
        /// </summary>
        public int CacheDuration
        {
            get { return _dataSource.CacheDuration; }
            set { _dataSource.CacheDuration = value; }
        }

        public string ConnectionString
        {
            get { return _dataSource.ConnectionString; }
            set { _dataSource.ConnectionString = value; }
        }

        public string EmptyItemText
        {
            get { return _emptyItem.Text; }
            set { _emptyItem.Text = value; }
        }

        // Super freakin' useful for tables that rarely ever update. The cache is used either page-wide
        // or site-wide(not sure) but it stops from having to repeatedly get static data from the db. 
        //
        // Having said that, it's off by default. 
        public bool EnableCaching
        {
            get { return _dataSource.EnableCaching; }
            set { _dataSource.EnableCaching = value; }
        }

        /// <summary>
        /// Set this to true if ControlState(not ViewState) is required. True by default. 
        /// </summary>
        public bool EnableControlState { get; set; }

        /// <summary>
        /// When this is set the control won't autogenerate one for you.
        /// </summary>
        public string SelectCommand { get; set; }

        public string SelectedText
        {
            get
            {
                var item = SelectedItem;
                if (item == null)
                {
                    return string.Empty;
                }
                if (item.Text == EmptyItemText)
                {
                    // Return string.empty because we don't wanna send 
                    // values like "-- Select Here --" to the database
                    // when the value should be null. That'd just be
                    // downright silly!
                    return string.Empty;
                }
                return item.Text;
            }
            set
            {
                foreach (ListItem item in Items)
                {
                    if (item.Text == value)
                    {
                        item.Selected = true;
                        continue;
                    }
                }
            }
        }

        public bool OrderByTextField { get; set; }

        public string TableName { get; set; }

        public string TextFieldName
        {
            get { return DataTextField; }
            set { DataTextField = value; }
        }
        public string ValueFieldName
        {
            get { return DataValueField; }
            set { DataValueField = value; }
        }

        public bool Required
        {
            get { return _requiredFieldValidator.Enabled; }
            set { _requiredFieldValidator.Enabled = value; }
        }

        #endregion

        #region Constructors

        public DataSourceDropDownList()
        {
            EnableViewState = false;
            EnableControlState = true;
            OrderByTextField = true;
            Required = false; // Call now to get the validator.Enabled/Required values in sync. 
            Items.Add(_emptyItem);
            EmptyItemText = "-- Select --"; // Default in case someone forgets to put in an empty text string. 
            _emptyItem.Value = string.Empty; // Needs to be this or else the RequiredFieldValidator won't work.  
        }

        #endregion

        #region Private Methods

        private void AddChildControls()
        {
            var c = Controls;
            c.Add(_dataSource);
            c.Add(_requiredFieldValidator);
        }

        protected override ControlCollection CreateControlCollection()
        {
            // Need to override because the base returns an EmptyControlCollection
            // that throws an exception when you try to add children to it.
            return new ControlCollection(this);
        }

        private string CreateSelectCommand()
        {
            CheckNull("TextFieldName", TextFieldName);
            CheckNull("ValueFieldName", ValueFieldName);

            if (!string.IsNullOrWhiteSpace(SelectCommand))
            {
                return SelectCommand;
            }

            // TableName only matters when we're generating the command.
            CheckNull("TableName", TableName);

            // SELECT {ValueFieldName}, {TextFieldName} FROM {TableName} ORDER BY {TextFieldName}
            const string sql = "SELECT {0}, {1} FROM {2}";
            const string orderBy = " ORDER BY {0}"; // Must have space at beginning. 

            // If TextFieldName is the same as ValueFieldName, just return the column once so we're
            // not returning double the data load. 
            const string singleColumnSql = "SELECT {0} FROM {1}";

            var comm = (TextFieldName == ValueFieldName) ?
                 string.Format(singleColumnSql, TextFieldName, TableName)
                 : string.Format(sql, ValueFieldName, TextFieldName, TableName);


            if (OrderByTextField)
            {
                comm += string.Format(orderBy, TextFieldName);
            }
            else
            {
                comm += string.Format(orderBy, ValueFieldName);
            }

            return comm;

        }

        private static void CheckNull(string propertyName, string prop)
        {
            if (string.IsNullOrWhiteSpace(prop))
            {
                throw new NullReferenceException(propertyName + " must be set.");
            }
        }

        protected sealed override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (EnableControlState)
            {
                Page.RegisterRequiresControlState(this);
            }

            EnsureID();
            _dataSource.SelectCommand = CreateSelectCommand();

            AppendDataBoundItems = true;
            _requiredFieldValidator.ErrorMessage = "Required";
            _requiredFieldValidator.InitialValue = string.Empty;
            _requiredFieldValidator.ClientIDMode = ClientIDMode.AutoID; // Must be AutoID

            AddChildControls();

            _requiredFieldValidator.ControlToValidate = ID;
            CreateItems();
        }

        protected void CreateItems()
        {
            using (var what = (DataView)_dataSource.Select(CreateDataSourceSelectArguments()))
            {
                var its = Items;
                foreach (DataRowView row in what)
                {
                    var li = new ListItem();
                    li.Text = row.Row[this.TextFieldName].ToString();
                    li.Value = row.Row[this.ValueFieldName].ToString();
                    its.Add(li);
                }

            }
        }
        
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);

            var cs = new ControlState(savedState);
            SelectedValue = cs.Get<string>("SelectedValue") ?? string.Empty;
        }

        protected override object SaveControlState()
        {
            var cs = new ControlState();
            cs.Add("SelectedValue", SelectedValue, null);
            return cs.GetControlStateObject();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Required)
            {
                Controls.Remove(_requiredFieldValidator);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            // This is needed in order to render the required field validator.
            // Somewhere in the chain, DropDownList does not render its children, which
            // makes sense since it normally throws an exception if you even try to
            // add children.
            RenderChildren(writer);
        }

        #endregion
    }


    /// <summary>
    /// This class can't be used in a DetailsView right now. Atleast as far as I know anyway.
    /// </summary>
    public class DataSourceListBox : ListBox
    {
        #region Private Methods

        private readonly McProdDataSource _dataSource = new McProdDataSource();

        #endregion

        #region Properties

        /// <summary>
        /// The duration, in seconds, that the bound items should be cached for before going back to the database to retrieve them.
        /// </summary>
        public int CacheDuration
        {
            get { return _dataSource.CacheDuration; }
            set { _dataSource.CacheDuration = value; }
        }

        public string ConnectionString
        {
            get { return _dataSource.ConnectionString; }
            set { _dataSource.ConnectionString = value; }
        }


        // Super freakin' useful for tables that rarely ever update. The cache is used either page-wide
        // or site-wide(not sure) but it stops from having to repeatedly get static data from the db. 
        //
        // Having said that, it's off by default. 
        public bool EnableCaching
        {
            get { return _dataSource.EnableCaching; }
            set { _dataSource.EnableCaching = value; }
        }

        /// <summary>
        /// When this is set the control won't autogenerate one for you.
        /// </summary>
        public string SelectCommand { get; set; }

        public IEnumerable<ListItem> SelectedItems
        {
            get
            {
                return (from ListItem sv in Items
                        where sv.Selected
                        select sv).ToArray();
            }
        }

        public bool OrderByTextField { get; set; }

        public string TableName { get; set; }

        public string TextFieldName
        {
            get { return DataTextField; }
            set { DataTextField = value; }
        }
        public string ValueFieldName
        {
            get { return DataValueField; }
            set { DataValueField = value; }
        }


        #endregion

        #region Constructors

        public DataSourceListBox()
        {
            this.EnableViewState = false;
            this.OrderByTextField = true;
            SelectionMode = ListSelectionMode.Multiple;
        }

        #endregion

        #region Private Methods

        protected void CreateItems()
        {
            using (var what = (DataView)_dataSource.Select(CreateDataSourceSelectArguments()))
            {
                var its = Items;
                foreach (DataRowView row in what)
                {
                    var li = new ListItem();
                    li.Text = row.Row[this.TextFieldName].ToString();
                    li.Value = row.Row[this.ValueFieldName].ToString();
                    its.Add(li);
                }

            }
        }

        private void AddChildControls()
        {
            var c = Controls;
            c.Add(_dataSource);
        }

        protected override ControlCollection CreateControlCollection()
        {
            // Need to override because the base returns an EmptyControlCollection
            // that throws an exception when you try to add children to it.
            return new ControlCollection(this);
        }

        private string CreateSelectCommand()
        {
            CheckNull("TextFieldName", TextFieldName);
            CheckNull("ValueFieldName", ValueFieldName);

            if (!string.IsNullOrWhiteSpace(SelectCommand))
            {
                return SelectCommand;
            }

            // TableName only matters when we're generating the command.
            CheckNull("TableName", TableName);

            // SELECT {ValueFieldName}, {TextFieldName} FROM {TableName} ORDER BY {TextFieldName}
            const string sql = "SELECT {0}, {1} FROM {2}";
            const string orderBy = " ORDER BY {0}"; // Must have space at beginning. 

            // If TextFieldName is the same as ValueFieldName, just return the column once so we're
            // not returning double the data load. 
            const string singleColumnSql = "SELECT {0} FROM {1}";

            var comm = (TextFieldName == ValueFieldName) ?
                 string.Format(singleColumnSql, TextFieldName, TableName)
                 : string.Format(sql, ValueFieldName, TextFieldName, TableName);


            if (OrderByTextField)
            {
                comm += string.Format(orderBy, TextFieldName);
            }
            else
            {
                comm += string.Format(orderBy, ValueFieldName);
            }

            return comm;

        }

// ReSharper disable UnusedParameter.Local: No shit, ReSharper!
        private static void CheckNull(string propertyName, string prop)
// ReSharper restore UnusedParameter.Local
        {
            if (string.IsNullOrWhiteSpace(prop))
            {
                throw new NullReferenceException(propertyName + " must be set.");
            }
        }

        protected sealed override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EnsureID();
            _dataSource.SelectCommand = CreateSelectCommand();

            AppendDataBoundItems = true;

            AddChildControls();

            CreateItems();
        }

        #endregion

    }

}