using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.Data
{

    [ParseChildren(true)]
    public class MultiCheckBoxList : UserControl
    {

        #region Fields

        private string _cssClass;

        #endregion

        #region Properties

        public CheckBoxList CheckBoxList { get; set; }
        public Literal LitNoBaseRole { get; set; }

        public Literal litNoBaseRole
        {
            get { return LitNoBaseRole; }
            set { LitNoBaseRole = value; }
        }

        public string CssClass
        {
            get { return (_cssClass ?? string.Empty); }
            set { _cssClass = value;  }
        }

        /// <summary>
        /// Gets or sets the SqlDataSource needed to get all the possible values for this control. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SqlDataSource DataSource { get; set; }

        /// <summary>
        /// Gets or sets the SqlDataSource needed to find the selected operating centers based on a secondary table.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SqlDataSource SelectedItemsDataSource { get; set; }

        /// <summary>
        /// Returns the ListItems that the Checkbox is using internally. 
        /// </summary>
        public ListItemCollection Items
        {
            get { return CheckBoxList.Items; }
        }

        public IEnumerable<ListItem> SelectedItems
        {
            get
            {
                return (from ListItem list in Items
                        where list.Selected
                        select list).ToList();
            }
        }

        /// <summary>
        /// The Checkbox's Text field.
        /// </summary>
        public string DataTextField
        {
            get { return CheckBoxList.DataTextField; }
            set { CheckBoxList.DataTextField = value; }
        }

        /// <summary>
        /// The Checkbox's Value field. 
        /// </summary>
        public string DataValueField
        {
            get { return CheckBoxList.DataValueField; }
            set { CheckBoxList.DataValueField = value; }
        }

        /// <summary>
        /// The field being returned by the SelectedDataSource that is compared againsted DataValueField. 
        /// </summary>
        public string SelectedSourceFieldName { get; set; }

        public string BaseRole { get; set; }

        #endregion

        public MultiCheckBoxList()
        {

            if (CheckBoxList == null) { CheckBoxList = new CheckBoxList(); }
            if (LitNoBaseRole == null)
            {
                LitNoBaseRole = new Literal();
            }

            this.Controls.Add(LitNoBaseRole);
            this.Controls.Add(CheckBoxList);
            //EnableViewState = false;
            //this.CheckBoxList.EnableViewState = false;
            //this.LitNoBaseRole.EnableViewState = false;

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


            InitDataSource(DataSource);
            CheckBoxList.DataSourceID = DataSource.ID;
            CheckBoxList.DataBind();
            InitDataSource(SelectedItemsDataSource);

        }

        private void InitDataSource(SqlDataSource ds)
        {
            if (ds == null)
            {
                throw new NullReferenceException("Missing DataSource");
            }

            // This sets the ID in case one isn't set in markup. If there's no ID,
            // an exception isn't thrown, but no data binding occurs. 
            if (string.IsNullOrEmpty(ds.ID))
            {
                ds.ID = "ds" + ds.GetHashCode();
            }

            // Add the data source to this control's NamingContainer, otherwise
            // it'll throw an exception when binding.
            NamingContainer.Controls.Add(ds);
        }

        public override void DataBind()
        {
            base.DataBind();


            var dView = (DataView)SelectedItemsDataSource.Select(DataSourceSelectArguments.Empty);

            // null dView means there are no rows. 
            if (dView != null)
            {
                // This caches the ListItems to a lookup so there's no need to repeatedly loop
                // through them when there's more than one selected operating center.
                var itemsById = new Dictionary<string, ListItem>();
                foreach (ListItem item in Items)
                {
                    itemsById.Add(item.Value, item);
                }

                foreach (DataRowView row in dView)
                {
                    //   System.Diagnostics.Debug.Print(row["opCntr"].ToString());
                    var recId = row[SelectedSourceFieldName].ToString();
                    itemsById[recId].Selected = true;

                }
            }

        }


        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (string.IsNullOrEmpty(BaseRole))
            {
                this.CheckBoxList.Items.Clear();
                LitNoBaseRole.Visible = true;
            }
            else
            {
                LitNoBaseRole.Visible = false;
            }

        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", ClientID);

            var css = CssClass;
            if (!string.IsNullOrEmpty(css))
            {
                writer.AddAttribute("class", css);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

        #endregion
    }
}