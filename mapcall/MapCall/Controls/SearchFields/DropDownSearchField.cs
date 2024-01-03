using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using MMSINC.DataPages;
using MapCall.Controls.DropDowns;

namespace MapCall.Controls.SearchFields
{
    public enum DropDownSelectMode
    {
        Single = 0,
        Multiple
    }

    public class DropDownSearchField : BaseSearchField
    {

        #region Fields

        private InternalHelper<DropDownSearchField> _helper;

        #endregion

        #region Properties

        public DropDownSelectMode SelectMode { get; set; }

        private InternalHelper<DropDownSearchField> Helper
        {
            get
            {
                if (_helper == null)
                {
                    switch(SelectMode)
                    {
                        case DropDownSelectMode.Single:
                            _helper = new DropDownHelper(this);
                            break;

                        case DropDownSelectMode.Multiple:
                            _helper = new ListBoxHelper(this);
                            break;
                    }
                }
                return _helper;
            }
        }

        public string EmptyItemText { get; set; }
        public bool EnableCaching { get; set; }

        /// <summary>
        /// When this is set the control won't autogenerate one for you.
        /// </summary>
        public string SelectCommand { get; set; }
        public bool OrderByTextField { get; set; }
        public string TableName { get; set; }
        public string TextFieldName { get; set; }
        public string ValueFieldName { get; set; }

        #endregion

        #region Constructors

        public DropDownSearchField()
        {
            OrderByTextField = true; // Set this as default.
        }

        #endregion


        public override void AddControlsToTemplate(Control template)
        {
            Helper.AddControlsToTemplate(template);
        }

        public override void SetValue(object value)
        {
            Helper.SetValue(value);
        }

        protected override void AddExpressions(IFilterBuilder builder)
        {
            Helper.AddExpressionsToFilterBuilder(builder);
        }

        #region Helper Classes

        private sealed class DropDownHelper : InternalHelper<DropDownSearchField>
        {

            #region Fields

            private DataSourceDropDownList _dropDown;

            #endregion

            #region Properties

            private DataSourceDropDownList DropDown
            {
                get
                {
                    if (_dropDown == null)
                    {
                        _dropDown = new DataSourceDropDownList();
                    }
                    return _dropDown;
                }
            }

            #endregion

            public DropDownHelper(DropDownSearchField parentOwner)
                : base(parentOwner)
            {
            }

            public override void AddControlsToTemplate(Control template)
            {
                var dd = DropDown;

                // We don't need any sort of state for the search fields
                // so we don't wanna bloat the viewstate. 
                dd.EnableControlState = false;
                dd.ClientIDMode = Owner.ClientIDMode;
                dd.EnableViewState = false; // It's false by default, but should that change we still want it to be false.
                dd.ID = Owner.GetControlID();
                dd.Required = Owner.Required;
                dd.EmptyItemText = (!string.IsNullOrWhiteSpace(Owner.EmptyItemText) ? Owner.EmptyItemText: "-- Select --");
                dd.EnableCaching = Owner.EnableCaching;
                dd.OrderByTextField = Owner.OrderByTextField;
                dd.SelectCommand = Owner.SelectCommand;
                dd.TableName = Owner.TableName;
                dd.TextFieldName = Owner.TextFieldName;
                dd.ValueFieldName = Owner.ValueFieldName;

                // Don't forget Required
                template.Controls.Add(dd);
            }

            public override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
            {
                var value = DropDown.SelectedValue;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builder.AddExpression(new FilterBuilderExpression(Owner.DataFieldName, DbType.String, value));
                }
            }

            public override void SetValue(object value)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class ListBoxHelper : InternalHelper<DropDownSearchField>
        {

            #region Fields

            private DataSourceListBox _listBox;

            #endregion

            #region Properties

            private DataSourceListBox ListBox
            {
                get
                {
                    if (_listBox == null)
                    {
                        _listBox = new DataSourceListBox();
                    }
                    return _listBox;
                }
            }

            #endregion

            public ListBoxHelper(DropDownSearchField parentOwner)
                : base(parentOwner)
            {
            }

            private DbType GetParameterTypeFromValue(string value)
            {
                int outref;

                // We don't know what kind of crazy values are being sent here. 
                // So lets make this as a int if it can be one, otherwise a string.
                if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out outref))
                {
                    return DbType.Int32;
                }
                return DbType.String;    
            }

            public override void AddControlsToTemplate(Control template)
            {
                var dd = ListBox;
                dd.ID = Owner.GetControlID();
                dd.ClientIDMode = Owner.ClientIDMode;
                dd.EnableCaching = Owner.EnableCaching;
                dd.OrderByTextField = Owner.OrderByTextField;
                dd.SelectCommand = Owner.SelectCommand;
                dd.TableName = Owner.TableName;
                dd.TextFieldName = Owner.TextFieldName;
                dd.ValueFieldName = Owner.ValueFieldName;

                // Don't forget Required
                template.Controls.Add(dd);
            }

            public override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
            {
                var selItems = _listBox.SelectedItems;
                if (!selItems.Any()) { return; }

                var exp = new FilterBuilderExpression();

                var innerParams = new List<string>(selItems.Count());

                int count = 0;

                foreach (var item in selItems)
                {
                    count++;
                    var p = new FilterBuilderParameter
                    {
                        Name = Owner.DataFieldName + count,
                        Value = item.Value,
                        DbType = GetParameterTypeFromValue(item.Value)
                    };
                    exp.AddParameter(p);
                    innerParams.Add("@" + p.ParameterFormattedName);
                }
               

                if (innerParams.Any())
                {
                    var parms = String.Join(", ", innerParams.ToArray());
                    var formattedLeftSide = FilterBuilderParameter.GetFormattedQualifiedFieldName(Owner.DataFieldName);
                    exp.CustomFilterExpression = string.Format("{0} in ({1})", formattedLeftSide, parms);
                    builder.AddExpression(exp);
                }

            }

            public override void SetValue(object value)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

    }
}