using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    // TODO: REPAY TECHNICAL DEBT. ARR
    public class DropDownListEditTemplate : IBindableTemplate
    {
        #region Constants

        private const string REQUIRED_TEXT = "Required",
                             CONTROL_ID_FORMAT = "ddl{0}";

        #endregion

        #region Private Members

        private ObjectDataSource _boundObjectDataSource;
        private IDropDownList _boundDropDownList;
        private RequiredFieldValidator _validator;

        #endregion

        #region Properties

        public IEntityBoundField Parent { get; set; }

        // TODO: Fix all of this.
        public IDropDownList BoundDropDownList
        {
            get
            {
                if (_boundDropDownList == null)
                {
                    _boundDropDownList = new MvpDropDownList {
                        DataTextField = Parent.DataTextField,
                        DataValueField = Parent.DataValueField,
                        DataSource = BoundObjectDataSource,
                        // TODO: Not sure this will work.
                        // the SelectedValue has to match the Value property of
                        // one of the Items
                        SelectedValue = Parent.SelectedValue,
                        AppendDataBoundItems = true,
                        ID = ControlID
                    };
                    _boundDropDownList.Items.Add(new ListItem("", ""));
                }

                return _boundDropDownList;
            }
        }

        public ObjectDataSource BoundObjectDataSource
        {
            get
            {
                if (_boundObjectDataSource == null)
                    _boundObjectDataSource = new ObjectDataSource {
                        TypeName = Parent.TypeName,
                        SelectMethod = Parent.SelectMethod
                    };
                return _boundObjectDataSource;
            }
        }

        public RequiredFieldValidator Validator
        {
            get
            {
                if (_validator == null)
                {
                    _validator = new RequiredFieldValidator {
                        ControlToValidate = ControlID,
                        Text = REQUIRED_TEXT
                    };
                }

                return _validator;
            }
        }

        public string ControlID
        {
            get { return string.Format(CONTROL_ID_FORMAT, Parent.DataField.Replace(".", "_")); }
        }

        #endregion

        #region Constructor

        public DropDownListEditTemplate(IEntityBoundField parent)
        {
            Parent = parent;
        }

        #endregion

        #region Exposed Methods

        public void InstantiateIn(Control container)
        {
            if (container != null)
            {
                BoundDropDownList.DataBinding += BoundDropDownList_OnDataBinding;
                BoundDropDownList.DataBound += OnDataBound;
                container.Controls.Add(BoundObjectDataSource);
                BoundDropDownList.DataSource = BoundObjectDataSource;
                container.Controls.Add((Control)BoundDropDownList);
                if (Parent.Required)
                    container.Controls.Add(Validator);
            }
        }

        protected virtual void BoundDropDownList_OnDataBinding(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
        }

        public IOrderedDictionary ExtractValues(Control container)
        {
            var od = new OrderedDictionary {{Parent.SelectedValueField, BoundDropDownList.SelectedValue}};
            return od;
        }

        #endregion

        #region Private Methods

        /// TODO: This needs some revision. 
        /// First call to this via Mode==Insert works, 
        /// second does not within a detailview.
        private void OnDataBound(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            var dv = ddl.NamingContainer as IDataItemContainer;
            if (Parent.DataField != null && dv.DataItem != null)
            {
                var obj = DataBinder.Eval(dv.DataItem, Parent.SelectedValueField);
                if (obj != null && ddl.Items.FindByValue(obj.ToString()) != null)
                    ddl.SelectedValue = obj.ToString();
            }
        }

        #endregion
    }
}
