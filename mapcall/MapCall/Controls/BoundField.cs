using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Utilities;
using MapCall.Controls.BoundFieldHelpers;

namespace MapCall.Controls
{

    // This class is to eventually replace the MMSINC.BoundField class that's hiding
    // in Green.cs. Instead of having everything in one class, this class will make
    // use of BoundFieldHelper-derived classes. It makes things organized nicer.

    // TODO: Move BoundField to MapCall.Controls instead. Change BoundFields namespace to BoundFieldHelpers.
    //       Just to have less namespace markup garbage in the aspx files. 

    // TODO: Add a ReadOnlyVisibleOnly property so we can easily hide stuff that's not editable.

    // NOTE: BoundField controls just do not work with retrieving values when a BoundField is set to ReadOnly.
    //       When you do a Bind() in a TemplateField, the process is entirely different.

    // Sealed unless we need to at some point(like making this testable?).
    [ParseChildren(true)]
    public sealed class BoundField : System.Web.UI.WebControls.BoundField
    {
        #region Fields

        private BooleanBoundFieldOptions _boolOptions;
        private BoundFieldHelper _helper;
        private string _headerText;
        private TextBoundFieldOptions _textOptions;

        #endregion

        #region Properties

        /// <summary>
        /// While this isn't a Control ID, if you set the value in markup
        /// it will generate a field in the code behind like a control would.
        /// Hurrah!
        /// </summary>
        public string ID { get; set; }

        ///// <summary>
        ///// Set to true if this field should be visible only for debugging purposes.
        ///// You'd have to be logged in as mcadmin to see it. 
        ///// </summary>
        //public bool IsDebugVisible { get; set; }
        public bool AutoGenerateHeaderText { get; set; }

        /// <summary>
        /// Set to true if the SortExpression property should be set to the same value as the DataField property.
        /// Defaults to true.
        /// </summary>
        public bool AutoGenerateSortExpression { get; set; }
        
        public SqlDbType DataType { get; set; }
        public string HelpText { get; set; }
        public bool Required { get; set; }
        public bool IsInEditMode { get; set; }

        public override string DataFormatString
        {
            get
            {
                var baseFormat = base.DataFormatString;
                return string.IsNullOrWhiteSpace(baseFormat)
                       ? _helper.DataFormatString
                       : baseFormat;
            }
            set
            {
                base.DataFormatString = value;
            }
        }

        public override string HeaderText
        {
            get
            {
                var header = base.HeaderText;
                if (!string.IsNullOrWhiteSpace(header))
                {
                    return header;
                }

                if (AutoGenerateHeaderText)
                {
                    if (_headerText == null)
                    {
                        _headerText = FormatDataFieldIntoHeaderText(this.DataField);
                    }
                    return _headerText;
                }
                else
                {
                    return DataField;
                }
            }
            set
            {
                base.HeaderText = value;
            }
        }

        public int MaxLength { get; set; }

        public override string SortExpression
        {
            get => (AutoGenerateSortExpression ? this.DataField : base.SortExpression);
            set => base.SortExpression = value;
        }

        public string ValidationGroup { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public BooleanBoundFieldOptions BooleanBoundFieldOptions
        {
            get
            {
                // lazyload this since not every field would require it.
                if (_boolOptions == null)
                {
                    _boolOptions = new BooleanBoundFieldOptions();
                }
                return _boolOptions;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TextBoundFieldOptions TextBoundFieldOptions
        {
            get
            {
                // lazyload this since not every field would require it.
                if (_textOptions == null)
                {
                    _textOptions = new TextBoundFieldOptions();
                }
                return _textOptions;
            }
        }

        #endregion

        #region Constructors

        public BoundField()
        {
            AutoGenerateHeaderText = true;
            AutoGenerateSortExpression = true;
            DataType = SqlDbType.NVarChar; // So we don't have to explicitly set the datatype. 
            // Also because otherwise it defaults to BigInt, and no one likes them.
        }

        #endregion

        #region Private Methods

        #region Static

        private static bool IsEditableState(DataControlRowState rowState)
        {
            if (rowState == DataControlRowState.Insert) { return true; }
            if (rowState == DataControlRowState.Edit) { return true; }
            if (rowState.HasFlag(DataControlRowState.Insert) && rowState.HasFlag(DataControlRowState.Alternate)) { return true; }
            if (rowState.HasFlag(DataControlRowState.Edit) && rowState.HasFlag(DataControlRowState.Alternate)) { return true; }
            return false;
        }

        internal static void SetCommonValidatorAttributes(BaseValidator val, Control c)
        {
            val.ClientIDMode = ClientIDMode.AutoID; // MUST be AutoID, will throw an exception otherwise. 
            val.Display = ValidatorDisplay.Dynamic;
            val.ControlToValidate = c.ID;
        }

        internal static RequiredFieldValidator CreateRequiredFieldValidator(Control c)
        {
            var rfv = new RequiredFieldValidator();
            SetCommonValidatorAttributes(rfv, c);
            rfv.ErrorMessage = "Required";
            rfv.InitialValue = string.Empty;
            return rfv;
        }

        private static string FormatDataFieldIntoHeaderText(string value)
        {
            // We don't like underscores and we most certainly don't like spaces!
            value = value
                       .Replace("_", string.Empty)
                       .Replace(" ", string.Empty);

            var words = Wordify.GetWordsFromCamelCase(value);
            return string.Join(" ", words.ToArray());
        }

        #endregion

        protected override DataControlField CreateField()
        {
            return new BoundField();
        }

        protected override string FormatDataValue(object dataValue, bool encode)
        {
            // Overriding this to stop BoundField from returning "&nbsp;" when a value
            // is null or empty. There's no way to stop it otherwise. 
            const string nbsp = "&nbsp;";
            var value = base.FormatDataValue(dataValue, encode);
            return ((encode && (value == nbsp)) ? string.Empty : value);
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            // If we end up doing a custom sorting thing here, we can probably go cell.Controls.Clear();
            // to get rid of the default sort header that sucks.
            //
            // TODO: This doesn't work as expected, espescially when sorting is enabled. 
            if (cellType == DataControlCellType.Header && !string.IsNullOrWhiteSpace(HelpText))
            {
                var help = string.Format("{0}<span class=\"help\">{1}</span>", HeaderText, HelpText);
                var lit = new LiteralControl(help);
                cell.Controls.Add(lit);
            }

        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            Control boundControl;
            if (IsEditableState(rowState))
                IsInEditMode = true;
            if (!ReadOnly && IsEditableState(rowState))
            {
                var c = _helper.CreateEditableControl();
                c.VerifyControlOrder();
                boundControl = c.ValidatableControl;

                for (var i = 0; i < c.Controls.Count; i++)
                {
                    cell.Controls.Add(c.Controls[i]);
                }

                if (Required)
                {
                    var rfv = CreateRequiredFieldValidator(c.ValidatableControl);
                    if (!String.IsNullOrWhiteSpace(ValidationGroup))
                        rfv.ValidationGroup = ValidationGroup;
                    cell.Controls.Add(rfv);
                }
            }
            else
            {
                var readOnly = _helper.CreateReadOnlyControl();
                boundControl = readOnly;
                readOnly.EnableViewState = false;
                cell.Controls.Add(readOnly);
            }

            boundControl.DataBinding += this.OnDataBindField;
        }


        private BoundFieldHelper GetHelp()
        {
            BoundFieldHelper helper;
            switch (DataType)
            {
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.SmallDateTime:
                    helper = new DateBoundFieldHelper();
                    break;

                case SqlDbType.Int:
                case SqlDbType.BigInt:
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Money:
                    helper = new NumericBoundFieldHelper();
                    break;

                case SqlDbType.Bit:
                    helper = new BooleanBoundFieldHelper(BooleanBoundFieldOptions);
                    break;

                case SqlDbType.NText:
                case SqlDbType.Text:
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    helper = new TextBoundFieldHelper(TextBoundFieldOptions);
                    break;
                default:
                    throw new NotSupportedException("DataType is not supported: " + DataType);
            }
            helper.Owner = this;
            return helper;
        }


        internal string FormatValue(object value, bool encode)
        {
            // TODO: Ross wants to know why Ross is calling the base and not the override of FormatDataValue.
            return base.FormatDataValue(value, encode);
        }


        protected override void OnDataBindField(object sender, EventArgs e)
        {
            // DO NOT CALL THE BASE, IT WILL JUST CRASH!
            //   System.Diagnostics.Debug.Print("OnDataBindField: " + DataField);
            _helper.OnControlDataBinding(sender, e);
        }

        #endregion

        #region Public Methods

        public override bool Initialize(bool enableSorting, Control control)
        {
            if (_helper == null)
            {
                _helper = GetHelp();
            }
            return base.Initialize(enableSorting, control);
        }

        public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
            if (!_helper.ExtractValuesFromCell(dictionary, cell, IsEditableState(rowState), includeReadOnly))
            {
                base.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly);
            }
        }

        #endregion
    }

    // TODO: Add the HelpText property to this if possible.
    /// <summary>
    /// This class exists solely for the ID property. 
    /// </summary>
    public class TemplateBoundField : TemplateField
    {
        #region Properties

        /// <summary>
        /// While this isn't a Control ID, if you set the value in markup
        /// it will generate a field in the code behind like a control would.
        /// Hurrah!
        /// </summary>
        public string ID { get; set; }

        public string HelpText { get; set; }

        #endregion

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            // If we end up doing a custom sorting thing here, we can probably go cell.Controls.Clear();
            // to get rid of the default sort header that sucks.
            //
            // TODO: This doesn't work as expected, espescially when sorting is enabled. 
            if (cellType == DataControlCellType.Header && !string.IsNullOrWhiteSpace(HelpText))
            {
                var help = string.Format("{0}<span class=\"help\">{1}</span>", HeaderText, HelpText);
                var lit = new LiteralControl(help);
                cell.Controls.Add(lit);
            }

        }
    }
}