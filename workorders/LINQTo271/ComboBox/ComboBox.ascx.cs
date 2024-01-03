using System;
using System.Web.UI.WebControls;
using WorkOrders.Library.Controls;

namespace LINQTo271.ComboBox
{
    public partial class ComboBox : WorkOrdersMvpUserControl
    {
        #region Constants

        private const string BASE_STYLE = "position: absolute;z-index: 1;",
                             ARROW_WRAPPER_STYLE_FORMAT =
                                 "{0}left: {1}px;top: {2}px;",
                             OPTIONS_STYLE_FORMAT =
                                 "{0}display: none;top: {1}px;width: {2}px;",
                             TEXT_CLIENT_ID = "Text",
                             IMG_CLIENT_ID = "Img";

        private const short ARROW_WRAPPER_LEFT_OFFSET_FF = 136,
                            ARROW_WRAPPER_LEFT_OFFSET_IE = 147,
                            ARROW_WRAPPER_TOP_OFFSET_FF = 10,
                            ARROW_WRAPPER_TOP_OFFSET_IE = 18;

        private const short OPTIONS_TOP_OFFSET_FF = 29,
                            OPTIONS_TOP_OFFSET_IE = 37;

        private const short OPTIONS_WIDTH_FF = 146,
                            OPTIONS_WIDTH_IE = 154;

        #endregion

        #region Properties

        public string Style { get; set; }

        protected string ArrowWrapperStyle
        {
            get
            {
                return String.Format(ARROW_WRAPPER_STYLE_FORMAT, BASE_STYLE,
                    ArrowLeftOffset, ArrowTopOffset);
            }
        }

        protected string OptionsStyle
        {
            get
            {
                return String.Format(OPTIONS_STYLE_FORMAT, BASE_STYLE,
                    OptionsTopOffset, OptionsWidth);
            }
        }

        protected string TextClientID
        {
            get { return ID + TEXT_CLIENT_ID; }
        }

        protected string ImgClientID
        {
            get { return ID + IMG_CLIENT_ID; }
        }

        protected short ArrowLeftOffset
        {
            get
            {
                return (Page.Request.Browser.ActiveXControls)
                           ? ARROW_WRAPPER_LEFT_OFFSET_IE
                           : ARROW_WRAPPER_LEFT_OFFSET_FF;
            }
        }

        protected short ArrowTopOffset
        {
            get
            {
                return (Page.Request.Browser.ActiveXControls)
                           ? ARROW_WRAPPER_TOP_OFFSET_IE
                           : ARROW_WRAPPER_TOP_OFFSET_FF;
            }
        }

        protected short OptionsTopOffset
        {
            get
            {
                return (Page.Request.Browser.ActiveXControls)
                           ? OPTIONS_TOP_OFFSET_IE
                           : OPTIONS_TOP_OFFSET_FF;
            }
        }

        protected short OptionsWidth
        {
            get
            {
                return (Page.Request.Browser.ActiveXControls)
                           ? OPTIONS_WIDTH_IE
                           : OPTIONS_WIDTH_FF;
            }
        }

        #endregion

        #region DataBoundControl Properties

        public bool AppendDataBoundItems
        {
            get => lbOptions.AppendDataBoundItems;
            set => lbOptions.AppendDataBoundItems = value;
        }

        public bool AutoPostBack
        {
            get { return lbOptions.AutoPostBack; }
            set { lbOptions.AutoPostBack = value; }
        }

        public bool CausesValidation
        {
            get { return lbOptions.CausesValidation; }
            set { lbOptions.CausesValidation = value; }
        }

        public string DataSourceID
        {
            get { return lbOptions.DataSourceID; }
            set { lbOptions.DataSourceID = value; }
        }

        public object DataSource
        {
            get { return lbOptions.DataSource; }
            set { lbOptions.DataSource = value; }
        }

        public string DataTextField
        {
            get { return lbOptions.DataTextField; }
            set { lbOptions.DataTextField = value; }
        }

        public string DataTextFormatString
        {
            get { return lbOptions.DataTextFormatString; }
            set { lbOptions.DataTextFormatString = value; }
        }

        public string DataValueField
        {
            get { return lbOptions.DataValueField; }
            set { lbOptions.DataValueField = value; }
        }

        public ListItemCollection Items
        {
            get { return lbOptions.Items; }
        }

        public int SelectedIndex
        {
            get { return lbOptions.SelectedIndex; }
            set { lbOptions.SelectedIndex = value; }
        }

        public ListItem SelectedItem
        {
            get { return lbOptions.SelectedItem; }
        }
        
        public string SelectedValue
        {
            get { return lbOptions.SelectedValue; }
            set { lbOptions.SelectedValue = value; }
        }

        public string Text
        {
            get { return lbOptions.Text; }
            set { lbOptions.Text = value; }
        }

        public string ValidationGroup
        {
            get { return lbOptions.ValidationGroup; }
            set { lbOptions.ValidationGroup = value; }
        }

        #endregion

        #region Private Methods

        private void SetupListBox()
        {
            lbOptions.Style.Add("position", "absolute");
            lbOptions.Style.Add("z-index", "1");
            lbOptions.Style.Add("display", "none");
            lbOptions.Style.Add("top", OptionsTopOffset + "px");
            lbOptions.Style.Add("width", OptionsWidth + "px");
            lbOptions.Attributes.Add("onchange",
                String.Format(
                    "ComboBox.Options.change($(this), $('#{0}'), $('#{1}'), $('#{2}'))",
                    hidValue.ClientID, hidText.ClientID, TextClientID));
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            SetupListBox();
        }

        #endregion
    }
}