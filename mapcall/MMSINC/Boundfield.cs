using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using MapCall.Common.Controls;

namespace MMSINC
{

    /// <summary>
    /// Numbers:
    ///     mmsinc:BoundField SqlDataType="Int" 
    ///     mmsinc:BoundField SqlDataType="Float" 
    /// Dates:
    ///     mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" 
    /// Strings w/MaxLength:
    ///     mmsinc:BoundField MaxLength="255" 
    /// </summary>
    public class BoundField : System.Web.UI.WebControls.BoundField
    {
        #region Constants

        private struct ViewStateKeys
        {
            public const string SqlDataType = "SqlDataType";
            public const string TextMode = "TextMode";
            public const string MaxLength = "MaxLength";
            public const string Columns = "Columns";
            public const string Rows = "Rows";
            public const string Wrap = "Wrap";
            public const string Required = "Required";
        }

        #endregion

        #region Private Members

        // Matches collected_by, created_by, entered_by, closed_by,
        // collectedby, createdby, enteredby, closedby
        private static Regex m_rgxUserNameField = new Regex(@"^(?:c(?:losed_?|ollected_?|reated_?)|entered_?)by$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region Properties

        /// <summary>
        /// This is terrible and only ever set to true on one page. Avoid setting this to true if you
        /// unless you want pages to be 5 megs in size.
        /// </summary>
        public bool SetTableCellId { get; set; }

        public SqlDbType SqlDataType
        {
            get
            {
                SqlDbType _dt = SqlDbType.VarChar;
                if (ViewState[ViewStateKeys.SqlDataType] != null)
                    _dt = (SqlDbType)ViewState[ViewStateKeys.SqlDataType];
                return _dt;
            }
            set { this.ViewState[ViewStateKeys.SqlDataType] = value; }
        }
        public TextBoxMode TextMode
        {
            get
            {
                TextBoxMode _tm = TextBoxMode.SingleLine;
                if (this.ViewState[ViewStateKeys.TextMode] != null)
                    _tm = (TextBoxMode)this.ViewState[ViewStateKeys.TextMode];
                return _tm;
            }
            set { this.ViewState[ViewStateKeys.TextMode] = value; }
        }
        public int MaxLength
        {
            get
            {
                int i = 0;
                if (this.ViewState[ViewStateKeys.MaxLength] != null)
                    i = Int32.Parse(this.ViewState[ViewStateKeys.MaxLength].ToString());
                return i;
            }
            set { this.ViewState[ViewStateKeys.MaxLength] = value; }
        }
        public int Columns
        {
            get
            {
                int i = 0;
                if (this.ViewState[ViewStateKeys.Columns] != null)
                    i = (int)this.ViewState[ViewStateKeys.Columns];
                return i;
            }
            set { this.ViewState[ViewStateKeys.Columns] = value; }
        }
        public int Rows
        {
            get
            {
                int i = 0;
                if (this.ViewState[ViewStateKeys.Rows] != null)
                    i = (int)this.ViewState[ViewStateKeys.Rows];
                return i;
            }
            set { this.ViewState[ViewStateKeys.Rows] = value; }
        }
        public bool Wrap
        {
            get
            {
                bool b = true;
                if (this.ViewState[ViewStateKeys.Wrap] != null)
                    b = (bool)this.ViewState[ViewStateKeys.Wrap];
                return b;
            }
            set { this.ViewState[ViewStateKeys.Wrap] = value; }
        }
        public bool Required
        {
            get
            {
                bool b = false;
                if (this.ViewState[ViewStateKeys.Required] != null)
                    b = (bool)this.ViewState[ViewStateKeys.Required];
                return b;
            }
            set { this.ViewState[ViewStateKeys.Required] = value; }
        }
        #endregion

        #region Private Methods

        private void AddTextBoxWithDefault(TextBox tb, DataControlFieldCell cell, int rowIndex)
        {
            tb.ID = string.Format("dvTxt{0}_{1}", DataField, rowIndex);
            tb.SkinID = "Small";
            tb.Attributes.Add("SqlDataType", SqlDataType.ToString());
            // short General date/time string,
            // gives 04/10/2008 10:30
            if (String.IsNullOrEmpty(tb.Text))
                tb.Text = DateTime.Now.ToString("g");
        }

        private void AddDateControl(TextBox tb, DataControlFieldCell cell, int rowIndex)
        {
            //<asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/calendar.png" OnClientClick="return false;"/>
            //<cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
            //    TargetControlID="TextBox3" PopupButtonID="ImageButton3" />
            //<br />
            //<asp:RegularExpressionValidator 
            //    ID="CompareValidator3" runat="server" 
            //    ErrorMessage="Must be a valid date/time"
            //    ControlToValidate="TextBox3"
            //    ValidationExpression="^(?=\d)(?:(?:(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})|(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))|(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2}))($|\ (?=\d)))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$"
            ///>

            tb.ID = String.Format("dvTxt{0}_{1}", this.DataField, rowIndex.ToString());
            tb.SkinID = "Small";
            tb.Attributes.Add("SqlDataType", SqlDataType.ToString());

            ImageButton img = new ImageButton();
            img.ID = String.Format("Img" + tb.ID);
            img.ImageUrl = "~/images/calendar.png";
            img.OnClientClick = "return false;";
            img.CssClass = "calButton";

            try
            {
                img.ImageUrl = VirtualPathUtility.ToAbsolute(img.ImageUrl);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause
            {
                // Just eat it(Eat it!). Just eat it(EAT IT!)

            }

            var ce = new CalendarExtender();
            ce.TargetControlID = tb.ID;
            ce.PopupButtonID = img.ID;


            RegularExpressionValidator rev = new RegularExpressionValidator();
            rev.ErrorMessage = "Must be a valid date.";
            rev.SetFocusOnError = true;
            rev.ValidationExpression = @"^(?=\d)(?:(?:(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})|(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))|(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2}))($|\ (?=\d)))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$";
            rev.EnableClientScript = true;
            rev.ControlToValidate = tb.ID;
            rev.ClientIDMode = ClientIDMode.AutoID; // WILL NOT WORK WITH ANYTHING BESIDES AUTOID IN .NET 4

            cell.Controls.Add(img);
            cell.Controls.Add(ce);
            cell.Controls.Add(rev);
        }
        private void AddRangeValidator(TextBox tb, DataControlFieldCell cell, int rowIndex, ValidationDataType validationDataType)
        {
            //<asp:RangeValidator  ID="RangeValidator5" runat="server"
            //    Type="Double" EnableClientScript="true"
            //    ControlToValidate="TextBox5"
            //    ErrorMessage="Value is out of the range."
            ///>            
            tb.ID = String.Format("dvTxt{0}_{1}", this.DataField, rowIndex.ToString());
            tb.Attributes.Add("SqlDataType", SqlDataType.ToString());

            // Standard Validators don't work with UpdatePanels
            // Once the patch is out this can be changed back. 
            RangeValidator rv = new RangeValidator();
            rv.Type = validationDataType;
            rv.SetFocusOnError = true;
            rv.ControlToValidate = tb.ID;
            rv.ClientIDMode = ClientIDMode.AutoID; // WILL NOT WORK WITH ANYTHING BESIDES AUTOID IN .NET 4

            switch (validationDataType.ToString())
            {
                case "Float":
                    rv.MaximumValue = float.MaxValue.ToString();
                    rv.MinimumValue = float.MinValue.ToString();
                    break;
                default:
                    // assume Int32 if its not something above.
                    rv.MaximumValue = Int32.MaxValue.ToString();
                    rv.MinimumValue = Int32.MinValue.ToString();
                    break;
            }
            rv.ErrorMessage = "The value is out of the acceptable range.";
            cell.Controls.Add(rv);
        }

        private void AddDateTimeControl(TextBox tb, DataControlFieldCell cell, int rowIndex)
        {
            var dtp = new DateTimePicker();
            dtp.ShowTimePicker = true;
            dtp.ID = String.Format("dvTxt{0}_{1}", DataField, rowIndex);
            tb.ID = dtp.ID + "_hid";
            dtp.Attributes.Add("SqlDataType", SqlDataType.ToString());
            tb.DataBinding += (sender, e) =>
                                  {
                                      var text = tb.Text;
                                      if (string.IsNullOrWhiteSpace(text))
                                      {
                                          dtp.SelectedDate = null;
                                      }
                                      else
                                      {
                                          dtp.SelectedDate = DateTime.Parse(text);
                                      }
                                  };
            dtp.SelectedDateChanged += (sender, e) =>
                                           {
                                               var val = dtp.SelectedDate;
                                               if (val.HasValue)
                                               {
                                                   tb.Text = val.Value.ToShortDateString() + " " + val.Value.ToShortTimeString();
                                               }
                                               else
                                               {
                                                   tb.Text = "";
                                               }
                                           };
            cell.Controls.Add(dtp);
            tb.Visible = false;

            //CalendarControl cc = new CalendarControl();
            //cc.ID = tb.ID = String.Format("dvTxt{0}_{1}", DataField, rowIndex);
            //tb.ID += "_hid";
            //cc.SkinID = "Small";
            //cc.Attributes.Add("SqlDataType", SqlDataType.ToString());
            //tb.DataBinding += (object sender, EventArgs e) =>
            //    cc.Text = tb.Text;
            //cc.TextChanged += (object sender, EventArgs e) =>
            //    tb.Text = cc.Text;

            //tb.Visible = false;
            //cell.Controls.Add(cc);
        }

        #endregion

        #region BoundField Methods

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cell != null && cell.Controls.Count > 0)
            {
                if (cell.Controls[0] is TextBox)
                {
                    TextBox tb = (TextBox)cell.Controls[0];
                    tb.ID = String.Format("dvTxt{0}_{1}", this.DataField, rowIndex.ToString());
                    if (this.MaxLength > 0)
                        tb.MaxLength = this.MaxLength;
                    switch (SqlDataType)
                    {
                        case SqlDbType.DateTime2:
                            AddDateTimeControl(tb, cell, rowIndex);
                            break;
                        case SqlDbType.DateTime:
                        case SqlDbType.SmallDateTime:
                            {
                                AddDateControl(tb, cell, rowIndex);
                                break;
                            }
                        case SqlDbType.Int:
                            {
                                AddRangeValidator(tb, cell, rowIndex, ValidationDataType.Integer);
                                break;
                            }
                        case SqlDbType.BigInt:
                        case SqlDbType.Decimal:
                        case SqlDbType.Float:
                        case SqlDbType.Money:
                            {
                                AddRangeValidator(tb, cell, rowIndex, ValidationDataType.Double);
                                break;
                            }
                        // TODO: This is only called for insert. You can override with the 
                        // boundfield textmode property.
                        case SqlDbType.Text:
                        case SqlDbType.NText:
                            tb.TextMode = TextBoxMode.MultiLine;
                            tb.Width = new Unit("98%");
                            break;
                        case SqlDbType.Char:
                        case SqlDbType.NChar:
                        case SqlDbType.NVarChar:
                        case SqlDbType.VarChar:
                            {
                                //tb.BackColor = System.Drawing.Color.Brown;
                                break;
                            }
                        default:
                            break;
                    }

                    if (this.Required)
                    {
                        RequiredFieldValidator rfv = new RequiredFieldValidator();
                        rfv.ClientIDMode = ClientIDMode.AutoID; // WILL NOT WORK WITH ANYTHING BESIDES AUTOID IN .NET 4
                        rfv.ControlToValidate = tb.ID;
                        rfv.SetFocusOnError = true;
                        rfv.Text = "Required";
                        cell.Controls.Add(rfv);
                    }

                    // if the name of this field is created_by or collected_by,
                    // and there's no current value, use the current user's username.
                    if (String.IsNullOrEmpty(tb.Text) && m_rgxUserNameField.IsMatch(DataField.Trim()) && Control.Page != null)
                        tb.Text = Control.Page.User.Identity.Name;
                }
            }
        }

        protected override void OnDataBindField(object sender, EventArgs e)
        {
            base.OnDataBindField(sender, e);
            var c = (Control)sender;
            if (SetTableCellId && c.ID == null)
            {
                // This is only ever actually needed once, in a DetailsView in the ExpenseLine.ascx control.
                // And at that, it's only used by a chart that you have to explicitly tell to show. 
                c.ID = String.Format("dvLbl{0}", this.DataField);
            }
            if (c is TextBox)
            {
                TextBox txt = (TextBox)c;
                Control controlContainer = c.BindingContainer;
                object val = GetValue(controlContainer);
                txt.TextMode = this.TextMode;
                txt.Columns = this.Columns;
                txt.Rows = this.Rows;
                txt.Wrap = this.Wrap;
                if (this.MaxLength > 0)
                    txt.MaxLength = this.MaxLength;
                switch (txt.Attributes["SqlDataType"])
                {
                    case "DateTime":
                    case "DateTime2":
                        txt.Text = FormatDataValue(val, false);
                        break;
                }
            }
            if (c is DataControlFieldCell && this.Rows > 0)
            {
                ((DataControlFieldCell)c).Text = "<pre class=\"BoundFieldTextArea\">" + ((DataControlFieldCell)c).Text + "</pre>";
            }
        }

        #endregion
    }

}
